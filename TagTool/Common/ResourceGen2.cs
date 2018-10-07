using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Common
{
    [TagStructure(Size = 0x10, MaxVersion = CacheVersion.Halo2Vista)]
    public class ResourceGen2 : TagStructure
	{
        public ResourceTypeGen2 Type;
        public sbyte AlignmentBit;
        public short FieldOffset;
        public short PrimaryLocator;
        public short SecondaryLocator;
        public int ResoureDataSize;
        public int ResourceDataOffset;
    }
}