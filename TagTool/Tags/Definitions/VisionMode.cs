using System;
using TagTool.Cache;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "vision_mode", Tag = "vmdx", Size = 0x188, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "vision_mode", Tag = "vmdx", Size = 0x194, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    public class VisionMode : TagStructure
    {
        public VisionModeGlobalFlagsDefinition GlobalFlags;

        public sbyte InternalVersion; // unused, always 1

        [TagField(Length = 0x1, Flags = Padding)]
        public byte[] Pad;

        public PingParametersBlock OnFootPing;
        public PingParametersBlock InVehiclePing;
        public PingParametersBlock FlyingCameraPing;

        [TagField(ValidTags = new[] { "snd!" })] public CachedTag PingSound;

        public PingColorBlock WeaponPingColor;
        public PingColorBlock AllyPingColor;
        public PingColorBlock EnemyPingColor;
        public PingColorBlock ObjectivePingColor;
        public PingColorBlock EnvironmentPingColor;

        [TagField(ValidTags = new[] { "bitm" })] public CachedTag MaskBitmap;
        [TagField(ValidTags = new[] { "cfxs" })] public CachedTag CameraFxOverlay;


            // This controls timing and distance of the pulse animation when VISR is active.

        [TagStructure(Size = 0x18, MinVersion = CacheVersion.HaloOnlineED)]
        [TagStructure(Size = 0x14, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public class PingParametersBlock : TagStructure
        {
            public float PingRadius; // maximum distance affected (world units)

            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public float VisionThroughWallsDistance;

            public float OnFootPingSpeed; // speed of the ping wave (world units per second)
            public float OnFootPingWidth; // width of the ping wave (world units)
            public float OnFootPingFalloff; // how fast the ping falls off (power [0-10])
            public float PingDelaySeconds;
        }

            // This controls the color and intensity of the pulse animation when VISR is active.

        [TagStructure(Size = 0x38, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        public class PingColorBlock : TagStructure
        {
            public TagFunction DefaultColourFunction;
            public TagFunction PingColourFunction;
            public float Alpha; // only affects "through walls" in halo online
            public float DefaultIntensity;
            public float PingIntensity;
            public float OverlappingDimmingFactor; // behaves differently in ODST
        }

        [Flags]
        public enum VisionModeGlobalFlagsDefinition : ushort
        {
            FirstPersonOnly = 1 << 0
        }
    }
}