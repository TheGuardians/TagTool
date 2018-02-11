using TagTool.Serialization;

namespace TagTool.Common
{
    [TagStructure(Size = 0x10)]
    public class ResourceGen2
    {
        public ResourceTypeGen2 Type;
        public sbyte Unknown1;
        public short Unknown2;
        public short PrimaryLocator;
        public short SecondaryLocator;
        public uint ResoureDataSize;
        public uint ResourceDataOffset;
    }
}