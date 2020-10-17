using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "projectile", Tag = "proj", Size = 0x1A4)]
    public class Projectile : TagStructure
    {
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public FlagsValue Flags;
        public float BoundingRadius; // world units
        public RealPoint3d BoundingOffset;
        /// <summary>
        /// marine 1.0, grunt 1.4, elite 0.9, hunter 0.5, etc.
        /// </summary>
        public float AccelerationScale; // [0,+inf]
        public LightmapShadowModeValue LightmapShadowMode;
        public SweetenerSizeValue SweetenerSize;
        [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        /// <summary>
        /// sphere to use for dynamic lights and shadows. only used if not 0
        /// </summary>
        public float DynamicLightSphereRadius;
        /// <summary>
        /// only used if radius not 0
        /// </summary>
        public RealPoint3d DynamicLightSphereOffset;
        public StringId DefaultModelVariant;
        [TagField(ValidTags = new [] { "hlmt" })]
        public CachedTag Model;
        [TagField(ValidTags = new [] { "bloc" })]
        public CachedTag CrateObject;
        [TagField(ValidTags = new [] { "shad" })]
        public CachedTag ModifierShader;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag CreationEffect;
        [TagField(ValidTags = new [] { "foot" })]
        public CachedTag MaterialEffects;
        public List<ObjectAiPropertiesBlock> AiProperties;
        public List<ObjectFunctionBlock> Functions;
        /// <summary>
        /// for things that want to cause more or less collision damage
        /// </summary>
        /// <summary>
        /// 0 means 1.  1 is standard scale.  Some things may want to apply more damage
        /// </summary>
        public float ApplyCollisionDamageScale;
        /// <summary>
        /// 0 - means take default value from globals.globals
        /// </summary>
        /// <summary>
        /// 0-oo
        /// </summary>
        public float MinGameAccDefault;
        /// <summary>
        /// 0-oo
        /// </summary>
        public float MaxGameAccDefault;
        /// <summary>
        /// 0-1
        /// </summary>
        public float MinGameScaleDefault;
        /// <summary>
        /// 0-1
        /// </summary>
        public float MaxGameScaleDefault;
        /// <summary>
        /// 0 - means take default value from globals.globals
        /// </summary>
        /// <summary>
        /// 0-oo
        /// </summary>
        public float MinAbsAccDefault;
        /// <summary>
        /// 0-oo
        /// </summary>
        public float MaxAbsAccDefault;
        /// <summary>
        /// 0-1
        /// </summary>
        public float MinAbsScaleDefault;
        /// <summary>
        /// 0-1
        /// </summary>
        public float MaxAbsScaleDefault;
        public short HudTextMessageIndex;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding3;
        public List<ObjectAttachmentBlock> Attachments;
        public List<ObjectWidgetBlock> Widgets;
        public List<OldObjectFunctionBlock> OldFunctions;
        public List<ObjectChangeColors> ChangeColors;
        public List<PredictedResourceBlock> PredictedResources;
        public FlagsValue1 Flags1;
        public DetonationTimerStartsValue DetonationTimerStarts;
        public ImpactNoiseValue ImpactNoise;
        public float AiPerceptionRadius; // world units
        public float CollisionRadius; // world units
        /// <summary>
        /// won't detonate before this time elapses
        /// </summary>
        public float ArmingTime; // seconds
        public float DangerRadius; // world units
        /// <summary>
        /// detonation countdown (zero is untimed)
        /// </summary>
        public Bounds<float> Timer; // seconds
        /// <summary>
        /// detonates when slowed below this velocity
        /// </summary>
        public float MinimumVelocity; // world units per second
        /// <summary>
        /// detonates after travelling this distance
        /// </summary>
        public float MaximumRange; // world units
        public DetonationNoiseValue DetonationNoise;
        public short SuperDetProjectileCount;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag DetonationStarted;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag DetonationEffectAirborne;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag DetonationEffectGround;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag DetonationDamage;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag AttachedDetonationDamage;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag SuperDetonation;
        public SuperDetonationDamageStructBlock YourMomma;
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag DetonationSound;
        public DamageReportingTypeValue DamageReportingType;
        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
        public byte[] Padding4;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag SuperAttachedDetonationDamage;
        /// <summary>
        /// radius within we will generate material effects
        /// </summary>
        public float MaterialEffectRadius;
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag FlybySound;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag ImpactEffect;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag ImpactDamage;
        public float BoardingDetonationTime;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag BoardingDetonationDamage;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag BoardingAttachedDetonationDamage;
        /// <summary>
        /// the proportion of normal gravity applied to the projectile when in air.
        /// </summary>
        public float AirGravityScale;
        /// <summary>
        /// the range over which damage is scaled when the projectile is in air.
        /// </summary>
        public Bounds<float> AirDamageRange; // world units
        /// <summary>
        /// the proportion of normal gravity applied to the projectile when in water.
        /// </summary>
        public float WaterGravityScale;
        /// <summary>
        /// the range over which damage is scaled when the projectile is in water.
        /// </summary>
        public Bounds<float> WaterDamageRange; // world units
        /// <summary>
        /// bullet's velocity when inflicting maximum damage
        /// </summary>
        public float InitialVelocity; // world units per second
        /// <summary>
        /// bullet's velocity when inflicting minimum damage
        /// </summary>
        public float FinalVelocity; // world units per second
        public AngularVelocityLowerBoundStructBlock Blah;
        public Angle GuidedAngularVelocityUpper; // degrees per second
        /// <summary>
        /// what distance range the projectile goes from initial velocity to final velocity
        /// </summary>
        public Bounds<float> AccelerationRange; // world units
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding5;
        public float TargetedLeadingFraction;
        public List<ProjectileMaterialResponseBlock> MaterialResponses;
        
        [Flags]
        public enum FlagsValue : ushort
        {
            DoesNotCastShadow = 1 << 0,
            SearchCardinalDirectionLightmapsOnFailure = 1 << 1,
            Unused = 1 << 2,
            NotAPathfindingObstacle = 1 << 3,
            /// <summary>
            /// object passes all function values to parent and uses parent's markers
            /// </summary>
            ExtensionOfParent = 1 << 4,
            DoesNotCauseCollisionDamage = 1 << 5,
            EarlyMover = 1 << 6,
            EarlyMoverLocalizedPhysics = 1 << 7,
            /// <summary>
            /// cast a ton of rays once and store the results for lighting
            /// </summary>
            UseStaticMassiveLightmapSample = 1 << 8,
            ObjectScalesAttachments = 1 << 9,
            InheritsPlayerSAppearance = 1 << 10,
            DeadBipedsCanTLocalize = 1 << 11,
            /// <summary>
            /// use this for the mac gun on spacestation
            /// </summary>
            AttachToClustersByDynamicSphere = 1 << 12,
            EffectsCreatedByThisObjectDoNotSpawnObjectsInMultiplayer = 1 << 13
        }
        
        public enum LightmapShadowModeValue : short
        {
            Default,
            Never,
            Always
        }
        
        public enum SweetenerSizeValue : sbyte
        {
            Small,
            Medium,
            Large
        }
        
        [TagStructure(Size = 0x10)]
        public class ObjectAiPropertiesBlock : TagStructure
        {
            public AiFlagsValue AiFlags;
            /// <summary>
            /// used for combat dialogue, etc.
            /// </summary>
            public StringId AiTypeName;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public AiSizeValue AiSize;
            public LeapJumpSpeedValue LeapJumpSpeed;
            
            [Flags]
            public enum AiFlagsValue : uint
            {
                DetroyableCover = 1 << 0,
                PathfindingIgnoreWhenDead = 1 << 1,
                DynamicCover = 1 << 2
            }
            
            public enum AiSizeValue : short
            {
                Default,
                Tiny,
                Small,
                Medium,
                Large,
                Huge,
                Immobile
            }
            
            public enum LeapJumpSpeedValue : short
            {
                None,
                Down,
                Step,
                Crouch,
                Stand,
                Storey,
                Tower,
                Infinite
            }
        }
        
        [TagStructure(Size = 0x20)]
        public class ObjectFunctionBlock : TagStructure
        {
            public FlagsValue Flags;
            public StringId ImportName;
            public StringId ExportName;
            /// <summary>
            /// if the specified function is off, so is this function
            /// </summary>
            public StringId TurnOffWith;
            /// <summary>
            /// function must exceed this value (after mapping) to be active 0. means do nothing
            /// </summary>
            public float MinValue;
            public MappingFunctionBlock DefaultFunction;
            public StringId ScaleBy;
            
            [Flags]
            public enum FlagsValue : uint
            {
                /// <summary>
                /// result of function is one minus actual result
                /// </summary>
                Invert = 1 << 0,
                /// <summary>
                /// the curve mapping can make the function active/inactive
                /// </summary>
                MappingDoesNotControlsActive = 1 << 1,
                /// <summary>
                /// function does not deactivate when at or below lower bound
                /// </summary>
                AlwaysActive = 1 << 2,
                /// <summary>
                /// function offsets periodic function input by random value between 0 and 1
                /// </summary>
                RandomTimeOffset = 1 << 3
            }
            
            [TagStructure(Size = 0x8)]
            public class MappingFunctionBlock : TagStructure
            {
                public List<ByteBlock> Data;
                
                [TagStructure(Size = 0x1)]
                public class ByteBlock : TagStructure
                {
                    public sbyte Value;
                }
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class ObjectAttachmentBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "ligh","MGS2","tdtl","cont","effe","lsnd","lens" })]
            public CachedTag Type;
            public StringId Marker;
            public ChangeColorValue ChangeColor;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public StringId PrimaryScale;
            public StringId SecondaryScale;
            
            public enum ChangeColorValue : short
            {
                None,
                Primary,
                Secondary,
                Tertiary,
                Quaternary
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class ObjectWidgetBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "ant!","devo","whip","BooM","tdtl" })]
            public CachedTag Type;
        }
        
        [TagStructure(Size = 0x50)]
        public class OldObjectFunctionBlock : TagStructure
        {
            [TagField(Length = 0x4C, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public StringId Unknown;
        }
        
        [TagStructure(Size = 0x10)]
        public class ObjectChangeColors : TagStructure
        {
            public List<ObjectChangeColorInitialPermutation> InitialPermutations;
            public List<ObjectChangeColorFunction> Functions;
            
            [TagStructure(Size = 0x20)]
            public class ObjectChangeColorInitialPermutation : TagStructure
            {
                public float Weight;
                public RealRgbColor ColorLowerBound;
                public RealRgbColor ColorUpperBound;
                /// <summary>
                /// if empty, may be used by any model variant
                /// </summary>
                public StringId VariantName;
            }
            
            [TagStructure(Size = 0x28)]
            public class ObjectChangeColorFunction : TagStructure
            {
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ScaleFlagsValue ScaleFlags;
                public RealRgbColor ColorLowerBound;
                public RealRgbColor ColorUpperBound;
                public StringId DarkenBy;
                public StringId ScaleBy;
                
                [Flags]
                public enum ScaleFlagsValue : uint
                {
                    /// <summary>
                    /// blends colors in hsv rather than rgb space
                    /// </summary>
                    BlendInHsv = 1 << 0,
                    /// <summary>
                    /// blends colors through more hues (goes the long way around the color wheel)
                    /// </summary>
                    MoreColors = 1 << 1
                }
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class PredictedResourceBlock : TagStructure
        {
            public TypeValue Type;
            public short ResourceIndex;
            public int TagIndex;
            
            public enum TypeValue : short
            {
                Bitmap,
                Sound,
                RenderModelGeometry,
                ClusterGeometry,
                ClusterInstancedGeometry,
                LightmapGeometryObjectBuckets,
                LightmapGeometryInstanceBuckets,
                LightmapClusterBitmaps,
                LightmapInstanceBitmaps
            }
        }
        
        [Flags]
        public enum FlagsValue1 : uint
        {
            OrientedAlongVelocity = 1 << 0,
            AiMustUseBallisticAiming = 1 << 1,
            DetonationMaxTimeIfAttached = 1 << 2,
            HasSuperCombiningExplosion = 1 << 3,
            DamageScalesBasedOnDistance = 1 << 4,
            TravelsInstantaneously = 1 << 5,
            SteeringAdjustsOrientation = 1 << 6,
            DonTNoiseUpSteering = 1 << 7,
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
        
        public enum ImpactNoiseValue : short
        {
            Silent,
            Medium,
            Loud,
            Shout,
            Quiet
        }
        
        public enum DetonationNoiseValue : short
        {
            Silent,
            Medium,
            Loud,
            Shout,
            Quiet
        }
        
        [TagStructure(Size = 0x8)]
        public class SuperDetonationDamageStructBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag SuperDetonationDamage;
        }
        
        public enum DamageReportingTypeValue : sbyte
        {
            TehGuardians11,
            FallingDamage,
            GenericCollisionDamage,
            GenericMeleeDamage,
            GenericExplosion,
            MagnumPistol,
            PlasmaPistol,
            Needler,
            Smg,
            PlasmaRifle,
            BattleRifle,
            Carbine,
            Shotgun,
            SniperRifle,
            BeamRifle,
            RocketLauncher,
            FlakCannon,
            BruteShot,
            Disintegrator,
            BrutePlasmaRifle,
            EnergySword,
            FragGrenade,
            PlasmaGrenade,
            FlagMeleeDamage,
            BombMeleeDamage,
            BombExplosionDamage,
            BallMeleeDamage,
            HumanTurret,
            PlasmaTurret,
            Banshee,
            Ghost,
            Mongoose,
            Scorpion,
            SpectreDriver,
            SpectreGunner,
            WarthogDriver,
            WarthogGunner,
            Wraith,
            Tank,
            SentinelBeam,
            SentinelRpg,
            Teleporter
        }
        
        [TagStructure(Size = 0x4)]
        public class AngularVelocityLowerBoundStructBlock : TagStructure
        {
            public Angle GuidedAngularVelocityLower; // degrees per second
        }
        
        [TagStructure(Size = 0x58)]
        public class ProjectileMaterialResponseBlock : TagStructure
        {
            public FlagsValue Flags;
            /// <summary>
            /// (if the potential result, below, fails to happen)
            /// </summary>
            public ResponseValue Response;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag DoNotUseOldEffect;
            public StringId MaterialName;
            [TagField(Length = 0x4)]
            public byte[] Unknown;
            public ResponseValue1 Response1;
            public FlagsValue1 Flags1;
            public float ChanceFraction; // [0,1]
            public Bounds<Angle> Between; // degrees
            public Bounds<float> And; // world units per second
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag DoNotUseOldEffect1;
            public ScaleEffectsByValue ScaleEffectsBy;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            /// <summary>
            /// the angle of incidence is randomly perturbed by at most this amount to simulate irregularity.
            /// </summary>
            public Angle AngularNoise; // degrees
            /// <summary>
            /// the velocity is randomly perturbed by at most this amount to simulate irregularity.
            /// </summary>
            public float VelocityNoise; // world units per second
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag DoNotUseOldDetonationEffect;
            /// <summary>
            /// the fraction of the projectile's velocity lost on penetration
            /// </summary>
            public float InitialFriction;
            /// <summary>
            /// the maximum distance the projectile can travel through on object of this material
            /// </summary>
            public float MaximumDistance;
            /// <summary>
            /// the fraction of the projectile's velocity parallel to the surface lost on impact
            /// </summary>
            public float ParallelFriction;
            /// <summary>
            /// the fraction of the projectile's velocity perpendicular to the surface lost on impact
            /// </summary>
            public float PerpendicularFriction;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                CannotBeOverpenetrated = 1 << 0
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
            
            public enum ResponseValue1 : short
            {
                ImpactDetonate,
                Fizzle,
                Overpenetrate,
                Attach,
                Bounce,
                BounceDud,
                FizzleRicochet
            }
            
            [Flags]
            public enum FlagsValue1 : ushort
            {
                OnlyAgainstUnits = 1 << 0,
                NeverAgainstUnits = 1 << 1
            }
            
            public enum ScaleEffectsByValue : short
            {
                Damage,
                Angle
            }
        }
    }
}

