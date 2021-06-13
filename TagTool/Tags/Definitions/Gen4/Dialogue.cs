using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "dialogue", Tag = "udlg", Size = 0x24)]
    public class Dialogue : TagStructure
    {
        [TagField(ValidTags = new [] { "adlg" })]
        public CachedTag GlobalDialogueInfo;
        public DialogueFlags Flags;
        public List<SoundReferencesBlock> Vocalizations;
        // 3-letter mission dialogue designator name
        public StringId MissionDialogueDesignator;
        
        [Flags]
        public enum DialogueFlags : uint
        {
            Female = 1 << 0
        }
        
        [TagStructure(Size = 0x10)]
        public class SoundReferencesBlock : TagStructure
        {
            public StringId Vocalization;
            public List<VocalizationStimuliBlock> Stimuli;
            
            [TagStructure(Size = 0x18)]
            public class VocalizationStimuliBlock : TagStructure
            {
                public VocalizationStimulusFlags Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public StringId Stimulus;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Sound;
                
                [Flags]
                public enum VocalizationStimulusFlags : ushort
                {
                    // this stimulus should add to the default vocalization instead of replacing it
                    Additive = 1 << 0,
                    // this stimulus cannot be suppressed
                    DonTSuppress = 1 << 1
                }
            }
        }
    }
}
