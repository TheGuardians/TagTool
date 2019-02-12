using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Bitmaps.Converter;
using TagTool.Bitmaps.DDS;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Porting
{
    public class ExtractBitmapCommand : Command
    {
        private CacheFile BlamCache;

        public ExtractBitmapCommand(CacheFile blamCache) :
            base(true,

                "ExtractBitmap",
                "Extracts a unmodified Bitmap from selected tag.",

                "ExtractBitmap <tag name> [output directory] ",

                "Extracts a unmodified Bitmap from selected tag.")
        {
            BlamCache = blamCache;
        }

        public override object Execute(List<string> args)
        {
            var directory = "";
            var blamTagName = "";
            bool convertAll = false;

            if (args.Count == 2)
            {
                blamTagName = args[0];
                directory = args[1];
            }
            else if (args.Count == 1)
            {
                blamTagName = args[0];
                directory = "Bitmaps";
            }
            else
                return false;

            if (args[0].ToLower() == "all")
                convertAll = true;

            //
            // Verify input
            //

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

            if (!convertAll)
            {
                CacheFile.IndexItem blamTag = null;

                foreach (var tag in BlamCache.IndexItems)
                {
                    if ((tag.GroupTag == "bitm") && (tag.Name == blamTagName))
                    {
                        blamTag = tag;
                        break;
                    }
                }

                if (blamTag == null)
                {
                    Console.WriteLine($"ERROR: Blam tag does not exist: {blamTagName}.bitmap");
                    return true;
                }

                ExtractBitmap(blamTag, directory);
            }
            else
            {
                foreach (var tag in BlamCache.IndexItems)
                {
                    if ((tag.GroupTag == "bitm"))
                    {
                        ExtractBitmap(tag, directory);
                    }
                }
            }

            Console.WriteLine("done.");

            return true;
        }

        private BaseBitmap ExtractBitmapData(Bitmap bitmap, int index)
        {
            return BitmapConverter.ConvertGen3Bitmap(BlamCache, bitmap, index, BlamCache.Version);
        }

        private void ExtractBitmap(CacheFile.IndexItem blamTag, string directory)
        {
            Console.WriteLine($"{blamTag.Name}");
            //
            // Load the Blam tag definition
            //

            var blamContext = new CacheSerializationContext(ref BlamCache, blamTag);

            var bitmap = BlamCache.Deserializer.Deserialize<Bitmap>(blamContext);


            var ddsOutDir = directory;
            string bitmap_name = blamTag.Name.Replace('\\', '_');
            if (bitmap.Images.Count > 1)
            {
                ddsOutDir = Path.Combine(directory, bitmap_name);
                Directory.CreateDirectory(ddsOutDir);
            }

            for (var i = 0; i < bitmap.Images.Count; i++)
            {
                var outPath = Path.Combine(ddsOutDir, ((bitmap.Images.Count > 1) ? i.ToString() : bitmap_name) + ".dds");
                var image = bitmap.Images[i];
                //
                // Get bitmap data and write file
                //

                BaseBitmap baseBitmap = ExtractBitmapData(bitmap, i);

                // Bitmap is not valid (not a failure to convert, tag data is not valid / no data to convert
                if (baseBitmap == null)
                    return;

                var header = new DDSHeader(baseBitmap);


                using (var outStream = File.Open(outPath, FileMode.Create, FileAccess.Write))
                {
                    header.Write(new EndianWriter(outStream));
                    var dataStream = new MemoryStream(baseBitmap.Data);
                    StreamUtil.Copy(dataStream, outStream, baseBitmap.Data.Length);
                }
            }
        }
    }
}
