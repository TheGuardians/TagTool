using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "color_table", Tag = "colo", Size = 0xC)]
    public class ColorTable : TagStructure
	{
        public List<ColorTableBlock> ColorTables;

        [TagStructure(Size = 0x30)]
        public class ColorTableBlock : TagStructure
		{
            [TagField(Flags = Label, Length = 32)]
            public string String;

            public RealArgbColor Color;
        }
    }
}
