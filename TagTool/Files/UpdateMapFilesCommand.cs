using BlamCore.Cache;
using BlamCore.Commands;
using BlamCore.Common;
using System;
using System.Collections.Generic;
using System.IO;

namespace TagTool.Files
{
    class UpdateMapFilesCommand : Command
    {
        public GameCacheContext CacheContext { get; }

        public UpdateMapFilesCommand(GameCacheContext cacheContext)
            : base(CommandFlags.Inherit,

                  "UpdateMapFiles",
                  "Updates the game's .map files to contain valid scenario tag indices.",

                  "UpdateMapFiles",

                  "Updates the game's .map files to contain valid scenario tag indices.")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;

            var mapScenarioIndices = new Dictionary<int, int>();

            using (var stream = CacheContext.OpenTagCacheRead())
            using (var reader = new BinaryReader(stream))
            {
                foreach (var tag in CacheContext.TagCache.Index.FindAllInGroup("scnr"))
                {
                    if (tag.HeaderOffset == -1)
                        continue;

                    reader.BaseStream.Position = tag.HeaderOffset + tag.DefinitionOffset + 0x8;
                    mapScenarioIndices[reader.ReadInt32()] = tag.Index;
                }
            }

            foreach (var mapFile in CacheContext.Directory.GetFiles("*.map"))
            {
                using (var stream = mapFile.Open(FileMode.Open, FileAccess.ReadWrite))
                using (var reader = new BinaryReader(stream))
                using (var writer = new BinaryWriter(stream))
                {
                    if (reader.ReadInt32() != new Tag("head").Value)
                    {
                        Console.Error.WriteLine("Invalid map file");
                        return true;
                    }

                    reader.BaseStream.Position = 0x2DEC;
                    var mapId = reader.ReadInt32();

                    if (!mapScenarioIndices.ContainsKey(mapId))
                        continue;

                    var mapIndex = mapScenarioIndices[mapId];

                    writer.BaseStream.Position = 0x2DF0;
                    writer.Write(mapIndex);

                    Console.WriteLine($"Scenario tag index for {mapFile.Name}: {mapIndex:X8}");
                }
            }

            Console.WriteLine("Done!");

            return true;
        }
    }
}