using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Audio
{
    [TagStructure(Size = 0x10, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x2C, MinVersion = CacheVersion.HaloOnline106708)]
    [TagStructure(Size = 0x14, MinVersion = CacheVersion.HaloReach)]
    public class Permutation : TagStructure
	{
        [TagField(Gen = CacheGeneration.Third)]
        public short ImportNameIndex;
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public StringId ImportName;

        //Convert
        [TagField(Gen = CacheGeneration.Third)]
        public short EncodedSkipFraction;
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public Bounds<float> SkipFraction;


        public uint SampleSize;

        [TagField(Gen = CacheGeneration.Third)]
        public int FirstPermutationChunkIndex;
        [TagField(Gen = CacheGeneration.Third)]
        public short PermutationChunkCount;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public sbyte EncodedGain;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public sbyte EncodedPermutationIndex;

        [TagField(Gen = CacheGeneration.Third)]
        public sbyte Gain;
        [TagField(Gen = CacheGeneration.Third)]
        public sbyte OverallPermutationIndex;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public byte UnknownReach1;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public byte UnknownReach2;


        [TagField(Gen = CacheGeneration.HaloOnline)]
        public uint PermutationNumber;
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public uint IsNotFirstPermutation;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public List<PermutationChunk> PermutationChunks;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public uint Unknown1;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public uint Unknown2;
    }
}