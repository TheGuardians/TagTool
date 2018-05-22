using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TagTool.Cache;
using TagTool.Commands;
using TagTool.Common;

namespace TagTool.Commands.Tags
{
    class TagResourceCommand : Command
    {
        public HaloOnlineCacheContext CacheContext { get; }

        public TagResourceCommand(HaloOnlineCacheContext cacheContext) :
            base(true,

                "TagResource",
                "Manage raw resource data",

                "TagResource Extract <Location> <Index> <Compressed Size> <Data File>\n" +
                "TagResource Import <Location> <Index> <Data File>",

                "Extracts and imports raw resource data.\n" +
                "When extracting, the compressed size must include chunk headers.\n\n" +
                "Note that this is extremely low-level and does NOT edit tags\n" +
                "which reference imported resources.")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 3)
                return false;

            var command = args[0];

            var location = ResourceLocation.None;

            switch (args[1])
            {
                case "resources":
                    location = ResourceLocation.Resources;
                    break;

                case "textures":
                    location = ResourceLocation.Textures;
                    break;

                case "textures_b":
                    location = ResourceLocation.TexturesB;
                    break;

                case "audio":
                    location = ResourceLocation.Audio;
                    break;

                case "resources_b":
                    location = ResourceLocation.ResourcesB;
                    break;

                case "render_models" when CacheContext.Version >= CacheVersion.HaloOnline235640:
                    location = ResourceLocation.RenderModels;
                    break;

                case "lightmaps" when CacheContext.Version >= CacheVersion.HaloOnline235640:
                    location = ResourceLocation.Lightmaps;
                    break;

                default:
                    Console.WriteLine($"Invalid resource location: {args[0]}");
                    return false;
            }

            var index = Convert.ToUInt32(args[2], 16);

            switch (command.ToLower())
            {
                case "extract":
                    return ExtractResource(location, index, args);

                case "import":
                    return ImportResource(location, index, args);

                default:
                    return false;
            }
        }

        private bool ExtractResource(ResourceLocation location, uint index, IReadOnlyList<string> args)
        {
            if (args.Count != 5)
                return false;

            var compressedSize = Convert.ToUInt32(args[3], 16);
            var outPath = args[4];
            var outFile = new FileInfo(args[4]);

            if (!outFile.Directory.Exists)
            {
                Console.Write("ERROR: Directory does not exist. Create it? [y/n]: ");

                switch (Console.ReadLine().ToLower())
                {
                    case "y":
                    case "yes":
                        outFile.Directory.Create();
                        break;

                    default:
                        return true;
                }
            }

            var cache = CacheContext.GetResourceCache(location);

            using (var stream = CacheContext.OpenResourceCacheRead(location))
            {
                using (var outStream = File.Open(outPath, FileMode.Create, FileAccess.Write))
                {
                    cache.Decompress(stream, (int)index, compressedSize, outStream);
                    Console.WriteLine("Wrote 0x{0:X} bytes to {1}.", outStream.Position, outPath);
                }
            }

            return true;
        }

        private bool ImportResource(ResourceLocation location, uint index, IReadOnlyList<string> args)
        {
            if (args.Count != 4)
                return false;

            var inPath = args[3];

            var cache = CacheContext.GetResourceCache(location);

            using (var stream = CacheContext.OpenResourceCacheReadWrite(location))
            {
                var data = File.ReadAllBytes(inPath);
                var compressedSize = cache.Compress(stream, (int)index, data);
                Console.WriteLine("Imported 0x{0:X} bytes.", data.Length);
                Console.WriteLine("Compressed size = 0x{0:X}", compressedSize);
            }

            return true;
        }
    }
}