using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Audio
{
    [TagStructure(Size = 0x1C, MinVersion = CacheVersion.Halo2Beta, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0x24, Platform = CachePlatform.Original, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x30, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x24, MinVersion = CacheVersion.HaloReach)]
    [TagStructure(Size = 0x30, Platform = CachePlatform.MCC, MinVersion = CacheVersion.Halo3Retail)]
    public class SoundPromotionStruct : TagStructure
    {
        public List<PromotionRule> Rules;
        public List<PromotionRuntimeTimer> RuntimeTimers;

        public uint RuntimeActivePromotionIndex;
        public uint RuntimeLastPromotionTime;
        public uint RuntimeSuppressionTimeout;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public uint LongestPermutationDuration;
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public uint TotalSampleSize;

        [TagField(Length = 8, Flags = TagFieldFlags.Padding, Platform = CachePlatform.MCC, MinVersion = CacheVersion.Halo3Retail)]
        public byte[] PaddingMCC;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        [TagField(Platform = CachePlatform.MCC, MinVersion = CacheVersion.Halo3Retail)]
        public StringId RuntimeUsage;

        [TagStructure(Size = 0x10)]
        public class PromotionRule : TagStructure
		{
            public short PitchRangeIndex;
            public short MaximumPlayingCount;
            public float SuppressionTime; // (seconds) time from when first permutation plays to when another sound from an equal or lower promotion can play
            public uint RuntimeRolloverTime;
            public uint ImpulsePromotionTime;
        }

        [TagStructure(Size = 0x4)]
        public class PromotionRuntimeTimer : TagStructure
		{
            public int TimerStorage;
        }
    }
}