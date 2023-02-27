using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using TagTool.Damage;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "projectile", Tag = "proj", Size = 0x1A8, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "projectile", Tag = "proj", Size = 0x1AC, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "projectile", Tag = "proj", Size = 0x1F8, MinVersion = CacheVersion.HaloReach)]
    public class Projectile : GameObject
    {
        [TagField(MaxVersion = CacheVersion.HaloOnline235640)]
        public ProjectileFlags Flags;
        [TagField(MinVersion = CacheVersion.HaloOnline301003)]
        public ProjectileFlagsHO FlagsHO;
        public DetonationTimerModes DetonationTimerStarts;
        public AiSoundVolume ImpactVolumeForAi;
        public float CollisionRadius; // world units
        public float ArmingTime; // won't detonate before this time elapses (seconds)
        public float DangerRadius; // world units     

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float DangerStimuliRadius; // // Overrides the danger radius when non-zero for stimuli related danger radius calculations. (world units)
        // The number of projectiles in this burst before this burst is considered dangerous
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short DangerGroupBurstCount;  // The maximum number of projectiles we allow in a group
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short DangerGroupBurstMaxCount;

        public Bounds<float> Timer; // detonation countdown (seconds, zero is untimed)
        public float MinimumVelocity; // detonates when slowed below this velocity (world units per second)
        public float MaximumRange; // detonates after traveling this distance (world units)
        public float BounceMaximumRange; // detonates after this distance, but is reset after a bounce. Combines with maximum range
        
        [TagField(MinVersion = CacheVersion.HaloOnline498295)]
        public float MaxLatchTimeToDetonate; // projectile will detonate regardless of weapon latching after this total time (seconds)
        [TagField(MinVersion = CacheVersion.HaloOnline498295)]
        public float MaxLatchTimeToArm; // projectile will arm itself regardless of detonation mode if latched for this amount of time. (seconds)
        
        public AiSoundVolume DetonationNoiseForAi;
        public short SuperDetonationProjectileCount;
        public float SuperDetonationDelay;

        [TagField(ValidTags = new[] { "effe" })]
        public CachedTag DetonationStarted;
        [TagField(ValidTags = new[] { "effe", "jpt!" })]
        public CachedTag AirborneDetonationEffect;
        [TagField(ValidTags = new[] { "effe", "jpt!" })]
        public CachedTag GroundDetonationEffect;
        [TagField(ValidTags = new[] { "jpt!" })]
        public CachedTag DetonationDamage;
        [TagField(ValidTags = new[] { "jpt!" })]
        public CachedTag AttachedDetonationDamage;
        [TagField(ValidTags = new[] { "effe" })]
        public CachedTag SuperDetonation;
        [TagField(ValidTags = new[] { "jpt!" })]
        public CachedTag SuperDetonationDamage;
        [TagField(ValidTags = new[] { "jpt!" })]
        public CachedTag DetonationSound;

        public DamageReportingType DamageReportingType;

        [TagField(Length = 3, Flags = Padding, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagField(Length = 1, Flags = Padding, MinVersion = CacheVersion.HaloReach)]
        public byte[] Padding1;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public GameObjectType16 SuperDetonationObjectTypes;
        public CachedTag AttachedSuperDetonationDamage;
        public float MaterialEffectRadius; // radius within we will generate material effects
        public CachedTag FlybySound;

        [TagField(ValidTags = new[] { "drdf" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag FlybyDamageResponse;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float FlybyDamageResponseMaxDistance;
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
        
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float AiVelocityScaleHeroic; // scale on the initial velocity when fired by the ai on heroic  difficulty
        
        public float AiVelocityScaleLegendary; // scale on the initial velocity when fired by the ai on legendary difficulty (0 defaults to 1.0)
        
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float AiGuidedAngularVelocityScaleNormal; // scale on the guided angular velocity when fired by the ai on normal difficulty (0 defaults to 1.0) [0-1]
        
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float AiGuidedAngularVelocityScaleLegendary; // // scale on the guided angular velocity when fired by the ai on legendary difficulty (0 defaults to 1.0) [0-1]
        
        public Bounds<Angle> GuidedAngularVelocity; // degrees per second
        public Angle GuidedAngularVelocityAtRest; // degrees per second
        public Bounds<float> AccelerationRange;
        public float RuntimeAccelerationBoundInverse;
        public float TargetedLeadingFraction;
        public float GuidedProjectileOuterRangeErrorRadius;
        public float AutoaimLeadingMaxLeadTime;

        public List<ProjectileMaterialResponseBlock> MaterialResponses;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<ProjectileMaterialResponseBlockNew> MaterialResponsesNew;

        public List<BruteGrenadeProperty> BruteGrenadeProperties;
        public List<FireBombGrenadeProperty> FireBombGrenadeProperties;
        public List<ConicalProjectionProperty> ConicalSpread;

        // If not present, the default from global.globals is used.
        [TagField(ValidTags = new[] { "grfr" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag GroundedFrictionSettings;

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
            RC1OverpenetrationFixes = 1 << 19,
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

        [Flags]
        public enum ProjectileFlagsHO : int
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
            RC1OverpenetrationFixes = 1 << 19,
            UnknownBit20 = 1 << 20,
            UnknownBit21 = 1 << 21,
            SupportsTethering = 1 << 22,
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
        
        [TagStructure(Size = 0x40, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x3C, MinVersion = CacheVersion.HaloReach)]
        public class ProjectileMaterialResponseBlock : TagStructure
		{
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public ProjectileMaterialResponseFlags Flags;
            public MaterialResponseValue DefaultResponse;

            [TagField(Length = 0x2, Flags = Padding, MinVersion = CacheVersion.HaloReach)]
            public byte[] Padding1;

            [TagField(Flags = GlobalMaterial | Label)]
            public StringId MaterialName;
            [TagField(Flags = GlobalMaterial)]
            public short RuntimeMaterialIndex;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;

            public MaterialResponseValue PotentialResponse;
            public MaterialPossibleResponseFlags ResponseFlags;
            public float ChanceFraction; // [0,1]
            public Bounds<Angle> BetweenAngle; // degrees
            public Bounds<float> AndVelocity; // world units per second
            public ScaleEffectsByValue ScaleEffectsBy;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding3;

            public Angle AngularNoise; // (degrees) angle of incidence is randomly perturbed by at most this amount to simulate irregularity.
            public float VelocityNoise; // (wu/s) the velocity is randomly perturbed by at most this amount to simulate irregularity.
            public float InitialFriction; // the fraction of the projectile's velocity lost on penetration
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
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

        [TagStructure(Size = 0x34)]
        public class ProjectileMaterialResponseBlockNew : TagStructure
        {
            [TagField(Flags = GlobalMaterial)]
            public StringId MaterialName;
            public short RuntimeMaterialIndex;
            public MaterialPossibleResponseFlags ResponseFlags;
            public float ChanceFraction; // [0,1]
            public Bounds<Angle> Between; // degrees
            public Bounds<float> And; // world units per second
            public MaterialResponse Response;
            public EffectScaleEnum ScaleEffectsBy;
            // the angle of incidence is randomly perturbed by at most this amount to simulate irregularity.
            public Angle AngularNoise; // degrees
            // the velocity is randomly perturbed by at most this amount to simulate irregularity.
            public float VelocityNoise; // world units per second
            // the fraction of the projectile's velocity lost on penetration
            public float InitialFriction;
            // the fraction of the projectile's velocity parallel to the surface lost on impact
            public float ParallelFriction;
            // the fraction of the projectile's velocity perpendicular to the surface lost on impact
            public float PerpendicularFriction;

            [Flags]
            public enum MaterialPossibleResponseFlags : ushort
            {
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

            public enum MaterialResponse : short
            {
                Impact,
                Fizzle,
                Overpenetrate,
                Attach,
                Bounce,
                Bounce1,
                Fizzle1,
                TurnPhysical
            }

            public enum EffectScaleEnum : short
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