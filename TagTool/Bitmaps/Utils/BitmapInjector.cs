using System;
using TagTool.Bitmaps.DDS;
using TagTool.Cache;
using TagTool.Tags;
using TagTool.Tags.Resources;

namespace TagTool.Bitmaps
{
    public class BitmapInjector
    {
        public static BitmapTextureInteropResource CreateBitmapResourceFromDDS(GameCache cache, DDSFile file)
        {
            BitmapTextureInteropResource result = BitmapUtils.CreateEmptyBitmapTextureInteropResource();

            if (cache is GameCacheHaloOnlineBase)
            {
                // TODO: for cubemaps, fix mipmap order to d3d9 expected order
                result.Texture.Definition.PrimaryResourceData = new TagData(file.BitmapData);
                result.Texture.Definition.Bitmap = BitmapUtils.CreateBitmapTextureInteropDefinition(file.Header);
            }
            else if(cache.GetType() == typeof(GameCacheGen3))
            {
                // need to do some serious conversion, might be better to require an uncompressed input
                throw new NotImplementedException();
            }
            return result;
        }
    }
}
