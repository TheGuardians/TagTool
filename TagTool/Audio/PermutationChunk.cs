﻿using TagTool.Tags;
using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Audio
{
    [TagStructure(Size = 0x14, MinVersion = CacheVersion.Halo3Beta, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x20, MinVersion = CacheVersion.Halo3Beta, Platform = CachePlatform.MCC)]
    public class PermutationChunk : TagStructure
	{
        public int Offset;

        /// <summary>
        /// Encoded size, to get the real size apply a mask of 0x3FFFFF. It should always have the bit 0x400000 activated.
        /// </summary>
        public int EncodedSize;

        /// <summary>
        /// Changes at runtime depending on the g_xbox_sound data array index
        /// </summary>
        public int RuntimeIndex;

        public uint FirstSample;
        public uint LastSample;

        [TagField(Platform = CachePlatform.MCC)]
        public uint RuntimeFsbSoundHash;
        [TagField(Platform = CachePlatform.MCC)]
        public int RuntimeFsbSoundRuntimeStatus;
        [TagField(Platform = CachePlatform.MCC)]
        public StringId RuntimeFsbSoundSuffix;

        public PermutationChunk()
        {
            Offset = 0;
            EncodedSize = 0x4000000;
            RuntimeIndex = -1;
            LastSample = 0;
            FirstSample = 0;
        }

        public PermutationChunk(int offset, int size)
        {
            Offset = offset;
            EncodedSize = (0x3FFFFFF & size) + 0x4000000;
            RuntimeIndex = -1;
            LastSample = 0;
            FirstSample = 0;
        }
        
        public PermutationChunk(int offset, int size, uint lastSample, uint firstSample)
        {
            Offset = offset;
            EncodedSize = (0x3FFFFFF & size) + 0x4000000;
            RuntimeIndex = -1;
            LastSample = lastSample;
            FirstSample = firstSample;
        }
    }

    [TagStructure(Size = 0xC)]
    public class Gen2PermutationChunk : TagStructure
    {
        public TagResourceReference ResourceReference;

        /// <summary>
        /// Encoded size, to get the real size apply a mask of 0x3FFFFF.
        /// </summary>
        public int EncodedSize;

        public int RuntimeIndex;

        public int GetSize()
        {
            return EncodedSize & 0x3FFFFFF;
        }
    }
}