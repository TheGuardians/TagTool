using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sky_atm_parameters", Tag = "skya", Size = 0x4C, MinVersion = CacheVersion.Halo3Retail,MaxVersion = CacheVersion.HaloOnline454665)]
    [TagStructure(Name = "sky_atm_parameters", Tag = "skya", Size = 0x54, MinVersion = CacheVersion.HaloOnline498295)]
    public class SkyAtmParameters : TagStructure
	{
        public int Unknown1;
        public CachedTag FogBitmap;
        public float Unknown2;
        public float Unknown3;
        public float Unknown4;
        public float Unknown5;
        public float Unknown6;
        public float Unknown7;
        public float Unknown8;
        public int Unknown9;

        [TagField(MinVersion = CacheVersion.HaloOnline498295)]
        public float Unknown10;
        [TagField(MinVersion = CacheVersion.HaloOnline498295)]
        public int Unknown11;

        public List<AtmosphereProperty> AtmosphereProperties;
        public List<UnderwaterBlock> Underwater;
        
        [TagStructure(Size = 0xA4)]
        public class AtmosphereProperty : TagStructure
		{
            public short Unknown1;
            public short Unknown2;
            public StringId Name;
            public float LightSourceY;
            public float LightSourceX;
            public RealRgbColor FogColor;
            public float Brightness;
            public float FogGradientThreshold;
            public float LightIntensity;
            public float SkyInvisiblilityThroughFog;
            public float Unknown3;
            public float Unknown4;
            public float LightSourceSpread;
            public uint Unknown5;
            public float FogIntensity;
            public float Unknown6;
            public float TintCyan;
            public float TintMagenta;
            public float TintYellow;
            public float FogIntensityCyan;
            public float FogIntensityMagenta;
            public float FogIntensityYellow;
            public float BackgroundColorRed;
            public float BackgroundColorGreen;
            public float BackgroundColorBlue;
            public float TintRed;
            public float Tint2Green;
            public float Tint2Blue;
            public float FogIntensity2;
            public float StartDistance;
            public float EndDistance;
            public float FogVelocityX;
            public float FogVelocityY;
            public float FogVelocityZ;
            public CachedTag WeatherEffect;
            public uint Unknown7;
            public uint Unknown8;
        }

        [TagStructure(Size = 0x14)]
        public class UnderwaterBlock : TagStructure
		{
            public StringId Name;
            public RealArgbColor Color;
        }
    }
}