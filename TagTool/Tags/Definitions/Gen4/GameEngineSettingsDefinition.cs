using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "game_engine_settings_definition", Tag = "wezr", Size = 0x4C)]
    public class GameEngineSettingsDefinition : TagStructure
    {
        public GameEngineSettingsFlags Flags;
        public List<GameEnginePlayerTraitsListBlock> PlayerTraits;
        public List<GameEngineAiTraitsListBlock> AiTraits;
        public List<GameEngineSandboxVariantBlock> SandboxVariants;
        public List<GameEngineSurvivalVariantBlock> SurvivalVariants;
        public List<GameEngineFirefightVariantShellBlock> NewFirefightVariants;
        public List<GameEngineCampaignVariantBlock> CampaignVariants;
        
        [Flags]
        public enum GameEngineSettingsFlags : uint
        {
            Unused = 1 << 0
        }
        
        [TagStructure(Size = 0x40)]
        public class GameEnginePlayerTraitsListBlock : TagStructure
        {
            public StringId Name;
            public List<PlayerTraitsVitalityBlock> VitalityTraits;
            public List<PlayerTraitsWeaponsBlock> WeaponTraits;
            public List<PlayerTraitsMovementBlock> MovementTraits;
            public List<PlayerTraitsAppearanceBlock> AppearanceTraits;
            public List<PlayerTraitsSensorsBlock> SensorTraits;
            
            [TagStructure(Size = 0x40)]
            public class PlayerTraitsVitalityBlock : TagStructure
            {
                public PlayerTraitsVitalityFloatFlags ShouldApplyTrait;
                public float DamageResistance;
                public float ShieldMultiplier;
                public float BodyMultiplier;
                public float ShieldStunDuration;
                public float ShieldRechargeRate;
                public float BodyRechargeRate;
                public float OvershieldRechargeRate;
                public float VampirismPercent;
                // incoming damage multiplied by (1 - resistance)
                public float ExplosiveDamageResistance;
                public float WheelmanArmorVehicleStunTimeModifier;
                public float WheelmanArmorVehicleRechargeTimeModifier;
                public float WheelmanArmorVehicleEmpDisabledTimeModifier;
                public float FallDamageMultiplier;
                public PlayerTraitBoolEnum HeadshotImmunity;
                public PlayerTraitBoolEnum AssassinationImmunity;
                public PlayerTraitBoolEnum Deathless;
                public PlayerTraitBoolEnum FastTrackArmor;
                public PlayerTraitPowerupCancellationEnum PowerupCancellation;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum PlayerTraitsVitalityFloatFlags : uint
                {
                    DamageResistance = 1 << 0,
                    ShieldMultiplier = 1 << 1,
                    BodyMultiplier = 1 << 2,
                    ShieldStunDuration = 1 << 3,
                    ShieldRechargeRate = 1 << 4,
                    BodyRechargeRate = 1 << 5,
                    OvershieldRechargeRate = 1 << 6,
                    VampirismPercent = 1 << 7,
                    ExplosiveDamageResistance = 1 << 8,
                    WheelmanArmorVehicleStunTimeModifier = 1 << 9,
                    WheelmanArmorVehicleRechargeTimeModifier = 1 << 10,
                    WheelmanArmorVehicleEmpDisabledTimeModifier = 1 << 11,
                    FallDamageMultiplier = 1 << 12
                }
                
                public enum PlayerTraitBoolEnum : sbyte
                {
                    Unchanged,
                    False,
                    True
                }
                
                public enum PlayerTraitPowerupCancellationEnum : sbyte
                {
                    Unchanged,
                    None,
                    NoOvershield
                }
            }
            
            [TagStructure(Size = 0x80)]
            public class PlayerTraitsWeaponsBlock : TagStructure
            {
                public PlayerTraitsWeaponsFloatFlags ShouldApplyTrait;
                public float DamageMultiplier;
                public float MeleeDamageMultiplier;
                public float GrenadeRechargeSecondsFrag;
                public float GrenadeRechargeSecondsPlasma;
                public float GrenadeRechargeSecondsSpike;
                public float HeroEquipmentEnergyUseRateModifier;
                public float HeroEquipmentEnergyRechargeDelayModifier;
                public float HeroEquipmentEnergyRechargeRateModifier;
                public float HeroEquipmentInitialEnergyModifier;
                public float EquipmentEnergyUseRateModifier;
                public float EquipmentEnergyRechargeDelayModifier;
                public float EquipmentEnergyUseRechargeRateModifier;
                public float EquipmentEnergyInitialEnergyModifier;
                public float SwitchSpeedModifier;
                public float ReloadSpeedModifier;
                public float OrdnancePointsModifier;
                public float ExplosiveAreaOfEffectRadiusModifier;
                public float GunnerArmorModifier;
                public float StabilityArmorModifier;
                public float DropReconWarningSeconds;
                public float DropReconDistanceModifier;
                public float AssassinationSpeedModifier;
                public PlayerTraitBoolEnum WeaponPickupAllowed;
                public PlayerTraitInitialGrenadeCountEnum InitialGrenadeCount;
                public PlayerTraitInfiniteAmmoEnum InfiniteAmmo;
                public PlayerTraitEquipmentUsageEnum EquipmentUsage;
                // false will disable all equipment except auto turret
                public PlayerTraitEquipmentUsageEnum EquipmentUsageExceptingAutoTurret;
                public PlayerTraitBoolEnum EquipmentDrop;
                public PlayerTraitBoolEnum InfiniteEquipment;
                public PlayerTraitBoolEnum WeaponsAmmopack;
                public PlayerTraitBoolEnum WeaponsGrenadier;
                // spawns projectile specified in globals.globals
                public PlayerTraitBoolEnum WeaponsExplodeOnDeathArmormod;
                public PlayerTraitBoolEnum OrdnanceMarkersVisible;
                public PlayerTraitBoolEnum WeaponsOrdnanceRerollAvailable;
                // grenade probabilities defined in grenade_list.game_globals_grenade_list
                public PlayerTraitBoolEnum WeaponsResourceful;
                public PlayerTraitBoolEnum WeaponsWellEquipped;
                public PlayerTraitBoolEnum OrdnanceDisabled;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public StringId InitialPrimaryWeapon;
                public StringId InitialSecondaryWeapon;
                public StringId InitialEquipment;
                public StringId InitialTacticalPackage;
                public StringId InitialSupportUpgrade;
                
                [Flags]
                public enum PlayerTraitsWeaponsFloatFlags : uint
                {
                    DamageMultiplier = 1 << 0,
                    MeleeDamageMultiplier = 1 << 1,
                    GrenadeRechargeSecondsFrag = 1 << 2,
                    GrenadeRechargeSecondsPlasma = 1 << 3,
                    GrenadeRechargeSecondsSpike = 1 << 4,
                    HeroEquipmentEnergyUseRateModifier = 1 << 5,
                    HeroEquipmentEnergyRechargeDelayModifier = 1 << 6,
                    HeroEquipmentEnergyRechargeRateModifier = 1 << 7,
                    HeroEquipmentInitialEnergyModifier = 1 << 8,
                    EquipmentEnergyUseRateModifier = 1 << 9,
                    EquipmentEnergyRechargeDelayModifier = 1 << 10,
                    EquipmentEnergyUseRechargeRateModifier = 1 << 11,
                    EquipmentEnergyInitialEnergyModifier = 1 << 12,
                    SwitchSpeedModifier = 1 << 13,
                    ReloadSpeedModifier = 1 << 14,
                    OrdnancePointsModifier = 1 << 15,
                    ExplosiveAreaOfEffectRadiusModifier = 1 << 16,
                    GunnerArmorModifier = 1 << 17,
                    StabilityArmorModifier = 1 << 18,
                    DropReconWarningSeconds = 1 << 19,
                    DropReconDistanceModifier = 1 << 20,
                    AssassinationSpeedModifier = 1 << 21
                }
                
                public enum PlayerTraitBoolEnum : sbyte
                {
                    Unchanged,
                    False,
                    True
                }
                
                public enum PlayerTraitInitialGrenadeCountEnum : sbyte
                {
                    Unchanged,
                    MapDefault,
                    _0,
                    _1Frag,
                    _2Frag,
                    _1Plasma,
                    _2Plasma,
                    _1Type2,
                    _2Type2,
                    _1Type3,
                    _2Type3,
                    _1Type4,
                    _2Type4,
                    _1Type5,
                    _2Type5,
                    _1Type6,
                    _2Type6,
                    _1Type7,
                    _2Type7
                }
                
                public enum PlayerTraitInfiniteAmmoEnum : sbyte
                {
                    Unchanged,
                    Off,
                    On,
                    BottomlessClip
                }
                
                public enum PlayerTraitEquipmentUsageEnum : sbyte
                {
                    Unchanged,
                    Off,
                    NotWithObjectives,
                    On
                }
            }
            
            [TagStructure(Size = 0x1C)]
            public class PlayerTraitsMovementBlock : TagStructure
            {
                public PlayerTraitsMovementFloatFlags ShouldApplyTrait;
                public float Speed;
                public float GravityMultiplier;
                public float JumpMultiplier;
                public float TurnSpeedMultiplier;
                public PlayerTraitVehicleUsage VehicleUsage;
                public PlayerTraitDoubleJump DoubleJump;
                public PlayerTraitBoolEnum SprintUsage;
                public PlayerTraitBoolEnum AutomaticMomentumUsage;
                public PlayerTraitBoolEnum VaultingEnabled;
                public PlayerTraitBoolEnum Stealthy;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum PlayerTraitsMovementFloatFlags : uint
                {
                    Speed = 1 << 0,
                    GravityMultiplier = 1 << 1,
                    JumpMultiplier = 1 << 2,
                    TurnSpeedMultiplier = 1 << 3
                }
                
                public enum PlayerTraitVehicleUsage : sbyte
                {
                    Unchanged,
                    None,
                    PassengerOnly,
                    DriverOnly,
                    GunnerOnly,
                    NotPassenger,
                    NotDriver,
                    NotGunner,
                    Full
                }
                
                public enum PlayerTraitDoubleJump : sbyte
                {
                    Unchanged,
                    Off,
                    On,
                    OnPlusLunge
                }
                
                public enum PlayerTraitBoolEnum : sbyte
                {
                    Unchanged,
                    False,
                    True
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class PlayerTraitsAppearanceBlock : TagStructure
            {
                public PlayerTraitActiveCamo ActiveCamo;
                public PlayerTraitWaypoint Waypoint;
                public PlayerTraitWaypoint GamertagVisible;
                public PlayerTraitAura Aura;
                public StringId DeathEffect;
                public StringId AttachedEffect;
                
                public enum PlayerTraitActiveCamo : sbyte
                {
                    Unchanged,
                    Off,
                    Poor,
                    Good,
                    Excellent,
                    Invisible
                }
                
                public enum PlayerTraitWaypoint : sbyte
                {
                    Unchanged,
                    Off,
                    Allies,
                    All
                }
                
                public enum PlayerTraitAura : sbyte
                {
                    Unchanged,
                    Off,
                    TeamColor,
                    Black,
                    White
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class PlayerTraitsSensorsBlock : TagStructure
            {
                public PlayerTraitsSensorsFloatFlags ShouldApplyTrait;
                public float MotionTrackerRange;
                public float NemesisDuration; // seconds
                public PlayerTraitMotionTracker MotionTracker;
                public PlayerTraitBoolEnum MotionTrackerWhileZoomed;
                public PlayerTraitBoolEnum DirectionalDamageIndicator;
                public PlayerTraitBoolEnum VisionMode;
                public PlayerTraitBoolEnum BattleAwareness;
                public PlayerTraitBoolEnum ThreatView;
                public PlayerTraitBoolEnum AuralEnhancement;
                public PlayerTraitBoolEnum Nemesis;
                
                [Flags]
                public enum PlayerTraitsSensorsFloatFlags : uint
                {
                    MotionTrackerRange = 1 << 0,
                    NemesisDuration = 1 << 1
                }
                
                public enum PlayerTraitMotionTracker : sbyte
                {
                    Unchanged,
                    Off,
                    MovingFriendlyBipedsMovingNeutralVehicles,
                    MovingBipedsMovingVehicles,
                    AllBipedsMovingVehicles
                }
                
                public enum PlayerTraitBoolEnum : sbyte
                {
                    Unchanged,
                    False,
                    True
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class GameEngineAiTraitsListBlock : TagStructure
        {
            public StringId Name;
            public GameEngineAiTraitsStruct AiTraits;
            
            [TagStructure(Size = 0xC)]
            public class GameEngineAiTraitsStruct : TagStructure
            {
                public AiTraitVisionSettings VisionTraits;
                public AiTraitSoundSettings SoundTraits;
                public AiTraitLuckSettings LuckTraits;
                public GlobalAiTraitWeaponSettings WeaponTraits;
                public AiTraitGrenadeSettings GrenadeTraits;
                public PlayerTraitBoolEnum DropEquipmentOnDeath;
                public PlayerTraitBoolEnum AssassinationImmunity;
                public PlayerTraitBoolEnum HeadshotImmunity;
                public PlayerTraitDamageResistance DamageResistancePercentage;
                public PlayerTraitDamageModifier DamageModifierPercentage;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                public enum AiTraitVisionSettings : sbyte
                {
                    Unchanged,
                    Normal,
                    Blind,
                    NearSighted,
                    EagleEye
                }
                
                public enum AiTraitSoundSettings : sbyte
                {
                    Unchanged,
                    Normal,
                    Deaf,
                    Sharp
                }
                
                public enum AiTraitLuckSettings : sbyte
                {
                    Unchanged,
                    Normal,
                    Unlucky,
                    Lucky,
                    Leprechaun
                }
                
                public enum GlobalAiTraitWeaponSettings : sbyte
                {
                    Unchanged,
                    Normal,
                    Marksman,
                    TriggerHappy
                }
                
                public enum AiTraitGrenadeSettings : sbyte
                {
                    Unchanged,
                    Normal,
                    None,
                    Catch
                }
                
                public enum PlayerTraitBoolEnum : sbyte
                {
                    Unchanged,
                    False,
                    True
                }
                
                public enum PlayerTraitDamageResistance : sbyte
                {
                    Unchanged,
                    _10Percent,
                    _50Percent,
                    _90Percent,
                    _100Percent,
                    _110Percent,
                    _150Percent,
                    _200Percent,
                    _300Percent,
                    _500Percent,
                    _1000Percent,
                    _2000Percent,
                    Invulnerable
                }
                
                public enum PlayerTraitDamageModifier : sbyte
                {
                    Unchanged,
                    _0Percent,
                    _25Percent,
                    _50Percent,
                    _75Percent,
                    _90Percent,
                    _100Percent,
                    _110Percent,
                    _125Percent,
                    _150Percent,
                    _200Percent,
                    _300Percent,
                    Fatality
                }
            }
        }
        
        [TagStructure(Size = 0x74)]
        public class GameEngineSandboxVariantBlock : TagStructure
        {
            public StringId LocalizableName;
            public StringId LocalizableDescription;
            public List<GameEngineMiscellaneousOptionsBlock> MiscellaneousOptions;
            public List<GameEnginePrototypeOptionsBlock> PrototypeOptions;
            public List<GameEngineRespawnOptionsBlock> RespawnOptions;
            public List<GameEngineSocialOptionsBlock> SocialOptions;
            public List<GameEngineMapOverrideOptionsBlock> MapOverrideOptions;
            public List<GameEngineTeamOptionsBlock> TeamOptions;
            public List<GameEngineLoadoutOptionsBlock> LoadoutOptions;
            public List<GameengineOrdnanceOptionsBlock> OrdnanceOptions;
            public SandboxFlags Flags;
            public SandboxEditingMode EditMode;
            public SandboxRespawnTime RespawnTime;
            public StringId AllPlayersTraits;
            
            [Flags]
            public enum SandboxFlags : uint
            {
                OpenChannelVoice = 1 << 0,
                RequiresAllObjects = 1 << 1
            }
            
            public enum SandboxEditingMode : short
            {
                AllPlayers,
                OnlyLeader
            }
            
            public enum SandboxRespawnTime : short
            {
                Instant,
                _3Seconds,
                _4Seconds,
                _5Seconds,
                _6Seconds,
                _7Seconds,
                _8Seconds,
                _9Seconds,
                _10Seconds,
                _15Seconds,
                _30Seconds,
                _60Seconds
            }
            
            [TagStructure(Size = 0x8)]
            public class GameEngineMiscellaneousOptionsBlock : TagStructure
            {
                public GameEngineMiscellaneousOptionsFlags Flags;
                public sbyte EarlyVictoryWinCount;
                public sbyte RoundTimeLimit; // minutes
                public sbyte NumberOfRounds;
                public MoshDifficulty MoshDifficultyLevel;
                public byte OvershieldDepleteTime;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum GameEngineMiscellaneousOptionsFlags : ushort
                {
                    TeamsEnabled = 1 << 0,
                    RoundResetPlayers = 1 << 1,
                    RoundResetMap = 1 << 2,
                    PerfectionEnabled = 1 << 3,
                    Mosh = 1 << 4,
                    DropWeaponsOnDeath = 1 << 5,
                    KillcamEnabled = 1 << 6,
                    MedalScoringEnabled = 1 << 7,
                    AsymmetricRoundScoring = 1 << 8
                }
                
                public enum MoshDifficulty : sbyte
                {
                    Easy,
                    Normal,
                    Heroic,
                    Legendary
                }
            }
            
            [TagStructure(Size = 0x6)]
            public class GameEnginePrototypeOptionsBlock : TagStructure
            {
                public sbyte PrototypeMode;
                public sbyte PrometheanEnergyKillPercent;
                public sbyte PrometheanEnergyTimePercent;
                public sbyte PrometheanEnergyMedalPercent;
                public sbyte PrometheanDuration;
                public sbyte ClassColorOverride;
            }
            
            [TagStructure(Size = 0x14)]
            public class GameEngineRespawnOptionsBlock : TagStructure
            {
                public GameEngineRespawnOptionsFlags Flags;
                public sbyte LivesPerRound;
                public sbyte TeamLivesPerRound;
                public sbyte MinRespawnTime; // seconds
                public sbyte RespawnTime; // seconds
                public sbyte SuicidePenalty; // seconds
                public sbyte BetrayalPenalty; // seconds
                public sbyte RespawnGrowth; // seconds
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public StringId RespawnPlayerTraitsName;
                // delay before spawning in at start of round
                public sbyte InitialLoadoutSelectionTime; // seconds
                public sbyte RespawnPlayerTraitsDuration; // seconds
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                
                [Flags]
                public enum GameEngineRespawnOptionsFlags : ushort
                {
                    InheritRespawnTime = 1 << 0,
                    RespawnWithTeammate = 1 << 1,
                    RespawnAtLocation = 1 << 2,
                    RespawnOnKills = 1 << 3,
                    EarlyRespawnAllowed = 1 << 4
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class GameEngineSocialOptionsBlock : TagStructure
            {
                public GameEngineSocialOptionsFlags Flags;
                
                [Flags]
                public enum GameEngineSocialOptionsFlags : uint
                {
                    ObserversEnabled = 1 << 0,
                    TeamChangingEnabled = 1 << 1,
                    TeamChangingBalancingOnly = 1 << 2,
                    FriendlyFireEnabled = 1 << 3,
                    BetrayalBootingEnabled = 1 << 4,
                    EnemyVoiceEnabled = 1 << 5,
                    OpenChannelVoiceEnabled = 1 << 6,
                    DeadPlayerVoiceEnabled = 1 << 7
                }
            }
            
            [TagStructure(Size = 0x3C)]
            public class GameEngineMapOverrideOptionsBlock : TagStructure
            {
                public StringId PlayerTraitsName;
                public StringId WeaponSetName;
                public StringId VehicleSetName;
                public StringId EquipmentSetName;
                public StringId RedPowerupTraitsName;
                public StringId BluePowerupTraitsName;
                public StringId YellowPowerupTraitsName;
                public StringId CustomPowerupTraitsName;
                public sbyte RedPowerupDuration; // seconds
                public sbyte BluePowerupDuration; // seconds
                public sbyte YellowPowerupDuration; // seconds
                public sbyte CustomPowerupDuration; // seconds
                public StringId RedPowerupSecondaryTraitsName;
                public StringId BluePowerupSecondaryTraitsName;
                public StringId YellowPowerupSecondaryTraitsName;
                public StringId CustomPowerupSecondaryTraitsName;
                public sbyte RedPowerupSecondaryDuration; // seconds
                public sbyte BluePowerupSecondaryDuration; // seconds
                public sbyte YellowPowerupSecondaryDuration; // seconds
                public sbyte CustomPowerupSecondaryDuration; // seconds
                public GameEngineMapOverrideOptionsFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum GameEngineMapOverrideOptionsFlags : byte
                {
                    GrenadesOnMap = 1 << 0,
                    ShortcutsOnMap = 1 << 1,
                    EquipmentOnMap = 1 << 2,
                    PowerupsOnMap = 1 << 3,
                    TurretsOnMap = 1 << 4,
                    IndestructibleVehicles = 1 << 5
                }
            }
            
            [TagStructure(Size = 0xC4)]
            public class GameEngineTeamOptionsBlock : TagStructure
            {
                public GameEngineTeamOptionsModelOverrideType ModelOverrideType;
                public GameEngineTeamOptionsDesignatorSwitchType DesignatorSwitchType;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(Length = 8)]
                public GameEngineTeamOptionsTeamBlock[]  Teams;
                
                public enum GameEngineTeamOptionsModelOverrideType : sbyte
                {
                    None,
                    ForceSpartan,
                    ForceElite,
                    SetByTeam,
                    SetByDesignator
                }
                
                public enum GameEngineTeamOptionsDesignatorSwitchType : sbyte
                {
                    None,
                    Random,
                    Rotate
                }
                
                [TagStructure(Size = 0x18)]
                public class GameEngineTeamOptionsTeamBlock : TagStructure
                {
                    public GameEngineTeamOptionsTeamFlags Flags;
                    public GlobalMultiplayerTeamDesignatorEnum InitialTeamDesignator;
                    public GameEngineTeamOptionsPlayerModelChoice ModelOverride;
                    public byte NumberOfFireteams;
                    public StringId Description;
                    public ArgbColor PrimaryColorOverride;
                    public ArgbColor SecondaryColorOverride;
                    public ArgbColor UiTextTintColorOverride;
                    public ArgbColor UiBitmapTintColorOverride;
                    
                    [Flags]
                    public enum GameEngineTeamOptionsTeamFlags : byte
                    {
                        Enabled = 1 << 0,
                        PrimaryOverrideColor = 1 << 1,
                        SecondaryOverrideColor = 1 << 2,
                        OverrideUiTextTintColor = 1 << 3,
                        OverrideUiBitmapTintColor = 1 << 4,
                        OverrideEmblem = 1 << 5
                    }
                    
                    public enum GlobalMultiplayerTeamDesignatorEnum : sbyte
                    {
                        Defender,
                        Attacker,
                        ThirdParty,
                        FourthParty,
                        FifthParty,
                        SixthParty,
                        SeventhParty,
                        EighthParty,
                        Neutral
                    }
                    
                    public enum GameEngineTeamOptionsPlayerModelChoice : sbyte
                    {
                        Spartan,
                        Elite
                    }
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class GameEngineLoadoutOptionsBlock : TagStructure
            {
                public LoadoutFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<GameEngineLoadoutPaletteEntryBlock> LoadoutPalettes;
                
                [Flags]
                public enum LoadoutFlags : byte
                {
                    CustomLoadoutsEnabled = 1 << 0,
                    SpartanLoadoutsEnabled = 1 << 1,
                    EliteLoadoutsEnabled = 1 << 2,
                    MapLoadoutsEnabled = 1 << 3
                }
                
                [TagStructure(Size = 0x4)]
                public class GameEngineLoadoutPaletteEntryBlock : TagStructure
                {
                    public StringId PaletteName;
                }
            }
            
            [TagStructure(Size = 0x1)]
            public class GameengineOrdnanceOptionsBlock : TagStructure
            {
                public GameengineOrdnanceOptionsFlags Flags;
                
                [Flags]
                public enum GameengineOrdnanceOptionsFlags : byte
                {
                    OrdnanceEnabled = 1 << 0
                }
            }
        }
        
        [TagStructure(Size = 0xD0)]
        public class GameEngineSurvivalVariantBlock : TagStructure
        {
            public StringId LocalizableName;
            public StringId LocalizableDescription;
            public List<GameEngineMiscellaneousOptionsBlock> MiscellaneousOptions;
            public List<GameEnginePrototypeOptionsBlock> PrototypeOptions;
            public List<GameEngineRespawnOptionsBlock> RespawnOptions;
            public List<GameEngineSocialOptionsBlock> SocialOptions;
            public List<GameEngineMapOverrideOptionsBlock> MapOverrideOptions;
            public List<GameEngineTeamOptionsBlock> TeamOptions;
            public List<GameEngineLoadoutOptionsBlock> LoadoutOptions;
            public List<GameengineOrdnanceOptionsBlock> OrdnanceOptions;
            public GameEngineSurvivalVariantFlags Flags;
            public GlobalCampaignDifficultyEnum GameDifficulty;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // survival_mode_get_set_count, -1 to loop last, -2 to loop last 3, -3 to loop all
            public sbyte SetCount;
            // survival_mode_get_bonus_lives_awarded
            public sbyte BonusLivesAwarded;
            // survival_mode_get_bonus_target
            public short BonusTarget;
            public short SpartanLivesOnEliteDeath;
            public short ExtraLifeScoreTarget;
            public short SharedTeamLifeCount;
            public short EliteLifeCount;
            public short MaximumLives;
            public short GeneratorCount;
            public StringId SpartanPlayerTraits;
            public StringId ElitePlayerTraits;
            public StringId AiTraits;
            public List<GameEngineRespawnOptionsBlock> EliteRespawnOptions;
            public List<GameEngineSurvivalSetPropertiesBlock> SetProperties;
            public List<GameEngineSurvivalRoundPropertiesBlock> RoundProperties;
            public GameEngineSurvivalBonusWavePropertiesStruct BonusRoundProperties;
            public List<GameEngineSurvivalCustomSkullBlock> CustomSkulls;
            
            [Flags]
            public enum GameEngineSurvivalVariantFlags : byte
            {
                EnableScenarioHazards = 1 << 0,
                GeneratorDefendAll = 1 << 1,
                GeneratorRandomSpawn = 1 << 2,
                EnableWeaponDrops = 1 << 3,
                EnableAmmoCrates = 1 << 4
            }
            
            public enum GlobalCampaignDifficultyEnum : sbyte
            {
                Easy,
                Normal,
                Heroic,
                Legendary
            }
            
            [TagStructure(Size = 0x8)]
            public class GameEngineMiscellaneousOptionsBlock : TagStructure
            {
                public GameEngineMiscellaneousOptionsFlags Flags;
                public sbyte EarlyVictoryWinCount;
                public sbyte RoundTimeLimit; // minutes
                public sbyte NumberOfRounds;
                public MoshDifficulty MoshDifficultyLevel;
                public byte OvershieldDepleteTime;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum GameEngineMiscellaneousOptionsFlags : ushort
                {
                    TeamsEnabled = 1 << 0,
                    RoundResetPlayers = 1 << 1,
                    RoundResetMap = 1 << 2,
                    PerfectionEnabled = 1 << 3,
                    Mosh = 1 << 4,
                    DropWeaponsOnDeath = 1 << 5,
                    KillcamEnabled = 1 << 6,
                    MedalScoringEnabled = 1 << 7,
                    AsymmetricRoundScoring = 1 << 8
                }
                
                public enum MoshDifficulty : sbyte
                {
                    Easy,
                    Normal,
                    Heroic,
                    Legendary
                }
            }
            
            [TagStructure(Size = 0x6)]
            public class GameEnginePrototypeOptionsBlock : TagStructure
            {
                public sbyte PrototypeMode;
                public sbyte PrometheanEnergyKillPercent;
                public sbyte PrometheanEnergyTimePercent;
                public sbyte PrometheanEnergyMedalPercent;
                public sbyte PrometheanDuration;
                public sbyte ClassColorOverride;
            }
            
            [TagStructure(Size = 0x14)]
            public class GameEngineRespawnOptionsBlock : TagStructure
            {
                public GameEngineRespawnOptionsFlags Flags;
                public sbyte LivesPerRound;
                public sbyte TeamLivesPerRound;
                public sbyte MinRespawnTime; // seconds
                public sbyte RespawnTime; // seconds
                public sbyte SuicidePenalty; // seconds
                public sbyte BetrayalPenalty; // seconds
                public sbyte RespawnGrowth; // seconds
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public StringId RespawnPlayerTraitsName;
                // delay before spawning in at start of round
                public sbyte InitialLoadoutSelectionTime; // seconds
                public sbyte RespawnPlayerTraitsDuration; // seconds
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                
                [Flags]
                public enum GameEngineRespawnOptionsFlags : ushort
                {
                    InheritRespawnTime = 1 << 0,
                    RespawnWithTeammate = 1 << 1,
                    RespawnAtLocation = 1 << 2,
                    RespawnOnKills = 1 << 3,
                    EarlyRespawnAllowed = 1 << 4
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class GameEngineSocialOptionsBlock : TagStructure
            {
                public GameEngineSocialOptionsFlags Flags;
                
                [Flags]
                public enum GameEngineSocialOptionsFlags : uint
                {
                    ObserversEnabled = 1 << 0,
                    TeamChangingEnabled = 1 << 1,
                    TeamChangingBalancingOnly = 1 << 2,
                    FriendlyFireEnabled = 1 << 3,
                    BetrayalBootingEnabled = 1 << 4,
                    EnemyVoiceEnabled = 1 << 5,
                    OpenChannelVoiceEnabled = 1 << 6,
                    DeadPlayerVoiceEnabled = 1 << 7
                }
            }
            
            [TagStructure(Size = 0x3C)]
            public class GameEngineMapOverrideOptionsBlock : TagStructure
            {
                public StringId PlayerTraitsName;
                public StringId WeaponSetName;
                public StringId VehicleSetName;
                public StringId EquipmentSetName;
                public StringId RedPowerupTraitsName;
                public StringId BluePowerupTraitsName;
                public StringId YellowPowerupTraitsName;
                public StringId CustomPowerupTraitsName;
                public sbyte RedPowerupDuration; // seconds
                public sbyte BluePowerupDuration; // seconds
                public sbyte YellowPowerupDuration; // seconds
                public sbyte CustomPowerupDuration; // seconds
                public StringId RedPowerupSecondaryTraitsName;
                public StringId BluePowerupSecondaryTraitsName;
                public StringId YellowPowerupSecondaryTraitsName;
                public StringId CustomPowerupSecondaryTraitsName;
                public sbyte RedPowerupSecondaryDuration; // seconds
                public sbyte BluePowerupSecondaryDuration; // seconds
                public sbyte YellowPowerupSecondaryDuration; // seconds
                public sbyte CustomPowerupSecondaryDuration; // seconds
                public GameEngineMapOverrideOptionsFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum GameEngineMapOverrideOptionsFlags : byte
                {
                    GrenadesOnMap = 1 << 0,
                    ShortcutsOnMap = 1 << 1,
                    EquipmentOnMap = 1 << 2,
                    PowerupsOnMap = 1 << 3,
                    TurretsOnMap = 1 << 4,
                    IndestructibleVehicles = 1 << 5
                }
            }
            
            [TagStructure(Size = 0xC4)]
            public class GameEngineTeamOptionsBlock : TagStructure
            {
                public GameEngineTeamOptionsModelOverrideType ModelOverrideType;
                public GameEngineTeamOptionsDesignatorSwitchType DesignatorSwitchType;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(Length = 8)]
                public GameEngineTeamOptionsTeamBlock[]  Teams;
                
                public enum GameEngineTeamOptionsModelOverrideType : sbyte
                {
                    None,
                    ForceSpartan,
                    ForceElite,
                    SetByTeam,
                    SetByDesignator
                }
                
                public enum GameEngineTeamOptionsDesignatorSwitchType : sbyte
                {
                    None,
                    Random,
                    Rotate
                }
                
                [TagStructure(Size = 0x18)]
                public class GameEngineTeamOptionsTeamBlock : TagStructure
                {
                    public GameEngineTeamOptionsTeamFlags Flags;
                    public GlobalMultiplayerTeamDesignatorEnum InitialTeamDesignator;
                    public GameEngineTeamOptionsPlayerModelChoice ModelOverride;
                    public byte NumberOfFireteams;
                    public StringId Description;
                    public ArgbColor PrimaryColorOverride;
                    public ArgbColor SecondaryColorOverride;
                    public ArgbColor UiTextTintColorOverride;
                    public ArgbColor UiBitmapTintColorOverride;
                    
                    [Flags]
                    public enum GameEngineTeamOptionsTeamFlags : byte
                    {
                        Enabled = 1 << 0,
                        PrimaryOverrideColor = 1 << 1,
                        SecondaryOverrideColor = 1 << 2,
                        OverrideUiTextTintColor = 1 << 3,
                        OverrideUiBitmapTintColor = 1 << 4,
                        OverrideEmblem = 1 << 5
                    }
                    
                    public enum GlobalMultiplayerTeamDesignatorEnum : sbyte
                    {
                        Defender,
                        Attacker,
                        ThirdParty,
                        FourthParty,
                        FifthParty,
                        SixthParty,
                        SeventhParty,
                        EighthParty,
                        Neutral
                    }
                    
                    public enum GameEngineTeamOptionsPlayerModelChoice : sbyte
                    {
                        Spartan,
                        Elite
                    }
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class GameEngineLoadoutOptionsBlock : TagStructure
            {
                public LoadoutFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<GameEngineLoadoutPaletteEntryBlock> LoadoutPalettes;
                
                [Flags]
                public enum LoadoutFlags : byte
                {
                    CustomLoadoutsEnabled = 1 << 0,
                    SpartanLoadoutsEnabled = 1 << 1,
                    EliteLoadoutsEnabled = 1 << 2,
                    MapLoadoutsEnabled = 1 << 3
                }
                
                [TagStructure(Size = 0x4)]
                public class GameEngineLoadoutPaletteEntryBlock : TagStructure
                {
                    public StringId PaletteName;
                }
            }
            
            [TagStructure(Size = 0x1)]
            public class GameengineOrdnanceOptionsBlock : TagStructure
            {
                public GameengineOrdnanceOptionsFlags Flags;
                
                [Flags]
                public enum GameengineOrdnanceOptionsFlags : byte
                {
                    OrdnanceEnabled = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class GameEngineSurvivalSetPropertiesBlock : TagStructure
            {
                public SkullFlags Skulls;
                
                [Flags]
                public enum SkullFlags : uint
                {
                    SkullIron = 1 << 0,
                    SkullBlackEye = 1 << 1,
                    SkullToughLuck = 1 << 2,
                    SkullCatch = 1 << 3,
                    SkullFog = 1 << 4,
                    SkullFamine = 1 << 5,
                    SkullThunderstorm = 1 << 6,
                    SkullTilt = 1 << 7,
                    SkullMythic = 1 << 8,
                    SkullAssassin = 1 << 9,
                    SkullBlind = 1 << 10,
                    SkullSuperman = 1 << 11,
                    SkullBirthdayParty = 1 << 12,
                    SkullDaddy = 1 << 13,
                    SkullRed = 1 << 14,
                    SkullYellow = 1 << 15,
                    SkullBlue = 1 << 16
                }
            }
            
            [TagStructure(Size = 0x34)]
            public class GameEngineSurvivalRoundPropertiesBlock : TagStructure
            {
                public SkullFlags Skulls;
                public GameEngineSurvivalWavePropertiesStruct InitialWaves;
                public GameEngineSurvivalWavePropertiesStruct PrimaryWaves;
                public GameEngineSurvivalWavePropertiesStruct BossWaves;
                
                [Flags]
                public enum SkullFlags : uint
                {
                    SkullIron = 1 << 0,
                    SkullBlackEye = 1 << 1,
                    SkullToughLuck = 1 << 2,
                    SkullCatch = 1 << 3,
                    SkullFog = 1 << 4,
                    SkullFamine = 1 << 5,
                    SkullThunderstorm = 1 << 6,
                    SkullTilt = 1 << 7,
                    SkullMythic = 1 << 8,
                    SkullAssassin = 1 << 9,
                    SkullBlind = 1 << 10,
                    SkullSuperman = 1 << 11,
                    SkullBirthdayParty = 1 << 12,
                    SkullDaddy = 1 << 13,
                    SkullRed = 1 << 14,
                    SkullYellow = 1 << 15,
                    SkullBlue = 1 << 16
                }
                
                [TagStructure(Size = 0x10)]
                public class GameEngineSurvivalWavePropertiesStruct : TagStructure
                {
                    public SurvivalWavePropertiesFlags Flags;
                    public SurvivalWaveSquadAdvanceTypeEnum WaveSelectionType;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public List<SurvivalWaveSquadBlock> WaveSquads;
                    
                    [Flags]
                    public enum SurvivalWavePropertiesFlags : byte
                    {
                        DeliveredViaDropship = 1 << 0
                    }
                    
                    public enum SurvivalWaveSquadAdvanceTypeEnum : sbyte
                    {
                        Random,
                        Sequence
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class SurvivalWaveSquadBlock : TagStructure
                    {
                        // survival_mode_get_wave_squad
                        public StringId SquadType;
                    }
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class GameEngineSurvivalBonusWavePropertiesStruct : TagStructure
            {
                public SkullFlags Skulls;
                public short Duration; // s
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public GameEngineSurvivalWavePropertiesStruct BaseProperties;
                
                [Flags]
                public enum SkullFlags : uint
                {
                    SkullIron = 1 << 0,
                    SkullBlackEye = 1 << 1,
                    SkullToughLuck = 1 << 2,
                    SkullCatch = 1 << 3,
                    SkullFog = 1 << 4,
                    SkullFamine = 1 << 5,
                    SkullThunderstorm = 1 << 6,
                    SkullTilt = 1 << 7,
                    SkullMythic = 1 << 8,
                    SkullAssassin = 1 << 9,
                    SkullBlind = 1 << 10,
                    SkullSuperman = 1 << 11,
                    SkullBirthdayParty = 1 << 12,
                    SkullDaddy = 1 << 13,
                    SkullRed = 1 << 14,
                    SkullYellow = 1 << 15,
                    SkullBlue = 1 << 16
                }
                
                [TagStructure(Size = 0x10)]
                public class GameEngineSurvivalWavePropertiesStruct : TagStructure
                {
                    public SurvivalWavePropertiesFlags Flags;
                    public SurvivalWaveSquadAdvanceTypeEnum WaveSelectionType;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public List<SurvivalWaveSquadBlock> WaveSquads;
                    
                    [Flags]
                    public enum SurvivalWavePropertiesFlags : byte
                    {
                        DeliveredViaDropship = 1 << 0
                    }
                    
                    public enum SurvivalWaveSquadAdvanceTypeEnum : sbyte
                    {
                        Random,
                        Sequence
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class SurvivalWaveSquadBlock : TagStructure
                    {
                        // survival_mode_get_wave_squad
                        public StringId SquadType;
                    }
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class GameEngineSurvivalCustomSkullBlock : TagStructure
            {
                public StringId SpartanPlayerTraits;
                public StringId ElitePlayerTraits;
                public StringId AiTraits;
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class GameEngineFirefightVariantShellBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "ffgt" })]
            public CachedTag Variant;
        }
        
        [TagStructure(Size = 0x68)]
        public class GameEngineCampaignVariantBlock : TagStructure
        {
            public StringId LocalizableName;
            public StringId LocalizableDescription;
            public List<GameEngineMiscellaneousOptionsBlock> MiscellaneousOptions;
            public List<GameEnginePrototypeOptionsBlock> PrototypeOptions;
            public List<GameEngineRespawnOptionsBlock> RespawnOptions;
            public List<GameEngineSocialOptionsBlock> SocialOptions;
            public List<GameEngineMapOverrideOptionsBlock> MapOverrideOptions;
            public List<GameEngineTeamOptionsBlock> TeamOptions;
            public List<GameEngineLoadoutOptionsBlock> LoadoutOptions;
            public List<GameengineOrdnanceOptionsBlock> OrdnanceOptions;
            
            [TagStructure(Size = 0x8)]
            public class GameEngineMiscellaneousOptionsBlock : TagStructure
            {
                public GameEngineMiscellaneousOptionsFlags Flags;
                public sbyte EarlyVictoryWinCount;
                public sbyte RoundTimeLimit; // minutes
                public sbyte NumberOfRounds;
                public MoshDifficulty MoshDifficultyLevel;
                public byte OvershieldDepleteTime;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum GameEngineMiscellaneousOptionsFlags : ushort
                {
                    TeamsEnabled = 1 << 0,
                    RoundResetPlayers = 1 << 1,
                    RoundResetMap = 1 << 2,
                    PerfectionEnabled = 1 << 3,
                    Mosh = 1 << 4,
                    DropWeaponsOnDeath = 1 << 5,
                    KillcamEnabled = 1 << 6,
                    MedalScoringEnabled = 1 << 7,
                    AsymmetricRoundScoring = 1 << 8
                }
                
                public enum MoshDifficulty : sbyte
                {
                    Easy,
                    Normal,
                    Heroic,
                    Legendary
                }
            }
            
            [TagStructure(Size = 0x6)]
            public class GameEnginePrototypeOptionsBlock : TagStructure
            {
                public sbyte PrototypeMode;
                public sbyte PrometheanEnergyKillPercent;
                public sbyte PrometheanEnergyTimePercent;
                public sbyte PrometheanEnergyMedalPercent;
                public sbyte PrometheanDuration;
                public sbyte ClassColorOverride;
            }
            
            [TagStructure(Size = 0x14)]
            public class GameEngineRespawnOptionsBlock : TagStructure
            {
                public GameEngineRespawnOptionsFlags Flags;
                public sbyte LivesPerRound;
                public sbyte TeamLivesPerRound;
                public sbyte MinRespawnTime; // seconds
                public sbyte RespawnTime; // seconds
                public sbyte SuicidePenalty; // seconds
                public sbyte BetrayalPenalty; // seconds
                public sbyte RespawnGrowth; // seconds
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public StringId RespawnPlayerTraitsName;
                // delay before spawning in at start of round
                public sbyte InitialLoadoutSelectionTime; // seconds
                public sbyte RespawnPlayerTraitsDuration; // seconds
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                
                [Flags]
                public enum GameEngineRespawnOptionsFlags : ushort
                {
                    InheritRespawnTime = 1 << 0,
                    RespawnWithTeammate = 1 << 1,
                    RespawnAtLocation = 1 << 2,
                    RespawnOnKills = 1 << 3,
                    EarlyRespawnAllowed = 1 << 4
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class GameEngineSocialOptionsBlock : TagStructure
            {
                public GameEngineSocialOptionsFlags Flags;
                
                [Flags]
                public enum GameEngineSocialOptionsFlags : uint
                {
                    ObserversEnabled = 1 << 0,
                    TeamChangingEnabled = 1 << 1,
                    TeamChangingBalancingOnly = 1 << 2,
                    FriendlyFireEnabled = 1 << 3,
                    BetrayalBootingEnabled = 1 << 4,
                    EnemyVoiceEnabled = 1 << 5,
                    OpenChannelVoiceEnabled = 1 << 6,
                    DeadPlayerVoiceEnabled = 1 << 7
                }
            }
            
            [TagStructure(Size = 0x3C)]
            public class GameEngineMapOverrideOptionsBlock : TagStructure
            {
                public StringId PlayerTraitsName;
                public StringId WeaponSetName;
                public StringId VehicleSetName;
                public StringId EquipmentSetName;
                public StringId RedPowerupTraitsName;
                public StringId BluePowerupTraitsName;
                public StringId YellowPowerupTraitsName;
                public StringId CustomPowerupTraitsName;
                public sbyte RedPowerupDuration; // seconds
                public sbyte BluePowerupDuration; // seconds
                public sbyte YellowPowerupDuration; // seconds
                public sbyte CustomPowerupDuration; // seconds
                public StringId RedPowerupSecondaryTraitsName;
                public StringId BluePowerupSecondaryTraitsName;
                public StringId YellowPowerupSecondaryTraitsName;
                public StringId CustomPowerupSecondaryTraitsName;
                public sbyte RedPowerupSecondaryDuration; // seconds
                public sbyte BluePowerupSecondaryDuration; // seconds
                public sbyte YellowPowerupSecondaryDuration; // seconds
                public sbyte CustomPowerupSecondaryDuration; // seconds
                public GameEngineMapOverrideOptionsFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum GameEngineMapOverrideOptionsFlags : byte
                {
                    GrenadesOnMap = 1 << 0,
                    ShortcutsOnMap = 1 << 1,
                    EquipmentOnMap = 1 << 2,
                    PowerupsOnMap = 1 << 3,
                    TurretsOnMap = 1 << 4,
                    IndestructibleVehicles = 1 << 5
                }
            }
            
            [TagStructure(Size = 0xC4)]
            public class GameEngineTeamOptionsBlock : TagStructure
            {
                public GameEngineTeamOptionsModelOverrideType ModelOverrideType;
                public GameEngineTeamOptionsDesignatorSwitchType DesignatorSwitchType;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(Length = 8)]
                public GameEngineTeamOptionsTeamBlock[]  Teams;
                
                public enum GameEngineTeamOptionsModelOverrideType : sbyte
                {
                    None,
                    ForceSpartan,
                    ForceElite,
                    SetByTeam,
                    SetByDesignator
                }
                
                public enum GameEngineTeamOptionsDesignatorSwitchType : sbyte
                {
                    None,
                    Random,
                    Rotate
                }
                
                [TagStructure(Size = 0x18)]
                public class GameEngineTeamOptionsTeamBlock : TagStructure
                {
                    public GameEngineTeamOptionsTeamFlags Flags;
                    public GlobalMultiplayerTeamDesignatorEnum InitialTeamDesignator;
                    public GameEngineTeamOptionsPlayerModelChoice ModelOverride;
                    public byte NumberOfFireteams;
                    public StringId Description;
                    public ArgbColor PrimaryColorOverride;
                    public ArgbColor SecondaryColorOverride;
                    public ArgbColor UiTextTintColorOverride;
                    public ArgbColor UiBitmapTintColorOverride;
                    
                    [Flags]
                    public enum GameEngineTeamOptionsTeamFlags : byte
                    {
                        Enabled = 1 << 0,
                        PrimaryOverrideColor = 1 << 1,
                        SecondaryOverrideColor = 1 << 2,
                        OverrideUiTextTintColor = 1 << 3,
                        OverrideUiBitmapTintColor = 1 << 4,
                        OverrideEmblem = 1 << 5
                    }
                    
                    public enum GlobalMultiplayerTeamDesignatorEnum : sbyte
                    {
                        Defender,
                        Attacker,
                        ThirdParty,
                        FourthParty,
                        FifthParty,
                        SixthParty,
                        SeventhParty,
                        EighthParty,
                        Neutral
                    }
                    
                    public enum GameEngineTeamOptionsPlayerModelChoice : sbyte
                    {
                        Spartan,
                        Elite
                    }
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class GameEngineLoadoutOptionsBlock : TagStructure
            {
                public LoadoutFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<GameEngineLoadoutPaletteEntryBlock> LoadoutPalettes;
                
                [Flags]
                public enum LoadoutFlags : byte
                {
                    CustomLoadoutsEnabled = 1 << 0,
                    SpartanLoadoutsEnabled = 1 << 1,
                    EliteLoadoutsEnabled = 1 << 2,
                    MapLoadoutsEnabled = 1 << 3
                }
                
                [TagStructure(Size = 0x4)]
                public class GameEngineLoadoutPaletteEntryBlock : TagStructure
                {
                    public StringId PaletteName;
                }
            }
            
            [TagStructure(Size = 0x1)]
            public class GameengineOrdnanceOptionsBlock : TagStructure
            {
                public GameengineOrdnanceOptionsFlags Flags;
                
                [Flags]
                public enum GameengineOrdnanceOptionsFlags : byte
                {
                    OrdnanceEnabled = 1 << 0
                }
            }
        }
    }
}
