using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using TagTool.Tags.GUI;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_list_widget_definition", Tag = "lst3", Size = 0x70)]
    public class GuiListWidgetDefinition : TagStructure
	{
        public uint Flags;
        public GuiDefinition GuiRenderBlock;

        public StringId DataSourceName;
        public CachedTagInstance Skin;
        public int RowCount;
        public List<ListWidgetItem> ListWidgetItems;
        public CachedTagInstance UpArrowBitmap;
        public CachedTagInstance DownArrowBitmap;

        [TagStructure(Size = 0x30)]
        public class ListWidgetItem : TagStructure
		{
            public uint Flags;
            public GuiDefinition GuiRenderBlock;
            public StringId Target;
        }
    }
}
