using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "hud_globals", Tag = "hudg", Size = 0x554)]
    public class HudGlobals : TagStructure
    {
        /// <summary>
        /// Messaging parameters
        /// </summary>
        public AnchorValue Anchor;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        [TagField(Flags = Padding, Length = 32)]
        public byte[] Padding2;
        public Point2d AnchorOffset;
        public float WidthScale;
        public float HeightScale;
        public ScalingFlagsValue ScalingFlags;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding3;
        [TagField(Flags = Padding, Length = 20)]
        public byte[] Padding4;
        public CachedTag Obsolete1;
        public CachedTag Obsolete2;
        public float UpTime;
        public float FadeTime;
        public RealArgbColor IconColor;
        public RealArgbColor TextColor;
        public float TextSpacing;
        public CachedTag ItemMessageText;
        public CachedTag IconBitmap;
        public CachedTag AlternateIconText;
        public List<IconHudElementDefinition> ButtonIcons;
        /// <summary>
        /// HUD HELP TEXT COLOR
        /// </summary>
        public ArgbColor DefaultColor;
        public ArgbColor FlashingColor;
        public float FlashPeriod;
        public float FlashDelay; // time between flashes
        public short NumberOfFlashes;
        public FlashFlagsValue FlashFlags;
        public float FlashLength; // time of each flash
        public ArgbColor DisabledColor;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding5;
        /// <summary>
        /// Other hud messaging data
        /// </summary>
        public CachedTag HudMessages;
        /// <summary>
        /// Objective colors
        /// </summary>
        public ArgbColor DefaultColor1;
        public ArgbColor FlashingColor2;
        public float FlashPeriod3;
        public float FlashDelay4; // time between flashes
        public short NumberOfFlashes5;
        public FlashFlagsValue FlashFlags6;
        public float FlashLength7; // time of each flash
        public ArgbColor DisabledColor8;
        public short UptimeTicks;
        public short FadeTicks;
        /// <summary>
        /// Waypoint parameters
        /// </summary>
        /// <remarks>
        /// The offset values are how much the waypoint rectangle border is offset from the safe camera bounds
        /// </remarks>
        public float TopOffset;
        public float BottomOffset;
        public float LeftOffset;
        public float RightOffset;
        [TagField(Flags = Padding, Length = 32)]
        public byte[] Padding6;
        public CachedTag ArrowBitmap;
        public List<HudWaypointArrow> WaypointArrows;
        [TagField(Flags = Padding, Length = 80)]
        public byte[] Padding7;
        /// <summary>
        /// Multiplayer parameters
        /// </summary>
        public float HudScaleInMultiplayer;
        [TagField(Flags = Padding, Length = 256)]
        public byte[] Padding8;
        /// <summary>
        /// Hud globals
        /// </summary>
        [TagField(Flags = Padding, Length = 16)]
        public byte[] Padding9;
        public float MotionSensorRange;
        public float MotionSensorVelocitySensitivity; // how fast something moves to show up on the motion sensor
        public float MotionSensorScaleDonTTouchEver;
        public Rectangle2d DefaultChapterTitleBounds;
        [TagField(Flags = Padding, Length = 44)]
        public byte[] Padding10;
        /// <summary>
        /// Hud damage indicators
        /// </summary>
        public short TopOffset9;
        public short BottomOffset10;
        public short LeftOffset11;
        public short RightOffset12;
        [TagField(Flags = Padding, Length = 32)]
        public byte[] Padding11;
        public CachedTag IndicatorBitmap;
        public short SequenceIndex;
        public short MultiplayerSequenceIndex;
        public ArgbColor Color;
        [TagField(Flags = Padding, Length = 16)]
        public byte[] Padding12;
        /// <summary>
        /// Hud timer definitions
        /// </summary>
        /// <summary>
        /// Not much time left flash color
        /// </summary>
        public ArgbColor DefaultColor13;
        public ArgbColor FlashingColor14;
        public float FlashPeriod15;
        public float FlashDelay16; // time between flashes
        public short NumberOfFlashes17;
        public FlashFlagsValue FlashFlags18;
        public float FlashLength19; // time of each flash
        public ArgbColor DisabledColor20;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding13;
        /// <summary>
        /// Time out flash color
        /// </summary>
        public ArgbColor DefaultColor21;
        public ArgbColor FlashingColor22;
        public float FlashPeriod23;
        public float FlashDelay24; // time between flashes
        public short NumberOfFlashes25;
        public FlashFlagsValue FlashFlags26;
        public float FlashLength27; // time of each flash
        public ArgbColor DisabledColor28;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding14;
        [TagField(Flags = Padding, Length = 40)]
        public byte[] Padding15;
        public CachedTag CarnageReportBitmap;
        /// <summary>
        /// Hud crap that wouldn't fit anywhere else
        /// </summary>
        public short LoadingBeginText;
        public short LoadingEndText;
        public short CheckpointBeginText;
        public short CheckpointEndText;
        public CachedTag CheckpointSound;
        [TagField(Flags = Padding, Length = 96)]
        public byte[] Padding16;
        public NewHudGlobals NewGlobals;
        
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
        public class IconHudElementDefinition : TagStructure
        {
            public short SequenceIndex; // sequence index into the global hud icon bitmap
            public short WidthOffset; // extra spacing beyond bitmap width for text alignment
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
        
        [TagStructure(Size = 0x68)]
        public class HudWaypointArrow : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding1;
            public ArgbColor Color;
            public float Opacity;
            public float Translucency;
            public short OnScreenSequenceIndex;
            public short OffScreenSequenceIndex;
            public short OccludedSequenceIndex;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding3;
            public FlagsValue Flags;
            [TagField(Flags = Padding, Length = 24)]
            public byte[] Padding4;
            
            [Flags]
            public enum FlagsValue : uint
            {
                DonTRotateWhenPointingOffscreen = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x104)]
        public class NewHudGlobals : TagStructure
        {
            public CachedTag HudText;
            public List<NewHudDashlightDefinition> Dashlights;
            public List<NewHudWaypointArrowDefinition> WaypointArrows;
            public List<NewHudWaypointDefinition> Waypoints;
            public List<NewHudSoundElementDefinition> HudSounds;
            public List<PlayerTrainingEntryData> PlayerTrainingData;
            public NewHudGlobalsConstants Constants;
            
            [TagStructure(Size = 0x34)]
            public class NewHudDashlightDefinition : TagStructure
            {
                public CachedTag Bitmap;
                public CachedTag Shader;
                public short SequenceIndex;
                public FlagsValue Flags;
                public CachedTag Sound;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    DonTScaleWhenPulsing = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x3C)]
            public class NewHudWaypointArrowDefinition : TagStructure
            {
                public CachedTag Bitmap;
                public CachedTag Shader;
                public short SequenceIndex;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public float SmallestSize;
                public float SmallestDistance;
                public CachedTag BorderBitmap;
            }
            
            [TagStructure(Size = 0x28)]
            public class NewHudWaypointDefinition : TagStructure
            {
                public CachedTag Bitmap;
                public CachedTag Shader;
                public short OnscreenSequenceIndex;
                public short OccludedSequenceIndex;
                public short OffscreenSequenceIndex;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
            }
            
            [TagStructure(Size = 0x28)]
            public class NewHudSoundElementDefinition : TagStructure
            {
                public CachedTag ChiefSound;
                public LatchedToValue LatchedTo;
                public float Scale;
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
            public class PlayerTrainingEntryData : TagStructure
            {
                public StringId DisplayString; // comes out of the HUD text globals
                public StringId DisplayString2; // comes out of the HUD text globals, used for grouped prompt
                public StringId DisplayString3; // comes out of the HUD text globals, used for ungrouped prompt
                public short MaxDisplayTime; // how long the message can be on screen before being hidden
                public short DisplayCount; // how many times a training message will get displayed (0-3 only!)
                public short DissapearDelay; // how long a displayed but untriggered message stays up
                public short RedisplayDelay; // how long after display this message will stay hidden
                public float DisplayDelayS; // how long the event can be triggered before it's displayed
                public FlagsValue Flags;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    NotInMultiplayer = 1 << 0
                }
            }
            
            [TagStructure(Size = 0xB8)]
            public class NewHudGlobalsConstants : TagStructure
            {
                public CachedTag PrimaryMessageSound;
                public CachedTag SecondaryMessageSound;
                public StringId BootGrieferString;
                public StringId CannotBootGrieferString;
                public CachedTag TrainingShader;
                public CachedTag HumanTrainingTopRight;
                public CachedTag HumanTrainingTopCenter;
                public CachedTag HumanTrainingTopLeft;
                public CachedTag HumanTrainingMiddle;
                public CachedTag EliteTrainingTopRight;
                public CachedTag EliteTrainingTopCenter;
                public CachedTag EliteTrainingTopLeft;
                public CachedTag EliteTrainingMiddle;
            }
        }
    }
}

