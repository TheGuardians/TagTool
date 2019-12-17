using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_text_widget_definition", Tag = "txt3", Size = 0x3C)]
    public class GuiTextWidgetDefinition : TagStructure
	{
        public uint Flags;
        public GuiDefinition GuiRenderBlock;
        public StringId DataSourceName;
        public StringId TextString;
        public StringId TextColor;
        public short TextFont;
        public short Unknown2;
    }
}
