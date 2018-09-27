using TagTool.Bitmaps;

namespace TagTool.Tags.Resources
{
    /// <summary>
    /// Resource definition data for bitmap textures.
    /// </summary>
    [TagStructure(Name = "bitmap_texture_interop_resource", Size = 0xC)]
    public class BitmapTextureInteropResource : TagStructure
	{
        /// <summary>
        /// The texture object.
        /// </summary>
        public TagStructureReference<BitmapDefinition> Texture;

        /// <summary>
        /// Describes a bitmap.
        /// </summary>
        [TagStructure(Size = 0x40)]
        public class BitmapDefinition : TagStructure
		{
            /// <summary>
            /// The reference to the bitmap data.
            /// </summary>
            public TagData Data;

            /// <summary>
            /// The reference to the unknown data.
            /// </summary>
            public TagData UnknownData;

            /// <summary>
            /// The bitmap's width in pixels.
            /// </summary>
            public short Width;

            /// <summary>
            /// The bitmap's height in pixels.
            /// </summary>
            public short Height;

            /// <summary>
            /// The bitmap's depth.
            /// Only used for <see cref="BitmapType.Texture3D"/> textures.
            /// </summary>
            public sbyte Depth;
            
            /// <summary>
            /// The number of mip levels in the bitmap. (1 = full size only)
            /// </summary>
            public sbyte MipmapCount;

            /// <summary>
            /// The bitmap's type.
            /// </summary>
            public BitmapType Type;

            public sbyte Unused2F;

            /// <summary>
            /// The format of the bitmap as a D3DFORMAT enum.
            /// Note that this is actually unused and the game reads the format from <see cref="Format"/>.
            /// Setting this value is still suggested however.
            /// </summary>
            public int D3DFormatUnused;

            /// <summary>
            /// The format of the bitmap data.
            /// </summary>
            public BitmapFormat Format;

            // Some sort of enum? No idea what this does but it IS used for something.
            public BitmapImageCurve Curve;

            /// <summary>
            /// Gets or sets flags describing the bitmap.
            /// </summary>
            public BitmapFlags Flags;

            public int Unused38;
            public int Unused3C;
        }
    }
}