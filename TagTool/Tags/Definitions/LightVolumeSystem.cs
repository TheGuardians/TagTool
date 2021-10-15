using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using System;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "light_volume_system", Tag = "ltvl", Size = 0xC, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "light_volume_system", Tag = "ltvl", Size = 0x14, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "light_volume_system", Tag = "ltvl", Size = 0xC, MaxVersion = CacheVersion.HaloReach)]
    public class LightVolumeSystem : TagStructure
	{
        public List<LightVolumeDefinitionBlock> LightVolumes;

        [TagField(Flags = Padding, Length = 8, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] Unused1;

        [TagStructure(Size = 0x17C, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x1C8, Align = 0x8, MinVersion = CacheVersion.HaloReach)]
        public class LightVolumeDefinitionBlock : TagStructure
		{
            public StringId LightVolumeName;

            public RenderMethod RenderMethod;

            public ushort AppearanceFlags;
            [TagField(Length = 2, Flags = Padding)]
            public byte[] Padd0;

            /// <summary>
            /// Average brightness head-on/side-view.
            /// </summary>
            public float BrightnessRatio;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int Unknown1;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown2;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown3;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown4;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown5;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown6;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown7;

            public LightVolumePropertyReal Length;
            public LightVolumePropertyReal Offset;
            public LightVolumePropertyReal ProfileDensity;
            public LightVolumePropertyReal ProfileLength;
            public LightVolumePropertyReal ProfileThickness;
            public LightVolumePropertyRealRgbColor ProfileColor;
            public LightVolumePropertyReal ProfileAlpha;
            public LightVolumePropertyReal ProfileIntensity;
            public int RuntimeMConstantPerProfileProperties;
            public int RuntimeMUsedStates;
            public int RuntimeMMaxProfileCount;
            public GpuPropertyFunctionColorStruct RuntimeMGpuData;

            [TagStructure(Size = 0x20)]
            public class LightVolumePropertyReal : TagStructure
            {
                public LightVolumeStateInputEnum InputVariable;
                public LightVolumeStateInputEnum RangeVariable;
                public OutputModEnum OutputModifier;
                public LightVolumeStateInputEnum OutputModifierInput;
                public byte[] MappingFunction;
                public float RuntimeMConstantValue;
                public sbyte RuntimeMFlags;
                [TagField(Length = 3, Flags = TagFieldFlags.Padding)]
                public byte[] DSFDSGLKJ;

                public enum LightVolumeStateInputEnum : sbyte
                {
                    ProfilePosition,
                    GameTime,
                    LightVolumeAge,
                    LightVolumeRandom,
                    LightVolumeCorrelation1,
                    LightVolumeCorrelation2,
                    LightVolumeLod,
                    EffectAScale,
                    EffectBScale,
                    InvalidStatePleaseSetAgain
                }

                public enum OutputModEnum : sbyte
                {
                    Unknown,
                    Plus,
                    Times
                }
            }

            [TagStructure(Size = 0x20)]
            public class LightVolumePropertyRealRgbColor : TagStructure
            {
                public LightVolumeStateInputEnum InputVariable;
                public LightVolumeStateInputEnum RangeVariable;
                public OutputModEnum OutputModifier;
                public LightVolumeStateInputEnum OutputModifierInput;
                public byte[] MappingFunction;
                public float RuntimeMConstantValue;
                public sbyte RuntimeMFlags;
                [TagField(Length = 3, Flags = TagFieldFlags.Padding)]
                public byte[] DSFDSGLKJ;

                public enum LightVolumeStateInputEnum : sbyte
                {
                    ProfilePosition,
                    GameTime,
                    LightVolumeAge,
                    LightVolumeRandom,
                    LightVolumeCorrelation1,
                    LightVolumeCorrelation2,
                    LightVolumeLod,
                    EffectAScale,
                    EffectBScale,
                    InvalidStatePleaseSetAgain
                }

                public enum OutputModEnum : sbyte
                {
                    Unknown,
                    Plus,
                    Times
                }
            }

            [TagStructure(Size = 0x24)]
            public class GpuPropertyFunctionColorStruct : TagStructure
            {
                public List<GpuPropertyBlock> RuntimeGpuPropertyBlock;
                public List<GpuFunctionBlock> RuntimeGpuFunctionsBlock;
                public List<GpuColorBlock> RuntimeGpuColorsBlock;

                [TagStructure(Size = 0x10)]
                public class GpuPropertyBlock : TagStructure
                {
                    [TagField(Length = 4)]
                    public GpuPropertySubArray[] RuntimeGpuPropertySubArray;

                    [TagStructure(Size = 0x4)]
                    public class GpuPropertySubArray : TagStructure
                    {
                        public float RuntimeGpuPropertyReal;
                    }
                }

                [TagStructure(Size = 0x40)]
                public class GpuFunctionBlock : TagStructure
                {
                    [TagField(Length = 16)]
                    public GpuFunctionSubArray[] RuntimeGpuFunctionSubArray;

                    [TagStructure(Size = 0x4)]
                    public class GpuFunctionSubArray : TagStructure
                    {
                        public float RuntimeGpuFunctionReal;
                    }
                }

                [TagStructure(Size = 0x10)]
                public class GpuColorBlock : TagStructure
                {
                    [TagField(Length = 4)]
                    public GpuColorSubArray[] RuntimeGpuColorSubArray;

                    [TagStructure(Size = 0x4)]
                    public class GpuColorSubArray : TagStructure
                    {
                        public float RuntimeGpuColorReal;
                    }
                }
            }
        }
    }
}