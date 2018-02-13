using TagTool.Cache;
using TagTool.Serialization;

namespace TagTool.Common
{
    [TagStructure(Size = 0x10)]
    public struct TagReferenceBlock
    {
        [TagField(Label = true)]
        public CachedTagInstance Instance;
    }
}