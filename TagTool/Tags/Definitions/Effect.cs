using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using TagTool.Damage;

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
        [TagField(Length = 3, Flags = TagFieldFlags.Padding, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] Padd;

        public short LoopStartEvent;
        public short LocalLocation0;
        public float RuntimeDangerRadius;

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
            [TagField(Length = 3, Flags = TagFieldFlags.Padding, MaxVersion = CacheVersion.HaloOnline700123)]
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
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.HaloReach)]
                public byte[] Padding1;

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
                [TagField(Length = 3, Flags = TagFieldFlags.Padding)]
                public byte[] Padd3;

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

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public int FlagsReach;

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public float Unknown18;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public float Unknown19;

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
                public float Unknown15;

                public List<Emitter> Emitters;

                public float RuntimeMaximumLifespan; // longest lifespan property out of all emitters
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public float Unknown17;

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
                    GpuOcclusion = 1 << 13,
                    Bit14 = 1 << 14,
                    AttachmentUnknown = 1 << 15,
                }

                [TagStructure(Size = 0x2F0, MaxVersion = CacheVersion.Halo3Retail)]
                [TagStructure(Size = 0x300, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
                [TagStructure(Size = 0x330, MinVersion = CacheVersion.HaloReach)]
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

                    [TagStructure(Size = 0x30)]
                    public class RuntimeMGpuData : TagStructure
                    {
                        public ParticleProperties ConstantPerParticleProperties;
                        public ParticleProperties ConstantOverTimeProperties;
                        public ParticlePropertyScalar.ParticleStatesFlags UsedParticleStates;
                        public List<Property> Properties; // 13 blocks (predefined usage, see Property.PropertyType)
                        public List<Function> Functions; // indexed in Properties
                        public List<GpuColor> Colors; // indexed in Properties

                        [TagStructure(Size = 0x10)]
                        public class Property : TagStructure
                        {
                            // X = Constant value, YZW = multiple packed fields
                            //public RealVector4d MInnards;
                            public float MConstantValue;
                            public InnardsY MInnardsY;
                            public InnardsZ MInnardsZ;
                            public InnardsW MInnardsW;

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
                                    uint temp = BitConverter.ToUInt32(BitConverter.GetBytes(FBitfield), 0);
                                    return (byte)(temp & 0x1F);
                                }
                                private ParticlePropertyScalar.ParticleStates GetInputIndexRed()
                                {
                                    uint temp = BitConverter.ToUInt32(BitConverter.GetBytes(FBitfield), 0);
                                    return (ParticlePropertyScalar.ParticleStates)((temp >> 5) & 0x1F);
                                }
                                private byte GetIsConstant()
                                {
                                    uint temp = BitConverter.ToUInt32(BitConverter.GetBytes(FBitfield), 0);
                                    return (byte)((temp >> 21) & 0x1);
                                }

                                private void SetFunctionIndexRed(byte value)
                                {
                                    uint temp = BitConverter.ToUInt32(BitConverter.GetBytes(FBitfield), 0);
                                    temp = (temp & 0xFFFFFFE0) | (uint)(value & 0x1F);
                                    FBitfield = BitConverter.ToSingle(BitConverter.GetBytes(temp), 0);
                                }
                                private void SetInputIndexRed(ParticlePropertyScalar.ParticleStates value)
                                {
                                    uint temp = BitConverter.ToUInt32(BitConverter.GetBytes(FBitfield), 0);
                                    temp = (temp & 0xFFFFFC1F) | (((uint)value & 0x1F) << 5);
                                    FBitfield = BitConverter.ToSingle(BitConverter.GetBytes(temp), 0);
                                }
                                private void SetIsConstant(byte value)
                                {
                                    uint temp = BitConverter.ToUInt32(BitConverter.GetBytes(FBitfield), 0);
                                    temp = (temp & 0xFFDFFFFF) | ((uint)(value & 0x1) << 21);
                                    FBitfield = BitConverter.ToSingle(BitConverter.GetBytes(temp), 0);
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
                                    uint temp = BitConverter.ToUInt32(BitConverter.GetBytes(FBitfield), 0);
                                    return (ParticlePropertyScalar.OutputModifierValue)(temp & 0x3);
                                }
                                private ParticlePropertyScalar.ParticleStates GetInputIndexModifier()
                                {
                                    uint temp = BitConverter.ToUInt32(BitConverter.GetBytes(FBitfield), 0);
                                    return (ParticlePropertyScalar.ParticleStates)((temp >> 2) & 0x1F);
                                }
                                private byte GetFunctionIndexGreen()
                                {
                                    uint temp = BitConverter.ToUInt32(BitConverter.GetBytes(FBitfield), 0);
                                    return (byte)((temp >> 17) & 0x1F);
                                }

                                private void SetModifierIndex(ParticlePropertyScalar.OutputModifierValue value)
                                {
                                    uint temp = BitConverter.ToUInt32(BitConverter.GetBytes(FBitfield), 0);
                                    temp = (temp & 0xFFFFFFFD) | ((uint)value & 0x2);
                                    FBitfield = BitConverter.ToSingle(BitConverter.GetBytes(temp), 0);
                                }
                                private void SetInputIndexModifier(ParticlePropertyScalar.ParticleStates value)
                                {
                                    uint temp = BitConverter.ToUInt32(BitConverter.GetBytes(FBitfield), 0);
                                    temp = (temp & 0xFFFFFF83) | (((uint)value & 0x1F) << 2);
                                    FBitfield = BitConverter.ToSingle(BitConverter.GetBytes(temp), 0);
                                }
                                private void SetFunctionIndexGreen(byte value)
                                {
                                    uint temp = BitConverter.ToUInt32(BitConverter.GetBytes(FBitfield), 0);
                                    temp = (temp & 0xFFC1FFFF) | ((uint)(value & 0x1F) << 17);
                                    FBitfield = BitConverter.ToSingle(BitConverter.GetBytes(temp), 0);
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
                                    uint temp = BitConverter.ToUInt32(BitConverter.GetBytes(FBitfield), 0);
                                    return (byte)(temp & 0x7);
                                }
                                private byte GetColorIndexHi()
                                {
                                    uint temp = BitConverter.ToUInt32(BitConverter.GetBytes(FBitfield), 0);
                                    return (byte)((temp >> 3) & 0x7);
                                }
                                private ParticlePropertyScalar.ParticleStates GetInputIndexGreen()
                                {
                                    uint temp = BitConverter.ToUInt32(BitConverter.GetBytes(FBitfield), 0);
                                    return (ParticlePropertyScalar.ParticleStates)((temp >> 17) & 0x1F);
                                }

                                private void SetColorIndexLo(byte value)
                                {
                                    uint temp = BitConverter.ToUInt32(BitConverter.GetBytes(FBitfield), 0);
                                    temp = (temp & 0xFFFFFFF8) | (uint)(value & 0x7);
                                    FBitfield = BitConverter.ToSingle(BitConverter.GetBytes(temp), 0);
                                }
                                private void SetColorIndexHi(byte value)
                                {
                                    uint temp = BitConverter.ToUInt32(BitConverter.GetBytes(FBitfield), 0);
                                    temp = (temp & 0xFFFFFFC7) | ((uint)(value & 0x7) << 3);
                                    FBitfield = BitConverter.ToSingle(BitConverter.GetBytes(temp), 0);
                                }
                                private void SetInputIndexGreen(ParticlePropertyScalar.ParticleStates value)
                                {
                                    uint temp = BitConverter.ToUInt32(BitConverter.GetBytes(FBitfield), 0);
                                    temp = (temp & 0xFFC1FFFF) | (((uint)value & 0x1F) << 17);
                                    FBitfield = BitConverter.ToSingle(BitConverter.GetBytes(temp), 0);
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

    [Flags]
    public enum EffectFlagsMCC : int
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
        HeavyPerformance = 1 << 8,
        HalfResolution = 1 << 9,
        ForceLooping = 1 << 10,
        ObsoleteEffectOrdnanceIsGone = 1 << 11,
        RenderInHologramPass = 1 << 12,
        LightprobeOnlySampleAirprobes = 1 << 13,
        PlayEffectEvenOutsideBsps = 1 << 14,
        DrawLensFlaresWhenStopped = 1 << 15,
        KillParticlesWhenStopped = 1 << 16,
        PlayEvenOnHiddenObjects = 1 << 17,
        DisableFirstPersonPartsInBlindSkull = 1 << 18,
        HidesAssociatedObjectOnEffectDeletion = 1 << 19,
        BypassMpThrottle = 1 << 20,
        RenderInNonFirstPersonPass = 1 << 21,
        UseAveragedLocationsForLods = 1 << 22
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