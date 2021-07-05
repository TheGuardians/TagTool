using TagTool.Cache;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "vision_mode", Tag = "vmdx", Size = 0x188, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "vision_mode", Tag = "vmdx", Size = 0x194, MinVersion = CacheVersion.HaloOnlineED)]
    public class VisionMode : TagStructure
    {
        public sbyte Unknown1;
        public sbyte Unknown2;
        public sbyte Unknown3; // unused, always 1
        public sbyte Unknown4;

        public PingParametersBlock Ping;
        public PingParametersBlock VehiclePing;
        public PingParametersBlock ObserverPing;
        public CachedTag PingSound;

        public PingColorBlock WeaponPingColor;
        public PingColorBlock AllyPingColor;
        public PingColorBlock EnemyPingColor;
        public PingColorBlock ObjectivePingColor;
        public PingColorBlock EnvironmentPingColor;

        public CachedTag VisionMask;
        public CachedTag VisionCameraFx;

        /// <summary>
        /// This controls timing and distance of the pulse animation when VISR is active.
        /// </summary>
        [TagStructure(Size = 0x18, MinVersion = CacheVersion.HaloOnlineED)]
        [TagStructure(Size = 0x14, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public class PingParametersBlock : TagStructure
        {
            public float PingDistance;
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public float VisionThroughWallsDistance;
            public float PingInterval; // 30 tick
            public float PingPulseDuration; // 30 tick
            public float Unused;
            public float PingDelaySeconds;
        }

        /// <summary>
        /// This controls the color and intensity of the pulse animation when VISR is active.
        /// </summary>
        [TagStructure(Size = 0x38, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline106708)]
        public class PingColorBlock : TagStructure
        {
            public TagFunction PrimaryColourFunction;
            public TagFunction SecondaryColourFunction;
            public float Alpha; // only affects "through walls" in halo online
            public float PrimaryColorScale;
            public float SecondaryColourScale;
            public float OverlappingDimmingFactor; // behaves differently in ODST
        }
    }
}