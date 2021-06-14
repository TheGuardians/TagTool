using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "rumble", Tag = "rmbl", Size = 0x30)]
    public class RumbleDefinition : TagStructure
    {
        public RumbleDefinitionStruct Rumble;
        
        [TagStructure(Size = 0x30)]
        public class RumbleDefinitionStruct : TagStructure
        {
            public RumbleFrequencyDefinitionStruct LowFrequencyRumble;
            public RumbleFrequencyDefinitionStruct HighFrequencyRumble;
            
            [TagStructure(Size = 0x18)]
            public class RumbleFrequencyDefinitionStruct : TagStructure
            {
                public float Duration; // seconds
                public MappingFunction DirtyWhore;
                [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [TagStructure(Size = 0x14)]
                public class MappingFunction : TagStructure
                {
                    public byte[] Data;
                }
            }
        }
    }
}
