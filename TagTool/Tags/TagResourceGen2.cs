using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Tags
{
    [TagStructure(Size = 0x10)]
    public class TagResourceGen2 : TagStructure
	{
        public TagResourceTypeGen2 Type;
        public sbyte AlignmentBit;
        public short FieldOffset;
        public short PrimaryLocator;
        public short SecondaryLocator;
        public int ResourceDataSize;
        public int ResourceDataOffset;
    }

    [TagStructure(Size = 0x24)]
    public class CacheFileResourceGen2 : TagStructure
    {
        public uint BlockOffset;
        public uint BlockSize;
        public uint SectionDataSize;
        public uint ResourceDataSize;
        public List<TagResourceGen2> TagResources;
        
        [TagField(Flags = TagFieldFlags.Short)]
        public CachedTag Original;

        public short OwnerTagSectionOffset;
        public byte RuntimeLinked;
        public byte RuntimeLoaded;

        [TagField(Flags = TagFieldFlags.Short)]
        public CachedTag Runtime;
    }
}