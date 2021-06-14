using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "camera_fx_settings", Tag = "cfxs", Size = 0x120)]
    public class CameraFxSettings : TagStructure
    {
        public CameraFxExposureStruct Exposure;
        public CameraFxExposureSensitivityStruct AutoExposureSensitivity;
        public CameraFxBloomHighlightStruct BloomHighlight;
        public CameraFxBloomInherentStruct BloomInherent;
        public CameraFxBloomSelfIllumStruct BloomSelfIllum;
        public CameraFxBloomIntensityStruct BloomIntensity;
        public CameraFxBloomLargeColorStruct BloomLargeColor;
        public CameraFxBloomMediumColorStruct BloomMediumColor;
        public CameraFxBloomSmallColorStruct BloomSmallColor;
        public CameraFxBlingIntensityStruct BlingIntensity;
        public CameraFxBlingSizeStruct BlingSize;
        public CameraFxBlingAngleStruct BlingAngle;
        public CameraFxBlingCountStruct BlingCount;
        public CameraFxSelfIllumPreferredStruct SelfIllumPreferred;
        public CameraFxSelfIllumScaleStruct SelfIllumScale;
        public CameraFxColorGradingStruct ColorGrading;
        public CameraFxFilmicToneCurveStruct FilmicToneCurve;
        
        [TagStructure(Size = 0x20)]
        public class CameraFxExposureStruct : TagStructure
        {
            public CameraFxParameterFlagsAutoAdjust Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // the target exposure (ONLY USED WHEN AUTO-EXPOSURE IS OFF)
            public float Exposure; // stops
            // the maximum allowed change in exposure between frames
            public float MaximumChange; // stops
            // 1 is instantaneous, 0.01 is a good speed, 0.001 is slower
            public float BlendSpeed; // percent per frame
            // the absolute target exposure is clamped to this range
            public float Minimum; // stops
            // the absolute target exposure is clamped to this range
            public float Maximum; // stops
            // how bright you want the screen to be - auto-exposure will make it happen
            public float AutoExposureScreenBrightness; // [0.0001-1]
            // how long to wait before auto-exposure kicks in to adjust the exposure
            public float AutoExposureDelay; // [0.1-1]seconds
            
            [Flags]
            public enum CameraFxParameterFlagsAutoAdjust : ushort
            {
                UseDefault = 1 << 0,
                MaximumChangeIsRelative = 1 << 1,
                AutoAdjustTarget = 1 << 2,
                Bit3 = 1 << 3,
                Fixed = 1 << 4,
                Unknown = 1 << 5,
                Fixed2 = 1 << 6
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class CameraFxExposureSensitivityStruct : TagStructure
        {
            public CameraFxParameterFlagsNoAutoAdjust Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float Sensitivity;
            
            [Flags]
            public enum CameraFxParameterFlagsNoAutoAdjust : ushort
            {
                UseDefault = 1 << 0,
                MaximumChangeIsRelative = 1 << 1,
                Unknown = 1 << 2,
                Bit3 = 1 << 3,
                Fixed = 1 << 4,
                Unknown1 = 1 << 5,
                Fixed2 = 1 << 6
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class CameraFxBloomHighlightStruct : TagStructure
        {
            public CameraFxParameterFlagsNoAutoAdjust Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float HighlightBloom;
            public float MaximumChange;
            public float BlendSpeed;
            
            [Flags]
            public enum CameraFxParameterFlagsNoAutoAdjust : ushort
            {
                UseDefault = 1 << 0,
                MaximumChangeIsRelative = 1 << 1,
                Unknown = 1 << 2,
                Bit3 = 1 << 3,
                Fixed = 1 << 4,
                Unknown1 = 1 << 5,
                Fixed2 = 1 << 6
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class CameraFxBloomInherentStruct : TagStructure
        {
            public CameraFxParameterFlagsNoAutoAdjust Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float InherentBloom;
            public float MaximumChange;
            public float BlendSpeed;
            
            [Flags]
            public enum CameraFxParameterFlagsNoAutoAdjust : ushort
            {
                UseDefault = 1 << 0,
                MaximumChangeIsRelative = 1 << 1,
                Unknown = 1 << 2,
                Bit3 = 1 << 3,
                Fixed = 1 << 4,
                Unknown1 = 1 << 5,
                Fixed2 = 1 << 6
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class CameraFxBloomSelfIllumStruct : TagStructure
        {
            public CameraFxParameterFlagsNoAutoAdjust Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float SelfIllumBloomBloom;
            public float MaximumChange;
            public float BlendSpeed;
            
            [Flags]
            public enum CameraFxParameterFlagsNoAutoAdjust : ushort
            {
                UseDefault = 1 << 0,
                MaximumChangeIsRelative = 1 << 1,
                Unknown = 1 << 2,
                Bit3 = 1 << 3,
                Fixed = 1 << 4,
                Unknown1 = 1 << 5,
                Fixed2 = 1 << 6
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class CameraFxBloomIntensityStruct : TagStructure
        {
            public CameraFxParameterFlagsNoAutoAdjust Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float BloomIntensity;
            public float MaximumChange;
            public float BlendSpeed;
            
            [Flags]
            public enum CameraFxParameterFlagsNoAutoAdjust : ushort
            {
                UseDefault = 1 << 0,
                MaximumChangeIsRelative = 1 << 1,
                Unknown = 1 << 2,
                Bit3 = 1 << 3,
                Fixed = 1 << 4,
                Unknown1 = 1 << 5,
                Fixed2 = 1 << 6
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class CameraFxBloomLargeColorStruct : TagStructure
        {
            public CameraFxParameterFlagsNoAutoAdjust Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public RealRgbColor LargeColor;
            
            [Flags]
            public enum CameraFxParameterFlagsNoAutoAdjust : ushort
            {
                UseDefault = 1 << 0,
                MaximumChangeIsRelative = 1 << 1,
                Unknown = 1 << 2,
                Bit3 = 1 << 3,
                Fixed = 1 << 4,
                Unknown1 = 1 << 5,
                Fixed2 = 1 << 6
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class CameraFxBloomMediumColorStruct : TagStructure
        {
            public CameraFxParameterFlagsNoAutoAdjust Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public RealRgbColor MediumColor;
            
            [Flags]
            public enum CameraFxParameterFlagsNoAutoAdjust : ushort
            {
                UseDefault = 1 << 0,
                MaximumChangeIsRelative = 1 << 1,
                Unknown = 1 << 2,
                Bit3 = 1 << 3,
                Fixed = 1 << 4,
                Unknown1 = 1 << 5,
                Fixed2 = 1 << 6
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class CameraFxBloomSmallColorStruct : TagStructure
        {
            public CameraFxParameterFlagsNoAutoAdjust Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public RealRgbColor SmallColor;
            
            [Flags]
            public enum CameraFxParameterFlagsNoAutoAdjust : ushort
            {
                UseDefault = 1 << 0,
                MaximumChangeIsRelative = 1 << 1,
                Unknown = 1 << 2,
                Bit3 = 1 << 3,
                Fixed = 1 << 4,
                Unknown1 = 1 << 5,
                Fixed2 = 1 << 6
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class CameraFxBlingIntensityStruct : TagStructure
        {
            public CameraFxParameterFlagsNoAutoAdjust Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float BlingIntensity;
            public float MaximumChange;
            public float BlendSpeed;
            
            [Flags]
            public enum CameraFxParameterFlagsNoAutoAdjust : ushort
            {
                UseDefault = 1 << 0,
                MaximumChangeIsRelative = 1 << 1,
                Unknown = 1 << 2,
                Bit3 = 1 << 3,
                Fixed = 1 << 4,
                Unknown1 = 1 << 5,
                Fixed2 = 1 << 6
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class CameraFxBlingSizeStruct : TagStructure
        {
            public CameraFxParameterFlagsNoAutoAdjust Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float BlingLength;
            public float MaximumChange;
            public float BlendSpeed;
            
            [Flags]
            public enum CameraFxParameterFlagsNoAutoAdjust : ushort
            {
                UseDefault = 1 << 0,
                MaximumChangeIsRelative = 1 << 1,
                Unknown = 1 << 2,
                Bit3 = 1 << 3,
                Fixed = 1 << 4,
                Unknown1 = 1 << 5,
                Fixed2 = 1 << 6
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class CameraFxBlingAngleStruct : TagStructure
        {
            public CameraFxParameterFlagsNoAutoAdjust Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float BlingAngle;
            public float MaximumChange;
            public float BlendSpeed;
            
            [Flags]
            public enum CameraFxParameterFlagsNoAutoAdjust : ushort
            {
                UseDefault = 1 << 0,
                MaximumChangeIsRelative = 1 << 1,
                Unknown = 1 << 2,
                Bit3 = 1 << 3,
                Fixed = 1 << 4,
                Unknown1 = 1 << 5,
                Fixed2 = 1 << 6
            }
        }
        
        [TagStructure(Size = 0x4)]
        public class CameraFxBlingCountStruct : TagStructure
        {
            public CameraFxParameterFlagsBlingSpikes Flags;
            public short BlingSpikes;
            
            [Flags]
            public enum CameraFxParameterFlagsBlingSpikes : ushort
            {
                UseDefault = 1 << 0,
                MaximumChangeIsRelative = 1 << 1,
                Unknown = 1 << 2,
                DoubleSidedBling = 1 << 3,
                Fixed = 1 << 4,
                Unknown1 = 1 << 5,
                Fixed2 = 1 << 6
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class CameraFxSelfIllumPreferredStruct : TagStructure
        {
            public CameraFxParameterFlagsNoAutoAdjust Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // the preferred exposure for self illum
            public float PreferredExposure; // stops
            public float MaximumChange;
            public float BlendSpeed;
            
            [Flags]
            public enum CameraFxParameterFlagsNoAutoAdjust : ushort
            {
                UseDefault = 1 << 0,
                MaximumChangeIsRelative = 1 << 1,
                Unknown = 1 << 2,
                Bit3 = 1 << 3,
                Fixed = 1 << 4,
                Unknown1 = 1 << 5,
                Fixed2 = 1 << 6
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class CameraFxSelfIllumScaleStruct : TagStructure
        {
            public CameraFxParameterFlagsNoAutoAdjust Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // how much the self illum is allowed to change, as a percentage of the normal exposure change
            public float ExposureChange; // [0-1]
            public float MaximumChange;
            public float BlendSpeed;
            
            [Flags]
            public enum CameraFxParameterFlagsNoAutoAdjust : ushort
            {
                UseDefault = 1 << 0,
                MaximumChangeIsRelative = 1 << 1,
                Unknown = 1 << 2,
                Bit3 = 1 << 3,
                Fixed = 1 << 4,
                Unknown1 = 1 << 5,
                Fixed2 = 1 << 6
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class CameraFxColorGradingStruct : TagStructure
        {
            public CameraFxParameterFlagsNoAutoAdjust Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag ColorGradingTexture;
            
            [Flags]
            public enum CameraFxParameterFlagsNoAutoAdjust : ushort
            {
                UseDefault = 1 << 0,
                MaximumChangeIsRelative = 1 << 1,
                Unknown = 1 << 2,
                Bit3 = 1 << 3,
                Fixed = 1 << 4,
                Unknown1 = 1 << 5,
                Fixed2 = 1 << 6
            }
        }
        
        [TagStructure(Size = 0x20)]
        public class CameraFxFilmicToneCurveStruct : TagStructure
        {
            public CameraFxParameterFlagsEnabled Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // How intense the shoulder is
            public float ShoulderStrength;
            // How intense the linear portion is
            public float LinearStrength;
            // Angle of linear portion of curve
            public float LinearAngle;
            // How intense the toe is
            public float ToeStrength;
            // Numerator of toe slope
            public float ToeNumerator;
            // Denominator of toe slope
            public float ToeDenominator;
            // The white point in linear space
            public float LinearWhitePoint;
            
            [Flags]
            public enum CameraFxParameterFlagsEnabled : ushort
            {
                UseDefault = 1 << 0,
                MaximumChangeIsRelative = 1 << 1,
                Unknown = 1 << 2,
                Bit3 = 1 << 3,
                Fixed = 1 << 4,
                Enabled = 1 << 5,
                Fixed2 = 1 << 6
            }
        }
    }
}
