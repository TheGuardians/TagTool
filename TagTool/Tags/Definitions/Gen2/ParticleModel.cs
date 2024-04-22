using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "particle_model", Tag = "PRTM", Size = 0xE0)]
    public class ParticleModel : TagStructure
    {
        public FlagsValue Flags;
        public OrientationValue Orientation;
        [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        [TagField(ValidTags = new [] { "shad" })]
        public CachedTag Shader;
        /// <summary>
        /// scale of model on x axis
        /// </summary>
        public ParticlePropertyScalarStructNewBlock ScaleX;
        /// <summary>
        /// scale of model on y axis
        /// </summary>
        public ParticlePropertyScalarStructNewBlock1 ScaleY;
        /// <summary>
        /// scale of model on z axis
        /// </summary>
        public ParticlePropertyScalarStructNewBlock2 ScaleZ;
        /// <summary>
        /// rotation where 0=0 degrees, 0.5=180 degrees, 1.0=360 degrees
        /// </summary>
        public ParticlePropertyScalarStructNewBlock3 Rotation;
        /// <summary>
        /// collision occurs when particle physics has collision, death spawned when particle dies
        /// </summary>
        /// <summary>
        /// effect, material effect or sound spawned when this particle collides with something
        /// </summary>
        [TagField(ValidTags = new [] { "effe","snd!","foot" })]
        public CachedTag CollisionEffect;
        /// <summary>
        /// effect, material effect or sound spawned when this particle dies
        /// </summary>
        [TagField(ValidTags = new [] { "effe","snd!","foot" })]
        public CachedTag DeathEffect;
        /// <summary>
        /// Locations:
        /// up - particles shoot straight up
        /// gravity - particles shoot straight down
        /// NONE - opposite of particle direction
        /// </summary>
        public List<EffectLocationsBlock> Locations;
        public List<ParticleSystemDefinitionBlockNew> AttachedParticleSystems;
        public List<ParticleModelsBlock> Models;
        public List<ParticleModelVerticesBlock> RawVertices;
        public List<ParticleModelIndicesBlock> Indices;
        public List<CachedDataBlock> CachedData;
        public GlobalGeometryBlockInfoStructBlock GeometrySectionInfo;
        [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding3;
        
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
        
        [TagStructure(Size = 0x10)]
        public class ParticlePropertyScalarStructNewBlock : TagStructure
        {
            public InputVariableValue InputVariable;
            public RangeVariableValue RangeVariable;
            public OutputModifierValue OutputModifier;
            public OutputModifierInputValue OutputModifierInput;
            public MappingFunctionBlock Mapping;
            
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
                Unknown,
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
        
        [TagStructure(Size = 0x10)]
        public class ParticlePropertyScalarStructNewBlock1 : TagStructure
        {
            public InputVariableValue InputVariable;
            public RangeVariableValue RangeVariable;
            public OutputModifierValue OutputModifier;
            public OutputModifierInputValue OutputModifierInput;
            public MappingFunctionBlock Mapping;
            
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
                Unknown,
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
        
        [TagStructure(Size = 0x10)]
        public class ParticlePropertyScalarStructNewBlock2 : TagStructure
        {
            public InputVariableValue InputVariable;
            public RangeVariableValue RangeVariable;
            public OutputModifierValue OutputModifier;
            public OutputModifierInputValue OutputModifierInput;
            public MappingFunctionBlock Mapping;
            
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
                Unknown,
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
        
        [TagStructure(Size = 0x10)]
        public class ParticlePropertyScalarStructNewBlock3 : TagStructure
        {
            public InputVariableValue InputVariable;
            public RangeVariableValue RangeVariable;
            public OutputModifierValue OutputModifier;
            public OutputModifierInputValue OutputModifierInput;
            public MappingFunctionBlock Mapping;
            
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
                Unknown,
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
            [TagField(Length = 0x4)]
            public byte[] Unknown;
            /// <summary>
            /// defaults to 30.0
            /// </summary>
            public float LodOutDistance;
            /// <summary>
            /// defaults to 10.0
            /// </summary>
            public float LodFeatherOutDelta;
            [TagField(Length = 0x4)]
            public byte[] Unknown1;
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
                public ParticlePropertyScalarStructNewBlock ParticleEmissionRate;
                public ParticlePropertyScalarStructNewBlock1 ParticleLifespan;
                public ParticlePropertyScalarStructNewBlock2 ParticleVelocity;
                public ParticlePropertyScalarStructNewBlock3 ParticleAngularVelocity;
                public ParticlePropertyScalarStructNewBlock4 ParticleSize;
                public ParticlePropertyColorStructNewBlock ParticleTint;
                public ParticlePropertyScalarStructNewBlock5 ParticleAlpha;
                public EmissionShapeValue EmissionShape;
                public ParticlePropertyScalarStructNewBlock6 EmissionRadius;
                public ParticlePropertyScalarStructNewBlock7 EmissionAngle;
                public RealPoint3d TranslationalOffset;
                /// <summary>
                /// particle initial velocity direction relative to the location's forward
                /// </summary>
                public RealEulerAngles2d RelativeDirection;
                [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [TagStructure(Size = 0x10)]
                public class ParticlePropertyScalarStructNewBlock : TagStructure
                {
                    public InputVariableValue InputVariable;
                    public RangeVariableValue RangeVariable;
                    public OutputModifierValue OutputModifier;
                    public OutputModifierInputValue OutputModifierInput;
                    public MappingFunctionBlock Mapping;
                    
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
                        Unknown,
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
                
                [TagStructure(Size = 0x10)]
                public class ParticlePropertyScalarStructNewBlock1 : TagStructure
                {
                    public InputVariableValue InputVariable;
                    public RangeVariableValue RangeVariable;
                    public OutputModifierValue OutputModifier;
                    public OutputModifierInputValue OutputModifierInput;
                    public MappingFunctionBlock Mapping;
                    
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
                        Unknown,
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
                
                [TagStructure(Size = 0x10)]
                public class ParticlePropertyScalarStructNewBlock2 : TagStructure
                {
                    public InputVariableValue InputVariable;
                    public RangeVariableValue RangeVariable;
                    public OutputModifierValue OutputModifier;
                    public OutputModifierInputValue OutputModifierInput;
                    public MappingFunctionBlock Mapping;
                    
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
                        Unknown,
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
                
                [TagStructure(Size = 0x10)]
                public class ParticlePropertyScalarStructNewBlock3 : TagStructure
                {
                    public InputVariableValue InputVariable;
                    public RangeVariableValue RangeVariable;
                    public OutputModifierValue OutputModifier;
                    public OutputModifierInputValue OutputModifierInput;
                    public MappingFunctionBlock Mapping;
                    
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
                        Unknown,
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
                
                [TagStructure(Size = 0x10)]
                public class ParticlePropertyScalarStructNewBlock4 : TagStructure
                {
                    public InputVariableValue InputVariable;
                    public RangeVariableValue RangeVariable;
                    public OutputModifierValue OutputModifier;
                    public OutputModifierInputValue OutputModifierInput;
                    public MappingFunctionBlock Mapping;
                    
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
                        Unknown,
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
                
                [TagStructure(Size = 0x10)]
                public class ParticlePropertyColorStructNewBlock : TagStructure
                {
                    public InputVariableValue InputVariable;
                    public RangeVariableValue RangeVariable;
                    public OutputModifierValue OutputModifier;
                    public OutputModifierInputValue OutputModifierInput;
                    public MappingFunctionBlock Mapping;
                    
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
                        Unknown,
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
                
                [TagStructure(Size = 0x10)]
                public class ParticlePropertyScalarStructNewBlock5 : TagStructure
                {
                    public InputVariableValue InputVariable;
                    public RangeVariableValue RangeVariable;
                    public OutputModifierValue OutputModifier;
                    public OutputModifierInputValue OutputModifierInput;
                    public MappingFunctionBlock Mapping;
                    
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
                        Unknown,
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
                
                [TagStructure(Size = 0x10)]
                public class ParticlePropertyScalarStructNewBlock6 : TagStructure
                {
                    public InputVariableValue InputVariable;
                    public RangeVariableValue RangeVariable;
                    public OutputModifierValue OutputModifier;
                    public OutputModifierInputValue OutputModifierInput;
                    public MappingFunctionBlock Mapping;
                    
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
                        Unknown,
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
                
                [TagStructure(Size = 0x10)]
                public class ParticlePropertyScalarStructNewBlock7 : TagStructure
                {
                    public InputVariableValue InputVariable;
                    public RangeVariableValue RangeVariable;
                    public OutputModifierValue OutputModifier;
                    public OutputModifierInputValue OutputModifierInput;
                    public MappingFunctionBlock Mapping;
                    
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
                        Unknown,
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
        }
        
        [TagStructure(Size = 0x8)]
        public class ParticleModelsBlock : TagStructure
        {
            public StringId ModelName;
            public short IndexStart;
            public short IndexCount;
        }
        
        [TagStructure(Size = 0x38)]
        public class ParticleModelVerticesBlock : TagStructure
        {
            public RealPoint3d Position;
            public RealVector3d Normal;
            public RealVector3d Tangent;
            public RealVector3d Binormal;
            public RealPoint2d Texcoord;
        }
        
        [TagStructure(Size = 0x2)]
        public class ParticleModelIndicesBlock : TagStructure
        {
            public short Index;
        }
        
        [TagStructure(Size = 0x20)]
        public class CachedDataBlock : TagStructure
        {
            public VertexBuffer VertexBuffer;
        }
        
        [TagStructure(Size = 0x24)]
        public class GlobalGeometryBlockInfoStructBlock : TagStructure
        {
            public int BlockOffset;
            public int BlockSize;
            public int SectionDataSize;
            public int ResourceDataSize;
            public List<GlobalGeometryBlockResourceBlock> Resources;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short OwnerTagSectionOffset;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            
            [TagStructure(Size = 0x10)]
            public class GlobalGeometryBlockResourceBlock : TagStructure
            {
                public TypeValue Type;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
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

