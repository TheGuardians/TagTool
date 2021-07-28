using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using TagTool.Tags.GUI;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_screen_widget_definition", Tag = "scn3", Size = 0xA8)]
    public class GuiScreenWidgetDefinition : TagStructure
	{
        public GuiScreenWidgetFlags Flags;
        public GuiDefinition GuiRenderBlock;
        public CachedTag Strings;
        public CachedTag Parent;
        public StringId DefaultKeyLegendString;
        public List<DataSource> DataSources;
        public List<GroupWidget> GroupWidgets;
        public List<ButtonKeyLegend> ButtonKeyLegends;
        public CachedTag UiSounds;
        [TagField(Length = 32)] public string ScriptTitle;
        public short ScriptIndex;
        public short Unknown2;

        [TagStructure(Size = 0x10)]
        public class DataSource : TagStructure
		{
            public CachedTag DataSource2;
        }

        [TagStructure(Size = 0x6C)]
        public class GroupWidget : TagStructure
		{
            public CachedTag Parent;
            public GroupWidgetsFlags Flags;
            public GuiDefinition GuiRenderBlock;
            public List<ListWidget> ListWidgets;
            public List<TextWidget> TextWidgets;
            public List<BitmapWidget> BitmapWidgets;
            public List<ModelWidget> ModelWidgets;
        }

        [TagStructure(Size = 0x10)]
        public class ButtonKeyLegend : TagStructure
		{
            public CachedTag Legend;
        }
    }

    [Flags]
    public enum GuiScreenWidgetFlags : int
    {
        None = 0,
        Bit0 = 1 << 0,
        Bit1 = 1 << 1,
        Bit2 = 1 << 2,
        Bit3 = 1 << 3,
        Bit4 = 1 << 4,
        Bit5 = 1 << 5,
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

    [Flags]
    public enum GroupWidgetsFlags : int
    {
        None = 0,
        Bit0 = 1 << 0,
        Bit1 = 1 << 1,
        Bit2 = 1 << 2,
        InitiallyHidden = 1 << 3,
        Bit4 = 1 << 4,
        Bit5 = 1 << 5,
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
