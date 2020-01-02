using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Resources;

namespace TagTool.Cache
{
    public abstract class GameCache
    {
        public CacheVersion Version;
        public TagSerializer Serializer;
        public TagDeserializer Deserializer;
        public DirectoryInfo Directory;

        public List<LocaleTable> LocaleTables;
        public abstract StringTable StringTable { get; }
        public abstract TagCacheTest TagCache { get; }
        public abstract ResourceCacheTest ResourceCache { get; }

        public abstract Stream OpenCacheRead();
        public abstract FileStream OpenCacheReadWrite();
        public abstract FileStream OpenCacheWrite();

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
                    return new GameCacheContextGen3(map, file);

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

                    return new GameCacheContextHaloOnline(tagsFile.Directory);
            }

            return null;
        }

        // Utilities

        public bool TryParseTag(string name, out Tag result)
        {
            result = ParseTag(name);
            if (result == Tag.Null)
                return false;
            else return true;
        }

        public Tag ParseTag(string name)
        {
            if (name == "****" || name == "null")
                return Tag.Null;

            if (name.Length < 4)
            {
                if (name.Length == 3)
                    name = $"{name} ";
                else if (name.Length == 2)
                    name = $"{name}  ";
            }

            if (TagDefinition.TryFind(name, out var type))
            {
                var attribute = TagStructure.GetTagStructureAttribute(type);
                return new Tag(attribute.Tag);
            }

            foreach (var pair in TagCache.TagGroupInstances)
                if (name == StringTable.GetString(pair.Value.Name))
                    return pair.Value.Tag;

            return Tag.Null;
        }

        public bool TryGetCachedTag(int index, out CachedTag instance)
        {
            if (index < 0 || index >= TagCache.TagTable.Count())
            {
                instance = null;
                return false;
            }

            instance = TagCache.GetTagByIndex(index);
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
                if (!name.TrySplit('.', out var startNamePieces) || !TryParseTag(startNamePieces[1], out var starGroupTag))
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

                result = TagCache.GetTagByIndex(tagIndex);
                return true;
            }

            if (!name.TrySplit('.', out var namePieces) || !TryParseTag(namePieces[namePieces.Length - 1], out var groupTag))
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

    }

    public abstract class CachedTag
    {
        public string Name;
        public int Index;
        public uint ID;
        public TagGroup Group;

        public abstract uint DefinitionOffset { get; }

        public CachedTag()
        {
            Index = -1;
            Name = null;
            Group = TagGroup.None;
        }

        public CachedTag(int index, string name = null) : this(index, TagGroup.None, name) { }

        public CachedTag(int index, TagGroup group, string name = null)
        {
            Index = index;
            Group = group;
            if (name != null)
                Name = name;
        }

        public override string ToString()
        {
            if(Name == null)
                return $"0x{Index.ToString("X8")}.{Group.ToString()}";
            else
                return $"{Name}.{Group.ToString()}";
        }

        public bool IsInGroup(params Tag[] groupTags)
        {
            return Group.BelongsTo(groupTags);
        }
    }

    public abstract class TagCacheTest
    {
        public CacheVersion Version;
        public Dictionary<Tag, TagGroup> TagGroupInstances { get; set; }
        public virtual IEnumerable<CachedTag> TagTable { get;}

        public abstract CachedTag GetTagByID(uint ID);
        public abstract CachedTag GetTagByIndex(int index);
        public abstract CachedTag GetTagByName(string name, Tag groupTag);

        public abstract Stream OpenTagCacheRead();
        public abstract FileStream OpenTagCacheReadWrite();
        public abstract FileStream OpenTagCacheWrite();

        // Utilities

        public bool IsTagIndexValid(int tagIndex)
        {
            if (tagIndex > 0 && tagIndex < TagTable.Count())
                return true;
            else
                return false;
        }
    }

    public abstract class StringTable : List<string>
    {
        public CacheVersion Version;
        public StringIdResolver Resolver;

        public abstract StringId AddString(string newString);

        public abstract void Save();

        // override if required
        public virtual string GetString(StringId id)
        {
            var index = Resolver.StringIDToIndex(id);
            if (index > 0 && index < Count)
                return this[index];
            else
                return "invalid";
        }
        // override if required
        public virtual StringId GetStringId(string str)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i] == str)
                {
                    return Resolver.IndexToStringID(i, Version);
                }
            }
            return StringId.Invalid;
        }
    }

    public abstract class ResourceCacheTest
    {
        //public abstract byte[] GetResourceData(TagResourceReference resourceReference);
        public abstract BinkResource GetBinkResource(TagResourceReference resourceReference);
        public abstract BitmapTextureInteropResourceTest GetBitmapTextureInteropResource(TagResourceReference resourceReference);
        public abstract BitmapTextureInterleavedInteropResourceTest GetBitmapTextureInterleavedInteropResource(TagResourceReference resourceReference);
        public abstract RenderGeometryApiResourceDefinitionTest GetRenderGeometryApiResourceDefinition(TagResourceReference resourceReference);
        public abstract ModelAnimationTagResourceTest GetModelAnimationTagResource(TagResourceReference resourceReference);
        public abstract SoundResourceDefinition GetSoundResourceDefinition(TagResourceReference resourceReference);
        public abstract StructureBspTagResourcesTest GetStructureBspTagResources(TagResourceReference resourceReference);
        public abstract StructureBspCacheFileTagResourcesTest GetStructureBspCacheFileTagResources(TagResourceReference resourceReference);

        public abstract TagResourceReference CreateBinkResource(BinkResource binkResourceDefinition);
        public abstract TagResourceReference CreateRenderGeometryApiResource(RenderGeometryApiResourceDefinitionTest renderGeometryDefinition);
        public abstract TagResourceReference CreateModelAnimationGraphResource(ModelAnimationTagResourceTest modelAnimationGraphDefinition);
        public abstract TagResourceReference CreateSoundResource(SoundResourceDefinition soundResourceDefinition);
        public abstract TagResourceReference CreateBitmapResource(BitmapTextureInteropResourceTest bitmapResourceDefinition);
        public abstract TagResourceReference CreateBitmapInterleavedResource(BitmapTextureInterleavedInteropResourceTest bitmapResourceDefinition);
        public abstract TagResourceReference CreateStructureBspResource(StructureBspTagResourcesTest sbspResource);
        public abstract TagResourceReference CreateStructureBspCacheFileResource(StructureBspCacheFileTagResourcesTest sbspCacheFileResource);
    }

    public class CacheLocalizedStringTest
    {
        public int StringIndex;
        public string String;
        public int Index;

        public CacheLocalizedStringTest(int index, string locale, int localeIndex)
        {
            StringIndex = index;
            String = locale;
            Index = localeIndex;
        }
    }

    public class LocaleTable : List<CacheLocalizedStringTest> { }

    public class ResourceSize
    {
        public int PrimarySize;
        public int SecondarySize;
    }
}