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
            return Permutations[permutationIndex].SampleSize;
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

    }
}