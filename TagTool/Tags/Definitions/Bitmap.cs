using TagTool.Bitmaps;
using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "bitmap", Tag = "bitm", Size = 0xA4, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST)]
	[TagStructure(Name = "bitmap", Tag = "bitm", Size = 0xB8, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline106708)]
    [TagStructure(Name = "bitmap", Tag = "bitm", Size = 0xAC, MinVersion = CacheVersion.HaloOnline235640, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "bitmap", Tag = "bitm", Size = 0xC0, MinVersion = CacheVersion.HaloReach)]
    public class Bitmap : TagStructure
	{
        /// <summary>
        /// Choose how you are using this bitmap
        /// </summary>
        public int Usage;

        /// <summary>
        /// The runtime flags of this bitmap
        /// </summary>
        public BitmapRuntimeFlags Flags;

        /// <summary>
        /// Number of pixels between adjacent sprites (0 uses default, negative numbers set no spacing)
        /// </summary>
        public short SpriteSpacing;

        /// <summary>
        /// The apparent height of the bump map above the triangle it is textured onto, in texture repeats
        /// (i.e., 1.0 would be as high as the texture is wide)
        /// </summary>
        [TagField(Format = "Repeats")]
        public float BumpMapHeight;

        /// <summary>
        /// Used by detail maps and illum maps. 0 means fade by last mipmap, 1 means fade by first mipmap
        /// </summary>
        [TagField(Flags = Fraction, Format = "[0,1]")]
        public float FadeFactor;

        /// <summary>
        /// How much to blur the input image
        /// </summary>
        [TagField(Format = "Pixels", MinVersion = CacheVersion.HaloReach)]
        public float Blur;

        /// <summary>
        /// How much to blur as each mip level is being downsampled
        /// </summary>
        [TagField(Format = "Pixels", MinVersion = CacheVersion.HaloReach)]
        public float MipMapBlur;

        /// <summary>
        /// Automatic chooses FAST if your bitmap is bright, and PRETTY if your bitmap has dark bits
        /// </summary>
        public BitmapCurveMode BitmapCurveMode;

        /// <summary>
        /// 0 = use default defined by usage
        /// </summary>
        public byte MaxMipMapLevel;

        /// <summary>
        /// 0 = do not downsample source image
        /// </summary>
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public short MaxResolution;

        /// <summary>
        /// Index into global atlas if the texture is missing its required resources and has been atlased
        /// </summary>
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public short AtlasIndex;

        /// <summary>
        /// Overrides the format defined by usage
        /// </summary>
        public short ForceBitmapFormat;

        /// <summary>
        /// This is the level cutoff for tight bounds. 0.0 is monochrome black, 1.0 is monochrome white
        /// </summary>
        [TagField(Format = "[0,1]", MinVersion = CacheVersion.HaloReach)]
        public float TightBoundsThreshold;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<TightBinding> TightBoundsOld;

        public List<UsageOverride> UsageOverrides;
        public List<Sequence> ManualSequences;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<TightBinding> TightBoundsNew;

        public byte[] SourceData;
        public byte[] ProcessedPixelData;
        public List<Sequence> Sequences;
        public List<Image> Images;
        public byte[] XenonProcessedPixelData;
        public List<Image> XenonImages;

        public List<TagResourceReference> Resources;
        public List<TagResourceReference> InterleavedResources;


        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public int UnknownB4;

        [TagStructure(Size = 0x8)]
        public class TightBinding : TagStructure
		{
            public RealPoint2d UV;
        }

        [TagStructure(Size = 0x28)]
        public class UsageOverride : TagStructure
		{
            /// <summary>
            /// 0.0 to use xenon curve (default)
            /// </summary>
            public float SourceGamma;

            public int BitmapCurveEnum;

            public byte Flags;
            public byte Unknown9;
            public byte UnknownA;
            public byte UnknownB;

            public byte UnknownC;
            public byte UnknownD;
            public short DownsamplingFlags;
            public short Unknown10;
            public short Unknown12;
            public int Unknown14;
            public int Unknown18;
            public int Unknown1C;
            public byte SwizzleRedEnum;
            public byte SwizzleBlueEnum;
            public byte SwizzleGreenEnum;
            public byte SwizzleAlphaEnum;
            public int FormatEnum; // 0 means the usage above is used
        }

        [TagStructure(Size = 0x40)]
        public class Sequence : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public short FirstBitmapIndex;
            public short BitmapCount;
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public List<Sprite> Sprites;

            [TagStructure(Size = 0x20)]
            public class Sprite : TagStructure
            {
                public short BitmapIndex;
                public short Unknown;
                public uint Unknown2;
                public float Left;
                public float Right;
                public float Top;
                public float Bottom;
                public RealPoint2d RegistrationPoint;
            }
        }

        [TagStructure(Size = 0x30, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x38, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.MCC)]
        [TagStructure(Size = 0x2C, MinVersion = CacheVersion.HaloReach)]
        public class Image : TagStructure
		{
            /// <summary>
            /// The group tag signature of the image.
            /// </summary>
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public Tag Signature;

            /// <summary>
            /// Pixels; DO NOT CHANGE
            /// </summary>
            public short Width;

            /// <summary>
            /// Pixels; DO NOT CHANGE
            /// </summary>
            public short Height;

            /// <summary>
            /// Pixels; DO NOT CHANGE
            /// </summary>
            public sbyte Depth;

            /// <summary>
            /// The xbox 360 flags of the bitmap image. DO NOT CHANGE
            /// </summary>
            public BitmapFlagsXbox XboxFlags;

            [TagField(Flags = Padding, Length = 1, MaxVersion = CacheVersion.Halo3ODST)]
            public byte[] Unused1;


            /// <summary>
            /// The type of the bitmap image. DO NOT CHANGE
            /// </summary>
            public BitmapType Type;

            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public byte UnknownFlags;

            // Handle the BitmapFormat enum as a sbyte instead of a short. This converts the endianness indirectly.
            [TagField(Flags = Padding, Length = 1, MaxVersion = CacheVersion.Halo3ODST)]
            public byte[] Unused2_1;
            [TagField(Flags = Padding, Length = 1, MinVersion = CacheVersion.HaloReach)]
            public byte[] Unused2_3;

            /// <summary>
            /// The format of the bitmap image. DO NOT CHANGE
            /// </summary>
            public BitmapFormat Format;

            [TagField(Flags = Padding, Length = 1, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public byte[] Unused2_2;


            /// <summary>
            /// The flags of the bitmap image. DO NOT CHANGE
            /// </summary>
            public BitmapFlags Flags;

            /// <summary>
            /// The 'center' of the bitmap - i.e. for particles
            /// </summary>
            public Point2d RegistrationPoint;

            /// <summary>
            /// DO NOT CHANGE (not counting the highest resolution)
            /// </summary>
            public sbyte MipmapCount;

            /// <summary>
            /// How to convert from pixel value to linear.
            /// </summary>
            public BitmapImageCurve Curve;

            public byte InterleavedTextureIndex1;
            public byte InterleavedTextureIndex2;

            public int DataOffset;
            public int DataSize;

            public int Unknown20;
            public int MipMapOffset;
            public PlatformUnsignedValue RasterizerTextureReference;
            public PlatformUnsignedValue RuntimeAddress;
        }

    }
}