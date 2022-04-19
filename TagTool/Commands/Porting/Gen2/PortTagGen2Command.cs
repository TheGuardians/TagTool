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
using CollisionModelGen2 = TagTool.Tags.Definitions.Gen2.CollisionModel;
using ModelAnimationGraphGen2 = TagTool.Tags.Definitions.Gen2.ModelAnimationGraph;
using PhysicsModelGen2 = TagTool.Tags.Definitions.Gen2.PhysicsModel;
using RenderModelGen2 = TagTool.Tags.Definitions.Gen2.RenderModel;
using ModelGen2 = TagTool.Tags.Definitions.Gen2.Model;

namespace TagTool.Commands.Porting.Gen2
{
    partial class PortTagGen2Command : Command
    {
        private readonly GameCacheHaloOnlineBase Cache;
        private readonly GameCacheGen2 Gen2Cache;
        string[] argParameters = new string[0];
        PortingFlags PortFlags;
        private Dictionary<int, CachedTag> PortedTags = new Dictionary<int, CachedTag>();
        private Dictionary<Tag, List<string>> ReplacedTags = new Dictionary<Tag, List<string>>();

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
            CachedTag result = ConvertTagInternal(cacheStream, gen2CacheStream, resourceStreams, gen2Tag);
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
                "hlmt"
            };
            if (!supportedTagGroups.Contains(gen2Tag.Group.ToString()))
            {
                new TagToolWarning($"Porting tag group '{gen2Tag.Group}' not yet supported!");
                return null;
            }

            CachedTag destinationTag = null;
            foreach (var instance in Cache.TagCache.TagTable)
            {
                if (instance == null || !instance.IsInGroup(gen2Tag.Group.Tag) || instance.Name == null || instance.Name != gen2Tag.Name)
                    continue;
                if (!PortFlags.HasFlag(PortingFlags.Replace))
                    return instance;
                else
                    destinationTag = instance;
            }

            object definition = Gen2Cache.Deserialize(gen2CacheStream, gen2Tag);
            definition = ConvertData(cacheStream, gen2CacheStream, resourceStreams, definition, definition, gen2Tag.Name);

            switch (definition)
            {
                case CollisionModelGen2 collisionModel:
                    definition = ConvertCollisionModel(collisionModel);
                    break;
                case ModelAnimationGraphGen2 modelAnimationGraph:
                    definition = ConvertModelAnimationGraph(modelAnimationGraph);
                    break;
                case PhysicsModelGen2 physicsModel:
                    definition = ConvertPhysicsModel(physicsModel);
                    break;
                case RenderModelGen2 renderModel:
                    definition = ConvertRenderModel(renderModel);
                    break;
                case ModelGen2 Model:
                    definition = ConvertModel(Model);
                    break;
                default:
                    new TagToolWarning($"Porting tag group '{gen2Tag.Group}' not yet supported!");
                    return null;
            }

            //allocate and serialize tag after conversion
            if(destinationTag == null)
                destinationTag = Cache.TagCache.AllocateTag(definition.GetType(), gen2Tag.Name);

            if (definition != null)
                Cache.Serialize(cacheStream, destinationTag, definition);

            Console.WriteLine($"['{destinationTag.Group.Tag}', 0x{destinationTag.Index:X4}] {destinationTag}");

            return destinationTag;
        }

        public object ConvertData(Stream cacheStream, Stream gen2CacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, object data, object definition, string blamTagName)
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
                    if (!PortFlags.HasFlag(PortingFlags.Recursive))
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
                    //prevent stack overflow from self-referencing tags
                    if (tag.Name == blamTagName)
                        return null;
                    return ConvertTag(cacheStream, gen2CacheStream, resourceStreams, tag);
                case Array _:
                case IList _: // All arrays and List<T> implement IList, so we should just use that
                    data = ConvertCollection(cacheStream, gen2CacheStream, resourceStreams, data as IList, definition, blamTagName);
                    return data;
                case TagStructure tagStructure: // much faster to pattern match a type than to check for custom attributes.
                    return ConvertStructure(cacheStream, gen2CacheStream, resourceStreams, tagStructure, definition, blamTagName);
                case PlatformSignedValue _:
                case PlatformUnsignedValue _:
                    return data;
                default:
                    new TagToolWarning($"Unhandled type in `ConvertData`: {data.GetType().Name} (probably harmless).");
                    break;
            }

            return data;
        }

        private IList ConvertCollection(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, IList data, object definition, string blamTagName)
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
                var newValue = ConvertData(cacheStream, blamCacheStream, resourceStreams, oldValue, definition, blamTagName);
                data[i] = newValue;
            }

            return data;
        }

        private T ConvertStructure<T>(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, T data, object definition, string blamTagName) where T : TagStructure
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
                var newValue = ConvertData(cacheStream, blamCacheStream, resourceStreams, oldValue, definition, blamTagName);
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
            None = 0,
            /// <summary>
            /// Replace tags of the same name when porting.
            /// </summary>
            Replace = 1 << 0,

            /// <summary>
            /// Recursively port all tag references available.
            /// </summary>
            Recursive = 1 << 1,

            // No [PortingFlagDescription] here means we'll flag names as the description.
            Default = None
        }
    }
}
