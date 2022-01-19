using TagTool.Shaders;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "global_pixel_shader", Tag = "glps", Size = 0x1C)]
    public class GlobalPixelShader : TagStructure
	{
        public List<EntryPointBlock> EntryPoints;
        public uint Version;
        public List<PixelShaderBlock> Shaders;

        [TagStructure(Size = 0x10, Platform = Cache.CachePlatform.Original)]
        [TagStructure(Size = 0x14, MaxVersion = Cache.CacheVersion.Halo3ODST, Platform = Cache.CachePlatform.MCC)]
        [TagStructure(Size = 0x10, MinVersion = Cache.CacheVersion.HaloReach, Platform = Cache.CachePlatform.MCC)]
        public class EntryPointBlock : TagStructure
		{
            public List<CategoryDependencyBlock> CategoryDependency;
            public int DefaultCompiledShaderIndex;
            [TagField(Platform = Cache.CachePlatform.MCC, MaxVersion = Cache.CacheVersion.Halo3ODST)]
            public int CustomCompiledShaderIndex;

            [TagStructure(Size = 0x10)]
            public class CategoryDependencyBlock : TagStructure
			{
                public int DefinitionCategoryIndex;
                public List<GlobalShaderOptionDependency> OptionDependency;

                [TagStructure(Size = 0x4)]
                public class GlobalShaderOptionDependency : TagStructure
                {
                    public int CompiledShaderIndex;
                }
            }
        }
    }
}
