using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.BlamFile;
using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.Commands.Common;
using TagTool.IO;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Modding
{
    class MapFileCommand : Command
    {
        public GameCacheModPackage Cache { get; }

        public MapFileCommand(GameCacheModPackage cache) :
            base(true,

                "MapFile",
                "Manage map files",

                "MapFile Add <path to .map> [sandbox.map]>\n" +
                "MapFile Remove <map id>\n" +
                "MapFile List\n",

                "\"MapFile Add\" adds a map file to the mod package.\n" +
                "\"MapFile Remove\" removes a map file from the mod package.\n" +
                "\"MapFile List lists\" the map files in the mod package.\n")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1)
                return new TagToolError(CommandError.ArgCount);

            var command = args[0].ToLower();
            args.RemoveAt(0);

            var commandMap = new Dictionary<string, Func<List<string>, object>>
            {
                ["add"] = ExecuteAdd,
                ["delete"] = ExecuteDelete,
                ["list"] = ExecuteList
            };

            if (!commandMap.TryGetValue(command, out var handler))
                return new TagToolError(CommandError.ArgInvalid);

            return handler(args);
        }

        private object ExecuteAdd(List<string> args)
        {
            var mapStream = new MemoryStream();
            using (var fs = File.OpenRead(args[0]))
                fs.CopyTo(mapStream);

            var mapFile = new MapFile();
            mapFile.Read(new EndianReader(mapStream));
            mapStream.Position = 0;

            if (mapFile.MapFileBlf == null)
                return new TagToolError(CommandError.CustomError, "Invalid map map file. Missing blf data");

            var header = (CacheFileHeaderGenHaloOnline)mapFile.Header;

            // handle optional sandbox.map argument
            if (args.Count > 1)
            {
                using (var mapVariantStream = File.OpenRead(args[1]))
                {
                    var mapVariantBlf = new Blf(Cache.Version, Cache.Platform);
                    if (!mapVariantBlf.Read(new EndianReader(mapVariantStream, Cache.Endianness)))
                        return new TagToolError(CommandError.CustomError, "Failed to read map variant");

                    if (mapVariantBlf.MapVariant.MapVariant.MapId != header.MapId)
                        return new TagToolError(CommandError.CustomError, "Tried to import a map variant into a scenario with a different map id");

                    InjectMapVariantIntoMapFile(mapFile, mapVariantBlf);
                    mapFile.Write(new EndianWriter(mapStream, Cache.Endianness));
                    mapStream.Position = 0;
                }
            }

            Cache.AddMapFile(mapStream, header.MapId);

            Console.WriteLine($"Map added successfully.");
            return true;
        }

        private object ExecuteDelete(List<string> args)
        {
            if (args.Count < 1)
                return new TagToolError(CommandError.ArgCount);

            if (!int.TryParse(args[0], out int mapId))
                return new TagToolError(CommandError.ArgInvalid, "Invalid map id");

            var mapFileIndex = Cache.BaseModPackage.MapIds.IndexOf(mapId);
            if (mapFileIndex == -1)
                return new TagToolError(CommandError.CustomError, $"Map file with map id: {mapId} not found");

            Cache.BaseModPackage.MapIds.RemoveAt(mapFileIndex);
            Cache.BaseModPackage.MapFileStreams.RemoveAt(mapFileIndex);
            Cache.BaseModPackage.MapToCacheMapping.Remove(mapFileIndex);
            Console.WriteLine($"Map deleted successfully.");
            return true;
        }

        private object ExecuteList(List<string> args)
        {
            string columnFormat = "{0,-10} {1,-20} {2,-50} {3,-20}";
            Console.WriteLine(new string('-', 100));
            Console.WriteLine(columnFormat, "Map Id", "Map Name", "Scenario", "Map Variant");
            Console.WriteLine(new string('-', 100));

            for (int i = 0; i < Cache.BaseModPackage.MapFileStreams.Count; i++)
            {
                var mapStream = Cache.BaseModPackage.MapFileStreams[i];
                mapStream.Position = 0;
                var mapFile = new MapFile();
                mapFile.Read(new EndianReader(mapStream));
                mapStream.Position = 0;

                var header = (CacheFileHeaderGenHaloOnline)mapFile.Header;
                var mapVariant = mapFile.MapFileBlf?.MapVariant?.MapVariant;

                Console.WriteLine(columnFormat, header.MapId, header.Name, header.ScenarioPath, mapVariant == null ? "None" : mapVariant.Metadata.Name);
            }

            return true;
        }

        private void InjectMapVariantIntoMapFile(MapFile mapFile, Blf mapVariantBlf)
        {
            var header = (CacheFileHeaderGenHaloOnline)mapFile.Header;

            mapVariantBlf.MapVariant.MapVariant.MapId = header.MapId;
            if (mapFile.MapFileBlf.Scenario != null)
            {
                UpdateMetadata(mapVariantBlf.ContentHeader.Metadata, mapFile.MapFileBlf.Scenario);
                UpdateMetadata(mapVariantBlf.MapVariant.MapVariant.Metadata, mapFile.MapFileBlf.Scenario);
            }

            mapFile.MapFileBlf.MapVariant = mapVariantBlf.MapVariant;
            mapFile.MapFileBlf.ContentFlags |= BlfFileContentFlags.MapVariant;
            mapFile.MapFileBlf.MapVariantTagNames = mapVariantBlf.MapVariantTagNames;
            mapFile.MapFileBlf.ContentFlags |= BlfFileContentFlags.MapVariantTagNames;
        }

        private void UpdateMetadata(ContentItemMetadata metadata, BlfScenario blfScenario)
        {
            metadata.MapId = blfScenario.MapId;
            metadata.Name = blfScenario.Names[0].Name;
            metadata.Description = blfScenario.Descriptions[0].Name;
        }
    }
}
