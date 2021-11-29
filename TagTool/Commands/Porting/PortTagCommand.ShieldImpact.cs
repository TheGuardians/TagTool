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
            CachedTag noise1 = shit.H3Values.ShieldImpactNoiseTexture1;
            CachedTag noise2 = shit.H3Values.ShieldImpactNoiseTexture2;

            StringId shield_intensity = CacheContext.StringTable.GetStringId("shield_intensity");
            StringId recent_shield_intensity = CacheContext.StringTable.GetStringId("recent_shield_intensity");
            StringId recent_shield_damage = CacheContext.StringTable.GetStringId("recent_shield_damage");
            StringId object_overshield_amount = CacheContext.StringTable.GetStringId("object_overshield_amount");
            StringId one = CacheContext.StringTable.GetStringId("one");

            RealVector2d tilingUV = blamVersion == CacheVersion.Halo3ODST ? shit.H3Values.TextureScaleUV : new RealVector2d(shit.H3Values.TextureScale);

            shit.Version = 4;

            // Shield Intensity
            shit.ShieldIntensity = new ShieldImpact.ShieldIntensityBlock
            {
                RecentDamageIntensity = 1.0f,
                CurrentDamageIntensity = 1.0f
            };

            // Shield Edge (Not present in H3, so we should not render this)
            shit.ShieldEdge = new ShieldImpact.ShieldEdgeBlock
            {
                //OvershieldScale = isOvershield ? 1.0f : 0.0f;
                EdgeGlowColor = new ShieldImpactFunction
                {
                    InputVariable = StringId.Invalid,
                    RangeVariable = StringId.Invalid,
                    Function = new TagFunction
                    {
                        Data = new byte[] { 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 } // black
                    }
                },
                EdgeGlowIntensity = new ShieldImpactFunction
                {
                    InputVariable = StringId.Invalid,
                    RangeVariable = StringId.Invalid,
                    Function = new TagFunction
                    {
                        Data = new byte[] { 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, } // zero intensity
                    }
                },
            };

            float magnitude = shit.H3Values.ImpactAmbientIntensity;
            //if (isOvershield)
            //    magnitude = shit.H3Values.OvershieldAmbientIntensity;

            byte[] magnitudeBytes = System.BitConverter.GetBytes(magnitude);

            ArgbColor plasmaColor = new ArgbColor
            {
                Alpha = 0xFF,
                Red = (byte)(shit.H3Values.ImpactAmbientColor.Red * 255.0f),
                Green = (byte)(shit.H3Values.ImpactAmbientColor.Green * 255.0f),
                Blue = (byte)(shit.H3Values.ImpactAmbientColor.Blue * 255.0f)
            };

            // Plasma
            shit.Plasma = new ShieldImpact.PlasmaBlock
            {
                PlasmaNoiseBitmap1 = noise1,
                PlasmaNoiseBitmap2 = noise2,
                TilingScale = tilingUV.I,
                ScrollSpeed = 1.0f / shit.H3Values.ScrollSpeed,

                // TODO: improve
                EdgeSharpness = shit.H3Values.PlasmaSharpness2,
                CenterSharpness = shit.H3Values.PlasmaSharpness1,

                // No depth fade
                PlasmaDepthFadeRange = 0.0f,

                // Adjust these values as needed
                PlasmaOuterFadeRadius = 0.0f,
                PlasmaCenterRadius = 0.25f,
                PlasmaInnerFadeRadius = 0.5f,

                PlasmaCenterColor = new ShieldImpactFunction
                {
                    InputVariable = shield_intensity,
                    RangeVariable = /*isOvershield ? object_overshield_amount : */recent_shield_damage,
                    Function = new TagFunction // Can simplify this
                    {
                        Data = new byte[] { 0x08, 0x34, 0x02, 0x00, 0x00, 0x00, 0x00, 0xFF,
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, plasmaColor.Blue,
                            plasmaColor.Green, plasmaColor.Red, plasmaColor.Alpha,
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            0x00, 0x01, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0xCD, 0xFF, 0xFF,
                            0x7F, 0x7F, 0xC4, 0x40, 0x67, 0x3F, 0xE0, 0xF9, 0xC5, 0x3D, 0x00,
                            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }
                    }
                },
                PlasmaCenterIntensity = new ShieldImpactFunction
                {
                    InputVariable = recent_shield_intensity,
                    RangeVariable = /*isOvershield ? StringId.Invalid : */recent_shield_damage,
                    Function = new TagFunction
                    {
                        Data = new byte[] { 0x08, 0x00, 0x00, 0x00, magnitudeBytes[3], magnitudeBytes[2], magnitudeBytes[1], magnitudeBytes[0] }
                    }
                },
                PlasmaEdgeColor = new ShieldImpactFunction
                {
                    InputVariable = StringId.Invalid,
                    RangeVariable = StringId.Invalid,
                    Function = new TagFunction
                    {
                        Data = new byte[] { 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 } // black 
                    }
                },
                PlasmaEdgeIntensity = new ShieldImpactFunction
                {
                    InputVariable = StringId.Invalid,
                    RangeVariable = StringId.Invalid,
                    Function = new TagFunction // zero intensity
                    {
                        Data = new byte[] { 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, }
                    }
                },
            };

            // Might need conversion?
            float extrusion = shit.H3Values.ExtrusionDistance;
            byte[] extrusionBytes = System.BitConverter.GetBytes(extrusion);

            shit.ExtrusionOscillation = new ShieldImpact.ExtrusionOscillationBlock
            {
                OscillationBitmap1 = noise1,
                OscillationBitmap2 = noise2,
                OscillationScrollSpeed = 0.0f,
                OscillationTilingScale = 0.0f,

                // Min\Max value (0.0f <-> extrusion), interpolated by the input and range variables
                // If the input is always one, this function behaves like H3

                ExtrusionAmount = new ShieldImpactFunction
                {
                    InputVariable = one,
                    Function = new TagFunction
                    {
                        Data = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                            extrusionBytes[3], extrusionBytes[2], extrusionBytes[1], extrusionBytes[0]
                        }
                    }
                },

                OscillationAmplitude = new ShieldImpactFunction()
            };

            // Hit Response (only used when rendering as effect)
            shit.HitResponse = new ShieldImpact.HitResponseBlock
            {
                HitColor = new ShieldImpactFunction(),
                HitIntensity = new ShieldImpactFunction()
            };

            shit.UpdateParameters();

            if (blamVersion == CacheVersion.Halo3ODST)
            {
                float plasmaPowerScale = shit.PlasmaScales.K;
                float plasmaPowerOffset = shit.PlasmaScales.W;
                shit.PlasmaScales = new RealQuaternion(tilingUV, plasmaPowerScale, plasmaPowerOffset);
            }

            return shit;
        }
    }
}
