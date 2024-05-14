using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using TagTool.Damage;
using static TagTool.Tags.Definitions.Gen4.BreakableSurface.ParticleSystemDefinitionBlockNew.ParticleSystemEmitterDefinitionBlock.GpuPropertyFunctionColorStruct;
using System.Linq;
using TagTool.Commands.Common;
using static TagTool.Effects.EditableProperty;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "effect", Tag = "effe", Size = 0x68, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "effect", Tag = "effe", Size = 0x60, MinVersion = CacheVersion.HaloReach)]
    public class Effect : TagStructure
	{
        [TagField(Platform = CachePlatform.Original)]
        public EffectFlags Flags;
        [TagField(Platform = CachePlatform.MCC)]
        public EffectFlagsMCC FlagsMCC;

        public uint FixedRandomSeed;
        public float RestartIfWithin;
        public float ContinueIfWithin;
        public float DeathDelay;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public GlobalEffectPriorityEnum Priority;
        [TagField(Length = 3, Flags = Padding, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] Padding0;

        public short LoopStartEvent;
        public short LocalLocation0;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float RuntimeDangerRadius;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short LocalLocation1Reach;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public GlobalEffectPriorityEnum PriorityReach;
        [TagField(MinVersion = CacheVersion.HaloReach, Length = 0x1, Flags = Padding)]
        public byte[] PaddingReach;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float RuntimeDangerRadiusReach;

        public List<Location> Locations;
        public List<Event> Events;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<LoopingSounds> LoopingSoundBlock;

        [TagField(ValidTags = new[] { "lsnd" }, MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag LoopingSound;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public sbyte LocationIndex;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public sbyte BindScaleToEvent;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public short LocalLocation1;

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
            public EffectLocationFlags Flags;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public GlobalEffectPriorityEnum Priority;
            [TagField(Length = 3, Flags = Padding, MaxVersion = CacheVersion.HaloOnline700123)]
            public byte[] Padd2;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public Int16 ReachFlags;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public EffectPriority ReachPriority;

            [Flags]
            public enum EffectLocationFlags : uint
            {
                Optional = 1 << 0,
                Destructible = 1 << 1,
                TrackSubframeMovements = 1 << 2
            }
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
            public GlobalEffectPriorityEnum Priority;
            [TagField(Length = 3, Flags = Padding, MaxVersion = CacheVersion.HaloOnline700123)]
            public byte[] Padd3;

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

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public EffectpartGameModeDefinition GameMode;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public DamageReportingType DamageReportingType;
                [TagField(Length = 0x2, Flags = Padding, MinVersion = CacheVersion.HaloReach)]
                public byte[] Padding1;

                [TagField(ValidTags = new[] { "beam", "char", "cmoe", "cntl", "cpem", "decs", "drdf",
                "gldf", "jpt!", "lens", "ligh", "ltvl", "obje", "rwrd", "sefc", "shit", "snd!" })]
                public CachedTag Type; // 020_base: effe | sandbox.map: hlmt

                public Bounds<float> VelocityBounds;
                public RealEulerAngles2d VelocityOrientation;
                public Angle VelocityConeAngle;
                public Bounds<Angle> AngularVelocityBounds;
                public Bounds<float> RadiusModifierBounds;
                public RealPoint3d RelativeOffset;
                public RealEulerAngles2d RelativeOrientation;
                public EffectEventPartScales AScalesValues;
                public EffectEventPartScales BScalesValues;

                public enum EffectpartGameModeDefinition : sbyte
                {
                    Any,
                    CampaignOnly,
                    MultiplayerOnly,
                    CampaignOnlyNotCinematics,
                    CampaignCinematicsOnly,
                    CampaignSoloOnly
                }
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
                public GlobalEffectPriorityEnum Priority;
                [TagField(Length = 3, Flags = Padding)]
                public byte[] Padding0;

                [TagField(ValidTags = new[] { "prt3" })]
                public CachedTag Particle;

                public uint LocationIndex;
                public ParticleCoordinateSystem CoordinateSystem;
                public EffectEnvironment Environment;
                public EffectViolenceMode Disposition;
                public ParticleCameraMode CameraMode;
                public short SortBias;

                [TagField(Flags = Padding, Length = 0x2, MinVersion = CacheVersion.HaloReach)]
                public byte[] Padding1;

                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public ParticleSystemFlags Flags;

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public ParticleSystemFlagsReach ReachFlags;

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public Bounds<float> PercentVelocityToInherit; // flag must be checked above

                public float PixelBudget; // ms
                public float NearRange; // if (Version>H3) NearRange = 1.0f / max(NearRange, 0.000099999997f)
                public float NearCutoff; // if (FLAG(OverrideNearFade)) NearCutoff = NearFadeOverride
                public float NearFadeOverride;
                public float LodInDistance;
                public float LodFeatherInDelta;
                public float InverseLodFeatherIn; // 1.0f / LodFeatherInDelta
                public float LodOutDistance;
                public float LodFeatherOutDelta;
                public float InverseLodFeatherOut; // 1.0f / LodFeatherOutDelta

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public float DynamicResolutionMultiplier; // lower numbers cause particle systems to render more at low-res - must check flag above

                public List<Emitter> Emitters;

                public float RuntimeMaximumLifespan; // longest lifespan property out of all emitters

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public float RuntimeOverdraw;

                [Flags]
                public enum ParticleSystemFlags : ushort
                {
                    ParticlesFreezeWhenOffscreen = 1 << 0,
                    ParticlesContinueAsUsualWhenOffscreen = 1 << 1,
                    LodAlways10 = 1 << 2,
                    LodSameInSplitscreen = 1 << 3,
                    DisabledIn3And4waySplitscreen = 1 << 4,
                    DisabledForDebugging = 1 << 5,
                    InheritEffectVelocity = 1 << 6,
                    DontRenderSystem = 1 << 7,
                    RenderWhenZoomed = 1 << 8,
                    ForceCpuUpdating = 1 << 9,
                    ForceGpuUpdating = 1 << 10,
                    OverrideNearFade = 1 << 11,
                    ParticlesDieWhenEffectEnds = 1 << 12,
                    GpuOcclusionWeatherOnly = 1 << 13,
                    TurnOffNearFadeOnEnhancedGraphics = 1 << 14,
                    AttachmentUnknown = 1 << 15,
                }

                [Flags]
                public enum ParticleSystemFlagsReach : uint
                {
                    ParticlesFreezeWhenOffscreen = 1 << 0,
                    ParticlesContinueAsUsualWhenOffscreen = 1 << 1,
                    LodAlways10 = 1 << 2,
                    LodSameInSplitscreen = 1 << 3,
                    DisabledIn3And4WaySplitscreen = 1 << 4,
                    DisabledForDebugging = 1 << 5,
                    InheritEffectVelocity = 1 << 6,
                    DontRenderSystem = 1 << 7,
                    RenderWhenZoomed = 1 << 8,
                    ForceCpuUpdating = 1 << 9,
                    ForceGpuUpdating = 1 << 10,
                    OverrideNearFadeUseWithCaution = 1 << 11,
                    ParticlesDieWhenEffectEnds = 1 << 12,
                    UseSynchronizedRandomSeed = 1 << 13, // synchronized across particle systems
                    UseWorldOrientation = 1 << 14, // particle system uses local-space position but up is always 'global up'
                    RenderInSpawnOrder = 1 << 15, // first particle spawned renders first (at the back), last particle spawned renders last (front)
                    DynamicParticleResolution = 1 << 16 // use distance and multiplier (below) to tune high- or low-res rendering
                }

                [TagStructure(Size = 0x2F0, MaxVersion = CacheVersion.Halo3Retail)]
                [TagStructure(Size = 0x300, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
                [TagStructure(Size = 0x2EC, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.MCC)]
                [TagStructure(Size = 0x330, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original)]
                [TagStructure(Size = 0x31C, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.MCC)]
                public class Emitter : TagStructure
                {
                    public StringId Name;

                    public EmissionShapeValue EmissionShape;
                    public FlagsValue EmitterFlags;

                    [TagField(MinVersion = CacheVersion.HaloReach)]
                    public byte Flags;

                    public ParticleModelAxis ModelAxis;
                    public ParticleReferenceAxis ReferenceAxis;

                    [TagField(MinVersion = CacheVersion.HaloReach, Length = 0x3, Flags = Padding)]
                    public byte[] Padding0;

                    [TagField(ValidTags = new[] { "pecp" }, MinVersion = CacheVersion.Halo3ODST)]
                    public CachedTag CustomShape;

                    [TagField(ValidTags = new[] { "ebhd" }, MinVersion = CacheVersion.HaloReach)]
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
                    public ParticlePropertyScalar EmissionRadius;

                    /// <summary>
                    /// Determines the angle at which particles are emitted.
                    /// </summary>
                    [TagField(Format = "Degrees")]
                    public ParticlePropertyScalar EmissionAngle;

                    public ParticlePropertyScalar EmissionAxisAngle;

                    /// <summary>
                    /// Number of particles that are spawned at the birth of the effect.
                    /// </summary>
                    public ParticlePropertyScalar ParticleStartingCount;

                    /// <summary>
                    /// Max number of particles allowed to exist at one time.
                    /// </summary>
                    [TagField(Format = "0 = Unlimited")]
                    public ParticlePropertyScalar ParticleMaxCount;

                    /// <summary>
                    /// Number of particles that are spawned every second from the emitters.
                    /// </summary>
                    [TagField(Format = "Particles Per Second")]
                    public ParticlePropertyScalar ParticleEmissionRate;

                    /// <summary>
                    /// Number of seconds a particle will live after emission.
                    /// </summary>
                    [TagField(Format = "Seconds")]
                    public ParticlePropertyScalar ParticleLifespan;

                    public ParticleMovementData ParticleMovement;

                    [TagField(Format = "World Units Per Second Per Second")]
                    public ParticleSelfAccelerationData ParticleSelfAcceleration;

                    [TagField(Format = "World Units Per Second")]
                    public ParticlePropertyScalar ParticleInitialVelocity;

                    [TagField(Format = "360 Degree Rotations Per Second")]
                    public ParticlePropertyScalar ParticleRotation;

                    public ParticlePropertyScalar ParticleInitialRotationRate;

                    [TagField(Format = "World Units")]
                    public ParticlePropertyScalar ParticleSize;

                    public ParticlePropertyScalar ParticleScale;

                    [TagField(Format = "RGB")]
                    public ParticlePropertyScalar ParticleTint;

                    public ParticlePropertyScalar ParticleAlpha;

                    [TagField(Format = "0 = Normal, 1 = Clamped")]
                    public ParticlePropertyScalar ParticleAlphaBlackPoint;

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
                        BreakableSurface,
                        CustomPoints,
                        // Reach+
                        BoatHullSurface,
                        Cube,
                        Cylinder,
                        UnweightedLine,
                        Plane,
                        Jetwash,
                        // H4
                        PlanarOrbit,
                        SphereOrbit
                    }

                    public enum ParticleModelAxis : byte
                    {
                        Constant,
                        Cone,
                        Disc,
                        Globe
                    }

                    public enum ParticleReferenceAxis : byte
                    {
                        X,
                        Y,
                        Z
                    }

                    [Flags]
                    public enum FlagsValue : byte
                    {
                        None,
                        Postprocessed = 1 << 0,
                        IsCpu = 1 << 1,
                        IsGpu = 1 << 2,
                        BecomesGpuWhenAtRest = 1 << 3,
                        AlphaBlackPoint_Bit3 = 1 << 4,
                    }

                    [TagStructure(Size = 0x38)]
                    public class TranslationalOffsetData : TagStructure
                    {
                        public ParticlePropertyScalar Mapping;
                        public RealPoint3d StartingInterpolant;
                        public RealPoint3d EndingInterpolant;
                    }

                    [TagStructure(Size = 0x38)]
                    public class RelativeDirectionData : TagStructure
                    {
                        public ParticlePropertyScalar Mapping;
                        public RealEulerAngles3d DirectionAt0;
                        public RealEulerAngles3d DirectionAt1;
                    }

                    [TagStructure(Size = 0x20, MaxVersion = CacheVersion.HaloOnline700123)]
                    [TagStructure(Size = 0x30, MinVersion = CacheVersion.HaloReach)]
                    public class ParticleMovementData : TagStructure
                    {
                        [TagField(ValidTags = new[] { "pmov" })]
                        public CachedTag Template;

                        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                        public PhysicsFlags Flags;

                        [TagField(MinVersion = CacheVersion.HaloReach)]
                        public PhysicsFlagsReach FlagsReach;

                        [TagField(MinVersion = CacheVersion.HaloReach)]
                        public byte CollisionControllerIndex;
                        [TagField(MinVersion = CacheVersion.HaloReach)]
                        public byte TurbulenceControllerIndex;

                        public List<Movement> Movements;

                        [TagField(ValidTags = new[] { "bitm" }, MinVersion = CacheVersion.HaloReach)]
                        public CachedTag TurbulenceTexture;

                        [Flags]
                        public enum PhysicsFlags : int
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

                        [Flags]
                        public enum PhysicsFlagsReach : short
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
                            public TypeValue Type;
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
                                public ParticlePropertyScalar Property;
                            }
                        }
                    }

                    [TagStructure(Size = 0x38)]
                    public class ParticleSelfAccelerationData : TagStructure
                    {
                        public ParticlePropertyScalar Mapping;
                        public RealVector3d StartingInterpolant;
                        public RealVector3d EndingInterpolant;
                    }

                    [TagStructure(Size = 0x30, Platform = CachePlatform.Original)]
                    [TagStructure(Size = 0x30, Platform = CachePlatform.MCC, MaxVersion = CacheVersion.Halo3Retail)]
                    [TagStructure(Size = 0x1C, Platform = CachePlatform.MCC, MinVersion = CacheVersion.Halo3ODST)]
                    public class RuntimeMGpuData : TagStructure
                    {
                        public ParticleProperties ConstantPerParticleProperties;
                        public ParticleProperties ConstantOverTimeProperties;
                        public ParticlePropertyScalar.ParticleStatesFlags UsedParticleStates;

                        [TagField(Platform = CachePlatform.MCC, MinVersion = CacheVersion.Halo3ODST)]
                        public int StructuredScenarioInteropIndex;

                        [TagField(Platform = CachePlatform.Original)]
                        [TagField(Platform = CachePlatform.MCC, MaxVersion = CacheVersion.Halo3Retail)]
                        public List<Property> Properties; // 13 blocks (predefined usage, see Property.PropertyType)

                        [TagField(Platform = CachePlatform.Original)]
                        [TagField(Platform = CachePlatform.MCC, MaxVersion = CacheVersion.Halo3Retail)]
                        public List<Function> Functions; // indexed in Properties

                        [TagField(Platform = CachePlatform.Original)]
                        [TagField(Platform = CachePlatform.MCC, MaxVersion = CacheVersion.Halo3Retail)]
                        public List<GpuColor> Colors; // indexed in Properties

                        [TagField(Platform = CachePlatform.MCC, MinVersion = CacheVersion.Halo3ODST)]
                        public List<ODSTMCCMGpuBlock> RuntimeGpuBlocks; // contains the above blocks in ODSTMCC

                        [TagStructure(Size = 0x10)]
                        public class Property : TagStructure
                        {
                            // X = Constant value, YZW = multiple packed fields
                            //public RealVector4d MInnards;
                            public float MConstantValue;
                            public InnardsY MInnardsY;
                            public InnardsZ MInnardsZ;
                            public InnardsW MInnardsW;

                            // These types are bitfields casted to floats
                            [TagStructure(Size = 0x4)]
                            public class InnardsY : TagStructure
                            {
                                public float FBitfield;
                                // Packed data:
                                // Bits 0-5: Function index red
                                // Bits 5-10: Input index red (ranged input)
                                // Bits 21-22: IsConstant
                                public byte FunctionIndexRed { get => GetFunctionIndexRed(); set => SetFunctionIndexRed(value); }
                                public ParticlePropertyScalar.ParticleStates InputIndexRed { get => GetInputIndexRed(); set => SetInputIndexRed(value); }
                                public byte IsConstant { get => GetIsConstant(); set => SetIsConstant(value); }

                                private byte GetFunctionIndexRed()
                                {
                                    return (byte)(((int)FBitfield) & 0x1F);
                                }
                                private ParticlePropertyScalar.ParticleStates GetInputIndexRed()
                                {
                                    return (ParticlePropertyScalar.ParticleStates)(((int)FBitfield >> 5) & 0x1F);
                                }
                                private byte GetIsConstant()
                                {
                                    return (byte)(((int)FBitfield >> 21) & 0x1);
                                }

                                private void SetFunctionIndexRed(byte value)
                                {
                                    uint temp = (uint)FBitfield;
                                    temp = (temp & 0xFFFFFFE0) | (uint)(value & 0x1F);
                                    FBitfield = (float)temp;
                                }
                                private void SetInputIndexRed(ParticlePropertyScalar.ParticleStates value)
                                {
                                    uint temp = (uint)FBitfield;
                                    temp = (temp & 0xFFFFFC1F) | (((uint)value & 0x1F) << 5);
                                    FBitfield = (float)temp;
                                }
                                private void SetIsConstant(byte value)
                                {
                                    uint temp = (uint)FBitfield;
                                    temp = (temp & 0xFFDFFFFF) | ((uint)(value & 0x1) << 21);
                                    FBitfield = (float)temp;
                                }
                            }
                            [TagStructure(Size = 0x4)]
                            public class InnardsZ : TagStructure
                            {
                                public float FBitfield;
                                // Packed data:
                                // Bits 0-2: Modifier Index
                                // Bits 2-7: Input Index Modifier
                                // Bits 17-22: Function index green
                                public ParticlePropertyScalar.OutputModifierValue ModifierIndex { get => GetModifierIndex(); set => SetModifierIndex(value); }
                                public ParticlePropertyScalar.ParticleStates InputIndexModifier { get => GetInputIndexModifier(); set => SetInputIndexModifier(value); }
                                public byte FunctionIndexGreen { get => GetFunctionIndexGreen(); set => SetFunctionIndexGreen(value); }

                                private ParticlePropertyScalar.OutputModifierValue GetModifierIndex()
                                {
                                    return (ParticlePropertyScalar.OutputModifierValue)((int)FBitfield & 0x3);
                                }
                                private ParticlePropertyScalar.ParticleStates GetInputIndexModifier()
                                {
                                    return (ParticlePropertyScalar.ParticleStates)(((int)FBitfield >> 2) & 0x1F);
                                }
                                private byte GetFunctionIndexGreen()
                                {
                                    return (byte)(((int)FBitfield >> 17) & 0x1F);
                                }

                                private void SetModifierIndex(ParticlePropertyScalar.OutputModifierValue value)
                                {
                                    uint temp = (uint)FBitfield;
                                    temp = (temp & 0xFFFFFFFD) | ((uint)value & 0x2);
                                    FBitfield = (float)temp;
                                }
                                private void SetInputIndexModifier(ParticlePropertyScalar.ParticleStates value)
                                {
                                    uint temp = (uint)FBitfield;
                                    temp = (temp & 0xFFFFFF83) | (((uint)value & 0x1F) << 2);
                                    FBitfield = (float)temp;
                                }
                                private void SetFunctionIndexGreen(byte value)
                                {
                                    uint temp = (uint)FBitfield;
                                    temp = (temp & 0xFFC1FFFF) | ((uint)(value & 0x1F) << 17);
                                    FBitfield = (float)temp;
                                }
                            }
                            [TagStructure(Size = 0x4)]
                            public class InnardsW : TagStructure
                            {
                                public float FBitfield;
                                // Packed data:
                                // Bits 0-3: Color index lo
                                // Bits 3-6: Color index hi
                                // Bits 17-22: Input index green
                                public byte ColorIndexLo { get => GetColorIndexLo(); set => SetColorIndexLo(value); }
                                public byte ColorIndexHi { get => GetColorIndexHi(); set => SetColorIndexHi(value); }
                                public ParticlePropertyScalar.ParticleStates InputIndexGreen { get => GetInputIndexGreen(); set => SetInputIndexGreen(value); }

                                private byte GetColorIndexLo()
                                {
                                    return (byte)(((int)FBitfield >> 0) & 0x7);
                                }
                                private byte GetColorIndexHi()
                                {
                                    return (byte)(((int)FBitfield >> 3) & 0x7);
                                }
                                private ParticlePropertyScalar.ParticleStates GetInputIndexGreen()
                                {
                                    return (ParticlePropertyScalar.ParticleStates)((((int)FBitfield >> 17) >> 0) & 0x1F);
                                }

                                private void SetColorIndexLo(byte value)
                                {
                                    uint temp = (uint)FBitfield;
                                    temp = (temp & 0xFFFFFFF8) | (uint)(value & 0x7);
                                    FBitfield = (float)temp;
                                }
                                private void SetColorIndexHi(byte value)
                                {
                                    uint temp = (uint)FBitfield;
                                    temp = (temp & 0xFFFFFFC7) | ((uint)(value & 0x7) << 3);
                                    FBitfield = (float)temp;
                                }
                                private void SetInputIndexGreen(ParticlePropertyScalar.ParticleStates value)
                                {
                                    uint temp = (uint)FBitfield;
                                    temp = (temp & 0xFFC1FFFF) | (((uint)value & 0x1F) << 17);
                                    FBitfield = (float)temp;
                                }
                            }

                            public enum PropertyType
                            {
                                EmitterTint,
                                EmitterAlpha,
                                EmitterSize,
                                ParticleColor,
                                ParticleIntensity,
                                ParticleAlpha,
                                ParticleScale,
                                ParticleRotation,
                                ParticleFrame,
                                ParticleBlackPoint,
                                ParticleAspect,
                                ParticleSelfAcceleration,
                                ParticlePalette
                            }

                            public Property()
                            {
                                this.MInnardsY = new InnardsY();
                                this.MInnardsZ = new InnardsZ();
                                this.MInnardsW = new InnardsW();
                            }
                        }

                        [TagStructure(Size = 0x40)]
                        public class Function : TagStructure
                        {
                            public FunctionTypeReal FunctionType;
                            public float DomainMax;
                            public float RangeMin;
                            public float RangeMax;
                            public float Flags;
                            public float ExclusionMin;
                            public float ExclusionMax;
                            [TagField(Flags = Padding, Length = 0x4)]
                            public byte[] Float4Padding;
                            [TagField(Length = 8)]
                            public float[] Innards;

                            [TagStructure(Size = 0x4)]
                            public class FunctionTypeReal : TagStructure
                            {
                                public float FunctionType;

                                public TagFunction.TagFunctionType Type { get => (TagFunction.TagFunctionType)FunctionType; set => FunctionType = (float)value; }
                            }

                            public Function()
                            {
                                FunctionType = new FunctionTypeReal();
                                Innards = new float[8];
                            }
                        }

                        [TagStructure(Size = 0x10)]
                        public class GpuColor : TagStructure
                        {
                            public RealRgbaColor Color;
                        }

                        [Flags]
                        public enum ParticleProperties : int
                        {
                            None = 0,
                            TranslationOffset = 1 << 0,
                            RelativeDirection = 1 << 1,
                            EmissionRadius = 1 << 2,
                            EmissionAngle = 1 << 3,
                            EmissionAxisAngle = 1 << 4,
                            ParticleStartingCount = 1 << 5,
                            ParticleMaxCount = 1 << 6,
                            ParticleEmissionRate = 1 << 7,
                            ParticleLifespan = 1 << 8,
                            ParticleSelfAcceleration = 1 << 9,
                            ParticleInitialVelocity = 1 << 10,
                            ParticleRotation = 1 << 11,
                            ParticleInitialRotationRate = 1 << 12,
                            ParticleSize = 1 << 13,
                            ParticleScale = 1 << 14,
                            ParticleTint = 1 << 15,
                            ParticleAlpha = 1 << 16,
                            ParticleAlphaBlackPoint = 1 << 17
                        }

                        [TagStructure(Size = 0x24)]
                        public class ODSTMCCMGpuBlock : TagStructure
                        {
                            public List<Property> Properties;
                            public List<Function> Functions;
                            public List<GpuColor> Colors;

                        }
                    }

                    public ParticlePropertyScalar.ParticleStatesFlags ValidateUsedStates()
                    {
                        ParticlePropertyScalar.ParticleStatesFlags usedStates = ParticlePropertyScalar.ParticleStatesFlags.None;
                        
                        usedStates |= ParticleEditablePropertyEvaluate(TranslationalOffset.Mapping, "TranslationalOffset",
                            0xFE117B0A, ParticlePropertyScalar.ParticleStates.Velocity);
                        usedStates |= ParticleEditablePropertyEvaluate(RelativeDirection.Mapping, "RelativeDirection",
                            0xFE117B0A, ParticlePropertyScalar.ParticleStates.Velocity);
                        usedStates |= ParticleEditablePropertyEvaluate(EmissionRadius, "EmissionRadius",
                            0x7F17FFE, ParticlePropertyScalar.ParticleStates.Velocity);
                        usedStates |= ParticleEditablePropertyEvaluate(EmissionAngle, "EmissionAngle",
                            0x7F17FFE, ParticlePropertyScalar.ParticleStates.Velocity);
                        usedStates |= ParticleEditablePropertyEvaluate(EmissionAxisAngle, "EmissionAxisAngle",
                            0x7F17FFE, ParticlePropertyScalar.ParticleStates.Velocity);
                        usedStates |= ParticleEditablePropertyEvaluate(ParticleStartingCount, "ParticleStartingCount",
                            0xFE117B0A, ParticlePropertyScalar.ParticleStates.Velocity);
                        usedStates |= ParticleEditablePropertyEvaluate(ParticleMaxCount, "ParticleMaxCount",
                            0xFE117B0A, ParticlePropertyScalar.ParticleStates.Velocity);
                        usedStates |= ParticleEditablePropertyEvaluate(ParticleEmissionRate, "ParticleEmissionRate",
                            0xFE117B0A, ParticlePropertyScalar.ParticleStates.Velocity);
                        usedStates |= ParticleEditablePropertyEvaluate(ParticleLifespan, "ParticleLifespan",
                            0x7F17FFE, ParticlePropertyScalar.ParticleStates.Velocity);
                        usedStates |= ParticleEditablePropertyEvaluate(ParticleSelfAcceleration.Mapping, "ParticleSelfAcceleration",
                            0x7FFFFFF, ParticlePropertyScalar.ParticleStates.Velocity);
                        usedStates |= ParticleEditablePropertyEvaluate(ParticleInitialVelocity, "ParticleInitialVelocity",
                            0x7F17FFE, ParticlePropertyScalar.ParticleStates.Velocity);
                        usedStates |= ParticleEditablePropertyEvaluate(ParticleRotation, "ParticleRotation",
                            0x7FFFFFF, ParticlePropertyScalar.ParticleStates.Velocity);
                        usedStates |= ParticleEditablePropertyEvaluate(ParticleInitialRotationRate, "ParticleInitialRotationRate",
                            0x7F17FFE, ParticlePropertyScalar.ParticleStates.Velocity);
                        usedStates |= ParticleEditablePropertyEvaluate(ParticleSize, "ParticleSize",
                            0x7FFFFFF, ParticlePropertyScalar.ParticleStates.Velocity);
                        usedStates |= ParticleEditablePropertyEvaluate(ParticleScale, "ParticleScale",
                            0x7FFFFFF, ParticlePropertyScalar.ParticleStates.Velocity);
                        usedStates |= ParticleEditablePropertyEvaluate(ParticleTint, "ParticleTint",
                            0x7FFFFFF, ParticlePropertyScalar.ParticleStates.Velocity);
                        usedStates |= ParticleEditablePropertyEvaluate(ParticleAlpha, "ParticleAlpha",
                            0x7FFFFFF, ParticlePropertyScalar.ParticleStates.Velocity);
                        usedStates |= ParticleEditablePropertyEvaluate(ParticleAlphaBlackPoint, "ParticleAlphaBlackPoint",
                            0x7FFFFFF, ParticlePropertyScalar.ParticleStates.Velocity);

                        return usedStates;
                    }

                    public void GetConstantStates(out RuntimeMGpuData.ParticleProperties cppStates, out RuntimeMGpuData.ParticleProperties cotStates)
                    {
                        cppStates = RuntimeMGpuData.ParticleProperties.None;

                        if (this.TranslationalOffset.Mapping.IsConstantPerParticle())
                            cppStates |= RuntimeMGpuData.ParticleProperties.TranslationOffset;
                        if (this.RelativeDirection.Mapping.IsConstantPerParticle())
                            cppStates |= RuntimeMGpuData.ParticleProperties.RelativeDirection;
                        if (this.EmissionRadius.IsConstantPerParticle())
                            cppStates |= RuntimeMGpuData.ParticleProperties.EmissionRadius;
                        if (this.EmissionAngle.IsConstantPerParticle())
                            cppStates |= RuntimeMGpuData.ParticleProperties.EmissionAngle;
                        if (this.EmissionAxisAngle.IsConstantPerParticle())
                            cppStates |= RuntimeMGpuData.ParticleProperties.EmissionAxisAngle;
                        if (this.ParticleStartingCount.IsConstantPerParticle())
                            cppStates |= RuntimeMGpuData.ParticleProperties.ParticleStartingCount;
                        if (this.ParticleMaxCount.IsConstantPerParticle())
                            cppStates |= RuntimeMGpuData.ParticleProperties.ParticleMaxCount;
                        if (this.ParticleEmissionRate.IsConstantPerParticle())
                            cppStates |= RuntimeMGpuData.ParticleProperties.ParticleEmissionRate;
                        if (this.ParticleLifespan.IsConstantPerParticle())
                            cppStates |= RuntimeMGpuData.ParticleProperties.ParticleLifespan;
                        if (this.ParticleSelfAcceleration.Mapping.IsConstantPerParticle())
                            cppStates |= RuntimeMGpuData.ParticleProperties.ParticleSelfAcceleration;
                        if (this.ParticleInitialVelocity.IsConstantPerParticle())
                            cppStates |= RuntimeMGpuData.ParticleProperties.ParticleInitialVelocity;
                        if (this.ParticleRotation.IsConstantPerParticle())
                            cppStates |= RuntimeMGpuData.ParticleProperties.ParticleRotation;
                        if (this.ParticleInitialRotationRate.IsConstantPerParticle())
                            cppStates |= RuntimeMGpuData.ParticleProperties.ParticleInitialRotationRate;
                        if (this.ParticleSize.IsConstantPerParticle())
                            cppStates |= RuntimeMGpuData.ParticleProperties.ParticleSize;
                        if (this.ParticleScale.IsConstantPerParticle())
                            cppStates |= RuntimeMGpuData.ParticleProperties.ParticleScale;
                        if (this.ParticleTint.IsConstantPerParticle())
                            cppStates |= RuntimeMGpuData.ParticleProperties.ParticleTint;
                        if (this.ParticleAlpha.IsConstantPerParticle())
                            cppStates |= RuntimeMGpuData.ParticleProperties.ParticleAlpha;
                        if (this.ParticleAlphaBlackPoint.IsConstantPerParticle())
                            cppStates |= RuntimeMGpuData.ParticleProperties.ParticleAlphaBlackPoint;

                        cotStates = RuntimeMGpuData.ParticleProperties.None;

                        if (this.TranslationalOffset.Mapping.IsConstantOverTime())
                            cotStates |= RuntimeMGpuData.ParticleProperties.TranslationOffset;
                        if (this.RelativeDirection.Mapping.IsConstantOverTime())
                            cotStates |= RuntimeMGpuData.ParticleProperties.RelativeDirection;
                        if (this.EmissionRadius.IsConstantOverTime())
                            cotStates |= RuntimeMGpuData.ParticleProperties.EmissionRadius;
                        if (this.EmissionAngle.IsConstantOverTime())
                            cotStates |= RuntimeMGpuData.ParticleProperties.EmissionAngle;
                        if (this.EmissionAxisAngle.IsConstantOverTime())
                            cotStates |= RuntimeMGpuData.ParticleProperties.EmissionAxisAngle;
                        if (this.ParticleStartingCount.IsConstantOverTime())
                            cotStates |= RuntimeMGpuData.ParticleProperties.ParticleStartingCount;
                        if (this.ParticleMaxCount.IsConstantOverTime())
                            cotStates |= RuntimeMGpuData.ParticleProperties.ParticleMaxCount;
                        if (this.ParticleEmissionRate.IsConstantOverTime())
                            cotStates |= RuntimeMGpuData.ParticleProperties.ParticleEmissionRate;
                        if (this.ParticleLifespan.IsConstantOverTime())
                            cotStates |= RuntimeMGpuData.ParticleProperties.ParticleLifespan;
                        if (this.ParticleSelfAcceleration.Mapping.IsConstantOverTime())
                            cotStates |= RuntimeMGpuData.ParticleProperties.ParticleSelfAcceleration;
                        if (this.ParticleInitialVelocity.IsConstantOverTime())
                            cotStates |= RuntimeMGpuData.ParticleProperties.ParticleInitialVelocity;
                        if (this.ParticleRotation.IsConstantOverTime())
                            cotStates |= RuntimeMGpuData.ParticleProperties.ParticleRotation;
                        if (this.ParticleInitialRotationRate.IsConstantOverTime())
                            cotStates |= RuntimeMGpuData.ParticleProperties.ParticleInitialRotationRate;
                        if (this.ParticleSize.IsConstantOverTime())
                            cotStates |= RuntimeMGpuData.ParticleProperties.ParticleSize;
                        if (this.ParticleScale.IsConstantOverTime())
                            cotStates |= RuntimeMGpuData.ParticleProperties.ParticleScale;
                        if (this.ParticleTint.IsConstantOverTime())
                            cotStates |= RuntimeMGpuData.ParticleProperties.ParticleTint;
                        if (this.ParticleAlpha.IsConstantOverTime())
                            cotStates |= RuntimeMGpuData.ParticleProperties.ParticleAlpha;
                        if (this.ParticleAlphaBlackPoint.IsConstantOverTime())
                            cotStates |= RuntimeMGpuData.ParticleProperties.ParticleAlphaBlackPoint;
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
        DoNotReusePartsWhenLooping = 1 << 2,
        AgeCreatorsWeapon = 1 << 3,
        UseParentPositionButWorldOrientation = 1 << 4,
        CanPenetrateWalls = 1 << 5,
        CannotBeRestarted = 1 << 6,
        ForceUseOwnLightprobe = 1 << 7,
        // Beyond here is runtime flags
        ForceLooping = 1 << 8, // delay <= 0 && duration <= 0 (duration of 0 is postprocessed, 1/tickrate)
        Deterministic = 1 << 9,
        TintFromLightmap = 1 << 10,
        TintFromDiffuseTexture = 1 << 11, // geometry sampler
        HasEnvironmentRestrictedPart = 1 << 12, // parts->create_in_environment != 0 || ( part->type==beam && any(location->name==stringid(child)) )
        UnusedBit = 1 << 13, // unused
        HasEnvironmentRestrictedParticleSystem = 1 << 14, // system->environment != 0
        TrackSubframeMovements = 1 << 15,
        UnknownHO = 1 << 16 // HO only. unknown
    }

    [Flags]
    public enum EffectFlagsMCC : int
    {
        None,
        DeletedWhenAttachmentDeactivates = 1 << 0,
        RunEventsInParallel = 1 << 1,
        DoNotReusePartsWhenLooping = 1 << 2,
        AgeCreatorsWeapon = 1 << 3,
        UseParentPositionButWorldOrientation = 1 << 4,
        CanPenetrateWalls = 1 << 5,
        CannotBeRestarted = 1 << 6,
        ForceUseOwnLightprobe = 1 << 7,
        HeavyPerformance = 1 << 8,
        HalfResolution = 1 << 9,
        // Beyond here is runtime flags
        ForceLooping = 1 << 10,
        Deterministic = 1 << 11,
        TintFromLightmap = 1 << 12,
        TintFromDiffuseTexture = 1 << 13,
        HasEnvironmentRestrictedPart = 1 << 14,
        UnusedBit = 1 << 15,
        HasEnvironmentRestrictedParticleSystem = 1 << 16,
        TrackSubframeMovements = 1 << 17
    }

    public enum GlobalEffectPriorityEnum : byte
    {
        Normal,
        High,
        Essential
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

    [Flags]
    public enum EffectEventPartScales : uint
    {
        Velocity = 1 << 0,
        VelocityDelta = 1 << 1,
        VelocityConeAngle = 1 << 2,
        AngularVelocity = 1 << 3,
        AngularVelocityDelta = 1 << 4,
        TypeSpecificScale = 1 << 5
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