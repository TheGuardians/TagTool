using TagTool.Tags;

namespace TagTool.Cache.Resources
{
    [TagStructure(Size = 0x8)]
    public class ResourceSubpage : TagStructure
    {
        public int Offset;
        public int Size;
    }
}