using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "hud_globals", Tag = "hudg", Size = 0x450)]
    public class HudGlobals : TagStructure
    {
        public AnchorValue Anchor;
        [TagField(Length = 0x2)]
        public byte[] Padding;
        [TagField(Length = 0x20)]
        public byte[] Padding1;
        public Point2d AnchorOffset;
        public float WidthScale;
        public float HeightScale;
        public ScalingFlagsValue ScalingFlags;
        [TagField(Length = 0x2)]
        public byte[] Padding2;
        [TagField(Length = 0x14)]
        public byte[] Padding3;
        [TagField(ValidTags = new [] { "font" })]
        public CachedTag SinglePlayerFont;
        [TagField(ValidTags = new [] { "font" })]
        public CachedTag MultiPlayerFont;
        public float UpTime;
        public float FadeTime;
        public RealArgbColor IconColor;
        public RealArgbColor TextColor;
        public float TextSpacing;
        [TagField(ValidTags = new [] { "ustr" })]
        public CachedTag ItemMessageText;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag IconBitmap;
        [TagField(ValidTags = new [] { "ustr" })]
        public CachedTag AlternateIconText;
        public List<HudButtonIconBlock> ButtonIcons;
        public ArgbColor DefaultColor;
        public ArgbColor FlashingColor;
        public float FlashPeriod;
        public float FlashDelay;
        public short NumberOfFlashes;
        public FlashFlagsValue FlashFlags;
        public float FlashLength;
        public ArgbColor DisabledColor;
        [TagField(Length = 0x4)]
        public byte[] Padding4;
        [TagField(ValidTags = new [] { "hmt " })]
        public CachedTag HudMessages;
        public ArgbColor DefaultColor1;
        public ArgbColor FlashingColor1;
        public float FlashPeriod1;
        public float FlashDelay1;
        public short NumberOfFlashes1;
        public FlashFlags1Value FlashFlags1;
        public float FlashLength1;
        public ArgbColor DisabledColor1;
        public short UptimeTicks;
        public short FadeTicks;
        /// <summary>
        /// The offset values are how much the waypoint rectangle border is offset from the safe camera bounds
        /// </summary>
        public float TopOffset;
        public float BottomOffset;
        public float LeftOffset;
        public float RightOffset;
        [TagField(Length = 0x20)]
        public byte[] Padding5;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag ArrowBitmap;
        public List<HudWaypointArrowBlock> WaypointArrows;
        [TagField(Length = 0x50)]
        public byte[] Padding6;
        public float HudScaleInMultiplayer;
        [TagField(Length = 0x100)]
        public byte[] Padding7;
        [TagField(ValidTags = new [] { "wphi" })]
        public CachedTag DefaultWeaponHud;
        public float MotionSensorRange;
        /// <summary>
        /// how fast something moves to show up on the motion sensor
        /// </summary>
        public float MotionSensorVelocitySensitivity;
        public float MotionSensorScaleDonTTouchEver;
        public Rectangle2d DefaultChapterTitleBounds;
        [TagField(Length = 0x2C)]
        public byte[] Padding8;
        public short TopOffset1;
        public short BottomOffset1;
        public short LeftOffset1;
        public short RightOffset1;
        [TagField(Length = 0x20)]
        public byte[] Padding9;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag IndicatorBitmap;
        public short SequenceIndex;
        public short MultiplayerSequenceIndex;
        public ArgbColor Color;
        [TagField(Length = 0x10)]
        public byte[] Padding10;
        public ArgbColor DefaultColor2;
        public ArgbColor FlashingColor2;
        public float FlashPeriod2;
        public float FlashDelay2;
        public short NumberOfFlashes2;
        public FlashFlags2Value FlashFlags2;
        public float FlashLength2;
        public ArgbColor DisabledColor2;
        [TagField(Length = 0x4)]
        public byte[] Padding11;
        public ArgbColor DefaultColor3;
        public ArgbColor FlashingColor3;
        public float FlashPeriod3;
        public float FlashDelay3;
        public short NumberOfFlashes3;
        public FlashFlags3Value FlashFlags3;
        public float FlashLength3;
        public ArgbColor DisabledColor3;
        [TagField(Length = 0x4)]
        public byte[] Padding12;
        [TagField(Length = 0x28)]
        public byte[] Padding13;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag CarnageReportBitmap;
        public short LoadingBeginText;
        public short LoadingEndText;
        public short CheckpointBeginText;
        public short CheckpointEndText;
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag CheckpointSound;
        [TagField(Length = 0x60)]
        public byte[] Padding14;
        
        public enum AnchorValue : short
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight,
            Center
        }
        
        public enum ScalingFlagsValue : ushort
        {
            DonTScaleOffset,
            DonTScaleSize,
            UseHighResScale
        }
        
        [TagStructure(Size = 0x10)]
        public class HudButtonIconBlock : TagStructure
        {
            /// <summary>
            /// sequence index into the global hud icon bitmap
            /// </summary>
            public short SequenceIndex;
            /// <summary>
            /// extra spacing beyond bitmap width for text alignment
            /// </summary>
            public short WidthOffset;
            public Point2d OffsetFromReferenceCorner;
            public ArgbColor OverrideIconColor;
            public sbyte FrameRate030;
            public FlagsValue Flags;
            public short TextIndex;
            
            public enum FlagsValue : byte
            {
                UseTextFromStringListInstead,
                OverrideDefaultColor,
                WidthOffsetIsAbsoluteIconWidth
            }
        }
        
        public enum FlashFlagsValue : ushort
        {
            ReverseDefaultFlashingColors
        }
        
        public enum FlashFlags1Value : ushort
        {
            ReverseDefaultFlashingColors
        }
        
        [TagStructure(Size = 0x68)]
        public class HudWaypointArrowBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            [TagField(Length = 0x8)]
            public byte[] Padding;
            public ArgbColor Color;
            public float Opacity;
            public float Translucency;
            public short OnScreenSequenceIndex;
            public short OffScreenSequenceIndex;
            public short OccludedSequenceIndex;
            [TagField(Length = 0x2)]
            public byte[] Padding1;
            [TagField(Length = 0x10)]
            public byte[] Padding2;
            public FlagsValue Flags;
            [TagField(Length = 0x18)]
            public byte[] Padding3;
            
            public enum FlagsValue : uint
            {
                DonTRotateWhenPointingOffscreen
            }
        }
        
        public enum FlashFlags2Value : ushort
        {
            ReverseDefaultFlashingColors
        }
        
        public enum FlashFlags3Value : ushort
        {
            ReverseDefaultFlashingColors
        }
    }
}

