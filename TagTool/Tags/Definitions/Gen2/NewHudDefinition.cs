using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "new_hud_definition", Tag = "nhdt", Size = 0x3C)]
    public class NewHudDefinition : TagStructure
    {
        public CachedTag DoNotUse;
        public List<HudBitmapWidgetDefinition> BitmapWidgets;
        public List<HudTextWidgetDefinition> TextWidgets;
        public NewHudDashlightData DashlightData;
        public List<HudScreenEffectWidgetDefinition> ScreenEffectWidgets;
        
        [TagStructure(Size = 0x78)]
        public class HudBitmapWidgetDefinition : TagStructure
        {
            public StringId Name;
            public HudWidgetInputsDefinition Unknown1;
            public HudWidgetStateDefinition Unknown2;
            public AnchorValue Anchor;
            public FlagsValue Flags;
            public CachedTag Bitmap;
            public CachedTag Shader;
            public sbyte FullscreenSequenceIndex;
            public sbyte HalfscreenSequenceIndex;
            public sbyte QuarterscreenSequenceIndex;
            [TagField(Flags = Padding, Length = 1)]
            public byte[] Padding1;
            public Point2d FullscreenOffset;
            public Point2d HalfscreenOffset;
            public Point2d QuarterscreenOffset;
            public RealPoint2d FullscreenRegistrationPoint;
            public RealPoint2d HalfscreenRegistrationPoint;
            public RealPoint2d QuarterscreenRegistrationPoint;
            public List<HudWidgetEffectDefinition> Effect;
            public SpecialHudTypeValue SpecialHudType;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            
            [TagStructure(Size = 0x4)]
            public class HudWidgetInputsDefinition : TagStructure
            {
                /// <summary>
                /// widget inputs
                /// </summary>
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
                    Unknown4,
                    Unknown5,
                    Unknown6,
                    Unknown7,
                    Unknown8,
                    Unknown9,
                    Unknown10,
                    Unknown11,
                    Unknown12,
                    Unknown13,
                    Unknown14,
                    Unknown15,
                    UnitShield,
                    UnitBody,
                    UnitAutoaimed,
                    UnitHasNoGrenades,
                    UnitFragGrenCnt,
                    UnitPlasmaGrenCnt,
                    UnitTimeOnDplShld,
                    UnitZoomFraction,
                    UnitCamoValue,
                    Unknown25,
                    Unknown26,
                    Unknown27,
                    Unknown28,
                    Unknown29,
                    Unknown30,
                    Unknown31,
                    ParentShield,
                    ParentBody,
                    Unknown34,
                    Unknown35,
                    Unknown36,
                    Unknown37,
                    Unknown38,
                    Unknown39,
                    Unknown40,
                    Unknown41,
                    Unknown42,
                    Unknown43,
                    Unknown44,
                    Unknown45,
                    Unknown46,
                    Unknown47,
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
                    Unknown58,
                    Unknown59,
                    Unknown60,
                    Unknown61,
                    Unknown62,
                    Unknown63,
                    Unknown64,
                    UserScoreFraction,
                    OtherUserScoreFraction,
                    UserWinning,
                    BombArmingAmount,
                    Unknown69,
                    Unknown70,
                    Unknown71,
                    Unknown72,
                    Unknown73,
                    Unknown74,
                    Unknown75,
                    Unknown76,
                    Unknown77,
                    Unknown78,
                    Unknown79,
                    Unknown80
                }
                
                public enum Input2Value : sbyte
                {
                    BasicZero,
                    BasicOne,
                    BasicTime,
                    BasicGlobalHudFade,
                    Unknown4,
                    Unknown5,
                    Unknown6,
                    Unknown7,
                    Unknown8,
                    Unknown9,
                    Unknown10,
                    Unknown11,
                    Unknown12,
                    Unknown13,
                    Unknown14,
                    Unknown15,
                    UnitShield,
                    UnitBody,
                    UnitAutoaimed,
                    UnitHasNoGrenades,
                    UnitFragGrenCnt,
                    UnitPlasmaGrenCnt,
                    UnitTimeOnDplShld,
                    UnitZoomFraction,
                    UnitCamoValue,
                    Unknown25,
                    Unknown26,
                    Unknown27,
                    Unknown28,
                    Unknown29,
                    Unknown30,
                    Unknown31,
                    ParentShield,
                    ParentBody,
                    Unknown34,
                    Unknown35,
                    Unknown36,
                    Unknown37,
                    Unknown38,
                    Unknown39,
                    Unknown40,
                    Unknown41,
                    Unknown42,
                    Unknown43,
                    Unknown44,
                    Unknown45,
                    Unknown46,
                    Unknown47,
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
                    Unknown58,
                    Unknown59,
                    Unknown60,
                    Unknown61,
                    Unknown62,
                    Unknown63,
                    Unknown64,
                    UserScoreFraction,
                    OtherUserScoreFraction,
                    UserWinning,
                    BombArmingAmount,
                    Unknown69,
                    Unknown70,
                    Unknown71,
                    Unknown72,
                    Unknown73,
                    Unknown74,
                    Unknown75,
                    Unknown76,
                    Unknown77,
                    Unknown78,
                    Unknown79,
                    Unknown80
                }
                
                public enum Input3Value : sbyte
                {
                    BasicZero,
                    BasicOne,
                    BasicTime,
                    BasicGlobalHudFade,
                    Unknown4,
                    Unknown5,
                    Unknown6,
                    Unknown7,
                    Unknown8,
                    Unknown9,
                    Unknown10,
                    Unknown11,
                    Unknown12,
                    Unknown13,
                    Unknown14,
                    Unknown15,
                    UnitShield,
                    UnitBody,
                    UnitAutoaimed,
                    UnitHasNoGrenades,
                    UnitFragGrenCnt,
                    UnitPlasmaGrenCnt,
                    UnitTimeOnDplShld,
                    UnitZoomFraction,
                    UnitCamoValue,
                    Unknown25,
                    Unknown26,
                    Unknown27,
                    Unknown28,
                    Unknown29,
                    Unknown30,
                    Unknown31,
                    ParentShield,
                    ParentBody,
                    Unknown34,
                    Unknown35,
                    Unknown36,
                    Unknown37,
                    Unknown38,
                    Unknown39,
                    Unknown40,
                    Unknown41,
                    Unknown42,
                    Unknown43,
                    Unknown44,
                    Unknown45,
                    Unknown46,
                    Unknown47,
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
                    Unknown58,
                    Unknown59,
                    Unknown60,
                    Unknown61,
                    Unknown62,
                    Unknown63,
                    Unknown64,
                    UserScoreFraction,
                    OtherUserScoreFraction,
                    UserWinning,
                    BombArmingAmount,
                    Unknown69,
                    Unknown70,
                    Unknown71,
                    Unknown72,
                    Unknown73,
                    Unknown74,
                    Unknown75,
                    Unknown76,
                    Unknown77,
                    Unknown78,
                    Unknown79,
                    Unknown80
                }
                
                public enum Input4Value : sbyte
                {
                    BasicZero,
                    BasicOne,
                    BasicTime,
                    BasicGlobalHudFade,
                    Unknown4,
                    Unknown5,
                    Unknown6,
                    Unknown7,
                    Unknown8,
                    Unknown9,
                    Unknown10,
                    Unknown11,
                    Unknown12,
                    Unknown13,
                    Unknown14,
                    Unknown15,
                    UnitShield,
                    UnitBody,
                    UnitAutoaimed,
                    UnitHasNoGrenades,
                    UnitFragGrenCnt,
                    UnitPlasmaGrenCnt,
                    UnitTimeOnDplShld,
                    UnitZoomFraction,
                    UnitCamoValue,
                    Unknown25,
                    Unknown26,
                    Unknown27,
                    Unknown28,
                    Unknown29,
                    Unknown30,
                    Unknown31,
                    ParentShield,
                    ParentBody,
                    Unknown34,
                    Unknown35,
                    Unknown36,
                    Unknown37,
                    Unknown38,
                    Unknown39,
                    Unknown40,
                    Unknown41,
                    Unknown42,
                    Unknown43,
                    Unknown44,
                    Unknown45,
                    Unknown46,
                    Unknown47,
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
                    Unknown58,
                    Unknown59,
                    Unknown60,
                    Unknown61,
                    Unknown62,
                    Unknown63,
                    Unknown64,
                    UserScoreFraction,
                    OtherUserScoreFraction,
                    UserWinning,
                    BombArmingAmount,
                    Unknown69,
                    Unknown70,
                    Unknown71,
                    Unknown72,
                    Unknown73,
                    Unknown74,
                    Unknown75,
                    Unknown76,
                    Unknown77,
                    Unknown78,
                    Unknown79,
                    Unknown80
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class HudWidgetStateDefinition : TagStructure
            {
                /// <summary>
                /// widget state
                /// </summary>
                /// <remarks>
                /// this section is split up into YES and NO flags.
                /// a widget will draw if any of it's YES flags are true,
                /// but it will NOT draw if any of it's NO flags are true.
                /// 
                /// </remarks>
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
                [TagField(Flags = Padding, Length = 1)]
                public byte[] Padding1;
                
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
            
            [TagStructure(Size = 0x7C)]
            public class HudWidgetEffectDefinition : TagStructure
            {
                /// <summary>
                /// WIDGET EFFECTS
                /// </summary>
                /// <remarks>
                /// allow the scaling, rotation, and offsetting of widgets
                /// </remarks>
                public FlagsValue Flags;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                /// <summary>
                /// horizontal and vertical scale
                /// </summary>
                public HudWidgetEffectFunction YourMom;
                public HudWidgetEffectFunction YourMom1;
                /// <summary>
                /// theta
                /// </summary>
                public HudWidgetEffectFunction YourMom2;
                /// <summary>
                /// horizontal and vertical offset
                /// </summary>
                public HudWidgetEffectFunction YourMom3;
                public HudWidgetEffectFunction YourMom4;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    ApplyScale = 1 << 0,
                    ApplyTheta = 1 << 1,
                    ApplyOffset = 1 << 2
                }
                
                [TagStructure(Size = 0x18)]
                public class HudWidgetEffectFunction : TagStructure
                {
                    public StringId InputName;
                    public StringId RangeName;
                    public float TimePeriodInSeconds;
                    public FunctionDefinition Function;
                    
                    [TagStructure(Size = 0xC)]
                    public class FunctionDefinition : TagStructure
                    {
                        public FunctionDefinition Function;
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
        
        [TagStructure(Size = 0x60)]
        public class HudTextWidgetDefinition : TagStructure
        {
            public StringId Name;
            public HudWidgetInputsDefinition Unknown1;
            public HudWidgetStateDefinition Unknown2;
            public AnchorValue Anchor;
            /// <summary>
            /// FLAGS
            /// </summary>
            /// <remarks>
            /// string is a number: treats the inputted string id as a function name, not a string name
            /// 
            /// force 2-digit number: when used in combination with above, forces output to be a 2-digit numberwith leading zeros if necessary
            /// 
            /// force 3-digit number: same as above, but with 3 digits instead of 2
            /// 
            /// 
            /// </remarks>
            public FlagsValue Flags;
            public CachedTag Shader;
            public StringId String;
            public JustificationValue Justification;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public FullscreenFontIndexValue FullscreenFontIndex;
            public HalfscreenFontIndexValue HalfscreenFontIndex;
            public QuarterscreenFontIndexValue QuarterscreenFontIndex;
            [TagField(Flags = Padding, Length = 1)]
            public byte[] Padding2;
            public float FullscreenScale;
            public float HalfscreenScale;
            public float QuarterscreenScale;
            public Point2d FullscreenOffset;
            public Point2d HalfscreenOffset;
            public Point2d QuarterscreenOffset;
            public List<HudWidgetEffectDefinition> Effect;
            
            [TagStructure(Size = 0x4)]
            public class HudWidgetInputsDefinition : TagStructure
            {
                /// <summary>
                /// widget inputs
                /// </summary>
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
                    Unknown4,
                    Unknown5,
                    Unknown6,
                    Unknown7,
                    Unknown8,
                    Unknown9,
                    Unknown10,
                    Unknown11,
                    Unknown12,
                    Unknown13,
                    Unknown14,
                    Unknown15,
                    UnitShield,
                    UnitBody,
                    UnitAutoaimed,
                    UnitHasNoGrenades,
                    UnitFragGrenCnt,
                    UnitPlasmaGrenCnt,
                    UnitTimeOnDplShld,
                    UnitZoomFraction,
                    UnitCamoValue,
                    Unknown25,
                    Unknown26,
                    Unknown27,
                    Unknown28,
                    Unknown29,
                    Unknown30,
                    Unknown31,
                    ParentShield,
                    ParentBody,
                    Unknown34,
                    Unknown35,
                    Unknown36,
                    Unknown37,
                    Unknown38,
                    Unknown39,
                    Unknown40,
                    Unknown41,
                    Unknown42,
                    Unknown43,
                    Unknown44,
                    Unknown45,
                    Unknown46,
                    Unknown47,
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
                    Unknown58,
                    Unknown59,
                    Unknown60,
                    Unknown61,
                    Unknown62,
                    Unknown63,
                    Unknown64,
                    UserScoreFraction,
                    OtherUserScoreFraction,
                    UserWinning,
                    BombArmingAmount,
                    Unknown69,
                    Unknown70,
                    Unknown71,
                    Unknown72,
                    Unknown73,
                    Unknown74,
                    Unknown75,
                    Unknown76,
                    Unknown77,
                    Unknown78,
                    Unknown79,
                    Unknown80
                }
                
                public enum Input2Value : sbyte
                {
                    BasicZero,
                    BasicOne,
                    BasicTime,
                    BasicGlobalHudFade,
                    Unknown4,
                    Unknown5,
                    Unknown6,
                    Unknown7,
                    Unknown8,
                    Unknown9,
                    Unknown10,
                    Unknown11,
                    Unknown12,
                    Unknown13,
                    Unknown14,
                    Unknown15,
                    UnitShield,
                    UnitBody,
                    UnitAutoaimed,
                    UnitHasNoGrenades,
                    UnitFragGrenCnt,
                    UnitPlasmaGrenCnt,
                    UnitTimeOnDplShld,
                    UnitZoomFraction,
                    UnitCamoValue,
                    Unknown25,
                    Unknown26,
                    Unknown27,
                    Unknown28,
                    Unknown29,
                    Unknown30,
                    Unknown31,
                    ParentShield,
                    ParentBody,
                    Unknown34,
                    Unknown35,
                    Unknown36,
                    Unknown37,
                    Unknown38,
                    Unknown39,
                    Unknown40,
                    Unknown41,
                    Unknown42,
                    Unknown43,
                    Unknown44,
                    Unknown45,
                    Unknown46,
                    Unknown47,
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
                    Unknown58,
                    Unknown59,
                    Unknown60,
                    Unknown61,
                    Unknown62,
                    Unknown63,
                    Unknown64,
                    UserScoreFraction,
                    OtherUserScoreFraction,
                    UserWinning,
                    BombArmingAmount,
                    Unknown69,
                    Unknown70,
                    Unknown71,
                    Unknown72,
                    Unknown73,
                    Unknown74,
                    Unknown75,
                    Unknown76,
                    Unknown77,
                    Unknown78,
                    Unknown79,
                    Unknown80
                }
                
                public enum Input3Value : sbyte
                {
                    BasicZero,
                    BasicOne,
                    BasicTime,
                    BasicGlobalHudFade,
                    Unknown4,
                    Unknown5,
                    Unknown6,
                    Unknown7,
                    Unknown8,
                    Unknown9,
                    Unknown10,
                    Unknown11,
                    Unknown12,
                    Unknown13,
                    Unknown14,
                    Unknown15,
                    UnitShield,
                    UnitBody,
                    UnitAutoaimed,
                    UnitHasNoGrenades,
                    UnitFragGrenCnt,
                    UnitPlasmaGrenCnt,
                    UnitTimeOnDplShld,
                    UnitZoomFraction,
                    UnitCamoValue,
                    Unknown25,
                    Unknown26,
                    Unknown27,
                    Unknown28,
                    Unknown29,
                    Unknown30,
                    Unknown31,
                    ParentShield,
                    ParentBody,
                    Unknown34,
                    Unknown35,
                    Unknown36,
                    Unknown37,
                    Unknown38,
                    Unknown39,
                    Unknown40,
                    Unknown41,
                    Unknown42,
                    Unknown43,
                    Unknown44,
                    Unknown45,
                    Unknown46,
                    Unknown47,
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
                    Unknown58,
                    Unknown59,
                    Unknown60,
                    Unknown61,
                    Unknown62,
                    Unknown63,
                    Unknown64,
                    UserScoreFraction,
                    OtherUserScoreFraction,
                    UserWinning,
                    BombArmingAmount,
                    Unknown69,
                    Unknown70,
                    Unknown71,
                    Unknown72,
                    Unknown73,
                    Unknown74,
                    Unknown75,
                    Unknown76,
                    Unknown77,
                    Unknown78,
                    Unknown79,
                    Unknown80
                }
                
                public enum Input4Value : sbyte
                {
                    BasicZero,
                    BasicOne,
                    BasicTime,
                    BasicGlobalHudFade,
                    Unknown4,
                    Unknown5,
                    Unknown6,
                    Unknown7,
                    Unknown8,
                    Unknown9,
                    Unknown10,
                    Unknown11,
                    Unknown12,
                    Unknown13,
                    Unknown14,
                    Unknown15,
                    UnitShield,
                    UnitBody,
                    UnitAutoaimed,
                    UnitHasNoGrenades,
                    UnitFragGrenCnt,
                    UnitPlasmaGrenCnt,
                    UnitTimeOnDplShld,
                    UnitZoomFraction,
                    UnitCamoValue,
                    Unknown25,
                    Unknown26,
                    Unknown27,
                    Unknown28,
                    Unknown29,
                    Unknown30,
                    Unknown31,
                    ParentShield,
                    ParentBody,
                    Unknown34,
                    Unknown35,
                    Unknown36,
                    Unknown37,
                    Unknown38,
                    Unknown39,
                    Unknown40,
                    Unknown41,
                    Unknown42,
                    Unknown43,
                    Unknown44,
                    Unknown45,
                    Unknown46,
                    Unknown47,
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
                    Unknown58,
                    Unknown59,
                    Unknown60,
                    Unknown61,
                    Unknown62,
                    Unknown63,
                    Unknown64,
                    UserScoreFraction,
                    OtherUserScoreFraction,
                    UserWinning,
                    BombArmingAmount,
                    Unknown69,
                    Unknown70,
                    Unknown71,
                    Unknown72,
                    Unknown73,
                    Unknown74,
                    Unknown75,
                    Unknown76,
                    Unknown77,
                    Unknown78,
                    Unknown79,
                    Unknown80
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class HudWidgetStateDefinition : TagStructure
            {
                /// <summary>
                /// widget state
                /// </summary>
                /// <remarks>
                /// this section is split up into YES and NO flags.
                /// a widget will draw if any of it's YES flags are true,
                /// but it will NOT draw if any of it's NO flags are true.
                /// 
                /// </remarks>
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
                [TagField(Flags = Padding, Length = 1)]
                public byte[] Padding1;
                
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
            
            [TagStructure(Size = 0x7C)]
            public class HudWidgetEffectDefinition : TagStructure
            {
                /// <summary>
                /// WIDGET EFFECTS
                /// </summary>
                /// <remarks>
                /// allow the scaling, rotation, and offsetting of widgets
                /// </remarks>
                public FlagsValue Flags;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                /// <summary>
                /// horizontal and vertical scale
                /// </summary>
                public HudWidgetEffectFunction YourMom;
                public HudWidgetEffectFunction YourMom1;
                /// <summary>
                /// theta
                /// </summary>
                public HudWidgetEffectFunction YourMom2;
                /// <summary>
                /// horizontal and vertical offset
                /// </summary>
                public HudWidgetEffectFunction YourMom3;
                public HudWidgetEffectFunction YourMom4;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    ApplyScale = 1 << 0,
                    ApplyTheta = 1 << 1,
                    ApplyOffset = 1 << 2
                }
                
                [TagStructure(Size = 0x18)]
                public class HudWidgetEffectFunction : TagStructure
                {
                    public StringId InputName;
                    public StringId RangeName;
                    public float TimePeriodInSeconds;
                    public FunctionDefinition Function;
                    
                    [TagStructure(Size = 0xC)]
                    public class FunctionDefinition : TagStructure
                    {
                        public FunctionDefinition Function;
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class NewHudDashlightData : TagStructure
        {
            /// <summary>
            /// dashlight data
            /// </summary>
            /// <remarks>
            /// only relevant to new hud tags for weapons
            /// </remarks>
            public short LowClipCutoff; // the cutoff for showing the reload dashlight
            public short LowAmmoCutoff; // the cutoff for showing the low ammo dashlight
            public float AgeCutoff; // the age cutoff for showing the low battery dashlight
        }
        
        [TagStructure(Size = 0x70)]
        public class HudScreenEffectWidgetDefinition : TagStructure
        {
            public StringId Name;
            public HudWidgetInputsDefinition Unknown1;
            public HudWidgetStateDefinition Unknown2;
            public AnchorValue Anchor;
            public FlagsValue Flags;
            public CachedTag Bitmap;
            public CachedTag FullscreenScreenEffect;
            public ScreenEffectBonusStructBlock Waa;
            public sbyte FullscreenSequenceIndex;
            public sbyte HalfscreenSequenceIndex;
            public sbyte QuarterscreenSequenceIndex;
            [TagField(Flags = Padding, Length = 1)]
            public byte[] Padding1;
            public Point2d FullscreenOffset;
            public Point2d HalfscreenOffset;
            public Point2d QuarterscreenOffset;
            
            [TagStructure(Size = 0x4)]
            public class HudWidgetInputsDefinition : TagStructure
            {
                /// <summary>
                /// widget inputs
                /// </summary>
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
                    Unknown4,
                    Unknown5,
                    Unknown6,
                    Unknown7,
                    Unknown8,
                    Unknown9,
                    Unknown10,
                    Unknown11,
                    Unknown12,
                    Unknown13,
                    Unknown14,
                    Unknown15,
                    UnitShield,
                    UnitBody,
                    UnitAutoaimed,
                    UnitHasNoGrenades,
                    UnitFragGrenCnt,
                    UnitPlasmaGrenCnt,
                    UnitTimeOnDplShld,
                    UnitZoomFraction,
                    UnitCamoValue,
                    Unknown25,
                    Unknown26,
                    Unknown27,
                    Unknown28,
                    Unknown29,
                    Unknown30,
                    Unknown31,
                    ParentShield,
                    ParentBody,
                    Unknown34,
                    Unknown35,
                    Unknown36,
                    Unknown37,
                    Unknown38,
                    Unknown39,
                    Unknown40,
                    Unknown41,
                    Unknown42,
                    Unknown43,
                    Unknown44,
                    Unknown45,
                    Unknown46,
                    Unknown47,
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
                    Unknown58,
                    Unknown59,
                    Unknown60,
                    Unknown61,
                    Unknown62,
                    Unknown63,
                    Unknown64,
                    UserScoreFraction,
                    OtherUserScoreFraction,
                    UserWinning,
                    BombArmingAmount,
                    Unknown69,
                    Unknown70,
                    Unknown71,
                    Unknown72,
                    Unknown73,
                    Unknown74,
                    Unknown75,
                    Unknown76,
                    Unknown77,
                    Unknown78,
                    Unknown79,
                    Unknown80
                }
                
                public enum Input2Value : sbyte
                {
                    BasicZero,
                    BasicOne,
                    BasicTime,
                    BasicGlobalHudFade,
                    Unknown4,
                    Unknown5,
                    Unknown6,
                    Unknown7,
                    Unknown8,
                    Unknown9,
                    Unknown10,
                    Unknown11,
                    Unknown12,
                    Unknown13,
                    Unknown14,
                    Unknown15,
                    UnitShield,
                    UnitBody,
                    UnitAutoaimed,
                    UnitHasNoGrenades,
                    UnitFragGrenCnt,
                    UnitPlasmaGrenCnt,
                    UnitTimeOnDplShld,
                    UnitZoomFraction,
                    UnitCamoValue,
                    Unknown25,
                    Unknown26,
                    Unknown27,
                    Unknown28,
                    Unknown29,
                    Unknown30,
                    Unknown31,
                    ParentShield,
                    ParentBody,
                    Unknown34,
                    Unknown35,
                    Unknown36,
                    Unknown37,
                    Unknown38,
                    Unknown39,
                    Unknown40,
                    Unknown41,
                    Unknown42,
                    Unknown43,
                    Unknown44,
                    Unknown45,
                    Unknown46,
                    Unknown47,
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
                    Unknown58,
                    Unknown59,
                    Unknown60,
                    Unknown61,
                    Unknown62,
                    Unknown63,
                    Unknown64,
                    UserScoreFraction,
                    OtherUserScoreFraction,
                    UserWinning,
                    BombArmingAmount,
                    Unknown69,
                    Unknown70,
                    Unknown71,
                    Unknown72,
                    Unknown73,
                    Unknown74,
                    Unknown75,
                    Unknown76,
                    Unknown77,
                    Unknown78,
                    Unknown79,
                    Unknown80
                }
                
                public enum Input3Value : sbyte
                {
                    BasicZero,
                    BasicOne,
                    BasicTime,
                    BasicGlobalHudFade,
                    Unknown4,
                    Unknown5,
                    Unknown6,
                    Unknown7,
                    Unknown8,
                    Unknown9,
                    Unknown10,
                    Unknown11,
                    Unknown12,
                    Unknown13,
                    Unknown14,
                    Unknown15,
                    UnitShield,
                    UnitBody,
                    UnitAutoaimed,
                    UnitHasNoGrenades,
                    UnitFragGrenCnt,
                    UnitPlasmaGrenCnt,
                    UnitTimeOnDplShld,
                    UnitZoomFraction,
                    UnitCamoValue,
                    Unknown25,
                    Unknown26,
                    Unknown27,
                    Unknown28,
                    Unknown29,
                    Unknown30,
                    Unknown31,
                    ParentShield,
                    ParentBody,
                    Unknown34,
                    Unknown35,
                    Unknown36,
                    Unknown37,
                    Unknown38,
                    Unknown39,
                    Unknown40,
                    Unknown41,
                    Unknown42,
                    Unknown43,
                    Unknown44,
                    Unknown45,
                    Unknown46,
                    Unknown47,
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
                    Unknown58,
                    Unknown59,
                    Unknown60,
                    Unknown61,
                    Unknown62,
                    Unknown63,
                    Unknown64,
                    UserScoreFraction,
                    OtherUserScoreFraction,
                    UserWinning,
                    BombArmingAmount,
                    Unknown69,
                    Unknown70,
                    Unknown71,
                    Unknown72,
                    Unknown73,
                    Unknown74,
                    Unknown75,
                    Unknown76,
                    Unknown77,
                    Unknown78,
                    Unknown79,
                    Unknown80
                }
                
                public enum Input4Value : sbyte
                {
                    BasicZero,
                    BasicOne,
                    BasicTime,
                    BasicGlobalHudFade,
                    Unknown4,
                    Unknown5,
                    Unknown6,
                    Unknown7,
                    Unknown8,
                    Unknown9,
                    Unknown10,
                    Unknown11,
                    Unknown12,
                    Unknown13,
                    Unknown14,
                    Unknown15,
                    UnitShield,
                    UnitBody,
                    UnitAutoaimed,
                    UnitHasNoGrenades,
                    UnitFragGrenCnt,
                    UnitPlasmaGrenCnt,
                    UnitTimeOnDplShld,
                    UnitZoomFraction,
                    UnitCamoValue,
                    Unknown25,
                    Unknown26,
                    Unknown27,
                    Unknown28,
                    Unknown29,
                    Unknown30,
                    Unknown31,
                    ParentShield,
                    ParentBody,
                    Unknown34,
                    Unknown35,
                    Unknown36,
                    Unknown37,
                    Unknown38,
                    Unknown39,
                    Unknown40,
                    Unknown41,
                    Unknown42,
                    Unknown43,
                    Unknown44,
                    Unknown45,
                    Unknown46,
                    Unknown47,
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
                    Unknown58,
                    Unknown59,
                    Unknown60,
                    Unknown61,
                    Unknown62,
                    Unknown63,
                    Unknown64,
                    UserScoreFraction,
                    OtherUserScoreFraction,
                    UserWinning,
                    BombArmingAmount,
                    Unknown69,
                    Unknown70,
                    Unknown71,
                    Unknown72,
                    Unknown73,
                    Unknown74,
                    Unknown75,
                    Unknown76,
                    Unknown77,
                    Unknown78,
                    Unknown79,
                    Unknown80
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class HudWidgetStateDefinition : TagStructure
            {
                /// <summary>
                /// widget state
                /// </summary>
                /// <remarks>
                /// this section is split up into YES and NO flags.
                /// a widget will draw if any of it's YES flags are true,
                /// but it will NOT draw if any of it's NO flags are true.
                /// 
                /// </remarks>
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
                [TagField(Flags = Padding, Length = 1)]
                public byte[] Padding1;
                
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
            
            [TagStructure(Size = 0x20)]
            public class ScreenEffectBonusStructBlock : TagStructure
            {
                public CachedTag HalfscreenScreenEffect;
                public CachedTag QuarterscreenScreenEffect;
            }
        }
    }
}

