using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "projectile", Tag = "proj", Size = 0x24C)]
    public class Projectile : GameObject
    {
        public ProjectileFlags Flags;
        public SecondaryProjectileFlags SecondaryFlags;
        public ProjectileDetonationTimerModes DetonationTimerStarts;
        public AiSoundVolumeEnum ImpactNoise;
        // if >0, both 'detonation timer starts' + a biped must be within this proximity for condition to be met; see 'biped
        // proximity enemies only' flag
        public float DetonationBipedProximity; // wu
        // if >0, projectile will detonate regardless of other conditions after this total time
        public float MaxLifetimeToDetonate; // seconds
        public float CollisionRadius; // world units
        // won't detonate before this time elapses
        public float ArmingTime; // seconds
        public float DangerRadius; // world units
        // Overrides the danger radius when non-zero for stimuli related danger radius calculations.
        public float DangerStimuliRadius; // world units
        // The number of projectiles in this burst before this burst is considered dangerous
        public short DangerGroupBurstCount;
        // The maximum number of projectiles we allow in a group
        public short DangerGroupBurstMaxCount;
        // detonation countdown (zero is untimed)
        public Bounds<float> Timer; // seconds
        // detonates when slowed below this velocity
        public float MinimumVelocity; // world units per second
        // detonates after travelling this distance
        public float MaximumRange; // world units
        // detonates after travelling this distance, but is reset after a bounce.  Combines with maximum range
        public float BounceMaximumRange; // world units
        // projectile will detonate regardless of weapon latching after this total time
        public float MaxLatchTimeToDetonate; // seconds
        // projectile will arm itself regardless of detonation mode if latched for this amount of time.
        public float MaxLatchTimeToArm; // seconds
        public AiSoundVolumeEnum DetonationNoise;
        public short SuperDetProjectileCount;
        public float SuperDetTime;
        // The range within which supercombine will happen - outside this range, no supercombine
        public Bounds<float> SuperDetRange; // world units
        [TagField(ValidTags = new [] { "obje" })]
        // An equipment reference that is attached to the target upon super detonation
        public CachedTag SuperDetBehavior;
        // if the weapon the projectile is tethered to loses its owner, this amount of time will pass before detonation
        public float TetherReleaseSafetyDelay;
        [TagField(ValidTags = new [] { "effe" })]
        // effect
        public CachedTag DetonationStarted;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag DetonationEffect;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag DetonationEffect1;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag DetonationDamage;
        [TagField(ValidTags = new [] { "obje" })]
        // An equipment reference that is attached to the target upon detonation
        public CachedTag DetonationBehavior;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag AttachedDetonationDamage;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag SuperDetonation;
        public SuperDetonationDamageStruct YourMomma;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DetonationSound;
        public GlobalDamageReportingEnum DamageReportingType;
        [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public ObjectTypeEnum SuperDetonationObjectTypes;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag SuperAttachedDetonationDamage;
        // radius within we will generate material effects
        public float MaterialEffectRadius;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag FlybySound;
        [TagField(ValidTags = new [] { "drdf" })]
        public CachedTag FlybyDamageResponse;
        public float FlybyDamageResponseMaxDistance;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag ImpactEffect;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag ObjectImpactEffect;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag ImpactDamage;
        public float BoardingDetonationTime;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag BoardingDetonationDamage;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag BoardingAttachedDetonationDamage;
        // the proportion of normal gravity applied to the projectile when in air.
        public float AirGravityScale;
        // the range over which damage is scaled when the projectile is in air.
        public Bounds<float> AirDamageRange; // world units
        // the proportion of normal gravity applied to the projectile when in water.
        public float WaterGravityScale;
        // the range over which damage is scaled when the projectile is in water.
        public Bounds<float> WaterDamageRange; // world units
        // bullet's velocity when inflicting maximum damage
        public float InitialVelocity; // world units per second
        // bullet's velocity when inflicting minimum damage
        public float FinalVelocity; // world units per second
        // base velocity used for ballistics calculations for indirect firing.
        public float IndirectFireVelocity; // world units per second
        // scale on the initial velocity when fired by the ai on normal difficulty (0 defaults to 1.0
        public float AiVelocityScale; // [0-1]
        // scale on the initial velocity when fired by the ai on heroic difficulty (0 defaults to 1.0)
        public float AiVelocityScale1; // [0-1]
        // scale on the initial velocity when fired by the ai on legendary difficulty (0 defaults to 1.0)
        public float AiVelocityScale2; // [0-1]
        // scale on the guided angular velocity when fired by the ai on normal difficulty (0 defaults to 1.0
        public float AiGuidedAngularVelocityScale; // [0-1]
        // scale on the guided angular velocity when fired by the ai on legendary difficulty (0 defaults to 1.0)
        public float AiGuidedAngularVelocityScale1; // [0-1]
        public AngularVelocityLowerBoundStruct Blah;
        public Angle GuidedAngularVelocity; // degrees per second
        public Angle GuidedAngularVelocityAtRest; // degrees per second
        // what distance range the projectile goes from initial velocity to final velocity
        public Bounds<float> AccelerationRange; // world units
        public float RuntimeAccelerationBoundInverse;
        public float TargetedLeadingFraction;
        public float GuidedProjectile;
        public float AutoaimLeadingMaxLeadTime;
        public List<OldProjectileMaterialResponseBlock> OldMaterialResponses;
        public List<ProjectileMaterialResponseBlock> MaterialResponse;
        public List<BruteGrenadeBlock> BruteGrenade;
        public List<FireBombGrenadeBlock> FireBombGrenade;
        public List<ConicalProjectionBlock> ConicalSpread;
        [TagField(ValidTags = new [] { "grfr" })]
        // If not present, the default from global.globals is used.
        public CachedTag GroundedFrictionSettings;
        [TagField(ValidTags = new [] { "kccd" })]
        // if not present, first person will be used.
        public CachedTag KillcamParameters;
        public List<ProjectileSoundRtpcblock> SoundRtpcs;
        
        [Flags]
        public enum ProjectileFlags : uint
        {
            OrientedAlongVelocity = 1 << 0,
            AiMustUseBallisticAiming = 1 << 1,
            // If attach happens timeout is set to timer.high
            DetonationMaxTimeIfAttached = 1 << 2,
            DamageScalesBasedOnDistance = 1 << 3,
            SteeringAdjustsOrientation = 1 << 4,
            DonTNoiseUpSteering = 1 << 5,
            CanTrackBehindItself = 1 << 6,
            // or robotech, maybe
            RobotronSteering = 1 << 7,
            AffectedByPhantomVolumes = 1 << 8,
            NotifiesTargetUnits = 1 << 9,
            UseGroundDetonationWhenAttached = 1 << 10,
            AiMinorTrackingThreat = 1 << 11,
            DangerousWhenInactive = 1 << 12,
            AiStimulusWhenAttached = 1 << 13,
            OverPeneDetonation = 1 << 14,
            NoImpactEffectsOnBounce = 1 << 15,
            Rc1OverpenetrationFixes = 1 << 16,
            DisableInstantaneousFirstTick = 1 << 17,
            ConstrainGravityToVelocityBounds = 1 << 18,
            // use for bouncing projectiles that also have initial/final velocity so that can reliably come to rest.
            AllowDecelerationBelowFinalVelocity = 1 << 19,
            // projectile waits for trigger unlatch before immediately detonating
            SupportsTethering = 1 << 20,
            // used on the focus rifle to disable observer shield flash prediction for a high-dps weapon that does low damage per
            // projectile
            DamageNotPredictableByClients = 1 << 21,
            // aka sphere-only collision.  Use this for projectiles that you want to bounce smoothly up stairs, but don't use it
            // for projectiles that may come to rest on stairs
            CollidesWithPhysicsOnlySurfaces = 1 << 22,
            // when projectiles move slowly enough they attach to objects or come to rest.  Check this for projectiles that don't
            // normally attach to things like frag grenades
            DetonatesWhenAttachedToObjects = 1 << 23,
            // armor lock will not detach these projectiles - for the airstrike
            CannotBeDetachedByEquipment = 1 << 24,
            AlwaysAttachRegardlessOfMaterial = 1 << 25,
            // this gun shoots through schools
            DoesNotCollideWithWorldGeometry = 1 << 26,
            // Projectile is collectible by projectile collector equipment
            IsCollectible = 1 << 27,
            ContinuousDamageWhileAttachedAndTethered = 1 << 28,
            CombinationsOfProjectilesFromDifferentWeaponsOrDifferentBurstsOfTheSameWeaponWillNotTriggerSuperCombineDetonation = 1 << 29,
            // damage scales from 1.f --> 0.f  between 'damage range - low' --> 'damage range - high'
            DistanceBasedDamageScalingUsesDamageRangeLowBounds = 1 << 30,
            // similar to Disable instantaneous first tick, but there's actually two types of ticks
            SkipObjectFirstTick = 1u << 31
        }
        
        [Flags]
        public enum SecondaryProjectileFlags : uint
        {
            // IF THIS IS OFF, NO THICKNESS OR CHUBBY TESTS ARE PERFORMED.
            UseProjectileRadiusForThicknessTesting = 1 << 0,
            // Will only be active if thickness testing is ON.
            ExpensiveChubbyTest = 1 << 1,
            // Will use simple and smooth collision mesh.
            UsePlayCollision = 1 << 2,
            // Makes the projectile show up as an enemy in vision mode
            HighlightProjectileInVisionMode = 1 << 3,
            // modifies behavior of 'detonation biped proximity'
            BipedProximityEnemiesOnly = 1 << 4,
            // overrides early mover localize projectiles
            AlwaysUseLocalizedPhysics = 1 << 5,
            // overrides early mover localize projectiles
            NeverUseLocalizedPhysics = 1 << 6
        }
        
        public enum ProjectileDetonationTimerModes : short
        {
            Immediately,
            AfterFirstBounceOffFloor,
            WhenAtRest,
            AfterFirstBounceOffAnySurface
        }
        
        public enum AiSoundVolumeEnum : short
        {
            // ai will not respond to this sound
            Silent,
            Quiet,
            Medium,
            Shout,
            // ai can hear this sound at any range
            Loud
        }
        
        public enum GlobalDamageReportingEnum : sbyte
        {
            Unknown,
            TehGuardians,
            Scripting,
            AiSuicide,
            ForerunnerSmg,
            SpreadGun,
            ForerunnerRifle,
            ForerunnerSniper,
            BishopBeam,
            BoltPistol,
            PulseGrenade,
            IncinerationLauncher,
            MagnumPistol,
            AssaultRifle,
            MarksmanRifle,
            Shotgun,
            BattleRifle,
            SniperRifle,
            RocketLauncher,
            SpartanLaser,
            FragGrenade,
            StickyGrenadeLauncher,
            LightMachineGun,
            RailGun,
            PlasmaPistol,
            Needler,
            GravityHammer,
            EnergySword,
            PlasmaGrenade,
            Carbine,
            BeamRifle,
            AssaultCarbine,
            ConcussionRifle,
            FuelRodCannon,
            Ghost,
            RevenantDriver,
            RevenantGunner,
            Wraith,
            WraithAntiInfantry,
            Banshee,
            BansheeBomb,
            Seraph,
            RevenantDeuxDriver,
            RevenantDeuxGunner,
            LichDriver,
            LichGunner,
            Mongoose,
            WarthogDriver,
            WarthogGunner,
            WarthogGunnerGauss,
            WarthogGunnerRocket,
            Scorpion,
            ScorpionGunner,
            FalconDriver,
            FalconGunner,
            WaspDriver,
            WaspGunner,
            WaspGunnerHeavy,
            MechMelee,
            MechChaingun,
            MechCannon,
            MechRocket,
            Broadsword,
            BroadswordMissile,
            TortoiseDriver,
            TortoiseGunner,
            MacCannon,
            TargetDesignator,
            OrdnanceDropPod,
            OrbitalCruiseMissile,
            PortableShield,
            PersonalAutoTurret,
            ThrusterPack,
            FallingDamage,
            GenericCollisionDamage,
            GenericMeleeDamage,
            GenericExplosion,
            FireDamage,
            BirthdayPartyExplosion,
            FlagMeleeDamage,
            BombMeleeDamage,
            BombExplosionDamage,
            BallMeleeDamage,
            Teleporter,
            TransferDamage,
            ArmorLockCrush,
            HumanTurret,
            PlasmaCannon,
            PlasmaMortar,
            PlasmaTurret,
            ShadeTurret,
            ForerunnerTurret,
            Tank,
            Chopper,
            Hornet,
            Mantis,
            MagnumPistolCtf,
            FloodProngs
        }
        
        [Flags]
        public enum ObjectTypeEnum : ushort
        {
            Biped = 1 << 0,
            Vehicle = 1 << 1,
            Weapon = 1 << 2,
            Equipment = 1 << 3,
            Terminal = 1 << 4,
            Projectile = 1 << 5,
            Scenery = 1 << 6,
            Machine = 1 << 7,
            Control = 1 << 8,
            Dispenser = 1 << 9,
            SoundScenery = 1 << 10,
            Crate = 1 << 11,
            Creature = 1 << 12,
            Giant = 1 << 13,
            EffectScenery = 1 << 14,
            Spawner = 1 << 15
        }
        
        [TagStructure(Size = 0x10)]
        public class SuperDetonationDamageStruct : TagStructure
        {
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag SuperDetonationDamage;
        }
        
        [TagStructure(Size = 0x4)]
        public class AngularVelocityLowerBoundStruct : TagStructure
        {
            public Angle GuidedAngularVelocity; // degrees per second
        }
        
        [TagStructure(Size = 0x3C)]
        public class OldProjectileMaterialResponseBlock : TagStructure
        {
            public MaterialResponse DefaultResponse;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public StringId MaterialName;
            public short RuntimeMaterialIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public MaterialResponse PotentialResponse;
            public MaterialPossibleResponseFlags ResponseFlags;
            public float ChanceFraction; // [0,1]
            public Bounds<Angle> Between; // degrees
            public Bounds<float> And; // world units per second
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            public EffectScaleEnum ScaleEffectsBy;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
            // the angle of incidence is randomly perturbed by at most this amount to simulate irregularity.
            public Angle AngularNoise; // degrees
            // the velocity is randomly perturbed by at most this amount to simulate irregularity.
            public float VelocityNoise; // world units per second
            [TagField(Length = 0x18, Flags = TagFieldFlags.Padding)]
            public byte[] Padding5;
            // the fraction of the projectile's velocity lost on penetration
            public float InitialFriction;
            // the fraction of the projectile's velocity parallel to the surface lost on impact
            public float ParallelFriction;
            // the fraction of the projectile's velocity perpendicular to the surface lost on impact
            public float PerpendicularFriction;
            
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
            
            public enum EffectScaleEnum : short
            {
                Damage,
                Angle
            }
        }
        
        [TagStructure(Size = 0x34)]
        public class ProjectileMaterialResponseBlock : TagStructure
        {
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
        public class BruteGrenadeBlock : TagStructure
        {
            // degrees/sec
            public Angle MinimumAngularVel;
            // degrees/sec
            public Angle MaximumAngularVel;
            // degrees/sec
            public Angle SpinAngularVel;
            // 0==nothing 30==almost comlete damping
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
        public class FireBombGrenadeBlock : TagStructure
        {
            public float ProjectionOffset;
        }
        
        [TagStructure(Size = 0xC)]
        public class ConicalProjectionBlock : TagStructure
        {
            public short YawCount;
            public short PitchCount;
            // exp==.5 even distribution, exp==1  halo2 distribution, exp>1== weighted towards center
            public float DistributionExponent;
            // degrees
            public Angle Spread;
        }
        
        [TagStructure(Size = 0xC)]
        public class ProjectileSoundRtpcblock : TagStructure
        {
            // Sound attachment to affect - leave empty for main body
            public int AttachmentIndex;
            // Function to drive the RTPC
            public StringId Function;
            // WWise RTPC string name
            public StringId RtpcName;
        }
    }
}
