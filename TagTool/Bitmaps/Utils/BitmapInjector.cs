using System;
using TagTool.Bitmaps.DDS;
using TagTool.Cache;
using TagTool.Tags;
using TagTool.Tags.Resources;

namespace TagTool.Bitmaps
{
    public class BitmapInjector
    {
        public static BitmapTextureInteropResourceTest CreateBitmapResourceFromDDS(GameCache cache, DDSFile file)
        {
            BitmapTextureInteropResourceTest result = new BitmapTextureInteropResourceTest
            {
                Texture = new D3DStructure<BitmapTextureInteropResourceTest.BitmapDefinition>
                {
                    Definition = new BitmapTextureInteropResourceTest.BitmapDefinition()
                }
            };

            if(cache.GetType() == typeof(GameCacheContextHaloOnline))
            {
                result.Texture.Definition.PrimaryResourceData = new TagData(file.BitmapData);
                result.Texture.Definition.Bitmap = BitmapUtils.CreateBitmapTextureInteropDefinition(file.Header);
            }
            else if(cache.GetType() == typeof(GameCacheContextGen3))
            {
                // need to do some serious conversion, might be better to require an uncompressed input
                throw new NotImplementedException();
            }
            return result;
        }
    }
}
