using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Common
{
    [TagStructure(Size = 0x10)]
	public /*was_struct*/ class TagReferenceBlock : TagStructure
	{
        [TagField(Label = true)]
        public CachedTagInstance Instance;
    }
}