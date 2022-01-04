using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.BlamFile;
using TagTool.BlamFile.Reach;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Files
{
    public class ConvertMapVariantCommand : Command
    {
        private readonly GameCacheHaloOnlineBase Cache;

        public ConvertMapVariantCommand(GameCacheHaloOnlineBase cache)
            : base(true,

                  "ConvertMapVariant",
                  "Converts a map variant",

                  "ConvertMapVariant <maps directory> <input file> <output file>",
                  "ConvertMapVariant")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 3)
                return new TagToolError(CommandError.ArgCount);

            var mapsDirectory = new DirectoryInfo(args[0]);
            if (!mapsDirectory.Exists)
                return new TagToolError(CommandError.DirectoryNotFound, "Maps directory not found");

            var inputFile = new FileInfo(args[1]);
            if (!inputFile.Exists)
                return new TagToolError(CommandError.DirectoryNotFound, "Input file not found");

            var outputFile = new FileInfo(args[2]);

            Dictionary<Tag, BlfChunk> blfChunks;
            using (var inputStream = inputFile.OpenRead())
                blfChunks = BlfReader.ReadChunks(inputStream).ToDictionary(c => c.Tag);

            Blf convertedBlf = null;

            if (blfChunks.ContainsKey("mvar"))
            {
                var chunk = blfChunks["mvar"];
                var stream = new MemoryStream(chunk.Data);
                if (chunk.MajorVerson == 31)
                {
                    convertedBlf = ConvertReachMapVariant(stream, mapsDirectory);
                }
                else
                {
                    return new TagToolError(CommandError.OperationFailed, "Unsupported Map Variant version");
                }
            }
            else
            {
                return new TagToolError(CommandError.OperationFailed, "Unsupported input file");
            }

            if (convertedBlf == null)
                return new TagToolError(CommandError.OperationFailed, "Failed to convert map variant");

            outputFile.Directory.Create();
            using (var writer = new EndianWriter(outputFile.Create()))
                convertedBlf.Write(writer);

            Console.WriteLine("Done.");
            return true;
        }

        private Blf ConvertReachMapVariant(MemoryStream stream, DirectoryInfo mapsDirectory)
        {
            var sourceBlf = new ReachBlfMapVariant();
            sourceBlf.Decode(stream);

            int mapId = sourceBlf.MapVariant.MapId;
            var sourceCache = GetMapCache(mapsDirectory, mapId);
            if (sourceCache == null)
                return null;

            var sourceCacheStream = sourceCache.OpenCacheRead();
            var sourceScenario = sourceCache.Deserialize<Scenario>(sourceCacheStream, sourceCache.TagCache.FindFirstInGroup("scnr"));
            if (sourceScenario.MapId != mapId)
            {
                new TagToolError(CommandError.FileNotFound, $"Scenario map id did not match");
                return null;
            }

            return ReachMapVariantConverter.Convert(sourceScenario, sourceBlf);
        }

        private GameCache GetMapCache(DirectoryInfo mapsDirectory, int mapId)
        {
            var mapFile = new FileInfo(Path.Combine(mapsDirectory.FullName, $"{MapIdToFilename[mapId]}.map"));
            if (!mapFile.Exists)
            {
                new TagToolError(CommandError.FileNotFound, $"'${MapIdToFilename[mapId]}.map' could not be found.");
                return null;
            }
            return GameCache.Open(mapFile);
        }

        private static readonly Dictionary<int, string> MapIdToFilename = new Dictionary<int, string>()
        {
            [030] = "zanzibar",
            [300] = "construct",
            [310] = "deadlock",
            [320] = "guardian",
            [330] = "isolation",
            [340] = "riverworld",
            [350] = "salvation",
            [360] = "snowbound",
            [380] = "chill",
            [390] = "cyberdyne",
            [400] = "shrine",
            [410] = "bunkerworld",
            [440] = "docks",
            [470] = "sidewinder",
            [480] = "warehouse",
            [490] = "descent",
            [500] = "spacecamp",
            [520] = "lockout",
            [580] = "armory",
            [590] = "ghosttown",
            [600] = "chillout",
            [720] = "midship",
            [730] = "sandbox",
            [740] = "fortress",
            [1000] = "20_sword_slayer",
            [1020] = "45_launch_station",
            [1035] = "50_panopticon",
            [1040] = "45_aftship",
            [1055] = "30_settlement",
            [1080] = "70_boneyard",
            [1150] = "52_ivory_tower",
            [1200] = "35_island",
            [1500] = "condemned",
            [1510] = "trainingpreserve",
            [2001] = "dlc_slayer",
            [2002] = "dlc_invasion",
            [2004] = "dlc_medium",
            [3006] = "forge_halo",
            [10010] = "cex_damnation",
            [10020] = "cex_beaver_creek",
            [10030] = "cex_timberland",
            [10050] = "cex_headlong",
            [10060] = "cex_hangemhigh",
            [10070] = "cex_prisoner",
        };
    }
}
