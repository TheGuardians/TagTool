using System;
using System.Collections.Generic;
using System.IO;
using BlamCore.Cache;
using BlamCore.Common;
using BlamCore.TagDefinitions;
using BlamCore.Serialization;
using BlamCore.IO;
using BlamCore.Commands;

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

            var mapIndices = new Dictionary<int, int>();
            var mapScenarios = new Dictionary<int, Scenario_mapId>();

            using (var cacheStream = CacheContext.OpenTagCacheRead())
            {
                foreach (var scnrTag in CacheContext.TagCache.Index.FindAllInGroup("scnr"))
                {
                    var tagContext = new TagSerializationContext(cacheStream, CacheContext, scnrTag);
                    var scnr = CacheContext.Deserializer.Deserialize<Scenario_mapId>(tagContext);
                    mapIndices[scnr.MapId] = scnrTag.Index;
                    mapScenarios[scnr.MapId] = scnr;
                }
            }

            foreach (var mapFile in CacheContext.Directory.GetFiles("*.map"))
            {
                try
                {
                    using (var stream = mapFile.Open(FileMode.Open, FileAccess.ReadWrite))
                    using (var reader = new EndianReader(stream))
                    using (var writer = new EndianWriter(stream))
                    {
                        if (reader.ReadInt32() != new Tag("head").Value)
                        {
                            Console.Error.WriteLine("Invalid map file");
                            return true;
                        }

                        reader.BaseStream.Position = 0x2DEC;
                        var mapId = reader.ReadInt32();

                        if (mapIndices.ContainsKey(mapId))
                        {
                            var mapIndex = mapIndices[mapId];

                            writer.BaseStream.Position = 0x2DF0;
                            writer.Write(mapIndex);

                            Console.WriteLine($"Scenario tag index for {mapFile.Name}: {mapIndex:X8}");

                            if (mapScenarios.ContainsKey(mapId) &&
                                mapScenarios[mapId].MapType == MapTypeValue.Multiplayer)
                            {
                                var dataContext = new DataSerializationContext(reader, writer);

                                stream.Seek(0xBD80, SeekOrigin.Begin);
                                var mapVariant = CacheContext.Deserializer.Deserialize<MapVariant>(dataContext);

                                foreach (var entry in mapVariant.BudgetEntries)
                                    if ((entry.TagIndex != -1) &&
                                        (CacheContext.GetTag(entry.TagIndex) == null))
                                        entry.TagIndex = -1;

                                stream.Seek(0xBD80, SeekOrigin.Begin);
                                CacheContext.Serializer.Serialize(dataContext, mapVariant);
                            }
                            else
                            {
                                stream.Seek(0x3390, SeekOrigin.Begin);
                            }

                            stream.SetLength(stream.Position);
                        }
                    }
                }
                catch (IOException)
                {
                    Console.Error.WriteLine("Unable to open the map file for reading.");
                }
            }

            Console.WriteLine("Done!");

            return true;
        }
                
        [TagStructure(Name = "scenario", Tag = "scnr", MinVersion = CacheVersion.Halo3Retail)]
        private class Scenario_mapId
        {
            [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail)]
            public byte MapTypePadding = 0;

            public MapTypeValue MapType = MapTypeValue.Multiplayer;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public MapSubTypeValue MapSubType = MapSubTypeValue.None;

            public ScenarioFlags Flags = ScenarioFlags.None;
            public int CampaignId = 1;
            public int MapId = 0;
        }
    }
}