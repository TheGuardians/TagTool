using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "camera_fx_settings", Tag = "cfxs", Size = 0xE4, Platform = CachePlatform.Original, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "camera_fx_settings", Tag = "cfxs", Size = 0x170, Platform = CachePlatform.MCC, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "camera_fx_settings", Tag = "cfxs", Size = 0x170, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "camera_fx_settings", Tag = "cfxs", Size = 0xDC, MinVersion = CacheVersion.HaloReach)]
    public class CameraFxSettings : TagStructure
	{
        public ExposureBlock Exposure;

        public CameraFxValue AutoExposureSensitivity;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CameraFxValue AutoExposureAntiBloom;

        public CameraFxStrength BloomPoint; // aka highlight bloom
        public CameraFxStrength InherentBloom;
        public CameraFxStrength BloomIntensity;

        public CameraFxColor HighlightBloomTint;
        public CameraFxColor InherentBloomTint;
        public CameraFxColor LightingBloomTint;

        public CameraFxStrength BlingIntensity;
        public CameraFxStrength BlingSize;
        public CameraFxStrength BlingAngle;
        public CameraFxBlingCountStruct BlingCount;

        public CameraFxStrength SelfIllumPreferred;
        public CameraFxStrength SelfIllumScale;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        public SsaoPropertiesBlock Ssao;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        public ColorGradingBlock ColorGrading;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        public LightshaftsBlock Lightshafts;

        [Flags]
        public enum FlagsValue : ushort
        {
            None = 0,
            UseDefault = 1 << 0,
            MaximumChangeIsRelative = 1 << 1,
            AutoAdjustTarget = 1 << 2,
            Unused1 = 1 << 3,
            Fixed1 = 1 << 4,
            Unused2 = 1 << 5,
            Fixed2 = 1 << 6
        }

        [TagStructure(Size = 0x10)]
        public class CameraFxStrength : TagStructure
        {
            public FlagsValue Flags;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused = new byte[2];

            public float Strength;
            public float MaximumChange;
            public float BlendSpeed;
        }

        [TagStructure(Size = 0x8)]
        public class CameraFxValue : TagStructure
        {
            public FlagsValue Flags;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused = new byte[2];

            public float Value;
        }

        [TagStructure(Size = 0x10)]
        public class CameraFxColor : TagStructure
        {
            public FlagsValue Flags;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused = new byte[2];

            public RealRgbColor Color;
        }

        [TagStructure(Size = 0x10)]
        public class ExposureBlock : CameraFxStrength
        {
            public Bounds<float> ExposureRange; // the absolute target exposure is clamped to this range
            public float AutoBrightness; // [0.0001 - 1]
            public float AutoBrightnessDelay; // [0.1 - 1]
        }

        [TagStructure(Size = 0x4)]
        public class CameraFxBlingCountStruct : TagStructure
        {
            public FlagsValue Flags;
            public short BlingSpikes;
        }

        [TagStructure(Size = 0x10)]
        public class SsaoPropertiesBlock : TagStructure
        {
            public SsaoFlags Flags;
            public float Intensity;
            public float Radius;
            public float SampleZThreshold;

            [Flags]
            public enum SsaoFlags : uint
            {
                UseDefault = 1 << 0,
                Enable = 1 << 1,
                Unused0 = 1 << 2,
                Unused1 = 1 << 3,
                Fixed = 1 << 4
            }
        }

        [TagStructure(Size = 0x50)]
        public class ColorGradingBlock : TagStructure
        {
            public CameraFxParameterFlagsCg Flags;
            public float BlendTime;
            public List<ColorGradingCurvesEditorBlock> CurvesEditor;
            public List<ColorGradingBrightnessContrastBlock> BrightnessContrast;
            public List<ColorGradingHslvBlock> HueSaturationLightnessVibrance;
            public List<ColorGradingColorizeEffectBlock> ColorizeEffect;
            public List<ColorGradingSelectiveColorBlock> SelectiveColor;
            public List<ColorGradingColorBalanceBlock> ColorBalance;

            [Flags]
            public enum CameraFxParameterFlagsCg : uint
            {
                UseDefault = 1 << 0,
                Enable = 1 << 1,
                Unused0 = 1 << 2,
                Unused1 = 1 << 3,
                Fixed = 1 << 4
            }

            [TagStructure(Size = 0x58)]
            public class ColorGradingCurvesEditorBlock : TagStructure
            {
                public CameraFxParameterFlagsEnable Flags;
                public ColGradCurvesEditorMode Mode;
                public ColorGradingScalarFunctionStruct BrightnessCurve;
                public ColorGradingScalarFunctionStruct RedCurve;
                public ColorGradingScalarFunctionStruct GreenCurve;
                public ColorGradingScalarFunctionStruct BlueCurve;

                [Flags]
                public enum CameraFxParameterFlagsEnable : uint
                {
                    Enable = 1 << 0
                }

                public enum ColGradCurvesEditorMode : int
                {
                    RGB,
                    Brightness
                }

                [TagStructure(Size = 0x14)]
                public class ColorGradingScalarFunctionStruct : TagStructure
                {
                    public MappingFunction Mapping;

                    [TagStructure(Size = 0x14)]
                    public class MappingFunction : TagStructure
                    {
                        public byte[] Data;
                    }
                }
            }

            [TagStructure(Size = 0xC)]
            public class ColorGradingBrightnessContrastBlock : TagStructure
            {
                public CameraFxParameterFlagsEnable Flags;
                public float Brightness;
                public float Contrast;

                [Flags]
                public enum CameraFxParameterFlagsEnable : uint
                {
                    Enable = 1 << 0
                }
            }

            [TagStructure(Size = 0x14)]
            public class ColorGradingHslvBlock : TagStructure
            {
                public CameraFxParameterFlagsEnable Flags;
                public float Hue;
                public float Saturation;
                public float Lightness;
                public float Vibrance;

                [Flags]
                public enum CameraFxParameterFlagsEnable : uint
                {
                    Enable = 1 << 0
                }
            }

            [TagStructure(Size = 0x14)]
            public class ColorGradingColorizeEffectBlock : TagStructure
            {
                public CameraFxParameterFlagsEnable Flags;
                public float Blendfactor;
                public float Hue;
                public float Saturation;
                public float Lightness;

                [Flags]
                public enum CameraFxParameterFlagsEnable : uint
                {
                    Enable = 1 << 0
                }
            }

            [TagStructure(Size = 0x94)]
            public class ColorGradingSelectiveColorBlock : TagStructure
            {
                public CameraFxParameterFlagsEnable Flags;
                public ColorGradingCmybStruct Reds;
                public ColorGradingCmybStruct Yellows;
                public ColorGradingCmybStruct Greens;
                public ColorGradingCmybStruct Cyans;
                public ColorGradingCmybStruct Blues;
                public ColorGradingCmybStruct Magentas;
                public ColorGradingCmybStruct Whites;
                public ColorGradingCmybStruct Neutrals;
                public ColorGradingCmybStruct Blacks;

                [Flags]
                public enum CameraFxParameterFlagsEnable : uint
                {
                    Enable = 1 << 0
                }

                [TagStructure(Size = 0x10)]
                public class ColorGradingCmybStruct : TagStructure
                {
                    public float Cyan;
                    public float Magenta;
                    public float Yellow;
                    public float Black;
                }
            }

            [TagStructure(Size = 0x28)]
            public class ColorGradingColorBalanceBlock : TagStructure
            {
                public CameraFxParameterFlagsEnable Flags;
                public ColorGradingCmyStruct Shadows;
                public ColorGradingCmyStruct Midtones;
                public ColorGradingCmyStruct Highlights;

                [Flags]
                public enum CameraFxParameterFlagsEnable : uint
                {
                    Enable = 1 << 0
                }

                [TagStructure(Size = 0xC)]
                public class ColorGradingCmyStruct : TagStructure
                {
                    public float CyanRed;
                    public float MagentaGreen;
                    public float YellowBlue;
                }
            }
        }

        [TagStructure(Size = 0x2C)]
        public class LightshaftsBlock : TagStructure
        {
            [TagField(EnumType = typeof(uint))]
            public FlagsValue Flags;
            public float Pitch; // [0...90]
            public float Heading; // [0...360]
            public RealRgbColor Tint;
            public float DepthClamp;
            public float IntensityClamp; // [0...1]
            public float FalloffRadius; // [0...2]
            public float Intensity; // [0...50]
            public float BlurRadius; // [0...20]
        }
    }
}
