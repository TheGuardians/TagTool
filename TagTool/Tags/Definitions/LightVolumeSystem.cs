using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using System;
using static TagTool.Tags.TagFieldFlags;
using TagTool.Tags.Definitions.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "light_volume_system", Tag = "ltvl", Size = 0xC, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "light_volume_system", Tag = "ltvl", Size = 0x14, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "light_volume_system", Tag = "ltvl", Size = 0xC, MaxVersion = CacheVersion.HaloReach11883)]
    public class LightVolumeSystem : TagStructure
	{
        public List<LightVolumeDefinitionBlock> LightVolumes;

        [TagField(Flags = Padding, Length = 8, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] Unused1;

        [TagStructure(Size = 0x17C, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x1C8, MinVersion = CacheVersion.HaloReach)]
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
            public LightVolumeFlags Flags;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float LodInDistance;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float LodFeatherInDelta;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float InverseLodFeatherIn;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float LodOutDistance;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float LodFeatherOutDelta;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float InverseLodFeatherOut;

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
            public RuntimeGpuData RuntimeMGpuData;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<LightVolumePrecompiledVertBlock> PrecompiledVertices;

            [Flags]
            public enum LightVolumeFlags : uint
            {
                // if not checked, the following flags do not matter, nor do LOD parameters below
                LodEnabled = 1 << 0,
                LodAlways10 = 1 << 1,
                LodSameInSplitscreen = 1 << 2,
                DisablePrecompiledProfiles = 1 << 3,
                ForcePrecompileProfiles = 1 << 4,
                CanBeLowRes = 1 << 5,
                Precompiled = 1 << 6
            }

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

            [TagStructure(Size = 0x8)]
            public class LightVolumePrecompiledVertBlock : TagStructure
            {
                public ushort R;
                public ushort G;
                public ushort B;
                public ushort Thickness;
            }
        }
    }
}