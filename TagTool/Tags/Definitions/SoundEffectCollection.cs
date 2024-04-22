using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound_effect_collection", Tag = "sfx+", Size = 0xC, MinVersion = CacheVersion.Halo3Retail)]
    public class SoundEffectCollection : TagStructure
	{
        public List<PlatformSoundPlaybackBlock> SoundEffects;

        [TagStructure(Size = 0x4C)]
        public class PlatformSoundPlaybackBlock : TagStructure
		{
            public StringId Name;
            public List<PlatformSoundOverrideMixbinsBlock> OverrideMixbins;
            public PlatformSoundPlaybackFlagsDefinition Flags;
            public uint RadioChannel;

            [TagField(Length = 0x4, Flags = Padding)]
            public byte[] Padding0;

            public List<FilterBlock> Filter;
            public List<PitchLFOBlock> PitchLFO;
            public List<FilterLFOBlock> FilterLFO;
            public List<SoundEffectBlock> SoundEffect;

            [TagStructure(Size = 0x8)]
            public class PlatformSoundOverrideMixbinsBlock : TagStructure
            {
                public PlatformSoundMixbinEnumDefinition Mixbin;
                public float Gain; // dB

                public enum PlatformSoundMixbinEnumDefinition : int
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
            public enum PlatformSoundPlaybackFlagsDefinition : uint
            {
                Use3dRadioHack = 1 << 0,
                ForceFirstPerson = 1 << 1
            }

            [TagStructure(Size = 0x48)]
            public class FilterBlock : TagStructure
			{
                public int FilterType;
                public int FilterWidth;
                public Bounds<float> LeftFrequencyScale;
                public Bounds<float> LeftFrequencyRandomBaseVariance;
                public Bounds<float> LeftGainScale;
                public Bounds<float> LeftGainRandomBaseVariance;
                public Bounds<float> RightFrequencyScale;
                public Bounds<float> RightFrequencyRandomBaseVariance;
                public Bounds<float> RightGainScale;
                public Bounds<float> RightGainRandomBaseVariance;
            }

            [TagStructure(Size = 0x30)]
            public class PitchLFOBlock : TagStructure
			{
                public Bounds<float> DelayScale;
                public Bounds<float> DelayRandomBaseVariance;
                public Bounds<float> FrequencyScale;
                public Bounds<float> FrequencyRandomBaseVariance;
                public Bounds<float> PitchModulationScale;
                public Bounds<float> PitchModulationRandomBaseVariance;
            }

            [TagStructure(Size = 0x40)]
            public class FilterLFOBlock : TagStructure
			{
                public Bounds<float> DelayScale;
                public Bounds<float> DelayRandomBaseVariance;
                public Bounds<float> FrequencyScale;
                public Bounds<float> FrequencyRandomBaseVariance;
                public Bounds<float> CutoffModulationScale;
                public Bounds<float> CutoffModulationRandomBaseVariance;
                public Bounds<float> GainModulationScale;
                public Bounds<float> GainModulationRandomBaseVariance;

            }

            [TagStructure(Size = 0x48)]
            public class SoundEffectBlock : TagStructure
			{
                [TagField(ValidTags = new[] { "<fx>" })]
                public CachedTag Template;

                public List<Component> Components;
                public List<TemplateCollectionBlock> TemplateCollection;
                public byte[] HardwareFormat;
                public List<GNullBlock> PlatformEffect;

                [TagStructure(Size = 0x18)]
                public class Component : TagStructure
				{
                    [TagField(ValidTags = new[] { "snd!", "lsnd" })]
                    public CachedTag Sound;
                    public uint Gain;
                    public SoundEffectComponentFlags Flags;

                    [Flags]
                    public enum SoundEffectComponentFlags : uint
                    {
                        DontPlayAtStart = 1 << 0,
                        PlayOnStop = 1 << 1,
                        PlayOnAbnormalStop = 1 << 2,
                        PlayAlternate = 1 << 3,
                        PlayAlternateOnAbnormalStop = 1 << 4,
                        SyncWithOriginLoopingSound = 1 << 5
                    }
                }

                [TagStructure(Size = 0x10)]
                public class TemplateCollectionBlock : TagStructure
				{
                    public StringId DSPEffect;
                    public List<Parameter> Overrides;

                    [TagStructure(Size = 0x2C)]
                    public class Parameter : TagStructure
					{
                        public StringId Name;
                        public StringId Input;
                        public StringId Range;
                        public float TimePeriod; // seconds
                        public int IntegerValue;
                        public float RealValue;
                        public TagFunction Function = new TagFunction { Data = new byte[0] };
                    }
                }

                [TagStructure(Size = 0x0)]
                public class GNullBlock : TagStructure
                {
                }
            }
        }
    }
}