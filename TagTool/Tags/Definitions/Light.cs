using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "light", Tag = "ligh", Size = 0x94, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "light", Tag = "ligh", Size = 0xCC, MinVersion = CacheVersion.HaloReach)]
    public class Light : TagStructure
	{
        public LightFlags Flags;

        public enum LightFlags : int
        {
            None = 0,
            AllowShadowsAndGels = 1 << 0,
            ShadowCasting = 1 << 1,
            RenderFirstPersonOnly = 1 << 2,
            RenderThirdPersonOnly = 1 << 3,
            DontRenderSplitscreen = 1 << 4,
            RenderWhileActiveCamo = 1 << 5,
            RenderInMultiplayerOverride = 1 << 6,
            MoveToCameraInFirstPerson = 1 << 7,
            NeverPriorityCull = 1 << 8,
            AffectedByGameCanUseFlashlights = 1 << 9,
            ScreenspaceSpecularLighting = 1 << 10,
            AlwaysOnForWeapon = 1 << 11
        }

        public TypeValue Type;
        public short Unknown;
        public float LightRange;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float UnknownIntensity;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown0;

        public float FrustumLightNearWidth;
        public float FrustumLightHeightStretch;
        public float FrustumLightFov;

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

        public enum ModifierEnum : short
        {
            None,
            Plus,
            Times
        }

        public StringId ColorChangeInputVariable;
        public StringId ColorChangeRangeVariable;
        public ModifierEnum OutputModifier;
        public short Unknown6;
        public StringId OutputModifierInput;
        public TagFunction Function = new TagFunction { Data = new byte[0] };
        public StringId FunctionName3;
        public StringId FunctionName4;
        public short Unknown8;
        public short Unknown9;
        public uint Unknown10;
        public TagFunction Function2 = new TagFunction { Data = new byte[0] };
        public CachedTag GelMap;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float FrustumMinimumViewDistance;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float MaxIntensityRange;

        public float Unknown12;
        public float Unknown13;

        public sbyte Unknown14;
        public sbyte Unknown15;
        public sbyte Unknown16;
        public sbyte Unknown17;
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

        public enum TypeValue : short
        {
            Sphere,
            Projective,
        }
    }
}