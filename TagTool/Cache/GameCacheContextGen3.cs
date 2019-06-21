using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using static TagTool.Cache.CacheFile;

namespace TagTool.Cache
{
    public class GameCacheContextGen3 : IGameCacheContext
    {

        public int Magic;
        public MapFile BaseMapFile;
        public CacheVersion Version;

        public TagDeserializer Deserializer;

        public StringIdResolver Resolver;

        public CacheIndexHeader IndexHeader;
        public CacheIndexTable IndexItems;
        public CacheStringTable Strings;
        public List<CacheLocaleTable> LocaleTables;

        /// <summary>
        /// Dictionary of shared map file names with IMapFile interface
        /// </summary>
        public Dictionary<string, IGameCacheContext> SharedMapFiles { get; } = new Dictionary<string, IGameCacheContext>();

        public string LocalesKey { get; }
        
        public string StringsKey { get; }

        public string TagsKey { get; }

        public string NetworkKey { get; }

        public string StringMods { get; }

        public int LocaleGlobalsSize { get; }

        public int LocaleGlobalsOffset { get; }

        public GameCacheContextGen3(MapFile mapFile, EndianReader reader)
        {
            BaseMapFile = mapFile;
            Version = BaseMapFile.Version;
            Deserializer = new TagDeserializer(Version);
            LocalesKey = SetLocalesKey();
            StringsKey = SetStringsKey();
            TagsKey = SetTagsKey();
            NetworkKey = SetNetworkKey();
            StringMods = SetStringMods();
            LocaleGlobalsSize = SetLocaleGlobalsSize();
            LocaleGlobalsOffset = SetLocaleGlobalsOffset();


            var interop = mapFile.Header.GetInterop();

            if (interop.ResourceBaseAddress == 0)
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

            IndexHeader = mapFile.GetIndexHeader(reader, Magic);
            Strings = CreateStringTable(reader);
            IndexItems = CreateCacheIndexTable(reader);
            
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

        private CacheIndexTable CreateCacheIndexTable(EndianReader reader)
        {
            CacheIndexTable indexTable = new CacheIndexTable();
            indexTable.ClassList = new List<TagClass>();

            #region Read Class List
            reader.SeekTo(IndexHeader.TagGroupsOffset);
            for (int i = 0; i < IndexHeader.TagGroupCount; i++)
            {
                var tc = new TagClass()
                {
                    ClassCode = reader.ReadString(4),
                    Parent = reader.ReadString(4),
                    Parent2 = reader.ReadString(4),
                    StringID = reader.ReadInt32()
                };
                indexTable.ClassList.Add(tc);
            }
            #endregion

            #region Read Tags Info
            reader.SeekTo(IndexHeader.TagsOffset);
            for (int i = 0; i < IndexHeader.TagCount; i++)
            {
                var classIndex = reader.ReadInt16();
                var tagClass = classIndex == -1 ? null : indexTable.ClassList[classIndex];
                string groupName = classIndex == -1 ? "" : Strings.GetItemByID(tagClass.StringID);
                CacheIndexItem item = new CacheIndexItem(classIndex, (reader.ReadInt16() << 16) | i, reader.ReadInt32() - Magic, i, tagClass, groupName);
                indexTable.Add(item);
            }
            #endregion

            #region Read Indices
            reader.SeekTo(BaseMapFile.Header.GetTagNamesIndicesOffset());
            int[] indices = new int[IndexHeader.TagCount];
            for (int i = 0; i < IndexHeader.TagCount; i++)
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
                    indexTable[i].Name = "<null>";
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
                    indexTable[i].Name = "<blank>";
                    continue;
                }

                if (length < 0)
                {
                    int i0 = indices[i];
                    int i1 = indices[i + 1];
                    int i2 = indices[i + 2];
                    int i3 = indices[i + 3];
                }

                indexTable[i].Name = newReader.ReadString(length);
            }

            newReader.Close();
            newReader.Dispose();
            #endregion

            return indexTable;
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

            foreach (var item in IndexItems)
            {
                if (item.IsInGroup("matg"))
                {
                    matgOffset = item.Offset;
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
                    return 68;

                default:
                    throw new ArgumentException(nameof(Version), new NotSupportedException(Version.ToString()));
            }
        }

        //
        // Interface methods
        //

        public ISerializationContext CreateSerializationContext(object tag)
        {
            return null;// new CacheSerializationContext(this, (CacheIndexItem)tag);
        }
    }
}
