using TagTool.Tags;

namespace TagTool.Cache.Resources
{
    [TagStructure(Size = 0x8)]
    public class ResourceInteropLocation : TagStructure
    {
        public CacheAddress Address;
        public int ResourceStructureTypeIndex;
    }
}