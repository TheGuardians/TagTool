using TagTool.Tags;
using TagTool.Cache;

namespace TagTool.Audio
{
    [TagStructure(Size = 0x14, MinVersion = CacheVersion.Halo3Beta)]
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

        public int UnknownSize;
        public short SoundDialogInfoIndex;
        public short Unknown;


        public PermutationChunk()
        {
            Offset = 0;
            EncodedSize = 0x4000000;
            RuntimeIndex = -1;
            SoundDialogInfoIndex = 0;
            Unknown = 0;
            UnknownSize = 0;
        }

        public PermutationChunk(int offset, int size)
        {
            Offset = offset;
            EncodedSize = (0x3FFFFFF & size) + 0x4000000;
            RuntimeIndex = -1;
            SoundDialogInfoIndex = 0;
            Unknown = 0;
            UnknownSize = 0;
        }
        
        public PermutationChunk(int offset, int size, short soundDialogInfoIndex, short unknown, int unknownSize)
        {
            Offset = offset;
            EncodedSize = (0x3FFFFFF & size) + 0x4000000;
            RuntimeIndex = -1;
            SoundDialogInfoIndex = soundDialogInfoIndex;
            Unknown = unknown;
            UnknownSize = unknownSize;

        }
    }
}