using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "particle_model", Tag = "PRTM", Size = 0x124)]
    public class ParticleModel : TagStructure
    {
        public FlagsValue Flags;
        public OrientationValue Orientation;
        [TagField(Flags = Padding, Length = 16)]
        public byte[] Padding1;
        public CachedTag Shader;
        /// <summary>
        /// SCALE X
        /// </summary>
        /// <remarks>
        /// scale of model on x axis
        /// </remarks>
        public ParticleProperty ScaleX;
        /// <summary>
        /// SCALE Y
        /// </summary>
        /// <remarks>
        /// scale of model on y axis
        /// </remarks>
        public ParticleProperty ScaleY;
        /// <summary>
        /// SCALE Z
        /// </summary>
        /// <remarks>
        /// scale of model on z axis
        /// </remarks>
        public ParticleProperty ScaleZ;
        /// <summary>
        /// ROTATION
        /// </summary>
        /// <remarks>
        /// rotation where 0=0 degrees, 0.5=180 degrees, 1.0=360 degrees
        /// </remarks>
        public ParticleProperty Rotation;
        /// <summary>
        /// Spawned Effects
        /// </summary>
        /// <remarks>
        /// collision occurs when particle physics has collision, death spawned when particle dies
        /// </remarks>
        public CachedTag CollisionEffect; // effect, material effect or sound spawned when this particle collides with something
        public CachedTag DeathEffect; // effect, material effect or sound spawned when this particle dies
        /// <summary>
        /// Attached Particle Systems
        /// </summary>
        /// <remarks>
        /// Locations:
        /// up - particles shoot straight up
        /// gravity - particles shoot straight down
        /// NONE - opposite of particle direction
        /// </remarks>
        public List<EffectLocationDefinition> Locations;
        public List<ParticleSystemDefinition> AttachedParticleSystems;
        public List<ParticleModelBlock> Models;
        public List<ParticleModelVertex> RawVertices;
        public List<Word> Indices;
        public List<CachedDataBlock> CachedData;
        public GeometryBlockInfo GeometrySectionInfo;
        [TagField(Flags = Padding, Length = 16)]
        public byte[] Padding2;
        [TagField(Flags = Padding, Length = 8)]
        public byte[] Padding3;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding4;
        
        [Flags]
        public enum FlagsValue : uint
        {
            Spins = 1 << 0,
            RandomUMirror = 1 << 1,
            RandomVMirror = 1 << 2,
            FrameAnimationOneShot = 1 << 3,
            SelectRandomSequence = 1 << 4,
            DisableFrameBlending = 1 << 5,
            CanAnimateBackwards = 1 << 6,
            ReceiveLightmapLighting = 1 << 7,
            TintFromDiffuseTexture = 1 << 8,
            DiesAtRest = 1 << 9,
            DiesOnStructureCollision = 1 << 10,
            DiesInMedia = 1 << 11,
            DiesInAir = 1 << 12,
            BitmapAuthoredVertically = 1 << 13,
            HasSweetener = 1 << 14
        }
        
        public enum OrientationValue : int
        {
            ScreenFacing,
            ParallelToDirection,
            PerpendicularToDirection,
            Vertical,
            Horizontal
        }
        
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
        
        [TagStructure(Size = 0x8)]
        public class ParticleModelBlock : TagStructure
        {
            public StringId ModelName;
            public short IndexStart;
            public short IndexCount;
        }
        
        [TagStructure(Size = 0x38)]
        public class ParticleModelVertex : TagStructure
        {
            public RealPoint3d Position;
            public RealVector3d Normal;
            public RealVector3d Tangent;
            public RealVector3d Binormal;
            public RealPoint2d Texcoord;
        }
        
        [TagStructure(Size = 0x2)]
        public class Word : TagStructure
        {
            public short Index;
        }
        
        [TagStructure(Size = 0x20)]
        public class CachedDataBlock : TagStructure
        {
            public VertexBuffer VertexBuffer;
        }
        
        [TagStructure(Size = 0x28)]
        public class GeometryBlockInfo : TagStructure
        {
            /// <summary>
            /// BLOCK INFO
            /// </summary>
            public int BlockOffset;
            public int BlockSize;
            public int SectionDataSize;
            public int ResourceDataSize;
            public List<GeometryBlockResource> Resources;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public short OwnerTagSectionOffset;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding3;
            
            [TagStructure(Size = 0x10)]
            public class GeometryBlockResource : TagStructure
            {
                public TypeValue Type;
                [TagField(Flags = Padding, Length = 3)]
                public byte[] Padding1;
                public short PrimaryLocator;
                public short SecondaryLocator;
                public int ResourceDataSize;
                public int ResourceDataOffset;
                
                public enum TypeValue : sbyte
                {
                    TagBlock,
                    TagData,
                    VertexBuffer
                }
            }
        }
    }
}

