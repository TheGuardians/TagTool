using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "dependency", Tag = "dpnd", Size = 0xC)]
    public class Dependency : TagStructure
    {
        public List<OneDependencyBlock> DependencyList;
        
        [TagStructure(Size = 0x10)]
        public class OneDependencyBlock : TagStructure
        {
            public CachedTag TagReference;
        }
    }
}
