using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "armor_sounds", Tag = "arms", Size = 0x10, MinVersion = CacheVersion.HaloOnline106708)]
    public class ArmorSounds : TagStructure
	{
        public List<ArmorSound> ArmorSounds2;
        public uint Unknown;

        [TagStructure(Size = 0x24)]
        public class ArmorSound : TagStructure
		{
            public List<TagReferenceBlock> Unknown1;
            public List<TagReferenceBlock> Unknown2;
            public List<TagReferenceBlock> Unknown3;
        }
    }
}