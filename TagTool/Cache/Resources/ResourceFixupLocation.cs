using TagTool.Tags;

namespace TagTool.Cache.Resources
{
    [TagStructure(Size = 0x8)]
    public class ResourceFixupLocation : TagStructure
    {
        public uint BlockOffset;
        public CacheAddress Address;

        [TagField(Flags = TagFieldFlags.Runtime)]
        public int Type;
        [TagField(Flags = TagFieldFlags.Runtime)]
        public int Offset;
        [TagField(Flags = TagFieldFlags.Runtime)]
        public int RawAddress;
    }
}