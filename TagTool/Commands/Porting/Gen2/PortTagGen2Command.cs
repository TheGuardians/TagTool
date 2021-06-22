using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.IO;
using TagTool.Tags;
using TagTool.Commands.Porting;
using CollisionModelGen2 = TagTool.Tags.Definitions.Gen2.CollisionModel;
using ModelAnimationGraphGen2 = TagTool.Tags.Definitions.Gen2.ModelAnimationGraph;
using PhysicsModelGen2 = TagTool.Tags.Definitions.Gen2.PhysicsModel;

namespace TagTool.Commands.Porting.Gen2
{
    partial class PortTagGen2Command : Command
    {
        private readonly GameCacheHaloOnlineBase Cache;
        private readonly GameCacheGen2 Gen2Cache;

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
            if (Gen2Cache.TagCache.TagDefinitions.GetTagDefinitionType(gen2Tag.Group.Tag) == null ||
                Cache.TagCache.TagDefinitions.GetTagDefinitionType(gen2Tag.Group.Tag) == null)
            {
                new TagToolError(CommandError.CustomError, $"Failed to convert tag '{gen2Tag}' Group not supported. Returning null");
                return null;
            }
            return ConvertTagInternal(cacheStream, gen2CacheStream, resourceStreams, gen2Tag);
        }

        private CachedTag ConvertTagInternal(Stream cacheStream, Stream gen2CacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, CachedTag gen2Tag)
        {
            object definition = Gen2Cache.Deserialize(gen2CacheStream, gen2Tag);
            definition = ConvertData(cacheStream, gen2CacheStream, resourceStreams, definition, definition, gen2Tag.Name);

            var tag = Cache.TagCache.AllocateTag(definition.GetType(), gen2Tag.Name);

            switch (definition)
            {
                case CollisionModelGen2 collisionModel:
                    definition = ConvertCollisionModel(tag, collisionModel);
                    break;
                case ModelAnimationGraphGen2 modelAnimationGraph:
                    definition = ConvertModelAnimationGraph(tag, modelAnimationGraph);
                    break;
                case PhysicsModelGen2 physicsModel:
                    definition = ConvertPhysicsModel(tag, physicsModel);
                    break;
            }

            if (definition != null)
                Cache.Serialize(cacheStream, tag, definition);

            Console.WriteLine($"['{tag.Group.Tag}', 0x{tag.Index:X4}] {tag}");

            return tag;
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
                    return null;
                    //FOR NOW, RETURN NULL INSTEAD OF PORTING DEPS, AS TAG SUPPORT IS LIMITED
                    //return ConvertTag(cacheStream, gen2CacheStream, resourceStreams, tag);
                case Array _:
                case IList _: // All arrays and List<T> implement IList, so we should just use that
                    data = ConvertCollection(cacheStream, gen2CacheStream, resourceStreams, data as IList, definition, blamTagName);
                    return data;
                case TagStructure tagStructure: // much faster to pattern match a type than to check for custom attributes.
                    return ConvertStructure(cacheStream, gen2CacheStream, resourceStreams, tagStructure, definition, blamTagName);
                default:
                    Console.WriteLine($"WARNING: Unhandled type in `ConvertData`: {data.GetType().Name} (probably harmless).");
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
            foreach (var tagFieldInfo in TagStructure.GetTagFieldEnumerable(data.GetType(), Gen2Cache.Version))
            {
                var attr = tagFieldInfo.Attribute;
                if (!CacheVersionDetection.AttributeInCacheVersion(attr, Gen2Cache.Version))
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

            if (result.Count == 0)
            {
                new TagToolError(CommandError.CustomError, $"Invalid tag name: {tagSpecifier}");
                return new List<CachedTag>();
            }

            return result;
        }
    }
}
