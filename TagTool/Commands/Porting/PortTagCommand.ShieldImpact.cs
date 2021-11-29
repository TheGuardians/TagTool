using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private ShieldImpact PreConvertShieldImpact(ShieldImpact shit, CacheVersion blamVersion, GameCacheHaloOnlineBase cache)
        {
            cache.TagCache.TryGetCachedTag(@"levels\shared\bitmaps\test_maps\cloud_1.bitmap", out CachedTag Noise1);
            cache.TagCache.TryGetCachedTag(@"levels\shared\bitmaps\test_maps\cloud_2.bitmap", out CachedTag Noise2);

            // Shield Intensity
            shit.ShieldIntensity = new ShieldImpact.ShieldIntensityBlock
            {
                RecentDamageIntensity = 1,
                CurrentDamageIntensity = 1
            };

            // Shield Edge
            shit.ShieldEdge = new ShieldImpact.ShieldEdgeBlock
            {
                EdgeGlowColor = new ShieldImpactFunction
                {
                    InputVariable = StringId.Invalid,
                    RangeVariable = StringId.Invalid,
                    Function = new TagFunction { Data = new byte[14] }
                },
                EdgeGlowIntensity = new ShieldImpactFunction
                {
                    InputVariable = StringId.Invalid,
                    RangeVariable = StringId.Invalid,
                    Function = new TagFunction { Data = new byte[14] }
                },
            };

            // Plasma
            shit.Plasma = new ShieldImpact.PlasmaBlock
            {
                PlasmaNoiseBitmap1 = Noise1,
                PlasmaNoiseBitmap2 = Noise2,
                ScrollSpeed = shit.H3Values.ScrollSpeed,
                EdgeSharpness = shit.H3Values.PlasmaSharpness1,
                CenterSharpness = shit.H3Values.PlasmaSharpness2,

                PlasmaCenterColor = new ShieldImpactFunction
                {
                    InputVariable = cache.StringTable.GetStringId("shield_intensity"),
                    RangeVariable = StringId.Invalid,
                    Function = new TagFunction { Data = new byte[60] }
                },
                PlasmaCenterIntensity = new ShieldImpactFunction
                {
                    InputVariable = cache.StringTable.GetStringId("shield_vitality"),
                    RangeVariable = StringId.Invalid,
                    Function = new TagFunction { Data = new byte[60] }
                },
                PlasmaEdgeColor = new ShieldImpactFunction
                {
                    InputVariable = cache.StringTable.GetStringId("shield_intensity"),
                    RangeVariable = StringId.Invalid,
                    Function = new TagFunction { Data = new byte[84] }
                },
                PlasmaEdgeIntensity = new ShieldImpactFunction
                {
                    InputVariable = cache.StringTable.GetStringId("shield_vitality"),
                    RangeVariable = StringId.Invalid,
                    Function = new TagFunction { Data = new byte[60] }
                },
            };

            if (blamVersion <= CacheVersion.Halo3Retail)
                shit.Plasma.TilingScale = shit.H3Values.TextureScale;
            else if (blamVersion == CacheVersion.Halo3ODST)
                shit.Plasma.TilingScale = shit.H3Values.TextureScaleUV.I;

            // Extrusion Oscillation
            shit.ExtrusionOscillation = new ShieldImpact.ExtrusionOscillationBlock
            {
                OscillationBitmap1 = Noise1,
                OscillationBitmap2 = Noise2,
                OscillationScrollSpeed = shit.H3Values.ScrollSpeed,

                ExtrusionAmount = new ShieldImpactFunction
                {
                    InputVariable = cache.StringTable.GetStringId("shield_vitality"),
                    RangeVariable = cache.StringTable.GetStringId("recent_shield_vitality"),
                    Function = new TagFunction { Data = new byte[48] }
                },
                OscillationAmplitude = new ShieldImpactFunction
                {
                    InputVariable = cache.StringTable.GetStringId("shield_vitality"),
                    RangeVariable = StringId.Invalid,
                    Function = new TagFunction { Data = new byte[20] }
                },
            };

            if (blamVersion <= CacheVersion.Halo3Retail)
                shit.ExtrusionOscillation.OscillationTilingScale = shit.H3Values.TextureScale;
            else if (blamVersion == CacheVersion.Halo3ODST)
                shit.ExtrusionOscillation.OscillationTilingScale = shit.H3Values.TextureScaleUV.I;

            // Hit Response
            shit.HitResponse = new ShieldImpact.HitResponseBlock
            {
                HitTime = 2.857143f,

                HitColor = new ShieldImpactFunction
                {
                    InputVariable = StringId.Invalid,
                    RangeVariable = cache.StringTable.GetStringId("hit_time"),
                    Function = new TagFunction { Data = new byte[20] }
                },
                HitIntensity = new ShieldImpactFunction
                {
                    InputVariable = cache.StringTable.GetStringId("shield_vitality"),
                    RangeVariable = cache.StringTable.GetStringId("shield_vitality"),
                    Function = new TagFunction { Data = new byte[20] }
                },
            };

            // Scales, Offsets, Depth Fade
            shit.EdgeScales = new RealQuaternion(4, -2.857143f, 2, -4);
            shit.EdgeOffsets = new RealQuaternion(-1, 2.428571f, 0, 3);
            shit.PlasmaScales = new RealQuaternion(3, 3, -45, 60);
            shit.DepthFadeParameters = new RealQuaternion(5, 20, 0, 0);

            return shit;
        }
    }
}
