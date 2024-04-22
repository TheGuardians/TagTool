using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "ai_mission_dialogue", Tag = "mdlg", Size = 0x8)]
    public class AiMissionDialogue : TagStructure
    {
        public List<MissionDialogueLinesBlock> Lines;
        
        [TagStructure(Size = 0x10)]
        public class MissionDialogueLinesBlock : TagStructure
        {
            public StringId Name;
            public List<MissionDialogueVariantsBlock> Variants;
            public StringId DefaultSoundEffect;
            
            [TagStructure(Size = 0x10)]
            public class MissionDialogueVariantsBlock : TagStructure
            {
                /// <summary>
                /// 3-letter designation for the character^
                /// </summary>
                public StringId VariantDesignation;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Sound;
                public StringId SoundEffect;
            }
        }
    }
}

