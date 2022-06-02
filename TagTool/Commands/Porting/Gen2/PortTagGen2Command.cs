using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Commands.Porting;
using TagTool.IO;
using TagTool.Tags;
using TagTool.Tags.Definitions.Gen2;
using TagTool.BlamFile;

namespace TagTool.Commands.Porting.Gen2
{
    partial class PortTagGen2Command : Command
    {
        private readonly GameCacheHaloOnlineBase Cache;
        private readonly GameCacheGen2 Gen2Cache;
        string[] argParameters = new string[0];
        PortingFlags PortFlags;
        private Dictionary<int, CachedTag> PortedTags = new Dictionary<int, CachedTag>();
        private List<int> InProgressTags = new List<int>();

        public PortTagGen2Command(GameCacheHaloOnlineBase cache, GameCacheGen2 gen2Cache) : base(false, "PortTag", "", "", "")
        {
            Cache = cache;
            Gen2Cache = gen2Cache;
        }

        public override object Execute(List<string> args)
        {
            var resourceStreams = new Dictionary<ResourceLocation, Stream>();

            if (args.Count < 1)
                return new TagToolError(CommandError.ArgCount);

            var portingOptions = args.Take(args.Count - 1).ToList();
            argParameters = ParsePortingOptions(portingOptions);

            try
            {
                using (var cacheStream = Cache.OpenCacheReadWrite())
                using (var gen2CacheStream = Gen2Cache.OpenCacheRead())
                {
                    foreach (var gen2Tag in ParseLegacyTag(args.Last()))
                        ConvertTag(cacheStream, gen2CacheStream, resourceStreams, gen2Tag);
                }
            }
            finally
            {
                foreach (var pair in resourceStreams)
                    pair.Value.Close();

                Cache.SaveStrings();
                Cache.SaveTagNames();
            }

            return true;
        }

        public CachedTag ConvertTag(Stream cacheStream, Stream gen2CacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, CachedTag gen2Tag)
        {
            //tag type checking not necessary here currently as it is handled in the subfunction
            /*
            if (Gen2Cache.TagCache.TagDefinitions.GetTagDefinitionType(gen2Tag.Group.Tag) == null ||
                Cache.TagCache.TagDefinitions.GetTagDefinitionType(gen2Tag.Group.Tag) == null)
            {
                new TagToolError(CommandError.CustomError, $"Failed to convert tag '{gen2Tag}' Group not supported. Returning null");
                return null;
            }
            */

            if (PortedTags.ContainsKey(gen2Tag.Index))
                return PortedTags[gen2Tag.Index];

            //keep track of tags currently being ported to handle child tags that reference the parent (infinite loop)
            if (InProgressTags.Contains(gen2Tag.Index))
                return null;
            InProgressTags.Add(gen2Tag.Index);

            CachedTag result = ConvertTagInternal(cacheStream, gen2CacheStream, resourceStreams, gen2Tag);

            //tag is now done porting
            InProgressTags.Remove(gen2Tag.Index);

            PortedTags[gen2Tag.Index] = result;
            return result;
        }

