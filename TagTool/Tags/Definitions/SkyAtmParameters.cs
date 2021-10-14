using TagTool.Cache;
using TagTool.Common;
using TagTool.Shaders;
using System.Collections.Generic;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sky_atm_parameters", Tag = "skya", Size = 0x4C, MinVersion = CacheVersion.Halo3Retail,MaxVersion = CacheVersion.HaloOnline454665)]
    [TagStructure(Name = "sky_atm_parameters", Tag = "skya", Size = 0x54, MinVersion = CacheVersion.HaloOnline498295)]
    public class SkyAtmParameters : TagStructure
	{
        public int Unknown1;
        public CachedTag FogBitmap;
        public float TextureRepeatRate;
        public float DistanceBetweenSheets;
        public float DepthFadeFactor;
        public float Unknown5;
        public float FalloffStartDistance;
        public float DistanceFalloffPower;
        public float TransparentSortDistance;
        public SortingLayerValue TransparentSortingLayer;

        [TagField(Flags = TagFieldFlags.Padding, Length = 0x3)]
        public byte[] Unused;

        [TagField(MinVersion = CacheVersion.HaloOnline498295)]
        public float Unknown10;
        [TagField(MinVersion = CacheVersion.HaloOnline498295)]
        public int Unknown11;

        public List<AtmosphereProperty> AtmosphereProperties;
        public List<UnderwaterBlock> Underwater;
        
        [TagStructure(Size = 0xA4)]
        public class AtmosphereProperty : TagStructure
		{
            public AtmosphereFlags Flags;
            public short Unused;
            public StringId Name;
            public float LightSourceY;
            public float LightSourceX;
            public RealRgbColor FogColor;
            public float Brightness;
            public float FogGradientThreshold;
            public float LightIntensity;
            public float SkyInvisibilityThroughFog;
            public float Unknown3;
            public float Unknown4;
            public float LightSourceSpread;
            public float Unknown5;
            public float BackgroundFogIntensity;
            public float Unknown6;
            public float TintCyan;
            public float TintMagenta;
            public float TintYellow;
            public float FogIntensityCyan;
            public float FogIntensityMagenta;
            public float FogIntensityYellow;
            public RealRgbColor BackgroundColor;
            public RealRgbColor Tint;
            public float PatchyFogIntensity;
            public float StartDistance;
            public float EndDistance;
            public RealVector3d FogVelocity;
            public CachedTag WeatherEffect;
            public uint Unknown7;
            public uint Unknown8;

            [Flags]
            public enum AtmosphereFlags : short
            {
                SkyFogEnabled = 1 << 0,
                UseFogColor = 1 << 1,
                PatchyFogEnabled = 1 << 2,
                Bit3 = 1 << 3,
            }
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