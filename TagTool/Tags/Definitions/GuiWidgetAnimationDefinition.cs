using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_widget_animation_definition", Tag = "wgan", Size = 0x78, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "gui_widget_animation_definition", Tag = "wgan", Size = 0x88, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "gui_widget_animation_definition", Tag = "wgan", Size = 0x80, MinVersion = CacheVersion.HaloOnline106708)]
    public class GuiWidgetAnimationDefinition : TagStructure
	{
        public StringId Stringid;
        public uint Unknown2;
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public CachedTag Unknown;
        public CachedTag WidgetColor;
        public CachedTag WidgetPosition;
        public CachedTag WidgetRotation;
        public CachedTag WidgetScale;
        public CachedTag WidgetTextureCoordinate;
        public CachedTag WidgetSprite;
        public CachedTag WidgetFont;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown3;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown4;
    }
}
