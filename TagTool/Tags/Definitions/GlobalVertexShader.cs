using TagTool.Shaders;
using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "global_vertex_shader", Tag = "glvs", Size = 0x1C)]
    public class GlobalVertexShader : TagStructure
	{
        public List<VertexTypeShaders> VertexTypes;
        public uint Version;
        public List<VertexShaderBlock> Shaders;

        [TagStructure(Size = 0xC)]
        public class VertexTypeShaders : TagStructure
		{
            public List<GlobalShaderEntryPointBlock> EntryPoints;

            [TagStructure(Size = 0x10, Platform = CachePlatform.Original)]
            [TagStructure(Size = 0x14, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.MCC)]
            [TagStructure(Size = 0x10, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.MCC)]
            public class GlobalShaderEntryPointBlock : TagStructure
			{
                public List<CategoryDependencyBlock> CategoryDependency;
                public int ShaderIndex;
                [TagField(MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.MCC)]
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
}
