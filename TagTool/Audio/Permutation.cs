using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Audio
{
    [TagStructure(Size = 0x10, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x2C, MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
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
        public float SkipFraction;
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public uint Unknown3;

        public uint SampleCount;

        [TagField(Gen = CacheGeneration.Third)]
        public int FirstPermutationChunkIndex;
        [TagField(Gen = CacheGeneration.Third)]
        public short PermutationChunkCount;

        [TagField(Gen = CacheGeneration.Third)]
        public sbyte Gain;
        [TagField(Gen = CacheGeneration.Third)]
        public sbyte OverallPermutationIndex;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public int UnknownReach3;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public uint PermutationNumber;
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public uint IsNotFirstPermutation;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public List<PermutationChunk> PermutationChunks;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public PermutationFlagsHaloOnline PermutationFlagsHO;

        [TagField(Gen = CacheGeneration.HaloOnline, Flags = TagFieldFlags.Padding, Length = 2)]
        public byte[] Padding;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public uint FirstSample;

        [Flags]
        public enum PermutationFlagsHaloOnline : short
        {
            /// <summary>
            /// Wait for the currently playing permutation to finish
            /// </summary>
            SequencedBit = (1 << 0)
        }
    }
}
