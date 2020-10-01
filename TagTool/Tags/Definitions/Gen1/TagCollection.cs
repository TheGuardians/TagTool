using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "tag_collection", Tag = "tagc", Size = 0xC)]
    public class TagCollection : TagStructure
    {
        public List<TagReference> TagReferences;
        
        [TagStructure(Size = 0x10)]
        public class TagReference : TagStructure
        {
            public CachedTag Tag;
        }
    }
}

