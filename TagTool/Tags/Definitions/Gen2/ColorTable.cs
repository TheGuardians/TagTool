using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "color_table", Tag = "colo", Size = 0x8)]
    public class ColorTable : TagStructure
    {
        public List<ColorBlock> Colors;
        
        [TagStructure(Size = 0x30)]
        public class ColorBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public RealArgbColor Color;
        }
    }
}

