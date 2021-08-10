using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Audio;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Tags;
using TagTool.IO;
using TagTool.Commands.Porting;

namespace TagTool.Commands.Porting
{
    partial class PortTagGen1Command : Command
    {
        private readonly GameCacheHaloOnlineBase Cache;
        private readonly GameCacheGen1 Gen1Cache;

        public PortTagGen1Command(GameCacheHaloOnlineBase cache, GameCacheGen1 gen1Cache) : base(false, "PortTag", "", "", "")
        {
            Cache = cache;
            Gen1Cache = gen1Cache;
        }

        public override object Execute(List<string> args)
        {
            var resourceStreams = new Dictionary<ResourceLocation, Stream>();

            if (args.Count < 1)
                return new TagToolError(CommandError.ArgCount);

            try
            {
                using (var cacheStream = Cache.OpenCacheReadWrite())
                using (var gen1CacheStream = Gen1Cache.OpenCacheRead())
                {
                    foreach (var gen1Tag in ParseLegacyTag(args.Last()))
                        ConvertTag(cacheStream, gen1CacheStream, resourceStreams, gen1Tag);
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

        public CachedTag ConvertTag(Stream cacheStream, Stream gen1CacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, CachedTag gen1Tag)
        {
            if (Gen1Cache.TagCache.TagDefinitions.GetTagDefinitionType(gen1Tag.Group.Tag) == null ||
                Cache.TagCache.TagDefinitions.GetTagDefinitionType(gen1Tag.Group.Tag) == null)
            {
                new TagToolError(CommandError.CustomError, $"Failed to convert tag '{gen1Tag}' Group not supported. Returning null");
                return null;
            }
            return ConvertTagInternal(cacheStream, gen1CacheStream, resourceStreams, gen1Tag);
        }

        private CachedTag ConvertTagInternal(Stream cacheStream, Stream gen1CacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, CachedTag gen1Tag)
        {
            object definition = Gen1Cache.Deserialize(gen1CacheStream, gen1Tag);
            definition = ConvertData(cacheStream, gen1CacheStream, resourceStreams, definition, definition, gen1Tag.Name);

            var tag = Cache.TagCache.AllocateTag(definition.GetType(), gen1Tag.Name);

            if (definition != null)
                Cache.Serialize(cacheStream, tag, definition);

            Console.WriteLine($"['{tag.Group.Tag}', 0x{tag.Index:X4}] {tag}");

            return tag;
        }

        public object ConvertData(Stream cacheStream, Stream gen1CacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, object data, object definition, string blamTagName)
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
                    data = ConvertCollection(cacheStream, gen1CacheStream, resourceStreams, data as IList, definition, blamTagName);
                    return data;
                case TagStructure tagStructure: // much faster to pattern match a type than to check for custom attributes.
                    return ConvertStructure(cacheStream, gen1CacheStream, resourceStreams, tagStructure, definition, blamTagName);
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
            foreach (var tagFieldInfo in TagStructure.GetTagFieldEnumerable(data.GetType(), Gen1Cache.Version, Gen1Cache.Platform))
            {
                var attr = tagFieldInfo.Attribute;
                if (!CacheVersionDetection.AttributeInCacheVersion(attr, Gen1Cache.Version))
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

            var value = Gen1Cache.StringTable.GetString(stringId);
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
            if (tagName == "*") result = Gen1Cache.TagCache.TagTable.ToList().FindAll(
                item => item != null && item.IsInGroup(groupTag));
            else result.Add(Gen1Cache.TagCache.TagTable.ToList().Find(
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
