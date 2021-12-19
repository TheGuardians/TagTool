using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using TagTool.Damage;
using System;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "weapon", Tag = "weap", Size = 0x354, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
    [TagStructure(Name = "weapon", Tag = "weap", Size = 0x358, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
    [TagStructure(Name = "weapon", Tag = "weap", Size = 0x384, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline449175, Platform = CachePlatform.Original)]
    [TagStructure(Name = "weapon", Tag = "weap", Size = 0x390, MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
    [TagStructure(Name = "weapon", Tag = "weap", Size = 0x2CC, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original)]
    [TagStructure(Name = "weapon", Tag = "weap", Size = 0x50C, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    public class Weapon : Item
    {
        public WeaponFlags WeaponFlags;
        public SecondaryWeaponFlags SecondaryWeaponFlags;
        public StringId UnusedLabel;
        public SecondaryTriggerModeValue SecondaryTriggerMode;
        public short MaximumAlternateShotsLoaded;
        public float TurnOnTime;
        // -------- old obsolete export to functions
        public float ReadyTime;
        public CachedTag ReadyEffect;
        public CachedTag ReadyDamageEffect;

        // -------- heat
        // the heat value a weapon must return to before leaving the overheated state, once it has become overheated in the first place
        public float HeatRecoveryThreshold; // [0,1]
        // the heat value over which a weapon first becomes overheated (should be greater than the heat recovery threshold)
        public float OverheatedThreshold; // [0,1]
        // the heat value above which the weapon has a chance of exploding each time it is fired
        public float HeatDetonationThreshold; // [0,1]
        // the percent chance (between 0.0 and 1.0) the weapon will explode when fired over the heat detonation threshold
        public float HeatDetonationFraction; // [0,1]
        // the amount of heat lost each second when the weapon is not being fired
        public float HeatLossPerSecond; // [0,1]
        // function values sets the current heat loss per second
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public StringId HeatLoss;
        // function value sets the heat loss per second while weapon is being vented
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public StringId HeatLossVenting;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float HeatVentingTime; // seconds
        // heat at which to begin the venting exit animations so that the weapon is just about fully cooled when the exit
        // animation completes.
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float HeatVentingExitHeat;
        // the amount of illumination given off when the weapon is overheated
        public float HeatIllumination; // [0,1]
        // the amount of heat at which a warning will be displayed on the hud
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float HeatWarningThreshold;
        // the amount of heat lost each second when the weapon is not being fired
        public float OverheatedHeatLossPerSecond; // [0,1]
        // function values sets the heat loss per second when weapon is overheated
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public StringId OverheatedHeatLoss;
        public CachedTag Overheated;
        public CachedTag OverheatedDamageEffect;
        public CachedTag Detonation;
        public CachedTag DetonationDamageEffect2;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag PlayerMeleeDamage;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag PlayerMeleeResponse;

        [TagField(MinVersion = CacheVersion.HaloOnline700123, MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown27;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public MeleeDamageParametersStruct MeleeDamageParameters;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<MeleeDamageParametersStruct> MeleeDamageParametersReach;

        public CachedTag ClangEffect;

        public DamageReportingType MeleeDamageReportingType;
        [TagField(Length = 1, Flags = TagFieldFlags.Padding)]
        public byte[] Padding4;

        // -------- zoom
        // the number of magnification levels this weapon allows
        public short MagnificationLevels;
        public Bounds<float> MagnificationRange;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public WeaponMagnificationFlags MagnificationFlags;
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public float WeaponSwitchReadySpeed; // 0 default

        public AimAssistStruct WeaponAimAssist;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public uint Unknown15;
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public List<Unit.TargetTrackingBlock> TargetTracking;

        // ballistics ?
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown16;
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown17;
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown18;
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown19;

        // At the min range (or closer), the minimum ballistic arcing is used, at the max (or farther away), the maximum
        // arcing is used
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public Bounds<float> BallisticArcingFiringBounds; // world units
        // Controls speed and degree of arc. 0 = low, fast, 1 = high, slow
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public Bounds<float> BallisticArcingFractionBounds; // [0-1]

        public MovementPenaltyModes MovementPenalized;
        [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
        public byte[] Paddng5;
        // percent slowdown to forward movement for units carrying this weapon
        public float ForwardsMovementPenalty;
        // percent slowdown to sideways and backward movement for units carrying this weapon
        public float SidewaysMovementPenalty;

        public float AiScariness;

        public float WeaponPowerOnTime; // seconds
        public float WeaponPowerOffTime; // seconds
        public CachedTag WeaponPowerOnEffect;
        public CachedTag WeaponPowerOffEffect;

        // how much the weapon's heat recovery is penalized as it ages
        public float AgeHeatRecoveryPenalty;
        // how much the weapon's rate of fire is penalized as it ages
        public float AgeRateOfFirePenalty;
        // the age threshold when the weapon begins to misfire
        public float AgeMisfireStart; // [0,1]
        // at age 1.0, the misfire chance per shot
        public float AgeMisfireChance; // [0,1]

        public CachedTag PickupSound;
        public CachedTag ZoomInSound;
        public CachedTag ZoomOutSound;

        // how much to decrease active camo when a round is fired
        public float ActiveCamoDing;
        // how fast to increase active camo (per tick) when a round is fired
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float ActiveCamoRegrowthRate;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown22; // HandleNode ?
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown23;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public StringId HandleNode;

        // -------- weapon labels
        public StringId WeaponClass;
        public StringId WeaponName;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public MultiplayerWeaponTypeValue MultiplayerWeaponType;
        public WeaponTypeValue WeaponType;

        public TrackingType Tracking;
        [TagField(Length = 2, Flags = TagFieldFlags.Padding, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] Padding5;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public SpecialHudVersionValue SpecialHudVersion;
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public int SpecialHudIcon;

        // -------- interface
        [TagField(Flags = Padding, Length = 16, MaxVersion = CacheVersion.Halo3ODST)]
        [TagField(Flags = Padding, Length = 16, MinVersion = CacheVersion.HaloReach)]
        public byte[] SharedInterface;
        public List<FirstPersonBlock> FirstPerson;
        public CachedTag HudInterface;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public CachedTag AlternateHudInterface;

        public List<PredictedResource> PredictedWeaponResources;
        public List<Magazine> Magazines;
        public List<Trigger> Triggers;
        public List<Barrel> Barrels;
        public float WeaponPowerOnVelocity;
        public float WeaponPowerOffVelocity;

        // -------- first-person movement control
        public float MaxMovementAcceleration;
        public float MaximumMovementVelocity;
        public float MaximumTurningAcceleration;
        public float MaximumTurningVelocity;

        [TagField(ValidTags = new[] { "vehi" })]
        public CachedTag DeployedVehicle;
        [TagField(ValidTags = new[] { "weap" })]
        public CachedTag DeployedWeapon;

        [TagField(ValidTags = new[] { "effe" })]
        public CachedTag AgeEffect;
        [TagField(ValidTags = new[] { "weap" })]
        public CachedTag AgedWeapon;
        [TagField(ValidTags = new[] { "foot" })]
        public CachedTag AgedMaterialEffects;
        public float ExternalAgingAmount;
        public uint CampaignExternalAgingAmount;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float ExternalHeatAmount;

        public RealVector3d FirstPersonWeaponOffset;
        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        public RealVector3d CenteredFirstPersonWeaponOffset;
        [TagField(MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<FirstPersonOffsetBlock> NewFirstPersonWeaponOffsets;
        public RealVector2d FirstPersonScopeSize;
        // range in degrees. 0 is straight, -90 is down, 90 is up
        public Bounds<float> SupportThirdPersonCameraRange;  // degrees
        public float WeaponZoomTime; // seconds
        public float WeaponReadyForUseTime; // seconds
        // e.g. - 2.0 makes playspeed twice as fast
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        public float WeaponReadyFirstPersonAnimationPlaybackScale;
        public StringId UnitStowAnchorName;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<WeaponScreenEffectBlock> ScreenEffects;

        public enum SecondaryTriggerModeValue : short
        {
            Normal,
            SlavedToPrimary,
            InhibitsPrimary,
            LoadsAlternateAmmunition,
            LoadsMultiplePrimaryAmmunition,
        }

        public enum MovementPenaltyModes : short
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
        public enum WeaponMagnificationFlags : uint
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
            public short RoundsLoadedMaximum;
            public short RoundsInventoryMaximum;
            [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            // the length of time it takes to load a single magazine into the weapon
            public float ReloadTime;  // seconds
            public short RoundsReloaded;
            [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            // the length of time it takes to chamber the next round
            public float ChamberTime; // seconds
            [TagField(Length = 8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            [TagField(Length = 16, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
            public CachedTag ReloadingEffect;
            public CachedTag ReloadingDamageEffect;
            public CachedTag ChamberingEffect;
            public CachedTag ChamberingDamageEffect;
            public List<MagazineEquipmentBlock> MagazineEquipment;

            [TagStructure(Size = 0x14)]
            public class MagazineEquipmentBlock : TagStructure
            {
                // 0 for max
                public short Rounds;
                [TagField(Length = 1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                [TagField(ValidTags = new[] { "eqip" })]
                public CachedTag Equipment;
            }

            [Flags]
            public enum MagazineFlags : uint
            {
                None = 0,
                WastesRoundsWhenReloaded = 1 << 0,
                EveryRoundMustBeChambered = 1 << 1,
                // will prevent reload until fire is complete (sticky det)
                MagazineCannotChangeStateWhileFiring = 1 << 2,
                AllowOverheatedReloadWhenEmpty = 1 << 3,
                BottomlessInventory = 1 << 4
            }
        }

        [TagStructure(Size = 0x90, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x8C, MinVersion = CacheVersion.HaloReach)]
        public class Trigger : TagStructure
        {
            public TriggerFlags Flags;
            public InputValue Input;
            public BehaviorValue Behavior;
            public short PrimaryBarrel;
            public short SecondaryBarrel;
            public PredictionValue Prediction;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public AutofireStruct Autofire;
            public ChargingStruct Charging;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float LockonHoldTime;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float LockonAcquireTime;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float LockonGraceTime;

            [TagStructure(Size = 0xC)]
            public class AutofireStruct : TagStructure
            {
                public float AutofireTime;
                public float AutofireThrow;
                public AutofireActionValue SecondaryAction;
                public AutofireActionValue PrimaryAction;

                public enum AutofireActionValue : short
                {
                    Fire,
                    Charge,
                    Track,
                    FireOther
                }
            }

            [TagStructure(Size = 0x68, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x70, MinVersion = CacheVersion.HaloReach)]
            public class ChargingStruct : TagStructure
            {
                // the amount of time it takes for this trigger to become fully charged
                public float ChargingTime; // seconds
                // the amount of time this trigger can be charged before becoming overcharged
                public float ChargedTime; // seconds
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public OverchargedActionValue OverchargedAction;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public OverchargedActionValueReach OverchargedActionReach;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public ChargingFlagsValue Flags;
                // 96 was the constant in code for the pp
                public short CancelledTriggerThrow;
                // the amount of illumination given off when the weapon is fully charged
                public float ChargedIllumination; // [0,1]
                // length of time the weapon will spew (fire continuously) while discharging
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float SpewTime; // seconds
                // the charging effect is created once when the trigger begins to charge
                public CachedTag ChargingEffect;
                // the charging effect is created once when the trigger begins to charge
                public CachedTag ChargingDamageEffect;
                // plays every tick you're charging or charged, scaled to charging fraction
                public CachedTag ChargingContinuousDamageResponse;
                // how much battery to drain per second when charged
                public float ChargedDrainRate;
                // the discharging effect is created once when the trigger releases its charge
                public CachedTag DischargeEffect;
                // the discharging effect is created once when the trigger releases its charge
                public CachedTag DischargeDamageEffect;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public List<WeaponTriggerChargingFireFraction> FireFractions;

                public enum OverchargedActionValue : short
                {
                    None,
                    Explode,
                    Discharge
                }

                public enum OverchargedActionValueReach : sbyte
                {
                    None,
                    Explode,
                    Discharge
                }

                [Flags]
                public enum ChargingFlagsValue : byte
                {
                    // discharging a partially charged weapon will spew for the charged fraction of the spew time set below
                    CanFireFromPartialCharge = 1 << 0,
                    // if magazine present, do not fire more than current rounds loaded (mantis rocket launcher)
                    LimitToCurrentRoundsLoaded = 1 << 1,
                    // spew-charge triggers only
                    WontChargeUnlessTrackedTargetIsValid = 1 << 2
                }

                [TagStructure(Size = 0x4)]
                public class WeaponTriggerChargingFireFraction : TagStructure
                {
                    // charging fraction at which the weapon should additionally fire a shot.
                    public float ChargeFraction; // [0.1]
                }
            }

            public enum TriggerFlags : uint
            {
                None = 0,
                AutofireSingleActionOnly = 1 << 0
            }

            public enum InputValue : short
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
        }

        [TagStructure(Size = 0x134, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x1AC, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x184, MinVersion = CacheVersion.HaloReach)]
        public class Barrel : TagStructure
        {
            public BarrelFlags Flags;
            public WeaponBarrelFiringParametersStruct Firing;
            public short MagazineIndex;
            public short RoundsPerShot;
            public short MinimumRoundsLoaded;
            public short RoundsBetweenTracers;
            public StringId OptionalBarrelMarkerName;

            // -------- prediction properties
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public PredictionTypeValue PredictionType;
            // how loud this weapon appears to the AI
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public FiringNoiseValue FiringNoise;
 
            // how loud this weapon appears to the AI
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public FiringNoiseValue FiringNoiseReach;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public PredictionTypeValue PredictionTypeReach;
            // Valid only for barrels set to prediction type "continuous". Controls how many projectiles per second can be
            // individually synchronized (use debug_projectiles to diagnose).
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float EventSynchronizedProjectilesPerSecond;
            // Valid only for barrels set to prediction type "continuous". If the barrel's current error level is over this value
            // (zero to one scale), we will not consider synchronizing projectiles with individual events (use debug_projectiles
            // to diagnose).
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float MaximumBarrelErrorForEventSynchronization;

            public FiringErrorStruct FiringError;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public DualWeaponErrorStruct DualWeaponError;
            public DistributionFunctionValue ProjectileDistributionFunction;
            public short ProjectilesPerShot;
            public Angle ProjectileDistributionAngle;
            public Angle ProjectileMinimumError;
            public Bounds<Angle> ProjectileErrorAngle;
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public ProjectileAccuracyPenaltyStruct AccuracyPenalties;
            public List<FirstPersonOffsetBlock> FirstPersonOffsets;
            public DamageReportingType DamageReportingType;
            [TagField(Length = 3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public CachedTag Projectile;
            public CachedTag OptionalSecondaryProjectile;
            public CachedTag DamageEffect;
            public CachedTag CrateProjectile;
            public float CrateProjectileSpeed;
            // the amount of time (in seconds) it takes for the ejection port to transition from 1.0 (open) to 0.0 (closed) after a shot has been fired
            public float EjectionPortRecoveryTime;
            // the amount of time (in seconds) it takes the illumination function to transition from 1.0 (bright) to 0.0 (dark) after a shot has been fired
            public float IlluminationRecoveryTime;
            // the amount of heat generated each time the trigger is fired
            public float HeatGeneratedPerRound;
            // function value sets the amount of heat to add to the weapon each tick the barrel is firing
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public StringId HeatGeneratedPerRoundFunction;
            // the amount the weapon ages each time the trigger is fired
            public float AgeGeneratedPerRoundMp;
            // the amount the weapon ages each time the trigger is fired
            public float AgeGeneratedPerRoundSp;
            // the next trigger fires this often while holding down this trigger
            public float OverloadTime;
           
            // -------- angle change (recoil)
            // angle change per shot of the weapon during firing
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public Bounds<Angle> AngleChangePerShot;
            // the continuous firing time it takes for the weapon to achieve its final angle change per shot
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float AngleChangeAccelerationTime;
            // the continuous idle time it takes for the weapon to return to its initial angle change per shot
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float AngleChangeDecelerationTime;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public AngleChangeFunctionValue AngleChangeFunction;
            [TagField(Length = 2, Flags = TagFieldFlags.Padding, MaxVersion = CacheVersion.HaloOnline700123)]
            public byte[] Padding2;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float AngleChangeAccelerationRate;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float AngleChangeDecelerationRate;

            public float IlluminationRecoveryRate;
            public float EjectionPortRecoveryRate;
            public float RateOfFireAccelerationTime;
            public float RateOfFireDecelerationTime;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float ErrorAccelerationRate;
            public float ErrorDecelerationRate;

            // firing effects determine what happens when this trigger is fired
            public List<FiringEffectBlock> FiringEffects;

            [TagStructure(Size = 0x24, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x34, MinVersion = CacheVersion.HaloReach)]
            public class WeaponBarrelFiringParametersStruct : TagStructure
            {
                // the number of firing effects created per second
                public Bounds<float> RoundsPerSecond;
                // function value sets the current rate of fire when the barrel is firing
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public StringId RateOfFireAcceleration;
                // the continuous firing time it takes for the weapon to achieve its final rounds per second
                public float AccelerationTime; // seconds
                // function value sets the current rate of fire when the barrel is not firing
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public StringId RateOfFireDeceleration;
                // the continuous idle time it takes for the weapon to return from its final rounds per second to its initial
                public float DecelerationTime; // seconds
                // scale the barrel spin speed by this amount
                public float BarrelSpinScale;
                // a percentage between 0 and 1 which controls how soon in its firing animation the weapon blurs
                public float BlurredRateOfFire;
                // allows designer caps to the shots you can fire from one firing action
                public Bounds<short> ShotsPerFire;
                // how long after a set of shots it takes before the barrel can fire again
                public float FireRecoveryTime; // seconds
                // how much of the recovery allows shots to be queued
                public float SoftRecoveryFraction;
                // how long after a set of shots it takes before the weapon can melee
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public float MeleeFireRecoveryTime; // seconds
                // how much of the melee recovery allows melee to be queued
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public float MeleeSoftRecoveryFraction;
            }

            [TagStructure(Size = 0x1C, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x18, MinVersion = CacheVersion.HaloReach)]
            public class FiringErrorStruct : TagStructure
            {
                // the continuous firing time it takes for the weapon to achieve its final error
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float AccelerationTime; // seconds
                // the continuous idle time it takes for the weapon to return to its initial error
                public float DecelerationTime; // seconds
                // the range of angles (in degrees) that a damaged weapon will skew fire
                public Bounds<float> DamageError;
                // yaw rate is doubled
                public Angle MinErrorLookPitchRate;
                // yaw rate is doubled
                public Angle FullErrorLookPitchRate;
                // use to soften or sharpen the rate ding
                public float LookPitchErrorPower;
            }

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

            [TagStructure(Size = 0x20)]
            public class DualWeaponErrorStruct : TagStructure
            {
                // the continuous firing time it takes for the weapon to achieve its final error
                public float AccelerationTime; // seconds
                // the continuous idle time it takes for the weapon to return to its initial error
                public float DecelerationTime; // seconds
                public float RuntimeAccelerationRate;
                public float RuntimeDecelerationRate;
                public Angle MinimumError; // degrees
                public Bounds<Angle> ErrorAngle; // degrees
                public float DualWieldDamageScale;
            }

            [TagStructure(Size = 0xC4)]
            public class FiringEffectBlock : TagStructure
            {
                // the minimum and maximum number of times this firing effect will be used, once it has been chosen
                public Bounds<short> ShotCount;
                // this effect is used when the weapon is loaded and fired normally
                public CachedTag FiringEffect;
                // this effect is used when the weapon is loaded but fired while overheated
                public CachedTag MisfireEffect;
                // this effect is used when the weapon is not loaded
                public CachedTag EmptyEffect;
                // this effect is used when the weapon is not loaded
                public CachedTag OptionalSecondaryFiringEffect;
                // this effect is used when the weapon is loaded and fired normally
                public CachedTag FiringDamage;
                // this effect is used when the weapon is loaded but fired while overheated
                public CachedTag MisfireDamage;
                // this effect is used when the weapon is not loaded
                public CachedTag EmptyDamage;
                // this effect is used when the weapon is loaded and fired normally
                public CachedTag OptionalSecondaryFiringDamage;
                // this effect is used when the weapon is loaded and fired normally
                public CachedTag FiringRiderDamage;
                // this effect is used when the weapon is loaded but fired while overheated
                public CachedTag MisfireRiderDamage;
                // this effect is used when the weapon is not loaded
                public CachedTag EmptyRiderDamage;
                // this effect is used when the weapon is loaded and fired normally
                public CachedTag OptionalSecondaryFiringRiderDamage;
            }

            [TagStructure(Size = 0x78)]
            public class ProjectileAccuracyPenaltyStruct : TagStructure
            {
                // percentage accuracy lost when reloading
                public float ReloadPenalty;
                // percentage accuracy lost when switching weapons
                public float SwitchPenalty;
                // percentage accuracy lost when zooming in/out
                public float ZoomPenalty;
                // percentage accuracy lost when jumping
                public float JumpPenalty;
                public PenaltyFunctionStruct SingleWieldPenalties;
                public PenaltyFunctionStruct DualWieldPenalties;

                [TagStructure(Size = 0x34)]
                public class PenaltyFunctionStruct : TagStructure
                {
                    // percentage accuracy lost when the barrel has fired
                    public List<FunctionBlock> FiringPenaltyFunction;
                    // percentage accuracy lost when the barrel has fired from a crouched position
                    public List<FunctionBlock> FiringCrouchedPenaltyFunction;
                    // percentage accuracy lost when moving
                    public List<FunctionBlock> MovingPenaltyFunction;
                    // percentage accuracy lost when turning the camera
                    public List<FunctionBlock> TurningPenaltyFunction;
                    // angle which represents the maximum input to the turning penalty function.
                    public float ErrorAngleMaxRotation;

                    [TagStructure(Size = 0x14)]
                    public class FunctionBlock : TagStructure
                    {
                        public TagFunction Function = new TagFunction { Data = new byte[0] };
                    }
                }
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

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public HaloOnlineValue HaloOnline;

        [Flags]
        public enum Halo3Value : int
        {
            None = 0,
            // poo poo ca ca pee pee
            TracksFiredProjectile = 1 << 0,
            // rather than being chosen sequentially, firing effects are picked randomly
            RandomFiringEffects = 1 << 1,
            // allows a weapon to be fired as long as there is a non-zero amount of ammunition loaded
            CanFireWithPartialAmmo = 1 << 2,
            // instead of coming out of the magic first person camera origin, the projectiles for this weapon actually come out of the gun
            ProjectilesUseWeaponOrigin = 1 << 3,
            // this trigger's ejection port is started during the key frame of its chamber animation
            EjectsDuringChamber = 1 << 4,
            UseErrorWhenUnzoomed = 1 << 5,
            // projectiles fired by this weapon cannot have their direction adjusted by the AI to hit the target
            ProjectileVectorCannotBeAdjusted = 1 << 6,
            ProjectilesHaveIdenticalError = 1 << 7,
            // If there are multiple guns for this trigger, the projectiles emerge in parallel beams (rather than independant aiming)
            ProjectilesFireParallel = 1 << 8,
            CantFireWhenOthersFiring = 1 << 9,
            CantFireWhenOthersRecovering = 1 << 10,
            DontClearFireBitAfterRecovering = 1 << 11,
            StaggerFireAcrossMultipleMarkers = 1 << 12,
            FiresLockedProjectiles = 1 << 13,
            CanFireAtMaximumAge = 1 << 14,
            Use1FiringEffectPerBurst = 1 << 15,
            IgnoreTrackedObject = 1 << 16,
            Bit17 = 1 << 17,
            ProjectileFiresInMarkerDirection = 1 << 18,
            Bit19 = 1 << 19,
            Bit20 = 1 << 20
        }

        [Flags]
        public enum HaloOnlineValue : int
        {
            None = 0,
            // poo poo ca ca pee pee
            TracksFiredProjectile = 1 << 0,
            // rather than being chosen sequentially, firing effects are picked randomly
            RandomFiringEffects = 1 << 1,
            // allows a weapon to be fired as long as there is a non-zero amount of ammunition loaded
            CanFireWithPartialAmmo = 1 << 2,
            // instead of coming out of the magic first person camera origin, the projectiles for this weapon actually come out of the gun
            ProjectilesUseWeaponOrigin = 1 << 3,
            // this trigger's ejection port is started during the key frame of its chamber animation
            EjectsDuringChamber = 1 << 4,
            UseErrorWhenUnzoomed = 1 << 5,
            // projectiles fired by this weapon cannot have their direction adjusted by the AI to hit the target
            ProjectileVectorCannotBeAdjusted = 1 << 6,
            ProjectilesHaveIdenticalError = 1 << 7,
            // If there are multiple guns for this trigger, the projectiles emerge in parallel beams (rather than independant aiming)
            ProjectilesFireParallel = 1 << 8,
            CantFireWhenOthersFiring = 1 << 9,
            CantFireWhenOthersRecovering = 1 << 10,
            DontClearFireBitAfterRecovering = 1 << 11,
            StaggerFireAcrossMultipleMarkers = 1 << 12,
            CanFireAtMaximumAge = 1 << 13,
            Use1FiringEffectPerBurst = 1 << 14,
            IgnoreTrackedObject = 1 << 15,
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

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
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
        MagnitizesOnlyWhenZoomed = 1 << 0,
        ForceEnableEquipmentTossing = 1 << 1,
        // melee-physics dash is disabled on melees that are not lunges
        NonlungeMeleeDashDisabled = 1 << 2
    }

    [TagStructure(Size = 0xCC, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0xC8, MinVersion = CacheVersion.HaloReach)]
    public class MeleeDamageParametersStruct : TagStructure
    {
        public RealEulerAngles2d DamagePyramidAngles;
        public float DamagePyramidDepth;

        // 0 defaults to 1.22f
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float MaximumLungeRange; // wu
        // the distance out from the pyramid center to spawn explosive effects.  This value will be clamped to the damage
        // pyramid depth. 0 defaults to the damage pyramid depth
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float DamageLungeExplosiveDepth; // wu
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float RuntimeDamageLungeExplosiveFraction;

        // -------- melee combo damage
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag FirstHitMeleeDamage;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag FirstHitMeleeResponse;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag SecondHitMeleeDamage;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag SecondHitMeleeResponse;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag ThirdHitMeleeDamage;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag ThirdHitMeleeResponse;

        // this is only important for the energy sword
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public CachedTag MeleeDamage;
        // this is only important for the energy sword
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public CachedTag MeleeResponse;

        // this is only important for the energy sword
        public CachedTag LungeMeleeDamage;
        // this is only important for the energy sword
        public CachedTag LungeMeleeResponse;
        // this is only important for the energy sword
        public CachedTag EmptyMeleeDamage;
        // this is only important for the energy sword
        public CachedTag EmptyMeleeResponse;
        // this is only important for the energy sword
        public CachedTag ClangMeleeDamage;
        // this is only important for the energy sword
        public CachedTag ClangMeleeResponse;
    }

    [TagStructure(Size = 0x30, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x38, MinVersion = CacheVersion.HaloOnlineED)]
    public class AimAssistStruct : TagStructure
    {
        // the maximum angle that autoaim works at full strength
        public Angle AutoaimAngle; // degrees
        // the maximum distance that autoaim works at full strength
        public float AutoaimRange; // world units
        // at what point the autoaim starts falling off
        public float AutoaimFalloffRange; // world units
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public float AutoaimNearFalloffRange;

        // the maximum angle that magnetism works at full strength
        public Angle MagnetismAngle; // degrees
        // the maximum distance that magnetism works at full strength
        public float MagnetismRange; // world units
        // at what point magnetism starts falling off
        public float MagnetismFalloffRange; // world units
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public float MagnetismNearFalloffRange;

        // the maximum angle that a projectile is allowed to deviate from the gun barrel
        public Angle DeviationAngle; // degrees

        [TagField(Length = 4, Flags = TagFieldFlags.Padding)]
        public byte[] ZHV;
        [TagField(Length = 12, Flags = TagFieldFlags.Padding)]
        public byte[] CVYGPMLMX;
        [TagField(Length = 4, Flags = TagFieldFlags.Padding)]
        public byte[] UQXKLVAXI;
    }

    [TagStructure(Size = 0x14)]
    public class WeaponScreenEffectBlock : TagStructure
    {
        public WeaponScreenEffectFlags Flags;
        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        [TagField(ValidTags = new[] { "sefc" })]
        public CachedTag ScreenEffect;

        [Flags]
        public enum WeaponScreenEffectFlags : byte
        {
            OverridesUnitAndCameraScreenEffects = 1 << 0,
            Unzoomed = 1 << 1,
            ZoomLevel1 = 1 << 2,
            ZoomLevel2 = 1 << 3
        }
    }
}