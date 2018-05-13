using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TagTool.Bitmaps;
using TagTool.Tags.Definitions;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Commands;

namespace TagTool.Commands.Bitmaps
{
    class ImportBitmapCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private Bitmap Bitmap { get; }

        public ImportBitmapCommand(HaloOnlineCacheContext cacheContext, CachedTagInstance tag, Bitmap bitmap)
            : base(CommandFlags.None,

                  "ImportBitmap",
                  "Imports an image from a DDS file.",

                  "ImportBitmap [location = textures] <image index> <dds file>",

                  "The image index must be in hexadecimal.\n" +
                  "No conversion will be done on the data in the DDS file.\n" +
                  "The pixel format must be supported by the game.")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Bitmap = bitmap;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 2 || args.Count > 3)
                return false;

            var location = ResourceLocation.Textures;

            if (args.Count == 3)
            {
                switch (args[0])
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

                    case "render_models":
                        location = ResourceLocation.RenderModels;
                        break;

                    case "lightmaps":
                        location = ResourceLocation.Lightmaps;
                        break;

                    default:
                        Console.WriteLine($"Invalid resource location: {args[0]}");
                        return false;
                }

                args.RemoveAt(0);
            }

            if (!int.TryParse(args[0], NumberStyles.HexNumber, null, out int imageIndex))
                return false;

            if (Bitmap.Images.Count == 0)
            {
                Bitmap.Flags = BitmapRuntimeFlags.UsingTagInteropAndTagResource;
                Bitmap.Images.Add(new Bitmap.Image { Signature = new Tag("bitm") });
                Bitmap.Resources.Add(new Bitmap.BitmapResource());
            }

            if (imageIndex < 0 || imageIndex >= Bitmap.Images.Count)
            {
                Console.Error.WriteLine("Invalid image index.");
                return true;
            }

            var imagePath = args[1];
            
            Console.WriteLine("Importing image data...");
            
            try
            {
                using (var imageStream = File.OpenRead(imagePath))
                {
                    var injector = new BitmapDdsInjector(CacheContext);
                    injector.InjectDds(CacheContext.Serializer, CacheContext.Deserializer, Bitmap, imageIndex, imageStream, location);
                }

                using (var tagsStream = CacheContext.OpenTagCacheReadWrite())
                {
                    var tagContext = new TagSerializationContext(tagsStream, CacheContext, Tag);
                    CacheContext.Serializer.Serialize(tagContext, Bitmap);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Importing image data failed: " + ex.Message);
                return true;
            }

            Console.WriteLine("Done!");

            return true;
        }
    }
}
