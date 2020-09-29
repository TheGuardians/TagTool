using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "multiplayer_globals", Tag = "mulg", Size = 0x18)]
    public class MultiplayerGlobals : TagStructure
    {
        public List<MultiplayerUniversalGlobalsDefinition> Universal;
        public List<MultiplayerRuntimeGlobalsDefinition> Runtime;
        
        [TagStructure(Size = 0x3C)]
        public class MultiplayerUniversalGlobalsDefinition : TagStructure
        {
            public CachedTag RandomPlayerNames;
            public CachedTag TeamNames;
            public List<RealRgbColor> TeamColors;
            public CachedTag MultiplayerText;
            
            [TagStructure(Size = 0xC)]
            public class RealRgbColor : TagStructure
            {
                public RealRgbColor Color;
            }
        }
        
        [TagStructure(Size = 0x668)]
        public class MultiplayerRuntimeGlobalsDefinition : TagStructure
        {
            public CachedTag Flag;
            public CachedTag Ball;
            public CachedTag Unit;
            public CachedTag FlagShader;
            public CachedTag HillShader;
            public CachedTag Head;
            public CachedTag JuggernautPowerup;
            public CachedTag DaBomb;
            public CachedTag Unknown1;
            public CachedTag Unknown2;
            public CachedTag Unknown3;
            public CachedTag Unknown4;
            public CachedTag Unknown5;
            public List<TagReference> Weapons;
            public List<TagReference> Vehicles;
            public GrenadeAndPowerupStructBlock Arr;
            public CachedTag InGameText;
            public List<TagReference> Sounds;
            public List<MultiplayerEventResponseDefinition> GeneralEvents;
            public List<MultiplayerEventResponseDefinition> FlavorEvents;
            public List<MultiplayerEventResponseDefinition> SlayerEvents;
            public List<MultiplayerEventResponseDefinition> CtfEvents;
            public List<MultiplayerEventResponseDefinition> OddballEvents;
            public List<GNullBlock> Unknown6;
            public List<MultiplayerEventResponseDefinition> KingEvents;
            public List<GNullBlock> Unknown7;
            public List<MultiplayerEventResponseDefinition> JuggernautEvents;
            public List<MultiplayerEventResponseDefinition> TerritoriesEvents;
            public List<MultiplayerEventResponseDefinition> InvasionEvents;
            public List<GNullBlock> Unknown8;
            public List<GNullBlock> Unknown9;
            public List<GNullBlock> Unknown10;
            public List<GNullBlock> Unknown11;
            public CachedTag DefaultItemCollection1;
            public CachedTag DefaultItemCollection2;
            public int DefaultFragGrenadeCount;
            public int DefaultPlasmaGrenadeCount;
            [TagField(Flags = Padding, Length = 40)]
            public byte[] Padding1;
            /// <summary>
            /// dynamic spawn zones
            /// </summary>
            public float DynamicZoneUpperHeight;
            public float DynamicZoneLowerHeight;
            [TagField(Flags = Padding, Length = 40)]
            public byte[] Padding2;
            /// <summary>
            /// enemy
            /// </summary>
            public float EnemyInnerRadius;
            public float EnemyOuterRadius;
            public float EnemyWeight;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding3;
            /// <summary>
            /// friend
            /// </summary>
            public float FriendInnerRadius;
            public float FriendOuterRadius;
            public float FriendWeight;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding4;
            /// <summary>
            /// enemy vehicle
            /// </summary>
            public float EnemyVehicleInnerRadius;
            public float EnemyVehicleOuterRadius;
            public float EnemyVehicleWeight;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding5;
            /// <summary>
            /// friendly vehicle
            /// </summary>
            public float FriendlyVehicleInnerRadius;
            public float FriendlyVehicleOuterRadius;
            public float FriendlyVehicleWeight;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding6;
            /// <summary>
            /// empty vehicle
            /// </summary>
            public float EmptyVehicleInnerRadius;
            public float EmptyVehicleOuterRadius;
            public float EmptyVehicleWeight;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding7;
            /// <summary>
            /// oddball inclusion
            /// </summary>
            public float OddballInclusionInnerRadius;
            public float OddballInclusionOuterRadius;
            public float OddballInclusionWeight;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding8;
            /// <summary>
            /// oddball exclusion
            /// </summary>
            public float OddballExclusionInnerRadius;
            public float OddballExclusionOuterRadius;
            public float OddballExclusionWeight;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding9;
            /// <summary>
            /// hill inclusion
            /// </summary>
            public float HillInclusionInnerRadius;
            public float HillInclusionOuterRadius;
            public float HillInclusionWeight;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding10;
            /// <summary>
            /// hill exclusion
            /// </summary>
            public float HillExclusionInnerRadius;
            public float HillExclusionOuterRadius;
            public float HillExclusionWeight;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding11;
            /// <summary>
            /// last race flag
            /// </summary>
            public float LastRaceFlagInnerRadius;
            public float LastRaceFlagOuterRadius;
            public float LastRaceFlagWeight;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding12;
            /// <summary>
            /// dead ally
            /// </summary>
            public float DeadAllyInnerRadius;
            public float DeadAllyOuterRadius;
            public float DeadAllyWeight;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding13;
            /// <summary>
            /// controlled territory
            /// </summary>
            public float ControlledTerritoryInnerRadius;
            public float ControlledTerritoryOuterRadius;
            public float ControlledTerritoryWeight;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding14;
            [TagField(Flags = Padding, Length = 560)]
            public byte[] Padding15;
            [TagField(Flags = Padding, Length = 48)]
            public byte[] Padding16;
            public List<MultiplayerConstantsBlock> MultiplayerConstants;
            public List<GameEngineStatusResponse> StateResponses;
            public CachedTag ScoreboardHudDefinition;
            public CachedTag ScoreboardEmblemShader;
            public CachedTag ScoreboardEmblemBitmap;
            public CachedTag ScoreboardDeadEmblemShader;
            public CachedTag ScoreboardDeadEmblemBitmap;
            
            [TagStructure(Size = 0x10)]
            public class TagReference : TagStructure
            {
                public CachedTag Weapon;
            }
            
            [TagStructure(Size = 0x18)]
            public class GrenadeAndPowerupStructBlock : TagStructure
            {
                public List<TagReference> Grenades;
                public List<TagReference> Powerups;
                
                [TagStructure(Size = 0x10)]
                public class TagReference : TagStructure
                {
                    public CachedTag Weapon;
                }
            }
            
            [TagStructure(Size = 0xF4)]
            public class MultiplayerEventResponseDefinition : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public EventValue Event;
                public AudienceValue Audience;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding2;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding3;
                public StringId DisplayString;
                public RequiredFieldValue RequiredField;
                public ExcludedAudienceValue ExcludedAudience;
                public StringId PrimaryString;
                public int PrimaryStringDuration; // seconds
                public StringId PluralDisplayString;
                [TagField(Flags = Padding, Length = 28)]
                public byte[] Padding4;
                public float SoundDelayAnnouncerOnly;
                public SoundFlagsValue SoundFlags;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding5;
                public CachedTag Sound;
                public _1 ExtraSounds;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding6;
                [TagField(Flags = Padding, Length = 16)]
                public byte[] Padding7;
                public List<MultiplayerEventSoundResponseDefinition> SoundPermutations;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    QuantityMessage = 1 << 0
                }
                
                public enum EventValue : short
                {
                    Kill,
                    Suicide,
                    KillTeammate,
                    Victory,
                    TeamVictory,
                    Unused1,
                    Unused2,
                    _1MinToWin,
                    Team1MinToWin,
                    _30SecsToWin,
                    Team30SecsToWin,
                    PlayerQuit,
                    PlayerJoined,
                    KilledByUnknown,
                    _30MinutesLeft,
                    _15MinutesLeft,
                    _5MinutesLeft,
                    _1MinuteLeft,
                    TimeExpired,
                    GameOver,
                    RespawnTick,
                    LastRespawnTick,
                    TeleporterUsed,
                    PlayerChangedTeam,
                    PlayerRejoined,
                    GainedLead,
                    GainedTeamLead,
                    LostLead,
                    LostTeamLead,
                    TiedLeader,
                    TiedTeamLeader,
                    RoundOver,
                    _30SecondsLeft,
                    _10SecondsLeft,
                    KillFalling,
                    KillCollision,
                    KillMelee,
                    SuddenDeath,
                    PlayerBootedPlayer,
                    KillFlagCarrier,
                    KillBombCarrier,
                    KillStickyGrenade,
                    KillSniper,
                    KillStMelee,
                    BoardedVehicle,
                    StartTeamNoti,
                    Telefrag,
                    _10SecsToWin,
                    Team10SecsToWin
                }
                
                public enum AudienceValue : short
                {
                    CausePlayer,
                    CauseTeam,
                    EffectPlayer,
                    EffectTeam,
                    All
                }
                
                public enum RequiredFieldValue : short
                {
                    None,
                    CausePlayer,
                    CauseTeam,
                    EffectPlayer,
                    EffectTeam
                }
                
                public enum ExcludedAudienceValue : short
                {
                    None,
                    CausePlayer,
                    CauseTeam,
                    EffectPlayer,
                    EffectTeam
                }
                
                [Flags]
                public enum SoundFlagsValue : ushort
                {
                    AnnouncerSound = 1 << 0
                }
                
                [TagStructure(Size = 0x80)]
                public class _1 : TagStructure
                {
                    public CachedTag JapaneseSound;
                    public CachedTag GermanSound;
                    public CachedTag FrenchSound;
                    public CachedTag SpanishSound;
                    public CachedTag ItalianSound;
                    public CachedTag KoreanSound;
                    public CachedTag ChineseSound;
                    public CachedTag PortugueseSound;
                }
                
                [TagStructure(Size = 0x98)]
                public class MultiplayerEventSoundResponseDefinition : TagStructure
                {
                    public SoundFlagsValue SoundFlags;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
                    public CachedTag EnglishSound;
                    public _1 ExtraSounds;
                    public float Probability;
                    
                    [Flags]
                    public enum SoundFlagsValue : ushort
                    {
                        AnnouncerSound = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x80)]
                    public class _1 : TagStructure
                    {
                        public CachedTag JapaneseSound;
                        public CachedTag GermanSound;
                        public CachedTag FrenchSound;
                        public CachedTag SpanishSound;
                        public CachedTag ItalianSound;
                        public CachedTag KoreanSound;
                        public CachedTag ChineseSound;
                        public CachedTag PortugueseSound;
                    }
                }
            }
            
            [TagStructure()]
            public class GNullBlock : TagStructure
            {
            }
            
            [TagStructure(Size = 0x180)]
            public class MultiplayerConstantsBlock : TagStructure
            {
                public float MaximumRandomSpawnBias;
                public float TeleporterRechargeTime; // seconds
                public float GrenadeDangerWeight;
                public float GrenadeDangerInnerRadius;
                public float GrenadeDangerOuterRadius;
                public float GrenadeDangerLeadTime; // seconds
                public float VehicleDangerMinSpeed; // wu/sec
                public float VehicleDangerWeight;
                public float VehicleDangerRadius;
                public float VehicleDangerLeadTime; // seconds
                public float VehicleNearbyPlayerDist; // how nearby a player is to count a vehicle as 'occupied'
                [TagField(Flags = Padding, Length = 84)]
                public byte[] Padding1;
                [TagField(Flags = Padding, Length = 32)]
                public byte[] Padding2;
                [TagField(Flags = Padding, Length = 32)]
                public byte[] Padding3;
                public CachedTag HillShader;
                [TagField(Flags = Padding, Length = 16)]
                public byte[] Padding4;
                public float FlagResetStopDistance;
                public CachedTag BombExplodeEffect;
                public CachedTag BombExplodeDmgEffect;
                public CachedTag BombDefuseEffect;
                public StringId BombDefusalString;
                public StringId BlockedTeleporterString;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding5;
                [TagField(Flags = Padding, Length = 32)]
                public byte[] Padding6;
                [TagField(Flags = Padding, Length = 32)]
                public byte[] Padding7;
                [TagField(Flags = Padding, Length = 32)]
                public byte[] Padding8;
            }
            
            [TagStructure(Size = 0x24)]
            public class GameEngineStatusResponse : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public StateValue State;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding2;
                public StringId FfaMessage;
                public StringId TeamMessage;
                public CachedTag Unknown1;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding3;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    Unused = 1 << 0
                }
                
                public enum StateValue : short
                {
                    WaitingForSpaceToClear,
                    Observing,
                    RespawningSoon,
                    SittingOut,
                    OutOfLives,
                    PlayingWinning,
                    PlayingTied,
                    PlayingLosing,
                    GameOverWon,
                    GameOverTied,
                    GameOverLost,
                    YouHaveFlag,
                    EnemyHasFlag,
                    FlagNotHome,
                    CarryingOddball,
                    YouAreJuggy,
                    YouControlHill,
                    SwitchingSidesSoon,
                    PlayerRecentlyStarted,
                    YouHaveBomb,
                    FlagContested,
                    BombContested,
                    LimitedLivesLeftMultiple,
                    LimitedLivesLeftSingle,
                    LimitedLivesLeftFinal,
                    PlayingWinningUnlimited,
                    PlayingTiedUnlimited,
                    PlayingLosingUnlimited
                }
            }
        }
    }
}

