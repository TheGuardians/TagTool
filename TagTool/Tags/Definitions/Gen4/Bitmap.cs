using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "bitmap", Tag = "bitm", Size = 0xCC)]
    public class Bitmap : TagStructure
    {
        // choose how you are using this bitmap
        public BitmapUsageGlobalEnum Usage;
        public BitmapGroupFlagsDef Flags;
        // number of pixels between adjacent sprites (0 uses default, negative numbers set no spacing)
        public short SpriteSpacing;
        // the apparent height of the bump map above the triangle it is textured onto, in texture repeats (i.e., 1.0 would be
        // as high as the texture is wide)
        public float BumpMapHeight; // repeats
        // used by detail maps and illum maps.  0 means fade by last mipmap, 1 means fade by first mipmap
        public float FadeFactor; // [0,1]
        // how much to blur the input image
        public float Blur; // pixels
        // how much to blur as each mip level is being downsampled
        public float MipMapBlur; // pixels
        // automatic chooses FAST if your bitmap is bright, and PRETTY if your bitmap has dark bits
        public BitmapCurveOverrideEnum CurveMode;
        // 0 = use default defined by usage
        public sbyte MaxMipmapLevel;
        // 0 = do not downsample source image
        public short MaxResolution;
        // index into global atlas if the texture is missing its required resources and has been atlased
        public short Atlas;
        // overrides the format defined by usage
        public BitmapUsageFormatDef ForceBitmapFormat;
        // This is the level cutoff for tight bounds.  0.0 is monochrome black, 1.0 is monochrome white
        public float TightBoundsThreshold; // [0.0 - 1.0]
        public List<BitmapUsageBlock> UsageOverride;
        public List<BitmapGroupSequenceBlockDef> ManualSequences;
        public List<BitmapTightBoundsBlockDef> TightBounds;
        public byte[] SourceData;
        public byte[] ProcessedPixelData;
        public List<BitmapGroupSequenceBlockDef> Sequences;
        public List<BitmapDataBlockDef> Bitmaps;
        public byte[] XenonProcessedPixelData;
        public List<BitmapDataBlockDef> XenonBitmaps;
        public List<BitmapTextureInteropBlockStruct> HardwareTextures;
        public List<StitchableBitmapTextureInteropBlockStruct> StitchableHardwareTextures;
        public List<BitmapTextureInterleavedInteropBlockStruct> InterleavedHardwareTextures;
        
        public enum BitmapUsageGlobalEnum : int
        {
            DiffuseMap,
            SpecularMap,
            BumpMap,
            DetailBumpMap,
            DetailMap,
            SelfIllumMap,
            ChangeColorMap,
            CubeMap,
            Sprite,
            Sprite1,
            Sprite2,
            InterfaceBitmap,
            WarpMap,
            VectorMap,
            _3dTexture,
            FloatMap, // HUGE)
            HalfFloatMap,
            HeightMap,
            ZbrushBumpMap,
            NormalMap,
            DetailZbrushBumpMap,
            DetailNormalMap,
            BlendMap,
            PalettizedEffectsOnly,
            ChudRelatedBitmap,
            LightmapArray,
            WaterArray,
            InterfaceSprite,
            InterfaceGradient,
            MaterialMap,
            SmokeWarp,
            MuxMaterialBlendMap,
            CubemapGel,
            LensFlareGamma22EffectsOnly,
            SignedNoise,
            RoughnessMap,
            NormalMap1,
            ColorGrading,
            DetailNormalMap1,
            DiffuseTextureArray,
            PalettizedTextureArray
        }
        
        [Flags]
        public enum BitmapGroupFlagsDef : ushort
        {
            // affects how height maps are converted to bump maps
            BitmapIsTiled = 1 << 0,
            // uses a sharper (and noisier) method of calculating bump maps from height maps
            UseLessBlurryBumpMap = 1 << 1,
            // lets the compressor use dithering
            DitherWhenCompressing = 1 << 2,
            // repopulates the manual sequences with random sprites upon reimport
            GenerateRandomSprites = 1 << 3,
            // FOR INTERNAL USE ONLY - DO NOT MODIFY
            UsingTagInteropAndTagResource = 1 << 4,
            // if you have an alpha channel but do not care about it, set this flag
            IgnoreAlphaChannel = 1 << 5,
            // if your alpha channel represents transparency (alpha blend or alpha-test only), set this bit to stop color bleeding
            // on edges
            AlphaChannelStoresTransparency = 1 << 6,
            // this will artificially thicken the alpha channel in mip maps, which can keep your bitmap from disappearing in the
            // distance when you are using alpha test
            PreserveAlphaChannelInMipmapsForAlphaTest = 1 << 7,
            // this bitmap will always be demand loaded, only supported by UI
            OnlyUseOnDemand = 1 << 8,
            // generate a polygonal bounding box around the non-empty pixels to save fill rate cost
            GenerateTightBounds = 1 << 9,
            // unchecked, tight bounds are generated from the color channel
            TightBoundsFromAlphaChannel = 1 << 10,
            // bitmap will have data split between medium and low optional sections
            DoNotGenerateRequiredSection = 1 << 11,
            // bitmap will not be available for stitching (2-priority only) - note, raises REQUIRED resource level
            DoNotAllowStitching = 1 << 12,
            // allows use of "max resolution" on bitmaps that have multiple frames, but may be buggy...?
            ApplyMaxResolutionAfterSlicing = 1 << 13,
            // generate a set of polygonal bounding boxes for various alpha black points
            GenerateBlackPointTightBounds = 1 << 14
        }
        
        public enum BitmapCurveOverrideEnum : sbyte
        {
            // will choose FAST if your bitmap is bright
            ChooseBest,
            // forces FAST mode, but causes banding in dark areas
            ForceFast,
            // chooses the best looking curve, probably slower
            ForcePretty
        }
        
        public enum BitmapUsageFormatDef : short
        {
            UseDefault,
            BestCompressedColorFormat,
            BestUncompressedColorFormat,
            BestCompressedBumpFormat,
            BestUncompressedBumpFormat,
            BestCompressedMonochromeFormat,
            BestUncompressedMonochromeFormat,
            BestCompressedMonochromeFormatWithoutAlpha,
            Unused2,
            Unused3,
            Unused4,
            Unused5,
            Unused6,
            ColorAndAlphaFormats,
            Dxt1,
            Dxt3,
            Dxt5,
            _24BitColor8BitAlpha,
            _8BitMonochrome8BitAlpha,
            ChannelMask,
            _30BitColor2BitAlpha,
            _48BitColor16BitAlpha,
            HalfColorAlpha,
            FloatColorAlpha,
            Ay8,
            Dxt3A,
            Dxt5A,
            CompressedMonochromeAlpha,
            A4R4G4B4,
            ColorOnlyFormats,
            _8BitMonochrome,
            Compressed24BitColor,
            _32BitColor,
            _16BitMonochrome,
            _16BitRedGreenOnly,
            _16BitSignedArgb,
            HalfRedOnly,
            FloatRedOnly,
            HalfRedGreenOnly,
            FloatRedGreenOnly,
            HalfMonochrome,
            Compressed4BitMonochrome,
            CompressedInterpolatedMonochrome,
            Dxt5Red,
            Dxt5Green,
            Dxt5Blue,
            AlphaOnlyFormats,
            Dxt3A1,
            Dxt5A1,
            _8BitAlpha,
            Unused13,
            Unused14,
            Unused15,
            NormalMapFormats,
            DxnCompressedNormals,
            Ctx1CompressedNormals,
            _16BitNormals,
            _32BitNormals,
            _8Bit4ChannelVector
        }
        
        [TagStructure(Size = 0x28)]
        public class BitmapUsageBlock : TagStructure
        {
            public float SourceGamma; // 0.0 to use xenon curve (default)
            public BitmapCurveEnum BitmapCurve;
            public BitmapUsageFlagsDef Flags;
            public BitmapUsageSlicerDef Slicer;
            public BitmapUsageDicerFlagsDef DicerFlags;
            public BitmapUsagePackerDef Packer;
            public BitmapUsagePackerFlagsDef PackerFlags;
            public BitmapTypes Type;
            public sbyte MipmapLimit;
            public BitmapSmallestMipDef SmallestMip;
            public BitmapUsageDownsampleFilterDef DownsampleFilter;
            public sbyte FilterRadiusBias;
            public BitmapUsageDownsampleFlagsDef DownsampleFlags;
            public RealRgbColor SpriteBackgroundColor;
            public BitmapUsageSwizzleDef SwizzleRed;
            public BitmapUsageSwizzleDef SwizzleGreen;
            public BitmapUsageSwizzleDef SwizzleBlue;
            public BitmapUsageSwizzleDef SwizzleAlpha;
            public BitmapUsageFormatDef BitmapFormat;
            
            public enum BitmapCurveEnum : int
            {
                Unknown,
                Xrgb,
                Gamma20,
                Linear,
                OffsetLog,
                Srgb
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
                NoSlicing,
                ColorPlateSlicer,
                CubeMapSlicer,
                ColorGradingSlicer
            }
            
            [Flags]
            public enum BitmapUsageDicerFlagsDef : byte
            {
                ConvertPlateColorKeyToAlphaChannel = 1 << 0,
                RotateCubeMapToMatchDirectXFormat = 1 << 1,
                SpritesShrinkElementsToSmallestNonZeroAlphaRegion = 1 << 2,
                SpritesShrinkElementsToSmallestNonZeroColorAndAlphaRegion = 1 << 3,
                UnsignedSignedScaleAndBias = 1 << 4,
                ColorGradingSRgbCorrection = 1 << 5
            }
            
            public enum BitmapUsagePackerDef : sbyte
            {
                NoPacking,
                SpritePack,
                SpritePackIfNeeded,
                _3dPack
            }
            
            [Flags]
            public enum BitmapUsagePackerFlagsDef : byte
            {
                ShrinkSpriteTexturePagesTightlyToContent = 1 << 0
            }
            
            public enum BitmapTypes : sbyte
            {
                _2dTexture,
                _3dTexture,
                CubeMap,
                Array
            }
            
            public enum BitmapSmallestMipDef : sbyte
            {
                _1Pixel,
                _2Pixel,
                _4Pixel,
                _8Pixel,
                _16Pixel,
                _32Pixel,
                _64Pixel,
                _128Pixel,
                _256Pixel,
                _512Pixel,
                _1024Pixel
            }
            
            public enum BitmapUsageDownsampleFilterDef : sbyte
            {
                PointSampled,
                BoxFilter,
                BlackmanFilter,
                LanczosFilter,
                NuttallFilter,
                BlackmanHarrisFilter,
                BlackmanNuttallFilter,
                FlatTopFilter,
                ExtremeFilter
            }
            
            [Flags]
            public enum BitmapUsageDownsampleFlagsDef : ushort
            {
                SpritesColorBleedInZeroAlphaRegions = 1 << 0,
                PreMultiplyAlpha = 1 << 1,
                PostDivideAlpha = 1 << 2,
                HeightMapConvertToBumpMap = 1 << 3,
                DetailMapFadeToGray = 1 << 4,
                SignedUnsignedScaleAndBias = 1 << 5,
                IllumMapFadeToBlack = 1 << 6,
                ZbumpScaleByHeightAndRenormalize = 1 << 7,
                CubemapFixSeams = 1 << 8,
                CalculateSpecularPower = 1 << 9,
                DownsampleBumpsInAngularSpace = 1 << 10,
                StandardOrientationOfNormalsInAngularSpaceAndRenormalize = 1 << 11,
                GenerateRgbLuminanceIntoAlphaChannel = 1 << 12
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
                SetTo05,
                Random
            }
        }
        
        [TagStructure(Size = 0x40)]
        public class BitmapGroupSequenceBlockDef : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public short FirstBitmapIndex;
            public short BitmapCount;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<BitmapGroupSpriteBlockDef> Sprites;
            
            [TagStructure(Size = 0x20)]
            public class BitmapGroupSpriteBlockDef : TagStructure
            {
                public short BitmapIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public float Left;
                public float Right;
                public float Top;
                public float Bottom;
                public RealPoint2d RegistrationPoint;
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class BitmapTightBoundsBlockDef : TagStructure
        {
            public RealPoint2d Uv;
        }
        
        [TagStructure(Size = 0x2C)]
        public class BitmapDataBlockDef : TagStructure
        {
            // DO NOT CHANGE
            public short Width; // pixels
            // DO NOT CHANGE
            public short Height; // pixels
            // DO NOT CHANGE
            public sbyte Depth; // pixels
            public BitmapMoreFlags MoreFlags;
            // DO NOT CHANGE
            public BitmapTypes Type;
            // DO NOT CHANGE
            public sbyte FourTimesLog2Size;
            // DO NOT CHANGE
            public BitmapFormats Format;
            public BitmapFlags Flags;
            public sbyte ExponentBias;
            // the 'center' of the bitmap - i.e. for particles
            public Point2d RegistrationPoint;
            // DO NOT CHANGE (not counting the highest resolution)
            public sbyte MipmapCount;
            // how to convert from pixel value to linear
            public BitmapCurveEnum Curve;
            public sbyte InterleavedInterop;
            public sbyte InterleavedTextureIndex;
            // DO NOT CHANGE (offset of the beginning of this bitmap, into pixel data)
            public int PixelsOffset; // bytes
            // DO NOT CHANGE (total bytes used by this bitmap)
            public int PixelsSize; // bytes
            // DO NOT CHANGE
            public int MediumResPixelsSize;
            // DO NOT CHANGE
            public int HighResPixelsSize;
            public int HardwareFormat;
            public int RuntimeTagBaseAddress;
            
            [Flags]
            public enum BitmapMoreFlags : byte
            {
                // DO NOT CHANGE
                Xbox360MediumResolutionOffsetIsValid = 1 << 0,
                // DO NOT CHANGE
                Xbox360Pitch = 1 << 1,
                // DO NOT CHANGE
                Xbox360ByteOrder = 1 << 2,
                // DO NOT CHANGE
                Xbox360TiledTexture = 1 << 3,
                // DO NOT CHANGE
                Xbox360CreatedCorrectly = 1 << 4,
                // DO NOT CHANGE
                Xbox360HighResolutionOffsetIsValid = 1 << 5,
                // DO NOT CHANGE
                Xbox360UseInterleavedTextures = 1 << 6,
                // DO NOT CHANGE
                Xbox360UseOnDemandOnly = 1 << 7
            }
            
            public enum BitmapTypes : sbyte
            {
                _2dTexture,
                _3dTexture,
                CubeMap,
                Array
            }
            
            public enum BitmapFormats : short
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
                Dxt5BiasAlpha,
                Dxt1,
                Dxt3,
                Dxt5,
                A4r4g4b4Font,
                Unused7,
                Unused8,
                SoftwareRgbfp32,
                Unused9,
                V8u8,
                G8b8,
                Abgrfp32,
                Abgrfp16,
                _16fMono,
                _16fRed,
                Q8w8v8u8,
                A2r10g10b10,
                A16b16g16r16,
                V16u16,
                L16,
                R16g16,
                Signedr16g16b16a16,
                Dxt3a,
                Dxt5a,
                Dxt3a1111,
                Dxn,
                Ctx1,
                Dxt3aAlpha,
                Dxt3aMono,
                Dxt5aAlpha,
                Dxt5aMono,
                DxnMonoAlpha,
                Dxt5Red,
                Dxt5Green,
                Dxt5Blue,
                Depth24
            }
            
            [Flags]
            public enum BitmapFlags : byte
            {
                // DO NOT CHANGE
                PowerOfTwoDimensions = 1 << 0,
                // DO NOT CHANGE
                Compressed = 1 << 1,
                // DO NOT CHANGE
                SwapAxes = 1 << 2
            }
            
            public enum BitmapCurveEnum : sbyte
            {
                Unknown,
                Xrgb,
                Gamma20,
                Linear,
                OffsetLog,
                Srgb
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class BitmapTextureInteropBlockStruct : TagStructure
        {
            public TagResourceReference TextureResource;
        }
        
        [TagStructure(Size = 0x8)]
        public class StitchableBitmapTextureInteropBlockStruct : TagStructure
        {
            public TagResourceReference TextureResource;
        }
        
        [TagStructure(Size = 0x8)]
        public class BitmapTextureInterleavedInteropBlockStruct : TagStructure
        {
            public TagResourceReference InterleavedTextureResource;
        }
    }
}
