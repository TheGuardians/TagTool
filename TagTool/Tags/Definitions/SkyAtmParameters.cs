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
            None,
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

            private void PostprocessFogConstants()
            {
                // todo: B particle and molecular calculation according to tool.
                // these are still accurate, as the only thing that changes these constants are:
                // mie_multiplier, rayleigh_multiplier, desaturation
                BetaM = new RealVector3d(0.000005893206f, 0.000010048514f, 0.00002117206f);
                BetaP = new RealVector3d(0.00005737284f, 0.00007397888f, 0.00010511476f);
                BetaMThetaPrefix = new RealVector3d(0.0000003577744f, 0.0000006100418f, 0.0000012853482f);
                BetaPThetaPrefix = new RealVector3d(0.000013338932f, 0.000017345952f, 0.000024978172f);

                // apply multipliers
                BetaM *= RayleighMultiplier;
                BetaMThetaPrefix *= RayleighMultiplier;
                BetaP *= MieMultiplier;
                BetaPThetaPrefix *= MieMultiplier;

                // rescale
                BetaM *= 1000.0f;
                BetaMThetaPrefix *= 1000.0f;
                BetaP *= 1000.0f;
                BetaPThetaPrefix *= 1000.0f;

                // apply desaturation
                if (Desaturation > 0.0f)
                {
                    float pLum = (BetaP.I + BetaP.J + BetaP.K) / 3.0f;
                    BetaP.I = MathHelper.Lerp(BetaP.I, pLum, Desaturation);
                    BetaP.J = MathHelper.Lerp(BetaP.J, pLum, Desaturation);
                    BetaP.K = MathHelper.Lerp(BetaP.K, pLum, Desaturation);

                    float mLum = (BetaM.I + BetaM.J + BetaM.K) / 3.0f;
                    BetaM.I = MathHelper.Lerp(BetaM.I, mLum, Desaturation);
                    BetaM.J = MathHelper.Lerp(BetaM.J, mLum, Desaturation);
                    BetaM.K = MathHelper.Lerp(BetaM.K, mLum, Desaturation);

                    float pThetaLum = (BetaPThetaPrefix.I + BetaPThetaPrefix.J + BetaPThetaPrefix.K) / 3.0f;
                    BetaPThetaPrefix.I = MathHelper.Lerp(BetaPThetaPrefix.I, pThetaLum, Desaturation);
                    BetaPThetaPrefix.J = MathHelper.Lerp(BetaPThetaPrefix.J, pThetaLum, Desaturation);
                    BetaPThetaPrefix.K = MathHelper.Lerp(BetaPThetaPrefix.K, pThetaLum, Desaturation);

                    float mThetaLum = (BetaMThetaPrefix.I + BetaMThetaPrefix.J + BetaMThetaPrefix.K) / 3.0f;
                    BetaMThetaPrefix.I = MathHelper.Lerp(BetaMThetaPrefix.I, mThetaLum, Desaturation);
                    BetaMThetaPrefix.J = MathHelper.Lerp(BetaMThetaPrefix.J, mThetaLum, Desaturation);
                    BetaMThetaPrefix.K = MathHelper.Lerp(BetaMThetaPrefix.K, mThetaLum, Desaturation);
                }
            }

            private void ClampValues()
            {
                SunAnglePitch = MathHelper.Clamp(SunAnglePitch, 0.0f, 90.0f);
                SunAngleYaw = MathHelper.Clamp(SunAngleYaw, 0.0f, 360.0f);
                Color = MathHelper.Clamp(Color, 0.0f, 1.0f);
                Intensity = MathHelper.Clamp(Intensity, 0.0f, 65536.0f);
                SeaLevel = MathHelper.Clamp(SeaLevel, -65536.0f, 65536.0f);
                RayleignHeightScale = MathHelper.Clamp(RayleignHeightScale, 0.01f, 65536.0f);
                MieHeightScale = MathHelper.Clamp(MieHeightScale, 0.01f, 65536.0f);
                RayleighMultiplier = MathHelper.Clamp(RayleighMultiplier, 0.0f, 1000.0f);
                MieMultiplier = MathHelper.Clamp(MieMultiplier, 0.0f, 1000.0f);
                SunPhaseFunction = MathHelper.Clamp(SunPhaseFunction, 0.0f, 0.95f);
                DistanceBias = MathHelper.Clamp(DistanceBias, -65536.0f, 65536.0f);
                MaxFogThickness = MathHelper.Clamp(MaxFogThickness, 0.01f, 65536.0f);
                Desaturation = MathHelper.Clamp(Desaturation, 0.0f, 1.0f);
                SheetDensity = MathHelper.Clamp(SheetDensity, 0.0f, 10000.0f);
            }

            public void Postprocess()
            {
                if (Math.Abs(MaxFogThickness) < 0.001f)
                    MaxFogThickness = 65535.0f;
                RuntimeWeight = 0.0f;

                ClampValues();
                PostprocessFogConstants();
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

        public void Postprocess()
        {
            if (Math.Abs(DistanceFalloffPower) < 0.001f)
                DistanceFalloffPower = 2.0f;
            if (Math.Abs(FalloffStartDistance) < 0.001f)
                FalloffStartDistance = 5.0f;
            if (Math.Abs(ClusterSearchRadius) < 0.001f)
                ClusterSearchRadius = 25.0f;
            if (Math.Abs(TransparentSortDistance) < 0.001f)
                TransparentSortDistance = 100.0f;

            foreach (var atmosphere in AtmosphereSettings)
                atmosphere.Postprocess();

            TextureRepeatRate = MathHelper.Clamp(TextureRepeatRate, 0.1f, 100.0f);
            DistanceBetweenSheets = MathHelper.Clamp(DistanceBetweenSheets, 0.1f, 10000.0f);
            DepthFadeFactor = MathHelper.Clamp(DepthFadeFactor, 0.00001f, 10.0f);
            ClusterSearchRadius = MathHelper.Clamp(ClusterSearchRadius, 1.0f, 50.0f);
            FalloffStartDistance = MathHelper.Clamp(FalloffStartDistance, 0.1f, 10.0f);
            DistanceFalloffPower = MathHelper.Clamp(DistanceFalloffPower, 0.1f, 10.0f);
            TransparentSortDistance = MathHelper.Clamp(TransparentSortDistance, 0.1f, 65535.0f);
        }
    }
}