using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "prefab", Tag = "prfb", Size = 0x10)]
    public class Prefab : TagStructure
    {
        [TagField(ValidTags = new [] { "sbsp" })]
        public CachedTag BspReference;
    }
}
