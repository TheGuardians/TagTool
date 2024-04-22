using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_list_widget_definition", Tag = "lst3", Size = 0x70)]
    public class GuiListWidgetDefinition : TagStructure
	{
        public GuiListWidgetFlags Flags;
        public GuiDefinition GuiRenderBlock;
        public StringId DataSourceName;
        public CachedTag Skin;
        public int Rows;
        public List<ListWidgetItem> Items;
        public CachedTag PrevIndicatorBitmap;
        public CachedTag NextIndicatorBitmap;

        [Flags]
        public enum GuiListWidgetFlags : int
        {
            DoNotApplyOldContentUpscaling = 1 << 0,
            OverrideTemplateFlags = 1 << 1,
            EnableAnimationDebugging = 1 << 2,
            SubmenuList = 1 << 3,
            HorizontalList = 1 << 4,
            ListWraps = 1 << 5,
            SelectionVisibleWithoutFocus = 1 << 6,
            Noninteractive = 1 << 7
        }

        [TagStructure(Size = 0x30)]
        public class ListWidgetItem : TagStructure
        {
            public ListItemWidgetFlags Flags;
            public GuiDefinition GuiRenderBlock;
            public StringId ItemLabel; // (only used if flag set in skin)

            [Flags]
            public enum ListItemWidgetFlags : uint
            {
                DoNotApplyOldContentUpscaling = 1 << 0,
                OverrideTemplateFlags = 1 << 1,
                EnableAnimationDebugging = 1 << 2
            }
        }
    }
}
