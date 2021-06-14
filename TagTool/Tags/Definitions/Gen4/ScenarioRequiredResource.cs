using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "scenario_required_resource", Tag = "sdzg", Size = 0xC)]
    public class ScenarioRequiredResource : TagStructure
    {
        public List<ScenarioBudgetReferencesBlock> Resources;
        
        [TagStructure(Size = 0x10)]
        public class ScenarioBudgetReferencesBlock : TagStructure
        {
            public CachedTag Reference;
        }
    }
}
