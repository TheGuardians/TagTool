using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "gui_list_widget_definition", Tag = "lst3", Size = 0x70)]
    public class GuiListWidgetDefinition
    {
        public uint Flags;
        public StringId Name;
        public short Unknown;
        public short Layer;
        public short WidescreenYOffset;
        public short WidescreenXOffset;
        public short WidescreenYUnknown;
        public short WidescreenXUnknown;
        public short StandardYOffset;
        public short StandardXOffset;
        public short StandardYUnknown;
        public short StandardXUnknown;
        public CachedTagInstance Animation;
        public StringId DataSourceName;
        public CachedTagInstance Skin;
        public int RowCount;
        public List<ListWidgetItem> ListWidgetItems;
        public CachedTagInstance UpArrowBitmap;
        public CachedTagInstance DownArrowBitmap;

        [TagStructure(Size = 0x30)]
        public class ListWidgetItem
        {
            public uint Flags;
            public StringId Name;
            public short Unknown;
            public short Layer;
            public short WidescreenYOffset;
            public short WidescreenXOffset;
            public short WidescreenYUnknown;
            public short WidescreenXUnknown;
            public short StandardYOffset;
            public short StandardXOffset;
            public short StandardYUnknown;
            public short StandardXUnknown;
            public CachedTagInstance Animation;
            public StringId Target;
        }
    }
}
