using TagTool.Tags;
using TagTool.Cache;

namespace TagTool.Audio
{
    [TagStructure(Size = 0xC, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0x14, MinVersion = CacheVersion.Halo3Retail)]
    public class PermutationChunk : TagStructure
	{
        
        public int Offset;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public short Flags;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public short Size;

        /// <summary>
        /// Encoded size, to get the real size apply a mask of 0x3FFFFF. It should always have the bit 0x400000 activated.
        /// </summary>
        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int EncodedSize;

        /// <summary>
        /// Changes at runtime depending on the g_xbox_sound data array index
        /// </summary>
        public int RuntimeIndex;
        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int UnknownA;
        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int UnknownSize;

        public PermutationChunk()
        {
            Offset = 0;
            EncodedSize = 0x4000000;
            RuntimeIndex = -1;
            UnknownA = 0;
            UnknownSize = 0;
        }

        public PermutationChunk(int offset, int size)
        {
            Offset = offset;
            EncodedSize = (0x3FFFFFF & size) + 0x4000000;
            RuntimeIndex = -1;
            UnknownA = 0;
            UnknownSize = 0;
        }
        
    }
}