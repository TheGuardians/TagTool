using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "effect", Tag = "effe", Size = 0x40)]
    public class Effect : TagStructure
    {
        public FlagsValue Flags;
        public short LoopStartEvent;
        public short LoopStopEvent;
        [TagField(Length = 0x20)]
        public byte[] Padding;
        public List<EffectLocationsBlock> Locations;
        public List<EffectEventBlock> Events;
        
        [Flags]
        public enum FlagsValue : uint
        {
            DeletedWhenAttachmentDeactivates = 1 << 0,
            RequiredForGameplayCannotOptimizeOut = 1 << 1
        }
        
        [TagStructure(Size = 0x20)]
        public class EffectLocationsBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string MarkerName;
        }
        
        [TagStructure(Size = 0x44)]
        public class EffectEventBlock : TagStructure
        {
            [TagField(Length = 0x4)]
            public byte[] Padding;
            /// <summary>
            /// chance that this event will be skipped entirely
            /// </summary>
            public float SkipFraction;
            /// <summary>
            /// delay before this event takes place
            /// </summary>
            public Bounds<float> DelayBounds; // seconds
            /// <summary>
            /// duration of this event
            /// </summary>
            public Bounds<float> DurationBounds; // seconds
            [TagField(Length = 0x14)]
            public byte[] Padding1;
            public List<EffectPartBlock> Parts;
            public List<EffectParticlesBlock> Particles;
            
            [TagStructure(Size = 0x68)]
            public class EffectPartBlock : TagStructure
            {
                public CreateInValue CreateIn;
                public CreateIn1Value CreateIn1;
                public short Location;
                public FlagsValue Flags;
                [TagField(Length = 0x10)]
                public byte[] Padding;
                [TagField(ValidTags = new [] { "jpt!","obje","pctl","snd!","deca","ligh" })]
                public CachedTag Type;
                [TagField(Length = 0x18)]
                public byte[] Padding1;
                /// <summary>
                /// initial velocity along the location's forward
                /// </summary>
                public Bounds<float> VelocityBounds; // world units per second
                /// <summary>
                /// initial velocity will be inside the cone defined by this angle.
                /// </summary>
                public Angle VelocityConeAngle; // degrees
                public Bounds<Angle> AngularVelocityBounds; // degrees per second
                public Bounds<float> RadiusModifierBounds;
                [TagField(Length = 0x4)]
                public byte[] Padding2;
                public AScalesValuesValue AScalesValues;
                public BScalesValuesValue BScalesValues;
                
                public enum CreateInValue : short
                {
                    AnyEnvironment,
                    AirOnly,
                    WaterOnly,
                    SpaceOnly
                }
                
                public enum CreateIn1Value : short
                {
                    EitherMode,
                    ViolentModeOnly,
                    NonviolentModeOnly
                }
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    FaceDownRegardlessOfLocationDecals = 1 << 0
                }
                
                [Flags]
                public enum AScalesValuesValue : uint
                {
                    Velocity = 1 << 0,
                    VelocityDelta = 1 << 1,
                    VelocityConeAngle = 1 << 2,
                    AngularVelocity = 1 << 3,
                    AngularVelocityDelta = 1 << 4,
                    TypeSpecificScale = 1 << 5
                }
                
                [Flags]
                public enum BScalesValuesValue : uint
                {
                    Velocity = 1 << 0,
                    VelocityDelta = 1 << 1,
                    VelocityConeAngle = 1 << 2,
                    AngularVelocity = 1 << 3,
                    AngularVelocityDelta = 1 << 4,
                    TypeSpecificScale = 1 << 5
                }
            }
            
            [TagStructure(Size = 0xE8)]
            public class EffectParticlesBlock : TagStructure
            {
                public CreateInValue CreateIn;
                public CreateIn1Value CreateIn1;
                public CreateValue Create;
                [TagField(Length = 0x2)]
                public byte[] Padding;
                public short Location;
                [TagField(Length = 0x2)]
                public byte[] Padding1;
                /// <summary>
                /// particle initial velocity direction relative to the location's forward
                /// </summary>
                public RealEulerAngles2d RelativeDirection;
                /// <summary>
                /// particle initial position offset relative to the locatin's forward
                /// </summary>
                public RealVector3d RelativeOffset;
                [TagField(Length = 0xC)]
                public byte[] Padding2;
                [TagField(Length = 0x28)]
                public byte[] Padding3;
                [TagField(ValidTags = new [] { "part" })]
                public CachedTag ParticleType;
                public FlagsValue Flags;
                /// <summary>
                /// describes how the part creations are distributed over the event duration
                /// </summary>
                public DistributionFunctionValue DistributionFunction;
                [TagField(Length = 0x2)]
                public byte[] Padding4;
                /// <summary>
                /// number of particles created
                /// </summary>
                public Bounds<short> Count;
                /// <summary>
                /// initial distance from the location's origin
                /// </summary>
                public Bounds<float> DistributionRadius; // world units
                [TagField(Length = 0xC)]
                public byte[] Padding5;
                /// <summary>
                /// initial velocity along the specified direction
                /// </summary>
                public Bounds<float> Velocity; // world units per second
                /// <summary>
                /// particle initial velocities will be inside the cone defined by this angle and the specified direction
                /// </summary>
                public Angle VelocityConeAngle; // degrees
                public Bounds<Angle> AngularVelocity; // degrees per second
                [TagField(Length = 0x8)]
                public byte[] Padding6;
                /// <summary>
                /// particle radius
                /// </summary>
                public Bounds<float> Radius; // world units
                [TagField(Length = 0x8)]
                public byte[] Padding7;
                public RealArgbColor TintLowerBound;
                public RealArgbColor TintUpperBound;
                [TagField(Length = 0x10)]
                public byte[] Padding8;
                public AScalesValuesValue AScalesValues;
                public BScalesValuesValue BScalesValues;
                
                public enum CreateInValue : short
                {
                    AnyEnvironment,
                    AirOnly,
                    WaterOnly,
                    SpaceOnly
                }
                
                public enum CreateIn1Value : short
                {
                    EitherMode,
                    ViolentModeOnly,
                    NonviolentModeOnly
                }
                
                public enum CreateValue : short
                {
                    IndependentOfCameraMode,
                    OnlyInFirstPerson,
                    OnlyInThirdPerson,
                    InFirstPersonIfPossible
                }
                
                [Flags]
                public enum FlagsValue : uint
                {
                    StayAttachedToMarker = 1 << 0,
                    RandomInitialAngle = 1 << 1,
                    TintFromObjectColor = 1 << 2,
                    InterpolateTintAsHsv = 1 << 3,
                    AcrossTheLongHuePath = 1 << 4
                }
                
                public enum DistributionFunctionValue : short
                {
                    Start,
                    End,
                    Constant,
                    Buildup,
                    Falloff,
                    BuildupAndFalloff
                }
                
                [Flags]
                public enum AScalesValuesValue : uint
                {
                    Velocity = 1 << 0,
                    VelocityDelta = 1 << 1,
                    VelocityConeAngle = 1 << 2,
                    AngularVelocity = 1 << 3,
                    AngularVelocityDelta = 1 << 4,
                    Count = 1 << 5,
                    CountDelta = 1 << 6,
                    DistributionRadius = 1 << 7,
                    DistributionRadiusDelta = 1 << 8,
                    ParticleRadius = 1 << 9,
                    ParticleRadiusDelta = 1 << 10,
                    Tint = 1 << 11
                }
                
                [Flags]
                public enum BScalesValuesValue : uint
                {
                    Velocity = 1 << 0,
                    VelocityDelta = 1 << 1,
                    VelocityConeAngle = 1 << 2,
                    AngularVelocity = 1 << 3,
                    AngularVelocityDelta = 1 << 4,
                    Count = 1 << 5,
                    CountDelta = 1 << 6,
                    DistributionRadius = 1 << 7,
                    DistributionRadiusDelta = 1 << 8,
                    ParticleRadius = 1 << 9,
                    ParticleRadiusDelta = 1 << 10,
                    Tint = 1 << 11
                }
            }
        }
    }
}

