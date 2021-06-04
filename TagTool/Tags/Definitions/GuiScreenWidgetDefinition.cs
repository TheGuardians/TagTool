using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using TagTool.Tags.GUI;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_screen_widget_definition", Tag = "scn3", Size = 0xA8)]
    public class GuiScreenWidgetDefinition : TagStructure
	{
        public GuiScreenWidgetFlags Flags;
        public GuiDefinition GuiRenderBlock;
        public CachedTag Strings;
        public CachedTag Parent;
        public StringId DefaultKeyLegendString;
        public List<DataSource> DataSources;
        public List<GroupWidget> GroupWidgets;
        public List<ButtonKeyLegend> ButtonKeyLegends;
        public CachedTag UiSounds;
        [TagField(Length = 32)] public string ScriptTitle;
        public short ScriptIndex;
        public short Unknown2;

        [TagStructure(Size = 0x10)]
        public class DataSource : TagStructure
		{
            public CachedTag DataSource2;
        }

        [TagStructure(Size = 0x6C)]
        public class GroupWidget : TagStructure
		{
            public CachedTag Parent;
            public GroupWidgetsFlags Flags;
            public GuiDefinition GuiRenderBlock;
            public List<ListWidget> ListWidgets;
            public List<TextWidget> TextWidgets;
            public List<BitmapWidget> BitmapWidgets;
            public List<ModelWidget> ModelWidgets;

            [TagStructure(Size = 0x80)]
            public class ListWidget : TagStructure
			{
                public CachedTag Parent;
                public ListWidgetsFlags Flags;
                public GuiDefinition GuiRenderBlock;
                public StringId DataSourceName;
                public CachedTag Skin;
                public int RowCount;
                public List<ListWidgetItem> ListWidgetItems;
                public CachedTag UpArrowBitmap;
                public CachedTag DownArrowBitmap;

                [TagStructure(Size = 0x30)]
                public class ListWidgetItem : TagStructure
				{
                    public ListWidgetItemsFlags Flags;
                    public GuiDefinition GuiRenderBlock;
                    public StringId Target;
                }
            }

            [TagStructure(Size = 0x4C)]
            public class TextWidget : TagStructure
			{
                public CachedTag Parent;
                public TextWidgetsFlags Flags;
                public GuiDefinition GuiRenderBlock;
                public StringId DataSourceName;
                public StringId TextString;
                public StringId TextColor;
                public FontValue TextFont;
                public short Unknown2;
            }

            [TagStructure(Size = 0x6C)]
            public class BitmapWidget : TagStructure
			{
                public CachedTag Parent;
                public BitmapWidgetsFlags Flags;
                public GuiDefinition GuiRenderBlock;
                public CachedTag Bitmap;
                public CachedTag Unknown2;
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

            [TagStructure(Size = 0x48, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x94, MinVersion = CacheVersion.Halo3ODST)]
            public class ModelWidget : TagStructure
			{
                public CachedTag Parent;
                public uint Flags;
                public GuiDefinition GuiRenderBlock;

                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public List<CameraRefinementOld> CameraRefinementsOld;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public List<CameraRefinementNew> CameraRefinementsNew;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public ModelWidgetData ModelData;

                [TagStructure(Size = 0x4C)]
				public class ModelWidgetData : TagStructure
				{
                    public uint Unknown4;
                    public uint Unknown5;
                    public uint Unknown6;
                    public uint Unknown7;
                    public uint Unknown8;
                    public uint Unknown9;
                    public uint Unknown10;
                    public List<UnknownBlock1> Unknown11;
                    public short Unknown12;
                    public short Unknown13;
                    public short Unknown14;
                    public short Unknown15;
                    public short Unknown16;
                    public short Unknown17;
                    public short Unknown18;
                    public short Unknown19;
                    public short Unknown20;
                    public short Unknown21;
                    public short Unknown22;
                    public short Unknown23;
                    public List<UnknownBlock2> Unknown24;

                    [TagStructure(Size = 0x14)]
                    public class UnknownBlock1 : TagStructure
					{
                        public byte[] Unknown;
                    }

                    [TagStructure(Size = 0x14)]
                    public class UnknownBlock2 : TagStructure
					{
                        public StringId Unknown;
                        public uint Unknown2;
                        public uint Unknown3;
                        public uint Unknown4;
                        public uint Unknown5;
                    }
                }

                [TagStructure(Size = 0x3C, MaxVersion = CacheVersion.Halo3Retail)]
                public class CameraRefinementOld : TagStructure
				{
                    public StringId Biped;
                    public float Unknown;
                    public float Unknown2;
                    public float Unknown3;
                    public float Unknown4;
                    public float Unknown5;
                    public float Unknown6;
                    public float Unknown7;
                    public float Unknown8;
                    public List<ZoomData> ZoomData1;
                    public List<ZoomData> ZoomData2;

                    [TagStructure(Size = 0x14)]
                    public class ZoomData : TagStructure
                    {
                        public byte[] Unknown;
                    }
                }

                [TagStructure(Size = 0xA0, MinVersion = CacheVersion.Halo3ODST)]
                public class CameraRefinementNew : TagStructure
				{
                    public StringId Biped;
                    public uint Unknown;
                    public uint Unknown2;
                    public uint Unknown3;
                    public uint Unknown4;
                    public uint Unknown5;
                    public uint Unknown6;
                    public uint Unknown7;
                    public uint Unknown8;
                    public uint Unknown9;
                    public uint Unknown10;
                    public uint Unknown11;
                    public uint Unknown12;
                    public uint Unknown13;
                    public uint Unknown14;
                    public uint Unknown15;
                    public uint Unknown16;
                    public uint Unknown17;
                    public uint Unknown18;
                    public uint Unknown19;
                    public uint Unknown20;
                    public uint Unknown21;
                    public uint Unknown22;
                    public uint Unknown23;
                    public uint Unknown24;
                    public List<UnknownBlock> Unknown25;
                    public uint Unknown26;
                    public uint Unknown27;
                    public Angle Unknown28;
                    public uint Unknown29;
                    public Angle Unknown30;
                    public uint Unknown31;
                    public uint Unknown32;
                    public CachedTag Unknown33;
                    public uint Unknown34;

                    [TagStructure(Size = 0x14)]
                    public class UnknownBlock : TagStructure
					{
                        public byte[] Unknown;
                    }
                }
            }
        }

        [TagStructure(Size = 0x10)]
        public class ButtonKeyLegend : TagStructure
		{
            public CachedTag Legend;
        }
    }

    [Flags]
    public enum GuiScreenWidgetFlags : int
    {
        None = 0,
        Bit0 = 1 << 0,
        Bit1 = 1 << 1,
        Bit2 = 1 << 2,
        Bit3 = 1 << 3,
        Bit4 = 1 << 4,
        Bit5 = 1 << 5,
        Bit6 = 1 << 6,
        Bit7 = 1 << 7,
        Bit8 = 1 << 8,
        Bit9 = 1 << 9,
        Bit10 = 1 << 10,
        Bit11 = 1 << 11,
        Bit12 = 1 << 12,
        Bit13 = 1 << 13,
        Bit14 = 1 << 14,
        Bit15 = 1 << 15,
        Bit16 = 1 << 16,
        Bit17 = 1 << 17,
        Bit18 = 1 << 18,
        Bit19 = 1 << 19,
        Bit20 = 1 << 20,
        Bit21 = 1 << 21,
        Bit22 = 1 << 22,
        Bit23 = 1 << 23,
        Bit24 = 1 << 24,
        Bit25 = 1 << 25,
        Bit26 = 1 << 26,
        Bit27 = 1 << 27,
        Bit28 = 1 << 28,
        Bit29 = 1 << 29,
        Bit30 = 1 << 30,
        Bit31 = 1 << 31
    }

    [Flags]
    public enum GroupWidgetsFlags : int
    {
        None = 0,
        Bit0 = 1 << 0,
        Bit1 = 1 << 1,
        Bit2 = 1 << 2,
        InitiallyHidden = 1 << 3,
        Bit4 = 1 << 4,
        Bit5 = 1 << 5,
        Bit6 = 1 << 6,
        Bit7 = 1 << 7,
        Bit8 = 1 << 8,
        Bit9 = 1 << 9,
        Bit10 = 1 << 10,
        Bit11 = 1 << 11,
        Bit12 = 1 << 12,
        Bit13 = 1 << 13,
        Bit14 = 1 << 14,
        Bit15 = 1 << 15,
        Bit16 = 1 << 16,
        Bit17 = 1 << 17,
        Bit18 = 1 << 18,
        Bit19 = 1 << 19,
        Bit20 = 1 << 20,
        Bit21 = 1 << 21,
        Bit22 = 1 << 22,
        Bit23 = 1 << 23,
        Bit24 = 1 << 24,
        Bit25 = 1 << 25,
        Bit26 = 1 << 26,
        Bit27 = 1 << 27,
        Bit28 = 1 << 28,
        Bit29 = 1 << 29,
        Bit30 = 1 << 30,
        Bit31 = 1 << 31
    }

    [Flags]
    public enum ListWidgetsFlags : int
    {
        None = 0,
        Bit0 = 1 << 0,
        Bit1 = 1 << 1,
        Bit2 = 1 << 2,
        Bit3 = 1 << 3,
        Horizontal = 1 << 4,
        Loops = 1 << 5,
        Bit6 = 1 << 6,
        Bit7 = 1 << 7,
        Bit8 = 1 << 8,
        Bit9 = 1 << 9,
        Bit10 = 1 << 10,
        Bit11 = 1 << 11,
        Bit12 = 1 << 12,
        Bit13 = 1 << 13,
        Bit14 = 1 << 14,
        Bit15 = 1 << 15,
        Bit16 = 1 << 16,
        Bit17 = 1 << 17,
        Bit18 = 1 << 18,
        Bit19 = 1 << 19,
        Bit20 = 1 << 20,
        Bit21 = 1 << 21,
        Bit22 = 1 << 22,
        Bit23 = 1 << 23,
        Bit24 = 1 << 24,
        Bit25 = 1 << 25,
        Bit26 = 1 << 26,
        Bit27 = 1 << 27,
        Bit28 = 1 << 28,
        Bit29 = 1 << 29,
        Bit30 = 1 << 30,
        Bit31 = 1 << 31
    }

    [Flags]
    public enum ListWidgetItemsFlags : int
    {
        None = 0,
        Bit0 = 1 << 0,
        Bit1 = 1 << 1,
        Bit2 = 1 << 2,
        Bit3 = 1 << 3,
        Bit4 = 1 << 4,
        Bit5 = 1 << 5,
        Bit6 = 1 << 6,
        Bit7 = 1 << 7,
        Bit8 = 1 << 8,
        Bit9 = 1 << 9,
        Bit10 = 1 << 10,
        Bit11 = 1 << 11,
        Bit12 = 1 << 12,
        Bit13 = 1 << 13,
        Bit14 = 1 << 14,
        Bit15 = 1 << 15,
        Bit16 = 1 << 16,
        Bit17 = 1 << 17,
        Bit18 = 1 << 18,
        Bit19 = 1 << 19,
        Bit20 = 1 << 20,
        Bit21 = 1 << 21,
        Bit22 = 1 << 22,
        Bit23 = 1 << 23,
        Bit24 = 1 << 24,
        Bit25 = 1 << 25,
        Bit26 = 1 << 26,
        Bit27 = 1 << 27,
        Bit28 = 1 << 28,
        Bit29 = 1 << 29,
        Bit30 = 1 << 30,
        Bit31 = 1 << 31
    }

    [Flags]
    public enum TextWidgetsFlags : int
    {
        None = 0,
        Bit0 = 1 << 0,
        Bit1 = 1 << 1,
        Bit2 = 1 << 2,
        LeftAlignment = 1 << 3,
        RightAlignment = 1 << 4,
        Bit5 = 1 << 5,
        AllCaps = 1 << 6,
        Bit7 = 1 << 7,
        Bit8 = 1 << 8,
        Bit9 = 1 << 9,
        Bit10 = 1 << 10,
        Bit11 = 1 << 11,
        Bit12 = 1 << 12,
        WrapAtBounds = 1 << 13,
        Bit14 = 1 << 14,
        Bit15 = 1 << 15,
        Bit16 = 1 << 16,
        Bit17 = 1 << 17,
        Bit18 = 1 << 18,
        Bit19 = 1 << 19,
        Bit20 = 1 << 20,
        Bit21 = 1 << 21,
        Bit22 = 1 << 22,
        Bit23 = 1 << 23,
        Bit24 = 1 << 24,
        Bit25 = 1 << 25,
        Bit26 = 1 << 26,
        Bit27 = 1 << 27,
        Bit28 = 1 << 28,
        Bit29 = 1 << 29,
        Bit30 = 1 << 30,
        Bit31 = 1 << 31
    }

    [Flags]
    public enum BitmapWidgetsFlags : int
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
        ReplaceWithBlack = 1 << 11,
        Bit12 = 1 << 12,
        Bit13 = 1 << 13,
        Bit14 = 1 << 14,
        Bit15 = 1 << 15,
        Bit16 = 1 << 16,
        Bit17 = 1 << 17,
        Bit18 = 1 << 18,
        Bit19 = 1 << 19,
        Bit20 = 1 << 20,
        Bit21 = 1 << 21,
        Bit22 = 1 << 22,
        Bit23 = 1 << 23,
        Bit24 = 1 << 24,
        Bit25 = 1 << 25,
        Bit26 = 1 << 26,
        Bit27 = 1 << 27,
        Bit28 = 1 << 28,
        Bit29 = 1 << 29,
        Bit30 = 1 << 30,
        Bit31 = 1 << 31
    }
}
