using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.GUI;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_bitmap_widget_definition", Tag = "bmp3", Size = 0x5C)]
    public class GuiBitmapWidgetDefinition : TagStructure
	{
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