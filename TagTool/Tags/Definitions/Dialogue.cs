using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "dialogue", Tag = "udlg", Size = 0x24)]
    public class Dialogue : TagStructure
	{
        public CachedTagInstance GlobalDialogueInfo;
        public uint Flags;
        public List<Vocalization> Vocalizations;
        public StringId MissionDialogueDesignator;

        [TagStructure(Size = 0x18)]
        public class Vocalization : TagStructure
		{
            public ushort Flags;
            public short Unknown;
            public StringId Name;
            public CachedTagInstance Sound;
        }
    }
}
