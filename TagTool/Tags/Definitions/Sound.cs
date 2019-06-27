using System;
using System.Collections.Generic;
using TagTool.Audio;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Resources;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound", Tag = "snd!", Size = 0x14, MinVersion = CacheVersion.Halo2Xbox, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Name = "sound", Tag = "snd!", Size = 0x20, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST)]
	[TagStructure(Name = "sound", Tag = "snd!", Size = 0xD4, MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline235640)]
	[TagStructure(Name = "sound", Tag = "snd!", Size = 0xD8, MinVersion = CacheVersion.HaloOnline301003, MaxVersion = CacheVersion.HaloOnline449175)]
    [TagStructure(Name = "sound", Tag = "snd!", Size = 0xD4, MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "sound", Tag = "snd!", Size = 0x20, MinVersion = CacheVersion.HaloReach)]
    public class Sound : TagStructure
	{
        public FlagsValue Flags;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public short Unknown1;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown2;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown3;
        
        public SoundClass SoundClass;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public SampleRate SampleRate;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public SoundCacheFileGestaltReference SoundReference;
        
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public byte Unknown6;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public ImportType ImportType;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public PlaybackParameter PlaybackParameters;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public Scale Scale;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public PlatformCodec PlatformCodec;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public Promotion Promotion;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<PitchRange> PitchRanges;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<CustomPlayback> CustomPlayBacks;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<ExtraInfo> ExtraInfo;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<LanguageBlock> Languages;

        [TagField(Flags = Pointer, MinVersion = CacheVersion.HaloOnline106708)]
        public PageableResource<SoundResourceDefinition> Resource;
        
        [TagField(Flags = Padding, Length = 4, MinVersion = CacheVersion.HaloOnline106708)]
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

        [TagStructure(Size = 0x11, MinVersion = CacheVersion.Halo2Xbox, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x1D, MinVersion = CacheVersion.Halo3Retail)]
        public class SoundCacheFileGestaltReference : TagStructure
		{
            [TagField(MinVersion = CacheVersion.Halo2Xbox, MaxVersion = CacheVersion.Halo2Vista)]
            public SampleRate SampleRate;
            [TagField(MinVersion = CacheVersion.Halo2Xbox, MaxVersion = CacheVersion.Halo2Vista)]
            public Compression Compression;
            [TagField(MinVersion = CacheVersion.Halo2Xbox, MaxVersion = CacheVersion.Halo2Vista)]
            public EncodingH2 Encoding;

            // Halo 3 Section
            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public sbyte PitchRangeCount;
            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public short PlatformCodecIndex;
            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public short PitchRangeIndex;
            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public short LanguageIndex;
            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public short Unknown4;
            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public short PlaybackParameterIndex;
            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public short ScaleIndex;
            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public sbyte PromotionIndex;
            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public sbyte CustomPlaybackIndex;
            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public short ExtraInfoIndex;

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

            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public DatumIndex ZoneAssetHandle;

            [TagField(Flags = Padding, Length = 4, MinVersion = CacheVersion.Halo3Retail)]
            public byte[] Unused;
        }
    }
}