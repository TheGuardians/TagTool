using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "rasterizer_globals", Size = 0xA4, Tag = "rasg", MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "rasterizer_globals", Size = 0xAC, Tag = "rasg", MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "rasterizer_globals", Size = 0xBC, Tag = "rasg", MinVersion = CacheVersion.HaloOnlineED)]
    public class RasterizerGlobals : TagStructure
	{
        public List<DefaultBitmap> DefaultBitmaps;
        public List<DefaultRasterizerBitmap> DefaultRasterizerBitmaps;
        public CachedTag VertexShaderSimple;
        public CachedTag PixelShaderSimple;
        public List<ExplicitShader> DefaultShaders;
        public uint Unknown;
        public uint Unknown2;
        public uint Unknown3;
        public int Unknown4;
        public int Unknown5;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag ActiveCamoDistortion;
        public CachedTag DefaultPerformanceTemplate;
        public CachedTag DefaultShieldImpact;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public CachedTag CheapParticleTypeLibrary;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public CachedTag EmblemLibrary;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag DefaultVisionMode;

        [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail)]
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public int Unknown6; //6 

        [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail)]
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public MotionBlurParametersLegacyBlock MotionBlurParametersLegacy;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public MotionBlurParametersBlock MotionBlurParameters;

        [TagStructure(Size = 0x14)]
        public class DefaultBitmap : TagStructure
		{
            public DefaultBitmapFlags Flags;
            public CachedTag Bitmap;

            [Flags]
            public enum DefaultBitmapFlags : int
            {
                None = 0,
                DoNotLoad = 1 << 0,
            }

            public enum RasterizerDefaultBitmap
            {
                default_white,
                default_normal,
                default_cubemap,
                default_environment_map,
                color_bars,
                color_black,
                color_black_transparent,
                color_gray,
                auto_exposure_weight,
                auto_exposure_weight_4x3,
                stencil_dither_pattern,
                noise_warp,
                ripple_pattern,
            };

            public static bool ParseDefaultBitmap(string input, out RasterizerDefaultBitmap rasterizerDefaultBitmap)
            {
                if (Enum.TryParse(input, out rasterizerDefaultBitmap))
                    return true;

                switch (input)
                {
                    case "color_white":
                        rasterizerDefaultBitmap = RasterizerDefaultBitmap.default_white;
                        break;
                    case "default_vector":
                        rasterizerDefaultBitmap = RasterizerDefaultBitmap.default_normal;
                        break;
                    case "default_dynamic_cubemap":
                        rasterizerDefaultBitmap = RasterizerDefaultBitmap.default_cubemap;
                        break;
                    case "color_black_alpha_black":
                        rasterizerDefaultBitmap = RasterizerDefaultBitmap.color_black_transparent;
                        break;
                    case "gray_50_percent":
                        rasterizerDefaultBitmap = RasterizerDefaultBitmap.color_gray;
                        break;
                    case "dither_pattern2":
                        rasterizerDefaultBitmap = RasterizerDefaultBitmap.stencil_dither_pattern;
                        break;
                    case "random4_warp":
                        rasterizerDefaultBitmap = RasterizerDefaultBitmap.noise_warp;
                        break;
                    case "water_ripples":
                        rasterizerDefaultBitmap = RasterizerDefaultBitmap.ripple_pattern;
                        break;
                    default:
                        //Console.WriteLine("WARNING: Could not parse default bitmap, using default_white.");
                        rasterizerDefaultBitmap = RasterizerDefaultBitmap.default_white;
                        return false;
                }

                return true;
            }
        }

        [TagStructure(Size = 0x10)]
        public class DefaultRasterizerBitmap : TagStructure
		{
            public CachedTag Bitmap;
        }

        [TagStructure(Size = 0x20, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x24, MinVersion = CacheVersion.HaloReach)]
        public class ExplicitShader : TagStructure
		{
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public StringId Name;

            public CachedTag VertexShader;
            public CachedTag PixelShader;
        }

        [TagStructure(Size = 0x18)]
        public class MotionBlurParametersLegacyBlock : TagStructure
        {
            public float MaxBlur;
            public float Unknown;
            public float BlurScale;
            public float CenterFalloffX;
            public float CenterFalloffY;
            public float ExpectedTimePerTick;
        }

        [TagStructure(Size = 0x18)]
        public class MotionBlurParametersBlock : TagStructure
        {
            public float MaxBlur;
            public float BlurScale;
            public float CenterFalloffX;
            public float CenterFalloffY;
            public float ExpectedTimePerTick;
            public float Unknown;
        }
    }
}