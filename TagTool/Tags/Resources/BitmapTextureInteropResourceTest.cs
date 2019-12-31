using TagTool.Bitmaps;
using TagTool.Cache;

namespace TagTool.Tags.Resources
{
    /// <summary>
    /// Resource definition data for bitmap textures.
    /// </summary>
    [TagStructure(Name = "bitmap_texture_interop_resource", Size = 0xC)]
    public class BitmapTextureInteropResourceTest : TagStructure
	{
        public D3DStructure<BitmapDefinition> Texture;

        [TagStructure(Size = 0x34, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x40, MinVersion = CacheVersion.HaloOnline106708)]
        public class BitmapDefinition : TagStructure
		{
            public TagData PrimaryResourceData;
            public TagData SecondaryResourceData;
            public BitmapTextureInteropDefinition Bitmap;
        }
    }
}