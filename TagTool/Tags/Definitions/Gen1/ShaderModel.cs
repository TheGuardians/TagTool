using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "shader_model", Tag = "soso", Size = 0x1B8)]
    public class ShaderModel : TagStructure
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
        /// <summary>
        /// Setting true atmospheric fog enables per-pixel atmospheric fog but disables point/spot lights, planar fog, and the
        /// ability to control the atmospheric fog density for this shader.
        /// </summary>
        public Flags2Value Flags2;
        [TagField(Length = 0x2)]
        public byte[] Padding2;
        [TagField(Length = 0xC)]
        public byte[] Padding3;
        /// <summary>
        /// amount of light that can illuminate the shader from behind
        /// </summary>
        public float Translucency;
        [TagField(Length = 0x10)]
        public byte[] Padding4;
        /// <summary>
        /// Change color is used to recolor the diffuse map, it affects pixels based on the BLUE channel of the multipurpose map.
        /// </summary>
        public ChangeColorSourceValue ChangeColorSource;
        [TagField(Length = 0x2)]
        public byte[] Padding5;
        [TagField(Length = 0x1C)]
        public byte[] Padding6;
        /// <summary>
        /// Self-illumination adds diffuse light to pixels based on the GREEN channel of the multipurpose map. The external
        /// self-illumination color referenced by color source is modulated by the self-illumination animation.
        /// </summary>
        public Flags3Value Flags3;
        [TagField(Length = 0x2)]
        public byte[] Padding7;
        public ColorSourceValue ColorSource;
        public AnimationFunctionValue AnimationFunction;
        public float AnimationPeriod; // seconds
        public RealRgbColor AnimationColorLowerBound;
        public RealRgbColor AnimationColorUpperBound;
        [TagField(Length = 0xC)]
        public byte[] Padding8;
        /// <summary>
        /// Base map alpha is used for alpha-testing.
        /// 
        /// Multipurpose map is used for the following:
        /// * RED: specular reflection mask
        /// (modulates reflections)
        /// * GREEN: self-illumination mask (adds to diffuse light)
        /// * BLUE: primary change color mask
        /// (recolors diffuse map)
        /// * ALPHA: auxiliary mask
        /// 
        /// Note that when DXT1 compressed color-key textures are used for the
        /// multipurpose map (as they should be normally), the alpha channel is 1-bit and any non-zero alpha pixels must have
        /// zero-color, therefore the secondary change color mask cannot affect pixels already affected by any of the other
        /// channels.
        /// 
        /// Detail map affects diffuse map, and optionally affects reflection if detail after reflection flag is set.
        /// </summary>
        /// <summary>
        /// 0 defaults to 1; scales all 2D maps simultaneously
        /// </summary>
        public float MapUScale;
        /// <summary>
        /// 0 defaults to 1; scales all 2D maps simultaneously
        /// </summary>
        public float MapVScale;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag BaseMap;
        [TagField(Length = 0x8)]
        public byte[] Padding9;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag MultipurposeMap;
        [TagField(Length = 0x8)]
        public byte[] Padding10;
        /// <summary>
        /// controls how detail map is applied to diffuse map
        /// </summary>
        public DetailFunctionValue DetailFunction;
        /// <summary>
        /// controls how detail map is masked
        /// </summary>
        public DetailMaskValue DetailMask;
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public float DetailMapScale;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag DetailMap;
        /// <summary>
        /// 0 defaults to 1 (applied on top of detail map scale above)
        /// </summary>
        public float DetailMapVScale;
        [TagField(Length = 0xC)]
        public byte[] Padding11;
        /// <summary>
        /// Scrolls all 2D maps simultaneously.
        /// </summary>
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
        [TagField(Length = 0x8)]
        public byte[] Padding12;
        /// <summary>
        /// distance at which the reflection begins to fade out
        /// </summary>
        public float ReflectionFalloffDistance; // world units
        /// <summary>
        /// distance at which the reflection fades out entirely (0 means no cutoff)
        /// </summary>
        public float ReflectionCutoffDistance; // world units
        /// <summary>
        /// reflection brightness when viewed perpendicularly
        /// </summary>
        public float PerpendicularBrightness; // [0,1]
        /// <summary>
        /// reflection tint color when viewed perpendicularly
        /// </summary>
        public RealRgbColor PerpendicularTintColor;
        /// <summary>
        /// reflection brightness when viewed at a glancing angle
        /// </summary>
        public float ParallelBrightness; // [0,1]
        /// <summary>
        /// reflection tint color when viewed at a glancing angle
        /// </summary>
        public RealRgbColor ParallelTintColor;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag ReflectionCubeMap;
        [TagField(Length = 0x10)]
        public byte[] Padding13;
        [TagField(Length = 0x4)]
        public byte[] Padding14;
        [TagField(Length = 0x10)]
        public byte[] Padding15;
        [TagField(Length = 0x20)]
        public byte[] Padding16;
        
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
        
        public enum Flags2Value : ushort
        {
            DetailAfterReflection,
            TwoSided,
            NotAlphaTested,
            AlphaBlendedDecal,
            TrueAtmosphericFog,
            DisableTwoSidedCulling
        }
        
        public enum ChangeColorSourceValue : short
        {
            None,
            A,
            B,
            C,
            D
        }
        
        public enum Flags3Value : ushort
        {
            NoRandomPhase
        }
        
        public enum ColorSourceValue : short
        {
            None,
            A,
            B,
            C,
            D
        }
        
        public enum AnimationFunctionValue : short
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
        
        public enum DetailFunctionValue : short
        {
            DoubleBiasedMultiply,
            Multiply,
            DoubleBiasedAdd
        }
        
        public enum DetailMaskValue : short
        {
            None,
            ReflectionMaskInverse,
            ReflectionMask,
            SelfIlluminationMaskInverse,
            SelfIlluminationMask,
            ChangeColorMaskInverse,
            ChangeColorMask,
            MultipurposeMapAlphaInverse,
            MultipurposeMapAlpha
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
}

