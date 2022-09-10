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
        public List<MultiplayerUniversalBlock> Universal;
        public List<MultiplayerRuntimeBlock> Runtime;

        [TagStructure(Size = 0xB4, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0xD8, MaxVersion = CacheVersion.HaloOnline449175)]
        [TagStructure(Size = 0xD0, MinVersion = CacheVersion.HaloOnline498295)]
        public class MultiplayerUniversalBlock : TagStructure
		{
            public CachedTag RandomPlayerNameStrings;
            public CachedTag TeamNameStrings;

            [TagField(MaxVersion = CacheVersion.HaloOnlineED)]
            public List<MultiplayerColor> TeamColors;

            [TagField(MaxVersion = CacheVersion.HaloOnlineED)]
            public List<CustomizedModelCharacter> CustomizableCharacters;

            [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline235640)]
            public List<CustomizedModelCharacter_HO> SpartanArmorCustomization;

            [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline235640)]
            public List<CustomizedModelCharacter_HO> EliteArmorCustomization;

            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public List<Consumable> Equipment;

            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public List<EnergyRegenerationBlock> EnergyRegeneration;

            public CachedTag MultiplayerStrings;
            public CachedTag SandboxUiStrings;
            public CachedTag SandboxObjectProperties;
            public List<GameVariantWeapon> GameVariantWeapons;
            public List<GameVariantVehicle> GameVariantVehicles;
            public List<GameVariantEquipmentBlock> GameVariantEquipment;

            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public CachedTag Unknown2;

            public List<WeaponSet> WeaponSets;
            public List<VehicleSet> VehicleSets;

            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public List<PodiumAnimation> PodiumAnimations;

            public CachedTag GameEngineSettings;

            [TagStructure(Size = 0xC)]
            public class MultiplayerColor : TagStructure
			{
                public RealRgbColor Color;
            }

            [TagStructure(Size = 0x14)]
            public class CustomizedModelCharacter_HO : TagStructure
            {
                public StringId armorRegion;
                public StringId bipedRegion;

                public List<Permutation> Permutations;

                [TagStructure(Size = 0x30)]
                public class Permutation : TagStructure
                {
                    public StringId Name;
                    public CachedTag ThirdPersonArmorObject;
                    public CachedTag FirstPersonArmorObject;

                    public Int16 unknown1;
                    public Int16 unknown2;

                    public StringId ParentAttachMarker;
                    public StringId ChildAttachMarker;
                }
            }

            [TagStructure(Size = 0x10)]
            public class CustomizedModelCharacter : TagStructure
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
                        public CustomizedModelSelectionFlags Flags;

                        [TagField(Length = 2, Flags = Padding)]
                        public byte[] Padding0;

                        public StringId AchievementRequirement;
                        public List<VariantBlock> Variant;

                        [Flags]
                        public enum CustomizedModelSelectionFlags : ushort
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
                public float RandomChance; // [0-1] used only for random weapon set
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

                [TagStructure(Size = 0x34)]
                public class StanceAnimation : TagStructure
				{
                    [TagField(Flags = Label, Length = 32)]
                    public string Name;
                    public StringId BaseAnimation;
                    public StringId LoopAnimation;
                    public StringId UnarmedTransition;
                    public StringId ArmedTransition;
                    public float CameraDistanceOffset;
                }

                [TagStructure(Size = 0x50, MaxVersion = CacheVersion.HaloOnline449175)]
                [TagStructure(Size = 0x58, MinVersion = CacheVersion.HaloOnline498295)]
                public class MoveAnimation : TagStructure
				{
                    [TagField(Flags = Label, Length = 32)]
                    public string Name;
                    public StringId InAnimation;
                    public StringId LoopAnimation;
                    public StringId OutAnimation;
                    [TagField(MaxVersion = CacheVersion.HaloOnlineED)]
                    public float offset;
                    [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                    public WeaponLoadout WeaponLoadout;
                    public CachedTag CustomPrimaryWeapon;
                    public CachedTag CustomSecondaryWeapon;
                    [TagField(MinVersion = CacheVersion.HaloOnline498295)]
                    public float Unknown2;
                    [TagField(MinVersion = CacheVersion.HaloOnline498295)]
                    public float Unknown3;
                }

                public enum WeaponLoadout : int
                {
                    Unarrmed,
                    LoadoutPrimary,
                    LoadoutSecondary,
                    Custom
                }
            }
        }

        [TagStructure(Size = 0x20C, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x2A8, MaxVersion = CacheVersion.HaloOnline449175)]
        [TagStructure(Size = 0x308, MinVersion = CacheVersion.HaloOnline498295)]
        public class MultiplayerRuntimeBlock : TagStructure
		{
            public CachedTag EditorBiped;
            public CachedTag EditorHelperObject;
            public CachedTag Flag;
            public CachedTag Ball;
            public CachedTag Bomb;
            public CachedTag VipInfluenceArea;
            public CachedTag InGameStrings;

            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public CachedTag Unknown;
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public CachedTag Unknown2;
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public CachedTag Unknown3;
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public CachedTag Unknown4;
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public CachedTag Unknown5;

            public List<Sound> Sounds;
            public List<LoopingSound> LoopingSounds;

            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public List<EventBlock> EarnWpEvents;

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
            public int MaximumFragCount;
            public int MaximumPlasmaCount;
            public List<MultiplayerConstant> MultiplayerConstants;
            public List<StateResponse> StateResponses;
            public CachedTag ScoreboardEmblemBitmap;
            public CachedTag ScoreboardDeadEmblemBitmap;
            public CachedTag HillShader;
            public CachedTag PregameIntroMessage;
            public CachedTag CtfIntroMessage;
            public CachedTag SlayerIntroMessage;
            public CachedTag OddballIntroMessage;
            public CachedTag KingIntroMessage;
            public CachedTag SandboxIntroMessage;
            public CachedTag VipIntroMessage;
            public CachedTag JuggernautIntroMessage;
            public CachedTag TerritoriesIntroMessage;
            public CachedTag AssaultIntroMessage;
            public CachedTag InfectionIntroMessage;

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
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public CachedTag MenuMusic1;
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public CachedTag MenuMusic2;
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public CachedTag MenuMusic3;
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
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
                public GameEngineEventFlags Flags;
                public TypeValue RuntimeEventType;

                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public GameEngineGeneralEventH3 Event_H3;

                [TagField(Flags = Label, MinVersion = CacheVersion.Halo3ODST)]
                public StringId Event;

                [TagField(Length = 256, MinVersion = CacheVersion.HaloOnline498295)]
                public string Unknown1;

                public AudienceValue Audience;
                public short DisplayPriority;
                public short SubPriority;
                public EventResponseContext DisplayContext;

                [TagField(Length = 2, Flags = TagFieldFlags.Padding, MaxVersion = CacheVersion.Halo3Retail)]
                public byte[] Padding0;

                public StringId DisplayString;
                public StringId MedalAward;

                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public short EarnedWp; // earned wp/exp
                [TagField(Length = 2, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.HaloOnlineED)]
                public byte[] Padding1;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public float SurvivalDisplayTime; // seconds

                public EventInputEnum RequiredField;
                public EventInputEnum ExcludedAudience;
                public EventInputEnum SplitscreenSuppression;

                [TagField(Length = 2, Flags = Padding)]
                public byte[] Padding2;

                public StringId PrimaryString;
                public int PrimaryStringDuration;
                public StringId PluralDisplayString;
                public float SoundDelayAnnouncerOnly;
                public SoundResponseFlags SoundFlags;

                [TagField(Length = 2, Flags = Padding)]
                public byte[] Padding3;

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
                public float Probability;
                public List<SoundResponseDefinitionBlock> SoundPermutations;

                [Flags]
                public enum GameEngineEventFlags : ushort
                {
                    QuantityMessage = 1 << 0
                }

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
                    EarnWp, // HO
                }

                public enum GameEngineGeneralEventH3 : short
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
                    Killfalling,
                    Killcollision,
                    Killmelee,
                    SuddenDeath,
                    PlayerBootedPlayer,
                    KillflagCarrier,
                    KillbombCarrier,
                    KillstickyGrenade,
                    Killsniper,
                    KillstMelee,
                    BoardedVehicle,
                    StartTeamNoti,
                    Telefrag,
                    _10SecsToWin,
                    Team10SecsToWin,
                    Bulltrue,
                    DeathFromTheGrave,
                    Hijack,
                    Skyjack,
                    KillspartanLaser,
                    Killflame,
                    AssisttoDriver,
                    Assist,
                    PreGameOver
                }

                public enum AudienceValue : short
                {
                    CausePlayer,
                    CauseTeam,
                    EffectPlayer,
                    EffectTeam,
                    All
                }

                public enum EventResponseContext : short
                {
                    Self,
                    Friendly,
                    Enemy,
                    Neutral,
                    Unknown4, // HO
                    Unknown5  // HO
                }

                public enum EventInputEnum : short
                {
                    None,
                    CausePlayer,
                    CauseTeam,
                    EffectPlayer,
                    EffectTeam
                }

                [Flags]
                public enum SoundResponseFlags : ushort
                {
                    AnnouncerSound = 1 << 0
                }

                [TagStructure(Size = 0xC8)]
                public class SoundResponseDefinitionBlock : TagStructure
                {
                    public GameEngineSoundResponseFlagsDefinition SoundFlags;
                    [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
                    public byte[] AGQD;
                    public CachedTag EnglishSound;
                    public CachedTag JapaneseSound;
                    public CachedTag GermanSound;
                    public CachedTag FrenchSound;
                    public CachedTag SpanishSound;
                    public CachedTag MexicanSound;
                    public CachedTag ItalianSound;
                    public CachedTag KoreanSound;
                    public CachedTag ChineseSoundtraditional;
                    public CachedTag ChineseSoundsimplified;
                    public CachedTag PortugueseSound;
                    public CachedTag PolishSound;
                    public float Probability;

                    [Flags]
                    public enum GameEngineSoundResponseFlagsDefinition : ushort
                    {
                        AnnouncerSound = 1 << 0
                    }
                }
            }

            [TagStructure(Size = 0x21C, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x220, MinVersion = CacheVersion.HaloOnlineED)]
            public class MultiplayerConstant : TagStructure
            {
                public SpawnConstantStruct ForbidEnemySpawnConstants;
                public SpawnConstantStruct EnemySpawnBiasConstants;
                public SpawnConstantStruct AllySpawnBias;
                public SpawnConstantStruct SpectatedAllySpawnBias;
                public SpawnConstantStruct ForbidAllySpawnConstants;
                public float DeadTeammateInfluenceDuration; // seconds

                public List<WeaponSpawnInfluence> Weapons;
                public List<VehicleSpawnInfluence> Vehicles;
                public List<ProjectileSpawnInfluence> Projectiles;
                public List<EquipmentSpawnInfluence> Equipment;

                public SpawnConstantStruct KothSpawnConstants;
                public SpawnConstantStruct OddballSpawnConstants;
                public SpawnConstantStruct CtfSpawnConstants;
                public SpawnConstantStruct TeraSpawnConstants;
                public SpawnConstantStruct TerritoriesSpawnConstants;
                public SpawnConstantStruct InfectionHumanSpawnConstants;
                public SpawnConstantStruct InfectionZombieSpawnConstants;
                public SpawnConstantStruct VipSpawnConstants;

                public float MaximumRandomSpawnBias;
                public float TeleporterRechargeTime; // seconds
                public float GrenadeDangerWeight;
                public float GrenadeDangerInnerRadius;
                public float GrenadeDangerOuterRadius;
                public float GrenadeDangerLeadTime; // seconds
                public float VehicleDangerMinimumSpeed; // wu/sec
                public float VehicleDangerWeight;
                public float VehicleDangerRadius;
                public float VehicleDangerLeadTime; // seconds
                public float VehicleNearbyPlayerDistance; // how nearby a player is to count a vehicle as 'occupied'

                public CachedTag HillBitmap;
                public float FlagReturnDistance;
                public float FlagContestInnerRadius;
                public float FlagContestOuterRadius;
                public float TerritoriesWaypointVerticalOffset;

                public CachedTag BombExplodeEffect;
                public CachedTag BombExplodeSecondaryEffect;
                public CachedTag BombExplodeDamageEffect;
                public CachedTag BombDefuseEffect;
                public CachedTag ForgeCursorImpactEffect;
                public StringId BombDefusalString;
                public StringId BlockedTeleporterString;

                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public int Unknown72;

                public StringId VoluntaryRespawnControlInstructions;
                public StringId SpawnAllowedDefaultRespawnString;
                public StringId SpawnAtPlayerAllowedLookingAtSelfString;
                public StringId SpawnAtPlayerAllowedLookingAtTargetString;
                public StringId SpawnAtPlayerAllowedLookingAtPotentialTargetString;
                public StringId SpawnAtTerritoryAllowedLookingAtTargetString;
                public StringId SpawnAtTerritoryAllowedLookingAtPotentialTargetString;
                public StringId PlayerOutOfLivesString;
                public StringId InvalidSpawnTargetString;
                public StringId TargetedPlayerEnemiesNearbyString;
                public StringId TargetedPlayerUnfriendlyTeamString;
                public StringId TargetedPlayerIsDeadString;
                public StringId TargetedPlayerInCombatString;
                public StringId TargetedPlayerTooFarFromOwnedFlagString;
                public StringId NoAvailableNetpointsString;
                public StringId NetpointContestedString;

                [TagStructure(Size = 0x14)]
                public class SpawnConstantStruct : TagStructure
                {
                    public float FullWeightRadius; // (world units)
                    public float FalloffRadius; // (world units)
                    public float CylinderUpperHeight; // (world units)
                    public float CylinderLowerHeight; // (world units)
                    public float Weight;
                }

                [TagStructure(Size = 0x20)]
                public class WeaponSpawnInfluence : TagStructure
				{
                    [TagField(Flags = Label)]
                    public CachedTag Weapon;
                    public float FullWeightRadius; // (world units)
                    public float FalloffRadius; // (world units)
                    public float FalloffConeRadius; // (world units)
                    public float Weight;
                }

                [TagStructure(Size = 0x20)]
                public class VehicleSpawnInfluence : TagStructure
				{
                    [TagField(Flags = Label)]
                    public CachedTag Vehicle;
                    public float PillRadius; // (world units)
                    public float LeadTime; // (seconds)
                    public float MinimumVelocity; // (world units per second)
                    public float Weight;
                }

                [TagStructure(Size = 0x1C)]
                public class ProjectileSpawnInfluence : TagStructure
				{
                    [TagField(Flags = Label)]
                    public CachedTag Type;
                    public float LeadTime; // (seconds)
                    public float CollisionCylinderRadius; // (world units)
                    public float Weight;
                }

                [TagStructure(Size = 0x14)]
                public class EquipmentSpawnInfluence : TagStructure
				{
                    [TagField(Flags = Label)]
                    public CachedTag Equipment;
                    public float Weight;
                }
            }

            [TagStructure(Size = 0x24)]
            public class StateResponse : TagStructure
			{
                public GameEngineStatusFlags Flags;

                [TagField(Length = 2, Flags = Padding)]
                public byte[] Padding0;

                [TagField(Flags = Label)]
                public GameEngineStatus State;

                [TagField(Length = 2, Flags = Padding)]
                public byte[] Padding1;

                public StringId FreeForAllMessage;
                public StringId TeamMessage;
                public CachedTag Unused;

                [TagField(Length = 4, Flags = Padding)]
                public byte[] Padding2;

                [Flags]
                public enum GameEngineStatusFlags : ushort
                {
                    Unused = 1 << 0
                }

                public enum GameEngineStatus : short
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
                    PlayingLosingUnlimited,
                    StartingSoon
                }
            }
        }
    }
}