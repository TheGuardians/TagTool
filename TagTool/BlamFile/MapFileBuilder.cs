using System;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags;
using TagTool.Tags.Definitions;

namespace TagTool.BlamFile
{
    public class MapFileBuilder
    {
        private CacheVersion Version;
        private CachePlatform CachePlatform;

        /// <summary>
        /// Optional map Display Name (overrides MapInfo)
        /// </summary>
        public string MapName { get; set; }

        /// <summary>
        /// Optional map description (overrides MapInfo)
        /// </summary>
        public string MapDescription { get; set; }

        /// <summary>
        /// Optional map info (if not provided, one will be generated)
        /// </summary>
        public BlfScenario MapInfo { get; set; }

        /// <summary>
        /// Optional map variant
        /// </summary>
        public Blf MapVariant { get; set; }

        public MapFileBuilder(CacheVersion version)
        {
            Version = version;
            CachePlatform = CachePlatform.Original;

            if (!CacheVersionDetection.IsBetween(Version, CacheVersion.HaloOnlineED, CacheVersion.HaloOnline106708))
                throw new ArgumentOutOfRangeException(nameof(Version), "Cache File version not supported");
        }

        public MapFile Build(CachedTag scnrTag, Scenario scnr)
        {
            VerifyInputs(scnr);

            var scenarioName = scnrTag.Name.Split('\\').Last();

            var map = new MapFile();
            map.Version = Version;
            map.CachePlatform = CachePlatform;
            map.EndianFormat = EndianFormat.LittleEndian;
            map.MapVersion = CacheFileVersion.HaloOnline;
            map.Header = GenerateCacheFileHeader(scnrTag, scnr, scenarioName);
            map.MapFileBlf = GenerateMapBlf(scnr, scenarioName);
            return map;
        }

        private void VerifyInputs(Scenario scnr)
        {
            if (MapInfo != null)
            {
                if (MapInfo.MapId != scnr.MapId)
                    throw new InvalidOperationException($"Map info map id did not match the scenario. {MapInfo.MapId} != {scnr.MapId}");

                if (!ValidMapInfoFlags())
                    throw new InvalidOperationException($"Map info map flags did not match the scenario. map flags: {MapInfo.MapFlags}, scenario type: {scnr.MapType}");
            }

            if (MapVariant != null)
            {
                var mapVariant = MapVariant.MapVariant.MapVariant;

                if (mapVariant.MapId != scnr.MapId)
                    throw new InvalidOperationException($"Map variant map id did not match the scenario. {mapVariant.MapId} != {scnr.MapId}");
                if (mapVariant.Metadata.MapId != scnr.MapId)
                    throw new InvalidOperationException($"Map variant metadata map id did not match the scenario. {mapVariant.MapId} != {scnr.MapId}");
            }

            bool ValidMapInfoFlags()
            {
                switch (scnr.MapType)
                {
                    case ScenarioMapType.SinglePlayer:
                        return MapInfo.MapFlags.HasFlag(BlfScenarioFlags.IsCampaign);
                    case ScenarioMapType.Multiplayer:
                        return MapInfo.MapFlags.HasFlag(BlfScenarioFlags.IsMultiplayer);
                    case ScenarioMapType.MainMenu:
                        return MapInfo.MapFlags.HasFlag(BlfScenarioFlags.IsMainmenu);
                    default:
                        return false;
                }
            }
        }

        private CacheFileHeaderGenHaloOnline GenerateCacheFileHeader(CachedTag scnrTag, Scenario scnr, string scenarioName)
        {
            var header = new CacheFileHeaderGenHaloOnline();
            header.HeaderSignature = new Tag("head");
            header.FooterSignature = new Tag("foot");
            header.FileVersion = CacheFileVersion.HaloOnline;
            header.Build = CacheVersionDetection.GetBuildName(Version, CachePlatform);

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

            header.FileLength = (int)TagStructure.GetStructureSize(typeof(CacheFileHeaderGenHaloOnline), Version, CachePlatform);
            header.SharedCacheType = CacheFileSharedType.None;
            header.MapId = scnr.MapId;
            header.Name = scenarioName;
            header.ScenarioPath = scnrTag.Name;
            header.ScenarioTagIndex = scnrTag.Index;
            return header;
        }

        private Blf GenerateMapBlf(Scenario scnr, string scenarioName)
        {
            var blf = new Blf(Version, CachePlatform);

            blf.StartOfFile = new BlfChunkStartOfFile()
            {
                Signature = "_blf",
                Length = (int)TagStructure.GetStructureSize(typeof(BlfChunkStartOfFile), Version, CachePlatform),
                MajorVersion = 1,
                MinorVersion = 2,
                ByteOrderMarker = -2,
            };

            blf.EndOfFile = new BlfChunkEndOfFile()
            {
                Signature = "_eof",
                Length = (int)TagStructure.GetStructureSize(typeof(BlfChunkEndOfFile), Version, CachePlatform),
                MajorVersion = 1,
                MinorVersion = 2
            };

            blf.ContentFlags |= BlfFileContentFlags.StartOfFile | BlfFileContentFlags.EndOfFile;

            blf.Scenario = MapInfo ?? GenerateMapInfo(scnr, scenarioName);
            blf.ContentFlags |= BlfFileContentFlags.Scenario;

            if (MapVariant != null)
            {
                blf.MapVariant = MapVariant.MapVariant;
                blf.MapVariantTagNames = MapVariant.MapVariantTagNames;
                blf.ContentFlags |= BlfFileContentFlags.MapVariant | BlfFileContentFlags.MapVariantTagNames;
            }

            return blf;
        }

        private BlfScenario GenerateMapInfo(Scenario scnr, string scenarioName)
        {
            var scnrBlf = new BlfScenario()
            {
                Signature = "levl",
                Length = (int)TagStructure.GetStructureSize(typeof(BlfScenario), Version, CachePlatform),
                MajorVersion = 3,
                MinorVersion = 1
            };

            scnrBlf.MapId = scnr.MapId;

            scnrBlf.Names = new NameUnicode32[12];
            for (int i = 0; i < scnrBlf.Names.Length; i++)
                scnrBlf.Names[i] = new NameUnicode32() { Name = MapName ?? scenarioName.ToPascalCase() };

            scnrBlf.Descriptions = new NameUnicode128[12];
            for (int i = 0; i < scnrBlf.Descriptions.Length; i++)
                scnrBlf.Descriptions[i] = new NameUnicode128() { Name = MapDescription ?? "" };

            scnrBlf.MapName = scenarioName;
            scnrBlf.ImageName = $"m_{scenarioName}";
            scnrBlf.Unknown1 = 2;
            scnrBlf.Unknown2 = 6;
            scnrBlf.GameEngineTeamCounts = new byte[11] { 00, 02, 08, 08, 08, 08, 08, 08, 04, 02, 08 };

            scnrBlf.MapFlags = BlfScenarioFlags.GeneratesFilm;
            switch (scnr.MapType)
            {
                case ScenarioMapType.MainMenu:
                    scnrBlf.MapFlags |= BlfScenarioFlags.IsMainmenu;
                    break;
                case ScenarioMapType.Multiplayer:
                    scnrBlf.MapFlags |= BlfScenarioFlags.IsMultiplayer;
                    break;
                case ScenarioMapType.SinglePlayer:
                    scnrBlf.MapFlags |= BlfScenarioFlags.IsCampaign;
                    break;
            }

            return scnrBlf;
        }
    }
}
