using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using static TagTool.Tags.Definitions.Gen4.MultiplayerGlobals.MultiplayerUniversalBlock;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "multiplayer_globals", Tag = "mulg", Size = 0x18)]
    public class MultiplayerGlobals : TagStructure
	{
        public List<MultiplayerUniversalBlock> Universal;
        public List<MultiplayerRuntimeBlock> Runtime;

        [TagStructure(Size = 0xB4, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0xD0, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.MCC)]
        [TagStructure(Size = 0xD8, MaxVersion = CacheVersion.HaloOnline449175)]
        [TagStructure(Size = 0xD0, MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x5C, MinVersion = CacheVersion.HaloReach)]
        public class MultiplayerUniversalBlock : TagStructure
        {
            [TagField(MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.MCC)]
            public CachedTag MultiplayerZoneRequiredResources;

            [TagField(ValidTags = new[] { "unic" })]
            public CachedTag RandomPlayerNameStrings;
            [TagField(ValidTags = new[] { "unic" })]
            public CachedTag TeamNameStrings;

            [TagField(MaxVersion = CacheVersion.HaloOnlineED)]
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<MultiplayerColor> TeamColors;

            [TagField(MaxVersion = CacheVersion.HaloOnlineED)]
            public List<CustomizedModelCharacter> CustomizableCharacters;

            [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline235640)]
            public List<CustomizedModelCharacter_HO> SpartanArmorCustomization;
            [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline235640)]
            public List<CustomizedModelCharacter_HO> EliteArmorCustomization;
            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public List<Consumable> Equipment;
            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public List<EnergyRegenerationBlock> EnergyRegeneration;

            [TagField(ValidTags = new[] { "unic" })] public CachedTag MultiplayerStrings;
            [TagField(ValidTags = new[] { "unic" })] public CachedTag SandboxUiStrings;
            [TagField(ValidTags = new[] { "jmrq" })] public CachedTag SandboxObjectProperties;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public List<GameVariantWeapon> GameVariantWeapons;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public List<GameVariantVehicle> GameVariantVehicles;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public List<GameVariantGrenadeBlock> GameVariantGrenades;
            [TagField(Platform = CachePlatform.MCC, MaxVersion = CacheVersion.Halo3ODST)]
            public List<GameVariantEquipmentBlock> GameVariantEquipment;

            [TagField(MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
            public CachedTag UnknownHO;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public List<WeaponSet> WeaponSets;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public List<VehicleSet> VehicleSets;

            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public List<PodiumAnimation> PodiumAnimations;

            [TagField(ValidTags = new[] { "wezr" }, MaxVersion = CacheVersion.HaloOnline700123)]
            public CachedTag GameEngineSettings;

            [TagStructure(Size = 0xC)]
            public class MultiplayerColor : TagStructure
			{
                public RealRgbColor Color;
            }

            [TagStructure(Size = 0x14)]
            public class CustomizedModelCharacter_HO : TagStructure
            {
                public StringId ArmorRegion;
                public StringId BipedRegion;

                public List<Permutation> Permutations;

                [TagStructure(Size = 0x30)]
                public class Permutation : TagStructure
                {
                    public StringId Name;
                    public CachedTag ThirdPersonArmorObject;
                    public CachedTag FirstPersonArmorObject;

                    public Int16 Unknown1;
                    public Int16 Unknown2;

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

                    [TagStructure(Size = 0x1C, Platform = CachePlatform.Original)]
                    [TagStructure(Size = 0x14, Platform = CachePlatform.MCC)]
                    public class Permutation : TagStructure
					{
                        [TagField(Flags = Label)]
                        public StringId Name;
                        public StringId Description;

                        [TagField(Platform = CachePlatform.Original)]
                        public CustomizedModelSelectionFlags Flags;
                        [TagField(Platform = CachePlatform.Original, Length = 2, Flags = Padding)]
                        public byte[] Padding0;
                        [TagField(Platform = CachePlatform.Original)]
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

                [TagField(ValidTags = new[] { "eqip" })]
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
                [TagField(ValidTags = new[] { "weap" })]
                public CachedTag Weapon;
            }

            [TagStructure(Size = 0x14)]
            public class GameVariantVehicle : TagStructure
			{
                [TagField(Flags = Label)]
                public StringId Name;
                [TagField(ValidTags = new[] { "vehi" })]
                public CachedTag Vehicle;
            }

            [TagStructure(Size = 0x14)]
            public class GameVariantGrenadeBlock : TagStructure
			{
                [TagField(Flags = Label)]
                public StringId Name;
                [TagField(ValidTags = new[] { "eqip" })]
                public CachedTag Grenade;
            }

            [TagStructure(Size = 0x18)]
            public class GameVariantEquipmentBlock : TagStructure
            {
                [TagField(Flags = Label)]
                public StringId Name;
                public float RandomChance; // [0-1] used only for random weapon set
                [TagField(ValidTags = new[] { "eqip" })]
                public CachedTag Equipment;
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
                [TagField(Flags = Label, ValidTags = new[] { "jmad" })]
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
                    public float Offset;
                    [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                    public WeaponLoadout WeaponLoadout;

                    [TagField(Flags = Label, ValidTags = new[] { "weap" })]
                    public CachedTag CustomPrimaryWeapon;
                    [TagField(Flags = Label, ValidTags = new[] { "weap" })]
                    public CachedTag CustomSecondaryWeapon;

                    [TagField(MinVersion = CacheVersion.HaloOnline498295)]
                    public float Unknown2;
                    [TagField(MinVersion = CacheVersion.HaloOnline498295)]
                    public float Unknown3;
                }

                public enum WeaponLoadout : int
                {
                    Unarmed,
                    LoadoutPrimary,
                    LoadoutSecondary,
                    Custom
                }
            }
        }

        [TagStructure(Size = 0x27C, Version = CacheVersion.Halo3ODST, Platform = CachePlatform.MCC)]
        [TagStructure(Size = 0x20C, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x2A8, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline449175)]
        [TagStructure(Size = 0x308, MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x1D4, MinVersion = CacheVersion.HaloReach)]
        public class MultiplayerRuntimeBlock : TagStructure
        {
            [TagField(ValidTags = new[] { "unit" })] public CachedTag EditorBiped;
            [TagField(ValidTags = new[] { "obje" })] public CachedTag EditorHelperObject;
            [TagField(ValidTags = new[] { "item" })] public CachedTag Flag;
            [TagField(ValidTags = new[] { "item" })] public CachedTag Ball;
            [TagField(ValidTags = new[] { "item" })] public CachedTag Bomb;
            [TagField(ValidTags = new[] { "obje" })] public CachedTag VipInfluenceArea;
            [TagField(ValidTags = new[] { "unic" })] public CachedTag InGameStrings;

            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public HORuntimeFxStruct HaloOnlineRuntimeEffects;

            public List<Sound> Sounds;
            public List<LoopingSound> LoopingSounds;

            [TagField(MinVersion = CacheVersion.HaloReach, ValidTags = new[] { "mgls" })]
            public CachedTag MegaloSounds;
            [TagField(MinVersion = CacheVersion.HaloReach, ValidTags = new[] { "coms" })]
            public CachedTag CommunicationSounds;

            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public List<EventBlock> EarnWpEvents;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)] public List<EventBlock> GeneralEvents;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)] public List<EventBlock> FlavorEvents;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)] public List<EventBlock> SlayerEvents;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)] public List<EventBlock> CtfEvents;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)] public List<EventBlock> OddballEvents;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)] public List<EventBlock> KingOfTheHillEvents;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)] public List<EventBlock> VipEvents;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)] public List<EventBlock> JuggernautEvents;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)] public List<EventBlock> TerritoriesEvents;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)] public List<EventBlock> AssaultEvents;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)] public List<EventBlock> InfectionEvents;

            public int MaximumFragCount;
            public int MaximumPlasmaCount;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<RequisitionConstantsReachBlock> RequisitionConstants;

            public List<MultiplayerConstant> MultiplayerConstants;
            public List<StateResponse> StateResponses;

            [TagField(ValidTags = new[] { "bitm" })] public CachedTag ScoreboardEmblemBitmap;
            [TagField(ValidTags = new[] { "bitm" })] public CachedTag ScoreboardDeadBitmap;
            [TagField(ValidTags = new[] { "rm  " })] public CachedTag HillShader;


            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public IntroMessageStruct GameIntroMessages;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public ReachIntroMessageStruct ReachGameIntroMessages;

            [TagField(MinVersion = CacheVersion.HaloOnline498295, Platform = CachePlatform.Original)]
            [TagField(Version = CacheVersion.Halo3ODST, Platform = CachePlatform.MCC)]
            public SimulationInterpolationStruct SimulationInterpolationDefaults;

            [TagField(ValidTags = new[] { "coop" }, MinVersion = CacheVersion.HaloReach)]
            public CachedTag CoopSpawningGlobals;
            [TagField(ValidTags = new[] { "msit" }, MinVersion = CacheVersion.HaloReach)]
            public CachedTag MegaloStringIdTable;

            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public CachedTag MusicFirstPlace;
            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public CachedTag MusicSecondPlace;
            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public CachedTag MusicThirdPlace;
            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public CachedTag MusicPostMatch;

            [TagStructure(Size = 0x10)]
            public class Sound : TagStructure
            {
                [TagField(ValidTags = new[] { "snd!" }, Flags = Label)]
                public CachedTag Type;
            }

            [TagStructure(Size = 0x10)]
            public class LoopingSound : TagStructure
            {
                [TagField(ValidTags = new[] { "lsnd" }, Flags = Label)]
                public CachedTag Type;
            }

            [TagStructure(Version = CacheVersion.Halo3ODST, Platform = CachePlatform.MCC, Size = 0xC0)]
            [TagStructure(MaxVersion = CacheVersion.HaloOnline700123, Size = 0xB0)]
            public class IntroMessageStruct : TagStructure
            {
                [TagField(ValidTags = new[] { "chdt" })] public CachedTag Pregame;
                [TagField(ValidTags = new[] { "chdt" })] public CachedTag Ctf;
                [TagField(ValidTags = new[] { "chdt" })] public CachedTag Slayer;
                [TagField(ValidTags = new[] { "chdt" })] public CachedTag Oddball;
                [TagField(ValidTags = new[] { "chdt" })] public CachedTag King;
                [TagField(ValidTags = new[] { "chdt" })] public CachedTag Sandbox;
                [TagField(ValidTags = new[] { "chdt" })] public CachedTag Vip;
                [TagField(ValidTags = new[] { "chdt" })] public CachedTag Juggernaut;
                [TagField(ValidTags = new[] { "chdt" })] public CachedTag Territories;
                [TagField(ValidTags = new[] { "chdt" })] public CachedTag Assault;
                [TagField(ValidTags = new[] { "chdt" })] public CachedTag Infection;

                [TagField(ValidTags = new[] { "chdt" }, Version = CacheVersion.Halo3ODST, Platform = CachePlatform.MCC)]
                public CachedTag Survival;
            }

            [TagStructure(Size = 0x50)]
            public class ReachIntroMessageStruct : TagStructure
            {
                [TagField(ValidTags = new[] { "chdt" })] public CachedTag Unused;
                [TagField(ValidTags = new[] { "chdt" })] public CachedTag Sandbox;
                [TagField(ValidTags = new[] { "chdt" })] public CachedTag Custom;
                [TagField(ValidTags = new[] { "chdt" })] public CachedTag Campaign;
                [TagField(ValidTags = new[] { "chdt" })] public CachedTag Survival;
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

            [TagStructure(Size = 0x21C, MaxVersion = CacheVersion.Halo3ODST)]
            [TagStructure(Size = 0x220, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x150, MinVersion = CacheVersion.HaloReach)]
            public class MultiplayerConstant : TagStructure
            {
                public SpawnConstantStruct ForbidEnemySpawnConstants;
                public SpawnConstantStruct EnemySpawnBiasConstants;
                public SpawnConstantStruct AllySpawnBias;
                public SpawnConstantStruct SpectatedAllySpawnBias;
                public SpawnConstantStruct ForbidAllySpawnConstants;

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public SpawnConstantStruct DeadTeammateSpawnBias;

                public float DeadTeammateInfluenceDuration; // seconds

                public List<WeaponSpawnInfluence> Weapons;
                public List<VehicleSpawnInfluence> Vehicles;
                public List<ProjectileSpawnInfluence> Projectiles;
                public List<EquipmentSpawnInfluence> Equipment;

                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public GametypeSpawnConstantStruct GametypeSpawnConstants;

                public float MaximumRandomSpawnBias;
                public float TeleporterRechargeTime; // seconds

                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public GrenadeDangerStruct GrenadeConstants;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public VehicleConstantStruct VehicleConstants;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public CachedTag HillBitmap;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public FlagConstantStruct FlagConstants;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float TerritoriesWaypointVerticalOffset;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public BombEffectStruct AssaultBombEffects;

                public CachedTag ForgeCursorImpactEffect;

                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public StringId BombDefusalString;

                public StringId BlockedTeleporterString;

                [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
                public int UnknownHO;

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

                [TagStructure(Size = 0x14, MaxVersion = CacheVersion.HaloOnline700123)]
                [TagStructure(Size = 0x20, MinVersion = CacheVersion.HaloReach)]
                public class SpawnConstantStruct : TagStructure
                {
                    public float FullWeightRadius; // (world units)
                    public float FalloffRadius; // (world units)

                    [TagField(MinVersion = CacheVersion.HaloReach)]
                    public List<FunctionBlock> FalloffFunction;

                    public float CylinderUpperHeight; // (world units)
                    public float CylinderLowerHeight; // (world units)
                    public float Weight;

                    [TagStructure(Size = 0x14)]
                    public class FunctionBlock : TagStructure
                    {
                        public TagFunction Function;
                    }
                }

                [TagStructure(Size = 0x20)]
                public class WeaponSpawnInfluence : TagStructure
                {
                    [TagField(Flags = Label, ValidTags = new[] { "weap" })]
                    public CachedTag Weapon;
                    public float FullWeightRadius; // (world units)
                    public float FalloffRadius; // (world units)
                    public float FalloffConeRadius; // (world units)
                    public float Weight;
                }

                [TagStructure(Size = 0x20)]
                public class VehicleSpawnInfluence : TagStructure
                {
                    [TagField(Flags = Label, ValidTags = new[] { "vehi" })]
                    public CachedTag Vehicle;
                    public float PillRadius; // (world units)
                    public float LeadTime; // (seconds)
                    public float MinimumVelocity; // (world units per second)
                    public float Weight;
                }

                [TagStructure(Size = 0x1C)]
                public class ProjectileSpawnInfluence : TagStructure
                {
                    [TagField(Flags = Label, ValidTags = new[] { "proj" })]
                    public CachedTag Projectile;
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

                [TagStructure(Size = 0xA0)]
                public class GametypeSpawnConstantStruct : TagStructure
                {
                    public SpawnConstantStruct KingOfTheHill;
                    public SpawnConstantStruct Oddball;
                    public SpawnConstantStruct CaptureTheFlag;
                    public SpawnConstantStruct TeraSpawnConstants;
                    public SpawnConstantStruct Territories;
                    public SpawnConstantStruct InfectionHumans;
                    public SpawnConstantStruct InfectionZombies;
                    public SpawnConstantStruct Vip;
                }

                [TagStructure(Size = 0x10)]
                public class GrenadeDangerStruct : TagStructure
                {
                    public float Weight;
                    public float InnerRadius;
                    public float OuterRadius;
                    public float LeadTime; // seconds
                }

                [TagStructure(Size = 0x14)]
                public class VehicleConstantStruct : TagStructure
                {
                    public float DangerMinimumSpeed; // wu/sec
                    public float DangerWeight;
                    public float DangerRadius;
                    public float DangerLeadTime; // seconds
                    public float NearbyPlayerDistance; // how nearby a player is to count a vehicle as 'occupied'
                }

                [TagStructure(Size = 0xC)]
                public class FlagConstantStruct : TagStructure
                {
                    public float ReturnDistance;
                    public float ContestedInnerRadius;
                    public float ContestedOuterRadius;
                }

                [TagStructure(Size = 0x40)]
                public class BombEffectStruct : TagStructure
                {
                    [TagField(ValidTags = new[] { "effe", "jpt!" })] public CachedTag ExplodeEffect;
                    [TagField(ValidTags = new[] { "effe", "jpt!" })] public CachedTag ExplodeSecondaryEffect;
                    [TagField(ValidTags = new[] { "effe", "jpt!" })] public CachedTag ExplodeDamageEffect;
                    [TagField(ValidTags = new[] { "effe", "jpt!" })] public CachedTag DefuseEffect;
                }
            }

            [TagStructure(Size = 0x24, Platform = CachePlatform.Original)]
            [TagStructure(Size = 0xC, Platform = CachePlatform.MCC)]
            public class StateResponse : TagStructure
			{
                [TagField(Platform = CachePlatform.Original)]
                public GameEngineStatusFlags Flags;

                [TagField(Length = 2, Flags = Padding, Platform = CachePlatform.Original)]
                public byte[] Padding0;

                [TagField(Flags = Label)]
                public GameEngineStatus State;

                [TagField(Length = 2, Flags = Padding)]
                public byte[] Padding1;

                public StringId FreeForAllMessage;
                public StringId TeamMessage;

                [TagField(Platform = CachePlatform.Original)]
                public CachedTag Unused;

                [TagField(Length = 4, Flags = Padding, Platform = CachePlatform.Original)]
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

            [TagStructure(Size = 0x60)]
            public class SimulationInterpolationStruct : TagStructure
            {
                [TagField(ValidTags = new [] { "siin" })] public CachedTag Biped;
                [TagField(ValidTags = new [] { "siin" })] public CachedTag Vehicle;
                [TagField(ValidTags = new [] { "siin" })] public CachedTag Crate;
                [TagField(ValidTags = new [] { "siin" })] public CachedTag Item;
                [TagField(ValidTags = new [] { "siin" })] public CachedTag Projectile;
                [TagField(ValidTags = new [] { "siin" })] public CachedTag Object;
            }

            [TagStructure(Size = 0x50)]
            public class HORuntimeFxStruct : TagStructure
            {
                [TagField(ValidTags = new[] { "obje" })] public CachedTag ThusIRefuteTheeEffect;
                [TagField(ValidTags = new[] { "effe" })] public CachedTag AutoFlipEffect;
                [TagField(ValidTags = new[] { "effe" })] public CachedTag SafetyBoosterEffect;
                [TagField(ValidTags = new[] { "snd!" })] public CachedTag RespawnBeep;
                [TagField(ValidTags = new[] { "snd!" })] public CachedTag EarlyRespawnSound;
            }


            [TagStructure(Size = 0x5C)]
            public class RequisitionConstantsReachBlock : TagStructure
            {
                public float FtlBonusFraction; // multiplier to apply to money earned by minions to also give to the fireteam leader

                /* AWARD AMOUNTS */
                public int Kill;
                public int Assist;
                public int FireTeamLeaderKill;
                public int VehicleKill; // Default, only applies if the vehicle doesn't have a custom award amount in the scenario requisition palette.
                public int ObjectiveDestroyed; // awarded to entire team
                public int ObjectiveArmed; // awarded to entire team
                public int ObjectiveDisarmed; // awarded to entire team
                public int ObjectiveDefending; // awarded every 3 seconds to any individuals near secondary defensive objectives
                public int NeutralTerritoryOwned; // awarded every 3 seconds to entire team that owns BFG
                public int ServedAsReinforcementTarget; // awarded to a reinforcement target when a teammate spawns on him (to encourage cooperation)
                public int UberassaultGunCaptured; // awarded on gaining ownership of a gun to every member of the new owning team
                public int UberassaultGunOwned; // awarded every 3 seconds to the entire team that owns this gun.  Money from multiple guns stacks (so if you own all 3, you'll get 3x this money every 3 seconds).

                /* PENALTY AMOUNTS */
                public int BetrayedATeammate;

                /* FIRE TEAM TIER KILL REQUIREMENTS */
                public int BronzeKillMinimum;
                public int SilverKillMinimum;
                public int GoldKillMinimum;

                /* FIRE TEAM TIER BONUS MULTIPLIERS */
                public float BronzeMultiplier;
                public float SilverMultiplier;
                public float GoldMultiplier;

                /* FIRE TEAM TIER TIME REQUIREMENT */
                public int BronzeAdvancementTime;
                public int SilverAdvancementTime;
                public int GoldAdvancementTime;
            }

            [TagStructure(Size = 0x24)]
            public class GameEngineStatusResponseBlock : TagStructure
            {
                public GameEngineStatusFlagsDefinition Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] FAW;
                public GameEngineStatusEnumDefinition State;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] BNYFIDDGX;
                public StringId FfaMessage;
                public StringId TeamMessage;
                public CachedTag Unused;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] GTL;

                [Flags]
                public enum GameEngineStatusFlagsDefinition : ushort
                {
                    Unused = 1 << 0
                }

                public enum GameEngineStatusEnumDefinition : short
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
                    GameOverYouLostButGameTied,
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
                    PlayingLosingUnlimited,
                    WaitingToSpawn,
                    WaitingForGameStart,
                    Blank
                }
            }
        }
    }
}