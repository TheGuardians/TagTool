using System;
using System.Collections.Generic;
using TagTool.Common;
using TagTool.Cache;

namespace TagTool.Tags.GUI
{
    [TagStructure(Size = 0x4C)]
    public class TextWidget : TagStructure
    {
        public CachedTag Parent;
        public GuiTextFlags Flags;
        public GuiDefinition GuiRenderBlock;
        public StringId DataSourceName;
        public StringId TextString;
        public StringId TextColor;
        public FontValue TextFont;
        public short Unknown;
    }

    [Flags]
    public enum GuiTextFlags : int
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
        CutAtBounds = 1 << 14,
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

    [TagStructure(Size = 0x6C)]
    public class BitmapWidget : TagStructure
    {
        public CachedTag Parent;
        public GuiBitmapFlags Flags;
        public GuiDefinition GuiRenderBlock;
        public CachedTag Bitmap;
        public CachedTag Unknown;
        public BlendMethodValue BlendMethod;
        public short Unknown1;
        public short SpriteIndex;
        public short Unknown2;
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

    [Flags]
    public enum GuiBitmapFlags : int
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

    [TagStructure(Size = 0x80)]
    public class ListWidget : TagStructure
    {
        public CachedTag Parent;
        public ListWidgetFlags Flags;
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
            public uint Flags;
            public GuiDefinition GuiRenderBlock;
            public StringId Target;
        }
    }

    [Flags]
    public enum ListWidgetFlags : int
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

    [TagStructure(Size = 0x48, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Size = 0x94, MinVersion = CacheVersion.Halo3ODST)]
    public class ModelWidget : TagStructure
    {
        public CachedTag Parent;
        public ModelWidgetFlags Flags;
        public GuiDefinition GuiRenderBlock;

        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public List<CameraRefinementOld> CameraRefinementsOld;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<CameraRefinementNew> CameraRefinementsNew;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public ModelWidgetData ModelData;

        [Flags]
        public enum ModelWidgetFlags : int
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

        [TagStructure(Size = 0x3C, MaxVersion = CacheVersion.Halo3Retail)]
        public class CameraRefinementOld : TagStructure
        {
            public StringId Biped;
            public float BaseDistance;
            public Bounds<float> DistanceRange;
            public float DistanceChangeRate;
            public float BaseHeight;
            public Bounds<float> HeightRange;
            public float Unknown;
            public TagFunction DistanceFunction;
            public TagFunction HeightFunction;
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
            public TagFunction Unknown25;
            public uint Unknown26;
            public uint Unknown27;
            public Angle Unknown28;
            public uint Unknown29;
            public Angle Unknown30;
            public uint Unknown31;
            public uint Unknown32;
            public CachedTag Unknown33;
            public uint Unknown34;
        }

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
            public TagFunction ZoomFunction;
            public GamepadButtonDefinition MovementLeft;
            public GamepadButtonDefinition MovementRight;
            public GamepadButtonDefinition MovementUp;
            public GamepadButtonDefinition MovementDown;
            public GamepadButtonDefinition Unknown16;
            public GamepadButtonDefinition Unknown17;
            public GamepadButtonDefinition ZoomIn;
            public GamepadButtonDefinition ZoomOut;
            public GamepadButtonDefinition RotateLeft;
            public GamepadButtonDefinition RotateRight;
            public GamepadButtonDefinition Unknown22;
            public GamepadButtonDefinition Unknown23;
            public List<TexCamBlock> TextureCameraSections;

            [TagStructure(Size = 0x14)]
            public class TexCamBlock : TagStructure
            {
                public StringId Name;
                public Bounds<float> BoundsX;
                public Bounds<float> BoundsY;
            }
        }
    }

    public enum GamepadButtonDefinition : short
    {
        LeftTrigger,
        RightTrigger,
        DpadUp,
        DpadDown,
        DpadLeft,
        DpadRight,
        Start,
        Back,
        LeftThumb,
        RightThumb,
        ButtonA,
        ButtonB,
        ButtonX,
        ButtonY,
        LeftBumper,
        RightBumper,
        LeftStickLeft,
        LeftStickRight,
        LeftStickUp,
        LeftStickDown,
        RightStickLeft,
        RightStickRight,
        RightStickUp,
        RightStickDown,
        Unknown
    }

    [Flags]
    public enum GuiAnimationFlags : int
    {
        None = 0,
        Loops = 1 << 0,
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
}