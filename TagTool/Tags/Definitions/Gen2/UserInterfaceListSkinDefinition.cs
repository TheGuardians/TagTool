using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "user_interface_list_skin_definition", Tag = "skin", Size = 0x3C)]
    public class UserInterfaceListSkinDefinition : TagStructure
    {
        public ListFlagsValue ListFlags;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag ArrowsBitmap;
        public Point2d UpArrowsOffset; // from bot-left of 1st item
        public Point2d DownArrowsOffset; // from bot-left of 1st item
        /// <summary>
        /// Animations ordered as follows:
        /// 0) list item focused
        /// 1) list item unfocused
        /// 2) list item ambient
        /// 3) list item hovered
        /// 4)
        /// list item unhovered
        /// 5) list item clicked (hovered->focused)
        /// 6) list item unfocused back to hovered state
        /// (focused->hovered)
        /// 
        /// </summary>
        public List<SingleAnimationReferenceBlock> ItemAnimations;
        public List<TextBlockReferenceBlock> TextBlocks;
        /// <summary>
        /// the bitmap block top-left is actually bottom-left here in list skin land!
        /// </summary>
        public List<BitmapBlockReferenceBlock> BitmapBlocks;
        public List<HudBlockReferenceBlock> HudBlocks;
        public List<PlayerBlockReferenceBlock> PlayerBlocks;
        
        [Flags]
        public enum ListFlagsValue : uint
        {
            Unused = 1 << 0
        }
        
        [TagStructure(Size = 0x10)]
        public class SingleAnimationReferenceBlock : TagStructure
        {
            public FlagsValue Flags;
            public int AnimationPeriod; // milliseconds
            public List<ScreenAnimationKeyframeReferenceBlock> Keyframes;
            
            [Flags]
            public enum FlagsValue : uint
            {
                Unused = 1 << 0
            }
            
            [TagStructure(Size = 0x14)]
            public class ScreenAnimationKeyframeReferenceBlock : TagStructure
            {
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float Alpha;
                public RealPoint3d Position;
            }
        }
        
        [TagStructure(Size = 0x2C)]
        public class TextBlockReferenceBlock : TagStructure
        {
            public TextFlagsValue TextFlags;
            public AnimationIndexValue AnimationIndex;
            public short IntroAnimationDelayMilliseconds;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public CustomFontValue CustomFont;
            public RealArgbColor TextColor;
            public Rectangle2d TextBounds;
            public StringId StringId;
            public short RenderDepthBias;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            
            [Flags]
            public enum TextFlagsValue : uint
            {
                LeftJustifyText = 1 << 0,
                RightJustifyText = 1 << 1,
                PulsatingText = 1 << 2,
                CalloutText = 1 << 3,
                Small31CharBuffer = 1 << 4
            }
            
            public enum AnimationIndexValue : short
            {
                None,
                _00,
                _01,
                _02,
                _03,
                _04,
                _05,
                _06,
                _07,
                _08,
                _09,
                _10,
                _11,
                _12,
                _13,
                _14,
                _15,
                _16,
                _17,
                _18,
                _19,
                _20,
                _21,
                _22,
                _23,
                _24,
                _25,
                _26,
                _27,
                _28,
                _29,
                _30,
                _31,
                _32,
                _33,
                _34,
                _35,
                _36,
                _37,
                _38,
                _39,
                _40,
                _41,
                _42,
                _43,
                _44,
                _45,
                _46,
                _47,
                _48,
                _49,
                _50,
                _51,
                _52,
                _53,
                _54,
                _55,
                _56,
                _57,
                _58,
                _59,
                _60,
                _61,
                _62,
                _63
            }
            
            public enum CustomFontValue : short
            {
                Terminal,
                BodyText,
                Title,
                SuperLargeFont,
                LargeBodyText,
                SplitScreenHudMessage,
                FullScreenHudMessage,
                EnglishBodyText,
                HudNumberText,
                SubtitleFont,
                MainMenuFont,
                TextChatFont
            }
        }
        
        [TagStructure(Size = 0x38)]
        public class BitmapBlockReferenceBlock : TagStructure
        {
            public FlagsValue Flags;
            public AnimationIndexValue AnimationIndex;
            public short IntroAnimationDelayMilliseconds;
            public BitmapBlendMethodValue BitmapBlendMethod;
            public short InitialSpriteFrame;
            public Point2d TopLeft;
            public float HorizTextureWrapsSecond;
            public float VertTextureWrapsSecond;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag BitmapTag;
            public short RenderDepthBias;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float SpriteAnimationSpeedFps;
            public Point2d ProgressBottomLeft;
            public StringId StringIdentifier;
            public RealVector2d ProgressScale;
            
            [Flags]
            public enum FlagsValue : uint
            {
                IgnoreForListSkinSizeCalculation = 1 << 0,
                SwapOnRelativeListPosition = 1 << 1,
                RenderAsProgressBar = 1 << 2
            }
            
            public enum AnimationIndexValue : short
            {
                None,
                _00,
                _01,
                _02,
                _03,
                _04,
                _05,
                _06,
                _07,
                _08,
                _09,
                _10,
                _11,
                _12,
                _13,
                _14,
                _15,
                _16,
                _17,
                _18,
                _19,
                _20,
                _21,
                _22,
                _23,
                _24,
                _25,
                _26,
                _27,
                _28,
                _29,
                _30,
                _31,
                _32,
                _33,
                _34,
                _35,
                _36,
                _37,
                _38,
                _39,
                _40,
                _41,
                _42,
                _43,
                _44,
                _45,
                _46,
                _47,
                _48,
                _49,
                _50,
                _51,
                _52,
                _53,
                _54,
                _55,
                _56,
                _57,
                _58,
                _59,
                _60,
                _61,
                _62,
                _63
            }
            
            public enum BitmapBlendMethodValue : short
            {
                Standard,
                Multiply,
                Unused
            }
        }
        
        [TagStructure(Size = 0x24)]
        public class HudBlockReferenceBlock : TagStructure
        {
            public FlagsValue Flags;
            public AnimationIndexValue AnimationIndex;
            public short IntroAnimationDelayMilliseconds;
            public short RenderDepthBias;
            public short StartingBitmapSequenceIndex;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Bitmap;
            [TagField(ValidTags = new [] { "shad" })]
            public CachedTag Shader;
            public Rectangle2d Bounds;
            
            [Flags]
            public enum FlagsValue : uint
            {
                IgnoreForListSkinSize = 1 << 0,
                NeedsValidRank = 1 << 1
            }
            
            public enum AnimationIndexValue : short
            {
                None,
                _00,
                _01,
                _02,
                _03,
                _04,
                _05,
                _06,
                _07,
                _08,
                _09,
                _10,
                _11,
                _12,
                _13,
                _14,
                _15,
                _16,
                _17,
                _18,
                _19,
                _20,
                _21,
                _22,
                _23,
                _24,
                _25,
                _26,
                _27,
                _28,
                _29,
                _30,
                _31,
                _32,
                _33,
                _34,
                _35,
                _36,
                _37,
                _38,
                _39,
                _40,
                _41,
                _42,
                _43,
                _44,
                _45,
                _46,
                _47,
                _48,
                _49,
                _50,
                _51,
                _52,
                _53,
                _54,
                _55,
                _56,
                _57,
                _58,
                _59,
                _60,
                _61,
                _62,
                _63
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class PlayerBlockReferenceBlock : TagStructure
        {
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "skin" })]
            public CachedTag Skin;
            public Point2d BottomLeft;
            public TableOrderValue TableOrder;
            public sbyte MaximumPlayerCount;
            public sbyte RowCount;
            public sbyte ColumnCount;
            public short RowHeight;
            public short ColumnWidth;
            
            public enum TableOrderValue : sbyte
            {
                RowMajor,
                ColumnMajor
            }
        }
    }
}

