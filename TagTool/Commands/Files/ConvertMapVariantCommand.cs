using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
                convertedBlf = ConvertMapVariant(mapsDirectory, blfChunks["mvar"]);
            }
            else if (blfChunks.ContainsKey("_cmp"))
            {
                var decompressed = DecompressChunk(blfChunks["_cmp"]);
                var chunk = BlfReader.ReadChunks(new MemoryStream(decompressed)).First();
                if (chunk.Tag != "mvar")
                    return new TagToolError(CommandError.OperationFailed, "Unsupported input file");

                convertedBlf = ConvertMapVariant(mapsDirectory, chunk);
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

        private Blf ConvertMapVariant(DirectoryInfo mapsDirectory, BlfChunk chunk)
        {
            var stream = new MemoryStream(chunk.Data);

            if (chunk.MajorVerson == 31)
            {
                return ConvertReachMapVariant(stream, mapsDirectory);
            }
            else
            {
                new TagToolError(CommandError.OperationFailed, "Unsupported Map Variant version");
                return null;
            }
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

            var converter = new ReachMapVariantConverter();

            // hardcode for now
            converter.SubstitutedTags.Add(@"objects\vehicles\human\warthog\warthog.vehi", @"objects\vehicles\warthog\warthog.vehi");
            converter.SubstitutedTags.Add(@"objects\vehicles\human\mongoose\mongoose.vehi", @"objects\vehicles\mongoose\mongoose.vehi");
            converter.SubstitutedTags.Add(@"objects\vehicles\human\scorpion\scorpion.vehi", @"objects\vehicles\scorpion\scorpion.vehi");
            converter.SubstitutedTags.Add(@"objects\vehicles\human\falcon\falcon.vehi", @"objects\vehicles\hornet\hornet.vehi");
            converter.SubstitutedTags.Add(@"objects\vehicles\covenant\ghost\ghost.vehi", @"objects\vehicles\ghost\ghost.vehi");
            converter.SubstitutedTags.Add(@"objects\vehicles\covenant\wraith\wraith.vehi", @"objects\vehicles\wraith\wraith.vehi");
            converter.SubstitutedTags.Add(@"objects\vehicles\covenant\banshee\banshee.vehi", @"objects\vehicles\banshee\banshee.vehi");
            converter.SubstitutedTags.Add(@"objects\vehicles\human\turrets\machinegun\machinegun.vehi", @"objects\weapons\turret\machinegun_turret\machinegun_turret.vehi");
            converter.SubstitutedTags.Add(@"objects\vehicles\covenant\turrets\plasma_turret\plasma_turret_mounted.vehi", @"objects\weapons\turret\plasma_cannon\plasma_cannon.vehi");
            converter.SubstitutedTags.Add(@"objects\vehicles\covenant\turrets\shade\shade.vehi", @"objects\vehicles\shade\shade.vehi");

            converter.SubstitutedTags.Add(@"objects\equipment\hologram\hologram.eqip", @"objects\equipment\hologram_equipment\hologram_equipment.eqip");
            converter.SubstitutedTags.Add(@"objects\equipment\active_camouflage\active_camouflage.eqip", @"objects\equipment\invisibility_equipment\invisibility_equipment.eqip");

            converter.SubstitutedTags.Add(@"objects\multi\models\mp_hill_beacon\mp_hill_beacon.bloc", @"objects\multi\koth\koth_hill_static.bloc");
            converter.SubstitutedTags.Add(@"objects\multi\models\mp_flag_base\mp_flag_base.bloc", @"objects\multi\ctf\ctf_flag_spawn_point.bloc");
            converter.SubstitutedTags.Add(@"objects\multi\models\mp_circle\mp_circle.bloc", @"objects\multi\oddball\oddball_ball_spawn_point.bloc");
            converter.SubstitutedTags.Add(@"objects\multi\archive\vip\vip_boundary.bloc", @"objects\multi\vip\vip_destination_static.bloc");

            converter.ExcludedTags.Add(@"objects\multi\boundaries\soft_safe_volume.scen");
            converter.ExcludedTags.Add(@"objects\multi\boundaries\soft_kill_volume.scen");
            converter.ExcludedTags.Add(@"objects\multi\boundaries\kill_volume.scen");
            converter.ExcludedTags.Add(@"objects\multi\spawning\respawn_zone.scen");
            converter.ExcludedTags.Add(@"objects\levels\forge\ff_light_flash_red\ff_light_flash_red.bloc");

            return converter.Convert(sourceScenario, sourceBlf);
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

        private static byte[] DecompressChunk(BlfChunk cmpChunk)
        {
            var stream = new MemoryStream(cmpChunk.Data);
            var reader = new EndianReader(stream, EndianFormat.BigEndian);
            var compressionType = reader.ReadSByte();
            if (compressionType != 0)
                throw new NotSupportedException();

            var size = reader.ReadInt32();
            reader.ReadBytes(2); // skip header
            var compressed = reader.ReadBytes(size - 2);
            return Decompress(compressed);
        }

        static byte[] Decompress(byte[] compressed)
        {
            using (var stream = new DeflateStream(new MemoryStream(compressed), CompressionMode.Decompress))
            {
                var outStream = new MemoryStream();
                stream.CopyTo(outStream);
                return outStream.ToArray();
            }
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
