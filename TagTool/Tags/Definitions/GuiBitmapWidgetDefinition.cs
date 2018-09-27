using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_bitmap_widget_definition", Tag = "bmp3", Size = 0x5C, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "gui_bitmap_widget_definition", Tag = "bmp3", Size = 0x60, MinVersion = CacheVersion.HaloOnline106708)]
    public class GuiBitmapWidgetDefinition : TagStructure
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
        public CachedTagInstance Bitmap;
        public CachedTagInstance Unknown2;
        public BlendMethodValue BlendMethod;
        public short Unknown3;
        public short SpriteIndex;
        public short Unknown4;
        public StringId DataSourceName;
        public StringId SpriteDataSourceName;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown5;

        public enum BlendMethodValue : short
        {
            Standard,
            Unknown,
            Unknown2,
            Alpha,
            Overlay,
            Unknown3,
            LighterColor,
            Unknown4,
            Unknown5,
            Unknown6,
            InvertedAlpha,
            Unknown7,
            Unknown8,
            Unknown9,
        }
    }
}