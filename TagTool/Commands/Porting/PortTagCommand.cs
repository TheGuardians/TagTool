using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.Havok;
using TagTool.IO;
using TagTool.Legacy.Base;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Tags;

namespace TagTool.Commands.Porting
{
    public partial class PortTagCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private CacheFile BlamCache { get; }
        private TagDeserializer BlamDeserializer { get; }
        private RenderGeometryConverter GeometryConverter { get; }

        private Dictionary<Tag, List<string>> ReplacedTags = new Dictionary<Tag, List<string>>();

        private List<Tag> RenderMethodTagGroups = new List<Tag> { new Tag("rmbk"), new Tag("rmcs"), new Tag("rmd "), new Tag("rmfl"), new Tag("rmhg"), new Tag("rmsh"), new Tag("rmss"), new Tag("rmtr"), new Tag("rmw "), new Tag("rmrd"), new Tag("rmct") };
        private List<Tag> EffectTagGroups = new List<Tag> { new Tag("beam"), new Tag("cntl"), new Tag("ltvl"), new Tag("decs"), new Tag("shit"), new Tag("prt3")};
        private List<Tag> OtherTagGroups = new List<Tag> { new Tag("foot") };

        private bool IsReplacing = false;
        private bool IsRecursive = true;
        private bool IsWildcard = false;
        private bool IsNew = false;
        private bool UseNull = false;
        private bool NoAudio = false;
        private bool UseShaderTest = false;

        public PortTagCommand(GameCacheContext cacheContext, CacheFile blamCache) :
            base(CommandFlags.Inherit,

                "PortTag",
                "Ports a tag from the current cache file. Options are : noaudio | noreplace | replace | new | single | nonnull" + Environment.NewLine + Environment.NewLine +
                "Replace: Use existing matching tag names if available." + Environment.NewLine +
                "New: Create a new tag after the last index." + Environment.NewLine +
                "Single: Port a new tag without any reference." + Environment.NewLine +
                "UseNull: Port a tag using nulled tag indices where available." + Environment.NewLine +
                "No option: Ports a tag if its name is not present in the tag names.",

                "PortTag [Options] <Tag Group> <Tag Name>",

                "")
        {
            CacheContext = cacheContext;
            BlamCache = blamCache;
            BlamDeserializer = new TagDeserializer(BlamCache.Version);
            GeometryConverter = new RenderGeometryConverter(cacheContext, blamCache);
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 2)
                return false;

            while (args.Count > 2 || args.Count >= 2 && args[0] == "*")
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

                    case "replace":
                        IsReplacing = true;
                        break;

                    case "new":
                        IsNew = true;
                        UseNull = false;
                        break;

                    case "single":
                        IsRecursive = false;
                        UseNull = false;
                        IsNew = true;
                        break;

                    case "*":
                        IsWildcard = true;
                        UseNull = false;
                        IsNew = true;
                        break;

                    case "usenull":
                        UseNull = true;
                        break;

                    case "shadertest":
                        UseShaderTest = true;
                        break;

                    default:
                        throw new NotImplementedException(args[0]);
                }

                args.RemoveAt(0);
            }

            var initialStringIdCount = CacheContext.StringIdCache.Strings.Count;

            //
            // Verify Blam group tag
            //

            String groupName = "";
            var groupTag = ArgumentParser.ParseGroupTag(CacheContext.StringIdCache, args[0]);

#if !DEBUG
            try
            {
#endif
            groupName = CacheContext.GetString(TagGroup.Instances[groupTag].Name);
#if !DEBUG
            }
            catch (Exception)
            {
                Console.WriteLine($"Failed to find the tag group: {args[0]}");
            }
