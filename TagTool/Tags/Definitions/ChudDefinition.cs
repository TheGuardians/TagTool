using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "chud_definition", Tag = "chdt", Size = 0x18)]
    public class ChudDefinition
    {
        public List<HudWidget> HudWidgets;
        public int LowClipCutoff;
        public int LowAmmoCutoff;
        public int AgeCutoff;

        [TagStructure(Size = 0x50, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x60, MinVersion = CacheVersion.HaloOnline106708)]
        public class HudWidget
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
            public class StateDatum
            {
                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public EngineFlagsValue_H3 EngineFlags_H3;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public EngineFlagsValue_ODST EngineFlags_ODST;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public EngineFlagsValue_HO EngineFlags_HO;
                public BipedFlagsValue BipedFlags;
                [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail)]
                public GamemodeFlagsValue GamemodeFlags_H3;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public GamemodeFlagsValue GamemodeFlags_HO;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public UnknownFlagsValue Unknown1;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public UnknownFlagsValue Unknown2;
                public ResolutionFlagsValue ResolutionFlags;
                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public ScoreboardFlagsValue ScoreboardFlags;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public ScoreboardFlagsValue_HO ScoreboardFlags_HO;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public ScoreboardFlagsBValue ScoreboardFlagsB;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public UntitledFlags1Value UntitledFlags1;
                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public UntitledFlags1Value_H3 UntitledFlags1_H3;
                public EditorFlagsValue EditorFlags;
                public MotionTrackerAndMetagameFlagsValue MotionTrackerAndMetageameFlags;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public SkullFlagsValue SkullFlags;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public SurvivalWaveFlagsValue SurvivalWaveFlags;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public SurvivalWaveFlagsBValue SurvivalWaveFlagsB;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public UnknownFlagsValue Unknown6;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public UnknownFlagsValue Unknown7;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public UnknownFlagsValue Unknown8;
                [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST)]
                public UntitledFlags2Value UntitledFlags2;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public UntitledFlags2Value_HO UntitledFlags2_HO;
                public ScopeFlagsValue ScopeFlags;
                public WeaponStateFlagsValue WeaponStateFlags;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public UnknownFlagsValue Unknown9;
                public UntitledFlags3Value UntitledFlags3;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public UnknownFlagsValue Unknown10;
                public UntitledFlags4Value UntitledFlags4;
                public WeaponStateFlagsValue WeaponStateFlags2;
                public AutoaimFlagsValue AutoaimFlags;
				public MissileLockFlagsValue MissileLockFlags;
				[TagField(MinVersion = CacheVersion.Halo3ODST)]
				public UntitledFlags5Value UntitledFlags5;
				[TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail)]
				public UntitledFlags5Value_H3 UntitledFlags5_H3;
				public AmmoFlagsValue AmmoFlags;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public UntitledFlags6Value UntitledFlags6;
                public ScopeFlagsValue ScopeFlags2;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public UnknownFlagsValue Unknown12;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public ConsumableFlagsAValue ConsumableFlagsA;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public ConsumableFlagsBValue ConsumableFlagsB;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public ConsumableFlagsCValue ConsumableFlagsC;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public UnknownFlagsValue Unknown13;
                [TagField(MinVersion = CacheVersion.HaloOnline700123)]
                public UnknownFlagsValue Unknown14;
                [TagField(MinVersion = CacheVersion.HaloOnline700123)]
                public UnknownFlagsValue Unknown15;

                [Flags]
                public enum EngineFlagsValue_HO : ushort
                {
                    None,
                    Bit0 = 1 << 0,
                    Bit1 = 1 << 1,
                    Survival = 1 << 2,
                    Bit3 = 1 << 3,
                    Theater = 1 << 4,
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
                    Bit15 = 1 << 15
                }

                [Flags]
                public enum EngineFlagsValue_ODST : ushort
                {
                    None,
                    Bit0 = 1 << 0,
                    Bit1 = 1 << 1,
                    Survival = 1 << 2,
                    Editor = 1 << 3,
                    Theater = 1 << 4,
                    CTF = 1 << 5,
                    Slayer = 1 << 6,
                    Oddball = 1 << 7,
                    KOTH = 1 << 8,
                    Juggernaut = 1 << 9,
                    Territories = 1 << 10,
                    Assault = 1 << 11,
                    VIP = 1 << 12,
                    Infection = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15
                }

                [Flags]
                public enum EngineFlagsValue_H3 : ushort
                {
                    None,
                    Bit0 = 1 << 0,
					Bit1 = 1 << 1,
					Bit2 = 1 << 2,
					Bit3 = 1 << 3,
					CTF = 1 << 4,
					Slayer = 1 << 5,
					Oddball = 1 << 6,
					KOTH = 1 << 7,
					Juggernaut = 1 << 8,
					Territories = 1 << 9,
					Assault = 1 << 10,
					VIP = 1 << 11,
					Infection = 1 << 12,
					Bit13 = 1 << 13,
					Editor = 1 << 14,
					Theater = 1 << 15
				}

				[Flags]
                public enum BipedFlagsValue : ushort
                {
                    None,
                    Biped1 = 1 << 0,
                    Biped2 = 1 << 1,
                    Biped3 = 1 << 2,
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
                public enum GamemodeFlagsValue : ushort
                {
                    None,
                    Offense = 1 << 0,
                    Defense = 1 << 1,
                    FreeForAll = 1 << 2,
                    Bit3 = 1 << 3,
                    Bit4 = 1 << 4,
                    Bit5 = 1 << 5,
                    TalkingDisabled = 1 << 6,
                    TapToTalk = 1 << 7,
                    TalkingEnabled = 1 << 8,
                    NotTalking = 1 << 9,
                    Talking = 1 << 10,
                    Bit11 = 1 << 11,
                    Bit12 = 1 << 12,
                    Bit13 = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15
                }

                [Flags]
                public enum ResolutionFlagsValue : ushort
                {
                    None,
                    Bit0 = 1 << 0,
                    Widescreen = 1 << 1,
                    Bit2 = 1 << 2,
                    Bit3 = 1 << 3,
                    Bit4 = 1 << 4,
                    Fullscreen = 1 << 5,
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
                public enum ScoreboardFlagsValue_HO : ushort
                {
                    None,
                    HasFriends = 1 << 0,
                    HasEnemies = 1 << 1,
                    HasVariantName = 1 << 2,
                    SomeoneIsTalking = 1 << 3,
                    IsArming = 1 << 4,
                    TimeEnabled = 1 << 5,
                    Bit6 = 1 << 6,
                    Bit7 = 1 << 7,
                    FriendlyAction = 1 << 8,
                    EnemyAction = 1 << 9,
                    XIsDown = 1 << 10,
                    AttackerBombDropped = 1 << 11,
                    AttackedBombPickedUp = 1 << 12,
                    DefenderTeamIsDead = 1 << 13,
                    AttackerTeamIsDead = 1 << 14,
                    SummaryEnabled = 1 << 15
                }

                [Flags]
                public enum ScoreboardFlagsValue : ushort
                {
                    None,
                    HasFriends = 1 << 0,
                    HasEnemies = 1 << 1,
                    HasVariantName = 1 << 2,
                    SomeoneIsTalking = 1 << 3,
                    IsArming = 1 << 4,
                    TimeEnabled = 1 << 5,
                    Bit6 = 1 << 6,
                    Bit7 = 1 << 7,
                    FriendlyAction = 1 << 8,
                    EnemyAction = 1 << 9,
                    XIsDown = 1 << 10,
                    SummaryEnabled = 1 << 11,
                    NetDebug = 1 << 12,
                    Bit13 = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15
                }

                //Although these flags don't seem related to the scoreboard,
                //They were in the scoreboard flags in ODST.
                [Flags]
                public enum ScoreboardFlagsBValue : ushort
                {
                    None,
                    Bit0 = 1 << 0,
                    NetDebug = 1 << 1,
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
                public enum UntitledFlags1Value_H3 : ushort
                {
                    None,
                    TextureCamEnabled = 1 << 0,
                    Autoaim = 1 << 1,
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
                public enum UntitledFlags1Value : uint
                {
                    None,
                    TextureCamEnabled = 1 << 0,
                    Autoaim = 1 << 1,
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
                }

                [Flags]
                public enum UnknownFlagsValue : ushort
                {
                    None,
                    Bit0 = 1 << 0,
                    Bit1 = 1 << 1,
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
                public enum EditorFlagsValue : ushort
                {
                    None,
                    EditorInactive = 1 << 0,
                    EditorAcitve = 1 << 1,
                    EditorHolding = 1 << 2,
                    EditorNotAllowed = 1 << 3,
                    IsEditorBiped = 1 << 4,
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
                public enum MotionTrackerAndMetagameFlagsValue : ushort
                {
                    None,
                    MotionTracker10M = 1 << 0,
                    MotionTracker25M = 1 << 1,
                    MotionTracker75M = 1 << 2,
                    MotionTracker150M = 1 << 3,
                    Bit4 = 1 << 4,
                    MetagamePlayer2Exists = 1 << 5,
                    Bit6 = 1 << 6,
                    MetagamePlayer3Exists = 1 << 7,
                    Bit8 = 1 << 8,
                    MetagamePlayer4Exists = 1 << 9,
                    Bit10 = 1 << 10,
                    MetagameScoreAdded = 1 << 11,
                    Bit12 = 1 << 12,
                    MetagameScoreRemoved = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15
                }

                [Flags]
                public enum SkullFlagsValue : ushort
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
                public enum SurvivalWaveFlagsValue : ushort
                {
                    None,
                    Wave1Background = 1 << 0,
                    Wave2Background = 1 << 1,
                    Wave3Background = 1 << 2,
                    Wave4Background = 1 << 3,
                    Bit4 = 1 << 4,
                    Bit5 = 1 << 5,
                    BonusRound = 1 << 6,
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
                public enum SurvivalWaveFlagsBValue : ushort
                {
                    None,
                    Wave1 = 1 << 0,
                    Wave2 = 1 << 1,
                    Wave3 = 1 << 2,
                    Wave4 = 1 << 3,
                    Wave5 = 1 << 4,
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
                public enum UntitledFlags2Value_HO : ushort
                {
                    None,
                    PickupGrenades = 1 << 0,
                    Bit1 = 1 << 1,
                    Bit2 = 1 << 2,
                    Bit3 = 1 << 3,
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
				public enum UntitledFlags2Value : ushort
                {
                    None,
                    PickupGrenades = 1 << 0,
                    Bit1 = 1 << 1,
                    Bit2 = 1 << 2,
                    Bit3 = 1 << 3,
                    Bit4 = 1 << 4,
					Bit5 = 1 << 5,
					Bit6 = 1 << 6,
					Bit7 = 1 << 7,
					Bit8 = 1 << 8,
					LivesAdded = 1 << 9,
					Bit10 = 1 << 10,
					Bit11 = 1 << 11,
					Bit12 = 1 << 12,
					Bit13 = 1 << 13,
					Bit14 = 1 << 14,
					Bit15 = 1 << 15
				}

				[Flags]
                public enum ScopeFlagsValue : ushort
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
                public enum WeaponStateFlagsValue : ushort
                {
                    None,
                    PrimaryWeapon = 1 << 0,
                    SecondaryWeapon = 1 << 1,
                    Backpack = 1 << 2,
                    Bit3 = 1 << 3,
                    WeaponCanBePickedUp = 1 << 4,
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
                public enum UntitledFlags3Value : ushort
                {
                    None,
                    MotionTrackerEnabled = 1 << 0,
                    Bit1 = 1 << 1,
                    SelectedFragGrenades = 1 << 2,
                    SelectedPlasmaGrenades = 1 << 3,
                    SelectedSpikeGrenades = 1 << 4,
                    SelectedFireGrenades = 1 << 5,
                    Bit6 = 1 << 6,
                    VisionWarning = 1 << 7,
                    Bit8 = 1 << 8,
                    Bit9 = 1 << 9,
                    Bit10 = 1 << 10,
                    Bit11 = 1 << 11,
                    HasOvershieldLevel1 = 1 << 12,
                    HasOvershieldLevel2 = 1 << 13,
                    HasOvershieldLevel3 = 1 << 14,
                    HasShields = 1 << 15
                }

				[Flags]
				public enum UntitledFlags4Value : ushort
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
				public enum AutoaimFlagsValue : ushort
				{
					None,
					NotAutoaim = 1 << 0,
					AutoaimFriendly = 1 << 1,
					AutoaimEnemy = 1 << 2,
					AutoaimHeadshot = 1 << 3,
					Bit4 = 1 << 4,
					Bit5 = 1 << 5,
					Bit6 = 1 << 6,
					PlasmaLockedOn = 1 << 7,
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
				public enum MissileLockFlagsValue : ushort
				{
					None,
					Bit0 = 1 << 0,
					MissileLocked = 1 << 1,
					MissileLocking = 1 << 2,
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
				public enum UntitledFlags5Value : ushort
				{
					None,
					VitalityMeterMinorDamage = 1 << 0,
					VitalityMeterMediumDamage = 1 << 1,
					VitalityMeterHeavyDamage = 1 << 2,
					Bit3 = 1 << 3,
					Bit4 = 1 << 4,
					Bit5 = 1 << 5,
					HasFragGrenades = 1 << 6,
					HasPlasmaGrenades = 1 << 7,
					HasSpikeGrenades = 1 << 8,
					HasFireGrenades = 1 << 9,
					Bit10 = 1 << 10,
					Bit11 = 1 << 11,
					Bit12 = 1 << 12,
					Bit13 = 1 << 13,
					Bit14 = 1 << 14,
					Bit15 = 1 << 15
				}

				[Flags]
				public enum UntitledFlags5Value_H3 : ushort
				{
					None,
					Bit0 = 1 << 0,
					Bit1 = 1 << 1,
					HasFragGrenades = 1 << 2,
					HasPlasmaGrenades = 1 << 3,
					HasSpikeGrenades = 1 << 4,
					HasFireGrenades = 1 << 5,
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
				public enum AmmoFlagsValue : ushort
				{
					None,
					ClipWarning = 1 << 0,
					AmmoWarning = 1 << 1,
					Bit2 = 1 << 2,
					Bit3 = 1 << 3,
					LowBattery1 = 1 << 4,
					LowBattery2 = 1 << 5,
					Overheated = 1 << 6,
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
				public enum UntitledFlags6Value : ushort
				{
					None,
					IsZoomed = 1 << 0,
					Bit1 = 1 << 1,
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
				public enum ConsumableFlagsAValue : ushort
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
					Consumable4DisabledA = 1 << 15
				}

                [Flags]
                public enum ConsumableFlagsBValue : ushort
				{
					None,
					Consumable1DisabledB = 1 << 0,
					Consumable2DisabledB = 1 << 1,
					Consumable3DisabledB = 1 << 2,
					Consumable4DisabledB = 1 << 3,
					Consumable1Active = 1 << 4,
					Consumable2Active = 1 << 5,
					Consumable3Active = 1 << 6,
					Consumable4Active = 1 << 7,
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
                public enum ConsumableFlagsCValue : ushort
				{
					None,
					EnergyMeter1Full = 1 << 0,
					EnergyMeter2Full = 1 << 1,
					EnergyMeter3Full = 1 << 2,
					EnergyMeter4Full = 1 << 3,
					EnergyMeter5Full = 1 << 4,
					Bit5 = 1 << 5,
					Bit6 = 1 << 6,
					Bit7 = 1 << 7,
					Bit8 = 1 << 8,
					Bit9 = 1 << 9,
					Bit10 = 1 << 10,
					Bit11 = 1 << 11,
					FFA = 1 << 12,
					Teams = 1 << 13,
					Bit14 = 1 << 14,
					Bit15 = 1 << 15
				}
			}

            [TagStructure(Size = 0x1C)]
            public class PlacementDatum
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
            public class AnimationDatum
            {
                //These fields have been identified and should be updated.
                //In halo 3 and odst, they're enum16's. They probably should be here.
                public AnimationFlags1Values Animation1Flags;
                public AnimationFunctionValues Animation1Function;
                public CachedTagInstance Animation1;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float Animation1Unknown;

                public AnimationFlags1Values Animation2Flags;
                public AnimationFunctionValues Animation2Function;
                public CachedTagInstance Animation2;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float Animation2Unknown;

                public AnimationFlags1Values Animation3Flags;
                public AnimationFunctionValues Animation3Function;
                public CachedTagInstance Animation3;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float Animation3Unknown;

                public AnimationFlags1Values Animation4Flags;
                public AnimationFunctionValues Animation4Function;
                public CachedTagInstance Animation4;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float Animation4Unknown;

                public AnimationFlags1Values Animation5Flags;
                public AnimationFunctionValues Animation5Function;
                public CachedTagInstance Animation5;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float Animation5Unknown;

                public AnimationFlags1Values Animation6Flags;
                public AnimationFunctionValues Animation6Function;
                public CachedTagInstance Animation6;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float Animation6Unknown;

                [Flags]
                public enum AnimationFlags1Values : short
                {
                    None,
                    ReverseFrames = 1 << 0,
                }

                public enum AnimationFunctionValues : short
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
            public class RenderDatum
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
                //these are ARGB colors but I set them to Int32 because they need endian-conversion
                public Int32 LocalColorA;
                public Int32 LocalColorB;
                public Int32 LocalColorC;
                public Int32 LocalColorD;
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
                    UnknownX3b,
                    UnknownX3c,
                    SurvivalCurrentLives,
                    SurvivalBonusTime,
                    SurvivalBonusScore,
                    SurvivalMultiplier,
                    UnknownX41,
                    UnknownX42,
                    UnknownX43,
                    UnknownX44,
                    UnknownX45,
                    UnknownX46,
                    UnknownX47,
                    UnknownX48,
                    UnknownX49,
                    UnknownX4a,
                    UnknownX4b,
                    UnknownX4c,
                    UnknownX4d,
                    Consumable1Icon,
                    Consumable2Icon,
                    UnknownX50,
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
                    Unknown4,
                    Unknown5,
                    SurvivalCurrentLives,
                    SurvivalBonusTime,
                    SurvivalBonusScore,
                    SurvivalMultiplier,
                    Achievement1Current,
                    Achievement2Current,
                    Achievement3Current,
                    Achievement4Current,
                    AchievementCurrent,
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
            public class BitmapWidget
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
            public class TextWidget
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