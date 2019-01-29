using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Bitmaps;
using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Bitmaps
{
    class ExtractBitmapCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private Bitmap Bitmap { get; }

        public ExtractBitmapCommand(HaloOnlineCacheContext cacheContext, CachedTagInstance tag, Bitmap bitmap)
            : base(false,

                  "ExtractBitmap",
                  "Extracts a bitmap to a file.",

                  "ExtractBitmap <output directory>",

                  "Extracts a bitmap to a file.")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Bitmap = bitmap;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;
            
            var directory = args[0];

            if (!Directory.Exists(directory))
            {
                Console.Write("Destination directory does not exist. Create it? [y/n] ");
                var answer = Console.ReadLine().ToLower();

                if (answer.Length == 0 || !(answer.StartsWith("y") || answer.StartsWith("n")))
                    return false;

                if (answer.StartsWith("y"))
                    Directory.CreateDirectory(directory);
                else
                    return false;
            }

            //var extractor = new BitmapExtractor(CacheContext);
            var extractor = new BitmapDdsExtractor(CacheContext);

            using (var tagsStream = CacheContext.OpenTagCacheRead())
            {
            #if !DEBUG
                try
                {
            #endif
                    var bitmap = CacheContext.Deserialize<Bitmap>(tagsStream, Tag);
                    var ddsOutDir = directory;

                    if (bitmap.Images.Count > 1)
                    {
                        ddsOutDir = Path.Combine(directory, Tag.Index.ToString("X8"));
                        Directory.CreateDirectory(ddsOutDir);
                    }

                    for (var i = 0; i < bitmap.Images.Count; i++)
                    {
                        var outPath = Path.Combine(ddsOutDir, ((bitmap.Images.Count > 1) ? i.ToString() : Tag.Index.ToString("X8")) + ".dds");

                        using (var outStream = File.Open(outPath, FileMode.Create, FileAccess.Write))
                        {
                            //extractor.ExtractBitmap(bitmap, i, outStream);
                            extractor.ExtractDds(bitmap, i, outStream);
                        }
                    }
            #if !DEBUG
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR: Failed to extract bitmap: " + ex.Message);
                }
            #endif
            }

            Console.WriteLine("Done!");

            return true;
        }
    }
}
