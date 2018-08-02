using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_text_widget_definition", Tag = "txt3", Size = 0x40, MinVersion = CacheVersion.HaloOnline106708)]
    [TagStructure(Name = "gui_text_widget_definition", Tag = "txt3", Size = 0x3C, MaxVersion = CacheVersion.Halo3ODST)]
    public class GuiTextWidgetDefinition
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
        public StringId DataSourceName;
        public StringId TextString;
        public StringId TextColor;
        public short TextFont;
        public short Unknown2;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown3;
    }
}
