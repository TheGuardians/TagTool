using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "NarrativeGlobals", Tag = "narg", Size = 0xC)]
    public class NarrativeGlobals : TagStructure
    {
        public List<NarrativeFlagDefinitionBlock> NarrativeFlagDefinitions;
        
        [TagStructure(Size = 0x8)]
        public class NarrativeFlagDefinitionBlock : TagStructure
        {
            public int Index;
            public NarrativeFlagTypeEnum Type;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum NarrativeFlagTypeEnum : sbyte
            {
                Unknown,
                Terminal
            }
        }
    }
}
