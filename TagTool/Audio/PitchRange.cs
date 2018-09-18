using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Audio
{
    [TagStructure(Size = 0xC, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x38, MinVersion = CacheVersion.HaloOnline106708)]
    public class PitchRange
    {
        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public short ImportNameIndex;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public StringId ImportName;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public short PitchRangeParametersIndex;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public PitchRangeParameter PitchRangeParameters;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown1;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown2;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown3;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown4;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public short Unknown5;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public short Unknown6;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public byte PermutationCount;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public byte Unknown7;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public short Unknown8;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public short EncodedPermutationDataIndex;
        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public short EncodedRuntimePermutationFlagIndex;
        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public short EncodedPermutationCount;
        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public ushort FirstPermutationIndex;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<Permutation> Permutations;
    }
}