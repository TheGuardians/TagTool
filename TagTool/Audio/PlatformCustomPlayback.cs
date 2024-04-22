using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;


namespace TagTool.Audio
{
    [TagStructure(Size = 0x34, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0x48, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x54, MinVersion = CacheVersion.HaloOnlineED)]
    public class PlatformCustomPlayback : TagStructure
	{
        public List<PlatformSoundOverrideMixbins> OverrideMixbins;

        public FlagsValue Flags;

        [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;

        public List<FilterBlock> Filter;
        public List<PitchLfoBlock> PitchLfo;
        public List<FilterLfoBlock> FilterLfo;
        public List<SoundEffectPlaybackBlock> SoundEffect;

        [TagField(Length = 0xC, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.HaloOnlineED)]
        public byte[] Padding2;

        [TagStructure(Size = 0x8)]
        public class PlatformSoundOverrideMixbins : TagStructure
        {
            public MixbinValue Mixbin;
            public float Gain; // dB

            public enum MixbinValue : int
            {
                FrontLeft,
                FrontRight,
                BackLeft,
                BackRight,
                Center,
                LowFrequency,
                Reverb,
                _3dFrontLeft,
                _3dFrontRight,
                _3dBackLeft,
                _3dBackRight,
                DefaultLeftSpeakers,
                DefaultRightSpeakers
            }
        }

        [Flags]
        public enum FlagsValue : int
        {
            None = 0,
            Use3dRadioHack = 1 << 0,
        }

        [TagStructure(Size = 0x10)]
        public class FilterParameters : TagStructure
        {
            public Bounds<float> ScaleBounds;
            public Bounds<float> RandomBaseAndVariance;
        }

        [TagStructure(Size = 0x48)]
        public class FilterBlock : TagStructure
		{
            public FilterTypeValue FilterType;

            // [0,7]
            public int FilterWidth;

            /// <summary>
            /// in Hz [0,8000]
            /// </summary>
            public FilterParameters LeftFilterFrequency;
            /// <summary>
            /// in dB
            /// </summary>
            public FilterParameters LeftFilterGain;
            /// <summary>
            /// in Hz [0,8000]
            /// </summary>
            public FilterParameters RightFilterFrequency;
            /// <summary>
            /// in dB
            /// </summary>
            public FilterParameters RightFilterGain;

            public enum FilterTypeValue : int
            {
                ParametricEq,
                Dls2,
                BothOnlyValidForMono
            }

        }

        [TagStructure(Size = 0x30)]
        public class PitchLfoBlock : TagStructure
        {
            /// <summary>
            /// seconds
            /// </summary>
            public FilterParameters Delay;
            /// <summary>
            /// Hz[0,43.7]
            /// </summary>
            public FilterParameters Frequency;
            /// <summary>
            /// octaves[-1,1]
            /// </summary>
            public FilterParameters PitchModulation;
        }

        [TagStructure(Size = 0x40)]
        public class FilterLfoBlock : TagStructure
        {
            /// <summary>
            /// in seconds
            /// </summary>
            public FilterParameters Delay;
            /// <summary>
            /// in Hz[0,43.7]
            /// </summary>
            public FilterParameters Frequency;
            /// <summary>
            /// octaves[-8,8]
            /// </summary>
            public FilterParameters CutoffModulation;
            /// <summary>
            /// dB[-16,16]
            /// </summary>
            public FilterParameters GainModulation;
        }

        [TagStructure(Size = 0x28, MinVersion = CacheVersion.Halo2Beta, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x48, MinVersion = CacheVersion.Halo3Beta)]
        public class SoundEffectPlaybackBlock : TagStructure
        {
            [TagField(ValidTags = new[] { "<fx>" })]
            public CachedTag SoundEffect;
            public List<SoundEffectComponentBlock> Components;
            public List<SoundEffectOverridesBlock> SoundEffectOverrides;
            public byte[] Unknown;
            public List<PlatformSoundEffectCollectionBlock> SoundEffectCollections;

            [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo2Beta, MaxVersion = CacheVersion.Halo2Vista)]
            [TagStructure(Size = 0x18, MinVersion = CacheVersion.Halo3Beta)]
            public class SoundEffectComponentBlock : TagStructure
            {
                [TagField(ValidTags = new[] { "lsnd", "snd!" })]
                public CachedTag Sound;
                /// <summary>
                /// additional attenuation to sound
                /// </summary>
                public float Gain; // dB
                public FlagsValue Flags;

                [Flags]
                public enum FlagsValue : uint
                {
                    DonTPlayAtStart = 1 << 0,
                    PlayOnStop = 1 << 1,
                    Unknown = 1 << 2,
                    PlayAlternate = 1 << 3,
                    Unknown1 = 1 << 4,
                    SyncWithOriginLoopingSound = 1 << 5
                }
            }

            [TagStructure(Size = 0xC, MinVersion = CacheVersion.Halo2Beta, MaxVersion = CacheVersion.Halo2Vista)]
            [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3Beta)]
            public class SoundEffectOverridesBlock : TagStructure
            {
                public StringId Name;
                public List<SoundEffectOverrideParametersBlock> Overrides;

