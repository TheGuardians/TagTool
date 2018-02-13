using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "multiplayer_globals", Tag = "mulg", Size = 0x18)]
    public class MultiplayerGlobals
    {
        public List<UniversalBlock> Universal;
        public List<RuntimeBlock> Runtime;

        [TagStructure(Size = 0xB4, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0xD8, MaxVersion = CacheVersion.HaloOnline449175)]
        [TagStructure(Size = 0xD0, MinVersion = CacheVersion.HaloOnline498295)]
        public class UniversalBlock
        {
            public CachedTagInstance RandomPlayerNameStrings;

            public CachedTagInstance TeamNameStrings;

            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public List<TeamColor> TeamColors;

            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public List<Halo3ArmorCustomizationBlock> ArmorCustomization;

            [TagField(MaxVersion = CacheVersion.HaloOnline449175)]
            public List<HaloOnlineArmorCustomizationBlock> SpartanArmorCustomization;

            [TagField(MaxVersion = CacheVersion.HaloOnline449175)]
            public List<HaloOnlineArmorCustomizationBlock> EliteArmorCustomization;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public List<EquipmentBlock> Equipment;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public List<UnknownBlock> Unknown;

            public CachedTagInstance MultiplayerStrings;
            public CachedTagInstance SandboxUiStrings;
            public CachedTagInstance SandboxUiProperties;
            public List<GameVariantWeapon> GameVariantWeapons;
            public List<GameVariantVehicle> GameVariantVehicles;
            public List<GameVariantEquipmentBlock> GameVariantEquipment;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public CachedTagInstance Unknown2;
            public List<WeaponSet> WeaponSets;
            public List<VehicleSet> VehicleSets;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public List<PodiumAnimation> PodiumAnimations;
            public CachedTagInstance EngineSettings;

            [TagStructure(Size = 0xC)]
            public class TeamColor
            {
                public RealRgbColor Color;
            }

            [TagStructure(Size = 0x10)]
            public class Halo3ArmorCustomizationBlock
            {
                [TagField(Label = true)]
                public StringId CharacterName;
                public List<Region> Regions;

                [TagStructure(Size = 0x10)]
                public class Region
                {
                    [TagField(Label = true)]
                    public StringId Name;
                    public List<Permuation> Permuations;

                    [TagStructure(Size = 0x1C)]
                    public class Permuation
                    {
                        [TagField(Label = true)]
                        public StringId Name;
                        public StringId Description;
                        public FlagsValue Flags;
                        public short Unknown;
                        public StringId AchievementRequirement;
                        public List<VariantBlock> Variant;

                        [Flags]
                        public enum FlagsValue : ushort
                        {
                            None = 0,
                            HasRequirement = 1 << 0,
                            HasSpecialRequirement = 1 << 1,
                            Bit2 = 1 << 2,
                            Bit3 = 1 << 3,
                            Bit4 = 1 << 4,
                            Bit5 = 1 << 5,
                            Bit6 = 1 << 6,
                            Bit7 = 1 << 7,
                            Bit8 = 1 << 8,
                            Bit9 = 1 << 9,
                            Bit10 = 1 << 10,
                            Bit11 = 1 << 11,
                            Bit12 = 1 << 12,
                            Bit13 = 1 << 13,
                            Bit14 = 1 << 14,
                            Bit15 = 1 << 15
                        }

                        [TagStructure(Size = 0x8)]
                        public class VariantBlock
                        {
                            [TagField(Label = true)]
                            public StringId Region;
                            public StringId Permutation;
                        }
                    }
                }
            }

            [TagStructure(Size = 0x14)]
            public class HaloOnlineArmorCustomizationBlock
            {
                [TagField(Label = true)]
                public StringId ArmorObjectRegion;
                public StringId BipedRegion;
                public List<Permutation> Permutations;

                [TagStructure(Size = 0x30)]
                public class Permutation
                {
                    [TagField(Label = true)]
                    public StringId Name;
                    public CachedTagInstance ThirdPersonArmorObject;
                    public CachedTagInstance FirstPersonArmorModel;
                    public short Unknown;
                    public short Unknown2;
                    public StringId ParentAttachMarker;
                    public StringId ChildAttachMarker;
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class EquipmentBlock
            {
                [TagField(Label = true)]
                public StringId Name;
                public CachedTagInstance Equipment;
                public short Unknown;
                public short Unknown2;
            }

            [TagStructure(Size = 0x8)]
            public class UnknownBlock
            {
                public int Unknown;
                public int Unknown2;
            }

            [TagStructure(Size = 0x18)]
            public class GameVariantWeapon
            {
                [TagField(Label = true)]
                public StringId Name;
                public float RandomChance;
                public CachedTagInstance Weapon;
            }

            [TagStructure(Size = 0x14)]
            public class GameVariantVehicle
            {
                [TagField(Label = true)]
                public StringId Name;
                public CachedTagInstance Vehicle;
            }

            [TagStructure(Size = 0x14)]
            public class GameVariantEquipmentBlock
            {
                [TagField(Label = true)]
                public StringId Name;
                public CachedTagInstance Grenade;
            }

            [TagStructure(Size = 0x10)]
            public class WeaponSet
            {
                [TagField(Label = true)]
                public StringId Name;
                public List<Substitution> Substitutions;

                [TagStructure(Size = 0x8)]
                public class Substitution
                {
                    [TagField(Label = true)]
                    public StringId OriginalWeapon;
                    public StringId SubstitutedWeapon;
                }
            }

            [TagStructure(Size = 0x10)]
            public class VehicleSet
            {
                [TagField(Label = true)]
                public StringId Name;
                public List<Substitution> Substitutions;

                [TagStructure(Size = 0x8)]
                public class Substitution
                {
                    [TagField(Label = true)]
                    public StringId OriginalVehicle;
                    public StringId SubstitutedVehicle;
                }
            }

            [TagStructure(Size = 0x30, MaxVersion = CacheVersion.HaloOnline449175)]
            [TagStructure(Size = 0x40, MinVersion = CacheVersion.HaloOnline498295)]
            public class PodiumAnimation
            {
                [TagField(Label = true)]
                public CachedTagInstance AnimationGraph;
                [TagField(MinVersion = CacheVersion.HaloOnline498295)]
                public CachedTagInstance Unknown;
                public StringId DefaultUnarmed;
                public StringId DefaultArmed;
                public List<StanceAnimation> StanceAnimations;
                public List<MoveAnimation> MoveAnimations;

                [TagStructure]
                public class StanceAnimation
                {
                    [TagField(Label = true, Length = 32)]
                    public string Name;
                    public StringId BaseAnimation;
                    public StringId LoopAnimation;
                    public StringId UnarmedTransition;
                    public StringId ArmedTransition;
                    public float Unknown;
                }

                [TagStructure]
                public class MoveAnimation
                {
                    [TagField(Label = true, Length = 32)]
                    public string Name;
                    public StringId InAnimation;
                    public StringId LoopAnimation;
                    public StringId OutAnimation;
                    public int Unknown;
                    public CachedTagInstance PrimaryWeapon;
                    public CachedTagInstance SecondaryWeapon;
                }
            }
        }

        [TagStructure(Size = 0x20C, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x2A8, MaxVersion = CacheVersion.HaloOnline449175)]
        [TagStructure(Size = 0x308, MinVersion = CacheVersion.HaloOnline498295)]
        public class RuntimeBlock
        {
            public CachedTagInstance SandboxEditorUnit;
            public CachedTagInstance SandboxEditorObject;
            public CachedTagInstance Flag;
            public CachedTagInstance Ball;
            public CachedTagInstance Bomb;
            public CachedTagInstance VipZone;
            public CachedTagInstance InGameStrings;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public CachedTagInstance Unknown;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public CachedTagInstance Unknown2;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public CachedTagInstance Unknown3;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public CachedTagInstance Unknown4;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public CachedTagInstance Unknown5;
            public List<Sound> Sounds;
            public List<LoopingSound> LoopingSounds;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public List<EventBlock> UnknownEvents;
            public List<EventBlock> GeneralEvents;
            public List<EventBlock> FlavorEvents;
            public List<EventBlock> SlayerEvents;
            public List<EventBlock> CtfEvents;
            public List<EventBlock> OddballEvents;
            public List<EventBlock> KingOfTheHillEvents;
            public List<EventBlock> VipEvents;
            public List<EventBlock> JuggernautEvents;
            public List<EventBlock> TerritoriesEvents;
            public List<EventBlock> AssaultEvents;
            public List<EventBlock> InfectionEvents;
            public int DefaultFragGrenadeCount;
            public int DefaultPlasmaGrenadeCount;
            public List<MultiplayerConstant> MultiplayerConstants;
            public List<StateResponse> StateResponses;
            public CachedTagInstance ScoreboardEmblemBitmap;
            public CachedTagInstance ScoreboardDeadEmblemBitmap;
            public CachedTagInstance DefaultShapeShader;
            public CachedTagInstance Unknown6;
            public CachedTagInstance CtfIntroUi;
            public CachedTagInstance SlayerIntroUi;
            public CachedTagInstance OddballIntroUi;
            public CachedTagInstance KingOfTheHillIntroUi;
            public CachedTagInstance SandboxIntroUi;
            public CachedTagInstance VipIntroUi;
            public CachedTagInstance JuggernautIntroUi;
            public CachedTagInstance TerritoriesIntroUi;
            public CachedTagInstance AssaultIntroUi;
            public CachedTagInstance InfectionIntroUi;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public CachedTagInstance SimulationInterpolation1;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public CachedTagInstance SimulationInterpolation2;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public CachedTagInstance SimulationInterpolation3;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public CachedTagInstance SimulationInterpolation4;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public CachedTagInstance SimulationInterpolation5;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public CachedTagInstance Unknown13;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public CachedTagInstance MenuMusic1;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public CachedTagInstance MenuMusic2;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public CachedTagInstance MenuMusic3;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public CachedTagInstance MenuMusic4;

            [TagStructure(Size = 0x10)]
            public class Sound
            {
                [TagField(Label = true)]
                public CachedTagInstance Type;
            }

            [TagStructure(Size = 0x10)]
            public class LoopingSound
            {
                [TagField(Label = true)]
                public CachedTagInstance Type;
            }

            [TagStructure(Size = 0x104, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x10C, MaxVersion = CacheVersion.HaloOnline449175)]
            [TagStructure(Size = 0x20C, MinVersion = CacheVersion.HaloOnline498295)]
            public class EventBlock
            {
                public ushort Flags;
                public TypeValue Type;
                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public EventValue Event_H3;
                [TagField(Label = true, MinVersion = CacheVersion.HaloOnline106708)]
                public StringId Event;
                [TagField(Length = 256, MinVersion = CacheVersion.HaloOnline498295)]
                public string Unknown_;
                public AudienceValue Audience;
                public short Unknown;
                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public short Unknown2_;
                public TeamValue Team;
                public short Unknown2;
                public StringId DisplayString;
                public StringId DisplayMedal;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public uint Unknown3;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public uint Unknown4;
                public RequiredFieldValue RequiredField;
                public RequiredFieldValue ExcludedAudience;
                public RequiredFieldValue RequiredField2;
                public RequiredFieldValue ExcludedAudience2;
                public StringId PrimaryString;
                public int PrimaryStringDuration;
                public StringId PluralDisplayString;
                public float SoundDelayAnnouncerOnly;
                public ushort SoundFlags;
                public short Unknown5;
                public CachedTagInstance EnglishSound;
                public CachedTagInstance JapaneseSound;
                public CachedTagInstance GermanSound;
                public CachedTagInstance FrenchSound;
                public CachedTagInstance SpanishSound;
                public CachedTagInstance LatinAmericanSpanishSound;
                public CachedTagInstance ItalianSound;
                public CachedTagInstance KoreanSound;
                public CachedTagInstance ChineseTraditionalSound;
                public CachedTagInstance ChineseSimplifiedSound;
                public CachedTagInstance PortugueseSound;
                public CachedTagInstance PolishSound;
                public uint Unknown6;
                public uint Unknown7;
                public uint Unknown8;
                public uint Unknown9;

                public enum TypeValue : short
                {
                    General,
                    Flavor,
                    Slayer,
                    CaptureTheFlag,
                    Oddball,
                    Unused,
                    KingOfTheHill,
                    Vip,
                    Juggernaut,
                    Territories,
                    Assault,
                    Infection,
                    Survival,
                    Unknown,
                }

                public enum EventValue : short
                {
                    Kill,
                    Suicide,
                    Betrayal,
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
                    TeleporterBlocked,
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
                    KillStandardMelee,
                    BoardedVehicle,
                    StartTeamNotice,
                    Telefrag,
                    _10SecsToWin,
                    Team10SecsToWin,
                    KillBulltrue,
                    KillPostMortem,
                    Highjack,
                    Skyjack,
                    KillLaser,
                    KillFire,
                    Wheelman,
                }

                public enum AudienceValue : short
                {
                    CausePlayer,
                    CauseTeam,
                    EffectPlayer,
                    EffectTeam,
                    All
                }

                public enum TeamValue : short
                {
                    NonePlayerOnly,
                    Cause,
                    Effect,
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
            }

            [TagStructure(Size = 0x21C, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x220, MinVersion = CacheVersion.HaloOnline106708)]
            public class MultiplayerConstant
            {
                public float Unknown;
                public float Unknown2;
                public float Unknown3;
                public float Unknown4;
                public float Unknown5;
                public float Unknown6;
                public float Unknown7;
                public float Unknown8;
                public float Unknown9;
                public float Unknown10;
                public float Unknown11;
                public float Unknown12;
                public float Unknown13;
                public float Unknown14;
                public float Unknown15;
                public float Unknown16;
                public float Unknown17;
                public float Unknown18;
                public float Unknown19;
                public float Unknown20;
                public float Unknown21;
                public float Unknown22;
                public float Unknown23;
                public float Unknown24;
                public float Unknown25;
                public float Unknown26;
                public List<Weapon> Weapons;
                public List<Vehicle> Vehicles;
                public List<Projectile> Projectiles;
                public List<EquipmentBlock> Equipment;
                public float Unknown27;
                public float Unknown28;
                public float Unknown29;
                public float Unknown30;
                public float Unknown31;
                public float Unknown32;
                public float Unknown33;
                public float Unknown34;
                public float Unknown35;
                public float Unknown36;
                public float Unknown37;
                public float Unknown38;
                public float Unknown39;
                public float Unknown40;
                public float Unknown41;
                public float Unknown42;
                public float Unknown43;
                public float Unknown44;
                public float Unknown45;
                public float Unknown46;
                public float Unknown47;
                public float Unknown48;
                public float Unknown49;
                public float Unknown50;
                public float Unknown51;
                public float Unknown52;
                public float Unknown53;
                public float Unknown54;
                public float Unknown55;
                public float Unknown56;
                public float Unknown57;
                public float Unknown58;
                public float Unknown59;
                public float Unknown60;
                public float Unknown61;
                public float Unknown62;
                public float Unknown63;
                public float Unknown64;
                public float Unknown65;
                public float Unknown66;
                public float MaximumRandomSpawnBias;
                public float TeleporterRechargeTime;
                public float GrenadeDangerWeight;
                public float GrenadeDangerInnerRadius;
                public float GrenadeDangerOuterRadius;
                public float GrenadeDangerLeadTime;
                public float VehicleDangerMinimumSpeed;
                public float VehicleDangerWeight;
                public float VehicleDangerRadius;
                public float VehicleDangerLeadTime;
                public float VehicleNearbyPlayerDistance;
                public CachedTagInstance HillShader;
                public float Unknown67;
                public float Unknown68;
                public float Unknown69;
                public float Unknown70;
                public CachedTagInstance BombExplodeEffect;
                public CachedTagInstance Unknown71;
                public CachedTagInstance BombExplodeDamageEffect;
                public CachedTagInstance BombDefuseEffect;
                public CachedTagInstance CursorImpactEffect;
                public StringId BombDefusalString;
                public StringId BlockedTeleporterString;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public int Unknown72;
                public StringId Unknown73;
                public StringId SpawnAllowedDefaultRespawnString;
                public StringId SpawnAtPlayerLookingAtSelfString;
                public StringId SpawnAtPlayerLookingAtTargetString;
                public StringId SpawnAtPlayerLookingAtPotentialTargetString;
                public StringId SpawnAtTerritoryAllowedLookingAtTargetString;
                public StringId SpawnAtTerritoryAllowedLookingAtPotentialTargetString;
                public StringId PlayerOutOfLivesString;
                public StringId InvalidSpawnTargetString;
                public StringId TargettedPlayerEnemiesNearbyString;
                public StringId TargettedPlayerUnfriendlyTeamString;
                public StringId TargettedPlayerIsDeadString;
                public StringId TargettedPlayerInCombatString;
                public StringId TargettedPlayerTooFarFromOwnedFlagString;
                public StringId NoAvailableNetpointsString;
                public StringId NetpointContestedString;

                [TagStructure(Size = 0x20)]
                public class Weapon
                {
                    [TagField(Label = true)]
                    public CachedTagInstance Type;
                    public float Unknown1;
                    public float Unknown2;
                    public float Unknown3;
                    public float Unknown4;
                }

                [TagStructure(Size = 0x20)]
                public class Vehicle
                {
                    [TagField(Label = true)]
                    public CachedTagInstance Type;
                    public float Unknown1;
                    public float Unknown2;
                    public float Unknown3;
                    public float Unknown4;
                }

                [TagStructure(Size = 0x1C)]
                public class Projectile
                {
                    [TagField(Label = true)]
                    public CachedTagInstance Type;
                    public float Unknown;
                    public float Unknown2;
                    public float Unknown3;
                }

                [TagStructure(Size = 0x14)]
                public class EquipmentBlock
                {
                    [TagField(Label = true)]
                    public CachedTagInstance Type;
                    public float Unknown;
                }
            }

            [TagStructure(Size = 0x24)]
            public class StateResponse
            {
                public ushort Flags;
                public short Unknown;
                [TagField(Label = true)]
                public StateValue State;
                public short Unknown2;
                public StringId FreeForAllMessage;
                public StringId TeamMessage;
                public CachedTagInstance Unknown3;
                public uint Unknown4;

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
                    GameOverTied2,
                    YouHaveFlag,
                    EnemyHasFlag,
                    FlagNotHome,
                    CarryingOddball,
                    YouAreJuggernaut,
                    YouControlHill,
                    SwitchingSidesSoon,
                    PlayerRecentlyStarted,
                    YouHaveBomb,
                    FlagContested,
                    BombContested,
                    LimitedLivesMultiple,
                    LimitedLivesSingle,
                    LimitedLivesFinal,
                    PlayingWinningUnlimited,
                    PlayingTiedUnlimited,
                    PlayingLosingUnlimited
                }
            }
        }
    }
}