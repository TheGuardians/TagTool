using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "dialogue", Tag = "udlg", Size = 0x24)]
    public class Dialogue : TagStructure
	{
        public CachedTag GlobalDialogueInfo;
        public uint Flags;
        public List<Vocalization> Vocalizations;
        public StringId MissionDialogueDesignator;

        [TagStructure(Size = 0x18, MinVersion = CacheVersion.Halo3Beta, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x10, MinVersion = CacheVersion.HaloReach)]
        public class Vocalization : TagStructure
		{
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public ushort Flags;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public short Unknown;


            public StringId Name;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public CachedTag Sound;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<VocalizationSound> ReachSounds;

            [TagStructure(MinVersion = CacheVersion.HaloReach, Size = 0x18)]
            public class VocalizationSound : TagStructure
            {
                public int Unknown1;
                public int Unknown2;
                public CachedTag Sound;
            }
        }
    }
}
