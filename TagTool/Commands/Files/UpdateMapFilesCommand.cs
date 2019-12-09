using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;
using TagTool.IO;
using TagTool.Serialization;

namespace TagTool.Commands.Files
{
    class UpdateMapFilesCommand : Command
    {
        public HaloOnlineCacheContext CacheContext { get; }

        public UpdateMapFilesCommand(HaloOnlineCacheContext cacheContext)
            : base(true,

                  "UpdateMapFiles",
                  "Updates the game's .map files to contain valid scenario indices.",

                  "UpdateMapFiles <MapInfo Directory> [forceupdate]",

                  "Updates the game's .map files to contain valid scenario indices.")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 2)
            {
                return false;
            }

            bool forceUpdate = false;
            bool hasMapInfo = false;

            if (args.Count >= 1)
                hasMapInfo = true;
            if (args.Count == 2)
                if (args[1].ToLower() == "forceupdate")
                    forceUpdate = true;

            // Generate / update the map files
            foreach(var scenario in CacheContext.TagCache.Index.FindAllInGroup("scnr"))
            {
                var name = scenario.Name.Split('\\').Last();
                var mapInfoName = $"{name}.mapinfo";
                var mapFileName = $"{name}.map";
                var targetPath = Path.Combine(CacheContext.Directory.FullName, mapFileName);

                MapFile map;
                Blf mapInfo = null;

                if(hasMapInfo)
                {
                    var mapInfoDir = new DirectoryInfo(args[0]);
                    var files = mapInfoDir.GetFiles(mapInfoName);
                    if(files.Length != 0)
                    {
                        var mapInfoFile = files[0];
                        using (var stream = mapInfoFile.Open(FileMode.Open, FileAccess.Read))
                        using (var reader = new EndianReader(stream))
                        {
                            CacheVersion version = CacheVersion.Halo3Retail;
                            switch (reader.Length)
                            {
                                case 0x4e91:
                                    version = CacheVersion.Halo3Retail;
                                    break;
                                case 0x9A01:
                                    version = CacheVersion.Halo3ODST;
                                    break;
                                default:
                                    throw new Exception("Unexpected map info file size");
                            }
                            mapInfo = new Blf(version);
                            mapInfo.Read(reader);
                            mapInfo.ConvertBlf(CacheContext.Version);
                        }
                    }
                }

                try
                {
                    var fileInfo = CacheContext.Directory.GetFiles(mapFileName)[0];
                    map = new MapFile();
                    using (var stream = fileInfo.Open(FileMode.Open, FileAccess.Read))
                    using (var reader = new EndianReader(stream))
                        map.Read(reader);
                    map.UpdateScenarioIndices(scenario.Index);

                    if(mapInfo != null)
                        if(forceUpdate || map.MapFileBlf == null)
                            map.MapFileBlf = mapInfo;
                                 
                }
                catch (Exception)
                {
                    map = GenerateMapFile(scenario, mapInfo);
                }

                var targetFile = new FileInfo(targetPath);
                using(var stream = targetFile.Create())
                using(var writer = new EndianWriter(stream))
                {
                    map.Write(writer);
                }

                if (mapInfo != null)
                    Console.WriteLine($"Scenario tag index for {name}: 0x{scenario.Index.ToString("X4")} (using map info)");
                else
                    Console.WriteLine($"Scenario tag index for {name}: 0x{scenario.Index.ToString("X4")} (WARNING: not using map info)");

            }
            Console.WriteLine("Done!");
            return true;
        }

        private MapFile GenerateMapFile(CachedTagInstance scenarioTag, Blf mapInfo = null)
        {
            MapFile map = new MapFile();
            var header = new MapFileHeader();
            Scenario scnr;
            using (var stream = CacheContext.OpenTagCacheRead())
            {
                var deserializer = new TagDeserializer(CacheContext.Version);
                scnr = (Scenario)CacheContext.Deserialize(stream, scenarioTag);
            }
                
            map.Version = CacheContext.Version;
            map.EndianFormat = EndianFormat.LittleEndian;
            map.MapVersion = MapFileVersion.HaloOnline;

            header.HeadTag = new Tag("head");
            header.FootTag = new Tag("foot");
            header.Version = (int)map.MapVersion;
            header.Build = CacheVersionDetection.GetBuildName(CacheContext.Version);

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
            header.SharedType = CacheFileSharedType.None;

            header.MapId = scnr.MapId;
            header.ScenarioTagIndex = scenarioTag.Index;
            header.Name = scenarioTag.Name.Split('\\').Last();
            header.ScenarioPath = scenarioTag.Name;
            header.ExternalDependencies = 0x3E;

            map.Header = header;

            header.FileLength = 0x3390;

            if(mapInfo != null)
            {
                if(mapInfo.ContentFlags.HasFlag(BlfFileContentFlags.StartOfFile) && mapInfo.ContentFlags.HasFlag(BlfFileContentFlags.EndOfFile) && mapInfo.ContentFlags.HasFlag(BlfFileContentFlags.Scenario))
                {
                    map.MapFileBlf = mapInfo;
                }
            }
            return map;
        }
    }
}