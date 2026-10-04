using TagTool.Bitmaps;
using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "bitmap", Tag = "bitm", Size = 0xA4, MinVersion = CacheVersion.Halo3Beta, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "bitmap", Tag = "bitm", Size = 0xB8, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline106708)]
    [TagStructure(Name = "bitmap", Tag = "bitm", Size = 0xAC, MinVersion = CacheVersion.HaloOnline235640, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "bitmap", Tag = "bitm", Size = 0xC0, MinVersion = CacheVersion.HaloReach)]
    public class Bitmap : TagStructure
	{
        [TagField(EnumType = typeof(int))]
        public BitmapUsageGlobalEnum Usage; // Choose how you are using this bitmap
        public BitmapRuntimeFlags Flags; // The runtime flags of this bitmap
        public short SpriteSpacing; // Number of pixels between adjacent sprites (0 uses default, negative numbers set no spacing)

        [TagField(Format = "Repeats")]
        public float BumpMapHeight; // The apparent height of the bump map above the triangle it is textured onto, in texture repeats (i.e., 1.0 would be as high as the texture is wide)

        [TagField(Flags = Fraction, Format = "[0,1]")]
        public float FadeFactor; // [0,1] Used by detail maps and illum maps. 0 means fade by last mipmap, 1 means fade by first mipmap

        [TagField(Format = "Pixels", MinVersion = CacheVersion.HaloReach)]
        public float Blur; // How much to blur the input image

        [TagField(Format = "Pixels", MinVersion = CacheVersion.HaloReach)]
        public float MipMapBlur; // How much to blur as each mip level is being downsampled

        public BitmapCurveMode CurveMode; // Automatic chooses FAST if your bitmap is bright, and PRETTY if your bitmap has dark bits
        public byte MaxMipMapLevel; // 0 = use default defined by usage

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public short MaxResolution; // 0 = do not downsample source image

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public short AtlasIndex; // Index into global atlas if the texture is missing its required resources and has been atlased

        [TagField(EnumType = typeof(short))]
        public BitmapUsageFormat ForceBitmapFormat; // Overrides the format defined by usage

        [TagField(Format = "[0,1]", MinVersion = CacheVersion.HaloReach)]
        public float TightBoundsThreshold; // This is the level cutoff for tight bounds. 0.0 is monochrome black, 1.0 is monochrome white

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
        public byte[] XenonBitmaps;
        public List<Image> XenonImages;

        public List<TagResourceReference> HardwareTextures;

        [TagField(MaxVersion = CacheVersion.HaloOnline106708)]
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<TagResourceReference> InterleavedHardwareTextures;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public int UnknownB4;

        [TagEnum(IsVersioned = true)]
        public enum BitmapUsageGlobalEnum : int
        {
            DiffuseMap,
            SpecularMap,
            BumpMapfromHeightMap,
            DetailBumpMapfromHeightMapFadesOut,
            DetailMap,
            SelfIllumMap,
            ChangeColorMap,
            CubeMapReflectionMap,
            SpriteAdditiveBlackBackground,
            SpriteBlendWhiteBackground,
            SpriteDoubleMultiplyGrayBackground,
            InterfaceBitmap,
            WarpMapEMBM,
            VectorMap,
            _3DTexture,
            FloatMapWARNING, // HUGE)

            [TagEnumMember(MinVersion = CacheVersion.HaloReach)]
            HalfFloatMapHalfHuge,

            HeightMapforParallax,
            ZBrushBumpMapfromBumpMap,

            [TagEnumMember(MinVersion = CacheVersion.HaloReach)]
            NormalMapAkaZbump,
            [TagEnumMember(MinVersion = CacheVersion.HaloReach)]
            DetailZbrushBumpMap,
            [TagEnumMember(MinVersion = CacheVersion.HaloReach)]
            DetailNormalMap,

            BlendMaplinearForTerrains,
            PalettizedEffectsOnly,
            CHUDRelatedBitmap,
            LightmapArray,
            WaterArray,
            InterfaceSprite,
            InterfaceGradient,

            [TagEnumMember(MinVersion = CacheVersion.HaloReach)]
            MaterialMap,
            [TagEnumMember(MinVersion = CacheVersion.HaloReach)]
            SmokeWarp,
            [TagEnumMember(MinVersion = CacheVersion.HaloReach)]
            MuxMaterialBlendMap,
            [TagEnumMember(MinVersion = CacheVersion.HaloReach)]
            CubemapGel,
            [TagEnumMember(MinVersion = CacheVersion.HaloReach)]
            LensFlareGamma22EffectsOnly,
            [TagEnumMember(MinVersion = CacheVersion.HaloReach)]
            SignedNoise,
            [TagEnumMember(MinVersion = CacheVersion.HaloReach)]
            RoughnessMapAuto,

            // Halo Online only, the Scaleform (GFx) UI textures listed by the gfx_textures_list (gfxt) tag
            [TagEnumMember(Gen = CacheGeneration.HaloOnline)]
            GfxTexture,                 // DXT1/DXT5, no mipmaps, any dimensions
            [TagEnumMember(Gen = CacheGeneration.HaloOnline)]
            GfxTextureUncompressed      // "*_nocompress_nopack" sources, A8R8G8B8, no mipmaps
        }

        [TagStructure(Size = 0x8)]
        public class TightBinding : TagStructure
		{
            public RealPoint2d UV;
        }

        [TagStructure(Size = 0x28)]
        public class UsageOverride : TagStructure
		{
            public float SourceGamma; // 0.0 to use xenon curve (default)
            public BitmapCurveEnum BitmapCurve;
            public BitmapUsageFlagsDef Flags;
            public BitmapUsageSlicerDef Slicer;
            public BitmapUsageDicerFlagsDef DicerFlags;
            public BitmapUsagePackerDef Packer;
            public BitmapTypes Type;
            public short MipmapLimit;
            public BitmapUsageDownsampleFilterDef DownsampleFilter;
            public BitmapUsageDownsampleFlagsDef DownsampleFlags;
            public RealRgbColor SpriteBackgroundColor;
            public BitmapUsageSwizzleDef SwizzleRed;
            public BitmapUsageSwizzleDef SwizzleGreen;
            public BitmapUsageSwizzleDef SwizzleBlue;
            public BitmapUsageSwizzleDef SwizzleAlpha;
            [TagField(EnumType = typeof(int))]
            public BitmapUsageFormat BitmapFormat; // 0 means the usage above is used

            public enum BitmapCurveEnum : int
            {
                Unknown,
                XRGBgammaAbout20SRGBgamma22,
                Gamma20,
                Linear,
                OffsetLog,
                SRGB
            }

            [Flags]
            public enum BitmapUsageFlagsDef : byte
            {
                IgnoreCurveOverride = 1 << 0,
                DontAllowSizeOptimization = 1 << 1,
                SwapAxes = 1 << 2
            }

            public enum BitmapUsageSlicerDef : sbyte
            {
                AutomaticallyDetermineSlicer,
                NoSlicingeachSourceBitmapGeneratesOneElement,
                ColorPlateSlicer,
                CubeMapSlicer
            }

            [Flags]
            public enum BitmapUsageDicerFlagsDef : ushort
            {
                ConvertPlateColorKeyToAlphaChannel = 1 << 0,
                RotateCubeMapToMatchDirectXFormat = 1 << 1,
                SpritesShrinkElementsToSmallestNonZeroAlphaRegion = 1 << 2,
                SpritesShrinkElementsToSmallestNonZeroColorAndAlphaRegion = 1 << 3,
                UnsignedSignedScaleAndBias = 1 << 4
            }

            public enum BitmapUsagePackerDef : sbyte
            {
                NoPacking,
                SpritePackpacksElementsIntoAsFewBitmapsAsPossible,
                SpritePackIfNeededpacksElementsIntoAsFewBitmapsAsPossible,
                _3DPackpacksElementsIntoA3DBitmap
            }

            public enum BitmapTypes : sbyte
            {
                _2DTexture,
                _3DTexture,
                CubeMap,
                Array
            }

            public enum BitmapUsageDownsampleFilterDef : short
            {
                PointSampled,
                BoxFilter,
                GaussianFilter
            }

            [Flags]
            public enum BitmapUsageDownsampleFlagsDef : ushort
            {
                SpritesColorBleedInZeroAlphaRegions = 1 << 0,
                PreMultiplyAlphabeforeDownsampling = 1 << 1,
                PostDivideAlphaafterDownsampling = 1 << 2,
                HeightMapConvertToBumpMap = 1 << 3,
                DetailMapFadeToGray = 1 << 4,
                SignedUnsignedScaleAndBias = 1 << 5,
                IllumMapFadeToBlack = 1 << 6,
                ZBumpScaleByHeightAndRenormalize = 1 << 7
            }

            public enum BitmapUsageSwizzleDef : sbyte
            {
                Default,
                SourceRedChannel,
                SourceGreenChannel,
                SourceBlueChannel,
                SourceAlphaChannel,
                SetTo10,
                SetTo00,
                SetTo05
            }
        }

        [TagStructure(Size = 0x40)]
        public class Sequence : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public short FirstBitmapIndex;
            public short BitmapCount;

            [TagField(Length = 16, Flags = Padding)]
            public byte[] Padding0;

            public List<Sprite> Sprites;

            [TagStructure(Size = 0x20)]
            public class Sprite : TagStructure
            {
                public short BitmapIndex;

                [TagField(Length = 2, Flags = Padding)]
                public byte[] Padding0;

                [TagField(Length = 4, Flags = Padding)]
                public byte[] Padding1;

                public float Left;
                public float Right;
                public float Top;
                public float Bottom;
                public RealPoint2d RegistrationPoint;
            }
        }

        [TagStructure(Size = 0x30, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x38, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.MCC)]
        [TagStructure(Size = 0x2C, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x38, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.MCC)]
        public class Image : TagStructure
        {
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public Tag Signature = "bitm"; // The group tag signature of the image.

            public short Width; // Pixels; DO NOT CHANGE
            public short Height; // Pixels; DO NOT CHANGE
            public sbyte Depth; // Pixels; DO NOT CHANGE
            public BitmapFlagsXbox XboxFlags; // The xbox 360 flags of the bitmap image. DO NOT CHANGE

            [TagField(MaxVersion = CacheVersion.HaloOnline700123, EnumType = typeof(short))]
            [TagField(MinVersion = CacheVersion.HaloReach, EnumType = typeof(sbyte))]
            public BitmapType Type; // The type of the bitmap image. DO NOT CHANGE

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public sbyte FourTimesLog2Size;

            [TagField(MinVersion = CacheVersion.Halo3Beta, EnumType = typeof(short))]
            public BitmapFormat Format; // The format of the bitmap image. DO NOT CHANGE

            [TagField(MaxVersion = CacheVersion.HaloOnline700123, EnumType = typeof(ushort))]
            [TagField(MinVersion = CacheVersion.HaloReach, EnumType = typeof(byte))]
            public BitmapFlags Flags; // The flags of the bitmap image. DO NOT CHANGE
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public sbyte ExponentBias;
            public Point2d RegistrationPoint; // The 'center' of the bitmap - i.e. for particles
            public sbyte MipmapCount; // DO NOT CHANGE (not counting the highest resolution)
            public BitmapImageCurve Curve; // How to convert from pixel value to linear.
            public sbyte InterleavedInterop;
            public sbyte InterleavedTextureIndex;
            public int PixelDataOffset;
            public int PixelDataSize;
            public int HighResPixelsOffsetOffset;
            public int HighResPixelsSize;
            public int HardwareFormat;
            public PlatformUnsignedValue RuntimeTagBaseAddress;

            [TagField(Platform = CachePlatform.MCC, Version = CacheVersion.HaloReach, Length = 8, Flags = Padding)]
            public byte[] ReachMCCPadding;
        }

        [TagEnum(IsVersioned = true)]
        public enum BitmapUsageFormat
        {
            UseDefaultDefinedByUsage,
            BestCompressedColorFormat,
            BestUncompressedColorFormat,
            BestCompressedBumpFormat,
            BestUncompressedBumpFormat,
            BestCompressedMonochromeFormat,
            BestUncompressedMonochromeFormat,
            [TagEnumMember(MinVersion = CacheVersion.HaloReach)]
            BestCompressedMonochromeFormatWithoutAlpha,
            Unused2,
            Unused3,
            Unused4,
            Unused5,
            Unused6,
            ColorAndAlphaFormats,
            Dxt1CompressedColorColorKeyAlpha,
            Dxt3CompressedColor4BitAlpha,
            Dxt5CompressedColorCompressed8BitAlpha,
            _24BitColor8BitAlpha,
            _8BitMonochrome8BitAlpha,
            ChannelMask3BitColor1BitAlpha,
            _30BitColor2BitAlpha,
            _48BitColor16BitAlpha,
            HalfColorAlpha,
            FloatColorAlpha,
            Ay88BitIntensityReplicatedToArgb,
            Dxt3A4BitIntensityReplicatedToArgb,
            Dxt5ADxtCompressedIntensityReplicatedToArgb,
            CompressedMonochromeAlpha,
            A4R4G4B412BitColor4BitAlpha,
            ColorOnlyFormats,
            _8BitMonochrome,
            Compressed24BitColor,
            _32BitColorR11G11B10,
            _16BitMonochrome,
            _16BitRedGreenOnly,
            [TagEnumMember(MinVersion = CacheVersion.HaloReach)]
            _16BitSignedArgb,
            HalfRedOnly,
            FloatRedOnly,
            HalfRedGreenOnly,
            FloatRedGreenOnly,
            [TagEnumMember(MinVersion = CacheVersion.HaloReach)]
            HalfMonochrome,
            Compressed4BitMonochrome,
            CompressedInterpolatedMonochrome,
            [TagEnumMember(MaxVersion = CacheVersion.HaloOnline700123)]
            Unused12,
            [TagEnumMember(MinVersion = CacheVersion.HaloReach)]
            Dxt5Red,
            [TagEnumMember(MinVersion = CacheVersion.HaloReach)]
            Dxt5Green,
            [TagEnumMember(MinVersion = CacheVersion.HaloReach)]
            Dxt5Blue,
            AlphaOnlyFormats,
            Dxt3A4BitAlpha,
            Dxt5A8BitCompressedAlpha,
            _8BitAlpha,
            Unused13,
            Unused14,
            Unused15,
            NormalMapFormats,
            DxnCompressedNormalsBetter,
            Ctx1CompressedNormalsSmaller,
            _16BitNormals,
            _32BitNormals,
            [TagEnumMember(MinVersion = CacheVersion.HaloReach)]
            _8Bit4ChannelVector
        }
    }
}