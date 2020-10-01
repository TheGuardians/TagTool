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
        
        public enum FlagsValue : uint
        {
            DeletedWhenAttachmentDeactivates,
            RequiredForGameplayCannotOptimizeOut
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
                
                public enum FlagsValue : ushort
                {
                    FaceDownRegardlessOfLocationDecals
                }
                
                public enum AScalesValuesValue : uint
                {
                    Velocity,
                    VelocityDelta,
                    VelocityConeAngle,
                    AngularVelocity,
                    AngularVelocityDelta,
                    TypeSpecificScale
                }
                
                public enum BScalesValuesValue : uint
                {
                    Velocity,
                    VelocityDelta,
                    VelocityConeAngle,
                    AngularVelocity,
                    AngularVelocityDelta,
                    TypeSpecificScale
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
                
                public enum FlagsValue : uint
                {
                    StayAttachedToMarker,
                    RandomInitialAngle,
                    TintFromObjectColor,
                    InterpolateTintAsHsv,
                    AcrossTheLongHuePath
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
                
                public enum AScalesValuesValue : uint
                {
                    Velocity,
                    VelocityDelta,
                    VelocityConeAngle,
                    AngularVelocity,
                    AngularVelocityDelta,
                    Count,
                    CountDelta,
                    DistributionRadius,
                    DistributionRadiusDelta,
                    ParticleRadius,
                    ParticleRadiusDelta,
                    Tint
                }
                
                public enum BScalesValuesValue : uint
                {
                    Velocity,
                    VelocityDelta,
                    VelocityConeAngle,
                    AngularVelocity,
                    AngularVelocityDelta,
                    Count,
                    CountDelta,
                    DistributionRadius,
                    DistributionRadiusDelta,
                    ParticleRadius,
                    ParticleRadiusDelta,
                    Tint
                }
            }
        }
    }
}

