using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using TagTool.Tags.GUI;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_screen_widget_definition", Tag = "scn3", Size = 0xA8)]
    public class GuiScreenWidgetDefinition : TagStructure
	{
        public uint Flags;
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
            public uint Flags;
            public GuiDefinition GuiRenderBlock;
            public List<ListWidget> ListWidgets;
            public List<TextWidget> TextWidgets;
            public List<BitmapWidget> BitmapWidgets;
            public List<ModelWidget> ModelWidgets;

            [TagStructure(Size = 0x80)]
            public class ListWidget : TagStructure
			{
                public CachedTag Parent;
                public uint Flags;
                public GuiDefinition GuiRenderBlock;
                public StringId DataSourceName;
                public CachedTag Skin;
                public int Unknown2;
                public List<ListWidgetItem> ListWidgetItems;
                public CachedTag UpArrowBitmap;
                public CachedTag DownArrowBitmap;

                [TagStructure(Size = 0x30)]
                public class ListWidgetItem : TagStructure
				{
                    public uint Flags;
                    public GuiDefinition GuiRenderBlock;
                    public StringId Target;
                }
            }

            [TagStructure(Size = 0x4C)]
            public class TextWidget : TagStructure
			{
                public CachedTag Parent;
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
                public CachedTag Parent;
                public uint Flags;
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
}