#endif

            if (groupTag == Tag.Null)
            {
                Console.WriteLine($"ERROR: Invalid tag group \"{args[0]}\"");
                return true;
            }

            //
            // Verify Blam tag instance
            //

            var blamTagName = IsWildcard ? null : args[1];

            List<CacheFile.IndexItem> blamTags = new List<CacheFile.IndexItem>();

            foreach (var tag in BlamCache.IndexItems)
            {
                if ((tag.ClassCode == groupTag.ToString()) && (IsWildcard || tag.Filename == blamTagName))
                {
                    blamTags.Add(tag);

                    if (!IsWildcard)
                        break;
                }
            }

            if (blamTags.Count == 0)
            {
                Console.WriteLine($"ERROR: Blam {groupName} tag does not exist: " + blamTagName);
                return true;
            }

            //
            // Convert Blam data to ElDorado data
            //

            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
                foreach (var blamTag in blamTags)
                    ConvertTag(cacheStream, blamTag);

            if (initialStringIdCount != CacheContext.StringIdCache.Strings.Count)
                using (var stringIdCacheStream = CacheContext.OpenStringIdCacheReadWrite())
                    CacheContext.StringIdCache.Save(stringIdCacheStream);

            CacheContext.SaveTagNames();

            return true;
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

            if (!IsNew)
            {
                foreach (var instance in CacheContext.TagCache.Index.FindAllInGroup(groupTag))
                {
                    if (instance == null || !CacheContext.TagNames.ContainsKey(instance.Index))
                        continue;

                    if (CacheContext.TagNames[instance.Index] == blamTag.Filename)
                    {
                        if (IsReplacing && !instance.IsInGroup("rm  "))
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

            var replacedTags = ReplacedTags.ContainsKey(groupTag) ?
                (ReplacedTags[groupTag] ?? new List<string>()) :
                new List<string>();

            replacedTags.Add(blamTag.Filename);
            ReplacedTags[groupTag] = replacedTags;

            //
            // Return engine default tags for any unsupported tag groups
            //

            if (RenderMethodTagGroups.Contains(groupTag) && !UseShaderTest)
            {
                if (groupTag == "rmw ")
                    return CacheContext.GetTag(0x400F);
                else if (groupTag == "rmhg")
                    return CacheContext.GetTag(0x2647);
                else if (groupTag == "rmtr")
                    return CacheContext.GetTag(0x3AAD);
                else if (groupTag == "rmcs")
                    return CacheContext.GetTag(0x101F);
                else if (groupTag == "rmd ")
                    return CacheContext.GetTag(0x1BA2);
                else if (groupTag == "rmfl")
                    return CacheContext.GetTag(0x4CA9);
                else if (groupTag == "rmct")
                    return null;
                else
                    return CacheContext.GetTag(0x101F);
            }
            else if (EffectTagGroups.Contains(groupTag) && !UseShaderTest)
            {
                if (groupTag == "beam")
                    return CacheContext.GetTag(0x18B5);
                else if (groupTag == "cntl")
                    return CacheContext.GetTag(0x528);
                else if (groupTag == "ltvl")
                    return CacheContext.GetTag(0x594);
                else if (groupTag == "decs")
                    return CacheContext.GetTag(0x3A4);
                else if (groupTag == "shit")
                    return CacheContext.GetTag(0x139C);
                else if (groupTag == "effe")
                    return CacheContext.GetTag(0x12FE);
                else
                    return CacheContext.GetTag(0x29E);
            }
            else if (OtherTagGroups.Contains(groupTag) && !UseShaderTest)
            {
                if (groupTag == "foot")
                    return CacheContext.GetTag(0xc0d);
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
            // Load the Blam tag definition and convert Blam data to ElDorado data
            //

            var blamContext = new CacheSerializationContext(CacheContext, BlamCache, blamTag);

            var blamDefinition = BlamDeserializer.Deserialize(blamContext, TagDefinition.Find(groupTag));
            blamDefinition = ConvertData(cacheStream, blamDefinition, blamDefinition, blamTag.Filename);

            //
            // Perform post-conversion fixups to Blam data
            //

            if (groupTag == "bitm")
                blamDefinition = ConvertBitmap((Bitmap)blamDefinition);

            if (groupTag == "bipd")
            {
                var biped = (Biped)blamDefinition;
                biped.PhysicsFlags = (Biped.PhysicsFlagBits)(((int)biped.PhysicsFlags & 0x7) + (((int)biped.PhysicsFlags & 0x7FFFFFF8) << 1));
            }

            if (groupTag == "char")
                blamDefinition = ConvertCharacter((Character)blamDefinition);

            if (groupTag == "cine")
                blamDefinition = ConvertCinematic((Cinematic)blamDefinition);

            if (groupTag == "cisc")
                blamDefinition = ConvertCinematicScene((CinematicScene)blamDefinition);

            if (groupTag == "effe")
            {
                var effect = (Effect)blamDefinition;

                foreach (var even in effect.Events)
                    foreach (var particle in even.ParticleSystems)
                        particle.Unknown7 = 1.0f / particle.Unknown7;
            }

            if (groupTag == "glps" && UseShaderTest)
                blamDefinition = ConvertGlobalPixelShader((GlobalPixelShader)blamDefinition);

            if (groupTag == "glvs" && UseShaderTest)
                blamDefinition = ConvertGlobalVertexShader((GlobalVertexShader)blamDefinition);

            if (groupTag == "hlmt")
            {
                var hlmt = (Model)blamDefinition;

                foreach (var damage in hlmt.NewDamageInfo)
                {
                    damage.CollisionDamageReportingType++;
                    damage.ResponseDamageReportingType++;

                    if (damage.CollisionDamageReportingType >= Model.NewDamageInfoBlock.DamageReportingTypeValue.ArmorLockCrush)
                        damage.CollisionDamageReportingType++;

                    if (damage.ResponseDamageReportingType >= Model.NewDamageInfoBlock.DamageReportingTypeValue.ArmorLockCrush)
                        damage.ResponseDamageReportingType++;
                }
            }

            if (groupTag == "jmad")
                blamDefinition = ConvertModelAnimationGraph(cacheStream, (ModelAnimationGraph)blamDefinition);

            if (groupTag == "lens")
                blamDefinition = ConvertLensFlare((LensFlare)blamDefinition);

            if (groupTag == "lsnd")
                blamDefinition = ConvertSoundLooping((SoundLooping)blamDefinition, BlamCache);

            if (groupTag == "matg")
                blamDefinition = ConvertGlobals((Globals)blamDefinition, cacheStream);

            if (groupTag == "phmo")
                blamDefinition = ConvertPhysicsModel((PhysicsModel)blamDefinition);

            if (groupTag == "pixl" && UseShaderTest)
                blamDefinition = ConvertPixelShader((PixelShader)blamDefinition);

            if (groupTag == "vtsh" && UseShaderTest)
                blamDefinition = ConvertVertexShader((VertexShader)blamDefinition);

            if (groupTag == "proj")
                blamDefinition = ConvertProjectile((Projectile)blamDefinition);

            if (groupTag == "rasg")
                blamDefinition = ConvertRasterizerGlobals((RasterizerGlobals)blamDefinition);

            if (groupTag == "sbsp")
                blamDefinition = ConvertScenarioStructureBsp((ScenarioStructureBsp)blamDefinition, edTag);

            if (groupTag == "scnr")
                blamDefinition = ConvertScenario((Scenario)blamDefinition);

            if (groupTag == "sefc")
            {
                var sefc = (AreaScreenEffect)blamDefinition;

                if (blamTag.Filename == "levels\\ui\\mainmenu\\sky\\ui")
                {
                    foreach (var screenEffect in sefc.ScreenEffects)
                    {
                        screenEffect.MaximumDistance = float.MaxValue;
                        screenEffect.Duration = float.MaxValue;
                    }
                }
            }

            if (groupTag == "sddt")
                blamDefinition = ConvertStructureDesign((StructureDesign)blamDefinition);

            if (groupTag == "sLdT")
                blamDefinition = ConvertScenarioLightmap(CacheContext, cacheStream, BlamCache, blamTag.Filename, (ScenarioLightmap)blamDefinition);

            if (groupTag == "Lbsp")
                blamDefinition = ConvertScenarionLightmapBspData((ScenarioLightmapBspData)blamDefinition);

            if (groupTag == "snd!")
                blamDefinition = ConvertSound((Sound)blamDefinition);

            if (groupTag == "snmx")
                blamDefinition = ConvertSoundMix(BlamCache, (SoundMix)blamDefinition);

            if (groupTag == "styl")
                blamDefinition = ConvertStyle((Style)blamDefinition);

            if (groupTag == "udlg")
                blamDefinition = ConvertDialogue(cacheStream, CacheContext, (Dialogue)blamDefinition);

            if (groupTag == "unic")
                blamDefinition = ConvertStrings((MultilingualUnicodeStringList)blamDefinition);

            if (groupTag == "chdt")
                blamDefinition = ConvertChudDefinition(blamContext.BlamCache.Version, (ChudDefinition)blamDefinition);

            if (groupTag == "chgd")
                blamDefinition = ConvertChudGlobalsDefinition(blamContext.BlamCache.Version, (ChudGlobalsDefinition)blamDefinition);

            if (groupTag == "weap")
            {
                var weapon = (Weapon)blamDefinition;
                weapon.MeleeDamageReportingType++;

                if (weapon.MeleeDamageReportingType >= Weapon.MeleeDamageReportingTypeValue.ArmorLockCrush)
                    weapon.MeleeDamageReportingType++;

                foreach (var barrel in weapon.Barrels)
                {
                    barrel.DamageReportingType++;

                    if (barrel.DamageReportingType >= Weapon.Barrel.DamageReportingTypeValue.ArmorLockCrush)
                        barrel.DamageReportingType++;
                }

                foreach (var attach in weapon.Attachments)
                    if (blamTag.Filename == "objects\\vehicles\\warthog\\weapon\\warthog_horn" || blamTag.Filename == "objects\\vehicles\\mongoose\\weapon\\mongoose_horn")
                        attach.PrimaryScale = CacheContext.GetStringId("primary_rate_of_fire");
            }

            //
            // Serialize new ElDorado tag definition
            //

            if (blamDefinition == null) //If blamDefinition is null, return null tag.
            {
                Console.WriteLine($"Something happened when converting  {blamTag.Filename.Substring(Math.Max(0, blamTag.Filename.Length - 30))}, returning null tag reference.");
                return null;
            }

            var edContext = new TagSerializationContext(cacheStream, CacheContext, edTag);
            CacheContext.Serializer.Serialize(edContext, blamDefinition);
            CacheContext.SaveTagNames(); // Always save new tagnames in case of a crash

            Console.WriteLine($"[Group: '{edTag.Group.Tag}', Index: 0x{edTag.Index:X4}] {CacheContext.TagNames[edTag.Index]}.{CacheContext.GetString(edTag.Group.Name)}");

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
                case RenderMethod renderMethod:
                    var rm = (RenderMethod)data;
                    ConvertData(cacheStream, rm.ShaderProperties[0].ShaderMaps, rm.ShaderProperties[0].ShaderMaps, blamTagName);
                    return ConvertRenderMethod(cacheStream, rm, blamTagName);

                case CollisionMoppCode collisionMopp:
                    collisionMopp.Data = ConvertCollisionMoppData(collisionMopp.Data);
                    return collisionMopp;

                case StringId stringId:
                    return ConvertStringId(stringId);

                case CachedTagInstance tag:
                    if (IsRecursive == false)
                        return null;
                    tag = PortTagReference(CacheContext, BlamCache, tag.Index);
                    if (tag != null && !(IsNew || IsReplacing))
                        return tag;
                    return ConvertTag(cacheStream, BlamCache.IndexItems.Find(i => i.ID == ((CachedTagInstance)data).Index));

                case TagFunction tagFunction:
                    return ConvertTagFunction(tagFunction);

                case RenderGeometry renderGeometry:
                    if (definition is ScenarioStructureBsp sbsp)
                        return GeometryConverter.Convert(cacheStream, renderGeometry, sbsp.Materials);
                    if (definition is RenderModel mode)
                        return GeometryConverter.Convert(cacheStream, renderGeometry, mode.Materials);
                    return GeometryConverter.Convert(cacheStream, renderGeometry, null);

                case GameObjectType gameObjectType:
                    return ConvertGameObjectType(gameObjectType);

                case ScenarioObjectType scenarioObjectType:
                    return ConvertScenarioObjectType(scenarioObjectType);
            }

            if (type.IsArray)
                return ConvertArray(cacheStream, (Array)data, definition, blamTagName);

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                return ConvertList(cacheStream, data, type, definition, blamTagName);

            if (type.GetCustomAttributes(typeof(TagStructureAttribute), false).Length > 0)
                return ConvertStructure(cacheStream, data, type, definition, blamTagName);

            return data;
        }

        private StringId ConvertStringId(StringId stringId)
        {
            var value = BlamCache.Strings.GetString(stringId);
            var edStringId = CacheContext.StringIdCache.GetStringId(stringId.Set, value);

            if ((stringId != StringId.Null) && (edStringId != StringId.Null))
                return edStringId;

            if (((stringId != StringId.Null) && (edStringId == StringId.Null)) || !CacheContext.StringIdCache.Contains(value))
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
            if (function == null || function.Data == null)
                return null;

            var result = new byte[function.Data.Length];

            using (var inputReader = new EndianReader(new MemoryStream(function.Data), EndianFormat.BigEndian))
            using (var outputWriter = new EndianWriter(new MemoryStream(result), EndianFormat.LittleEndian))
            {
                while (!inputReader.EOF)
                {
                    var opcode = inputReader.ReadByte();
                    var isOpcode = false;

                    switch (opcode)
                    {
                        case 0x01:
                        case 0x02:
                        case 0x03:
                        case 0x04:
                        case 0x05:
                        case 0x06:
                        case 0x07:
                        case 0x08:
                        case 0x09:
                        case 0x0A:
                            isOpcode = true;
                            break;
                    }

                    inputReader.SeekTo(inputReader.Position - 1);

                    if (isOpcode)
                    {
                        outputWriter.Format = EndianFormat.BigEndian;
                        outputWriter.Write(inputReader.ReadUInt32());
                    }
                    else
                    {
                        outputWriter.Format = EndianFormat.LittleEndian;
                        outputWriter.Write(inputReader.ReadUInt32());
                    }
                }

                function.Data = result;
            }

            return function;
        }

        private GameObjectType ConvertGameObjectType(GameObjectType objectType)
        {
            if (BlamCache.Version == CacheVersion.Halo3Retail)
                if (Enum.TryParse<GameObjectTypeHalo3ODST>(objectType.Halo3Retail.ToString(), out var result))
                    objectType.Halo3ODST = result;
                else if (BlamCache.Version == CacheVersion.Halo3ODST)
                    if (Enum.TryParse<GameObjectTypeHalo3ODST>(objectType.Halo3ODST.ToString(), out var result2))
                        objectType.Halo3ODST = result2;

            return objectType;

        }

        private ScenarioObjectType ConvertScenarioObjectType(ScenarioObjectType objectType)
        {
            if (BlamCache.Version == CacheVersion.Halo3Retail)
                if (Enum.TryParse<GameObjectTypeHalo3ODST>(objectType.Halo3Retail.ToString(), out var result))
                    objectType.Halo3ODST = result;
                else if (BlamCache.Version == CacheVersion.Halo3ODST)
                    if (Enum.TryParse<GameObjectTypeHalo3ODST>(objectType.Halo3ODST.ToString(), out var result2))
                        objectType.Halo3ODST = result2;

            return objectType;
        }

        public static CachedTagInstance PortTagReference(GameCacheContext cacheContext, CacheFile blamCache, int index, int maxIndex = 0xFFFF)
        {
            if (index == -1)
                return null;

            var instance = blamCache.IndexItems.Find(i => i.ID == index);

            if (instance != null)
            {
                var chars = new char[] { ' ', ' ', ' ', ' ' };
                for (var i = 0; i < instance.ClassCode.Length; i++)
                    chars[i] = instance.ClassCode[i];

                var tags = cacheContext.TagCache.Index.FindAllInGroup(new string(chars));

                foreach (var tag in tags)
                {
                    if (!cacheContext.TagNames.ContainsKey(tag.Index))
                        continue;

                    if (instance.Filename == cacheContext.TagNames[tag.Index] && tag.Index < maxIndex)
                        return tag;
                }
            }

            return null;
        }
    }
}