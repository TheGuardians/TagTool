using System;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Definitions;

namespace TagTool.BlamFile
{
    public class MapFile 
    {
        private static readonly Tag Head = new Tag("head");
        private static readonly Tag Foot = new Tag("foot");
        private static readonly int MapFileVersionOffset = 0x4;

        public CacheVersion Version { get; set; }
        public CacheFileVersion MapVersion { get; set; }
        public EndianFormat EndianFormat { get; set; }
        public CachePlatform CachePlatform { get; set; }

        public CacheFileHeader Header;

        public Blf MapFileBlf;

        public MapFile()
        {
        }

        public void Write(EndianWriter writer)
        {
            var dataContext = new DataSerializationContext(writer);
            var serializer = new TagSerializer(Version, CachePlatform, EndianFormat);
            serializer.Serialize(dataContext, Header);

            if(CacheVersionDetection.IsBetween(Version, CacheVersion.HaloOnlineED, CacheVersion.HaloOnline106708))
            {
                if(MapFileBlf != null)
                    MapFileBlf.Write(writer);
            }
        }

        public void Read(EndianReader reader)
        {
            EndianFormat = DetectEndianFormat(reader);
            MapVersion = GetMapFileVersion(reader);
            CacheVersion version = CacheVersion.Unknown;
            CachePlatform platform = CachePlatform.All;
            DetectCacheVersionAndPlatform(reader, ref version, ref platform);
            Version = version;
            CachePlatform = platform;

            Header = CacheFileHeader.Read(Version, CachePlatform, reader);

            if (!Header.IsValid())
            {
                Console.WriteLine("Warning: invalid map file header or footer detected. Verify definition");
            }

            // temporary code until map file format cleanup
            if (MapVersion == CacheFileVersion.HaloOnline)
            {
                var mapFileHeaderSize = (int)TagStructure.GetTagStructureInfo(Header.GetType(), Version, CachePlatform).TotalSize;

                // Seek to the blf
                reader.SeekTo(mapFileHeaderSize);
                // Read blf
                MapFileBlf = new Blf(Version, CachePlatform);
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
            var unknownValue = reader.ReadInt32();
            if (unknownValue == -1 || unknownValue == 0xB165400)
                return true;
            else
                return false;
        }

        private static bool IsGen3MCCFormat(EndianReader reader)
        {
            reader.SeekTo(0x120);
            CacheVersion version = CacheVersion.Unknown;
            CachePlatform platform = CachePlatform.All;
            CacheVersionDetection.GetFromBuildName(reader.ReadString(0x20), ref version, ref platform);
            if (platform == CachePlatform.MCC)
                return true;
            else
                return false;
        }

        private static bool IsModifiedReachFormat(EndianReader reader)
        {
            reader.SeekTo(0x120);
            CacheVersion version = CacheVersion.Unknown;
            CachePlatform platform = CachePlatform.All;
            CacheVersionDetection.GetFromBuildName(reader.ReadString(0x20), ref version, ref platform);
            if (version == CacheVersion.Unknown)
                return false;
            else
                return true;
        }

        private static string GetBuildDate(EndianReader reader, CacheFileVersion version)
        {
            var buildDataLength = 0x20;
            switch (version)
            {
                case CacheFileVersion.HaloPC:
                case CacheFileVersion.HaloCustomEdition:
                case CacheFileVersion.HaloXbox:
                    reader.SeekTo(0x40);
                    break;
                case CacheFileVersion.Halo2:
                    if (IsHalo2Vista(reader))
                        reader.SeekTo(0x12C);
                    else
                        reader.SeekTo(0x120);
                    break;

                case CacheFileVersion.Halo3Beta:
                case CacheFileVersion.Halo3:
                case CacheFileVersion.HaloOnline:
                    if (IsGen3MCCFormat(reader))
                        reader.SeekTo(0x120);
                    else
                        reader.SeekTo(0x11C);
                    break;

                case CacheFileVersion.HaloReach:
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

        private static CacheFileVersion GetMapFileVersion(EndianReader reader)
        {
            reader.SeekTo(MapFileVersionOffset);
            return (CacheFileVersion)reader.ReadInt32();
        }

        private static void DetectCacheVersionAndPlatform(EndianReader reader, ref CacheVersion cacheVersion, ref CachePlatform cachePlatform)
        {
            var version = GetMapFileVersion(reader);
            var buildDate = GetBuildDate(reader, version);
            CacheVersionDetection.GetFromBuildName(buildDate, ref cacheVersion, ref cachePlatform);
        }

        public static MapFile GenerateMapFile(CacheVersion version, Scenario scnr, CachedTag scenarioTag, Blf mapInfo = null)
        {
            if(version == CacheVersion.HaloOnlineED)
            {
                MapFile map = new MapFile();
                var header = new CacheFileHeaderGenHaloOnline();


                map.Version = version;
                map.EndianFormat = EndianFormat.LittleEndian;
                map.MapVersion = CacheFileVersion.HaloOnline;
                map.CachePlatform = CachePlatform.Original;

                header.HeaderSignature = new Tag("head");
                header.FooterSignature = new Tag("foot");
                header.FileVersion = map.MapVersion;
                header.Build = CacheVersionDetection.GetBuildName(version, map.CachePlatform);

                switch (scnr.MapType)
                {
                    case ScenarioMapType.MainMenu:
                        header.CacheType = CacheFileType.MainMenu;
                        break;
                    case ScenarioMapType.SinglePlayer:
                        header.CacheType = CacheFileType.Campaign;
                        break;
                    case ScenarioMapType.Multiplayer:
                        header.CacheType = CacheFileType.Multiplayer;
                        break;
                }
                header.SharedCacheType = CacheFileSharedType.None;

                header.MapId = scnr.MapId;
                header.ScenarioTagIndex = scenarioTag.Index;
                header.Name = scenarioTag.Name.Split('\\').Last();
                header.ScenarioPath = scenarioTag.Name;

                map.Header = header;

                header.FileLength = 0x3390;

                if (mapInfo != null)
                {
                    if (mapInfo.ContentFlags.HasFlag(BlfFileContentFlags.StartOfFile) && mapInfo.ContentFlags.HasFlag(BlfFileContentFlags.EndOfFile) && mapInfo.ContentFlags.HasFlag(BlfFileContentFlags.Scenario))
                    {
                        map.MapFileBlf = mapInfo;
                    }
                }
                return map;
            }

            return null;
        }
    }
}