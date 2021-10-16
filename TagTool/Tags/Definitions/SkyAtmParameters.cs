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
        public SkyAtmFlags Flags;
        [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;

        public CachedTag FogBitmap;
        public float TextureRepeatRate;
        public float DistanceBetweenSheets;
        public float DepthFadeFactor;
        public float ClusterSearchRadius;
        public float FalloffStartDistance;
        public float DistanceFalloffPower;
        public float TransparentSortDistance;

        public SortingLayerValue TransparentSortLayer;
        [TagField(Flags = TagFieldFlags.Padding, Length = 0x3)]
        public byte[] Padding1;

        [TagField(MinVersion = CacheVersion.HaloOnline498295)]
        public float Unknown10;
        [TagField(MinVersion = CacheVersion.HaloOnline498295)]
        public int Unknown11;

        public List<AtmosphereProperty> AtmosphereSettings;
        public List<UnderwaterBlock> UnderwaterSettings;

        [Flags]
        public enum SkyAtmFlags : ushort
        {
            LockEffectsToNearestCluster = 1 << 0
        }

        [TagStructure(Size = 0xA4)]
        public class AtmosphereProperty : TagStructure
        {
            public AtmosphereFlags Flags;

            [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;

            public StringId Name;

            public float SunAnglePitch; // 0 to 90
            public float SunAngleYaw; // 0 to 360
            public RealRgbColor Color;
            public float Intensity;

            public float SeaLevel;
            public float RayleignHeightScale;
            public float MieHeightScale;
            public float RayleighMultiplier;
            public float MieMultiplier;
            public float SunPhaseFunction;
            public float Desaturation;
            public float DistanceBias;
            public float MaxFogThickness;
            public RealVector3d BetaM;
            public RealVector3d BetaP;
            public RealVector3d BetaMThetaPrefix;
            public RealVector3d BetaPThetaPrefix;

            public float SheetDensity;
            public float FullIntensityHeight;
            public float HalfIntensityHeight;
            public RealVector3d WindDirection;

            public CachedTag WeatherEffect;
            public float RuntimeWeight;
            public float RuntimeEffectWeight;

            [Flags]
            public enum AtmosphereFlags : short
            {
                None,
                EnableAtmosphere = 1 << 0,
                OverrideRealSunValues = 1 << 1,
                PatchyFog = 1 << 2,
                Bit3 = 1 << 3,
            }
        }

        [TagStructure(Size = 0x14)]
        public class UnderwaterBlock : TagStructure
		{
            public StringId Name;
            public float Murkiness;
            public RealRgbColor FogColor;
        }
    }
}