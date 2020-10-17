using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "shader_environment", Tag = "senv", Size = 0x344)]
    public class ShaderEnvironment : TagStructure
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
        /// Setting true atmospheric fog enables per-pixel atmospheric fog (for models) but disables point/spot lights, planar fog,
        /// and the ability to control the atmospheric fog density for this shader.
        /// </summary>
        public Flags2Value Flags2;
        /// <summary>
        /// Controls how diffuse maps are combined:
        /// 
        /// NORMAL:
        /// Secondary detail map alpha controls blend between primary and secondary
        /// detail map. Specular mask is alpha of blended primary and secondary detail map alpha multiplied by alpha of micro detail
        /// map.
        /// 
        /// BLENDED:
        /// Base map alpha controls blend between primary and secondary detail map. Specular mask is alpha of blended
        /// primary and secondary detail map alpha multiplied by alpha of micro detail map.
        /// 
        /// BLENDED BASE SPECULAR:
        /// Same as BLENDED,
        /// except specular mask is alpha is base map times alpha of micro detail map.
        /// </summary>
        public TypeValue Type;
        /// <summary>
        /// 0 places a single lens flare
        /// </summary>
        public float LensFlareSpacing; // world units
        [TagField(ValidTags = new [] { "lens" })]
        public CachedTag LensFlare;
        [TagField(Length = 0x2C)]
        public byte[] Padding2;
        public Flags3Value Flags3;
        [TagField(Length = 0x2)]
        public byte[] Padding3;
        [TagField(Length = 0x18)]
        public byte[] Padding4;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag BaseMap;
        [TagField(Length = 0x18)]
        public byte[] Padding5;
        /// <summary>
        /// affects primary and secondary detail maps
        /// </summary>
        public DetailMapFunctionValue DetailMapFunction;
        [TagField(Length = 0x2)]
        public byte[] Padding6;
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public float PrimaryDetailMapScale;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag PrimaryDetailMap;
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public float SecondaryDetailMapScale;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag SecondaryDetailMap;
        [TagField(Length = 0x18)]
        public byte[] Padding7;
        public MicroDetailMapFunctionValue MicroDetailMapFunction;
        [TagField(Length = 0x2)]
        public byte[] Padding8;
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public float MicroDetailMapScale;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag MicroDetailMap;
        /// <summary>
        /// modulates incoming diffuse light, including lightmaps, but excluding self-illumination and specular effects
        /// </summary>
        public RealRgbColor MaterialColor;
        [TagField(Length = 0xC)]
        public byte[] Padding9;
        /// <summary>
        /// Perforated (alpha-tested) shaders use alpha in bump map.
        /// </summary>
        public float BumpMapScale;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag BumpMap;
        [TagField(Length = 0x8)]
        public byte[] Padding10;
        [TagField(Length = 0x10)]
        public byte[] Padding11;
        /// <summary>
        /// Scrolls all 2D maps simultaneously.
        /// </summary>
        public UAnimationFunctionValue UAnimationFunction;
        [TagField(Length = 0x2)]
        public byte[] Padding12;
        public float UAnimationPeriod; // seconds
        public float UAnimationScale; // base map repeats
        public VAnimationFunctionValue VAnimationFunction;
        [TagField(Length = 0x2)]
        public byte[] Padding13;
        public float VAnimationPeriod; // seconds
        public float VAnimationScale; // base map repeats
        [TagField(Length = 0x18)]
        public byte[] Padding14;
        /// <summary>
        /// There are three self-illumination effects which are added together. Each effect has an on color, used when the shader
        /// is active, and an off color, used when the shader is not active. The self-illumination map is used as follows:
        /// * RED:
        /// primary mask
        /// * GREEN: secondary mask
        /// * BLUE: plasma mask
        /// * ALPHA: plasma animation reference
        /// 
        /// Each effect also has an
        /// animation function, period and phase, used when the shader is active. The primary and secondary effects simply
        /// modulate the on color by the animation value to produce an animation color, and then blend between the animation color
        /// and the off color based on the shader's activation level, and finally modulate by the mask.
        /// 
        /// The plasma shader compares
        /// the animation value with the alpha channel of the map (the plasma animation reference) and produces a high value when
        /// they are similar and a dark value when they are different. This value modulates the plasma on color to produce a plasma
        /// animation color, and the rest proceeds just like the primary and secondary effects.
        /// </summary>
        public Flags4Value Flags4;
        [TagField(Length = 0x2)]
        public byte[] Padding15;
        [TagField(Length = 0x18)]
        public byte[] Padding16;
        public RealRgbColor PrimaryOnColor;
        public RealRgbColor PrimaryOffColor;
        public PrimaryAnimationFunctionValue PrimaryAnimationFunction;
        [TagField(Length = 0x2)]
        public byte[] Padding17;
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public float PrimaryAnimationPeriod; // seconds
        public float PrimaryAnimationPhase; // seconds
        [TagField(Length = 0x18)]
        public byte[] Padding18;
        public RealRgbColor SecondaryOnColor;
        public RealRgbColor SecondaryOffColor;
        public SecondaryAnimationFunctionValue SecondaryAnimationFunction;
        [TagField(Length = 0x2)]
        public byte[] Padding19;
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public float SecondaryAnimationPeriod; // seconds
        public float SecondaryAnimationPhase; // seconds
        [TagField(Length = 0x18)]
        public byte[] Padding20;
        public RealRgbColor PlasmaOnColor;
        public RealRgbColor PlasmaOffColor;
        public PlasmaAnimationFunctionValue PlasmaAnimationFunction;
        [TagField(Length = 0x2)]
        public byte[] Padding21;
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public float PlasmaAnimationPeriod; // seconds
        public float PlasmaAnimationPhase; // seconds
        [TagField(Length = 0x18)]
        public byte[] Padding22;
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public float MapScale;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Map;
        [TagField(Length = 0x18)]
        public byte[] Padding23;
        /// <summary>
        /// Controls dynamic specular highlights. The highlight is modulated by brightness as well as a blend between
        /// perpendicular color and parallel color.
        /// 
        /// Set brightness to zero to disable.
        /// </summary>
        public Flags5Value Flags5;
        [TagField(Length = 0x2)]
        public byte[] Padding24;
        [TagField(Length = 0x10)]
        public byte[] Padding25;
        /// <summary>
        /// 0 is no specular hilights
        /// </summary>
        public float Brightness; // [0,1]
        [TagField(Length = 0x14)]
        public byte[] Padding26;
        /// <summary>
        /// hilight color when viewed perpendicularly
        /// </summary>
        public RealRgbColor PerpendicularColor;
        /// <summary>
        /// hilight color when viewed at a glancing angle
        /// </summary>
        public RealRgbColor ParallelColor;
        [TagField(Length = 0x10)]
        public byte[] Padding27;
        /// <summary>
        /// Controls environment cube map reflections. The color of the cube map is "tinted" by a blend between perpendicular color
        /// and parallel color from the SPECULAR PROPERTIES above, and then modulated by a blend between perpendicular brightness
        /// and parallel brightness.
        /// 
        /// BUMPED CUBE MAP:
        /// This type of reflection uses the shader's bump map (if it exists) to affect
        /// the reflection, as well as the perpendicular and parallel brightness (i.e. the "fresnel" effect).
        /// 
        /// FLAT CUBE MAP:
        /// This is
        /// the fastest type of reflection. The bump map is used to attenuate the fresnel effect, but the reflection image itself is
        /// not bumped.
        /// 
        /// Clear reflection cube map or set both perpendicular brightness and parallel brightness to zero to
        /// disable.
        /// </summary>
        public Flags6Value Flags6;
        public Type1Value Type1;
        /// <summary>
        /// reflection brightness when lightmap brightness is 1
        /// </summary>
        public float LightmapBrightnessScale; // [0,1]
        [TagField(Length = 0x1C)]
        public byte[] Padding28;
        /// <summary>
        /// brightness when viewed perpendicularly
        /// </summary>
        public float PerpendicularBrightness; // [0,1]
        /// <summary>
        /// brightness when viewed at a glancing angle
        /// </summary>
        public float ParallelBrightness; // [0,1]
        [TagField(Length = 0x10)]
        public byte[] Padding29;
        [TagField(Length = 0x8)]
        public byte[] Padding30;
        [TagField(Length = 0x10)]
        public byte[] Padding31;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag ReflectionCubeMap;
        [TagField(Length = 0x10)]
        public byte[] Padding32;
        
        [Flags]
        public enum FlagsValue : ushort
        {
            /// <summary>
            /// lightmap texture parametrization should correspond to diffuse texture parametrization
            /// </summary>
            SimpleParameterization = 1 << 0,
            /// <summary>
            /// light independent of normals (trees)
            /// </summary>
            IgnoreNormals = 1 << 1,
            TransparentLit = 1 << 2
        }
        
        public enum DetailLevelValue : short
        {
            High,
            Medium,
            Low,
            Turd
        }
        
        [Flags]
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
        
        [Flags]
        public enum Flags2Value : ushort
        {
            AlphaTested = 1 << 0,
            BumpMapIsSpecularMask = 1 << 1,
            TrueAtmosphericFog = 1 << 2
        }
        
        public enum TypeValue : short
        {
            Normal,
            Blended,
            BlendedBaseSpecular
        }
        
        [Flags]
        public enum Flags3Value : ushort
        {
            RescaleDetailMaps = 1 << 0,
            RescaleBumpMap = 1 << 1
        }
        
        public enum DetailMapFunctionValue : short
        {
            DoubleBiasedMultiply,
            Multiply,
            DoubleBiasedAdd
        }
        
        public enum MicroDetailMapFunctionValue : short
        {
            DoubleBiasedMultiply,
            Multiply,
            DoubleBiasedAdd
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
        
        [Flags]
        public enum Flags4Value : ushort
        {
            Unfiltered = 1 << 0
        }
        
        public enum PrimaryAnimationFunctionValue : short
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
        
        public enum SecondaryAnimationFunctionValue : short
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
        
        public enum PlasmaAnimationFunctionValue : short
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
        
        [Flags]
        public enum Flags5Value : ushort
        {
            Overbright = 1 << 0,
            ExtraShiny = 1 << 1,
            LightmapIsSpecular = 1 << 2
        }
        
        [Flags]
        public enum Flags6Value : ushort
        {
            DynamicMirror = 1 << 0
        }
        
        public enum Type1Value : short
        {
            BumpedCubeMap,
            FlatCubeMap,
            BumpedRadiosity
        }
    }
}

