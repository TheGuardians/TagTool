using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using TagTool.Tags.Definitions.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "contrail_system", Tag = "cntl", Size = 0xC)]
    public class ContrailSystem : TagStructure
	{
        public List<ContrailDefinitionBlock> Contrails;

        [TagStructure(Size = 0x26C, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x294, MinVersion = CacheVersion.HaloReach)]
        public class ContrailDefinitionBlock : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;

            public float OriginFadeRange;
            public float OriginFadeCutoff;
            public float EdgeFadeRange;
            public float EdgeFadeCutoff;
            public float LodInDistance;
            public float LodFeatherInDistance;
            public float LodOutDistance;
            public float LodFeatherOutDistance;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float UnknownReach;

            public ContrailPropertyScalar EmissionRate;
            public ContrailPropertyScalar ProfileLifespan;
            public ContrailPropertyRealVector3d ProfileSelfAcceleration;
            public ContrailPropertyScalar ProfileSize;
            public ContrailPropertyRealPoint2d ProfileOffset;
            public ContrailPropertyScalar ProfileRotation;
            public ContrailPropertyScalar ProfileRotationRate;

            public ContrailAppearanceFlags AppearanceFlags;
            public ContrailProfileShapeEnum ProfileShape;
            public byte NumberOfNgonSides;

            public RenderMethod RenderMethod;

            public RealPoint2d Tiling;
            public RealPoint2d ScrollSpeed;

            public ContrailPropertyColor ProfileColor;
            public ContrailPropertyScalar ProfileAlpha;
            public ContrailPropertyScalar ProfileSecondaryAlpha;
            public ContrailPropertyScalar ProfileBlackPoint;
            public ContrailPropertyScalar ProfilePalette;
            public ContrailPropertyScalar ProfileIntensity;

            public uint RuntimeMConstantPerProfileProperties;
            public uint RuntimeMUsedStates;
            public RuntimeGpuData RuntimeMGpuData;

            [Flags]
            public enum ContrailAppearanceFlags : ushort
            {
                TintFromLightmap = 1 << 0,
                Doublesided = 1 << 1,
                ProfileOpacityFromScaleA = 1 << 2,
                RandomUOffset = 1 << 3,
                RandomVOffset = 1 << 4
            }

            public enum ContrailProfileShapeEnum : sbyte
            {
                AlignedRibbon,
                Cross,
                Ngon
            }

            [TagStructure(Size = 0x20)]
            public class ContrailPropertyScalar : TagStructure
            {
                public ContrailStateInputEnum InputVariable;
                public ContrailStateInputEnum RangeVariable;
                public OutputModEnum OutputModifier;
                public ContrailStateInputEnum OutputModifierInput;
                public byte[] MappingFunction;
                public float RuntimeMConstantValue;
                public sbyte RuntimeMFlags;
                [TagField(Length = 3, Flags = TagFieldFlags.Padding)]
                public byte[] DSFDSGLKJ;

                public enum ContrailStateInputEnum : sbyte
                {
                    ProfileAge,
                    ProfileRandom,
                    ProfileCorrelation1,
                    ProfileCorrelation2,
                    ProfileCorrelation3,
                    ProfileCorrelation4,
                    GameTime,
                    ContrailAge,
                    ContrailRandom,
                    ContrailCorrelation1,
                    ContrailCorrelation2,
                    ContrailSpeed,
                    ContrailLod,
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

            [TagStructure(Size = 0x38)]
            public class ContrailPropertyRealVector3d : TagStructure
            {
                public ContrailStateInputEnum InputVariable;
                public ContrailStateInputEnum RangeVariable;
                public OutputModEnum OutputModifier;
                public ContrailStateInputEnum OutputModifierInput;
                public byte[] MappingFunction;
                public float RuntimeMConstantValue;
                public sbyte RuntimeMFlags;
                [TagField(Length = 3, Flags = TagFieldFlags.Padding)]
                public byte[] DSFDSGLKJ;
                public RealVector3d StartingInterpolant;
                public RealVector3d EndingInterpolant;

                public enum ContrailStateInputEnum : sbyte
                {
                    ProfileAge,
                    ProfileRandom,
                    ProfileCorrelation1,
                    ProfileCorrelation2,
                    ProfileCorrelation3,
                    ProfileCorrelation4,
                    GameTime,
                    ContrailAge,
                    ContrailRandom,
                    ContrailCorrelation1,
                    ContrailCorrelation2,
                    ContrailSpeed,
                    ContrailLod,
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

            [TagStructure(Size = 0x30)]
            public class ContrailPropertyRealPoint2d : TagStructure
            {
                public ContrailStateInputEnum InputVariable;
                public ContrailStateInputEnum RangeVariable;
                public OutputModEnum OutputModifier;
                public ContrailStateInputEnum OutputModifierInput;
                public byte[] MappingFunction;
                public float RuntimeMConstantValue;
                public sbyte RuntimeMFlags;
                [TagField(Length = 3, Flags = TagFieldFlags.Padding)]
                public byte[] DSFDSGLKJ;
                public RealPoint2d StartingInterpolant;
                public RealPoint2d EndingInterpolant;

                public enum ContrailStateInputEnum : sbyte
                {
                    ProfileAge,
                    ProfileRandom,
                    ProfileCorrelation1,
                    ProfileCorrelation2,
                    ProfileCorrelation3,
                    ProfileCorrelation4,
                    GameTime,
                    ContrailAge,
                    ContrailRandom,
                    ContrailCorrelation1,
                    ContrailCorrelation2,
                    ContrailSpeed,
                    ContrailLod,
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
            public class ContrailPropertyColor : TagStructure
            {
                public ContrailStateInputEnum InputVariable;
                public ContrailStateInputEnum RangeVariable;
                public OutputModEnum OutputModifier;
                public ContrailStateInputEnum OutputModifierInput;
                public byte[] MappingFunction;
                public float RuntimeMConstantValue;
                public sbyte RuntimeMFlags;
                [TagField(Length = 3, Flags = TagFieldFlags.Padding)]
                public byte[] DSFDSGLKJ;

                public enum ContrailStateInputEnum : sbyte
                {
                    ProfileAge,
                    ProfileRandom,
                    ProfileCorrelation1,
                    ProfileCorrelation2,
                    ProfileCorrelation3,
                    ProfileCorrelation4,
                    GameTime,
                    ContrailAge,
                    ContrailRandom,
                    ContrailCorrelation1,
                    ContrailCorrelation2,
                    ContrailSpeed,
                    ContrailLod,
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
        }
    }
}
