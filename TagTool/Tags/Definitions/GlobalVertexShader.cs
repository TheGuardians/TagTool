using TagTool.Shaders;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "global_vertex_shader", Tag = "glvs", Size = 0x1C)]
    public class GlobalVertexShader : TagStructure
	{
        /// <summary>
        /// Indexed by <see cref="TagTool.Geometry.VertexType"/>
        /// </summary>
        public List<VertexTypeShaders> VertexTypes;
        public uint Version;
        public List<VertexShaderBlock> Shaders;

        [TagStructure(Size = 0xC)]
        public class VertexTypeShaders : TagStructure
		{
            /// <summary>
            /// Indexed by <see cref="TagTool.Shaders.EntryPoint"/>
            /// </summary>
            public List<DrawMode> DrawModes;

            [TagStructure(Size = 0x10, Platform = Cache.CachePlatform.Original)]
            [TagStructure(Size = 0x14, Platform = Cache.CachePlatform.MCC)]
            public class DrawMode : TagStructure
			{
                public List<CategoryDependencyBlock> CategoryDependency;
                public int ShaderIndex;

                [TagField(Platform = Cache.CachePlatform.MCC)]
                public int UnknownIndex;

                [TagStructure(Size = 0x10)]
                public class CategoryDependencyBlock : TagStructure
                {
                    public int DefinitionCategoryIndex;

                    [TagStructure(Size = 0x4)]
                    public class OptionDependencyBlock : TagStructure
                    {
                        public int CompiledShaderIndex;
                    }
                }
            }
        }
    }
}
