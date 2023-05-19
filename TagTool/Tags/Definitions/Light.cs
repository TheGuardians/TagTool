using TagTool.Cache;
using TagTool.Common;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "light", Tag = "ligh", Size = 0x94, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "light", Tag = "ligh", Size = 0xCC, MinVersion = CacheVersion.HaloReach)]
    public class Light : TagStructure
	{
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public ReachLightMethodEnum LightMethod;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public sbyte Version;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public LightFlags Flags;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public LightFlagsReach ReachFlags;

        public TypeValue Type;

        [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;

        public float MaximumDistance; // (wu) max distance the light reaches (can become very dark well before if distance diffusion is low)

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float AttenuationStartDistance;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float LightRainVolumeRange;

        public float FrustumNearWidth; // width of the frustum at the near plane (world units)
        public float FrustumHeightScale; // how much the gel is stretched vertically (0.0 or 1.0 = aspect ratio same as gel)
        public float FrustumFieldOfView; // horizontal angle that the frustum light covers (0.0 = no spread, a straight beam of light)

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float AngularHotspot; // inner falloff angle
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float AngularFalloffSpeed; // 1.0 for sharp edges, greater than 1.0 for smooth edges ([1 - 12])
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float DistanceDiffusionReach;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float NearFadingDistance; // world units

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float ShadowCutoffDistance;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public LightShadowQualityEnumeration ShadowQuality;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.HaloReach)]
        public short PaddingReach;

        public LightColorFunctionStruct Color;
        public LightScalarFunctionStruct Intensity;

        [TagField(ValidTags = new[] { "bitm" })]
        public CachedTag GelBitmap; // requires 'expensive light'

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float DistanceDiffusion; // [0.01 - 10.0+] approximately the physical size in world units of the light source itself.  small values cause the light to become very bright close to the light source, but quickly die off as you move away
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float AngularSmoothness; // [0.2 - 8.0] less than 1.0 for sharp edges, greater than 1.0 for smooth edges

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float PercentSpherical; // [0.0 - 1.0] percentage of ambient (spherical) light

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float RainLightReduction; // 0 means no reduction, 1 means completely removed

        public float DestroyLightAfter; // automatically destroys the light after this many seconds (0 to disable)

        public LightPriorityEnumeration NearPriority; // priority when the light is fullscreen in size
        public LightPriorityEnumeration FarPriority; // priority when the light is very far away

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public LightPriorityBiasEnumeration TransitionDistance; // specifies where the transition occurs between near and far priority

        [TagField(Length = 1, Flags = TagFieldFlags.Padding, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagField(Length = 2, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.HaloReach)]
        public byte[] Padding1;

        [TagField(ValidTags = new[] { "lens" })]
        public CachedTag LensFlare;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public ClipPlanesStruct ClipPlanes;

        [Flags]
        public enum LightFlags : uint
        {
            None = 0,
            AllowShadowsAndGels = 1 << 0, // CPU lights can have shadows and gels
            ShadowCasting = 1 << 1, // turns on shadow casting
            RenderFirstPersonOnly = 1 << 2, // only render when camera is 1st person
            RenderThirdPersonOnly = 1 << 3, // don't render when camera is 1st person
            DontRenderSplitscreen = 1 << 4, // no rendering this light in splitscreen mode
            RenderWhileActiveCamo = 1 << 5, // keep rendering this light when the attached player goes camo
            RenderInMultiplayerOverride = 1 << 6, // overrides game settings that disable dynamic lights
            MoveToCameraInFirstPerson = 1 << 7, // moves the light to match the camera
            NeverPriorityCull = 1 << 8, // never cull this light because of low priority
            AffectedByGameCanUseFlashlights = 1 << 9,
            ScreenspaceSpecularLighting = 1 << 10,
            AlwaysOnForWeapon = 1 << 11,
            Bit12 = 1 << 12,
            Bit13 = 1 << 13,
            Bit14 = 1 << 14,
            Bit15 = 1 << 15,
            Bit16 = 1 << 16,
            Bit17 = 1 << 17,
            Bit18 = 1 << 18,
            Bit19 = 1 << 19,
            Bit20 = 1 << 20,
            Bit21 = 1 << 21,
            Bit22 = 1 << 22,
            Bit23 = 1 << 23,
            Bit24 = 1 << 24,
            Bit25 = 1 << 25,
            Bit26 = 1 << 26,
            Bit27 = 1 << 27,
            Bit28 = 1 << 28,
            Bit29 = 1 << 29,
            Bit30 = 1 << 30,
            ElDewritoForgeLight = 1u << 31, // Legacy
        }

        [Flags]
        public enum LightFlagsReach : ushort
        {
            None = 0,
            AllowShadowsAndGels = 1 << 0,
            ShadowCasting = 1 << 1, // turns on shadow casting
            RenderFirstPersonOnly = 1 << 2, // only render when camera is 1st person
            RenderThirdPersonOnly = 1 << 3, // don't render when camera is 1st person
            DontRenderSplitscreen = 1 << 4, // no rendering this light in splitscreen mode
            RenderWhileActiveCamo = 1 << 5, // keep rendering this light when the attached player goes camo
            RenderInMultiplayerOverride = 1 << 6, // overrides game settings that disable dynamic lights
            MoveToCameraInFirstPerson = 1 << 7, // moves the light to match the camera
            NeverPriorityCull = 1 << 8, // never cull this light because of low priority
            AffectedByGameCanUseFlashlights = 1 << 9,
            ScreenspaceSpecularLighting = 1 << 10, // uses expensive specular lighting on screenspace lights
            AlwaysOnForWeapon = 1 << 11 // even it is dropped
        }

        public enum ReachLightMethodEnum : sbyte
        {
            Screenspace,
            Inline,
            ReRender
        }

        public enum TypeValue : short
        {
            Sphere,
            Frustum,
        }

        public enum ModifierEnum : short
        {
            None,
            Plus,
            Times
        }

        [TagStructure(Size = 0x24)]
        public class LightColorFunctionStruct : TagStructure
        {
            public StringId InputVariable;
            public StringId RangeVariable;
            public ModifierEnum OutputModifier;

            [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;

            public StringId OutputModifierInput;
            public TagFunction Function = new TagFunction { Data = new byte[0] };
        }

        [TagStructure(Size = 0x24)]
        public class LightScalarFunctionStruct : TagStructure
        {
            public StringId InputVariable;
            public StringId RangeVariable;
            public ModifierEnum OutputModifier;

            [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;

            public StringId OutputModifierInput;
            public TagFunction Function = new TagFunction { Data = new byte[0] };
        }

        [TagStructure(Size = 0x20, MinVersion = CacheVersion.HaloReach)]
        public class ClipPlanesStruct : TagStructure
        {
            public LightClipFlagsDefinition ClipFlags;

            public int UnusedPaddingDontTouch; // often nonzero

            public Bounds<float> ClipX; // world units
            public Bounds<float> ClipY; // world units
            public Bounds<float> ClipZ; // world units

            [Flags]
            public enum LightClipFlagsDefinition : uint
            {
                ClipX = 1 << 0,
                ClipY = 1 << 1,
                ClipZ = 1 << 2
            }
        }

        public enum LightTypeEnumDefinition : short
        {
            Sphere,
            Frustum
        }

        public enum LightPriorityEnumeration : sbyte
        {
            Default,
            InsanelyHigh,
            _1VeryHigh,
            _2,
            _3High,
            _4,
            _5Default,
            _6,
            _7Low,
            _8,
            _9VeryLow,
            NextToNothing
        }

        public enum LightPriorityBiasEnumeration : sbyte
        {
            Default,
            VeryClose,
            Close,
            Middle,
            Far,
            VeryFar
        }

        public enum LightShadowQualityEnumeration : short
        {
            Shadow128x128,
            Shadow256x256,
            Shadow512x512,
            Shadow800x800
        }
    }
}