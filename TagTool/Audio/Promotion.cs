using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Serialization;

namespace TagTool.Audio
{
    [TagStructure(Size = 0x24, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x30, MinVersion = CacheVersion.HaloOnline106708)]
    public class Promotion
    {
        public List<Rule> Rules;
        public List<RuntimeTimer> RuntimeTimers;
        public int Unknown1;
        public uint Unknown2;
        public uint Unknown3;

        [TagField(HaloOnlineOnly = true)]
        public uint LongestPermutationDuration;
        [TagField(HaloOnlineOnly = true)]
        public uint TotalSampleSize;
        [TagField(HaloOnlineOnly = true)]
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
}