using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "gui_button_key_definition", Tag = "bkey", Size = 0x54)]
    public class GuiButtonKeyDefinition
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
        public CachedTagInstance Strings;
        public List<TextWidget> TextWidgets;
        public List<BitmapWidget> BitmapWidgets;

        [TagStructure(Size = 0x4C)]
        public class TextWidget
        {
            public CachedTagInstance Parent;
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
        }

        [TagStructure(Size = 0x6C)]
        public class BitmapWidget
        {
            public CachedTagInstance Parent;
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
}