        private CachedTag ConvertTagInternal(Stream cacheStream, Stream gen2CacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, CachedTag gen2Tag)
        {
            //use hardcoded list of supported tags to prevent unnecessary deserialization
            List<string> supportedTagGroups = new List<string>
            {
                "coll",
                "jmad",
                "phmo",
                "mode",
                "hlmt",
                "bitm",
                "bloc",
                "vehi",
                "weap",
                "scen",
                "jpt!",
                "proj",
                "trak",
                "shad",
                "sbsp",
                "scnr"
            };
            if (!supportedTagGroups.Contains(gen2Tag.Group.ToString()))
            {
                new TagToolWarning($"Porting tag group '{gen2Tag.Group}' not yet supported, returning null!");
                return null;
            }

            CachedTag destinationTag = null;
            foreach (var instance in Cache.TagCache.TagTable)
            {
                var grouptag = gen2Tag.Group.Tag;

                //method for finding tags with altered tag groups
                switch (grouptag.ToString())
                {
                    case "shad":
                        grouptag = new Tag("rmsh");
                        break;
                    default:
                        break;
                }

                if (instance == null || !instance.IsInGroup(grouptag) || instance.Name == null || instance.Name != gen2Tag.Name)
                    continue;
                if (!PortingFlagIsSet(PortingFlags.Replace))
                    return instance;
                else
                    destinationTag = instance;
            }

            object gen2definition = Gen2Cache.Deserialize(gen2CacheStream, gen2Tag);
            gen2definition = ConvertData(cacheStream, gen2CacheStream, resourceStreams, gen2definition, gen2definition, gen2Tag);
            TagStructure definition;

            switch (gen2definition)
            {
                case CollisionModel collisionModel:
                    definition = ConvertCollisionModel(collisionModel);
                    break;
                case ModelAnimationGraph modelAnimationGraph:
                    definition = ConvertModelAnimationGraph(modelAnimationGraph);
                    break;
                case PhysicsModel physicsModel:
                    definition = ConvertPhysicsModel(physicsModel);
                    break;
                case RenderModel renderModel:
                    definition = ConvertRenderModel(renderModel);
                    break;
                case Model model:
                    definition = ConvertModel(model, cacheStream);
                    break;
                case Bitmap bitmap:
                    definition = ConvertBitmap(bitmap);
                    break;
                case Crate crate:
                case Scenery scenery:
                case Weapon weapon:
                case Vehicle vehicle:
                case Projectile projectile:
                case CameraTrack track:
                    definition = ConvertObject(gen2definition, cacheStream);
                    break;
                case DamageEffect damage:
                    definition = ConvertEffect(damage);
                    break;
                case Shader shader:
                    //preserve a copy of unconverted data
                    Shader oldshader = Gen2Cache.Deserialize<Shader>(gen2CacheStream, gen2Tag);
                    definition = ConvertShader(shader, oldshader, cacheStream);
                    break;
                case ScenarioStructureBsp sbsp:
                    definition = ConvertStructureBSP(sbsp);
                    break;
                case Scenario scnr:
                    Scenario oldscnr = Gen2Cache.Deserialize<Scenario>(gen2CacheStream, gen2Tag);
                    definition = ConvertScenario(scnr, oldscnr, gen2Tag.Name, cacheStream, gen2CacheStream, resourceStreams);
                    break;
                default:
                    new TagToolWarning($"Porting tag group '{gen2Tag.Group}' not yet supported, returning null");
                    return null;
            }

            if (definition == null)
                return null;

            //allocate and serialize tag after conversion
            if (destinationTag == null)
                destinationTag = Cache.TagCache.AllocateTag(definition.GetType(), gen2Tag.Name);

            Cache.Serialize(cacheStream, destinationTag, definition);

            PostFixups(definition, destinationTag, cacheStream);

            Console.WriteLine($"['{destinationTag.Group.Tag}', 0x{destinationTag.Index:X4}] {destinationTag}");

            return destinationTag;
        }

        public void PostFixups(TagStructure definition, CachedTag destinationTag, Stream cacheStream)
        {
            switch (definition)
            {
                case TagTool.Tags.Definitions.ScenarioStructureBsp sbsp:
                    foreach (var cluster in sbsp.Clusters)
                        cluster.InstancedGeometryPhysics.StructureBsp = destinationTag;
                    break;
                case TagTool.Tags.Definitions.Scenario scnr:
                    GenerateMapFile(cacheStream, Cache, destinationTag, destinationTag.Name.Split('\\').ToList().Last(), "", "Bungie");
                    break;
                default:
                    return;
            }
            Cache.Serialize(cacheStream, destinationTag, definition);
        }

