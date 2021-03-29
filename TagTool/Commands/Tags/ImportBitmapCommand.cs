using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TagTool.Bitmaps;
using TagTool.Tags.Definitions;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Bitmaps.DDS;
using TagTool.IO;

namespace TagTool.Commands.Tags
{
    class ImportBitmapCommand : Command
    {
        public GameCache Cache { get; }

        public ImportBitmapCommand(GameCache cache)
            : base(false,

                  "ImportBitmap",
                  "Imports an image from a DDS file.",

                  "ImportBitmap <image index> <tagname> <dds file> [curve mode]",

                  "The image index must be in hexadecimal.\n" +
                  "No conversion will be done on the data in the DDS file.\n" +
                  "The pixel format must be supported by the game.")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 4 || args.Count < 3)
                return new TagToolError(CommandError.ArgCount);

            if (!int.TryParse(args[0], NumberStyles.HexNumber, null, out int imageIndex))
                return new TagToolError(CommandError.ArgInvalid, $"\"{args[0]}\"");

            if (!args[1].Contains(".bitmap"))
                args[1] += ".bitmap";

            if (!Cache.TagCache.TryGetCachedTag(args[1], out var tag))
                return new TagToolError(CommandError.TagInvalid);

            using (var stream = Cache.OpenCacheReadWrite())
            {
                var bitmap = Cache.Deserialize<Bitmap>(stream, tag);

                if (bitmap.Images.Count == 0)
                {
                    bitmap.Flags = BitmapRuntimeFlags.UsingTagInteropAndTagResource;
                    bitmap.Images.Add(new Bitmap.Image { Signature = new Tag("bitm") });
                    bitmap.Resources.Add(new TagResourceReference());
                }

                if (imageIndex < 0 || imageIndex >= bitmap.Images.Count)
                    return new TagToolError(CommandError.ArgInvalid, "Invalid image index");

                var imagePath = args[2];
                if (!File.Exists(imagePath))
                    return new TagToolError(CommandError.FileNotFound, $"\"{imagePath}\"");

                BitmapImageCurve curve = BitmapImageCurve.xRGB;
                string inputCurve = null;
                if (args.Count == 4)
                    inputCurve = args[3];

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
                    using (var reader = new EndianReader(imageStream))
                    {
                        file.Read(reader);
                    }

                    var bitmapTextureInteropDefinition = BitmapInjector.CreateBitmapResourceFromDDS(Cache, file, curve);
                    var reference = Cache.ResourceCache.CreateBitmapResource(bitmapTextureInteropDefinition);

                    // set the tag data

                    bitmap.Resources[imageIndex] = reference;
                    bitmap.Images[imageIndex] = BitmapUtils.CreateBitmapImageFromResourceDefinition(bitmapTextureInteropDefinition.Texture.Definition.Bitmap);

                    Cache.Serialize(stream, tag, bitmap);
#if !DEBUG
                }
                catch (Exception ex)
                {
                    return new TagToolError(CommandError.OperationFailed, "Importing image data failed: " + ex.Message);
                }
#endif


                Console.WriteLine("Done!");
            }

            return true;
        }
    }
}
