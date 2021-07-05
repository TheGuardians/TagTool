using TagTool.Bitmaps;

namespace TagTool.Tags.Resources
{
    /// <summary>
    /// Resource definition data for bitmap textures.
    /// </summary>
    [TagStructure(Name = "bitmap_texture_interleaved_interop_resource", Size = 0xC)]
    public class BitmapTextureInterleavedInteropResource : TagStructure
    {
        /// <summary>
        /// The texture object.
        /// </summary>
        public D3DStructure<BitmapInterleavedDefinition> Texture;

        /// <summary>
        /// Describes a bitmap.
        /// </summary>
        [TagStructure(Size = 0x40, MaxVersion = Cache.CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x58, MinVersion = Cache.CacheVersion.HaloOnlineED)]
        public class BitmapInterleavedDefinition : TagStructure
        {
            public TagData PrimaryResourceData;
            public TagData SecondaryResourceData;
            public BitmapTextureInteropDefinition Bitmap1;
            public BitmapTextureInteropDefinition Bitmap2;
        }
    }
}