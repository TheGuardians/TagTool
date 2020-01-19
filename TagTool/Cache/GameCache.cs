using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.BlamFile;
using TagTool.Cache.HaloOnline;

namespace TagTool.Cache
{
    public abstract class GameCache
    {
        public string DisplayName = "default";
        public CacheVersion Version;
        public EndianFormat Endianness;
        public TagSerializer Serializer;
        public TagDeserializer Deserializer;
        public DirectoryInfo Directory;

        public List<LocaleTable> LocaleTables;
        public abstract StringTable StringTable { get; }
        public abstract TagCache TagCache { get; }
        public abstract ResourceCache ResourceCache { get; }

        public abstract Stream OpenCacheRead();
        public abstract Stream OpenCacheReadWrite();
        public abstract Stream OpenCacheWrite();

        public abstract void Serialize(Stream stream, CachedTag instance, object definition);
        public abstract object Deserialize(Stream stream, CachedTag instance);
        public abstract T Deserialize<T>(Stream stream, CachedTag instance);

        public static GameCache Open(FileInfo file)
        {
            MapFile map = new MapFile();
            var estimatedVersion = CacheVersion.HaloOnline106708;

            using (var stream = file.OpenRead())
            using (var reader = new EndianReader(stream))
            {
                if (file.Name.Contains(".map"))
                {
                    map.Read(reader);
                    estimatedVersion = map.Version;
                }
                else if (file.Name.Equals("tags.dat"))
                    estimatedVersion = CacheVersion.HaloOnline106708;
                else if (file.Name.Contains(".pak"))
                {
                    return new GameCacheModPackage(file);
                }
                else
                    throw new Exception("Invalid file passed to GameCache constructor");
            }

            switch (estimatedVersion)
            {
                case CacheVersion.HaloPC:
                case CacheVersion.HaloXbox:
                case CacheVersion.Halo2Vista:
                case CacheVersion.Halo2Xbox:
                    throw new Exception("Not implemented!");

                case CacheVersion.Halo3Beta:
                case CacheVersion.Halo3ODST:
                case CacheVersion.Halo3Retail:
                case CacheVersion.HaloReach:
                    return new GameCacheGen3(map, file);

                case CacheVersion.HaloOnline106708:
                case CacheVersion.HaloOnline235640:
                case CacheVersion.HaloOnline301003:
                case CacheVersion.HaloOnline327043:
                case CacheVersion.HaloOnline372731:
                case CacheVersion.HaloOnline416097:
                case CacheVersion.HaloOnline430475:
                case CacheVersion.HaloOnline454665:
                case CacheVersion.HaloOnline449175:
                case CacheVersion.HaloOnline498295:
                case CacheVersion.HaloOnline530605:
                case CacheVersion.HaloOnline532911:
                case CacheVersion.HaloOnline554482:
                case CacheVersion.HaloOnline571627:
                case CacheVersion.HaloOnline700123:
                    var directory = file.Directory.FullName;
                    var tagsPath = Path.Combine(directory, "tags.dat");
                    var tagsFile = new FileInfo(tagsPath);

                    if (!tagsFile.Exists)
                        throw new Exception("Failed to find tags.dat");

                    return new GameCacheHaloOnline(tagsFile.Directory);
            }

            return null;
        }

        // Utilities, I believe they don't belong here but I haven't found a better solution yet. I think GroupTag should store the string,
        // not the stringid, therefore we could hardcode the list of tag group types

        public bool TryGetCachedTag(int index, out CachedTag instance)
        {
            if (index < 0 || index >= TagCache.TagTable.Count())
            {
                instance = null;
                return false;
            }

            instance = TagCache.GetTag(index);
            return true;
        }

        public bool TryGetCachedTag(string name, out CachedTag result)
        {
            if (name.Length == 0)
            {
                result = null;
                return false;
            }

            if (name == "null")
            {
                result = null;
                return true;
            }

            if (name == "*")
            {
                if (TagCache.TagTable.Count() == 0)
                {
                    result = null;
                    return false;
                }

                result = TagCache.TagTable.Last();
                return true;
            }

            if (name.StartsWith("*."))
            {
                if (!name.TrySplit('.', out var startNamePieces) || !TryParseGroupTag(startNamePieces[1], out var starGroupTag))
                {
                    result = null;
                    return false;
                }

                result = TagCache.TagTable.Last(tag => tag != null && tag.IsInGroup(starGroupTag));
                return true;
            }

            if (name.StartsWith("0x"))
            {
                name = name.Substring(2);

                if (name.TrySplit('.', out var hexNamePieces))
                    name = hexNamePieces[0];

                if (!int.TryParse(name, NumberStyles.HexNumber, null, out int tagIndex) || !TagCache.IsTagIndexValid(tagIndex))
                {
                    result = null;
                    return false;
                }

                result = TagCache.GetTag(tagIndex);

                if (result == null) // failsafe for null tags
                    result = TagCache.CreateCachedTag(tagIndex, TagGroup.None);

                return true;
            }

            if (!name.TrySplit('.', out var namePieces) || !TryParseGroupTag(namePieces[namePieces.Length - 1], out var groupTag))
                throw new Exception($"Invalid tag name: {name}");

            var tagName = name.Substring(0, name.Length - (1 + namePieces[namePieces.Length - 1].Length));

            foreach (var instance in TagCache.TagTable)
            {
                if (instance is null)
                    continue;

                if (instance.IsInGroup(groupTag) && instance.Name == tagName)
                {
                    result = instance;
                    return true;
                }
            }

            result = null;
            return false;
        }

