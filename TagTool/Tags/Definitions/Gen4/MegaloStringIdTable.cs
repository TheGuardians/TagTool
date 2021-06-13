using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "megalo_string_id_table", Tag = "msit", Size = 0x18)]
    public class MegaloStringIdTable : TagStructure
    {
        public List<MegaloStringIdBlock> MegaloStringIds;
        public List<MegaloStringIdToSpriteIndexBlock> MegaloStringIdsSpriteMapping;
        
        [TagStructure(Size = 0x4)]
        public class MegaloStringIdBlock : TagStructure
        {
            public StringId StringId;
        }
        
        [TagStructure(Size = 0x8)]
        public class MegaloStringIdToSpriteIndexBlock : TagStructure
        {
            public int FromMegaloStringId;
            public int LoadoutMenuSpriteFrame;
        }
    }
}
