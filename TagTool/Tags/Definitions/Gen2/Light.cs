using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "light", Tag = "ligh", Size = 0x110)]
    public class Light : TagStructure
    {
        public FlagsValue Flags;
        /// <summary>
        /// SHAPE
        /// </summary>
        /// <remarks>
        /// overall shape of the light
        /// </remarks>
        public TypeValue Type;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        public Bounds<float> SizeModifer; // how the light's size changes with external scale
        public float ShadowQualityBias; // larger positive numbers improve quality, larger negative numbers improve speed
        public ShadowTapBiasValue ShadowTapBias; // the less taps you use, the faster the light (but edges can look worse)
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding2;
        /// <summary>
        /// SPHERE LIGHT
        /// </summary>
        /// <remarks>
        /// default shape parameters for spherical lights
        /// </remarks>
        public float Radius; // world units
        public float SpecularRadius; // world units
        /// <summary>
        /// FRUSTUM LIGHT
        /// </summary>
        /// <remarks>
        /// default shape parameters for frustum lights (orthogonal, projective or pyramid types)
        /// </remarks>
        public float NearWidth; // world units
        public float HeightStretch; // how much the gel is stretched vertically (0.0 or 1.0 = aspect ratio same as gel)
        public float FieldOfView; // degrees
        public float FalloffDistance; // distance from near plane to where the light falloff starts
        public float CutoffDistance; // distance from near plane to where illumination falls off to zero
        /// <summary>
        /// COLOR
        /// </summary>
        public InterpolationFlagsValue InterpolationFlags;
        public Bounds<float> BloomBounds; // [0..2]
        public RealRgbColor SpecularLowerBound;
        public RealRgbColor SpecularUpperBound;
        public RealRgbColor DiffuseLowerBound;
        public RealRgbColor DiffuseUpperBound;
        public Bounds<float> BrightnessBounds; // [0..2]
        /// <summary>
        /// GEL
        /// </summary>
        /// <remarks>
        /// the gel map tints the light's illumination per-pixel
        /// </remarks>
        public CachedTag GelMap; // must be a cubemap for spherical light and a 2d texture for frustum light
        public SpecularMaskValue SpecularMask;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding3;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding4;
        /// <summary>
        /// FALLOFF
        /// </summary>
        public FalloffFunctionValue FalloffFunction;
        public DiffuseContrastValue DiffuseContrast;
        public SpecularContrastValue SpecularContrast;
        public FalloffGeometryValue FalloffGeometry;
        /// <summary>
        /// LENS FLARE
        /// </summary>
        /// <remarks>
        /// optional lens flare and light volume associated with this light
        /// </remarks>
        public CachedTag LensFlare;
        public float BoundingRadius; // world units
        public CachedTag LightVolume;
        /// <summary>
        /// RADIOSITY
        /// </summary>
        /// <remarks>
        /// how the light affects the lightmaps (ignored for dynamic lights)
        /// </remarks>
        public DefaultLightmapSettingValue DefaultLightmapSetting;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding5;
        public float LightmapHalfLife;
        public float LightmapLightScale;
        /// <summary>
        /// EFFECT PARAMETERS
        /// </summary>
        /// <remarks>
        /// if the light is created by an effect, it will animate itself as follows
        /// </remarks>
        public float Duration; // seconds
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding6;
        public FalloffFunctionValue FalloffFunction1; // the scale of the light will diminish over time according to this function
        /// <summary>
        /// DISTANCE FADING PARAMETERS
        /// </summary>
        /// <remarks>
        /// To fade the light's illumination and shadow casting abilities
        /// </remarks>
        public IlluminationFadeValue IlluminationFade;
        public ShadowFadeValue ShadowFade;
        public SpecularFadeValue SpecularFade;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding7;
        /// <summary>
        /// ANIMATION PARAMETERS
        /// </summary>
        public FlagsValue Flags2;
        public List<LightBrightnessAnimationParameters> BrightnessAnimation;
        public List<LightColorAnimationParameters> ColorAnimation;
        public List<LightGelAnimationParameters> GelAnimation;
        /// <summary>
        /// SHADER
        /// </summary>
        public CachedTag Shader;
        
        [Flags]
        public enum FlagsValue : uint
        {
            NoIllumination = 1 << 0,
            NoSpecular = 1 << 1,
            ForceCastEnvironmentShadowsThroughPortals = 1 << 2,
            NoShadow = 1 << 3,
            ForceFrustumVisibilityOnSmallLight = 1 << 4,
            OnlyRenderInFirstPerson = 1 << 5,
            OnlyRenderInThirdPerson = 1 << 6,
            DonTFadeWhenInvisible = 1 << 7,
            MultiplayerOverride = 1 << 8,
            AnimatedGel = 1 << 9,
            OnlyInDynamicEnvmap = 1 << 10,
            IgnoreParentObject = 1 << 11,
            DonTShadowParent = 1 << 12,
            IgnoreAllParents = 1 << 13,
            MarchMilestoneHack = 1 << 14,
            ForceLightInsideWorld = 1 << 15,
            EnvironmentDoesntCastStencilShadows = 1 << 16,
            ObjectsDonTCastStencilShadows = 1 << 17,
            FirstPersonFromCamera = 1 << 18,
            TextureCameraGel = 1 << 19,
            LightFramerateKiller = 1 << 20,
            AllowedInSplitScreen = 1 << 21,
            OnlyOnParentBipeds = 1 << 22
        }
        
        public enum TypeValue : short
        {
            Sphere,
            Orthogonal,
            Projective,
            Pyramid
        }
        
        public enum ShadowTapBiasValue : short
        {
            _3Tap,
            Unused,
            _1Tap
        }
        
        [Flags]
        public enum InterpolationFlagsValue : uint
        {
            BlendInHsv = 1 << 0,
            MoreColors = 1 << 1
        }
        
        public enum SpecularMaskValue : short
        {
            Default,
            NoneNoMask,
            GelAlpha,
            GelColor
        }
        
        public enum FalloffFunctionValue : short
        {
            Default,
            Narrow,
            Broad,
            VeryBroad
        }
        
        public enum DiffuseContrastValue : short
        {
            DefaultLinear,
            High,
            Low,
            VeryLow
        }
        
        public enum SpecularContrastValue : short
        {
            DefaultOne,
            HighLinear,
            Low,
            VeryLow
        }
        
        public enum FalloffGeometryValue : short
        {
            Default,
            Directional,
            Spherical
        }
        
        public enum DefaultLightmapSettingValue : short
        {
            DynamicOnly,
            DynamicWithLightmaps,
            LightmapsOnly
        }
        
        public enum IlluminationFadeValue : short
        {
            FadeVeryFar,
            FadeFar,
            FadeMedium,
            FadeClose,
            FadeVeryClose
        }
        
        public enum ShadowFadeValue : short
        {
            FadeVeryFar,
            FadeFar,
            FadeMedium,
            FadeClose,
            FadeVeryClose
        }
        
        public enum SpecularFadeValue : short
        {
            FadeVeryFar,
            FadeFar,
            FadeMedium,
            FadeClose,
            FadeVeryClose
        }
        
        [TagStructure(Size = 0xC)]
        public class LightBrightnessAnimationParameters : TagStructure
        {
            public FunctionDefinition Function;
            
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
        
        [TagStructure(Size = 0xC)]
        public class LightColorAnimationParameters : TagStructure
        {
            public FunctionDefinition Function;
            
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
        
        [TagStructure(Size = 0x18)]
        public class LightGelAnimationParameters : TagStructure
        {
            public FunctionDefinition Dx;
            public FunctionDefinition Dy;
            
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
}

