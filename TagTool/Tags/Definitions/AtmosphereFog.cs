using TagTool.Cache;
using TagTool.Common;
using TagTool.Shaders;
using System.Collections.Generic;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "atmosphere_fog", Tag = "fogg", Size = 0xBC, MinVersion = CacheVersion.HaloReach)]
    public class AtmosphereFog : TagStructure
    {
        public AtmosphereFogFlags Flags;

        public byte Version;
        [TagField(Flags = TagFieldFlags.Padding, Length = 0x1)]
        public byte[] Padding0;

        public float DistanceBias;
        public float DepthFade;

        public FogSettings SkyFog;
        public FogSettings GroundFog;
        public FogLightSettings FogLight;
        public PatchyFogPerClusterParameters PatchyFog;

        public CachedTag WeatherEffect;

        [Flags]
        public enum AtmosphereFogFlags : ushort
        {
            None,
            SkyFogEnabled = 1 << 0,
            GroundFogEnabled = 1 << 1,
            FogLightEnabled = 1 << 2,
            PatchyFogEnabled = 1 << 3,
            Bit4 = 1 << 4,
            Bit5 = 1 << 5,
            Bit6 = 1 << 6,
            Bit7 = 1 << 7,
        }

        [TagStructure(Size = 0x20)]
        public class FogSettings : TagStructure
        {
            public float BaseHeight;
            public float FogHeight;
            public float FogThickness;
            public float FogFalloffEnd;
            public RealRgbColor FogColor;
            public float FogColorIntensity;
        }

        [TagStructure(Size = 0x28)]
        public class FogLightSettings : TagStructure
        {
            public float PitchAngle;
            public float YawAngle;
            public float AngularRadius;
            public RealRgbColor TintColor;
            public float TintColorIntensity;
            public float AngularFalloffSteepness;
            public float DistanceFalloffSteepness;
            public float NearbyCutoffPercentage;
        }

        [TagStructure(Size = 0x38)]
        public class PatchyFogPerClusterParameters: TagStructure
        {
            public float SheetDensity;
            public RealRgbColor ColorTint;
            public RealRgbColor ColorTintInner;
            public float Intensity;
            public float FullIntensityHeight;
            public float HalfIntensityHeight;
            public RealVector3d WindDirection;
            public float ReferencePlaneHeight;
        }
    }
}
