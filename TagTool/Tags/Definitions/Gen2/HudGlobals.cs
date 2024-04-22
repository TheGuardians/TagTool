using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "hud_globals", Tag = "hudg", Size = 0x488)]
    public class HudGlobals : TagStructure
    {
        public AnchorValue Anchor;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        public Point2d AnchorOffset;
        public float WidthScale;
        public float HeightScale;
        public ScalingFlagsValue ScalingFlags;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
        public byte[] Padding3;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Obsolete1;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Obsolete2;
        public float UpTime;
        public float FadeTime;
        public RealArgbColor IconColor;
        public RealArgbColor TextColor;
        public float TextSpacing;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag ItemMessageText;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag IconBitmap;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag AlternateIconText;
        public List<HudButtonIconBlock> ButtonIcons;
        public ArgbColor DefaultColor;
        public ArgbColor FlashingColor;
        public float FlashPeriod;
        /// <summary>
        /// time between flashes
        /// </summary>
        public float FlashDelay;
        public short NumberOfFlashes;
        public FlashFlagsValue FlashFlags;
        /// <summary>
        /// time of each flash
        /// </summary>
        public float FlashLength;
        public ArgbColor DisabledColor;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding4;
        [TagField(ValidTags = new [] { "hmt " })]
        public CachedTag HudMessages;
        public ArgbColor DefaultColor1;
        public ArgbColor FlashingColor1;
        public float FlashPeriod1;
        /// <summary>
        /// time between flashes
        /// </summary>
        public float FlashDelay1;
        public short NumberOfFlashes1;
        public FlashFlagsValue1 FlashFlags1;
        /// <summary>
        /// time of each flash
        /// </summary>
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
        [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
        public byte[] Padding5;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag ArrowBitmap;
        public List<HudWaypointArrowBlock> WaypointArrows;
        [TagField(Length = 0x50, Flags = TagFieldFlags.Padding)]
        public byte[] Padding6;
        public float HudScaleInMultiplayer;
        [TagField(Length = 0x100, Flags = TagFieldFlags.Padding)]
        public byte[] Padding7;
        [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
        public byte[] Padding8;
        public float MotionSensorRange;
        /// <summary>
        /// how fast something moves to show up on the motion sensor
        /// </summary>
        public float MotionSensorVelocitySensitivity;
        public float MotionSensorScaleDonTTouchEver;
        public Rectangle2d DefaultChapterTitleBounds;
        [TagField(Length = 0x2C, Flags = TagFieldFlags.Padding)]
        public byte[] Padding9;
        public short TopOffset1;
        public short BottomOffset1;
        public short LeftOffset1;
        public short RightOffset1;
        [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
        public byte[] Padding10;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag IndicatorBitmap;
        public short SequenceIndex;
        public short MultiplayerSequenceIndex;
        public ArgbColor Color;
        [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
        public byte[] Padding11;
        public ArgbColor DefaultColor2;
        public ArgbColor FlashingColor2;
        public float FlashPeriod2;
        /// <summary>
        /// time between flashes
        /// </summary>
        public float FlashDelay2;
        public short NumberOfFlashes2;
        public FlashFlagsValue2 FlashFlags2;
        /// <summary>
        /// time of each flash
        /// </summary>
        public float FlashLength2;
        public ArgbColor DisabledColor2;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding12;
        public ArgbColor DefaultColor3;
        public ArgbColor FlashingColor3;
        public float FlashPeriod3;
        /// <summary>
        /// time between flashes
        /// </summary>
        public float FlashDelay3;
        public short NumberOfFlashes3;
        public FlashFlagsValue3 FlashFlags3;
        /// <summary>
        /// time of each flash
        /// </summary>
        public float FlashLength3;
        public ArgbColor DisabledColor3;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding13;
        [TagField(Length = 0x28, Flags = TagFieldFlags.Padding)]
        public byte[] Padding14;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag CarnageReportBitmap;
        public short LoadingBeginText;
        public short LoadingEndText;
        public short CheckpointBeginText;
        public short CheckpointEndText;
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag CheckpointSound;
        [TagField(Length = 0x60, Flags = TagFieldFlags.Padding)]
        public byte[] Padding15;
        public GlobalNewHudGlobalsStructBlock NewGlobals;
        
        public enum AnchorValue : short
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight,
            Center,
            Crosshair
        }
        
        [Flags]
        public enum ScalingFlagsValue : ushort
        {
            DonTScaleOffset = 1 << 0,
            DonTScaleSize = 1 << 1
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
            
            [Flags]
            public enum FlagsValue : byte
            {
                UseTextFromStringListInstead = 1 << 0,
                OverrideDefaultColor = 1 << 1,
                WidthOffsetIsAbsoluteIconWidth = 1 << 2
            }
        }
        
        [Flags]
        public enum FlashFlagsValue : ushort
        {
            ReverseDefaultFlashingColors = 1 << 0
        }
        
        [Flags]
        public enum FlashFlagsValue1 : ushort
        {
            ReverseDefaultFlashingColors = 1 << 0
        }
        
        [TagStructure(Size = 0x68)]
        public class HudWaypointArrowBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public ArgbColor Color;
            public float Opacity;
            public float Translucency;
            public short OnScreenSequenceIndex;
            public short OffScreenSequenceIndex;
            public short OccludedSequenceIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public FlagsValue Flags;
            [TagField(Length = 0x18, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            
            [Flags]
            public enum FlagsValue : uint
            {
                DonTRotateWhenPointingOffscreen = 1 << 0
            }
        }
        
        [Flags]
        public enum FlashFlagsValue2 : ushort
        {
            ReverseDefaultFlashingColors = 1 << 0
        }
        
        [Flags]
        public enum FlashFlagsValue3 : ushort
        {
            ReverseDefaultFlashingColors = 1 << 0
        }
        
        [TagStructure(Size = 0x90)]
        public class GlobalNewHudGlobalsStructBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "unic" })]
            public CachedTag HudText;
            public List<HudDashlightsBlock> Dashlights;
            public List<HudWaypointArrowBlock> WaypointArrows;
            public List<HudWaypointBlock> Waypoints;
            public List<NewHudSoundBlock> HudSounds;
            public List<PlayerTrainingEntryDataBlock> PlayerTrainingData;
            public GlobalNewHudGlobalsConstantsStructBlock Constants;
            
            [TagStructure(Size = 0x1C)]
            public class HudDashlightsBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag Bitmap;
                [TagField(ValidTags = new [] { "shad" })]
                public CachedTag Shader;
                public short SequenceIndex;
                public FlagsValue Flags;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Sound;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    DonTScaleWhenPulsing = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x24)]
            public class HudWaypointArrowBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag Bitmap;
                [TagField(ValidTags = new [] { "shad" })]
                public CachedTag Shader;
                public short SequenceIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float SmallestSize;
                public float SmallestDistance;
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag BorderBitmap;
            }
            
            [TagStructure(Size = 0x18)]
            public class HudWaypointBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag Bitmap;
                [TagField(ValidTags = new [] { "shad" })]
                public CachedTag Shader;
                public short OnscreenSequenceIndex;
                public short OccludedSequenceIndex;
                public short OffscreenSequenceIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x18)]
            public class NewHudSoundBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "snd!","lsnd" })]
                public CachedTag ChiefSound;
                public LatchedToValue LatchedTo;
                public float Scale;
                [TagField(ValidTags = new [] { "snd!","lsnd" })]
                public CachedTag DervishSound;
                
                [Flags]
                public enum LatchedToValue : uint
                {
                    ShieldRecharging = 1 << 0,
                    ShieldDamaged = 1 << 1,
                    ShieldLow = 1 << 2,
                    ShieldEmpty = 1 << 3,
                    HealthLow = 1 << 4,
                    HealthEmpty = 1 << 5,
                    HealthMinorDamage = 1 << 6,
                    HealthMajorDamage = 1 << 7,
                    RocketLocking = 1 << 8,
                    RocketLocked = 1 << 9
                }
            }
            
            [TagStructure(Size = 0x1C)]
            public class PlayerTrainingEntryDataBlock : TagStructure
            {
                /// <summary>
                /// comes out of the HUD text globals
                /// </summary>
                public StringId DisplayString;
                /// <summary>
                /// comes out of the HUD text globals, used for grouped prompt
                /// </summary>
                public StringId DisplayString2;
                /// <summary>
                /// comes out of the HUD text globals, used for ungrouped prompt
                /// </summary>
                public StringId DisplayString3;
                /// <summary>
                /// how long the message can be on screen before being hidden
                /// </summary>
                public short MaxDisplayTime;
                /// <summary>
                /// how many times a training message will get displayed (0-3 only!)
                /// </summary>
                public short DisplayCount;
                /// <summary>
                /// how long a displayed but untriggered message stays up
                /// </summary>
                public short DissapearDelay;
                /// <summary>
                /// how long after display this message will stay hidden
                /// </summary>
                public short RedisplayDelay;
                /// <summary>
                /// how long the event can be triggered before it's displayed
                /// </summary>
                public float DisplayDelayS;
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    NotInMultiplayer = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x60)]
            public class GlobalNewHudGlobalsConstantsStructBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "snd!","lsnd" })]
                public CachedTag PrimaryMessageSound;
                [TagField(ValidTags = new [] { "snd!","lsnd" })]
                public CachedTag SecondaryMessageSound;
                public StringId BootGrieferString;
                public StringId CannotBootGrieferString;
                [TagField(ValidTags = new [] { "shad" })]
                public CachedTag TrainingShader;
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag HumanTrainingTopRight;
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag HumanTrainingTopCenter;
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag HumanTrainingTopLeft;
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag HumanTrainingMiddle;
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag EliteTrainingTopRight;
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag EliteTrainingTopCenter;
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag EliteTrainingTopLeft;
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag EliteTrainingMiddle;
            }
        }
    }
}

