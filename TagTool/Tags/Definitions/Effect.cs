using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "effect", Tag = "effe", Size = 0x68, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "effect", Tag = "effe", Size = 0x70, MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline106708)]
    [TagStructure(Name = "effect", Tag = "effe", Size = 0x60, MinVersion = CacheVersion.HaloReach)]
    public class Effect
    {
        public EffectFlags Flags;
        public uint FixedRandomSeed;
        public float OverlapThreshold;
        public float ContinueIfWithin;
        public float DeathDelay;
        [TagField(MaxVersion = CacheVersion.HaloOnline106708)]
        public sbyte Unknown1;
        [TagField(MaxVersion = CacheVersion.HaloOnline106708)]
        public sbyte Unknown2;
        [TagField(MaxVersion = CacheVersion.HaloOnline106708)]
        public sbyte Unknown3;
        [TagField(MaxVersion = CacheVersion.HaloOnline106708)]
        public sbyte Unknown4;
        public short LoopStartEvent;
        public EffectPriority Priority;
        public uint Unknown5;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint Unknown6;
        public List<Location> Locations;
        public List<Event> Events;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<LoopingSounds> LoopingSoundBlock;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTagInstance LoopingSound;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public sbyte LocationIndex;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public sbyte EventIndex;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public short Unknown11;
        public float AlwaysPlayDistance;
        public float NeverPlayDistance;
        public float RuntimeLightprobeDeathDelay;
        public float RuntimeLocalSpaceDeathDelay;
        public List<ConicalDistribution> ConicalDistributions;

        [TagField(Padding = true, Length = 8, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;

        [TagStructure(Size = 0xC)]
        [TagStructure(Size = 0x8, MinVersion = CacheVersion.HaloReach)]
        public class Location
        {
            public StringId MarkerName;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public int Unknown1;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public sbyte Unknown2;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public sbyte Unknown3;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public sbyte Unknown4;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public sbyte Unknown5;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public Int16 Flags;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public EffectPriority Priority;
        }

        [TagStructure(Size = 0x44)]
        [TagStructure(Size = 0x40, MinVersion = CacheVersion.HaloReach)]
        public class Event
        {
            public StringId Name;
            public int Unknown;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public sbyte Unknown2;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public sbyte Unknown3;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public sbyte Unknown4;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public sbyte Unknown5;
            public float SkipFraction;
            public float DelayBoundsMin;
            public float DelayBoundsMax;
            public float DurationBoundsMin;
            public float DurationBoundsMax;
            public List<Part> Parts;
            public List<Acceleration> Accelerations;
            public List<ParticleSystem> ParticleSystems;

            [TagStructure(Size = 0x60)]
            [TagStructure(Size = 0x64, MinVersion = CacheVersion.HaloReach)]
            public class Part
            {
                public EffectEnvironment CreateInEnvironment;
                public EffectViolenceMode CreateInDisposition;
                public short PrimaryLocation;
                public short SecondaryLocation;
                public EffectEventPartFlags Flags;
                public EffectEventPriority Priority;
                public EffectEventPartCameraMode CameraMode;
                public Tag RuntimeBaseGroupTag;
                [TagField(Padding = true, Length = 4, MinVersion = CacheVersion.HaloReach)]
                public byte[] Unused;
                public CachedTagInstance Type;
                public Bounds<float> VelocityBounds;
                public RealEulerAngles2d VelocityOrientation;
                public Angle VelocityConeAngle;
                public Bounds<Angle> AngularVelocityBounds;
                public Bounds<float> RadiusModifierBounds;
                public RealPoint3d RelativeOffset;
                public RealEulerAngles2d RelativeOrientation;
                public EffectEventPartScales AScalesValues;
                public EffectEventPartScales BScalesValues;
            }

            [TagStructure(Size = 0x14)]
            public class Acceleration
            {
                public CreateInEnvironmentValue CreateInEnvironment;
                public CreateInDispositionValue CreateInDisposition;

                public short LocationIndex;
                [TagField(Padding = true, Length = 2)]
                public byte[] Unused;

                public float AccelerationAmount;
                public float InnerConeAngle;
                public float OuterConeAngle;

                public enum CreateInEnvironmentValue : short
                {
                    AnyEnvironment,
                    AirOnly,
                    WaterOnly,
                    SpaceOnly
                }

                public enum CreateInDispositionValue : short
                {
                    EitherMode,
                    ViolentModeOnly,
                    NonviolentModeOnly
                }
            }

            [TagStructure(Size = 0x5C, MaxVersion = CacheVersion.HaloOnline106708)]
            [TagStructure(Size = 0x70, MinVersion = CacheVersion.HaloReach)]
            public class ParticleSystem
            {
                public EffectEventPriority Priority;
                public sbyte GameMode;
                public sbyte Unknown3;
                public sbyte Unknown4;
                public CachedTagInstance Particle;
                public uint LocationIndex;
                public ParticleCoordinateSystem CoordinateSystem;
                public EffectEnvironment Environment;
                public EffectViolenceMode Disposition;
                public ParticleCameraMode CameraMode;
                public short SortBias;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public short Unknownshort;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public short Unknownshort2;
                public ushort Flags;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public float Unknown18;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public float Unknown19;
                public float Unknown6;
                public float Unknown7;
                public float Unknown8;
                public float Unknown9;
                public uint Unknown10;
                public float Unknown11;
                public float AmountSize;
                public float Unknown12;
                public float LodInDistance;
                public float LodFeatherInDelta;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public float Unknown15;
                public List<Emitter> Emitters;
                public float Unknown16;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public float Unknown17;

                [TagStructure(Size = 0x2F0, MaxVersion = CacheVersion.Halo3Retail)]
                [TagStructure(Size = 0x300, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline106708)]
                [TagStructure(Size = 0x330, MinVersion = CacheVersion.HaloReach)]
                public class Emitter
                {
                    public StringId Name;
                    public byte Version;
                    public EmissionShapeValue EmissionShape;

                    [TagField(Padding = true, Length = 1)]
                    public byte[] Unused;

                    public FlagsValue EmitterFlags;

                    [TagField(MinVersion = CacheVersion.HaloReach)]
                    public float Unknown2;

                    [TagField(MinVersion = CacheVersion.Halo3ODST)]
                    public CachedTagInstance CustomShape;

                    [TagField(MinVersion = CacheVersion.HaloReach)]
                    public CachedTagInstance BoatHull;

                    public uint Unknown3;
                    public uint Unknown4;
                    public uint Unknown5;
                    public uint Unknown6;

                    [TagField(MinVersion = CacheVersion.HaloReach)]
                    public uint Unknown7;
                    [TagField(MinVersion = CacheVersion.HaloReach)]
                    public uint Unknown8;
                    [TagField(MinVersion = CacheVersion.HaloReach)]
                    public uint Unknown9;

                    /// <summary>
                    /// XYZ controls that offset the emitter's origin from the original location.
                    /// </summary>
                    [TagField(Format = "World Units")]
                    public TranslationalOffsetData TranslationalOffset;

                    /// <summary>
                    /// Yaw/Pitch that changes the initial rotation of the emitter.
                    /// </summary>
                    public RelativeDirectionData RelativeDirection;

                    /// <summary>
                    /// Defines the size of the emitter.
                    /// </summary>
                    [TagField(Format = "World Units")]
                    public TagMapping EmissionRadius;

                    /// <summary>
                    /// Determines the angle at which particles are emitted.
                    /// </summary>
                    [TagField(Format = "Degrees")]
                    public TagMapping EmissionAngle;

                    /// <summary>
                    /// Number of particles that are spawned at the birth of the effect.
                    /// </summary>
                    public TagMapping ParticleStartingCount;

                    /// <summary>
                    /// Max number of particles allowed to exist at one time.
                    /// </summary>
                    [TagField(Format = "0 = Unlimited")]
                    public TagMapping ParticleMaxCount;

                    /// <summary>
                    /// Number of particles that are spawned every second from the emitters.
                    /// </summary>
                    [TagField(Format = "Particles Per Second")]
                    public TagMapping ParticleEmissionRate;

                    /// <summary>
                    /// Number of particles that are spawned every world unit of motion from the emitters.
                    /// </summary>
                    [TagField(Format = "Particles Per World Unit")]
                    public TagMapping ParticleEmissionPerDistance;

                    /// <summary>
                    /// Number of seconds a particle will live after emission.
                    /// </summary>
                    [TagField(Format = "Seconds")]
                    public TagMapping ParticleLifespan;

                    public ParticleMovementData ParticleMovement;

                    [TagField(Format = "World Units Per Second Per Second")]
                    public ParticleSelfAccelerationData ParticleSelfAcceleration;

                    [TagField(Format = "World Units Per Second")]
                    public TagMapping ParticleVelocity;

                    [TagField(Format = "360 Degree Rotations Per Second")]
                    public TagMapping ParticleAngularVelocity;

                    public TagMapping ParticleMass;
                    public TagMapping ParticleDragCoefficient;

                    [TagField(Format = "World Units")]
                    public TagMapping ParticleSize;

                    [TagField(Format = "RGB")]
                    public TagMapping ParticleTint;

                    public TagMapping ParticleAlpha;

                    [TagField(Format = "0 = Normal, 1 = Clamped")]
                    public TagMapping ParticleAlphaBlackPoint;

                    public RuntimeMGpuData RuntimeMGpu;

                    public enum EmissionShapeValue : sbyte
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
                        CustomPoints,
                        BoatHullSurface,
                        Cube,
                        Cylinder,
                        UnweightedLine,
                        Plane,
                        Jetwash,
                        PlanarOrbit,
                        SphereOrbit,
                        PlaneSpray
                    }

                    [Flags]
                    public enum FlagsValue : byte
                    {
                        None,
                        VolumeEmitterParticleVelocitiesAreRandom = 1 << 0,
                        ClampParticleVelocities = 1 << 1,
                        ParticleEmittedInsideShape = 1 << 2,
                        OverrideParticleDirection = 1 << 3
                    }

                    [TagStructure(Size = 0x38)]
                    public struct TranslationalOffsetData
                    {
                        public TagMapping Mapping;
                        public RealPoint3d StartingInterpolant;
                        public RealPoint3d EndingInterpolant;
                    }

                    [TagStructure(Size = 0x38)]
                    public struct RelativeDirectionData
                    {
                        public TagMapping Mapping;
                        public RealEulerAngles3d DirectionAt0;
                        public RealEulerAngles3d DirectionAt1;
                    }

                    [TagStructure(Size = 0x20, MaxVersion = CacheVersion.HaloOnline700123)]
                    [TagStructure(Size = 0x30, MinVersion = CacheVersion.HaloReach)]
                    public struct ParticleMovementData
                    {
                        public CachedTagInstance Template;

                        public FlagsValue Flags;

                        public List<Movement> Movements;

                        [TagField(ValidTags = new[] { "bitm" }, MinVersion = CacheVersion.HaloReach)]
                        public CachedTagInstance TurbulenceTexture;

                        [Flags]
                        public enum FlagsValue : int
                        {
                            None,
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
                            DisableSwarmCollision = 1 << 11,
                        }

                        [TagStructure(Size = 0x18)]
                        public class Movement
                        {
                            public TypeValue Tyoe;
                            public FlagsValue Flags;

                            [TagField(Padding = true, Length = 1)]
                            public byte[] Unused;

                            public List<Parameter> Parameters;

                            public int RuntimeMConstantParameters;
                            public int RuntimeMUsedParticleStates;

                            public enum TypeValue : short
                            {
                                Physics,
                                Collider,
                                Swarm,
                                Wind,
                                Turbulence,
                                GlobalForce,
                            }

                            [Flags]
                            public enum FlagsValue : byte
                            {
                                Sphere,
                                Cylinder,
                                Plane
                            }

                            [TagStructure(Size = 0x24)]
                            public class Parameter
                            {
                                public int ParameterId;
                                public TagMapping Property;
                            }
                        }
                    }

                    [TagStructure(Size = 0x38)]
                    public struct ParticleSelfAccelerationData
                    {
                        public TagMapping Mapping;
                        public RealVector3d StartingInterpolant;
                        public RealVector3d EndingInterpolant;
                    }

                    [TagStructure(Size = 0x30)]
                    public struct RuntimeMGpuData
                    {
                        public int ConstantPerParticleProperties;
                        public int ConstantOverTimeProperties;
                        public int UsedParticleStates;
                        public List<Property> Properties;
                        public List<Function> Functions;
                        public List<Color> Colors;

                        [TagStructure(Size = 0x10)]
                        public struct Property
                        {
                            [TagField(Length = 4)]
                            public float[] Data;
                        }

                        [TagStructure(Size = 0x40)]
                        public struct Function
                        {
                            [TagField(Length = 16)]
                            public float[] Data;
                        }

                        [TagStructure(Size = 0x10)]
                        public struct Color
                        {
                            [TagField(Length = 4)]
                            public float[] Data;
                        }
                    }
                }
            }
        }

        [TagStructure(Size = 0xC)]
        public class ConicalDistribution
        {
            public short ProjectionYawCount;
            public short ProjectionPitchCount;
            public float DistributionExponent;
            public Angle Spread;
        }

        [TagStructure(Size = 0x14, MinVersion = CacheVersion.HaloReach)]
        public class LoopingSounds
        {
            public CachedTagInstance LoopingSound;
            public sbyte LocationIndex;
            public sbyte EventIndex;
            public short BindScale;
        }
    }

    [Flags]
    public enum EffectFlags : int
    {
        None,
        DeletedWhenAttachmentDeactivates = 1 << 0,
        RunEventsInParallel = 1 << 1,
        DoNotReUsePartsWhenLooping = 1 << 2,
        AgeCreatorSWeapon = 1 << 3,
        UseParentPositionButWorldOrientation = 1 << 4,
        CanPenetrateWalls = 1 << 5,
        CannotBeRestarted = 1 << 6,
        ForceUseOwnLightprobe = 1 << 7,
        ForceLooping = 1 << 8,
        ObsoleteEffectOrdnanceIsGone = 1 << 9,
        RenderInHologramPass = 1 << 10,
        LightprobeOnlySampleAirprobes = 1 << 11,
        PlayEffectEvenOutsideBsps = 1 << 12,
        DrawLensFlaresWhenStopped = 1 << 13,
        KillParticlesWhenStopped = 1 << 14,
        PlayEvenOnHiddenObjects = 1 << 15,
        DisableFirstPersonPartsInBlindSkull = 1 << 16,
        HidesAssociatedObjectOnEffectDeletion = 1 << 17,
        BypassMpThrottle = 1 << 18,
        RenderInNonFirstPersonPass = 1 << 19,
        UseAveragedLocationsForLods = 1 << 20
    }

    public enum EffectPriority : short
    {
        Low,
        Normal,
        AboveNormal,
        High,
        VeryHigh,
        Essential
    }

    public enum EffectEnvironment : short
    {
        AnyEnvironment,
        AirOnly,
        WaterOnly,
        SpaceOnly,
        WetOnly,
        DryOnly
    }

    public enum EffectViolenceMode : short
    {
        EitherMode,
        ViolentModeOnly,
        NonviolentModeOnly
    }

    [Flags]
    public enum EffectEventPartFlags : ushort
    {
        None,
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
        SoundOnlyPlaysInKillcam = 1 << 10,
        DisableInAimedDownSight = 1 << 11,
        EnableOnlyInAimedDownSight = 1 << 12,
        UseDynamicDirection = 1 << 13
    }

    public enum EffectEventPriority : sbyte
    {
        Low,
        Normal,
        AboveNormal,
        High,
        VeryHigh,
        Essential
    }

    public enum EffectEventPartCameraMode : sbyte
    {
        IndependentOfCameraMode,
        FirstPersonOnly,
        ThirdPersonOnly,
        BothFirstAndThird
    }

    public enum EffectEventPartScales : int
    {
        Velocity,
        VelocityDelta,
        VelocityConeAngle,
        AngularVelocity,
        AngularVelocityDelta,
        TypeSpecificScale
    }

    public enum ParticleCoordinateSystem : short
    {
        World,
        Local,
        Parent
    }

    public enum ParticleCameraMode : short
    {
        IndependentOfCameraMode,
        FirstPersonOnly,
        ThirdPersonOnly,
        BothFirstAndThird
    }
}