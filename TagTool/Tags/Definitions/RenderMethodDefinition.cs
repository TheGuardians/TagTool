using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "render_method_definition", Size = 0x5C, Tag = "rmdf")]
    public class RenderMethodDefinition : TagStructure
	{
        public CachedTag RenderMethodOptions;
        public List<Method> Methods;
        public List<DrawMode> DrawModes;
        public List<VertexBlock> Vertices;
        public CachedTag GlobalPixelShader;
        public CachedTag GlobalVertexShader;
        public int Unknown2;
        public int Unknown3;

        [TagStructure(Size = 0x18)]
        public class Method : TagStructure
		{
            public StringId Type;
            public List<ShaderOption> ShaderOptions;
            public StringId VertexShaderMethodMacroName;
            public StringId PixelShaderMethodMacroName;

            [TagStructure(Size = 0x1C)]
            public class ShaderOption : TagStructure
			{
                public StringId Type;
                public CachedTag Option;
                public StringId VertexShaderOptionMacroName;
                public StringId PixelShaderOptionMacroName;
            }
        }

        [TagStructure(Size = 0x10)]
        public class DrawMode : TagStructure
		{
            public int Mode;
            public List<UnknownBlock2> Unknown2;

            [TagStructure(Size = 0x10)]
            public class UnknownBlock2 : TagStructure
			{
                public uint Unknown;
                public List<UnknownBlock> Unknown2;

                [TagStructure(Size = 0x4)]
                public class UnknownBlock : TagStructure
				{
                    public uint Unknown;
                }
            }
        }

        [TagStructure(Size = 0x10)]
        public class VertexBlock : TagStructure
		{
            public short VertexType;
            public short Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
        }
    }
}