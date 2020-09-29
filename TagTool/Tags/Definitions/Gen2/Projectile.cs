using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "projectile", Tag = "proj", Size = 0x25C)]
    public class Projectile : TagStructure
    {
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        public FlagsValue Flags;
        public float BoundingRadius; // world units
        public RealPoint3d BoundingOffset;
        public float AccelerationScale; // [0,+inf]
        public LightmapShadowModeValue LightmapShadowMode;
        public SweetenerSizeValue SweetenerSize;
        [TagField(Flags = Padding, Length = 1)]
        public byte[] Padding2;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding3;
        public float DynamicLightSphereRadius; // sphere to use for dynamic lights and shadows. only used if not 0
        public RealPoint3d DynamicLightSphereOffset; // only used if radius not 0
        public StringId DefaultModelVariant;
        public CachedTag Model;
        public CachedTag CrateObject;
        public CachedTag ModifierShader;
        public CachedTag CreationEffect;
        public CachedTag MaterialEffects;
        public List<ObjectAiProperties> AiProperties;
        public List<ObjectFunctionDefinition> Functions;
        /// <summary>
        /// Applying collision damage
        /// </summary>
        /// <remarks>
        /// for things that want to cause more or less collision damage
        /// </remarks>
        public float ApplyCollisionDamageScale; // 0 means 1.  1 is standard scale.  Some things may want to apply more damage
        /// <summary>
        /// Game collision damage parameters
        /// </summary>
        /// <remarks>
        /// 0 - means take default value from globals.globals
        /// </remarks>
        public float MinGameAccDefault; // 0-oo
        public float MaxGameAccDefault; // 0-oo
        public float MinGameScaleDefault; // 0-1
        public float MaxGameScaleDefault; // 0-1
        /// <summary>
        /// Absolute collision damage parameters
        /// </summary>
        /// <remarks>
        /// 0 - means take default value from globals.globals
        /// </remarks>
        public float MinAbsAccDefault; // 0-oo
        public float MaxAbsAccDefault; // 0-oo
        public float MinAbsScaleDefault; // 0-1
        public float MaxAbsScaleDefault; // 0-1
        public short HudTextMessageIndex;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding4;
        public List<ObjectAttachmentDefinition> Attachments;
        public List<ObjectDefinitionWidget> Widgets;
        public List<OldObjectFunctionDefinition> OldFunctions;
        public List<ObjectChangeColorDefinition> ChangeColors;
        public List<PredictedResource> PredictedResources;
        /// <summary>
        /// $$$ PROJECTILE $$$
        /// </summary>
        public FlagsValue Flags1;
        public DetonationTimerStartsValue DetonationTimerStarts;
        public ImpactNoiseValue ImpactNoise;
        public float AiPerceptionRadius; // world units
        public float CollisionRadius; // world units
        /// <summary>
        /// detonation
        /// </summary>
        public float ArmingTime; // seconds
        public float DangerRadius; // world units
        public Bounds<float> Timer; // seconds
        public float MinimumVelocity; // world units per second
        public float MaximumRange; // world units
        public DetonationNoiseValue DetonationNoise;
        public short SuperDetProjectileCount;
        public CachedTag DetonationStarted;
        public CachedTag DetonationEffectAirborne;
        public CachedTag DetonationEffectGround;
        public CachedTag DetonationDamage;
        public CachedTag AttachedDetonationDamage;
        public CachedTag SuperDetonation;
        public TagReference YourMomma;
        public CachedTag DetonationSound;
        public DamageReportingTypeValue DamageReportingType;
        [TagField(Flags = Padding, Length = 3)]
        public byte[] Padding12;
        public CachedTag SuperAttachedDetonationDamage;
        public float MaterialEffectRadius; // radius within we will generate material effects
        /// <summary>
        /// flyby/impact
        /// </summary>
        public CachedTag FlybySound;
        public CachedTag ImpactEffect;
        public CachedTag ImpactDamage;
        /// <summary>
        /// boarding fields
        /// </summary>
        public float BoardingDetonationTime;
        public CachedTag BoardingDetonationDamage;
        public CachedTag BoardingAttachedDetonationDamage;
        /// <summary>
        /// physics
        /// </summary>
        public float AirGravityScale; // the proportion of normal gravity applied to the projectile when in air.
        public Bounds<float> AirDamageRange; // world units
        public float WaterGravityScale; // the proportion of normal gravity applied to the projectile when in water.
        public Bounds<float> WaterDamageRange; // world units
        public float InitialVelocity; // world units per second
        public float FinalVelocity; // world units per second
        public Real Blah;
        public Angle GuidedAngularVelocityUpper; // degrees per second
        public Bounds<float> AccelerationRange; // world units
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding23;
        public float TargetedLeadingFraction;
        public List<ProjectileMaterialResponseDefinition> MaterialResponses;
        
        [Flags]
        public enum FlagsValue : ushort
        {
            DoesNotCastShadow = 1 << 0,
            SearchCardinalDirectionLightmapsOnFailure = 1 << 1,
            Unused = 1 << 2,
            NotAPathfindingObstacle = 1 << 3,
            ExtensionOfParent = 1 << 4,
            DoesNotCauseCollisionDamage = 1 << 5,
            EarlyMover = 1 << 6,
            EarlyMoverLocalizedPhysics = 1 << 7,
            UseStaticMassiveLightmapSample = 1 << 8,
            ObjectScalesAttachments = 1 << 9,
            InheritsPlayerSAppearance = 1 << 10,
            DeadBipedsCanTLocalize = 1 << 11,
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
        public class ObjectAiProperties : TagStructure
        {
            public AiFlagsValue AiFlags;
            public StringId AiTypeName; // used for combat dialogue, etc.
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
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
        
        [TagStructure(Size = 0x24)]
        public class ObjectFunctionDefinition : TagStructure
        {
            public FlagsValue Flags;
            public StringId ImportName;
            public StringId ExportName;
            public StringId TurnOffWith; // if the specified function is off, so is this function
            public float MinValue; // function must exceed this value (after mapping) to be active 0. means do nothing
            public FunctionDefinition DefaultFunction;
            public StringId ScaleBy;
            
            [Flags]
            public enum FlagsValue : uint
            {
                Invert = 1 << 0,
                MappingDoesNotControlsActive = 1 << 1,
                AlwaysActive = 1 << 2,
                RandomTimeOffset = 1 << 3
            }
            
            [TagStructure(Size = 0xC)]
            public class FunctionDefinition : TagStructure
            {
                public List<Byte> Data;
                
                [TagStructure(Size = 0x1)]
                public class Byte : TagStructure
                {
                    public sbyte Value;
                }
            }
        }
        
        [TagStructure(Size = 0x20)]
        public class ObjectAttachmentDefinition : TagStructure
        {
            public CachedTag Type;
            public StringId Marker;
            public ChangeColorValue ChangeColor;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
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
        
        [TagStructure(Size = 0x10)]
        public class ObjectDefinitionWidget : TagStructure
        {
            public CachedTag Type;
        }
        
        [TagStructure(Size = 0x50)]
        public class OldObjectFunctionDefinition : TagStructure
        {
            [TagField(Flags = Padding, Length = 76)]
            public byte[] Padding1;
            public StringId Unknown1;
        }
        
        [TagStructure(Size = 0x18)]
        public class ObjectChangeColorDefinition : TagStructure
        {
            public List<ObjectChangeColorInitialPermutation> InitialPermutations;
            public List<ObjectChangeColorFunction> Functions;
            
            [TagStructure(Size = 0x20)]
            public class ObjectChangeColorInitialPermutation : TagStructure
            {
                public float Weight;
                public RealRgbColor ColorLowerBound;
                public RealRgbColor ColorUpperBound;
                public StringId VariantName; // if empty, may be used by any model variant
            }
            
            [TagStructure(Size = 0x28)]
            public class ObjectChangeColorFunction : TagStructure
            {
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding1;
                public ScaleFlagsValue ScaleFlags;
                public RealRgbColor ColorLowerBound;
                public RealRgbColor ColorUpperBound;
                public StringId DarkenBy;
                public StringId ScaleBy;
                
                [Flags]
                public enum ScaleFlagsValue : uint
                {
                    BlendInHsv = 1 << 0,
                    MoreColors = 1 << 1
                }
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class PredictedResource : TagStructure
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
        
        [TagStructure(Size = 0x10)]
        public class TagReference : TagStructure
        {
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
        public class Real : TagStructure
        {
            public Angle GuidedAngularVelocityLower; // degrees per second
        }
        
        [TagStructure(Size = 0x70)]
        public class ProjectileMaterialResponseDefinition : TagStructure
        {
            public FlagsValue Flags;
            /// <summary>
            /// default result
            /// </summary>
            /// <remarks>
            /// (if the potential result, below, fails to happen)
            /// </remarks>
            public ResponseValue Response;
            public CachedTag DoNotUseOldEffect;
            public StringId MaterialName;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown1;
            /// <summary>
            /// potential result
            /// </summary>
            public ResponseValue Response1;
            public FlagsValue Flags2;
            public float ChanceFraction; // [0,1]
            public Bounds<Angle> Between; // degrees
            public Bounds<float> And; // world units per second
            public CachedTag DoNotUseOldEffect3;
            /// <summary>
            /// misc
            /// </summary>
            public ScaleEffectsByValue ScaleEffectsBy;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public Angle AngularNoise; // degrees
            public float VelocityNoise; // world units per second
            public CachedTag DoNotUseOldDetonationEffect;
            /// <summary>
            /// penetration
            /// </summary>
            public float InitialFriction; // the fraction of the projectile's velocity lost on penetration
            public float MaximumDistance; // the maximum distance the projectile can travel through on object of this material
            /// <summary>
            /// reflection
            /// </summary>
            public float ParallelFriction; // the fraction of the projectile's velocity parallel to the surface lost on impact
            public float PerpendicularFriction; // the fraction of the projectile's velocity perpendicular to the surface lost on impact
            
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
            
            public enum ScaleEffectsByValue : short
            {
                Damage,
                Angle
            }
        }
    }
}

