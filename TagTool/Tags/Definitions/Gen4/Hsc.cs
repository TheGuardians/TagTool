using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "hsc", Tag = "hsc*", Size = 0x38)]
    public class HsSourceFiles : TagStructure
    {
        [TagField(Length = 32)]
        public string Name;
        public byte[] Source;
        public HsSourceFileFlags Flags;
        
        [Flags]
        public enum HsSourceFileFlags : uint
        {
            GeneratedAtRuntime = 1 << 0,
            AiFragments = 1 << 1,
            AiPerformances = 1 << 2
        }
    }
}
