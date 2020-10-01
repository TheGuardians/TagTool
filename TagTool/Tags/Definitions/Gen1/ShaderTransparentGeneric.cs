using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "shader_transparent_generic", Tag = "sotr", Size = 0x6C)]
    public class ShaderTransparentGeneric : TagStructure
    {
        public FlagsValue Flags;
        /// <summary>
        /// affects the density of tesselation (high means slow).
        /// </summary>
        public DetailLevelValue DetailLevel;
        /// <summary>
        /// power of emitted light from 0 to infinity
        /// </summary>
        public float Power;
        public RealRgbColor ColorOfEmittedLight;
        /// <summary>
        /// light passing through this surface (if it's transparent) will be tinted this color.
        /// </summary>
        public RealRgbColor TintColor;
        public Flags1Value Flags1;
        public MaterialTypeValue MaterialType;
        [TagField(Length = 0x2)]
        public byte[] Padding;
        [TagField(Length = 0x2)]
        public byte[] Padding1;
        public sbyte NumericCounterLimit; // [0,255]
        public Flags2Value Flags2;
        public FirstMapTypeValue FirstMapType;
        public FramebufferBlendFunctionValue FramebufferBlendFunction;
        public FramebufferFadeModeValue FramebufferFadeMode;
        /// <summary>
        /// fade is multiplied by this external value
        /// </summary>
        public FramebufferFadeSourceValue FramebufferFadeSource;
        [TagField(Length = 0x2)]
        public byte[] Padding2;
        /// <summary>
        /// 0 places a single lens flare
        /// </summary>
        public float LensFlareSpacing; // world units
        [TagField(ValidTags = new [] { "lens" })]
        public CachedTag LensFlare;
        public List<ShaderTransparentLayerBlock> ExtraLayers;
        public List<ShaderTransparentGenericMapBlock> Maps;
        public List<ShaderTransparentGenericStageBlock> Stages;
        
        public enum FlagsValue : ushort
        {
            /// <summary>
            /// lightmap texture parametrization should correspond to diffuse texture parametrization
            /// </summary>
            SimpleParameterization,
            /// <summary>
            /// light independent of normals (trees)
            /// </summary>
            IgnoreNormals,
            TransparentLit
        }
        
        public enum DetailLevelValue : short
        {
            High,
            Medium,
            Low,
            Turd
        }
        
        public enum Flags1Value : ushort
        {
        }
        
        public enum MaterialTypeValue : short
        {
            Dirt,
            Sand,
            Stone,
            Snow,
            Wood,
            MetalHollow,
            MetalThin,
            MetalThick,
            Rubber,
            Glass,
            ForceField,
            Grunt,
            HunterArmor,
            HunterSkin,
            Elite,
            Jackal,
            JackalEnergyShield,
            EngineerSkin,
            EngineerForceField,
            FloodCombatForm,
            FloodCarrierForm,
            CyborgArmor,
            CyborgEnergyShield,
            HumanArmor,
            HumanSkin,
            Sentinel,
            Monitor,
            Plastic,
            Water,
            Leaves,
            EliteEnergyShield,
            Ice,
            HunterShield
        }
        
        public enum Flags2Value : byte
        {
            AlphaTested,
            Decal,
            TwoSided,
            FirstMapIsInScreenspace,
            DrawBeforeWater,
            IgnoreEffect,
            ScaleFirstMapWithDistance,
            Numeric
        }
        
        public enum FirstMapTypeValue : short
        {
            _2dMap,
            ReflectionCubeMap,
            ObjectCenteredCubeMap,
            ViewerCenteredCubeMap
        }
        
        public enum FramebufferBlendFunctionValue : short
        {
            AlphaBlend,
            Multiply,
            DoubleMultiply,
            Add,
            Subtract,
            ComponentMin,
            ComponentMax,
            AlphaMultiplyAdd
        }
        
        public enum FramebufferFadeModeValue : short
        {
            None,
            FadeWhenPerpendicular,
            FadeWhenParallel
        }
        
        public enum FramebufferFadeSourceValue : short
        {
            None,
            AOut,
            BOut,
            COut,
            DOut
        }
        
        [TagStructure(Size = 0x10)]
        public class ShaderTransparentLayerBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "shdr" })]
            public CachedTag Shader;
        }
        
        [TagStructure(Size = 0x64)]
        public class ShaderTransparentGenericMapBlock : TagStructure
        {
            public FlagsValue Flags;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            /// <summary>
            /// 0 defaults to 1
            /// </summary>
            public float MapUScale;
            /// <summary>
            /// 0 defaults to 1
            /// </summary>
            public float MapVScale;
            public float MapUOffset;
            public float MapVOffset;
            public float MapRotation; // degrees
            public float MipmapBias; // [0,1]
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Map;
            public UAnimationSourceValue UAnimationSource;
            public UAnimationFunctionValue UAnimationFunction;
            /// <summary>
            /// 0 defaults to 1
            /// </summary>
            public float UAnimationPeriod; // seconds
            public float UAnimationPhase;
            /// <summary>
            /// 0 defaults to 1
            /// </summary>
            public float UAnimationScale; // repeats
            public VAnimationSourceValue VAnimationSource;
            public VAnimationFunctionValue VAnimationFunction;
            /// <summary>
            /// 0 defaults to 1
            /// </summary>
            public float VAnimationPeriod; // seconds
            public float VAnimationPhase;
            /// <summary>
            /// 0 defaults to 1
            /// </summary>
            public float VAnimationScale; // repeats
            public RotationAnimationSourceValue RotationAnimationSource;
            public RotationAnimationFunctionValue RotationAnimationFunction;
            /// <summary>
            /// 0 defaults to 1
            /// </summary>
            public float RotationAnimationPeriod; // seconds
            public float RotationAnimationPhase;
            /// <summary>
            /// 0 defaults to 360
            /// </summary>
            public float RotationAnimationScale; // degrees
            public RealPoint2d RotationAnimationCenter;
            
            public enum FlagsValue : ushort
            {
                Unfiltered,
                UClamped,
                VClamped
            }
            
            public enum UAnimationSourceValue : short
            {
                None,
                AOut,
                BOut,
                COut,
                DOut
            }
            
            public enum UAnimationFunctionValue : short
            {
                One,
                Zero,
                Cosine,
                CosineVariablePeriod,
                DiagonalWave,
                DiagonalWaveVariablePeriod,
                Slide,
                SlideVariablePeriod,
                Noise,
                Jitter,
                Wander,
                Spark
            }
            
            public enum VAnimationSourceValue : short
            {
                None,
                AOut,
                BOut,
                COut,
                DOut
            }
            
            public enum VAnimationFunctionValue : short
            {
                One,
                Zero,
                Cosine,
                CosineVariablePeriod,
                DiagonalWave,
                DiagonalWaveVariablePeriod,
                Slide,
                SlideVariablePeriod,
                Noise,
                Jitter,
                Wander,
                Spark
            }
            
            public enum RotationAnimationSourceValue : short
            {
                None,
                AOut,
                BOut,
                COut,
                DOut
            }
            
            public enum RotationAnimationFunctionValue : short
            {
                One,
                Zero,
                Cosine,
                CosineVariablePeriod,
                DiagonalWave,
                DiagonalWaveVariablePeriod,
                Slide,
                SlideVariablePeriod,
                Noise,
                Jitter,
                Wander,
                Spark
            }
        }
        
        [TagStructure(Size = 0x70)]
        public class ShaderTransparentGenericStageBlock : TagStructure
        {
            public FlagsValue Flags;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            /// <summary>
            /// Constant color 0 is animated in exactly the same way as the self-illumination color of the model shader, except that it
            /// has an alpha component in addition to the RGB components. Constant color 1 is just a constant.
            /// </summary>
            public Color0SourceValue Color0Source;
            public Color0AnimationFunctionValue Color0AnimationFunction;
            /// <summary>
            /// 0 defaults to 1
            /// </summary>
            public float Color0AnimationPeriod; // seconds
            public RealArgbColor Color0AnimationLowerBound;
            public RealArgbColor Color0AnimationUpperBound;
            public RealArgbColor Color1;
            public InputAValue InputA;
            public InputAMappingValue InputAMapping;
            public InputBValue InputB;
            public InputBMappingValue InputBMapping;
            public InputCValue InputC;
            public InputCMappingValue InputCMapping;
            public InputDValue InputD;
            public InputDMappingValue InputDMapping;
            public OutputAbValue OutputAb;
            public OutputAbFunctionValue OutputAbFunction;
            public OutputCdValue OutputCd;
            public OutputCdFunctionValue OutputCdFunction;
            public OutputAbCdMuxSumValue OutputAbCdMuxSum;
            public OutputMappingValue OutputMapping;
            public InputA1Value InputA1;
            public InputAMapping1Value InputAMapping1;
            public InputB1Value InputB1;
            public InputBMapping1Value InputBMapping1;
            public InputC1Value InputC1;
            public InputCMapping1Value InputCMapping1;
            public InputD1Value InputD1;
            public InputDMapping1Value InputDMapping1;
            public OutputAb1Value OutputAb1;
            public OutputCd1Value OutputCd1;
            public OutputAbCdMuxSum1Value OutputAbCdMuxSum1;
            public OutputMapping1Value OutputMapping1;
            
            public enum FlagsValue : ushort
            {
                ColorMux,
                AlphaMux,
                AOutControlsColor0Animation
            }
            
            public enum Color0SourceValue : short
            {
                None,
                A,
                B,
                C,
                D
            }
            
            public enum Color0AnimationFunctionValue : short
            {
                One,
                Zero,
                Cosine,
                CosineVariablePeriod,
                DiagonalWave,
                DiagonalWaveVariablePeriod,
                Slide,
                SlideVariablePeriod,
                Noise,
                Jitter,
                Wander,
                Spark
            }
            
            public enum InputAValue : short
            {
                Zero,
                One,
                OneHalf,
                NegativeOne,
                NegativeOneHalf,
                MapColor0,
                MapColor1,
                MapColor2,
                MapColor3,
                VertexColor0DiffuseLight,
                VertexColor1FadePerpendicular,
                ScratchColor0,
                ScratchColor1,
                ConstantColor0,
                ConstantColor1,
                MapAlpha0,
                MapAlpha1,
                MapAlpha2,
                MapAlpha3,
                VertexAlpha0FadeNone,
                VertexAlpha1FadePerpendicular,
                ScratchAlpha0,
                ScratchAlpha1,
                ConstantAlpha0,
                ConstantAlpha1
            }
            
            public enum InputAMappingValue : short
            {
                ClampX,
                _1ClampX,
                _2ClampX1,
                _12ClampX,
                ClampX12,
                _12ClampX1,
                X,
                X1
            }
            
            public enum InputBValue : short
            {
                Zero,
                One,
                OneHalf,
                NegativeOne,
                NegativeOneHalf,
                MapColor0,
                MapColor1,
                MapColor2,
                MapColor3,
                VertexColor0DiffuseLight,
                VertexColor1FadePerpendicular,
                ScratchColor0,
                ScratchColor1,
                ConstantColor0,
                ConstantColor1,
                MapAlpha0,
                MapAlpha1,
                MapAlpha2,
                MapAlpha3,
                VertexAlpha0FadeNone,
                VertexAlpha1FadePerpendicular,
                ScratchAlpha0,
                ScratchAlpha1,
                ConstantAlpha0,
                ConstantAlpha1
            }
            
            public enum InputBMappingValue : short
            {
                ClampX,
                _1ClampX,
                _2ClampX1,
                _12ClampX,
                ClampX12,
                _12ClampX1,
                X,
                X1
            }
            
            public enum InputCValue : short
            {
                Zero,
                One,
                OneHalf,
                NegativeOne,
                NegativeOneHalf,
                MapColor0,
                MapColor1,
                MapColor2,
                MapColor3,
                VertexColor0DiffuseLight,
                VertexColor1FadePerpendicular,
                ScratchColor0,
                ScratchColor1,
                ConstantColor0,
                ConstantColor1,
                MapAlpha0,
                MapAlpha1,
                MapAlpha2,
                MapAlpha3,
                VertexAlpha0FadeNone,
                VertexAlpha1FadePerpendicular,
                ScratchAlpha0,
                ScratchAlpha1,
                ConstantAlpha0,
                ConstantAlpha1
            }
            
            public enum InputCMappingValue : short
            {
                ClampX,
                _1ClampX,
                _2ClampX1,
                _12ClampX,
                ClampX12,
                _12ClampX1,
                X,
                X1
            }
            
            public enum InputDValue : short
            {
                Zero,
                One,
                OneHalf,
                NegativeOne,
                NegativeOneHalf,
                MapColor0,
                MapColor1,
                MapColor2,
                MapColor3,
                VertexColor0DiffuseLight,
                VertexColor1FadePerpendicular,
                ScratchColor0,
                ScratchColor1,
                ConstantColor0,
                ConstantColor1,
                MapAlpha0,
                MapAlpha1,
                MapAlpha2,
                MapAlpha3,
                VertexAlpha0FadeNone,
                VertexAlpha1FadePerpendicular,
                ScratchAlpha0,
                ScratchAlpha1,
                ConstantAlpha0,
                ConstantAlpha1
            }
            
            public enum InputDMappingValue : short
            {
                ClampX,
                _1ClampX,
                _2ClampX1,
                _12ClampX,
                ClampX12,
                _12ClampX1,
                X,
                X1
            }
            
            public enum OutputAbValue : short
            {
                Discard,
                ScratchColor0FinalColor,
                ScratchColor1,
                VertexColor0,
                VertexColor1,
                MapColor0,
                MapColor1,
                MapColor2,
                MapColor3
            }
            
            public enum OutputAbFunctionValue : short
            {
                Multiply,
                DotProduct
            }
            
            public enum OutputCdValue : short
            {
                Discard,
                ScratchColor0FinalColor,
                ScratchColor1,
                VertexColor0,
                VertexColor1,
                MapColor0,
                MapColor1,
                MapColor2,
                MapColor3
            }
            
            public enum OutputCdFunctionValue : short
            {
                Multiply,
                DotProduct
            }
            
            public enum OutputAbCdMuxSumValue : short
            {
                Discard,
                ScratchColor0FinalColor,
                ScratchColor1,
                VertexColor0,
                VertexColor1,
                MapColor0,
                MapColor1,
                MapColor2,
                MapColor3
            }
            
            public enum OutputMappingValue : short
            {
                Identity,
                ScaleBy12,
                ScaleBy2,
                ScaleBy4,
                BiasBy12,
                ExpandNormal
            }
            
            public enum InputA1Value : short
            {
                Zero,
                One,
                OneHalf,
                NegativeOne,
                NegativeOneHalf,
                MapAlpha0,
                MapAlpha1,
                MapAlpha2,
                MapAlpha3,
                VertexAlpha0FadeNone,
                VertexAlpha1FadePerpendicular,
                ScratchAlpha0,
                ScratchAlpha1,
                ConstantAlpha0,
                ConstantAlpha1,
                MapBlue0,
                MapBlue1,
                MapBlue2,
                MapBlue3,
                VertexBlue0BlueLight,
                VertexBlue1FadeParallel,
                ScratchBlue0,
                ScratchBlue1,
                ConstantBlue0,
                ConstantBlue1
            }
            
            public enum InputAMapping1Value : short
            {
                ClampX,
                _1ClampX,
                _2ClampX1,
                _12ClampX,
                ClampX12,
                _12ClampX1,
                X,
                X1
            }
            
            public enum InputB1Value : short
            {
                Zero,
                One,
                OneHalf,
                NegativeOne,
                NegativeOneHalf,
                MapAlpha0,
                MapAlpha1,
                MapAlpha2,
                MapAlpha3,
                VertexAlpha0FadeNone,
                VertexAlpha1FadePerpendicular,
                ScratchAlpha0,
                ScratchAlpha1,
                ConstantAlpha0,
                ConstantAlpha1,
                MapBlue0,
                MapBlue1,
                MapBlue2,
                MapBlue3,
                VertexBlue0BlueLight,
                VertexBlue1FadeParallel,
                ScratchBlue0,
                ScratchBlue1,
                ConstantBlue0,
                ConstantBlue1
            }
            
            public enum InputBMapping1Value : short
            {
                ClampX,
                _1ClampX,
                _2ClampX1,
                _12ClampX,
                ClampX12,
                _12ClampX1,
                X,
                X1
            }
            
            public enum InputC1Value : short
            {
                Zero,
                One,
                OneHalf,
                NegativeOne,
                NegativeOneHalf,
                MapAlpha0,
                MapAlpha1,
                MapAlpha2,
                MapAlpha3,
                VertexAlpha0FadeNone,
                VertexAlpha1FadePerpendicular,
                ScratchAlpha0,
                ScratchAlpha1,
                ConstantAlpha0,
                ConstantAlpha1,
                MapBlue0,
                MapBlue1,
                MapBlue2,
                MapBlue3,
                VertexBlue0BlueLight,
                VertexBlue1FadeParallel,
                ScratchBlue0,
                ScratchBlue1,
                ConstantBlue0,
                ConstantBlue1
            }
            
            public enum InputCMapping1Value : short
            {
                ClampX,
                _1ClampX,
                _2ClampX1,
                _12ClampX,
                ClampX12,
                _12ClampX1,
                X,
                X1
            }
            
            public enum InputD1Value : short
            {
                Zero,
                One,
                OneHalf,
                NegativeOne,
                NegativeOneHalf,
                MapAlpha0,
                MapAlpha1,
                MapAlpha2,
                MapAlpha3,
                VertexAlpha0FadeNone,
                VertexAlpha1FadePerpendicular,
                ScratchAlpha0,
                ScratchAlpha1,
                ConstantAlpha0,
                ConstantAlpha1,
                MapBlue0,
                MapBlue1,
                MapBlue2,
                MapBlue3,
                VertexBlue0BlueLight,
                VertexBlue1FadeParallel,
                ScratchBlue0,
                ScratchBlue1,
                ConstantBlue0,
                ConstantBlue1
            }
            
            public enum InputDMapping1Value : short
            {
                ClampX,
                _1ClampX,
                _2ClampX1,
                _12ClampX,
                ClampX12,
                _12ClampX1,
                X,
                X1
            }
            
            public enum OutputAb1Value : short
            {
                Discard,
                ScratchAlpha0FinalAlpha,
                ScratchAlpha1,
                VertexAlpha0Fog,
                VertexAlpha1,
                MapAlpha0,
                MapAlpha1,
                MapAlpha2,
                MapAlpha3
            }
            
            public enum OutputCd1Value : short
            {
                Discard,
                ScratchAlpha0FinalAlpha,
                ScratchAlpha1,
                VertexAlpha0Fog,
                VertexAlpha1,
                MapAlpha0,
                MapAlpha1,
                MapAlpha2,
                MapAlpha3
            }
            
            public enum OutputAbCdMuxSum1Value : short
            {
                Discard,
                ScratchAlpha0FinalAlpha,
                ScratchAlpha1,
                VertexAlpha0Fog,
                VertexAlpha1,
                MapAlpha0,
                MapAlpha1,
                MapAlpha2,
                MapAlpha3
            }
            
            public enum OutputMapping1Value : short
            {
                Identity,
                ScaleBy12,
                ScaleBy2,
                ScaleBy4,
                BiasBy12,
                ExpandNormal
            }
        }
    }
}

