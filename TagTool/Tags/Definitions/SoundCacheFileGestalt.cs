using TagTool.Cache;
using System.Collections.Generic;
using TagTool.Audio;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound_cache_file_gestalt", Tag = "ugh!", Size = 0xC8, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    [TagStructure(Name = "sound_cache_file_gestalt", Tag = "ugh!", Size = 0xB8, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
    [TagStructure(Name = "sound_cache_file_gestalt", Tag = "ugh!", Size = 0xD4, Version = CacheVersion.Halo3ODST, Platform = CachePlatform.MCC)]
    [TagStructure(Name = "sound_cache_file_gestalt", Tag = "ugh!", Size = 0xC4, Version = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
    [TagStructure(Name = "sound_cache_file_gestalt", Tag = "ugh!", Size = 0xDC, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original)]
    public class SoundCacheFileGestalt : TagStructure
	{
        [TagField(Platform = CachePlatform.MCC)]
        public uint ContentType;

        public List<PlatformCodec> PlatformCodecs;

        public List<PlaybackParameter> PlaybackParameters;
        public List<Scale> Scales;
        public List<ImportName> ImportNames;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<PitchRangeDistance> PitchRangeDistances;

        public List<PitchRangeParameter> PitchRangeParameters;
        public List<PitchRange> PitchRanges;
        public List<Permutation> Permutations;

        [TagField(MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original)]
        [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.MCC)]
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

        public TagFunction NativeSampleData = new TagFunction { Data = new byte[0] };
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
        /// <param name="platform"></param>
        /// <returns></returns>
        public int GetFirstPermutationIndex(int pitchRangeIndex, CachePlatform platform)
        {
            var pitchRange = PitchRanges[pitchRangeIndex];
            return GetFirstPermutationIndex(pitchRange, platform);
        }

        /// <summary>
        /// Get the index of the first permutation in a pitch range block.
        /// </summary>
        /// <param name="pitchRange"></param>
        /// <param name="platform"></param>
        /// <returns></returns>
        public int GetFirstPermutationIndex(PitchRange pitchRange, CachePlatform platform)
        {
            if (platform == CachePlatform.MCC)
            {
                return (int)(pitchRange.EncodedPermutationInfoMCC << 12 >> 12);
            }
            else
            {
                return pitchRange.FirstPermutationIndex;
            }
        }

        /// <summary>
        /// Get the number of permutation in the pitch range block.
        /// </summary>
        /// <param name="pitchRangeIndex"></param>
        /// <param name="platform"></param>
        /// <returns></returns>
        public int GetPermutationCount(int pitchRangeIndex, CachePlatform platform)
        {
            var pitchRange = PitchRanges[pitchRangeIndex];
            if (platform == CachePlatform.MCC)
            {
                return (int)((pitchRange.EncodedPermutationInfoMCC >> 20) & 63);
            }
            else
            {
                return (pitchRange.EncodedPermutationCount >> 4) & 63;
            } 
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
        /// <param name="platform"></param>
        /// <returns></returns>
        public uint GetSamplesPerPitchRange(int pitchRangeIndex, CachePlatform platform)
        {
            uint samples = 0;

            var firstPermutationIndex = GetFirstPermutationIndex(pitchRangeIndex, platform);

            for(int i = 0; i < GetPermutationCount(pitchRangeIndex, platform); i++)
            {
                samples += GetPermutationSamples(firstPermutationIndex + i);
            }

            return samples;
        }

        /// <summary>
        /// Get the order of the permutations.
        /// </summary>
        /// <param name="pitchRangeIndex"></param>
        /// <param name="platform"></param>
        /// <returns></returns>
        public int[] GetPermutationOrder(int pitchRangeIndex, CachePlatform platform)
        {
            var pitchRange = PitchRanges[pitchRangeIndex];
            var permutationCount = GetPermutationCount(pitchRangeIndex, platform);
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
        /// <param name="platform"></param>
        /// <returns></returns>
        public int GetFileSize(int basePitchRangeIndex, int pitchRangeCount, CachePlatform platform)
        {
            int fileSize = 0;

            // largest offset among all permutations
            var maxOffset = -1;
            // permutation index where the largest offset resides.
            var maxIndex = -1;

            //Loop through all the permutation in all pitch ranges to get the largest offset, then use the maxIndex to get totalSize
            for(int i = basePitchRangeIndex; i < basePitchRangeIndex + pitchRangeCount; i++)
            {
                for( int j = GetFirstPermutationIndex(i, platform); j < GetFirstPermutationIndex(i, platform)+GetPermutationCount(i, platform); j++)
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