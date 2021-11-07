using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "dialogue", Tag = "udlg", Size = 0x24)]
    public class Dialogue : TagStructure
	{
        public CachedTag GlobalDialogueInfo;
        public DialogueFlags Flags;
        public List<SoundReference> Vocalizations;
        public StringId MissionDialogueDesignator;

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

            [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding0;

            public StringId Vocalization;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public CachedTag Sound;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<VocalizationSound> ReachSounds;

            [Flags]
            public enum SoundReferenceFlags : ushort
            {
                None = 0,
                NewVocalization = 1 << 0
            }

            [TagStructure(MinVersion = CacheVersion.HaloReach, Size = 0x18)]
            public class VocalizationSound : TagStructure
            {
                public int Unknown1;
                public StringId Name;
                public CachedTag Sound;
            }
        }
    }
}
