using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "effect", Tag = "effe", Size = 0x68)]
    public class Effect : TagStructure
    {
        public EffectFlags Flags;
        // If this is non-zero, the effect will usually be exactly repeatable
        public uint FixedRandomSeed;
        public float RestartIfWithin; // world units
        public float ContinueIfWithin; // world units
        public float DeathDelay;
        public short LoopStartEvent;
        public GlobalEffectPriorityEnum Priority;
        [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public float GlobalSizeScale;
        // Handle with care. FYI only works if the effect is attached to an object
        public StringId ScaleAOverride;
        // Handle with care. FYI only works if the effect is attached to an object
        public StringId ScaleBOverride;
        public float RuntimeDangerRadius;
        public List<EffectLocationsBlock> Locations;
        public List<EffectEventBlock> Events;
        public List<EffectLoopingSoundBlockStruct> LoopingSounds;
        public float AlwaysPlayDistance;
        public float NeverPlayDistance;
        public float RuntimeLightprobeDeathDelay;
        public float RuntimeLocalSpaceDeathDelay;
        public List<EffectConicalDistributionBlock> ConicalDistribution;
        
        [Flags]
        public enum EffectFlags : uint
        {
            DeletedWhenAttachmentDeactivates = 1 << 0,
            RunEventsInParallel = 1 << 1,
            // This option will hurt performance on effects with very short duration.
            DoNotReUsePartsWhenLooping = 1 << 2,
            AgeCreatorSWeapon = 1 << 3,
            UseParentPositionButWorldOrientation = 1 << 4,
            CanPenetrateWalls = 1 << 5,
            // Will help performance for parallel events of different lengths, if the effect is unlikely to be reused
            CannotBeRestarted = 1 << 6,
            // Do not adopt parent object's lightprobe, even when it's available
            ForceUseOwnLightprobe = 1 << 7,
            // Do not use this unless you know you stop the effect at some point
            ForceLooping = 1 << 8,
            // trigger creation of player's ordnance droppod
            OrdnanceDroppodLocator = 1 << 9,
            // this effect should be lumped in with Cortana and other holograms when rendered
            RenderInHologramPass = 1 << 10,
            // Only works with "force use own lightprobe"; causes effect to never sample lightmap, only placed light probes
            LightprobeOnlySampleAirprobes = 1 << 11,
            PlayEffectEvenOutsideBsps = 1 << 12,
            DrawPartsWhenStopped = 1 << 13,
            // particularly useful for particles that live forever
            KillParticlesWhenStopped = 1 << 14,
            PlayEvenOnHiddenObjects = 1 << 15,
            // blind skull hides the first person weapon
            DisableFirstPersonPartsInBlindSkull = 1 << 16,
            // when the effect is deleted, it hides the object it is associated with (for hologram in mp)
            HidesAssociatedObjectOnEffectDeletion = 1 << 17,
            // effect can not ignore generated crates or be non-networked
            BypassMpThrottle = 1 << 18
        }
        
        public enum GlobalEffectPriorityEnum : sbyte
        {
            Low,
            Normal,
            AboveNormal,
            High,
            VeryHigh,
            Essential
        }
        
        [TagStructure(Size = 0x8)]
        public class EffectLocationsBlock : TagStructure
        {
            public StringId MarkerName;
            public EffectLocationFlags Flags;
            public GlobalEffectPriorityEnum Priority;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            [Flags]
            public enum EffectLocationFlags : ushort
            {
                Optional = 1 << 0,
                Destructible = 1 << 1,
                // When effect moves multiple times within one frame, update location each time
                TrackSubFrameMovements = 1 << 2,
                // Even if this effect is told to stagger across available markers, always fire from all instances
                NeverStaggerAcrossMarkers = 1 << 3
            }
        }
        
        [TagStructure(Size = 0x44)]
        public class EffectEventBlock : TagStructure
        {
            public StringId EventName;
            public EventFlags Flags;
            public GlobalEffectPriorityEnum Priority;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // chance that this event will be skipped entirely
            public float SkipFraction;
            // delay before this event takes place
            public Bounds<float> DelayBounds; // seconds
            // duration of this event
            public Bounds<float> DurationBounds; // seconds
            // the amount of time over which the "event age" function input goes from 0 to 1
            public float EventAgeDurationOverride; // seconds
            public List<EffectPartBlock> Parts;
            public List<EffectAccelerationsBlock> Accelerations;
            public List<ParticleSystemDefinitionBlockNew> ParticleSystems;
            
            [Flags]
            public enum EventFlags : ushort
            {
                DisabledForDebugging = 1 << 0,
                ParticlesDieWhenEffectEnds = 1 << 1,
                LoopEventAgeDurationOverride = 1 << 2
            }
            
            [TagStructure(Size = 0x74)]
            public class EffectPartBlock : TagStructure
            {
                public EffectEnvironments CreateIn;
                public EffectDispositions ViolenceMode;
                public short Location;
                public short SecondaryLocation;
                public EffectPartFlags Flags;
                public GlobalEffectPriorityEnum Priority;
                // Currently, the only supported use is to turn parts off in first-person view.
                public EffectCameraModes CameraMode;
                public Tag RuntimeBaseGroupTag;
                public EffectpartGameModeDefinition GameMode;
                public GlobalDamageReportingEnum DamageReportingType;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(ValidTags = new [] { "rwrd","jpt!","drdf","sndo","decs","lens","ligh","gldf","ltvl","sefc","char","cpem","obje","shit","cmoe","snd!","trac","airs" })]
                public CachedTag Type;
                // initial velocity along the location's forward, for decals the distance at which decal is created (defaults to 0.5)
                public Bounds<float> VelocityBounds; // world units per second
                // NOT USED!
                public RealEulerAngles2d VelocityOrientation;
                // initial velocity will be inside the cone defined by this angle.
                public Angle VelocityConeAngle; // degrees
                public Bounds<Angle> AngularVelocityBounds; // degrees per second
                public Bounds<float> RadiusModifierBounds;
                public RealPoint3d RelativeOffset;
                public RealEulerAngles2d RelativeOrientation;
                public EffectPartScaleableValues AScalesValues;
                public EffectPartScaleableValues BScalesValues;
                // optional particleization effect definition, if you want this to particleize on spawn
                [TagField(ValidTags = new [] { "pman" })]
                public CachedTag Particleize;
                
                public enum EffectEnvironments : short
                {
                    AnyEnvironment,
                    AirOnly,
                    WaterOnly,
                    SpaceOnly,
                    WetOnly,
                    DryOnly
                }
                
                public enum EffectDispositions : short
                {
                    EitherMode,
                    ViolentModeOnly,
                    NonviolentModeOnly
                }
                
                [Flags]
                public enum EffectPartFlags : ushort
                {
                    FaceDownRegardlessOfLocation = 1 << 0,
                    OffsetOriginAwayFromGeometry = 1 << 1,
                    NeverAttachedToObject = 1 << 2,
                    DisabledForDebugging = 1 << 3,
                    DrawRegardlessOfDistance = 1 << 4,
                    MakeEveryTick = 1 << 5,
                    InheritParentVariant = 1 << 6,
                    BatchAoeDamage = 1 << 7,
                    CreateEvenWhenEventLoopsBackToSelf = 1 << 8,
                    FaceUpRegardlessOfLocation = 1 << 9,
                    // NOTE this is only for sound, and really only for firing effect sounds
                    SoundOnlyPlaysInKillcam = 1 << 10
                }
                
                public enum EffectCameraModes : sbyte
                {
                    IndependentOfCameraMode,
                    OnlyInFirstPerson,
                    OnlyInThirdPerson,
                    BothFirstAndThird
                }
                
                public enum EffectpartGameModeDefinition : sbyte
                {
                    Any,
                    CampaignOnly,
                    MultiplayerOnly,
                    CampaignOnlyNotCinematics,
                    CampaignCinematicsOnly,
                    CampaignSoloOnly
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
                public enum EffectPartScaleableValues : uint
                {
                    Velocity = 1 << 0,
                    VelocityDelta = 1 << 1,
                    VelocityConeAngle = 1 << 2,
                    AngularVelocity = 1 << 3,
                    AngularVelocityDelta = 1 << 4,
                    TypeSpecificScale = 1 << 5
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class EffectAccelerationsBlock : TagStructure
            {
                public EffectEnvironments CreateIn;
                public EffectDispositions ViolenceMode;
                public short Location;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float Acceleration;
                public float InnerConeAngle; // degrees
                public float OuterConeAngle; // degrees
                
                public enum EffectEnvironments : short
                {
                    AnyEnvironment,
                    AirOnly,
                    WaterOnly,
                    SpaceOnly,
                    WetOnly,
                    DryOnly
                }
                
                public enum EffectDispositions : short
                {
                    EitherMode,
                    ViolentModeOnly,
                    NonviolentModeOnly
                }
            }
            
            [TagStructure(Size = 0x7C)]
            public class ParticleSystemDefinitionBlockNew : TagStructure
            {
                public GlobalEffectPriorityEnum Priority;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(ValidTags = new [] { "prt3" })]
                public CachedTag Particle;
                public int Location;
                public CoordinateSystemEnum CoordinateSystem;
                public EffectEnvironments Environment;
                public EffectDispositions Disposition;
                public EffectCameraModes CameraMode;
                public EffectpartGameModeDefinition GameMode;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                // use values between -10 and 10 to move closer and farther from camera (positive is closer)
                public short SortBias;
                public ParticleSystemFlags Flags;
                // flag must be checked above
                public Bounds<float> PercentVelocityToInherit;
                // multiplied by all "size" related fields, like scale, velocity, acceleration
                public float SizeScale;
                // the particle is pushed away from the camera this distance (can be negative)
                public float CameraOffset; // world units
                public float PixelBudget; // ms
                // distance beyond cutoff over which particles fade
                public float NearFadeRange; // world units
                // distance in front of camera where fade is complete*
                public float NearFadeCutoff; // world units
                // distance in front of camera where fade is complete
                public float NearFadeOverride; // world units
                // distance before cutoff over which particles fade
                public float FarFadeRange; // world units
                // distance from camera where fade is complete
                public float FarFadeCutoff; // world units
                public float LodInDistance;
                // minimum is 0.0001
                public float LodFeatherInDelta;
                public float InverseLodFeatherIn;
                // defaults to 20.0
                public float LodOutDistance;
                // 0 defaults to 5.0, minimum is 0.0001
                public float LodFeatherOutDelta;
                public float InverseLodFeatherOut;
                public List<ParticleSystemEmitterDefinitionBlock> Emitters;
                public float RuntimeMaxLifespan;
                public float RuntimeOverdraw;
                
                public enum CoordinateSystemEnum : short
                {
                    World,
                    Local
                }
                
                public enum EffectEnvironments : short
                {
                    AnyEnvironment,
                    AirOnly,
                    WaterOnly,
                    SpaceOnly,
                    WetOnly,
                    DryOnly
                }
                
                public enum EffectDispositions : short
                {
                    EitherMode,
                    ViolentModeOnly,
                    NonviolentModeOnly
                }
                
                public enum EffectCameraModes : short
                {
                    IndependentOfCameraMode,
                    OnlyInFirstPerson,
                    OnlyInThirdPerson,
                    BothFirstAndThird
                }
                
                public enum EffectpartGameModeDefinition : sbyte
                {
                    Any,
                    CampaignOnly,
                    MultiplayerOnly,
                    CampaignOnlyNotCinematics,
                    CampaignCinematicsOnly,
                    CampaignSoloOnly
                }
                
                [Flags]
                public enum ParticleSystemFlags : uint
                {
                    ParticlesFreezeWhenOffscreen = 1 << 0,
                    ParticlesContinueAsUsualWhenOffscreen = 1 << 1,
                    LodAlways10 = 1 << 2,
                    LodSameInSplitscreen = 1 << 3,
                    DisabledInAnySplitscreen = 1 << 4,
                    DisabledIn3And4WaySplitscreen = 1 << 5,
                    DisabledForDebugging = 1 << 6,
                    InheritEffectVelocity = 1 << 7,
                    DonTRenderSystem = 1 << 8,
                    RenderWhenZoomed = 1 << 9,
                    ForceCpuUpdating = 1 << 10,
                    ForceGpuUpdating = 1 << 11,
                    OverrideNearFade = 1 << 12,
                    ParticlesDieWhenEffectEnds = 1 << 13,
                    // synchronized across particle systems
                    UseSynchronizedRandomSeed = 1 << 14,
                    // particle system uses local-space position but up is always 'global up'
                    UseWorldOrientation = 1 << 15,
                    // first particle spawned renders first (at the back), last particle spawned renders last (front)
                    RenderInSpawnOrder = 1 << 16,
                    // use distance and multiplier (below) to tune high- or low-res rendering
                    DynamicParticleResolution = 1 << 17,
                    ParticlesLiveForever = 1 << 18,
                    DisableInVisionMode = 1 << 19,
                    DisableVelocity = 1 << 20,
                    DisableWhenZoomed = 1 << 21,
                    ContinueWhenOffscreenIgnoresPerformanceThrottlesTag = 1 << 22
                }
                
                [TagStructure(Size = 0x3C4)]
                public class ParticleSystemEmitterDefinitionBlock : TagStructure
                {
                    public StringId EmitterName;
                    public EmissionShapeEnum EmissionShape;
                    public EmitterFlags Flags;
                    public VisibleEmitterFlags EmitterFlags1;
                    public EmissionAxisEnum ParticleAxis;
                    public EmissionReferenceAxisEnum ParticleReferenceAxis;
                    [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    [TagField(ValidTags = new [] { "pecp" })]
                    public CachedTag CustomShape;
                    [TagField(ValidTags = new [] { "ebhd" })]
                    public CachedTag BoatHull;
                    // used if override is zero
                    public float BoundingRadiusEstimate; // world units
                    // used if non-zero
                    public float BoundingRadiusOverride; // world units
                    // NOTE - setting this will break automatic bounding sphere calculation, you must enter radius manually
                    public RealPoint3d AxisScale;
                    public RealVector2d UvScrolling; // tiles per second
                    // XYZ controls that offset the emitter's origin from the original location
                    public ParticlePropertyRealPoint3dStructNew TranslationalOffset; // world units
                    // yaw/pitch that changes the initial rotation of the emitter
                    public ParticlePropertyRealEulerAngles2dStructNew RelativeDirection;
                    // defines the size of the emitter
                    public ParticlePropertyScalarStructNew EmissionRadius; // world units
                    // determines the angle at which particles are emitted
                    public ParticlePropertyScalarStructNew EmissionAngle; // degrees
                    // determines the max tilt for particle axis
                    public ParticlePropertyScalarStructNew EmissionAxisAngle; // degrees
                    // number of particles that are spawned at the birth of the effect
                    public ParticlePropertyScalarStructNew ParticleStartingCount;
                    // max number of particles allowed to exist at one time
                    public ParticlePropertyScalarStructNew ParticleMaxCount; // 0=unlimited
                    // number of particles that are spawned every second from the emitters
                    public ParticlePropertyScalarStructNew ParticleEmissionRate; // particles per second
                    // number of particles that are spawned every world unit of motion from the emitters
                    public ParticlePropertyScalarStructNew ParticleEmissionPerDistance; // particles per world unit
                    // the number of seconds a particle will live after emission
                    public ParticlePropertyScalarStructNew ParticleLifespan; // seconds
                    public ParticlePhysicsStruct ParticleMovement;
                    public List<EmitterglobalForceBlock> ParticleAttractorRepulsor;
                    public List<EmitterclipSphereBlock> ParticleClipSphere;
                    public ParticlePropertyRealVector3dStructNew ParticleSelfAcceleration; // world units per second per second
                    public ParticlePropertyScalarStructNew ParticleInitialVelocity; // world units per second
                    public ParticlePropertyScalarStructNew ParticleRotation; // .25=90�, .5=180�, 1=360� ... adds to physics
                    public ParticlePropertyScalarStructNew ParticleInitialRotationRate; // 360 degree rotations per second
                    public ParticlePropertyScalarStructNew ParticleSize; // world units
                    public ParticlePropertyScalarStructNew ParticleScale; // multiple of size
                    public ParticlePropertyScalarStructNew ParticleScaleX; // multiple of size
                    public ParticlePropertyScalarStructNew ParticleScaleY; // multiple of size
                    // controls the overall tint of the particle
                    public ParticlePropertyColorStructNew ParticleTint; // RGB
                    public ParticlePropertyScalarStructNew ParticleAlpha;
                    public ParticlePropertyScalarStructNew ParticleAlphaBlackPoint; // 0=normal, 1=clamped
                    public ParticlePropertyScalarStructNew ParticleAlphaWhitePoint; // 1=normal, 0=clamped
                    public int RuntimeMConstantPerParticleProperties;
                    public int RuntimeMConstantOverTimeProperties;
                    public int RuntimeMUsedParticleStates;
                    public GpuPropertyFunctionColorStruct RuntimeMGpuData;
                    
                    public enum EmissionShapeEnum : sbyte
                    {
                        Sprayer,
                        Disc,
                        Globe,
                        Implode,
                        Tube,
                        Halo,
                        ImpactContour,
                        ImpactArea,
                        Debris,
                        Line,
                        BreakableSurface,
                        CustomPoints,
                        BoatHullSurface,
                        Cube,
                        Cylinder,
                        UnweightedLine,
                        Plane,
                        Jetwash,
                        PlanarOrbit,
                        SphereOrbit
                    }
                    
                    [Flags]
                    public enum EmitterFlags : byte
                    {
                    }
                    
                    [Flags]
                    public enum VisibleEmitterFlags : byte
                    {
                        // By default, particles emit radially away from the emitter center.  This option gives them random velocity instead.
                        // This only applies to volume emitters.
                        VolumeEmitterParticleVelocitiesAreRandom = 1 << 0
                    }
                    
                    public enum EmissionAxisEnum : sbyte
                    {
                        Constant,
                        Cone,
                        Disc,
                        Globe
                    }
                    
                    public enum EmissionReferenceAxisEnum : sbyte
                    {
                        X,
                        Y,
                        Z
                    }
                    
                    [TagStructure(Size = 0x38)]
                    public class ParticlePropertyRealPoint3dStructNew : TagStructure
                    {
                        public GameStateTypeEnum InputVariable;
                        public GameStateTypeEnum RangeVariable;
                        public OutputModEnum OutputModifier;
                        public GameStateTypeEnum OutputModifierInput;
                        public MappingFunction Mapping;
                        public float RuntimeMConstantValue;
                        public ushort RuntimeMFlags;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        public RealPoint3d StartingInterpolant;
                        public RealPoint3d EndingInterpolant;
                        
                        public enum GameStateTypeEnum : sbyte
                        {
                            ParticleAge,
                            SystemAge,
                            ParticleRandom,
                            SystemRandom,
                            ParticleCorrelation1,
                            ParticleCorrelation2,
                            ParticleCorrelation3,
                            ParticleCorrelation4,
                            SystemCorrelation1,
                            SystemCorrelation2,
                            ParticleEmissionTime,
                            LocationLod,
                            GameTime,
                            EffectAScale,
                            EffectBScale,
                            ParticleRotation,
                            LocationRandom,
                            DistanceFromEmitter,
                            ParticleSimulationA,
                            ParticleSimulationB,
                            ParticleVelocity,
                            InvalidStatePleaseSetAgain
                        }
                        
                        public enum OutputModEnum : sbyte
                        {
                            Unknown,
                            Plus,
                            Times
                        }
                        
                        [TagStructure(Size = 0x14)]
                        public class MappingFunction : TagStructure
                        {
                            public byte[] Data;
                        }
                    }
                    
                    [TagStructure(Size = 0x30)]
                    public class ParticlePropertyRealEulerAngles2dStructNew : TagStructure
                    {
                        public GameStateTypeEnum InputVariable;
                        public GameStateTypeEnum RangeVariable;
                        public OutputModEnum OutputModifier;
                        public GameStateTypeEnum OutputModifierInput;
                        public MappingFunction Mapping;
                        public float RuntimeMConstantValue;
                        public ushort RuntimeMFlags;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        public RealEulerAngles2d DirectionAt0;
                        public RealEulerAngles2d DirectionAt1;
                        
                        public enum GameStateTypeEnum : sbyte
                        {
                            ParticleAge,
                            SystemAge,
                            ParticleRandom,
                            SystemRandom,
                            ParticleCorrelation1,
                            ParticleCorrelation2,
                            ParticleCorrelation3,
                            ParticleCorrelation4,
                            SystemCorrelation1,
                            SystemCorrelation2,
                            ParticleEmissionTime,
                            LocationLod,
                            GameTime,
                            EffectAScale,
                            EffectBScale,
                            ParticleRotation,
                            LocationRandom,
                            DistanceFromEmitter,
                            ParticleSimulationA,
                            ParticleSimulationB,
                            ParticleVelocity,
                            InvalidStatePleaseSetAgain
                        }
                        
                        public enum OutputModEnum : sbyte
                        {
                            Unknown,
                            Plus,
                            Times
                        }
                        
                        [TagStructure(Size = 0x14)]
                        public class MappingFunction : TagStructure
                        {
                            public byte[] Data;
                        }
                    }
                    
                    [TagStructure(Size = 0x20)]
                    public class ParticlePropertyScalarStructNew : TagStructure
                    {
                        public GameStateTypeEnum InputVariable;
                        public GameStateTypeEnum RangeVariable;
                        public OutputModEnum OutputModifier;
                        public GameStateTypeEnum OutputModifierInput;
                        public MappingFunction Mapping;
                        public float RuntimeMConstantValue;
                        public ushort RuntimeMFlags;
                        public ForceFlags ForceFlags1;
                        [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        
                        public enum GameStateTypeEnum : sbyte
                        {
                            ParticleAge,
                            SystemAge,
                            ParticleRandom,
                            SystemRandom,
                            ParticleCorrelation1,
                            ParticleCorrelation2,
                            ParticleCorrelation3,
                            ParticleCorrelation4,
                            SystemCorrelation1,
                            SystemCorrelation2,
                            ParticleEmissionTime,
                            LocationLod,
                            GameTime,
                            EffectAScale,
                            EffectBScale,
                            ParticleRotation,
                            LocationRandom,
                            DistanceFromEmitter,
                            ParticleSimulationA,
                            ParticleSimulationB,
                            ParticleVelocity,
                            InvalidStatePleaseSetAgain
                        }
                        
                        public enum OutputModEnum : sbyte
                        {
                            Unknown,
                            Plus,
                            Times
                        }
                        
                        [Flags]
                        public enum ForceFlags : byte
                        {
                            ForceConstant = 1 << 0
                        }
                        
                        [TagStructure(Size = 0x14)]
                        public class MappingFunction : TagStructure
                        {
                            public byte[] Data;
                        }
                    }
                    
                    [TagStructure(Size = 0x34)]
                    public class ParticlePhysicsStruct : TagStructure
                    {
                        [TagField(ValidTags = new [] { "pmov" })]
                        public CachedTag Template;
                        public ParticleMovementFlags Flags;
                        public sbyte CollisionControllerIndex;
                        public sbyte TurbulenceControllerIndex;
                        public sbyte GlobalForceControllerIndex;
                        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        public List<ParticleController> Movements;
                        [TagField(ValidTags = new [] { "bitm" })]
                        public CachedTag TurbulenceTexture;
                        
                        [Flags]
                        public enum ParticleMovementFlags : ushort
                        {
                            Physics = 1 << 0,
                            CollideWithStructure = 1 << 1,
                            CollideWithWater = 1 << 2,
                            CollideWithScenery = 1 << 3,
                            CollideWithVehicles = 1 << 4,
                            CollideWithBipeds = 1 << 5,
                            AlwaysCollideEveryFrame = 1 << 6,
                            Swarm = 1 << 7,
                            Wind = 1 << 8,
                            Turbulence = 1 << 9,
                            GlobalForce = 1 << 10,
                            DisableSwarmCollision = 1 << 11
                        }
                        
                        [TagStructure(Size = 0x18)]
                        public class ParticleController : TagStructure
                        {
                            public ParticleMovementType Type;
                            public ParticleControllerFlags Flags;
                            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                            public byte[] Padding;
                            public List<ParticleControllerParameters> Parameters;
                            public int RuntimeMConstantParameters;
                            public int RuntimeMUsedParticleStates;
                            
                            public enum ParticleMovementType : short
                            {
                                Physics,
                                Collider,
                                Swarm,
                                Wind,
                                Turbulence,
                                GlobalForce
                            }
                            
                            [Flags]
                            public enum ParticleControllerFlags : byte
                            {
                                PropertiesFullyIndexed = 1 << 0
                            }
                            
                            [TagStructure(Size = 0x24)]
                            public class ParticleControllerParameters : TagStructure
                            {
                                public int ParameterId;
                                public ParticlePropertyScalarStructNew Property;
                            }
                        }
                    }
                    
                    [TagStructure(Size = 0x28)]
                    public class EmitterglobalForceBlock : TagStructure
                    {
                        public EffectGlobalForce GlobalForce;
                        public RealVector3d Offset;
                        public RealVector3d Direction;
                    }
                    
                    [TagStructure(Size = 0x10)]
                    public class EmitterclipSphereBlock : TagStructure
                    {
                        public RealVector3d Offset;
                        public float Radius;
                    }
                    
                    [TagStructure(Size = 0x38)]
                    public class ParticlePropertyRealVector3dStructNew : TagStructure
                    {
                        public GameStateTypeEnum InputVariable;
                        public GameStateTypeEnum RangeVariable;
                        public OutputModEnum OutputModifier;
                        public GameStateTypeEnum OutputModifierInput;
                        public MappingFunction Mapping;
                        public float RuntimeMConstantValue;
                        public ushort RuntimeMFlags;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        public RealVector3d StartingInterpolant;
                        public RealVector3d EndingInterpolant;
                        
                        public enum GameStateTypeEnum : sbyte
                        {
                            ParticleAge,
                            SystemAge,
                            ParticleRandom,
                            SystemRandom,
                            ParticleCorrelation1,
                            ParticleCorrelation2,
                            ParticleCorrelation3,
                            ParticleCorrelation4,
                            SystemCorrelation1,
                            SystemCorrelation2,
                            ParticleEmissionTime,
                            LocationLod,
                            GameTime,
                            EffectAScale,
                            EffectBScale,
                            ParticleRotation,
                            LocationRandom,
                            DistanceFromEmitter,
                            ParticleSimulationA,
                            ParticleSimulationB,
                            ParticleVelocity,
                            InvalidStatePleaseSetAgain
                        }
                        
                        public enum OutputModEnum : sbyte
                        {
                            Unknown,
                            Plus,
                            Times
                        }
                        
                        [TagStructure(Size = 0x14)]
                        public class MappingFunction : TagStructure
                        {
                            public byte[] Data;
                        }
                    }
                    
                    [TagStructure(Size = 0x20)]
                    public class ParticlePropertyColorStructNew : TagStructure
                    {
                        public GameStateTypeEnum InputVariable;
                        public GameStateTypeEnum RangeVariable;
                        public OutputModEnum OutputModifier;
                        public GameStateTypeEnum OutputModifierInput;
                        public MappingFunction Mapping;
                        public float RuntimeMConstantValue;
                        public ushort RuntimeMFlags;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        
                        public enum GameStateTypeEnum : sbyte
                        {
                            ParticleAge,
                            SystemAge,
                            ParticleRandom,
                            SystemRandom,
                            ParticleCorrelation1,
                            ParticleCorrelation2,
                            ParticleCorrelation3,
                            ParticleCorrelation4,
                            SystemCorrelation1,
                            SystemCorrelation2,
                            ParticleEmissionTime,
                            LocationLod,
                            GameTime,
                            EffectAScale,
                            EffectBScale,
                            ParticleRotation,
                            LocationRandom,
                            DistanceFromEmitter,
                            ParticleSimulationA,
                            ParticleSimulationB,
                            ParticleVelocity,
                            InvalidStatePleaseSetAgain
                        }
                        
                        public enum OutputModEnum : sbyte
                        {
                            Unknown,
                            Plus,
                            Times
                        }
                        
                        [TagStructure(Size = 0x14)]
                        public class MappingFunction : TagStructure
                        {
                            public byte[] Data;
                        }
                    }
                    
                    [TagStructure(Size = 0x24)]
                    public class GpuPropertyFunctionColorStruct : TagStructure
                    {
                        public List<GpuPropertyBlock> RuntimeGpuPropertyBlock;
                        public List<GpuFunctionBlock> RuntimeGpuFunctionsBlock;
                        public List<GpuColorBlock> RuntimeGpuColorsBlock;
                        
                        [TagStructure(Size = 0x10)]
                        public class GpuPropertyBlock : TagStructure
                        {
                            [TagField(Length = 4)]
                            public GpuPropertySubArray[]  RuntimeGpuPropertySubArray;
                            
                            [TagStructure(Size = 0x4)]
                            public class GpuPropertySubArray : TagStructure
                            {
                                public float RuntimeGpuPropertyReal;
                            }
                        }
                        
                        [TagStructure(Size = 0x40)]
                        public class GpuFunctionBlock : TagStructure
                        {
                            [TagField(Length = 16)]
                            public GpuFunctionSubArray[]  RuntimeGpuFunctionSubArray;
                            
                            [TagStructure(Size = 0x4)]
                            public class GpuFunctionSubArray : TagStructure
                            {
                                public float RuntimeGpuFunctionReal;
                            }
                        }
                        
                        [TagStructure(Size = 0x10)]
                        public class GpuColorBlock : TagStructure
                        {
                            [TagField(Length = 4)]
                            public GpuColorSubArray[]  RuntimeGpuColorSubArray;
                            
                            [TagStructure(Size = 0x4)]
                            public class GpuColorSubArray : TagStructure
                            {
                                public float RuntimeGpuColorReal;
                            }
                        }
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class EffectLoopingSoundBlockStruct : TagStructure
        {
            [TagField(ValidTags = new [] { "lsnd" })]
            public CachedTag LoopingSound;
            public short Location;
            public short BindScaleToEvent;
        }
        
        [TagStructure(Size = 0xC)]
        public class EffectConicalDistributionBlock : TagStructure
        {
            public short YawCount;
            public short PitchCount;
            // exp==.5 even distribution, exp>.5== tighter
            public float DistributionExponent;
            // degrees
            public Angle Spread;
        }
    }
}
