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
        public ProjectileFlagBits ProjectileFlags;
        public DetonationTimerStartsValue DetonationTimerStarts;
        public NoiseLevelValue ImpactNoise;
        public float CollisionRadius;
        public float ArmingTime;
        public float DangerRadius;
        public Bounds<float> Timer;
        public float MinimumVelocity;
        public float MaximumRange;
        public float DetonationChargeTime;

        [TagField(MinVersion = CacheVersion.HaloOnline498295)]
        public uint Unknown1;

        [TagField(MinVersion = CacheVersion.HaloOnline498295)]
        public uint Unknown2;

        public NoiseLevelValue DetonationNoise;
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

        public byte Unknown3;
        public byte Unknown4;
        public byte Unknown5;

        public CachedTag AttachedSuperDetonationDamage;
        public float MaterialEffectRadius;
        public CachedTag FlybySound;
        public CachedTag FlybyResponse;
        public CachedTag ImpactEffect;
        public CachedTag ImpactDamage;
        public float BoardingDetonationTime;
        public CachedTag BoardingDetonationDamage;
        public CachedTag BoardingAttachedDetonationDamage;
        public float AirGravityScale;
        public Bounds<float> AirDamageRange;
        public float WaterGravityScale;
        public Bounds<float> WaterDamageRange;
        public float InitialVelocity;
        public float FinalVelocity;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public float IndirectFireVelocity;

        public float AiVelocityScale;
        public float AiGuidedAngularVelocityScale;
        public Bounds<Angle> GuidedAngularVelocity;
        public Angle GuidedAngularVelocityAtRest;
        public Bounds<float> AccelerationRange;
        public float AiTargetLeadingScale;
        public float TargetedLeadingFraction;
        public float GuidedProjectileOuterRangeErrorRadius;
        public float AutoaimLeadingMaxLeadTime;

        public List<MaterialResponse> MaterialResponses;
        public List<BruteGrenadeProperty> BruteGrenadeProperties;
        public List<FireBombGrenadeProperty> FireBombGrenadeProperties;
        public List<ShotgunProperty> ShotgunProperties;

        [Flags]
        public enum ProjectileFlagBits : int
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
            FasterWhenOwnedByPlayer = 1 << 10
        }

        public enum DetonationTimerStartsValue : short
        {
            Immediately,
            AfterFirstBounce,
            WhenAtRest,
            AfterFirstBounceOffAnySurface
        }

        public enum NoiseLevelValue : short
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
            public FlagBits Flags;
            public ResponseValue DefaultResponse;
            [TagField(Flags = Label)]
            public StringId MaterialName;
            public short GlobalMaterialIndex;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused1;

            public ResponseValue PotentialResponse;
            public ResponseFlagBits ResponseFlags;
            public float ChanceFraction;
            public Bounds<Angle> BetweenAngle;
            public Bounds<float> AndVelocity;
            public ScaleEffectsByValue ScaleEffectsBy;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused2;

            public Angle AngularNoise;
            public float VelocityNoise;
            public float InitialFriction;
            public float MaximumDistance;
            public float ParallelFriction;
            public float PerpendicularFriction;

            [Flags]
            public enum FlagBits : ushort
            {
                None,
                CannotBeOverpenetrated = 1 << 0
            }

            [Flags]
            public enum ResponseFlagBits : ushort
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

            public enum ResponseValue : short
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
            public Bounds<Angle> AngularVelocityRange;
            public Angle SpinAngularVelocity;
            public float AngularDamping;
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
        public class ShotgunProperty : TagStructure
		{
            public short Amount;
            public short Distance;
            public float Accuracy;
            public Angle SpreadConeAngle;
        }
    }
}