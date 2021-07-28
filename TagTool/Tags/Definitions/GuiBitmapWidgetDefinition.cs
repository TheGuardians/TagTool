using System;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.GUI;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_bitmap_widget_definition", Tag = "bmp3", Size = 0x5C)]
    public class GuiBitmapWidgetDefinition : TagStructure
	{
        public GuiBitmapWidgetFlags Flags;
        public GuiDefinition GuiRenderBlock;

        public CachedTag Bitmap;
        public CachedTag Unknown;
        public BlendMethodValue BlendMethod;
        public short Unknown1;
        public short SpriteIndex;
        public short Unknown2;
        public StringId DataSourceName;
        public StringId SpriteDataSourceName;

        [Flags]
        public enum GuiBitmapWidgetFlags : uint
        {
            None = 0,
            Bit0 = 1 << 0,
            Bit1 = 1 << 1,
            Bit2 = 1 << 2,
            ScaleToBounds = 1 << 3,
            ReplaceWithBlur = 1 << 4,
            Bit5 = 1 << 5,
            Bit6 = 1 << 6,
            Bit7 = 1 << 7,
            Bit8 = 1 << 8,
            Bit9 = 1 << 9,
            ReplaceWithWhite = 1 << 10,
            ReplaceWithBlack = 1 << 11
        }

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