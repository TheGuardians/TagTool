using TagTool.Cache;
using TagTool.Common;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_widget_animation_definition", Tag = "wgan", Size = 0x78, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "gui_widget_animation_definition", Tag = "wgan", Size = 0x88, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "gui_widget_animation_definition", Tag = "wgan", Size = 0x80, MinVersion = CacheVersion.HaloOnlineED)]
    public class GuiWidgetAnimationDefinition : TagStructure
	{
        public StringId Name;
        public WidgetAnimationFlags Flags;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public CachedTag ScreenEffect;

        public CachedTag ColorAnimation;
        public CachedTag PositionAnimation;
        public CachedTag RotationAnimation;
        public CachedTag ScaleAnimation;
        public CachedTag TextureCoordinateAnimation;
        public CachedTag SpriteAnimation;
        public CachedTag FontAnimation;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, Flags = TagFieldFlags.Padding, Length = 0x8)]
        public byte[] Padding;

        [Flags]
        public enum WidgetAnimationFlags : uint
        {
            UNUSED = 1 << 0
        }
    }
}
