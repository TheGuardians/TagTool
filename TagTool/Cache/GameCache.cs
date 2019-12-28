using System;
using System.Collections;
using System.Collections.Generic;
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

        public virtual IEnumerable<CachedTag> TagTable { get;}

        public abstract CachedTag GetTagByID(uint ID);
        public abstract CachedTag GetTagByIndex(int index);
        public abstract CachedTag GetTagByName(string name, Tag groupTag);

        public abstract Stream OpenTagCacheRead();
        public abstract FileStream OpenTagCacheReadWrite();
        public abstract FileStream OpenTagCacheWrite();
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
        public abstract BitmapTextureInteropResource GetBitmapTextureInteropResource(TagResourceReference resourceReference);
        public abstract BitmapTextureInterleavedInteropResource GetBitmapTextureInterleavedInteropResource(TagResourceReference resourceReference);
        public abstract RenderGeometryApiResourceDefinition GetRenderGeometryApiResourceDefinition(TagResourceReference resourceReference);
        public abstract ModelAnimationTagResource GetModelAnimationTagResource(TagResourceReference resourceReference);
        public abstract SoundResourceDefinition GetSoundResourceDefinition(TagResourceReference resourceReference);
        public abstract StructureBspTagResources GetStructureBspTagResources(TagResourceReference resourceReference);
        public abstract StructureBspCacheFileTagResources GetStructureBspCacheFileTagResources(TagResourceReference resourceReference);
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