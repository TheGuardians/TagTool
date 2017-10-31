using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using BlamCore.Cache;

namespace TagTool.Commands.Tags
{
    class TagResourceCommand : Command
    {
        public TagResourceCommand() : base(
            CommandFlags.None,

            "TagResource",
            "Manage raw resource data",

            "TagResource Extract <.dat file> <index> <compressed size> <output file>\n" +
            "TagResource Import <.dat file> <index> <input file>\n" +
            "TagResource Delete <.dat file> <index>",

            "Extracts, imports and deletes raw resource data.\n" +
            "When extracting, the compressed size must include chunk headers.\n\n" +
            "Note that this is extremely low-level and does NOT edit tags\n" +
            "which reference imported resources.")
        {
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 3)
                return false;

            var command = args[0];
            var cachePath = args[1];

            if (!uint.TryParse(args[2], NumberStyles.HexNumber, null, out uint index))
                return false;

            switch (command.ToLower())
            {
                case "extract":
                    return ExtractResource(cachePath, index, args);

                case "import":
                    return ImportResource(cachePath, index, args);

                default:
                    return false;
            }
        }

        private static bool ExtractResource(string cachePath, uint index, IReadOnlyList<string> args)
        {
            if (args.Count != 5)
                return false;

            if (!uint.TryParse(args[3], NumberStyles.HexNumber, null, out uint compressedSize))
                return false;

            var outPath = args[4];

            using (var stream = File.OpenRead(cachePath))
                {
                    var cache = new ResourceCache(stream);
                    using (var outStream = File.Open(outPath, FileMode.Create, FileAccess.Write))
                    {
                        cache.Decompress(stream, (int)index, compressedSize, outStream);
                        Console.WriteLine("Wrote 0x{0:X} bytes to {1}.", outStream.Position, outPath);
                    }
                }

            return true;
        }

        private static bool ImportResource(string cachePath, uint index, IReadOnlyList<string> args)
        {
            if (args.Count != 4)
                return false;

            var inPath = args[3];
            try
            {
                using (var stream = File.Open(cachePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    var cache = new ResourceCache(stream);
                    var data = File.ReadAllBytes(inPath);
                    var compressedSize = cache.Compress(stream, (int)index, data);
                    Console.WriteLine("Imported 0x{0:X} bytes.", data.Length);
                    Console.WriteLine("Compressed size = 0x{0:X}", compressedSize);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to import resource: {0}", ex.Message);
            }

            return true;
        }
    }
}
