using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "multiplayer_globals", Tag = "mulg", Size = 0x10)]
    public class MultiplayerGlobals : TagStructure
    {
        public List<MultiplayerUniversalBlock> Universal;
        public List<MultiplayerRuntimeBlock> Runtime;
        
        [TagStructure(Size = 0x20)]
        public class MultiplayerUniversalBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "unic" })]
            public CachedTag RandomPlayerNames;
            [TagField(ValidTags = new [] { "unic" })]
            public CachedTag TeamNames;
            public List<MultiplayerColorBlock> TeamColors;
            [TagField(ValidTags = new [] { "unic" })]
            public CachedTag MultiplayerText;
            
            [TagStructure(Size = 0xC)]
            public class MultiplayerColorBlock : TagStructure
            {
                public RealRgbColor Color;
            }
        }
        
        [TagStructure(Size = 0x568)]
        public class MultiplayerRuntimeBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "item" })]
            public CachedTag Flag;
            [TagField(ValidTags = new [] { "item" })]
            public CachedTag Ball;
            [TagField(ValidTags = new [] { "unit" })]
            public CachedTag Unit;
            [TagField(ValidTags = new [] { "shad" })]
            public CachedTag FlagShader;
            [TagField(ValidTags = new [] { "shad" })]
            public CachedTag HillShader;
            [TagField(ValidTags = new [] { "item" })]
            public CachedTag Head;
            [TagField(ValidTags = new [] { "item" })]
            public CachedTag JuggernautPowerup;
            [TagField(ValidTags = new [] { "item" })]
            public CachedTag DaBomb;
            public CachedTag Unknown;
            public CachedTag Unknown1;
            public CachedTag Unknown2;
            public CachedTag Unknown3;
            public CachedTag Unknown4;
            public List<WeaponsBlock> Weapons;
            public List<VehiclesBlock> Vehicles;
            public GrenadeAndPowerupStructBlock Arr;
            [TagField(ValidTags = new [] { "unic" })]
            public CachedTag InGameText;
            public List<SoundsBlock> Sounds;
            public List<GameEngineGeneralEventBlock> GeneralEvents;
            public List<GameEngineFlavorEventBlock> FlavorEvents;
            public List<GameEngineSlayerEventBlock> SlayerEvents;
            public List<GameEngineCtfEventBlock> CtfEvents;
            public List<GameEngineOddballEventBlock> OddballEvents;
            public List<GNullBlock> Unknown5;
            public List<GameEngineKingEventBlock> KingEvents;
            public List<GNullBlock1> Unknown6;
            public List<GameEngineJuggernautEventBlock> JuggernautEvents;
            public List<GameEngineTerritoriesEventBlock> TerritoriesEvents;
            public List<GameEngineAssaultEventBlock> InvasionEvents;
            public List<GNullBlock2> Unknown7;
            public List<GNullBlock3> Unknown8;
            public List<GNullBlock4> Unknown9;
            public List<GNullBlock5> Unknown10;
            [TagField(ValidTags = new [] { "itmc" })]
            public CachedTag DefaultItemCollection1;
            [TagField(ValidTags = new [] { "itmc" })]
            public CachedTag DefaultItemCollection2;
            public int DefaultFragGrenadeCount;
            public int DefaultPlasmaGrenadeCount;
            [TagField(Length = 0x28, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float DynamicZoneUpperHeight;
            public float DynamicZoneLowerHeight;
            [TagField(Length = 0x28, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public float EnemyInnerRadius;
            public float EnemyOuterRadius;
            public float EnemyWeight;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public float FriendInnerRadius;
            public float FriendOuterRadius;
            public float FriendWeight;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            public float EnemyVehicleInnerRadius;
            public float EnemyVehicleOuterRadius;
            public float EnemyVehicleWeight;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
            public float FriendlyVehicleInnerRadius;
            public float FriendlyVehicleOuterRadius;
            public float FriendlyVehicleWeight;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding5;
            public float EmptyVehicleInnerRadius;
            public float EmptyVehicleOuterRadius;
            public float EmptyVehicleWeight;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding6;
            public float OddballInclusionInnerRadius;
            public float OddballInclusionOuterRadius;
            public float OddballInclusionWeight;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding7;
            public float OddballExclusionInnerRadius;
            public float OddballExclusionOuterRadius;
            public float OddballExclusionWeight;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding8;
            public float HillInclusionInnerRadius;
            public float HillInclusionOuterRadius;
            public float HillInclusionWeight;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding9;
            public float HillExclusionInnerRadius;
            public float HillExclusionOuterRadius;
            public float HillExclusionWeight;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding10;
            public float LastRaceFlagInnerRadius;
            public float LastRaceFlagOuterRadius;
            public float LastRaceFlagWeight;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding11;
            public float DeadAllyInnerRadius;
            public float DeadAllyOuterRadius;
            public float DeadAllyWeight;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding12;
            public float ControlledTerritoryInnerRadius;
            public float ControlledTerritoryOuterRadius;
            public float ControlledTerritoryWeight;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding13;
            [TagField(Length = 0x230, Flags = TagFieldFlags.Padding)]
            public byte[] Padding14;
            [TagField(Length = 0x30, Flags = TagFieldFlags.Padding)]
            public byte[] Padding15;
            public List<MultiplayerConstantsBlock> MultiplayerConstants;
            public List<GameEngineStatusResponseBlock> StateResponses;
            [TagField(ValidTags = new [] { "nhdt" })]
            public CachedTag ScoreboardHudDefinition;
            [TagField(ValidTags = new [] { "shad" })]
            public CachedTag ScoreboardEmblemShader;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag ScoreboardEmblemBitmap;
            [TagField(ValidTags = new [] { "shad" })]
            public CachedTag ScoreboardDeadEmblemShader;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag ScoreboardDeadEmblemBitmap;
            
            [TagStructure(Size = 0x8)]
            public class WeaponsBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "item" })]
                public CachedTag Weapon;
            }
            
            [TagStructure(Size = 0x8)]
            public class VehiclesBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "vehi" })]
                public CachedTag Vehicle;
            }
            
            [TagStructure(Size = 0x10)]
            public class GrenadeAndPowerupStructBlock : TagStructure
            {
                public List<GrenadeBlock> Grenades;
                public List<PowerupBlock> Powerups;
                
                [TagStructure(Size = 0x8)]
                public class GrenadeBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "eqip" })]
                    public CachedTag Weapon;
                }
                
                [TagStructure(Size = 0x8)]
                public class PowerupBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "eqip" })]
                    public CachedTag Weapon;
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class SoundsBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Sound;
            }
            
            [TagStructure(Size = 0xA8)]
            public class GameEngineGeneralEventBlock : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public EventValue Event;
                public AudienceValue Audience;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public StringId DisplayString;
                public RequiredFieldValue RequiredField;
                public ExcludedAudienceValue ExcludedAudience;
                public StringId PrimaryString;
                public int PrimaryStringDuration; // seconds
                public StringId PluralDisplayString;
                [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                public float SoundDelayAnnouncerOnly;
                public SoundFlagsValue SoundFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding4;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Sound;
                public SoundResponseExtraSoundsStructBlock ExtraSounds;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding5;
                [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                public byte[] Padding6;
                public List<SoundResponseDefinitionBlock> SoundPermutations;
                
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
                
                [TagStructure(Size = 0x40)]
                public class SoundResponseExtraSoundsStructBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag JapaneseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag GermanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag FrenchSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag SpanishSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ItalianSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag KoreanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ChineseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag PortugueseSound;
                }
                
                [TagStructure(Size = 0x50)]
                public class SoundResponseDefinitionBlock : TagStructure
                {
                    public SoundFlagsValue SoundFlags;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag EnglishSound;
                    public SoundResponseExtraSoundsStructBlock ExtraSounds;
                    public float Probability;
                    
                    [Flags]
                    public enum SoundFlagsValue : ushort
                    {
                        AnnouncerSound = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x40)]
                    public class SoundResponseExtraSoundsStructBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag JapaneseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag GermanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag FrenchSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag SpanishSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ItalianSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag KoreanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ChineseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag PortugueseSound;
                    }
                }
            }
            
            [TagStructure(Size = 0xA8)]
            public class GameEngineFlavorEventBlock : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public EventValue Event;
                public AudienceValue Audience;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public StringId DisplayString;
                public RequiredFieldValue RequiredField;
                public ExcludedAudienceValue ExcludedAudience;
                public StringId PrimaryString;
                public int PrimaryStringDuration; // seconds
                public StringId PluralDisplayString;
                [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                public float SoundDelayAnnouncerOnly;
                public SoundFlagsValue SoundFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding4;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Sound;
                public SoundResponseExtraSoundsStructBlock ExtraSounds;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding5;
                [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                public byte[] Padding6;
                public List<SoundResponseDefinitionBlock> SoundPermutations;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    QuantityMessage = 1 << 0
                }
                
                public enum EventValue : short
                {
                    DoubleKill,
                    TripleKill,
                    Killtacular,
                    KillingSpree,
                    RunningRiot,
                    WellPlacedKill,
                    BrokeKillingSpree,
                    KillFrenzy,
                    Killtrocity,
                    Killimajaro,
                    _15InARow,
                    _20InARow,
                    _25InARow
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
                
                [TagStructure(Size = 0x40)]
                public class SoundResponseExtraSoundsStructBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag JapaneseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag GermanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag FrenchSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag SpanishSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ItalianSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag KoreanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ChineseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag PortugueseSound;
                }
                
                [TagStructure(Size = 0x50)]
                public class SoundResponseDefinitionBlock : TagStructure
                {
                    public SoundFlagsValue SoundFlags;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag EnglishSound;
                    public SoundResponseExtraSoundsStructBlock ExtraSounds;
                    public float Probability;
                    
                    [Flags]
                    public enum SoundFlagsValue : ushort
                    {
                        AnnouncerSound = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x40)]
                    public class SoundResponseExtraSoundsStructBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag JapaneseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag GermanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag FrenchSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag SpanishSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ItalianSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag KoreanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ChineseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag PortugueseSound;
                    }
                }
            }
            
            [TagStructure(Size = 0xA8)]
            public class GameEngineSlayerEventBlock : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public EventValue Event;
                public AudienceValue Audience;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public StringId DisplayString;
                public RequiredFieldValue RequiredField;
                public ExcludedAudienceValue ExcludedAudience;
                public StringId PrimaryString;
                public int PrimaryStringDuration; // seconds
                public StringId PluralDisplayString;
                [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                public float SoundDelayAnnouncerOnly;
                public SoundFlagsValue SoundFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding4;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Sound;
                public SoundResponseExtraSoundsStructBlock ExtraSounds;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding5;
                [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                public byte[] Padding6;
                public List<SoundResponseDefinitionBlock> SoundPermutations;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    QuantityMessage = 1 << 0
                }
                
                public enum EventValue : short
                {
                    GameStart,
                    NewTarget
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
                
                [TagStructure(Size = 0x40)]
                public class SoundResponseExtraSoundsStructBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag JapaneseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag GermanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag FrenchSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag SpanishSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ItalianSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag KoreanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ChineseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag PortugueseSound;
                }
                
                [TagStructure(Size = 0x50)]
                public class SoundResponseDefinitionBlock : TagStructure
                {
                    public SoundFlagsValue SoundFlags;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag EnglishSound;
                    public SoundResponseExtraSoundsStructBlock ExtraSounds;
                    public float Probability;
                    
                    [Flags]
                    public enum SoundFlagsValue : ushort
                    {
                        AnnouncerSound = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x40)]
                    public class SoundResponseExtraSoundsStructBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag JapaneseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag GermanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag FrenchSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag SpanishSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ItalianSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag KoreanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ChineseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag PortugueseSound;
                    }
                }
            }
            
            [TagStructure(Size = 0xA8)]
            public class GameEngineCtfEventBlock : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public EventValue Event;
                public AudienceValue Audience;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public StringId DisplayString;
                public RequiredFieldValue RequiredField;
                public ExcludedAudienceValue ExcludedAudience;
                public StringId PrimaryString;
                public int PrimaryStringDuration; // seconds
                public StringId PluralDisplayString;
                [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                public float SoundDelayAnnouncerOnly;
                public SoundFlagsValue SoundFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding4;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Sound;
                public SoundResponseExtraSoundsStructBlock ExtraSounds;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding5;
                [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                public byte[] Padding6;
                public List<SoundResponseDefinitionBlock> SoundPermutations;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    QuantityMessage = 1 << 0
                }
                
                public enum EventValue : short
                {
                    GameStart,
                    FlagTaken,
                    FlagDropped,
                    FlagReturnedByPlayer,
                    FlagReturnedByTimeout,
                    FlagCaptured,
                    FlagNewDefensiveTeam,
                    FlagReturnFaliure,
                    SideSwitchTick,
                    SideSwitchFinalTick,
                    SideSwitch30Seconds,
                    SideSwitch10Seconds,
                    FlagContested,
                    FlagCaptureFaliure
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
                
                [TagStructure(Size = 0x40)]
                public class SoundResponseExtraSoundsStructBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag JapaneseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag GermanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag FrenchSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag SpanishSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ItalianSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag KoreanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ChineseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag PortugueseSound;
                }
                
                [TagStructure(Size = 0x50)]
                public class SoundResponseDefinitionBlock : TagStructure
                {
                    public SoundFlagsValue SoundFlags;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag EnglishSound;
                    public SoundResponseExtraSoundsStructBlock ExtraSounds;
                    public float Probability;
                    
                    [Flags]
                    public enum SoundFlagsValue : ushort
                    {
                        AnnouncerSound = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x40)]
                    public class SoundResponseExtraSoundsStructBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag JapaneseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag GermanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag FrenchSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag SpanishSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ItalianSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag KoreanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ChineseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag PortugueseSound;
                    }
                }
            }
            
            [TagStructure(Size = 0xA8)]
            public class GameEngineOddballEventBlock : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public EventValue Event;
                public AudienceValue Audience;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public StringId DisplayString;
                public RequiredFieldValue RequiredField;
                public ExcludedAudienceValue ExcludedAudience;
                public StringId PrimaryString;
                public int PrimaryStringDuration; // seconds
                public StringId PluralDisplayString;
                [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                public float SoundDelayAnnouncerOnly;
                public SoundFlagsValue SoundFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding4;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Sound;
                public SoundResponseExtraSoundsStructBlock ExtraSounds;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding5;
                [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                public byte[] Padding6;
                public List<SoundResponseDefinitionBlock> SoundPermutations;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    QuantityMessage = 1 << 0
                }
                
                public enum EventValue : short
                {
                    GameStart,
                    BallSpawned,
                    BallPickedUp,
                    BallDropped,
                    BallReset,
                    BallTick
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
                
                [TagStructure(Size = 0x40)]
                public class SoundResponseExtraSoundsStructBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag JapaneseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag GermanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag FrenchSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag SpanishSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ItalianSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag KoreanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ChineseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag PortugueseSound;
                }
                
                [TagStructure(Size = 0x50)]
                public class SoundResponseDefinitionBlock : TagStructure
                {
                    public SoundFlagsValue SoundFlags;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag EnglishSound;
                    public SoundResponseExtraSoundsStructBlock ExtraSounds;
                    public float Probability;
                    
                    [Flags]
                    public enum SoundFlagsValue : ushort
                    {
                        AnnouncerSound = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x40)]
                    public class SoundResponseExtraSoundsStructBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag JapaneseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag GermanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag FrenchSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag SpanishSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ItalianSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag KoreanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ChineseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag PortugueseSound;
                    }
                }
            }
            
            [TagStructure()]
            public class GNullBlock : TagStructure
            {
            }
            
            [TagStructure(Size = 0xA8)]
            public class GameEngineKingEventBlock : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public EventValue Event;
                public AudienceValue Audience;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public StringId DisplayString;
                public RequiredFieldValue RequiredField;
                public ExcludedAudienceValue ExcludedAudience;
                public StringId PrimaryString;
                public int PrimaryStringDuration; // seconds
                public StringId PluralDisplayString;
                [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                public float SoundDelayAnnouncerOnly;
                public SoundFlagsValue SoundFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding4;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Sound;
                public SoundResponseExtraSoundsStructBlock ExtraSounds;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding5;
                [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                public byte[] Padding6;
                public List<SoundResponseDefinitionBlock> SoundPermutations;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    QuantityMessage = 1 << 0
                }
                
                public enum EventValue : short
                {
                    GameStart,
                    HillControlled,
                    HillContested,
                    HillTick,
                    HillMove,
                    HillControlledTeam,
                    HillContestedTeam
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
                
                [TagStructure(Size = 0x40)]
                public class SoundResponseExtraSoundsStructBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag JapaneseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag GermanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag FrenchSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag SpanishSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ItalianSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag KoreanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ChineseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag PortugueseSound;
                }
                
                [TagStructure(Size = 0x50)]
                public class SoundResponseDefinitionBlock : TagStructure
                {
                    public SoundFlagsValue SoundFlags;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag EnglishSound;
                    public SoundResponseExtraSoundsStructBlock ExtraSounds;
                    public float Probability;
                    
                    [Flags]
                    public enum SoundFlagsValue : ushort
                    {
                        AnnouncerSound = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x40)]
                    public class SoundResponseExtraSoundsStructBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag JapaneseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag GermanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag FrenchSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag SpanishSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ItalianSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag KoreanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ChineseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag PortugueseSound;
                    }
                }
            }
            
            [TagStructure()]
            public class GNullBlock1 : TagStructure
            {
            }
            
            [TagStructure(Size = 0xA8)]
            public class GameEngineJuggernautEventBlock : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public EventValue Event;
                public AudienceValue Audience;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public StringId DisplayString;
                public RequiredFieldValue RequiredField;
                public ExcludedAudienceValue ExcludedAudience;
                public StringId PrimaryString;
                public int PrimaryStringDuration; // seconds
                public StringId PluralDisplayString;
                [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                public float SoundDelayAnnouncerOnly;
                public SoundFlagsValue SoundFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding4;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Sound;
                public SoundResponseExtraSoundsStructBlock ExtraSounds;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding5;
                [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                public byte[] Padding6;
                public List<SoundResponseDefinitionBlock> SoundPermutations;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    QuantityMessage = 1 << 0
                }
                
                public enum EventValue : short
                {
                    GameStart,
                    NewJuggernaut,
                    JuggernautKilled
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
                
                [TagStructure(Size = 0x40)]
                public class SoundResponseExtraSoundsStructBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag JapaneseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag GermanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag FrenchSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag SpanishSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ItalianSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag KoreanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ChineseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag PortugueseSound;
                }
                
                [TagStructure(Size = 0x50)]
                public class SoundResponseDefinitionBlock : TagStructure
                {
                    public SoundFlagsValue SoundFlags;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag EnglishSound;
                    public SoundResponseExtraSoundsStructBlock ExtraSounds;
                    public float Probability;
                    
                    [Flags]
                    public enum SoundFlagsValue : ushort
                    {
                        AnnouncerSound = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x40)]
                    public class SoundResponseExtraSoundsStructBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag JapaneseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag GermanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag FrenchSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag SpanishSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ItalianSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag KoreanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ChineseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag PortugueseSound;
                    }
                }
            }
            
            [TagStructure(Size = 0xA8)]
            public class GameEngineTerritoriesEventBlock : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public EventValue Event;
                public AudienceValue Audience;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public StringId DisplayString;
                public RequiredFieldValue RequiredField;
                public ExcludedAudienceValue ExcludedAudience;
                public StringId PrimaryString;
                public int PrimaryStringDuration; // seconds
                public StringId PluralDisplayString;
                [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                public float SoundDelayAnnouncerOnly;
                public SoundFlagsValue SoundFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding4;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Sound;
                public SoundResponseExtraSoundsStructBlock ExtraSounds;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding5;
                [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                public byte[] Padding6;
                public List<SoundResponseDefinitionBlock> SoundPermutations;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    QuantityMessage = 1 << 0
                }
                
                public enum EventValue : short
                {
                    GameStart,
                    TerritoryControlGained,
                    TerritoryContestLost,
                    AllTerritoriesCntrld,
                    TeamTerritoryCtrlGained,
                    TeamTerritoryCtrlLost,
                    TeamAllTerritoriesCntrld
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
                
                [TagStructure(Size = 0x40)]
                public class SoundResponseExtraSoundsStructBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag JapaneseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag GermanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag FrenchSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag SpanishSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ItalianSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag KoreanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ChineseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag PortugueseSound;
                }
                
                [TagStructure(Size = 0x50)]
                public class SoundResponseDefinitionBlock : TagStructure
                {
                    public SoundFlagsValue SoundFlags;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag EnglishSound;
                    public SoundResponseExtraSoundsStructBlock ExtraSounds;
                    public float Probability;
                    
                    [Flags]
                    public enum SoundFlagsValue : ushort
                    {
                        AnnouncerSound = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x40)]
                    public class SoundResponseExtraSoundsStructBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag JapaneseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag GermanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag FrenchSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag SpanishSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ItalianSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag KoreanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ChineseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag PortugueseSound;
                    }
                }
            }
            
            [TagStructure(Size = 0xA8)]
            public class GameEngineAssaultEventBlock : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public EventValue Event;
                public AudienceValue Audience;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public StringId DisplayString;
                public RequiredFieldValue RequiredField;
                public ExcludedAudienceValue ExcludedAudience;
                public StringId PrimaryString;
                public int PrimaryStringDuration; // seconds
                public StringId PluralDisplayString;
                [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                public float SoundDelayAnnouncerOnly;
                public SoundFlagsValue SoundFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding4;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Sound;
                public SoundResponseExtraSoundsStructBlock ExtraSounds;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding5;
                [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                public byte[] Padding6;
                public List<SoundResponseDefinitionBlock> SoundPermutations;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    QuantityMessage = 1 << 0
                }
                
                public enum EventValue : short
                {
                    GameStart,
                    BombTaken,
                    BombDropped,
                    BombReturnedByPlayer,
                    BombReturnedByTimeout,
                    BombCaptured,
                    BombNewDefensiveTeam,
                    BombReturnFaliure,
                    SideSwitchTick,
                    SideSwitchFinalTick,
                    SideSwitch30Seconds,
                    SideSwitch10Seconds,
                    BombReturnedByDefusing,
                    BombPlacedOnEnemyPost,
                    BombArmingStarted,
                    BombArmingCompleted,
                    BombContested
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
                
                [TagStructure(Size = 0x40)]
                public class SoundResponseExtraSoundsStructBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag JapaneseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag GermanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag FrenchSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag SpanishSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ItalianSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag KoreanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ChineseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag PortugueseSound;
                }
                
                [TagStructure(Size = 0x50)]
                public class SoundResponseDefinitionBlock : TagStructure
                {
                    public SoundFlagsValue SoundFlags;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag EnglishSound;
                    public SoundResponseExtraSoundsStructBlock ExtraSounds;
                    public float Probability;
                    
                    [Flags]
                    public enum SoundFlagsValue : ushort
                    {
                        AnnouncerSound = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x40)]
                    public class SoundResponseExtraSoundsStructBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag JapaneseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag GermanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag FrenchSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag SpanishSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ItalianSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag KoreanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ChineseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag PortugueseSound;
                    }
                }
            }
            
            [TagStructure()]
            public class GNullBlock2 : TagStructure
            {
            }
            
            [TagStructure()]
            public class GNullBlock3 : TagStructure
            {
            }
            
            [TagStructure()]
            public class GNullBlock4 : TagStructure
            {
            }
            
            [TagStructure()]
            public class GNullBlock5 : TagStructure
            {
            }
            
            [TagStructure(Size = 0x160)]
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
                /// <summary>
                /// how nearby a player is to count a vehicle as 'occupied'
                /// </summary>
                public float VehicleNearbyPlayerDist;
                [TagField(Length = 0x54, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                [TagField(ValidTags = new [] { "shad" })]
                public CachedTag HillShader;
                [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                public float FlagResetStopDistance;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag BombExplodeEffect;
                [TagField(ValidTags = new [] { "jpt!" })]
                public CachedTag BombExplodeDmgEffect;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag BombDefuseEffect;
                public StringId BombDefusalString;
                public StringId BlockedTeleporterString;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding4;
                [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
                public byte[] Padding5;
                [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
                public byte[] Padding6;
                [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
                public byte[] Padding7;
            }
            
            [TagStructure(Size = 0x1C)]
            public class GameEngineStatusResponseBlock : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public StateValue State;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public StringId FfaMessage;
                public StringId TeamMessage;
                public CachedTag Unknown;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                
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

