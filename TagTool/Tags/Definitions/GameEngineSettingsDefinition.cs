using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "game_engine_settings_definition", Tag = "wezr", Size = 0x88, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "game_engine_settings_definition", Tag = "wezr", Size = 0x8C, MinVersion = CacheVersion.HaloOnline106708)]
    public class GameEngineSettingsDefinition : TagStructure
	{
        public FlagsValue Flags;
        public List<TraitProfile> TraitProfiles;
        public List<SlayerVariant> SlayerVariants;
        public List<OddballVariant> OddballVariants;
        public List<CaptureTheFlagVariant> CaptureTheFlagVariants;
        public List<AssaultVariant> AssaultVariants;
        public List<InfectionVariant> InfectionVariants;
        public List<KingOfTheHillVariant> KingOfTheHillVariants;
        public List<TerritoriesVariant> TerritoriesVariants;
        public List<JuggernautVariant> JuggernautVariants;
        public List<VipVariant> VipVariants;
        public List<SandboxEditorVariant> SandboxEditorVariants;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown2;

        [Flags]
        public enum FlagsValue : int
        {
            None,
            Unused = 1 << 0
        }

        [TagStructure(Size = 0x40)]
        public class TraitProfile : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;
            public List<ShieldsAndHealthBlock> ShieldsAndHealth;
            public List<WeaponsAndDamageBlock> WeaponsAndDamage;
            public List<MovementBlock> Movement;
            public List<AppearanceBlock> Appearance;
            public List<Sensor> Sensors;

            [TagStructure(Size = 0x8)]
            public class ShieldsAndHealthBlock : TagStructure
			{
                public DamageResistanceValue DamageResistance;
                public ShieldMultiplierValue ShieldMultiplier;
                public ShieldRechargeRateValue ShieldRechargeRate;
                public HeadshotImmunityValue HeadshotImmunity;
                public ShieldVampirismValue ShieldVampirism;
                public sbyte Unknown;
                public sbyte Unknown2;
                public sbyte Unknown3;

                public enum DamageResistanceValue : sbyte
                {
                    Unchanged,
                    _10,
                    _50,
                    _90,
                    _100,
                    _110,
                    _150,
                    _200,
                    _300,
                    _500,
                    _1000,
                    _2000,
                    Invulnerable
                }

                public enum ShieldMultiplierValue : sbyte
                {
                    Unchanged,
                    NoShields,
                    NormalShields,
                    _2xOvershields,
                    _3xOvershields,
                    _4xOvershields
                }

                public enum ShieldRechargeRateValue : sbyte
                {
                    Unchanged,
                    _25,
                    _10,
                    _5,
                    _0,
                    _50,
                    _90,
                    _100,
                    _110,
                    _200
                }

                public enum HeadshotImmunityValue : sbyte
                {
                    Unchanged,
                    Enabled,
                    Disabled
                }

                public enum ShieldVampirismValue : sbyte
                {
                    Unchanged,
                    Disabled,
                    _10,
                    _25,
                    _50,
                    _100
                }
            }

            [TagStructure(Size = 0x10)]
            public class WeaponsAndDamageBlock : TagStructure
			{
                public DamageModifierValue DamageModifier;
                public GrenadeRegenerationValue GrenadeRegeneration;
                public WeaponPickupValue WeaponPickup;
                public InfiniteAmmoValue InfiniteAmmo;
                public StringId PrimaryWeapon;
                public StringId SecondaryWeapon;
                public GrenadeCountValue GrenadeCount;
                public sbyte Unknown;
                public sbyte Unknown2;

                public enum DamageModifierValue : sbyte
                {
                    Unchanged,
                    _0,
                    _25,
                    _50,
                    _75,
                    _90,
                    _100,
                    _110,
                    _125,
                    _150,
                    _200,
                    _300,
                    InstantKill
                }

                public enum GrenadeRegenerationValue : sbyte
                {
                    Unchanged,
                    Enabled,
                    Disabled
                }

                public enum WeaponPickupValue : sbyte
                {
                    Unchanged,
                    Enabled,
                    Disabled
                }

                public enum InfiniteAmmoValue : sbyte
                {
                    Unchanged,
                    Disabled,
                    Enabled
                }

                public enum GrenadeCountValue : short
                {
                    Unchanged,
                    MapDefault,
                    None
                }
            }

            [TagStructure(Size = 0x4)]
            public class MovementBlock : TagStructure
			{
                public PlayerSpeedValue PlayerSpeed;
                public PlayerGravityValue PlayerGravity;
                public VehicleUseValue VehicleUse;
                public sbyte Unknown;

                public enum PlayerSpeedValue : sbyte
                {
                    Unchanged,
                    _25,
                    _50,
                    _75,
                    _90,
                    _100,
                    _110,
                    _125,
                    _150,
                    _200,
                    _300
                }

                public enum PlayerGravityValue : sbyte
                {
                    Unchanged,
                    _50,
                    _75,
                    _100,
                    _150,
                    _200
                }

                public enum VehicleUseValue : sbyte
                {
                    Unchanged,
                    None,
                    PassengerOnly,
                    FullUse
                }
            }

            [TagStructure(Size = 0x4)]
            public class AppearanceBlock : TagStructure
			{
                public ActiveCamoValue ActiveCamo;
                public WaypointValue Waypoint;
                public AuraValue Aura;
                public ForcedColorValue ForcedColor;

                public enum ActiveCamoValue : sbyte
                {
                    Unchanged,
                    Disabled,
                    BadCamo,
                    PoorCamo,
                    GoodCamo
                }

                public enum WaypointValue : sbyte
                {
                    Unchanged,
                    None,
                    VisibleToAllies,
                    VisibleToEveryone
                }

                public enum AuraValue : sbyte
                {
                    Unchanged,
                    Disabled,
                    Team,
                    Black,
                    White
                }

                public enum ForcedColorValue : sbyte
                {
                    Unchanged,
                    Off,
                    Red,
                    Blue,
                    Green,
                    Orange,
                    Purple,
                    Gold,
                    Brown,
                    Pink,
                    White,
                    Black,
                    Zombie,
                    PinkUnused
                }
            }

            [TagStructure(Size = 0x8)]
            public class Sensor : TagStructure
			{
                public MotionTrackerModeValue MotionTrackerMode;
                public MotionTrackerRangeValue MotionTrackerRange;

                public enum MotionTrackerModeValue : int
                {
                    Unchanged,
                    Disabled,
                    AllyMovement,
                    PlayerMovement,
                    PlayerLocations
                }

                public enum MotionTrackerRangeValue : int
                {
                    Unchanged,
                    _10m,
                    _15m,
                    _25m,
                    _50m,
                    _75m,
                    _100m,
                    _150m
                }
            }
        }

        [TagStructure(Size = 0x38, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x58, MinVersion = CacheVersion.HaloOnline106708)]
        public abstract class BaseVariant : TagStructure
		{
            [TagField(Flags = Label, Length = 32, MinVersion = CacheVersion.HaloOnline106708)]
            public string NameAscii;
            [TagField(Flags = Label)]
            public StringId Name;
            public StringId Description;
            public List<GeneralSetting> GeneralSettings;
            public List<RespawnSetting> RespawnSettings;
            public List<SocialSetting> SocialSettings;
            public List<MapOverride> MapOverrides;

            [TagStructure(Size = 0x8)]
            public class GeneralSetting : TagStructure
			{
                public FlagsValue Flags;
                public sbyte TimeLimit;
                public sbyte NumberOfRounds;
                public sbyte EarlyVictoryWinCount;
                public RoundResetsValue RoundResets;

                [Flags]
                public enum FlagsValue : int
                {
                    None,
                    TeamsEnabled = 1 << 0,
                    RoundResetsPlayers = 1 << 1,
                    RoundResetsMap = 1 << 2
                }

                public enum RoundResetsValue : sbyte
                {
                    Nothing,
                    PlayersOnly,
                    Everything,
                }
            }

            [TagStructure(Size = 0x10, MaxVersion = CacheVersion.Halo3ODST)]
            [TagStructure(Size = 0x14, MinVersion = CacheVersion.HaloOnline106708)]
            public class RespawnSetting : TagStructure
			{
                public FlagsValue Flags;
                public sbyte LivesPerRound;
                public sbyte SharedTeamLives;

                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public byte Unknown;

                public byte RespawnTime;
                public byte SuicidePenalty;
                public byte BetrayalPenalty;
                public byte RespawnTimeGrowth;

                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public sbyte Unknown1;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public sbyte Unknown2;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public sbyte Unknown3;

                public StringId RespawnTraitProfile;
                public sbyte RespawnTraitDuration;
                public sbyte Unknown4;
                public sbyte Unknown5;
                public sbyte Unknown6;

                [Flags]
                public enum FlagsValue : ushort
                {
                    None,
                    InheritRespawnTime = 1 << 0,
                    RespawnWithTeam = 1 << 1,
                    RespawnAtLocation = 1 << 2,
                    RespawnOnKills = 1 << 3
                }
            }

            [TagStructure(Size = 0x4)]
            public class SocialSetting : TagStructure
			{
                public FlagsValue Flags;

                [Flags]
                public enum FlagsValue : int
                {
                    None,
                    ObserversEnabled = 1 << 0,
                    TeamChangingEnabled = 1 << 1,
                    BalancedTeamChanging = 1 << 2,
                    FriendlyFireEnabled = 1 << 3,
                    BetrayalBootingEnabled = 1 << 4,
                    EnemyVoiceEnabled = 1 << 5,
                    OpenChannelVoiceEnabled = 1 << 6,
                    DeadPlayerVoiceEnabled = 1 << 7
                }
            }

            [TagStructure(Size = 0x20)]
            public class MapOverride : TagStructure
			{
                public FlagsValue Flags;
                public StringId BasePlayerTraitProfile;
                public StringId WeaponSet;
                public StringId VehicleSet;
                public StringId OvershieldTraitProfile;
                public StringId ActiveCamoTraitProfile;
                public StringId CustomPowerupTraitProfile;
                public sbyte OvershieldTraitDuration;
                public sbyte ActiveCamoTraitDuration;
                public sbyte CustomPowerupTraitDuration;
                public sbyte Unknown;

                [Flags]
                public enum FlagsValue : int
                {
                    None,
                    GrenadesOnMap,
                    IndestructableVehicles
                }
            }
        }

        [TagStructure(Size = 0x18)]
        public class SlayerVariant : BaseVariant
        {
            public TeamScoringValue TeamScoring;
            public short PointsToWin;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public short Unknown;
            public sbyte KillPoints;
            public sbyte AssistPoints;
            public sbyte DeathPoints;
            public sbyte SuicidePoints;
            public sbyte BetrayalPoints;
            public sbyte LeaderKillBonus;
            public sbyte EliminationBonus;
            public sbyte AssassinationBonus;
            public sbyte HeadshotBonus;
            public sbyte BeatdownBonus;
            public sbyte StickyBonus;
            public sbyte SplatterBonus;
            public sbyte SpreeBonus;
            public sbyte Unknown2;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public sbyte Unknown3;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public sbyte Unknown4;
            public StringId LeaderTraitProfile;

            public enum TeamScoringValue : short
            {
                SumOfTeam,
                MinimumScore,
                MaximumScore,
                Default
            }
        }

        [TagStructure(Size = 0x18)]
        public class OddballVariant : BaseVariant
        {
            public FlagsValue Flags;
            public TeamScoringValue TeamScoring;
            public short PointsToWin;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public short Unknown;
            public sbyte CarryingPoints;
            public sbyte KillPoints;
            public sbyte BallKillPoints;
            public sbyte BallCarrierKillPoints;
            public sbyte BallCount;
            public sbyte Unknown2;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public sbyte Unknown3;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public sbyte Unknown4;
            public short InitialBallDelay;
            public short BallRespawnDelay;
            public StringId BallCarrierTraitProfile;

            [Flags]
            public enum FlagsValue : int
            {
                None,
                AutopickupEnabled = 1 << 0,
                BallEffectEnabled = 1 << 1
            }

            public enum TeamScoringValue : short
            {
                SumOfTeam,
                MinimumScore,
                MaximumScore,
                Default
            }
        }

        [TagStructure(Size = 0x18)]
        public class CaptureTheFlagVariant : BaseVariant
        {
            public FlagsValue Flags;
            public HomeFlagWaypointValue HomeFlagWaypoint;
            public GameModeValue GameMode;
            public RespawnOnCaptureValue RespawnOnCapture;
            public short FlagReturnTime;
            public short SuddenDeathTime;
            public short ScoreToWin;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public short Unknown;
            public short FlagResetTime;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public sbyte Unknown1;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public sbyte Unknown2;
            public StringId FlagCarrierTraitProfile;

            [Flags]
            public enum FlagsValue : int
            {
                None,
                FlagAtHomeToScore = 1 << 0
            }

            public enum HomeFlagWaypointValue : short
            {
                Unknown1,
                Unknown2,
                Unknown3,
                NotInSingle
            }

            public enum GameModeValue : short
            {
                Multiple,
                Single,
                Neutral
            }

            public enum RespawnOnCaptureValue : short
            {
                Disabled,
                OnAllyCapture,
                OnEnemyCapture,
                OnAnyCapture
            }
        }

        [TagStructure(Size = 0x20, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x28, MinVersion = CacheVersion.HaloOnline106708)]
        public class AssaultVariant : BaseVariant
        {
            public FlagsValue Flags;
            public RespawnOnCaptureValue RespawnOnCapture;
            public GameModeValue GameMode;
            public EnemyBombWaypointValue EnemyBombWaypoint;
            public short SuddenDeathTime;
            public short DetonationsToWin;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public short Unknown;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public short Unknown2;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public short Unknown3;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public short Unknown4;
            public short BombResetTime;
            public short BombArmingTime;
            public short BombDisarmingTime;
            public short BombFuseTime;
            public short Unknown5;
            public StringId BombCarrierTraitProfile;
            public StringId UnknownTraitProfile;

            [Flags]
            public enum FlagsValue : int
            {
                None,
                ResetOnDisarm = 1 << 0
            }

            public enum RespawnOnCaptureValue : short
            {
                Disabled,
                OnAllyCapture,
                OnEnemyCapture,
                OnAnyCapture
            }

            public enum GameModeValue : short
            {
                Multiple,
                Single,
                Neutral
            }

            public enum EnemyBombWaypointValue : short
            {
                Unknown1,
                Unknown2,
                Unknown3,
                NotInSingle
            }
        }

        [TagStructure(Size = 0x24)]
        public class InfectionVariant : BaseVariant
        {
            public FlagsValue Flags;
            public SafeHavensValue SafeHavens;
            public NextZombieValue NextZombie;
            public short InitialZombieCount;
            public short SafeHavenMovementTime;
            public sbyte ZombieKillPoints;
            public sbyte InfectionPoints;
            public sbyte SafeHavenArrivalPoints;
            public sbyte SuicidePoints;
            public sbyte BetrayalPoints;
            public sbyte LastManStandingBonus;
            public sbyte Unknown;
            public sbyte Unknown2;
            public StringId ZombieTraitProfile;
            public StringId AlphaZombieTraitProfile;
            public StringId OnHavenTraitProfile;
            public StringId LastHumanTraitProfile;

            [Flags]
            public enum FlagsValue : int
            {
                None,
                RespawnOnHavenMove = 1 << 0
            }

            public enum SafeHavensValue : short
            {
                None,
                Random,
                Sequence
            }

            public enum NextZombieValue : short
            {
                MostPoints,
                FirstInfected,
                Unchanged,
                Random
            }
        }

        [TagStructure(Size = 0x14, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x18, MinVersion = CacheVersion.HaloOnline106708)]
        public class KingOfTheHillVariant : BaseVariant
        {
            public FlagsValue Flags;
            public short ScoreToWin;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public short Unknown;
            public TeamScoringValue TeamScoring;
            public HillMovementValue HillMovement;
            public HillMovementOrderValue HillMovementOrder;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public short Unknown2;
            public sbyte OnHillPoints;
            public sbyte UncontestedControlPoints;
            public sbyte OffHillPoints;
            public sbyte KillPoints;
            public StringId OnHillTraitProfile;

            [Flags]
            public enum FlagsValue : int
            {
                None,
                OpaqueHill = 1 << 0
            }

            public enum TeamScoringValue : short
            {
                Sum,
                Minimum,
                Maximum,
                Default
            }

            public enum HillMovementValue : short
            {
                NoMovement,
                After10Seconds,
                After15Seconds,
                After30Seconds,
                After1Minute,
                After2Minutes,
                After3Minutes,
                After4Minutes,
                After5Minutes
            }

            public enum HillMovementOrderValue : short
            {
                Random,
                Sequence
            }
        }

        [TagStructure(Size = 0x14)]
        public class TerritoriesVariant : BaseVariant
        {
            public FlagsValue Flags;
            public RespawnOnCaptureValue RespawnOnCapture;
            public short TerritoryCaptureTime;
            public short SuddenDeathTime;
            public short Unknown;
            public StringId DefenderTraitProfile;
            public StringId AttackerTraitProfile;

            [Flags]
            public enum FlagsValue : int
            {
                None,
                OneSided = 1 << 0,
                LockAfterFirstCapture = 1 << 1
            }

            public enum RespawnOnCaptureValue : short
            {
                Disabled,
                OnAllyCapture,
                OnEnemyCapture,
                OnAnyCapture
            }
        }

        [TagStructure(Size = 0x1C)]
        public class JuggernautVariant : BaseVariant
        {
            public FlagsValue Flags;
            public FirstJuggernautValue FirstJuggernaut;
            public NextJuggernautValue NextJuggernaut;
            public GoalZoneMovementValue GoalZoneMovement;
            public GoalZoneOrderValue GoalZoneOrder;
            public short ScoreToWin;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public short Unknown;
            public sbyte KillPoints;
            public sbyte TakedownPoints;
            public sbyte KillAsJuggernautPoints;
            public sbyte GoalArrivalPoints;
            public sbyte SuicidePoints;
            public sbyte BetrayalPoints;
            public sbyte NextJuggernautDelay;
            public sbyte Unknown2;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public sbyte Unknown3;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public sbyte Unknown4;
            public StringId JuggernautTraitProfile;

            [Flags]
            public enum FlagsValue : int
            {
                None,
                AlliedAgainstJuggernaut = 1 << 0,
                RespawnOnLoneJuggernaut = 1 << 1,
                GoalZonesEnabled = 1 << 2
            }

            public enum FirstJuggernautValue : short
            {
                Random,
                FirstKill,
                FirstDeath,
            }

            public enum NextJuggernautValue : short
            {
                Killer,
                Killed,
                Unchanged,
                Random,
            }

            public enum GoalZoneMovementValue : short
            {
                NoMovement,
                After10Seconds,
                After15Seconds,
                After30Seconds,
                After1Minute,
                After2Minutes,
                After3Minutes,
                After4Minutes,
                After5Minutes,
                OnArrival,
                OnNewJuggernaut,
            }

            public enum GoalZoneOrderValue : short
            {
                Random,
                Sequence,
            }
        }

        [TagStructure(Size = 0x24)]
        public class VipVariant : BaseVariant
        {
            public FlagsValue Flags;
            public short ScoreToWin;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public short Unknown;
            public NextVipValue NextVip;
            public GoalZoneMovementValue GoalZoneMovement;
            public GoalZoneMovementOrderValue GoalZoneMovementOrder;
            public sbyte KillPoints;
            public sbyte VipTakedownPoints;
            public sbyte KillAsVipPoints;
            public sbyte VipDeathPoints;
            public sbyte GoalArrivalPoints;
            public sbyte SuicidePoints;
            public sbyte VipBetrayalPoints;
            public sbyte BetrayalPoints;
            public sbyte VipProximityTraitRadius;
            public sbyte Unknown2;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public sbyte Unknown3;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public sbyte Unknown4;
            public StringId VipTeamTraitProfile;
            public StringId VipProximityTraitProfile;
            public StringId VipTraitProfile;

            [Flags]
            public enum FlagsValue : int
            {
                None,
                SingleVip = 1 << 0,
                GoalZonesEnabled = 1 << 1,
                EndRoundOnVipDeath = 1 << 2
            }

            public enum NextVipValue : short
            {
                Random,
                Unknown,
                NextDeath,
                Unchanged,
            }

            public enum GoalZoneMovementValue : short
            {
                NoMovement,
                After10Seconds,
                After15Seconds,
                After30Seconds,
                After1Minute,
                After2Minutes,
                After3Minutes,
                After4Minutes,
                After5Minutes,
                OnArrival,
                OnNewVip,
            }

            public enum GoalZoneMovementOrderValue : short
            {
                Random,
                Sequence,
            }
        }

        [TagStructure(Size = 0xC)]
        public class SandboxEditorVariant : BaseVariant
        {
            public FlagsValue Flags;
            public EditModeValue EditMode;
            public short EditorRespawnTime;
            public StringId EditorTraitProfile;

            [Flags]
            public enum FlagsValue : int
            {
                None,
                OpenChannelVoiceEnabled = 1 << 0
            }

            public enum EditModeValue : short
            {
                Everyone,
                LeaderOnly
            }
        }
    }
}