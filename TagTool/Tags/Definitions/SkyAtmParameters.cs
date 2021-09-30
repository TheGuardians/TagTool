using TagTool.Cache;
using TagTool.Common;
using TagTool.Shaders;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sky_atm_parameters", Tag = "skya", Size = 0x4C, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline454665)]
    [TagStructure(Name = "sky_atm_parameters", Tag = "skya", Size = 0x54, MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "sky_atm_parameters", Tag = "skya", Size = 0x44, MinVersion = CacheVersion.HaloReach)]
    public class SkyAtmParameters : TagStructure
	{
        [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
        public int Unknown0;

        public CachedTag FogBitmap;
        public float TextureRepeatRate;
        public float DistanceBetweenSheets;
        public float DepthFadeFactor;

        [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown5;

        public float FalloffStartDistance;
        public float DistanceFalloffPower;
        public float TransparentSortDistance;
        public SortingLayerValue TransparentSortingLayer;

        [TagField(Flags = TagFieldFlags.Padding, Length = 0x3)]
        public byte[] Unused;

        [TagField(MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown10;
        [TagField(MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
        public int Unknown11;

        public List<AtmosphereProperty> AtmosphereProperties;
        public List<UnderwaterBlock> Underwater;
        
        [TagStructure(Size = 0xA4, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0xAC, MinVersion = CacheVersion.HaloReach)]
        public class AtmosphereProperty : TagStructure
		{
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public StringId ReachName;

            public short Unknown1;
            public short Unknown2;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public StringId Name;

            public RealPoint2d LightSource;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public RealRgbColor FogColor;

            public float Brightness;
            public float FogGradientThreshold;
            public float LightIntensity;
            public float SkyInvisibilityThroughFog;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public RealRgbColor FogColorReach;

            public float Unknown3;
            public float Unknown4;
            public float LightSourceSpread;
            public float Unknown5;
            public float FogIntensity;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float UnknownFlags;

            public float TintCyan;
            public float TintMagenta;
            public float TintYellow;

            public float FogIntensityCyan;
            public float FogIntensityMagenta;
            public float FogIntensityYellow;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float ReachUnknown;

            public RealRgbColor BackgroundColor;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float ReachUnknown1;

            public RealRgbColor Tint2;

            public float FogIntensity2;
            public float StartDistance;
            public float EndDistance;

            public RealVector3d FogVelocity;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float ReachUnknown2;

            public CachedTag WeatherEffect;
            public uint Unknown7;
            public uint Unknown8;
        }

        [TagStructure(Size = 0x14)]
        public class UnderwaterBlock : TagStructure
		{
            public StringId Name;
            public float Murkiness;
            public RealRgbColor Color;
        }
    }
}