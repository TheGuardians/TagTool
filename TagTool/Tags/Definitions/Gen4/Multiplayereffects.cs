using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "multiplayerEffects", Tag = "mgee", Size = 0xC)]
    public class Multiplayereffects : TagStructure
    {
        public List<MultiplayereffectsBlock> Effects;
        
        [TagStructure(Size = 0x14)]
        public class MultiplayereffectsBlock : TagStructure
        {
            public StringId Name;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag Effect;
        }
    }
}
