using TagTool.Cache;
using TagTool.Serialization;

namespace TagTool.Common
{
    [TagStructure(Size = 0x10)]
	public /*was_struct*/ class TagReferenceBlock
    {
        [TagField(Label = true)]
        public CachedTagInstance Instance;
    }
}