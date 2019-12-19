using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.Cache
{
    public class GameCacheContextGen3 : GameCache
    {
        public int Magic;
        public MapFile BaseMapFile;
        public FileInfo CacheFile;
        public string NetworkKey;

        public StringTableGen3 StringTableGen3;
        public TagCacheGen3 TagCacheGen3;

        public override TagCacheTest TagCache => TagCacheGen3;
        public override StringTable StringTable => StringTableGen3;

        public GameCacheContextGen3(MapFile mapFile, FileInfo file)
        {
            BaseMapFile = mapFile;
            Version = BaseMapFile.Version;
            CacheFile = file;
            Deserializer = new TagDeserializer(Version);
            Serializer = new TagSerializer(Version);

            var interop = mapFile.Header.GetInterop();

            if ( interop != null && interop.ResourceBaseAddress == 0)
                Magic = (int)(interop.RuntimeBaseAddress - mapFile.Header.GetMemoryBufferSize());
            else
            {
                mapFile.Header.ApplyMagic(mapFile.Header.GetStringIDsIndicesOffset() - mapFile.Header.GetHeaderSize(mapFile.Version));
                var resourcePartition = mapFile.Header.GetPartitions()[(int)CacheFilePartitionType.Resources];
                var resourceSection = interop.Sections[(int)CacheFileSectionType.Resource];
                Magic = BitConverter.ToInt32(BitConverter.GetBytes(resourcePartition.BaseAddress), 0) - (interop.DebugSectionSize + resourceSection.Size);
            }

            if (mapFile.Header.GetTagIndexAddress() == 0)
                return;

            mapFile.Header.SetTagIndexAddress(BitConverter.ToUInt32(BitConverter.GetBytes(mapFile.Header.GetTagIndexAddress() - Magic), 0));

            using(var reader = new EndianReader(OpenCacheRead(), BaseMapFile.EndianFormat))
            {
                StringTableGen3 = new StringTableGen3(reader, BaseMapFile);
                TagCacheGen3 = new TagCacheGen3(reader, BaseMapFile, StringTableGen3, Magic);
                LocaleTables = LocalesTableGen3.CreateLocalesTable(reader, BaseMapFile, TagCacheGen3);
            }

            // unused but kept for future uses
            switch (Version)
            {
                case CacheVersion.Halo3Retail:
                case CacheVersion.Halo3ODST:
                    NetworkKey = "";
                    break;
                case CacheVersion.HaloReach:
                    NetworkKey = "SneakerNetReigns";
                    break;
                
            }
        }

        //
        // Overrides from abstract class
        //

        public override Stream OpenCacheRead() => CacheFile.OpenRead();

        public override Stream OpenTagCacheRead() => OpenCacheRead();


        public override T Deserialize<T>(Stream stream, CachedTag instance) =>
            Deserialize<T>(new Gen3SerializationContext(stream, this, (CachedTagGen3)instance));

        public override object Deserialize(Stream stream, CachedTag instance) =>
            Deserialize(new Gen3SerializationContext(stream, this, (CachedTagGen3)instance), TagDefinition.Find(instance.Group.Tag));

        //
        // private methods for internal use
        //

        private T Deserialize<T>(ISerializationContext context) =>
            Deserializer.Deserialize<T>(context);

        private object Deserialize(ISerializationContext context, Type type) =>
            Deserializer.Deserialize(context, type);
        
        //
        // public methods specific to gen3
        //

        public T Deserialize<T>(Stream stream, CachedTagGen3 instance) =>
            Deserialize<T>(new Gen3SerializationContext(stream, this, instance));

        public object Deserialize(Stream stream, CachedTagGen3 instance) =>
            Deserialize(new Gen3SerializationContext(stream, this, instance), TagDefinition.Find(instance.Group.Tag));

    }

    public class CachedTagGen3 : CachedTag
    {
        public uint Offset;
        public int GroupIndex;
        public int Size;

        public override uint DefinitionOffset => Offset;

        public CachedTagGen3() : base() { }

        public CachedTagGen3(int groupIndex, uint id, uint offset, int index, TagGroup tagGroup, string groupName)
        {
            GroupIndex = groupIndex;
            ID = id;
            Offset = offset;
            Index = index;
            Group = tagGroup;
        }
    }

    public class TagTableHeaderGen3
    {
        public int TagGroupsOffset;
        public int TagGroupCount;
        public int TagsOffset;
        public DatumIndex ScenarioHandle;
        public DatumIndex GlobalsHandle;
        public int CRC;
        public int TagCount;
        public int TagInfoHeaderCount;
        public int TagInfoHeaderOffset;
        public int TagInfoHeaderCount2;
        public int TagInfoHeaderOffset2;
    }

    public class TagCacheGen3 : TagCacheTest
    {
        public List<CachedTagGen3> Tags;
        public TagTableHeaderGen3 TagTableHeader;
        public List<TagGroup> TagGroups;
        public string TagsKey = "";

        public override IEnumerable<CachedTag> TagTable { get => Tags; }

        public override CachedTag GetTagByID(int ID) => GetTagByIndex(0xFFFF & ID);

        public override CachedTag GetTagByIndex(int index)
        {
            if (index > 0 && index < Tags.Count)
                return Tags[index];
            else
                return null;
        }

        public override CachedTag GetTagByName(string name, Tag groupTag)
        {
            foreach (var tag in Tags)
            {
                if (groupTag == tag.Group.Tag && name == tag.Name)
                    return tag;
            }
            return null;
        }

        public TagCacheGen3(EndianReader reader, MapFile baseMapFile, StringTableGen3 stringTable, int Magic)
        {
            Tags = new List<CachedTagGen3>();
            TagGroups = new List<TagGroup>();
            TagTableHeader = baseMapFile.GetTagTableHeader(reader, Magic);
            Version = baseMapFile.Version;

            switch (Version)
            {
                case CacheVersion.Halo3Retail:
                case CacheVersion.Halo3ODST:
                    TagsKey = "";
                    break;
                case CacheVersion.HaloReach:
                    TagsKey =  "LetsAllPlayNice!";
                    break;
            }

            #region Read Class List
            reader.SeekTo(TagTableHeader.TagGroupsOffset);
            for (int i = 0; i < TagTableHeader.TagGroupCount; i++)
            {
                var group = new TagGroup()
                {
                    Tag = new Tag(reader.ReadString(4)),
                    ParentTag = new Tag(reader.ReadString(4)),
                    GrandparentTag = new Tag(reader.ReadString(4)),
                    Name = new StringId(reader.ReadUInt32())
                };
                TagGroups.Add(group);
            }
            #endregion

            #region Read Tags Info
            reader.SeekTo(TagTableHeader.TagsOffset);
            for (int i = 0; i < TagTableHeader.TagCount; i++)
            {
                var groupIndex = reader.ReadInt16();
                var tagGroup = groupIndex == -1 ? new TagGroup() : TagGroups[groupIndex];
                string groupName = groupIndex == -1 ? "" : stringTable.GetString(tagGroup.Name);
                CachedTagGen3 tag = new CachedTagGen3(groupIndex, (uint)((reader.ReadInt16() << 16) | i), (uint)(reader.ReadUInt32() - Magic), i, tagGroup, groupName);
                Tags.Add(tag);
            }
            #endregion

            #region Read Indices
            reader.SeekTo(baseMapFile.Header.GetTagNamesIndicesOffset());
            int[] indices = new int[TagTableHeader.TagCount];
            for (int i = 0; i < TagTableHeader.TagCount; i++)
                indices[i] = reader.ReadInt32();
            #endregion

            #region Read Names
            reader.SeekTo(baseMapFile.Header.GetTagNamesBufferOffset());

            EndianReader newReader = null;

            if (TagsKey == "" || TagsKey == null)
            {
                newReader = new EndianReader(new MemoryStream(reader.ReadBytes(baseMapFile.Header.GetTagNamesBufferSize())), EndianFormat.BigEndian);
            }
            else
            {
                reader.BaseStream.Position = baseMapFile.Header.GetTagNamesBufferOffset();
                newReader = new EndianReader(reader.DecryptAesSegment(baseMapFile.Header.GetTagNamesBufferSize(), TagsKey), EndianFormat.BigEndian);
            }

            for (int i = 0; i < indices.Length; i++)
            {
                if (indices[i] == -1)
                {
                    Tags[i].Name = null;
                    continue;
                }

                newReader.SeekTo(indices[i]);

                int length;
                if (i == indices.Length - 1)
                    length = baseMapFile.Header.GetTagNamesBufferSize() - indices[i];
                else
                {
                    if (indices[i + 1] == -1)
                    {
                        int index = -1;

                        for (int j = i + 1; j < indices.Length; j++)
                        {
                            if (indices[j] != -1)
                            {
                                index = j;
                                break;
                            }
                        }

                        length = (index == -1) ? baseMapFile.Header.GetTagNamesBufferSize() - indices[i] : indices[index] - indices[i];
                    }
                    else
                        length = indices[i + 1] - indices[i];
                }

                if (length == 1)
                {
                    Tags[i].Name = "<blank>";
                    continue;
                }

                Tags[i].Name = newReader.ReadString(length);
            }

            newReader.Close();
            newReader.Dispose();
            #endregion

        }
    }

    public class StringTableGen3 : StringTable
    {
        public string StringKey = "";

        public StringTableGen3(EndianReader reader, MapFile baseMapFile) : base()
        {
            Version = baseMapFile.Version;
            
            switch (Version)
            {
                case CacheVersion.Halo3Retail:
                    Resolver = new StringIdResolverHalo3();
                    break;

                case CacheVersion.Halo3ODST:
                    Resolver = new StringIdResolverHalo3ODST();
                    break;

                case CacheVersion.HaloReach:
                    Resolver = new StringIdResolverHaloReach();
                    StringKey = "ILikeSafeStrings";
                    break;

                default:
                    throw new NotSupportedException(CacheVersionDetection.GetBuildName(Version));
            }

            reader.SeekTo(baseMapFile.Header.GetStringIDsIndicesOffset());
            int[] indices = new int[baseMapFile.Header.GetStringIDsCount()];
            for (var i = 0; i < baseMapFile.Header.GetStringIDsCount(); i++)
            {
                indices[i] = reader.ReadInt32();
                Add("");
            }

            reader.SeekTo(baseMapFile.Header.GetStringIDsBufferOffset());

            EndianReader newReader;

            if (StringKey == "")
            {
                newReader = new EndianReader(new MemoryStream(reader.ReadBytes(baseMapFile.Header.GetStringIDsBufferSize())), reader.Format);
            }
            else
            {
                reader.BaseStream.Position = baseMapFile.Header.GetStringIDsBufferOffset();
                newReader = new EndianReader(reader.DecryptAesSegment(baseMapFile.Header.GetStringIDsBufferSize(), StringKey), reader.Format);
            }

            for (var i = 0; i < indices.Length; i++)
            {
                if (indices[i] == -1)
                {
                    this[i] = "<null>";
                    continue;
                }

                newReader.SeekTo(indices[i]);

                int length;
                if (i == indices.Length - 1)
                    length = baseMapFile.Header.GetStringIDsBufferSize() - indices[i];
                else
                    length = (indices[i + 1] != -1)
                        ? indices[i + 1] - indices[i]
                        : indices[i + 2] - indices[i];

                if (length == 1)
                {
                    this[i] = "";
                    continue;
                }

                this[i] = newReader.ReadString(length);
            }
            newReader.Close();
            newReader.Dispose();
        }

        public override StringId AddString(string newString)
        {
            throw new NotImplementedException();
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }
    }

    public class LocalesTableGen3
    {
        public static List<LocaleTable> CreateLocalesTable(EndianReader reader, MapFile baseMapFile, TagCacheGen3 tagCache)
        {
            CachedTagGen3 matg = null;
            foreach (var tag in tagCache.Tags)
                if (tag.IsInGroup("matg"))
                {
                    matg = tag;
                    break;
                }
                    
            List < LocaleTable >  localesTable = new List<LocaleTable>();
            string localesKey = "";
            uint localeGlobalsOffset = 0;
            uint localeGlobalsSize = 0;
            uint matgOffset = matg.DefinitionOffset;
            var interop = baseMapFile.Header.GetInterop();

            switch (baseMapFile.Version)
            {
                case CacheVersion.Halo3Retail:
                    localesKey = "";
                    localeGlobalsOffset = 452;
                    localeGlobalsSize = 68;
                    break;
                case CacheVersion.Halo3ODST:
                    localesKey = "";
                    localeGlobalsOffset = 508;
                    localeGlobalsSize = 68;
                    break;
                case CacheVersion.HaloReach:
                    localesKey = "BungieHaloReach!";
                    localeGlobalsOffset = 656;
                    localeGlobalsSize = 68;
                    break;
            }

            foreach (var language in Enum.GetValues(typeof(GameLanguage)))
            {
                LocaleTable table = new LocaleTable();

                reader.SeekTo(matgOffset + localeGlobalsOffset + ((int)language * localeGlobalsSize));

                var localeCount = reader.ReadInt32();
                var tableSize = reader.ReadInt32();
                var indexOffset = (int)(reader.ReadInt32() + interop.UnknownBaseAddress);
                var tableOffset = (int)(reader.ReadInt32() + interop.UnknownBaseAddress);

                reader.SeekTo(indexOffset);
                var indices = new int[localeCount];

                for (var i = 0; i < localeCount; i++)
                {
                    table.Add(new CacheLocalizedStringTest(reader.ReadInt32(), "", i));
                    indices[i] = reader.ReadInt32();
                }

                reader.SeekTo(tableOffset);

                EndianReader newReader = null;

                if (localesKey == "")
                {
                    newReader = new EndianReader(new MemoryStream(reader.ReadBytes(tableSize)), EndianFormat.BigEndian);
                }
                else
                {
                    reader.BaseStream.Position = tableOffset;
                    newReader = new EndianReader(reader.DecryptAesSegment(tableSize, localesKey));
                }

                for (var i = 0; i < indices.Length; i++)
                {
                    if (indices[i] == -1)
                    {
                        table[i].String = "<null>";
                        continue;
                    }

                    newReader.SeekTo(indices[i]);

                    int length;
                    if (i == indices.Length - 1)
                        length = tableSize - indices[i];
                    else
                        length = (indices[i + 1] != -1)
                            ? indices[i + 1] - indices[i]
                            : indices[i + 2] - indices[i];

                    if (length == 1)
                    {

                        table[i].String = "<blank>";
                        continue;
                    }
                    table[i].String = newReader.ReadString(length);
                }

                newReader.Close();
                newReader.Dispose();

                localesTable.Add(table);
            }

            return localesTable;
        }
    }
}
