using TagTool.Cache;
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
            public int Unknown;
            public CachedTag Bitmap;
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