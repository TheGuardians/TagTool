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
        [TagField(Gen3Only = true)]
        public short ImportNameIndex;
        [TagField(HaloOnlineOnly = true)]
        public StringId ImportName;

        //Convert
        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public short EncodedSkipFraction;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public Bounds<float> SkipFraction;

        public uint SampleSize;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public int FirstPermutationChunkIndex;
        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public short PermutationChunkCount;
        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public sbyte Gain;
        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public sbyte OverallPermutationIndex;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint PermutationNumber;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint IsNotFirstPermutation;

        [TagField(HaloOnlineOnly = true)]
        public List<PermutationChunk> PermutationChunks;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown1;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown2;
    }
}