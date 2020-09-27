using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "effect", Tag = "effe", Size = 0x40)]
    public class Effect : TagStructure
    {
        public FlagsValue Flags;
        public short LoopStartEvent;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Unknown1;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding1;
        public List<EffectLocationDefinition> Locations;
        public List<EffectEventDefinition> Events;
        /// <summary>
        /// Looping Sound
        /// </summary>
        public CachedTag LoopingSound;
        public short Location;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Unknown2;
        public float AlwaysPlayDistance;
        public float NeverPlayDistance;
        
        [Flags]
        public enum FlagsValue : uint
        {
            DeletedWhenAttachmentDeactivates = 1 << 0
        }
        
        [TagStructure(Size = 0x4)]
        public class EffectLocationDefinition : TagStructure
        {
            /// <summary>
            /// MARKER NAMES
            /// </summary>
            /// <remarks>
            /// In addition to the marker in the render model there are several special marker names:
            /// 
            /// replace
            /// Replace allows you to use the same effect with different markers. Damage transition effects support this for example.
            /// 
            /// gravity, up
            /// The direction of gravity (down) and the opposite direction (up).  Always supplied
            /// 
            /// normal
            /// Vector pointing directly away from the surface you collided with. Supplied for effects from collision.
            /// 
            /// forward
            /// The 'negative incident' vector i.e. the direction the object is moving in. Most commonly used to generated decals. Supplied for effects from collision.
            /// 
            /// backward
            /// The 'incident' vector i.e. the opposite of the direction the object is moving in. Supplied for effects from collision.
            /// 
            /// reflection
            /// The way the effect would reflect off the surface it hit. Supplied for effects from collision.
            /// 
            /// root
            /// The object root (pivot). These can used for all effects which are associated with an object.
            /// 
            /// impact
            /// The location of a havok impact.
            /// 
            /// 
            /// </remarks>
            public StringId MarkerName;
        }
        
        [TagStructure(Size = 0x48)]
        public class EffectEventDefinition : TagStructure
        {
            public FlagsValue Flags;
            public float SkipFraction; // chance that this event will be skipped entirely
            public Bounds<float> DelayBounds; // seconds
            public Bounds<float> DurationBounds; // seconds
            public List<EffectPartDefinition> Parts;
            public List<BeamDefinition> Beams;
            public List<EffectAccelerationDefinition> Accelerations;
            public List<ParticleSystemDefinition> ParticleSystems;
            
            [Flags]
            public enum FlagsValue : uint
            {
                DisabledForDebugging = 1 << 0
            }
            
            [TagStructure(Size = 0x40)]
            public class EffectPartDefinition : TagStructure
            {
                public CreateInValue CreateIn;
                public CreateInValue CreateIn1;
                public short Location;
                public FlagsValue Flags;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding1;
                public CachedTag Type;
                public Bounds<float> VelocityBounds; // world units per second
                public Angle VelocityConeAngle; // degrees
                public Bounds<Angle> AngularVelocityBounds; // degrees per second
                public Bounds<float> RadiusModifierBounds;
                /// <summary>
                /// SCALE MODIFIERS
                /// </summary>
                public AScalesValuesValue AScalesValues;
                public BScalesValuesValue BScalesValues;
                
                public enum CreateInValue : short
                {
                    AnyEnvironment,
                    AirOnly,
                    WaterOnly,
                    SpaceOnly
                }
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    FaceDownRegardlessOfLocationDecals = 1 << 0,
                    OffsetOriginAwayFromGeometryLights = 1 << 1,
                    NeverAttachedToObject = 1 << 2,
                    DisabledForDebugging = 1 << 3,
                    DrawRegardlessOfDistance = 1 << 4
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
            
            [TagStructure(Size = 0x5C)]
            public class BeamDefinition : TagStructure
            {
                public CachedTag Shader;
                public short Location;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                /// <summary>
                /// COLOR
                /// </summary>
                /// <remarks>
                /// tint color of the beam
                /// </remarks>
                public FunctionDefinition Color;
                /// <summary>
                /// ALPHA
                /// </summary>
                /// <remarks>
                /// how much to fade out beam
                /// </remarks>
                public FunctionDefinition Alpha;
                /// <summary>
                /// WIDTH
                /// </summary>
                /// <remarks>
                /// how wide in world units
                /// </remarks>
                public FunctionDefinition Width;
                /// <summary>
                /// LENGTH
                /// </summary>
                /// <remarks>
                /// how long in world units
                /// </remarks>
                public FunctionDefinition Length;
                /// <summary>
                /// YAW
                /// </summary>
                /// <remarks>
                /// rotate the marker
                /// </remarks>
                public FunctionDefinition Yaw;
                /// <summary>
                /// PITCH
                /// </summary>
                /// <remarks>
                /// rotate the marker
                /// </remarks>
                public FunctionDefinition Pitch;
                
                [TagStructure(Size = 0xC)]
                public class FunctionDefinition : TagStructure
                {
                    public FunctionDefinition Function;
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class EffectAccelerationDefinition : TagStructure
            {
                public CreateInValue CreateIn;
                public CreateInValue CreateIn1;
                public short Location;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public float Acceleration;
                public float InnerConeAngle; // degrees
                public float OuterConeAngle; // degrees
                
                public enum CreateInValue : short
                {
                    AnyEnvironment,
                    AirOnly,
                    WaterOnly,
                    SpaceOnly
                }
            }
            
            [TagStructure(Size = 0x44)]
            public class ParticleSystemDefinition : TagStructure
            {
                public CachedTag Particle;
                public int Location;
                public CoordinateSystemValue CoordinateSystem;
                public EnvironmentValue Environment;
                public DispositionValue Disposition;
                public CameraModeValue CameraMode;
                public short SortBias; // use values between -10 and 10 to move closer and farther from camera (positive is closer)
                public FlagsValue Flags;
                public float LodInDistance; // defaults to 0.0
                public float LodFeatherInDelta; // defaults to 0.0
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown1;
                public float LodOutDistance; // defaults to 30.0
                public float LodFeatherOutDelta; // defaults to 10.0
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown2;
                public List<ParticleEmitterDefinition> Emitters;
                
                public enum CoordinateSystemValue : short
                {
                    World,
                    Local,
                    Parent
                }
                
                public enum EnvironmentValue : short
                {
                    AnyEnvironment,
                    AirOnly,
                    WaterOnly,
                    SpaceOnly
                }
                
                public enum DispositionValue : short
                {
                    EitherMode,
                    ViolentModeOnly,
                    NonviolentModeOnly
                }
                
                public enum CameraModeValue : short
                {
                    IndependentOfCameraMode,
                    OnlyInFirstPerson,
                    OnlyInThirdPerson,
                    BothFirstAndThird
                }
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    Glow = 1 << 0,
                    Cinematics = 1 << 1,
                    LoopingParticle = 1 << 2,
                    DisabledForDebugging = 1 << 3,
                    InheritEffectVelocity = 1 << 4,
                    DonTRenderSystem = 1 << 5,
                    RenderWhenZoomed = 1 << 6,
                    SpreadBetweenTicks = 1 << 7,
                    PersistentParticle = 1 << 8,
                    ExpensiveVisibility = 1 << 9
                }
                
                [TagStructure(Size = 0xE4)]
                public class ParticleEmitterDefinition : TagStructure
                {
                    public CachedTag ParticlePhysics;
                    /// <summary>
                    /// particle emission rate (particles/tick)
                    /// </summary>
                    public ParticleProperty ParticleEmissionRate;
                    /// <summary>
                    /// particle lifespan(seconds)
                    /// </summary>
                    public ParticleProperty ParticleLifespan;
                    /// <summary>
                    /// particle velocity(world units/second)
                    /// </summary>
                    public ParticleProperty ParticleVelocity;
                    /// <summary>
                    /// particle angular velocity(degrees/second)
                    /// </summary>
                    public ParticleProperty ParticleAngularVelocity;
                    /// <summary>
                    /// particle size(world units)
                    /// </summary>
                    public ParticleProperty ParticleSize;
                    /// <summary>
                    /// particle tint
                    /// </summary>
                    public ParticleProperty ParticleTint;
                    /// <summary>
                    /// particle alpha
                    /// </summary>
                    public ParticleProperty ParticleAlpha;
                    /// <summary>
                    /// EMISSION SETTINGS
                    /// </summary>
                    public EmissionShapeValue EmissionShape;
                    /// <summary>
                    /// emission radius(world units)
                    /// </summary>
                    public ParticleProperty EmissionRadius;
                    /// <summary>
                    /// emission angle(degrees)
                    /// </summary>
                    public ParticleProperty EmissionAngle;
                    public RealPoint3d TranslationalOffset;
                    public RealEulerAngles2d RelativeDirection; // particle initial velocity direction relative to the location's forward
                    [TagField(Flags = Padding, Length = 8)]
                    public byte[] Padding1;
                    
                    [TagStructure(Size = 0x14)]
                    public class ParticleProperty : TagStructure
                    {
                        public InputVariableValue InputVariable;
                        public RangeVariableValue RangeVariable;
                        public OutputModifierValue OutputModifier;
                        public OutputModifierInputValue OutputModifierInput;
                        public FunctionDefinition Mapping;
                        
                        public enum InputVariableValue : short
                        {
                            ParticleAge,
                            ParticleEmitTime,
                            ParticleRandom1,
                            ParticleRandom2,
                            EmitterAge,
                            EmitterRandom1,
                            EmitterRandom2,
                            SystemLod,
                            GameTime,
                            EffectAScale,
                            EffectBScale,
                            ParticleRotation,
                            ExplosionAnimation,
                            ExplosionRotation,
                            ParticleRandom3,
                            ParticleRandom4,
                            LocationRandom
                        }
                        
                        public enum RangeVariableValue : short
                        {
                            ParticleAge,
                            ParticleEmitTime,
                            ParticleRandom1,
                            ParticleRandom2,
                            EmitterAge,
                            EmitterRandom1,
                            EmitterRandom2,
                            SystemLod,
                            GameTime,
                            EffectAScale,
                            EffectBScale,
                            ParticleRotation,
                            ExplosionAnimation,
                            ExplosionRotation,
                            ParticleRandom3,
                            ParticleRandom4,
                            LocationRandom
                        }
                        
                        public enum OutputModifierValue : short
                        {
                            Unknown0,
                            Plus,
                            Times
                        }
                        
                        public enum OutputModifierInputValue : short
                        {
                            ParticleAge,
                            ParticleEmitTime,
                            ParticleRandom1,
                            ParticleRandom2,
                            EmitterAge,
                            EmitterRandom1,
                            EmitterRandom2,
                            SystemLod,
                            GameTime,
                            EffectAScale,
                            EffectBScale,
                            ParticleRotation,
                            ExplosionAnimation,
                            ExplosionRotation,
                            ParticleRandom3,
                            ParticleRandom4,
                            LocationRandom
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
                    
                    public enum EmissionShapeValue : int
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
                        Line
                    }
                }
            }
        }
    }
}

