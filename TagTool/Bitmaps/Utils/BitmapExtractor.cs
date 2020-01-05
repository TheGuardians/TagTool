using System;
using System.IO;
using TagTool.Cache;
using TagTool.IO;
using TagTool.Tags.Definitions;
using TagTool.Bitmaps.DDS;

namespace TagTool.Bitmaps
{
    public class BitmapExtractor
    {
        private GameCache Cache { get; }

        public BitmapExtractor(GameCache cache)
        {
            Cache = cache;
        }

        public byte[] ExtractBitmapData(Bitmap bitmap, int imageIndex)
        {
            if(Cache.GetType() == typeof(GameCacheContextHaloOnline))
            {
                var resourceReference = bitmap.Resources[imageIndex];
                var resourceDefinition = Cache.ResourceCache.GetBitmapTextureInteropResource(resourceReference);
                if(resourceDefinition != null)
                {
                    return resourceDefinition.Texture.Definition.PrimaryResourceData.Data;
                }
                else
                {
                    Console.Error.WriteLine("No resource associated to this bitmap.");
                    return null;
                }
            }
            else
            {
                // need to implement a converter with the new cache code
                throw new NotImplementedException();
            }
        }

        public DDSFile ExtractBitmap(Bitmap bitmap, int imageIndex)
        {
            byte[] data = ExtractBitmapData(bitmap, imageIndex);
            DDSHeader header = new DDSHeader(bitmap.Images[imageIndex]);
            return new DDSFile(header, data);
        }

        public byte[] ExtractBitmapToDDSArray(Bitmap bitmap, int imageIndex)
        {
            var ddsFile = ExtractBitmap(bitmap, imageIndex);
            var stream = new MemoryStream();
            using(var writer = new EndianWriter(stream))
            {
                ddsFile.Write(writer);
            }
            var result = stream.ToArray();
            stream.Dispose();
            return result;
        }
    }
}
