using TagTool.Cache;
using TagTool.Common;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "light", Tag = "ligh", Size = 0x94, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "light", Tag = "ligh", Size = 0xCC, MinVersion = CacheVersion.HaloReach)]
    public class Light : TagStructure
	{
        public LightFlags Flags;
        public TypeValue Type;

        [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;

        public float MaximumDistance; // maximum distance the light reaches (the light can become very dark well before this distance if you set your distance diffusion low) (world units)

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float UnknownIntensity;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown0;

        public float FrustumNearWidth; // width of the frustum at the near plane (world units)
        public float FrustumHeightScale; // how much the gel is stretched vertically (0.0 or 1.0 = aspect ratio same as gel)
        public float FrustumFieldOfView; // horizontal angle that the frustum light covers (0.0 = no spread, a straight beam of light)

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float UnknownAngle;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float MaxIntensityRangeReach;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float FrustumMinimumViewDistanceReach;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown1;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown2;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short Unknown3;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short Unknown4;

        public LightColorFunctionStruct Color;
        public LightScalarFunctionStruct Intensity;
        public CachedTag GelBitmap; // requires 'expensive light'

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float DistanceDiffusion; // [0.01 - 10.0+] approximately the physical size in world units of the light source itself.  small values cause the light to become very bright close to the light source, but quickly die off as you move away
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float AngularSmoothness; // [0.2 - 8.0] less than 1.0 for sharp edges, greater than 1.0 for smooth edges

        public float PercentSpherical; // [0.0 - 1.0] percentage of ambient (spherical) light
        public float DestroyLightAfter; // automatically destroys the light after this many seconds (0 to disable)
        public LightPriorityEnumeration NearPriority; // priority when the light is fullscreen in size
        public LightPriorityEnumeration FarPriority; // priority when the light is very far away
        public LightPriorityBiasEnumeration TransitionDistance; // specifies where the transition occurs between near and far priority

        [TagField(Length = 1, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;

        public CachedTag LensFlare;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float RadiusModifer1;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float RadiusModifer2;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float RadiusModifer3;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown18;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown19;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown20;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown21;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown22;

        [Flags]
        public enum LightFlags : int
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
            AlwaysOnForWeapon = 1 << 11
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
    }
}