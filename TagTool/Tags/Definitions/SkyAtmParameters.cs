using TagTool.Cache;
using TagTool.Common;
using TagTool.Shaders;
using System.Collections.Generic;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sky_atm_parameters", Tag = "skya", Size = 0x4C, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline454665)]
    [TagStructure(Name = "sky_atm_parameters", Tag = "skya", Size = 0x54, MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "sky_atm_parameters", Tag = "skya", Size = 0x44, MinVersion = CacheVersion.HaloReach)]
    public class SkyAtmParameters : TagStructure
    {
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public SkyAtmFlags Flags;
        [TagField(Length = 2, Flags = TagFieldFlags.Padding, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] Padding;

        public CachedTag FogBitmap;
        public float TextureRepeatRate;
        public float DistanceBetweenSheets;
        public float DepthFadeFactor;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float ClusterSearchRadius;
        public float FalloffStartDistance;
        public float DistanceFalloffPower;
        public float TransparentSortDistance;

        public SortingLayerValue TransparentSortLayer;
        [TagField(Flags = TagFieldFlags.Padding, Length = 0x3)]
        public byte[] Padding1;

        [TagField(MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown10;
        [TagField(MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
        public int Unknown11;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<AtmosphereProperty> AtmosphereSettings;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<AtmospherePropertyReach> AtmosphereSettingsReach;
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

        // probably lots of wrong fields, unused anyway
        [TagStructure(Size = 0xAC)]
        public class AtmospherePropertyReach : TagStructure
        {
            public StringId Name;

            public AtmosphereFlags Flags;
            [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;

            public float DistanceBias;
            public float DepthFade;

            public AtmosphereFog.FogSettings SkyFog;
            public AtmosphereFog.FogSettings GroundFog;
            public AtmosphereFog.FogLightSettings FogLight;

            public float SheetDensity;
            public float FullIntensityHeight;
            public float HalfIntensityHeight;
            public RealVector3d WindDirection;
            public float ReferencePlaneHeight;

            public CachedTag WeatherEffect;
            public float RuntimeWeight;
            public float RuntimeEffectWeight;

            [Flags]
            public enum AtmosphereFlags : short
            {
                None,
                Bit0 = 1 << 0,
                Bit1 = 1 << 1,
                Bit2 = 1 << 2,
                Bit3 = 1 << 3,
                Bit4 = 1 << 4,
                Bit5 = 1 << 5,
                Bit6 = 1 << 6,
                Bit7 = 1 << 7,
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