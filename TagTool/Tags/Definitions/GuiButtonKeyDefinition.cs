using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_button_key_definition", Tag = "bkey", Size = 0x54)]
    public class GuiButtonKeyDefinition : TagStructure
	{
        public uint Flags;
        public GuiDefinition GuiRenderBlock;
        public CachedTagInstance Strings;
        public List<TextWidget> TextWidgets;
        public List<BitmapWidget> BitmapWidgets;

        [TagStructure(Size = 0x4C)]
        public class TextWidget : TagStructure
		{
            public CachedTagInstance Parent;
            public uint Flags;
            public GuiDefinition GuiRenderBlock;
            public StringId DataSourceName;
            public StringId TextString;
            public StringId TextColor;
            public short TextFont;
            public short Unknown2;
        }

        [TagStructure(Size = 0x6C)]
        public class BitmapWidget : TagStructure
		{
            public CachedTagInstance Parent;
            public uint Flags;
            public GuiDefinition GuiRenderBlock;
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
