using TagTool.Ai;
using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "globals", Tag = "matg", Size = 0x600, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "globals", Tag = "matg", Size = 0x608, MaxVersion = CacheVersion.HaloOnline449175)]
    [TagStructure(Name = "globals", Tag = "matg", Size = 0x618, MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline571627)]
    [TagStructure(Name = "globals", Tag = "matg", Size = 0x614, MinVersion = CacheVersion.HaloOnline700123, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "globals", Tag = "matg", Size = 0x714, MinVersion = CacheVersion.HaloReach, MaxVersion = CacheVersion.HaloReach)]
    [TagStructure(Name = "globals", Tag = "matg", Size = 0x7A8, MinVersion = CacheVersion.HaloReachMCC0824)]
    public class Globals : TagStructure
	{
        [TagField(Flags = Padding, Length = 172)]
        public byte[] Unused;

        public GameLanguage Language;

        public List<TagReferenceBlock> HavokObjectCleanupEffects;

        public List<SoundGlobalsDefinition> SoundGlobals;

        public List<AiGlobalsDatum> AiGlobalsOld;

        [TagField(ValidTags = new[] { "aigl" }, MinVersion = CacheVersion.Halo3ODST)]
        public CachedTag AiGlobals;

        public List<DamageTableBlock> DamageTable;

        public uint Unknown45;
        public uint Unknown46;
        public uint Unknown47;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint Unknown;

        public List<TagReferenceBlock> SoundsOld;

        public List<CameraGlobalsDefinition> Camera;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<ThumbStickDeadZone> ThumbStickDeadZones;

        public List<PlayerControlBlock> PlayerControl;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<PlayerTraitDefault> PlayerTraitDefaults;

        public List<DifficultyBlock> Difficulty;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<UnknownBlock1> UnknownBlocks1;

        public List<Grenade> Grenades;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<SoftBarrierProperty> SoftBarrierProperties;

        public uint Unknown48;
        public uint Unknown49;
        public uint Unknown50;

        public List<InterfaceTagsBlock> InterfaceTags;

        public uint Unknown51;
        public uint Unknown52;
        public uint Unknown53;

        public uint Unknown54;
        public uint Unknown55;
        public uint Unknown56;

        public List<PlayerInformationBlock> PlayerInformation;

        public List<PlayerRepresentationBlock> PlayerRepresentation;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<PlayerCharacterType> PlayerCharacterTypes;

        public List<FallingDamageBlock> FallingDamage;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<UnknownBlock> Unknown60;

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

        public List<CinematicAnchorBlock> CinematicAnchors;
        public List<MetagameGlobal> MetagameGlobals;

        [TagField(ValidTags = new[] { "gmeg" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag MedalGlobals;

        [TagField(Length = 12)]
        public LocaleGlobalsBlock[] LocaleGlobals = new LocaleGlobalsBlock[12];

        [TagField(ValidTags = new[] { "rasg" })]
        public CachedTag RasterizerGlobals;

        [TagField(ValidTags = new[] { "cfxs" })]
        public CachedTag DefaultCameraEffect;

        [TagField(ValidTags = new[] { "pdm!" }, MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag PodiumDefinition;

        [TagField(ValidTags = new[] { "wind" })]
        public CachedTag DefaultWind;

        [TagField(ValidTags = new[] { "wxcg" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag WeatherGlobals;

        [TagField(ValidTags = new[] { "jpt!" })]
        public CachedTag DefaultDamageEffect;

        [TagField(ValidTags = new[] { "cddf" })]
        public CachedTag DefaultCollisionDamage;

        public StringId DefaultWaterMaterial;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public StringId UnknownMaterial;

        public short UnknownGlobalMaterialIndex;
        public short Unknown265;

        [TagField(ValidTags = new[] { "effg" })]
        public CachedTag EffectGlobals;

        [TagField(ValidTags = new[] { "hcfd" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag CollisionFilter;

        [TagField(ValidTags = new[] { "grfr" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag GroundedFriction;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<UnknownBlock2> UnknownBlocks2;

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
        
        [TagField(ValidTags = new[] { "inpg" }, MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag InputGlobals;

        [TagField(ValidTags = new[] { "avat" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag AvatarAwardGlobals;
        [TagField(ValidTags = new[] { "gptd" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag PerformanceThrottleGlobals;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<UnknownBlock3> UnknownBlocks3;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<UnknownBlock4> UnknownBlocks4;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<Material> AlternateMaterials;

        [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown266;
        [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown267;
        [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown268;
        [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown269;
        [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
        public TagFunction Unknown270 = new TagFunction { Data = new byte[0] };
        [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown271;
        [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown272;
        [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown273;
        [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown274;
        [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown275;
        [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<DamageReportingType> DamageReportingTypes;

        [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline571627)]
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
            public List<UnknownBlock> UnknownBlocks1;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<UnknownBlock> UnknownBlocks2;

            public float MagnetismFriction;
            public float MagnetismAdhesion;
            public float InconsequentialTargetScale;

            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown1;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown2;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown3;

            [TagField(MaxVersion = CacheVersion.HaloOnline449175)]
            public RealPoint2d CrosshairLocation;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public RealPoint2d CrosshairLocation_Reach;
            [TagField(MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
            public List<CrosshairLocationBlock> CrosshairLocations;

            //Sprinting
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float SecondsToStart;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float SecondsToFullSpeed;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float DecayRate;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float FullSpeedMultiplier;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float PeggedMagnitude;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float PeggedAngularThreshold;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float Unknown4;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float Unknown5;
            [TagField(MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown6;

            //Looking
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown7;

            public float LookDefaultPitchRate;
            public float LookDefaultYawRate; // need to check

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float LookPegThreshold;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float LookYawAccelerationTime;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float LookYawAccelerationScale;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float LookPitchAccelerationTime;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float LookPitchAccelerationScale;

            public float LookAutolevelingScale;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float Unknown8;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float Unknown9;

            public float GravityScale;
            public short Unknown10;
            public short MinimumAutolevelingTicks;

            [TagField(Gen = CacheGeneration.Third)]
            public float MinVehicleFlipAngle;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public List<LookFunctionBlock> LookFunction;

            [TagField(Gen = CacheGeneration.Third)]
            public float MinimumActionHoldTime;
            [TagField(Gen = CacheGeneration.Third)]
            public float Unknown11;

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
            public class UnknownBlock : TagStructure
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

            [TagStructure]
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

                [TagField(Length = 0x2, Flags = Padding)]
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
            public float EasyEnemyDamage;
            public float NormalEnemyDamage;
            public float HardEnemyDamage;
            public float ImpossibleEnemyDamage;
            public float EasyEnemyVitality;
            public float NormalEnemyVitality;
            public float HardEnemyVitality;
            public float ImpossibleEnemyVitality;
            public float EasyEnemyShield;
            public float NormalEnemyShield;
            public float HardEnemyShield;
            public float ImpossibleEnemyShield;
            public float EasyEnemyRecharge;
            public float NormalEnemyRecharge;
            public float HardEnemyRecharge;
            public float ImpossibleEnemyRecharge;
            public float EasyFriendDamage;
            public float NormalFriendDamage;
            public float HardFriendDamage;
            public float ImpossibleFriendDamage;
            public float EasyFriendVitality;
            public float NormalFriendVitality;
            public float HardFriendVitality;
            public float ImpossibleFriendVitality;
            public float EasyFriendShield;
            public float NormalFriendShield;
            public float HardFriendShield;
            public float ImpossibleFriendShield;
            public float EasyFriendRecharge;
            public float NormalFriendRecharge;
            public float HardFriendRecharge;
            public float ImpossibleFriendRecharge;
            public float EasyInfectionForms;
            public float NormalInfectionForms;
            public float HardInfectionForms;
            public float ImpossibleInfectionForms;
            public float EasyUnknown;
            public float NormalUnknown;
            public float HardUnknown;
            public float ImpossibleUnknown;
            public float EasyRateOfFire;
            public float NormalRateOfFire;
            public float HardRateOfFire;
            public float ImpossibleRateOfFire;
            public float EasyProjectileError;
            public float NormalProjectileError;
            public float HardProjectileError;
            public float ImpossibleProjectileError;
            public float EasyBurstError;
            public float NormalBurstError;
            public float HardBurstError;
            public float ImpossibleBurstError;
            public float EasyTargetDelay;
            public float NormalTargetDelay;
            public float HardTargetDelay;
            public float ImpossibleTargetDelay;
            public float EasyBurstSeparation;
            public float NormalBurstSeparation;
            public float HardBurstSeparation;
            public float ImpossibleBurstSeparation;
            public float EasyTargetTracking;
            public float NormalTargetTracking;
            public float HardTargetTracking;
            public float ImpossibleTargetTracking;
            public float EasyTargetLeading;
            public float NormalTargetLeading;
            public float HardTargetLeading;
            public float ImpossibleTargetLeading;
            public float EasyOverchargeChance;
            public float NormalOverchargeChance;
            public float HardOverchargeChance;
            public float ImpossibleOverchargeChance;
            public float EasySpecialFireDelay;
            public float NormalSpecialFireDelay;
            public float HardSpecialFireDelay;
            public float ImpossibleSpecialFireDelay;
            public float EasyGuidanceVsPlayer;
            public float NormalGuidanceVsPlayer;
            public float HardGuidanceVsPlayer;
            public float ImpossibleGuidanceVsPlayer;
            public float EasyMeleeDelayBase;
            public float NormalMeleeDelayBase;
            public float HardMeleeDelayBase;
            public float ImpossibleMeleeDelayBase;
            public float EasyMeleeDelayScale;
            public float NormalMeleeDelayScale;
            public float HardMeleeDelayScale;
            public float ImpossibleMeleeDelayScale;
            public float EasyUnknown2;
            public float NormalUnknown2;
            public float HardUnknown2;
            public float ImpossibleUnknown2;
            public float EasyGrenadeChanceScale;
            public float NormalGrenadeChanceScale;
            public float HardGrenadeChanceScale;
            public float ImpossibleGrenadeChanceScale;
            public float EasyGrenadeTimerScale;
            public float NormalGrenadeTimerScale;
            public float HardGrenadeTimerScale;
            public float ImpossibleGrenadeTimerScale;
            public float EasyUnknown3;
            public float NormalUnknown3;
            public float HardUnknown3;
            public float ImpossibleUnknown3;
            public float EasyUnknown4;
            public float NormalUnknown4;
            public float HardUnknown4;
            public float ImpossibleUnknown4;
            public float EasyUnknown5;
            public float NormalUnknown5;
            public float HardUnknown5;
            public float ImpossibleUnknown5;
            public float EasyMajorUpgradeNormal;
            public float NormalMajorUpgradeNormal;
            public float HardMajorUpgradeNormal;
            public float ImpossibleMajorUpgradeNormal;
            public float EasyMajorUpgradeFew;
            public float NormalMajorUpgradeFew;
            public float HardMajorUpgradeFew;
            public float ImpossibleMajorUpgradeFew;
            public float EasyMajorUpgradeMany;
            public float NormalMajorUpgradeMany;
            public float HardMajorUpgradeMany;
            public float ImpossibleMajorUpgradeMany;
            public float EasyPlayerVehicleRamChance;
            public float NormalPlayerVehicleRamChance;
            public float HardPlayerVehicleRamChance;
            public float ImpossiblePlayerVehicleRamChance;
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
            public uint Unknown26;
            public uint Unknown27;
            public uint Unknown28;
            public uint Unknown29;
            public uint Unknown30;
            public uint Unknown31;
            public uint Unknown32;
            public uint Unknown33;
        }

        [TagStructure(Size = 0x84)]
        public class UnknownBlock1 : TagStructure
        {
            public float Unknown1;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;
            public float Unknown6;
            public float Unknown7;
            public float Unknown8;
            public float Unknown9;
            public float Unknown10;
            public float Unknown11;
            public float Unknown12;
            public float Unknown13;
            public float Unknown14;
            public float Unknown15;
            public float Unknown16;
            public float Unknown17;
            public float Unknown18;
            public float Unknown19;
            public float Unknown20;
            public float Unknown21;
            public float Unknown22;
            public float Unknown23;
            public float Unknown24;
            public float Unknown25;
            public float Unknown26;
            public float Unknown27;
            public float Unknown28;
            public float Unknown29;
            public float Unknown30;
            public float Unknown31;
            public float Unknown32;
            public float Unknown33;
        }

        [TagStructure(Size = 0x44, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x38, MinVersion = CacheVersion.HaloReach)]
        public class Grenade : TagStructure
		{
            public short MaximumCount;
            public short Unknown;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown6;

            /// <summary>
            /// The effect produced by the grenade when a biped throws it.
            /// </summary>
            [TagField(ValidTags = new[] { "effe" })]
            public CachedTag ThrowingEffect;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown2;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown3;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown4;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown5;

            /// <summary>
            /// The equipment tag associated with the grenade.
            /// </summary>
            [TagField(Flags = Label, ValidTags = new[] { "eqip" })]
            public CachedTag Equipment;

            /// <summary>
            /// The projectile tag associated with the grenade.
            /// </summary>
            [TagField(ValidTags = new[] { "proj" })]
            public CachedTag Projectile;
        }

        [TagStructure(Size = 0x1C, MinVersion = CacheVersion.HaloReach)]
        public class SoftBarrierProperty : TagStructure
        {
            public float BipedGive;
            public float BipedBounciness;
            public float BipedBumpiness;
            public float Unknown;
            public float VehicleGive;
            public float VehicleBounciness;
            public float VehicleBumpiness;
        }

        [TagStructure(Size = 0x120, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x12C, MinVersion = CacheVersion.HaloOnline106708)]
        [TagStructure(Size = 0x120, MinVersion = CacheVersion.HaloReach)]
        public class InterfaceTagsBlock : TagStructure
		{
            public CachedTag Spinner;
            public CachedTag Obsolete;
            public CachedTag ScreenColorTable;
            public CachedTag HudColorTable;
            public CachedTag EditorColorTable;
            public CachedTag DialogColorTable;
            public CachedTag MotionSensorSweepBitmap;
            public CachedTag MotionSensorSweepBitmapMask;
            public CachedTag MultiplayerHudBitmap;
            public CachedTag HudDigitsDefinition;
            public CachedTag MotionSensorBlipBitmap;
            public CachedTag InterfaceGooMap1;
            public CachedTag InterfaceGooMap2;
            public CachedTag InterfaceGooMap3;
            public CachedTag MainMenuUiGlobals;
            public CachedTag SinglePlayerUiGlobals;
            public CachedTag MultiplayerUiGlobals;
            public CachedTag HudGlobals;

            [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
            public List<GfxUiString> GfxUiStrings;

            [TagStructure(Size = 0x30)]
            public class GfxUiString : TagStructure
			{
                [TagField(Flags = Label, Length = 32)]
                public string Name;
                public CachedTag Strings;
            }
        }

        [TagStructure(Size = 0xC0, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0xC4, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0xCC, MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0xF4, MinVersion = CacheVersion.HaloReach)]
        public class PlayerInformationBlock : TagStructure
		{
            public float WalkingSpeed;
            public float RunForward;
            public float RunBackward;
            public float RunSideways;
            public float RunAcceleration;
            public float SneakForward;
            public float SneakBackward;
            public float SneakSideways;
            public float SneakAcceleration;
            public float AirborneAcceleration;
            public RealPoint3d GrenadeOrigin;
            
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public TagFunction StunFunction = new TagFunction { Data = new byte[0] };

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float StunMovementPenalty;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float StunTurningPenalty;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float StunJumpingPenalty;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public Bounds<float> StunTimeRange;

            public Bounds<float> FirstPersonIdleTimeRange;
            public float FirstPersonSkipFraction;

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public float Unknown1;

            public CachedTag Unknown2;
            public CachedTag Unknown3;
            public CachedTag Unknown4;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public CachedTag Unknown7;

            public int BinocularsZoomCount;
            public Bounds<float> BinocularZoomRange;

            [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown5;
            [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown6;

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
            public Bounds<float> HarmfulFallingDistanceBounds;

            [TagField(ValidTags = new[] { "jpt!" })]
            public CachedTag FallingDamage;

            [TagField(ValidTags = new[] { "jpt!" })]
            public CachedTag Unknown;

            [TagField(ValidTags = new[] { "jpt!" })]
            public CachedTag SoftLanding;

            [TagField(ValidTags = new[] { "jpt!" })]
            public CachedTag HardLanding;

            [TagField(ValidTags = new[] { "jpt!" })]
            public CachedTag ScriptDamage;

            public float TerminalVelocity;

            [TagField(ValidTags = new[] { "jpt!" })]
            public CachedTag DistanceDamage;

            [TagField(ValidTags = new[] { "drdf" }, MinVersion = CacheVersion.HaloReach)]
            public CachedTag FallingDamageResponse;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public CachedTag Unknown1;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float Unknown2;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float Unknown3;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float Unknown4;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown5;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown6;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown7;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown8;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown9;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown10;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown11;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown12;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown13;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown14;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown15;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown16;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown17;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown18;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown19;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown20;
        }

        [TagStructure(Size = 0xC)]
        public class UnknownBlock : TagStructure //Single block
		{
            public uint Unknown1; //100
            public uint Unknown2; //1
            public uint Unknown3; //1
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

            [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown2;
            [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
            public short Unknown3;
            [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
            public short Unknown4;

            [TagStructure(Size = 0x28, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x10, MinVersion = CacheVersion.HaloReach)]
            public class WaterDragProperty : TagStructure
			{
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float Unknown;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float Unknown2;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float Unknown3;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float Unknown4;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float Unknown5;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float Unknown6;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float Unknown7;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float Unknown8;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float Unknown9;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float Unknown10;

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
        public class CinematicAnchorBlock : TagStructure
		{
            public CachedTag CinematicAnchor;

            public float FovConstant;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public float Unknown2;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown3;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown4;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown5;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown6;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown7;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown8;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown9;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown10;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown11;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown12;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown13;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown14;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown15;
        }

        [TagStructure(Size = 0x48, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x98, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x38, MinVersion = CacheVersion.HaloReach)]
        public class MetagameGlobal : TagStructure
		{
            public List<Medal> Medals;
            public List<MultiplierBlock> Difficulty;
            public List<MultiplierBlock> PrimarySkulls;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public List<MultiplierBlock> SecondarySkulls;
            public int Unknown;
            public int DeathPenalty;
            public int BetrayalPenalty;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public int Unknown2;
            public float MultikillWindow;
            public float EmpWindow;
            
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown3;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown4;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown5;
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

        [TagStructure(Size = 0x44, MaxVersion = CacheVersion.HaloReach)]
        [TagStructure(Size = 0x50, MinVersion = CacheVersion.HaloReachMCC0824)]
        public class LocaleGlobalsBlock : TagStructure
        {
            [TagField(MaxVersion = CacheVersion.HaloReach)]
            public uint Unknown1;
            [TagField(MaxVersion = CacheVersion.HaloReach)]
            public uint Unknown2;

            [TagField(MinVersion = CacheVersion.HaloReachMCC0824)]
            public ulong Unknown1_64;
            [TagField(MinVersion = CacheVersion.HaloReachMCC0824)]
            public ulong Unknown2_64;

            public int StringCount;
            public int LocaleTableSize;
            public uint LocaleIndexTableAddress;
            public uint LocaleDataIndexAddress;

            [TagField(Length = 20)]
            public byte[] IndexTableHash;

            [TagField(Length = 20)]
            public byte[] StringDataHash;

            public uint Unknown3;

            [TagField(MinVersion = CacheVersion.HaloReachMCC0824)]
            public uint Unknown4;
        }

        [TagStructure(Size = 0x54)]
        public class UnknownBlock2 : TagStructure
        {
            public float Unknown1;
            public float Unknown2;
            public float Unknown3;

            public TagFunction Unknown4 = new TagFunction { Data = new byte[0] };
            public TagFunction Unknown5 = new TagFunction { Data = new byte[0] };
            public TagFunction Unknown6 = new TagFunction { Data = new byte[0] };

            public List<UnknownBlock> UnknownBlocks;

            [TagStructure(Size = 0x24)]
            public class UnknownBlock : TagStructure
            {
                public float Unknown1;
                public float Unknown2;
                public float Unknown3;
                public float Unknown4;
                public TagFunction Unknown5 = new TagFunction { Data = new byte[0] };
            }
        }

        [TagStructure(Size = 0x18)]
        public class UnknownBlock3 : TagStructure
        {
            public float Unknown1;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;
            public float Unknown6;
        }

        [TagStructure(Size = 0x14)]
        public class UnknownBlock4 : TagStructure
        {
            public TagFunction Unknown = new TagFunction { Data = new byte[0] };
        }

        [TagStructure(Size = 0x24)]
        public class DamageReportingType : TagStructure
		{
            public short Index;
            public short Version;
            [TagField(Flags = Label, Length = 32)]
            public string Name;
        }
    }
}