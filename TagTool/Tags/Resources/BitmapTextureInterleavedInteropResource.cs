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
        public TagStructureReference<BitmapDefinition> Texture;

        /// <summary>
        /// Describes a bitmap.
        /// </summary>
        [TagStructure(Size = 0x40, MaxVersion = Cache.CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x58, MinVersion = Cache.CacheVersion.HaloOnline106708)]
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
            public short Width1;

            /// <summary>
            /// The bitmap's height in pixels.
            /// </summary>
            public short Height1;

            /// <summary>
            /// The bitmap's depth.
            /// Only used for <see cref="BitmapType.Texture3D"/> textures.
            /// </summary>
            public sbyte Depth1;

            /// <summary>
            /// The number of mip levels in the bitmap. (1 = full size only)
            /// </summary>
            public sbyte MipmapCount1;

            /// <summary>
            /// The bitmap's type.
            /// </summary>
            public BitmapType Type1;

            public sbyte Unused2F_1;

            /// <summary>
            /// The format of the bitmap as a D3DFORMAT enum.
            /// Note that this is actually unused and the game reads the format from <see cref="Format1"/>.
            /// Setting this value is still suggested however.
            /// </summary>
            public int D3DFormatUnused1;

            /// <summary>
            /// The format of the bitmap data.
            /// </summary>
            [TagField(MinVersion = Cache.CacheVersion.HaloOnline106708)]
            public BitmapFormat Format1;

            // Some sort of enum? No idea what this does but it IS used for something.
            [TagField(MinVersion = Cache.CacheVersion.HaloOnline106708)]
            public BitmapImageCurve Curve1;

            /// <summary>
            /// Gets or sets flags describing the bitmap.
            /// </summary>
            [TagField(MinVersion = Cache.CacheVersion.HaloOnline106708)]
            public BitmapFlags Flags1;

            [TagField(MinVersion = Cache.CacheVersion.HaloOnline106708)]
            public int Unused38_1;
            [TagField(MinVersion = Cache.CacheVersion.HaloOnline106708)]
            public int Unused3C_1;

            /// <summary>
            /// The bitmap's width in pixels.
            /// </summary>
            public short Width2;

            /// <summary>
            /// The bitmap's height in pixels.
            /// </summary>
            public short Height2;

            /// <summary>
            /// The bitmap's depth.
            /// Only used for <see cref="BitmapType.Texture3D"/> textures.
            /// </summary>
            public sbyte Depth2;

            /// <summary>
            /// The number of mip levels in the bitmap. (1 = full size only)
            /// </summary>
            public sbyte MipmapCount2;

            /// <summary>
            /// The bitmap's type.
            /// </summary>
            public BitmapType Type2;

            public sbyte Unused2F_2;

            /// <summary>
            /// The format of the bitmap as a D3DFORMAT enum.
            /// Note that this is actually unused and the game reads the format from <see cref="Format2"/>.
            /// Setting this value is still suggested however.
            /// </summary>
            public int D3DFormatUnused2;

            /// <summary>
            /// The format of the bitmap data.
            /// </summary>
            [TagField(MinVersion = Cache.CacheVersion.HaloOnline106708)]
            public BitmapFormat Format2;

            // Some sort of enum? No idea what this does but it IS used for something.
            [TagField(MinVersion = Cache.CacheVersion.HaloOnline106708)]
            public BitmapImageCurve Curve2;

            /// <summary>
            /// Gets or sets flags describing the bitmap.
            /// </summary>
            [TagField(MinVersion = Cache.CacheVersion.HaloOnline106708)]
            public BitmapFlags Flags2;

            [TagField(MinVersion = Cache.CacheVersion.HaloOnline106708)]
            public int Unused38_2;
            [TagField(MinVersion = Cache.CacheVersion.HaloOnline106708)]
            public int Unused3C_2;
        }
    }
}