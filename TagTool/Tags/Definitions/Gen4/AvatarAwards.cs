using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "avatar_awards", Tag = "avat", Size = 0xC)]
    public class AvatarAwards : TagStructure
    {
        public List<SingleAvatarAwardDefinitionBlock> AvatarAward;
        
        [TagStructure(Size = 0x8)]
        public class SingleAvatarAwardDefinitionBlock : TagStructure
        {
            public StringId Name;
            public GlobalAvatarAwardEnum Type;
            
            public enum GlobalAvatarAwardEnum : int
            {
                Award0,
                Award1,
                Award2
            }
        }
    }
}
