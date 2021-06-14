using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "sound_dialogue_constants", Tag = "spk!", Size = 0x28)]
    public class SoundDialogueConstants : TagStructure
    {
        public float AlmostNever;
        public float Rarely;
        public float Somewhat;
        public float Often;
        [TagField(Length = 0x18, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
    }
}