        public bool TryParseGroupTag(string name, out Tag result)
        {
            if (TagDefinition.TryFind(name, out var type))
            {
                var attribute = TagStructure.GetTagStructureAttribute(type);
                result = new Tag(attribute.Tag);
                return true;
            }

            foreach (var pair in TagGroup.Instances)
            {
                if (name == StringTable.GetString(pair.Value.Name))
                {
                    result = pair.Value.Tag;
                    return true;
                }
            }

            result = Tag.Null;
            return name == "none" || name == "null";
        }

        public bool TryGetTag<T>(string name, out CachedTag result) where T : TagStructure
        {
            if (name == "none" || name == "null")
            {
                result = null;
                return true;
            }

            if (Tags.TagDefinition.Types.Values.Contains(typeof(T)))
            {
                var groupTag = Tags.TagDefinition.Types.First((KeyValuePair<Tag, Type> entry) => entry.Value == typeof(T)).Key;

                foreach (var instance in TagCache.TagTable)
                {
                    if (instance is null)
                        continue;

                    if (instance.IsInGroup(groupTag) && instance.Name == name)
                    {
                        result = instance;
                        return true;
                    }
                }
            }

            result = null;
            return false;
        }

        public CachedTag GetTag<T>(string name) where T : TagStructure
        {
            if (TryGetTag<T>(name, out var result))
                return result;

            var attribute = TagStructure.GetTagStructureAttribute(typeof(T));
            var typeName = attribute.Name ?? typeof(T).Name.ToSnakeCase();

            throw new KeyNotFoundException($"'{typeName}' tag \"{name}\"");
        }

        public bool TryGetTag(string name, out CachedTag result)
        {
            if (name.Length == 0)
            {
                result = null;
                return false;
            }

            if (name == "null")
            {
                result = null;
                return true;
            }

            if (name == "*")
            {
                if (TagCache.Count == 0)
                {
                    result = null;
                    return false;
                }

                result = TagCache.TagTable.Last();
                return true;
            }

            if (name.StartsWith("*."))
            {
                if (!name.TrySplit('.', out var startNamePieces) || !TryParseGroupTag(startNamePieces[1], out var starGroupTag))
                {
                    result = null;
                    return false;
                }

                result = TagCache.TagTable.Last(tag => tag != null && tag.IsInGroup(starGroupTag));
                return true;
            }

            if (name.StartsWith("0x"))
            {
                name = name.Substring(2);

                if (name.TrySplit('.', out var hexNamePieces))
                    name = hexNamePieces[0];

                if (!int.TryParse(name, NumberStyles.HexNumber, null, out int tagIndex) || (TagCache.GetTag(tagIndex) == null))
                {
                    result = null;
                    return false;
                }

                result = TagCache.GetTag(tagIndex);
                return true;
            }

            if (!name.TrySplit('.', out var namePieces) || !TryParseGroupTag(namePieces[namePieces.Length - 1], out var groupTag))
                throw new Exception($"Invalid tag name: {name}");

            //var tagName = namePieces[0];

            var tagName = name.Substring(0, name.Length - (1 + namePieces[namePieces.Length - 1].Length));

            foreach (var instance in TagCache.TagTable)
            {
                if (instance is null)
                    continue;

                if (instance.IsInGroup(groupTag) && instance.Name == tagName)
                {
                    result = instance;
                    return true;
                }
            }

            result = null;
            return false;
        }

        public CachedTag GetTag(string name)
        {
            if (TryGetTag(name, out var result))
                return result;

            throw new KeyNotFoundException(name);
        }

        public Tag ParseGroupTag(string name)
        {
            if (!TryParseGroupTag(name, out Tag result))
                return Tag.Null;
            else
                return result;
        }

        public abstract void SaveStrings();
    }
}
