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
        
        public CacheStringTable Strings;
        public List<CacheLocaleTable> LocaleTables;

        public TagCacheGen3 TagCacheGen3;
        public override TagCacheTest TagCache => TagCacheGen3;

        public Dictionary<string, GameCacheContextGen3> SharedMapFiles { get; } = new Dictionary<string, GameCacheContextGen3>();
        public string LocalesKey { get; }
        public string StringsKey { get; }
        public string TagsKey { get; }
        public string NetworkKey { get; }
        public string StringMods { get; }
        public int LocaleGlobalsSize { get; }
        public int LocaleGlobalsOffset { get; }

        public GameCacheContextGen3(MapFile mapFile, FileInfo file)
        {
            BaseMapFile = mapFile;
            Version = BaseMapFile.Version;
            CacheFile = file;
            Deserializer = new TagDeserializer(Version);
            Serializer = new TagSerializer(Version);
            LocalesKey = SetLocalesKey();
            StringsKey = SetStringsKey();
            TagsKey = SetTagsKey();
            NetworkKey = SetNetworkKey();
            StringMods = SetStringMods();
            LocaleGlobalsSize = SetLocaleGlobalsSize();
            LocaleGlobalsOffset = SetLocaleGlobalsOffset();

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
                Strings = CreateStringTable(reader);
                TagCacheGen3 = new TagCacheGen3(reader, BaseMapFile, Strings, Magic, TagsKey);
                
                LocaleTables = new List<CacheLocaleTable>();

                switch (mapFile.Version)
                {
                    case CacheVersion.Halo3Retail:
                        Resolver = new StringIdResolverHalo3();
                        break;

                    case CacheVersion.Halo3ODST:
                        Resolver = new StringIdResolverHalo3ODST();
                        break;

                    case CacheVersion.HaloReach:
                        Resolver = new StringIdResolverHaloReach();
                        break;

                    default:
                        throw new NotSupportedException(CacheVersionDetection.GetBuildName(mapFile.Version));
                }

                foreach (var language in Enum.GetValues(typeof(GameLanguage)))
                    LocaleTables.Add(CreateLocaleTable(reader, (GameLanguage)language));
            }
        }

        private CacheStringTable CreateStringTable(EndianReader reader)
        {
            CacheStringTable table = new CacheStringTable();
            table.StringMods = StringMods;
            reader.SeekTo(BaseMapFile.Header.GetStringIDsIndicesOffset());
            int[] indices = new int[BaseMapFile.Header.GetStringIDsCount()];
            for (var i = 0; i < BaseMapFile.Header.GetStringIDsCount(); i++)
            {
                indices[i] = reader.ReadInt32();
                table.Add("");
            }

            reader.SeekTo(BaseMapFile.Header.GetStringIDsBufferOffset());

            EndianReader newReader = null;

            if (StringsKey == "" || StringsKey == null)
            {
                newReader = new EndianReader(new MemoryStream(reader.ReadBytes(BaseMapFile.Header.GetStringIDsBufferSize())), reader.Format);
            }
            else
            {
                reader.BaseStream.Position = BaseMapFile.Header.GetStringIDsBufferOffset();
                newReader = new EndianReader(reader.DecryptAesSegment(BaseMapFile.Header.GetStringIDsBufferSize(), StringsKey), reader.Format);
            }

            for (var i = 0; i < indices.Length; i++)
            {
                if (indices[i] == -1)
                {
                    table[i] = "<null>";
                    continue;
                }

                newReader.SeekTo(indices[i]);

                int length;
                if (i == indices.Length - 1)
                    length = BaseMapFile.Header.GetStringIDsBufferSize() - indices[i];
                else
                    length = (indices[i + 1] != -1)
                        ? indices[i + 1] - indices[i]
                        : indices[i + 2] - indices[i];

                if (length == 1)
                {
                    table[i] = "";
                    continue;
                }

                table[i] = newReader.ReadString(length);
            }
            newReader.Close();
            newReader.Dispose();
            return table;
        }

        private CacheLocaleTable CreateLocaleTable(EndianReader reader, GameLanguage language)
        {
            CacheLocaleTable table = new CacheLocaleTable();

            int matgOffset = -1;
            var interop = BaseMapFile.Header.GetInterop();

            foreach (var item in TagCacheGen3.Tags)
            {
                if (item.IsInGroup("matg"))
                {
                    matgOffset = (int)item.DefinitionOffset;
                    break;
                }
            }

            if (matgOffset == -1)
                return null;

            reader.SeekTo(matgOffset + LocaleGlobalsOffset + ((int)language * LocaleGlobalsSize));

            var localeCount = reader.ReadInt32();
            var tableSize = reader.ReadInt32();
            var indexOffset = (int)(reader.ReadInt32() + interop.UnknownBaseAddress);
            var tableOffset = (int)(reader.ReadInt32() + interop.UnknownBaseAddress);

            reader.SeekTo(indexOffset);
            var indices = new int[localeCount];

            for (var i = 0; i < localeCount; i++)
            {
                table.Add(new CacheLocalizedString(reader.ReadInt32(), "", i));
                indices[i] = reader.ReadInt32();
            }

            reader.SeekTo(tableOffset);

            EndianReader newReader = null;

            if (LocalesKey == null || LocalesKey == "")
            {
                newReader = new EndianReader(new MemoryStream(reader.ReadBytes(tableSize)), EndianFormat.BigEndian);
            }
            else
            {
                reader.BaseStream.Position = tableOffset;
                newReader = new EndianReader(reader.DecryptAesSegment(tableSize, LocalesKey));
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

            return table;
        }

        private string SetLocalesKey()
        {
            switch (Version)
            {
                case CacheVersion.Halo3Retail:
                case CacheVersion.Halo3ODST:
                    return "";

                case CacheVersion.HaloReach:
                    return "BungieHaloReach!";

                case CacheVersion.HaloReachMCC824:
                    return "";

                default:
                    throw new ArgumentException(nameof(Version), new NotSupportedException(Version.ToString()));
            }
        }

        private string SetStringsKey()
        {
            switch (Version)
            {
                case CacheVersion.Halo3Retail:
                case CacheVersion.Halo3ODST:
                    return "";

                case CacheVersion.HaloReach:
                    return "ILikeSafeStrings";

                case CacheVersion.HaloReachMCC824:
                    return "";

                default:
                    throw new ArgumentException(nameof(Version), new NotSupportedException(Version.ToString()));
            }
        }

        private string SetTagsKey()
        {
            switch (Version)
            {
                case CacheVersion.Halo3Retail:
                case CacheVersion.Halo3ODST:
                    return "";

                case CacheVersion.HaloReach:
                    return "LetsAllPlayNice!";

                case CacheVersion.HaloReachMCC824:
                    return "";

                default:
                    throw new ArgumentException(nameof(Version), new NotSupportedException(Version.ToString()));
            }
        }

        private string SetNetworkKey()
        {
            switch (Version)
            {
                case CacheVersion.Halo3Retail:
                case CacheVersion.Halo3ODST:
                    return "";

                case CacheVersion.HaloReach:
                    return "SneakerNetReigns";

                case CacheVersion.HaloReachMCC824:
                    return "";

                default:
                    throw new ArgumentException(nameof(Version), new NotSupportedException(Version.ToString()));
            }
        }

        private string SetStringMods()
        {
            switch (Version)
            {
                case CacheVersion.Halo3Retail:
                    return "+262143,-259153;+64329,-64329;+1208,+1882";

                case CacheVersion.Halo3ODST:
                    return "+258846,-258846;+64231,-64231;+1304,+2098";

                case CacheVersion.HaloReach:
                case CacheVersion.HaloReachMCC824:   // verify
                    return "+1174139,-1174139;+129874,-129874;+1123,+4604";


                default:
                    throw new ArgumentException(nameof(Version), new NotSupportedException(Version.ToString()));
            }
        }

        private int SetLocaleGlobalsOffset()
        {
            switch (Version)
            {
                case CacheVersion.Halo3Retail:
                    return 452;

                case CacheVersion.Halo3ODST:
                    return 508;

                case CacheVersion.HaloReach:
                case CacheVersion.HaloReachMCC824:   // verify
                    return 656;

                default:
                    throw new ArgumentException(nameof(Version), new NotSupportedException(Version.ToString()));
            }
        }

        private int SetLocaleGlobalsSize()
        {
            switch (Version)
            {
                case CacheVersion.Halo3Retail:
                case CacheVersion.Halo3ODST:
                case CacheVersion.HaloReach:
                case CacheVersion.HaloReachMCC824:   // verify
                    return 68;

                default:
                    throw new ArgumentException(nameof(Version), new NotSupportedException(Version.ToString()));
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

        public TagCacheGen3(EndianReader reader, MapFile BaseMapFile, CacheStringTable Strings, int Magic, string TagsKey)
        {
            Tags = new List<CachedTagGen3>();
            TagGroups = new List<TagGroup>();
            TagTableHeader = BaseMapFile.GetTagTableHeader(reader, Magic);

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
                string groupName = groupIndex == -1 ? "" : Strings.GetItemByID((int)tagGroup.Name.Value);
                CachedTagGen3 tag = new CachedTagGen3(groupIndex, (uint)((reader.ReadInt16() << 16) | i), (uint)(reader.ReadUInt32() - Magic), i, tagGroup, groupName);
                Tags.Add(tag);
            }
            #endregion

            #region Read Indices
            reader.SeekTo(BaseMapFile.Header.GetTagNamesIndicesOffset());
            int[] indices = new int[TagTableHeader.TagCount];
            for (int i = 0; i < TagTableHeader.TagCount; i++)
                indices[i] = reader.ReadInt32();
            #endregion

            #region Read Names
            reader.SeekTo(BaseMapFile.Header.GetTagNamesBufferOffset());

            EndianReader newReader = null;

            if (TagsKey == "" || TagsKey == null)
            {
                newReader = new EndianReader(new MemoryStream(reader.ReadBytes(BaseMapFile.Header.GetTagNamesBufferSize())), EndianFormat.BigEndian);
            }
            else
            {
                reader.BaseStream.Position = BaseMapFile.Header.GetTagNamesBufferOffset();
                newReader = new EndianReader(reader.DecryptAesSegment(BaseMapFile.Header.GetTagNamesBufferSize(), TagsKey), EndianFormat.BigEndian);
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
                    length = BaseMapFile.Header.GetTagNamesBufferSize() - indices[i];
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

                        length = (index == -1) ? BaseMapFile.Header.GetTagNamesBufferSize() - indices[i] : indices[index] - indices[i];
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

}
