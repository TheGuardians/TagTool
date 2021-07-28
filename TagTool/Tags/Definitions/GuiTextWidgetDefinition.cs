using System;
using TagTool.Common;
using TagTool.Tags.GUI;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_text_widget_definition", Tag = "txt3", Size = 0x3C)]
    public class GuiTextWidgetDefinition : TagStructure
	{
        public GuiTextFlags Flags;
        public GuiDefinition GuiRenderBlock;
        public StringId DataSourceName;
        public StringId TextString;
        public StringId TextColor;
        public FontValue TextFont;
        public short Unknown2;
    }

    [Flags]
    public enum GuiTextFlags : int
    {
        None = 0,
        Bit0 = 1 << 0,
        Bit1 = 1 << 1,
        Bit2 = 1 << 2,
        LeftAlignment = 1 << 3,
        RightAlignment = 1 << 4,
        Bit5 = 1 << 5,
        AllCaps = 1 << 6,
        Bit7 = 1 << 7,
        Bit8 = 1 << 8,
        Bit9 = 1 << 9,
        Bit10 = 1 << 10,
        Bit11 = 1 << 11,
        Bit12 = 1 << 12,
        WrapAtBounds = 1 << 13,
        CutAtBounds = 1 << 14,
        Bit15 = 1 << 15,
        Bit16 = 1 << 16,
        Bit17 = 1 << 17,
        Bit18 = 1 << 18,
        Bit19 = 1 << 19,
        Bit20 = 1 << 20,
        Bit21 = 1 << 21,
        Bit22 = 1 << 22,
        Bit23 = 1 << 23,
        Bit24 = 1 << 24,
        Bit25 = 1 << 25,
        Bit26 = 1 << 26,
        Bit27 = 1 << 27,
        Bit28 = 1 << 28,
        Bit29 = 1 << 29,
        Bit30 = 1 << 30,
        Bit31 = 1 << 31
    }
}
