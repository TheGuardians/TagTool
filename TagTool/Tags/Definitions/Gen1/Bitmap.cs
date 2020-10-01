using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "bitmap", Tag = "bitm", Size = 0x6C)]
    public class Bitmap : TagStructure
    {
        /// <summary>
        /// Type controls bitmap 'geometry'. All dimensions must be a power of two except for SPRITES and INTERFACE BITMAPS:
        /// 
        /// * 2D
        /// TEXTURES: Ordinary, 2D textures will be generated.
        /// * 3D TEXTURES: Volume textures will be generated from each sequence of
        /// 2D texture 'slices'.
        /// * CUBE MAPS: Cube maps will be generated from each consecutive set of six 2D textures in each
        /// sequence, all faces of a cube map must be square and the same size.
        /// * SPRITES: Sprite texture pages will be generated.
        /// *
        /// INTERFACE BITMAPS: Similar to 2D TEXTURES, but without mipmaps and without the power of two restriction.
        /// </summary>
        public TypeValue Type;
        /// <summary>
        /// Format controls how pixels will be stored internally:
        /// 
        /// * COMPRESSED WITH COLOR-KEY TRANSPARENCY: DXT1 compression, uses 4
        /// bits per pixel. 4x4 blocks of pixels are reduced to 2 colors and interpolated, alpha channel uses color-key transparency
        /// instead of alpha from the plate (all zero-alpha pixels also have zero-color).
        /// * COMPRESSED WITH EXPLICIT ALPHA: DXT2/3
        /// compression, uses 8 bits per pixel. Same as DXT1 without the color key transparency, alpha channel uses alpha from plate
        /// quantized down to 4 bits per pixel.
        /// * COMPRESSED WITH INTERPOLATED ALPHA: DXT4/5 compression, uses 8 bits per pixel. Same
        /// as DXT2/3, except alpha is smoother. Better for smooth alpha gradients, worse for noisy alpha.
        /// * 16-BIT COLOR: Uses 16
        /// bits per pixel. Depending on the alpha channel, bitmaps are quantized to either r5g6b5 (no alpha), a1r5g5b5 (1-bit
        /// alpha), or a4r4g4b4 (1-bit alpha).
        /// * 32-BIT COLOR: Uses 32 bits per pixel. Very high quality, can have alpha at no added
        /// cost. This format takes up the most memory, however. Bitmap formats are x8r8g8b8 and a8r8g8b.
        /// * MONOCHROME: Uses either 8
        /// or 16 bits per pixel. Bitmap formats are a8 (alpha), y8 (intensity), ay8 (combined alpha-intensity) and a8y8 (separate
        /// alpha-intensity).
        /// 
        /// Note: Height maps (a.k.a. bump maps) should use 32-bit color; this is internally converted to a
        /// palettized format which takes less memory.
        /// </summary>
        public FormatValue Format;
        /// <summary>
        /// Usage controls how mipmaps are generated:
        /// 
        /// * ALPHA BLEND: Pixels with zero alpha are ignored in mipmaps, to prevent
        /// bleeding the transparent color.
        /// * DEFAULT: Downsampling works normally, as in Photoshop.
        /// * HEIGHT MAP: The bitmap
        /// (normally grayscale) is a height map which gets converted to a bump map. Uses bump height below. Alpha is passed
        /// through unmodified.
        /// * DETAIL MAP: Mipmap color fades to gray, controlled by detail fade factor below. Alpha fades to
        /// white.
        /// * LIGHT MAP: Generates no mipmaps. Do not use!
        /// * VECTOR MAP: Used mostly for special effects; pixels are treated
        /// as XYZ vectors and normalized after downsampling. Alpha is passed through unmodified.
        /// </summary>
        public UsageValue Usage;
        public FlagsValue Flags;
        /// <summary>
        /// These properties control how mipmaps are post-processed.
        /// </summary>
        /// <summary>
        /// 0 means fade to gray by last mipmap, 1 means fade to gray by first mipmap
        /// </summary>
        public float DetailFadeFactor; // [0,1]
        /// <summary>
        /// sharpens mipmap after downsampling
        /// </summary>
        public float SharpenAmount; // [0,1]
        /// <summary>
        /// the apparent height of the bump map above the triangle it is textured onto, in texture repeats (i.e., 1.0 would be as
        /// high as the texture is wide)
        /// </summary>
        public float BumpHeight; // repeats
        /// <summary>
        /// When creating a sprite group, specify the number and size of textures that the group is allowed to occupy. During
        /// importing, you'll receive feedback about how well the alloted space was used.
        /// </summary>
        public SpriteBudgetSizeValue SpriteBudgetSize;
        public short SpriteBudgetCount;
        /// <summary>
        /// The original TIFF file used to import the bitmap group.
        /// </summary>
        public short ColorPlateWidth; // pixels
        public short ColorPlateHeight; // pixels
        public byte[] CompressedColorPlateData;
        /// <summary>
        /// Pixel data after being processed by the tool.
        /// </summary>
        public byte[] ProcessedPixelData;
        /// <summary>
        /// blurs the bitmap before generating mipmaps
        /// </summary>
        public float BlurFilterSize; // [0,10] pixels
        /// <summary>
        /// affects alpha mipmap generation
        /// </summary>
        public float AlphaBias; // [-1,1]
        /// <summary>
        /// 0 defaults to all levels
        /// </summary>
        public short MipmapCount; // levels
        /// <summary>
        /// Sprite usage controls the background color of sprite plates.
        /// </summary>
        public SpriteUsageValue SpriteUsage;
        public short SpriteSpacing;
        [TagField(Length = 0x2)]
        public byte[] Padding;
        public List<BitmapGroupSequenceBlock> Sequences;
        public List<BitmapDataBlock> Bitmaps;
        
        public enum TypeValue : short
        {
            _2dTextures,
            _3dTextures,
            CubeMaps,
            Sprites,
            InterfaceBitmaps
        }
        
        public enum FormatValue : short
        {
            CompressedWithColorKeyTransparency,
            CompressedWithExplicitAlpha,
            CompressedWithInterpolatedAlpha,
            _16BitColor,
            _32BitColor,
            Monochrome
        }
        
        public enum UsageValue : short
        {
            AlphaBlend,
            Default,
            HeightMap,
            DetailMap,
            LightMap,
            VectorMap
        }
        
        [Flags]
        public enum FlagsValue : ushort
        {
            EnableDiffusionDithering = 1 << 0,
            DisableHeightMapCompression = 1 << 1,
            UniformSpriteSequences = 1 << 2,
            FilthySpriteBugFix = 1 << 3
        }
        
        public enum SpriteBudgetSizeValue : short
        {
            _32x32,
            _64x64,
            _128x128,
            _256x256,
            _512x512
        }
        
        public enum SpriteUsageValue : short
        {
            BlendAddSubtractMax,
            MultiplyMin,
            DoubleMultiply
        }
        
        [TagStructure(Size = 0x40)]
        public class BitmapGroupSequenceBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public short FirstBitmapIndex;
            public short BitmapCount;
            [TagField(Length = 0x10)]
            public byte[] Padding;
            public List<BitmapGroupSpriteBlock> Sprites;
            
            [TagStructure(Size = 0x20)]
            public class BitmapGroupSpriteBlock : TagStructure
            {
                public short BitmapIndex;
                [TagField(Length = 0x2)]
                public byte[] Padding;
                [TagField(Length = 0x4)]
                public byte[] Padding1;
                public float Left;
                public float Right;
                public float Top;
                public float Bottom;
                public RealPoint2d RegistrationPoint;
            }
        }
        
        [TagStructure(Size = 0x30)]
        public class BitmapDataBlock : TagStructure
        {
            public Tag Signature;
            public short Width; // pixels
            public short Height; // pixels
            /// <summary>
            /// depth is 1 for 2D textures and cube maps
            /// </summary>
            public short Depth; // pixels
            /// <summary>
            /// determines bitmap 'geometry'
            /// </summary>
            public TypeValue Type;
            /// <summary>
            /// determines how pixels are represented internally
            /// </summary>
            public FormatValue Format;
            public FlagsValue Flags;
            public Point2d RegistrationPoint;
            public short MipmapCount;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            public int PixelsOffset;
            [TagField(Length = 0x4)]
            public byte[] Padding1;
            [TagField(Length = 0x4)]
            public byte[] Padding2;
            [TagField(Length = 0x4)]
            public byte[] Padding3;
            [TagField(Length = 0x8)]
            public byte[] Padding4;
            
            public enum TypeValue : short
            {
                _2dTexture,
                _3dTexture,
                CubeMap,
                White
            }
            
            public enum FormatValue : short
            {
                A8,
                Y8,
                Ay8,
                A8y8,
                Unused1,
                Unused2,
                R5g6b5,
                Unused3,
                A1r5g5b5,
                A4r4g4b4,
                X8r8g8b8,
                A8r8g8b8,
                Unused4,
                Unused5,
                Dxt1,
                Dxt3,
                Dxt5,
                P8Bump
            }
            
            [Flags]
            public enum FlagsValue : ushort
            {
                PowerOfTwoDimensions = 1 << 0,
                Compressed = 1 << 1,
                Palettized = 1 << 2,
                Swizzled = 1 << 3,
                Linear = 1 << 4,
                V16u16 = 1 << 5
            }
        }
    }
}

