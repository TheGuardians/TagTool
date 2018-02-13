using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_model_widget_definition", Tag = "mdl3", Size = 0x90)]
    public class GuiModelWidgetDefinition
    {
        public uint Flags;
        public StringId Name;
        public short Unknown;
        public short Layer;
        public short WidescreenYBoundsMin;
        public short WidescreenXBoundsMin;
        public short WidescreenYBoundsMax;
        public short WidescreenXBoundsMax;
        public short StandardYBoundsMin;
        public short StandardXBoundsMin;
        public short StandardYBoundsMax;
        public short StandardXBoundsMax;
        public CachedTagInstance Animation;
        public List<Biped> Bipeds;
        public uint Unknown2;
        public uint Unknown3;
        public uint Unknown4;
        public uint Unknown5;
        public uint Unknown6;
        public uint Unknown7;
        public uint Unknown8;
        public List<UnknownBlock> Unknown9;
        public uint Unknown10;
        public uint Unknown11;
        public uint Unknown12;
        public uint Unknown13;
        public uint Unknown14;
        public uint Unknown15;
        public List<UnknownBlock2> Unknown16;
        public uint Unknown17;
        public uint Unknown18;
        public uint Unknown19;

        [TagStructure(Size = 0xA0)]
        public class Biped
        {
            public StringId Biped2;
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;
            public uint Unknown7;
            public uint Unknown8;
            public uint Unknown9;
            public uint Unknown10;
            public uint Unknown11;
            public uint Unknown12;
            public uint Unknown13;
            public uint Unknown14;
            public uint Unknown15;
            public uint Unknown16;
            public uint Unknown17;
            public uint Unknown18;
            public uint Unknown19;
            public uint Unknown20;
            public uint Unknown21;
            public uint Unknown22;
            public uint Unknown23;
            public uint Unknown24;
            public List<UnknownBlock> Unknown25;
            public uint Unknown26;
            public uint Unknown27;
            public Angle Unknown28;
            public uint Unknown29;
            public Angle Unknown30;
            public uint Unknown31;
            public uint Unknown32;
            public CachedTagInstance Unknown33;
            public uint Unknown34;

            [TagStructure(Size = 0x14)]
            public class UnknownBlock
            {
                public byte[] Unknown;
            }
        }

        [TagStructure(Size = 0x14)]
        public class UnknownBlock
        {
            public byte[] Unknown;
        }

        [TagStructure(Size = 0x14)]
        public class UnknownBlock2
        {
            public StringId Unknown;
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
        }
    }
}
