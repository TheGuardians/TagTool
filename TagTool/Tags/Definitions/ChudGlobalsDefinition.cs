using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "chud_globals_definition", Tag = "chgd", Size = 0xF0, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "chud_globals_definition", Tag = "chgd", Size = 0x208, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "chud_globals_definition", Tag = "chgd", Size = 0x2C0, MinVersion = CacheVersion.HaloOnline106708)]
    public class ChudGlobalsDefinition : TagStructure
    {
        public List<HudGlobal> HudGlobals;
        public List<HudShader> HudShaders;
        public List<UnknownBlock> UnknownBlock40;
        public List<CortanaSuckBlock> CortanaSuck;
        public List<PlayerTrainingDatum> PlayerTrainingData;
        public CachedTagInstance StartMenuEmblems;
        public CachedTagInstance CampaignMedals;
        public CachedTagInstance CampaignMedalHudAnimation;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public ChudDefinition.HudWidget.PlacementDatum.AnchorValue CampaignMedalChudAnchor;
        [TagField(MinVersion = CacheVersion.Halo3ODST, Length = 2, Flags = Padding)]
        public byte[] PostAnchorPadding;
        public float CampaignMedalScale;
        public float CampaignMedalSpacing;
        public float CampaignMedalOffsetX;
        public float CampaignMedalOffsetY;
        public float MetagameScoreboardTopY;
        public float MetagameScoreboardSpacing;
        public CachedTagInstance UnitDamageGrid;
        public float MicroTextureTileAmount;
        public float MediumSensorBlipScale;
        public float SmallSensorBlipScale;
        public float LargeSensorBlipScale;
        public float SensorBlipGlowAmount;
        public float SensorBlipGlowRadius;
        public float SensorBlipGlowOpacity;
        public CachedTagInstance MotionSensorBlip;
        public CachedTagInstance BirthdayPartyEffect;
        public CachedTagInstance CampaignFloodMask;
        public CachedTagInstance CampaignFloodMaskTile;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float MotionSensorBlipHeightModifier;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float ShieldMinorThreshold;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float ShieldMajorThreshold;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float ShieldCriticalThreshold;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float HealthMinorThreshold;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float HealthMajorThreshold;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float HealthCriticalThreshold;

        //suspect that these are realArgbcolors
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown12 = 1.0f;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown13 = 1.0f;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown13_2 = 1.0f;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown14;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown15;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown16;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown17;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown18;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown19;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown20;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown21;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown22;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown23;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown24;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown25;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown26;

        //DAMAGE MASK FUNCTIONS AND VARIABLES
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public TagFunction DamageMaskFunction1 = new TagFunction
        {
            Data = new byte[]
            {
                0x08, 0x3C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x6F, 0x12, 0x83, 0x3A,
                0x6F, 0x12, 0x03, 0x3B, 0x28, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                0x0A, 0x00, 0x00, 0xCD, 0xFF, 0xFF, 0x7F, 0x7F, 0x77, 0xBE, 0xFF, 0xBF,
                0xD9, 0xCE, 0x3F, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x4B, 0xDD, 0x97, 0x40
            }
        };

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public RealRgbColor DamageMaskColorOverlay;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float DamageMaskColorOverlayAlpha;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float DamageMaskOpacityRed;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float DamageMaskOpacityGreen;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float DamageMaskOpacityBlue;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float DamageMaskOpacityAlpha;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public RealRgbColor DamageMaskColorFloor;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float DamageMaskColorFloorAlpha;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public RealRgbColor DamageMaskBitmapTint;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float DamageMaskBitmapTintAlpha;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public TagFunction DamageMaskFunction2 = new TagFunction
        {
            Data = new byte[]
            {
                0x08, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x28, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                0x0A, 0x00, 0x00, 0xCD, 0xFF, 0xFF, 0x7F, 0x7F, 0x00, 0x00, 0x00, 0xC0,
                0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x4B, 0xDD, 0x97, 0x40
            }
        };

        //ODST VALUES FOR PDA/BEACON
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public float PDABeaconTextOuterFadeAngle;
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public float PDABeaconTextInnerFadeAngle;
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public float PDABeaconRadiusmin;
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public float PDABeaconRadiusmax;
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public float PDAUserBeaconRadius;
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public float PDAUserBeaconAngle;
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public TagFunction PDABeaconFunction1;
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public TagFunction PDABeaconFunction2;

        //HO VALUES
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float SprintFOVMultiplier = 1.0f;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float SprintFOVTransitionInTime = 0.5f;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float SprintFOXTransitionOutTime = 1.0f;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public CachedTagInstance ParallaxData = null;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown49;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public TagFunction Unknown60 = new TagFunction
        {
            Data = new byte[]
            {
                0x01, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            }
        };

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public TagFunction Unknown61 = new TagFunction
        {
            Data = new byte[]
            {
                0x01, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            }
        };

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public TagFunction Unknown62 = new TagFunction
        {
            Data = new byte[]
            {
                0x01, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            }
        };

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public TagFunction Unknown63 = new TagFunction
        {
            Data = new byte[]
            {
                0x01, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            }
        };

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public TagFunction Unknown64 = new TagFunction
        {
            Data = new byte[]
            {
                0x01, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            }
        };

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown65 = 1.33f;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public TagFunction Unknown66 = new TagFunction
        {
            Data = new byte[]
            {
                0x01, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            }
        };

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public TagFunction Unknown67 = new TagFunction
        {
            Data = new byte[]
            {
                0x03, 0x34, 0x00, 0x00, 0x9A, 0x99, 0x99, 0xBD, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0xCD, 0xCC, 0xCC, 0xBD,
                0xCD, 0xCC, 0x8C, 0x3F
            }
        };

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public TagFunction Unknown68 = new TagFunction
        {
            Data = new byte[]
            {
                0x03, 0x34, 0x00, 0x00, 0xBC, 0x74, 0x93, 0xBB, 0xBC, 0x74, 0x93, 0x3B,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x80, 0x3F
            }
        };

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public TagFunction Unknown69 = new TagFunction
        {
            Data = new byte[]
            {
                0x01, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            }
        };

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public TagFunction Unknown70 = new TagFunction
        {
            Data = new byte[]
            {
                0x03, 0x34, 0x00, 0x00, 0x6F, 0x12, 0x83, 0xBB, 0x6F, 0x12, 0x83, 0x3B,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x3F, 0xCD, 0xCC, 0xCC, 0xBD,
                0xCD, 0xCC, 0x8C, 0x3F
            }
        };

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public CachedTagInstance SurvivalHUDUnknown = null;  //chdt tagreference which activates in firefight

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown72 = 3.0f;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown73;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown74;

        [TagStructure(Size = 0x208, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x23C, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x2B0, MinVersion = CacheVersion.HaloOnline106708)]
        public class HudGlobal : TagStructure
        {
            [TagField(Flags = Label)]
            public BipedValue Biped;

            public ArgbColor HUDDisabled;
            public ArgbColor HUDPrimary;
            public ArgbColor HUDForeground;
            public ArgbColor HUDWarning;
            public ArgbColor NeutralReticule;
            public ArgbColor HostileReticule;
            public ArgbColor FriendlyReticule;
            public ArgbColor GlobalDynamic7_UnknownBlip;
            public ArgbColor NeutralBlip;
            public ArgbColor HostileBlip;
            public ArgbColor FriendlyPlayerBlip;
            public ArgbColor FriendlyAIBlip;
            public ArgbColor GlobalDynamic12;
            public ArgbColor WaypointBlip;
            public ArgbColor DistantWaypointBlip;
            public ArgbColor FriendlyWaypoint;
            public ArgbColor GlobalDynamic16;
            public ArgbColor GlobalDynamic17;
            public ArgbColor GlobalDynamic18;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor BlueWaypoint_HO;

            public ArgbColor GlobalDynamic19;
            public ArgbColor GlobalDynamic20;
            public ArgbColor GlobalDynamic21;
            public ArgbColor GlobalDynamic22;
            public ArgbColor GlobalDynamic23;
            public ArgbColor GlobalDynamic24;
            public ArgbColor GlobalDynamic25;
            public ArgbColor GlobalDynamic26;
            public ArgbColor GlobalDynamic27;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor GlobalDynamic29_HO; //White

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor DefaultItemOutline;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor MAGItemOutline;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor DMGItemOutline;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor ACCItemOutline;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor ROFItemOutline;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor RNGItemOutline;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor PWRItemOutline;

            public List<HudAttribute> HudAttributes;
            public List<HudSound> HudSounds;
            public CachedTagInstance Unknown;
            public CachedTagInstance FragGrenadeSwapSound;
            public CachedTagInstance PlasmaGrenadeSwapSound;
            public CachedTagInstance SpikeGrenadeSwapSound;
            public CachedTagInstance FirebombGrenadeSwapSound;
            public CachedTagInstance DamageMicrotexture;
            public CachedTagInstance DamageNoise;
            public CachedTagInstance DirectionalArrow;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public CachedTagInstance GrenadeWaypoint = null;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public CachedTagInstance PinkGradient = null;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public float DirectionalArrowDisplayTime;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public float DirectionalCutoffAngle;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public float DirectionalArrowScale;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public float Unknown10;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public float Unknown11;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public float Unknown12;

            public CachedTagInstance ObjectiveWaypoints;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public CachedTagInstance PlayerWaypoints = null;
            public CachedTagInstance ScoreboardHud;
            public CachedTagInstance MetagameScoreboardHud;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public CachedTagInstance SurvivalHud = null;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public CachedTagInstance MetagameScoreboardHud2 = null;
            public CachedTagInstance TheaterHud;
            public CachedTagInstance ForgeHud;
            public CachedTagInstance HudStrings;
            public CachedTagInstance Medals;
            public List<MultiplayerMedal> MultiplayerMedals;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public CachedTagInstance MedalHudAnimation2 = null;
            public CachedTagInstance MedalHudAnimation;
            public CachedTagInstance CortanaChannel;
            public CachedTagInstance Unknown20;
            public CachedTagInstance Unknown21;
            public CachedTagInstance JammerResponse;
            public CachedTagInstance JammerShieldHit;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public RealPoint2d GrenadeSchematicsOffset;
            public float GrenadeScematicsSpacing;
            public float EquipmentScematicOffsetY;
            public float DualEquipmentScematicOffsetY;
            public float UnknownScematicOffsetY; //equipment related
            public float UnknownScematicOffsetY_2; //equipment related
            public float ScoreboardLeaderOffsetY;
            public float WaypointScaleMin;
            public float WaypointScaleMax;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public float Unknown29;

            public enum BipedValue : int
            {
                Spartan,
                Elite,
                Monitor
            }

            [TagStructure(Size = 0x60, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x130, MaxVersion = CacheVersion.Halo3ODST)]
            [TagStructure(Size = 0xE8, MinVersion = CacheVersion.HaloOnline106708)]
            public class HudAttribute : TagStructure
            {
                public ResolutionFlagValue ResolutionFlags;

                public Angle WarpAngle;
                public float WarpAmount;
                public float WarpDirection;

                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float Unknown4;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float Unknown5;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float Unknown6;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float Unknown7;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float Unknown8;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float Unknown9;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float Unknown10;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float Unknown11;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float Unknown12;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float Unknown13;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float Unknown14;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float Unknown15;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float Unknown16;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float Unknown17;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float Unknown18;

                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float HorizontalRoll;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float VeticalBow;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float InverseHorizontalRoll;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float InverseVerticalBow;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float HorizontalRoll2;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float VericalBow2;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float HorizontalTwist;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float VerticalTwist;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float HorizontalTwist2;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float VerticalTwist2;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float VerticalScale2;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float VerticalTwist3;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float HorizontalSkew;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float VerticalFlip;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float InverseHorizontalSkew;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float VerticalFlip2;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public RealPoint2d HUDOffset;

                public uint ResolutionWidth;
                public uint ResolutionHeight;
                public RealPoint2d MotionSensorOffset;
                public float MotionSensorRadius;
                public float MotionSensorScale;
                public float HorizontalScale;
                public float VerticalScale;
                public float HorizontalStretch;
                public float VerticalStretch;

                //these four tagrefs have no function in HO
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public CachedTagInstance FirstPersonUnknownBitmap = null;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public CachedTagInstance ThirdPersonUnknownBitmap = null;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public CachedTagInstance FirstPersonDamageBorder = null;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public CachedTagInstance ThirdPersonDamageBorder = null;

                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float StateScale_HO;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public RealPoint2d StateLeftRightOffset_HO;

                //these only exist in HO
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float ScaleUnknown1; //related to state scale (alternate version?)
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float SpacingUnknown1;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float ScaleUnknown2;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float SpacingUnknown2;

                //From here, fields have been moved around a bit.
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float NotificationOffsetX_H3;
                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public float NotificationOffsetY_H3;
                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public float StateLeftRightOffsetY_H3;

                public float StateCenterOffsetY;
                public float StateCenterOffsetY_2;
                public float MedalScale;
                public float MedalSpacing;

                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public float StateScale;

                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public RealPoint2d SurvivalMedalsOffset; //referenced by chud anchors -- must be neither campaign nor multiplayer
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float Unknown32;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public RealPoint2d MultiplayerMedalsOffset; //referenced by chud anchors

                public float NotificationScale;
                public float NotificationLineSpacing;
                public int NotificationLineCountModifier; //controls max number of notification lines onscreen

                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float NotificationOffsetX_HO;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float NotificationOffsetY_HO;

                //This group of 5 floats is all part of the same system
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float ScaleUnknown;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float SpacingUnknown;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float NullUnknown;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public RealPoint2d UnknownOffset3; //referenced by chud anchors

                //this is present in HO, it is still used in chud anchors
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public float PromptOffsetY;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public float PromptOffsetX;

                [Flags]
                public enum ResolutionFlagValue : int
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
                    Bit8 = 1 << 8,
                    Bit9 = 1 << 9,
                    Bit10 = 1 << 10,
                    Bit11 = 1 << 11,
                    Bit12 = 1 << 12,
                    Bit13 = 1 << 13,
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
                    Bit31 = 1 << 31
                }
            }

            [TagStructure(Size = 0x28, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x14, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline449175)]
            [TagStructure(Size = 0x18, MinVersion = CacheVersion.HaloOnline498295)]
            public class HudSound : TagStructure
            {
                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public CachedTagInstance SpartanSound;

                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public LatchedToValues_H3 LatchedTo_H3;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public LatchedToValues LatchedTo;

                [TagField(MinVersion = CacheVersion.HaloOnline498295)]
                public uint LatchedTo2;

                public float Scale;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public List<BipedData> Bipeds;

                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public CachedTagInstance EliteSound;

                [Flags]
                public enum LatchedToValues : int
                {
                    None,
                    HealthRecharging = 1 << 0,
                    HealthMinorDamage = 1 << 1,
                    HealthMajorDamage = 1 << 2,
                    HealthCriticalDamage = 1 << 3,
                    HealthMinorState = 1 << 4,
                    HealthLow = 1 << 5,
                    HealthEmpty = 1 << 6,
                    ShieldRecharging = 1 << 7,
                    ShieldMinorDamage = 1 << 8,
                    ShieldMajorDamage = 1 << 9,
                    ShieldCriticalDamage = 1 << 10,
                    ShieldMinorState = 1 << 11,
                    ShieldLow = 1 << 12,
                    ShieldEmpty = 1 << 13,
                    RocketLocking = 1 << 14,
                    RocketLocked = 1 << 15,
                    MissileLocking = 1 << 16,
                    MissileLocked = 1 << 17,
                    Bit18 = 1 << 18,
                    Bit19 = 1 << 19,
                    Bit20 = 1 << 20,
                    Bit21 = 1 << 21,
                    StaminaFull = 1 << 22,
                    StaminaWarning = 1 << 23,
                    StaminaRecharge = 1 << 24,
                    Bit25 = 1 << 25,
                    Bit26 = 1 << 26,
                    Bit27 = 1 << 27,
                    TacticalPackageError = 1 << 28,
                    TacticalPackageUsed = 1 << 29,
                    GainMedal = 1 << 30,
                    WinningPoints = 1 << 31
                }

                [Flags]
                public enum LatchedToValues_H3 : int
                {
                    None,
                    ShieldRecharging = 1 << 0,
                    ShieldMinorDamage = 1 << 1,
                    ShieldLow = 1 << 2,
                    ShieldEmpty = 1 << 3,
                    HealthLow = 1 << 4,
                    HealthEmpty = 1 << 5,
                    HealthMinorDamage = 1 << 6,
                    HealthMajorDamage = 1 << 7,
                    RocketLocking = 1 << 8,
                    RocketLocked = 1 << 9,
                    MissileLocking = 1 << 10,
                    MissileLocked = 1 << 11,
                    Bit12 = 1 << 12,
                    Bit13 = 1 << 13,
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
                    Bit31 = 1 << 31
                }

                [TagStructure(Size = 0x14, MinVersion = CacheVersion.Halo3ODST)]
                public class BipedData : TagStructure
                {
                    [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                    public BipedTypeValue_ODST BipedType_ODST;

                    [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                    public BipedTypeValue_HO BipedType_HO;

                    [TagField(Flags = Padding, Length = 3, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                    public byte[] Unused = new byte[3];

                    public CachedTagInstance Sound;

                    public enum BipedTypeValue_ODST : sbyte
                    {
                        Any = 0,
                        Rookie = 1,
                        Buck = 2,
                        Dare = 3,
                        Dutch = 4,
                        Johnson = 5,
                        Mickey = 6,
                        Romeo = 7
                    }

                    public enum BipedTypeValue_HO : int
                    {
                        Spartan = 0,
                        Elite = 1,
                        Monitor = 2
                    }
                }
            }

            [TagStructure(Size = 0x4)]
            public class MultiplayerMedal : TagStructure
            {
                [TagField(Flags = Label)]
                public StringId Medal;
            }
        }

        [TagStructure(Size = 0x20)]
        public class HudShader : TagStructure
        {
            public CachedTagInstance VertexShader;
            public CachedTagInstance PixelShader;
        }

        [TagStructure(Size = 0x40)]
        public class UnknownBlock : TagStructure
        {
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;
            public uint Unknown7;
            public uint Unknown8;
            public uint Unknown9;
            public uint Unknown10;
            public uint Unknown11;
            public uint Unknown12;
            public uint Unknown13;
            public uint Unknown14;
            public uint Unknown15;
            public uint Unknown16;
        }

        [TagStructure(Size = 0x10)]
        public class CortanaSuckBlock : TagStructure
        {
            public uint Unknown;
            public List<LevelsBlock> Levels;

            [TagStructure(Size = 0xE4)]
            public class LevelsBlock : TagStructure
            {
                //Section A
                public float NoiseaVelocitymin;
                public float NoiseaVelocitymax;
                public float NoiseaScalexmin;
                public float NoiseaScalexmax;
                public float NoiseaScaleymin;
                public float NoiseaScaleymax;
                public float NoisebVelocitymin;
                public float NoisebVelocitymax;
                public float NoisebScalexmin;
                public float NoisebScalexmax;
                public float NoisebScaleymin;
                public float NoisebScaleymax;
                public float NoisePixelThresholdmin;
                public float NoisePixelThresholdmax;
                public float NoisePixelPersistencemin;
                public float NoisePixelPersistencemax;
                public float NoisePixelVelocitymin;
                public float NoisePixelVelocitymax;
                public float NoisePixelTurbulencemin;
                public float NoisePixelTurbulencemax;
                public float NoiseTranslationScalexmin;
                public float NoiseTranslationScalexmax;
                public float NoiseTranslationScaleymin;
                public float NoiseTranslationScaleymax;
                public CachedTagInstance Message;
                //Section B
                public float NoiseaVelocitymin_B;
                public float NoiseaVelocitymax_B;
                public float NoiseaScalexmin_B;
                public float NoiseaScalexmax_B;
                public float NoiseaScaleymin_B;
                public float NoiseaScaleymax_B;
                public float NoisebVelocitymin_B;
                public float NoisebVelocitymax_B;
                public float NoisebScalexmin_B;
                public float NoisebScalexmax_B;
                public float NoisebScaleymin_B;
                public float NoisebScaleymax_B;
                public float NoisePixelThresholdmin_B;
                public float NoisePixelThresholdmax_B;
                public float NoisePixelPersistencemin_B;
                public float NoisePixelPersistencemax_B;
                public float NoisePixelVelocitymin_B;
                public float NoisePixelVelocitymax_B;
                public float NoisePixelTurbulencemin_B;
                public float NoisePixelTurbulencemax_B;
                public float NoiseTranslationScalexmin_B;
                public float NoiseTranslationScalexmax_B;
                public float NoiseTranslationScaleymin_B;
                public float NoiseTranslationScaleymax_B;
                public CachedTagInstance Message_B;
            }
        }

        [TagStructure(Size = 0x14)]
        public class PlayerTrainingDatum : TagStructure
        {
            [TagField(Flags = Label)]
            public StringId DisplayString;
            public short MaxDisplayTime;
            public short DisplayCount;
            public short DisappearDelay;
            public short RedisplayDelay;
            public float DisplayDelay;
            public ushort Flags;
            public short Unknown;
        }
    }
}
