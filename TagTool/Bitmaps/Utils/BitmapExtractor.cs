using System;
using System.IO;
using TagTool.Cache;
using TagTool.IO;
using TagTool.Tags.Definitions;
using TagTool.Bitmaps.DDS;
using TagTool.Bitmaps.Utils;

namespace TagTool.Bitmaps
{
    public static class BitmapExtractor
    {
        public static byte[] ExtractBitmapData(GameCache cache, Bitmap bitmap, int imageIndex)
        {
            var resourceReference = bitmap.Resources[imageIndex];
            var resourceDefinition = cache.ResourceCache.GetBitmapTextureInteropResource(resourceReference);
            if (cache.GetType() == typeof(GameCacheContextHaloOnline))
            {
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
            else if(cache.GetType() == typeof(GameCacheContextGen3))
            {
                if (resourceDefinition != null)
                {
                    var bitmapTextureInteropDefinition = resourceDefinition.Texture.Definition.Bitmap;

                    if(bitmapTextureInteropDefinition.HighResInSecondaryResource == 1)
                    {
                        var result = new byte[resourceDefinition.Texture.Definition.PrimaryResourceData.Data.Length + resourceDefinition.Texture.Definition.SecondaryResourceData.Data.Length];
                        Array.Copy(resourceDefinition.Texture.Definition.PrimaryResourceData.Data, 0, result, 0, resourceDefinition.Texture.Definition.PrimaryResourceData.Data.Length);
                        Array.Copy(resourceDefinition.Texture.Definition.SecondaryResourceData.Data, 0, result, resourceDefinition.Texture.Definition.PrimaryResourceData.Data.Length, resourceDefinition.Texture.Definition.SecondaryResourceData.Data.Length);
                        return result;
                    }
                    else
                    {
                        return resourceDefinition.Texture.Definition.PrimaryResourceData.Data;
                    }
                }
                else
                {
                    Console.Error.WriteLine("No resource associated to this bitmap.");
                    return null;
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static DDSFile ExtractBitmap(GameCache cache, Bitmap bitmap, int imageIndex)
        {
            if (cache.GetType() == typeof(GameCacheContextHaloOnline))
            {
                byte[] data = ExtractBitmapData(cache, bitmap, imageIndex);
                DDSHeader header = new DDSHeader(bitmap.Images[imageIndex]);
                return new DDSFile(header, data);
            }
            else if (cache.GetType() == typeof(GameCacheContextGen3))
            {
                var baseBitmap = BitmapConverterNew.ConvertGen3Bitmap(cache, bitmap, imageIndex);
                if (baseBitmap == null)
                    return null;
                return new DDSFile(baseBitmap);
            }
            else
                return null;
            
        }

        public static byte[] ExtractBitmapToDDSArray(GameCache cache, Bitmap bitmap, int imageIndex)
        {
            var ddsFile = ExtractBitmap(cache, bitmap, imageIndex);
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
