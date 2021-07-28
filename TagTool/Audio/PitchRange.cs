using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Audio
{
    [TagStructure(Size = 0xC, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x38, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0xC, MinVersion = CacheVersion.HaloReach)]
    public class PitchRange : TagStructure
	{
        [TagField(Gen = CacheGeneration.Third)]
        public short ImportNameIndex;
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public StringId ImportName;

        [TagField(Gen = CacheGeneration.Third)]
        public short PitchRangeParametersIndex;
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public PitchRangeParameter PitchRangeParameters;

        //
        // Attenuation override
        //

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public Bounds<float> DistanceBounds1;
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public Bounds<float> DistanceBounds2;

        /// <summary>
        /// keeps track of played permutations
        /// </summary>
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public int RuntimePermutationFlags;
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public short PermutationCount;
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public sbyte RuntimeDiscardedPermutationIndex;
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public sbyte RuntimeLastPermutationIndex;

        [TagField(Gen = CacheGeneration.Third)]
        public short EncodedPermutationDataIndex;
        [TagField(Gen = CacheGeneration.Third)]
        public short EncodedRuntimePermutationFlagIndex;
        [TagField(Gen = CacheGeneration.Third, Platform = CachePlatform.Original)]
        public short EncodedPermutationCount;
        [TagField(Gen = CacheGeneration.Third, Platform = CachePlatform.Original)]
        public ushort FirstPermutationIndex;
        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public short PermutationCountH2;

        [TagField(Platform = CachePlatform.MCC)]
        public uint EncodedPermutationInfoMCC;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public List<Permutation> Permutations;
    }

    [TagStructure(Size = 0xC)]
    public class Gen2PitchRange : TagStructure
    {
        public short Name;
        public short Parameters;
        public short EncodedPermutationData;
        public short FirstRuntimePermutationFlagIndex;
        public short FirstPermutation;
        public short PermutationCount;
    }
}