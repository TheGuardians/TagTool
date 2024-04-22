using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;
using TagTool.Audio;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "sound_cache_file_gestalt", Tag = "ugh!", Size = 0x58)]
    public class SoundCacheFileGestalt : TagStructure
    {
        public List<PlaybackParameter> Playbacks;
        public List<Scale> Scales;
        public List<ImportName> ImportNames;
        public List<PitchRangeParameter> PitchRangeParameters;
        public List<Gen2PitchRange> PitchRanges;
        public List<Gen2Permutation> Permutations;
        public List<CustomPlayback> CustomPlaybacks;

        /// <summary>
        /// Bit vector
        /// </summary>
        public List<sbyte> RuntimePermutationFlags;

        public List<Gen2PermutationChunk> Chunks;
        public List<Promotion> Promotions;

        public List<ExtraInfo> ExtraInfos;

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
            return Chunks[chunkIndex].EncodedSize & 0x3FFFFFF;
        }

        /// <summary>
        /// Get the chunk offset.
        /// </summary>
        /// <param name="chunkIndex"></param>
        /// <returns></returns>
        public int GetChunkOffset(int chunkIndex)
        {
            return (int)Chunks[chunkIndex].ResourceReference.Gen2ResourceAddress & 0x3FFFFFFF;
        }

        /// <summary>
        /// Get the size of the data for a permutation
        /// </summary>
        /// <param name="permutationIndex"></param>
        /// <returns></returns>
        public int GetPermutationSize(int permutationIndex)
        {
            var permutation = Permutations[permutationIndex];
            var lastChunk = Chunks[permutation.ChunkCount + permutation.FirstChunk - 1];
            var firstChunk = Chunks[permutation.FirstChunk];

            return (int)((lastChunk.ResourceReference.Gen2ResourceAddress & 0x3FFFFFFF) + (lastChunk.EncodedSize & 0x3FFFFF) - (firstChunk.ResourceReference.Gen2ResourceAddress & 0x3FFFFFFF));
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
            for (int i = 0; i < permutation.ChunkCount; i++)
            {
                var tempOffset = GetChunkOffset(permutation.FirstChunk + i);
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
            return PitchRanges[pitchRangeIndex].FirstPermutation;
        }

        /// <summary>
        /// Get the number of permutation in the pitch range block.
        /// </summary>
        /// <param name="pitchRangeIndex"></param>
        /// <returns></returns>
        public int GetPermutationCount(int pitchRangeIndex)
        {
            var pitchRange = PitchRanges[pitchRangeIndex];
            int permutationCount = pitchRange.PermutationCount;
            return permutationCount;
        }

        /// <summary>
        /// Get permutation block.
        /// </summary>
        /// <param name="permutationIndex"></param>
        /// <returns></returns>
        public Gen2Permutation GetPermutation(int permutationIndex)
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
            return (uint)Permutations[permutationIndex].SampleSize;
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

            for (int i = 0; i < GetPermutationCount(pitchRangeIndex); i++)
            {
                samples += GetPermutationSamples(firstPermutationIndex + i);
            }

            return samples;
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
            for (int i = basePitchRangeIndex; i < basePitchRangeIndex + pitchRangeCount; i++)
            {
                for (int j = GetFirstPermutationIndex(i); j < GetFirstPermutationIndex(i) + GetPermutationCount(i); j++)
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

        public Gen2PermutationChunk GetPermutationChunk(int permutationChunkIndex)
        {
            return Chunks[permutationChunkIndex];
        }

        public Gen2PermutationChunk GetFirstPermutationChunk(int permutationIndex)
        {
            var permutation = Permutations[permutationIndex];
            return Chunks[permutation.FirstChunk];
        }

    }
}

