using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "multiplayer_globals", Tag = "mulg", Size = 0x18)]
    public class MultiplayerGlobals : TagStructure
	{
        public List<UniversalBlock> Universal;
        public List<RuntimeBlock> Runtime;

        [TagStructure(Size = 0xB4, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0xD8, MaxVersion = CacheVersion.HaloOnline449175)]
        [TagStructure(Size = 0xD0, MinVersion = CacheVersion.HaloOnline498295)]
        public class UniversalBlock : TagStructure
		{
            public CachedTag RandomPlayerNameStrings;
            public CachedTag TeamNameStrings;
            public List<TeamColor> TeamColors;

            public List<ArmorCustomizationBlock> ArmorCustomization;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public List<Consumable> Equipment;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public List<EnergyRegenerationBlock> EnergyRegeneration;

            public CachedTag MultiplayerStrings;
            public CachedTag SandboxUiStrings;
            public CachedTag SandboxUiProperties;
            public List<GameVariantWeapon> GameVariantWeapons;
            public List<GameVariantVehicle> GameVariantVehicles;
            public List<GameVariantEquipmentBlock> GameVariantEquipment;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public CachedTag Unknown2;
            public List<WeaponSet> WeaponSets;
            public List<VehicleSet> VehicleSets;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public List<PodiumAnimation> PodiumAnimations;

            public CachedTag EngineSettings;

            [TagStructure(Size = 0xC)]
            public class TeamColor : TagStructure
			{
                public RealRgbColor Color;
            }

            [TagStructure(Size = 0x10)]
            public class ArmorCustomizationBlock : TagStructure
			{
                [TagField(Flags = Label)]
                public StringId CharacterName;
                public List<Region> Regions;

                [TagStructure(Size = 0x10)]
                public class Region : TagStructure
				{
                    [TagField(Flags = Label)]
                    public StringId Name;
                    public List<Permutation> Permutations;

                    [TagStructure(Size = 0x1C)]
                    public class Permutation : TagStructure
					{
                        [TagField(Flags = Label)]
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
                        public class VariantBlock : TagStructure
						{
                            [TagField(Flags = Label)]
                            public StringId Region;
                            public StringId Permutation;
                        }
                    }
                }
            }

            [TagStructure(Size = 0x18)]
            public class Consumable : TagStructure
			{
                [TagField(Flags = Label)]
                public StringId Name;
                public CachedTag Object;
                public short Type;

                [TagField(Flags = Padding, Length = 2)]
                public byte[] Unused = new byte[2];
            }

            [TagStructure(Size = 0x8)]
            public class EnergyRegenerationBlock : TagStructure
			{
                public int Duration;
                public int EnergyLevel;
            }

            [TagStructure(Size = 0x18)]
            public class GameVariantWeapon : TagStructure
			{
                [TagField(Flags = Label)]
                public StringId Name;
                public float RandomChance;
                public CachedTag Weapon;
            }

            [TagStructure(Size = 0x14)]
            public class GameVariantVehicle : TagStructure
			{
                [TagField(Flags = Label)]
                public StringId Name;
                public CachedTag Vehicle;
            }

            [TagStructure(Size = 0x14)]
            public class GameVariantEquipmentBlock : TagStructure
			{
                [TagField(Flags = Label)]
                public StringId Name;
                public CachedTag Grenade;
            }

            [TagStructure(Size = 0x10)]
            public class WeaponSet : TagStructure
			{
                [TagField(Flags = Label)]
                public StringId Name;
                public List<Substitution> Substitutions;

                [TagStructure(Size = 0x8)]
                public class Substitution : TagStructure
				{
                    [TagField(Flags = Label)]
                    public StringId OriginalWeapon;
                    public StringId SubstitutedWeapon;
                }
            }

            [TagStructure(Size = 0x10)]
            public class VehicleSet : TagStructure
			{
                [TagField(Flags = Label)]
                public StringId Name;
                public List<Substitution> Substitutions;

                [TagStructure(Size = 0x8)]
                public class Substitution : TagStructure
				{
                    [TagField(Flags = Label)]
                    public StringId OriginalVehicle;
                    public StringId SubstitutedVehicle;
                }
            }

            [TagStructure(Size = 0x4)]
            public class PlayerCharacterType : TagStructure
            {
                public StringId Name;
            }

            [TagStructure(Size = 0x30, MaxVersion = CacheVersion.HaloOnline449175)]
            [TagStructure(Size = 0x40, MinVersion = CacheVersion.HaloOnline498295)]
            public class PodiumAnimation : TagStructure
			{
                [TagField(Flags = Label)]
                public CachedTag AnimationGraph;
                [TagField(MinVersion = CacheVersion.HaloOnline498295)]
                public CachedTag Unknown;
                public StringId DefaultUnarmed;
                public StringId DefaultArmed;
                public List<StanceAnimation> StanceAnimations;
                public List<MoveAnimation> MoveAnimations;

                [TagStructure]
                public class StanceAnimation : TagStructure
				{
                    [TagField(Flags = Label, Length = 32)]
                    public string Name;
                    public StringId BaseAnimation;
                    public StringId LoopAnimation;
                    public StringId UnarmedTransition;
                    public StringId ArmedTransition;
                    public float Unknown;
                }

                [TagStructure]
                public class MoveAnimation : TagStructure
				{
                    [TagField(Flags = Label, Length = 32)]
                    public string Name;
                    public StringId InAnimation;
                    public StringId LoopAnimation;
                    public StringId OutAnimation;
                    public int Unknown;
                    public CachedTag PrimaryWeapon;
                    public CachedTag SecondaryWeapon;
                }
            }
        }

        [TagStructure(Size = 0x20C, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x2A8, MaxVersion = CacheVersion.HaloOnline449175)]
        [TagStructure(Size = 0x308, MinVersion = CacheVersion.HaloOnline498295)]
        public class RuntimeBlock : TagStructure
		{
            public CachedTag SandboxEditorUnit;
            public CachedTag SandboxEditorObject;
            public CachedTag Flag;
            public CachedTag Ball;
            public CachedTag Bomb;
            public CachedTag VipZone;
            public CachedTag InGameStrings;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public CachedTag Unknown;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public CachedTag Unknown2;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public CachedTag Unknown3;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public CachedTag Unknown4;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public CachedTag Unknown5;
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
            public CachedTag ScoreboardEmblemBitmap;
            public CachedTag ScoreboardDeadEmblemBitmap;
            public CachedTag DefaultShapeShader;
            public CachedTag Unknown6;
            public CachedTag CtfIntroUi;
            public CachedTag SlayerIntroUi;
            public CachedTag OddballIntroUi;
            public CachedTag KingOfTheHillIntroUi;
            public CachedTag SandboxIntroUi;
            public CachedTag VipIntroUi;
            public CachedTag JuggernautIntroUi;
            public CachedTag TerritoriesIntroUi;
            public CachedTag AssaultIntroUi;
            public CachedTag InfectionIntroUi;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public CachedTag SimulationInterpolation1;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public CachedTag SimulationInterpolation2;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public CachedTag SimulationInterpolation3;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public CachedTag SimulationInterpolation4;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public CachedTag SimulationInterpolation5;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public CachedTag Unknown13;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public CachedTag MenuMusic1;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public CachedTag MenuMusic2;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public CachedTag MenuMusic3;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public CachedTag MenuMusic4;

            [TagStructure(Size = 0x10)]
            public class Sound : TagStructure
			{
                [TagField(Flags = Label)]
                public CachedTag Type;
            }

            [TagStructure(Size = 0x10)]
            public class LoopingSound : TagStructure
			{
                [TagField(Flags = Label)]
                public CachedTag Type;
            }

			[TagStructure(Size = 0x104, MaxVersion = CacheVersion.Halo3Retail)]
			[TagStructure(Size = 0x108, MaxVersion = CacheVersion.Halo3ODST)]
			[TagStructure(Size = 0x10C, MaxVersion = CacheVersion.HaloOnline449175)]
            [TagStructure(Size = 0x20C, MinVersion = CacheVersion.HaloOnline498295)]
            public class EventBlock : TagStructure
			{
                public ushort Flags;
                public TypeValue Type;
                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public EventValue Event_H3;
                [TagField(Flags = Label, MinVersion = CacheVersion.HaloOnline106708)]
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
                public CachedTag EnglishSound;
                public CachedTag JapaneseSound;
                public CachedTag GermanSound;
                public CachedTag FrenchSound;
                public CachedTag SpanishSound;
                public CachedTag LatinAmericanSpanishSound;
                public CachedTag ItalianSound;
                public CachedTag KoreanSound;
                public CachedTag ChineseTraditionalSound;
                public CachedTag ChineseSimplifiedSound;
                public CachedTag PortugueseSound;
                public CachedTag PolishSound;
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
            public class MultiplayerConstant : TagStructure
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
                public CachedTag HillShader;
                public float Unknown67;
                public float Unknown68;
                public float Unknown69;
                public float Unknown70;
                public CachedTag BombExplodeEffect;
                public CachedTag Unknown71;
                public CachedTag BombExplodeDamageEffect;
                public CachedTag BombDefuseEffect;
                public CachedTag CursorImpactEffect;
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
                public class Weapon : TagStructure
				{
                    [TagField(Flags = Label)]
                    public CachedTag Type;
                    public float Unknown1;
                    public float Unknown2;
                    public float Unknown3;
                    public float Unknown4;
                }

                [TagStructure(Size = 0x20)]
                public class Vehicle : TagStructure
				{
                    [TagField(Flags = Label)]
                    public CachedTag Type;
                    public float Unknown1;
                    public float Unknown2;
                    public float Unknown3;
                    public float Unknown4;
                }

                [TagStructure(Size = 0x1C)]
                public class Projectile : TagStructure
				{
                    [TagField(Flags = Label)]
                    public CachedTag Type;
                    public float Unknown;
                    public float Unknown2;
                    public float Unknown3;
                }

                [TagStructure(Size = 0x14)]
                public class EquipmentBlock : TagStructure
				{
                    [TagField(Flags = Label)]
                    public CachedTag Type;
                    public float Unknown;
                }
            }

            [TagStructure(Size = 0x24)]
            public class StateResponse : TagStructure
			{
                public ushort Flags;
                public short Unknown;
                [TagField(Flags = Label)]
                public StateValue State;
                public short Unknown2;
                public StringId FreeForAllMessage;
                public StringId TeamMessage;
                public CachedTag Unknown3;
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