using TagTool.Cache;
using TagTool.Serialization;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_widget_animation_definition", Tag = "wgan", Size = 0x78, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "gui_widget_animation_definition", Tag = "wgan", Size = 0x80, MinVersion = CacheVersion.HaloOnline106708)]
    public class GuiWidgetAnimationDefinition
    {
        public uint Unknown;
        public uint Unknown2;
        public CachedTagInstance WidgetColor;
        public CachedTagInstance WidgetPosition;
        public CachedTagInstance WidgetRotation;
        public CachedTagInstance WidgetScale;
        public CachedTagInstance WidgetTextureCoordinate;
        public CachedTagInstance WidgetSprite;
        public CachedTagInstance WidgetFont;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown3;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown4;
    }
}
