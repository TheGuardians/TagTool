using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "light", Tag = "ligh", Size = 0xE4)]
    public class Light : TagStructure
    {
        public FlagsValue Flags;
        /// <summary>
        /// overall shape of the light
        /// </summary>
        public TypeValue Type;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        /// <summary>
        /// how the light's size changes with external scale
        /// </summary>
        public Bounds<float> SizeModifer;
        /// <summary>
        /// larger positive numbers improve quality, larger negative numbers improve speed
        /// </summary>
        public float ShadowQualityBias;
        /// <summary>
        /// the less taps you use, the faster the light (but edges can look worse)
        /// </summary>
        public ShadowTapBiasValue ShadowTapBias;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        /// <summary>
        /// default shape parameters for spherical lights
        /// </summary>
        /// <summary>
        /// the radius at which illumination falls off to zero
        /// </summary>
        public float Radius; // world units
        /// <summary>
        /// the radius at which specular highlights fall off to zero (if zero, same as maximum radius)
        /// </summary>
        public float SpecularRadius; // world units
        /// <summary>
        /// default shape parameters for frustum lights (orthogonal, projective or pyramid types)
        /// </summary>
        /// <summary>
        /// width of the frustum light at its near plane
        /// </summary>
        public float NearWidth; // world units
        /// <summary>
        /// how much the gel is stretched vertically (0.0 or 1.0 = aspect ratio same as gel)
        /// </summary>
        public float HeightStretch;
        /// <summary>
        /// horizontal angle that the frustum light covers (0.0 = no spread, a parallel beam)
        /// </summary>
        public float FieldOfView; // degrees
        /// <summary>
        /// distance from near plane to where the light falloff starts
        /// </summary>
        public float FalloffDistance;
        /// <summary>
        /// distance from near plane to where illumination falls off to zero
        /// </summary>
        public float CutoffDistance;
        public InterpolationFlagsValue InterpolationFlags;
        public Bounds<float> BloomBounds; // [0..2]
        public RealRgbColor SpecularLowerBound;
        public RealRgbColor SpecularUpperBound;
        public RealRgbColor DiffuseLowerBound;
        public RealRgbColor DiffuseUpperBound;
        public Bounds<float> BrightnessBounds; // [0..2]
        /// <summary>
        /// the gel map tints the light's illumination per-pixel
        /// </summary>
        /// <summary>
        /// must be a cubemap for spherical light and a 2d texture for frustum light
        /// </summary>
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag GelMap;
        public SpecularMaskValue SpecularMask;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding3;
        public FalloffFunctionValue FalloffFunction;
        public DiffuseContrastValue DiffuseContrast;
        public SpecularContrastValue SpecularContrast;
        public FalloffGeometryValue FalloffGeometry;
        /// <summary>
        /// optional lens flare and light volume associated with this light
        /// </summary>
        [TagField(ValidTags = new [] { "lens" })]
        public CachedTag LensFlare;
        /// <summary>
        /// used to generate a bounding radius for lensflare-only lights
        /// </summary>
        public float BoundingRadius; // world units
        [TagField(ValidTags = new [] { "MGS2" })]
        public CachedTag LightVolume;
        /// <summary>
        /// how the light affects the lightmaps (ignored for dynamic lights)
        /// </summary>
        public DefaultLightmapSettingValue DefaultLightmapSetting;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding4;
        public float LightmapHalfLife;
        public float LightmapLightScale;
        /// <summary>
        /// if the light is created by an effect, it will animate itself as follows
        /// </summary>
        /// <summary>
        /// the light will last this long when created by an effect
        /// </summary>
        public float Duration; // seconds
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding5;
        /// <summary>
        /// the scale of the light will diminish over time according to this function
        /// </summary>
        public FalloffFunctionValue1 FalloffFunction1;
        /// <summary>
        /// To fade the light's illumination and shadow casting abilities
        /// </summary>
        public IlluminationFadeValue IlluminationFade;
        public ShadowFadeValue ShadowFade;
        public SpecularFadeValue SpecularFade;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding6;
        public FlagsValue1 Flags1;
        public List<LightBrightnessAnimationBlock> BrightnessAnimation;
        public List<LightColorAnimationBlock> ColorAnimation;
        public List<LightGelAnimationBlock> GelAnimation;
        [TagField(ValidTags = new [] { "shad" })]
        public CachedTag Shader;
        
        [Flags]
        public enum FlagsValue : uint
        {
            /// <summary>
            /// don't cast any per-pixel dynamic light
            /// </summary>
            NoIllumination = 1 << 0,
            /// <summary>
            /// don't cast any specular highlights
            /// </summary>
            NoSpecular = 1 << 1,
            ForceCastEnvironmentShadowsThroughPortals = 1 << 2,
            /// <summary>
            /// don't cast any stencil shadows
            /// </summary>
            NoShadow = 1 << 3,
            ForceFrustumVisibilityOnSmallLight = 1 << 4,
            OnlyRenderInFirstPerson = 1 << 5,
            OnlyRenderInThirdPerson = 1 << 6,
            /// <summary>
            /// don't fade out this light when under active-camouflage
            /// </summary>
            DonTFadeWhenInvisible = 1 << 7,
            /// <summary>
            /// don't turn off in multiplayer
            /// </summary>
            MultiplayerOverride = 1 << 8,
            AnimatedGel = 1 << 9,
            /// <summary>
            /// only draw this light in dynamic reflection maps
            /// </summary>
            OnlyInDynamicEnvmap = 1 << 10,
            /// <summary>
            /// don't illuminate or shadow the single object we are attached to
            /// </summary>
            IgnoreParentObject = 1 << 11,
            /// <summary>
            /// don't shadow the object we are attached to
            /// </summary>
            DonTShadowParent = 1 << 12,
            /// <summary>
            /// don't illuminate or shadow all the way up to our parent object
            /// </summary>
            IgnoreAllParents = 1 << 13,
            /// <summary>
            /// don't click this unless you know what you're doing
            /// </summary>
            MarchMilestoneHack = 1 << 14,
            /// <summary>
            /// every update will push light back inside the world
            /// </summary>
            ForceLightInsideWorld = 1 << 15,
            /// <summary>
            /// environment in this light will not cast stencil shadows
            /// </summary>
            EnvironmentDoesntCastStencilShadows = 1 << 16,
            /// <summary>
            /// objects in this light will not cast stencil shadows
            /// </summary>
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
            /// <summary>
            /// blends colors in hsv rather than rgb space
            /// </summary>
            BlendInHsv = 1 << 0,
            /// <summary>
            /// blends colors through more hues (goes the long way around the color wheel)
            /// </summary>
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
        
        public enum FalloffFunctionValue1 : short
        {
            Linear,
            Late,
            VeryLate,
            Early,
            VeryEarly,
            Cosine,
            Zero,
            One
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
        
        [Flags]
        public enum FlagsValue1 : uint
        {
            Synchronized = 1 << 0
        }
        
        [TagStructure(Size = 0x8)]
        public class LightBrightnessAnimationBlock : TagStructure
        {
            public MappingFunctionBlock Function;
            
            [TagStructure(Size = 0x8)]
            public class MappingFunctionBlock : TagStructure
            {
                public List<ByteBlock> Data;
                
                [TagStructure(Size = 0x1)]
                public class ByteBlock : TagStructure
                {
                    public sbyte Value;
                }
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class LightColorAnimationBlock : TagStructure
        {
            public MappingFunctionBlock Function;
            
            [TagStructure(Size = 0x8)]
            public class MappingFunctionBlock : TagStructure
            {
                public List<ByteBlock> Data;
                
                [TagStructure(Size = 0x1)]
                public class ByteBlock : TagStructure
                {
                    public sbyte Value;
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class LightGelAnimationBlock : TagStructure
        {
            public MappingFunctionBlock Dx;
            public MappingFunctionBlock1 Dy;
            
            [TagStructure(Size = 0x8)]
            public class MappingFunctionBlock : TagStructure
            {
                public List<ByteBlock> Data;
                
                [TagStructure(Size = 0x1)]
                public class ByteBlock : TagStructure
                {
                    public sbyte Value;
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class MappingFunctionBlock1 : TagStructure
            {
                public List<ByteBlock> Data;
                
                [TagStructure(Size = 0x1)]
                public class ByteBlock : TagStructure
                {
                    public sbyte Value;
                }
            }
        }
    }
}

