using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "shader_pass", Tag = "spas", Size = 0x3C)]
    public class ShaderPass : TagStructure
    {
        public byte[] Documentation;
        public List<ShaderPassParameter> Parameters;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding2;
        public List<ShaderPassImplementation> Implementations;
        public List<ShaderPassPostprocessDefinitionNew> PostprocessDefinition;
        
        [TagStructure(Size = 0x40)]
        public class ShaderPassParameter : TagStructure
        {
            public StringId Name;
            public byte[] Explanation;
            public TypeValue Type;
            public FlagsValue Flags;
            public CachedTag DefaultBitmap;
            public float DefaultConstValue;
            public RealRgbColor DefaultConstColor;
            public SourceExternValue SourceExtern;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            
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
                GlobalEyeForwardVectorZ,
                GlobalEyeRightVectorX,
                GlobalEyeUpVectorY,
                ObjectPrimaryColor,
                ObjectSecondaryColor,
                ObjectFunctionValue,
                LightDiffuseColor,
                LightSpecularColor,
                LightForwardVectorZ,
                LightRightVectorX,
                LightUpVectorY,
                LightObjectRelativeForwardVectorZ,
                LightObjectRelativeRightVectorX,
                LightObjectRelativeUpVectorY,
                LightObjectFalloffValue,
                LightObjectGelColor,
                LightmapObjectAmbientFactor,
                LightmapObjectDirectVector,
                LightmapObjectDirectColor,
                LightmapObjectIndirectVector,
                LightmapObjectIndirectColor,
                OldFogAtmosphericColor,
                OldFogAtmosphericMaxDensity,
                OldFogPlanarColor,
                OldFogPlanarMaxDensity,
                OldFogAtmosphericPlanarBlendValue,
                OldFogObjectAtmosphericDensity,
                OldFogObjectPlanarDensity,
                OldFogObjectColor,
                OldFogObjectDensity,
                ObjectModelAlpha,
                ObjectShadowAlpha,
                LightOverbrightenDiffuseShift,
                LightOverbrightenSpecularShift,
                LightDiffuseContrast,
                LightSpecularGel,
                ShaderSpecularType,
                Pad3,
                Pad3Scale,
                PadThai,
                TacoSalad,
                AnisotropicBinormal,
                ObjectLightShadowFade,
                LightShadowFade,
                OldFogAtmosphericDensity,
                OldFogPlanarDensity,
                OldFogPlanarDensityInvert,
                ObjectChangeColorTertiary,
                ObjectChangeColorQuaternary,
                LightmapObjectSpecularColor,
                ShaderLightmapType,
                LightmapObjectAmbientColor,
                ShaderLightmapSpecularBrightness,
                GlobalLightmapShadowDarkening,
                LightmapObjectEnvBrightness,
                FogAtmosphericMaxDensity,
                FogAtmosphericColor,
                FogAtmosphericColorAdj,
                FogAtmosphericPlanarBlend,
                FogAtmosphericPlanarBlendAdjInv,
                FogAtmosphericPlanarBlendAdj,
                FogSecondaryMaxDensity,
                FogSecondaryColor,
                FogSecondaryColorAdj,
                FogAtmosphericSecondaryBlend,
                FogAtmosphericSecondaryBlendAdjInv,
                FogAtmosphericSecondaryBlendAdj,
                FogSkyDensity,
                FogSkyColor,
                FogSkyColorAdj,
                FogPlanarMaxDensity,
                FogPlanarColor,
                FogPlanarColorAdj,
                FogPlanarEyeDensity,
                FogPlanarEyeDensityAdjInv,
                FogPlanarEyeDensityAdj,
                HudWaypointPrimaryColor,
                HudWaypointSecondaryColor,
                LightmapObjectSpecularColorTimesOneHalf,
                LightSpecularEnabled,
                LightDefinitionSpecularEnabled,
                ObjectActiveCamoAmount,
                ObjectSuperCamoAmount,
                HudCustomColor1,
                HudCustomColor2,
                HudCustomColor3,
                HudCustomColor4,
                ObjectActiveCamoRgb,
                FogPatchyPlaneNXyz,
                FogPatchyPlaneDW,
                HudGlobalFade,
                ScreenEffectPrimary,
                ScreenEffectSecondary
            }
        }
        
        [TagStructure(Size = 0xB8)]
        public class ShaderPassImplementation : TagStructure
        {
            public FlagsValue Flags;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public List<ShaderPassTexture> Textures;
            /// <summary>
            /// VERTEX SHADER
            /// </summary>
            public CachedTag VertexShader;
            public List<ShaderPassConstant> VsConstants;
            public byte[] PixelShaderCodeNoLongerUsed;
            /// <summary>
            /// STATE
            /// </summary>
            public ChannelsValue Channels;
            public AlphaBlendValue AlphaBlend;
            public DepthValue Depth;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            public List<ShaderStateChannelsState> ChannelState;
            public List<ShaderStateAlphaBlendState> AlphaBlendState;
            public List<ShaderStateAlphaTestState> AlphaTestState;
            public List<ShaderStateDepthState> DepthState;
            public List<ShaderStateCullState> CullState;
            public List<ShaderStateFillState> FillState;
            public List<ShaderStateMiscState> MiscState;
            public List<ShaderStateConstant> Constants;
            public CachedTag PixelShader;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                DeleteFromCacheFile = 1 << 0,
                Critical = 1 << 1
            }
            
            [TagStructure(Size = 0x50)]
            public class ShaderPassTexture : TagStructure
            {
                public StringId SourceParameter;
                public SourceExternValue SourceExtern;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Unknown1;
                public ModeValue Mode;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding2;
                public DotMappingValue DotMapping;
                public short InputStage; // [0,3]
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding3;
                public List<ShaderTextureStateAddressState> AddressState;
                public List<ShaderTextureStateFilterState> FilterState;
                public List<ShaderTextureStateKillState> KillState;
                public List<ShaderTextureStateMiscState> MiscState;
                public List<ShaderStateConstant> Constants;
                
                public enum SourceExternValue : short
                {
                    None,
                    GlobalVectorNormalization,
                    Unused,
                    GlobalTargetTexaccum,
                    Unused0,
                    GlobalTargetFrameBuffer,
                    GlobaTargetZ,
                    Unused1,
                    GlobalTargetShadow,
                    LightFalloff,
                    LightGel,
                    Lightmap,
                    Unused2,
                    GlobalShadowBuffer,
                    GlobalGradientSeparate,
                    GlobalGradientProduct,
                    HudBitmap,
                    GlobalActiveCamo,
                    GlobalTextureCamera,
                    GlobalWaterReflection,
                    GlobalWaterRefraction,
                    GlobalAux1,
                    GlobalAux2,
                    GlobalParticleDistortion,
                    GlobalConvolution1,
                    GlobalConvolution2,
                    ShaderActiveCamoBump,
                    FirstPersonScope
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
                    Hilo0To1,
                    HiloSignedHemisphereD3d,
                    HiloSignedHemisphereGl,
                    HiloSignedHemisphereNv
                }
                
                [TagStructure(Size = 0x8)]
                public class ShaderTextureStateAddressState : TagStructure
                {
                    public UAddressModeValue UAddressMode;
                    public VAddressModeValue VAddressMode;
                    public WAddressModeValue WAddressMode;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
                    
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
                public class ShaderTextureStateFilterState : TagStructure
                {
                    public MagFilterValue MagFilter;
                    public MinFilterValue MinFilter;
                    public MipFilterValue MipFilter;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
                    public float MipmapBias;
                    public short MaxMipmapIndex; // 0 means all mipmap levels are used
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
                public class ShaderTextureStateKillState : TagStructure
                {
                    public FlagsValue Flags;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
                    public ColorkeyModeValue ColorkeyMode;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding2;
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
                public class ShaderTextureStateMiscState : TagStructure
                {
                    public ComponentSignFlagsValue ComponentSignFlags;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
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
                public class ShaderStateConstant : TagStructure
                {
                    public StringId SourceParameter;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
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
            public class ShaderPassConstant : TagStructure
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
            public class ShaderStateChannelsState : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                
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
            public class ShaderStateAlphaBlendState : TagStructure
            {
                public BlendFunctionValue BlendFunction;
                public BlendSrcFactorValue BlendSrcFactor;
                public BlendDstFactorValue BlendDstFactor;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public ArgbColor BlendColor;
                public LogicOpFlagsValue LogicOpFlags;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding2;
                
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
            public class ShaderStateAlphaTestState : TagStructure
            {
                public FlagsValue Flags;
                public AlphaCompareFunctionValue AlphaCompareFunction;
                public short AlphaTestRef; // [0,255]
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                
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
            public class ShaderStateDepthState : TagStructure
            {
                public ModeValue Mode;
                public DepthCompareFunctionValue DepthCompareFunction;
                public FlagsValue Flags;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
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
            public class ShaderStateCullState : TagStructure
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
            public class ShaderStateFillState : TagStructure
            {
                public FlagsValue Flags;
                public FillModeValue FillMode;
                public BackFillModeValue BackFillMode;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
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
            public class ShaderStateMiscState : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
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
            public class ShaderStateConstant : TagStructure
            {
                public StringId SourceParameter;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
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
        
        [TagStructure(Size = 0x84)]
        public class ShaderPassPostprocessDefinitionNew : TagStructure
        {
            public List<ShaderPassPostprocessImplementationNew> Implementations;
            public List<ShaderPassPostprocessTextureNew> Textures;
            public List<RenderState> RenderStates;
            public List<ShaderPassPostprocessTextureState> TextureStates;
            public List<PixelShaderFragment> PsFragments;
            public List<PixelShaderPermutationNew> PsPermutations;
            public List<PixelShaderCombiner> PsCombiners;
            public List<ShaderPassPostprocessExternNew> Externs;
            public List<ShaderPassPostprocessConstantNew> Constants;
            public List<ShaderPassPostprocessConstantInfoNew> ConstantInfo;
            public List<ShaderPassPostprocessImplementation> OldImplementations;
            
            [TagStructure(Size = 0x15E)]
            public class ShaderPassPostprocessImplementationNew : TagStructure
            {
                public TagBlockIndex Textures;
                public TagBlockIndex RenderStates;
                public TagBlockIndex TextureStates;
                [TagField(Flags = Padding, Length = 240)]
                public byte[] Unknown1;
                public TagBlockIndex PsFragments;
                public TagBlockIndex PsPermutations;
                public TagBlockIndex PsCombiners;
                public CachedTag VertexShader;
                [TagField(Flags = Padding, Length = 8)]
                public byte[] Unknown2;
                [TagField(Flags = Padding, Length = 8)]
                public byte[] Unknown3;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown4;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown5;
                public TagBlockIndex DefaultRenderStates;
                public TagBlockIndex RenderStateExterns;
                public TagBlockIndex TextureStateExterns;
                public TagBlockIndex PixelConstantExterns;
                public TagBlockIndex VertexConstantExterns;
                public TagBlockIndex PsConstants;
                public TagBlockIndex VsConstants;
                public TagBlockIndex PixelConstantInfo;
                public TagBlockIndex VertexConstantInfo;
                public TagBlockIndex RenderStateInfo;
                public TagBlockIndex TextureStateInfo;
                public List<PixelShaderBlock> PixelShader;
                public List<PixelShaderExternMap> PixelShaderSwitchExternMap;
                public List<PixelShaderIndex> PixelShaderIndexBlock;
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndex : TagStructure
                {
                    public short BlockIndexData;
                }
                
                [TagStructure(Size = 0x54)]
                public class PixelShaderBlock : TagStructure
                {
                    public int PixelShaderHandleRuntime;
                    public int PixelShaderHandleRuntime1;
                    public int PixelShaderHandleRuntime2;
                    public List<PixelShaderConstantDefaults> ConstantRegisterDefaults;
                    public byte[] CompiledShader;
                    public byte[] CompiledShader3;
                    public byte[] CompiledShader4;
                    
                    [TagStructure(Size = 0x4)]
                    public class PixelShaderConstantDefaults : TagStructure
                    {
                        public int Defaults;
                    }
                }
                
                [TagStructure(Size = 0x2)]
                public class PixelShaderExternMap : TagStructure
                {
                    public sbyte SwitchParameter;
                    public sbyte CaseScalar;
                }
                
                [TagStructure(Size = 0x1)]
                public class PixelShaderIndex : TagStructure
                {
                    public sbyte PixelShaderIndexVal;
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class ShaderPassPostprocessTextureNew : TagStructure
            {
                public sbyte BitmapParameterIndex;
                public sbyte BitmapExternIndex;
                public sbyte TextureStageIndex;
                public sbyte Flags;
            }
            
            [TagStructure(Size = 0x5)]
            public class RenderState : TagStructure
            {
                public sbyte StateIndex;
                public int StateValue;
            }
            
            [TagStructure(Size = 0x18)]
            public class ShaderPassPostprocessTextureState : TagStructure
            {
                [TagField(Flags = Padding, Length = 24)]
                public byte[] Unknown1;
            }
            
            [TagStructure(Size = 0x3)]
            public class PixelShaderFragment : TagStructure
            {
                public sbyte SwitchParameterIndex;
                public TagBlockIndex PermutationsIndex;
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndex : TagStructure
                {
                    public short BlockIndexData;
                }
            }
            
            [TagStructure(Size = 0x6)]
            public class PixelShaderPermutationNew : TagStructure
            {
                public short EnumIndex;
                public short Flags;
                public TagBlockIndex Combiners;
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndex : TagStructure
                {
                    public short BlockIndexData;
                }
            }
            
            [TagStructure(Size = 0x20)]
            public class PixelShaderCombiner : TagStructure
            {
                [TagField(Flags = Padding, Length = 16)]
                public byte[] Padding1;
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
            public class ShaderPassPostprocessExternNew : TagStructure
            {
                [TagField(Flags = Padding, Length = 3)]
                public byte[] Unknown1;
                public sbyte ExternIndex;
            }
            
            [TagStructure(Size = 0x7)]
            public class ShaderPassPostprocessConstantNew : TagStructure
            {
                public StringId ParameterName;
                public sbyte ComponentMask;
                public sbyte ScaleByTextureStage;
                public sbyte FunctionIndex;
            }
            
            [TagStructure(Size = 0x7)]
            public class ShaderPassPostprocessConstantInfoNew : TagStructure
            {
                public StringId ParameterName;
                [TagField(Flags = Padding, Length = 3)]
                public byte[] Padding1;
            }
            
            [TagStructure(Size = 0x1F6)]
            public class ShaderPassPostprocessImplementation : TagStructure
            {
                public ShaderGpuState GpuState;
                public ShaderGpuStateReference GpuConstantState;
                public ShaderGpuStateReference GpuVolatileState;
                public ShaderGpuStateReference GpuDefaultState;
                public CachedTag VertexShader;
                [TagField(Flags = Padding, Length = 8)]
                public byte[] Unknown1;
                [TagField(Flags = Padding, Length = 8)]
                public byte[] Unknown2;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown3;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown4;
                public List<ExternReference> ValueExterns;
                public List<ExternReference> ColorExterns;
                public List<ExternReference> SwitchExterns;
                public short BitmapParameterCount;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                [TagField(Flags = Padding, Length = 240)]
                public byte[] Unknown5;
                public List<PixelShaderFragment> PixelShaderFragments;
                public List<PixelShaderPermutation> PixelShaderPermutations;
                public List<PixelShaderCombiner> PixelShaderCombiners;
                public List<PixelShaderConstant> PixelShaderConstants;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown6;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown7;
                
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
                public class ExternReference : TagStructure
                {
                    public sbyte ParameterIndex;
                    public sbyte ExternIndex;
                }
                
                [TagStructure(Size = 0x3)]
                public class PixelShaderFragment : TagStructure
                {
                    public sbyte SwitchParameterIndex;
                    public TagBlockIndex PermutationsIndex;
                    
                    [TagStructure(Size = 0x2)]
                    public class TagBlockIndex : TagStructure
                    {
                        public short BlockIndexData;
                    }
                }
                
                [TagStructure(Size = 0x10)]
                public class PixelShaderPermutation : TagStructure
                {
                    public short EnumIndex;
                    public FlagsValue Flags;
                    public TagBlockIndex Constants;
                    public TagBlockIndex Combiners;
                    [TagField(Flags = Padding, Length = 4)]
                    public byte[] Unknown1;
                    [TagField(Flags = Padding, Length = 4)]
                    public byte[] Unknown2;
                    
                    [Flags]
                    public enum FlagsValue : ushort
                    {
                        HasFinalCombiner = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class TagBlockIndex : TagStructure
                    {
                        public short BlockIndexData;
                    }
                }
                
                [TagStructure(Size = 0x20)]
                public class PixelShaderCombiner : TagStructure
                {
                    [TagField(Flags = Padding, Length = 16)]
                    public byte[] Padding1;
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
                public class PixelShaderConstant : TagStructure
                {
                    public ParameterTypeValue ParameterType;
                    public sbyte CombinerIndex;
                    public sbyte RegisterIndex;
                    public ComponentMaskValue ComponentMask;
                    [TagField(Flags = Padding, Length = 1)]
                    public byte[] Padding1;
                    [TagField(Flags = Padding, Length = 1)]
                    public byte[] Padding2;
                    
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

