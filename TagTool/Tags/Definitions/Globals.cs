using TagTool.Ai;
using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "globals", Tag = "matg", Size = 0x554, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
    [TagStructure(Name = "globals", Tag = "matg", Size = 0x5F8, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    [TagStructure(Name = "globals", Tag = "matg", Size = 0x5AC, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "globals", Tag = "matg", Size = 0x608, MaxVersion = CacheVersion.HaloOnline449175)]
    [TagStructure(Name = "globals", Tag = "matg", Size = 0x618, MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline604673)]
    [TagStructure(Name = "globals", Tag = "matg", Size = 0x614, MinVersion = CacheVersion.HaloOnline700123, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "globals", Tag = "matg", Size = 0x714, MinVersion = CacheVersion.HaloReach, MaxVersion = CacheVersion.HaloReach11883)]
    [TagStructure(Name = "globals", Tag = "matg", Size = 0x7A8, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.MCC)]
    public class Globals : TagStructure
	{
        [TagField(Flags = Padding, Length = 172)]
        public byte[] Unused;

        public GameLanguage Language;
        public List<HavokCleanupResource> HavokCleanupResources;
        public List<SoundGlobalsDefinition> SoundGlobals;
        public List<AiGlobalsDatum> AiGlobals;

        [TagField(ValidTags = new[] { "aigl" }, MinVersion = CacheVersion.Halo3ODST)]
        public CachedTag AiGlobalsTag;

        public List<DamageTableBlock> DamageTable;

        // ?????? see List<GNullBlock> Empty in h3ek def. size = 0x0
        public uint Unknown45;
        public uint Unknown46;
        public uint Unknown47;
        // ??????

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint Unknown;

        public List<TagReferenceBlock> Sounds;

        public List<CameraGlobalsDefinition> Camera;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<ThumbStickDeadZone> ThumbStickDeadZones;

        public List<PlayerControlBlock> PlayerControl;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<PlayerTraitDefault> PlayerTraitDefaults;

        public List<DifficultyBlock> Difficulty;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<CoopDifficultyBlock> CoopDifficulty;

        public List<Grenade> Grenades;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<SoftBarrierProperty> SoftBarrierProperties;

        // ?????? see List<GNullBlock> What in h3ek def. size = 0x0
        public uint Unknown48;
        public uint Unknown49;
        public uint Unknown50;

        public List<InterfaceTagsBlock> InterfaceTags;

        //should be public List<CheatWeapon> WeaponList;
        public uint Unknown51;
        public uint Unknown52;
        public uint Unknown53;
        public uint Unknown54;

        // ?????? supposed to be CheatPowerups but size doesn't match. should be 16 bytes
        public uint Unknown55;
        public uint Unknown56;

        public List<PlayerInformationBlock> PlayerInformation;

        public List<PlayerRepresentationBlock> PlayerRepresentation;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<PlayerCharacterType> PlayerCharacterTypes;

        public List<FallingDamageBlock> FallDamage;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<ShieldBoostBlock> ShieldBoost;

        public List<Material> Materials;

        [TagField(Gen = CacheGeneration.Third)]
        public List<ColorBlock> PlayerColors;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<ColorBlock> EmblemColors;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<VisorColorBlock> VisorColors;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public RealRgbColor EliteArmorShine;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public RealRgbColor EliteArmorColor;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<ForgeColorBlock> ForgeColors;

        [TagField(ValidTags = new[] { "gegl" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag GameEngineGlobals;

        [TagField(ValidTags = new[] { "mulg" })]
        public CachedTag MultiplayerGlobals;

        [TagField(ValidTags = new[] { "smdt" }, MinVersion = CacheVersion.Halo3ODST)]
        public CachedTag SurvivalGlobals;

        [TagField(MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag ArmorGlobals;

        [TagField(ValidTags = new[] { "motl" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag ObjectTypeList;

        public List<CinematicsGlobals> CinematicGlobals;
        public List<CampaignMetagameGlobal> CampaignMetagameGlobals;

        [TagField(ValidTags = new[] { "gmeg" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag MedalGlobals;

        [TagField(Length = 12, Align = 0x4, Platform = CachePlatform.Original)]
        [TagField(Length = 12, Align = 0x8, Platform = CachePlatform.MCC)]
        public LanguagePack[] LanguagePacks = new LanguagePack[12];

        [TagField(ValidTags = new[] { "rasg" })]
        public CachedTag RasterizerGlobals;

        [TagField(ValidTags = new[] { "cfxs" })]
        public CachedTag DefaultCameraFxSettings;

        [TagField(ValidTags = new[] { "pdm!" }, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag PodiumDefinition;

        [TagField(ValidTags = new[] { "wind" })]
        public CachedTag DefaultWindSettings;

        [TagField(ValidTags = new[] { "wxcg" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag WeatherGlobals;

        [TagField(ValidTags = new[] { "jpt!" })]
        public CachedTag CollisionDamageEffect;

        [TagField(ValidTags = new[] { "cddf" })]
        public CachedTag CollisionDamage;

        public StringId GlobalWaterMaterial;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public StringId GlobalAirMaterial;

        public short GlobalWaterMaterialType;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short GlobalAirMaterialType;

        [TagField(Length = 2, Flags = Padding, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] Padding5;

        [TagField(ValidTags = new[] { "effg" })]
        public CachedTag EffectGlobals;

        [TagField(Platform = CachePlatform.MCC)]
        public CachedTag RenderObjectSkins;

        [TagField(ValidTags = new[] { "hcfd" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag CollisionFilter;

        [TagField(ValidTags = new[] { "grfr" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag GroundedFriction;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<ActiveCamoGlobalsBlock> ActiveCamo;

        [TagField(ValidTags = new[] { "igpd" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag IncidentGlobals;
        [TagField(ValidTags = new[] { "pggd" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag PlayerGradeGlobals;
        [TagField(ValidTags = new[] { "pmcg" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag PlayerCustomizationGlobals;
        [TagField(ValidTags = new[] { "lgtd" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag LoadoutGlobals;
        [TagField(ValidTags = new[] { "chdg" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag ChallengeGlobals;
        [TagField(ValidTags = new[] { "gcrg" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag RewardGlobals;

        [TagField(ValidTags = new[] { "gpdt" }, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag GameProgressionGlobals;

        [TagField(ValidTags = new[] { "achi" }, MinVersion = CacheVersion.Halo3ODST)]
        public CachedTag AchievementGlobals;
        
        [TagField(ValidTags = new[] { "inpg" }, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag InputGlobals;

        [TagField(ValidTags = new[] { "avat" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag AvatarAwardGlobals;
        [TagField(ValidTags = new[] { "gptd" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag PerformanceThrottleGlobals;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<GarbageCollectionBlock> GarbageCollection;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<GlobalCameraImpulseBlock> CameraImpulse;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<Material> AlternateMaterials;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown266;
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown267;
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown268;
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown269;
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public TagFunction Unknown270 = new TagFunction { Data = new byte[0] };
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown271;
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown272;
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown273;
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown274;
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown275;
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<DamageReportingType> DamageReportingTypes;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline571627)]
        public uint Unknown276;
        
		[TagStructure(Size = 0xC)]
		public class DamageTableBlock : TagStructure
		{
			public List<DamageGroup> DamageGroups;

			[TagStructure(Size = 0x10)]
			public class DamageGroup : TagStructure
			{
                [TagField(Flags = Label)]
                public StringId Name;
				public List<ArmorModifier> ArmorModifiers;

				[TagStructure(Size = 0x8)]
				public class ArmorModifier : TagStructure
				{
                    [TagField(Flags = Label)]
                    public StringId Name;
					public float DamageMultiplier;
				}
			}
		}

        [TagStructure(Size = 0x8, MinVersion = CacheVersion.HaloReach)]
        public class ThumbStickDeadZone : TagStructure
        {
            public Bounds<float> Bounds;
        }

        [TagStructure(Size = 0x88, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x70, MaxVersion = CacheVersion.HaloOnline449175)]
        [TagStructure(Size = 0x78, MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x8C, MinVersion = CacheVersion.HaloReach)]
        public class PlayerControlBlock : TagStructure
		{
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<UnknownBlockReach> UnknownBlocks1;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<UnknownBlockReach> UnknownBlocks2;

            public float CrosshairMagnetismFriction; // how much the crosshair slows over enemies
            public float CrosshairMagnetismAdhesion; // how much the crosshair sticks to enemies
            public float InconsequentialTargetScale; // scales magnetism level for inconsequential targets like infection forms

            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float PaddingUnused;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float PaddingUnused1;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float PaddingUnused2;

            [TagField(MaxVersion = CacheVersion.HaloOnline449175)]
            public RealPoint2d CrosshairLocation; // -1..1, 0 is middle of the screen
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public RealPoint2d CrosshairLocation_Reach;
            [TagField(MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
            public List<CrosshairLocationBlock> CrosshairLocations;

            //Running
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float SprintStartDelay; // how long you must be pegged before you start sprinting
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float SprintFullSpeedTime; // how long you must sprint before you reach top speed
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float SprintDecayRate; // how fast being unpegged decays the timer (seconds per second)
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float SprintFullSpeedMultiplier; // how much faster we actually go when at full sprint
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float SprintPeggedMagnitude; // how far the stick needs to be pressed before being considered pegged
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float SprintPeggedAngularThreshold; // how far off straight up (in degrees) we consider pegged

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float StaminaDepleteRestoreTime;       // time to restore stamina from empty or deplete from full (seconds)
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float SprintCooldownTime;       // time between sprint end and next available use (seconds)

            [TagField(MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown6_HO;

            //Looking
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown7_Reach;

            public float LookDefaultPitchRate; // degrees
            public float LookDefaultYawRate; // degrees

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float LookPegThreshold; // magnitude of yaw for pegged acceleration to kick in
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float LookYawAccelerationTime; // time for a pegged look to reach maximum effect (seconds)
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float LookYawAccelerationScale; // maximum effect of a pegged look (scales last value in the look function below)
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float LookPitchAccelerationTime; // time for a pegged look to reach maximum effect (seconds)
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float LookPitchAccelerationScale;// maximum effect of a pegged look (scales last value in the look function below)

            public float LookAutolevelingScale; // 1 is fast, 0 is none, >1 will probably be really fast

            [TagField(MaxVersion = CacheVersion.HaloOnline700123, Length = 8, Flags = Padding)]
            public byte[] Padding2;

            public float GravityScale;

            [TagField(Length = 2, Flags = Padding)]
            public byte[] Padding3;

            public short MinimumAutolevelingTicks; // amount of time player needs to move and not look up or down for autolevelling to kick in

            [TagField(Gen = CacheGeneration.Third)]
            public float MinimumVehicleFlipAngle; // 0 means the vehicle's up vector is along the ground, 90 means the up vector is pointing straight up

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public List<LookFunctionBlock> LookFunction;

            [TagField(Gen = CacheGeneration.Third)]
            public float MinimumActionHoldTime; // time that player needs to press ACTION to register as a HOLD (seconds)
            [TagField(Gen = CacheGeneration.Third)] // seconds
            public float PeggedZoomSupressionThreshold; // for spinny-shotgun goodness

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int Unknown12;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int Unknown13;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int Unknown14;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int Unknown15;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int Unknown16;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int Unknown17;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int Unknown18;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown19;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown20;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown21;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown22;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown23;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown24;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown25;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int Unknown26;

            [TagStructure(Size = 0x28, MinVersion = CacheVersion.HaloReach)]
            public class UnknownBlockReach : TagStructure
            {
                public int Flags;
                public float Unknown1;
                public float Unknown2;
                public float Unknown3;
                public float Unknown4;
                public float Unknown5;
                public Angle Unknown6;
                public List<UnknownFunction> UnknownFunctions;

                [TagStructure(Size = 0x14, MinVersion = CacheVersion.HaloReach)]
                public class UnknownFunction : TagStructure
                {
                    public TagFunction Function = new TagFunction { Data = new byte[0] };
                }
            }

            [TagStructure(Size = 0x24)]
            public class CrosshairLocationBlock : TagStructure
			{
                public float DefaultCrosshairLocationY;
                public uint Unknown;
                public float CenteredCrosshairLocationY;
                public uint Unknown2;
                public uint Unknown3;
                public uint Unknown4;
                public uint Unknown5;
                public uint Unknown6;
                public uint Unknown7;
            }

            [TagStructure(Size = 4)]
            public class LookFunctionBlock : TagStructure
			{
                public float Scale;
            }
        }

        [TagStructure(Size = 0x3C)]
        public class PlayerTraitDefault : TagStructure
        {
            public List<HealthAndShieldsTrait> HealthAndShields;
            public List<WeaponsAndEquipmentTrait> WeaponsAndEquipment;
            public List<MovementTrait> Movement;
            public List<AppearanceTrait> Appearance;
            public List<HudTrait> Hud;

            [TagStructure(Size = 0xC)]
            public class HealthAndShieldsTrait : TagStructure
            {
                public TraitDamageResistancePercentage DamageResistanceModifier;

                public TraitHealthPercentage HealthModifier;
                public TraitHealthRatePercentage HealthRechargeRate;
                public TraitHealthPercentage ShieldModifier;
                public TraitHealthRatePercentage ShieldRechargeRate;
                public TraitHealthRatePercentage ShieldRechargeRate2;

                public TraitBoolean HeadshotImmunity;
                public TraitVampirismPercentage VampirismModifier;
                public TraitBoolean AssassinationImmunity;
                public TraitBoolean UnitCanDie;

                [TagField(Length = 2, Flags = Padding)]
                public byte[] Unused; // need to double check

                public enum TraitDamageResistancePercentage : sbyte
                {
                    Unchanged,
                    Ten,
                    Fifty,
                    Ninety,
                    OneHundred,
                    OneHundredTen,
                    OneHundredFifty,
                    TwoHundred,
                    ThreeHundred,
                    FiveHundred,
                    OneThousand,
                    TwoThousand,
                    Invulnerable
                }
                public enum TraitHealthPercentage : sbyte
                {
                    Unchanged,
                    Zero,
                    OneHundred,
                    OneHundredFifty,
                    TwoHundred,
                    ThreeHundred,
                    FourHundred
                }
                public enum TraitHealthRatePercentage : sbyte
                {
                    Unchanged,
                    NegativeTwentyFive,
                    NegativeTen,
                    NegativeFive,
                    Zero,
                    Ten,
                    TwentyFive,
                    Fifty,
                    SeventyFive,
                    Ninety,
                    OneHundred,
                    OneHundredTen,
                    OneHundredTwentyFive,
                    OneHundredFifty,
                    TwoHundred
                }
                public enum TraitVampirismPercentage : sbyte
                {
                    Unchanged,
                    Zero,
                    Ten,
                    TwentyFive,
                    Fifty,
                    OneHundred
                }
            }

            [TagStructure(Size = 0x18)]
            public class WeaponsAndEquipmentTrait : TagStructure
            {
                public TraitDamagePercentage DamageModifier;
                public TraitDamagePercentage MeleeDamageModifier;

                public TraitBoolean GrenadeRegeneration;
                public TraitBoolean WeaponPickup;

                public TraitGrenadeCount GrenadeCount;
                public TraitInfiniteAmmo InfiniteAmmo;
                public TraitEquipmentUsage EquipmentUsage;
                public TraitBoolean DropsEquipment;
                public TraitBoolean InfiniteEquipment;

                [TagField(Length = 0x3, Flags = Padding)]
                public byte[] Unused; // need to double check

                public StringId PrimaryWeapon;
                public StringId SecondaryWeapon;
                public StringId Equipment;

                public enum TraitDamagePercentage : sbyte
                {
                    Unchanged,
                    Zero,
                    TwentyFive,
                    Fifty,
                    SeventyFive,
                    Ninety,
                    OneHundred,
                    OneHundredTen,
                    OneHundredTwentyFive,
                    OneHundredFifty,
                    TwoHundred,
                    ThreeHundred,
                    InstantKill
                }
                public enum TraitGrenadeCount : sbyte
                {
                    Unchanged,
                    MapDefault,
                    None,
                    OneFrag,
                    TwoFrag,
                    ThreeFrag,
                    FourFrag,
                    OnePlasma,
                    TwoPlasma,
                    ThreePlasma,
                    FourPlasma,
                    OneEach,
                    TwoEach,
                    ThreeEach,
                    FourEach
                }
                public enum TraitInfiniteAmmo : sbyte
                {
                    Unchanged,
                    Disabled,
                    Enabled,
                    BottomlessClip
                }
                public enum TraitEquipmentUsage : sbyte
                {
                    Unchanged,
                    Disabled,
                    Enabled,
                    SprintOnly // need to double check
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class MovementTrait : TagStructure
            {
                public TraitPlayerSpeedPercentage MovementSpeedModifier;
                public TraitPlayerGravityPercentage GravityModifier;
                public TraitVehicleUse VehicleUse;
                public TraitBoolean CanDoubleJump;
                public int JumpHeight; // percentage, -1 is unchanged

                public enum TraitPlayerSpeedPercentage : sbyte
                {
                    Unchanged,
                    Zero,
                    TwentyFive,
                    Fifty,
                    SeventyFive,
                    Ninety,
                    OneHundred,
                    OneHundredTen,
                    OneHunderedTwenty,
                    OneHunderedThirty,
                    OneHunderedForty,
                    OneHundredFifty,
                    OneHunderedSixty,
                    OneHunderedSeventy,
                    OneHunderedEighty,
                    OneHunderedNinety,
                    TwoHundred,
                    ThreeHundred
                }
                public enum TraitPlayerGravityPercentage : sbyte
                {
                    Unchanged,
                    Fifty,
                    SeventyFive,
                    OneHundred,
                    OneHundredFifty,
                    TwoHundred,
                    TwoHundredFifty,
                    ThreeHundred,
                    ThreeHundredFifty,
                    FourHundred,
                    FourHundredFifty,
                    FiveHundred,
                    FiveHundredFifty,
                    SixHundred
                }
                public enum TraitVehicleUse : sbyte
                {
                    Unchanged,
                    None,
                    PassengerOnly,
                    DriverOnly,
                    GunnerOnly,
                    NoPassenger,
                    NoDriver,
                    NoGunner,
                    Full
                }
            }

            [TagStructure(Size = 0x8)]
            public class AppearanceTrait : TagStructure
            {
                public TraitActiveCamo ActiveCamoQuality;
                public TraitVisibility WaypointVisibility;
                public TraitVisibility NameVisibility;
                public TraitAura Aura;
                public TraitColor Color;

                [TagField(Length = 0x3, Flags = Padding)]
                public byte[] Unused; // need to double check

                public enum TraitActiveCamo : sbyte
                {
                    Unchanged,
                    Disabled,
                    Bad,
                    Poor,
                    Good,
                    Invisible
                }
                public enum TraitVisibility : sbyte
                {
                    Unchanged,
                    None,
                    Allies,
                    Everyone
                }
                public enum TraitAura : sbyte
                {
                    Unchanged,
                    None,
                    Team,
                    Black,
                    White
                }
                public enum TraitColor : sbyte
                {
                    Unchanged,
                    Player,
                    Red,
                    Blue,
                    Green,
                    Orange,
                    Purple,
                    Gold,
                    Brown,
                    Pink,
                    White,
                    Black,
                    Zombie,
                    Unused
                }
            }

            [TagStructure(Size = 0x4)]
            public class HudTrait : TagStructure
            {
                public TraitMotionTrackerMode MotionTrackerMode;
                public TraitMotionTrackerRangeMetres MotionTrackerRange;
                public TraitBoolean DirectionalDamageIndicator;

                [TagField(Length = 0x1, Flags = Padding)]
                public byte[] Unused; // need to double check

                public enum TraitMotionTrackerMode : sbyte
                {
                    Unchanged,
                    Disabled,
                    AllyMovement,
                    PlayerMovement,
                    PlayerLocation
                }
                public enum TraitMotionTrackerRangeMetres : sbyte
                {
                    Unchanged,
                    Ten,
                    Fifteen,
                    TwentyFive,
                    Fifty,
                    SeventyFive,
                    OneHundred,
                    OneHundredFifty
                }
            }

            public enum TraitBoolean : sbyte
            {
                Unchanged,
                Disabled,
                Enabled
            }
        }

        [TagStructure(Size = 0x284)]
        public class DifficultyBlock : TagStructure
        {
            // enemy damage multiplier on easy difficulty
            public float EasyEnemyDamage;
            // enemy damage multiplier on normal difficulty
            public float NormalEnemyDamage;
            // enemy damage multiplier on hard difficulty
            public float HardEnemyDamage;
            // enemy damage multiplier on impossible difficulty
            public float ImpossibleEnemyDamage;
            // enemy maximum body vitality scale on easy difficulty
            public float EasyEnemyVitality;
            // enemy maximum body vitality scale on normal difficulty
            public float NormalEnemyVitality;
            // enemy maximum body vitality scale on hard difficulty
            public float HardEnemyVitality;
            // enemy maximum body vitality scale on impossible difficulty
            public float ImpossibleEnemyVitality;
            // enemy maximum shield vitality scale on easy difficulty
            public float EasyEnemyShield;
            // enemy maximum shield vitality scale on normal difficulty
            public float NormalEnemyShield;
            // enemy maximum shield vitality scale on hard difficulty
            public float HardEnemyShield;
            // enemy maximum shield vitality scale on impossible difficulty
            public float ImpossibleEnemyShield;
            // enemy shield recharge scale on easy difficulty
            public float EasyEnemyRecharge;
            // enemy shield recharge scale on normal difficulty
            public float NormalEnemyRecharge;
            // enemy shield recharge scale on hard difficulty
            public float HardEnemyRecharge;
            // enemy shield recharge scale on impossible difficulty
            public float ImpossibleEnemyRecharge;
            // friend damage multiplier on easy difficulty
            public float EasyFriendDamage;
            // friend damage multiplier on normal difficulty
            public float NormalFriendDamage;
            // friend damage multiplier on hard difficulty
            public float HardFriendDamage;
            // friend damage multiplier on impossible difficulty
            public float ImpossibleFriendDamage;
            // friend maximum body vitality scale on easy difficulty
            public float EasyFriendVitality;
            // friend maximum body vitality scale on normal difficulty
            public float NormalFriendVitality;
            // friend maximum body vitality scale on hard difficulty
            public float HardFriendVitality;
            // friend maximum body vitality scale on impossible difficulty
            public float ImpossibleFriendVitality;
            // friend maximum shield vitality scale on easy difficulty
            public float EasyFriendShield;
            // friend maximum shield vitality scale on normal difficulty
            public float NormalFriendShield;
            // friend maximum shield vitality scale on hard difficulty
            public float HardFriendShield;
            // friend maximum shield vitality scale on impossible difficulty
            public float ImpossibleFriendShield;
            // friend shield recharge scale on easy difficulty
            public float EasyFriendRecharge;
            // friend shield recharge scale on normal difficulty
            public float NormalFriendRecharge;
            // friend shield recharge scale on hard difficulty
            public float HardFriendRecharge;
            // friend shield recharge scale on impossible difficulty
            public float ImpossibleFriendRecharge;
            // toughness of infection forms (may be negative) on easy difficulty
            public float EasyInfectionForms;
            // toughness of infection forms (may be negative) on normal difficulty
            public float NormalInfectionForms;
            // toughness of infection forms (may be negative) on hard difficulty
            public float HardInfectionForms;
            // toughness of infection forms (may be negative) on impossible difficulty
            public float ImpossibleInfectionForms;
            [TagField(Length = 16, Flags = Padding)]
            public byte[] Padding0;
            // enemy rate of fire scale on easy difficulty
            public float EasyRateOfFire;
            // enemy rate of fire scale on normal difficulty
            public float NormalRateOfFire;
            // enemy rate of fire scale on hard difficulty
            public float HardRateOfFire;
            // enemy rate of fire scale on impossible difficulty
            public float ImpossibleRateOfFire;
            // enemy projectile error scale, as a fraction of their base firing error. on easy difficulty
            public float EasyProjectileError;
            // enemy projectile error scale, as a fraction of their base firing error. on normal difficulty
            public float NormalProjectileError;
            // enemy projectile error scale, as a fraction of their base firing error. on hard difficulty
            public float HardProjectileError;
            // enemy projectile error scale, as a fraction of their base firing error. on impossible difficulty
            public float ImpossibleProjectileError;
            // enemy burst error scale; reduces intra-burst shot distance. on easy difficulty
            public float EasyBurstError;
            // enemy burst error scale; reduces intra-burst shot distance. on normal difficulty
            public float NormalBurstError;
            // enemy burst error scale; reduces intra-burst shot distance. on hard difficulty
            public float HardBurstError;
            // enemy burst error scale; reduces intra-burst shot distance. on impossible difficulty
            public float ImpossibleBurstError;
            // enemy new-target delay scale factor. on easy difficulty
            public float EasyNewTargetDelay;
            // enemy new-target delay scale factor. on normal difficulty
            public float NormalNewTargetDelay;
            // enemy new-target delay scale factor. on hard difficulty
            public float HardNewTargetDelay;
            // enemy new-target delay scale factor. on impossible difficulty
            public float ImpossibleNewTargetDelay;
            // delay time between bursts scale factor for enemies. on easy difficulty
            public float EasyBurstSeparation;
            // delay time between bursts scale factor for enemies. on normal difficulty
            public float NormalBurstSeparation;
            // delay time between bursts scale factor for enemies. on hard difficulty
            public float HardBurstSeparation;
            // delay time between bursts scale factor for enemies. on impossible difficulty
            public float ImpossibleBurstSeparation;
            // additional target tracking fraction for enemies. on easy difficulty
            public float EasyTargetTracking;
            // additional target tracking fraction for enemies. on normal difficulty
            public float NormalTargetTracking;
            // additional target tracking fraction for enemies. on hard difficulty
            public float HardTargetTracking;
            // additional target tracking fraction for enemies. on impossible difficulty
            public float ImpossibleTargetTracking;
            // additional target leading fraction for enemies. on easy difficulty
            public float EasyTargetLeading;
            // additional target leading fraction for enemies. on normal difficulty
            public float NormalTargetLeading;
            // additional target leading fraction for enemies. on hard difficulty
            public float HardTargetLeading;
            // additional target leading fraction for enemies. on impossible difficulty
            public float ImpossibleTargetLeading;
            // overcharge chance scale factor for enemies. on easy difficulty
            public float EasyOverchargeChance;
            // overcharge chance scale factor for enemies. on normal difficulty
            public float NormalOverchargeChance;
            // overcharge chance scale factor for enemies. on hard difficulty
            public float HardOverchargeChance;
            // overcharge chance scale factor for enemies. on impossible difficulty
            public float ImpossibleOverchargeChance;
            // delay between special-fire shots (overcharge, banshee bombs) scale factor for enemies. on easy difficulty
            public float EasySpecialFireDelay;
            // delay between special-fire shots (overcharge, banshee bombs) scale factor for enemies. on normal difficulty
            public float NormalSpecialFireDelay;
            // delay between special-fire shots (overcharge, banshee bombs) scale factor for enemies. on hard difficulty
            public float HardSpecialFireDelay;
            // delay between special-fire shots (overcharge, banshee bombs) scale factor for enemies. on impossible difficulty
            public float ImpossibleSpecialFireDelay;
            // guidance velocity scale factor for all projectiles targeted on a player. on easy difficulty
            public float EasyGuidanceVsPlayer;
            // guidance velocity scale factor for all projectiles targeted on a player. on normal difficulty
            public float NormalGuidanceVsPlayer;
            // guidance velocity scale factor for all projectiles targeted on a player. on hard difficulty
            public float HardGuidanceVsPlayer;
            // guidance velocity scale factor for all projectiles targeted on a player. on impossible difficulty
            public float ImpossibleGuidanceVsPlayer;
            // delay period added to all melee attacks, even when berserk. on easy difficulty
            public float EasyMeleeDelayBase;
            // delay period added to all melee attacks, even when berserk. on normal difficulty
            public float NormalMeleeDelayBase;
            // delay period added to all melee attacks, even when berserk. on hard difficulty
            public float HardMeleeDelayBase;
            // delay period added to all melee attacks, even when berserk. on impossible difficulty
            public float ImpossibleMeleeDelayBase;
            // multiplier for all existing non-berserk melee delay times. on easy difficulty
            public float EasyMeleeDelayScale;
            // multiplier for all existing non-berserk melee delay times. on normal difficulty
            public float NormalMeleeDelayScale;
            // multiplier for all existing non-berserk melee delay times. on hard difficulty
            public float HardMeleeDelayScale;
            // multiplier for all existing non-berserk melee delay times. on impossible difficulty
            public float ImpossibleMeleeDelayScale;
            [TagField(Length = 16, Flags = Padding)]
            public byte[] Padding1;
            // scale factor affecting the desicions to throw a grenade. on easy difficulty
            public float EasyGrenadeChanceScale;
            // scale factor affecting the desicions to throw a grenade. on normal difficulty
            public float NormalGrenadeChanceScale;
            // scale factor affecting the desicions to throw a grenade. on hard difficulty
            public float HardGrenadeChanceScale;
            // scale factor affecting the desicions to throw a grenade. on impossible difficulty
            public float ImpossibleGrenadeChanceScale;
            // scale factor affecting the delay period between grenades thrown from the same encounter (lower is more often). on easy difficulty
            public float EasyGrenadeTimerScale;
            // scale factor affecting the delay period between grenades thrown from the same encounter (lower is more often). on normal difficulty
            public float NormalGrenadeTimerScale;
            // scale factor affecting the delay period between grenades thrown from the same encounter (lower is more often). on hard difficulty
            public float HardGrenadeTimerScale;
            // scale factor affecting the delay period between grenades thrown from the same encounter (lower is more often). on impossible difficulty
            public float ImpossibleGrenadeTimerScale;
            [TagField(Length = 16, Flags = Padding)]
            public byte[] Padding2;
            [TagField(Length = 16, Flags = Padding)]
            public byte[] Padding3;
            [TagField(Length = 16, Flags = Padding)]
            public byte[] Padding4;
            // fraction of actors upgraded to their major variant. on easy difficulty
            public float EasyMajorUpgradenormal;
            // fraction of actors upgraded to their major variant. on normal difficulty
            public float NormalMajorUpgradenormal;
            // fraction of actors upgraded to their major variant. on hard difficulty
            public float HardMajorUpgradenormal;
            // fraction of actors upgraded to their major variant. on impossible difficulty
            public float ImpossibleMajorUpgradenormal;
            // fraction of actors upgraded to their major variant when mix = normal. on easy difficulty
            public float EasyMajorUpgradefew;
            // fraction of actors upgraded to their major variant when mix = normal. on normal difficulty
            public float NormalMajorUpgradefew;
            // fraction of actors upgraded to their major variant when mix = normal. on hard difficulty
            public float HardMajorUpgradefew;
            // fraction of actors upgraded to their major variant when mix = normal. on impossible difficulty
            public float ImpossibleMajorUpgradefew;
            // fraction of actors upgraded to their major variant when mix = many. on easy difficulty
            public float EasyMajorUpgrademany;
            // fraction of actors upgraded to their major variant when mix = many. on normal difficulty
            public float NormalMajorUpgrademany;
            // fraction of actors upgraded to their major variant when mix = many. on hard difficulty
            public float HardMajorUpgrademany;
            // fraction of actors upgraded to their major variant when mix = many. on impossible difficulty
            public float ImpossibleMajorUpgrademany;
            // Chance of deciding to ram the player in a vehicle on easy difficulty
            public float EasyPlayerVehicleRamChance;
            // Chance of deciding to ram the player in a vehicle on normal difficulty
            public float NormalPlayerVehicleRamChance;
            // Chance of deciding to ram the player in a vehicle on hard difficulty
            public float HardPlayerVehicleRamChance;
            // Chance of deciding to ram the player in a vehicle on impossible difficulty
            public float ImpossiblePlayerVehicleRamChance;
            [TagField(Length = 16, Flags = Padding)]
            public byte[] Padding5;
            [TagField(Length = 16, Flags = Padding)]
            public byte[] Padding6;
            [TagField(Length = 16, Flags = Padding)]
            public byte[] Padding7;
            [TagField(Length = 84, Flags = Padding)]
            public byte[] Padding8;
        }

        [TagStructure(Size = 0x84)]
        public class CoopDifficultyBlock : TagStructure
        {
            /* vitality */
            public float TwoPlayerShieldRechargeDelay; // multiplier on enemy shield recharge delay with two coop players
            public float FourPlayerShieldRechargeDelay; // multiplier on enemy shield recharge delay with four coop players
            public float SixPlayerShieldRechargeDelay; // multiplier on enemy shield recharge delay with six coop players or more
            public float TwoPlayerShieldRechargeTimer; // multiplier on enemy shield recharge timer with two coop players
            public float FourPlayerShieldRechargeTimer; // multiplier on enemy shield recharge timer with four coop players
            public float SixPlayerShieldRechargeTimer; // multiplier on enemy shield recharge timer with six coop players or more

            /* movement */
            public float TwoPlayerGrenadeDiveChance; // multiplier on enemy grenade dive chance with two coop players
            public float FourPlayerGrenadeDiveChance; // multiplier on enemy grenade dive chance with four coop players
            public float SixPlayerGrenadeDiveChance; // multiplier on enemy grenade dive chance with six coop players or more
            public float TwoPlayerArmorLockChance; // multiplier on enemy armor lock chance with two coop players
            public float FourPlayerArmorLockChance; // multiplier on enemy armor lock chance with four coop players
            public float SixPlayerArmorLockChance; // multiplier on enemy armor lock chance with six coop players or more

            /* evasion */
            public float TwoPlayerEvasionDangerThreshold; // multiplier on enemy evasion danger threshold with two coop players
            public float FourPlayerEvasionDangerThreshold; // multiplier on enemy evasion danger threshold with four coop players
            public float SixPlayerEvasionDangerThreshold; // multiplier on enemy evasion danger threshold with six coop players or more
            public float TwoPlayerEvasionDelayTimer; // multiplier on enemy evasion delay timer with two coop players
            public float FourPlayerEvasionDelayTimer; // multiplier on enemy evasion delay timer with four coop players
            public float SixPlayerEvasionDelayTimer; // multiplier on enemy evasion delay timer with six coop players or more
            public float TwoPlayerEvasionChance; // multiplier on enemy evasion chance with two coop players
            public float FourPlayerEvasionChance; // multiplier on enemy evasion chance with four coop players
            public float SixPlayerEvasionChance; // multiplier on enemy evasion chance with six coop players or more

            /* shooting */
            public float TwoPlayerBurstDuration; // multiplier on the enemy shooting burst duration with two coop players
            public float FourPlayerBurstDuration; // multiplier on the enemy shooting burst duration with four coop players
            public float SixPlayerBurstDuration; // multiplier on the enemy shooting burst duration with six coop players or more
            public float TwoPlayerBurstSeparation; // multipler on the enemy shooting burst separation with two coop players
            public float FourPlayerBurstSeparation; // multipler on the enemy shooting burst separation with four coop players
            public float SixPlayerBurstSeparation; // multipler on the enemy shooting burst separation with six coop players or more
            public float TwoPlayerDamageModifier; // multipler on the enemy shooting damage multiplier with two coop players
            public float FourPlayerDamageModifier; // multipler on the enemy shooting damage multiplier with four coop players
            public float SixPlayerDamageModifier; // multipler on the enemy shooting damage multiplier with six coop players or more

            /* projectile */
            public float TwoPlayerProjectileSpeed; // multiplier on the speed of projectiles fired by enemies with two coop players
            public float FourPlayerProjectileSpeed; // multiplier on the speed of projectiles fired by enemies with four coop players
            public float SixPlayerProjectileSpeed; // multiplier on the speed of projectiles fired by enemies with six coop players or more
        }

        [TagStructure(Size = 0x44, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x38, MinVersion = CacheVersion.HaloReach)]
        public class Grenade : TagStructure
		{
            public short MaximumCount;

            [TagField(Length = 2, Flags = Padding)]
            public byte[] Padding0;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float DropPercentage;

            [TagField(ValidTags = new[] { "effe" })]
            public CachedTag ThrowingEffect; // The effect produced by the grenade when a biped throws it.

            [TagField(MaxVersion = CacheVersion.HaloOnline700123, Length = 16, Flags = Padding)]
            public byte[] Padding1;

            [TagField(Flags = Label, ValidTags = new[] { "eqip" })]
            public CachedTag Equipment; // The equipment tag associated with the grenade.

            [TagField(ValidTags = new[] { "proj" })]
            public CachedTag Projectile; // The projectile tag associated with the grenade.
        }

        [TagStructure(Size = 0x1C, MinVersion = CacheVersion.HaloReach)]
        public class SoftBarrierProperty : TagStructure
        {
            public float BipedSpringConstant;
            public float BipedNormalDamping;
            public float BipedTangentDamping;
            public float BipedMinTangentDampVelocity;
            public float VehicleSpringConstant;
            public float VehicleNormalDamping;
            public float VehicleTangentDamping;
        }

        [TagStructure(Size = 0x120, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x12C, MinVersion = CacheVersion.HaloOnlineED)]
        [TagStructure(Size = 0x120, MinVersion = CacheVersion.HaloReach)]
        public class InterfaceTagsBlock : TagStructure
		{
            public CachedTag SpinnerObsolete;
            public CachedTag Obsolete2;
            public CachedTag ScreenColorTable;
            public CachedTag HudColorTable;
            public CachedTag EditorColorTable;
            public CachedTag DialogColorTable;
            public CachedTag MotionSensorSweepBitmap;
            public CachedTag MotionSensorSweepBitmapMask;
            public CachedTag MultiplayerHudBitmap;
            public CachedTag Unused;
            public CachedTag MotionSensorBlipBitmap;
            public CachedTag InterfaceGooMap1;
            public CachedTag InterfaceGooMap2;
            public CachedTag InterfaceGooMap3;
            public CachedTag MainMenuUiGlobals;
            public CachedTag SinglePlayerUiGlobals;
            public CachedTag MultiplayerUiGlobals;
            public CachedTag HudGlobals;

            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public List<GfxUiString> GfxUiStrings;

            [TagStructure(Size = 0x30)]
            public class GfxUiString : TagStructure
			{
                [TagField(Flags = Label, Length = 32)]
                public string Name;
                public CachedTag Strings;
            }
        }

        [TagStructure(Size = 0x10)]
        public class CheatWeapon : TagStructure
        {
            public CachedTag Weapon;
        }

        [TagStructure(Size = 0x10)]
        public class CheatPowerupsBlock : TagStructure
        {
            public CachedTag Powerup;
        }

        [TagStructure(Size = 0xC0, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0xC4, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0xCC, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0xF4, MinVersion = CacheVersion.HaloReach)]
        public class PlayerInformationBlock : TagStructure
		{
            public float WalkingSpeed; // world units per second
            public float RunForward; // world units per second
            public float RunBackward; // world units per second
            public float RunSideways; // world units per second
            public float RunAcceleration; // world units per second squared
            public float SneakForward; // world units per second
            public float SneakBackward; // world units per second
            public float SneakSideways; // world units per second
            public float SneakAcceleration; // world units per second squared
            public float AirborneAcceleration; // world units per second squared
            public RealPoint3d GrenadeOrigin;
            
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public TagFunction StunFunction = new TagFunction { Data = new byte[0] };

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float StunMovementPenalty; // [0,1] 1.0 prevents moving while stunned
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float StunTurningPenalty; // [0,1] 1.0 turning moving while stunned
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float StunJumpingPenalty; // [0,1] 1.0 jumping moving while stunned
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public Bounds<float> StunTimeRange; // (seconds) no stun can last shorter/longer than this

            public Bounds<float> FirstPersonIdleTimeRange; // (seconds)
            public float FirstPersonSkipFraction; // [0,1]

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public float Unknown1;

            public CachedTag CoopCountdownSound;
            public CachedTag CoopRespawnSound;
            public CachedTag CoopRespawnEffect;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public CachedTag Unknown7;

            public int BinocularsZoomCount;
            public Bounds<float> BinocularZoomRange;

            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public float Unknown5;
            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public float Unknown6;

            public CachedTag FlashlightOn;
            public CachedTag FlashlightOff;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public CachedTag DefaultDamageResponse;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public uint Unknown8;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public uint Unknown9;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public uint Unknown10;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public uint Unknown11;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown12;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown13;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float SprintSpeedMultiplier;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float SprintLookSpeedMultiplier;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public Angle Unknown14;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public Angle Unknown15;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public uint Unknown16;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public uint Unknown17;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public StringId SprintExport;
        }

        [TagStructure(Size = 0x54, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x6C, MinVersion = CacheVersion.Halo3ODST)]
        public class PlayerRepresentationBlock : TagStructure
		{
            [TagField(Flags = Label, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public StringId Name;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public uint Flags;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public CachedTag UnitHud;

            public CachedTag FirstPersonHands;
            public CachedTag FirstPersonBody;
            public CachedTag ThirdPersonUnit;
            public StringId ThirdPersonVariant;
            public CachedTag BinocularsZoomInSound;
            public CachedTag BinocularsZoomOutSound;

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public CachedTag CombatDialogue;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int PlayerInformationIndex;
        }

        [TagStructure(Size = 0xC)]
        public class PlayerCharacterType : TagStructure
        {
            [TagField(Flags = Label)]
            public StringId Name;

            public FlagsValue Flags;
            public sbyte PlayerInformation;
            public sbyte PlayerControl;
            public sbyte CampaignRepresentation;
            public sbyte MultiplayerRepresentation;
            public sbyte MultiplayerArmorCustomization;
            public sbyte ChudGlobals;
            public sbyte FirstPersonInterface;

            [Flags]
            public enum FlagsValue : byte
            {
                None,
                CanSprint = 1 << 0
            }
        }

        [TagStructure(Size = 0x78, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0xC4, MinVersion = CacheVersion.HaloReach)]
        public class FallingDamageBlock : TagStructure
		{
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public Bounds<float> HarmfulFallingDistanceBounds; // world units

            [TagField(ValidTags = new[] { "jpt!" })]
            public CachedTag FallingDamage;

            [TagField(ValidTags = new[] { "jpt!" })]
            public CachedTag JumpingDamage;

            [TagField(ValidTags = new[] { "jpt!" })]
            public CachedTag SoftLandingDamage;

            [TagField(ValidTags = new[] { "jpt!" })]
            public CachedTag HardLandingDamage;

            [TagField(ValidTags = new[] { "jpt!" })]
            public CachedTag ScriptDamage;

            public float MaximumFallingDistance; // world units

            [TagField(ValidTags = new[] { "jpt!" })]
            public CachedTag DistanceDamage;

            [TagField(ValidTags = new[] { "drdf" }, MinVersion = CacheVersion.HaloReach)]
            public CachedTag FallingDamageResponse;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public CachedTag FriendlyFireDamageResponse;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float RuntimeMaximumFallingVelocity;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public Bounds<float> RuntimeDamageVelocityBounds;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float PlayerShieldSpillover;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float CurrentDamageDecayDelay;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float CurrentDamageDecayTime;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float CurrentDamageDecayRate;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float RecentDamageDecayDelay;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float RecentDamageDecayTime;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float AiCurrentDamageDecayDelay;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float AiCurrentDamageDecayTime;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float AiCurrentDamageDecayRate;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float AiRecentDamageDecayDelay;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float AiRecentDamageDecayTime;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float ShieldImpactCurrentDamageDecayDelay;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float ShieldImpactCurrentDamageDecayTime;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float ShieldImpactCurrentDamageDecayRate;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float ShieldImpactRecentDamageDecayDelay;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float ShieldImpactRecentDamageDecayTime;
        }

        [TagStructure(Size = 0xC)]
        public class ShieldBoostBlock : TagStructure //Single block
		{
            public float ShieldBoostDecay; //100
            public float ShieldBoostRechargeTime; //1
            public float ShieldBoostStunTime; //1
        }

        [Flags]
        public enum MaterialFlags : ushort
        {
            None = 0,
            Flammable = 1 << 0,
            Biomass = 1 << 1,
            RadXferInterior = 1 << 2
        }
        
        [Flags]
        public enum SweetenerInheritanceFlags : int
        {
            None = 0,
            SoundSmall = 1 << 0,
            SoundMedium = 1 << 1,
            SoundLarge = 1 << 2,
            SoundRolling = 1 << 3,
            SoundGrinding = 1 << 4,
            SoundMeleeSmall = 1 << 5,
            SoundMeleeMedium = 1 << 6,
            SoundMeleeLarge = 1 << 7,
            EffectSmall = 1 << 8,
            EffectMedium = 1 << 9,
            EffectLarge = 1 << 10,
            EffectRolling = 1 << 11,
            EffectGrinding = 1 << 12,
            EffectMelee = 1 << 13,
            WaterRippleSmall = 1 << 14,
            WaterRippleMedium = 1 << 15,
            WaterRippleLarge = 1 << 16
        }
        
        [TagStructure(Size = 0x170, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x178, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x1A8, MinVersion = CacheVersion.HaloReach)]
        public class Material : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;
            public StringId ParentName;
            public short RuntimeMaterialIndex;
            public MaterialFlags Flags;

            public StringId GeneralArmor;
            public StringId SpecificArmor;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public StringId WetArmor;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short WetArmorIndex;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short WetArmorUnknown;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short WetArmorReferenceIndex;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short Unknown;

            public int PhysicsFlags;
            public float Friction;
            public float Restitution;
            public float Density;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public CachedTag GlobalWaterDragProperties;

            public List<WaterDragProperty> WaterDragProperties;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float WaterDragUnknown1;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float WaterDragUnknown2;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float WaterDragUnknown3;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float WaterDragUnknown4;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float WaterDragUnknown5;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float WaterDragUnknown6;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float WaterDragUnknown7;

            [TagField(ValidTags = new[] { "bsdt" })]
            public CachedTag BreakableSurface;

            public CachedTag SoundSweetenerSmall;
            public CachedTag SoundSweetenerMedium;
            public CachedTag SoundSweetenerLarge;

            [TagField(ValidTags = new[] { "lsnd" })]
            public CachedTag SoundSweetenerRolling;

            [TagField(ValidTags = new[] { "lsnd" })]
            public CachedTag SoundSweetenerGrinding;

            public CachedTag SoundSweetenerMeleeSmall;
            public CachedTag SoundSweetenerMeleeMedium;
            public CachedTag SoundSweetenerMeleeLarge;

            [TagField(ValidTags = new[] { "effe" })]
            public CachedTag EffectSweetenerSmall;

            [TagField(ValidTags = new[] { "effe" })]
            public CachedTag EffectSweetenerMedium;

            [TagField(ValidTags = new[] { "effe" })]
            public CachedTag EffectSweetenerLarge;

            [TagField(ValidTags = new[] { "effe" })]
            public CachedTag EffectSweetenerRolling;

            [TagField(ValidTags = new[] { "effe" })]
            public CachedTag EffectSweetenerGrinding;

            [TagField(ValidTags = new[] { "effe" })]
            public CachedTag EffectSweetenerMelee;

            [TagField(ValidTags = new[] { "rwrd" })]
            public CachedTag WaterRippleSmall;

            [TagField(ValidTags = new[] { "rwrd" })]
            public CachedTag WaterRippleMedium;

            [TagField(ValidTags = new[] { "rwrd" })]
            public CachedTag WaterRippleLarge;

            public SweetenerInheritanceFlags InheritanceFlags;

            [TagField(ValidTags = new[] { "foot" })]
            public CachedTag MaterialEffects;

            public List<UnderwaterProxy> UnderwaterProxies;

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public StringId FootstepsInRainMaterial;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public short RuntimeRainMaterialIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public byte[] Padding;

            [TagStructure(Size = 0x28, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x10, MinVersion = CacheVersion.HaloReach)]
            public class WaterDragProperty : TagStructure
			{
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float DragK;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float DragQ;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float DragE;

                [TagField(Length = 4, Flags = TagFieldFlags.Padding, MaxVersion = CacheVersion.HaloOnline700123)]
                public byte[] Padding0;

                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float SuperFloater;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float Floater;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float Neutral;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float Sinker;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float SuperSinker;

                [TagField(Length = 4, Flags = TagFieldFlags.Padding, MaxVersion = CacheVersion.HaloOnline700123)]
                public byte[] Padding1;

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public CachedTag WaterDragProperties;
            }

            [TagStructure(Size = 0xC)]
            public class UnderwaterProxy : TagStructure
			{
                [TagField(Flags = Label)]
                public StringId SurfaceName;
                public StringId SubmergedName;
                public short SurfaceIndex;
                public short SubmergedIndex;
            }
        }

        [TagStructure(Size = 0xC)]
        public class ColorBlock : TagStructure
		{
            public RealRgbColor Color;
        }

        [TagStructure(Size = 0x1C)]
        public class VisorColorBlock : TagStructure
        {
            public StringId Name;
            public RealRgbColor PrimaryColor;
            public RealRgbColor SecondaryColor;
        }

        [TagStructure(Size = 0x10)]
        public class ForgeColorBlock : TagStructure
        {
            public StringId Name;
            public RealRgbColor Color;
        }

        [TagStructure(Size = 0x14, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x18, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x4C, MinVersion = CacheVersion.HaloReach)]
        public class CinematicsGlobals : TagStructure
		{
            public CachedTag CinematicAnchor;

            public float CinematicFilmAperture;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public float CinematicSkipUiUpTime;

            // percentage towards the center - 0=default, 0.5=center of the screen
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public Bounds<float> SubtitleRectWidth;
            // 0=default, 0.5=center of the screen
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public Bounds<float> SubtitleRectHeight;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public RealRgbColor DefaultSubtitleColor;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public RealRgbColor DefaultSubtitleShadowColor;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<CinematicCharactersBlock> CinematicCharacters;

            [TagStructure(Size = 0x1C, MinVersion = CacheVersion.HaloReach)]
            public class CinematicCharactersBlock : TagStructure
            {
                public StringId CharacterName;
                public RealRgbColor SubtitleColor;
                public RealRgbColor ShadowColor;
            }
        }

        [TagStructure(Size = 0x48, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x98, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x38, MinVersion = CacheVersion.HaloReach)]
        public class CampaignMetagameGlobal : TagStructure
		{
            public List<Medal> Medals;
            public List<MultiplierBlock> Difficulty;
            public List<MultiplierBlock> PrimarySkulls;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public List<MultiplierBlock> SecondarySkulls;
            public int FriendlyDeathPenalty;
            public int DeathPenalty;
            public int BetrayalPenalty;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public int MultiKillCount; // how many kills for this to happen
            public float MultiKillWindow; // in what period of time does this have to happen (seconds)
            public float EmpKillWindow; // time after taking a guys shields down with emp damage you have to get the emp kill bonus (seconds)

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public float HijackKillWindowPeriod;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public float SkyjackKillWindowPeriod;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public uint KillFromTheGraveRequiredPeriod;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public int FirstWeaponSpree;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public int SecondWeaponSpree;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public int KillingSpree;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public int KillingFrenzy;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public int RunningRiot;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public int Rampage;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public int Untouchable;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public int Invincible;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public int DoubleKill;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public int TripleKill;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public int Overkill;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public int Killtacular;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public int Killtrocity;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public int Killimanjaro;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public int Killtastrophe;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public int Killpocalypse;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public int Killionaire;

            [TagStructure(Size = 0x4, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x8, MinVersion = CacheVersion.HaloReach)]
            public class Medal : TagStructure
			{
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public StringId EventNameReach;

                public float Multiplier;

                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
                public int AwardedPoints;

                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
                public int MedalUptime;

                [TagField(Flags = Label, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
                public StringId EventName;
            }

            [TagStructure(Size = 0x4)]
            public class MultiplierBlock : TagStructure
			{
                public float Multiplier;
            }
        }

        [TagStructure(Size = 0x44, MaxVersion = CacheVersion.HaloReach11883, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x50, Align = 0x8, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        public class LanguagePack : TagStructure
        {
            public PlatformUnsignedValue StringReferenceAddress;
            public PlatformUnsignedValue StringDataAddress;

            public int StringCount;
            public int LocaleTableSize;
            public uint LocaleIndexTableAddress;
            public uint LocaleDataIndexAddress;

            [TagField(Length = 20)]
            public byte[] IndexTableHash;

            [TagField(Length = 20)]
            public byte[] StringDataHash;

            public uint Unknown3;

            [TagField(Platform = CachePlatform.MCC, MinVersion = CacheVersion.Halo3Retail, Length = 4)]
            public byte[] Padding;
        }

        [TagStructure(Size = 0x54)]
        public class ActiveCamoGlobalsBlock : TagStructure
        {
            // for bipeds, the speed at which you are on the far right of the 'speed to max camo' graph
            public float BipedSpeedReference; // wu/s
            // for vehicles, the speed at which you are on the far right of the 'speed to max camo' graph
            public float VehicleSpeedReference; // wu/s
            // minimum active camo percentage at which a player's game name will start becoming visible
            public float CamoValueForGameName;

            public TagFunction CamoValueToDistortion = new TagFunction { Data = new byte[0] };
            public TagFunction CamoValueToTransparency = new TagFunction { Data = new byte[0] };
            public TagFunction CamoDistortionTextureStrength = new TagFunction { Data = new byte[0] };

            public List<ActiveCamoLevelDefinitionBlock> CamoLevels;

            [TagStructure(Size = 0x24)]
            public class ActiveCamoLevelDefinitionBlock : TagStructure
            {
                // reduces camo value by this much when throwing a grenade
                public float GrenadeThrowPenalty; // 0..1
                // reduces camo by this much when meleeing
                public float MeleePenalty; // 0..1
                // when taking damage or doing other actions that reduce camo, we will never drop below this value
                public float MinimumDingedValue;
                // time it takes to interpolate from 0.0 to 1.0
                public float InterpolationTime; // s
                public TagFunction SpeedToMaximumCamo = new TagFunction { Data = new byte[0] };
            }
        }

        [TagStructure(Size = 0x18)]
        public class GarbageCollectionBlock : TagStructure
        {
            public float DroppedItem; // seconds
            public float DroppedItemByPlayer; // seconds
            public float DroppedItemInMultiplayer; // seconds
            public float BrokenConstraints; // seconds
            public float DeadUnit; // seconds
            public float DeadPlayer; // seconds
        }

        [TagStructure(Size = 0x14)]
        public class GlobalCameraImpulseBlock : TagStructure
        {
            public TagFunction Function = new TagFunction { Data = new byte[0] };
        }

        [TagStructure(Size = 0x24)]
        public class DamageReportingType : TagStructure
		{
            public short Index;
            public short Version;
            [TagField(Flags = Label, Length = 32)]
            public string Name;
        }

        [TagStructure(Size = 0x8, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3Retail)]
        public class HavokCleanupResource : TagStructure
        {
            [TagField(Flags = Label)]
            public CachedTag ObjectCleanupEffect;
        }
    }
}