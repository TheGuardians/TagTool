using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "dialogue", Tag = "udlg", Size = 0x24, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "dialogue", Tag = "udlg", Size = 0x30, MinVersion = CacheVersion.HaloOnline106708)]
    public class Dialogue
    {
        public CachedTagInstance GlobalDialogueInfo;
        public uint Flags;
        public List<Vocalization> Vocalizations;
        public StringId MissionDialogueDesignator;

        [TagField(Padding = true, Length = 12, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;

        [TagStructure(Size = 0x18)]
        public class Vocalization
        {
            public ushort Flags;
            public short Unknown;
            public StringId Name;
            public CachedTagInstance Sound;
        }
    }
}
