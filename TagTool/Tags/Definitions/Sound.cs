using System;
using System.Collections.Generic;
using TagTool.Audio;
using TagTool.Cache;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound", Tag = "snd!", Size = 0x20, MinVersion = CacheVersion.Halo3Beta, MaxVersion = CacheVersion.Halo3ODST)]
	[TagStructure(Name = "sound", Tag = "snd!", Size = 0xD4, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline235640)]
	[TagStructure(Name = "sound", Tag = "snd!", Size = 0xD8, MinVersion = CacheVersion.HaloOnline301003, MaxVersion = CacheVersion.HaloOnline449175)]
    [TagStructure(Name = "sound", Tag = "snd!", Size = 0xD4, MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "sound", Tag = "snd!", Size = 0x24, MinVersion = CacheVersion.HaloReach, BuildType = CacheBuildType.ReleaseBuild)]
    [TagStructure(Name = "sound", Tag = "snd!", Size = 0xE0, MinVersion = CacheVersion.HaloReach, BuildType = CacheBuildType.TagsBuild)]
    public class Sound : TagStructure
	{    
        [TagField(EnumType = typeof(ushort), MinVersion = CacheVersion.Halo3Beta, MaxVersion = CacheVersion.Halo3ODST)]
        [TagField(EnumType = typeof(uint), MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public FlagsValue Flags;

        [TagField(EnumType = typeof(ushort), Version = CacheVersion.HaloReach)]
        [TagField(EnumType = typeof(uint), Version = CacheVersion.HaloReach11883)]
        public FlagsValueReach FlagsReach;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        [TagField(BuildType = CacheBuildType.TagsBuild)]
        public SoundImportFlags ImportFlags;
        [TagField(Gen = CacheGeneration.HaloOnline)]
        [TagField(BuildType = CacheBuildType.TagsBuild)]
        public SoundXsyncFlags XSyncFlags;
        
        public SoundClass SoundClass;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        [TagField(BuildType = CacheBuildType.TagsBuild)]
        public SampleRate SampleRate;

        [TagField(Gen = CacheGeneration.Third, BuildType = CacheBuildType.ReleaseBuild)]
        public SoundCacheFileGestaltReference SoundReference;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        [TagField(BuildType = CacheBuildType.TagsBuild)]
        public sbyte OverrideXmaCompression;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        [TagField(BuildType = CacheBuildType.TagsBuild)]
        public ImportType ImportType;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        [TagField(BuildType = CacheBuildType.TagsBuild)]
        public PlaybackParameter Playback;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        [TagField(BuildType = CacheBuildType.TagsBuild)]
        public Scale Scale;

        [TagField(MinVersion = CacheVersion.HaloReach, BuildType = CacheBuildType.TagsBuild)]
        public float SubPriority;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        [TagField(BuildType = CacheBuildType.TagsBuild)]
        public PlatformCodec PlatformCodec;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        [TagField(BuildType = CacheBuildType.TagsBuild)]
        public Promotion Promotion;

        [TagField(Length = 4, Flags = TagFieldFlags.Padding, Gen = CacheGeneration.Third, BuildType = CacheBuildType.TagsBuild)]
        public byte[] Padding2;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        [TagField(BuildType = CacheBuildType.TagsBuild)]
        public List<PitchRange> PitchRanges;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        [TagField(BuildType = CacheBuildType.TagsBuild)]
        public List<CustomPlayback> CustomPlaybacks;

        [TagField(MinVersion = CacheVersion.HaloReach, BuildType = CacheBuildType.TagsBuild)]
        public TagResourceReference ResourceReachTagsBuild;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        [TagField(BuildType = CacheBuildType.TagsBuild)]
        public List<ExtraInfo> ExtraInfo;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        [TagField(BuildType = CacheBuildType.TagsBuild)]
        public List<LanguageBlock> Languages;

        [TagField(MaxVersion = CacheVersion.HaloReach)]
        public TagResourceReference Resource;

        [TagField(MinVersion = CacheVersion.HaloOnline301003, MaxVersion = CacheVersion.HaloOnline449175)]
        public uint Unknown12;

        public TagResourceReference GetResource(CacheVersion version, CachePlatform platform)
        {
            switch(version)
            {
                case CacheVersion.HaloReach11883:
                    return ResourceReachTagsBuild;
                default:
                    return Resource;
            }
        }

        [Flags]
        public enum FlagsValue : ushort
        {
            None = 0,
            FitToAdpcmBlocksize = 1 << 0,
            AlwaysSpatialize = 1 << 1,
            NeverObstruct = 1 << 2,
            InternalDontTouch = 1 << 3,
            UseHugeTranmission = 1 << 4,
            LinkCountToOwnerUnit = 1 << 5,
            PitchRangeIsLanguage = 1 << 6,
            DontUseSoundClassSpeakerFlag = 1 << 7,
            DontUseLipsyncData = 1 << 8,
            InstantSoundPropagation = 1 << 9,
            FakeSpatializationWithDistance = 1 << 10,
            PlayPermutationsInOrder = 1 << 11,
        }

        [Flags]
        public enum FlagsValueReach : ushort
        {
            None = 0,
            FitToAdpcmBlocksize = 1 << 0,
            AlwaysSpatialize = 1 << 1,
            NeverObstruct = 1 << 2,
            InternalDontTouch = 1 << 3,
            FacialAnimationDataIsStripped = 1 << 4,
            UseHugeTranmission = 1 << 5,
            LinkCountToOwnerUnit = 1 << 6,
            PitchRangeIsLanguage = 1 << 7,
            DontUseSoundClassSpeakerFlag = 1 << 8,
            DontUseLipsyncData = 1 << 9,
            InstantSoundPropagation = 1 << 10,
            FakeSpatializationWithDistance = 1 << 11,
            PlayPermutationsInOrder = 1 << 12 // verify this
        }

        [Flags]
        public enum SoundImportFlags : uint
        {
            DuplicateDirectoryName = 1 << 0,
            CutToBlockSize = 1 << 1,
            UseMarkers = 1 << 2
        }

        [Flags]
        public enum SoundXsyncFlags : uint
        {
            ProcessedLanguageTimes = 1 << 0
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