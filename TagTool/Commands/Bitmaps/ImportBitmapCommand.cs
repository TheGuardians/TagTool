using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TagTool.Bitmaps;
using TagTool.Tags.Definitions;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Bitmaps.DDS;
using TagTool.IO;

namespace TagTool.Commands.Bitmaps
{
    class ImportBitmapCommand : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private Bitmap Bitmap { get; }

        public ImportBitmapCommand(GameCache cache, CachedTag tag, Bitmap bitmap)
            : base(false,

                  "ImportBitmap",
                  "Imports an image from a DDS file.",

                  "ImportBitmap <image index> <dds file> [curve mode]",

                  "The image index must be in hexadecimal.\n" +
                  "No conversion will be done on the data in the DDS file.\n" +
                  "The pixel format must be supported by the game.")
        {
            Cache = cache;
            Tag = tag;
            Bitmap = bitmap;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 3 || args.Count < 2)
                return false;

            if (!int.TryParse(args[0], NumberStyles.HexNumber, null, out int imageIndex))
                return false;

            if (Bitmap.Images.Count == 0)
            {
                Bitmap.Flags = BitmapRuntimeFlags.UsingTagInteropAndTagResource;
                Bitmap.Images.Add(new Bitmap.Image { Signature = new Tag("bitm") });
                Bitmap.Resources.Add(new TagResourceReference());
            }

            if (imageIndex < 0 || imageIndex >= Bitmap.Images.Count)
            {
                Console.Error.WriteLine("Invalid image index.");
                return true;
            }

            var imagePath = args[1];

            BitmapImageCurve curve = BitmapImageCurve.xRGB;
            string inputCurve = null;
            if (args.Count == 3)
                inputCurve = args[2];

            if (inputCurve != null)
            {
                switch (inputCurve)
                {
                    case "linear":
                        curve = BitmapImageCurve.Linear;
                        break;
                    case "sRGB":
                    case "srgb":
                        curve = BitmapImageCurve.sRGB;
                        break;
                    case "gamma2":
                        curve = BitmapImageCurve.Gamma2;
                        break;
                    case "xRGB":
                    case "xrgb":
                        curve = BitmapImageCurve.xRGB;
                        break;
                    default:
                        Console.WriteLine($"Invalid bitmap curve {inputCurve}, using xRGB instead");
                        break;
                }
            }

            Console.WriteLine("Importing image data...");

#if !DEBUG
            try
            {
#endif
            DDSFile file = new DDSFile();

                using (var imageStream = File.OpenRead(imagePath))
                using(var reader = new EndianReader(imageStream))
                {
                    file.Read(reader);
                }

                var bitmapTextureInteropDefinition = BitmapInjector.CreateBitmapResourceFromDDS(Cache, file, curve);
                var reference = Cache.ResourceCache.CreateBitmapResource(bitmapTextureInteropDefinition);

                // set the tag data

                Bitmap.Resources[imageIndex] = reference;
                Bitmap.Images[imageIndex] = BitmapUtils.CreateBitmapImageFromResourceDefinition(bitmapTextureInteropDefinition.Texture.Definition.Bitmap);

                using (var tagsStream = Cache.OpenCacheReadWrite())
                    Cache.Serialize(tagsStream, Tag, Bitmap);
#if !DEBUG
            }
            catch (Exception ex)
            {
                Console.WriteLine("Importing image data failed: " + ex.Message);
                return true;
            }
#endif


            Console.WriteLine("Done!");

            return true;
        }
    }
}
