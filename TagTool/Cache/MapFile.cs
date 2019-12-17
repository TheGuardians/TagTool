using System;
using System.IO;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.Cache
{
    public enum MapFileVersion : int
    {
        Invalid = 0x0,
        HaloXbox = 0x5,
        HaloPC = 0x7,
        Halo2 = 0x8,
        Halo3Beta = 0x9,
        Halo3 = 0xB,
        HaloReach = 0xC,
        HaloOnline = 0x12,
    }

    public class MapFile : IMapFile
    {
        private static readonly Tag Head = new Tag("head");
        private static readonly Tag Foot = new Tag("foot");
        private static readonly int MapFileVersionOffset = 0x4;

        public CacheVersion Version { get; set; }
        public MapFileVersion MapVersion { get; set; }
        public EndianFormat EndianFormat { get; set; }

        public IMapFileHeader Header;

        public Blf MapFileBlf;

        public MapFile()
        {
        }

        public void Write(EndianWriter writer)
        {
            var dataContext = new DataSerializationContext(writer);
            var serializer = new TagSerializer(Version, EndianFormat);
            serializer.Serialize(dataContext, Header);

            if(Version == CacheVersion.HaloOnline106708)
            {
                if(MapFileBlf != null)
                    MapFileBlf.Write(writer);
            }
        }

        public void Read(EndianReader reader)
        {
            EndianFormat = DetectEndianFormat(reader);
            MapVersion = GetMapFileVersion(reader);
            Version = DetectCacheVersion(reader);
            var deserializer = new TagDeserializer(Version);
            reader.SeekTo(0);
            var dataContext = new DataSerializationContext(reader);
            Header = (MapFileHeader)deserializer.Deserialize(dataContext, typeof(MapFileHeader));

            // temporary code until map file format cleanup
            if (MapVersion == MapFileVersion.HaloOnline)
            {
                var mapFileHeaderSize = (int)TagStructure.GetTagStructureInfo(typeof(MapFileHeader), Version).TotalSize;

                // Seek to the blf
                reader.SeekTo(mapFileHeaderSize);
                // Read blf
                MapFileBlf = new Blf(Version);
                if (!MapFileBlf.Read(reader))
                    MapFileBlf = null;
            }
        }

        private static EndianFormat DetectEndianFormat(EndianReader reader)
        {
            reader.SeekTo(0);
            reader.Format = EndianFormat.LittleEndian;
            if (reader.ReadTag() == Head)
                return EndianFormat.LittleEndian;
            else
            {
                reader.SeekTo(0);
                reader.Format = EndianFormat.BigEndian;
                if (reader.ReadTag() == Head)
                    return EndianFormat.BigEndian;
                else
                    throw new Exception("Invalid map file header tag!");
            }
        }

        private static bool IsHalo2Vista(EndianReader reader)
        {
            reader.SeekTo(0x24);
            if (reader.ReadInt32() == -1)
                return true;
            else
                return false;
        }

        private static bool IsModifiedReachFormat(EndianReader reader)
        {
            reader.SeekTo(0x120);
            var version = CacheVersionDetection.GetFromBuildName(reader.ReadString(0x20));
            if (version == CacheVersion.Unknown)
                return false;
            else
                return true;
        }

        private static string GetBuildDate(EndianReader reader, MapFileVersion version)
        {
            var buildDataLength = 0x20;
            switch (version)
            {
                case MapFileVersion.HaloPC:
                case MapFileVersion.HaloXbox:
                    reader.SeekTo(0x40);
                    break;
                case MapFileVersion.Halo2:
                    if (IsHalo2Vista(reader))
                        reader.SeekTo(0x12C);
                    else
                        reader.SeekTo(0x120);
                    break;

                case MapFileVersion.Halo3Beta:
                case MapFileVersion.Halo3:
                case MapFileVersion.HaloOnline:
                    reader.SeekTo(0x11C);
                    break;

                case MapFileVersion.HaloReach:
                    if (IsModifiedReachFormat(reader))
                        reader.SeekTo(0x120);
                    else
                        reader.SeekTo(0x11C);
                    break; 

                default:
                    throw new Exception("Map file version not supported (build date)!");
            }
            return reader.ReadString(buildDataLength);
        }

        private static MapFileVersion GetMapFileVersion(EndianReader reader)
        {
            reader.SeekTo(MapFileVersionOffset);
            return (MapFileVersion)reader.ReadInt32();
        }

        private static CacheVersion DetectCacheVersion(EndianReader reader)
        {
            var version = GetMapFileVersion(reader);
            var buildDate = GetBuildDate(reader, version);
            return CacheVersionDetection.GetFromBuildName(buildDate);
        }

        //
        // Interface methods
        //

        public TagTableHeaderGen3 GetTagTableHeader(EndianReader reader, int magic)
        {
            switch (Version)
            {
                case CacheVersion.Halo3Retail:
                case CacheVersion.Halo3ODST:
                case CacheVersion.HaloReach:
                    reader.SeekTo(Header.GetTagIndexAddress());
                    return new TagTableHeaderGen3
                    {
                        TagGroupCount = reader.ReadInt32(),
                        TagGroupsOffset = reader.ReadInt32() - magic,
                        TagCount = reader.ReadInt32(),
                        TagsOffset = reader.ReadInt32() - magic,
                        TagInfoHeaderCount = reader.ReadInt32(),
                        TagInfoHeaderOffset = reader.ReadInt32() - magic,
                        TagInfoHeaderCount2 = reader.ReadInt32(),
                        TagInfoHeaderOffset2 = reader.ReadInt32() - magic
                    };

                default:
                    throw new NotImplementedException();
            }
        }

        public void UpdateScenarioIndices(int newIndex)
        {
            Header.SetScenarioTagIndex(newIndex);
        }
    }

    public interface IMapFile
    {
        TagTableHeaderGen3 GetTagTableHeader(EndianReader reader, int magic);
    }

    public interface IMapFileHeader
    {
        CacheFileInterop GetInterop();
        CacheFilePartition[] GetPartitions();
        int GetMemoryBufferSize();
        int GetHeaderSize(CacheVersion version);
        void ApplyMagic(int magic);

        uint GetTagIndexAddress();
        void SetTagIndexAddress(uint newAddress);
        
        int GetTagNamesIndicesOffset();
        int GetTagNamesBufferOffset();
        int GetTagNamesBufferSize();

        int GetStringIDsIndicesOffset();
        int GetStringIDsBufferOffset();
        int GetStringIDsBufferSize();
        int GetStringIDsCount();

        void SetScenarioTagIndex(int index);
        int GetScenarioTagIndex();
        string GetName();
        CacheFileType GetCacheType();
    }

}
