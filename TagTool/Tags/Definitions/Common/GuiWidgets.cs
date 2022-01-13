using TagTool.Cache;

namespace TagTool.Tags.Definitions.Common
{
    [TagStructure(Size = 0x4C)]
    public class TextWidget : TagStructure
    {
        public CachedTag TemplateReference;
        public GuiTextWidgetDefinition Definition;
    }

    [TagStructure(Size = 0x6C)]
    public class BitmapWidget : TagStructure
    {
        public CachedTag TemplateReference;
        public GuiBitmapWidgetDefinition Definition;
    }

    [TagStructure(Size = 0x80)]
    public class ListWidget : TagStructure
    {
        public CachedTag TemplateReference;
        public GuiListWidgetDefinition Definition;
    }

    [TagStructure(Size = 0x48, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Size = 0x94, MinVersion = CacheVersion.Halo3ODST)]
    public class ModelWidget : TagStructure
    {
        public CachedTag TemplateReference;
        public GuiModelWidgetDefinition Definition;
    }
}
