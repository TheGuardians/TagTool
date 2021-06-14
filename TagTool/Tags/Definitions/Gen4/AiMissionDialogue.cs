using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "ai_mission_dialogue", Tag = "mdlg", Size = 0xC)]
    public class AiMissionDialogue : TagStructure
    {
        public List<MissionDialogueLinesBlock> Lines;
        
        [TagStructure(Size = 0x14)]
        public class MissionDialogueLinesBlock : TagStructure
        {
            public StringId Name;
            public List<MissionDialogueVariantsBlock> Variants;
            public StringId DefaultSoundEffect;
            
            [TagStructure(Size = 0x18)]
            public class MissionDialogueVariantsBlock : TagStructure
            {
                // 3-letter designation for the character^
                public StringId VariantDesignation;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Sound;
                public StringId SoundEffect;
            }
        }
    }
}
