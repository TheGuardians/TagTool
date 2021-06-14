using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "main_menu_voiceover", Tag = "mmvo", Size = 0xC)]
    public class MainMenuVoiceover : TagStructure
    {
        public List<MainMenuVoiceoverLinesBlock> Lines;
        
        [TagStructure(Size = 0x10)]
        public class MainMenuVoiceoverLinesBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag Sound;
        }
    }
}
