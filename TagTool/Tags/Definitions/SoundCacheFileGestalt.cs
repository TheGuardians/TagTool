using TagTool.Cache;
using TagTool.Serialization;
using System.Collections.Generic;
using TagTool.Audio;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound_cache_file_gestalt", Tag = "ugh!", Size = 0xB8, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "sound_cache_file_gestalt", Tag = "ugh!", Size = 0xC4, MinVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "sound_cache_file_gestalt", Tag = "ugh!", Size = 0xDC, MinVersion = CacheVersion.HaloReach)]
    public class SoundCacheFileGestalt
    {
        public List<PlatformCodec> PlatformCodecs;
        public List<PlaybackParameter> PlaybackParameters;
        public List<Scale> Scales;
        public List<ImportName> ImportNames;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<UnknownBlock> Unknown;

        public List<PitchRangeParameter> PitchRangeParameters;
        public List<PitchRange> PitchRanges;
        public List<Permutation> Permutations;

        [TagField(Version = CacheVersion.HaloReach)]
        public List<LanguagePermutation> LanguagePermutations;

        // Unknown9 block Reach

        // not in Reach
        public List<CustomPlayback> CustomPlaybacks;

        public List<LanguageBlock> Languages;
        public List<RuntimePermutationFlag> RuntimePermutationFlags;
        public TagFunction Unknown2 = new TagFunction { Data = new byte[0] };
        public uint Unknown3;
        public uint Unknown4;
        public List<PermutationChunk> PermutationChunks;
        public List<Promotion> Promotions;
        public List<ExtraInfo> ExtraInfo; 

        //Unknown15 Reach
    }
}