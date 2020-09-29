using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "bitmap", Tag = "bitm", Size = 0x70)]
    public class Bitmap : TagStructure
    {
        /// <summary>
        /// Type
        /// </summary>
        /// <remarks>
        /// Type controls bitmap geometry. All dimensions must be a power of 2 except for SPRITES and INTERFACE BITMAPS:
        /// 
        /// * 2D TEXTURES: Ordinary 2D textures will be generated.
        /// * 3D TEXTURES: Volume textures will be generated from each sequence of 2D texture slices.
        /// * CUBE MAPS: Generated from each consecutive set of six 2D textures in each sequence, all faces of a cube map must be square and the same size.
        /// * SPRITES: Sprite texture pages will be generated.
        /// * INTERFACE BITMAPS: Similar to 2D TEXTURES but without mipmaps and without the power of 2 restriction.
        /// </remarks>
        public TypeValue Type;
        /// <summary>
        /// Format
        /// </summary>
        /// <remarks>
        /// Format controls how pixels will be stored internally:
        /// 
        /// * COMPRESSED WITH COLOR-KEY TRANSPARENCY: DXT1 compression, using 4 bits/pixel. 4-x-4 blocks of pixels, are reduced to two colors and interpolated, alpha channel uses color-key transparency instead of alpha from the plate (all zero-alpha pixels also have zero-color).
        /// * COMPRESSED WITH EXPLICIT ALPHA: DXT2/3 compression, using 8 bits/pixel. Same as DXT1 without the color key transparency, alpha channel uses alpha from plate quantized down to 4 bits/pixel.
        /// * COMPRESSED WITH INTERPOLATED ALPHA: DXT4/5 compression, using 8 bits/pixel. Same as DXT2/3, except alpha is smoother. Better for smooth alpha gradients, but worse for noisy alpha.
        /// * 16-BIT COLOR: Uses 16 bits/pixel. Depending on the alpha channel, bitmaps are quantized to either r5g6b5 (no alpha), a1r5g5b5 (1-bit alpha), or a4r4g4b4 (1-bit alpha).
        /// * 32-BIT COLOR: Uses 32 bits/pixel. Very high quality and can have alpha at no added cost. This format takes up the most memory, however. Bitmap formats are x8r8g8b8 and a8r8g8b.
        /// * MONOCHROME: Uses either 8 or 16 bits/pixel. Bitmap formats are a8 (alpha), y8 (intensity), ay8 (combined alpha intensity), and a8y8 (separate alpha intensity).
        /// </remarks>
        public FormatValue Format;
        /// <summary>
        /// Usage
        /// </summary>
        /// <remarks>
        /// Usage controls how mipmaps are generated:
        /// 
        /// * ALPHA BLEND: Pixels with zero alpha are ignored in mipmaps, to prevent bleeding the transparent color.
        /// * DEFAULT: Downsampling works normally, as in Photoshop.
        /// * HEIGHT MAP: The bitmap (normally grayscale) is a height map that gets converted to a bump map. Uses bump height below. Alpha is passed through unmodified.
        /// * DETAIL MAP: Mipmap color fades to gray, controlled by detail fade factor below. Alpha fades to white.
        /// * LIGHT MAP: Generates no mipmaps. Do not use!
        /// * VECTOR MAP: Used mostly for special effects; pixels are treated as xyz vectors and normalized after downsampling. Alpha is passed through unmodified.
        /// </remarks>
        public UsageValue Usage;
        public FlagsValue Flags;
        /// <summary>
        /// Postprocessing
        /// </summary>
        /// <remarks>
        /// These properties control how mipmaps are postprocessed.
        /// </remarks>
        public float DetailFadeFactor; // [0,1]
        public float SharpenAmount; // [0,1]
        public float BumpHeight; // repeats
        public Unknown2Value Unknown2;
        public short Unknown3;
        /// <summary>
        /// Color Plate
        /// </summary>
        /// <remarks>
        /// The original image file used to import the bitmap group.
        /// </remarks>
        public short ColorPlateWidth; // pixels
        public short ColorPlateHeight; // pixels
        public byte[] CompressedColorPlateData;
        /// <summary>
        /// Processed Pixel Data
        /// </summary>
        /// <remarks>
        /// Pixel data after being processed by the tool.
        /// </remarks>
        public byte[] ProcessedPixelData;
        /// <summary>
        /// Miscellaneous
        /// </summary>
        public float BlurFilterSize; // [0,10] pixels
        public float AlphaBias; // [-1,1]
        public short MipmapCount; // levels
        /// <summary>
        /// ...More Sprite Processing
        /// </summary>
        /// <remarks>
        /// Sprite usage controls the background color of sprite plates.
        /// </remarks>
        public SpriteUsageValue SpriteUsage;
        public short SpriteSpacing;
        /// <summary>
        /// ...More Stuff
        /// </summary>
        public ForceFormatValue ForceFormat;
        public List<BitmapGroupSequence> Sequences;
        public List<BitmapData> Bitmaps;
        /// <summary>
        /// WDP fields
        /// </summary>
        public sbyte ColorCompressionQuality; // [1,127]
        public sbyte AlphaCompressionQuality; // [1,127]
        public sbyte Overlap;
        public ColorSubsamplingValue ColorSubsampling;
        
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
            VectorMap,
            HeightMapBlue255,
            Embm,
            HeightMapA8l8,
            HeightMapG8b8,
            HeightMapG8b8WAlpha
        }
        
        [Flags]
        public enum FlagsValue : ushort
        {
            EnableDiffusionDithering = 1 << 0,
            DisableHeightMapCompression = 1 << 1,
            UniformSpriteSequences = 1 << 2,
            FilthySpriteBugFix = 1 << 3,
            UseSharpBumpFilter = 1 << 4,
            Unused = 1 << 5,
            UseClampedMirroredBumpFilter = 1 << 6,
            InvertDetailFade = 1 << 7,
            SwapXYVectorComponents = 1 << 8,
            ConvertFromSigned = 1 << 9,
            ConvertToSigned = 1 << 10,
            ImportMipmapChains = 1 << 11,
            IntentionallyTrueColor = 1 << 12
        }
        
        public enum Unknown2Value : short
        {
            _32X32,
            _64X64,
            _128X128,
            _256X256,
            _512X512,
            _1024X1024
        }
        
        public enum SpriteUsageValue : short
        {
            BlendAddSubtractMax,
            MultiplyMin,
            DoubleMultiply
        }
        
        public enum ForceFormatValue : short
        {
            Default,
            ForceG8b8,
            ForceDxt1,
            ForceDxt3,
            ForceDxt5,
            ForceAlphaLuminance8,
            ForceA4r4g4b4
        }
        
        [TagStructure(Size = 0x40)]
        public class BitmapGroupSequence : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public short FirstBitmapIndex;
            public short BitmapCount;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding1;
            public List<BitmapGroupSprite> Sprites;
            
            [TagStructure(Size = 0x20)]
            public class BitmapGroupSprite : TagStructure
            {
                public short BitmapIndex;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding2;
                public float Left;
                public float Right;
                public float Top;
                public float Bottom;
                public RealPoint2d RegistrationPoint;
            }
        }
        
        [TagStructure(Size = 0x74)]
        public class BitmapData : TagStructure
        {
            public Tag Signature;
            public short Width; // pixels
            public short Height; // pixels
            public sbyte Depth; // pixels
            public MoreFlagsValue MoreFlags;
            public TypeValue Type; // Determines bitmap "geometry."
            public FormatValue Format; // Determines how pixels are represented internally.
            public FlagsValue Flags;
            public Point2d RegistrationPoint;
            public short MipmapCount;
            public short LowDetailMipmapCount;
            public int PixelsOffset;
            [TagField(Flags = Padding, Length = 12)]
            public byte[] Unknown1;
            [TagField(Flags = Padding, Length = 12)]
            public byte[] Unknown2;
            [TagField(Flags = Padding, Length = 12)]
            public byte[] Unknown3;
            [TagField(Flags = Padding, Length = 12)]
            public byte[] Unknown4;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown5;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown6;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown7;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown8;
            [TagField(Flags = Padding, Length = 20)]
            public byte[] Unknown9;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown10;
            
            [Flags]
            public enum MoreFlagsValue : byte
            {
                DeleteFromCacheFile = 1 << 0,
                BitmapCreateAttempted = 1 << 1,
                Bit2 = 1 << 2
            }
            
            public enum TypeValue : short
            {
                _2dTexture,
                _3dTexture,
                CubeMap
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
                P8Bump,
                P8,
                Argbfp32,
                Rgbfp32,
                Rgbfp16,
                V8u8,
                G8b8
            }
            
            [Flags]
            public enum FlagsValue : ushort
            {
                PowerOfTwoDimensions = 1 << 0,
                Compressed = 1 << 1,
                Palettized = 1 << 2,
                Swizzled = 1 << 3,
                Linear = 1 << 4,
                V16u16 = 1 << 5,
                MipMapDebugLevel = 1 << 6,
                PreferStutterPreferLowDetail = 1 << 7
            }
        }
        
        public enum ColorSubsamplingValue : sbyte
        {
            _400,
            _420,
            _422,
            _444
        }
    }
}

