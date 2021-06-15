using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "light_volume_system", Tag = "ltvl", Size = 0xC)]
    public class LightVolumeSystem : TagStructure
    {
        public List<LightVolumeDefinitionBlock> LightVolumes;
        
        [TagStructure(Size = 0x1A8)]
        public class LightVolumeDefinitionBlock : TagStructure
        {
            public StringId LightVolumeName;
            public MaterialStruct ActualMaterial;
            public LightVolumeAppearanceFlags AppearanceFlags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // avg. brightness head-on/side-view
            public float BrightnessRatio;
            public LightVolumeFlags Flags;
            // defaults to 0.0, ignored if 'lod enabled' not checked above
            public float LodInDistance;
            // defaults to 0.0, ignored if 'lod enabled' not checked above
            public float LodFeatherInDelta;
            public float InverseLodFeatherIn;
            // defaults to 30.0, ignored if 'lod enabled' not checked above
            [TagField(Length = 32)]
            public string LodOutDistance;
            // defaults to 10.0, ignored if 'lod enabled' not checked above
            public float LodFeatherOutDelta;
            public float InverseLodFeatherOut;
            public LightVolumePropertyReal Length;
            public LightVolumePropertyReal Offset;
            public LightVolumePropertyReal ProfileDensity;
            public LightVolumePropertyReal ProfileLength;
            public LightVolumePropertyReal ProfileThickness;
            public LightVolumePropertyRealRgbColor ProfileColor;
            public LightVolumePropertyReal ProfileAlpha;
            public LightVolumePropertyReal ProfileIntensity;
            public uint RuntimeMConstantPerProfileProperties;
            [TagField(Length = 32)]
            public string RuntimeMUsedStates;
            public uint RuntimeMMaxProfileCount;
            public GpuPropertyFunctionColorStruct RuntimeMGpuData;
            public List<LightVolumePrecompiledVertBlock> PrecompiledVertices;
            
            [Flags]
            public enum LightVolumeAppearanceFlags : ushort
            {
            }
            
            [Flags]
            public enum LightVolumeFlags : uint
            {
                // if not checked, the following flags do not matter, nor do LOD parameters below
                LodEnabled = 1 << 0,
                LodAlways10 = 1 << 1,
                LodSameInSplitscreen = 1 << 2,
                DisablePrecompiledProfiles = 1 << 3,
                ForcePrecompileProfiles = 1 << 4,
                CanBeLowRes = 1 << 5,
                Precompiled = 1 << 6
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
            public class LightVolumePropertyReal : TagStructure
            {
                public LightVolumeStateInputEnum InputVariable;
                public LightVolumeStateInputEnum RangeVariable;
                public OutputModEnum OutputModifier;
                public LightVolumeStateInputEnum OutputModifierInput;
                public MappingFunction Mapping;
                public float RuntimeMConstantValue;
                public ushort RuntimeMFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                public enum LightVolumeStateInputEnum : sbyte
                {
                    ProfilePosition,
                    GameTime,
                    LightVolumeAge,
                    LightVolumeRandom,
                    LightVolumeCorrelation1,
                    LightVolumeCorrelation2,
                    LightVolumeLod,
                    EffectAScale,
                    EffectBScale,
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
            
            [TagStructure(Size = 0x20)]
            public class LightVolumePropertyRealRgbColor : TagStructure
            {
                public LightVolumeStateInputEnum InputVariable;
                public LightVolumeStateInputEnum RangeVariable;
                public OutputModEnum OutputModifier;
                public LightVolumeStateInputEnum OutputModifierInput;
                public MappingFunction Mapping;
                public float RuntimeMConstantValue;
                public ushort RuntimeMFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                public enum LightVolumeStateInputEnum : sbyte
                {
                    ProfilePosition,
                    GameTime,
                    LightVolumeAge,
                    LightVolumeRandom,
                    LightVolumeCorrelation1,
                    LightVolumeCorrelation2,
                    LightVolumeLod,
                    EffectAScale,
                    EffectBScale,
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
            
            [TagStructure(Size = 0x24)]
            public class GpuPropertyFunctionColorStruct : TagStructure
            {
                public List<GpuPropertyBlock> RuntimeGpuPropertyBlock;
                public List<GpuFunctionBlock> RuntimeGpuFunctionsBlock;
                public List<GpuColorBlock> RuntimeGpuColorsBlock;
                
                [TagStructure(Size = 0x10)]
                public class GpuPropertyBlock : TagStructure
                {
                    [TagField(Length = 4)]
                    public GpuPropertySubArray[]  RuntimeGpuPropertySubArray;
                    
                    [TagStructure(Size = 0x4)]
                    public class GpuPropertySubArray : TagStructure
                    {
                        public float RuntimeGpuPropertyReal;
                    }
                }
                
                [TagStructure(Size = 0x40)]
                public class GpuFunctionBlock : TagStructure
                {
                    [TagField(Length = 16)]
                    public GpuFunctionSubArray[]  RuntimeGpuFunctionSubArray;
                    
                    [TagStructure(Size = 0x4)]
                    public class GpuFunctionSubArray : TagStructure
                    {
                        public float RuntimeGpuFunctionReal;
                    }
                }
                
                [TagStructure(Size = 0x10)]
                public class GpuColorBlock : TagStructure
                {
                    [TagField(Length = 4)]
                    public GpuColorSubArray[]  RuntimeGpuColorSubArray;
                    
                    [TagStructure(Size = 0x4)]
                    public class GpuColorSubArray : TagStructure
                    {
                        public float RuntimeGpuColorReal;
                    }
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class LightVolumePrecompiledVertBlock : TagStructure
            {
                public ushort R;
                public ushort G;
                public ushort B;
                public ushort Thickness;
            }
        }
    }
}
