using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Audio
{
    [TagStructure(Size = 0x24, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x30, MinVersion = CacheVersion.HaloOnline106708)]
    public class Promotion : TagStructure
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
        public class Rule : TagStructure
		{
            public short PitchRangeIndex;
            public short MaximumPlayingCount;
            public float SuppressionTime;
            public int Unknown;
            public int Unknown2;
        }

        [TagStructure(Size = 0x4)]
        public class RuntimeTimer : TagStructure
		{
            public int Unknown;
        }
    }
}