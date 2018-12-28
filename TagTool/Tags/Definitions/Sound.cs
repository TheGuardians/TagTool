using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using TagTool.Audio;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound", Tag = "snd!", Size = 0x20, MaxVersion = CacheVersion.Halo3ODST)]
	[TagStructure(Name = "sound", Tag = "snd!", Size = 0xD4, MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline235640)]
	[TagStructure(Name = "sound", Tag = "snd!", Size = 0xD8, MinVersion = CacheVersion.HaloOnline301003, MaxVersion = CacheVersion.HaloOnline449175)]
	[TagStructure(Name = "sound", Tag = "snd!", Size = 0x20, MinVersion = CacheVersion.HaloReach)]
    public class Sound : TagStructure
	{
        public FlagsValue Flags;

        [TagField(HaloOnlineOnly = true)]
        public short Unknown1;
        [TagField(HaloOnlineOnly = true)]
        public uint Unknown2;
        [TagField(HaloOnlineOnly = true)]
        public uint Unknown3;
        
        public SoundClass SoundClass;

        [TagField(HaloOnlineOnly = true)]
        public SampleRate SampleRate;

        [TagField(Gen3Only = true)]
        public SoundCacheFileGestaltReference SoundReference;
        
        [TagField(HaloOnlineOnly = true)]
        public byte Unknown6;

        [TagField(HaloOnlineOnly = true)]
        public ImportType ImportType;

        [TagField(HaloOnlineOnly = true)]
        public PlaybackParameter PlaybackParameters;

        [TagField(HaloOnlineOnly = true)]
        public Scale Scale;

        [TagField(HaloOnlineOnly = true)]
        public PlatformCodec PlatformCodec;

        [TagField(HaloOnlineOnly = true)]
        public Promotion Promotion;

        [TagField(HaloOnlineOnly = true)]
        public List<PitchRange> PitchRanges;

        [TagField(HaloOnlineOnly = true)]
        public List<CustomPlayback> CustomPlayBacks;

        [TagField(HaloOnlineOnly = true)]
        public List<ExtraInfo> ExtraInfo;

        [TagField(HaloOnlineOnly = true)]
        public List<LanguageBlock> Languages;

        [TagField(Pointer = true, HaloOnlineOnly = true)]
        public PageableResource Resource;
        
        [TagField(Padding = true, Length = 4, HaloOnlineOnly = true)]
        public byte[] Unused;

        [TagField(MinVersion = CacheVersion.HaloOnline301003, MaxVersion = CacheVersion.HaloOnline449175)]
        public uint Unknown12;
        
        [Flags]
        public enum FlagsValue : ushort
        {
            None = 0,
            LoopingSound = 1 << 0,
            SplitLongSoundIntoPermutations  = 1 << 1
        }

        [TagStructure(Size = 0x1D)]
        public class SoundCacheFileGestaltReference : TagStructure
		{
            public byte PitchRangeCount;
            public short PlatformCodecIndex;
            public short PitchRangeIndex;
            public short LanguageIndex;
            public short Unknown4;
            public short PlaybackParameterIndex;
            public short ScaleIndex;
            public sbyte PromotionIndex;
            public sbyte CustomPlaybackIndex;
            public short ExtraInfoIndex;
            public int Unknown5;

            public int ZoneAssetHandle;
            [TagField(Padding = true, Length = 4)]
            public byte[] Unused;
        }

    }
}