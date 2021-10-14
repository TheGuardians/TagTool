using System;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Cache.MCC;
using TagTool.Commands.Common;
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
            DetectCacheVersionAndPlatform(reader, MapVersion, ref version, ref platform);
            Version = version;
            CachePlatform = platform;

            Header = CacheFileHeader.Read(MapVersion, Version, CachePlatform, reader);

            if (!Header.IsValid())
            {
                new TagToolWarning($"Invalid map file header or footer detected. Verify definition");
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
                case CacheFileVersion.HaloMCCUniversal:
                    reader.SeekTo(0xA0);
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

        private static void DetectCacheVersionAndPlatform(EndianReader reader, CacheFileVersion mapVersion, ref CacheVersion cacheVersion, ref CachePlatform cachePlatform)
        {
            var version = GetMapFileVersion(reader);
            var buildDate = GetBuildDate(reader, version);

            if(mapVersion == CacheFileVersion.HaloMCCUniversal)
            {
                // force it to h3 for now as that is the only one supported
                cacheVersion = CacheVersion.Halo3Retail;
                cachePlatform = CachePlatform.MCC;
            }
            else
            {
                CacheVersionDetection.GetFromBuildName(buildDate, ref cacheVersion, ref cachePlatform);
            }
        }

        public static MapFile GenerateMapFile(CacheVersion version, Scenario scnr, CachedTag scenarioTag, Blf mapInfo = null)
        {
            if(CacheVersionDetection.IsBetween(version, CacheVersion.HaloOnlineED, CacheVersion.HaloOnline106708))
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

                var scenarioName = scenarioTag.Name.Split('\\').Last();
                var mapName = scenarioName;

                header.MapId = scnr.MapId;
                header.ScenarioTagIndex = scenarioTag.Index;
                header.Name = scenarioName;
                header.ScenarioPath = scenarioTag.Name;

                map.Header = header;

                map.MapFileBlf = new Blf(version, CachePlatform.Original);
                map.MapFileBlf.StartOfFile = new BlfChunkStartOfFile() { Signature = "_blf", Length = 0x30, MajorVersion = 1, MinorVersion = 2, ByteOrderMarker = -2, };
                map.MapFileBlf.Scenario = new BlfScenario() { Signature = "levl", Length = 0x98C0, MajorVersion = 3, MinorVersion = 1 };
                map.MapFileBlf.EndOfFile = new BlfChunkEndOfFile() { Signature = "_eof", Length = 0x11, MajorVersion = 1, MinorVersion = 2 };

                var scnrBlf = map.MapFileBlf.Scenario;
                scnrBlf.MapId = scnr.MapId;
                scnrBlf.Names = new NameUnicode32[12];
                for (int i = 0; i < scnrBlf.Names.Length; i++)
                    scnrBlf.Names[i] = new NameUnicode32() { Name = mapName };

                scnrBlf.Descriptions = new NameUnicode128[12];
                for (int i = 0; i < scnrBlf.Descriptions.Length; i++)
                    scnrBlf.Descriptions[i] = new NameUnicode128() { Name = "" };

                scnrBlf.Names[0] = new NameUnicode32() { Name = mapName };
                scnrBlf.Descriptions[0] = new NameUnicode128() { Name = "" };

                scnrBlf.MapName = scenarioName;
                scnrBlf.ImageName = $"m_{scenarioName}";
                scnrBlf.UnknownTeamCount1 = 2;
                scnrBlf.UnknownTeamCount2 = 6;
                scnrBlf.GameEngineTeamCounts = new byte[11] { 00, 02, 08, 08, 08, 08, 08, 08, 04, 02, 08 };

                scnrBlf.MapFlags = BlfScenarioFlags.GeneratesFilm | BlfScenarioFlags.IsMainmenu | BlfScenarioFlags.IsDlc;

                map.MapFileBlf.ContentFlags |= BlfFileContentFlags.StartOfFile | BlfFileContentFlags.Scenario | BlfFileContentFlags.EndOfFile;

                header.FileLength = 0x3390;

                if (mapInfo != null)
                {
                    if (mapInfo.ContentFlags.HasFlag(BlfFileContentFlags.StartOfFile) && mapInfo.ContentFlags.HasFlag(BlfFileContentFlags.EndOfFile) && mapInfo.ContentFlags.HasFlag(BlfFileContentFlags.Scenario))
                    {
                        mapInfo.Format = map.EndianFormat;
                        mapInfo.Version = map.Version;
                        mapInfo.CachePlatform = map.CachePlatform;
                        map.MapFileBlf = mapInfo;
                    }
                }
                return map;
            }

            return null;
        }
    }
}