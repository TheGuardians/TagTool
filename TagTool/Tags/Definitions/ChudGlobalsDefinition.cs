using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "chud_globals_definition", Tag = "chgd", Size = 0xF0, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "chud_globals_definition", Tag = "chgd", Size = 0x2C0, MinVersion = CacheVersion.Halo3ODST)]
    public class ChudGlobalsDefinition
	{
		public List<HudGlobal> HudGlobals;
		public List<HudShader> HudShaders;
		public List<UnknownBlock> Unknown;
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
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public float Unknown12;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public float Unknown13;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public float Unknownundefined;
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
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public byte[] Unknown27;
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
		public byte[] Unknown44;

		[TagField(MinVersion = CacheVersion.HaloOnline106708)]
		public float SprintFOVMultiplier;
		[TagField(MinVersion = CacheVersion.HaloOnline106708)]
		public float SprintFOVTransitionInTime;
		[TagField(MinVersion = CacheVersion.HaloOnline106708)]
		public float SprintFOXTransitionOutTime;
		[TagField(MinVersion = CacheVersion.HaloOnline106708)]
		public CachedTagInstance ParallaxData;
		[TagField(MinVersion = CacheVersion.HaloOnline106708)]
		public float Unknown49;

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

		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public byte[] Unknown56;
		[TagField(MinVersion = CacheVersion.Halo3ODST)]
		public byte[] Unknown57;

		[TagField(MaxVersion = CacheVersion.Halo3ODST)]
		public CachedTagInstance Unknown58;
		[TagField(MaxVersion = CacheVersion.Halo3ODST)]
		public float Unknown59;

		[TagField(MinVersion = CacheVersion.HaloOnline106708)]
		public byte[] Unknown60;
		[TagField(MinVersion = CacheVersion.HaloOnline106708)]
		public byte[] Unknown61;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unknown62;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown63;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unknown64;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
		public byte[] Unknown65;
		[TagField(MinVersion = CacheVersion.HaloOnline106708)]
		public byte[] Unknown66;
		[TagField(MinVersion = CacheVersion.HaloOnline106708)]
		public byte[] Unknown67;
		[TagField(MinVersion = CacheVersion.HaloOnline106708)]
		public byte[] Unknown68;
		[TagField(MinVersion = CacheVersion.HaloOnline106708)]
		public CachedTagInstance Unknown69;
		[TagField(MinVersion = CacheVersion.HaloOnline106708)]
		public float Unknown70;
		[TagField(MinVersion = CacheVersion.HaloOnline106708)]
		public float Unknown71;
		[TagField(MinVersion = CacheVersion.HaloOnline106708)]
		public float Unknown72;

		[TagStructure(Size = 0x208, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x23C, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x2B0, MinVersion = CacheVersion.HaloOnline106708)]
		public class HudGlobal
        {
            [TagField(Label = true)]
            public BipedValue Biped;

            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor HUDDisabled;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor HUDPrimary;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor HUDForeground;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor HUDWarning;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor NeutralReticule;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor HostileReticule;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor FriendlyReticule;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor GlobalDyanamic7_UnknownBlip;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor NeutralBlip;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor HostileBlip;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor FriendlyPlayerBlip;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor FriendlyAIBlip;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor GlobalDynamic12;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor WaypointBlip;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor DistantWaypointBlip;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor FriendlyWaypoint;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor NeutralWaypoint;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor HostileWaypoint;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor DeadWaypoint;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor GlobalDynamic19;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor GlobalDynamic20;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor GlobalDynamic21;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor TextFadeIn;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor GlobalDynamic23;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor GlobalDynamic24;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor GlobalDynamic25;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor GlobalDynamic26;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public ArgbColor GlobalDynamic27;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor HUDDisabled_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor HUDPrimary_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor HUDForeground_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor HUDWarning_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor NeutralReticule_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor HostileReticule_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor FriendlyReticule_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor GlobalDynamic7_UnknownBlip_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor NeutralBlip_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor HostileBlip_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor FriendlyPlayerBlip_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor FriendlyAIBlip_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor GlobalDynamic12_UnknownBlip_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor WaypointBlip_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor DistantWaypointBlip_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor FriendlyWaypoint_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor NeutralWaypoint_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor HostileWaypoint_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor DeadWaypoint_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor BlueWaypoint_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor TextFadeIn_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor GlobalDynamic21_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor GlobalDynamic22_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor GlobalDynamic23_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor GlobalDynamic24_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor GlobalDynamic25_UnknownWaypoint_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor ShootingWaypoint_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor TakingDamageWaypoint_HO;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ArgbColor SpeakingWaypoint_HO;
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

			[TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
			public float Unknown2;
			[TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
			public int Unknown3;
			[TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
			public int Unknown4;

			[TagField(MinVersion = CacheVersion.HaloOnline106708)]
			public CachedTagInstance Unknown5;
			[TagField(MinVersion = CacheVersion.HaloOnline106708)]
			public CachedTagInstance Unknown6;
			[TagField(MinVersion = CacheVersion.HaloOnline106708)]
			public int Unknown7;
			[TagField(MinVersion = CacheVersion.HaloOnline106708)]
			public int Unknown8;
			[TagField(MinVersion = CacheVersion.HaloOnline106708)]
			public int Unknown9;
			[TagField(MinVersion = CacheVersion.HaloOnline106708)]
			public int Unknown10;
			[TagField(MinVersion = CacheVersion.HaloOnline106708)]
			public int Unknown11;
			[TagField(MinVersion = CacheVersion.HaloOnline106708)]
			public int Unknown12;

			public CachedTagInstance Waypoints;
			[TagField(MinVersion = CacheVersion.HaloOnline106708)]
			public CachedTagInstance Unknown19;
			public CachedTagInstance ScoreboardHud;
			public CachedTagInstance MetagameScoreboardHud;
			[TagField(MinVersion = CacheVersion.Halo3ODST)]
			public CachedTagInstance SurvivalHud;
			[TagField(MinVersion = CacheVersion.Halo3ODST)]
			public CachedTagInstance MetagameScoreboardHud2;
			public CachedTagInstance TheaterHud;
			public CachedTagInstance ForgeHud;
			public CachedTagInstance HudStrings;
			public CachedTagInstance Medals;
			public List<MultiplayerMedal> MultiplayerMedals;
			[TagField(MinVersion = CacheVersion.HaloOnline106708)]
			public CachedTagInstance MedalHudAnimation2;
			public CachedTagInstance MedalHudAnimation;
			public CachedTagInstance CortanaChannel;
			public CachedTagInstance Unknown20;
			public CachedTagInstance Unknown21;
			public CachedTagInstance Unknown22;
			public CachedTagInstance Unknown23;
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

			[TagStructure(Size = 0x60, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x130, MaxVersion = CacheVersion.Halo3ODST)]
            [TagStructure(Size = 0xE8, MinVersion = CacheVersion.HaloOnline106708)]
			public class HudAttribute
			{
				public ResolutionFlagValue ResolutionFlags;

				[TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
				public float Unknown1;
				[TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
				public float Unknown2;
				[TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
				public float Unknown3;
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

				[TagField(MaxVersion = CacheVersion.Halo3Retail)]
				public Angle WarpAngle_H3;
				[TagField(MaxVersion = CacheVersion.Halo3Retail)]
				public float WarpAmount_H3;
				[TagField(MaxVersion = CacheVersion.Halo3Retail)]
				public float WarpDirection_H3;

				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public Angle WarpAngle_HO;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float WarpAmount_HO;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float WarpDirection_HO;

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
				public CachedTagInstance Unknown;
				[TagField(MinVersion = CacheVersion.Halo3ODST)]
				public CachedTagInstance Unknown19;
				[TagField(MinVersion = CacheVersion.Halo3ODST)]
				public CachedTagInstance FirstPersonDamageBorder;
				[TagField(MinVersion = CacheVersion.Halo3ODST)]
				public CachedTagInstance ThirdPersonDamageBorder;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float Unknown20;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float Unknown21;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float Unknown22;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float Unknown23;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float Unknown24;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float Unknown25;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float Unknown26;
				//From here, fields have been moved around a bit.
				//I'm not too familiar with tag definition conventions, so I'm following the lens tag, which has the same problem.
				//In other words, this could probably be cleaner! - Alex-231 17/7/17
				[TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
				public float NotificationOffsetX;
				[TagField(MaxVersion = CacheVersion.Halo3Retail)]
				public float NotificationOffsetY_H3;
				[TagField(MaxVersion = CacheVersion.Halo3Retail)]
				public float StateLeftRightOffsetY_H3;

				[TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
				public float Unknown27;

				public float StateCenterOffsetY;
				[TagField(MinVersion = CacheVersion.Halo3Retail)]
				public float Unknown28;
				public float Unknown29;
				public float Unknown30;

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

                [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST)]
                public float StateScale; //This may be an unknown in H:O. Perhaps unknown20.

                public float NotificationScale;
				public float NotificationLineSpacing;

				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float Unknown36;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float NotificationOffsetX_HO;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float NotificationOffsetY_HO;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float Unknown38;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float Unknown39;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
				public float Unknown40;
				[TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float Unknown41;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public float Unknown42;

                [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail)]
                public short Unknown43;
                [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail)]
                public short Unknown44;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public RealPoint2d PromptOffset; //Might not actually be prompt offset in Halo Online, not sure.

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
                    StandardQuater = 1 << 7,
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
            public class HudSound
            {
                [TagField(Label = true, MaxVersion = CacheVersion.Halo3Retail)]
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
                    HealthMinor = 1 << 1,
                    HealthMajor = 1 << 2,
                    HealthCritical = 1 << 3,
                    HealthLowMinor = 1 << 4,
                    HealthLowMajor = 1 << 5,
                    HealthLowCritical = 1 << 6,
                    ShieldRecharging = 1 << 7,
                    ShieldMinor = 1 << 8,
                    ShieldMajor = 1 << 9,
                    ShieldCritical = 1 << 10,
                    ShieldMinorState = 1 << 11,
                    ShieldMajorState = 1 << 12,
                    ShieldCriticalState = 1 << 13,
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
                    ShieldDamaged = 1 << 1,
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

                [TagStructure(Size = 0x14)]
                public class BipedData
                {
                    [TagField(Label = true, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                    public BipedTypeValue_ODST BipedType_ODST;

                    [TagField(Label = true, MinVersion = CacheVersion.HaloOnline106708)]
					public BipedTypeValue_HO BipedType_HO;

                    [TagField(MaxVersion = CacheVersion.Halo3ODST, Padding = true, Length = 3)]
                    public byte[] Unused;

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
			public class MultiplayerMedal
            {
                [TagField(Label = true)]
                public StringId Medal;
			}
		}

		[TagStructure(Size = 0x20)]
		public class HudShader
		{
            public CachedTagInstance VertexShader;
            public CachedTagInstance PixelShader;
        }

        [TagStructure(Size = 0x40)]
		public class UnknownBlock
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
		public class UnknownBlock2
		{
			public uint Unknown;
			public List<UnknownBlock> Unknown2;

			[TagStructure(Size = 0xE4)]
			public class UnknownBlock
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
		public class PlayerTrainingDatum
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