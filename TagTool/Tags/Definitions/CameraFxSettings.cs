using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "camera_fx_settings", Tag = "cfxs", Size = 0xE4, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
    [TagStructure(Name = "camera_fx_settings", Tag = "cfxs", Size = 0x170, MinVersion = CacheVersion.HaloOnline106708, Platform = CachePlatform.Original)]

    [TagStructure(Name = "camera_fx_settings", Tag = "cfxs", Size = 0x170, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    public class CameraFxSettings : TagStructure
	{
        public ExposureBlock Exposure;

        public CameraFxValue AutoExposureSensitivity;
        public CameraFxValue ExposureCompensation;

        public CameraFxStrength HighlightBloom;
        public CameraFxStrength InherentBloom;
        public CameraFxStrength LightingBloom;

        public CameraFxColor HighlightBloomTint;
        public CameraFxColor InherentBloomTint;
        public CameraFxColor LightingBloomTint;

        public CameraFxStrength UnknownStrength1;
        public CameraFxStrength UnknownStrength2;

        public UnknownBlock Unknown;

        public CameraFxStrength ConstantLight;
        public CameraFxStrength DynamicLight;

        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagField(MinVersion = CacheVersion.HaloOnline106708, Platform = CachePlatform.Original)]
        public SsaoPropertiesBlock SsaoProperties;

        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagField(MinVersion = CacheVersion.HaloOnline106708, Platform = CachePlatform.Original)]
        public CameraFxValue UnknownIntensity1;

        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagField(MinVersion = CacheVersion.HaloOnline106708, Platform = CachePlatform.Original)]
        public List<UnknownBlock1> Unknown33;

        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagField(MinVersion = CacheVersion.HaloOnline106708, Platform = CachePlatform.Original)]
        public List<UnknownBlock2> Unknown34;

        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagField(MinVersion = CacheVersion.HaloOnline106708, Platform = CachePlatform.Original)]
        public List<UnknownBlock3> Unknown35;

        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagField(MinVersion = CacheVersion.HaloOnline106708, Platform = CachePlatform.Original)]
        public List<UnknownBlock4> Unknown36;

        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagField(MinVersion = CacheVersion.HaloOnline106708, Platform = CachePlatform.Original)]
        public List<UnknownBlock5> Unknown37;

        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagField(MinVersion = CacheVersion.HaloOnline106708, Platform = CachePlatform.Original)]
        public List<UnknownBlock6> Unknown38;

        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagField(MinVersion = CacheVersion.HaloOnline106708, Platform = CachePlatform.Original)]
        public GodraysPropertiesBlock GodraysProperties;

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
        public class UnknownBlock : CameraFxStrength
        {
            public short Unknown24;
            public short Unknown25;
        }

        [TagStructure(Size = 0x10)]
        public class SsaoPropertiesBlock : TagStructure
        {
            public FlagsValue Flags;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused = new byte[2];

            public float SsaoRadius;
            public float SsaoBlurRadius;
            public float SsaoBackDropStrength;
        }

        [TagStructure(Size = 0x58)]
        public class UnknownBlock1 : TagStructure
		{
            public float Unknown;
            public int Unknown2;
            public TagFunction Unknown3 = new TagFunction { Data = new byte[0] };
            public TagFunction Unknown4 = new TagFunction { Data = new byte[0] };
            public TagFunction Unknown5 = new TagFunction { Data = new byte[0] };
            public TagFunction Unknown6 = new TagFunction { Data = new byte[0] };
        }

        [TagStructure(Size = 0xC)]
        public class UnknownBlock2 : TagStructure
		{
            public int Unknown;
            public float Unknown2;
            public float Unknown3;
        }

        [TagStructure(Size = 0x14)]
        public class UnknownBlock3 : TagStructure
		{
            public float Unknown;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;
        }

        [TagStructure(Size = 0x14)]
        public class UnknownBlock4 : TagStructure
		{
            public float Unknown;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;
        }

        [TagStructure(Size = 0x94)]
        public class UnknownBlock5 : TagStructure
		{
            public int Unknown;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;
            public float Unknown6;
            public float Unknown7;
            public float Unknown8;
            public float Unknown9;
            public float Unknown10;
            public float Unknown11;
            public float Unknown12;
            public float Unknown13;
            public float Unknown14;
            public float Unknown15;
            public float Unknown16;
            public float Unknown17;
            public float Unknown18;
            public float Unknown19;
            public float Unknown20;
            public float Unknown21;
            public float Unknown22;
            public float Unknown23;
            public float Unknown24;
            public float Unknown25;
            public float Unknown26;
            public float Unknown27;
            public float Unknown28;
            public float Unknown29;
            public float Unknown30;
            public float Unknown31;
            public float Unknown32;
            public float Unknown33;
            public float Unknown34;
            public float Unknown35;
            public float Unknown36;
            public float Unknown37;
        }

        [TagStructure(Size = 0x28)]
        public class UnknownBlock6 : TagStructure
		{
            public int Unknown;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;
            public float Unknown6;
            public float Unknown7;
            public float Unknown8;
            public float Unknown9;
            public float Unknown10;
        }

        [TagStructure(Size = 0x24)]
        public class GodraysPropertiesBlock : TagStructure
        {
            public FlagsValue Flags;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused = new byte[2];

            public float Radius;
            public float AngleBias;
            public RealRgbColor Color;
            public float Strength;
            public float PowerExponent;
            public float BlurSharpness;
            public float DecoratorDarkening;
            public float HemiRejectionFalloff;
        }
    }
}
