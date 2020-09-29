using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenario_hs_source_file", Tag = "hsc*", Size = 0x34)]
    public class ScenarioHsSourceFile : TagStructure
    {
        [TagField(Length = 32)]
        public string Name;
        public byte[] Source;
    }
}

