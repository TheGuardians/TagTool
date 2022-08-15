using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "chud_globals_definition", Tag = "chgd", Size = 0xF0, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "chud_globals_definition", Tag = "chgd", Size = 0x208, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "chud_globals_definition", Tag = "chgd", Size = 0x2C0, MinVersion = CacheVersion.HaloOnlineED)]
    public class ChudGlobalsDefinition : TagStructure
    {
        public List<HudGlobal> HudGlobals;
        public List<HudShader> HudShaders;
        public List<ChudSuckProfile> SuckProfiles;
        public List<CortanaEffectConfig> CortanaConfigs;
        public List<PlayerTrainingDatum> PlayerTrainingData;
        public CampaignMetagameStruct CampaignMetagame;
        public CachedTag DirectDamageMicrotexture;
        public float MicrotextureScale;
        public float MediumSensorBlipScale;
        public float SmallSensorBlipScale;
        public float LargeSensorBlipScale;
        public float MaxAgeSize; // SensorBlipGlowAmount ??
        public float SizePower; // SensorBlipGlowRadius ??
        public float AlphaPower; // SensorBlipGlowOpacity ??
        public CachedTag MotionSensorBlip;
        public CachedTag GruntBirthdayEffect;
        public CachedTag CampaignFloodMask; // TentaclePorn
        public CachedTag CampaignFloodMaskTile; // FloodGoo

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public float MotionSensorLevelHeightRange = float.MaxValue; // if object outside this height range of player, up/down indicator is used respectively
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

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public RealRgbColor HealthMultiplyColor0 = new RealRgbColor(1.0f, 1.0f, 1.0f);
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float HealthMultiplyColor0Alpha;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public RealRgbColor HealthMultiplyColor1 = new RealRgbColor(1.0f, 1.0f, 1.0f);
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float HealthMultiplyColor1Alpha;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public RealRgbColor HealthAdditiveColor0;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float HealthAdditiveColor0Alpha;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public RealRgbColor HealthAdditiveColor1;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float HealthAdditiveColor1Alpha;

        //DAMAGE MASK FUNCTIONS AND VARIABLES
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public TagFunction HealthEffectDamageMaskIntensity = new TagFunction
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
        public RealRgbColor ShieldMultiplyColor0;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float ShieldMultiplyColor0Alpha;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public RealRgbColor ShieldMultiplyColor1;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float ShieldMultiplyColor1Alpha;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public RealRgbColor ShieldAdditiveColor0;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float ShieldAdditiveColor0Alpha;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public RealRgbColor ShieldAdditiveColor1;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float ShieldAdditiveColor1Alpha;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public TagFunction ShieldEffectDamageMaskIntensity = new TagFunction
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
        public float WaypointDistanceMax;
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public TagFunction WaypointDistanceModifier;
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public TagFunction WaypointAngleModifier;

        //HO VALUES
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public float SprintFOVMultiplier = 1.0f;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public float SprintFOVTransitionInTime = 0.5f;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public float SprintFOXTransitionOutTime = 1.0f;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public CachedTag ParallaxData = null;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public float Unknown49;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public TagFunction Unknown60 = new TagFunction
        {
            Data = new byte[]
            {
                0x01, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            }
        };

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public TagFunction Unknown61 = new TagFunction
        {
            Data = new byte[]
            {
                0x01, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            }
        };

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public TagFunction Unknown62 = new TagFunction
        {
            Data = new byte[]
            {
                0x01, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            }
        };

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public TagFunction Unknown63 = new TagFunction
        {
            Data = new byte[]
            {
                0x01, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            }
        };

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public TagFunction Unknown64 = new TagFunction
        {
            Data = new byte[]
            {
                0x01, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            }
        };

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public float Unknown65 = 1.33f;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public TagFunction Unknown66 = new TagFunction
        {
            Data = new byte[]
            {
                0x01, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            }
        };

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
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

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
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

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public TagFunction Unknown69 = new TagFunction
        {
            Data = new byte[]
            {
                0x01, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            }
        };

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
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
        public CachedTag SurvivalModeMultiplayerIntro = null;  //chdt tagreference which activates in firefight

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float AchievementDisplayTime = 3.0f;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public float Unknown73;
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public float Unknown74;

        [TagStructure(Size = 0x208, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x23C, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x2B0, MinVersion = CacheVersion.HaloOnlineED)]
        public class HudGlobal : TagStructure
        {
            [TagField(Flags = Label)]
            public ChudSkinType Type;
            // possible mismatches below
            public ArgbColor PrimaryBackground;         // global 0
            public ArgbColor SecondaryBackground;
            public ArgbColor HighlightForeground;
            public ArgbColor WarningFlash;
            public ArgbColor CrosshairNormal;
            public ArgbColor CrosshairEnemy;            // global 5
            public ArgbColor CrosshairFriendly;
            public ArgbColor BaseBlip;
            public ArgbColor SelfBlip;
            public ArgbColor EnemyBlip;
            public ArgbColor NeutralBlip;               // global 10
            public ArgbColor FriendlyBlip;
            public ArgbColor BlipPing;
            public ArgbColor ObjectiveBlipOnRadar;
            public ArgbColor ObjectiveBlipOffRadar;
            public ArgbColor NavpointFriendly;          // global 15
            public ArgbColor NavpointNeutral;
            public ArgbColor NavpointEnemy;
            public ArgbColor NavpointAllyDead;

            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public ArgbColor NavpointAllyBlue;

            public ArgbColor MessageFlashSelf;          // global 20
            public ArgbColor MessageFlashFriendly;
            public ArgbColor MessageFlashEnemy;
            public ArgbColor MessageFlashNeutral;
            public ArgbColor InvincibleShield;
            public ArgbColor NavpointAllyStandingBy;    // global 25
            public ArgbColor NavpointAllyFiring;
            public ArgbColor NavpointAllyTakingDamage;
            public ArgbColor NavpointAllySpeaking;

            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public ArgbColor GlobalDynamic29_HO;        // White

            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public ArgbColor DefaultItemOutline;        // global 30
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public ArgbColor MAGItemOutline;
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public ArgbColor DMGItemOutline;
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public ArgbColor ACCItemOutline;
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public ArgbColor ROFItemOutline;
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public ArgbColor RNGItemOutline;
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public ArgbColor PWRItemOutline;

            public List<HudAttribute> HudAttributes;
            public List<HudSound> HudSounds;
            public CachedTag BannedVehicleEntranceSound;
            public CachedTag FragGrenadeSwapSound;
            public CachedTag PlasmaGrenadeSwapSound;
            public CachedTag SpikeGrenadeSwapSound;
            public CachedTag FirebombGrenadeSwapSound;
            public CachedTag DamageMicrotexture;
            public CachedTag DamageNoise;
            public CachedTag DamageDirectionalArrow;

            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public CachedTag GrenadeWaypoint = null;
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public CachedTag PinkGradient = null;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public float DirectionalArrowDisplayTime;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public float DirectionalCutoffAngle;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public float DirectionalArrowScale;
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public float Unknown10;
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public float Unknown11;
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public float Unknown12;

            public CachedTag Waypoints;

            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public CachedTag PlayerWaypoints = null;

            public CachedTag ScoreboardHud;
            public CachedTag MetagameScoreboardHud;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public CachedTag SurvivalHud = null;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public CachedTag MetagameScoreboardHud2 = null;

            public CachedTag TheaterHud;
            public CachedTag ForgeHud;
            public CachedTag HudStrings;
            public CachedTag Medals;
            public List<MultiplayerMedal> MultiplayerMedals;

            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public CachedTag MedalHudAnimation2 = null;

            public CachedTag MedalAnimation;
            public CachedTag CortanaChannel; // TestBitmap0
            public CachedTag TestBitmap1;
            public CachedTag TestBitmap2;
            public CachedTag JammerDamage;
            public CachedTag JammerDamageSound;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public RealPoint2d GrenadeSchematicsOffset;

            public float GrenadeAnchorOffset; // GrenadeScematicsSpacing
            public float EquipmentVerticalOffset;
            public float EquipmentVerticalOffsetDual;
            public float EquipmentVerticalOffsetNone;
            public float EquipmentHorizontalSize;
            public float ScoreboardSpacingSize;
            public float WaypointMinDistanceScale;
            public float WaypointMaxDistanceScale;

            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public float Unknown29;

            public enum ChudSkinType : int
            {
                Default,
                Dervish,
                Monitor,
                MpFfa,
                MpRedTeam,
                MpBlueTeam
            }

            [TagStructure(Size = 0x60, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x130, MaxVersion = CacheVersion.Halo3ODST)]
            [TagStructure(Size = 0xE8, MinVersion = CacheVersion.HaloOnlineED)]
            public class HudAttribute : TagStructure
            {
                public ResolutionFlagValue ResolutionFlags;

                //Not present in ODST, ugly version hacks
                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public Angle WarpSourceFovY; // WarpAngle
                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public float WarpAmount; // WarpAmount
                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public float WarpDestinationZOffset; // the Z-axis is along the camera line of sight.

                //ODST only warp fields
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public RealPoint2d CurvePointTopLeft;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public RealPoint2d CurvePointCenterLeft;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public RealPoint2d CurvePointBottomLeft;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public RealPoint2d CurvePointTopMiddle;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public RealPoint2d CurvePointCenterMiddle;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public RealPoint2d CurvePointBottomMiddle;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public RealPoint2d CurvePointTopRight;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public RealPoint2d CurvePointCenterRight;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public RealPoint2d CurvePointBottomRight;

                //undocumented fields
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

                public uint VirtualWidth; // resolution
                public uint VirtualHeight; // resolution
                public RealPoint2d MotionSensorOrigin;
                public float MotionSensorRadius;
                public float BlipRadius; // MotionSensorScale ?
                public float GlobalSafeFrameHorizontal; // HorizontalScale
                public float GlobalSafeFrameVertical; // VerticalScale
                public float SafeFrameHorizontalDing; // HorizontalStretch
                public float SafeFrameVerticalDing; // VerticalStretch

                //these four tagrefs have no function in HO
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public CachedTag DamageBorderHealthEffect = null;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public CachedTag ThirdPersonDamageBorderHealthEffect = null;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public CachedTag DamageBorderShieldEffect = null;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public CachedTag ThirdPersonDamageBorderShieldEffect = null;

                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public float StateMessageScale;
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public RealPoint2d StateLeftRightOffset_HO;

                //these only exist in HO
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public float ScaleUnknown1; //related to state scale (alternate version?)
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public float SpacingUnknown1;
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public float ScaleUnknown2;
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public float SpacingUnknown2;

                //From here, fields have been moved around a bit.
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public float NotificationOffsetX_H3;
                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public float NotificationOffsetY_H3;
                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public float StateLeftRightOffsetY_H3;

                public float MessageAnchorVerticalOffset;
                public float StateMessageVerticalOffset;
                public float MedalScale;
                public float MedalWidth; // medal spacing

                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public float StateMessageScaleH3;

                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public RealPoint2d SurvivalMedalsOffset; //referenced by chud anchors -- must be neither campaign nor multiplayer
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public float Unknown32;
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public RealPoint2d MultiplayerMedalsOffset; //referenced by chud anchors

                public float MessageScale;
                public float MessageHeight; // line spacing
                public int MessageCountDelta; //controls max number of notification lines onscreen

                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public RealPoint2d MessageOffset;

                //This group of 5 floats is all part of the same system
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public float ScaleUnknown;
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public float SpacingUnknown;
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public float NullUnknown;
                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
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
                    StandardQuarter = 1 << 7
                }
            }

            [TagStructure(Size = 0x28, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x14, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline449175)]
            [TagStructure(Size = 0x18, MinVersion = CacheVersion.HaloOnline498295)]
            public class HudSound : TagStructure
            {
                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public CachedTag SpartanSound;

                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public ChudSoundCueFlags_H3 LatchedTo_H3;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public ChudSoundCueFlags LatchedTo;

                [TagField(MinVersion = CacheVersion.HaloOnline498295)]
                public uint LatchedTo2;

                public float Scale;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public List<BipedData> Bipeds;

                [TagField(MaxVersion = CacheVersion.Halo3Retail)]
                public CachedTag EliteSound;

                [Flags]
                public enum ChudSoundCueFlags : int
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
                    TrackedTarget = 1 << 16,
                    LockedTarget = 1 << 17,
                    Vip = 1 << 18,
                    Juggernaut = 1 << 19,
                    Zombie = 1 << 20,
                    LastManStanding = 1 << 21,
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
                public enum ChudSoundCueFlags_H3 : int
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
                    TrackedTarget = 1 << 10,
                    LockedTarget = 1 << 11,
                    Vip = 1 << 12,
                    Juggernaut = 1 << 13,
                    Zombie = 1 << 14,
                    LastManStanding = 1 << 15
                }

                [TagStructure(Size = 0x14, MinVersion = CacheVersion.Halo3ODST)]
                public class BipedData : TagStructure
                {
                    [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                    public BipedTypeValue_ODST BipedType_ODST;

                    [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                    public BipedTypeValue_HO BipedType_HO;

                    [TagField(Flags = Padding, Length = 3, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                    public byte[] Unused = new byte[3];

                    public CachedTag Sound;

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
                public StringId MedalName;
            }
        }

        [TagStructure(Size = 0x20)]
        public class HudShader : TagStructure
        {
            public CachedTag VertexShader;
            public CachedTag PixelShader;
        }

        [TagStructure(Size = 0x40)]
        public class ChudSuckProfile : TagStructure
        {
            public float EffectRadius;
            public Bounds<float> VertexNoiseBounds;
            public float VertexNoisePower;
            public Bounds<float> PixelNoiseBounds;
            public float PixelNoisePower;
            public Bounds<float> WarpRadiusBounds;
            public float WarpRadiusPower;
            public Bounds<float> WarpIntensityBounds;
            public float WarpIntensityPower;
            public Bounds<float> NoiseSharpnessBounds;
            public float NoiseSharpnessPower;
        }

        [TagStructure(Size = 0x10)]
        public class CortanaEffectConfig : TagStructure
        {
            public StringId Name;
            public List<CortanaEffectDistanceConfig> DistanceConfigs;

            [TagStructure(Size = 0xE4)]
            public class CortanaEffectDistanceConfig : TagStructure
            {
                public float Distance;
                public CortanaEffectHeadingConfigStruct Facing;
                public CortanaEffectHeadingConfigStruct Oblique;

                [TagStructure(Size = 0x70)]
                public class CortanaEffectHeadingConfigStruct : TagStructure
                {
                    public Bounds<float> NoiseAVelocity;
                    public Bounds<float> NoiseAScaleX;
                    public Bounds<float> NoiseAScaleY;
                    public Bounds<float> NoiseBVelocity;
                    public Bounds<float> NoiseBScaleX;
                    public Bounds<float> NoiseBScaleY;
                    public Bounds<float> PixelationThreshold;
                    public Bounds<float> PixelationPersistence;
                    public Bounds<float> PixelationVelocity;
                    public Bounds<float> PixelationTurbulence;
                    public Bounds<float> TranslationScaleX;
                    public Bounds<float> TranslationScaleY;
                    public CachedTag SoundReference;
                }
            }
        }

        [TagStructure(Size = 0x14)]
        public class PlayerTrainingDatum : TagStructure
        {
            [TagField(Flags = Label)]
            public StringId DisplayString; // comes out of the HUD text globals
            public short MaxDisplayTime; // how long the message can be on screen before being hidden
            public short DisplayCount; // how many times a training message will get displayed (0-3 only!)
            public short DisappearDelay; // how long a displayed but untriggered message stays up
            public short RedisplayDelay; // how long after display this message will stay hidden
            public float DisplayDelay; // how long the event can be triggered before it's displayed
            public PlayerTrainingFlags Flags;

            [TagField(Length = 2, Flags = Padding)]
            public byte[] Padding0;

            [Flags]
            public enum PlayerTrainingFlags : ushort
            {
                NotInMultiplayer = 1 << 0
            }
        }

        [TagStructure(Size = 0x48, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x4C, MinVersion = CacheVersion.Halo3ODST)]
        public class CampaignMetagameStruct : TagStructure
        {
            public CachedTag Emblems;
            public CachedTag Medals;
            public CachedTag MedalAnimation;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public ChudDefinition.HudWidget.PlacementDatum.ChudAnchorType MedalChudAnchor;

            [TagField(MinVersion = CacheVersion.Halo3ODST, Length = 2, Flags = Padding)]
            public byte[] PostAnchorPadding;

            public float MedalScale;
            public float MedalSpacing;
            public RealPoint2d MedalOffset;
            public float ScoreboardTopY;
            public float ScoreboardSpacing;
        }
    }
}
