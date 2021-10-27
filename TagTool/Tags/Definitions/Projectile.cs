using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using TagTool.Damage;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "projectile", Tag = "proj", Size = 0x1A8, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "projectile", Tag = "proj", Size = 0x1AC, MinVersion = CacheVersion.HaloOnlineED)]
    public class Projectile : GameObject
    {
        public ProjectileFlags Flags;
        public DetonationTimerModes DetonationTimerStarts;
        public AiSoundVolume ImpactVolumeForAi;
        public float CollisionRadius; // world units
        public float ArmingTime; // won't detonate before this time elapses (seconds)
        public float DangerRadius; // world units
        public Bounds<float> Timer; // detonation countdown (seconds, zero is untimed)
        public float MinimumVelocity; // detonates when slowed below this velocity (world units per second)
        public float MaximumRange; // detonates after traveling this distance (world units)
        public float BounceMaximumRange; // detonates after this distance, but is reset after a bounce. Combines with maximum range

        [TagField(MinVersion = CacheVersion.HaloOnline498295)]
        public uint Unknown1;

        [TagField(MinVersion = CacheVersion.HaloOnline498295)]
        public uint Unknown2;

        public AiSoundVolume DetonationNoiseForAi;
        public short SuperDetonationProjectileCount;
        public float SuperDetonationDelay;
        public CachedTag DetonationStarted;
        public CachedTag AirborneDetonationEffect;
        public CachedTag GroundDetonationEffect;
        public CachedTag DetonationDamage;
        public CachedTag AttachedDetonationDamage;
        public CachedTag SuperDetonation;
        public CachedTag SuperDetonationDamage;
        public CachedTag DetonationSound;
        public DamageReportingType DamageReportingType;

        public byte Padding0;
        public byte Padding1;
        public byte Padding2;

        public CachedTag AttachedSuperDetonationDamage;
        public float MaterialEffectRadius; // radius within we will generate material effects
        public CachedTag FlybySound;
        public CachedTag ImpactEffect;
        public CachedTag ObjectImpactEffect;
        public CachedTag ImpactDamage;
        public float BoardingDetonationTime;
        public CachedTag BoardingDetonationDamage;
        public CachedTag BoardingAttachedDetonationDamage;
        public float AirGravityScale; // the proportion of normal gravity applied to the projectile when in air.
        public Bounds<float> AirDamageRange; // the range over which damage is scaled when the projectile is in air.
        public float WaterGravityScale; // the proportion of normal gravity applied to the projectile when in water.
        public Bounds<float> WaterDamageRange; // the range over which damage is scaled when the projectile is in water.
        public float InitialVelocity;// bullet's velocity when inflicting maximum damage (world units per second)
        public float FinalVelocity; // bullet's velocity when inflicting minimum damage (world units per second)

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public float IndirectFireVelocity;

        public float AiVelocityScaleNormal; // scale on the initial velocity when fired by the ai on normal difficulty
        public float AiVelocityScaleLegendary; // scale on the initial velocity when fired by the ai on legendary difficulty (0 defaults to 1.0)
        public Bounds<Angle> GuidedAngularVelocity; // degrees per second
        public Angle GuidedAngularVelocityAtRest; // degrees per second
        public Bounds<float> AccelerationRange;
        public float RuntimeAccelerationBoundInverse;
        public float TargetedLeadingFraction;
        public float GuidedProjectileOuterRangeErrorRadius;
        public float AutoaimLeadingMaxLeadTime;

        public List<MaterialResponse> MaterialResponses;
        public List<BruteGrenadeProperty> BruteGrenadeProperties;
        public List<FireBombGrenadeProperty> FireBombGrenadeProperties;
        public List<ConicalProjectionProperty> ConicalSpread;

        [Flags]
        public enum ProjectileFlags : int
        {
            None,
            OrientedAlongVelocity = 1 << 0,
            AiMustUseBallisticAiming = 1 << 1,
            DetonationMaxTimeIfAttached = 1 << 2,
            HasSuperCombiningExplosion = 1 << 3,
            DamageScalesBasedOnDistance = 1 << 4,
            TravelsInstantaneously = 1 << 5,
            SteeringAdjustsOrientation = 1 << 6,
            DoNotNoiseUpSteering = 1 << 7,
            CanTrackBehindItself = 1 << 8,
            RobotronSteering = 1 << 9,
            AffectedByPhantomVolumes = 1 << 10,
            ExpensiveChubbyTest = 1 << 11,
            NotifiesTargetUnits = 1 << 12,
            UseGroundDetonationWhenAttached = 1 << 13,
            AIMinorTrackingThreat = 1 << 14,
            DangerousWhenInactive = 1 << 15,
            AIStimulusWhenAttached = 1 << 16,
            OverPeneDetonation = 1 << 17,
            NoImpactEffectsOnBounce = 1 << 18,
            RC1OverpenetrationFixes = 1 << 19
        }

        public enum DetonationTimerModes : short
        {
            Immediately,
            AfterFirstBounce,
            WhenAtRest,
            AfterFirstBounceOffAnySurface
        }

        public enum AiSoundVolume : short
        {
            Silent,
            Medium,
            Loud,
            Shout,
            Quiet
        }
        
        [TagStructure(Size = 0x40)]
        public class MaterialResponse : TagStructure
		{
            public ProjectileMaterialResponseFlags Flags;
            public MaterialResponseValue DefaultResponse;
            [TagField(Flags = Label)]
            public StringId MaterialName;
            public short RuntimeMaterialIndex;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding0;

            public MaterialResponseValue PotentialResponse;
            public MaterialPossibleResponseFlags ResponseFlags;
            public float ChanceFraction; // [0,1]
            public Bounds<Angle> BetweenAngle; // degrees
            public Bounds<float> AndVelocity; // world units per second
            public ScaleEffectsByValue ScaleEffectsBy;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;

            public Angle AngularNoise; // (degrees) angle of incidence is randomly perturbed by at most this amount to simulate irregularity.
            public float VelocityNoise; // (wu/s) the velocity is randomly perturbed by at most this amount to simulate irregularity.
            public float InitialFriction; // the fraction of the projectile's velocity lost on penetration
            public float MaximumDistance; // the maximum distance the projectile can travel through on object of this material
            public float ParallelFriction; // the fraction of the projectile's velocity parallel to the surface lost on impact
            public float PerpendicularFriction; // the fraction of the projectile's velocity perpendicular to the surface lost on impact

            [Flags]
            public enum ProjectileMaterialResponseFlags : ushort
            {
                None,
                CannotBeOverpenetrated = 1 << 0
            }

            [Flags]
            public enum MaterialPossibleResponseFlags : ushort
			{
				None,
				OnlyAgainstUnits = 1 << 0,
				NeverAgainstUnits = 1 << 1,
				OnlyAgainstBipeds = 1 << 2,
				OnlyAgainstVehicles = 1 << 3,
				NeverAgainstWussPlayers = 1 << 4,
				OnlyWhenTethered = 1 << 5,
				OnlyWhenNotTethered = 1 << 6,
				OnlyAgainstDeadBipeds = 1 << 7,
				NeverAgainstDeadBipeds = 1 << 8,
				OnlyAiProjectiles = 1 << 9,
				NeverAiProjectiles = 1 << 10
			}

            public enum MaterialResponseValue : short
            {
                ImpactDetonate,
                Fizzle,
                Overpenetrate,
                Attach,
                Bounce,
                BounceDud,
                FizzleRicochet
            }
            
            public enum ScaleEffectsByValue : short
            {
                Damage,
                Angle
            }
        }

        [TagStructure(Size = 0x30)]
        public class BruteGrenadeProperty : TagStructure
		{
            public Bounds<Angle> AngularVelocityRange; // degrees/sec
            public Angle SpinAngularVelocity; // degrees/sec
            public float AngularDamping; // 0==nothing 30==almost comlete damping
            public float DragAngleK;
            public float DragSpeedK;
            public float DragExponent;
            public float AttachSampleRadius;
            public float AttachAccK;
            public float AttachAccS;
            public float AttachAccE;
            public float AttachAccDamping;
        }

        [TagStructure(Size = 0x4)]
        public class FireBombGrenadeProperty : TagStructure
		{
            public float ProjectionOffset;
        }

        [TagStructure(Size = 0xC)]
        public class ConicalProjectionProperty : TagStructure
		{
            public short YawCount;
            public short PitchCount;
            public float DistributionExponent; // exp==.5 even distribution, exp==1  halo2 distribution, exp>1== weighted towards center
            public Angle Spread; // degrees
        }
    }
}