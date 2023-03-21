using TagTool.Cache;
using TagTool.Common;
using static TagTool.Tags.TagFieldFlags;
using System.Collections.Generic;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "dialogue", Tag = "udlg", Size = 0x24)]
    public class Dialogue : TagStructure
	{
        [TagField(ValidTags = new[] { "adlg" })]
        public CachedTag GlobalDialogueInfo;

        public DialogueFlags Flags;
        public List<SoundReference> Vocalizations;
        public StringId MissionDialogueDesignator; // 3-letter mission dialogue designator name

        [Flags]
        public enum DialogueFlags : uint
        {
            None = 0,
            Female = 1 << 0
        }

        [TagStructure(Size = 0x18, MinVersion = CacheVersion.Halo3Beta, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x10, MinVersion = CacheVersion.HaloReach)]
        public class SoundReference : TagStructure
		{
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public SoundReferenceFlags Flags;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123, Length = 2, Flags = Padding)]
            public byte[] Padding0;

            public StringId Vocalization;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public CachedTag Sound;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<VocalizationStimulus> Stimuli;

            [Flags]
            public enum SoundReferenceFlags : ushort
            {
                None = 0,
                NewVocalization = 1 << 0
            }

            [TagStructure(MinVersion = CacheVersion.HaloReach, Size = 0x18)]
            public class VocalizationStimulus : TagStructure
            {
                public VocalizationStimulusFlags Flags;

                [TagField(Length = 0x2, Flags = Padding)]
                public byte[] ReachPadding;

                public StringId Stimulus;

                [TagField(ValidTags = new[] { "snd!" })]
                public CachedTag Sound;

                [Flags]
                public enum VocalizationStimulusFlags : ushort
                {
                    Additive = 1 << 0, // this stimulus should add to the default vocalization instead of replacing it
                    DontSuppress = 1 << 1 // this stimulus cannot be suppressed
                }
            }
        }
    }
}
