using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "particle", Tag = "prt3", Size = 0x238)]
    public class Particle : TagStructure
    {
        public ParticleMainFlags MainFlags;
        public List<AttachmentBlock> Attachments;
        public ParticleAppearanceFlags AppearanceFlags;
        public ParticleBillboardTypeEnum ParticleBillboardStyle;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public short FirstSequenceIndex;
        public short SequenceCount;
        // the distance at which we switch from low res (close) to high res (far)
        public float LowResSwitchDistance; // wu
        public RealPoint2d CenterOffset;
        public float Curvature; // 0=flat, 1=hemisphere
        // degrees beyond cutoff over which particles fade
        public float AngleFadeRange; // degrees
        // degrees away from edge-on where fade is total
        public float AngleFadeCutoff; // degrees
        // affects billboard tilt from observer motion
        public float MotionBlurTranslationScale;
        // affects billboard tilt from observer turning
        public float MotionBlurRotationScale;
        // affects aspect ratio stretching from particle and observer motion
        public float MotionBlurAspectScale;
        public MaterialStruct ActualMaterial;
        public ParticlePropertyScalarStructNew AspectRatio;
        // controls how the color of the particle changes as a function of its input
        public ParticlePropertyColorStructNew Color; // RGB
        // multiplies color to give dynamic range outside [0,1]
        public ParticlePropertyScalarStructNew Intensity;
        // separate from color, controls how the particle fades as a function of its input
        public ParticlePropertyScalarStructNew Alpha;
        // switches between modulate (multiply) and tint(preserve whites)
        [TagField(Length = 32)]
        public string TintFactor; // 0=modulate, 1=tint
        public ParticleAnimationFlags AnimationFlags;
        // 0=first frame, 1=last frame
        public ParticlePropertyScalarStructNew FrameIndex;
        public ParticlePropertyScalarStructNew AnimationRate; // index cycles per second
        public ParticlePropertyScalarStructNew PaletteAnimation; // v coord of palette
        [TagField(ValidTags = new [] { "pmdf" })]
        public CachedTag Model;
        public ShaderParticleStruct ActualShader;
        public uint RuntimeMUsedParticleStates;
        public uint RuntimeMConstantPerParticleProperties;
        [TagField(Length = 32)]
        public string RuntimeMConstantOverTimeProperties;
        public GpuDataStruct RuntimeMGpuData;
        
        [Flags]
        public enum ParticleMainFlags : uint
        {
            DiesAtRest = 1 << 0,
            DiesOnStructureCollision = 1 << 1,
            DiesInWater = 1 << 2,
            DiesInAir = 1 << 3,
            HasSweetener = 1 << 4,
            UsesCheapShader = 1 << 5
        }
        
        [Flags]
        public enum ParticleAppearanceFlags : uint
        {
            RandomUMirror = 1 << 0,
            RandomVMirror = 1 << 1,
            RandomStartingRotation = 1 << 2,
            TintFromLightmap = 1 << 3,
            TintFromDiffuseTexture = 1 << 4,
            BitmapAuthoredVertically = 1 << 5,
            IntensityAffectsAlpha = 1 << 6,
            FadeWhenViewedEdgeOn = 1 << 7,
            MotionBlur = 1 << 8,
            DoubleSided = 1 << 9,
            // renders heavy overdraw particles faster
            LowRes = 1 << 10,
            // requires depth fade
            LowResTighterMask = 1 << 11,
            NeverKillVertsOnGpu = 1 << 12,
            // makes parallel and perpindicular to velocity behave differently based upon camera motion
            ParticleVelocityRelativeToCamera = 1 << 13
        }
        
        public enum ParticleBillboardTypeEnum : short
        {
            ScreenFacing,
            CameraFacing,
            ParallelToDirection,
            PerpendicularToDirection,
            Vertical,
            Horizontal,
            LocalVertical,
            LocalHorizontal,
            World,
            VelocityHorizontal
        }
        
        [Flags]
        public enum ParticleAnimationFlags : uint
        {
            FrameAnimationOneShot = 1 << 0,
            CanAnimateBackwards = 1 << 1
        }
        
        [TagStructure(Size = 0x18)]
        public class AttachmentBlock : TagStructure
        {
            public AttachmentFlags Flags;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "effe","sndo","foot" })]
            public CachedTag Type;
            public AttachmentTypeEnum Trigger;
            // 0 will always play, 127 will be extremely rare
            public byte SkipFraction; // [0-127]
            public GameStateTypeEnum PrimaryScale;
            public GameStateTypeEnum SecondaryScale;
            
            [Flags]
            public enum AttachmentFlags : byte
            {
                DisabledForDebugging = 1 << 0
            }
            
            public enum AttachmentTypeEnum : sbyte
            {
                Birth,
                Collision,
                Death,
                FirstCollision
            }
            
            public enum GameStateTypeEnum : sbyte
            {
                ParticleAge,
                SystemAge,
                ParticleRandom,
                SystemRandom,
                ParticleCorrelation1,
                ParticleCorrelation2,
                ParticleCorrelation3,
                ParticleCorrelation4,
                SystemCorrelation1,
                SystemCorrelation2,
                ParticleEmissionTime,
                LocationLod,
                GameTime,
                EffectAScale,
                EffectBScale,
                ParticleRotation,
                LocationRandom,
                DistanceFromEmitter,
                ParticleSimulationA,
                ParticleSimulationB,
                ParticleVelocity,
                InvalidStatePleaseSetAgain
            }
        }
        
        [TagStructure(Size = 0x44)]
        public class MaterialStruct : TagStructure
        {
            [TagField(ValidTags = new [] { "mats" })]
            public CachedTag MaterialShader;
            public List<MaterialShaderParameterBlock> MaterialParameters;
            public List<MaterialPostprocessBlock> PostprocessDefinition;
            public StringId PhysicsMaterialName;
            public StringId PhysicsMaterialName2;
            public StringId PhysicsMaterialName3;
            public StringId PhysicsMaterialName4;
            public float SortOffset;
            public AlphaBlendModeEnum AlphaBlendMode;
            public GlobalSortLayerEnumDefintion SortLayer;
            public Materialflags Flags;
            public MaterialrenderFlags RenderFlags;
            public MaterialTransparentShadowPolicyEnum TransparentShadowPolicy;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum AlphaBlendModeEnum : sbyte
            {
                Opaque,
                Additive,
                Multiply,
                AlphaBlend,
                DoubleMultiply,
                PreMultipliedAlpha,
                Maximum,
                MultiplyAdd,
                AddSrcTimesDstalpha,
                AddSrcTimesSrcalpha,
                InvAlphaBlend,
                MotionBlurStatic,
                MotionBlurInhibit,
                ApplyShadowIntoShadowMask,
                AlphaBlendConstant,
                OverdrawApply,
                WetScreenEffect,
                Minimum,
                Revsubtract,
                ForgeLightmap,
                ForgeLightmapInv,
                ReplaceAllChannels,
                AlphaBlendMax,
                OpaqueAlphaBlend,
                AlphaBlendAdditiveTransparent
            }
            
            public enum GlobalSortLayerEnumDefintion : sbyte
            {
                Invalid,
                PrePass,
                Normal,
                PostPass
            }
            
            [Flags]
            public enum Materialflags : byte
            {
                ConvertedFromShader = 1 << 0,
                DecalPostLighting = 1 << 1
            }
            
            [Flags]
            public enum MaterialrenderFlags : byte
            {
                ResolveScreenBeforeRendering = 1 << 0
            }
            
            public enum MaterialTransparentShadowPolicyEnum : sbyte
            {
                None,
                RenderAsDecal,
                RenderWithMaterial
            }
            
            [TagStructure(Size = 0xA8)]
            public class MaterialShaderParameterBlock : TagStructure
            {
                public StringId ParameterName;
                public MaterialShaderParameterTypeEnum ParameterType;
                public int ParameterIndex;
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag Bitmap;
                public StringId BitmapPath;
                public RealArgbColor Color;
                public float Real;
                public RealVector3d Vector;
                public int IntBool;
                public ushort BitmapFlags;
                public ushort BitmapFilterMode;
                public ushort BitmapAddressMode;
                public ushort BitmapAddressModeX;
                public ushort BitmapAddressModeY;
                public ushort BitmapSharpenMode;
                public byte BitmapExternMode;
                public byte BitmapMinMipmap;
                public byte BitmapMaxMipmap;
                public byte RenderPhasesUsed;
                public List<MaterialShaderFunctionParameterBlock> FunctionParameters;
                public byte[] DisplayName;
                public byte[] DisplayGroup;
                public byte[] DisplayHelpText;
                public float DisplayMinimum;
                public float DisplayMaximum;
                public byte RegisterIndex;
                public byte RegisterOffset;
                public byte RegisterCount;
                public RegisterSetEnum RegisterSet;
                
                public enum MaterialShaderParameterTypeEnum : int
                {
                    Bitmap,
                    Real,
                    Int,
                    Bool,
                    Color
                }
                
                public enum RegisterSetEnum : sbyte
                {
                    Bool,
                    Int,
                    Float,
                    Sampler,
                    VertexBool,
                    VertexInt,
                    VertexFloat,
                    VertexSampler
                }
                
                [TagStructure(Size = 0x2C)]
                public class MaterialShaderFunctionParameterBlock : TagStructure
                {
                    public MaterialAnimatedParameterTypeEnum Type;
                    public StringId InputName;
                    public StringId RangeName;
                    public MaterialfunctionOutputModEnum OutputModifier;
                    [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public StringId OutputModifierInput;
                    public float TimePeriod; // seconds
                    public MappingFunction Function;
                    
                    public enum MaterialAnimatedParameterTypeEnum : int
                    {
                        Value,
                        Color,
                        ScaleUniform,
                        ScaleU,
                        ScaleV,
                        OffsetU,
                        OffsetV,
                        FrameIndex,
                        Alpha
                    }
                    
                    public enum MaterialfunctionOutputModEnum : sbyte
                    {
                        Unknown,
                        Add,
                        Multiply
                    }
                    
                    [TagStructure(Size = 0x14)]
                    public class MappingFunction : TagStructure
                    {
                        public byte[] Data;
                    }
                }
            }
            
            [TagStructure(Size = 0x9C)]
            public class MaterialPostprocessBlock : TagStructure
            {
                public List<MaterialPostprocessTextureBlock> Textures;
                public List<RealVector4dBlock> TextureConstants;
                public List<RealVector4dBlock> RealVectors;
                public List<RealVector4dBlock> RealVertexVectors;
                public List<IntBlock> IntConstants;
                public int BoolConstants;
                public int UsedBoolConstants;
                public int BoolRenderPhaseMask;
                public int VertexBoolConstants;
                public int UsedVertexBoolConstants;
                public int VertexBoolRenderPhaseMask;
                public List<MaterialShaderFunctionParameterBlock> Functions;
                public List<FunctionparameterBlock> FunctionParameters;
                public List<ExternparameterBlock> ExternParameters;
                public AlphaBlendModeEnum AlphaBlendMode;
                public LayerblendModeEnum LayerBlendMode;
                public MaterialpostprocessFlags Flags;
                [TagField(Length = 12)]
                public RuntimeQueryableProperties[]  RuntimeQueryablePropertiesTable;
                public MaterialTypeStruct PhysicsMaterial0;
                public MaterialTypeStruct PhysicsMaterial1;
                public MaterialTypeStruct PhysicsMaterial2;
                public MaterialTypeStruct PhysicsMaterial3;
                
                public enum AlphaBlendModeEnum : sbyte
                {
                    Opaque,
                    Additive,
                    Multiply,
                    AlphaBlend,
                    DoubleMultiply,
                    PreMultipliedAlpha,
                    Maximum,
                    MultiplyAdd,
                    AddSrcTimesDstalpha,
                    AddSrcTimesSrcalpha,
                    InvAlphaBlend,
                    MotionBlurStatic,
                    MotionBlurInhibit,
                    ApplyShadowIntoShadowMask,
                    AlphaBlendConstant,
                    OverdrawApply,
                    WetScreenEffect,
                    Minimum,
                    Revsubtract,
                    ForgeLightmap,
                    ForgeLightmapInv,
                    ReplaceAllChannels,
                    AlphaBlendMax,
                    OpaqueAlphaBlend,
                    AlphaBlendAdditiveTransparent
                }
                
                public enum LayerblendModeEnum : sbyte
                {
                    None,
                    Blended,
                    Layered
                }
                
                [Flags]
                public enum MaterialpostprocessFlags : ushort
                {
                    WireframeOutline = 1 << 0,
                    ForceSinglePass = 1 << 1,
                    HasPixelConstantFunctions = 1 << 2,
                    HasVertexConstantFunctions = 1 << 3,
                    HasTextureTransformFunctions = 1 << 4,
                    HasTextureFrameFunctions = 1 << 5,
                    ResolveScreenBeforeRendering = 1 << 6,
                    DisableAtmosphereFog = 1 << 7,
                    UsesDepthCamera = 1 << 8,
                    MaterialIsVariable = 1 << 9
                }
                
                [TagStructure(Size = 0x18)]
                public class MaterialPostprocessTextureBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "bitm" })]
                    public CachedTag BitmapReference;
                    public byte AddressMode;
                    public byte FilterMode;
                    public byte FrameIndexParameter;
                    public byte SamplerIndex;
                    public sbyte LevelOfSmallestMipmapToUse;
                    public sbyte LevelOfLargestMipmapToUse;
                    public byte RenderPhaseMask;
                    [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                }
                
                [TagStructure(Size = 0x10)]
                public class RealVector4dBlock : TagStructure
                {
                    public RealVector3d Vector;
                    public float VectorW;
                }
                
                [TagStructure(Size = 0x4)]
                public class IntBlock : TagStructure
                {
                    public int IntValue;
                }
                
                [TagStructure(Size = 0x2C)]
                public class MaterialShaderFunctionParameterBlock : TagStructure
                {
                    public MaterialAnimatedParameterTypeEnum Type;
                    public StringId InputName;
                    public StringId RangeName;
                    public MaterialfunctionOutputModEnum OutputModifier;
                    [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public StringId OutputModifierInput;
                    public float TimePeriod; // seconds
                    public MappingFunction Function;
                    
                    public enum MaterialAnimatedParameterTypeEnum : int
                    {
                        Value,
                        Color,
                        ScaleUniform,
                        ScaleU,
                        ScaleV,
                        OffsetU,
                        OffsetV,
                        FrameIndex,
                        Alpha
                    }
                    
                    public enum MaterialfunctionOutputModEnum : sbyte
                    {
                        Unknown,
                        Add,
                        Multiply
                    }
                    
                    [TagStructure(Size = 0x14)]
                    public class MappingFunction : TagStructure
                    {
                        public byte[] Data;
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class FunctionparameterBlock : TagStructure
                {
                    public byte FunctionIndex;
                    public byte RenderPhaseMask;
                    public byte RegisterIndex;
                    public byte RegisterOffset;
                }
                
                [TagStructure(Size = 0x4)]
                public class ExternparameterBlock : TagStructure
                {
                    public byte ExternIndex;
                    public byte ExternRegister;
                    public byte ExternOffset;
                    public byte RenderPhaseMask;
                }
                
                [TagStructure(Size = 0x2)]
                public class RuntimeQueryableProperties : TagStructure
                {
                    public short Index;
                }
                
                [TagStructure(Size = 0x2)]
                public class MaterialTypeStruct : TagStructure
                {
                    public short GlobalMaterialIndex;
                }
            }
        }
        
        [TagStructure(Size = 0x20)]
        public class ParticlePropertyScalarStructNew : TagStructure
        {
            public GameStateTypeEnum InputVariable;
            public GameStateTypeEnum RangeVariable;
            public OutputModEnum OutputModifier;
            public GameStateTypeEnum OutputModifierInput;
            public MappingFunction Mapping;
            public float RuntimeMConstantValue;
            public ushort RuntimeMFlags;
            public ForceFlags ForceFlags1;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum GameStateTypeEnum : sbyte
            {
                ParticleAge,
                SystemAge,
                ParticleRandom,
                SystemRandom,
                ParticleCorrelation1,
                ParticleCorrelation2,
                ParticleCorrelation3,
                ParticleCorrelation4,
                SystemCorrelation1,
                SystemCorrelation2,
                ParticleEmissionTime,
                LocationLod,
                GameTime,
                EffectAScale,
                EffectBScale,
                ParticleRotation,
                LocationRandom,
                DistanceFromEmitter,
                ParticleSimulationA,
                ParticleSimulationB,
                ParticleVelocity,
                InvalidStatePleaseSetAgain
            }
            
            public enum OutputModEnum : sbyte
            {
                Unknown,
                Plus,
                Times
            }
            
            [Flags]
            public enum ForceFlags : byte
            {
                ForceConstant = 1 << 0
            }
            
            [TagStructure(Size = 0x14)]
            public class MappingFunction : TagStructure
            {
                public byte[] Data;
            }
        }
        
        [TagStructure(Size = 0x20)]
        public class ParticlePropertyColorStructNew : TagStructure
        {
            public GameStateTypeEnum InputVariable;
            public GameStateTypeEnum RangeVariable;
            public OutputModEnum OutputModifier;
            public GameStateTypeEnum OutputModifierInput;
            public MappingFunction Mapping;
            public float RuntimeMConstantValue;
            public ushort RuntimeMFlags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum GameStateTypeEnum : sbyte
            {
                ParticleAge,
                SystemAge,
                ParticleRandom,
                SystemRandom,
                ParticleCorrelation1,
                ParticleCorrelation2,
                ParticleCorrelation3,
                ParticleCorrelation4,
                SystemCorrelation1,
                SystemCorrelation2,
                ParticleEmissionTime,
                LocationLod,
                GameTime,
                EffectAScale,
                EffectBScale,
                ParticleRotation,
                LocationRandom,
                DistanceFromEmitter,
                ParticleSimulationA,
                ParticleSimulationB,
                ParticleVelocity,
                InvalidStatePleaseSetAgain
            }
            
            public enum OutputModEnum : sbyte
            {
                Unknown,
                Plus,
                Times
            }
            
            [TagStructure(Size = 0x14)]
            public class MappingFunction : TagStructure
            {
                public byte[] Data;
            }
        }
        
        [TagStructure(Size = 0x34)]
        public class ShaderParticleStruct : TagStructure
        {
            public RealRgbColor BrightTint;
            public RealRgbColor AmbientTint;
            public float Contrast;
            public float BlurWeight;
            public float IntensityScale;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Palette;
        }
        
        [TagStructure(Size = 0x18)]
        public class GpuDataStruct : TagStructure
        {
            public List<GpuSpriteBlock> RuntimeMSprite;
            public List<GpuVariantsBlock> RuntimeMFrames;
            
            [TagStructure(Size = 0x10)]
            public class GpuSpriteBlock : TagStructure
            {
                [TagField(Length = 4)]
                public GpuSingleConstantRegisterArray[]  RuntimeGpuSpriteArray;
                
                [TagStructure(Size = 0x4)]
                public class GpuSingleConstantRegisterArray : TagStructure
                {
                    public float RuntimeGpuReal;
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class GpuVariantsBlock : TagStructure
            {
                [TagField(Length = 4)]
                public GpuSingleConstantRegisterArray[]  RuntimeMCount;
                
                [TagStructure(Size = 0x4)]
                public class GpuSingleConstantRegisterArray : TagStructure
                {
                    public float RuntimeGpuReal;
                }
            }
        }
    }
}