        private void GenerateMapFile(Stream cacheStream, GameCache cache, CachedTag scenarioTag, string mapName, string mapDescription, string author)
        {
            var scenarioName = Path.GetFileName(scenarioTag.Name);
            var scnr = cache.Deserialize<TagTool.Tags.Definitions.Scenario>(cacheStream, scenarioTag);

            var mapBuilder = new MapFileBuilder(cache.Version);
            mapBuilder.MapName = mapName;
            mapBuilder.MapDescription = mapDescription;
            MapFile map = mapBuilder.Build(scenarioTag, scnr);

            if (cache is GameCacheModPackage)
            {
                var mapStream = new MemoryStream();
                var writer = new EndianWriter(mapStream, leaveOpen: true);
                map.Write(writer);

                var modPackCache = cache as GameCacheModPackage;
                modPackCache.AddMapFile(mapStream, scnr.MapId);
            }
            else
            {
                var mapFile = new FileInfo(Path.Combine(cache.Directory.FullName, $"{scenarioName}.map"));

                using (var mapFileStream = mapFile.Create())
                {
                    map.Write(new EndianWriter(mapFileStream));
                }
            }
        }

        public object ConvertData(Stream cacheStream, Stream gen2CacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, object data, object definition, CachedTag gen2Tag)
        {
            switch (data)
            {
                case StringId stringId:
                    stringId = ConvertStringId(stringId);
                    return stringId;
                case null:  // no conversion necessary
                case ValueType _:   // no conversion necessary
                case string _:  // no conversion necessary
                    return data;
                case CachedTag tag:
                    if (!PortingFlagIsSet(PortingFlags.Recursive))
                    {
                        foreach (var instance in Cache.TagCache.FindAllInGroup(tag.Group.Tag))
                        {
                            if (instance == null || instance.Name == null)
                                continue;

                            if (instance.Name == tag.Name)
                                return instance;
                        }

                        return null;
                    }
                    return ConvertTag(cacheStream, gen2CacheStream, resourceStreams, tag);
                case Array _:
                case IList _: // All arrays and List<T> implement IList, so we should just use that
                    data = ConvertCollection(cacheStream, gen2CacheStream, resourceStreams, data as IList, definition, gen2Tag);
                    return data;
                case TagStructure tagStructure: // much faster to pattern match a type than to check for custom attributes.
                    return ConvertStructure(cacheStream, gen2CacheStream, resourceStreams, tagStructure, definition, gen2Tag);
                case PlatformSignedValue _:
                case PlatformUnsignedValue _:
                    return data;
                default:
                    new TagToolWarning($"Unhandled type in `ConvertData`: {data.GetType().Name} (probably harmless).");
                    break;
            }

            return data;
        }

        private IList ConvertCollection(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, IList data, object definition, CachedTag gen2Tag)
        {
            // return early where possible
            if (data is null || data.Count == 0)
                return data;

            if (data[0] == null)
                return data;

            var type = data[0].GetType();
            if ((type.IsValueType && type != typeof(StringId)) ||
                type == typeof(string))
                return data;

            // convert each element
            for (var i = 0; i < data.Count; i++)
            {
                var oldValue = data[i];
                var newValue = ConvertData(cacheStream, blamCacheStream, resourceStreams, oldValue, definition, gen2Tag);
                data[i] = newValue;
            }

            return data;
        }

        private T ConvertStructure<T>(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, T data, object definition, CachedTag gen2Tag) where T : TagStructure
        {
            foreach (var tagFieldInfo in TagStructure.GetTagFieldEnumerable(data.GetType(), Gen2Cache.Version, Gen2Cache.Platform))
            {
                var attr = tagFieldInfo.Attribute;
                if (!CacheVersionDetection.TestAttribute(attr, Gen2Cache.Version, Gen2Cache.Platform))
                    continue;

                // skip the field if no conversion is needed
                if ((tagFieldInfo.FieldType.IsValueType && tagFieldInfo.FieldType != typeof(StringId)) ||
                tagFieldInfo.FieldType == typeof(string))
                    continue;

                var oldValue = tagFieldInfo.GetValue(data);
                if (oldValue is null)
                    continue;

                // convert the field
                var newValue = ConvertData(cacheStream, blamCacheStream, resourceStreams, oldValue, definition, gen2Tag);
                tagFieldInfo.SetValue(data, newValue);
            }

            return data;
        }

        public StringId ConvertStringId(StringId stringId)
        {
            if (stringId == StringId.Invalid)
                return stringId;

            var value = Gen2Cache.StringTable.GetString(stringId);
            var edStringId = Cache.StringTable.GetStringId(value);

            if (edStringId != StringId.Invalid)
                return edStringId;

            if (edStringId == StringId.Invalid || !Cache.StringTable.Contains(value))
                return Cache.StringTable.AddString(value);

            return stringId;
        }

