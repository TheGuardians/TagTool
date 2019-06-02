using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Audio
{
    [TagStructure(Size = 0x10, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x2C, MinVersion = CacheVersion.HaloOnline106708)]
    public class Permutation : TagStructure
	{
        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public short ImportNameIndex;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public StringId ImportName;

        //Convert
        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public short EncodedSkipFraction;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public Bounds<float> SkipFraction;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public sbyte GainH2;
        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public sbyte PermutationInfoIndex;
        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public short LanguageNeutralTime;

        public uint SampleSize;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public int FirstPermutationChunkIndex;
        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public short PermutationChunkCount;

        [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST)]
        public sbyte Gain;
        [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST)]
        public sbyte OverallPermutationIndex;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint PermutationNumber;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint IsNotFirstPermutation;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<PermutationChunk> PermutationChunks;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown1;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown2;
    }
}