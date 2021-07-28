using TagTool.Cache;
using System.Collections.Generic;
using TagTool.Audio;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound_cache_file_gestalt", Tag = "ugh!", Size = 0xC8, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    [TagStructure(Name = "sound_cache_file_gestalt", Tag = "ugh!", Size = 0xB8, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
    [TagStructure(Name = "sound_cache_file_gestalt", Tag = "ugh!", Size = 0xC4, MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
    [TagStructure(Name = "sound_cache_file_gestalt", Tag = "ugh!", Size = 0xDC, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original)]
    public class SoundCacheFileGestalt : TagStructure
	{
        [TagField(Platform = CachePlatform.MCC)]
        public uint Unknown1;

        public List<PlatformCodec> PlatformCodecs;

        public List<PlaybackParameter> PlaybackParameters;
        public List<Scale> Scales;
        public List<ImportName> ImportNames;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<UnknownBlock> Unknown2;

        public List<PitchRangeParameter> PitchRangeParameters;
        public List<PitchRange> PitchRanges;
        public List<Permutation> Permutations;

        // likely one of the two blocks bellow
        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        public uint UnknownMCC1;
        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        public uint UnknownMCC2;
        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        public uint UnknownMCC3;

        [TagField(MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original)]
        public List<LanguagePermutation> LanguagePermutations;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<Unknown6C> UnknownReach1;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<CustomPlayback> CustomPlaybacks;

        public List<LanguageBlock> Languages;

        /// <summary>
        /// Bit vector
        /// </summary>
        public List<sbyte> RuntimePermutationFlags;

        public TagFunction Unknown3 = new TagFunction { Data = new byte[0] };
        public uint Unknown4;
        public uint Unknown5;

        public List<PermutationChunk> PermutationChunks;
        public List<Promotion> Promotions;
        public List<ExtraInfo> ExtraInfo;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<int> UnknownReach2;


        [TagStructure(MinVersion = CacheVersion.HaloReach, Size = 0x2C)]
        public class Unknown6C : TagStructure
        {
            public int Unknown1;
            public CachedTag Unknown2;
            public int Unknown3;
            public int Unknown4;
            public int Unknown5;
            public int Unknown6;
            public int Unknown7;
            public int Unknown8;
        }


        //
        // Functions for sound conversion 
        //

        /// <summary>
        /// Get the chunk size from a chunk in Gen3 Blam engine
        /// </summary>
        /// <param name="chunkIndex"></param>
        /// <returns></returns>
        public int GetChunkSize(int chunkIndex)
        {
            return PermutationChunks[chunkIndex].EncodedSize & 0xFFFFFF;
        }

        /// <summary>
        /// Get the chunk offset.
        /// </summary>
        /// <param name="chunkIndex"></param>
        /// <returns></returns>
        public int GetChunkOffset(int chunkIndex)
        {
            return PermutationChunks[chunkIndex].Offset;
        }

        /// <summary>
        /// Get the size of the data for a permutation
        /// </summary>
        /// <param name="permutationIndex"></param>
        /// <returns></returns>
        public int GetPermutationSize(int permutationIndex)
        {
            var permutation = Permutations[permutationIndex];
            var lastChunk = PermutationChunks[permutation.PermutationChunkCount + permutation.FirstPermutationChunkIndex - 1];
            var firstChunk = PermutationChunks[permutation.FirstPermutationChunkIndex];

            return lastChunk.Offset + (lastChunk.EncodedSize & 0xFFFFFF) - firstChunk.Offset;
        }

        /// <summary>
        /// Get the offset of the permutation in the resource.
        /// </summary>
        /// <param name="permutationIndex"></param>
        /// <returns></returns>
        public int GetPermutationOffset(int permutationIndex)
        {
            int offset = int.MaxValue;
            var permutation = Permutations[permutationIndex];

            // Get the minimal offset value for all the chunks in the permutation
            for (int i = 0; i < permutation.PermutationChunkCount; i++)
            {
                var tempOffset = GetChunkOffset(permutation.FirstPermutationChunkIndex + i);
                if (tempOffset < offset)
                    offset = tempOffset;
            }

            return offset;
        }

        /// <summary>
        /// Get the index of the first permutation in a pitch range block.
        /// </summary>
        /// <param name="pitchRangeIndex"></param>
        /// <returns></returns>
        public int GetFirstPermutationIndex(int pitchRangeIndex)
        {
            return PitchRanges[pitchRangeIndex].FirstPermutationIndex;
        }

        /// <summary>
        /// Get the number of permutation in the pitch range block.
        /// </summary>
        /// <param name="pitchRangeIndex"></param>
        /// <returns></returns>
        public int GetPermutationCount(int pitchRangeIndex)
        {
            var pitchRange = PitchRanges[pitchRangeIndex];
            int permutationCount = pitchRange.EncodedPermutationCount;
            permutationCount = (permutationCount >> 4) & 63;
            return permutationCount;
        }

        /// <summary>
        /// Get permutation block.
        /// </summary>
        /// <param name="permutationIndex"></param>
        /// <returns></returns>
        public Permutation GetPermutation(int permutationIndex)
        {
            return Permutations[permutationIndex];
        }

        /// <summary>
        /// Get the number of samples in a permutation
        /// </summary>
        /// <param name="permutationIndex"></param>
        /// <returns></returns>
        public uint GetPermutationSamples(int permutationIndex)
        {
            return Permutations[permutationIndex].SampleCount;
        }

        /// <summary>
        /// Get the total number of audio samples in a pitch range block.
        /// </summary>
        /// <param name="pitchRangeIndex"></param>
        /// <returns></returns>
        public uint GetSamplesPerPitchRange(int pitchRangeIndex)
        {
            uint samples = 0;
            var pitchRange = PitchRanges[pitchRangeIndex];
            var firstPermutationIndex = GetFirstPermutationIndex(pitchRangeIndex);

            for(int i = 0; i < GetPermutationCount(pitchRangeIndex); i++)
            {
                samples += GetPermutationSamples(firstPermutationIndex + i);
            }

            return samples;
        }

        /// <summary>
        /// Get the order of the permutations.
        /// </summary>
        /// <param name="pitchRangeIndex"></param>
        /// <returns></returns>
        public int[] GetPermutationOrder(int pitchRangeIndex)
        {
            var pitchRange = PitchRanges[pitchRangeIndex];
            var permutationCount = GetPermutationCount(pitchRangeIndex);
            int[] permutationOrder = new int[permutationCount];

            for(int i = 0; i < permutationCount; i++)
            {
                permutationOrder[i] = Permutations[pitchRange.FirstPermutationIndex + i].OverallPermutationIndex;
            }
            return permutationOrder;
        }

        /// <summary>
        /// Get the file size given the pitch range blocks
        /// </summary>
        /// <param name="basePitchRangeIndex"></param>
        /// <param name="pitchRangeCount"></param>
        /// <returns></returns>
        public int GetFileSize(int basePitchRangeIndex, int pitchRangeCount)
        {
            int fileSize = 0;

            // largest offset among all permutations
            var maxOffset = -1;
            // permutation index where the largest offset resides.
            var maxIndex = -1;

            //Loop through all the permutation in all pitch ranges to get the largest offset, then use the maxIndex to get totalSize
            for(int i = basePitchRangeIndex; i < basePitchRangeIndex + pitchRangeCount; i++)
            {
                for( int j = GetFirstPermutationIndex(i); j < GetFirstPermutationIndex(i)+GetPermutationCount(i); j++)
                {
                    if (GetPermutationOffset(j) >= maxOffset)
                    {
                        maxOffset = GetPermutationOffset(j);
                        maxIndex = j;
                    }
                }
            }
            if (maxOffset < 0 || maxIndex < 0)
                return -1;

            fileSize = maxOffset + GetPermutationSize(maxIndex);

            return fileSize;
        }

        public PermutationChunk GetPermutationChunk(int permutationChunkIndex)
        {
            return PermutationChunks[permutationChunkIndex];
        }

        public PermutationChunk GetFirstPermutationChunk(int permutationIndex)
        {
            var permutation = Permutations[permutationIndex];
            return PermutationChunks[permutation.FirstPermutationChunkIndex];
        }

    }
}