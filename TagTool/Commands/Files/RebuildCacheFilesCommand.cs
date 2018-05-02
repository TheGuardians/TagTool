using TagTool.Cache;
using TagTool.Commands;
using TagTool.Common;
using TagTool.IO;
using TagTool.Scripting;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Tags;

namespace TagTool.Commands.Files
{
    class RebuildCacheFilesCommand : Command
    {
        public GameCacheContext CacheContext { get; }
        private Dictionary<int, CachedTagInstance> ConvertedTags { get; } = new Dictionary<int, CachedTagInstance>();
        private Dictionary<ResourceLocation, Dictionary<int, PageableResource>> CopiedResources { get; } = new Dictionary<ResourceLocation, Dictionary<int, PageableResource>>();
        private MultiplayerGlobals MulgDefinition { get; set; } = null;
        private Dictionary<int, int> MapScenarios { get; } = new Dictionary<int, int>();
        private bool NoVariants { get; set; } = false;

        public RebuildCacheFilesCommand(GameCacheContext cacheContext)
            : base(CommandFlags.None,

                  "RebuildCacheFiles",
                  "Rebuilds the cache files into the specified output directory.",

                  "RebuildCacheFiles [NoVariants] <Output Directory>",

                  "Rebuilds the cache files into the specified directory.")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 2)
                return false;
            
            while (args.Count > 1)
            {
                switch (args[0].ToLower())
                {
                    case "novariants":
                        NoVariants = true;
                        break;

                    default:
                        throw new NotImplementedException(args[0]);
                }

                args.RemoveAt(0);
            }

            ConvertedTags.Clear();
            CopiedResources.Clear();

            var destDirectory = new DirectoryInfo(args[0]);

            if (!destDirectory.Exists)
                destDirectory.Create();

            var srcResourceCaches = new Dictionary<ResourceLocation, ResourceCache>();

            var destTagCache = CacheContext.CreateTagCache(destDirectory);
            CacheContext.StringIdCacheFile.CopyTo(Path.Combine(destDirectory.FullName, CacheContext.StringIdCacheFile.Name));
            var destResourceCaches = new Dictionary<ResourceLocation, ResourceCache>();

            foreach (var value in Enum.GetValues(typeof(ResourceLocation)))
            {
                ResourceCache resourceCache = null;
                var location = (ResourceLocation)value;

                if (location == ResourceLocation.None)
                    continue;

                try
                {
                    resourceCache = CacheContext.GetResourceCache(location);
                }
                catch (FileNotFoundException)
                {
                    continue;
                }

                CopiedResources[location] = new Dictionary<int, PageableResource>();

                srcResourceCaches[location] = resourceCache;
                destResourceCaches[location] = CacheContext.CreateResourceCache(destDirectory, location);
            }

            var destCacheContext = new GameCacheContext(destDirectory);

            using (var srcStream = CacheContext.OpenTagCacheRead())
            using (var destStream = destCacheContext.OpenTagCacheReadWrite())
            {
                CopyTag(CacheContext.TagCache.Index.FindFirstInGroup("cfgt"), CacheContext, srcStream, destCacheContext, destStream);

                foreach (var scnrTag in CacheContext.TagCache.Index.FindAllInGroup("scnr"))
                    CopyTag(scnrTag, CacheContext, srcStream, destCacheContext, destStream);
            }

            destCacheContext.SaveTagNames();
            
