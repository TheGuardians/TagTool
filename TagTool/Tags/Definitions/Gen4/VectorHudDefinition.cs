using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "vector_hud_definition", Tag = "vchd", Size = 0x14)]
    public class VectorHudDefinition : TagStructure
    {
        public byte[] RawHudData;
    }
}
