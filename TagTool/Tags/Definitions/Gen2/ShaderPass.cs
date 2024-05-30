using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "shader_pass", Tag = "spas", Size = 0x24)]
    public class ShaderPass : TagStructure
    {
        public byte[] Documentation;
        public List<ShaderPassParameterBlock> Parameters;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        public List<ShaderPassImplementationBlock> Implementations;
        public List<ShaderPassPostprocessDefinitionNewBlock> PostprocessDefinition;
        
        [TagStructure(Size = 0x2C)]
        public class ShaderPassParameterBlock : TagStructure
        {
            public StringId Name;
            public byte[] Explanation;
            public TypeValue Type;
            public FlagsValue Flags;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag DefaultBitmap;
            public float DefaultConstValue;
            public RealRgbColor DefaultConstColor;
            public SourceExternValue SourceExtern;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum TypeValue : short
            {
                Bitmap,
                Value,
                Color,
                Switch
            }
            
            [Flags]
            public enum FlagsValue : ushort
            {
                NoBitmapLod = 1 << 0,
                RequiredParameter = 1 << 1
            }
            
            public enum SourceExternValue : short
            {
                None,
                Global, //  Eye Forward Vector (z)
                Global1, //  Eye Right Vector (x)
                Global2, //  Eye Up Vector (y)
                Object, //  Primary Color
                Object1, //  Secondary Color
                Object2, //  Function Value
                Light, //  Diffuse Color
                Light1, //  Specular Color
                Light2, //  Forward Vector (z)
                Light3, //  Right Vector (x)
                Light4, //  Up Vector (y)
                Light5, //  Object-Relative Forward Vector (z)
                Light6, //  Object-Relative Right Vector (x)
                Light7, //  Object-Relative Up Vector (y)
                Light8, //  Object Falloff Value
                Light9, //  Object Gel Color
                Lightmap, //  Object Ambient Factor
                Lightmap1, //  Object Direct Vector
                Lightmap2, //  Object Direct Color
                Lightmap3, //  Object Indirect Vector
                Lightmap4, //  Object Indirect Color
                OldFog, //  Atmospheric Color
                OldFog1, //  Atmospheric Max Density
                OldFog2, //  Planar Color
                OldFog3, //  Planar Max Density
                OldFog4, //  Atmospheric Planar Blend Value
                OldFog5, //  Object Atmospheric Density
                OldFog6, //  Object Planar Density
                OldFog7, //  Object Color
                OldFog8, //  Object Density
                Object3, //  Model Alpha
                Object4, //  Shadow Alpha
                Light10, //  Overbrighten Diffuse Shift
                Light11, //  Overbrighten Specular Shift
                Light12, //  Diffuse Contrast
                Light13, //  Specular Gel
                Shader, //  Specular Type
                Pad3,
                Pad3Scale,
                PadThai,
                TacoSalad,
                AnisotropicBinormal,
                ObjectLight, //  Shadow Fade
                Light14, //  Shadow Fade
                OldFog9, //  Atmospheric Density
                OldFog10, //  Planar Density
                OldFog11, //  Planar Density Invert
                Object5, //  Change Color Tertiary
                Object6, //  Change Color Quaternary
                Lightmap5, //  Object Specular Color
                Shader1, //  Lightmap Type
                Lightmap6, //  Object Ambient Color
                Shader2, //  Lightmap Specular Brightness
                Global3, //  Lightmap Shadow Darkening
                Lightmap7, //  Object Env Brightness
                Fog, //  Atmospheric Max Density
                Fog1, //  Atmospheric Color
                Fog2, //  Atmospheric Color Adj
                Fog3, //  Atmospheric Planar Blend
                Fog4, //  Atmospheric Planar Blend Adj Inv
                Fog5, //  Atmospheric Planar Blend Adj
                Fog6, //  Secondary Max Density
                Fog7, //  Secondary Color
                Fog8, //  Secondary Color Adj
                Fog9, //  Atmospheric Secondary Blend
                Fog10, //  Atmospheric Secondary Blend Adj Inv
                Fog11, //  Atmospheric Secondary Blend Adj
                Fog12, //  Sky Density
                Fog13, //  Sky Color
                Fog14, //  Sky Color Adj
                Fog15, //  Planar Max Density
                Fog16, //  Planar Color
                Fog17, //  Planar Color Adj
                Fog18, //  Planar Eye Density
                Fog19, //  Planar Eye Density Adj Inv
                Fog20, //  Planar Eye Density Adj
                Hud, //  Waypoint Primary Color
                Hud1, //  Waypoint Secondary Color
                Lightmap8, //  Object Specular Color Times One Half
                Light15, //  Specular Enabled
                Light16, //  Definition Specular Enabled
                Object7, //  Active Camo Amount
                Object8, //  Super Camo Amount
                Hud2, //  Custom Color 1
                Hud3, //  Custom Color 2
                Hud4, //  Custom Color 3
                Hud5, //  Custom Color 4
                Object9, //  Active Camo RGB
                Fog21, //  Patchy Plane n(xyz)
                Fog22, //  Patchy Plane d(w)
                Hud6, //  Global Fade
                ScreenEffect, //  Primary
                ScreenEffect1 //  Secondary
            }
        }
        
        [TagStructure(Size = 0x74)]
        public class ShaderPassImplementationBlock : TagStructure
        {
            public FlagsValue Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<ShaderPassTextureBlock> Textures;
            [TagField(ValidTags = new [] { "vrtx" })]
            public CachedTag VertexShader;
            public List<ShaderPassVertexShaderConstantBlock> VsConstants;
            public byte[] PixelShaderCodeNoLongerUsed;
            public ChannelsValue Channels;
            public AlphaBlendValue AlphaBlend;
            public DepthValue Depth;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public List<ShaderStateChannelsStateBlock> ChannelState;
            public List<ShaderStateAlphaBlendStateBlock> AlphaBlendState;
            public List<ShaderStateAlphaTestStateBlock> AlphaTestState;
            public List<ShaderStateDepthStateBlock> DepthState;
            public List<ShaderStateCullStateBlock> CullState;
            public List<ShaderStateFillStateBlock> FillState;
            public List<ShaderStateMiscStateBlock> MiscState;
            public List<ShaderStateConstantBlock> Constants;
            [TagField(ValidTags = new [] { "pixl" })]
            public CachedTag PixelShader;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                DeleteFromCacheFile = 1 << 0,
                Critical = 1 << 1
            }
            
            [TagStructure(Size = 0x3C)]
            public class ShaderPassTextureBlock : TagStructure
            {
                public StringId SourceParameter;
                public SourceExternValue SourceExtern;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(Length = 0x2)]
                public byte[] Unknown;
                public ModeValue Mode;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public DotMappingValue DotMapping;
                public short InputStage; // [0,3]
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public List<ShaderTextureStateAddressStateBlock> AddressState;
                public List<ShaderTextureStateFilterStateBlock> FilterState;
                public List<ShaderTextureStateKillStateBlock> KillState;
                public List<ShaderTextureStateMiscStateBlock> MiscState;
                public List<ShaderTextureStateConstantBlock> Constants;
                
                public enum SourceExternValue : short
                {
                    None,
                    Global, //  Vector Normalization
                    Unused,
                    Global1, //  Target texaccum
                    Unused1,
                    Global2, //  Target Frame Buffer
                    Globa, //  Target z
                    Unused2,
                    Global3, //  Target Shadow
                    Light, //  Falloff
                    Light1, //  Gel
                    Lightmap,
                    Unused3,
                    Global4, //  Shadow Buffer
                    Global5, //  Gradient Separate
                    Global6, //  Gradient Product
                    Hud, //  Bitmap
                    Global7, //  active camo
                    Global8, //  Texture Camera
                    Global9, //  Water Reflection
                    Global10, //  Water Refraction
                    Global11, //  Aux 1
                    Global12, //  Aux 2
                    Global13, //  Particle Distortion
                    Global14, //  Convolution 1
                    Global15, //  Convolution 2
                    Shader, //  Active Camo Bump
                    FirstPerson //  Scope
                }
                
                public enum ModeValue : short
                {
                    _2d,
                    _3d,
                    CubeMap,
                    Passthrough,
                    Texkill,
                    _2dDependentAr,
                    _2dDependentGb,
                    _2dBumpenv,
                    _2dBumpenvLuminance,
                    _3dBrdf,
                    DotProduct,
                    DotProduct2d,
                    DotProduct3d,
                    DotProductCubeMap,
                    DotProductZw,
                    DotReflectDiffuse,
                    DotReflectSpecular,
                    DotReflectSpecularConst,
                    None
                }
                
                public enum DotMappingValue : short
                {
                    _0To1,
                    SignedD3d,
                    SignedGl,
                    SignedNv,
                    Hilo, //  0 to 1
                    Hilo1, //  Signed Hemisphere D3D
                    Hilo2, //  Signed Hemisphere GL
                    Hilo3 //  Signed Hemisphere NV
                }
                
                [TagStructure(Size = 0x8)]
                public class ShaderTextureStateAddressStateBlock : TagStructure
                {
                    public UAddressModeValue UAddressMode;
                    public VAddressModeValue VAddressMode;
                    public WAddressModeValue WAddressMode;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    public enum UAddressModeValue : short
                    {
                        Wrap,
                        Mirror,
                        Clamp,
                        Border,
                        ClampToEdge
                    }
                    
                    public enum VAddressModeValue : short
                    {
                        Wrap,
                        Mirror,
                        Clamp,
                        Border,
                        ClampToEdge
                    }
                    
                    public enum WAddressModeValue : short
                    {
                        Wrap,
                        Mirror,
                        Clamp,
                        Border,
                        ClampToEdge
                    }
                }
                
                [TagStructure(Size = 0x10)]
                public class ShaderTextureStateFilterStateBlock : TagStructure
                {
                    public MagFilterValue MagFilter;
                    public MinFilterValue MinFilter;
                    public MipFilterValue MipFilter;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public float MipmapBias;
                    /// <summary>
                    /// 0 means all mipmap levels are used
                    /// </summary>
                    public short MaxMipmapIndex;
                    public AnisotropyValue Anisotropy;
                    
                    public enum MagFilterValue : short
                    {
                        None,
                        PointSampled,
                        Linear,
                        Anisotropic,
                        Quincunx,
                        GaussianCubic
                    }
                    
                    public enum MinFilterValue : short
                    {
                        None,
                        PointSampled,
                        Linear,
                        Anisotropic,
                        Quincunx,
                        GaussianCubic
                    }
                    
                    public enum MipFilterValue : short
                    {
                        None,
                        PointSampled,
                        Linear,
                        Anisotropic,
                        Quincunx,
                        GaussianCubic
                    }
                    
                    public enum AnisotropyValue : short
                    {
                        NonAnisotropic,
                        _2Tap,
                        _3Tap,
                        _4Tap
                    }
                }
                
                [TagStructure(Size = 0xC)]
                public class ShaderTextureStateKillStateBlock : TagStructure
                {
                    public FlagsValue Flags;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public ColorkeyModeValue ColorkeyMode;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding1;
                    public ArgbColor ColorkeyColor;
                    
                    [Flags]
                    public enum FlagsValue : ushort
                    {
                        AlphaKill = 1 << 0
                    }
                    
                    public enum ColorkeyModeValue : short
                    {
                        Disabled,
                        ZeroAlpha,
                        ZeroArgb,
                        Kill
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ShaderTextureStateMiscStateBlock : TagStructure
                {
                    public ComponentSignFlagsValue ComponentSignFlags;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public ArgbColor BorderColor;
                    
                    [Flags]
                    public enum ComponentSignFlagsValue : ushort
                    {
                        RSigned = 1 << 0,
                        GSigned = 1 << 1,
                        BSigned = 1 << 2,
                        ASigned = 1 << 3
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ShaderTextureStateConstantBlock : TagStructure
                {
                    public StringId SourceParameter;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public ConstantValue Constant;
                    
                    public enum ConstantValue : short
                    {
                        MipmapBiasValue,
                        ColorkeyColor,
                        BorderColor,
                        BorderAlphaValue,
                        BumpenvMat00,
                        BumpenvMat01,
                        BumpenvMat10,
                        BumpenvMat11,
                        BumpenvLumScaleValue,
                        BumpenvLumOffsetValue
                    }
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class ShaderPassVertexShaderConstantBlock : TagStructure
            {
                public StringId SourceParameter;
                public ScaleByTextureStageValue ScaleByTextureStage;
                public RegisterBankValue RegisterBank;
                public short RegisterIndex;
                public ComponentMaskValue ComponentMask;
                
                public enum ScaleByTextureStageValue : short
                {
                    None,
                    Stage0,
                    Stage1,
                    Stage2,
                    Stage3
                }
                
                public enum RegisterBankValue : short
                {
                    Vn015,
                    Cn012
                }
                
                public enum ComponentMaskValue : short
                {
                    XValue,
                    YValue,
                    ZValue,
                    WValue,
                    XyzRgbColor,
                    XUniformScale,
                    YUniformScale,
                    ZUniformScale,
                    WUniformScale,
                    Xy2dScale,
                    Zw2dScale,
                    Xy2dTranslation,
                    Zw2dTranslation,
                    Xyzw2dSimpleXform,
                    XywRow12dAffineXform,
                    XywRow22dAffineXform,
                    Xyz3dScale,
                    Xyz3dTranslation,
                    XyzwRow13dAffineXform,
                    XyzwRow23dAffineXform,
                    XyzwRow33dAffineXform
                }
            }
            
            public enum ChannelsValue : short
            {
                All,
                ColorOnly,
                AlphaOnly,
                Custom
            }
            
            public enum AlphaBlendValue : short
            {
                Disabled,
                Add,
                Multiply,
                AddSrcTimesDstalpha,
                AddSrcTimesSrcalpha,
                AddDstTimesSrcalphaInverse,
                AlphaBlend,
                Custom
            }
            
            public enum DepthValue : short
            {
                Disabled,
                DefaultOpaque,
                DefaultOpaqueWrite,
                DefaultTransparent,
                Custom
            }
            
            [TagStructure(Size = 0x4)]
            public class ShaderStateChannelsStateBlock : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    R = 1 << 0,
                    G = 1 << 1,
                    B = 1 << 2,
                    A = 1 << 3
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class ShaderStateAlphaBlendStateBlock : TagStructure
            {
                public BlendFunctionValue BlendFunction;
                public BlendSrcFactorValue BlendSrcFactor;
                public BlendDstFactorValue BlendDstFactor;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ArgbColor BlendColor;
                public LogicOpFlagsValue LogicOpFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                
                public enum BlendFunctionValue : short
                {
                    Add,
                    Subtract,
                    ReverseSubtract,
                    Min,
                    Max,
                    AddSigned,
                    ReverseSubtractSigned,
                    LogicOp
                }
                
                public enum BlendSrcFactorValue : short
                {
                    Zero,
                    One,
                    Srccolor,
                    SrccolorInverse,
                    Srcalpha,
                    SrcalphaInverse,
                    Dstcolor,
                    DstcolorInverse,
                    Dstalpha,
                    DstalphaInverse,
                    SrcalphaSaturate,
                    ConstantColor,
                    ConstantColorInverse,
                    ConstantAlpha,
                    ConstantAlphaInverse
                }
                
                public enum BlendDstFactorValue : short
                {
                    Zero,
                    One,
                    Srccolor,
                    SrccolorInverse,
                    Srcalpha,
                    SrcalphaInverse,
                    Dstcolor,
                    DstcolorInverse,
                    Dstalpha,
                    DstalphaInverse,
                    SrcalphaSaturate,
                    ConstantColor,
                    ConstantColorInverse,
                    ConstantAlpha,
                    ConstantAlphaInverse
                }
                
                [Flags]
                public enum LogicOpFlagsValue : ushort
                {
                    Src0Dst0 = 1 << 0,
                    Src0Dst1 = 1 << 1,
                    Src1Dst0 = 1 << 2,
                    Src1Dst1 = 1 << 3
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class ShaderStateAlphaTestStateBlock : TagStructure
            {
                public FlagsValue Flags;
                public AlphaCompareFunctionValue AlphaCompareFunction;
                public short AlphaTestRef; // [0,255]
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    AlphaTestEnabled = 1 << 0,
                    SampleAlphaToCoverage = 1 << 1,
                    SampleAlphaToOne = 1 << 2
                }
                
                public enum AlphaCompareFunctionValue : short
                {
                    Never,
                    Less,
                    Equal,
                    LessOrEqual,
                    Greater,
                    NotEqual,
                    GreaterOrEqual,
                    Always
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class ShaderStateDepthStateBlock : TagStructure
            {
                public ModeValue Mode;
                public DepthCompareFunctionValue DepthCompareFunction;
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float DepthBiasSlopeScale;
                public float DepthBias;
                
                public enum ModeValue : short
                {
                    UseZ,
                    UseW
                }
                
                public enum DepthCompareFunctionValue : short
                {
                    Never,
                    Less,
                    Equal,
                    LessOrEqual,
                    Greater,
                    NotEqual,
                    GreaterOrEqual,
                    Always
                }
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    DepthWrite = 1 << 0,
                    OffsetPoints = 1 << 1,
                    OffsetLines = 1 << 2,
                    OffsetTriangles = 1 << 3,
                    ClipControlDonTCullPrimitive = 1 << 4,
                    ClipControlClamp = 1 << 5,
                    ClipControlIgnoreWSign = 1 << 6
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class ShaderStateCullStateBlock : TagStructure
            {
                public ModeValue Mode;
                public FrontFaceValue FrontFace;
                
                public enum ModeValue : short
                {
                    None,
                    Cw,
                    Ccw
                }
                
                public enum FrontFaceValue : short
                {
                    Cw,
                    Ccw
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class ShaderStateFillStateBlock : TagStructure
            {
                public FlagsValue Flags;
                public FillModeValue FillMode;
                public BackFillModeValue BackFillMode;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float LineWidth;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    FlatShading = 1 << 0,
                    EdgeAntialiasing = 1 << 1
                }
                
                public enum FillModeValue : short
                {
                    Solid,
                    Wireframe,
                    Points
                }
                
                public enum BackFillModeValue : short
                {
                    Solid,
                    Wireframe,
                    Points
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class ShaderStateMiscStateBlock : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ArgbColor FogColor;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    YuvToRgb = 1 << 0,
                    _16BitDither = 1 << 1,
                    _32BitDxt1Noise = 1 << 2
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class ShaderStateConstantBlock : TagStructure
            {
                public StringId SourceParameter;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ConstantValue Constant;
                
                public enum ConstantValue : short
                {
                    ConstantBlendColor,
                    ConstantBlendAlphaValue,
                    AlphaTestRefValue,
                    DepthBiasSlopeScaleValue,
                    DepthBiasValue,
                    LineWidthValue,
                    FogColor
                }
            }
        }
        
        [TagStructure(Size = 0x58)]
        public class ShaderPassPostprocessDefinitionNewBlock : TagStructure
        {
            public List<ShaderPassPostprocessImplementationNewBlock> Implementations;
            public List<ShaderPassPostprocessTextureNewBlock> Textures;
            public List<RenderStateBlock> RenderStates;
            public List<ShaderPassPostprocessTextureStateBlock> TextureStates;
            public List<PixelShaderFragmentBlock> PsFragments;
            public List<PixelShaderPermutationNewBlock> PsPermutations;
            public List<PixelShaderCombinerBlock> PsCombiners;
            public List<ShaderPassPostprocessExternNewBlock> Externs;
            public List<ShaderPassPostprocessConstantNewBlock> Constants;
            public List<ShaderPassPostprocessConstantInfoNewBlock> ConstantInfo;
            public List<ShaderPassPostprocessImplementationBlock> OldImplementations;
            
            [TagStructure(Size = 0x14A)]
            public class ShaderPassPostprocessImplementationNewBlock : TagStructure
            {
                public TagBlockIndexGen2 Textures;
                public TagBlockIndexGen2 RenderStates;
                public TagBlockIndexGen2 TextureStates;
                [TagField(Length = 0xF0)]
                public byte[] Unknown;
                public TagBlockIndexGen2 PsFragments;
                public TagBlockIndexGen2 PsPermutations;
                public TagBlockIndexGen2 PsCombiners;
                [TagField(ValidTags = new [] { "vrtx" })]
                public CachedTag VertexShader;
                [TagField(Length = 0x8)]
                public byte[] Unknown1;
                [TagField(Length = 0x8)]
                public byte[] Unknown2;
                [TagField(Length = 0x4)]
                public byte[] Unknown3;
                [TagField(Length = 0x4)]
                public byte[] Unknown4;
                public TagBlockIndexGen2 DefaultRenderStates;
                public TagBlockIndexGen2 RenderStateExterns;
                public TagBlockIndexGen2 TextureStateExterns;
                public TagBlockIndexGen2 PixelConstantExterns;
                public TagBlockIndexGen2 VertexConstantExterns;
                public TagBlockIndexGen2 PsConstants;
                public TagBlockIndexGen2 VsConstants;
                public TagBlockIndexGen2 PixelConstantInfo;
                public TagBlockIndexGen2 VertexConstantInfo;
                public TagBlockIndexGen2 RenderStateInfo;
                public TagBlockIndexGen2 TextureStateInfo;
                public List<ShaderPostprocessPixelShader> PixelShader;
                public List<PixelShaderExternMapBlock> PixelShaderSwitchExternMap;
                public List<PixelShaderIndexBlock> PixelShaderIndexBlock1;
                
                [TagStructure(Size = 0x2C)]
                public class ShaderPostprocessPixelShader : TagStructure
                {
                    public int PixelShaderHandleRuntime;
                    public int PixelShaderHandleRuntime1;
                    public int PixelShaderHandleRuntime2;
                    public List<ShaderPostprocessPixelShaderConstantDefaults> ConstantRegisterDefaults;
                    public byte[] CompiledShader;
                    public byte[] CompiledShader1;
                    public byte[] CompiledShader2;
                    
                    [TagStructure(Size = 0x4)]
                    public class ShaderPostprocessPixelShaderConstantDefaults : TagStructure
                    {
                        public int Defaults;
                    }
                }
                
                [TagStructure(Size = 0x2)]
                public class PixelShaderExternMapBlock : TagStructure
                {
                    public sbyte SwitchParameter;
                    public sbyte CaseScalar;
                }
                
                [TagStructure(Size = 0x1)]
                public class PixelShaderIndexBlock : TagStructure
                {
                    public sbyte PixelShaderIndex;
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class ShaderPassPostprocessTextureNewBlock : TagStructure
            {
                public sbyte BitmapParameterIndex;
                public sbyte BitmapExternIndex;
                public sbyte TextureStageIndex;
                public sbyte Flags;
            }
            
            [TagStructure(Size = 0x5)]
            public class RenderStateBlock : TagStructure
            {
                public sbyte StateIndex;
                public int StateValue;
            }
            
            [TagStructure(Size = 0x18)]
            public class ShaderPassPostprocessTextureStateBlock : TagStructure
            {
                [TagField(Length = 0x18)]
                public byte[] Unknown;
            }
            
            [TagStructure(Size = 0x3)]
            public class PixelShaderFragmentBlock : TagStructure
            {
                public sbyte SwitchParameterIndex;
                public TagBlockIndexGen2 PermutationsIndex;
            }
            
            [TagStructure(Size = 0x6)]
            public class PixelShaderPermutationNewBlock : TagStructure
            {
                public short EnumIndex;
                public short Flags;
                public TagBlockIndexGen2 Combiners;
            }
            
            [TagStructure(Size = 0x20)]
            public class PixelShaderCombinerBlock : TagStructure
            {
                [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ArgbColor ConstantColor0;
                public ArgbColor ConstantColor1;
                public sbyte ColorARegisterPtrIndex;
                public sbyte ColorBRegisterPtrIndex;
                public sbyte ColorCRegisterPtrIndex;
                public sbyte ColorDRegisterPtrIndex;
                public sbyte AlphaARegisterPtrIndex;
                public sbyte AlphaBRegisterPtrIndex;
                public sbyte AlphaCRegisterPtrIndex;
                public sbyte AlphaDRegisterPtrIndex;
            }
            
            [TagStructure(Size = 0x4)]
            public class ShaderPassPostprocessExternNewBlock : TagStructure
            {
                [TagField(Length = 0x3)]
                public byte[] Unknown;
                public sbyte ExternIndex;
            }
            
            [TagStructure(Size = 0x7)]
            public class ShaderPassPostprocessConstantNewBlock : TagStructure
            {
                public StringId ParameterName;
                public sbyte ComponentMask;
                public sbyte ScaleByTextureStage;
                public sbyte FunctionIndex;
            }
            
            [TagStructure(Size = 0x7)]
            public class ShaderPassPostprocessConstantInfoNewBlock : TagStructure
            {
                public StringId ParameterName;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x1B6)]
            public class ShaderPassPostprocessImplementationBlock : TagStructure
            {
                public ShaderGpuStateStructBlock GpuState;
                public ShaderGpuStateReferenceStructBlock GpuConstantState;
                public ShaderGpuStateReferenceStructBlock1 GpuVolatileState;
                public ShaderGpuStateReferenceStructBlock2 GpuDefaultState;
                [TagField(ValidTags = new [] { "vrtx" })]
                public CachedTag VertexShader;
                [TagField(Length = 0x8)]
                public byte[] Unknown;
                [TagField(Length = 0x8)]
                public byte[] Unknown1;
                [TagField(Length = 0x4)]
                public byte[] Unknown2;
                [TagField(Length = 0x4)]
                public byte[] Unknown3;
                public List<ExternReferenceBlock> ValueExterns;
                public List<ExternReferenceBlock1> ColorExterns;
                public List<ExternReferenceBlock2> SwitchExterns;
                public short BitmapParameterCount;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(Length = 0xF0)]
                public byte[] Unknown4;
                public List<PixelShaderFragmentBlock> PixelShaderFragments;
                public List<PixelShaderPermutationBlock> PixelShaderPermutations;
                public List<PixelShaderCombinerBlock> PixelShaderCombiners;
                public List<PixelShaderConstantBlock> PixelShaderConstants;
                [TagField(Length = 0x4)]
                public byte[] Unknown5;
                [TagField(Length = 0x4)]
                public byte[] Unknown6;
                
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
                
                [TagStructure(Size = 0xE)]
                public class ShaderGpuStateReferenceStructBlock : TagStructure
                {
                    public TagBlockIndexGen2 RenderStates;
                    public TagBlockIndexGen2 TextureStageStates;
                    public TagBlockIndexGen2 RenderStateParameters;
                    public TagBlockIndexGen2 TextureStageParameters;
                    public TagBlockIndexGen2 Textures;
                    public TagBlockIndexGen2 VnConstants;
                    public TagBlockIndexGen2 CnConstants;
                }
                
                [TagStructure(Size = 0xE)]
                public class ShaderGpuStateReferenceStructBlock1 : TagStructure
                {
                    public TagBlockIndexGen2 RenderStates;
                    public TagBlockIndexGen2 TextureStageStates;
                    public TagBlockIndexGen2 RenderStateParameters;
                    public TagBlockIndexGen2 TextureStageParameters;
                    public TagBlockIndexGen2 Textures;
                    public TagBlockIndexGen2 VnConstants;
                    public TagBlockIndexGen2 CnConstants;
                }
                
                [TagStructure(Size = 0xE)]
                public class ShaderGpuStateReferenceStructBlock2 : TagStructure
                {
                    public TagBlockIndexGen2 RenderStates;
                    public TagBlockIndexGen2 TextureStageStates;
                    public TagBlockIndexGen2 RenderStateParameters;
                    public TagBlockIndexGen2 TextureStageParameters;
                    public TagBlockIndexGen2 Textures;
                    public TagBlockIndexGen2 VnConstants;
                    public TagBlockIndexGen2 CnConstants;
                }
                
                [TagStructure(Size = 0x2)]
                public class ExternReferenceBlock : TagStructure
                {
                    public sbyte ParameterIndex;
                    public sbyte ExternIndex;
                }
                
                [TagStructure(Size = 0x2)]
                public class ExternReferenceBlock1 : TagStructure
                {
                    public sbyte ParameterIndex;
                    public sbyte ExternIndex;
                }
                
                [TagStructure(Size = 0x2)]
                public class ExternReferenceBlock2 : TagStructure
                {
                    public sbyte ParameterIndex;
                    public sbyte ExternIndex;
                }
                
                [TagStructure(Size = 0x3)]
                public class PixelShaderFragmentBlock : TagStructure
                {
                    public sbyte SwitchParameterIndex;
                    public TagBlockIndexGen2 PermutationsIndex;
                }
                
                [TagStructure(Size = 0x10)]
                public class PixelShaderPermutationBlock : TagStructure
                {
                    public short EnumIndex;
                    public FlagsValue Flags;
                    public TagBlockIndexGen2 Constants;
                    public TagBlockIndexGen2 Combiners;
                    [TagField(Length = 0x4)]
                    public byte[] Unknown;
                    [TagField(Length = 0x4)]
                    public byte[] Unknown1;
                    
                    [Flags]
                    public enum FlagsValue : ushort
                    {
                        HasFinalCombiner = 1 << 0
                    }
                }
                
                [TagStructure(Size = 0x20)]
                public class PixelShaderCombinerBlock : TagStructure
                {
                    [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public ArgbColor ConstantColor0;
                    public ArgbColor ConstantColor1;
                    public sbyte ColorARegisterPtrIndex;
                    public sbyte ColorBRegisterPtrIndex;
                    public sbyte ColorCRegisterPtrIndex;
                    public sbyte ColorDRegisterPtrIndex;
                    public sbyte AlphaARegisterPtrIndex;
                    public sbyte AlphaBRegisterPtrIndex;
                    public sbyte AlphaCRegisterPtrIndex;
                    public sbyte AlphaDRegisterPtrIndex;
                }
                
                [TagStructure(Size = 0x6)]
                public class PixelShaderConstantBlock : TagStructure
                {
                    public ParameterTypeValue ParameterType;
                    public sbyte CombinerIndex;
                    public sbyte RegisterIndex;
                    public ComponentMaskValue ComponentMask;
                    [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding1;
                    
                    public enum ParameterTypeValue : sbyte
                    {
                        Bitmap,
                        Value,
                        Color,
                        Switch
                    }
                    
                    public enum ComponentMaskValue : sbyte
                    {
                        XValue,
                        YValue,
                        ZValue,
                        WValue,
                        XyzRgbColor
                    }
                }
            }
        }
    }
}

