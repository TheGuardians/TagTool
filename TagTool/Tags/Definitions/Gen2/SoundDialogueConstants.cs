using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "sound_dialogue_constants", Tag = "spk!", Size = 0x28)]
    public class SoundDialogueConstants : TagStructure
    {
        /// <summary>
        /// these values correspond to the named play fractions in the dialogue editor (It's really skip fractions, but who cares?)
        /// </summary>
        public float AlmostNever;
        public float Rarely;
        public float Somewhat;
        public float Often;
        [TagField(Length = 0x18, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
    }
}

