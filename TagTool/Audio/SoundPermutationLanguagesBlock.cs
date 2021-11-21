using TagTool.Tags;
using TagTool.Cache;

namespace TagTool.Audio
{
    [TagStructure(Size = 0x14, MinVersion = CacheVersion.Halo3Beta, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x1C, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    public class SoundPermutationLanguagesBlock : TagStructure
	{
        public int Offset;

        // Encoded size, to get the real size apply a mask of 0x3FFFFF.
        // It should always have the bit 0x400000 activated.
        public int EncodedSizeAndFlags;
        public int RuntimeIndex; // Changes at runtime depending on the g_xbox_sound data array index
        public uint FirstSample;
        public uint LastSample;

        [TagField(Platform = CachePlatform.MCC)]
        public uint RuntimeFsbSoundHash;
        [TagField(Platform = CachePlatform.MCC)]
        public int RuntimeFsbSoundRuntimeStatus;

        public SoundPermutationLanguagesBlock()
        {
            Offset = 0;
            EncodedSizeAndFlags = 0x4000000;
            RuntimeIndex = -1;
            LastSample = 0;
            FirstSample = 0;
        }

        public SoundPermutationLanguagesBlock(int offset, int size)
        {
            Offset = offset;
            EncodedSizeAndFlags = (0x3FFFFFF & size) + 0x4000000;
            RuntimeIndex = -1;
            LastSample = 0;
            FirstSample = 0;
        }
        
        public SoundPermutationLanguagesBlock(int offset, int size, uint lastSample, uint firstSample)
        {
            Offset = offset;
            EncodedSizeAndFlags = (0x3FFFFFF & size) + 0x4000000;
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