using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound_cache_file_gestalt", Tag = "ugh!", Size = 0xB8, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "sound_cache_file_gestalt", Tag = "ugh!", Size = 0xC4, MinVersion = CacheVersion.Halo3ODST)]
    public class SoundCacheFileGestalt
    {
        public List<PlatformCodecBlock> PlatformCodecs;
        public List<PlaybackParameter> PlaybackParameters;
        public List<Scale> Scales;
        public List<ImportName> ImportNames;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<UnknownBlock> Unknown;

        public List<PitchRangeParametersBlock> PitchRangeParameters;
        public List<PitchRange> PitchRanges;
        public List<Permutation> Permutations;
        public List<CustomPlayback> CustomPlaybacks;
        public List<LanguageBlock> Languages;
        public List<RuntimePermutationFlag> RuntimePermutationFlags;
        public TagFunction Unknown2 = new TagFunction { Data = new byte[0] };
        public uint Unknown3;
        public uint Unknown4;
        public List<PermutationChunk> PermutationChunks;
        public List<Promotion> Promotions;
        public List<ExtraInfoBlock> ExtraInfo;

        [TagStructure(Size = 0x3, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x4, MinVersion = CacheVersion.HaloOnline106708)]
        public class PlatformCodecBlock
        {
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public short Unknown;

            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public Sound.SampleRateValue SampleRate;

            public Sound.EncodingValue Encoding;

            public sbyte Compression;
        }

        [TagStructure(Size = 0x44)]
        public class PlaybackParameter
        {
            public FieldDisableFlagsValue FieldDisableFlags;
            public float DistanceA;
            public float DistanceB;
            public float DistanceC;
            public float DistanceD;
            public float SkipFraction;
            public float MaximumBendPerSecond;
            public float GainBase;
            public float GainVariance;
            public Bounds<short> RandomPitchBounds;
            public Bounds<Angle> ConeAngleBounds;
            public float OuterConeGain;
            public FlagsValue Flags;
            public Angle Azimuth;
            public float PositionalGain;
            public float FirstPersonGain;

            [Flags]
            public enum FieldDisableFlagsValue : int
            {
                None = 0,
                DistanceA = 1 << 0,
                DistanceB = 1 << 1,
                DistanceC = 1 << 2,
                DistanceD = 1 << 3,
                Bit4 = 1 << 4,
                Bit5 = 1 << 5,
                Bit6 = 1 << 6,
                Bit7 = 1 << 7,
                Bit8 = 1 << 8,
                Bit9 = 1 << 9,
                Bit10 = 1 << 10,
                Bit11 = 1 << 11,
                Bit12 = 1 << 12,
                Bit13 = 1 << 13,
                Bit14 = 1 << 14,
                Bit15 = 1 << 15,
                Bit16 = 1 << 16,
                Bit17 = 1 << 17,
                Bit18 = 1 << 18,
                Bit19 = 1 << 19,
                Bit20 = 1 << 20,
                Bit21 = 1 << 21,
                Bit22 = 1 << 22,
                Bit23 = 1 << 23,
                Bit24 = 1 << 24,
                Bit25 = 1 << 25,
                Bit26 = 1 << 26,
                Bit27 = 1 << 27,
                Bit28 = 1 << 28,
                Bit29 = 1 << 29,
                Bit30 = 1 << 30,
                Bit31 = 1 << 31
            }

            [Flags]
            public enum FlagsValue : int
            {
                None = 0,
                OverrideAzimuth = 1 << 0,
                Override3dGain = 1 << 1,
                OverrideSpeakerGain = 1 << 2,
            }
        }

        [TagStructure(Size = 0x14)]
        public class Scale
        {
            public Bounds<float> GainModifierBounds;
            public Bounds<short> PitchModifierBounds;
            public Bounds<float> SkipFractionModifierBounds;
        }

        [TagStructure(Size = 0x4)]
        public class ImportName
        {
            public StringId Name;
        }

        [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3ODST)]
        public class UnknownBlock
        {
            public uint Unknown;
            public List<UnknownBlock2> Unknown2;

            [TagStructure(Size = 0x4)]
            public class UnknownBlock2
            {
                public uint Unknown;
            }
        }

        [TagStructure(Size = 0xE, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3ODST)]
        public class PitchRangeParametersBlock
        {
            public short NaturalPitch;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public short Unknown;

            public Bounds<short> BendBounds;
            public Bounds<short> MaxGainPitchBounds;
            public Bounds<short> UnknownBounds;
        }

        [TagStructure(Size = 0xC, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x38, MinVersion = CacheVersion.HaloOnline106708)]
        public class PitchRange
        {
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public short ImportNameIndex;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public StringId ImportName;

            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public short PitchRangeParametersIndex;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public PitchRangeParametersBlock PitchRangeParameters;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public uint Unknown1;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public uint Unknown2;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public uint Unknown3;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public uint Unknown4;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public short Unknown5;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public short Unknown6;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public byte PermutationCount;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public byte Unknown7;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public short Unknown8;


            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public short EncodedPermutationDataIndex;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public short EncodedRuntimePermutationFlagIndex;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public short EncodedPermutationCount;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public short FirstPermutationIndex;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public List<Permutation> Permutations;
        }

        [TagStructure(Size = 0x10, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x2C, MinVersion = CacheVersion.HaloOnline106708)]
        public class Permutation
        {
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public short ImportNameIndex;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public StringId ImportName;

            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public short EncodedSkipFraction;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public Bounds<float> SkipFraction;

            public uint SampleSize;

            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public int FirstPermutationChunkIndex;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public short PermutationChunkCount;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public sbyte Gain;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public sbyte OverallPermutationIndex;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public uint PermutationNumber;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public uint IsNotFirstPermutation;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public List<PermutationChunk> PermutationChunks;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public uint Unknown1;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public uint Unknown2;
        }

        [TagStructure(Size = 0x48, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x54, MinVersion = CacheVersion.HaloOnline106708)]
        public class CustomPlayback
        {
            public uint Unknown1;
            public uint Unknown2;
            public uint Unknown3;
            public FlagsValue Flags;
            public uint Unknown4;
            public uint Unknown5;
            public List<FilterBlock> Filter;
            public uint Unknown6;
            public uint Unknown7;
            public uint Unknown8;
            public uint Unknown9;
            public uint Unknown10;
            public uint Unknown11;
            public uint Unknown12;
            public uint Unknown13;
            public uint Unknown14;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public uint Unknown15;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public uint Unknown16;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public uint Unknown17;

            [Flags]
            public enum FlagsValue : int
            {
                None = 0,
                Use3dRadioHack = 1 << 0,
            }

            [TagStructure(Size = 0x48)]
            public class FilterBlock
            {
                public int FilterType;
                public int FilterWidth;

                public Bounds<float> ScaleBounds1;
                public float RandomBase1;
                public float RandomVariance1;

                public Bounds<float> ScaleBounds2;
                public float RandomBase2;
                public float RandomVariance2;

                public Bounds<float> ScaleBounds3;
                public float RandomBase3;
                public float RandomVariance3;

                public Bounds<float> ScaleBounds4;
                public float RandomBase4;
                public float RandomVariance4;
            }
        }

        [TagStructure(Size = 0x1C)]
        public class LanguageBlock
        {
            public GameLanguage Language;
            public List<PermutationDurationBlock> PermutationDurations;
            public List<PitchRangeDurationBlock> PitchRangeDurations;

            [TagStructure(Size = 0x2)]
            public class PermutationDurationBlock
            {
                public short FrameCount;
            }

            [TagStructure(Size = 0x4)]
            public class PitchRangeDurationBlock
            {
                public short PermutationStartIndex;
                public short PermutationCount;
            }
        }

        [TagStructure(Size = 0x1)]
        public class RuntimePermutationFlag
        {
            public sbyte Unknown;
        }

        [TagStructure(Size = 0x14)]
        public class PermutationChunk
        {
            public uint Offset;

            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public FlagsValue Flags;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public byte Unknown1; // size extra byte for big endian

            public ushort Size;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public byte Unknown2; // size extra byte for little endian
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public byte Unknown3; // always 4?

            public int RuntimeIndex;
            public int UnknownA;
            public int UnknownSize;

            [Flags]
            public enum FlagsValue : byte
            {
                None = 0,
                Bit0 = 1 << 0,
                Bit1 = 1 << 1,
                Bit2 = 1 << 2,
                Bit3 = 1 << 3,
                Bit4 = 1 << 4,
                HasUnknownAValue = 1 << 5,
                Bit6 = 1 << 6,
                Bit7 = 1 << 7
            }
        }

        [TagStructure(Size = 0x24, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x30, MinVersion = CacheVersion.HaloOnline106708)]
        public class Promotion
        {
            public List<Rule> Rules;
            public List<RuntimeTimer> RuntimeTimers;
            public int Unknown1;
            public uint Unknown2;
            public uint Unknown3;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public uint LongestPermutationDuration;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public uint TotalSampleSize;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public uint Unknown11;

            [TagStructure(Size = 0x10)]
            public class Rule
            {
                public short PitchRangeIndex;
                public short MaximumPlayingCount;
                public float SuppressionTime;
                public int Unknown;
                public int Unknown2;
            }

            [TagStructure(Size = 0x4)]
            public class RuntimeTimer
            {
                public int Unknown;
            }
        }

        [TagStructure(Size = 0xC, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x28, MinVersion = CacheVersion.HaloOnline106708)]
        public class ExtraInfoBlock
        {
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public List<LanguagePermutation> LanguagePermutations;

            public List<EncodedPermutationSection> EncodedPermutationSections;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public uint Unknown1;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public uint Unknown2;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public uint Unknown3;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public uint Unknown4;

            [TagStructure(Size = 0xC)]
            public class LanguagePermutation
            {
                public List<RawInfoBlock> RawInfo;

                [TagStructure(Size = 0x7C)]
                public class RawInfoBlock
                {
                    public StringId SkipFractionName;
                    public uint Unknown1;
                    public uint Unknown2;
                    public uint Unknown3;
                    public uint Unknown4;
                    public uint Unknown5;
                    public uint Unknown6;
                    public uint Unknown7;
                    public uint Unknown8;
                    public uint Unknown9;
                    public uint Unknown10;
                    public uint Unknown11;
                    public uint Unknown12;
                    public uint Unknown13;
                    public uint Unknown14;
                    public uint Unknown15;
                    public uint Unknown16;
                    public uint Unknown17;
                    public uint Unknown18;
                    public List<Unknown> UnknownList;
                    public short Compression;
                    public byte Language;
                    public byte Unknown19;
                    public uint SampleCount;
                    public uint ResourceSampleOffset;
                    public uint ResourceSampleSize;
                    public uint Unknown20;
                    public uint Unknown21;
                    public uint Unknown22;
                    public uint Unknown23;
                    public int Unknown24;

                    [TagStructure(Size = 0x18)]
                    public class Unknown
                    {
                        public uint Unknown1;
                        public uint Unknown2;
                        public uint Unknown3;
                        public uint Unknown4;
                        public uint Unknown5;
                        public uint Unknown6;
                    }
                }
            }

            [TagStructure(Size = 0x2C)]
            public class EncodedPermutationSection
            {
                public byte[] EncodedData;
                public List<SoundDialogueInfoBlock> SoundDialogueInfo;
                public List<UnknownBlock> Unknown;

                [TagStructure(Size = 0x10)]
                public class SoundDialogueInfoBlock
                {
                    public uint MouthDataOffset;
                    public uint MouthDataLength;
                    public uint LipsyncDataOffset;
                    public uint LipsynceDataLength;
                }

                [TagStructure(Size = 0xC)]
                public class UnknownBlock
                {
                    [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                    public uint Unknown1;
                    [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                    public uint Unknown2;
                    [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                    public uint Unknown3;

                    [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                    public List<UnknownBlock2> Unknown;

                    [TagStructure(Size = 0x28)]
                    public class UnknownBlock2
                    {
                        public uint Unknown;
                        public uint Unknown2;
                        public uint Unknown3;
                        public uint Unknown4;
                        public List<UnknownBlock> Unknown5;
                        public List<UnknownBlock2_2> Unknown6;

                        [TagStructure(Size = 0x8)]
                        public class UnknownBlock
                        {
                            public uint Unknown;
                            public uint Unknown2;
                        }

                        [TagStructure(Size = 0x8)]
                        public class UnknownBlock2_2
                        {
                            public short Unknown;
                            public short Unknown2;
                            public short Unknown3;
                            public short Unknown4;
                        }
                    }
                }
            }
        }
    }
}