using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "effect", Tag = "effe", Size = 0x30)]
    public class Effect : TagStructure
    {
        public FlagsValue Flags;
        public short LoopStartEvent;
        [TagField(Length = 0x2)]
        public byte[] Unknown;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public List<EffectLocationsBlock> Locations;
        public List<EffectEventBlock> Events;
        [TagField(ValidTags = new [] { "lsnd" })]
        public CachedTag LoopingSound;
        public short Location;
        [TagField(Length = 0x2)]
        public byte[] Unknown1;
        public float AlwaysPlayDistance;
        public float NeverPlayDistance;
        
        [Flags]
        public enum FlagsValue : uint
        {
            DeletedWhenAttachmentDeactivates = 1 << 0
        }
        
        [TagStructure(Size = 0x4)]
        public class EffectLocationsBlock : TagStructure
        {
            /// <summary>
            /// In addition to the marker in the render model there are several special marker names:
            /// 
            /// replace
            /// Replace allows you to use
            /// the same effect with different markers. Damage transition effects support this for example.
            /// 
            /// gravity, up
            /// The direction of
            /// gravity (down) and the opposite direction (up).  Always supplied
            /// 
            /// normal
            /// Vector pointing directly away from the surface
            /// you collided with. Supplied for effects from collision.
            /// 
            /// forward
            /// The 'negative incident' vector i.e. the direction the
            /// object is moving in. Most commonly used to generated decals. Supplied for effects from collision.
            /// 
            /// backward
            /// The
            /// 'incident' vector i.e. the opposite of the direction the object is moving in. Supplied for effects from
            /// collision.
            /// 
            /// reflection
            /// The way the effect would reflect off the surface it hit. Supplied for effects from
            /// collision.
            /// 
            /// root
            /// The object root (pivot). These can used for all effects which are associated with an object.
            /// 
            /// impact
            /// The
            /// location of a havok impact.
            /// 
            /// 
            /// </summary>
            public StringId MarkerName;
        }
        
        [TagStructure(Size = 0x38)]
        public class EffectEventBlock : TagStructure
        {
            public FlagsValue Flags;
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
            public List<EffectPartBlock> Parts;
            public List<BeamBlock> Beams;
            public List<EffectAccelerationsBlock> Accelerations;
            public List<ParticleSystemDefinitionBlockNew> ParticleSystems;
            
            [Flags]
            public enum FlagsValue : uint
            {
                DisabledForDebugging = 1 << 0
            }
            
            [TagStructure(Size = 0x38)]
            public class EffectPartBlock : TagStructure
            {
                public CreateInValue CreateIn;
                public CreateInValue1 CreateIn1;
                public short Location;
                public FlagsValue Flags;
                public Tag RuntimeBaseGroupTag;
                [TagField(ValidTags = new [] { "jpt!","obje","snd!","deca","coln","ligh","MGS2","tdtl","lens" })]
                public CachedTag Type;
                /// <summary>
                /// initial velocity along the location's forward, for decals the distance at which decal is created (defaults to 0.5)
                /// </summary>
                public Bounds<float> VelocityBounds; // world units per second
                /// <summary>
                /// initial velocity will be inside the cone defined by this angle.
                /// </summary>
                public Angle VelocityConeAngle; // degrees
                public Bounds<Angle> AngularVelocityBounds; // degrees per second
                public Bounds<float> RadiusModifierBounds;
                public AScalesValuesValue AScalesValues;
                public BScalesValuesValue BScalesValues;
                
                public enum CreateInValue : short
                {
                    AnyEnvironment,
                    AirOnly,
                    WaterOnly,
                    SpaceOnly
                }
                
                public enum CreateInValue1 : short
                {
                    EitherMode,
                    ViolentModeOnly,
                    NonviolentModeOnly
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
            
            [TagStructure(Size = 0x3C)]
            public class BeamBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "shad" })]
                public CachedTag Shader;
                public short Location;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                /// <summary>
                /// tint color of the beam
                /// </summary>
                public ColorFunctionStructBlock Color;
                /// <summary>
                /// how much to fade out beam
                /// </summary>
                public ScalarFunctionStructBlock Alpha;
                /// <summary>
                /// how wide in world units
                /// </summary>
                public ScalarFunctionStructBlock1 Width;
                /// <summary>
                /// how long in world units
                /// </summary>
                public ScalarFunctionStructBlock2 Length;
                /// <summary>
                /// rotate the marker
                /// </summary>
                public ScalarFunctionStructBlock3 Yaw;
                /// <summary>
                /// rotate the marker
                /// </summary>
                public ScalarFunctionStructBlock4 Pitch;
                
                [TagStructure(Size = 0x8)]
                public class ColorFunctionStructBlock : TagStructure
                {
                    public MappingFunctionBlock Function;
                    
                    [TagStructure(Size = 0x8)]
                    public class MappingFunctionBlock : TagStructure
                    {
                        public List<ByteBlock> Data;
                        
                        [TagStructure(Size = 0x1)]
                        public class ByteBlock : TagStructure
                        {
                            public sbyte Value;
                        }
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ScalarFunctionStructBlock : TagStructure
                {
                    public MappingFunctionBlock Function;
                    
                    [TagStructure(Size = 0x8)]
                    public class MappingFunctionBlock : TagStructure
                    {
                        public List<ByteBlock> Data;
                        
                        [TagStructure(Size = 0x1)]
                        public class ByteBlock : TagStructure
                        {
                            public sbyte Value;
                        }
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ScalarFunctionStructBlock1 : TagStructure
                {
                    public MappingFunctionBlock Function;
                    
                    [TagStructure(Size = 0x8)]
                    public class MappingFunctionBlock : TagStructure
                    {
                        public List<ByteBlock> Data;
                        
                        [TagStructure(Size = 0x1)]
                        public class ByteBlock : TagStructure
                        {
                            public sbyte Value;
                        }
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ScalarFunctionStructBlock2 : TagStructure
                {
                    public MappingFunctionBlock Function;
                    
                    [TagStructure(Size = 0x8)]
                    public class MappingFunctionBlock : TagStructure
                    {
                        public List<ByteBlock> Data;
                        
                        [TagStructure(Size = 0x1)]
                        public class ByteBlock : TagStructure
                        {
                            public sbyte Value;
                        }
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ScalarFunctionStructBlock3 : TagStructure
                {
                    public MappingFunctionBlock Function;
                    
                    [TagStructure(Size = 0x8)]
                    public class MappingFunctionBlock : TagStructure
                    {
                        public List<ByteBlock> Data;
                        
                        [TagStructure(Size = 0x1)]
                        public class ByteBlock : TagStructure
                        {
                            public sbyte Value;
                        }
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ScalarFunctionStructBlock4 : TagStructure
                {
                    public MappingFunctionBlock Function;
                    
                    [TagStructure(Size = 0x8)]
                    public class MappingFunctionBlock : TagStructure
                    {
                        public List<ByteBlock> Data;
                        
                        [TagStructure(Size = 0x1)]
                        public class ByteBlock : TagStructure
                        {
                            public sbyte Value;
                        }
                    }
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class EffectAccelerationsBlock : TagStructure
            {
                public CreateInValue CreateIn;
                public CreateInValue1 CreateIn1;
                public short Location;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
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
                
                public enum CreateInValue1 : short
                {
                    EitherMode,
                    ViolentModeOnly,
                    NonviolentModeOnly
                }
            }
            
            [TagStructure(Size = 0x38)]
            public class ParticleSystemDefinitionBlockNew : TagStructure
            {
                [TagField(ValidTags = new [] { "prt3","PRTM" })]
                public CachedTag Particle;
                public int Location;
                public CoordinateSystemValue CoordinateSystem;
                public EnvironmentValue Environment;
                public DispositionValue Disposition;
                public CameraModeValue CameraMode;
                /// <summary>
                /// use values between -10 and 10 to move closer and farther from camera (positive is closer)
                /// </summary>
                public short SortBias;
                public FlagsValue Flags;
                /// <summary>
                /// defaults to 0.0
                /// </summary>
                public float LodInDistance;
                /// <summary>
                /// defaults to 0.0
                /// </summary>
                public float LodFeatherInDelta;
                public float InverseLodFeatherIn;
                /// <summary>
                /// defaults to 30.0
                /// </summary>
                public float LodOutDistance;
                /// <summary>
                /// defaults to 10.0
                /// </summary>
                public float LodFeatherOutDelta;
                public float InverseLodFeatherOut;
                public List<ParticleSystemEmitterDefinitionBlock> Emitters;
                
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
                
                [TagStructure(Size = 0xB8)]
                public class ParticleSystemEmitterDefinitionBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "pmov" })]
                    public CachedTag ParticlePhysics;
                    public EditablePropertyBlockGen2 ParticleEmissionRate;
                    public EditablePropertyBlockGen2 ParticleLifespan;
                    public EditablePropertyBlockGen2 ParticleVelocity;
                    public EditablePropertyBlockGen2 ParticleAngularVelocity;
                    public EditablePropertyBlockGen2 ParticleSize;
                    public EditablePropertyBlockGen2 ParticleTint;
                    public EditablePropertyBlockGen2 ParticleAlpha;
                    public EmissionShapeValue EmissionShape;
                    public EditablePropertyBlockGen2 EmissionRadius;
                    public EditablePropertyBlockGen2 EmissionAngle;
                    public RealPoint3d TranslationalOffset;
                    /// <summary>
                    /// particle initial velocity direction relative to the location's forward
                    /// </summary>
                    public RealEulerAngles2d RelativeDirection;
                    [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    [TagStructure(Size = 0x10)]
                    public class EditablePropertyBlockGen2 : TagStructure
                    {
                        public VariableValue InputVariable;
                        public VariableValue RangeVariable;
                        public OutputModifierValue OutputModifier;
                        public VariableValue OutputModifierInput;
                        public List<byte> Mapping;

                        public enum VariableValue : short
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
                            Unknown,
                            Plus,
                            Times
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

