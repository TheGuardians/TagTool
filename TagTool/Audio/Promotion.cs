using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Audio
{
    [TagStructure(Size = 0x1C, MinVersion = CacheVersion.Halo2Beta, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0x24, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x30, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x24, MinVersion = CacheVersion.HaloReach)]
    public class Promotion : TagStructure
	{
        public List<Rule> Rules;
        public List<RuntimeTimer> RuntimeTimers;

        public uint Unknown1;
        public uint Unknown2;
        public uint Unknown3;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public uint LongestPermutationDuration;
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public uint TotalSampleSize;
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public uint Unknown11;

        [TagStructure(Size = 0x10)]
        public class Rule : TagStructure
		{
            public short PitchRangeIndex;
            public short MaximumPlayingCount;

            /// <summary>
            /// time from when first permutation plays to when another sound from an equal or lower promotion can play
            /// </summary>
            public float SuppressionTime;   // seconds

            public uint Unknown1;
            public uint Unknown2;
        }

        [TagStructure(Size = 0x4)]
        public class RuntimeTimer : TagStructure
		{
            public int Timer;
        }
    }
}