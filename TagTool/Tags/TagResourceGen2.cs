using TagTool.Cache;

namespace TagTool.Tags
{
    [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo2Xbox, MaxVersion = CacheVersion.Halo2Vista)]
    public class TagResourceGen2 : TagStructure
	{
        public TagResourceTypeGen2 Type;
        public sbyte AlignmentBit;
        public short FieldOffset;
        public short PrimaryLocator;
        public short SecondaryLocator;
        public int ResoureDataSize;
        public int ResourceDataOffset;
    }
}