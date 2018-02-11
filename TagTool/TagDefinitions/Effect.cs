using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "effect", Tag = "effe", Size = 0x68, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "effect", Tag = "effe", Size = 0x70, MinVersion = CacheVersion.HaloOnline106708)]
    public class Effect
    {
        public EffectFlags Flags;
        public uint FixedRandomSeed;
        public float OverlapThreshold;
        public float ContinueIfWithin;
        public float DeathDelay;
        public sbyte Unknown5;
        public sbyte Unknown6;
        public sbyte Unknown7;
        public sbyte Unknown8;
        public short LoopStartEvent;
        public EffectPriority Priority;
        public uint Unknown10;
        public List<Location> Locations;
        public List<Event> Events;
        public CachedTagInstance LoopingSound;
        public sbyte LocationIndex;
        public sbyte EventIndex;
        public short Unknown11;
        public float AlwaysPlayDistance;
        public float NeverPlayDistance;
        public float RuntimeLightprobeDeathDelay;
        public float RuntimeLocalSpaceDeathDelay;
        public List<UnknownBlock> Unknown14;

        [TagField(Padding = true, Length = 8, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused2;

        [TagStructure(Size = 0xC)]
        public class Location
        {
            public StringId MarkerName;
            public int Unknown1;
            public sbyte Unknown2;
            public sbyte Unknown3;
            public sbyte Unknown4;
            public sbyte Unknown5;
        }

        [TagStructure(Size = 0x44)]
        public class Event
        {
            public StringId Name;
            public int Unknown;
            public sbyte Unknown2;
            public sbyte Unknown3;
            public sbyte Unknown4;
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
            public class Part
            {
                public EffectEnvironment CreateInEnvironment;
                public EffectViolenceMode CreateInDisposition;
                public short PrimaryLocation;
                public short SecondaryLocation;
                public EffectEventPartFlags Flags;
                public EffectEventPartPriority Priority;
                public EffectEventPartCameraMode CameraMode;
                public Tag RuntimeBaseGroupTag;
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
                public short Unknown;
                public float Acceleration2;
                public float InnerConeAngle;
                public float OuterConeAngle;

                public enum CreateInEnvironmentValue : short
                {
                    AnyEnvironment,
                    AirOnly,
                    WaterOnly,
                    SpaceOnly,
                }

                public enum CreateInDispositionValue : short
                {
                    EitherMode,
                    ViolentModeOnly,
                    NonviolentModeOnly,
                }
            }

            [TagStructure(Size = 0x5C)]
            public class ParticleSystem
            {
                public sbyte Unknown;
                public sbyte Unknown2;
                public sbyte Unknown3;
                public sbyte Unknown4;
                public CachedTagInstance Particle;
                public uint LocationIndex;
                public ParticleCoordinateSystem CoordinateSystem;
                public EffectEnvironment Environment;
                public EffectViolenceMode Disposition;
                public ParticleCameraMode CameraMode;
                public short SortBias;
                public ushort Flags;
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
                public List<Emitter> Emitters;
                public float Unknown13;

                [TagStructure(Size = 0x2F0, MaxVersion = CacheVersion.Halo3Retail)]
                [TagStructure(Size = 0x300, MinVersion = CacheVersion.Halo3ODST)]
                public class Emitter
                {
                    public StringId Name;
                    public byte Fix1;
                    public byte Fix2;
                    public short Unknown2;

                    [TagField(MinVersion = CacheVersion.Halo3ODST)]
                    public CachedTagInstance CustomEmitterPoints;

                    public uint Unknown3;
                    public uint Unknown4;
                    public uint Unknown5;
                    public uint Unknown6;
                    public TagMapping Function1;
                    public uint Unknown10;
                    public uint Unknown11;
                    public uint Unknown12;
                    public uint Unknown13;
                    public uint Unknown14;
                    public uint Unknown15;
                    public TagMapping Function2;
                    public uint Unknown19;
                    public uint Unknown20;
                    public uint Unknown21;
                    public uint Unknown22;
                    public uint Unknown23;
                    public uint Unknown24;
                    public TagMapping Function3;
                    public TagMapping Function4;
                    public TagMapping Function5;
                    public TagMapping Function6;
                    public TagMapping Function7;
                    public TagMapping Function8;
                    public TagMapping Function9;

                    public CachedTagInstance ParticlePhysics;

                    public uint Unknown46;

                    public List<UnknownBlock> Unknown47;

                    public TagMapping Function10;
                    public uint Unknown51;
                    public uint Unknown52;
                    public uint Unknown53;
                    public uint Unknown54;
                    public uint Unknown55;
                    public uint Unknown56;
                    public TagMapping Function11;
                    public TagMapping Function12;
                    public TagMapping Function13;
                    public TagMapping Function14;
                    public TagMapping Function15;
                    public TagMapping Function16;
                    public TagMapping Function17;
                    public TagMapping Function18;
                    public int Unknown77;
                    public int Unknown78;
                    public int Unknown79;
                    public List<UnknownBlock2> Unknown80;
                    public List<CompiledFunction> CompiledFunctions;
                    public List<CompiledColorValue> CompiledColorValues;

                    [TagStructure(Size = 0x18)]
                    public class UnknownBlock
                    {
                        public short Unknown1;
                        public short Unknown2;
                        public List<UnknownBlock2> Unknown3;
                        public int Unknown4;
                        public uint Unknown5;

                        [TagStructure(Size = 0x24)]
                        public class UnknownBlock2
                        {
                            public int Unknown;
                            public TagMapping Function;
                        }
                    }

                    [TagStructure(Size = 0x10)]
                    public class UnknownBlock2
                    {
                        public uint Unknown;
                        public uint Unknown2;
                        public uint Unknown3;
                        public uint Unknown4;
                    }

                    [TagStructure(Size = 0x40)]
                    public class CompiledFunction
                    {
                        public uint Unknown;
                        public uint Unknown2;
                        public uint Unknown3;
                        public uint Unknown4;
                        public uint Unknown5;
                        public uint Unknown6;
                        public uint Unknown7;
                        public uint Unknown8;
                        public uint Unknown9;
                        public uint Unknown10;
                        public uint Unknown11;
                        public uint Unknown12;
                        public uint Unknown13;
                        public uint Unknown14;
                        public uint Unknown15;
                        public uint Unknown16;
                    }

                    [TagStructure(Size = 0x10)]
                    public class CompiledColorValue
                    {
                        public RealRgbColor Color;
                        public float Magnitude;
                    }
                }
            }
        }

        [TagStructure(Size = 0xC)]
        public class UnknownBlock
        {
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
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

    public enum EffectEventPartPriority : sbyte
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