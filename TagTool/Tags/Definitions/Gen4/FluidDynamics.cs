using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "fluid_dynamics", Tag = "fldy", Size = 0x178)]
    public class FluidDynamics : TagStructure
    {
        public FluidDynamicsFlags Flags;
        public StringId MarkerAttachmentName;
        // 0.0000001->0.0000100
        public float DiffusionRate;
        // 0.0000001->0.0060000
        public float Viscosity;
        [TagField(Length = 0x28, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        // grid cells across
        public short NumberGridCellsWidth;
        // game updates per velocity field update
        public short UpdateRate;
        public float ParticleGravityMultiplier;
        // world units
        public float GridCellWidth;
        // world units
        public float GridCellHeight;
        // away randomly
        public short MaximumParticlesPerCell;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        [TagField(Length = 0x4C, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        public short NumberOfTracers;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding3;
        public Bounds<float> TracerTimeMultipierRange;
        // 1-32
        public float TracerMaxVelocityInfluenceScale;
        [TagField(Length = 0x24, Flags = TagFieldFlags.Padding)]
        public byte[] Padding4;
        // should = number of particles in the effect
        public short NumberOfParticles;
        // game updates per collision update
        public short ParticleCollisionRate;
        // world units
        public float ParticleCollisionRadius;
        // 0.001-5.0
        public float ParticleMovementMultiplier;
        // 0.001-5.0
        public float ParticleForceToCenterOfGridScale;
        // 0-1
        public float ParticleDragCoeff;
        public float MaximumParticleVelocity;
        [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
        public byte[] Padding5;
        public List<ParticleSystemDefinitionBlockNew> ParticleEffects;
        [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
        public byte[] Padding6;
        public List<FluidDynamicsSquirterBlock> Squirters;
        public List<FluidDynamicsInflowBlock> Inflow;
        public List<FluidDynamicsOutflowBlock> Outflow;
        public List<FluidDynamicsObstacleBlock> Obstacles;
        public GSingleScalarFunctionEvaluationStruct CylindricalRadius;
        
        [Flags]
        public enum FluidDynamicsFlags : uint
        {
            UseTracers = 1 << 0,
            UseDensityDiffuse = 1 << 1,
            WrapParticlesWidth = 1 << 2,
            WrapParticlesHeight = 1 << 3,
            FlatHorizontal = 1 << 4,
            FlatVertical = 1 << 5,
            CylindricalHorizontal = 1 << 6,
            CylindricalVertical = 1 << 7,
            ApproximateVolume = 1 << 8,
            RandomParticlePlacement = 1 << 9,
            TracerJumpAround = 1 << 10
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
            
            public enum GlobalEffectPriorityEnum : sbyte
            {
                Low,
                Normal,
                AboveNormal,
                High,
                VeryHigh,
                Essential
            }
            
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
                    public EffectGlobalForceStruct GlobalForce;
                    public RealVector3d Offset;
                    public RealVector3d Direction;
                    
                    [TagStructure(Size = 0x10)]
                    public class EffectGlobalForceStruct : TagStructure
                    {
                        public GlobalforceFlags Flags;
                        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        // positive pushes out, negative pulls in
                        public float ForceStrength;
                        // doesn't work on cylinders, due to shader constant constraints
                        public float SphereFalloffBegin; // wus
                        // doesn't work on cylinders, due to shader constant constraints
                        public float SphereFalloffEnd; // wus
                        
                        [Flags]
                        public enum GlobalforceFlags : byte
                        {
                            IsInfinitelyLongCylinder = 1 << 0
                        }
                    }
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
        
        [TagStructure(Size = 0x38)]
        public class FluidDynamicsSquirterBlock : TagStructure
        {
            public Point2d GridLocation;
            [TagField(Length = 0x30, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float SquirtScale;
        }
        
        [TagStructure(Size = 0x10)]
        public class FluidDynamicsInflowBlock : TagStructure
        {
            public Point2d GridLocation;
            // particles/sec
            public float InflowRate;
            // degrees
            public float InflowAngle;
            public float InflowVelocityScale;
        }
        
        [TagStructure(Size = 0x4)]
        public class FluidDynamicsOutflowBlock : TagStructure
        {
            public Point2d GridLocation;
        }
        
        [TagStructure(Size = 0x4)]
        public class FluidDynamicsObstacleBlock : TagStructure
        {
            public Point2d _2x2GridLocation;
        }
        
        [TagStructure(Size = 0x28)]
        public class GSingleScalarFunctionEvaluationStruct : TagStructure
        {
            public float Input;
            public MappingFunction Scalar;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            [TagStructure(Size = 0x14)]
            public class MappingFunction : TagStructure
            {
                public byte[] Data;
            }
        }
    }
}
