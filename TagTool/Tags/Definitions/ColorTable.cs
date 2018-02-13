using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "color_table", Tag = "colo", Size = 0xC, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "color_table", Tag = "colo", Size = 0x10, MinVersion = CacheVersion.HaloOnline106708)]
    public class ColorTable
    {
        public List<ColorTableBlock> ColorTables;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown;

        [TagStructure(Size = 0x30)]
        public class ColorTableBlock
        {
            [TagField(Label = true, Length = 32)]
            public string String;

            public RealArgbColor Color;
        }
    }
}
