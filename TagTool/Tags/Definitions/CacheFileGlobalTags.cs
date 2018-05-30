using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "cache_file_global_tags", Tag = "cfgt", Size = 0x10)]
    public class CacheFileGlobalTags
    {
        public List<TagReferenceBlock> GlobalTags;

        [TagField(Padding = true, Length = 4)]
        public byte[] Unused;
    }
}
