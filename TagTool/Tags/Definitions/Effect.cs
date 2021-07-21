using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "effect", Tag = "effe", Size = 0x68, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "effect", Tag = "effe", Size = 0x60, MinVersion = CacheVersion.HaloReach)]
    public class Effect : TagStructure
	{
        public EffectFlags Flags;
        public uint FixedRandomSeed;
        public float OverlapThreshold;
        public float ContinueIfWithin;
        public float DeathDelay;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public sbyte Unknown1;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public sbyte Unknown2;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public sbyte Unknown3;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
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
        public CachedTag LoopingSound;
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

        [TagStructure(Size = 0xC, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x8, MinVersion = CacheVersion.HaloReach)]
        public class Location : TagStructure
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

        [TagStructure(Size = 0x44, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x40, MinVersion = CacheVersion.HaloReach)]
        public class Event : TagStructure
		{
            public StringId Name;
            public EventFlags Flags;

            [Flags]
            public enum EventFlags : int
            {
                None = 0,
                DisabledForDebugging = 1 << 0,
                ParticlesDieWhenEffectEnds = 1 << 1,
                LoopEventAgeDurationOverride = 1 << 2
            }

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public EffectPriority Priority;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public sbyte Unknown3;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public sbyte Unknown4;

            public float SkipFraction;
            public Bounds<float> DelayBounds;
            public Bounds<float> DurationBounds;

            public List<Part> Parts;
            public List<Acceleration> Accelerations;
            public List<ParticleSystem> ParticleSystems;

            [TagStructure(Size = 0x60, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x64, MinVersion = CacheVersion.HaloReach)]
            public class Part : TagStructure
			{
                public EffectEnvironment CreateInEnvironment;
                public EffectViolenceMode CreateInDisposition;
                public short PrimaryLocation;
                public short SecondaryLocation;
                public EffectEventPartFlags Flags;
                public EffectEventPriority Priority;
                public EffectEventPartCameraMode CameraMode;
                public Tag RuntimeBaseGroupTag;

                [TagField(Flags = Padding, Length = 4, MinVersion = CacheVersion.HaloReach)]
                public byte[] Unused;

                public CachedTag Type;
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
            public class Acceleration : TagStructure
			{
                public CreateInEnvironmentValue CreateInEnvironment;
                public CreateInDispositionValue CreateInDisposition;

                public short LocationIndex;
                [TagField(Flags = Padding, Length = 2)]
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

            [TagStructure(Size = 0x5C, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x70, MinVersion = CacheVersion.HaloReach)]
            public class ParticleSystem : TagStructure
			{
                public EffectEventPriority Priority;
                public sbyte GameMode;
                public sbyte Unknown3;
                public sbyte Unknown4;
                public CachedTag Particle;
                public uint LocationIndex;
                public ParticleCoordinateSystem CoordinateSystem;
                public EffectEnvironment Environment;
                public EffectViolenceMode Disposition;
                public ParticleCameraMode CameraMode;
                public short SortBias;
                [TagField(Flags = Padding, Length = 0x2, MinVersion = CacheVersion.HaloReach)]
                public byte[] Unused0;

                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public ParticleSystemFlags Flags;

                [Flags]
                public enum ParticleSystemFlags : ushort
                {
                    None = 0,
                    ParticlesFreezeWhenOffscreen = 1 << 0,
                    ParticlesContinueAsUsualWhenOffscreen = 1 << 1,
                    LodAlways1 = 1 << 2,
                    Bit3 = 1 << 3,
                    Bit4 = 1 << 4,
                    DisabledForDebugging = 1 << 5,
                    InheritEffectVelocity = 1 << 6,
                    DontRenderSystem = 1 << 7,
                    RenderWhenZoomed = 1 << 8,
                    Bit9 = 1 << 9,
                    Bit10 = 1 << 10,
                    Bit11 = 1 << 11,
                    Bit12 = 1 << 12,
                    EnableCollision = 1 << 13,
                    Bit14 = 1 << 14,
                    Bit15 = 1 << 15,
                }

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public int FlagsReach;

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public float Unknown18;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public float Unknown19;
                public float PixelBudget; // ms
                public float NearRange;
                public float NearCutoff;
                public float NearFadeOverride;
                public float LodInDistance;
                public float LodFeatherInDelta;
                public float InverseLodFeatherIn;
                public float LodOutDistance;
                public float LodFeatherOutDelta;
                public float InverseLodFeatherOut;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public float Unknown15;

                public List<Emitter> Emitters;

                public float RuntimeMaximumLifespan;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public float Unknown17;

                [TagStructure(Size = 0x2F0, MaxVersion = CacheVersion.Halo3Retail)]
                [TagStructure(Size = 0x300, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
                [TagStructure(Size = 0x330, MinVersion = CacheVersion.HaloReach)]
                public class Emitter : TagStructure
				{
                    public StringId Name;

                    public EmissionShapeValue EmissionShape;
                    public FlagsValue EmitterFlags;
                    public short Unknown1;

                    [TagField(MinVersion = CacheVersion.HaloReach)]
                    public float Unknown2;

                    [TagField(MinVersion = CacheVersion.Halo3ODST)]
                    public CachedTag CustomShape;

                    [TagField(MinVersion = CacheVersion.HaloReach)]
                    public CachedTag BoatHull;

                    public float BoundingRadiusEstimate;
                    public float BoundRadiusOverride;
                    [TagField(MinVersion = CacheVersion.HaloReach)]
                    public RealPoint3d AxisScale;
                    public RealPoint2d UVScrollRate;

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
                        VolumeEmitterParticleVelocitiesAreRandom = 1 << 0, // might be wrong
                        IsCpu = 1 << 1,
                        IsGpu = 1 << 2,
                        BecomesGpuWhenAtRest = 1 << 3
                    }

                    [TagStructure(Size = 0x38)]
					public class TranslationalOffsetData : TagStructure
					{
                        public TagMapping Mapping;
                        public RealPoint3d StartingInterpolant;
                        public RealPoint3d EndingInterpolant;
                    }

                    [TagStructure(Size = 0x38)]
					public class RelativeDirectionData : TagStructure
					{
                        public TagMapping Mapping;
                        public RealEulerAngles3d DirectionAt0;
                        public RealEulerAngles3d DirectionAt1;
                    }

                    [TagStructure(Size = 0x20, MaxVersion = CacheVersion.HaloOnline700123)]
                    [TagStructure(Size = 0x30, MinVersion = CacheVersion.HaloReach)]
					public class ParticleMovementData : TagStructure
					{
                        public CachedTag Template;

                        public FlagsValue Flags;

                        public List<Movement> Movements;

                        [TagField(ValidTags = new[] { "bitm" }, MinVersion = CacheVersion.HaloReach)]
                        public CachedTag TurbulenceTexture;

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
                        public class Movement : TagStructure
						{
                            public TypeValue Tyoe;
                            public FlagsValue Flags;

                            [TagField(Flags = Padding, Length = 1)]
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
                            public class Parameter : TagStructure
							{
                                public int ParameterId;
                                public TagMapping Property;
                            }
                        }
                    }

                    [TagStructure(Size = 0x38)]
					public class ParticleSelfAccelerationData : TagStructure
					{
                        public TagMapping Mapping;
                        public RealVector3d StartingInterpolant;
                        public RealVector3d EndingInterpolant;
                    }

                    [TagStructure(Size = 0x30)]
					public class RuntimeMGpuData : TagStructure
					{
                        public int ConstantPerParticleProperties;
                        public int ConstantOverTimeProperties;
                        public int UsedParticleStates;
                        public List<Property> Properties;
                        public List<Function> Functions;
                        public List<Color> Colors;

                        [TagStructure(Size = 0x10)]
						public class Property : TagStructure
						{
                            [TagField(Length = 4)]
                            public float[] Data;
                        }

                        [TagStructure(Size = 0x40)]
						public class Function : TagStructure
						{
                            [TagField(Length = 16)]
                            public float[] Data;
                        }

                        [TagStructure(Size = 0x10)]
						public class Color : TagStructure
						{
                            [TagField(Length = 4)]
                            public float[] Data;
                        }
                    }
                }
            }
        }

        [TagStructure(Size = 0xC)]
        public class ConicalDistribution : TagStructure
		{
            public short ProjectionYawCount;
            public short ProjectionPitchCount;
            public float DistributionExponent;
            public Angle Spread;
        }

        [TagStructure(Size = 0x14, MinVersion = CacheVersion.HaloReach)]
        public class LoopingSounds : TagStructure
		{
            public CachedTag LoopingSound;
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