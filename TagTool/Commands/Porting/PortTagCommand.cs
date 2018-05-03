using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.Havok;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Tags;
using TagTool.Damage;
using TagTool.Audio;

namespace TagTool.Commands.Porting
{
    public partial class PortTagCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private CacheFile BlamCache { get; }
        private RenderGeometryConverter GeometryConverter { get; }

        private Dictionary<Tag, List<string>> ReplacedTags = new Dictionary<Tag, List<string>>();

        private List<Tag> RenderMethodTagGroups = new List<Tag> { new Tag("rmbk"), new Tag("rmcs"), new Tag("rmd "), new Tag("rmfl"), new Tag("rmhg"), new Tag("rmsh"), new Tag("rmss"), new Tag("rmtr"), new Tag("rmw "), new Tag("rmrd"), new Tag("rmct") };
        private List<Tag> EffectTagGroups = new List<Tag> { new Tag("beam"), new Tag("cntl"), new Tag("ltvl"), new Tag("decs"), new Tag("prt3") };
        private List<Tag> OtherTagGroups = new List<Tag> { /*new Tag("effe"), new Tag("foot"),*/ new Tag("shit"), new Tag("sncl") };

        private bool IsReplacing = false;
        private bool IsRecursive = true;
        private bool IsNew = false;
        private bool UseNull = false;
        private bool NoAudio = false;
        private bool NoElites = false;
        private bool NoForgePalette = false;
        private bool UseShaderTest = false;
        private bool MatchShaders = true;
        private bool ConvertScripts = true;
        private bool NoSquads = false;

        public PortTagCommand(GameCacheContext cacheContext, CacheFile blamCache) :
            base(CommandFlags.Inherit,

                "PortTag",
                "Ports a tag from the current cache file. Options are : noaudio | noreplace | noelites | noforgepalette | replace | new | single | nonnull" + Environment.NewLine + Environment.NewLine +
                "Replace: Use existing matching tag names if available." + Environment.NewLine +
                "New: Create a new tag after the last index." + Environment.NewLine +
                "Single: Port a new tag without any reference." + Environment.NewLine +
                "UseNull: Port a tag using nulled tag indices where available." + Environment.NewLine +
                "No option: Ports a tag if its name is not present in the tag names.",

                "PortTag [Options] <Tag>",

                "")
        {
            CacheContext = cacheContext;
            BlamCache = blamCache;
            GeometryConverter = new RenderGeometryConverter(cacheContext, blamCache);
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1)
                return false;

            while (args.Count > 1)
            {
                var arg = args[0].ToLower();

                switch (arg)
                {
                    case "noaudio":
                        NoAudio = true;
                        break;

                    case "noreplace":
                        IsReplacing = false;
                        break;

                    case "noelites":
                        NoElites = true;
                        break;

                    case "replace":
                        IsReplacing = true;
                        break;

                    case "new":
                        IsNew = true;
                        break;

                    case "single":
                        IsRecursive = false;
                        IsNew = true;
                        break;

                    case "usenull":
                        UseNull = true;
                        break;

                    case "shadertest":
                        UseShaderTest = true;
                        MatchShaders = false;
                        break;

                    case "noshaders":
                        MatchShaders = false;
                        break;

                    case "noscripts":
                        ConvertScripts = false;
                        break;

                    case "noforgepalette":
                        NoForgePalette = true;
                        break;

                    case "nosquads":
                        NoSquads = true;
                        break;

                    default:
                        throw new NotImplementedException(args[0]);
                }

                args.RemoveAt(0);
            }
            
            var initialStringIdCount = CacheContext.StringIdCache.Strings.Count;

            //
            // Convert Blam data to ElDorado data
            //

            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
                foreach (var item in ParseLegacyTag(args[0]))
                    ConvertTag(cacheStream, item);

            if (initialStringIdCount != CacheContext.StringIdCache.Strings.Count)
                using (var stringIdCacheStream = CacheContext.OpenStringIdCacheReadWrite())
                    CacheContext.StringIdCache.Save(stringIdCacheStream);

            CacheContext.SaveTagNames();

