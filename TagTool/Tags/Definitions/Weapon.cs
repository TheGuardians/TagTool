using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using TagTool.Damage;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "weapon", Tag = "weap", Size = 0x354, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "weapon", Tag = "weap", Size = 0x358, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "weapon", Tag = "weap", Size = 0x384, MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline449175)]
    [TagStructure(Name = "weapon", Tag = "weap", Size = 0x390, MinVersion = CacheVersion.HaloOnline498295)]
    public class Weapon : Item
    {
        public WeaponFlags WeaponFlags;
        public uint MoreFlags;
        public StringId Unknown8;
        public SecondaryTriggerModeValue SecondaryTriggerMode;
        public short MaximumAlternateShotsLoaded;
        public float TurnOnTime;
        public float ReadyTime;

        public CachedTagInstance ReadyEffect;
        public CachedTagInstance ReadyDamageEffect;

        public float HeatRecoveryThreshold;
        public float OverheatedThreshold;
        public float HeatDetonationThreshold;
        public float HeatDetonationFraction;
        public float HeatLossPerSecond;
        public float HeatIllumination;
        public float OverheatedHeatLossPerSecond;

        public CachedTagInstance Overheated;
        public CachedTagInstance OverheatedDamageEffect;
        public CachedTagInstance Detonation;
        public CachedTagInstance DetonationDamageEffect2;
        public CachedTagInstance PlayerMeleeDamage;
        public CachedTagInstance PlayerMeleeResponse;

        [TagField(MinVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown27;

        public Angle DamagePyramidAnglesY;
        public Angle DamagePyramidAnglesP;
        public float DamagePyramidDepth;

        public CachedTagInstance FirstHitDamage;
        public CachedTagInstance FirstHitResponse;
        public CachedTagInstance SeconddHitDamage;
        public CachedTagInstance SecondHitResponse;
        public CachedTagInstance ThirdHitDamage;
        public CachedTagInstance ThirdHitResponse;
        public CachedTagInstance LungeMeleeDamage;
        public CachedTagInstance LungeMeleeResponse;
        public CachedTagInstance GunGunClangDamage;
        public CachedTagInstance GunGunClangResponse;
        public CachedTagInstance GunSwordClangDamage;
        public CachedTagInstance GunSwordClangResponse;
        public CachedTagInstance ClashEffect;

        public DamageReportingType MeleeDamageReportingType;
        public sbyte Unknown9;
        public short MagnificationLevels;
        public Bounds<float> MagnificationBounds;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint MagnificationFlags;
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
        public List<TargetTrackingBlock> TargetTracking;
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
        public CachedTagInstance WeaponPowerOnEffect;
        public CachedTagInstance WeaponPowerOffEffect;
        public float AgeHeatRecoveryPenalty;
        public float AgeRateOfFirePenalty;
        public float AgeMisfireStart;
        public float AgeMisfireChance;
        public CachedTagInstance PickupSound;
        public CachedTagInstance ZoomInSound;
        public CachedTagInstance ZoomOutSound;
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
        
        [TagField(Flags = TagFieldFlags.Padding, Length = 16, MaxVersion = CacheVersion.Halo3ODST)]
        public byte[] UnknownBlock;
        
        public List<FirstPersonBlock> FirstPerson;
        public CachedTagInstance HudInterface;
        public List<PredictedResource> PredictedWeaponResources;
        public List<Magazine> Magazines;
        public List<Trigger> Triggers;
        public List<Barrel> Barrels;
        public float Unknown25;
        public float Unknown26;
        public float MaximumMovementAcceleration;
        public float MaximumMovementVelocity;
        public float MaximumTurningAcceleration;
        public float MaximumTurningVelocity;
        public CachedTagInstance DeployedVehicle;
        public CachedTagInstance DeployedWeapon;
        public CachedTagInstance AgeModel;
        public CachedTagInstance AgeWeapon;
        public CachedTagInstance AgedMaterialEffects;
        public float HammerAgePerUse;
        public uint UnknownSwordAgePerUse;
        public RealVector3d FirstPersonWeaponOffset;

        [TagField(MinVersion = CacheVersion.HaloOnline498295)]
        public List<FirstPersonOffsetBlock> NewFirstPersonWeaponOffsets;

        public RealVector2d FirstPersonScopeSize;
        public Bounds<float> ThirdPersonPitchBounds;
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
        
        [TagStructure(Size = 0x38)]
        public class TargetTrackingBlock : TagStructure
		{
            public List<TrackingType> TrackingTypes;
            public float AcquireTime;
            public float GraceTime;
            public float DecayTime;
            public CachedTagInstance TrackingSound;
            public CachedTagInstance LockedSound;

            [TagStructure(Size = 0x4)]
            public class TrackingType : TagStructure
			{
                public StringId TrackingType2;
            }
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
            CovenantTracking =2,
        }

        [TagStructure(Size = 0x20)]
        public class FirstPersonBlock : TagStructure
		{
            public CachedTagInstance FirstPersonModel;
            public CachedTagInstance FirstPersonAnimations;
        }

        [TagStructure(Size = 0x80)]
        public class Magazine : TagStructure
		{
            public uint Flags;
            public short RoundsRecharged;
            public short RoundsTotalInitial;
            public short RoundsTotalMaximum;
            public short RoundsTotalLoadedMaximum;
            public short MaximumRoundsHeld;
            public short Unknown;
            public float ReloadTime;
            public short RoundsReloaded;
            public short Unknown2;
            public float ChamberTime;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;
            public uint Unknown7;
            public uint Unknown8;
            public CachedTagInstance ReloadingEffect;
            public CachedTagInstance ReloadingDamageEffect;
            public CachedTagInstance ChamberingEffect;
            public CachedTagInstance ChamberingDamageEffect;
            public List<MagazineEquipmentBlock> MagazineEquipment;

            [TagStructure(Size = 0x14)]
            public class MagazineEquipmentBlock : TagStructure
			{
                public short Rounds0ForMax;
                public short Unknown;
                public CachedTagInstance Equipment;
            }
        }

        [TagStructure(Size = 0x90)]
        public class Trigger : TagStructure
		{
            public uint Flags;
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
            public ushort ChargeFlags;
            public float ChargedIllumination;
            public float SpewTime;
            public CachedTagInstance ChargingEffect;
            public CachedTagInstance ChargingDamageEffect;
            public CachedTagInstance ChargingResponse;
            public float ChargingAgeDegeneration;
            public CachedTagInstance Unknown2;
            public CachedTagInstance Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;

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
            public short Magazine;
            public short RoundsPerShot;
            public short MinimumRoundsLoaded;
            public short RoundsBetweenTracers;
            public StringId OptionalBarrelMarkerName;
            public PredictionTypeValue PredictionType;
            public FiringNoiseValue FiringNoise;
            public float AccelerationTime2;
            public float DecelerationTime2;
            public Bounds<float> DamageError;
            public Angle BaseTurningSpeed;
            public Bounds<Angle> DynamicTurningSpeed;
            public float AccelerationTime3;
            public float DecelerationTime3;
            public uint Unknown;
            public uint Unknown2;
            public Angle MinimumError;
            public Bounds<Angle> ErrorAngle;
            public float DualWieldDamageScale;
            public DistributionFunctionValue DistributionFunction;
            public short ProjectilesPerShot;
            public Angle DistributionAngle;
            public Angle MinimumError2;
            public Bounds<Angle> ErrorAngle2;

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
            public CachedTagInstance InitialProjectile;
            public CachedTagInstance TrailingProjectile;
            public CachedTagInstance DamageEffect;
            public CachedTagInstance CrateProjectile;
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
            public uint Unknown6;
            public uint Unknown7;
            public float FiringEffectDecelerationTime;
            public uint Unknown8;
            public float RateOfFireAccelerationTime;
            public float RateOfFireDecelerationTime;
            public uint Unknown9;
            public float BloomRateOfDecay;
            public List<FiringEffect> FiringEffects;            

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
            public class FiringEffect : TagStructure
			{
                public short ShotCountLowerBound;
                public short ShotCountUpperBound;
                public CachedTagInstance FiringEffect2;
                public CachedTagInstance MisfireEffect;
                public CachedTagInstance EmptyEffect;
                public CachedTagInstance UnknownEffect;
                public CachedTagInstance FiringResponse;
                public CachedTagInstance MisfireResponse;
                public CachedTagInstance EmptyResponse;
                public CachedTagInstance UnknownResponse;
                public CachedTagInstance RiderFiringResponse;
                public CachedTagInstance RiderMisfireResponse;
                public CachedTagInstance RiderEmptyResponse;
                public CachedTagInstance RiderUnknownResponse;
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
            DoesNotDepowerActiveCamoInMultiplayer = 1 << 13,
            EnablesIntegratedNightVision = 1 << 14,
            AIsUseWeaponMeleeDamage = 1 << 15,
            ForcesNoBinoculars = 1 << 16,
            LoopFPFiringAnimation = 1 << 17,
            PreventsSprinting = 1 << 18,
            CannotFireWhileBoosting = 1 << 19,
            PreventsDriving = 1 << 20,
            ThirdPersonCamera = 1 << 21,
            CanBeDualWielded = 1 << 22,
            CanOnlyBeDualWielded = 1 << 23,
            MeleeOnly = 1 << 24,
            CannotFireIfParentDead = 1 << 25,
            WeaponAgesWithEachKill = 1 << 26,
            WeaponUsesOldDualFireErrorCode = 1 << 27, // removed in later games
            PrimaryTriggerMeleeAttacks = 1 << 28,
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
            DoesNotDepowerActiveCamoInMultiplayer = 1 << 13,
            EnablesIntegratedNightVision = 1 << 14,
            AIsUseWeaponMeleeDamage = 1 << 15,
            ForcesNoBinoculars = 1 << 16,
            LoopFPFiringAnimation = 1 << 17,
            PreventsSprinting = 1 << 18,
            CannotFireWhileBoosting = 1 << 19,
            PreventsDriving = 1 << 20,
            ThirdPersonCamera = 1 << 21,
            CanBeDualWielded = 1 << 22,
            CanOnlyBeDualWielded = 1 << 23,
            MeleeOnly = 1 << 24,
            CannotFireIfParentDead = 1 << 25,
            WeaponAgesWithEachKill = 1 << 26,
            PrimaryTriggerMeleeAttacks = 1 << 27,
            CannotBeUsedByPlayer = 1 << 28,
        }
    }
}