using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "particle", Tag = "prt3", Size = 0xF8)]
    public class Particle : TagStructure
    {
        public FlagsValue Flags;
        public ParticleBillboardStyleValue ParticleBillboardStyle;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        public short FirstSequenceIndex;
        public short SequenceCount;
        /// <summary>
        /// Shader Parameters
        /// </summary>
        public CachedTag ShaderTemplate;
        public List<ShaderParameter> ShaderParameters;
        /// <summary>
        /// Color
        /// </summary>
        /// <remarks>
        /// controls how the color of the particle changes as
        /// a function of its input
        /// </remarks>
        public ParticleProperty Color;
        /// <summary>
        /// Alpha
        /// </summary>
        /// <remarks>
        /// seperate from color, controls how the particle fades
        /// as a function of its input
        /// </remarks>
        public ParticleProperty Alpha;
        /// <summary>
        /// Scale
        /// </summary>
        /// <remarks>
        /// controls how the size of a particle changes as
        /// a function of its input
        /// </remarks>
        public ParticleProperty Scale;
        /// <summary>
        /// Rotation
        /// </summary>
        /// <remarks>
        /// controls how the particle rotates. 0= 0 degrees, .5= 180 degrees, 1.0= 360 degrees
        /// </remarks>
        public ParticleProperty Rotation;
        /// <summary>
        /// Frame index
        /// </summary>
        /// <remarks>
        /// provides finer grain control over how the animation
        /// happens.  a output of 0.25 means that when that input
        /// is fed in, the particle will be 25% of the way through
        /// its animation.
        /// </remarks>
        public ParticleProperty FrameIndex;
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
        public List<ShaderPostprocessDefinitionNew> Unknown3;
        [TagField(Flags = Padding, Length = 8)]
        public byte[] Padding2;
        [TagField(Flags = Padding, Length = 16)]
        public byte[] Padding3;
        [TagField(Flags = Padding, Length = 16)]
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
        
        public enum ParticleBillboardStyleValue : short
        {
            ScreenFacing,
            ParallelToDirection,
            PerpendicularToDirection,
            Vertical,
            Horizontal
        }
        
        [TagStructure(Size = 0x34)]
        public class ShaderParameter : TagStructure
        {
            public StringId Name;
            public TypeValue Type;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public CachedTag Bitmap;
            public float ConstValue;
            public RealRgbColor ConstColor;
            public List<ShaderAnimationProperty> AnimationProperties;
            
            public enum TypeValue : short
            {
                Bitmap,
                Value,
                Color,
                Switch
            }
            
            [TagStructure(Size = 0x1C)]
            public class ShaderAnimationProperty : TagStructure
            {
                public TypeValue Type;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public StringId InputName;
                public StringId RangeName;
                public float TimePeriod; // sec
                /// <summary>
                /// FUNCTION
                /// </summary>
                public FunctionDefinition Function;
                
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
        
        [TagStructure(Size = 0xB8)]
        public class ShaderPostprocessDefinitionNew : TagStructure
        {
            public int ShaderTemplateIndex;
            public List<ShaderPostprocessBitmapNew> Bitmaps;
            public List<Pixel32> PixelConstants;
            public List<RealVector4d> VertexConstants;
            public List<ShaderPostprocessLevelOfDetailNew> LevelsOfDetail;
            public List<TagBlockIndex> Layers;
            public List<TagBlockIndex> Passes;
            public List<ShaderPostprocessImplementationNew> Implementations;
            public List<ShaderPostprocessOverlayNew> Overlays;
            public List<ShaderPostprocessOverlayReference> OverlayReferences;
            public List<ShaderPostprocessAnimatedParameter> AnimatedParameters;
            public List<ShaderPostprocessAnimatedParameterReference> AnimatedParameterReferences;
            public List<ShaderPostprocessBitmapProperty> BitmapProperties;
            public List<ShaderPostprocessColorProperty> ColorProperties;
            public List<ShaderPostprocessValueProperty> ValueProperties;
            public List<ShaderPostprocessLevelOfDetail> OldLevelsOfDetail;
            
            [TagStructure(Size = 0xC)]
            public class ShaderPostprocessBitmapNew : TagStructure
            {
                public int BitmapGroup;
                public int BitmapIndex;
                public float LogBitmapDimension;
            }
            
            [TagStructure(Size = 0x4)]
            public class Pixel32 : TagStructure
            {
                public ArgbColor Color;
            }
            
            [TagStructure(Size = 0x10)]
            public class RealVector4d : TagStructure
            {
                public RealVector3d Vector3;
                public float W;
            }
            
            [TagStructure(Size = 0x6)]
            public class ShaderPostprocessLevelOfDetailNew : TagStructure
            {
                public int AvailableLayerFlags;
                public TagBlockIndex Layers;
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndex : TagStructure
                {
                    public short BlockIndexData;
                }
            }
            
            [TagStructure(Size = 0x2)]
            public class TagBlockIndex : TagStructure
            {
                public TagBlockIndex Indices;
            }
            
            [TagStructure(Size = 0xA)]
            public class ShaderPostprocessImplementationNew : TagStructure
            {
                public TagBlockIndex BitmapTransforms;
                public TagBlockIndex RenderStates;
                public TagBlockIndex TextureStates;
                public TagBlockIndex PixelConstants;
                public TagBlockIndex VertexConstants;
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndex : TagStructure
                {
                    public short BlockIndexData;
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class ShaderPostprocessOverlayNew : TagStructure
            {
                public StringId InputName;
                public StringId RangeName;
                public float TimePeriodInSeconds;
                public FunctionDefinition Function;
                
                [TagStructure(Size = 0xC)]
                public class FunctionDefinition : TagStructure
                {
                    public FunctionDefinition Function;
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class ShaderPostprocessOverlayReference : TagStructure
            {
                public short OverlayIndex;
                public short TransformIndex;
            }
            
            [TagStructure(Size = 0x2)]
            public class ShaderPostprocessAnimatedParameter : TagStructure
            {
                public TagBlockIndex OverlayReferences;
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndex : TagStructure
                {
                    public short BlockIndexData;
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class ShaderPostprocessAnimatedParameterReference : TagStructure
            {
                [TagField(Flags = Padding, Length = 3)]
                public byte[] Unknown1;
                public sbyte ParameterIndex;
            }
            
            [TagStructure(Size = 0x4)]
            public class ShaderPostprocessBitmapProperty : TagStructure
            {
                public short BitmapIndex;
                public short AnimatedParameterIndex;
            }
            
            [TagStructure(Size = 0xC)]
            public class ShaderPostprocessColorProperty : TagStructure
            {
                public RealRgbColor Color;
            }
            
            [TagStructure(Size = 0x4)]
            public class ShaderPostprocessValueProperty : TagStructure
            {
                public float Value;
            }
            
            [TagStructure(Size = 0xE0)]
            public class ShaderPostprocessLevelOfDetail : TagStructure
            {
                public float ProjectedHeightPercentage;
                public int AvailableLayers;
                public List<ShaderPostprocessLayer> Layers;
                public List<ShaderPostprocessPass> Passes;
                public List<ShaderPostprocessImplementation> Implementations;
                public List<ShaderPostprocessBitmap> Bitmaps;
                public List<ShaderPostprocessBitmapTransform> BitmapTransforms;
                public List<ShaderPostprocessValue> Values;
                public List<ShaderPostprocessColor> Colors;
                public List<ShaderPostprocessBitmapTransformOverlay> BitmapTransformOverlays;
                public List<ShaderPostprocessValueOverlay> ValueOverlays;
                public List<ShaderPostprocessColorOverlay> ColorOverlays;
                public List<ShaderVertexShaderConstant> VertexShaderConstants;
                public ShaderGpuState GpuState;
                
                [TagStructure(Size = 0x2)]
                public class ShaderPostprocessLayer : TagStructure
                {
                    public TagBlockIndex Passes;
                    
                    [TagStructure(Size = 0x2)]
                    public class TagBlockIndex : TagStructure
                    {
                        public short BlockIndexData;
                    }
                }
                
                [TagStructure(Size = 0x12)]
                public class ShaderPostprocessPass : TagStructure
                {
                    public CachedTag ShaderPass;
                    public TagBlockIndex Implementations;
                    
                    [TagStructure(Size = 0x2)]
                    public class TagBlockIndex : TagStructure
                    {
                        public short BlockIndexData;
                    }
                }
                
                [TagStructure(Size = 0x2C)]
                public class ShaderPostprocessImplementation : TagStructure
                {
                    public ShaderGpuStateReference GpuConstantState;
                    public ShaderGpuStateReference GpuVolatileState;
                    public TagBlockIndex BitmapParameters;
                    public TagBlockIndex BitmapTransforms;
                    public TagBlockIndex ValueParameters;
                    public TagBlockIndex ColorParameters;
                    public TagBlockIndex BitmapTransformOverlays;
                    public TagBlockIndex ValueOverlays;
                    public TagBlockIndex ColorOverlays;
                    public TagBlockIndex VertexShaderConstants;
                    
                    [TagStructure(Size = 0xE)]
                    public class ShaderGpuStateReference : TagStructure
                    {
                        public TagBlockIndex RenderStates;
                        public TagBlockIndex TextureStageStates;
                        public TagBlockIndex RenderStateParameters;
                        public TagBlockIndex TextureStageParameters;
                        public TagBlockIndex Textures;
                        public TagBlockIndex VnConstants;
                        public TagBlockIndex CnConstants;
                        
                        [TagStructure(Size = 0x2)]
                        public class TagBlockIndex : TagStructure
                        {
                            public short BlockIndexData;
                        }
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class TagBlockIndex : TagStructure
                    {
                        public short BlockIndexData;
                    }
                }
                
                [TagStructure(Size = 0xA)]
                public class ShaderPostprocessBitmap : TagStructure
                {
                    public sbyte ParameterIndex;
                    public sbyte Flags;
                    public int BitmapGroupIndex;
                    public float LogBitmapDimension;
                }
                
                [TagStructure(Size = 0x6)]
                public class ShaderPostprocessBitmapTransform : TagStructure
                {
                    public sbyte ParameterIndex;
                    public sbyte BitmapTransformIndex;
                    public float Value;
                }
                
                [TagStructure(Size = 0x5)]
                public class ShaderPostprocessValue : TagStructure
                {
                    public sbyte ParameterIndex;
                    public float Value;
                }
                
                [TagStructure(Size = 0xD)]
                public class ShaderPostprocessColor : TagStructure
                {
                    public sbyte ParameterIndex;
                    public RealRgbColor Color;
                }
                
                [TagStructure(Size = 0x1B)]
                public class ShaderPostprocessBitmapTransformOverlay : TagStructure
                {
                    public sbyte ParameterIndex;
                    public sbyte TransformIndex;
                    public sbyte AnimationPropertyType;
                    public StringId InputName;
                    public StringId RangeName;
                    public float TimePeriodInSeconds;
                    public FunctionDefinition Function;
                    
                    [TagStructure(Size = 0xC)]
                    public class FunctionDefinition : TagStructure
                    {
                        public FunctionDefinition Function;
                    }
                }
                
                [TagStructure(Size = 0x19)]
                public class ShaderPostprocessValueOverlay : TagStructure
                {
                    public sbyte ParameterIndex;
                    public StringId InputName;
                    public StringId RangeName;
                    public float TimePeriodInSeconds;
                    public FunctionDefinition Function;
                    
                    [TagStructure(Size = 0xC)]
                    public class FunctionDefinition : TagStructure
                    {
                        public FunctionDefinition Function;
                    }
                }
                
                [TagStructure(Size = 0x19)]
                public class ShaderPostprocessColorOverlay : TagStructure
                {
                    public sbyte ParameterIndex;
                    public StringId InputName;
                    public StringId RangeName;
                    public float TimePeriodInSeconds;
                    public FunctionDefinition Function;
                    
                    [TagStructure(Size = 0xC)]
                    public class FunctionDefinition : TagStructure
                    {
                        public FunctionDefinition Function;
                    }
                }
                
                [TagStructure(Size = 0x12)]
                public class ShaderVertexShaderConstant : TagStructure
                {
                    public sbyte RegisterIndex;
                    public sbyte RegisterBank;
                    public float Data;
                    public float Data1;
                    public float Data2;
                    public float Data3;
                }
                
                [TagStructure(Size = 0x54)]
                public class ShaderGpuState : TagStructure
                {
                    public List<RenderState> RenderStates;
                    public List<TextureStageState> TextureStageStates;
                    public List<RenderStateParameter> RenderStateParameters;
                    public List<TextureStageStateParameter> TextureStageParameters;
                    public List<Texture> Textures;
                    public List<VertexShaderConstant> VnConstants;
                    public List<VertexShaderConstant> CnConstants;
                    
                    [TagStructure(Size = 0x5)]
                    public class RenderState : TagStructure
                    {
                        public sbyte StateIndex;
                        public int StateValue;
                    }
                    
                    [TagStructure(Size = 0x6)]
                    public class TextureStageState : TagStructure
                    {
                        public sbyte StateIndex;
                        public sbyte StageIndex;
                        public int StateValue;
                    }
                    
                    [TagStructure(Size = 0x3)]
                    public class RenderStateParameter : TagStructure
                    {
                        public sbyte ParameterIndex;
                        public sbyte ParameterType;
                        public sbyte StateIndex;
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class TextureStageStateParameter : TagStructure
                    {
                        public sbyte ParameterIndex;
                        public sbyte ParameterType;
                        public sbyte StateIndex;
                        public sbyte StageIndex;
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class Texture : TagStructure
                    {
                        public sbyte StageIndex;
                        public sbyte ParameterIndex;
                        public sbyte GlobalTextureIndex;
                        public sbyte Flags;
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class VertexShaderConstant : TagStructure
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

