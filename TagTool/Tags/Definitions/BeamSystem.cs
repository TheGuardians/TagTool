using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using TagTool.Tags.Definitions.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "beam_system", Tag = "beam", Size = 0xC, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "beam_system", Tag = "beam", Size = 0x18, MinVersion = CacheVersion.HaloOnlineED)]
    public class BeamSystem : TagStructure
	{
        public List<BeamDefinitionBlock> Beams;

        [TagField(Flags = Padding, Length = 12, MinVersion = CacheVersion.HaloOnlineED)]
        public byte[] Unused1; // can prob yeet this

        [TagStructure(Size = 0x208)]
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
                Doublesided = 1 << 0
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
                public byte[] MappingFunction;
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

            [TagStructure(Size = 0x30)]
            public class BeamPropertyRealPoint2d : TagStructure
            {
                public BeamStateInputEnum InputVariable;
                public BeamStateInputEnum RangeVariable;
                public OutputModEnum OutputModifier;
                public BeamStateInputEnum OutputModifierInput;
                public byte[] MappingFunction;
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
                public byte[] MappingFunction;
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
