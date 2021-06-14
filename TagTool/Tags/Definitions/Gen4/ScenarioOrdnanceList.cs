using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "scenario_ordnance_list", Tag = "scol", Size = 0x18)]
    public class ScenarioOrdnanceList : TagStructure
    {
        public List<RandomOrdnanceItemBlock> RandomOrdnanceDropList;
        public List<PlayerOrdnanceGroupBlock> PlayerOrdnanceDropGroups;
        
        [TagStructure(Size = 0x4)]
        public class RandomOrdnanceItemBlock : TagStructure
        {
            // This must match one of the global ordnance objects.
            public StringId OrdnanceName;
        }
        
        [TagStructure(Size = 0xC)]
        public class PlayerOrdnanceGroupBlock : TagStructure
        {
            public List<PlayerOrdnanceItemBlock> PlayerOrdnanceDrops;
            
            [TagStructure(Size = 0x24)]
            public class PlayerOrdnanceItemBlock : TagStructure
            {
                [TagField(Length = 32)]
                // This must match one of the global ordnance objects.
                public string OrdnanceName;
                // chance that an ordnance from this group will be chosen
                public float OrdnanceFrequency;
            }
        }
    }
}