        public Damage.DamageReportingType ConvertDamageReportingType(Damage.DamageReportingType damageReportingType)
        {
            string value = damageReportingType.Halo2Retail.ToString();

            if (value == null || !Enum.TryParse(value, out damageReportingType.HaloOnline))
            {
                new TagToolWarning($"Unsupported Damage reporting type '{value}'. Using default.");
                damageReportingType.HaloOnline = Damage.DamageReportingType.HaloOnlineValue.GuardiansUnknown;
            }

            return damageReportingType;
        }

        private List<CachedTag> ParseLegacyTag(string tagSpecifier)
        {
            List<CachedTag> result = new List<CachedTag>();

            if (tagSpecifier.Length == 0 || (!char.IsLetter(tagSpecifier[0]) && !tagSpecifier.Contains('*')) || !tagSpecifier.Contains('.'))
            {
                new TagToolError(CommandError.CustomError, $"Invalid tag name: {tagSpecifier}");
                return new List<CachedTag>();
            }

            var tagIdentifiers = tagSpecifier.Split('.');

            if (!Cache.TagCache.TryParseGroupTag(tagIdentifiers[1], out var groupTag))
            {
                new TagToolError(CommandError.CustomError, $"Invalid tag name: {tagSpecifier}");
                return new List<CachedTag>();
            }

            var tagName = tagIdentifiers[0];

            // find the CacheFile.IndexItem(s)
            if (tagName == "*") result = Gen2Cache.TagCache.TagTable.ToList().FindAll(
                item => item != null && item.IsInGroup(groupTag));
            else result.Add(Gen2Cache.TagCache.TagTable.ToList().Find(
                item => item != null && item.IsInGroup(groupTag) && tagName == item.Name));

            if (result.Count == 0 || result.Any(r => r == null))
            {
                new TagToolError(CommandError.CustomError, $"Invalid tag name: {tagSpecifier}");
                return new List<CachedTag>();
            }

            return result;
        }

        private string[] ParsePortingOptions(List<string> args)
        {
            PortFlags = PortingFlags.Default;

            var flagNames = Enum.GetNames(typeof(PortingFlags)).Select(name => name.ToLower());
            var flagValues = Enum.GetValues(typeof(PortingFlags)) as PortingFlags[];

            string[] argParameters = new string[0];

            for (var a = 0; a < args.Count(); a++)
            {
                string[] argSegments = args[a].Split('[');

                var arg = argSegments[0].ToLower();

                // Use '!' or 'No' to negate an argument.
                var toggleOn = !(arg.StartsWith("!") || arg.StartsWith("no"));
                if (!toggleOn && arg.StartsWith("!"))
                    arg = arg.Remove(0, 1);
                else if (!toggleOn && arg.StartsWith("no"))
                    arg = arg.Remove(0, 2);

                // Throw exceptions at clumsy typers.
                if (!flagNames.Contains(arg))
                    throw new FormatException($"Invalid {typeof(PortingFlags).FullName}: {args[0]}");

                // Add/remove flags based on if they appeared as arguments, 
                // and whether they were negated with '!' or 'No'
                for (var i = 0; i < flagNames.Count(); i++)
                    if (arg == flagNames.ElementAt(i))
                        if (toggleOn)
                            PortFlags |= flagValues[i];
                        else
                            PortFlags &= ~flagValues[i];
            }
            return argParameters;
        }

        /// <summary>
		/// Flags which can be used to affect the behavior of <see cref="PortTagCommand"/>.
		/// </summary>
		[Flags]
        public enum PortingFlags
        {
            /// <summary>
            /// Replace tags of the same name when porting.
            /// </summary>
            Replace = 1 << 0,

            /// <summary>
            /// Recursively port all tag references available.
            /// </summary>
            Recursive = 1 << 1,

            // No [PortingFlagDescription] here means we'll flag names as the description.
            Default = Recursive
        }
        public bool PortingFlagIsSet(PortingFlags flag) => (PortFlags & flag) != 0;
    }
}