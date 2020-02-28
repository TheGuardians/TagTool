using System.Collections.Generic;

namespace TagTool.Tags
{
    [TagStructure(Size = 0x8, MaxVersion = Cache.CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0xC, MinVersion = Cache.CacheVersion.Halo3Beta)]
    public class TagBlockBitVector : TagStructure
    {
        public List<Word> Words;

        [TagStructure(Size = 0x4)]
        public class Word : TagStructure
        {
            public uint Value;
        }
    }
}