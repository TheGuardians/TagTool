using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions.Common;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "chud_definition", Tag = "chdt", Size = 0x18, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "chud_definition", Tag = "chdt", Size = 0x28, MinVersion = CacheVersion.HaloReach)]
    public class ChudDefinition : TagStructure
    {
        public List<HudWidget> HudWidgets;
        public ChudAmmunitionInfo HudAmmunitionInfo;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<NullBlock> CompiledWidgetData;

        [TagStructure(Size = 0xC, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x10, MinVersion = CacheVersion.HaloReach)]
        public class ChudAmmunitionInfo : TagStructure
        {
            public int LowAmmoLoadedThreshold;
            public int LowAmmoReserveThreshold;
            public int LowBatteryThreshold;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int FtMemberWeaponSequence;
        }

        [TagStructure(Size = 0x38, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x78, MinVersion = CacheVersion.HaloReach)]
        public class HudWidgetBase : TagStructure
        {
            [TagField(Flags = Label)]
            public StringId Name;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public ChudScriptingClass ScriptingClass;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public WidgetFlags BaseFlags;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public WidgetLayerEnum SortLayer;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public sbyte SpecialHudType;
            [TagField(Length = 3, Flags = Padding, MinVersion = CacheVersion.HaloReach)]
            public byte[] Padding1;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short ImportInput;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short ImportRangeInput;

            // TODO: consolidate
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public CachedTag StateDataTemplate;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<StateDatumReach> StateDataReach;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public List<StateDatum> StateData;
          
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public CachedTag PlacementDataTemplate;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<PlacementDatumReach> PlacementDataReach;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public List<PlacementDatum> PlacementData;
          
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public CachedTag AnimationDataTemplate;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<AnimationDatumReach> AnimationDataReach;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public List<AnimationDatum> AnimationData;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public CachedTag RenderDataTemplate;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<RenderDatumReach> RenderDataReach;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public List<RenderDatum> RenderData;
           

            [TagStructure(Size = 0x28, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x38, MaxVersion = CacheVersion.Halo3ODST)]
            [TagStructure(Size = 0x44, MaxVersion = CacheVersion.HaloOnline604673)]
            [TagStructure(Size = 0x48, MinVersion = CacheVersion.HaloOnline700123)]
            public class StateDatum : TagStructure
            {
                [TagField(MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
                public ChudGameStateH3 GameStateH3;
                [TagField(MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
                public ChudGameStateH3MCC GameStateH3MCC;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public ChudGameStateODSTFlags GameStateODST;
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public ChudGameStateED GameState;

                public ChudSkinState SkinState;

                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public PDA PDAFlags;

                public ChudGameTeam GameTeam;
                public ChudWindowState WindowState;

                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public ChudGameEngineState_Retail MultiplayerEventsFlags_H3;
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public ChudGameEngineState_ED MultiplayerEvents;

                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public ChudMiscState_ED UnitBaseFlags;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public ChudMiscState_ODST UnitBaseFlags_ODST;
                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public ChudMiscState_H3 UnitBaseFlags_H3;

                public ChudSandboxEditorState EditorFlags;

                public ChudHindsightState HindsightState;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public Skulls SkullFlags;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public SurvivalRounds SurvivalRoundFlags;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public SurvivalWaves SurvivalWaveFlags;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public SurvivalLives SurvivalLivesFlags;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public SurvivalDifficulty SurvivalDifficultyFlags;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public ushort Unused;

                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public GeneralKudos GeneralKudosFlags;
                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public GeneralKudos_H3 GeneralKudosFlags_H3;

                public UnitZoom UnitZoomFlags;
                public UnitInventory UnitInventoryFlags;

                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public ushort Unused3;

                public UnitGeneral UnitGeneralFlags;
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public ushort Unused4;

                public ChudWeaponImpulseState WeaponKudosFlags;
                public WeaponStatus WeaponStatusFlags;
                public WeaponTarget WeaponTargetFlags;
                public ChudWeaponMiscState WeaponTargetBFlags;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public Player_Special Player_SpecialFlags;
                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public Player_Special_H3 Player_SpecialFlags_H3;

                public Weapon_Special Weapon_SpecialFlags;
                public Inverse InverseFlags;

                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public PDA2 PDA2Flags;
                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public short UnusedFlags3;

                //HO EXCLUSIVE FLAGS
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public short UnusedFlags4;
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public Consumable ConsumableFlags;
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public EnergyMeter EnergyMeterFlags;

                [Flags]
                public enum ChudGameStateED : ushort
                {
                    None,
                    CampaignSolo = 1 << 0,
                    CampaignCoop = 1 << 1,
                    Survival = 1 << 2, //not sure about this one
                    FreeForAll = 1 << 3,
                    TeamGame = 1 << 4,
                    CTF = 1 << 5,
                    Slayer = 1 << 6,
                    Oddball = 1 << 7,
                    KOTH = 1 << 8,
                    Juggernaut = 1 << 9,
                    Territories = 1 << 10,
                    Assault = 1 << 11,
                    VIP = 1 << 12,
                    Infection = 1 << 13,
                    Editor = 1 << 14,
                    Theater = 1 << 15
                }

                [Flags]
                public enum ChudGameStateODSTFlags : ushort
                {
                    None,
                    CampaignSolo = 1 << 0,
                    CampaignCoop = 1 << 1,
                    Survival = 1 << 2,
                    Editor = 1 << 3,
                    Theater = 1 << 4,
                }

                [Flags]
                public enum ChudGameStateH3 : ushort
                {
                    None,
                    CampaignSolo = 1 << 0,
                    CampaignCoop = 1 << 1,
                    FreeForAll = 1 << 2,
                    TeamGame = 1 << 3,
                    CTF = 1 << 4,
                    Slayer = 1 << 5,
                    Oddball = 1 << 6,
                    KOTH = 1 << 7,
                    Juggernaut = 1 << 8,
                    Territories = 1 << 9,
                    Assault = 1 << 10,
                    VIP = 1 << 11,
                    Infection = 1 << 12,
                    Unused = 1 << 13,
                    Editor = 1 << 14,
                    Theater = 1 << 15
                }

                [Flags]
                public enum ChudGameStateH3MCC : ushort
                {
                    None,
                    CampaignSolo = 1 << 0,
                    CampaignCoop = 1 << 1,
                    FreeForAll = 1 << 2,
                    TeamGame = 1 << 3,
                    CTF = 1 << 4,
                    Slayer = 1 << 5,
                    Oddball = 1 << 6,
                    KOTH = 1 << 7,
                    Juggernaut = 1 << 8,
                    Territories = 1 << 9,
                    Assault = 1 << 10,
                    VIP = 1 << 11,
                    Infection = 1 << 12,
                    Editor = 1 << 13,
                    Theater = 1 << 14,
                    Unused = 1 << 15
                }

                [Flags]
                public enum ChudSkinState : ushort
                {
                    None,
                    Spartan = 1 << 0,
                    Elite = 1 << 1,
                    Monitor = 1 << 2,
                }

                [Flags]
                public enum PDA : ushort
                {
                    None,
                    PdaActive = 1 << 0,
                    PdaInactive = 1 << 1,
                    FirstPerson = 1 << 2 // Combine this with PDA active to have elements hide in third person without the PDA up
                }

                [Flags]
                public enum PDA2 : ushort
                {
                    None,
                    VisibleInPDA = 1 << 0,
                }

                [Flags]
                public enum ChudGameTeam : ushort
                {
                    None,
                    Offense = 1 << 0,
                    Defense = 1 << 1,
                    NotApplicable = 1 << 2,
                    PlayerIsSpecial = 1 << 3,
                    PlayerSpecialAndDefense = 1 << 4, //broken?
                    NoMicrophone = 1 << 5,
                    TalkingDisabled = 1 << 6,
                    TapToTalk = 1 << 7,
                    TalkingEnabled = 1 << 8,
                    NotTalking = 1 << 9,
                    Talking = 1 << 10,
                }

                [Flags]
                public enum ChudWindowState : ushort
                {
                    None,
                    WideFull = 1 << 0,
                    WideHalf = 1 << 1,
                    NativeFull = 1 << 2,
                    StandardFull = 1 << 3,
                    WideQuarter = 1 << 4,
                    StandardHalf = 1 << 5,
                    NativeQuarter = 1 << 6,
                    StandardQuarter = 1 << 7,
                }

                [Flags]
                public enum ChudGameEngineState_ED : uint
                {
                    None,
                    FriendlyScoreAvailable = 1 << 0,
                    EnemyScoreAvailable = 1 << 1,
                    VariantNameAvailable = 1 << 2,
                    TalkingPlayerAvailable = 1 << 3,
                    ArmingMeterAvailable = 1 << 4,
                    TimeLeftAvailable = 1 << 5,
                    FriendlyPossession = 1 << 6,
                    EnemyPossession = 1 << 7,
                    VariantCustomA = 1 << 8,
                    VariantCustomB = 1 << 9,
                    VariantCustomC = 1 << 10,
                    AttackerBombDropped = 1 << 11,
                    AttackerBombPickedUp = 1 << 12,
                    DefenderTeamIsDead = 1 << 13,
                    AttackerTeamIsDead = 1 << 14,
                    RoundStartPeriod = 1 << 15,
                    Unknown16 = 1 << 16,
                    TestEnabled = 1 << 17
                }

                [Flags]
                public enum ChudGameEngineState_Retail : ushort
                {
                    None,
                    FriendlyScoreAvailable = 1 << 0,
                    EnemyScoreAvailable = 1 << 1,
                    VariantNameAvailable = 1 << 2,
                    TalkingPlayerAvailable = 1 << 3,
                    ArmingMeterAvailable = 1 << 4,
                    TimeLeftAvailable = 1 << 5,
                    FriendlyPossession = 1 << 6,
                    EnemyPossession = 1 << 7,
                    VariantCustomA = 1 << 8,
                    VariantCustomB = 1 << 9,
                    VariantCustomC = 1 << 10,
                    RoundStartPeriod = 1 << 11,
                    TestEnabled = 1 << 12
                }

                [Flags]
                public enum ChudMiscState_H3 : ushort
                {
                    None,
                    TextureCamAvailable = 1 << 0,
                    SniperFlavaAvailable = 1 << 1,
                    SavedFilmRecordingMode = 1 << 2,
                    SavedFilmNormalMode = 1 << 3,
                    PlayerTrainingAvailable = 1 << 4,
                    CampaignObjectiveAvailable = 1 << 5
                }

                [Flags]
                public enum ChudMiscState_ODST : uint
                {
                    None,
                    TextureCamAvailable = 1 << 0,
                    SniperFlavaAvailable = 1 << 1,
                    SavedFilmRecordingMode = 1 << 2,
                    SavedFilmNormalMode = 1 << 3,
                    PlayerTrainingAvailable = 1 << 4,
                    CampaignObjectiveAvailable = 1 << 5,
                    SurvivalState = 1 << 6,
                    BeaconEnabled = 1 << 7,
                    UserWaypointEnabled = 1 << 8,
                    Bit9 = 1 << 9,
                    Achievement1 = 1 << 10,
                    Achievement2 = 1 << 11,
                    Achievement3 = 1 << 12,
                    Achievement4 = 1 << 13,
                    Achievement5 = 1 << 14,
                    ARGEnabled = 1 << 15,
                }

                [Flags]
                public enum ChudMiscState_ED : uint
                {
                    None,
                    TextureCamAvailable = 1 << 0,
                    SniperFlavaAvailable = 1 << 1,
                    SavedFilmRecordingMode = 1 << 2,
                    SavedFilmNormalMode = 1 << 3,
                    PlayerTrainingAvailable = 1 << 4,
                    CampaignObjectiveAvailable = 1 << 5,
                    SurvivalState = 1 << 6,
                    BeaconEnabled = 1 << 7, //unused, kept for odst porting
                    Achievement1 = 1 << 8,
                    Achievement2 = 1 << 9,
                    Achievement3 = 1 << 10,
                    Achievement4 = 1 << 11,
                    Achievement5 = 1 << 12,
                    GameTimeUnknown = 1 << 13,
                    UserWaypointEnabled = 1 << 14, //unused, kept for odst porting
                    ARGEnabled = 1 << 15, //unused, kept for odst porting
                }


                [Flags]
                public enum ChudSandboxEditorState : ushort
                {
                    None,
                    DefaultCrosshair = 1 << 0,
                    ActiveCrosshair = 1 << 1,
                    ManipulationCrosshair = 1 << 2,
                    NotallowedCrosshair = 1 << 3,
                    BudgetAvailable = 1 << 4
                }

                [Flags]
                public enum ChudHindsightState : ushort
                {
                    None,
                    SensorRange10m = 1 << 0,
                    SensorRange25m = 1 << 1,
                    SensorRange75m = 1 << 2,
                    SensorRange150m = 1 << 3,
                    MetagameP1Talking = 1 << 4,
                    MetagameP2Enabled = 1 << 5,
                    MetagameP2Talking = 1 << 6,
                    MetagameP3Enabled = 1 << 7,
                    MetagameP3Talking = 1 << 8,
                    MetagameP4Enabled = 1 << 9,
                    MetagameP4Talking = 1 << 10,
                    TransientScoreAvail = 1 << 11,
                    MetagameMultikillAvail = 1 << 12,
                    MetagameNegScoreAvail = 1 << 13,
                    //ODST
                    MetagameTeamScoring = 1 << 14,
                    MetagameFfaScoring = 1 << 15
                }

                [Flags]
                public enum Skulls : ushort
                {
                    None,
                    IronSkullEnabled = 1 << 0,
                    BlackEyeSkullEnabled = 1 << 1,
                    ToughLuckSkullEnabled = 1 << 2,
                    CatchSkullEnabled = 1 << 3,
                    CloudSkullEnabled = 1 << 4,
                    FamineSkullEnabled = 1 << 5,
                    ThunderstormSkullEnabled = 1 << 6,
                    TiltSkullEnabled = 1 << 7,
                    MythicSkullEnabled = 1 << 8,
                    AssassinsSkullEnabled = 1 << 9,
                    BlindSkullEnabled = 1 << 10,
                    CowbellSkullEnabled = 1 << 11,
                    GruntBirthdayPartySkullEnabled = 1 << 12,
                    IWHBYDSkullEnabled = 1 << 13,
                    ThirdPersonSkullEnabled = 1 << 14,
                    DirectorsCutSkullEnabled = 1 << 15
                }

                [Flags]
                public enum SurvivalRounds : ushort
                {
                    None,
                    Round0 = 1 << 0,
                    Round1 = 1 << 1,
                    Round2 = 1 << 2,
                    Round3 = 1 << 3,
                    Round4 = 1 << 4,
                    Round5 = 1 << 5,
                    BonusRound = 1 << 6,
                }

                [Flags]
                public enum SurvivalWaves : ushort
                {
                    None,
                    Wave1 = 1 << 0,
                    Wave2 = 1 << 1,
                    Wave3 = 1 << 2,
                    Wave4 = 1 << 3,
                    Wave5 = 1 << 4,
                    Wave6 = 1 << 5,
                    Wave7 = 1 << 6,
                    Wave8 = 1 << 7,
                    Wave9 = 1 << 8,
                    Wave10 = 1 << 9,
                    Wave11 = 1 << 10,
                    Wave12 = 1 << 11,
                    Wave13 = 1 << 12,
                    Wave14 = 1 << 13,
                    Wave15 = 1 << 14,
                    Wave16 = 1 << 15
                }

                [Flags]
                public enum SurvivalLives : ushort
                {
                    None,
                    lives0 = 1 << 0,
                    lives1 = 1 << 1,
                    lives2 = 1 << 2,
                    lives3 = 1 << 3,
                    lives4 = 1 << 4,
                    lives5 = 1 << 5,
                    lives6 = 1 << 6,
                    lives7 = 1 << 7,
                    lives8 = 1 << 8,
                    lives9 = 1 << 9,
                    lives10 = 1 << 10,
                    lives11 = 1 << 11,
                    lives12 = 1 << 12,
                    lives13 = 1 << 13,
                    lives14 = 1 << 14,
                    lives15 = 1 << 15
                }

                [Flags]
                public enum SurvivalDifficulty : ushort
                {
                    None,
                    Easy = 1 << 0,
                    Normal = 1 << 1,
                    Heroic = 1 << 2,
                    Legendary = 1 << 3,
                }

                [Flags]
                public enum GeneralKudos : ushort
                {
                    None,
                    PickupFragGrenades = 1 << 0,
                    PickupPlasmaGrenades = 1 << 1,
                    PickupSpikeGrenades = 1 << 2,
                    PickupFireGrenades = 1 << 3,
                    Bit4 = 1 << 4,
                    LivesAdded = 1 << 5,
                    Consumable1Unknown = 1 << 6,
                    Consumable2Unknown = 1 << 7,
                    Consumable3Unknown = 1 << 8,
                    Consumable4Unknown = 1 << 9,
                    EnemyVisionActive = 1 << 10,
                    Bit11 = 1 << 11,
                    HitMarkerLow = 1 << 12,
                    HitMarkerMedium = 1 << 13,
                    HitMarkerHigh = 1 << 14,
                    Bit15 = 1 << 15
                }

                [Flags]
                public enum GeneralKudos_H3 : ushort
                {
                    None,
                    PickupFragGrenades = 1 << 0,
                    PickupPlasmaGrenades = 1 << 1,
                    PickupSpikeGrenades = 1 << 2,
                    PickupFireGrenades = 1 << 3,
                    Bit4 = 1 << 4,
                    Bit5 = 1 << 5,
                    Bit6 = 1 << 6,
                    Bit7 = 1 << 7,
                    Bit8 = 1 << 8,
                    LivesAdded = 1 << 9,
                }

                [Flags]
                public enum UnitZoom : ushort
                {
                    None,
                    BinocularsEnabled = 1 << 0,
                    UnitIsZoomedLevel1 = 1 << 1,
                    UnitIsZoomedLevel2 = 1 << 2
                }

                [Flags]
                public enum UnitInventory : ushort
                {
                    None = 1 << 0,
                    IsSingleWielding = 1 << 1,
                    IsDualWielding = 1 << 2,
                    HasSupportWeapon = 1 << 3
                }

                [Flags]
                public enum UnitGeneral : ushort
                {
                    None,
                    MotionTrackerEnabled = 1 << 0,
                    MotionTrackerDisabled = 1 << 1,
                    SelectedFragGrenades = 1 << 2,
                    SelectedPlasmaGrenades = 1 << 3,
                    SelectedSpikeGrenades = 1 << 4,
                    SelectedFireGrenades = 1 << 5,
                    BinocularsActive = 1 << 6,
                    BinocularsNotActive = 1 << 7,
                    ThirdPersonCamera = 1 << 8,
                    FirstPersonCamera = 1 << 9,
                    IsSpeaking = 1 << 10,
                    IsTappingToTalk = 1 << 11,
                    HasOvershieldLevel1 = 1 << 12,
                    HasOvershieldLevel2 = 1 << 13,
                    HasOvershieldLevel3 = 1 << 14,
                    HasShields = 1 << 15
                }

                [Flags]
                public enum ChudWeaponImpulseState : ushort
                {
                    None,
                    AmmoUsed = 1 << 0,
                    AmmoPickup = 1 << 1,
                    AmmoThreshold = 1 << 2
                }

                [Flags]
                public enum WeaponTarget : ushort
                {
                    None,
                    Normal = 1 << 0,
                    Friendly = 1 << 1,
                    Enemy = 1 << 2,
                    EnemyHeadshot = 1 << 3,
                    EnemyWeakpoint = 1 << 4,
                    Invincible = 1 << 5, //Defunct
                    LockAvailable = 1 << 6,
                    PlasmaTrack = 1 << 7,
                    LockUnavailable = 1 << 8,
                    TargetUnobstructed = 1 << 9,
                    TargetObstructed = 1 << 10,
                }

                [Flags]
                public enum ChudWeaponMiscState : ushort
                {
                    None,
                    LockingOnAvailable = 1 << 0,
                    LockedOnAvailable = 1 << 1,
                    LockedOnUnavailable = 1 << 2,
                }

                [Flags]
                public enum WeaponStatus : ushort
                {
                    None,
                    SourceIsPrimaryWeapon = 1 << 0,
                    SourceIsDualWeapon = 1 << 1,
                    SourceIsBackpacked = 1 << 2,
                    Hidden = 1 << 3,
                    PickupMessage = 1 << 4,
                    Bit5 = 1 << 5,
                    Bit6 = 1 << 6,
                    Bit7 = 1 << 7
                }

                [Flags]
                public enum Player_Special : ushort
                {
                    None,
                    HealthMinorDamage = 1 << 0,
                    HealthMediumDamage = 1 << 1,
                    HealthHeavyDamage = 1 << 2,
                    ShieldsMinorDamage = 1 << 3,
                    ShieldsMediumDamage = 1 << 4,
                    ShieldsHeavyDamage = 1 << 5,
                    HasFragGrenades = 1 << 6,
                    HasPlasmaGrenades = 1 << 7,
                    HasSpikeGrenades = 1 << 8,
                    HasFireGrenades = 1 << 9
                }

                [Flags]
                public enum Player_Special_H3 : ushort
                {
                    None,
                    ShieldsMediumDamage = 1 << 0,
                    ShieldsHeavyDamage = 1 << 1,
                    HasFragGrenades = 1 << 2,
                    HasPlasmaGrenades = 1 << 3,
                    HasSpikeGrenades = 1 << 4,
                    HasFireGrenades = 1 << 5,
                    Unknown1 = 1 << 6,
                    Unknown2 = 1 << 7,
                }

                [Flags]
                public enum Weapon_Special : ushort
                {
                    None,
                    ClipBelowCutoff = 1 << 0,
                    ClipEmpty = 1 << 1,
                    AmmoBelowCutoff = 1 << 2,
                    AmmoEmpty = 1 << 3,
                    BatteryBelowCutoff = 1 << 4,
                    BatteryEmpty = 1 << 5,
                    Overheated = 1 << 6,
                }

                [Flags]
                public enum Inverse : ushort
                {
                    None,
                    NotZoomedIn = 1 << 0,
                    NotArmedWithSupportWeapon = 1 << 1,
                    NotFullyArmed = 1 << 2,
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

                [Flags]
                public enum Consumable : uint
                {
                    None,
                    Consumable1Low = 1 << 0,
                    Consumable2Low = 1 << 1,
                    Consumable3Low = 1 << 2,
                    Consumable4Low = 1 << 3,
                    Consumable1Empty = 1 << 4,
                    Consumable2Empty = 1 << 5,
                    Consumable3Empty = 1 << 6,
                    Consumable4Empty = 1 << 7,
                    Consumable1Available = 1 << 8,
                    Consumable2Available = 1 << 9,
                    Consumable3Available = 1 << 10,
                    Consumable4Available = 1 << 11,
                    Consumable1DisabledA = 1 << 12,
                    Consumable2DisabledA = 1 << 13,
                    Consumable3DisabledA = 1 << 14,
                    Consumable4DisabledA = 1 << 15,
                    Consumable1DisabledB = 1 << 16,
                    Consumable2DisabledB = 1 << 17,
                    Consumable3DisabledB = 1 << 18,
                    Consumable4DisabledB = 1 << 19,
                    Consumable1Active = 1 << 20,
                    Consumable2Active = 1 << 21,
                    Consumable3Active = 1 << 22,
                    Consumable4Active = 1 << 23,
                }

                [Flags]
                public enum EnergyMeter : uint
                {
                    None,
                    EnergyMeter1Full = 1 << 0,
                    EnergyMeter2Full = 1 << 1,
                    EnergyMeter3Full = 1 << 2,
                    EnergyMeter4Full = 1 << 3,
                    EnergyMeter5Full = 1 << 4,
                }
            }

            [TagStructure(Size = 0x38, MinVersion = CacheVersion.HaloReach)]
            public class StateDatumReach : TagStructure
            {
                public List<NullBlock> Unknown1;
                public List<NullBlock> Unknown2;
                public List<NullBlock> Unknown3;
                public ushort Unknown4;
                public ushort Unknown5;
                public ushort Unknown6;
                public ushort Unknown7;
                public uint Unknown8;
                public uint Unknown9;
                public uint Unknown10;
            }

            [TagStructure(Size = 0x1C)]
            public class PlacementDatum : TagStructure
            {
                [TagField(Flags = Label)]
                public ChudAnchorType Anchor;
                public ChudWidgetPlacementFlags AnchorFlags;
                public RealPoint2d Origin;
                public RealPoint2d Offset;
                public RealPoint2d Scale;

                public enum ChudAnchorType : short
                {
                    TopLeft,
                    TopRight,
                    BottomRight,
                    BottomLeft,
                    Crosshair,
                    TopCenter,
                    GrenadeHuman,
                    GrenadePlasma,
                    GrenadeBrute,
                    GrenadeFire,
                    ScoreboardFriendly,
                    ScoreboardEnemy,
                    Parent,
                    Center, //same as crosshair
                    BackpackWeapon, //uses both x and y offsets
                    Equipment, //x offset always 0
                    Ddamge, //same as center
                    Territory1,
                    Territory2,
                    Territory3,
                    Territory4,
                    Territory5,
                    Messaging, //uses notification offset in chgd
                    BottomCenter, //bottom center
                    WeaponTarget,
                    StateMessageRight,
                    StateMessageLeft,
                    MessageBottomState,
                    MessageBottomPrimary, //separate float for this in chgd
                    ScoreboardFriendlyScoreOffset,
                    ScoreboardEnemyScoreOffset,
                    MetagameBar,
                    MetagamePlayer1,
                    MetagamePlayer2,
                    MetagamePlayer3,
                    MetagamePlayer4,
                    TopCenterSavedfilm,
                    Prompt, //ODST, corresponds to prompt offset in chgd
                    HologramTarget,
                    Medals, //uses medal offset in chgd
                    SurvivalMedals, //has values in chgd
                    UnknownHO_Offset3, //has values in chgd
                    MotionSensor
                }

                [Flags]
                public enum ChudWidgetPlacementFlags : ushort
                {
                    MoreSoon = 1 << 0
                }
            }

            [TagStructure(Size = 0x1C, MinVersion = CacheVersion.HaloReach)]
            public class PlacementDatumReach : TagStructure
            {
                public ChudCurvatureResFlags WindowState;
                public ChudAnchorTypeEnum AnchorType;
                public ChudWidgetPlacementFlags AnchorFlags;
                [TagField(Length = 0x1, Flags = Padding)]
                public byte[] DSFKSLVJ;
                public RealPoint2d Origin;
                public RealPoint2d Offset;
                public RealPoint2d Scale;

                [Flags]
                public enum ChudCurvatureResFlags : byte
                {
                    FullscreenWide = 1 << 0,
                    FullscreenStandard = 1 << 1,
                    Halfscreen = 1 << 2,
                    QuarterscreenWide = 1 << 3,
                    QuarterscreenStandard = 1 << 4
                }

                public enum ChudAnchorTypeEnum : sbyte
                {
                    Parent,
                    TopLeft,
                    TopCenter,
                    TopRight,
                    Center,
                    BottomLeft,
                    BottomCenter,
                    BottomRight,
                    MotionSensor,
                    DDamge,
                    Messaging,
                    StateMsgLeft,
                    StateMsgRight,
                    MsgBottomState,
                    MsgBottomPrim,
                    TrackedTarget,
                    TrackingObject,
                    Crosshair,
                    BackpackWeapon,
                    Grenade,
                    Equipment,
                    WeaponTarget,
                    GhostReticule,
                    HologramTarget,
                    AirstrikeTarget,
                    Player,
                    ScriptedObject,
                    MetagameBar,
                    MetagameP1,
                    MetagameP2,
                    MetagameP3,
                    MetagameP4,
                    SBFriendly,
                    SBEnemy,
                    Territory1,
                    Territory2,
                    Territory3,
                    Territory4,
                    Territory5,
                    StateMsgFireteam,
                    FireteamPossibleActionObject,
                    FireteamPendingTargetObject,
                    FireteamCurrentTargetObject,
                    FireteamPendingDirectiveUnit,
                    FireteamCurrentDirectiveUnit,
                    EnemyObjectiveObject,
                    FriendlyObjectiveObject,
                    NeutralObjectiveObject,
                    LasingTargetObject,
                    CampaignFireteamMember,
                    TopCenterSavedfilm
                }

                [Flags]
                public enum ChudWidgetPlacementFlags : byte
                {
                    ClampPlacementToScreenCircle = 1 << 0,
                    ClampPlacementUnlessSplitscreen = 1 << 1,
                    DoNotRotate = 1 << 2
                }
            }

            [TagStructure(Size = 0x78, MaxVersion = CacheVersion.Halo3ODST)]
            [TagStructure(Size = 0x90, MinVersion = CacheVersion.HaloOnlineED)]
            public class AnimationDatum : TagStructure
            {
                public ChudWidgetAnimationStruct Initializing;
                public ChudWidgetAnimationStruct Active;
                public ChudWidgetAnimationStruct Flashing;
                public ChudWidgetAnimationStruct Readying;
                public ChudWidgetAnimationStruct Unreadying;
                public ChudWidgetAnimationStruct Impulse;

                [TagStructure(Size = 0x14, MaxVersion = CacheVersion.Halo3ODST)]
                [TagStructure(Size = 0x18, MinVersion = CacheVersion.HaloOnlineED)]
                public class ChudWidgetAnimationStruct : TagStructure
                {
                    public ChudWidgetAnimationFlags Flags;
                    public ChudWidgetAnimationInputTypeEnum InputType;
                    public CachedTag Animation;

                    [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                    public float StateUnknown;

                    [Flags]
                    public enum ChudWidgetAnimationFlags : ushort
                    {
                        PlayReverse = 1 << 0
                    }

                    public enum ChudWidgetAnimationInputTypeEnum : short
                    {
                        Time,
                        Extern1,
                        Extern2,
                        UseCompassTarget, //? HO or invalid
                        UseUserTarget //? HO or invalid
                    }
                }
            }

            [TagStructure(Size = 0x64, MinVersion = CacheVersion.HaloReach)]
            public class AnimationDatumReach : TagStructure
            {
                public ChudWidgetAnimationStruct Unknown1;
                public ChudWidgetAnimationStruct Unknown2;
                public ChudWidgetAnimationStruct Unknown3;
                public ChudWidgetAnimationStruct Unknown4;
                public ChudWidgetAnimationStruct Unknown5;

                [TagStructure(Size = 0x14)]
                public class ChudWidgetAnimationStruct
                {
                    public byte Flags;
                    public byte Function;
                    public ushort Unknown1;
                    public CachedTag Animation;
                }
            }

            [TagStructure(Size = 0x40, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x48, MinVersion = CacheVersion.Halo3ODST)]
            public class RenderDatum : TagStructure
            {
                [TagField(Flags = Label)]
                public ChudShaderType ShaderType;

                [TagField(Flags = TagFieldFlags.Padding, Length = 0x2, MinVersion = CacheVersion.Halo3Beta, MaxVersion = CacheVersion.Halo3ODST)]
                public byte[] Padding;

                [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
                public ChudBlendMode BlendModeHO;
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public ChudRenderExternalInputHO ExternalInput;
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public ChudRenderExternalInputHO RangeInput;

                [TagField(MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
                public ChudRenderExternalInput_H3 ExternalInput_H3;
                [TagField(MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
                public ChudRenderExternalInput_H3 RangeInput_H3;

                [TagField(MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
                public ChudRenderExternalInput_H3MCC ExternalInput_H3MCC;
                [TagField(MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
                public ChudRenderExternalInput_H3MCC RangeInput_H3MCC;

                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
                public ChudRenderExternalInput_ODST ExternalInput_ODST;

                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public ChudRenderExternalInput_ODST RangeInput_ODST;

                public ArgbColor LocalColorA;
                public ArgbColor LocalColorB;
                public ArgbColor LocalColorC;
                public ArgbColor LocalColorD;
                public float LocalScalarA;
                public float LocalScalarB;
                public float LocalScalarC;
                public float LocalScalarD;

                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public OutputColorValue OutputColorA_Retail;
                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public OutputColorValue OutputColorB_Retail;
                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public OutputColorValue OutputColorC_Retail;
                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public OutputColorValue OutputColorD_Retail;
                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public OutputColorValue OutputColorE_Retail;
                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public OutputColorValue OutputColorF_Retail;

                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public OutputColorValue_HO OutputColorA;
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public OutputColorValue_HO OutputColorB;
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public OutputColorValue_HO OutputColorC;
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public OutputColorValue_HO OutputColorD;
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public OutputColorValue_HO OutputColorE;
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public OutputColorValue_HO OutputColorF;

                public OutputScalarValue OutputScalarA;
                public OutputScalarValue OutputScalarB;
                public OutputScalarValue OutputScalarC;
                public OutputScalarValue OutputScalarD;
                public OutputScalarValue OutputScalarE;
                public OutputScalarValue OutputScalarF;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public Rectangle2d ScissorRect;

                public enum ChudShaderType : short
                {
                    Simple,
                    Meter,
                    TextSimple,
                    MeterShield,
                    MeterGradient,
                    Crosshair,
                    DirectionalDamage, // dontUse
                    Solid, // dontUse
                    Sensor, // dontUse
                    MeterSingleColor,
                    Navpoints,
                    Medal,
                    TextureCam,
                    CortanaScreen,
                    CortanaCamera,
                    CortanaOffscreen,
                    CortanaScreenFinal,
                    MeterChapter,
                    MeterDoubleGradient,
                    MeterRadialGradient,
                    DistortionAndBlur,
                    Emblem,
                    CortanaComposite,
                    DirectionalDamageApply,
                    ReallySimple,
                    Unknown // ?? not in H3
                }

                public enum ChudBlendMode : short
                {
                    AlphaBlend,
                    Additive,
                    Multiply,
                    Opaque,
                    DoubleMultiply,
                    PreMultipliedAlpha,
                    Maximum,
                    MultiplyAdd,
                    AddSrcTimesDstAlpha,
                    AddSrcTimesSrcAlpha,
                    InvAlphaBlend,
                    SeparateAlphaBlend,
                    SeparateAlphaBlendAdditive
                }

                public enum ChudRenderExternalInputHO : short
                {
                    Zero,
                    One,
                    DebugSlide1,    // ? Time
                    DebugSlide100,  // ? Fade
                    BodyVitality,
                    HealthRecentDamage,
                    ShieldAmount,
                    ShieldRecentDamage,
                    WeaponAmmoLoaded,
                    WeaponAmmoReserve,
                    WeaponVersionNumber,
                    WeaponHeatFraction,
                    WeaponBatteryFraction,
                    WeaponErrorCurrent1,
                    WeaponErrorCurrent2,
                    ImpulseValue,
                    Autoaim,
                    GrenadeSelected,
                    GrenadeCount,
                    WeaponChargeFraction,
                    FriendlyScore,
                    EnemyScore,
                    ScoreToWin,
                    ArmingFraction,
                    LockingAmount,
                    Unit1xOvershieldCurrent,
                    Unit1xOvershieldRecentDmg,
                    Unit2xOvershieldCurrent,
                    Unit2xOvershieldRecentDmg,
                    Unit3xOvershieldCurrent,
                    Unit3xOvershieldRecentDmg,
                    CameraYaw,
                    CameraPitch,
                    TargetDistance,
                    TargetElevation,
                    EditorBudgetFraction,
                    EditorBudgetLeft,
                    SavedFilmTotalTime,
                    SavedFilmMarkerTime,
                    SavedFilmChapterWidth,
                    SavedFilmBufferedTheta,
                    SavedFilmCurrentPositionTheta,
                    SavedFilmRecordStartTheta,
                    SavedFilmPieFraction,
                    MetagameTime,
                    MetagameTransientScore,
                    MetagameP1Score,
                    MetagameP2Score,
                    MetagameP3Score,
                    MetagameP4Score,
                    MetagameTimeMultiplier,
                    MetagameSkullDifficultyModifier,
                    MotionSensorRange,
                    NetworkLatency,
                    NetworkLatencyQuality,
                    NetworkHostQuality,
                    NetworkLocalQuality,
                    MetagameScoreNegative,
                    SurvivalCurrentSet,
                    SurvivalCurrentRound,
                    SurvivalCurrentWave,
                    SurvivalCurrentLives,
                    SurvivalBonusTime,
                    SurvivalBonusScore,
                    SurvivalMultiplier,
                    MetagameTotalModifier,
                    Achievement1Current,
                    Achievement2Current,
                    Achievement3Current,
                    Achievement4Current,
                    Achievement5Current,
                    Achievement1Goal,
                    Achievement2Goal,
                    Achievement3Goal,
                    Achievement4Goal,
                    Achievement5Goal,
                    Achievement1Icon,
                    Achievement2Icon,
                    Achievement3Icon,
                    Achievement4Icon,
                    Achievement5Icon,
                    UnknownX51,
                    UnknownX52,
                    Consumable3Icon,
                    Consumable4Icon,
                    ConsumableName,
                    UnknownX56,
                    UnknownX57,
                    UnknownX58,
                    ConsumableCooldownText,
                    ConsumableCooldownMeter,
                    UnknownX5b,
                    UnknownX5c,
                    UnknownX5d,
                    UnknownX5e,
                    Consumable1Charge,
                    Consumable2Charge,
                    Consumable3Charge,
                    Consumable4Charge,
                    UnknownX63,
                    UnknownX64,
                    EnergyMeter1,
                    EnergyMeter2,
                    EnergyMeter3,
                    EnergyMeter4,
                    EnergyMeter5,
                    Consumable1Cost,
                    Consumable2Cost,
                    Consumable3Cost,
                    Consumable4Cost,
                    UnitStaminaCurrent
                }

                public enum ChudRenderExternalInput_H3 : short
                {
                    Zero,
                    One,
                    DebugSlide1,    // ? Time
                    DebugSlide100,  // ? Fade
                    ShieldAmount,
                    ShieldRecentDamage,
                    WeaponAmmoLoaded,
                    WeaponAmmoReserve,
                    WeaponHeatFraction,
                    WeaponBatteryFraction,
                    ImpulseValue,
                    Autoaim,
                    GrenadeSelected,
                    GrenadeCount,
                    WeaponChargeFraction,
                    FriendlyScore,
                    EnemyScore,
                    ScoreToWin,
                    ArmingFraction,
                    LockingAmount,
                    Unit1xOvershieldCurrent,
                    Unit1xOvershieldRecentDmg,
                    Unit2xOvershieldCurrent,
                    Unit2xOvershieldRecentDmg,
                    Unit3xOvershieldCurrent,
                    Unit3xOvershieldRecentDmg,
                    CameraYaw,
                    CameraPitch,
                    TargetDistance,
                    TargetElevation,
                    EditorBudgetFraction,
                    EditorBudgetLeft,
                    SavedFilmTotalTime,
                    SavedFilmMarkerTime,
                    SavedFilmChapterWidth,
                    SavedFilmBufferedTheta,
                    SavedFilmCurrentPositionTheta,
                    SavedFilmRecordStartTheta,
                    SavedFilmPieFraction,
                    MetagameTime,
                    MetagameTransientScore,
                    MetagameP1Score,
                    MetagameP2Score,
                    MetagameP3Score,
                    MetagameP4Score,
                    MetagameTimeMultiplier,
                    MetagameSkullDifficultyModifer,
                    MotionSensorRange,
                    NetworkLatency,
                    NetworkLatencyQuality,
                    NetworkHostQuality,
                    NetworkLocalQuality,
                    MetagameScoreNegative,
                }

                public enum ChudRenderExternalInput_ODST : short
                {
                    Zero,
                    One,
                    DebugSlide1,        // ? Time
                    DebugSlide100,      // ? Fade
                    BodyVitality,
                    HealthRecentDamage,
                    ShieldAmount,
                    ShieldRecentDamage,
                    WeaponAmmoLoaded,
                    WeaponAmmoReserve,
                    WeaponHeatFraction,
                    WeaponBatteryFraction,
                    ImpulseValue,
                    Autoaim,
                    GrenadeSelected,
                    GrenadeCount,
                    WeaponChargeFraction,
                    FriendlyScore,
                    EnemyScore,
                    ScoreToWin,
                    ArmingFraction,
                    LockingAmount,
                    Unit1xOvershieldCurrent,
                    Unit1xOvershieldRecentDmg,
                    Unit2xOvershieldCurrent,
                    Unit2xOvershieldRecentDmg,
                    Unit3xOvershieldCurrent,
                    Unit3xOvershieldRecentDmg,
                    CameraYaw,
                    CameraPitch,
                    TargetDistance,
                    TargetElevation,
                    EditorBudgetFraction,
                    EditorBudgetLeft,
                    SavedFilmTotalTime,
                    SavedFilmMarkerTime,
                    SavedFilmChapterWidth,
                    SavedFilmBufferedTheta,
                    SavedFilmCurrentPositionTheta,
                    SavedFilmRecordStartTheta,
                    SavedFilmPieFraction,
                    MetagameTime,
                    MetagameTransientScore,
                    MetagameP1Score,
                    MetagameP2Score,
                    MetagameP3Score,
                    MetagameP4Score,
                    MetagameTimeMultiplier,
                    MetagameSkullDifficultyModifer,
                    MotionSensorRange,
                    NetworkLatency,
                    NetworkLatencyQuality,
                    NetworkHostQuality,
                    NetworkLocalQuality,
                    MetagameScoreNegative,
                    CompassUserTargetUnknown, // Unknown1
                    CompassUserTargetYaw, // CompassDistanceToUserTarget
                    CompassUserTargetDistance, // CompassDistanceToUserTarget2
                    CompassTargetUnknown, // Unknown2
                    CompassTargetYaw, // CompassDistanceToTarget
                    CompassTargetDistance, // CompassDistanceToTarget2
                    SurvivalCurrentSet,
                    SurvivalCurrentRound,
                    SurvivalCurrentWave,
                    SurvivalCurrentLives,
                    SurvivalBonusTime,
                    SurvivalBonusScore,
                    SurvivalMultiplier,
                    Achievement1Current,
                    Achievement2Current,
                    Achievement3Current,
                    Achievement4Current,
                    Achievement5Current,
                    Achievement1Goal,
                    Achievement2Goal,
                    Achievement3Goal,
                    Achievement4Goal,
                    Achievement5Goal,
                    Achievement1Icon,
                    Achievement2Icon,
                    Achievement3Icon,
                    Achievement4Icon,
                    Achievement5Icon,
                }

                public enum ChudRenderExternalInput_H3MCC : short
                {
                    Zero,
                    One,
                    DebugSlide1,    // ? Time
                    DebugSlide100,  // ? Fade
                    BodyVitality,       // MCC only
                    HealthRecentDamage, // MCC only
                    ShieldAmount,
                    ShieldRecentDamage,
                    WeaponAmmoLoaded,
                    WeaponAmmoReserve,
                    WeaponHeatFraction,
                    WeaponBatteryFraction,
                    ImpulseValue,
                    Autoaim,
                    GrenadeSelected,
                    GrenadeCount,
                    WeaponChargeFraction,
                    FriendlyScore,
                    EnemyScore,
                    ScoreToWin,
                    ArmingFraction,
                    LockingAmount,
                    Unit1xOvershieldCurrent,
                    Unit1xOvershieldRecentDmg,
                    Unit2xOvershieldCurrent,
                    Unit2xOvershieldRecentDmg,
                    Unit3xOvershieldCurrent,
                    Unit3xOvershieldRecentDmg,
                    CameraYaw,
                    CameraPitch,
                    TargetDistance,
                    TargetElevation,
                    EditorBudgetFraction,
                    EditorBudgetLeft,
                    SavedFilmTotalTime,
                    SavedFilmMarkerTime,
                    SavedFilmChapterWidth,
                    SavedFilmBufferedTheta,
                    SavedFilmCurrentPositionTheta,
                    SavedFilmRecordStartTheta,
                    SavedFilmPieFraction,
                    MetagameTime,
                    MetagameTransientScore,
                    MetagameP1Score,
                    MetagameP2Score,
                    MetagameP3Score,
                    MetagameP4Score,
                    MetagameTimeMultiplier,
                    MetagameSkullDifficultyModifer,
                    MotionSensorRange,
                    NetworkLatency,
                    NetworkLatencyQuality,
                    NetworkHostQuality,
                    NetworkLocalQuality,
                    MetagameScoreNegative,
                }

                public enum OutputColorValue : short
                {
                    LocalA,
                    LocalB,
                    LocalC,
                    LocalD,
                    ColorAnimationA, //just returns 0 for the color
                    ColorAnimationB, //just returns 0 for the color
                    ScoreboardFriendly,
                    ScoreboardEnemy,
                    ArmingMeter,
                    MetagamePlayer1,
                    MetagamePlayer2,
                    MetagamePlayer3,
                    MetagamePlayer4,
                    GameTimeRemaining, //returns LocalA if there is more than 60 seconds left, or LocalB if there is less
                    PrimaryBackground,      // global 0
                    SecondaryBackground,
                    HighlightForeground,
                    WarningFlash,
                    CrosshairNormal,
                    CrosshairEnemy,         // global 5
                    CrosshairFriendly,
                    BlipBase,
                    SelfBlip,
                    EnemyBlip,
                    NeutralBlip,            // global 10
                    FriendlyBlip,
                    BlipPing,
                    ObjectiveBlipOnRadar,
                    ObjectiveBlipOffRadar,
                    NavpointFriendly,       // global 15
                    NavpointNeutral,
                    NavpointEnemy,
                    NavpointAllyDead,
                    MessageFlashSelf,
                    MessageFlashFriendly,   // global 20
                    MessageFlashEnemy,
                    MessageFlashNeutral,
                    InvincibleShield,
                    NavpointAllyStandingBy,
                    NavpointAllyFiring,   // global 25
                    NavpointAllyTakingDamage,
                    NavpointAllySpeaking,
                }

                public enum OutputColorValue_HO : short
                {
                    LocalA,
                    LocalB,
                    LocalC,
                    LocalD,
                    ColorAnimationA,
                    ColorAnimationB,
                    ScoreboardFriendly,
                    ScoreboardEnemy,
                    ArmingMeter,
                    MetagamePlayer1,
                    MetagamePlayer2,
                    MetagamePlayer3,
                    MetagamePlayer4,
                    WeaponVersion,
                    GameTimeRemaining,      // Unknown14
                    PrimaryBackground,      // global 0
                    SecondaryBackground,
                    HighlightForeground,
                    WarningFlash,
                    CrosshairNormal,        // global 4
                    CrosshairEnemy,
                    CrosshairFriendly,
                    BlipBase,
                    SelfBlip,
                    EnemyBlip,
                    NeutralBlip,            // global 10
                    FriendlyBlip,
                    BlipPing,
                    ObjectiveBlipOnRadar,
                    ObjectiveBlipOffRadar,
                    NavpointFriendly,       // global 15
                    NavpointNeutral,
                    NavpointEnemy,
                    NavpointAllyDead,
                    NavpointAllyBlue,
                    MessageFlashSelf,        // global 20
                    MessageFlashFriendly,
                    MessageFlashEnemy,
                    MessageFlashNeutral,
                    InvincibleShield,
                    NavpointAllyStandingBy,
                    NavpointAllyFiring,
                    NavpointAllyTakingDamage,
                    NavpointAllySpeaking,
                    GlobalDynamic29_HO,     // White
                    WeaponOutlineDefault,
                    WeaponOutlineAmmo,
                    WeaponOutlineDamage,
                    WeaponOutlineAccuracy,
                    WeaponOutlineRateOfFire,
                    WeaponOutlineRange,
                    WeaponOutlinePower
                }

                public enum OutputScalarValue : short
                {
                    Input,
                    RangeInput,
                    LocalA,
                    LocalB,
                    LocalC,
                    LocalD,
                    FlashAnimation,
                    ScalarAnimationB
                }
            }

            [TagStructure(Size = 0x78, MinVersion = CacheVersion.HaloReach)]
            public class RenderDatumReach : TagStructure
            {
                public sbyte ShaderType;
                [TagField(Length = 3, Flags = Padding)]
                public byte[] Padding1;
                public RealArgbColor LocalColorA;
                public RealArgbColor LocalColorB;
                public RealArgbColor LocalColorC;
                public RealArgbColor LocalColorD;
                public float LocalScalarA;
                public float LocalScalarB;
                public float LocalScalarC;
                public float LocalScalarD;
                public short OutputColorA;
                public short OutputColorB;
                public short OutputColorC;
                public short OutputColorD;
                public short OutputColorE;
                public short OutputColorF;
                public short OutputScalarA;
                public short OutputScalarB;
                public short OutputScalarC;
                public short OutputScalarD;
                public short OutputScalarE;
                public short OutputScalarF;
                public Rectangle2d ScissorRect;
                public RenderBlendMode AlphaBlendMode;
                [TagField(Length = 0x2, Flags = Padding)]
                public byte[] VMOWELA;

                public enum RenderBlendMode : short
                {
                    Opaque,
                    Additive,
                    Multiply,
                    AlphaBlend,
                    DoubleMultiply,
                    PreMultipliedAlpha,
                    Maximum,
                    MultiplyAdd,
                    AddSrcTimesDstalpha,
                    AddSrcTimesSrcalpha,
                    InverseAlphaBlend,
                    MotionBlurStatic,
                    MotionBlurInhibit,
                    ApplyShadowToShadowMask,
                    AlphaBlendConstant,
                    OverdrawApply,
                    WetEffect,
                    Minimum
                }
            }
        }

        [TagStructure(Size = 0x18, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x28, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x64, MinVersion = CacheVersion.HaloReach)]
        public class HudWidget : HudWidgetBase
        {
            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public CachedTag ParallaxData;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public CachedTag DatasourceTemplate;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<NullBlock> Datasource;

            public List<BitmapWidget> BitmapWidgets;
            public List<TextWidget> TextWidgets;

            [TagStructure(Size = 0x1C, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x24, MinVersion = CacheVersion.HaloReach)]
            public class BitmapWidget : HudWidgetBase
            {
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public int RuntimeWidgetIndex;

                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public WidgetBitmapFlagsH3 FlagsH3;

                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public WidgetBitmapFlagsODST FlagsODST;

                [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnlineED)]
                public WidgetBitmapFlagsHO Flags;

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public WidgetBitmapFlagsReach FlagsReach;

                [TagField(Length = 2, Flags = Padding, MaxVersion = CacheVersion.Halo3ODST)]
                public byte[] Padding0;

                public CachedTag Bitmap;
                public byte BitmapSequenceIndex;

                [TagField(Length = 3, Flags = Padding)]
                public byte[] Padding2;

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public Rectangle2d ClipBounds;

                [Flags]
                public enum WidgetBitmapFlagsH3 : ushort
                {
                    None,
                    MirrorHorizontally = 1 << 0,
                    MirrorVertically = 1 << 1,
                    ExtendBorder = 1 << 2, // stretch edges
                    UseTextureCam = 1 << 3,
                    UseWrapSampling = 1 << 4, // looping
                    AutoPermute = 1 << 5,
                    Player1Emblem = 1 << 6,
                    Player2Emblem = 1 << 7,
                    Player3Emblem = 1 << 8,
                    Player4Emblem = 1 << 9,
                    ScaleAlphaByColOutA = 1 << 10,
                    Stretch = 1 << 11, // NCC
                }

                [Flags]
                public enum WidgetBitmapFlagsODST : ushort
                {
                    None,
                    MirrorHorizontally = 1 << 0,
                    MirrorVertically = 1 << 1,
                    ExtendBorder = 1 << 2, // stretch edges
                    UseTextureCam = 1 << 3,
                    UseWrapSampling = 1 << 4, // looping
                    SpriteFromPlayerCharacterType = 1 << 5,
                    SpriteFromSurivalRounds = 1 << 6,
                    SpriteFromUnknon1 = 1 << 7,
                    SpriteFromUnknon2 = 1 << 8,
                    Player1Emblem = 1 << 9,
                    Player2Emblem = 1 << 10,
                    Player3Emblem = 1 << 11,
                    Player4Emblem = 1 << 12,
                    ScaleAlphaByColOutA = 1 << 13,
                    Stretch = 1 << 14, // MCC
                }

                [Flags]
                public enum WidgetBitmapFlagsHO : uint
                {
                    None,
                    MirrorHorizontally = 1 << 0,
                    MirrorVertically = 1 << 1,
                    ExtendBorder = 1 << 2, // stretch edges
                    UseTextureCam = 1 << 3,
                    UseWrapSampling = 1 << 4, // looping
                    SpriteFromPlayerCharacterType = 1 << 5,
                    SpriteFromSurivalRounds = 1 << 6,
                    SpriteFromUnknon1 = 1 << 7,
                    SpriteFromUnknon2 = 1 << 8,
                    Player1Emblem = 1 << 9,
                    Player2Emblem = 1 << 10,
                    Player3Emblem = 1 << 11,
                    Player4Emblem = 1 << 12,
                    ScaleAlphaByColOutA = 1 << 13,
                    SpriteFromConsumable = 1 << 14,
                    SpriteFromWeapon = 1 << 15,
                    Stretch = 1 << 16, // ED
                }

                [Flags]
                public enum WidgetBitmapFlagsReach : uint
                {
                    None,
                    MirrorHorizontally = 1 << 0,
                    MirrorVertically = 1 << 1,
                    ExtendBorder = 1 << 2, // stretch edges
                    UseTextureCam = 1 << 3,
                    UseWrapSampling = 1 << 4, // looping
                    SpriteFromPlayerCharacterType = 1 << 5,
                    SpriteFromSurivalRounds = 1 << 6,
                    SpriteFromUnknon1 = 1 << 7,
                    SpriteFromUnknon2 = 1 << 8,
                    Player1Emblem = 1 << 9,
                    Player2Emblem = 1 << 10,
                    Player3Emblem = 1 << 11,
                    Player4Emblem = 1 << 12,
                    ScaleAlphaByColOutA = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15,
                    Bit16 = 1 << 16,
                    Bit17 = 1 << 17,
                    Bit18 = 1 << 18,
                    Bit19 = 1 << 19,
                    Bit20 = 1 << 20,
                    Bit21 = 1 << 21,
                    Bit22 = 1 << 22,
                    Bit23 = 1 << 23,
                    Bit24 = 1 << 24,
                    Bit25 = 1 << 25,
                    Bit26 = 1 << 26,
                    Bit27 = 1 << 27,
                    Bit28 = 1 << 28,
                    Bit29 = 1 << 29,
                    Bit30 = 1 << 30,
                    Bit31 = 1u << 31
                }
            }

            [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
            [TagStructure(Size = 0xC, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
            [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0xC, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original)]
            public class TextWidget : HudWidgetBase
            {
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public int WidgetIndex;

                // flags

                [TagField(MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
                public WidgetTextFlags_H3Original TextFlags_H3Original; // ushort

                [TagField(MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
                public WidgetTextFlags_H3MCC TextFlags_H3MCC; // uint

                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
                public WidgetTextFlags TextFlags; // uint

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public ushort FlagsReach; // ushort

                // fonts
                [TagField(MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
                public WidgetFontValue_H3Original Font_H3; // short

                [TagField(EnumType = typeof(int), MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
                public WidgetFontValue_H3MCC Font_H3MCC; // int

                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
                public WidgetFontValue_ODST Font_ODST; // short

                [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
                public WidgetFontValue Font; // short

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public ushort FontReach; // short

                [TagField(Length = 2, Flags = Padding, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
                public byte[] FontPadding;

                public StringId InputString;

                [Flags]
                public enum WidgetTextFlags_H3Original : ushort
                {
                    StringIsANumber = 1 << 0,
                    Force2DigitNumber = 1 << 1,
                    Force3DigitNumber = 1 << 2,
                    SuffixWithPlus = 1 << 3,
                    SuffixWithM = 1 << 4,
                    Decimal2Digits = 1 << 5,
                    Decimal3Digits = 1 << 6,
                    Decimal5Digits = 1 << 7,
                    SuperHugeNumber = 1 << 8,
                    SuffixWithX = 1 << 9,
                    WrapWithBrackets = 1 << 10,
                    FormatAsTime = 1 << 11,
                    FormatAsHhmmssTime = 1 << 12,
                    FormatAsBudgetNumber = 1 << 13,
                    PrefixWithMinus = 1 << 14,
                    OnlyAxesGlobal = 1 << 15
                }

                [Flags]
                public enum WidgetTextFlags_H3MCC : uint
                {
                    StringIsANumber = 1 << 0,
                    Force2DigitNumber = 1 << 1,
                    Force3DigitNumber = 1 << 2,
                    SuffixWithPlus = 1 << 3,
                    SuffixWithM = 1 << 4,
                    Decimal2Digits = 1 << 5,
                    Decimal3Digits = 1 << 6,
                    Decimal5Digits = 1 << 7,
                    SuperHugeNumber = 1 << 8,
                    SuffixWithX = 1 << 9,
                    WrapWithBrackets = 1 << 10,
                    FormatAsTime = 1 << 11,
                    FormatAsHhmmssTime = 1 << 12,
                    FormatAsBudgetNumber = 1 << 13,
                    PrefixWithMinus = 1 << 14,
                    OnlyAxesGlobal = 1 << 15,
                    OnlyAxesLocal = 1 << 16
                }

                [Flags]
                public enum WidgetTextFlags : uint
                {
                    None,
                    StringIsANumber = 1 << 0,
                    Force2DigitNumber = 1 << 1,
                    Force3DigitNumber = 1 << 2,
                    SuffixWithPlus = 1 << 3,
                    SuffixWithM = 1 << 4,
                    Decimal1Digit = 1 << 5,
                    Decimal2Digits = 1 << 6,
                    Decimal3Digits = 1 << 7,
                    Decimal5Digits = 1 << 8,
                    SuperHugeNumber = 1 << 9,
                    SuffixWithX = 1 << 10,
                    WrapWithBrackets = 1 << 11,
                    FormatAsTime = 1 << 12,
                    FormatAsHhmmssTime = 1 << 13,
                    FormatAsBudgetNumber = 1 << 14,
                    PrefixWithMinus = 1 << 15,
                    CenterHorizontally = 1 << 16,
                    OnlyAxesGlobal = 1 << 17,
                    OnlyAxesLocal = 1 << 18,
                    Bit19 = 1 << 19,
                    Bit20 = 1 << 20,
                    Bit21 = 1 << 21,
                    Bit22 = 1 << 22,
                    Bit23 = 1 << 23,
                    Bit24 = 1 << 24,
                    Bit25 = 1 << 25,
                    Bit26 = 1 << 26,
                    Bit27 = 1 << 27,
                    Bit28 = 1 << 28,
                    Bit29 = 1 << 29,
                    Bit30 = 1 << 30,
                }
            }
        }

        [TagStructure(Size = 0x0)]
        public class NullBlock : TagStructure
        {

        }

        public enum ChudScriptingClass : short
        {
            UseParent,
            WeaponStats,
            Crosshair,
            Shield,
            Grenades,
            Messages, //gametype related
            MotionSensor,
            SpikeGrenade,
            FireGrenade,
            Compass,
            Stamina,
            EnergyMeter,
            Consumable
        }

        [Flags]
        public enum WidgetFlags : byte
        {
            None,
            DieOnActive = 1 << 0,
            SkipInputUpdateWhenUnreadying = 1 << 1,
            ResetTimerOnInputChange = 1 << 2,
            WeaponSwapHack = 1 << 3,
            EquipmentHack = 1 << 4
        }

        public enum WidgetLayerEnum : byte
        {
            Background,
            Flashes,
            Parent,
            Middle,
            Child,
            Foreground,
            Inherited,
            SavedFilm
        }
    }
}