            return true;
        }

        private CachedTagInstance CopyTag(CachedTagInstance srcTag, GameCacheContext srcCacheContext, Stream srcStream, GameCacheContext destCacheContext, Stream destStream)
        {
            if (srcTag == null)
                return null;

            if (ConvertedTags.ContainsKey(srcTag.Index))
                return ConvertedTags[srcTag.Index];

            var structureType = TagDefinition.Find(srcTag.Group.Tag);
            var srcContext = new TagSerializationContext(srcStream, srcCacheContext, srcTag);
            var tagData = srcCacheContext.Deserializer.Deserialize(srcContext, structureType);

            CachedTagInstance destTag = null;

            for (var i = 0; i < destCacheContext.TagCache.Index.Count; i++)
            {
                if (destCacheContext.TagCache.Index[i] == null)
                {
                    destCacheContext.TagCache.Index[i] = destTag = new CachedTagInstance(i, TagGroup.Instances[srcTag.Group.Tag]);
                    break;
                }
            }

            if (destTag == null)
                destTag = destCacheContext.TagCache.AllocateTag(srcTag.Group);

            ConvertedTags[srcTag.Index] = destTag;

            if (srcCacheContext.TagNames.ContainsKey(srcTag.Index))
                destCacheContext.TagNames[destTag.Index] = srcCacheContext.TagNames[srcTag.Index];

            if (NoVariants && tagData is MultiplayerGlobals mulg)
                CleanMultiplayerGlobals(mulg);

            if (structureType == typeof(Scenario))
            {
                var scenario = (Scenario)tagData;

                if (!MapScenarios.ContainsKey(scenario.MapId))
                    MapScenarios[scenario.MapId] = destTag.Index;

                if (NoVariants)
                    CleanScenario(srcStream, scenario);
            }

            tagData = CopyData(tagData, srcCacheContext, srcStream, destCacheContext, destStream);

            if (structureType == typeof(Scenario))
                CopyScenario((Scenario)tagData);

            var destContext = new TagSerializationContext(destStream, destCacheContext, destTag);
            destCacheContext.Serializer.Serialize(destContext, tagData);

            var tagName = destCacheContext.TagNames.ContainsKey(destTag.Index) ?
                destCacheContext.TagNames[destTag.Index] :
                $"0x{destTag.Index:X2}";
            
            return destTag;
        }

        private object CopyData(object data, GameCacheContext srcCacheContext, Stream srcStream, GameCacheContext destCacheContext, Stream destStream)
        {
            if (data == null)
                return null;

            var type = data.GetType();

            if (type.IsPrimitive)
                return data;

            if (type == typeof(CachedTagInstance))
                return CopyTag((CachedTagInstance)data, srcCacheContext, srcStream, destCacheContext, destStream);

            if (type == typeof(PageableResource))
                return CopyResource((PageableResource)data, srcCacheContext, destCacheContext);
            
            if (type.IsArray)
                return CopyArray((Array)data, srcCacheContext, srcStream, destCacheContext, destStream);

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                return CopyList(data, type, srcCacheContext, srcStream, destCacheContext, destStream);

            if (type.GetCustomAttributes(typeof(TagStructureAttribute), false).Length > 0)
                return CopyStructure(data, type, srcCacheContext, srcStream, destCacheContext, destStream);

            return data;
        }

        private Array CopyArray(Array array, GameCacheContext srcCacheContext, Stream srcStream, GameCacheContext destCacheContext, Stream destStream)
        {
            if (array.GetType().GetElementType().IsPrimitive)
                return array;

            for (var i = 0; i < array.Length; i++)
            {
                var oldValue = array.GetValue(i);
                var newValue = CopyData(oldValue, srcCacheContext, srcStream, destCacheContext, destStream);
                array.SetValue(newValue, i);
            }

            return array;
        }

        private object CopyList(object list, Type type, GameCacheContext srcCacheContext, Stream srcStream, GameCacheContext destCacheContext, Stream destStream)
        {
            if (type.GenericTypeArguments[0].IsPrimitive)
                return list;

            var count = (int)type.GetProperty("Count").GetValue(list);
            var getItem = type.GetMethod("get_Item");
            var setItem = type.GetMethod("set_Item");

            for (var i = 0; i < count; i++)
            {
                var oldValue = getItem.Invoke(list, new object[] { i });
                var newValue = CopyData(oldValue, srcCacheContext, srcStream, destCacheContext, destStream);

                setItem.Invoke(list, new object[] { i, newValue });
            }

            return list;
        }

        private object CopyStructure(object data, Type type, GameCacheContext srcCacheContext, Stream srcStream, GameCacheContext destCacheContext, Stream destStream)
        {
            var enumerator = new TagFieldEnumerator(new TagStructureInfo(type, destCacheContext.Version));

            while (enumerator.Next())
            {
                var oldValue = enumerator.Field.GetValue(data);
                var newValue = CopyData(oldValue, srcCacheContext, srcStream, destCacheContext, destStream);
                enumerator.Field.SetValue(data, newValue);
            }
            
            return data;
        }

        private PageableResource CopyResource(PageableResource resource, GameCacheContext srcCacheContext, GameCacheContext destCacheContext)
        {
            if (resource == null || resource.Page.Index < 0 || !resource.GetLocation(out var location))
                return null;

            if (resource.Page.CompressedBlockSize > 0)
            {
                var index = resource.Page.Index;

                if (CopiedResources[location].ContainsKey(index))
                    return CopiedResources[location][index];

                var data = srcCacheContext.ExtractRawResource(resource);
                destCacheContext.AddRawResource(resource, location, data);

                CopiedResources[location][index] = resource;
            }

            return resource;
        }

        private bool ScriptExpressionIsValue(ScriptExpression expr)
        {
            switch (expr.ExpressionType)
            {
                case ScriptExpressionType.ParameterReference:
                    return true;

                case ScriptExpressionType.Expression:
                    if ((int)expr.ValueType.HaloOnline > 0x4)
                        return true;
                    else
                        return false;

                case ScriptExpressionType.ScriptReference: // The opcode is the tagblock index of the script it uses, so ignore
                case ScriptExpressionType.GlobalsReference: // The opcode is the tagblock index of the global it uses, so ignore
                case ScriptExpressionType.Group:
                    return false;

                default:
                    return false;
            }
        }

        private void CopyScriptTagReferenceExpressionData(ScriptExpression expr)
        {
            var srcTagIndex = BitConverter.ToInt32(expr.Data, 0);
            var destTagIndex = srcTagIndex == -1 ? -1 : ConvertedTags[srcTagIndex].Index;
            var newData = BitConverter.GetBytes(destTagIndex);
            expr.Data = newData;
        }

        private void CleanMultiplayerGlobals(MultiplayerGlobals mulgDefinition)
        {
            if (MulgDefinition == null)
                MulgDefinition = mulgDefinition;

            mulgDefinition.Universal[0].GameVariantWeapons = new List<MultiplayerGlobals.UniversalBlock.GameVariantWeapon>
            {
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("battle_rifle"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x157C]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("assault_rifle"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x151E]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("plasma_pistol"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x14F7]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("spike_rifle"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x1500]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("smg"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x157D]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("carbine"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x14FE]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("energy_sword"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x159E]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("magnum"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x157E]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("needler"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x14F8]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("plasma_rifle"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x1525]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("rocket_launcher"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x15B3]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("shotgun"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x1A45]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("sniper_rifle"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x15B1]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("brute_shot"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x14FF]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("unarmed"),
                    RandomChance = 0,
                    Weapon = CacheContext.TagCache.Index[0x157F]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("beam_rifle"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x1509]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("spartan_laser"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x15B2]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("none"),
                    RandomChance = 0,
                    Weapon = null
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("gravity_hammer"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x150C]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("excavator"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x1504]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("flamethrower"),
                    RandomChance = 0,
                    Weapon = CacheContext.TagCache.Index[0x1A55]
                },
                new MultiplayerGlobals.UniversalBlock.GameVariantWeapon
                {
                    Name = CacheContext.GetStringId("missile_pod"),
                    RandomChance = 0.1f,
                    Weapon = CacheContext.TagCache.Index[0x1A54]
                }
            };

            mulgDefinition.Runtime[0].MultiplayerConstants[0].Weapons = new List<MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon>
            {
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // battle_rifle
                    Type = CacheContext.TagCache.Index[0x157C],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // carbine
                    Type = CacheContext.TagCache.Index[0x14FE],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // sniper_rifle
                    Type = CacheContext.TagCache.Index[0x15B1],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // beam_rifle
                    Type = CacheContext.TagCache.Index[0x1509],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // spartan_laster
                    Type = CacheContext.TagCache.Index[0x15B2],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // rocket_launcher
                    Type = CacheContext.TagCache.Index[0x15B3],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // chaingun_turret
                    Type = CacheContext.TagCache.Index[0x15B4],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // machinegun_turret
                    Type = CacheContext.TagCache.Index[0x15B5],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // machinegun_turret_integrated
                    Type = CacheContext.TagCache.Index[0x15B6],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // plasma_cannon
                    Type = CacheContext.TagCache.Index[0x150E],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // plasma_cannon_integrated
                    Type = CacheContext.TagCache.Index[0x15B7],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // needler
                    Type = CacheContext.TagCache.Index[0x14F8],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // flak_cannon
                    Type = CacheContext.TagCache.Index[0x14F9],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // gauss_turret
                    Type = CacheContext.TagCache.Index[0x15B8],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // anti_infantry
                    Type = CacheContext.TagCache.Index[0x15B9],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                },
                new MultiplayerGlobals.RuntimeBlock.MultiplayerConstant.Weapon
                {
                    // behemoth_chaingun_turret
                    Type = CacheContext.TagCache.Index[0x15BA],
                    Unknown1 = 5.0f,
                    Unknown2 = 15.0f,
                    Unknown3 = 5.0f,
                    Unknown4 = -10.0f
                }
            };
        }

        private void CleanScenario(Stream srcStream, Scenario scnrDefinition)
        {
            if (scnrDefinition.MapType != ScenarioMapType.Multiplayer)
                return;

            if (MulgDefinition == null)
            {
                var context = new TagSerializationContext(srcStream, CacheContext, CacheContext.TagCache.Index.FindFirstInGroup("mulg"));
                MulgDefinition = CacheContext.Deserializer.Deserialize<MultiplayerGlobals>(context);
            }

            var weaponPalette = new List<Scenario.ScenarioPaletteEntry>();

            foreach (var entry in scnrDefinition.WeaponPalette)
                if ((MulgDefinition.Universal[0].GameVariantWeapons.Find(i => i.Weapon == entry.Object) != null) ||
                    (MulgDefinition.Runtime[0].MultiplayerConstants[0].Weapons.Find(i => i.Type == entry.Object) != null))
                    weaponPalette.Add(entry);
                else
                    weaponPalette.Add(new Scenario.ScenarioPaletteEntry());

            scnrDefinition.WeaponPalette = weaponPalette;

            var sandboxWeapons = new List<Scenario.SandboxObject>();

            foreach (var entry in scnrDefinition.SandboxWeapons)
                if ((MulgDefinition.Universal[0].GameVariantWeapons.Find(i => i.Weapon == entry.Object) != null) ||
                    (MulgDefinition.Runtime[0].MultiplayerConstants[0].Weapons.Find(i => i.Type == entry.Object) != null))
                    sandboxWeapons.Add(entry);

            scnrDefinition.SandboxWeapons = sandboxWeapons;
        }

        private void CopyScenario(Scenario scnrDefinition)
        {
            foreach (var expr in scnrDefinition.ScriptExpressions)
            {
                if (!ScriptExpressionIsValue(expr))
                    continue;

                switch (expr.ValueType.HaloOnline)
                {
                    case ScriptValueType.HaloOnlineValue.Sound:
                    case ScriptValueType.HaloOnlineValue.Effect:
                    case ScriptValueType.HaloOnlineValue.Damage:
                    case ScriptValueType.HaloOnlineValue.LoopingSound:
                    case ScriptValueType.HaloOnlineValue.AnimationGraph:
                    case ScriptValueType.HaloOnlineValue.DamageEffect:
                    case ScriptValueType.HaloOnlineValue.ObjectDefinition:
                    case ScriptValueType.HaloOnlineValue.Bitmap:
                    case ScriptValueType.HaloOnlineValue.Shader:
                    case ScriptValueType.HaloOnlineValue.RenderModel:
                    case ScriptValueType.HaloOnlineValue.StructureDefinition:
                    case ScriptValueType.HaloOnlineValue.LightmapDefinition:
                    case ScriptValueType.HaloOnlineValue.CinematicDefinition:
                    case ScriptValueType.HaloOnlineValue.CinematicSceneDefinition:
                    case ScriptValueType.HaloOnlineValue.BinkDefinition:
                    case ScriptValueType.HaloOnlineValue.AnyTag:
                    case ScriptValueType.HaloOnlineValue.AnyTagNotResolving:
                        CopyScriptTagReferenceExpressionData(expr);
                        break;

                    default:
                        break;
                }
            }
        }
    }
}