using TagTool.Tags;

namespace TagTool.Cache.Resources
{
    [TagStructure(Size = 0x10, Align = 0x8)]
    public class ResourceSection : TagStructure
    {
        public short RequiredPageIndex;
        public short OptionalPageIndex;
        public int RequiredSegmentOffset;
        public int OptionalSegmentOffset;
        public short RequiredSizeIndex;
        public short OptionalSizeIndex;
    }
}