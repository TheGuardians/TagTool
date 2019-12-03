using TagTool.Common;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "cache_file_global_tags", Tag = "cfgt", Size = 0x10)]
    public class CacheFileGlobalTags : TagStructure
	{
        public List<TagReferenceBlock> GlobalTags;

        [TagField(Flags = Padding, Length = 4)]
        public byte[] Unused;
    }
}
