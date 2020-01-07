using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Bitmaps.Utils
{
    public static class BitmapConverterNew
    {
        public static BaseBitmap ConvertGen3Bitmap(GameCache cache, Bitmap bitmap, int imageIndex)
        {
            var image = bitmap.Images[imageIndex];

            if (image.XboxFlags.HasFlag(BitmapFlagsXbox.UseInterleavedTextures))
                return ConvertGen3InterleavedBitmap(cache, bitmap, imageIndex);
            else
                return ConvertGen3RegularBitmap(cache, bitmap, imageIndex);
        }

        private static BaseBitmap ConvertGen3InterleavedBitmap(GameCache cache, Bitmap bitmap, int imageIndex)
        {
            var image = bitmap.Images[imageIndex];

            var resourceReference = bitmap.InterleavedResources[image.InterleavedTextureIndex1];
            var resourceDefinition = cache.ResourceCache.GetBitmapTextureInterleavedInteropResource(resourceReference);
            BitmapTextureInteropDefinition bitmapTextureInteropDef = null;

            if(image.InterleavedTextureIndex2 == 0)
                bitmapTextureInteropDef = resourceDefinition.Texture.Definition.Bitmap1;
            else
                bitmapTextureInteropDef = resourceDefinition.Texture.Definition.Bitmap2;

            return null;
        }

        private static BaseBitmap ConvertGen3RegularBitmap(GameCache cache, Bitmap bitmap, int imageIndex)
        {
            var image = bitmap.Images[imageIndex];

            var resourceReference = bitmap.InterleavedResources[image.InterleavedTextureIndex1];
            var resourceDefinition = cache.ResourceCache.GetBitmapTextureInteropResource(resourceReference);
            BitmapTextureInteropDefinition bitmapTextureInteropDef = resourceDefinition.Texture.Definition.Bitmap;



            return null;
        }

    }
}
