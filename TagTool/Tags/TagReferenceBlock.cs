using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Common
{
    [TagStructure(Size = 0x8, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3Retail)]
	public class TagReferenceBlock : TagStructure
	{
        [TagField(Label = true)]
        public CachedTagInstance Instance;
    }
}