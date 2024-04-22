using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "incident_global_properties_definition", Tag = "igpd", Size = 0x88)]
    public class IncidentGlobalPropertiesDefinition : TagStructure
    {
        public float CampaignMultikillTime; // s
        public float SurvivalMultikillTime; // s
        public float MultiplayerMultikillTime; // s
        public float LowHealthThreshold; // [0,1]
        public float ShieldRechargeThreshold; // [0,1]
        public float MaximumVengeanceTime; // s
        public float LifesaverDamageThreshold; // [0,2]
        public float AvengerDeadTime; // s
        public float HologramRecentlyUsedMaximumTime; // s
        public ActiveCamoEnum ActiveCamouflageIncidentMinimumLevel;
        public float ThrusterPackRecentlyUsedMaximumTime; // s
        public float ActiveShieldRecentlyUsedMaximumTime; // s
        public float DamageThresholdForHologramIncidents; // [0,1]
        public float DamageThresholdForDistractionIncidentKiller; // [0,1]
        public float DamageThresholdForDistractionIncidentDistractor; // [0,1]
        [TagField(ValidTags = new [] { "cook" })]
        public CachedTag RewardGlobals;
        [TagField(ValidTags = new [] { "comg" })]
        public CachedTag CommendationGlobals;
        public short MaximumHeat;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        // seconds to completely deplete a full heat meter
        public float FullHeatDecayTime; // S
        // seconds from the time you are at maximum heat until it starts decaying again
        public float FullHeatStunTime; // s
        // seconds from the time you are at maximum heat until it starts decaying again
        public float BetrayalHeatStunTime; // s
        [TagField(ValidTags = new [] { "ingd" })]
        public CachedTag IncidentDefinitions;
        // generated in code
        public List<IncidentDefinitionBlockStruct> DefaultIncidentDefinition;
        
        public enum ActiveCamoEnum : int
        {
            Poor,
            Good,
            Excellent,
            Invisible,
            Ai
        }
        
        [TagStructure(Size = 0x54)]
        public class IncidentDefinitionBlockStruct : TagStructure
        {
            public StringId Name;
            public IncidentDefinitionFlags Flags;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public GameModeFlagsStruct DisallowedGameModes;
            public List<SuppressedIncidentBlock> SuppressedIncidents;
            public List<SuppressedIncidentBlockReferenceDefinition> SuppressedIncidentBlocks;
            public List<SpecializedIncidentBlock> SpecializedIncidents;
            public List<IncidentAccumulatorBlock> AccumulatorIncidents;
            public List<IncidentSumAccumulatorBlock> SumAccumulatorIncidents;
            public List<GameIncidentResponseBlockStruct> Response;
            
            [Flags]
            public enum IncidentDefinitionFlags : byte
            {
                NeverNetwork = 1 << 0,
                AlwaysNetworkToEveryone = 1 << 1,
                UseLongDelay = 1 << 2,
                ForceGameEngineEventDisplayInCampaign = 1 << 3
            }
            
            [TagStructure(Size = 0x4)]
            public class GameModeFlagsStruct : TagStructure
            {
                public GameTypeEnum GameMode;
                public GameMatchmakingFlags MatchmakingType;
                public GlobalCampaignDifficultyFlags Difficulty;
                public GamePlayerCountFlags PlayerCount;
                
                [Flags]
                public enum GameTypeEnum : byte
                {
                    Campaign = 1 << 0,
                    Firefight = 1 << 1,
                    Multiplayer = 1 << 2
                }
                
                [Flags]
                public enum GameMatchmakingFlags : byte
                {
                    CustomGame = 1 << 0,
                    Matchmaking = 1 << 1
                }
                
                [Flags]
                public enum GlobalCampaignDifficultyFlags : byte
                {
                    Easy = 1 << 0,
                    Normal = 1 << 1,
                    Heroic = 1 << 2,
                    Legendary = 1 << 3
                }
                
                public enum GamePlayerCountFlags : sbyte
                {
                    Any,
                    _1PlayerOnly,
                    _4PlayersOnly,
                    MoreThanOnePlayer
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class SuppressedIncidentBlock : TagStructure
            {
                public StringId IncidentName;
                public SuppressedIncidentFlags SuppressionType;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum SuppressedIncidentFlags : byte
                {
                    SuppressCausePlayerGameEngineEvent = 1 << 0,
                    SuppressEffectPlayerGameEngineEvent = 1 << 1,
                    SuppressCauseTeamGameEngineEvent = 1 << 2,
                    SuppressEffectTeamGameEngineEvent = 1 << 3,
                    SuppressMedalDisplay = 1 << 4,
                    SuppressMedalStats = 1 << 5,
                    SuppressFanfare = 1 << 6,
                    SuppressAudio = 1 << 7
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class SuppressedIncidentBlockReferenceDefinition : TagStructure
            {
                [TagField(ValidTags = new [] { "sigd" })]
                public CachedTag SuppressionBlock;
            }
            
            [TagStructure(Size = 0x64)]
            public class SpecializedIncidentBlock : TagStructure
            {
                public StringId BaseIncident;
                public List<SpecializedIncidentKillImplementBlock> KillImplements;
                public List<SpecializedIncidentObjectPropertiesBlock> CauseObject;
                public List<SpecializedIncidentObjectPropertiesBlock> EffectObject;
                public List<SpecializedIncidentSpecialKillTypeBlock> SpecialKillType;
                public List<SpecializedincidentGameOverBlock> GameOverFilter;
                public List<SpecializedincidentRandomOrdnanceBlock> OrdnanceFilter;
                public List<SpecializedincidentCustomDataFilterBlock> CustomDataFilter;
                public List<SpecializedincidentDistanceFilterBlock> DistanceFilter;
                
                [TagStructure(Size = 0x4)]
                public class SpecializedIncidentKillImplementBlock : TagStructure
                {
                    public GlobalDamageReportingEnum DamageReportingType;
                    [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    public enum GlobalDamageReportingEnum : sbyte
                    {
                        Unknown,
                        TehGuardians,
                        Scripting,
                        AiSuicide,
                        ForerunnerSmg,
                        SpreadGun,
                        ForerunnerRifle,
                        ForerunnerSniper,
                        BishopBeam,
                        BoltPistol,
                        PulseGrenade,
                        IncinerationLauncher,
                        MagnumPistol,
                        AssaultRifle,
                        MarksmanRifle,
                        Shotgun,
                        BattleRifle,
                        SniperRifle,
                        RocketLauncher,
                        SpartanLaser,
                        FragGrenade,
                        StickyGrenadeLauncher,
                        LightMachineGun,
                        RailGun,
                        PlasmaPistol,
                        Needler,
                        GravityHammer,
                        EnergySword,
                        PlasmaGrenade,
                        Carbine,
                        BeamRifle,
                        AssaultCarbine,
                        ConcussionRifle,
                        FuelRodCannon,
                        Ghost,
                        RevenantDriver,
                        RevenantGunner,
                        Wraith,
                        WraithAntiInfantry,
                        Banshee,
                        BansheeBomb,
                        Seraph,
                        RevenantDeuxDriver,
                        RevenantDeuxGunner,
                        LichDriver,
                        LichGunner,
                        Mongoose,
                        WarthogDriver,
                        WarthogGunner,
                        WarthogGunnerGauss,
                        WarthogGunnerRocket,
                        Scorpion,
                        ScorpionGunner,
                        FalconDriver,
                        FalconGunner,
                        WaspDriver,
                        WaspGunner,
                        WaspGunnerHeavy,
                        MechMelee,
                        MechChaingun,
                        MechCannon,
                        MechRocket,
                        Broadsword,
                        BroadswordMissile,
                        TortoiseDriver,
                        TortoiseGunner,
                        MacCannon,
                        TargetDesignator,
                        OrdnanceDropPod,
                        OrbitalCruiseMissile,
                        PortableShield,
                        PersonalAutoTurret,
                        ThrusterPack,
                        FallingDamage,
                        GenericCollisionDamage,
                        GenericMeleeDamage,
                        GenericExplosion,
                        FireDamage,
                        BirthdayPartyExplosion,
                        FlagMeleeDamage,
                        BombMeleeDamage,
                        BombExplosionDamage,
                        BallMeleeDamage,
                        Teleporter,
                        TransferDamage,
                        ArmorLockCrush,
                        HumanTurret,
                        PlasmaCannon,
                        PlasmaMortar,
                        PlasmaTurret,
                        ShadeTurret,
                        ForerunnerTurret,
                        Tank,
                        Chopper,
                        Hornet,
                        Mantis,
                        MagnumPistolCtf,
                        FloodProngs
                    }
                }
                
                [TagStructure(Size = 0x34)]
                public class SpecializedIncidentObjectPropertiesBlock : TagStructure
                {
                    public SpecializedIncidentKillBucketFlags Flags;
                    public CampaignMetagameBucketTypeWithNoneEnum BucketType;
                    public CampaignMetagameBucketClassWithNoneEnum BucketClass;
                    public SpecializedIncidentKillBucketSecondaryFlags SecondaryFlags;
                    [TagField(Length = 32)]
                    public string Gamertag;
                    public List<SpecializedIncidentObjectRidingInVehiclePropertiesBlock> RidingInVehicles;
                    
                    [Flags]
                    public enum SpecializedIncidentKillBucketFlags : uint
                    {
                        Player = 1 << 0,
                        Ai = 1 << 1,
                        _343iEmployee = 1 << 2,
                        Hologram = 1 << 3,
                        Airborne = 1 << 4,
                        Sprinting = 1 << 5,
                        Crouched = 1 << 6,
                        Reloading = 1 << 7,
                        NotFancyAssassinating = 1 << 8,
                        FancyAssassinating = 1 << 9,
                        BeingFancyAssassinated = 1 << 10,
                        LowHealth = 1 << 11,
                        Unshielded = 1 << 12,
                        InStasisField = 1 << 13,
                        ActiveCamouflageActive = 1 << 14,
                        HologramActive = 1 << 15,
                        JetpackActive = 1 << 16,
                        PortableAutomatedTurretActive = 1 << 17,
                        ReflectiveShieldActive = 1 << 18,
                        ThrusterPackActive = 1 << 19,
                        XRayVisionActive = 1 << 20,
                        ActiveCamouflageActiveAndEffective = 1 << 21,
                        HologramRecentlyActivated = 1 << 22,
                        ThrusterPackRecentlyActivated = 1 << 23,
                        HologramTookDamageFromOtherPlayer = 1 << 24,
                        HologramDidNotTakeDamageFromOtherPlayer = 1 << 25,
                        ActiveShieldRecentlyActivated = 1 << 26,
                        InAFullVehicle = 1 << 27,
                        ScopedIn = 1 << 28,
                        NotScopedIn = 1 << 29,
                        JackingVehicle = 1 << 30,
                        Airsassination = 1u << 31
                    }
                    
                    public enum CampaignMetagameBucketTypeWithNoneEnum : sbyte
                    {
                        Any,
                        Brute,
                        Grunt,
                        Jackel,
                        Skirmisher,
                        Marine,
                        Spartan,
                        Bugger,
                        Hunter,
                        FloodInfection,
                        FloodCarrier,
                        FloodCombat,
                        FloodPure,
                        Sentinel,
                        Elite,
                        Engineer,
                        Mule,
                        Turret,
                        Mongoose,
                        Warthog,
                        Scorpion,
                        Hornet,
                        Pelican,
                        Revenant,
                        Seraph,
                        Shade,
                        Watchtower,
                        Ghost,
                        Chopper,
                        Mauler,
                        Wraith,
                        Banshee,
                        Phantom,
                        Scarab,
                        Guntower,
                        TuningFork,
                        Broadsword,
                        Mammoth,
                        Lich,
                        Mantis,
                        Wasp,
                        Phaeton,
                        Bishop,
                        Knight,
                        Pawn
                    }
                    
                    public enum CampaignMetagameBucketClassWithNoneEnum : sbyte
                    {
                        Any,
                        Infantry,
                        Leader,
                        Hero,
                        Specialist,
                        LightVehicle,
                        HeavyVehicle,
                        GiantVehicle,
                        StandardVehicle
                    }
                    
                    [Flags]
                    public enum SpecializedIncidentKillBucketSecondaryFlags : ushort
                    {
                        LaunchedFromManCannon = 1 << 0,
                        InAirborneVehicle = 1 << 1,
                        NotInVehicle = 1 << 2
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class SpecializedIncidentObjectRidingInVehiclePropertiesBlock : TagStructure
                    {
                        public VehicleTypeEnum VehicleType;
                        
                        public enum VehicleTypeEnum : int
                        {
                            VehicleTypeHumanTank,
                            VehicleTypeHumanJeep,
                            VehicleTypeHumanPlane,
                            VehicleTypeWolverine,
                            VehicleTypeAlienScout,
                            VehicleTypeAlienFighter,
                            VehicleTypeTurret,
                            VehicleTypeMantis,
                            VehicleTypeVtol,
                            VehicleTypeChopper,
                            VehicleTypeGuardian,
                            VehicleTypeJackalGlider,
                            VehicleTypeBoat,
                            VehicleTypeSpaceFighter,
                            VehicleTypeRevenant
                        }
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class SpecializedIncidentSpecialKillTypeBlock : TagStructure
                {
                    public SpecializedIncidentSpecialKillTypeEnum SpecialKillType;
                    [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    public enum SpecializedIncidentSpecialKillTypeEnum : sbyte
                    {
                        Headshot,
                        GrenadeStick,
                        StealthKill,
                        FancyAssassination,
                        Superdetonation,
                        EmpKill,
                        Melee,
                        // vehicular splatter
                        Collision,
                        // killed the player in 1st place
                        LeaderKilled,
                        // killed the last surviving player on a team in team game
                        TeamEliminationKill,
                        // killed the last surviving player in a ffa game
                        FfaEliminationKill
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class SpecializedincidentGameOverBlock : TagStructure
                {
                    public SpecializedincidentGameOverFlags Flags;
                    public short MinimumGameLength; // seconds
                    // only used if the "check megalo category" flag is set
                    public sbyte MegaloCategoryIndex;
                    [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    [Flags]
                    public enum SpecializedincidentGameOverFlags : ushort
                    {
                        // only looks at the enemies in-game at the end
                        KilledAllEnemies = 1 << 0,
                        // had the highest score
                        HighestScore = 1 << 1,
                        // one of the top three scorers
                        Top3Score = 1 << 2,
                        // individual winner or on the winning team
                        Winner = 1 << 3,
                        // was the host of the game at the end
                        Hosted = 1 << 4,
                        // if set, compare the Megalo category index against the tag value
                        CheckMegaloCategory = 1 << 5,
                        Loser = 1 << 6,
                        Tied = 1 << 7,
                        NotTied = 1 << 8
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class SpecializedincidentRandomOrdnanceBlock : TagStructure
                {
                    // This must match one of the global ordnance objects.
                    public StringId OrdnanceName;
                }
                
                [TagStructure(Size = 0x8)]
                public class SpecializedincidentCustomDataFilterBlock : TagStructure
                {
                    public NumericComparisonFlags Flags;
                    [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public int Value;
                    
                    [Flags]
                    public enum NumericComparisonFlags : byte
                    {
                        EqualTo = 1 << 0,
                        GreaterThan = 1 << 1,
                        LessThan = 1 << 2
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class SpecializedincidentDistanceFilterBlock : TagStructure
                {
                    public NumericComparisonFlags Flags;
                    [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public int DistanceBetweenEffectAndCausePlayer; // wu
                    
                    [Flags]
                    public enum NumericComparisonFlags : byte
                    {
                        EqualTo = 1 << 0,
                        GreaterThan = 1 << 1,
                        LessThan = 1 << 2
                    }
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class IncidentAccumulatorBlock : TagStructure
            {
                public IncidentAccumulatorAggregationMethodEnum AggregationType;
                public IncidentAccumulatorResetEnum ResetsOn;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<IncidentAccumulatorChildIncidentBlock> ChildIncidents;
                
                public enum IncidentAccumulatorAggregationMethodEnum : sbyte
                {
                    All,
                    Any
                }
                
                public enum IncidentAccumulatorResetEnum : sbyte
                {
                    FourSecondsSinceLastEvent,
                    PlayerDeath,
                    RoundOver,
                    GameOver,
                    OnIncident
                }
                
                [TagStructure(Size = 0x14)]
                public class IncidentAccumulatorChildIncidentBlock : TagStructure
                {
                    public NumericComparisonFlags Flags;
                    [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public int Count;
                    public StringId IncidentName;
                    public StringId ResetIncidentName;
                    public float ResetTimeout;
                    
                    [Flags]
                    public enum NumericComparisonFlags : byte
                    {
                        EqualTo = 1 << 0,
                        GreaterThan = 1 << 1,
                        LessThan = 1 << 2
                    }
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class IncidentSumAccumulatorBlock : TagStructure
            {
                public IncidentAccumulatorResetEnum ResetsOn;
                public NumericComparisonFlags Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public int Count;
                public List<IncidentSumAccumulatorChildIncidentBlock> ChildIncidents;
                
                public enum IncidentAccumulatorResetEnum : sbyte
                {
                    FourSecondsSinceLastEvent,
                    PlayerDeath,
                    RoundOver,
                    GameOver,
                    OnIncident
                }
                
                [Flags]
                public enum NumericComparisonFlags : byte
                {
                    EqualTo = 1 << 0,
                    GreaterThan = 1 << 1,
                    LessThan = 1 << 2
                }
                
                [TagStructure(Size = 0xC)]
                public class IncidentSumAccumulatorChildIncidentBlock : TagStructure
                {
                    public StringId IncidentName;
                    public StringId ResetIncidentName;
                    public float ResetTimeout;
                }
            }
            
            [TagStructure(Size = 0x54)]
            public class GameIncidentResponseBlockStruct : TagStructure
            {
                public GameModeFlagsStruct AllowedGameModes;
                // Can only be triggered on this level.
                public StringId AllowedLevelName;
                // These skulls must be enabled to trigger.
                public SkullFlags RequiredSkulls;
                public int HeatAward;
                public StringId GameEngineEvent;
                public StringId Medal;
                public StringId Commendation;
                public StringId Achievement;
                public StringId AvatarAward;
                public List<GameIncidentDailyChallengeToIncrementBlock> Challenges;
                public StringId HsScript;
                public short InternalHsScriptIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<SpecializedIncidentFanfareBlock> Fanfare;
                [TagField(ValidTags = new [] { "sirg" })]
                public CachedTag SoundResponse;
                
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
                
                [TagStructure(Size = 0x4)]
                public class GameIncidentDailyChallengeToIncrementBlock : TagStructure
                {
                    public StringId DailyChallenge;
                }
                
                [TagStructure(Size = 0x34)]
                public class SpecializedIncidentFanfareBlock : TagStructure
                {
                    // 0 is highest prioroty
                    public int Priority;
                    public IncidentFanfareQueueType QueueType;
                    public IncidentFanfareEventInputEnum ExcludedAudience;
                    public IncidentFanfareEventFlags BroadCastMessage;
                    [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public StringId FanfareString;
                    public short FanfareSpriteIndex;
                    public short DisplayTimeInSeconds;
                    [TagField(ValidTags = new [] { "cusc" })]
                    public CachedTag HudScreenReference;
                    [TagField(ValidTags = new [] { "sgrp" })]
                    public CachedTag SoundResponse;
                    public StringId CarriedObjectType;
                    
                    public enum IncidentFanfareQueueType : sbyte
                    {
                        Center,
                        GameMode,
                        Territory,
                        GameModeSecondary,
                        Ordnance
                    }
                    
                    [Flags]
                    public enum IncidentFanfareEventInputEnum : byte
                    {
                        None = 1 << 0,
                        CausePlayer = 1 << 1,
                        CauseTeam = 1 << 2,
                        EffectPlayer = 1 << 3,
                        EffectTeam = 1 << 4
                    }
                    
                    [Flags]
                    public enum IncidentFanfareEventFlags : byte
                    {
                        BroadCastMessage = 1 << 0
                    }
                }
            }
        }
    }
}
