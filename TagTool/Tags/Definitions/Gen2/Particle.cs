using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "particle", Tag = "prt3", Size = 0xBC)]
    public class Particle : TagStructure
    {
        public FlagsValue Flags;
        public ParticleBillboardStyleValue ParticleBillboardStyle;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public short FirstSequenceIndex;
        public short SequenceCount;
        [TagField(ValidTags = new [] { "stem" })]
        public CachedTag ShaderTemplate;
        public List<GlobalShaderParameterBlock> ShaderParameters;
        /// <summary>
        /// controls how the color of the particle changes as
        /// a function of its input
        /// </summary>
        public ParticlePropertyColorStructNewBlock Color;
        /// <summary>
        /// seperate from color, controls how the particle fades
        /// as a function of its input
        /// </summary>
        public ParticlePropertyScalarStructNewBlock Alpha;
        /// <summary>
        /// controls how the size of a particle changes as
        /// a function of its input
        /// </summary>
        public ParticlePropertyScalarStructNewBlock1 Scale;
        /// <summary>
        /// controls how the particle rotates. 0= 0 degrees, .5= 180 degrees, 1.0= 360 degrees
        /// </summary>
        public ParticlePropertyScalarStructNewBlock2 Rotation;
        /// <summary>
        /// provides finer grain control over how the animation
        /// happens.  a output of 0.25 means that when that input
        /// is fed in, the
        /// particle will be 25% of the way through
        /// its animation.
        /// </summary>
        public ParticlePropertyScalarStructNewBlock3 FrameIndex;
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
        public List<ShaderPostprocessDefinitionNewBlock> Unknown;
        [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
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
        
        public enum ParticleBillboardStyleValue : short
        {
            ScreenFacing,
            ParallelToDirection,
            PerpendicularToDirection,
            Vertical,
            Horizontal
        }
        
        [TagStructure(Size = 0x28)]
        public class GlobalShaderParameterBlock : TagStructure
        {
            public StringId Name;
            public TypeValue Type;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Bitmap;
            public float ConstValue;
            public RealRgbColor ConstColor;
            public List<ShaderAnimationPropertyBlock> AnimationProperties;
            
            public enum TypeValue : short
            {
                Bitmap,
                Value,
                Color,
                Switch
            }
            
            [TagStructure(Size = 0x18)]
            public class ShaderAnimationPropertyBlock : TagStructure
            {
                public TypeValue Type;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public StringId InputName;
                public StringId RangeName;
                public float TimePeriod; // sec
                public MappingFunctionBlock Function;
                
                public enum TypeValue : short
                {
                    BitmapScaleUniform,
                    BitmapScaleX,
                    BitmapScaleY,
                    BitmapScaleZ,
                    BitmapTranslationX,
                    BitmapTranslationY,
                    BitmapTranslationZ,
                    BitmapRotationAngle,
                    BitmapRotationAxisX,
                    BitmapRotationAxisY,
                    BitmapRotationAxisZ,
                    Value,
                    Color,
                    BitmapIndex
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
        
        [TagStructure(Size = 0x7C)]
        public class ShaderPostprocessDefinitionNewBlock : TagStructure
        {
            public int ShaderTemplateIndex;
            public List<ShaderPostprocessBitmapNewBlock> Bitmaps;
            public List<Pixel32Block> PixelConstants;
            public List<RealVector4dBlock> VertexConstants;
            public List<ShaderPostprocessLevelOfDetailNewBlock> LevelsOfDetail;
            public List<TagBlockIndexBlock> Layers;
            public List<TagBlockIndexBlock1> Passes;
            public List<ShaderPostprocessImplementationNewBlock> Implementations;
            public List<ShaderPostprocessOverlayNewBlock> Overlays;
            public List<ShaderPostprocessOverlayReferenceNewBlock> OverlayReferences;
            public List<ShaderPostprocessAnimatedParameterNewBlock> AnimatedParameters;
            public List<ShaderPostprocessAnimatedParameterReferenceNewBlock> AnimatedParameterReferences;
            public List<ShaderPostprocessBitmapPropertyBlock> BitmapProperties;
            public List<ShaderPostprocessColorPropertyBlock> ColorProperties;
            public List<ShaderPostprocessValuePropertyBlock> ValueProperties;
            public List<ShaderPostprocessLevelOfDetailBlock> OldLevelsOfDetail;
            
            [TagStructure(Size = 0xC)]
            public class ShaderPostprocessBitmapNewBlock : TagStructure
            {
                public int BitmapGroup;
                public int BitmapIndex;
                public float LogBitmapDimension;
            }
            
            [TagStructure(Size = 0x4)]
            public class Pixel32Block : TagStructure
            {
                public ArgbColor Color;
            }
            
            [TagStructure(Size = 0x10)]
            public class RealVector4dBlock : TagStructure
            {
                public RealVector3d Vector3;
                public float W;
            }
            
            [TagStructure(Size = 0x6)]
            public class ShaderPostprocessLevelOfDetailNewBlock : TagStructure
            {
                public int AvailableLayerFlags;
                public TagBlockIndexStructBlock Layers;
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndexStructBlock : TagStructure
                {
                    public short BlockIndexData;
                }
            }
            
            [TagStructure(Size = 0x2)]
            public class TagBlockIndexBlock : TagStructure
            {
                public TagBlockIndexStructBlock Indices;
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndexStructBlock : TagStructure
                {
                    public short BlockIndexData;
                }
            }
            
            [TagStructure(Size = 0x2)]
            public class TagBlockIndexBlock1 : TagStructure
            {
                public TagBlockIndexStructBlock Indices;
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndexStructBlock : TagStructure
                {
                    public short BlockIndexData;
                }
            }
            
            [TagStructure(Size = 0xA)]
            public class ShaderPostprocessImplementationNewBlock : TagStructure
            {
                public TagBlockIndexStructBlock BitmapTransforms;
                public TagBlockIndexStructBlock1 RenderStates;
                public TagBlockIndexStructBlock2 TextureStates;
                public TagBlockIndexStructBlock3 PixelConstants;
                public TagBlockIndexStructBlock4 VertexConstants;
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndexStructBlock : TagStructure
                {
                    public short BlockIndexData;
                }
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndexStructBlock1 : TagStructure
                {
                    public short BlockIndexData;
                }
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndexStructBlock2 : TagStructure
                {
                    public short BlockIndexData;
                }
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndexStructBlock3 : TagStructure
                {
                    public short BlockIndexData;
                }
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndexStructBlock4 : TagStructure
                {
                    public short BlockIndexData;
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class ShaderPostprocessOverlayNewBlock : TagStructure
            {
                public StringId InputName;
                public StringId RangeName;
                public float TimePeriodInSeconds;
                public ScalarFunctionStructBlock Function;
                
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
            }
            
            [TagStructure(Size = 0x4)]
            public class ShaderPostprocessOverlayReferenceNewBlock : TagStructure
            {
                public short OverlayIndex;
                public short TransformIndex;
            }
            
            [TagStructure(Size = 0x2)]
            public class ShaderPostprocessAnimatedParameterNewBlock : TagStructure
            {
                public TagBlockIndexStructBlock OverlayReferences;
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndexStructBlock : TagStructure
                {
                    public short BlockIndexData;
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class ShaderPostprocessAnimatedParameterReferenceNewBlock : TagStructure
            {
                [TagField(Length = 0x3)]
                public byte[] Unknown;
                public sbyte ParameterIndex;
            }
            
            [TagStructure(Size = 0x4)]
            public class ShaderPostprocessBitmapPropertyBlock : TagStructure
            {
                public short BitmapIndex;
                public short AnimatedParameterIndex;
            }
            
            [TagStructure(Size = 0xC)]
            public class ShaderPostprocessColorPropertyBlock : TagStructure
            {
                public RealRgbColor Color;
            }
            
            [TagStructure(Size = 0x4)]
            public class ShaderPostprocessValuePropertyBlock : TagStructure
            {
                public float Value;
            }
            
            [TagStructure(Size = 0x98)]
            public class ShaderPostprocessLevelOfDetailBlock : TagStructure
            {
                public float ProjectedHeightPercentage;
                public int AvailableLayers;
                public List<ShaderPostprocessLayerBlock> Layers;
                public List<ShaderPostprocessPassBlock> Passes;
                public List<ShaderPostprocessImplementationBlock> Implementations;
                public List<ShaderPostprocessBitmapBlock> Bitmaps;
                public List<ShaderPostprocessBitmapTransformBlock> BitmapTransforms;
                public List<ShaderPostprocessValueBlock> Values;
                public List<ShaderPostprocessColorBlock> Colors;
                public List<ShaderPostprocessBitmapTransformOverlayBlock> BitmapTransformOverlays;
                public List<ShaderPostprocessValueOverlayBlock> ValueOverlays;
                public List<ShaderPostprocessColorOverlayBlock> ColorOverlays;
                public List<ShaderPostprocessVertexShaderConstantBlock> VertexShaderConstants;
                public ShaderGpuStateStructBlock GpuState;
                
                [TagStructure(Size = 0x2)]
                public class ShaderPostprocessLayerBlock : TagStructure
                {
                    public TagBlockIndexStructBlock Passes;
                    
                    [TagStructure(Size = 0x2)]
                    public class TagBlockIndexStructBlock : TagStructure
                    {
                        public short BlockIndexData;
                    }
                }
                
                [TagStructure(Size = 0xA)]
                public class ShaderPostprocessPassBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "spas" })]
                    public CachedTag ShaderPass;
                    public TagBlockIndexStructBlock Implementations;
                    
                    [TagStructure(Size = 0x2)]
                    public class TagBlockIndexStructBlock : TagStructure
                    {
                        public short BlockIndexData;
                    }
                }
                
                [TagStructure(Size = 0x2C)]
                public class ShaderPostprocessImplementationBlock : TagStructure
                {
                    public ShaderGpuStateReferenceStructBlock GpuConstantState;
                    public ShaderGpuStateReferenceStructBlock1 GpuVolatileState;
                    public TagBlockIndexStructBlock BitmapParameters;
                    public TagBlockIndexStructBlock1 BitmapTransforms;
                    public TagBlockIndexStructBlock2 ValueParameters;
                    public TagBlockIndexStructBlock3 ColorParameters;
                    public TagBlockIndexStructBlock4 BitmapTransformOverlays;
                    public TagBlockIndexStructBlock5 ValueOverlays;
                    public TagBlockIndexStructBlock6 ColorOverlays;
                    public TagBlockIndexStructBlock7 VertexShaderConstants;
                    
                    [TagStructure(Size = 0xE)]
                    public class ShaderGpuStateReferenceStructBlock : TagStructure
                    {
                        public TagBlockIndexStructBlock RenderStates;
                        public TagBlockIndexStructBlock1 TextureStageStates;
                        public TagBlockIndexStructBlock2 RenderStateParameters;
                        public TagBlockIndexStructBlock3 TextureStageParameters;
                        public TagBlockIndexStructBlock4 Textures;
                        public TagBlockIndexStructBlock5 VnConstants;
                        public TagBlockIndexStructBlock6 CnConstants;
                        
                        [TagStructure(Size = 0x2)]
                        public class TagBlockIndexStructBlock : TagStructure
                        {
                            public short BlockIndexData;
                        }
                        
                        [TagStructure(Size = 0x2)]
                        public class TagBlockIndexStructBlock1 : TagStructure
                        {
                            public short BlockIndexData;
                        }
                        
                        [TagStructure(Size = 0x2)]
                        public class TagBlockIndexStructBlock2 : TagStructure
                        {
                            public short BlockIndexData;
                        }
                        
                        [TagStructure(Size = 0x2)]
                        public class TagBlockIndexStructBlock3 : TagStructure
                        {
                            public short BlockIndexData;
                        }
                        
                        [TagStructure(Size = 0x2)]
                        public class TagBlockIndexStructBlock4 : TagStructure
                        {
                            public short BlockIndexData;
                        }
                        
                        [TagStructure(Size = 0x2)]
                        public class TagBlockIndexStructBlock5 : TagStructure
                        {
                            public short BlockIndexData;
                        }
                        
                        [TagStructure(Size = 0x2)]
                        public class TagBlockIndexStructBlock6 : TagStructure
                        {
                            public short BlockIndexData;
                        }
                    }
                    
                    [TagStructure(Size = 0xE)]
                    public class ShaderGpuStateReferenceStructBlock1 : TagStructure
                    {
                        public TagBlockIndexStructBlock RenderStates;
                        public TagBlockIndexStructBlock1 TextureStageStates;
                        public TagBlockIndexStructBlock2 RenderStateParameters;
                        public TagBlockIndexStructBlock3 TextureStageParameters;
                        public TagBlockIndexStructBlock4 Textures;
                        public TagBlockIndexStructBlock5 VnConstants;
                        public TagBlockIndexStructBlock6 CnConstants;
                        
                        [TagStructure(Size = 0x2)]
                        public class TagBlockIndexStructBlock : TagStructure
                        {
                            public short BlockIndexData;
                        }
                        
                        [TagStructure(Size = 0x2)]
                        public class TagBlockIndexStructBlock1 : TagStructure
                        {
                            public short BlockIndexData;
                        }
                        
                        [TagStructure(Size = 0x2)]
                        public class TagBlockIndexStructBlock2 : TagStructure
                        {
                            public short BlockIndexData;
                        }
                        
                        [TagStructure(Size = 0x2)]
                        public class TagBlockIndexStructBlock3 : TagStructure
                        {
                            public short BlockIndexData;
                        }
                        
                        [TagStructure(Size = 0x2)]
                        public class TagBlockIndexStructBlock4 : TagStructure
                        {
                            public short BlockIndexData;
                        }
                        
                        [TagStructure(Size = 0x2)]
                        public class TagBlockIndexStructBlock5 : TagStructure
                        {
                            public short BlockIndexData;
                        }
                        
                        [TagStructure(Size = 0x2)]
                        public class TagBlockIndexStructBlock6 : TagStructure
                        {
                            public short BlockIndexData;
                        }
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class TagBlockIndexStructBlock : TagStructure
                    {
                        public short BlockIndexData;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class TagBlockIndexStructBlock1 : TagStructure
                    {
                        public short BlockIndexData;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class TagBlockIndexStructBlock2 : TagStructure
                    {
                        public short BlockIndexData;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class TagBlockIndexStructBlock3 : TagStructure
                    {
                        public short BlockIndexData;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class TagBlockIndexStructBlock4 : TagStructure
                    {
                        public short BlockIndexData;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class TagBlockIndexStructBlock5 : TagStructure
                    {
                        public short BlockIndexData;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class TagBlockIndexStructBlock6 : TagStructure
                    {
                        public short BlockIndexData;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class TagBlockIndexStructBlock7 : TagStructure
                    {
                        public short BlockIndexData;
                    }
                }
                
                [TagStructure(Size = 0xA)]
                public class ShaderPostprocessBitmapBlock : TagStructure
                {
                    public sbyte ParameterIndex;
                    public sbyte Flags;
                    public int BitmapGroupIndex;
                    public float LogBitmapDimension;
                }
                
                [TagStructure(Size = 0x6)]
                public class ShaderPostprocessBitmapTransformBlock : TagStructure
                {
                    public sbyte ParameterIndex;
                    public sbyte BitmapTransformIndex;
                    public float Value;
                }
                
                [TagStructure(Size = 0x5)]
                public class ShaderPostprocessValueBlock : TagStructure
                {
                    public sbyte ParameterIndex;
                    public float Value;
                }
                
                [TagStructure(Size = 0xD)]
                public class ShaderPostprocessColorBlock : TagStructure
                {
                    public sbyte ParameterIndex;
                    public RealRgbColor Color;
                }
                
                [TagStructure(Size = 0x17)]
                public class ShaderPostprocessBitmapTransformOverlayBlock : TagStructure
                {
                    public sbyte ParameterIndex;
                    public sbyte TransformIndex;
                    public sbyte AnimationPropertyType;
                    public StringId InputName;
                    public StringId RangeName;
                    public float TimePeriodInSeconds;
                    public ScalarFunctionStructBlock Function;
                    
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
                }
                
                [TagStructure(Size = 0x15)]
                public class ShaderPostprocessValueOverlayBlock : TagStructure
                {
                    public sbyte ParameterIndex;
                    public StringId InputName;
                    public StringId RangeName;
                    public float TimePeriodInSeconds;
                    public ScalarFunctionStructBlock Function;
                    
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
                }
                
                [TagStructure(Size = 0x15)]
                public class ShaderPostprocessColorOverlayBlock : TagStructure
                {
                    public sbyte ParameterIndex;
                    public StringId InputName;
                    public StringId RangeName;
                    public float TimePeriodInSeconds;
                    public ColorFunctionStructBlock Function;
                    
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
                }
                
                [TagStructure(Size = 0x12)]
                public class ShaderPostprocessVertexShaderConstantBlock : TagStructure
                {
                    public sbyte RegisterIndex;
                    public sbyte RegisterBank;
                    public float Data;
                    public float Data1;
                    public float Data2;
                    public float Data3;
                }
                
                [TagStructure(Size = 0x38)]
                public class ShaderGpuStateStructBlock : TagStructure
                {
                    public List<RenderStateBlock> RenderStates;
                    public List<TextureStageStateBlock> TextureStageStates;
                    public List<RenderStateParameterBlock> RenderStateParameters;
                    public List<TextureStageStateParameterBlock> TextureStageParameters;
                    public List<TextureBlock> Textures;
                    public List<VertexShaderConstantBlock> VnConstants;
                    public List<VertexShaderConstantBlock1> CnConstants;
                    
                    [TagStructure(Size = 0x5)]
                    public class RenderStateBlock : TagStructure
                    {
                        public sbyte StateIndex;
                        public int StateValue;
                    }
                    
                    [TagStructure(Size = 0x6)]
                    public class TextureStageStateBlock : TagStructure
                    {
                        public sbyte StateIndex;
                        public sbyte StageIndex;
                        public int StateValue;
                    }
                    
                    [TagStructure(Size = 0x3)]
                    public class RenderStateParameterBlock : TagStructure
                    {
                        public sbyte ParameterIndex;
                        public sbyte ParameterType;
                        public sbyte StateIndex;
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class TextureStageStateParameterBlock : TagStructure
                    {
                        public sbyte ParameterIndex;
                        public sbyte ParameterType;
                        public sbyte StateIndex;
                        public sbyte StageIndex;
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class TextureBlock : TagStructure
                    {
                        public sbyte StageIndex;
                        public sbyte ParameterIndex;
                        public sbyte GlobalTextureIndex;
                        public sbyte Flags;
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class VertexShaderConstantBlock : TagStructure
                    {
                        public sbyte RegisterIndex;
                        public sbyte ParameterIndex;
                        public sbyte DestinationMask;
                        public sbyte ScaleByTextureStage;
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class VertexShaderConstantBlock1 : TagStructure
                    {
                        public sbyte RegisterIndex;
                        public sbyte ParameterIndex;
                        public sbyte DestinationMask;
                        public sbyte ScaleByTextureStage;
                    }
                }
            }
        }
    }
}