            return true;
        }

        private List<CacheFile.IndexItem> ParseLegacyTag(string name)
        {
            if (name.Length == 0 || (!char.IsLetter(name[0]) && !name.Contains('*')) || !name.Contains('.'))
                throw new Exception($"Invalid tag name: {name}");

            var namePieces = name.Split('.');

            var groupTag = ArgumentParser.ParseGroupTag(CacheContext.StringIdCache, namePieces[1]);
            if (groupTag == Tag.Null)
                throw new Exception($"Invalid tag name: {name}");

            var tagName = namePieces[0];

            List<CacheFile.IndexItem> result = new List<CacheFile.IndexItem>();

            foreach (var item in BlamCache.IndexItems)
            {
                if(tagName == "*")
                {
                    if(item != null && groupTag == item.ClassCode)
                        result.Add(item);
                }
                  
                else
                {
                    if (item == null || item.Filename != tagName)
                        continue;

                    if (groupTag == item.ClassCode)
                    {
                        result.Add(item);
                        break;
                    }    
                }
            }

            if(result.Count == 0)
                Console.WriteLine($"Invalid tag name: {name}");

            return result;
        }

        public CachedTagInstance ConvertTag(Stream cacheStream, CacheFile.IndexItem blamTag)
        {
            if (blamTag == null)
                return null;
            
            //
            // Check to see if the ElDorado tag exists
            //

            var groupTagChars = new char[] { ' ', ' ', ' ', ' ' };
            for (var i = 0; i < blamTag.ClassCode.Length; i++)
                groupTagChars[i] = blamTag.ClassCode[i];

            var groupTag = new Tag(new string(groupTagChars));

            CachedTagInstance edTag = null;

            if ((groupTag == "snd!") && NoAudio)
                return null;

            var wasReplacing = IsReplacing;
            var wasNew = IsNew;
            
            if (NoElites && (groupTag == "bipd") && blamTag.Filename.Contains("elite"))
                return null;

            if (!IsNew || groupTag == "glps" || groupTag == "glvs" || groupTag == "rmdf")
            {
                foreach (var instance in CacheContext.TagCache.Index.FindAllInGroup(groupTag))
                {
                    if (instance == null || !CacheContext.TagNames.ContainsKey(instance.Index))
                        continue;

                    if (CacheContext.TagNames[instance.Index] == blamTag.Filename)
                    {
                        if (IsReplacing)
                            edTag = instance;
                        else
                        {
                            edTag = instance;
                            Console.WriteLine($"[Group: '{edTag.Group.Tag}', Index: 0x{edTag.Index:X4}] {CacheContext.TagNames[edTag.Index]}.{CacheContext.GetString(edTag.Group.Name)}");
                            return edTag;
                        }
                    }
                }
            }

            //
            // Check to see if the tag was already replaced (if replacing)
            //

            if (ReplacedTags.ContainsKey(groupTag) && ReplacedTags[groupTag].Contains(blamTag.Filename))
            {
                var entries = CacheContext.TagNames.Where(i => i.Value == blamTag.Filename);

                foreach (var entry in entries)
                {
                    var tagInstance = CacheContext.GetTag(entry.Key);

                    if (tagInstance.Group.Tag == groupTag)
                    {
                        edTag = tagInstance;
                        Console.WriteLine($"[Group: '{edTag.Group.Tag}', Index: 0x{edTag.Index:X4}] {CacheContext.TagNames[edTag.Index]}.{CacheContext.GetString(edTag.Group.Name)}");
                        return edTag;
                    }
                }
            }

            //
            // If isReplacing is true, check current tags if there is an existing instance to replace
            //

            if (IsReplacing)
            {
                var listEntries = CacheContext.TagNames.Where(i => i.Value == blamTag.Filename);

                foreach (var entry in listEntries)
                {
                    var tagInstance = CacheContext.GetTag(entry.Key);

                    if (tagInstance.Group.Tag == groupTag)
                    {
                        edTag = tagInstance;
                        //If not recursive, use existing tags
                        if (!IsRecursive)
                            IsReplacing = false;
                        break;
                    }
                }
            }
            
            var replacedTags = ReplacedTags.ContainsKey(groupTag) ?
                (ReplacedTags[groupTag] ?? new List<string>()) :
                new List<string>();

            replacedTags.Add(blamTag.Filename);
            ReplacedTags[groupTag] = replacedTags;
            
            //
            // Handle shaders that do not exist (either in code or in tags)
            //

            switch (groupTag.ToString())
            {
                case "rmw ": // Until water vertices port, always null water shaders to prevent the screen from turning blue. Can return 0x400F when fixed
                    return null;

                case "rmct": // Cortana shaders have no example in HO, they need a real port
                    return CacheContext.GetTagInstance<Shader>(@"objects\characters\masterchief\shaders\mp_masterchief_rubber");

                case "rmbk": // Unknown, black shaders don't exist in HO, only in ODST, might be just complete blackness
                    return CacheContext.GetTagInstance<Shader>(@"objects\characters\masterchief\shaders\mp_masterchief_rubber");
            }

            //
            // Handle shader tags when not porting or matching shaders
            //

            if ((RenderMethodTagGroups.Contains(groupTag) || EffectTagGroups.Contains(groupTag)) &&
                (!UseShaderTest && !MatchShaders))
            {
                switch (groupTag.ToString())
                {
                    case "rmhg":
                        return CacheContext.GetTagInstance<ShaderHalogram>(@"objects\ui\shaders\editor_gizmo");

                    case "rmtr":
                        return CacheContext.GetTagInstance<ShaderTerrain>(@"levels\multi\riverworld\shaders\riverworld_ground");

                    case "rmd ":
                        return CacheContext.GetTagInstance<ShaderDecal>(@"objects\gear\human\military\shaders\human_military_decals");

                    case "rmfl":
                        return CacheContext.GetTagInstance<ShaderFoliage>(@"levels\multi\riverworld\shaders\riverworld_tree_leafa");

                    case "rmsh":
                    case "rmss":
                    case "rmrd":
                    case "rmcs":
                        return CacheContext.GetTagInstance<Shader>(@"objects\characters\masterchief\shaders\mp_masterchief_rubber");

                    case "beam":
                        return CacheContext.GetTagInstance<BeamSystem>(@"objects\weapons\support_high\spartan_laser\fx\firing_3p");

                    case "cntl":
                        return CacheContext.GetTagInstance<ContrailSystem>(@"objects\weapons\pistol\needler\fx\projectile");

                    case "ltvl":
                        return CacheContext.GetTagInstance<LightVolumeSystem>(@"objects\weapons\pistol\plasma_pistol\fx\charged\projectile");

                    case "decs":
                        return CacheContext.GetTagInstance<DecalSystem>(@"fx\decals\impact_plasma\impact_plasma_medium\hard");

                    case "prt3":
                        return CacheContext.GetTagInstance<Particle>(@"fx\particles\energy\sparks\impact_spark_orange");
                }
            }

            //
            // Handle tags that are not ready to be ported
            //

            if (OtherTagGroups.Contains(groupTag))
            {
                switch (groupTag.ToString())
                {
                    case "effe":
                        return CacheContext.GetTagInstance<Effect>(@"objects\characters\grunt\fx\grunt_birthday_party");

                    case "foot":
                        return CacheContext.GetTagInstance<MaterialEffects>(@"fx\material_effects\objects\characters\masterchief");

                    case "shit":
                        return CacheContext.GetTagInstance<ShieldImpact>(@"globals\global_shield_impact_settings");

                    case "sncl":
                        return CacheContext.GetTagInstance<SoundClasses>(@"sound\sound_classes");
                }    
            }

            //
            // Allocate Eldorado Tag
            //

            if (edTag == null && UseNull)
            {
                for (var i = 0; i < CacheContext.TagCache.Index.Count; i++)
                {
                    if (CacheContext.TagCache.Index[i] == null)
                    {
                        CacheContext.TagCache.Index[i] = edTag = new CachedTagInstance(i, TagGroup.Instances[groupTag]);
                        break;
                    }
                }
            }

            if (edTag == null)
                edTag = CacheContext.TagCache.AllocateTag(TagGroup.Instances[groupTag]);

            CacheContext.TagNames[edTag.Index] = blamTag.Filename;

            //
            // Load the Blam tag definition
            //

            Console.WriteLine($"Porting {blamTag.Filename}.{groupTag.ToString()}");

            var blamContext = new CacheSerializationContext(BlamCache, blamTag);
            var blamDefinition = BlamCache.Deserializer.Deserialize(blamContext, TagDefinition.Find(groupTag));

            //
            // Perform pre-conversion fixups to the Blam tag definition
            //

            switch (blamDefinition)
            {
                case ChudGlobalsDefinition chgd:
                    chgd.HudShaders.Clear();
                    for (int hudGlobalsIndex = 0; hudGlobalsIndex < chgd.HudGlobals.Count; hudGlobalsIndex++)
                        chgd.HudGlobals[hudGlobalsIndex].HudSounds.Clear();
                    break;

                case Scenario scenario:
                    if (NoForgePalette)
                    {
                        scenario.SandboxEquipment.Clear();
                        scenario.SandboxGoalObjects.Clear();
                        scenario.SandboxScenery.Clear();
                        scenario.SandboxSpawning.Clear();
                        scenario.SandboxTeleporters.Clear();
                        scenario.SandboxVehicles.Clear();
                        scenario.SandboxWeapons.Clear();
                    }
                    break;

                case ScenarioStructureBsp bsp:
                    foreach (var instance in bsp.InstancedGeometryInstances)
                        instance.Name = StringId.Invalid;
                    break;
            }
            
            //
            // Perform automatic conversion on the Blam tag definition
            //

            blamDefinition = ConvertData(cacheStream, blamDefinition, blamDefinition, blamTag.Filename);

            //
            // Perform post-conversion fixups to Blam data
            //

            switch (blamDefinition)
            {
                case Bitmap bitm:
                    blamDefinition = ConvertBitmap(bitm);
                    break;

                case Character character:
                    blamDefinition = ConvertCharacter(character);
                    break;

                case ChudDefinition chdt:
                    blamDefinition = ConvertChudDefinition(chdt);
                    break;

                case ChudGlobalsDefinition chudGlobals:
                    blamDefinition = ConvertChudGlobalsDefinition(cacheStream, chudGlobals, blamTag, edTag);
                    break;

                case Cinematic cine:
                    blamDefinition = ConvertCinematic(cine);
                    break;

                case CinematicScene cisc:
                    blamDefinition = ConvertCinematicScene(cisc);
                    break;

                case CortanaEffectDefinition crte:
                    blamDefinition = ConvertCortanaEffect(crte);
                    break;

                case Effect effect:
                    if (BlamCache.Version == CacheVersion.Halo3Retail)
                    {
                        foreach (var even in effect.Events)
                            foreach (var particleSystem in even.ParticleSystems)
                                particleSystem.Unknown7 = 1.0f / particleSystem.Unknown7;
                    }
                    break;

                case Particle particle:
                    if (BlamCache.Version != CacheVersion.Halo3Retail)
                        break;
                    // Shift all flags above 2 by 1.
                    particle.Flags = (particle.Flags & 0x3) + ((int)(particle.Flags & 0xFFFFFFFC) << 1);
                    break;

                case GlobalPixelShader glps:
                    if (UseShaderTest)
                        blamDefinition = ConvertGlobalPixelShader(glps);
                    break;

                case GlobalVertexShader glvs:
                    if (UseShaderTest)
                        blamDefinition = ConvertGlobalVertexShader(glvs);
                    break;

                case ModelAnimationGraph jmad:
                    blamDefinition = ConvertModelAnimationGraph(cacheStream, jmad);
                    break;

                case ScenarioLightmapBspData Lbsp:
                    blamDefinition = ConvertScenarionLightmapBspData(Lbsp);
                    break;

                case LensFlare lens:
                    blamDefinition = ConvertLensFlare(lens);
                    break;

                case SoundLooping lsnd:
                    blamDefinition = ConvertSoundLooping(lsnd);
                    break;

                case Globals matg:
                    blamDefinition = ConvertGlobals(matg, cacheStream);
                    break;

                case RenderModel mode:
                    // If there is no valid resource in the mode tag, null the mode itself to prevent crashes (engineer head, harness)
                    if (mode.Geometry.Resource.Page.Index == -1)
                        blamDefinition = null;
                    break;

                case PhysicsModel phmo:
                    blamDefinition = ConvertPhysicsModel(phmo);
                    break;

                case PixelShader pixl:
                    if (UseShaderTest)
                        blamDefinition = ConvertPixelShader(pixl, blamTag);
                    break;

                case VertexShader vtsh:
                    if (UseShaderTest)
                        blamDefinition = ConvertVertexShader(vtsh);
                    break;

                case Projectile proj:
                    blamDefinition = ConvertProjectile(proj);
                    break;

                case RasterizerGlobals rasg:
                    blamDefinition = ConvertRasterizerGlobals(rasg);
                    break;

                case ScenarioStructureBsp sbsp:
                    blamDefinition = ConvertScenarioStructureBsp(sbsp, edTag);
                    break;

                case Scenario scnr:
                    blamDefinition = ConvertScenario(scnr, blamTag.Filename);
                    break;

                case StructureDesign sddt:
                    blamDefinition = ConvertStructureDesign(sddt);
                    break;

                case AreaScreenEffect sefc:
                    if (blamTag.Filename == "levels\\ui\\mainmenu\\sky\\ui")
                    {
                        foreach (var screenEffect in sefc.ScreenEffects)
                        {
                            screenEffect.MaximumDistance = float.MaxValue;
                            screenEffect.Duration = float.MaxValue;
                        }
                    }
                    break;

                case SkyAtmParameters skya:
                    // Decrease secondary fog intensity (it's quite sickening in ms23)
                    foreach (var atmosphere in skya.AtmosphereProperties)
                        atmosphere.FogIntensity2 /= 36.0f;
                    break;

                case ScenarioLightmap sLdT:
                    blamDefinition = ConvertScenarioLightmap(cacheStream, blamTag.Filename, sLdT);
                    break;

                case Sound sound:
                    blamDefinition = ConvertSound(sound);
                    break;

                case SoundMix snmx:
                    blamDefinition = ConvertSoundMix(snmx);
                    break;

                case Style style:
                    blamDefinition = ConvertStyle(style);
                    break;

                case Dialogue udlg:
                    blamDefinition = ConvertDialogue(cacheStream, udlg);
                    break;

                case MultilingualUnicodeStringList unic:
                    blamDefinition = ConvertMultilingualUnicodeStringList(unic);
                    break;

                case Weapon weapon:
                    // Fix shotgun reloading
                    if (blamTag.Filename == "objects\\weapons\\rifle\\shotgun\\shotgun")
                    {
                        weapon.Unknown24 = 1 << 16;
                    }

                    foreach (var attach in weapon.Attachments)
                        if (blamTag.Filename == "objects\\vehicles\\warthog\\weapon\\warthog_horn" || blamTag.Filename == "objects\\vehicles\\mongoose\\weapon\\mongoose_horn")
                            attach.PrimaryScale = CacheContext.GetStringId("primary_rate_of_fire");
                    break;
            }

            //
            // Finalize and serialize the new ElDorado tag definition
            //
            
            if (blamDefinition == null) //If blamDefinition is null, return null tag.
            {
                Console.WriteLine($"Something happened when converting  {blamTag.Filename.Substring(Math.Max(0, blamTag.Filename.Length - 30))}, returning null tag reference.");
                CacheContext.TagNames.Remove(edTag.Index);
                CacheContext.TagCache.Index[edTag.Index] = null;
                return null;
            }

            var edContext = new TagSerializationContext(cacheStream, CacheContext, edTag);
            CacheContext.Serializer.Serialize(edContext, blamDefinition);
            CacheContext.SaveTagNames(); // Always save new tagnames in case of a crash

            Console.WriteLine($"['{edTag.Group.Tag}', 0x{edTag.Index:X4}] {CacheContext.TagNames[edTag.Index]}.{CacheContext.GetString(edTag.Group.Name)}");

            return edTag;
        }

        private object ConvertData(Stream cacheStream, object data, object definition, string blamTagName)
        {
            if (data == null)
                return null;

            var type = data.GetType();

            if (type.IsPrimitive)
                return data;

            switch (data)
            {
                case StringId stringId:
                    return ConvertStringId(stringId);

                case CachedTagInstance tag:
                    if (IsRecursive == false)
                        return null;
                    tag = PortTagReference(tag.Index);
                    if (tag != null && !(IsNew || IsReplacing))
                        return tag;
                    return ConvertTag(cacheStream, BlamCache.IndexItems.Find(i => i.ID == ((CachedTagInstance)data).Index));

                case TagFunction tagFunction:
                    return ConvertTagFunction(tagFunction);

                case RenderGeometry renderGeometry:
                    if (definition is ScenarioStructureBsp sbsp)
                        return GeometryConverter.Convert(cacheStream, renderGeometry);
                    if (definition is RenderModel mode)
                        return GeometryConverter.Convert(cacheStream, renderGeometry);
                    return GeometryConverter.Convert(cacheStream, renderGeometry);

                case RenderMethod renderMethod:
                    var rm = (RenderMethod)data;
                    
                    if (MatchShaders)
                    {
                        ConvertData(cacheStream, rm.ShaderProperties[0].ShaderMaps, rm.ShaderProperties[0].ShaderMaps, blamTagName);
                        return ConvertRenderMethod(cacheStream, rm, blamTagName);
                    }
                    else
                    {
                        // Convert structure before applying fixups
                        if (type.GetCustomAttributes(typeof(TagStructureAttribute), false).Length > 0)
                            data = ConvertStructure(cacheStream, data, type, definition, blamTagName);

                        return  ConvertRenderMethodGenerated(cacheStream, rm, blamTagName);
                    }

                case CollisionMoppCode collisionMopp:
                    collisionMopp.Data = ConvertCollisionMoppData(collisionMopp.Data);
                    return collisionMopp;

                case DamageReportingType damageReportingType:
                    return ConvertDamageReportingType(damageReportingType);

                case SoundClass soundClass:
                    return soundClass.ConvertSoundClass(BlamCache.Version);

                case GameObjectType gameObjectType:
                    return ConvertGameObjectType(gameObjectType);

                case ObjectTypeFlags objectTypeFlags:
                    return ConvertObjectTypeFlags(objectTypeFlags);

                case ScenarioObjectType scenarioObjectType:
                    return ConvertScenarioObjectType(scenarioObjectType);

                case BipedPhysicsFlags bipedPhysicsFlags:
                    return ConvertBipedPhysicsFlags(bipedPhysicsFlags);
            }

            if (type.IsArray)
                return ConvertArray(cacheStream, (Array)data, definition, blamTagName);

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                return ConvertList(cacheStream, data, type, definition, blamTagName);

            if (type.GetCustomAttributes(typeof(TagStructureAttribute), false).Length > 0)
                return ConvertStructure(cacheStream, data, type, definition, blamTagName);

            return data;
        }

        private ObjectTypeFlags ConvertObjectTypeFlags(ObjectTypeFlags objectTypeFlags)
        {
            if (BlamCache.Version == CacheVersion.Halo3Retail)
            {
                if (!Enum.TryParse(objectTypeFlags.Halo3Retail.ToString(), out objectTypeFlags.Halo3ODST))
                    throw new FormatException(BlamCache.Version.ToString());
            }

            return objectTypeFlags;
        }

        private BipedPhysicsFlags ConvertBipedPhysicsFlags(BipedPhysicsFlags bipedPhysicsFlags)
        {
            if (!Enum.TryParse(bipedPhysicsFlags.Halo3Retail.ToString(), out bipedPhysicsFlags.Halo3Odst))
                throw new FormatException(BlamCache.Version.ToString());

            return bipedPhysicsFlags;
        }

        private DamageReportingType ConvertDamageReportingType(DamageReportingType damageReportingType)
        {
            string value = null;

            switch (BlamCache.Version)
            {
                case CacheVersion.Halo2Xbox:
                case CacheVersion.Halo2Vista:
                    value = damageReportingType.Halo2Retail.ToString();
                    break;

                case CacheVersion.Halo3Retail:
                    value = damageReportingType.Halo3Retail.ToString();
                    break;

                case CacheVersion.Halo3ODST:
                    value = damageReportingType.Halo3ODST.ToString();
                    break;
            }

            if (value == null || !Enum.TryParse(value, out damageReportingType.HaloOnline))
                throw new NotSupportedException(value ?? CacheContext.Version.ToString());
            
            return damageReportingType;
        }

        private StringId ConvertStringId(StringId stringId)
        {
            var value = BlamCache.Strings.GetString(stringId);
            var edStringId = CacheContext.StringIdCache.GetStringId(stringId.Set, value);

            if ((stringId != StringId.Invalid) && (edStringId != StringId.Invalid))
                return edStringId;

            if (((stringId != StringId.Invalid) && (edStringId == StringId.Invalid)) || !CacheContext.StringIdCache.Contains(value))
                CacheContext.StringIdCache.AddString(value);

            return CacheContext.GetStringId(value);
        }

        private Array ConvertArray(Stream cacheStream, Array array, object definition, string blamTagName)
        {
            if (array.GetType().GetElementType().IsPrimitive)
                return array;

            for (var i = 0; i < array.Length; i++)
            {
                var oldValue = array.GetValue(i);
                var newValue = ConvertData(cacheStream, oldValue, definition, blamTagName);
                array.SetValue(newValue, i);
            }

            return array;
        }

        private object ConvertList(Stream cacheStream, object list, Type type, object definition, string blamTagName)
        {
            if (type.GenericTypeArguments[0].IsPrimitive)
                return list;

            var count = (int)type.GetProperty("Count").GetValue(list);

            var getItem = type.GetMethod("get_Item");
            var setItem = type.GetMethod("set_Item");

            for (var i = 0; i < count; i++)
            {
                var oldValue = getItem.Invoke(list, new object[] { i });
                var newValue = ConvertData(cacheStream, oldValue, definition, blamTagName);
                setItem.Invoke(list, new object[] { i, newValue });
            }

            return list;
        }

        private object ConvertStructure(Stream cacheStream, object data, Type type, object definition, string blamTagName)
        {
            var enumerator = new TagFieldEnumerator(new TagStructureInfo(type, CacheContext.Version));

            while (enumerator.Next())
            {
                var oldValue = enumerator.Field.GetValue(data);
                var newValue = ConvertData(cacheStream, oldValue, definition, blamTagName);
                enumerator.Field.SetValue(data, newValue);
            }

            return data;
        }

        private TagFunction ConvertTagFunction(TagFunction function)
        {
            return TagFunction.ConvertTagFunction(function);
        }

        private GameObjectType ConvertGameObjectType(GameObjectType objectType)
        {
            if (BlamCache.Version == CacheVersion.Halo3Retail)
                if (!Enum.TryParse(objectType.Halo3Retail.ToString(), out objectType.Halo3ODST))
                    throw new FormatException(BlamCache.Version.ToString());

            return objectType;
        }

        private ScenarioObjectType ConvertScenarioObjectType(ScenarioObjectType objectType)
        {
            if (BlamCache.Version == CacheVersion.Halo3Retail)
                if (!Enum.TryParse(objectType.Halo3Retail.ToString(), out objectType.Halo3ODST))
                    throw new FormatException(BlamCache.Version.ToString());

            return objectType;
        }

        private CachedTagInstance PortTagReference(int index, int maxIndex = 0xFFFF)
        {
            if (index == -1)
                return null;

            var instance = BlamCache.IndexItems.Find(i => i.ID == index);

            if (instance != null)
            {
                var chars = new char[] { ' ', ' ', ' ', ' ' };
                for (var i = 0; i < instance.ClassCode.Length; i++)
                    chars[i] = instance.ClassCode[i];

                var tags = CacheContext.TagCache.Index.FindAllInGroup(new string(chars));

                foreach (var tag in tags)
                {
                    if (!CacheContext.TagNames.ContainsKey(tag.Index))
                        continue;

                    if (instance.Filename == CacheContext.TagNames[tag.Index] && tag.Index < maxIndex)
                        return tag;
                }
            }

            return null;
        }
    }
}