using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "chud_definition", Tag = "chdt", Size = 0x18)]
    public class ChudDefinition : TagStructure
	{
        public List<HudWidget> HudWidgets;
        public int LowClipCutoff;
        public int LowAmmoCutoff;
        public int AgeCutoff;

        [TagStructure(Size = 0x50, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x60, MinVersion = CacheVersion.HaloOnline106708)]
        public class HudWidget : TagStructure
		{
            [TagField(Label = true)]
            public StringId Name;
            public SpecialHudTypeValue SpecialHudType;
            public byte Unknown;
            public byte Unknown2;
            public List<StateDatum> StateData;
            public List<PlacementDatum> PlacementData;
            public List<AnimationDatum> AnimationData;
            public List<RenderDatum> RenderData;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public CachedTagInstance ParallaxData;

            public List<BitmapWidget> BitmapWidgets;
            public List<TextWidget> TextWidgets;

            public enum SpecialHudTypeValue : short
            {
                Unspecial,
                Ammo,
                CrosshairAndScope,
                UnitShieldMeter,
                Grenades,
                Gametype,
                MotionSensor,
                SpikeGrenade,
                FirebombGrenade,
                Compass,
                Stamina,
                EnergyMeter,
                Consumable
            }

            [TagStructure(Size = 0x28, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x38, MaxVersion = CacheVersion.Halo3ODST)]
            [TagStructure(Size = 0x44, MaxVersion = CacheVersion.HaloOnline571627)]
            [TagStructure(Size = 0x48, MinVersion = CacheVersion.HaloOnline700123)]
            public class StateDatum : TagStructure
			{
                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public Engine_H3 EngineFlags_H3;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public Engine_ODST EngineFlags_ODST;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public Engine EngineFlags;

                public PlayerType PlayerTypeFlags;

                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public PDA PDAFlags;

                public Multiplayer MultiplayerFlags;
                public Resolution ResolutionFlags;

                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public MultiplayerEvents_H3 MultiplayerEventsFlags_H3;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public MultiplayerEvents MultiplayerEventsFlags;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public UnitBase UnitBaseFlags;
                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public UnitBase_H3 UnitBaseFlags_H3;

                public Editor EditorFlags;

                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public EngineGeneralH3 EngineGeneralFlags_H3;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public EngineGeneral EngineGeneralFlags;               

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

                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public GeneralKudos GeneralKudosFlags;
                [TagField(MaxVersion = CacheVersion.Halo3ODST)]         
                public GeneralKudos_H3 GeneralKudosFlags_H3;

				public UnitZoom UnitZoomFlags;
                public UnitInventory UnitInventoryFlags;

                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public ushort Unused3;

                public UnitGeneral UnitGeneralFlags;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public ushort Unused4;

                public WeaponKudos WeaponKudosFlags;
                public WeaponStatus WeaponStatusFlags;
                public WeaponTarget WeaponTargetFlags;
                public WeaponTargetB WeaponTargetBFlags;

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
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public short UnusedFlags4;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public Consumable ConsumableFlags;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public EnergyMeter EnergyMeterFlags;

                [Flags]
                public enum Engine : ushort
                {
                    None,
                    CampaignSolo = 1 << 0,
                    CampaignCoop = 1 << 1,
                    Survival = 1 << 2, //not sure about this one
                    FFAgame = 1 << 3,
                    Teamgame = 1 << 4,
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
                public enum Engine_ODST : ushort
                {
                    None,
                    CampaignSolo = 1 << 0,
                    CampaignCoop = 1 << 1,
                    Survival = 1 << 2,
                    Editor = 1 << 3,
                    Theater = 1 << 4,
                }

                [Flags]
                public enum Engine_H3 : ushort
                {
                    None,
                    CampaignSolo = 1 << 0,
                    CampaignCoop = 1 << 1,
                    Teamgame = 1 << 2,
					FFAgame = 1 << 3,
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
                public enum PlayerType : ushort
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
                    SplitscreenPDAOpen = 1 << 0,
                    SplitscreenPDAClose = 1 << 1,
                    CameraUnknown = 1 << 2,
                }

                [Flags]
                public enum PDA2 : ushort
                {
                    None,
                    VisibleInPDA = 1 << 0,
                }

                [Flags]
                public enum Multiplayer : ushort
                {
                    None,
                    OffenseTeam = 1 << 0,
                    DefenseTeam = 1 << 1,
                    NormalTeam = 1 << 2,
                    SpecialTeam = 1 << 3,
                    Extraspecialteam = 1 << 4, //broken?
                    NoMicrophone = 1 << 5,
                    TalkingDisabled = 1 << 6,
                    TapToTalk = 1 << 7,
                    TalkingEnabled = 1 << 8,
                    NotTalking = 1 << 9,
                    Talking = 1 << 10,
                }

                [Flags]
                public enum Resolution : ushort
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
                public enum MultiplayerEvents : uint
                {
                    None,
                    HasFriends = 1 << 0,
                    HasEnemies = 1 << 1,
                    VariantNameValid = 1 << 2,
                    SomeoneIsTalking = 1 << 3,
                    IsArming = 1 << 4,
                    TimeEnabled = 1 << 5,
                    FriendsHaveX = 1 << 6,
                    EnemiesHaveX = 1 << 7,
                    FriendsAreX = 1 << 8,
                    EnemiesAreX = 1 << 9,
                    XIsDown = 1 << 10,
                    AttackerBombDropped = 1 << 11,
                    AttackerBombPickedUp = 1 << 12,
                    DefenderTeamIsDead = 1 << 13,
                    AttackerTeamIsDead = 1 << 14,
                    SummaryEnabled = 1 << 15,
                    Unknown16 = 1 << 16,
                    NetDebugEnabled = 1 << 17
                }

                [Flags]
                public enum MultiplayerEvents_H3 : ushort
                {
                    None,
                    HasFriends = 1 << 0,
                    HasEnemies = 1 << 1,
                    VariantNameValid = 1 << 2,
                    SomeoneIsTalking = 1 << 3,
                    IsArming = 1 << 4,
                    TimeEnabled = 1 << 5,
                    FriendsHaveX = 1 << 6,
                    EnemiesHaveX = 1 << 7,
                    FriendsAreX = 1 << 8,
                    EnemiesAreX = 1 << 9,
                    XIsDown = 1 << 10,
                    SummaryEnabled = 1 << 11,
                    NetDebugEnabled = 1 << 12,
                }

                [Flags]
                public enum UnitBase_H3 : ushort
                {
                    None,
                    TextureCamEnabled = 1 << 0,
                    BinocularsTargeted = 1 << 1,
                    Bit2 = 1 << 2,
                    Bit3 = 1 << 3,
                    TrainingPrompt = 1 << 4,
                    ObjectivePrompt = 1 << 5,
                }

                [Flags]
                public enum UnitBase : uint
                {
                    None,
                    TextureCamEnabled = 1 << 0,
                    BinocularsTargeted = 1 << 1,
                    Bit2 = 1 << 2,
                    Bit3 = 1 << 3,
                    TrainingPrompt = 1 << 4,
                    ObjectivePrompt = 1 << 5,
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
                public enum Editor : ushort
                {
                    None,
                    EditorInactive = 1 << 0,
                    EditorActive = 1 << 1,
                    EditorHolding = 1 << 2,
                    EditorNotAllowed = 1 << 3,
                    IsEditorBiped = 1 << 4,
                }

                [Flags]
                public enum EngineGeneralH3 : ushort
                {
                    None,
                    MotionTracker10M = 1 << 0,
                    MotionTracker25M = 1 << 1,
                    MotionTracker75M = 1 << 2,
                    MotionTracker150M = 1 << 3,
                    MetagameDebugEnabled = 1 << 4,
                    Bit5 = 1 << 5,
                    MetagamePlayer2Exists = 1 << 6,
                    Bit7 = 1 << 7,
                    MetagamePlayer3Exists = 1 << 8,
                    Bit9 = 1 << 9,
                    MetagamePlayer4Exists = 1 << 10,
                    Bit11 = 1 << 11,
                    MetagameScoreAdded = 1 << 12,
                    MetagameMultikill = 1 << 13,
                    MetagameScoreRemoved = 1 << 14,
                    Bit14 = 1 << 15,
                }

                [Flags]
                public enum EngineGeneral : ushort
                {
                    None,
                    MotionTracker10M = 1 << 0,
                    MotionTracker25M = 1 << 1,
                    MotionTracker75M = 1 << 2,
                    MotionTracker150M = 1 << 3,
                    MetagameDebugEnabled = 1 << 4,
                    MetagamePlayer2Exists = 1 << 5,
                    Bit6 = 1 << 6,
                    MetagamePlayer3Exists = 1 << 7,
                    Bit8 = 1 << 8,
                    MetagamePlayer4Exists = 1 << 9,
                    Bit10 = 1 << 10,
                    MetagameScoreAdded = 1 << 11,
                    MetagameMultikill = 1 << 12,
                    MetagameScoreRemoved = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15
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
                    Bit10 = 1 << 10,
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
                    UnitIsZoomedLevel2 = 1 << 2,
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
                public enum UnitInventory : ushort
                {
                    None = 1 << 0,
                    IsSingleWielding = 1 << 1,
                    IsDualWielding = 1 << 2,
                    HasSupportWeapon = 1 << 3,
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
				public enum WeaponKudos : ushort
				{
					None,
					Bit0 = 1 << 0,
					PickupAmmo = 1 << 1,
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

				[Flags]
				public enum WeaponTarget : ushort
				{
					None,
					NoTarget = 1 << 0,
					FriendlyTarget = 1 << 1,
					EnemyTarget = 1 << 2,
					HeadshotTarget = 1 << 3,
					VulnerableTarget = 1 << 4,
					InvincibleTarget = 1 << 5, //Defunct
					PlasmaLocked = 1 << 6,
					PlasmaLocking = 1 << 7,
					PlasmaLockAvailable = 1 << 8,
					Bit9 = 1 << 9,
					Bit10 = 1 << 10,
				}

				[Flags]
				public enum WeaponTargetB : ushort
				{
					None,
					HumanLocking = 1 << 0,
					HumanLocked = 1 << 1,
					HumanLockAvailable = 1 << 2,
				}

                [Flags]
                public enum WeaponStatus : ushort
                {
                    None,
                    SourceIsPrimaryWeapon = 1 << 0,
                    SourceIsDualWeapon = 1 << 1,
                    SourceIsBackpacked = 1 << 2,
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
					HasFireGrenades = 1 << 9,
					Unknown1 = 1 << 10,
					Unknown2 = 1 << 11,
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

            [TagStructure(Size = 0x1C)]
            public class PlacementDatum : TagStructure
			{
                [TagField(Label = true)]
                public AnchorValue Anchor;
                public short Unknown;
                public RealPoint2d MirrorOffset;
                public RealPoint2d Offset;
                public RealPoint2d Scale;

                public enum AnchorValue : short
                {
                    TopLeft,
                    TopRight,
                    BottomRight,
                    BottomLeft,
                    Center,
                    TopEdge,
                    GrenadeA,
                    GrenadeB,
                    GrenadeC,
                    GrenadeD,
                    ScoreboardFriendly,
                    ScoreboardEnemy,
                    HealthAndShield,
                    BottomEdge,
                    Unknown,
                    Equipment,
                    Unknown2,
                    Depreciated,
                    Depreciated2,
                    Depreciated3,
                    Depreciated4,
                    Depreciated5,
                    Unknown3,
                    Gametype,
                    Unknown4,
                    StateRight,
                    StateLeft,
                    StateCenter,
                    Unknown5,
                    GametypeFriendly,
                    GametypeEnemy,
                    MetagameTop,
                    MetagamePlayer1,
                    MetagamePlayer2,
                    MetagamePlayer3,
                    MetagamePlayer4,
                    Theater,
                    Unknown6 //ODST
                }
            }

            [TagStructure(Size = 0x78, MaxVersion = CacheVersion.Halo3ODST)]
            [TagStructure(Size = 0x90, MinVersion = CacheVersion.HaloOnline106708)]
            public class AnimationDatum : TagStructure
			{
                //HUD Initialize Animation
                public AnimationFlags HUDInitializeFlags;
                public AnimationFunction HUDInitializeFunction;
                public CachedTagInstance HUDInitialize;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float HUDInitializeUnknown;

                //Idle Animation
                public AnimationFlags IdleFlags;
                public AnimationFunction IdleFunction;
                public CachedTagInstance Idle;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float IdleUnknown;

                //Special State Animation
                public AnimationFlags SpecialStateFlags;
                public AnimationFunction SpecialStateFunction;
                public CachedTagInstance SpecialState;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float SpecialStateUnknown;

                //Transition In Animation
                public AnimationFlags TransitionInFlags;
                public AnimationFunction TransitionInFunction;
                public CachedTagInstance TransitionIn;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float TransitionInUnknown;

                //Transition Out Animation
                public AnimationFlags TransitionOutFlags;
                public AnimationFunction TransitionOutFunction;
                public CachedTagInstance TransitionOut;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float TransitionOutUnknown;

                //Brief State Animation
                public AnimationFlags BriefStateFlags;
                public AnimationFunction BriefStateFunction;
                public CachedTagInstance BriefState;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float BriefStateUnknown;

                [Flags]
                public enum AnimationFlags : short
                {
                    None,
                    ReverseFrames = 1 << 0,
                }

                public enum AnimationFunction : short
                {
                    Default,
                    UseInput,
                    UseRangeInput,
                    UseCompassTarget, //?
                    UseUserTarget //?
                }
            }

            [TagStructure(Size = 0x40, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x48, MinVersion = CacheVersion.Halo3ODST)]
            public class RenderDatum : TagStructure
			{
                [TagField(Label = true)]
                public ShaderIndexValue ShaderIndex;
                public short Unknown;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public InputValue_HO Input_HO;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public InputValue_HO RangeInput_HO;
                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public InputValue_H3 Input_H3;
                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public InputValue_H3 RangeInput_H3;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public InputValue_ODST Input_ODST;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public InputValue_ODST RangeInput_ODST;
                public ArgbColor LocalColorA;
                public ArgbColor LocalColorB;
                public ArgbColor LocalColorC;
                public ArgbColor LocalColorD;
                public float LocalScalarA;
                public float LocalScalarB;
                public float LocalScalarC;
                public float LocalScalarD;
                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public OutputColorValue OutputColorA;
                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public OutputColorValue OutputColorB;
                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public OutputColorValue OutputColorC;
                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public OutputColorValue OutputColorD;
                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public OutputColorValue OutputColorE;
                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public OutputColorValue OutputColorF;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public OutputColorValue_HO OutputColorA_HO;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public OutputColorValue_HO OutputColorB_HO;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public OutputColorValue_HO OutputColorC_HO;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public OutputColorValue_HO OutputColorD_HO;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public OutputColorValue_HO OutputColorE_HO;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public OutputColorValue_HO OutputColorF_HO;
                public OutputScalarValue OutputScalarA;
                public OutputScalarValue OutputScalarB;
                public OutputScalarValue OutputScalarC;
                public OutputScalarValue OutputScalarD;
                public OutputScalarValue OutputScalarE;
                public OutputScalarValue OutputScalarF;
				[TagField(MinVersion = CacheVersion.Halo3ODST)]
                public short Unknown2;
				[TagField(MinVersion = CacheVersion.Halo3ODST)]
				public short Unknown3;
				[TagField(MinVersion = CacheVersion.Halo3ODST)]
				public short Unknown4;
				[TagField(MinVersion = CacheVersion.Halo3ODST)]
				public short Unknown5;

                public enum ShaderIndexValue : short
                {
                    Simple,
                    Meter,
                    TextSimple,
                    MeterShield,
                    MeterGradient,
                    Crosshair,
                    DirectionalDamage,
                    Solid,
                    Sensor,
                    MeterSingleColor,
                    Navpoint,
                    Medal,
                    TextureCam,
                    CortanaScreen,
                    CortanaCamera,
                    CortanaOffscreen,
                    CortanaScreenFinal,
                    MeterChapter,
                    MeterDoubleGradient,
                    MeterRadialGradient,
                    Turbulence,
                    Emblem,
                    CortanaComposite,
                    DirectionalDamageApply,
                    ReallySimple,
                    Unknown
                }

                public enum InputValue_HO : short
                {
                    Zero,
                    One,
                    Time,
                    Fade,
                    UnitHealthCurrent,
                    UnitHealth,
                    UnitShieldCurrent,
                    UnitShield,
                    ClipAmmoFraction,
                    TotalAmmoFraction,
                    WeaponVersionNumber,
                    HeatFraction,
                    BatteryFraction,
                    WeaponErrorCurrent1,
                    WeaponErrorCurrent2,
                    Pickup,
                    UnitAutoaimed,
                    Grenade,
                    GrenadeFraction,
                    ChargeFraction,
                    FriendlyScore,
                    EnemyScore,
                    ScoreToWin,
                    ArmingFraction,
                    UnknownX18,
                    Unit1xOvershieldCurrent,
                    Unit1xOvershield,
                    Unit2xOvershieldCurrent,
                    Unit2xOvershield,
                    Unit3xOvershieldCurrent,
                    Unit3xOvershield,
                    AimYaw,
                    AimPitch,
                    TargetDistance,
                    TargetElevation,
                    EditorBudget,
                    EditorBudgetCost,
                    FilmTotalTime,
                    FilmCurrentTime,
                    UnknownX27,
                    FilmTimelineFraction1,
                    FilmTimelineFraction2,
                    UnknownX2a,
                    UnknownX2b,
                    MetagameTime,
                    MetagameScoreTransient,
                    MetagameScorePlayer1,
                    MetagameScorePlayer2,
                    MetagameScorePlayer3,
                    MetagameScorePlayer4,
                    MetagameModifier,
                    MetagameSkullModifier,
                    SensorRange,
                    NetdebugLatency,
                    NetdebugLatencyQuality,
                    NetdebugHostQuality,
                    NetdebugLocalQuality,
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

                public enum InputValue_ODST : short
                {
                    Zero,
                    One,
                    Time,
                    Fade,
                    UnitHealthCurrent,
                    UnitHealth,
                    UnitShieldCurrent,
                    UnitShield,
                    ClipAmmoFraction,
                    TotalAmmoFraction,
                    HeatFraction,
                    BatteryFraction,
                    Pickup,
                    UnitAutoaimed,
                    Grenade,
                    GrenadeFraction,
                    ChargeFraction,
                    FirstPlaceScore,
                    SecondPlaceScore,
                    ScoreToWin,
                    ArmingFraction,
                    UnknownX18,
                    Unit1xOvershieldCurrent,
                    Unit1xOvershield,
                    Unit2xOvershieldCurrent,
                    Unit2xOvershield,
                    Unit3xOvershieldCurrent,
                    Unit3xOvershield,
                    AimYaw,
                    AimPitch,
                    TargetDistance,
                    TargetElevation,
                    EditorBudget,
                    EditorBudgetCost,
                    FilmTotalTime,
                    FilmCurrentTime,
                    UnknownX27,
                    FilmTimelineFraction1,
                    FilmTimelineFraction2,
                    UnknownX2a,
                    UnknownX2b,
                    MetagameTime,
                    MetagameScoreTransient,
                    MetagameScorePlayer1,
                    MetagameScorePlayer2,
                    MetagameScorePlayer3,
                    MetagameScorePlayer4,
                    MetagameModifier,
                    MetagameSkullModifier,
                    SensorRange,
                    NetdebugLatency,
                    NetdebugLatencyQuality,
                    NetdebugHostQuality,
                    NetdebugLocalQuality,
                    MetagameScoreNegative,
                    Unknown1,
                    CompassDistanceToUserTarget,
                    CompassDistanceToUserTarget2,
                    Unknown2,
                    CompassDistanceToTarget,
                    CompassDistanceToTarget2,
                    Unknown3,
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

                public enum InputValue_H3 : short
                {
                    Zero,
                    One,
                    Time,
                    Fade,
                    UnitShieldCurrent,
                    UnitShield,
                    ClipAmmoFraction,
                    TotalAmmoFraction,
                    HeatFraction,
                    BatteryFraction,
                    Pickup,
                    UnitAutoaimed,
                    Grenade,
                    GrenadeFraction,
                    ChargeFraction,
                    FirstPlaceScore,
                    SecondPlaceScore,
                    ScoreToWin,
                    ArmingFraction,
                    UnknownX18,
                    Unit1xOvershieldCurrent,
                    Unit1xOvershield,
                    Unit2xOvershieldCurrent,
                    Unit2xOvershield,
                    Unit3xOvershieldCurrent,
                    Unit3xOvershield,
                    AimYaw,
                    AimPitch,
                    TargetDistance,
                    TargetElevation,
                    EditorBudget,
                    EditorBudgetCost,
                    FilmTotalTime,
                    FilmCurrentTime,
                    UnknownX27,
                    FilmTimelineFraction1,
                    FilmTimelineFraction2,
                    UnknownX2a,
                    UnknownX2b,
                    MetagameTime,
                    MetagameScoreTransient,
                    MetagameScorePlayer1,
                    MetagameScorePlayer2,
                    MetagameScorePlayer3,
                    MetagameScorePlayer4,
                    MetagameModifier,
                    Unknown,
                    SensorRange,
                    NetdebugLatency,
                    NetdebugLatencyQuality,
                    NetdebugHostQuality,
                    NetdebugLocalQuality,
                    MetagameScoreNegative,
                }

                public enum OutputColorValue :short
                {
                    LocalA,
                    LocalB,
                    LocalC,
                    LocalD,
                    Unknown4,
                    Unknown5,
                    ScoreboardFriendly,
                    ScoreboardEnemy,
                    ArmingTeam,
                    MetagamePlayer1,
                    MetagamePlayer2,
                    MetagamePlayer3,
                    MetagamePlayer4,
                    Unknown14,
                    GlobalDynamic0,
                    GlobalDynamic1,
                    GlobalDynamic2,
                    GlobalDynamic3,
                    GlobalDynamic4,
                    GlobalDynamic5,
                    GlobalDynamic6,
                    GlobalDynamic7,
                    GlobalDynamic8,
                    GlobalDynamic9,
                    GlobalDynamic10,
                    GlobalDynamic11,
                    GlobalDynamic12,
                    GlobalDynamic13,
                    GlobalDynamic14,
                    GlobalDynamic15,
                    GlobalDynamic16,
                    GlobalDynamic17,
                    GlobalDynamic18,
                    GlobalDynamic19,
                    GlobalDynamic20,
                    GlobalDynamic21,
                    GlobalDynamic22,
                    GlobalDynamic23,
                    GlobalDynamic24,
                    GlobalDynamic25,
                    GlobalDynamic26,
                    GlobalDynamic27,
                }

                public enum OutputColorValue_HO : short
                {
                    LocalA,
                    LocalB,
                    LocalC,
                    LocalD,
                    Unknown4,
                    Unknown5,
                    ScoreboardFriendly,
                    ScoreboardEnemy,
                    ArmingTeam,
                    MetagamePlayer1,
                    MetagamePlayer2,
                    MetagamePlayer3,
                    MetagamePlayer4,
                    WeaponVersion,
                    Unknown14,
                    GlobalDynamic0,
                    GlobalDynamic1,
                    GlobalDynamic2,
                    GlobalDynamic3,
                    GlobalDynamic4,
                    GlobalDynamic5,
                    GlobalDynamic6,
                    GlobalDynamic7,
                    GlobalDynamic8,
                    GlobalDynamic9,
                    GlobalDynamic10,
                    GlobalDynamic11,
                    GlobalDynamic12,
                    GlobalDynamic13,
                    GlobalDynamic14,
                    GlobalDynamic15,
                    GlobalDynamic16,
                    GlobalDynamic17,
                    GlobalDynamic18,
                    GlobalDynamic19,
                    GlobalDynamic20,
                    GlobalDynamic21,
                    GlobalDynamic22,
                    GlobalDynamic23,
                    GlobalDynamic24,
                    GlobalDynamic25,
                    GlobalDynamic26,
                    GlobalDynamic27,
                    GlobalDynamic28,
                    GlobalDynamic29,
                    GlobalDynamic30,
                    GlobalDynamic31,
                    GlobalDynamic32,
                    GlobalDynamic33,
                    GlobalDynamic34,
                    GlobalDynamic35,
                    GlobalDynamic36
                }
                
                public enum OutputScalarValue : short
                {
                    Input,
                    RangeInput,
                    LocalA,
                    LocalB,
                    LocalC,
                    LocalD,
                    Unknown6,
                    Unknown7
                }
            }

            [TagStructure(Size = 0x54)]
            public class BitmapWidget : TagStructure
			{
                [TagField(Label = true)]
                public StringId Name;
                public SpecialHudTypeValue SpecialHudType;
                public byte Unknown;
                public byte Unknown2;
                public List<StateDatum> StateData;
                public List<PlacementDatum> PlacementData;
                public List<AnimationDatum> AnimationData;
                public List<RenderDatum> RenderData;
                public int WidgetIndex;
                public FlagsValue Flags;
                public short Unknown3;
                public CachedTagInstance Bitmap;
                public byte BitmapSpriteIndex;
                public byte Unknown4;
                public byte Unknown5;
                public byte Unknown6;

                [Flags]
                public enum FlagsValue : ushort
                {
                    None,
                    MirrorHorizontally = 1 << 0,
                    MirrorVertically = 1 << 1,
                    StretchEdges = 1 << 2,
                    EnableTextureCam = 1 << 3,
                    Looping = 1 << 4,
                    Bit5 = 1 << 5,
                    Player1Emblem = 1 << 6,
                    Player2Emblem = 1 << 7,
                    Player3Emblem = 1 << 8,
                    Player4Emblem = 1 << 9,
                    Bit10 = 1 << 10,
                    Bit11 = 1 << 11,
                    Bit12 = 1 << 12,
                    Bit13 = 1 << 13,
                    InputControlsConsumable = 1 << 14,
                    InputControlsWeapon = 1 << 15
                }
            }

            [TagStructure(Size = 0x44, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x48, MinVersion = CacheVersion.Halo3ODST)]
            public class TextWidget : TagStructure
			{
                [TagField(Label = true)]
                public StringId Name;
                public SpecialHudTypeValue SpecialHudType;
                public byte Unknown1;
                public byte Unknown2;
                public List<StateDatum> StateData;
                public List<PlacementDatum> PlacementData;
                public List<AnimationDatum> AnimationData;
                public List<RenderDatum> RenderData;
                public int WidgetIndex;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public FlagsValue Flags;
				[TagField(MaxVersion = CacheVersion.Halo3Retail)]
				public FlagsValue_H3 Flags_H3;
                public FontValue Font;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public short Unknown4;
                public StringId String;

                [Flags]
				public enum FlagsValue_H3 : ushort
                {
                    None,
                    StringIsANumber = 1 << 0,
                    Force2Digit = 1 << 1,
                    Force3Digit = 1 << 2,
                    PlusSuffix = 1 << 3,
                    MSuffix = 1 << 4,
					HundredthsDecimal = 1 << 5,
					ThousandthsDecimal = 1 << 6,
					HundredThousandthsDecimal = 1 << 7,
					OnlyANumber = 1 << 8,
					XSuffix = 1 << 9,
					InBrackets = 1 << 10,
					TimeFormat_S_MS = 1 << 11,
					TimeFormat_H_M_S = 1 << 12,
					MoneyFormat = 1 << 13,
					MinusPrefix = 1 << 14,
					Bit15 = 1 << 15
				}

                [Flags]
                public enum FlagsValue : uint
                {
                    None,
                    StringIsANumber = 1 << 0,
                    Force2Digit = 1 << 1,
                    Force3Digit = 1 << 2,
                    PlusSuffix = 1 << 3,
                    MSuffix = 1 << 4,
                    TenthsDecimal = 1 << 5,
                    HundredthsDecimal = 1 << 6,
                    ThousandthsDecimal = 1 << 7,
                    HundredThousandthsDecimal = 1 << 8,
                    OnlyANumber = 1 << 9,
                    XSuffix = 1 << 10,
                    InBrackets = 1 << 11,
                    TimeFormat_S_MS = 1 << 12,
                    TimeFormat_H_M_S = 1 << 13,
                    MoneyFormat = 1 << 14,
                    MinusPrefix = 1 << 15,
                    Centered = 1 << 16,
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
                }

                public enum FontValue : short
                {
                    Conduit18,
                    Agency16,
                    Fixedsys9,
                    Conduit14,
                    Conduit32,
                    Agency32,
                    Conduit23,
                    Agency18,
                    Conduit18_2,
                    Conduit16,
                    Agency23
                }
            }
        }
    }
}
