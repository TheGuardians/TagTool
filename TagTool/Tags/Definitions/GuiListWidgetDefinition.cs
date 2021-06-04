using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using TagTool.Tags.GUI;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_list_widget_definition", Tag = "lst3", Size = 0x70)]
    public class GuiListWidgetDefinition : TagStructure
	{
        public GuiListWidgetFlags Flags;
        public GuiDefinition GuiRenderBlock;

        public StringId DataSourceName;
        public CachedTag Skin;
        public int RowCount;
        public List<ListWidgetItem> ListWidgetItems;
        public CachedTag UpArrowBitmap;
        public CachedTag DownArrowBitmap;

        [TagStructure(Size = 0x30)]
        public class ListWidgetItem : TagStructure
		{
            public uint Flags;
            public GuiDefinition GuiRenderBlock;
            public StringId Target;
        }
    }

    [Flags]
    public enum GuiListWidgetFlags : int
    {
        None = 0,
        Bit0 = 1 << 0,
        Bit1 = 1 << 1,
        Bit2 = 1 << 2,
        Bit3 = 1 << 3,
        Horizontal = 1 << 4,
        Loops = 1 << 5,
        Bit6 = 1 << 6,
        Bit7 = 1 << 7,
        Bit8 = 1 << 8,
        Bit9 = 1 << 9,
        Bit10 = 1 << 10,
        Bit11 = 1 << 11,
        Bit12 = 1 << 12,
        Bit13 = 1 << 13,
        Bit14 = 1 << 14,
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
