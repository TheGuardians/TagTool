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

            public float OriginFadeRange; // distance beyond cutoff over which contrails fade (world units)
            public float OriginFadeCutoff; // distance from contrail origin where fade begins (world units)
            public float EdgeFadeRange; // degrees beyond cutoff over which contrails fade (degrees)
            public float EdgeFadeCutoff; // degrees away from edge-on where fade is total (degrees)
            public float LodInDistance; // world units
            public float LodFeatherInDistance; // world units
            public float LodOutDistance; // world units
            public float LodFeatherOutDistance; // world units

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float UnknownReach;

            public ContrailPropertyScalar EmissionRate; // profiles per second
            public ContrailPropertyScalar ProfileLifespan; // seconds
            public ContrailPropertyRealVector3d ProfileSelfAcceleration; // world units per second per second
            public ContrailPropertyScalar ProfileSize; // world units
            public ContrailPropertyRealPoint2d ProfileOffset; // world units
            public ContrailPropertyScalar ProfileRotation; // degrees
            public ContrailPropertyScalar ProfileRotationRate; // degrees per second

            public ContrailAppearanceFlags AppearanceFlags;
            public ContrailProfileShapeEnum ProfileShape;
            public byte NumberOfNgonSides;

            public RenderMethod RenderMethod;

            public RealPoint2d Tiling; // u is tiles/world unit, v is absolute tiles (u lengthwise, v crosswise)
            public RealPoint2d ScrollSpeed; // tiles per second

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

                [TagField(Length = 3, Flags = Padding)]
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

                [TagField(Length = 3, Flags = Padding)]
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

                [TagField(Length = 3, Flags = Padding)]
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

                [TagField(Length = 3, Flags = Padding)]
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
