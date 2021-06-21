using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using TagTool.Damage;
using System;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "weapon", Tag = "weap", Size = 0x354, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "weapon", Tag = "weap", Size = 0x358, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "weapon", Tag = "weap", Size = 0x384, MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline449175)]
    [TagStructure(Name = "weapon", Tag = "weap", Size = 0x390, MinVersion = CacheVersion.HaloOnline498295)]
    public class Weapon : Item
    {
        public WeaponFlags WeaponFlags;
        public SecondaryWeaponFlags MoreFlags;
        public StringId Unknown8;
        public SecondaryTriggerModeValue SecondaryTriggerMode;
        public short MaximumAlternateShotsLoaded;
        public float TurnOnTime;
        public float ReadyTime;

        public CachedTag ReadyEffect;
        public CachedTag ReadyDamageEffect;

        public float HeatRecoveryThreshold;
        public float OverheatedThreshold;
        public float HeatDetonationThreshold;
        public float HeatDetonationFraction;
        public float HeatLossPerSecond;
        public float HeatIllumination;
        public float OverheatedHeatLossPerSecond;

        public CachedTag Overheated;
        public CachedTag OverheatedDamageEffect;
        public CachedTag Detonation;
        public CachedTag DetonationDamageEffect2;
        public CachedTag PlayerMeleeDamage;
        public CachedTag PlayerMeleeResponse;

        [TagField(MinVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown27;

        public Angle DamagePyramidAnglesY;
        public Angle DamagePyramidAnglesP;
        public float DamagePyramidDepth;

        public CachedTag FirstHitDamage;
        public CachedTag FirstHitResponse;
        public CachedTag SeconddHitDamage;
        public CachedTag SecondHitResponse;
        public CachedTag ThirdHitDamage;
        public CachedTag ThirdHitResponse;
        public CachedTag LungeMeleeDamage;
        public CachedTag LungeMeleeResponse;
        public CachedTag GunGunClangDamage;
        public CachedTag GunGunClangResponse;
        public CachedTag GunSwordClangDamage;
        public CachedTag GunSwordClangResponse;
        public CachedTag ClashEffect;

        public DamageReportingType MeleeDamageReportingType;
        public sbyte Unknown9;
        public short MagnificationLevels;
        public Bounds<float> MagnificationBounds;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public WeapMagnificationFlags MagnificationFlags;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float WeaponSwitchReadySpeed0Default;

        public Angle AutoaimAngle;
        public float AutoaimRangeLong;
        public float AutoaimRangeShort;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float AutoaimSafeRadius;

        public Angle MagnetismAngle;
        public float MagnetismRangeLong;
        public float MagnetismRangeShort;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float MagnetismSafeRadius;

        public Angle DeviationAngle;
        public uint Unknown10;
        public uint Unknown11;
        public uint Unknown12;
        public uint Unknown13;
        public uint Unknown14;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown15;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<Unit.TargetTrackingBlock> TargetTracking;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown16;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown17;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown18;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown19;

        public MovementPenalizedValue MovementPenalized;
        public short Unknown20;
        public float ForwardsMovementPenalty;
        public float SidewaysMovementPenalty;
        public float AiScariness;
        public float WeaponPowerOnTime;
        public float WeaponPowerOffTime;
        public CachedTag WeaponPowerOnEffect;
        public CachedTag WeaponPowerOffEffect;
        public float AgeHeatRecoveryPenalty;
        public float AgeRateOfFirePenalty;
        public float AgeMisfireStart;
        public float AgeMisfireChance;
        public CachedTag PickupSound;
        public CachedTag ZoomInSound;
        public CachedTag ZoomOutSound;
        public float ActiveCamoDing;
        public float CamoRegrowthRate;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown22;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown23;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public StringId HandleNode;

        public StringId WeaponClass;
        public StringId WeaponName;

        public MultiplayerWeaponTypeValue MultiplayerWeaponType;
        public WeaponTypeValue WeaponType;
        public TrackingType Tracking;
        public short UnknownEnum;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public SpecialHudVersionValue SpecialHudVersion;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public int SpecialHudIcon;
        
        [TagField(Flags = Padding, Length = 16, MaxVersion = CacheVersion.Halo3ODST)]
        public byte[] UnknownBlock;
        
        public List<FirstPersonBlock> FirstPerson;
        public CachedTag HudInterface;
        public List<PredictedResource> PredictedWeaponResources;
        public List<Magazine> Magazines;
        public List<Trigger> Triggers;
        public List<Barrel> Barrels;
        public float WeaponPowerOnVelocity;
        public float WeaponPowerOffVelocity;
        public float MaxMovementAcceleration;
        public float MaximumMovementVelocity;
        public float MaximumTurningAcceleration;
        public float MaximumTurningVelocity;
        public CachedTag DeployedVehicle;
        public CachedTag DeployedWeapon;
        public CachedTag AgeModel;
        public CachedTag AgeWeapon;
        public CachedTag AgedMaterialEffects;
        public float HammerAgePerUse;
        public uint UnknownSwordAgePerUse;
        public RealVector3d FirstPersonWeaponOffset;

        [TagField(MinVersion = CacheVersion.HaloOnline498295)]
        public List<FirstPersonOffsetBlock> NewFirstPersonWeaponOffsets;

        public RealVector2d FirstPersonScopeSize;
        public Bounds<float> CameraPitchBounds3p;
        public float ZoomTransitionTime;
        public float MeleeWeaponDelay;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float ReadyAnimationDuration;

        public StringId WeaponHolsterMarker;

        public enum SecondaryTriggerModeValue : short
        {
            Normal,
            SlavedToPrimary,
            InhibitsPrimary,
            LoadsAlternateAmmunition,
            LoadsMultiplePrimaryAmmunition,
        }

        public enum MovementPenalizedValue : short
        {
            Always,
            WhenZoomed,
            WhenZoomedOrReloading,
        }

        public enum MultiplayerWeaponTypeValue : short
        {
            None,
            CtfFlag,
            OddballBall,
            HeadhunterHead,
            JuggernautPowerup,
        }

        public enum WeaponTypeValue : short
        {
            Undefined,
            Shotgun,
            Needler,
            PlasmaPistol,
            PlasmaRifle,
            RocketLauncher,
            EnergySword,
            SpartanLaser,
        }

        public enum SpecialHudVersionValue : int
        {
            DefaultNoOutline2 = -28,
            Default30 = 0,
            Ammo31,
            Damage32,
            Accuracy33,
            RateOfFire34,
            Range35,
            Power36,
        }

        public enum TrackingType : short
        {
            NoTracking = 0,
            HumanTracking = 1,
            CovenantTracking = 2,
        }

        [Flags]
        public enum WeapMagnificationFlags : uint
        {
            None = 0,
            Bit0 = 1 << 0,
            Bit1 = 1 << 1,
            Bit2 = 1 << 2,
            Bit3 = 1 << 3
        }

        [TagStructure(Size = 0x20)]
        public class FirstPersonBlock : TagStructure
		{
            public CachedTag FirstPersonModel;
            public CachedTag FirstPersonAnimations;
        }

        [TagStructure(Size = 0x80)]
        public class Magazine : TagStructure
		{
            public MagazineFlags Flags;
            public short RoundsRecharged;
            public short RoundsTotalInitial;
            public short RoundsTotalMaximum;
            public short RoundsTotalLoadedMaximum;
            public short MaximumRoundsHeld;
            public short Unknown;
            public float ReloadDialogTime;
            public short RoundsReloaded;
            public short Unknown2;
            public float ChamberTime;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;
            public uint Unknown7;
            public uint Unknown8;
            public CachedTag ReloadingEffect;
            public CachedTag ReloadingDamageEffect;
            public CachedTag ChamberingEffect;
            public CachedTag ChamberingDamageEffect;
            public List<MagazineEquipmentBlock> MagazineEquipment;

            [TagStructure(Size = 0x14)]
            public class MagazineEquipmentBlock : TagStructure
			{
                public short Rounds0ForMax;
                public short Unknown;
                public CachedTag Equipment;
            }

            [Flags]
            public enum MagazineFlags : uint
            {
                None = 0,
                WastesRoundsWhenReloaded = 1 << 0,
                EveryRoundMustBeChambered = 1 << 1,
                Bit2 = 1 << 2,
                Bit3 = 1 << 3
            }
        }

        [TagStructure(Size = 0x90)]
        public class Trigger : TagStructure
		{
            public TriggerFlags Flags;
            public ButtonUsedValue ButtonUsed;
            public BehaviorValue Behavior;
            public short PrimaryBarrel;
            public short SecondaryBarrel;
            public PredictionValue Prediction;
            public short Unknown;
            public float AutofireTime;
            public float AutofireThrow;
            public SecondaryActionValue SecondaryAction;
            public PrimaryActionValue PrimaryAction;
            public float ChargingTime;
            public float ChargedTime;
            public OverchargeActionValue OverchargeAction;
            public TriggerChargingFlags ChargeFlags;
            public float ChargedIlluminationStrength;
            public float SpewTime;
            public CachedTag ChargingEffect;
            public CachedTag ChargingDamageEffect;
            public CachedTag ChargingResponse;
            public float ChargingAgeDegeneration;
            public CachedTag DischargeEffect;
            public CachedTag DischargeDamageEffect;
            public uint TargetTrackingDecayTime;
            public uint TargetTrackingLockTime;
            public uint Unknown6;

            public enum TriggerFlags : uint
            {
                None = 0,
                AutofireSingleActionOnly = 1 << 0
            }

            public enum ButtonUsedValue : short
            {
                RightTrigger,
                LeftTrigger,
                MeleeAttack,
                AutomatedFire,
                RightBumper,
            }

            public enum BehaviorValue : short
            {
                Spew,
                Latch,
                LatchAutofire,
                Charge,
                LatchZoom,
                LatchRocketlauncher,
                SpewCharge,
            }

            public enum PredictionValue : short
            {
                None,
                Spew,
                Charge,
            }

            public enum SecondaryActionValue : short
            {
                Fire,
                Charge,
                Track,
                FireOther,
            }

            public enum PrimaryActionValue : short
            {
                Fire,
                Charge,
                Track,
                FireOther,
            }

            public enum OverchargeActionValue : short
            {
                None,
                Explode,
                Discharge,
            }

            [Flags]
            public enum TriggerChargingFlags : ushort
            {
                None = 0,
                CanFireFromPartialCharge = 1 << 0,
                LimitToCurrentRoundsLoaded = 1 << 1,
                WontChargeUnlessTrackedTargetIsValid = 1 << 2,
                Bit3 = 1 << 3,
                Bit4 = 1 << 4,
                Bit5 = 1 << 5,
                Bit6 = 1 << 6,
                Bit7 = 1 << 7
            }
        }

        [TagStructure(Size = 0x134, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x1AC, MinVersion = CacheVersion.HaloOnline106708)]
        public class Barrel : TagStructure
		{
            public BarrelFlags Flags;
            public Bounds<float> RoundsPerSecond;
            public float AccelerationTime;
            public float DecelerationTime;
            public float BarrelSpinScale;
            public float BlurredRateOfFire;
            public Bounds<short> ShotsPerFire;
            public float FireRecoveryTime;
            public float SoftRecoveryFraction;
            public short MagazineIndex;
            public short RoundsPerShot;
            public short MinimumRoundsLoaded;
            public short RoundsBetweenTracers;
            public StringId OptionalBarrelMarkerName;
            public PredictionTypeValue PredictionType;
            public FiringNoiseValue FiringNoise;
            public float ErrorAccelerationTime;
            public float ErrorDecelerationTime;
            public Bounds<float> DamageError;
            public Angle BaseTurningSpeed;
            public Bounds<Angle> DynamicTurningSpeed;
            public float DualErrorAccelerationTime;
            public float DualErrorDecelerationTime;
            public float DualAccelerationRate;
            public float DualDecelerationRate;
            public Angle DualWieldMinimumError;
            public Bounds<Angle> DualWieldErrorAngle;
            public float DualWieldDamageScale;
            public DistributionFunctionValue ProjectileDistributionFunction;
            public short ProjectilesPerShot;
            public Angle ProjectileDistributionAngle;
            public Angle ProjectileMinimumError;
            public Bounds<Angle> ProjectileErrorAngle;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public float ReloadPenalty;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public float SwitchPenalty;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public float ZoomPenalty;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public float JumpPenalty;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public List<FiringPenaltyFunctionBlock> FiringPenaltyFunction;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public List<FiringCrouchedPenaltyFunctionBlock> FiringCrouchedPenaltyFunction;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public List<MovingPenaltyFunctionBlock> MovingPenaltyFunction;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public List<TurningPenaltyFunctionBlock> TurningPenaltyFunction;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public float ErrorAngleMaximumRotation;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public List<DualFiringPenaltyFunctionBlock> DualFiringPenaltyFunction;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public List<DualFiringCrouchedPenaltyFunctionBlock> DualFiringCrouchedPenaltyFunction;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public List<DualMovingPenaltyFunctionBlock> DualMovingPenaltyFunction;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public List<DualTurningPenaltyFunctionBlock> DualTurningPenaltyFunction;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public float DualErrorAngleMaximumRotation;

            public List<FirstPersonOffsetBlock> FirstPersonOffsets;
            public DamageReportingType DamageReportingType;
            public sbyte Unknown3;
            public short Unknown4;
            public CachedTag InitialProjectile;
            public CachedTag TrailingProjectile;
            public CachedTag DamageEffect;
            public CachedTag CrateProjectile;
            public float CrateProjectileSpeed;
            public float EjectionPortRecoveryTime;
            public float IlluminationRecoveryTime;
            public float HeatGeneratedPerRound;
            public float AgeGeneratedPerRoundMp;
            public float AgeGeneratedPerRoundSp;
            public float OverloadTime;
            public Bounds<Angle> AngleChangePerShot;
            public float AngleChangeAccelerationTime;
            public float AngleChangeDecelerationTime;
            public AngleChangeFunctionValue AngleChangeFunction;
            public short Unknown5;
            public float AngleChangeAccelerationRate;
            public float AngleChangeDecelerationRate;
            public float IlluminationRecoveryRate;
            public float EjectionPortRecoveryRate;
            public float RateOfFireAccelerationTime;
            public float RateOfFireDecelerationTime;
            public float ErrorAccelerationRate;
            public float ErrorDecelerationRate;
            public List<FiringEffectBlock> FiringEffects;

            public enum PredictionTypeValue : short
            {
                None,
                Continuous,
                Instant,
            }

            public enum FiringNoiseValue : short
            {
                Silent,
                Medium,
                Loud,
                Shout,
                Quiet,
            }

            public enum DistributionFunctionValue : short
            {
                Point,
                HorizontalFan,
            }

            [TagStructure(Size = 0x14)]
            public class FiringPenaltyFunctionBlock : TagStructure
			{
                public TagFunction Function = new TagFunction { Data = new byte[0] };
            }

            [TagStructure(Size = 0x14)]
            public class FiringCrouchedPenaltyFunctionBlock : TagStructure
			{
                public TagFunction Function = new TagFunction { Data = new byte[0] };
            }

            [TagStructure(Size = 0x14)]
            public class MovingPenaltyFunctionBlock : TagStructure
			{
                public TagFunction Function = new TagFunction { Data = new byte[0] };
            }

            [TagStructure(Size = 0x14)]
            public class TurningPenaltyFunctionBlock : TagStructure
			{
                public TagFunction Function = new TagFunction { Data = new byte[0] };
            }

            [TagStructure(Size = 0x14)]
            public class DualFiringPenaltyFunctionBlock : TagStructure
			{
                public TagFunction Function = new TagFunction { Data = new byte[0] };
            }

            [TagStructure(Size = 0x14)]
            public class DualFiringCrouchedPenaltyFunctionBlock : TagStructure
			{
                public TagFunction Function = new TagFunction { Data = new byte[0] };
            }

            [TagStructure(Size = 0x14)]
            public class DualMovingPenaltyFunctionBlock : TagStructure
			{
                public TagFunction Function = new TagFunction { Data = new byte[0] };
            }

            [TagStructure(Size = 0x14)]
            public class DualTurningPenaltyFunctionBlock : TagStructure
			{
                public TagFunction Function = new TagFunction { Data = new byte[0] };
            }

            public enum AngleChangeFunctionValue : short
            {
                Linear,
                Early,
                VeryEarly,
                Late,
                VeryLate,
                Cosine,
                One,
                Zero,
            }

            [TagStructure(Size = 0xC4)]
            public class FiringEffectBlock : TagStructure
			{
                public Bounds<short> EffectUseCountRange;
                public CachedTag FiringEffect;
                public CachedTag MisfireEffect;
                public CachedTag EmptyEffect;
                public CachedTag SecondaryEffect;
                public CachedTag FiringResponse;
                public CachedTag MisfireResponse;
                public CachedTag EmptyResponse;
                public CachedTag SecondaryResponse;
                public CachedTag RiderFiringResponse;
                public CachedTag RiderMisfireResponse;
                public CachedTag RiderEmptyResponse;
                public CachedTag RiderSecondaryResponse;
            }
        }

        [TagStructure(Size = 0xC)]
        public class FirstPersonOffsetBlock : TagStructure
		{
            public RealVector3d Offset;
        }
    }

    [TagStructure(Size = 0x4)]
    public class BarrelFlags : TagStructure
    {
        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public Halo3Value Halo3;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public HaloOnlineValue HaloOnline;

        [Flags]
        public enum Halo3Value : int
        {
            None = 0,
            TracksFiredProjectile = 1 << 0,
            RandomFiringEffects = 1 << 1,
            CanFireWithPartialAmmo = 1 << 2,
            ProjectilesUseWeaponOrigin = 1 << 3,
            EjectsDuringChamber = 1 << 4,
            UseErrorWhenUnzoomed = 1 << 5,
            ProjectileVectorCannotBeAdjusted = 1 << 6,
            ProjectilesHaveIdenticalError = 1 << 7,
            ProjectilesFireParallel = 1 << 8,
            CantFireWhenOthersFiring = 1 << 9,
            CantFireWhenOthersRecovering = 1 << 10,
            DontClearFireBitAfterRecovering = 1 << 11,
            StaggerFireAcrossMultipleMarkers = 1 << 12,
            FiresLockedProjectiles = 1 << 13,
            PlasmaPistol = 1 << 14,
            ShrineDefender = 1 << 15,
            HornetWeapons = 1 << 16,
            Bit17 = 1 << 17,
            ProjectileFiresInMarkerDirection = 1 << 18,
            Bit19 = 1 << 19,
            Bit20 = 1 << 20
        }

        [Flags]
        public enum HaloOnlineValue : int
        {
            None = 0,
            TracksFiredProjectile = 1 << 0,
            RandomFiringEffects = 1 << 1,
            CanFireWithPartialAmmo = 1 << 2,
            ProjectilesUseWeaponOrigin = 1 << 3,
            EjectsDuringChamber = 1 << 4,
            UseErrorWhenUnzoomed = 1 << 5,
            ProjectileVectorCannotBeAdjusted = 1 << 6,
            ProjectilesHaveIdenticalError = 1 << 7,
            ProjectilesFireParallel = 1 << 8,
            CantFireWhenOthersFiring = 1 << 9,
            CantFireWhenOthersRecovering = 1 << 10,
            DontClearFireBitAfterRecovering = 1 << 11,
            StaggerFireAcrossMultipleMarkers = 1 << 12,
            PlasmaPistol = 1 << 13,
            ShrineDefender = 1 << 14,
            HornetWeapons = 1 << 15,
            Bit16 = 1 << 16,
            Bit17 = 1 << 17,
            Bit18 = 1 << 18,
            ProjectileFiresInMarkerDirection = 1 << 19,
            Bit20 = 1 << 20
        }
    }

    [TagStructure(Size = 0x4)]
	public class WeaponFlags : TagStructure
	{
        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public OldWeaponFlags OldFlags;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public NewWeaponFlags NewFlags;

        [Flags]
        public enum OldWeaponFlags : int
        {
            None = 0,
            VerticalHeatDisplay = 1 << 0,
            MutuallyExclusiveTriggers = 1 << 1,
            AttacksAutomaticallyOnBump = 1 << 2,
            MustBeReadied = 1 << 3,
            DoesNotCountTowardsMaximum = 1 << 4,
            AimAssistsOnlyWhenZoomed = 1 << 5,
            PreventsGrenadeThrowing = 1 << 6,
            MustBePickedUp = 1 << 7,
            HoldsTriggersWhenDropped = 1 << 8,
            PreventsMeleeAttack = 1 << 9,
            DetonatesWhenDropped = 1 << 10,
            CannotFireAtMaximumAge = 1 << 11,
            SecondaryTriggerOverridesGrenades = 1 << 12,
            SupportWeapon = 1 << 13,
            HideFPWeaponWhenInIronSights = 1 << 14,
            AIsUseWeaponMeleeDamage = 1 << 15,
            PreventsBinoculars = 1 << 16,
            LoopFPFiringAnimation = 1 << 17,
            PreventsCrouching = 1 << 18,
            CannotFireWhileBoosting = 1 << 19,
            UsesEmptyMeleeOnEmpty = 1 << 20,
            ThirdPersonCamera = 1 << 21,
            CanBeDualWielded = 1 << 22,
            CanOnlyBeDualWielded = 1 << 23,
            MeleeOnly = 1 << 24,
            CannotFireIfParentDead = 1 << 25,
            WeaponAgesWithEachKill = 1 << 26,
            WeaponUsesOldDualFireErrorCode = 1 << 27, // removed in later games
            AllowsUnaimedLunge = 1 << 28,
            CannotBeUsedByPlayer = 1 << 29,
        }

        [Flags]
        public enum NewWeaponFlags : int
        {
            None = 0,
            VerticalHeatDisplay = 1 << 0,
            MutuallyExclusiveTriggers = 1 << 1,
            AttacksAutomaticallyOnBump = 1 << 2,
            MustBeReadied = 1 << 3,
            DoesNotCountTowardsMaximum = 1 << 4,
            AimAssistsOnlyWhenZoomed = 1 << 5,
            PreventsGrenadeThrowing = 1 << 6,
            MustBePickedUp = 1 << 7,
            HoldsTriggersWhenDropped = 1 << 8,
            PreventsMeleeAttack = 1 << 9,
            DetonatesWhenDropped = 1 << 10,
            CannotFireAtMaximumAge = 1 << 11,
            SecondaryTriggerOverridesGrenades = 1 << 12,
            SupportWeapon = 1 << 13,
            HideFPWeaponWhenInIronSights = 1 << 14,
            AIsUseWeaponMeleeDamage = 1 << 15,
            PreventsBinoculars = 1 << 16,
            LoopFPFiringAnimation = 1 << 17,
            PreventsCrouching = 1 << 18,
            CannotFireWhileBoosting = 1 << 19,
            UsesEmptyMeleeOnEmpty = 1 << 20,
            ThirdPersonCamera = 1 << 21,
            CanBeDualWielded = 1 << 22,
            CanOnlyBeDualWielded = 1 << 23,
            MeleeOnly = 1 << 24,
            CannotFireIfParentDead = 1 << 25,
            WeaponAgesWithEachKill = 1 << 26,
            AllowsUnaimedLunge = 1 << 27,
            CannotBeUsedByPlayer = 1 << 28,
        }
    }

    [Flags]
    public enum SecondaryWeaponFlags : uint
    {
        None = 0,
        Bit0 = 1 << 0,
        Bit1 = 1 << 1,
        Bit2 = 1 << 2,
        Bit3 = 1 << 3,
        Bit4 = 1 << 4,
        Bit5 = 1 << 5,
        Bit6 = 1 << 6,
        Bit7 = 1 << 7
    }
}