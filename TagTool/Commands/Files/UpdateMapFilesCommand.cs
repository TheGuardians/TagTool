using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TagTool.Cache;
using TagTool.Commands;
using TagTool.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Files
{
    class UpdateMapFilesCommand : Command
    {
        private const int MapNameOffset = 0x01A4;
        private const int ScenarioPathOffset = 0x01C8;
        private const int ScenarioMapIdOffset = 0x2DEC;
        private const int ScenarioTagIndexOffset = 0x2DF0;

        private const int ContentNameOffset = 0x42D4;
        private const int ContentMapNameOffset = 0x43D4;
        private static readonly int[] ContentMapIdOffsets = new[] { 0x33CC, 0xBE60, 0xBE80 };

        public GameCacheContext CacheContext { get; }

        public UpdateMapFilesCommand(GameCacheContext cacheContext)
            : base(CommandFlags.Inherit,

                  "UpdateMapFiles",
                  "Updates the game's .map files to contain valid scenario indices.",

                  "UpdateMapFiles",

                  "Updates the game's .map files to contain valid scenario indices.")
        {
            CacheContext = cacheContext;
        }
        
        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;
            
            var scenarioIndices = new Dictionary<int, (ScenarioMapType, int)>();

            using (var stream = CacheContext.OpenTagCacheRead())
            using (var reader = new BinaryReader(stream))
            {
                foreach (var tag in CacheContext.TagCache.Index.FindAllInGroup("scnr"))
                {
                    if (tag.HeaderOffset == -1)
                        continue;

                    reader.BaseStream.Position = tag.HeaderOffset + tag.DefinitionOffset;
                    var mapType = reader.ReadEnum<ScenarioMapType>();

                    reader.BaseStream.Position = tag.HeaderOffset + tag.DefinitionOffset + 0x8;
                    var mapId = reader.ReadInt32();

                    scenarioIndices[mapId] = (mapType, tag.Index);
                }
            }

            var mainmenuMapFile = CacheContext.Directory.GetFiles("mainmenu.map")[0];
            var guardianMapFile = CacheContext.Directory.GetFiles("guardian.map")[0];

            foreach (var entry in scenarioIndices)
            {
                var scenarioPath = CacheContext.TagNames.ContainsKey(entry.Value.Item2) ?
                    CacheContext.TagNames[entry.Value.Item2] :
                    $"0x{entry.Value:X4}";

                var mapName = scenarioPath.Split('\\').Last();
                var mapFile = new FileInfo(Path.Combine(CacheContext.Directory.FullName, $"{mapName}.map"));

                if (!mapFile.Exists)
                    mapFile = ((entry.Value.Item1 == ScenarioMapType.Multiplayer) ? guardianMapFile : mainmenuMapFile).CopyTo(mapFile.FullName);

                using (var stream = mapFile.Open(FileMode.Open, FileAccess.ReadWrite))
                using (var reader = new BinaryReader(stream))
                using (var writer = new BinaryWriter(stream))
                {
                    if (reader.ReadInt32() != new Tag("head").Value)
                        continue;

                    stream.Position = MapNameOffset;
                    for (var i = 0; i < 32; i++)
                        writer.Write(i < mapName.Length ? mapName[i] : '\0');

                    stream.Position = ScenarioPathOffset;
                    for (var i = 0; i < 256; i++)
                        writer.Write(i < scenarioPath.Length ? scenarioPath[i] : '\0');

                    stream.Position = ScenarioMapIdOffset;
                    writer.Write(entry.Key);

                    stream.Position = ScenarioTagIndexOffset;
                    writer.Write(entry.Value.Item2);

                    if (entry.Value.Item1 == ScenarioMapType.Multiplayer)
                    {
                        var contentName = scenarioPath.StartsWith("levels\\dlc\\") ?
                            $"dlc_{mapName}" :
                            $"m_{mapName}";

                        stream.Position = ContentNameOffset;
                        for (var i = 0; i < 256; i++)
                            writer.Write(i < contentName.Length ? contentName[i] : '\0');

                        stream.Position = ContentMapNameOffset;
                        for (var i = 0; i < 256; i++)
                            writer.Write(i < mapName.Length ? mapName[i] : '\0');

                        foreach (var offset in ContentMapIdOffsets)
                        {
                            stream.Position = offset;
                            writer.Write(entry.Key);
                        }
                    }
                    
                    Console.WriteLine($"Scenario tag index for {mapFile.Name}: 0x{entry.Value.Item2:X4}");
                }
            }
            
            Console.WriteLine("Done!");

            return true;
        }
    }
}