using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "color_table", Tag = "colo", Size = 0xC, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "color_table", Tag = "colo", Size = 0x10, MinVersion = CacheVersion.HaloOnline106708)]
    public class ColorTable : TagStructure
	{
        public List<ColorTableBlock> ColorTables;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown;

        [TagStructure(Size = 0x30)]
        public class ColorTableBlock : TagStructure
		{
            [TagField(Flags = Label, Length = 32)]
            public string String;

            public RealArgbColor Color;
        }
    }
}
