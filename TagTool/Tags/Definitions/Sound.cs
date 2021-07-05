using System;
using System.Collections.Generic;
using TagTool.Audio;
using TagTool.Cache;
using TagTool.Common;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound", Tag = "snd!", Size = 0x20, MinVersion = CacheVersion.Halo3Beta, MaxVersion = CacheVersion.Halo3ODST)]
	[TagStructure(Name = "sound", Tag = "snd!", Size = 0xD4, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline235640)]
	[TagStructure(Name = "sound", Tag = "snd!", Size = 0xD8, MinVersion = CacheVersion.HaloOnline301003, MaxVersion = CacheVersion.HaloOnline449175)]
    [TagStructure(Name = "sound", Tag = "snd!", Size = 0xD4, MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "sound", Tag = "snd!", Size = 0x24, MinVersion = CacheVersion.HaloReach)]
    public class Sound : TagStructure
	{    
        [TagField(Gen = CacheGeneration.Third)]
        public FlagsValue Flags;
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public FlagsValueHaloOnline FlagsHO;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public uint Unknown2;
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public uint Unknown3;
        
        public SoundClass SoundClass;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public SampleRate SampleRate;

        [TagField(Gen = CacheGeneration.Third)]
        public SoundCacheFileGestaltReference SoundReference;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public byte Unknown6;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public ImportType ImportType;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public PlaybackParameter PlaybackParameters;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public Scale Scale;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public PlatformCodec PlatformCodec;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public Promotion Promotion;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public List<PitchRange> PitchRanges;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public List<CustomPlayback> CustomPlayBacks;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public List<ExtraInfo> ExtraInfo;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public List<LanguageBlock> Languages;

        public TagResourceReference Resource;

        [TagField(MinVersion = CacheVersion.HaloOnline301003, MaxVersion = CacheVersion.HaloOnline449175)]
        public uint Unknown12;

        [Flags]
        public enum FlagsValue : ushort
        {
            None = 0,
            LoopingSound = 1 << 0,
            AlwaysSpatialize = 1 << 1,
            NeverObstruct = 1 << 2,
            FacialAnimationDataStripped = 1 << 3,
            UseHugeTranmission = 1 << 4,
            LinkToOwner = 1 << 5,
            PitchRangeIsLanguage = 1 << 6,
            DontUseSoundClassSpeakerFlag = 1 << 7,
            DontUseLipsyncData = 1 << 8,
            Bit9 = 1 << 9,
            Bit10 = 1 << 10,
            FakeSpatialization = 1 << 11,
            Invalid = 1 << 12,
            // ODST
            LowFrequencyEffect = 1 << 13
        }

        [Flags]
        public enum FlagsValueHaloOnline : uint
        {
            None = 0,
            LoopingSound = 1 << 0,
            AlwaysSpatialize = 1 << 1,
            NeverObstruct = 1 << 2,
            FacialAnimationDataStripped = 1 << 3,
            Bit4 = 1 << 4,
            LinkToOwner = 1 << 5,
            PitchRangeIsLanguage = 1 << 6,
            DontUseSoundClassSpeakerFlag = 1 << 7,
            DontUseLipsyncData = 1 << 8,
            Bit9 = 1 << 9,
            Bit10 = 1 << 10,
            CopyIntoMemoryBeforePlaying = 1 << 11,
            FakeSpatialization = 1 << 12,
            Invalid = 1 << 13,
            LowFrequencyEffect = 1 << 14
        }

        [TagStructure(Size = 0x9, MinVersion = CacheVersion.Halo2Xbox, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x15, MinVersion = CacheVersion.Halo3Beta, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x19, MinVersion = CacheVersion.HaloReach)]
        public class SoundCacheFileGestaltReference : TagStructure
		{
            [TagField(MinVersion = CacheVersion.Halo2Xbox, MaxVersion = CacheVersion.Halo2Vista)]
            public SampleRate SampleRate;
            [TagField(MinVersion = CacheVersion.Halo2Xbox, MaxVersion = CacheVersion.Halo2Vista)]
            public Compression Compression;
            [TagField(MinVersion = CacheVersion.Halo2Xbox, MaxVersion = CacheVersion.Halo2Vista)]
            public Gen2Encoding Encoding;

            // Halo 3 Section
            [TagField(MinVersion = CacheVersion.Halo3Beta)]
            public sbyte PitchRangeCount;
            [TagField(MinVersion = CacheVersion.Halo3Beta)]
            public short PlatformCodecIndex;
            [TagField(MinVersion = CacheVersion.Halo3Beta)]
            public short PitchRangeIndex;
            [TagField(MinVersion = CacheVersion.Halo3Beta)]
            public short LanguageIndex;
            [TagField(MinVersion = CacheVersion.Halo3Beta)]
            public short Unknown4;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short UnknownIndexReach1;
            [TagField(MinVersion = CacheVersion.Halo3Beta)]
            public short PlaybackParameterIndex;
            [TagField(MinVersion = CacheVersion.Halo3Beta)]
            public short ScaleIndex;
            [TagField(MinVersion = CacheVersion.Halo3Beta)]
            public sbyte PromotionIndex;
            [TagField(MinVersion = CacheVersion.Halo3Beta)]
            public sbyte CustomPlaybackIndex;
            [TagField(MinVersion = CacheVersion.Halo3Beta)]
            public short ExtraInfoIndex;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short UnknownIndexReach2;

            //Halo 2 Section

            [TagField(MinVersion = CacheVersion.Halo2Xbox, MaxVersion = CacheVersion.Halo2Vista, Upgrade = nameof(CustomPlaybackIndex))]
            public short PlaybackParameterIndexOld;
            [TagField(MinVersion = CacheVersion.Halo2Xbox, MaxVersion = CacheVersion.Halo2Vista, Upgrade = nameof(PitchRangeIndex))]
            public short PitchRangeIndexOld;
            [TagField(MinVersion = CacheVersion.Halo2Xbox, MaxVersion = CacheVersion.Halo2Vista, Upgrade = nameof(PitchRangeCount))]
            public sbyte PitchRangeCountOld;
            [TagField(MinVersion = CacheVersion.Halo2Xbox, MaxVersion = CacheVersion.Halo2Vista, Upgrade = nameof(ScaleIndex))]
            public sbyte ScaleIndexOld;
            [TagField(MinVersion = CacheVersion.Halo2Xbox, MaxVersion = CacheVersion.Halo2Vista, Upgrade = nameof(PromotionIndex))]
            public sbyte PromotionIndexOld;
            [TagField(MinVersion = CacheVersion.Halo2Xbox, MaxVersion = CacheVersion.Halo2Vista, Upgrade = nameof(CustomPlaybackIndex))]
            public sbyte CustomPlaybackIndexOld;
            [TagField(MinVersion = CacheVersion.Halo2Xbox, MaxVersion = CacheVersion.Halo2Vista, Upgrade = nameof(ExtraInfoIndex))]
            public short ExtraInfoIndexOld;


            public int MaximumPlayTime;
        }
    }
}