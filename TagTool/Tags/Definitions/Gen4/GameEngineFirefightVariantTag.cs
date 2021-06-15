using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "GameEngineFirefightVariantTag", Tag = "ffgt", Size = 0xAC)]
    public class GameEngineFirefightVariantTag : TagStructure
    {
        public GameengineFirefightVariantDefinition Variant;
        
        [TagStructure(Size = 0xAC)]
        public class GameengineFirefightVariantDefinition : TagStructure
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
            public int MissionId;
            public GlobalCampaignDifficultyEnum GameDifficulty;
            public GameEngineFirefightVariantFlags FirefightVariantFlags;
            public short SharedTeamLifeCount;
            public short MaximumLives;
            public sbyte StartingCrate1;
            public sbyte StartingCrate2;
            public sbyte StartingCrate3;
            public sbyte StartingCrate4;
            public sbyte StartingCrate5;
            public sbyte StartingCrate6;
            public sbyte StartingCrate7;
            public sbyte StartingCrate8;
            public sbyte StartingCrate9;
            public sbyte StartingCrate10;
            public sbyte StartingCrate11;
            public sbyte StartingCrate12;
            public sbyte StartingCrate13;
            public sbyte StartingCrate14;
            // This event is always set for script
            [TagField(Length = 32)]
            public string VariantEvent;
            public List<GameEngineFirefightPlayerGoalPropertiesBlock> MidnightPlayerGoalProperties;
            
            public enum GlobalCampaignDifficultyEnum : sbyte
            {
                Easy,
                Normal,
                Heroic,
                Legendary
            }
            
            [Flags]
            public enum GameEngineFirefightVariantFlags : byte
            {
                UseAmmoCrates = 1 << 0,
                PlayersRespawnOnWave = 1 << 1
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
            
            [TagStructure(Size = 0x68)]
            public class GameEngineFirefightPlayerGoalPropertiesBlock : TagStructure
            {
                public FirefightGoal PlayerGoal;
                public sbyte Lives;
                // minutes
                public sbyte TimeLimit;
                public FirefightWaveDifficulty WaveDifficulty;
                public List<GameEngineFirefightWavePropertiesBlock> FirefightWaves;
                public GameEnginePlayerGoalFlags PlayerGoalFlags;
                public StringId SpartanPlayerTraits;
                public StringId AiTraits;
                [TagField(Length = 32)]
                public string StartEvent;
                [TagField(Length = 32)]
                public string EndEvent;
                public SkullFlags Skulls;
                public sbyte Objective1;
                public sbyte Objective2;
                public sbyte Objective3;
                public sbyte Objective4;
                public sbyte UserData;
                public sbyte StartLocationFolder;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                public enum FirefightGoal : sbyte
                {
                    ObjectDestruction,
                    ObjectDelivery,
                    LocationArrival,
                    TimePassed,
                    NoMoreWaves,
                    KillBoss,
                    Defense,
                    Other
                }
                
                [Flags]
                public enum FirefightWaveDifficulty : byte
                {
                    Easy = 1 << 0,
                    Medium = 1 << 1,
                    Hard = 1 << 2
                }
                
                [Flags]
                public enum GameEnginePlayerGoalFlags : uint
                {
                    Looping = 1 << 0,
                    RandomWaves = 1 << 1
                }
                
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
                
                [TagStructure(Size = 0x4C)]
                public class GameEngineFirefightWavePropertiesBlock : TagStructure
                {
                    // survival_mode_get_wave_squad
                    public StringId SquadType;
                    public FirefightWaveDeliveryMethod DeliveryType;
                    public sbyte AiAllowedBeforeNextWaveSpawns;
                    public sbyte EnemiesLeftBeforeHudMarking;
                    public sbyte WaveType;
                    public sbyte WeaponToDrop;
                    public sbyte VehicleToDrop;
                    public sbyte Squad1;
                    public sbyte Squad2;
                    [TagField(Length = 32)]
                    public string StartEvent;
                    [TagField(Length = 32)]
                    public string EndEvent;
                    
                    public enum FirefightWaveDeliveryMethod : sbyte
                    {
                        Dropship,
                        MonsterCloset,
                        DropPod,
                        TestSpawn
                    }
                }
            }
        }
    }
}
