using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "ai_mission_dialogue", Tag = "mdlg", Size = 0xC)]
    public class AiMissionDialogue : TagStructure
    {
        public List<MissionDialogueLine> Lines;
        
        [TagStructure(Size = 0x14)]
        public class MissionDialogueLine : TagStructure
        {
            public StringId Name;
            public List<MissionDialogueVariant> Variants;
            public StringId DefaultSoundEffect;
            
            [TagStructure(Size = 0x18)]
            public class MissionDialogueVariant : TagStructure
            {
                public StringId VariantDesignation; // 3-letter designation for the character^
                public CachedTag Sound;
                public StringId SoundEffect;
            }
        }
    }
}

