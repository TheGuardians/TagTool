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
    class ExtractBitmapsCommand : Command
    {
        private GameCache Cache { get; }

        public ExtractBitmapsCommand(GameCache cache) :
            base(true,
                
                "ExtractBitmaps",
                "Extract all bitmaps to a folder",
                
                "ExtractBitmaps <Folder>",
                
                "Extract all bitmap tags and any subimages to the given folder.\n" +
                "If the folder does not exist, it will be created.")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var outDir = args[0];
            Directory.CreateDirectory(outDir);

            Console.WriteLine("Loading resource caches...");

            var count = 0;

            using (var tagsStream = Cache.TagCache.OpenTagCacheRead())
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
                        Console.WriteLine("ERROR: Failed to extract bitmap: " + ex.Message);
                    }
                #endif
                }
            }

            Console.WriteLine("Extracted {0} bitmaps.", count);

            return true;
        }
    }
}