using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using TagTool.Tags.Definitions.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "beam_system", Tag = "beam", Size = 0xC, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "beam_system", Tag = "beam", Size = 0x18, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "beam_system", Tag = "beam", Size = 0xC, MinVersion = CacheVersion.HaloReach)]
    public class BeamSystem : TagStructure
	{
        public List<BeamDefinitionBlock> Beams;

        [TagField(Flags = Padding, Length = 12, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] Unused1; // can prob yeet this

        [TagStructure(Size = 0x208, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x238, MinVersion = CacheVersion.HaloReach)]
        public class BeamDefinitionBlock : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;

            public RenderMethod RenderMethod;

            public BeamAppearanceFlags AppearanceFlags;
            public BeamProfileShapeEnum ProfileShape;
            public byte NumberOfNgonSides;

            public RealVector2d UvTiling;
            public RealVector2d UvScrolling;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float AngleFadeRange; // radial (from beam axis) degrees beyond beginning angle over which beam fades (degrees)
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float AngleFadeBegin; // radial (from beam axis) degrees away from face-on where fade begins (degrees)
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short SortBias; // use values between -10 and 10 to move closer and farther from camera (positive is closer)

            [TagField(MinVersion = CacheVersion.HaloReach, Length = 0x2, Flags = Padding)]
            public byte[] PaddingReach;

            public float OriginFadeRange;
            public float OriginFadeCutoff;
            public float EdgeFadeRange;
            public float EdgeFadeCutoff;

            public BeamPropertyReal Length;
            public BeamPropertyReal Offset;
            public BeamPropertyReal ProfileDensity;
            public BeamPropertyRealPoint2d ProfileOffset;
            public BeamPropertyReal ProfileRotation;
            public BeamPropertyReal ProfileThickness;
            public BeamPropertyRealRgbColor ProfileColor;
            public BeamPropertyReal ProfileAlpha;
            public BeamPropertyReal ProfileBlackPoint;
            public BeamPropertyReal ProfilePalette;
            public BeamPropertyReal ProfileIntensity;

            public int RuntimeMConstantPerProfileProperties;
            public int RuntimeMUsedStates;
            public int RuntimeMMaxProfileCount;

            public RuntimeGpuData RuntimeMGpuData;

            [Flags]
            public enum BeamAppearanceFlags : ushort
            {
                DoubleSided = 1 << 0,
                OriginFaded = 1 << 1,
                EdgeFaded = 1 << 2,
                Fogged = 1 << 3
            }

            public enum BeamProfileShapeEnum : byte
            {
                AlignedRibbon,
                Cross,
                Ngon
            }

            [TagStructure(Size = 0x20)]
            public class BeamPropertyReal : TagStructure
            {
                public BeamStateInputEnum InputVariable;
                public BeamStateInputEnum RangeVariable;
                public OutputModEnum OutputModifier;
                public BeamStateInputEnum OutputModifierInput;
                public TagFunction MappingFunction;
                public float RuntimeMConstantValue;
                public byte RuntimeMFlags;
                [TagField(Length = 3, Flags = Padding)]
                public byte[] Padding0;

                public enum BeamStateInputEnum : byte
                {
                    ProfilePosition,
                    ProfileUncappedPosition,
                    GameTime,
                    BeamAge,
                    BeamRandom,
                    BeamCorrelation1,
                    BeamCorrelation2,
                    BeamLength,
                    BeamLod,
                    EffectAScale,
                    EffectBScale,
                    InvalidStatePleaseSetAgain
                }

                public enum OutputModEnum : byte
                {
                    Unknown,
                    Plus,
                    Times
                }
            }

            [TagStructure(Size = 0x30)]
            public class BeamPropertyRealPoint2d : TagStructure
            {
                public BeamStateInputEnum InputVariable;
                public BeamStateInputEnum RangeVariable;
                public OutputModEnum OutputModifier;
                public BeamStateInputEnum OutputModifierInput;
                public TagFunction MappingFunction;
                public float RuntimeMConstantValue;
                public byte RuntimeMFlags;
                [TagField(Length = 3, Flags = TagFieldFlags.Padding)]
                public byte[] DSFDSGLKJ;
                public RealPoint2d StartingInterpolant;
                public RealPoint2d EndingInterpolant;

                public enum BeamStateInputEnum : byte
                {
                    ProfilePosition,
                    ProfileUncappedPosition,
                    GameTime,
                    BeamAge,
                    BeamRandom,
                    BeamCorrelation1,
                    BeamCorrelation2,
                    BeamLength,
                    BeamLod,
                    EffectAScale,
                    EffectBScale,
                    InvalidStatePleaseSetAgain
                }

                public enum OutputModEnum : byte
                {
                    Unknown,
                    Plus,
                    Times
                }
            }

            [TagStructure(Size = 0x20)]
            public class BeamPropertyRealRgbColor : TagStructure
            {
                public BeamStateInputEnum InputVariable;
                public BeamStateInputEnum RangeVariable;
                public OutputModEnum OutputModifier;
                public BeamStateInputEnum OutputModifierInput;
                public TagFunction MappingFunction;
                public float RuntimeMConstantValue;
                public byte RuntimeMFlags;
                [TagField(Length = 3, Flags = TagFieldFlags.Padding)]
                public byte[] DSFDSGLKJ;

                public enum BeamStateInputEnum : byte
                {
                    ProfilePosition,
                    ProfileUncappedPosition,
                    GameTime,
                    BeamAge,
                    BeamRandom,
                    BeamCorrelation1,
                    BeamCorrelation2,
                    BeamLength,
                    BeamLod,
                    EffectAScale,
                    EffectBScale,
                    InvalidStatePleaseSetAgain
                }

                public enum OutputModEnum : byte
                {
                    Unknown,
                    Plus,
                    Times
                }
            }
        }
    }
}
