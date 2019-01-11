using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "chud_globals_definition", Tag = "chgd", Size = 0xF0, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "chud_globals_definition", Tag = "chgd", Size = 0x208, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "chud_globals_definition", Tag = "chgd", Size = 0x2C0, MinVersion = CacheVersion.HaloOnline106708)]
    public class ChudGlobalsDefinition : TagStructure
	{
		public List<HudGlobal> HudGlobals;
		public List<HudShader> HudShaders;
		public List<UnusedCortanaEffectBlock> UnusedCortanaEffect;
		public List<UnknownBlock2> Unknown2;
		public List<PlayerTrainingDatum> PlayerTrainingData;
		public CachedTagInstance StartMenuEmblems;
		public CachedTagInstance CampaignMedals;
		public CachedTagInstance CampaignMedalHudAnimation;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public short Unknown3;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public short Unknown4;
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
		public float Unknown5;
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

        //POSSIBLY SHADER RELATED
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public float Unknown12;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public float Unknown13;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public float Unknown13_2;
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
		public TagFunction DamageMaskFunction1;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public float Unknown28;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public float Unknown29;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public float Unknown30;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public float Unknown31;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public float Unknown32;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public float Unknown33;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public float Unknown34;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public float Unknown35;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public float Unknown36;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public float Unknown37;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public float Unknown38;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public float Unknown39;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public float Unknown40;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public float Unknown41;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public float Unknown42;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public float Unknown43;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public TagFunction DamageMaskFunction2;

        //ODST VALUES FOR PDA/BEACON
		[TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
		public float Unknown50;
		[TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
		public float Unknown51;
		[TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
		public float Unknown52;
		[TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
		public float Unknown53;
		[TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
		public float Unknown54;
		[TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
		public float Unknown55;
		[TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
		public TagFunction PDABeaconFunction1;
		[TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
		public TagFunction PDABeaconFunction2;

        //HO VALUES
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float SprintFOVMultiplier;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float SprintFOVTransitionInTime;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float SprintFOXTransitionOutTime;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public CachedTagInstance ParallaxData = null;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown49;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public TagFunction Unknown60;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
		public TagFunction Unknown61;
		[TagField(MinVersion = CacheVersion.HaloOnline106708)]
		public TagFunction Unknown62;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public TagFunction Unknown63;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public TagFunction Unknown64;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown65;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public TagFunction Unknown66;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
		public TagFunction Unknown67;
		[TagField(MinVersion = CacheVersion.HaloOnline106708)]
		public TagFunction Unknown68;
		[TagField(MinVersion = CacheVersion.HaloOnline106708)]
		public TagFunction Unknown69;
		[TagField(MinVersion = CacheVersion.HaloOnline106708)]
		public TagFunction Unknown70;

        //NOT SURE WHAT THIS IS
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public CachedTagInstance Unknown71 = null;  //chdt tagreference which activates in firefight

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
            [TagField(Label = true)]
            public BipedValue Biped;

            public ArgbColorFixed HUDDisabled;
            public ArgbColorFixed HUDPrimary;
            public ArgbColorFixed HUDForeground;
            public ArgbColorFixed HUDWarning;
            public ArgbColorFixed NeutralReticule;
            public ArgbColorFixed HostileReticule;
            public ArgbColorFixed FriendlyReticule;
            public ArgbColorFixed GlobalDynamic7_UnknownBlip;
            public ArgbColorFixed NeutralBlip;
            public ArgbColorFixed HostileBlip;
            public ArgbColorFixed FriendlyPlayerBlip;
            public ArgbColorFixed FriendlyAIBlip;
            public ArgbColorFixed GlobalDynamic12;
            public ArgbColorFixed WaypointBlip;
            public ArgbColorFixed DistantWaypointBlip;
            public ArgbColorFixed FriendlyWaypoint;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColorFixed GlobalDynamic16;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColorFixed GlobalDynamic17;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColorFixed GlobalDynamic18;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColorFixed GlobalDynamic19;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColorFixed GlobalDynamic20;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColorFixed GlobalDynamic21;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColorFixed TextFadeIn;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColorFixed GlobalDynamic23;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColorFixed GlobalDynamic24;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColorFixed GlobalDynamic25;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColorFixed GlobalDynamic26;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColorFixed GlobalDynamic27;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColorFixed NeutralWaypoint_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColorFixed HostileWaypoint_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColorFixed DeadWaypoint_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColorFixed BlueWaypoint_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColorFixed TextFadeIn_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColorFixed GlobalDynamic21_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColorFixed GlobalDynamic22_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColorFixed GlobalDynamic23_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColorFixed GlobalDynamic24_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColorFixed GlobalDynamic25_UnknownWaypoint_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColorFixed ShootingWaypoint_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColorFixed TakingDamageWaypoint_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColorFixed SpeakingWaypoint_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
			public ArgbColorFixed GlobalDynamic29_HO; //White
			[TagField(MinVersion = CacheVersion.HaloOnline106708)]
			public ArgbColorFixed DefaultItemOutline;
			[TagField(MinVersion = CacheVersion.HaloOnline106708)]
			public ArgbColorFixed MAGItemOutline;
			[TagField(MinVersion = CacheVersion.HaloOnline106708)]
			public ArgbColorFixed DMGItemOutline;
			[TagField(MinVersion = CacheVersion.HaloOnline106708)]
			public ArgbColorFixed ACCItemOutline;
			[TagField(MinVersion = CacheVersion.HaloOnline106708)]
			public ArgbColorFixed ROFItemOutline;
			[TagField(MinVersion = CacheVersion.HaloOnline106708)]
			public ArgbColorFixed RNGItemOutline;
			[TagField(MinVersion = CacheVersion.HaloOnline106708)]
			public ArgbColorFixed PWRItemOutline;
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
			public float Unknown7;
			[TagField(MinVersion = CacheVersion.Halo3ODST)]
			public float Unknown8;
			[TagField(MinVersion = CacheVersion.Halo3ODST)]
			public float Unknown9;
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
			public uint Unknown24;
			[TagField(MinVersion = CacheVersion.Halo3ODST)]
			public uint Unknown25;
			public float GrenadeScematicsSpacing;
			public float EquipmentScematicOffsetY;
			public float DualEquipmentScematicOffsetY;
			public float Unknown26;
			public float Unknown27;
			public float ScoreboardLeaderOffsetY;
			public float Unknown28;
			public float WaypointScale;
			[TagField(MinVersion = CacheVersion.HaloOnline106708)]
			public float Unknown29;

			public enum BipedValue : int
			{
				Spartan,
				Elite,
				Monitor
            }

            [TagStructure(Size = 0x4)]
            public class ArgbColorFixed
            {
                public byte Alpha;
                public byte Red;
                public byte Green;
                public byte Blue;
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
				[TagField(MinVersion = CacheVersion.Halo3ODST)]
				public CachedTagInstance Unknown = null;
				[TagField(MinVersion = CacheVersion.Halo3ODST)]
				public CachedTagInstance Unknown19 = null;
				[TagField(MinVersion = CacheVersion.Halo3ODST)]
				public CachedTagInstance FirstPersonDamageBorder = null;
				[TagField(MinVersion = CacheVersion.Halo3ODST)]
				public CachedTagInstance ThirdPersonDamageBorder = null;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float PickupDialogScale;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public RealPoint2d PickupDialogOffset;

                //these only exist in HO
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float Unknown23;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float Unknown24;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float Unknown25;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float Unknown26;

				//From here, fields have been moved around a bit.
				[TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
				public float NotificationOffsetX_H3;
				[TagField(MaxVersion = CacheVersion.Halo3ODST)]
				public float NotificationOffsetY_H3;
				[TagField(MaxVersion = CacheVersion.Halo3ODST)]
				public float StateLeftRightOffsetY_H3;

				public float StateCenterOffsetY;
				public float Unknown28;
				public float Unknown29;
				public float StateScale;

                //HO ONLY
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float Unknown31;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float Unknown32;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float Unknown33;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float Unknown34;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float Unknown35;

                public float NotificationScale;
				public float NotificationLineSpacing;
                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public RealPoint2d PromptOffset;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float Unknown35_2;

                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float NotificationOffsetX_HO;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float NotificationOffsetY_HO;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float Unknown36;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float Unknown37;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float Unknown38;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public float Unknown39;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public float Unknown40;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float Unknown41;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float Unknown42;

                [Flags]
                public enum ResolutionFlagValue : int
                {
                    None,
                    WideFull = 1 << 0,
                    WideHalf = 1 << 1,
                    Bit2 = 1 << 2,
                    StandardFull = 1 << 3,
                    WideQuarter = 1 << 4,
                    StandardHalf = 1 << 5,
                    Bite6 = 1 << 6,
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

                    [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST, Padding = true, Length = 3)]
                    public byte[] unused;

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
                [TagField(Label = true)]
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
		public class UnusedCortanaEffectBlock : TagStructure
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
		public class UnknownBlock2 : TagStructure
		{
			public uint Unknown;
			public List<UnknownBlock> Unknown2;

			[TagStructure(Size = 0xE4)]
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
				public uint Unknown17;
				public uint Unknown18;
				public uint Unknown19;
				public uint Unknown20;
				public uint Unknown21;
				public uint Unknown22;
				public uint Unknown23;
				public uint Unknown24;
				public uint Unknown25;
				public CachedTagInstance Sound;
				public uint Unknown26;
				public uint Unknown27;
				public uint Unknown28;
				public uint Unknown29;
				public uint Unknown30;
				public uint Unknown31;
				public uint Unknown32;
				public uint Unknown33;
				public uint Unknown34;
				public uint Unknown35;
				public uint Unknown36;
				public uint Unknown37;
				public uint Unknown38;
				public uint Unknown39;
				public uint Unknown40;
				public uint Unknown41;
				public uint Unknown42;
				public uint Unknown43;
				public uint Unknown44;
				public uint Unknown45;
				public uint Unknown46;
				public uint Unknown47;
				public uint Unknown48;
				public uint Unknown49;
				public CachedTagInstance Sound2;
			}
		}

		[TagStructure(Size = 0x14)]
		public class PlayerTrainingDatum : TagStructure
		{
            [TagField(Label = true)]
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