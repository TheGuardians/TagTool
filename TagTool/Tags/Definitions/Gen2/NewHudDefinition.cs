using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "new_hud_definition", Tag = "nhdt", Size = 0x28)]
    public class NewHudDefinition : TagStructure
    {
        [TagField(ValidTags = new [] { "nhdt" })]
        public CachedTag DoNotUse;
        public List<HudBitmapWidgets> BitmapWidgets;
        public List<HudTextWidgets> TextWidgets;
        public NewHudDashlightDataStructBlock DashlightData;
        public List<HudScreenEffectWidgets> ScreenEffectWidgets;
        
        [TagStructure(Size = 0x64)]
        public class HudBitmapWidgets : TagStructure
        {
            public StringId Name;
            public HudWidgetInputsStructBlock Unknown;
            public HudWidgetStateDefinitionStructBlock WidgetState;
            public AnchorValue Anchor;
            public FlagsValue Flags;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Bitmap;
            [TagField(ValidTags = new [] { "shad" })]
            public CachedTag Shader;
            public sbyte FullscreenSequenceIndex;
            public sbyte HalfscreenSequenceIndex;
            public sbyte QuarterscreenSequenceIndex;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public Point2d FullscreenOffset;
            public Point2d HalfscreenOffset;
            public Point2d QuarterscreenOffset;
            public RealPoint2d FullscreenRegistrationPoint;
            public RealPoint2d HalfscreenRegistrationPoint;
            public RealPoint2d QuarterscreenRegistrationPoint;
            public List<HudWidgetEffectBlock> Effect;
            public SpecialHudTypeValue SpecialHudType;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            
            [TagStructure(Size = 0x4)]
            public class HudWidgetInputsStructBlock : TagStructure
            {
                public Input1Value Input1;
                public Input2Value Input2;
                public Input3Value Input3;
                public Input4Value Input4;
                
                public enum Input1Value : sbyte
                {
                    BasicZero,
                    BasicOne,
                    BasicTime,
                    BasicGlobalHudFade,
                    Unknown,
                    Unknown1,
                    Unknown2,
                    Unknown3,
                    Unknown4,
                    Unknown5,
                    Unknown6,
                    Unknown7,
                    Unknown8,
                    Unknown9,
                    Unknown10,
                    Unknown11,
                    UnitShield,
                    UnitBody,
                    UnitAutoaimed,
                    UnitHasNoGrenades,
                    UnitFragGrenCnt,
                    UnitPlasmaGrenCnt,
                    UnitTimeOnDplShld,
                    UnitZoomFraction,
                    UnitCamoValue,
                    Unknown12,
                    Unknown13,
                    Unknown14,
                    Unknown15,
                    Unknown16,
                    Unknown17,
                    Unknown18,
                    ParentShield,
                    ParentBody,
                    Unknown19,
                    Unknown20,
                    Unknown21,
                    Unknown22,
                    Unknown23,
                    Unknown24,
                    Unknown25,
                    Unknown26,
                    Unknown27,
                    Unknown28,
                    Unknown29,
                    Unknown30,
                    Unknown31,
                    Unknown32,
                    WeaponClipAmmo,
                    WeaponHeat,
                    WeaponBattery,
                    WeaponTotalAmmo,
                    WeaponBarrelSpin,
                    WeaponOverheated,
                    WeaponClipAmmoFraction,
                    WeaponTimeOnOverheat,
                    WeaponBatteryFraction,
                    WeaponLockingFraction,
                    Unknown33,
                    Unknown34,
                    Unknown35,
                    Unknown36,
                    Unknown37,
                    Unknown38,
                    Unknown39,
                    UserScoreFraction,
                    OtherUserScoreFraction,
                    UserWinning,
                    BombArmingAmount,
                    Unknown40,
                    Unknown41,
                    Unknown42,
                    Unknown43,
                    Unknown44,
                    Unknown45,
                    Unknown46,
                    Unknown47,
                    Unknown48,
                    Unknown49,
                    Unknown50,
                    Unknown51
                }
                
                public enum Input2Value : sbyte
                {
                    BasicZero,
                    BasicOne,
                    BasicTime,
                    BasicGlobalHudFade,
                    Unknown,
                    Unknown1,
                    Unknown2,
                    Unknown3,
                    Unknown4,
                    Unknown5,
                    Unknown6,
                    Unknown7,
                    Unknown8,
                    Unknown9,
                    Unknown10,
                    Unknown11,
                    UnitShield,
                    UnitBody,
                    UnitAutoaimed,
                    UnitHasNoGrenades,
                    UnitFragGrenCnt,
                    UnitPlasmaGrenCnt,
                    UnitTimeOnDplShld,
                    UnitZoomFraction,
                    UnitCamoValue,
                    Unknown12,
                    Unknown13,
                    Unknown14,
                    Unknown15,
                    Unknown16,
                    Unknown17,
                    Unknown18,
                    ParentShield,
                    ParentBody,
                    Unknown19,
                    Unknown20,
                    Unknown21,
                    Unknown22,
                    Unknown23,
                    Unknown24,
                    Unknown25,
                    Unknown26,
                    Unknown27,
                    Unknown28,
                    Unknown29,
                    Unknown30,
                    Unknown31,
                    Unknown32,
                    WeaponClipAmmo,
                    WeaponHeat,
                    WeaponBattery,
                    WeaponTotalAmmo,
                    WeaponBarrelSpin,
                    WeaponOverheated,
                    WeaponClipAmmoFraction,
                    WeaponTimeOnOverheat,
                    WeaponBatteryFraction,
                    WeaponLockingFraction,
                    Unknown33,
                    Unknown34,
                    Unknown35,
                    Unknown36,
                    Unknown37,
                    Unknown38,
                    Unknown39,
                    UserScoreFraction,
                    OtherUserScoreFraction,
                    UserWinning,
                    BombArmingAmount,
                    Unknown40,
                    Unknown41,
                    Unknown42,
                    Unknown43,
                    Unknown44,
                    Unknown45,
                    Unknown46,
                    Unknown47,
                    Unknown48,
                    Unknown49,
                    Unknown50,
                    Unknown51
                }
                
                public enum Input3Value : sbyte
                {
                    BasicZero,
                    BasicOne,
                    BasicTime,
                    BasicGlobalHudFade,
                    Unknown,
                    Unknown1,
                    Unknown2,
                    Unknown3,
                    Unknown4,
                    Unknown5,
                    Unknown6,
                    Unknown7,
                    Unknown8,
                    Unknown9,
                    Unknown10,
                    Unknown11,
                    UnitShield,
                    UnitBody,
                    UnitAutoaimed,
                    UnitHasNoGrenades,
                    UnitFragGrenCnt,
                    UnitPlasmaGrenCnt,
                    UnitTimeOnDplShld,
                    UnitZoomFraction,
                    UnitCamoValue,
                    Unknown12,
                    Unknown13,
                    Unknown14,
                    Unknown15,
                    Unknown16,
                    Unknown17,
                    Unknown18,
                    ParentShield,
                    ParentBody,
                    Unknown19,
                    Unknown20,
                    Unknown21,
                    Unknown22,
                    Unknown23,
                    Unknown24,
                    Unknown25,
                    Unknown26,
                    Unknown27,
                    Unknown28,
                    Unknown29,
                    Unknown30,
                    Unknown31,
                    Unknown32,
                    WeaponClipAmmo,
                    WeaponHeat,
                    WeaponBattery,
                    WeaponTotalAmmo,
                    WeaponBarrelSpin,
                    WeaponOverheated,
                    WeaponClipAmmoFraction,
                    WeaponTimeOnOverheat,
                    WeaponBatteryFraction,
                    WeaponLockingFraction,
                    Unknown33,
                    Unknown34,
                    Unknown35,
                    Unknown36,
                    Unknown37,
                    Unknown38,
                    Unknown39,
                    UserScoreFraction,
                    OtherUserScoreFraction,
                    UserWinning,
                    BombArmingAmount,
                    Unknown40,
                    Unknown41,
                    Unknown42,
                    Unknown43,
                    Unknown44,
                    Unknown45,
                    Unknown46,
                    Unknown47,
                    Unknown48,
                    Unknown49,
                    Unknown50,
                    Unknown51
                }
                
                public enum Input4Value : sbyte
                {
                    BasicZero,
                    BasicOne,
                    BasicTime,
                    BasicGlobalHudFade,
                    Unknown,
                    Unknown1,
                    Unknown2,
                    Unknown3,
                    Unknown4,
                    Unknown5,
                    Unknown6,
                    Unknown7,
                    Unknown8,
                    Unknown9,
                    Unknown10,
                    Unknown11,
                    UnitShield,
                    UnitBody,
                    UnitAutoaimed,
                    UnitHasNoGrenades,
                    UnitFragGrenCnt,
                    UnitPlasmaGrenCnt,
                    UnitTimeOnDplShld,
                    UnitZoomFraction,
                    UnitCamoValue,
                    Unknown12,
                    Unknown13,
                    Unknown14,
                    Unknown15,
                    Unknown16,
                    Unknown17,
                    Unknown18,
                    ParentShield,
                    ParentBody,
                    Unknown19,
                    Unknown20,
                    Unknown21,
                    Unknown22,
                    Unknown23,
                    Unknown24,
                    Unknown25,
                    Unknown26,
                    Unknown27,
                    Unknown28,
                    Unknown29,
                    Unknown30,
                    Unknown31,
                    Unknown32,
                    WeaponClipAmmo,
                    WeaponHeat,
                    WeaponBattery,
                    WeaponTotalAmmo,
                    WeaponBarrelSpin,
                    WeaponOverheated,
                    WeaponClipAmmoFraction,
                    WeaponTimeOnOverheat,
                    WeaponBatteryFraction,
                    WeaponLockingFraction,
                    Unknown33,
                    Unknown34,
                    Unknown35,
                    Unknown36,
                    Unknown37,
                    Unknown38,
                    Unknown39,
                    UserScoreFraction,
                    OtherUserScoreFraction,
                    UserWinning,
                    BombArmingAmount,
                    Unknown40,
                    Unknown41,
                    Unknown42,
                    Unknown43,
                    Unknown44,
                    Unknown45,
                    Unknown46,
                    Unknown47,
                    Unknown48,
                    Unknown49,
                    Unknown50,
                    Unknown51
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class HudWidgetStateDefinitionStructBlock : TagStructure
            {
                /// <summary>
                /// this section is split up into YES and NO flags.
                /// a widget will draw if any of it's YES flags are true,
                /// but it will NOT
                /// draw if any of it's NO flags are true.
                /// 
                /// </summary>
                public YUnitFlagsValue YUnitFlags;
                public YExtraFlagsValue YExtraFlags;
                public YWeaponFlagsValue YWeaponFlags;
                public YGameEngineStateFlagsValue YGameEngineStateFlags;
                public NUnitFlagsValue NUnitFlags;
                public NExtraFlagsValue NExtraFlags;
                public NWeaponFlagsValue NWeaponFlags;
                public NGameEngineStateFlagsValue NGameEngineStateFlags;
                public sbyte AgeCutoff;
                public sbyte ClipCutoff;
                public sbyte TotalCutoff;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum YUnitFlagsValue : ushort
                {
                    Default = 1 << 0,
                    GrenadeTypeIsNone = 1 << 1,
                    GrenadeTypeIsFrag = 1 << 2,
                    GrenadeTypeIsPlasma = 1 << 3,
                    UnitIsSingleWielding = 1 << 4,
                    UnitIsDualWielding = 1 << 5,
                    UnitIsUnzoomed = 1 << 6,
                    UnitIsZoomedLevel1 = 1 << 7,
                    UnitIsZoomedLevel2 = 1 << 8,
                    GrenadesDisabled = 1 << 9,
                    BinocularsEnabled = 1 << 10,
                    MotionSensorEnabled = 1 << 11,
                    ShieldEnabled = 1 << 12,
                    Dervish = 1 << 13
                }
                
                [Flags]
                public enum YExtraFlagsValue : ushort
                {
                    AutoaimFriendly = 1 << 0,
                    AutoaimPlasma = 1 << 1,
                    AutoaimHeadshot = 1 << 2,
                    AutoaimVulnerable = 1 << 3,
                    AutoaimInvincible = 1 << 4
                }
                
                [Flags]
                public enum YWeaponFlagsValue : ushort
                {
                    PrimaryWeapon = 1 << 0,
                    SecondaryWeapon = 1 << 1,
                    BackpackWeapon = 1 << 2,
                    AgeBelowCutoff = 1 << 3,
                    ClipBelowCutoff = 1 << 4,
                    TotalBelowCutoff = 1 << 5,
                    Overheated = 1 << 6,
                    OutOfAmmo = 1 << 7,
                    LockTargetAvailable = 1 << 8,
                    Locking = 1 << 9,
                    Locked = 1 << 10
                }
                
                [Flags]
                public enum YGameEngineStateFlagsValue : ushort
                {
                    CampaignSolo = 1 << 0,
                    CampaignCoop = 1 << 1,
                    FreeForAll = 1 << 2,
                    TeamGame = 1 << 3,
                    UserLeading = 1 << 4,
                    UserNotLeading = 1 << 5,
                    TimedGame = 1 << 6,
                    UntimedGame = 1 << 7,
                    OtherScoreValid = 1 << 8,
                    OtherScoreInvalid = 1 << 9,
                    PlayerIsArmingBomb = 1 << 10,
                    PlayerTalking = 1 << 11
                }
                
                [Flags]
                public enum NUnitFlagsValue : ushort
                {
                    Default = 1 << 0,
                    GrenadeTypeIsNone = 1 << 1,
                    GrenadeTypeIsFrag = 1 << 2,
                    GrenadeTypeIsPlasma = 1 << 3,
                    UnitIsSingleWielding = 1 << 4,
                    UnitIsDualWielding = 1 << 5,
                    UnitIsUnzoomed = 1 << 6,
                    UnitIsZoomedLevel1 = 1 << 7,
                    UnitIsZoomedLevel2 = 1 << 8,
                    GrenadesDisabled = 1 << 9,
                    BinocularsEnabled = 1 << 10,
                    MotionSensorEnabled = 1 << 11,
                    ShieldEnabled = 1 << 12,
                    Dervish = 1 << 13
                }
                
                [Flags]
                public enum NExtraFlagsValue : ushort
                {
                    AutoaimFriendly = 1 << 0,
                    AutoaimPlasma = 1 << 1,
                    AutoaimHeadshot = 1 << 2,
                    AutoaimVulnerable = 1 << 3,
                    AutoaimInvincible = 1 << 4
                }
                
                [Flags]
                public enum NWeaponFlagsValue : ushort
                {
                    PrimaryWeapon = 1 << 0,
                    SecondaryWeapon = 1 << 1,
                    BackpackWeapon = 1 << 2,
                    AgeBelowCutoff = 1 << 3,
                    ClipBelowCutoff = 1 << 4,
                    TotalBelowCutoff = 1 << 5,
                    Overheated = 1 << 6,
                    OutOfAmmo = 1 << 7,
                    LockTargetAvailable = 1 << 8,
                    Locking = 1 << 9,
                    Locked = 1 << 10
                }
                
                [Flags]
                public enum NGameEngineStateFlagsValue : ushort
                {
                    CampaignSolo = 1 << 0,
                    CampaignCoop = 1 << 1,
                    FreeForAll = 1 << 2,
                    TeamGame = 1 << 3,
                    UserLeading = 1 << 4,
                    UserNotLeading = 1 << 5,
                    TimedGame = 1 << 6,
                    UntimedGame = 1 << 7,
                    OtherScoreValid = 1 << 8,
                    OtherScoreInvalid = 1 << 9,
                    PlayerIsArmingBomb = 1 << 10,
                    PlayerTalking = 1 << 11
                }
            }
            
            public enum AnchorValue : short
            {
                HealthAndShield,
                WeaponHud,
                MotionSensor,
                Scoreboard,
                Crosshair,
                LockOnTarget
            }
            
            [Flags]
            public enum FlagsValue : ushort
            {
                FlipHorizontally = 1 << 0,
                FlipVertically = 1 << 1,
                ScopeMirrorHorizontally = 1 << 2,
                ScopeMirrorVertically = 1 << 3,
                ScopeStretch = 1 << 4
            }
            
            [TagStructure(Size = 0x68)]
            public class HudWidgetEffectBlock : TagStructure
            {
                /// <summary>
                /// allow the scaling, rotation, and offsetting of widgets
                /// </summary>
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public HudWidgetEffectFunctionStructBlock YourMom;
                public HudWidgetEffectFunctionStructBlock1 YourMom1;
                public HudWidgetEffectFunctionStructBlock2 YourMom2;
                public HudWidgetEffectFunctionStructBlock3 YourMom3;
                public HudWidgetEffectFunctionStructBlock4 YourMom4;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    ApplyScale = 1 << 0,
                    ApplyTheta = 1 << 1,
                    ApplyOffset = 1 << 2
                }
                
                [TagStructure(Size = 0x14)]
                public class HudWidgetEffectFunctionStructBlock : TagStructure
                {
                    public StringId InputName;
                    public StringId RangeName;
                    public float TimePeriodInSeconds;
                    public ScalarFunctionStructBlock Function;
                    
                    [TagStructure(Size = 0x8)]
                    public class ScalarFunctionStructBlock : TagStructure
                    {
                        public MappingFunctionBlock Function;
                        
                        [TagStructure(Size = 0x8)]
                        public class MappingFunctionBlock : TagStructure
                        {
                            public List<ByteBlock> Data;
                            
                            [TagStructure(Size = 0x1)]
                            public class ByteBlock : TagStructure
                            {
                                public sbyte Value;
                            }
                        }
                    }
                }
                
                [TagStructure(Size = 0x14)]
                public class HudWidgetEffectFunctionStructBlock1 : TagStructure
                {
                    public StringId InputName;
                    public StringId RangeName;
                    public float TimePeriodInSeconds;
                    public ScalarFunctionStructBlock Function;
                    
                    [TagStructure(Size = 0x8)]
                    public class ScalarFunctionStructBlock : TagStructure
                    {
                        public MappingFunctionBlock Function;
                        
                        [TagStructure(Size = 0x8)]
                        public class MappingFunctionBlock : TagStructure
                        {
                            public List<ByteBlock> Data;
                            
                            [TagStructure(Size = 0x1)]
                            public class ByteBlock : TagStructure
                            {
                                public sbyte Value;
                            }
                        }
                    }
                }
                
                [TagStructure(Size = 0x14)]
                public class HudWidgetEffectFunctionStructBlock2 : TagStructure
                {
                    public StringId InputName;
                    public StringId RangeName;
                    public float TimePeriodInSeconds;
                    public ScalarFunctionStructBlock Function;
                    
                    [TagStructure(Size = 0x8)]
                    public class ScalarFunctionStructBlock : TagStructure
                    {
                        public MappingFunctionBlock Function;
                        
                        [TagStructure(Size = 0x8)]
                        public class MappingFunctionBlock : TagStructure
                        {
                            public List<ByteBlock> Data;
                            
                            [TagStructure(Size = 0x1)]
                            public class ByteBlock : TagStructure
                            {
                                public sbyte Value;
                            }
                        }
                    }
                }
                
                [TagStructure(Size = 0x14)]
                public class HudWidgetEffectFunctionStructBlock3 : TagStructure
                {
                    public StringId InputName;
                    public StringId RangeName;
                    public float TimePeriodInSeconds;
                    public ScalarFunctionStructBlock Function;
                    
                    [TagStructure(Size = 0x8)]
                    public class ScalarFunctionStructBlock : TagStructure
                    {
                        public MappingFunctionBlock Function;
                        
                        [TagStructure(Size = 0x8)]
                        public class MappingFunctionBlock : TagStructure
                        {
                            public List<ByteBlock> Data;
                            
                            [TagStructure(Size = 0x1)]
                            public class ByteBlock : TagStructure
                            {
                                public sbyte Value;
                            }
                        }
                    }
                }
                
                [TagStructure(Size = 0x14)]
                public class HudWidgetEffectFunctionStructBlock4 : TagStructure
                {
                    public StringId InputName;
                    public StringId RangeName;
                    public float TimePeriodInSeconds;
                    public ScalarFunctionStructBlock Function;
                    
                    [TagStructure(Size = 0x8)]
                    public class ScalarFunctionStructBlock : TagStructure
                    {
                        public MappingFunctionBlock Function;
                        
                        [TagStructure(Size = 0x8)]
                        public class MappingFunctionBlock : TagStructure
                        {
                            public List<ByteBlock> Data;
                            
                            [TagStructure(Size = 0x1)]
                            public class ByteBlock : TagStructure
                            {
                                public sbyte Value;
                            }
                        }
                    }
                }
            }
            
            public enum SpecialHudTypeValue : short
            {
                Unspecial,
                SBPlayerEmblem,
                SBOtherPlayerEmblem,
                SBPlayerScoreMeter,
                SBOtherPlayerScoreMeter,
                UnitShieldMeter,
                MotionSensor,
                TerritoryMeter
            }
        }
        
        [TagStructure(Size = 0x54)]
        public class HudTextWidgets : TagStructure
        {
            public StringId Name;
            public HudWidgetInputsStructBlock Unknown;
            public HudWidgetStateDefinitionStructBlock WidgetState;
            public AnchorValue Anchor;
            /// <summary>
            /// string is a number: treats the inputted string id as a function name, not a string name
            /// 
            /// force 2-digit number: when used
            /// in combination with above, forces output to be a 2-digit numberwith leading zeros if necessary
            /// 
            /// force 3-digit number:
            /// same as above, but with 3 digits instead of 2
            /// 
            /// 
            /// </summary>
            public FlagsValue Flags;
            [TagField(ValidTags = new [] { "shad" })]
            public CachedTag Shader;
            public StringId String;
            public JustificationValue Justification;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public FullscreenFontIndexValue FullscreenFontIndex;
            public HalfscreenFontIndexValue HalfscreenFontIndex;
            public QuarterscreenFontIndexValue QuarterscreenFontIndex;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public float FullscreenScale;
            public float HalfscreenScale;
            public float QuarterscreenScale;
            public Point2d FullscreenOffset;
            public Point2d HalfscreenOffset;
            public Point2d QuarterscreenOffset;
            public List<HudWidgetEffectBlock> Effect;
            
            [TagStructure(Size = 0x4)]
            public class HudWidgetInputsStructBlock : TagStructure
            {
                public Input1Value Input1;
                public Input2Value Input2;
                public Input3Value Input3;
                public Input4Value Input4;
                
                public enum Input1Value : sbyte
                {
                    BasicZero,
                    BasicOne,
                    BasicTime,
                    BasicGlobalHudFade,
                    Unknown,
                    Unknown1,
                    Unknown2,
                    Unknown3,
                    Unknown4,
                    Unknown5,
                    Unknown6,
                    Unknown7,
                    Unknown8,
                    Unknown9,
                    Unknown10,
                    Unknown11,
                    UnitShield,
                    UnitBody,
                    UnitAutoaimed,
                    UnitHasNoGrenades,
                    UnitFragGrenCnt,
                    UnitPlasmaGrenCnt,
                    UnitTimeOnDplShld,
                    UnitZoomFraction,
                    UnitCamoValue,
                    Unknown12,
                    Unknown13,
                    Unknown14,
                    Unknown15,
                    Unknown16,
                    Unknown17,
                    Unknown18,
                    ParentShield,
                    ParentBody,
                    Unknown19,
                    Unknown20,
                    Unknown21,
                    Unknown22,
                    Unknown23,
                    Unknown24,
                    Unknown25,
                    Unknown26,
                    Unknown27,
                    Unknown28,
                    Unknown29,
                    Unknown30,
                    Unknown31,
                    Unknown32,
                    WeaponClipAmmo,
                    WeaponHeat,
                    WeaponBattery,
                    WeaponTotalAmmo,
                    WeaponBarrelSpin,
                    WeaponOverheated,
                    WeaponClipAmmoFraction,
                    WeaponTimeOnOverheat,
                    WeaponBatteryFraction,
                    WeaponLockingFraction,
                    Unknown33,
                    Unknown34,
                    Unknown35,
                    Unknown36,
                    Unknown37,
                    Unknown38,
                    Unknown39,
                    UserScoreFraction,
                    OtherUserScoreFraction,
                    UserWinning,
                    BombArmingAmount,
                    Unknown40,
                    Unknown41,
                    Unknown42,
                    Unknown43,
                    Unknown44,
                    Unknown45,
                    Unknown46,
                    Unknown47,
                    Unknown48,
                    Unknown49,
                    Unknown50,
                    Unknown51
                }
                
                public enum Input2Value : sbyte
                {
                    BasicZero,
                    BasicOne,
                    BasicTime,
                    BasicGlobalHudFade,
                    Unknown,
                    Unknown1,
                    Unknown2,
                    Unknown3,
                    Unknown4,
                    Unknown5,
                    Unknown6,
                    Unknown7,
                    Unknown8,
                    Unknown9,
                    Unknown10,
                    Unknown11,
                    UnitShield,
                    UnitBody,
                    UnitAutoaimed,
                    UnitHasNoGrenades,
                    UnitFragGrenCnt,
                    UnitPlasmaGrenCnt,
                    UnitTimeOnDplShld,
                    UnitZoomFraction,
                    UnitCamoValue,
                    Unknown12,
                    Unknown13,
                    Unknown14,
                    Unknown15,
                    Unknown16,
                    Unknown17,
                    Unknown18,
                    ParentShield,
                    ParentBody,
                    Unknown19,
                    Unknown20,
                    Unknown21,
                    Unknown22,
                    Unknown23,
                    Unknown24,
                    Unknown25,
                    Unknown26,
                    Unknown27,
                    Unknown28,
                    Unknown29,
                    Unknown30,
                    Unknown31,
                    Unknown32,
                    WeaponClipAmmo,
                    WeaponHeat,
                    WeaponBattery,
                    WeaponTotalAmmo,
                    WeaponBarrelSpin,
                    WeaponOverheated,
                    WeaponClipAmmoFraction,
                    WeaponTimeOnOverheat,
                    WeaponBatteryFraction,
                    WeaponLockingFraction,
                    Unknown33,
                    Unknown34,
                    Unknown35,
                    Unknown36,
                    Unknown37,
                    Unknown38,
                    Unknown39,
                    UserScoreFraction,
                    OtherUserScoreFraction,
                    UserWinning,
                    BombArmingAmount,
                    Unknown40,
                    Unknown41,
                    Unknown42,
                    Unknown43,
                    Unknown44,
                    Unknown45,
                    Unknown46,
                    Unknown47,
                    Unknown48,
                    Unknown49,
                    Unknown50,
                    Unknown51
                }
                
                public enum Input3Value : sbyte
                {
                    BasicZero,
                    BasicOne,
                    BasicTime,
                    BasicGlobalHudFade,
                    Unknown,
                    Unknown1,
                    Unknown2,
                    Unknown3,
                    Unknown4,
                    Unknown5,
                    Unknown6,
                    Unknown7,
                    Unknown8,
                    Unknown9,
                    Unknown10,
                    Unknown11,
                    UnitShield,
                    UnitBody,
                    UnitAutoaimed,
                    UnitHasNoGrenades,
                    UnitFragGrenCnt,
                    UnitPlasmaGrenCnt,
                    UnitTimeOnDplShld,
                    UnitZoomFraction,
                    UnitCamoValue,
                    Unknown12,
                    Unknown13,
                    Unknown14,
                    Unknown15,
                    Unknown16,
                    Unknown17,
                    Unknown18,
                    ParentShield,
                    ParentBody,
                    Unknown19,
                    Unknown20,
                    Unknown21,
                    Unknown22,
                    Unknown23,
                    Unknown24,
                    Unknown25,
                    Unknown26,
                    Unknown27,
                    Unknown28,
                    Unknown29,
                    Unknown30,
                    Unknown31,
                    Unknown32,
                    WeaponClipAmmo,
                    WeaponHeat,
                    WeaponBattery,
                    WeaponTotalAmmo,
                    WeaponBarrelSpin,
                    WeaponOverheated,
                    WeaponClipAmmoFraction,
                    WeaponTimeOnOverheat,
                    WeaponBatteryFraction,
                    WeaponLockingFraction,
                    Unknown33,
                    Unknown34,
                    Unknown35,
                    Unknown36,
                    Unknown37,
                    Unknown38,
                    Unknown39,
                    UserScoreFraction,
                    OtherUserScoreFraction,
                    UserWinning,
                    BombArmingAmount,
                    Unknown40,
                    Unknown41,
                    Unknown42,
                    Unknown43,
                    Unknown44,
                    Unknown45,
                    Unknown46,
                    Unknown47,
                    Unknown48,
                    Unknown49,
                    Unknown50,
                    Unknown51
                }
                
                public enum Input4Value : sbyte
                {
                    BasicZero,
                    BasicOne,
                    BasicTime,
                    BasicGlobalHudFade,
                    Unknown,
                    Unknown1,
                    Unknown2,
                    Unknown3,
                    Unknown4,
                    Unknown5,
                    Unknown6,
                    Unknown7,
                    Unknown8,
                    Unknown9,
                    Unknown10,
                    Unknown11,
                    UnitShield,
                    UnitBody,
                    UnitAutoaimed,
                    UnitHasNoGrenades,
                    UnitFragGrenCnt,
                    UnitPlasmaGrenCnt,
                    UnitTimeOnDplShld,
                    UnitZoomFraction,
                    UnitCamoValue,
                    Unknown12,
                    Unknown13,
                    Unknown14,
                    Unknown15,
                    Unknown16,
                    Unknown17,
                    Unknown18,
                    ParentShield,
                    ParentBody,
                    Unknown19,
                    Unknown20,
                    Unknown21,
                    Unknown22,
                    Unknown23,
                    Unknown24,
                    Unknown25,
                    Unknown26,
                    Unknown27,
                    Unknown28,
                    Unknown29,
                    Unknown30,
                    Unknown31,
                    Unknown32,
                    WeaponClipAmmo,
                    WeaponHeat,
                    WeaponBattery,
                    WeaponTotalAmmo,
                    WeaponBarrelSpin,
                    WeaponOverheated,
                    WeaponClipAmmoFraction,
                    WeaponTimeOnOverheat,
                    WeaponBatteryFraction,
                    WeaponLockingFraction,
                    Unknown33,
                    Unknown34,
                    Unknown35,
                    Unknown36,
                    Unknown37,
                    Unknown38,
                    Unknown39,
                    UserScoreFraction,
                    OtherUserScoreFraction,
                    UserWinning,
                    BombArmingAmount,
                    Unknown40,
                    Unknown41,
                    Unknown42,
                    Unknown43,
                    Unknown44,
                    Unknown45,
                    Unknown46,
                    Unknown47,
                    Unknown48,
                    Unknown49,
                    Unknown50,
                    Unknown51
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class HudWidgetStateDefinitionStructBlock : TagStructure
            {
                /// <summary>
                /// this section is split up into YES and NO flags.
                /// a widget will draw if any of it's YES flags are true,
                /// but it will NOT
                /// draw if any of it's NO flags are true.
                /// 
                /// </summary>
                public YUnitFlagsValue YUnitFlags;
                public YExtraFlagsValue YExtraFlags;
                public YWeaponFlagsValue YWeaponFlags;
                public YGameEngineStateFlagsValue YGameEngineStateFlags;
                public NUnitFlagsValue NUnitFlags;
                public NExtraFlagsValue NExtraFlags;
                public NWeaponFlagsValue NWeaponFlags;
                public NGameEngineStateFlagsValue NGameEngineStateFlags;
                public sbyte AgeCutoff;
                public sbyte ClipCutoff;
                public sbyte TotalCutoff;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum YUnitFlagsValue : ushort
                {
                    Default = 1 << 0,
                    GrenadeTypeIsNone = 1 << 1,
                    GrenadeTypeIsFrag = 1 << 2,
                    GrenadeTypeIsPlasma = 1 << 3,
                    UnitIsSingleWielding = 1 << 4,
                    UnitIsDualWielding = 1 << 5,
                    UnitIsUnzoomed = 1 << 6,
                    UnitIsZoomedLevel1 = 1 << 7,
                    UnitIsZoomedLevel2 = 1 << 8,
                    GrenadesDisabled = 1 << 9,
                    BinocularsEnabled = 1 << 10,
                    MotionSensorEnabled = 1 << 11,
                    ShieldEnabled = 1 << 12,
                    Dervish = 1 << 13
                }
                
                [Flags]
                public enum YExtraFlagsValue : ushort
                {
                    AutoaimFriendly = 1 << 0,
                    AutoaimPlasma = 1 << 1,
                    AutoaimHeadshot = 1 << 2,
                    AutoaimVulnerable = 1 << 3,
                    AutoaimInvincible = 1 << 4
                }
                
                [Flags]
                public enum YWeaponFlagsValue : ushort
                {
                    PrimaryWeapon = 1 << 0,
                    SecondaryWeapon = 1 << 1,
                    BackpackWeapon = 1 << 2,
                    AgeBelowCutoff = 1 << 3,
                    ClipBelowCutoff = 1 << 4,
                    TotalBelowCutoff = 1 << 5,
                    Overheated = 1 << 6,
                    OutOfAmmo = 1 << 7,
                    LockTargetAvailable = 1 << 8,
                    Locking = 1 << 9,
                    Locked = 1 << 10
                }
                
                [Flags]
                public enum YGameEngineStateFlagsValue : ushort
                {
                    CampaignSolo = 1 << 0,
                    CampaignCoop = 1 << 1,
                    FreeForAll = 1 << 2,
                    TeamGame = 1 << 3,
                    UserLeading = 1 << 4,
                    UserNotLeading = 1 << 5,
                    TimedGame = 1 << 6,
                    UntimedGame = 1 << 7,
                    OtherScoreValid = 1 << 8,
                    OtherScoreInvalid = 1 << 9,
                    PlayerIsArmingBomb = 1 << 10,
                    PlayerTalking = 1 << 11
                }
                
                [Flags]
                public enum NUnitFlagsValue : ushort
                {
                    Default = 1 << 0,
                    GrenadeTypeIsNone = 1 << 1,
                    GrenadeTypeIsFrag = 1 << 2,
                    GrenadeTypeIsPlasma = 1 << 3,
                    UnitIsSingleWielding = 1 << 4,
                    UnitIsDualWielding = 1 << 5,
                    UnitIsUnzoomed = 1 << 6,
                    UnitIsZoomedLevel1 = 1 << 7,
                    UnitIsZoomedLevel2 = 1 << 8,
                    GrenadesDisabled = 1 << 9,
                    BinocularsEnabled = 1 << 10,
                    MotionSensorEnabled = 1 << 11,
                    ShieldEnabled = 1 << 12,
                    Dervish = 1 << 13
                }
                
                [Flags]
                public enum NExtraFlagsValue : ushort
                {
                    AutoaimFriendly = 1 << 0,
                    AutoaimPlasma = 1 << 1,
                    AutoaimHeadshot = 1 << 2,
                    AutoaimVulnerable = 1 << 3,
                    AutoaimInvincible = 1 << 4
                }
                
                [Flags]
                public enum NWeaponFlagsValue : ushort
                {
                    PrimaryWeapon = 1 << 0,
                    SecondaryWeapon = 1 << 1,
                    BackpackWeapon = 1 << 2,
                    AgeBelowCutoff = 1 << 3,
                    ClipBelowCutoff = 1 << 4,
                    TotalBelowCutoff = 1 << 5,
                    Overheated = 1 << 6,
                    OutOfAmmo = 1 << 7,
                    LockTargetAvailable = 1 << 8,
                    Locking = 1 << 9,
                    Locked = 1 << 10
                }
                
                [Flags]
                public enum NGameEngineStateFlagsValue : ushort
                {
                    CampaignSolo = 1 << 0,
                    CampaignCoop = 1 << 1,
                    FreeForAll = 1 << 2,
                    TeamGame = 1 << 3,
                    UserLeading = 1 << 4,
                    UserNotLeading = 1 << 5,
                    TimedGame = 1 << 6,
                    UntimedGame = 1 << 7,
                    OtherScoreValid = 1 << 8,
                    OtherScoreInvalid = 1 << 9,
                    PlayerIsArmingBomb = 1 << 10,
                    PlayerTalking = 1 << 11
                }
            }
            
            public enum AnchorValue : short
            {
                HealthAndShield,
                WeaponHud,
                MotionSensor,
                Scoreboard,
                Crosshair,
                LockOnTarget
            }
            
            [Flags]
            public enum FlagsValue : ushort
            {
                StringIsANumber = 1 << 0,
                Force2DigitNumber = 1 << 1,
                Force3DigitNumber = 1 << 2,
                TalkingPlayerHack = 1 << 3
            }
            
            public enum JustificationValue : short
            {
                Left,
                Center,
                Right
            }
            
            public enum FullscreenFontIndexValue : sbyte
            {
                Defualt,
                NumberFont
            }
            
            public enum HalfscreenFontIndexValue : sbyte
            {
                Defualt,
                NumberFont
            }
            
            public enum QuarterscreenFontIndexValue : sbyte
            {
                Defualt,
                NumberFont
            }
            
            [TagStructure(Size = 0x68)]
            public class HudWidgetEffectBlock : TagStructure
            {
                /// <summary>
                /// allow the scaling, rotation, and offsetting of widgets
                /// </summary>
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public HudWidgetEffectFunctionStructBlock YourMom;
                public HudWidgetEffectFunctionStructBlock1 YourMom1;
                public HudWidgetEffectFunctionStructBlock2 YourMom2;
                public HudWidgetEffectFunctionStructBlock3 YourMom3;
                public HudWidgetEffectFunctionStructBlock4 YourMom4;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    ApplyScale = 1 << 0,
                    ApplyTheta = 1 << 1,
                    ApplyOffset = 1 << 2
                }
                
                [TagStructure(Size = 0x14)]
                public class HudWidgetEffectFunctionStructBlock : TagStructure
                {
                    public StringId InputName;
                    public StringId RangeName;
                    public float TimePeriodInSeconds;
                    public ScalarFunctionStructBlock Function;
                    
                    [TagStructure(Size = 0x8)]
                    public class ScalarFunctionStructBlock : TagStructure
                    {
                        public MappingFunctionBlock Function;
                        
                        [TagStructure(Size = 0x8)]
                        public class MappingFunctionBlock : TagStructure
                        {
                            public List<ByteBlock> Data;
                            
                            [TagStructure(Size = 0x1)]
                            public class ByteBlock : TagStructure
                            {
                                public sbyte Value;
                            }
                        }
                    }
                }
                
                [TagStructure(Size = 0x14)]
                public class HudWidgetEffectFunctionStructBlock1 : TagStructure
                {
                    public StringId InputName;
                    public StringId RangeName;
                    public float TimePeriodInSeconds;
                    public ScalarFunctionStructBlock Function;
                    
                    [TagStructure(Size = 0x8)]
                    public class ScalarFunctionStructBlock : TagStructure
                    {
                        public MappingFunctionBlock Function;
                        
                        [TagStructure(Size = 0x8)]
                        public class MappingFunctionBlock : TagStructure
                        {
                            public List<ByteBlock> Data;
                            
                            [TagStructure(Size = 0x1)]
                            public class ByteBlock : TagStructure
                            {
                                public sbyte Value;
                            }
                        }
                    }
                }
                
                [TagStructure(Size = 0x14)]
                public class HudWidgetEffectFunctionStructBlock2 : TagStructure
                {
                    public StringId InputName;
                    public StringId RangeName;
                    public float TimePeriodInSeconds;
                    public ScalarFunctionStructBlock Function;
                    
                    [TagStructure(Size = 0x8)]
                    public class ScalarFunctionStructBlock : TagStructure
                    {
                        public MappingFunctionBlock Function;
                        
                        [TagStructure(Size = 0x8)]
                        public class MappingFunctionBlock : TagStructure
                        {
                            public List<ByteBlock> Data;
                            
                            [TagStructure(Size = 0x1)]
                            public class ByteBlock : TagStructure
                            {
                                public sbyte Value;
                            }
                        }
                    }
                }
                
                [TagStructure(Size = 0x14)]
                public class HudWidgetEffectFunctionStructBlock3 : TagStructure
                {
                    public StringId InputName;
                    public StringId RangeName;
                    public float TimePeriodInSeconds;
                    public ScalarFunctionStructBlock Function;
                    
                    [TagStructure(Size = 0x8)]
                    public class ScalarFunctionStructBlock : TagStructure
                    {
                        public MappingFunctionBlock Function;
                        
                        [TagStructure(Size = 0x8)]
                        public class MappingFunctionBlock : TagStructure
                        {
                            public List<ByteBlock> Data;
                            
                            [TagStructure(Size = 0x1)]
                            public class ByteBlock : TagStructure
                            {
                                public sbyte Value;
                            }
                        }
                    }
                }
                
                [TagStructure(Size = 0x14)]
                public class HudWidgetEffectFunctionStructBlock4 : TagStructure
                {
                    public StringId InputName;
                    public StringId RangeName;
                    public float TimePeriodInSeconds;
                    public ScalarFunctionStructBlock Function;
                    
                    [TagStructure(Size = 0x8)]
                    public class ScalarFunctionStructBlock : TagStructure
                    {
                        public MappingFunctionBlock Function;
                        
                        [TagStructure(Size = 0x8)]
                        public class MappingFunctionBlock : TagStructure
                        {
                            public List<ByteBlock> Data;
                            
                            [TagStructure(Size = 0x1)]
                            public class ByteBlock : TagStructure
                            {
                                public sbyte Value;
                            }
                        }
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class NewHudDashlightDataStructBlock : TagStructure
        {
            /// <summary>
            /// only relevant to new hud tags for weapons
            /// </summary>
            /// <summary>
            /// the cutoff for showing the reload dashlight
            /// </summary>
            public short LowClipCutoff;
            /// <summary>
            /// the cutoff for showing the low ammo dashlight
            /// </summary>
            public short LowAmmoCutoff;
            /// <summary>
            /// the age cutoff for showing the low battery dashlight
            /// </summary>
            public float AgeCutoff;
        }
        
        [TagStructure(Size = 0x50)]
        public class HudScreenEffectWidgets : TagStructure
        {
            public StringId Name;
            public HudWidgetInputsStructBlock Unknown;
            public HudWidgetStateDefinitionStructBlock Unknown1;
            public AnchorValue Anchor;
            public FlagsValue Flags;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Bitmap;
            [TagField(ValidTags = new [] { "egor" })]
            public CachedTag FullscreenScreenEffect;
            public ScreenEffectBonusStructBlock Waa;
            public sbyte FullscreenSequenceIndex;
            public sbyte HalfscreenSequenceIndex;
            public sbyte QuarterscreenSequenceIndex;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public Point2d FullscreenOffset;
            public Point2d HalfscreenOffset;
            public Point2d QuarterscreenOffset;
            
            [TagStructure(Size = 0x4)]
            public class HudWidgetInputsStructBlock : TagStructure
            {
                public Input1Value Input1;
                public Input2Value Input2;
                public Input3Value Input3;
                public Input4Value Input4;
                
                public enum Input1Value : sbyte
                {
                    BasicZero,
                    BasicOne,
                    BasicTime,
                    BasicGlobalHudFade,
                    Unknown,
                    Unknown1,
                    Unknown2,
                    Unknown3,
                    Unknown4,
                    Unknown5,
                    Unknown6,
                    Unknown7,
                    Unknown8,
                    Unknown9,
                    Unknown10,
                    Unknown11,
                    UnitShield,
                    UnitBody,
                    UnitAutoaimed,
                    UnitHasNoGrenades,
                    UnitFragGrenCnt,
                    UnitPlasmaGrenCnt,
                    UnitTimeOnDplShld,
                    UnitZoomFraction,
                    UnitCamoValue,
                    Unknown12,
                    Unknown13,
                    Unknown14,
                    Unknown15,
                    Unknown16,
                    Unknown17,
                    Unknown18,
                    ParentShield,
                    ParentBody,
                    Unknown19,
                    Unknown20,
                    Unknown21,
                    Unknown22,
                    Unknown23,
                    Unknown24,
                    Unknown25,
                    Unknown26,
                    Unknown27,
                    Unknown28,
                    Unknown29,
                    Unknown30,
                    Unknown31,
                    Unknown32,
                    WeaponClipAmmo,
                    WeaponHeat,
                    WeaponBattery,
                    WeaponTotalAmmo,
                    WeaponBarrelSpin,
                    WeaponOverheated,
                    WeaponClipAmmoFraction,
                    WeaponTimeOnOverheat,
                    WeaponBatteryFraction,
                    WeaponLockingFraction,
                    Unknown33,
                    Unknown34,
                    Unknown35,
                    Unknown36,
                    Unknown37,
                    Unknown38,
                    Unknown39,
                    UserScoreFraction,
                    OtherUserScoreFraction,
                    UserWinning,
                    BombArmingAmount,
                    Unknown40,
                    Unknown41,
                    Unknown42,
                    Unknown43,
                    Unknown44,
                    Unknown45,
                    Unknown46,
                    Unknown47,
                    Unknown48,
                    Unknown49,
                    Unknown50,
                    Unknown51
                }
                
                public enum Input2Value : sbyte
                {
                    BasicZero,
                    BasicOne,
                    BasicTime,
                    BasicGlobalHudFade,
                    Unknown,
                    Unknown1,
                    Unknown2,
                    Unknown3,
                    Unknown4,
                    Unknown5,
                    Unknown6,
                    Unknown7,
                    Unknown8,
                    Unknown9,
                    Unknown10,
                    Unknown11,
                    UnitShield,
                    UnitBody,
                    UnitAutoaimed,
                    UnitHasNoGrenades,
                    UnitFragGrenCnt,
                    UnitPlasmaGrenCnt,
                    UnitTimeOnDplShld,
                    UnitZoomFraction,
                    UnitCamoValue,
                    Unknown12,
                    Unknown13,
                    Unknown14,
                    Unknown15,
                    Unknown16,
                    Unknown17,
                    Unknown18,
                    ParentShield,
                    ParentBody,
                    Unknown19,
                    Unknown20,
                    Unknown21,
                    Unknown22,
                    Unknown23,
                    Unknown24,
                    Unknown25,
                    Unknown26,
                    Unknown27,
                    Unknown28,
                    Unknown29,
                    Unknown30,
                    Unknown31,
                    Unknown32,
                    WeaponClipAmmo,
                    WeaponHeat,
                    WeaponBattery,
                    WeaponTotalAmmo,
                    WeaponBarrelSpin,
                    WeaponOverheated,
                    WeaponClipAmmoFraction,
                    WeaponTimeOnOverheat,
                    WeaponBatteryFraction,
                    WeaponLockingFraction,
                    Unknown33,
                    Unknown34,
                    Unknown35,
                    Unknown36,
                    Unknown37,
                    Unknown38,
                    Unknown39,
                    UserScoreFraction,
                    OtherUserScoreFraction,
                    UserWinning,
                    BombArmingAmount,
                    Unknown40,
                    Unknown41,
                    Unknown42,
                    Unknown43,
                    Unknown44,
                    Unknown45,
                    Unknown46,
                    Unknown47,
                    Unknown48,
                    Unknown49,
                    Unknown50,
                    Unknown51
                }
                
                public enum Input3Value : sbyte
                {
                    BasicZero,
                    BasicOne,
                    BasicTime,
                    BasicGlobalHudFade,
                    Unknown,
                    Unknown1,
                    Unknown2,
                    Unknown3,
                    Unknown4,
                    Unknown5,
                    Unknown6,
                    Unknown7,
                    Unknown8,
                    Unknown9,
                    Unknown10,
                    Unknown11,
                    UnitShield,
                    UnitBody,
                    UnitAutoaimed,
                    UnitHasNoGrenades,
                    UnitFragGrenCnt,
                    UnitPlasmaGrenCnt,
                    UnitTimeOnDplShld,
                    UnitZoomFraction,
                    UnitCamoValue,
                    Unknown12,
                    Unknown13,
                    Unknown14,
                    Unknown15,
                    Unknown16,
                    Unknown17,
                    Unknown18,
                    ParentShield,
                    ParentBody,
                    Unknown19,
                    Unknown20,
                    Unknown21,
                    Unknown22,
                    Unknown23,
                    Unknown24,
                    Unknown25,
                    Unknown26,
                    Unknown27,
                    Unknown28,
                    Unknown29,
                    Unknown30,
                    Unknown31,
                    Unknown32,
                    WeaponClipAmmo,
                    WeaponHeat,
                    WeaponBattery,
                    WeaponTotalAmmo,
                    WeaponBarrelSpin,
                    WeaponOverheated,
                    WeaponClipAmmoFraction,
                    WeaponTimeOnOverheat,
                    WeaponBatteryFraction,
                    WeaponLockingFraction,
                    Unknown33,
                    Unknown34,
                    Unknown35,
                    Unknown36,
                    Unknown37,
                    Unknown38,
                    Unknown39,
                    UserScoreFraction,
                    OtherUserScoreFraction,
                    UserWinning,
                    BombArmingAmount,
                    Unknown40,
                    Unknown41,
                    Unknown42,
                    Unknown43,
                    Unknown44,
                    Unknown45,
                    Unknown46,
                    Unknown47,
                    Unknown48,
                    Unknown49,
                    Unknown50,
                    Unknown51
                }
                
                public enum Input4Value : sbyte
                {
                    BasicZero,
                    BasicOne,
                    BasicTime,
                    BasicGlobalHudFade,
                    Unknown,
                    Unknown1,
                    Unknown2,
                    Unknown3,
                    Unknown4,
                    Unknown5,
                    Unknown6,
                    Unknown7,
                    Unknown8,
                    Unknown9,
                    Unknown10,
                    Unknown11,
                    UnitShield,
                    UnitBody,
                    UnitAutoaimed,
                    UnitHasNoGrenades,
                    UnitFragGrenCnt,
                    UnitPlasmaGrenCnt,
                    UnitTimeOnDplShld,
                    UnitZoomFraction,
                    UnitCamoValue,
                    Unknown12,
                    Unknown13,
                    Unknown14,
                    Unknown15,
                    Unknown16,
                    Unknown17,
                    Unknown18,
                    ParentShield,
                    ParentBody,
                    Unknown19,
                    Unknown20,
                    Unknown21,
                    Unknown22,
                    Unknown23,
                    Unknown24,
                    Unknown25,
                    Unknown26,
                    Unknown27,
                    Unknown28,
                    Unknown29,
                    Unknown30,
                    Unknown31,
                    Unknown32,
                    WeaponClipAmmo,
                    WeaponHeat,
                    WeaponBattery,
                    WeaponTotalAmmo,
                    WeaponBarrelSpin,
                    WeaponOverheated,
                    WeaponClipAmmoFraction,
                    WeaponTimeOnOverheat,
                    WeaponBatteryFraction,
                    WeaponLockingFraction,
                    Unknown33,
                    Unknown34,
                    Unknown35,
                    Unknown36,
                    Unknown37,
                    Unknown38,
                    Unknown39,
                    UserScoreFraction,
                    OtherUserScoreFraction,
                    UserWinning,
                    BombArmingAmount,
                    Unknown40,
                    Unknown41,
                    Unknown42,
                    Unknown43,
                    Unknown44,
                    Unknown45,
                    Unknown46,
                    Unknown47,
                    Unknown48,
                    Unknown49,
                    Unknown50,
                    Unknown51
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class HudWidgetStateDefinitionStructBlock : TagStructure
            {
                /// <summary>
                /// this section is split up into YES and NO flags.
                /// a widget will draw if any of it's YES flags are true,
                /// but it will NOT
                /// draw if any of it's NO flags are true.
                /// 
                /// </summary>
                public YUnitFlagsValue YUnitFlags;
                public YExtraFlagsValue YExtraFlags;
                public YWeaponFlagsValue YWeaponFlags;
                public YGameEngineStateFlagsValue YGameEngineStateFlags;
                public NUnitFlagsValue NUnitFlags;
                public NExtraFlagsValue NExtraFlags;
                public NWeaponFlagsValue NWeaponFlags;
                public NGameEngineStateFlagsValue NGameEngineStateFlags;
                public sbyte AgeCutoff;
                public sbyte ClipCutoff;
                public sbyte TotalCutoff;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum YUnitFlagsValue : ushort
                {
                    Default = 1 << 0,
                    GrenadeTypeIsNone = 1 << 1,
                    GrenadeTypeIsFrag = 1 << 2,
                    GrenadeTypeIsPlasma = 1 << 3,
                    UnitIsSingleWielding = 1 << 4,
                    UnitIsDualWielding = 1 << 5,
                    UnitIsUnzoomed = 1 << 6,
                    UnitIsZoomedLevel1 = 1 << 7,
                    UnitIsZoomedLevel2 = 1 << 8,
                    GrenadesDisabled = 1 << 9,
                    BinocularsEnabled = 1 << 10,
                    MotionSensorEnabled = 1 << 11,
                    ShieldEnabled = 1 << 12,
                    Dervish = 1 << 13
                }
                
                [Flags]
                public enum YExtraFlagsValue : ushort
                {
                    AutoaimFriendly = 1 << 0,
                    AutoaimPlasma = 1 << 1,
                    AutoaimHeadshot = 1 << 2,
                    AutoaimVulnerable = 1 << 3,
                    AutoaimInvincible = 1 << 4
                }
                
                [Flags]
                public enum YWeaponFlagsValue : ushort
                {
                    PrimaryWeapon = 1 << 0,
                    SecondaryWeapon = 1 << 1,
                    BackpackWeapon = 1 << 2,
                    AgeBelowCutoff = 1 << 3,
                    ClipBelowCutoff = 1 << 4,
                    TotalBelowCutoff = 1 << 5,
                    Overheated = 1 << 6,
                    OutOfAmmo = 1 << 7,
                    LockTargetAvailable = 1 << 8,
                    Locking = 1 << 9,
                    Locked = 1 << 10
                }
                
                [Flags]
                public enum YGameEngineStateFlagsValue : ushort
                {
                    CampaignSolo = 1 << 0,
                    CampaignCoop = 1 << 1,
                    FreeForAll = 1 << 2,
                    TeamGame = 1 << 3,
                    UserLeading = 1 << 4,
                    UserNotLeading = 1 << 5,
                    TimedGame = 1 << 6,
                    UntimedGame = 1 << 7,
                    OtherScoreValid = 1 << 8,
                    OtherScoreInvalid = 1 << 9,
                    PlayerIsArmingBomb = 1 << 10,
                    PlayerTalking = 1 << 11
                }
                
                [Flags]
                public enum NUnitFlagsValue : ushort
                {
                    Default = 1 << 0,
                    GrenadeTypeIsNone = 1 << 1,
                    GrenadeTypeIsFrag = 1 << 2,
                    GrenadeTypeIsPlasma = 1 << 3,
                    UnitIsSingleWielding = 1 << 4,
                    UnitIsDualWielding = 1 << 5,
                    UnitIsUnzoomed = 1 << 6,
                    UnitIsZoomedLevel1 = 1 << 7,
                    UnitIsZoomedLevel2 = 1 << 8,
                    GrenadesDisabled = 1 << 9,
                    BinocularsEnabled = 1 << 10,
                    MotionSensorEnabled = 1 << 11,
                    ShieldEnabled = 1 << 12,
                    Dervish = 1 << 13
                }
                
                [Flags]
                public enum NExtraFlagsValue : ushort
                {
                    AutoaimFriendly = 1 << 0,
                    AutoaimPlasma = 1 << 1,
                    AutoaimHeadshot = 1 << 2,
                    AutoaimVulnerable = 1 << 3,
                    AutoaimInvincible = 1 << 4
                }
                
                [Flags]
                public enum NWeaponFlagsValue : ushort
                {
                    PrimaryWeapon = 1 << 0,
                    SecondaryWeapon = 1 << 1,
                    BackpackWeapon = 1 << 2,
                    AgeBelowCutoff = 1 << 3,
                    ClipBelowCutoff = 1 << 4,
                    TotalBelowCutoff = 1 << 5,
                    Overheated = 1 << 6,
                    OutOfAmmo = 1 << 7,
                    LockTargetAvailable = 1 << 8,
                    Locking = 1 << 9,
                    Locked = 1 << 10
                }
                
                [Flags]
                public enum NGameEngineStateFlagsValue : ushort
                {
                    CampaignSolo = 1 << 0,
                    CampaignCoop = 1 << 1,
                    FreeForAll = 1 << 2,
                    TeamGame = 1 << 3,
                    UserLeading = 1 << 4,
                    UserNotLeading = 1 << 5,
                    TimedGame = 1 << 6,
                    UntimedGame = 1 << 7,
                    OtherScoreValid = 1 << 8,
                    OtherScoreInvalid = 1 << 9,
                    PlayerIsArmingBomb = 1 << 10,
                    PlayerTalking = 1 << 11
                }
            }
            
            public enum AnchorValue : short
            {
                HealthAndShield,
                WeaponHud,
                MotionSensor,
                Scoreboard,
                Crosshair,
                LockOnTarget
            }
            
            [Flags]
            public enum FlagsValue : ushort
            {
                Unused = 1 << 0
            }
            
            [TagStructure(Size = 0x10)]
            public class ScreenEffectBonusStructBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "egor" })]
                public CachedTag HalfscreenScreenEffect;
                [TagField(ValidTags = new [] { "egor" })]
                public CachedTag QuarterscreenScreenEffect;
            }
        }
    }
}

