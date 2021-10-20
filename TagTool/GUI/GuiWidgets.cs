using System;
using System.Collections.Generic;
using TagTool.Common;
using TagTool.Cache;

namespace TagTool.Tags.GUI
{
    [TagStructure(Size = 0x4C)]
    public class TextWidget : TagStructure
    {
        public CachedTag TemplateReference;
        public GuiTextFlags Flags;
        public GuiDefinition GuiRenderBlock;
        public StringId ValueOverrideList;
        public StringId ValueIdentifier;
        public StringId TextColorPreset;

        [TagField(MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
        public WidgetFontValue_H3Original CustomFont_H3;

        [TagField(MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original, Length = 2, Flags = TagFieldFlags.Padding)]
        public byte[] TextWidgetH3OriginalPadding;

        [TagField(MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        public WidgetFontValue_H3MCC CustomFont_H3MCC;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
        public WidgetFontValue_ODST CustomFont_ODST;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public WidgetFontValue CustomFont;
	}

    [Flags]
    public enum GuiTextFlags : int
    {
        DoNotApplyOldContentUpscaling = 1 << 0,
        OverrideTemplateFlags = 1 << 1,
        EnableAnimationDebugging = 1 << 2,
        LeftJustify = 1 << 3,
        RightJustify = 1 << 4,
        Scrollable = 1 << 5,
        Uppercase = 1 << 6,
        StringFromExportedText = 1 << 7,
        StringFromExportedStringId = 1 << 8,
        StringFromExportedGlobalStringId = 1 << 9,
        StringFromExportedInteger = 1 << 10,
        StringFromListItemLabel = 1 << 11,
        UseBracketsToIndicateFocus = 1 << 12,
        LargeTextBuffer255Chars = 1 << 13,
        ExtralargeTextBuffer = 1 << 14,
        SingleDropShadow = 1 << 15,
        NoDropShadow = 1 << 16,
        AllowListItemToOverrideAnimationSkin = 1 << 17,
        DoNotWrapText = 1 << 18,
        Clickable = 1 << 19
    }

    [TagStructure(Size = 0x6C)]
    public class BitmapWidget : TagStructure
    {
        public CachedTag TemplateReference;
        public GuiBitmapFlags Flags;
        public GuiDefinition GuiRenderBlock;
        public CachedTag Bitmap;
        public CachedTag CustomPixelShader;
        public BlendMethodValue BlendMethod;
        public short InitialSpriteSequence;
        public short InitialSpriteFrame;

        [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
        public byte[] BitmapWidgetDefinitionAlignmentPad;

        public StringId ValueOverrideList;
        public StringId ValueIdentifier;

        public enum BlendMethodValue : short
        {
            Opaque,
            Additive,
            Multiply,
            AlphaBlend,
            DoubleMultiply,
            PreMultipliedAlpha,
            Maximum,
            MultiplyAdd,
            AddSrcTimesDstalpha,
            AddSrcTimesSrcalpha,
            InverseAlphaBlend,
            MotionBlurStatic,
            MotionBlurInhibit,
            AddSrcTimesSrcalphaAlphaAccum
        }
    }

    [Flags]
    public enum GuiBitmapFlags : int
    {
        DoNotApplyOldContentUpscaling = 1 << 0,
        OverrideTemplateFlags = 1 << 1,
        EnableAnimationDebugging = 1 << 2,
        ScaleToFitBounds = 1 << 3,
        RenderAsScreenBlur = 1 << 4,
        RenderAsPlayerEmblem = 1 << 5,
        SpriteFromExportedInteger = 1 << 6,
        SequenceFromExportedInteger = 1 << 7,
        AttachShaderToExportedInteger = 1 << 8,
        AllowListItemToOverrideAnimationSkin = 1 << 9,
        Clickable = 1 << 10
    }

    [TagStructure(Size = 0x80)]
    public class ListWidget : TagStructure
    {
        public CachedTag TemplateReference;
        public ListWidgetFlags Flags;
        public GuiDefinition GuiRenderBlock;
        public StringId DataSourceName;
        public CachedTag Skin;
        public uint Rows;
        public List<ListWidgetItem> ListWidgetItems;
        public CachedTag UpArrowBitmap;
        public CachedTag DownArrowBitmap;

        [Flags]
        public enum ListWidgetFlags : int
        {
            DoNotApplyOldContentUpscaling = 1 << 0,
            OverrideTemplateFlags = 1 << 1,
            EnableAnimationDebugging = 1 << 2,
            SubmenuList = 1 << 3,
            HorizontalList = 1 << 4,
            ListWraps = 1 << 5,
            SelectionVisibleWithoutFocus = 1 << 6,
            Noninteractive = 1 << 7
        }

        [TagStructure(Size = 0x30)]
        public class ListWidgetItem : TagStructure
        {
            public ListItemWidgetFlags Flags;
            public GuiDefinition GuiRenderBlock;
            public StringId ItemLabel;

            [Flags]
            public enum ListItemWidgetFlags : uint
            {
                DoNotApplyOldContentUpscaling = 1 << 0,
                OverrideTemplateFlags = 1 << 1,
                EnableAnimationDebugging = 1 << 2
            }
        }
    }

    [TagStructure(Size = 0x48, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Size = 0x94, MinVersion = CacheVersion.Halo3ODST)]
    public class ModelWidget : TagStructure
    {
        public CachedTag TemplateReference;
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
            DoNotApplyOldContentUpscaling = 1 << 0,
            OverrideTemplateFlags = 1 << 1,
            EnableAnimationDebugging = 1 << 2,
            UNUSED = 1 << 3
        }

        [TagStructure(Size = 0x48, MaxVersion = CacheVersion.Halo3Retail)]
        public class CameraRefinementOld : TagStructure
        {
            public StringId Name;
            public float Fov;
            public float InitialRadialOffset;
            public float FinalRadialOffset;
            public float CameraRadialStepSize;
            public float InitialVerticalOffset;
            public float FinalVerticalOffset;
            public float CameraVerticalStepSize;
            public float CameraRotationalStep;
            //public TagFunction DistanceFunction;
            //public TagFunction HeightFunction;
            [TagField(Length = 4, Flags = TagFieldFlags.Padding)]
            public byte[] Pad6400;
            public List<KeyframeTransitionFunctionBlock> RadialTransitionFxn;
            [TagField(Length = 4, Flags = TagFieldFlags.Padding)]
            public byte[] Pad6401;
            public List<KeyframeTransitionFunctionBlock> VerticalTransitionFxn;
            [TagField(Length = 4, Flags = TagFieldFlags.Padding)]
            public byte[] Pad6402;

            [TagStructure(Size = 0x14)]
            public class KeyframeTransitionFunctionBlock : TagStructure
            {
                public byte[] CustomFunction;
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