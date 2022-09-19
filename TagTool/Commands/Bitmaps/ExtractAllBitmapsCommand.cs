using TagTool.Bitmaps;
using TagTool.Cache;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Commands.Common;
using TagTool.IO;

namespace TagTool.Commands.Bitmaps
{
    class ExtractAllBitmapsCommand : Command
    {
        private GameCache Cache { get; }

        public ExtractAllBitmapsCommand(GameCache cache) :
            base(true,
                
                "ExtractBitmaps",
                "Extract all bitmaps to a folder",
                
                "ExtractBitmaps <type> <directory>",
                
                "Extract multiple images to the provided directory.\n\n" +
                "Available types:\n" +
				"\t all -> Extracts every image in the current cache.")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return new TagToolError(CommandError.ArgCount);

            string[] types = { "all" };

            if (Array.IndexOf(types, args[0]) == -1)
                return new TagToolError(CommandError.CustomError, "Invalid extract type specified!");

            var outDir = args[1];
            Directory.CreateDirectory(outDir);

            Console.WriteLine("Loading resource caches...");

            var count = 0;

            using (var tagsStream = Cache.OpenCacheRead())
            {

                foreach (var tag in Cache.TagCache.FindAllInGroup("bitm"))
                {
                    Console.Write("Extracting ");
                    TagPrinter.PrintTagShort(tag);

                #if !DEBUG
                    try
                    {
                #endif
                        var bitmap = Cache.Deserialize<Bitmap>(tagsStream, tag);
                        var ddsOutDir = outDir;

                        if (bitmap.Images.Count > 1)
                        {
                            ddsOutDir = Path.Combine(outDir, tag.Index.ToString("X8"));
                            Directory.CreateDirectory(ddsOutDir);
                        }

                        for (var i = 0; i < bitmap.Images.Count; i++)
                        {
                            var outPath = Path.Combine(ddsOutDir, ((bitmap.Images.Count > 1) ? i.ToString() : tag.Index.ToString("X8")) + ".dds");
                            var ddsFile = BitmapExtractor.ExtractBitmap(Cache, bitmap, i);
                            if (ddsFile == null)
                                continue;
                            using (var outStream = File.Open(outPath, FileMode.Create, FileAccess.Write))
                            using (var writer = new EndianWriter(outStream, EndianFormat.LittleEndian))
                            {
                                ddsFile.Write(writer);
                            }
                        }
                        count++;
#if !DEBUG
                    }
                    catch (Exception ex)
                    {
                        return new TagToolError(CommandError.OperationFailed, "Failed to extract bitmap: " + ex.Message);
                    }
#endif
                }
            }

            Console.WriteLine("Extracted {0} bitmaps.", count);

            return true;
        }
    }
}