using TagTool.Cache;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "rasterizer_globals", Size = 0xA4, Tag = "rasg", MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "rasterizer_globals", Size = 0xAC, Tag = "rasg", MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "rasterizer_globals", Size = 0xBC, Tag = "rasg", MinVersion = CacheVersion.HaloOnline106708)]
    public class RasterizerGlobals : TagStructure
	{
        public List<DefaultBitmap> DefaultBitmaps;
        public List<DefaultRasterizerBitmap> DefaultRasterizerBitmaps;
        public CachedTag VertexShaderSimple;
        public CachedTag PixelShaderSimple;
        public List<DefaultShader> DefaultShaders;
        public uint Unknown;
        public uint Unknown2;
        public uint Unknown3;
        public int Unknown4;
        public int Unknown5;
        public CachedTag ActiveCamoDistortion;
        public CachedTag DefaultPerformanceTemplate;
        public CachedTag DefaultShieldImpact;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public CachedTag DefaultVisionMode;

        [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail)]
        public int Unknown6;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public int Unknown6HO; //6 

        public float Unknown7;
        public float Unknown8;
        public float Unknown9;
        public float Unknown10;

        public float Unknown11;
        public float Unknown12;

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
                    case "color_vector":
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

        [TagStructure(Size = 0x20)]
        public class DefaultShader : TagStructure
		{
            public CachedTag VertexShader;
            public CachedTag PixelShader;
        }
    }
}