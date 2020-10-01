using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "shader_transparent_chicago", Tag = "schi", Size = 0x6C)]
    public class ShaderTransparentChicago : TagStructure
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
        public List<ShaderTransparentChicagoMapBlock> Maps;
        public ExtraFlagsValue ExtraFlags;
        [TagField(Length = 0x8)]
        public byte[] Padding3;
        
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
            FirstMapIsReflectionCubeMap,
            FirstMapIsObjectCenteredCubeMap,
            FirstMapIsViewerCenteredCubeMap
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
        
        [TagStructure(Size = 0xDC)]
        public class ShaderTransparentChicagoMapBlock : TagStructure
        {
            public FlagsValue Flags;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            [TagField(Length = 0x28)]
            public byte[] Padding1;
            /// <summary>
            /// ignored for last map
            /// </summary>
            public ColorFunctionValue ColorFunction;
            /// <summary>
            /// ignored for last map
            /// </summary>
            public AlphaFunctionValue AlphaFunction;
            [TagField(Length = 0x24)]
            public byte[] Padding2;
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
            [TagField(Length = 0x28)]
            public byte[] Padding3;
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
                AlphaReplicate,
                UClamped,
                VClamped
            }
            
            public enum ColorFunctionValue : short
            {
                Current,
                NextMap,
                Multiply,
                DoubleMultiply,
                Add,
                AddSignedCurrent,
                AddSignedNextMap,
                SubtractCurrent,
                SubtractNextMap,
                BlendCurrentAlpha,
                BlendCurrentAlphaInverse,
                BlendNextMapAlpha,
                BlendNextMapAlphaInverse
            }
            
            public enum AlphaFunctionValue : short
            {
                Current,
                NextMap,
                Multiply,
                DoubleMultiply,
                Add,
                AddSignedCurrent,
                AddSignedNextMap,
                SubtractCurrent,
                SubtractNextMap,
                BlendCurrentAlpha,
                BlendCurrentAlphaInverse,
                BlendNextMapAlpha,
                BlendNextMapAlphaInverse
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
        
        public enum ExtraFlagsValue : uint
        {
            DonTFadeActiveCamouflage,
            NumericCountdownTimer
        }
    }
}