                [TagStructure(Size = 0x20, MinVersion = CacheVersion.Halo2Beta, MaxVersion = CacheVersion.Halo2Vista)]
                [TagStructure(Size = 0x24, MinVersion = CacheVersion.Halo3Beta)]
                public class SoundEffectOverrideParametersBlock : TagStructure
                {
                    public StringId Name;
                    public StringId Input;
                    public StringId Range;
                    public float TimePeriod; //  seconds
                    public int IntegerValue;
                    public float RealValue;
                    public MappingFunctionBlock FunctionValue;

                    [TagStructure(Size = 0x8, MinVersion = CacheVersion.Halo2Beta, MaxVersion = CacheVersion.Halo2Vista)]
                    [TagStructure(Size = 0xC, MinVersion = CacheVersion.Halo3Beta)]
                    public class MappingFunctionBlock : TagStructure
                    {
                        public List<ByteBlock> Data;

                        [TagStructure(Size = 0x1)]
                        public class ByteBlock : TagStructure
                        {
                            public sbyte Value;
                        }
                    }
                }
            }

            [TagStructure(Size = 0x14, MinVersion = CacheVersion.Halo2Beta, MaxVersion = CacheVersion.Halo2Vista)]
            [TagStructure(Size = 0x1C, MinVersion = CacheVersion.Halo3Beta)]
            public class PlatformSoundEffectCollectionBlock : TagStructure
            {
                public List<PlatformSoundEffectBlock> SoundEffects;
                public List<PlatformSoundEffectFunctionBlock> LowFrequencyInput;
                public int SoundEffectOverrides;

                [TagStructure(Size = 0x1C, MinVersion = CacheVersion.Halo2Beta, MaxVersion = CacheVersion.Halo2Vista)]
                [TagStructure(Size = 0x28, MinVersion = CacheVersion.Halo3Beta)]
                public class PlatformSoundEffectBlock : TagStructure
                {
                    public List<PlatformSoundEffectFunctionBlock> FunctionInputs;
                    public List<PlatformSoundEffectConstantBlock> ConstantInputs;
                    public List<PlatformSoundEffectOverrideDescriptorBlock> TemplateOverrideDescriptors;
                    public int InputOverrides;

                    [TagStructure(Size = 0x4)]
                    public class PlatformSoundEffectConstantBlock : TagStructure
                    {
                        public float ConstantValue;
                    }

                    [TagStructure(Size = 0x1)]
                    public class PlatformSoundEffectOverrideDescriptorBlock : TagStructure
                    {
                        public sbyte OverrideDescriptor;
                    }
                }

                [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo2Beta, MaxVersion = CacheVersion.Halo2Vista)]
                [TagStructure(Size = 0x14, MinVersion = CacheVersion.Halo3Beta)]
                public class PlatformSoundEffectFunctionBlock : TagStructure
                {
                    public InputValue Input;
                    public RangeValue Range;
                    public MappingFunctionBlock Function;
                    public float TimePeriod; //  seconds

                    public enum InputValue : short
                    {
                        Zero,
                        Time,
                        Scale,
                        Rolloff
                    }

                    public enum RangeValue : short
                    {
                        Zero,
                        Time,
                        Scale,
                        Rolloff
                    }

                    [TagStructure(Size = 0x8, MinVersion = CacheVersion.Halo2Beta, MaxVersion = CacheVersion.Halo2Vista)]
                    [TagStructure(Size = 0xC, MinVersion = CacheVersion.Halo3Beta)]
                    public class MappingFunctionBlock : TagStructure
                    {
                        public List<ByteBlock> Data;

                        [TagStructure(Size = 0x1)]
                        public class ByteBlock : TagStructure
                        {
                            public sbyte Value;
                        }
                    }
                }
            }
        }
    }
}