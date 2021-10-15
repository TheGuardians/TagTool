using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "render_method_definition", Size = 0x5C, Tag = "rmdf")]
    public class RenderMethodDefinition : TagStructure
	{
        public CachedTag GlobalOptions;
        public List<CategoryBlock> Categories;
        public List<EntryPointBlock> EntryPoints;
        public List<VertexBlock> VertexTypes;
        public CachedTag GlobalPixelShader;
        public CachedTag GlobalVertexShader;
        public int Flags; // UseAutomaticMacros
        public int Version;

        [TagStructure(Size = 0x18)]
        public class CategoryBlock : TagStructure
		{
            public StringId Name;
            public List<ShaderOption> ShaderOptions;
            public StringId VertexFunction;
            public StringId PixelFunction;

            [TagStructure(Size = 0x1C)]
            public class ShaderOption : TagStructure
			{
                public StringId Name;
                public CachedTag Option;
                public StringId VertexFunction;
                public StringId PixelFunction;
            }
        }

        [TagStructure(Size = 0x10)]
        public class EntryPointBlock : TagStructure
		{
            public uint EntryPoint;
            public List<PassBlock> Passes;

            [TagStructure(Size = 0x10)]
            public class PassBlock : TagStructure
			{
                public ushort Flags;
                [TagField(Flags = TagFieldFlags.Padding, Length = 0x2)]
                public byte[] Padding;
                public List<CategoryDependency> CategoryDependencies;

                [TagStructure(Size = 0x2)]
                public class CategoryDependency : TagStructure
				{
                    public ushort Category;
                }
            }
        }

        [TagStructure(Size = 0x10)]
        public class VertexBlock : TagStructure
		{
            public short VertexType;
            [TagField(Flags = TagFieldFlags.Padding, Length = 0x2)]
            public byte[] Padding;
            public List<EntryPointDependency> Dependencies;

            [TagStructure(Size = 0x10)]
            public class EntryPointDependency : TagStructure
            {
                public uint EntryPoint;
                public List<CategoryDependency> CategoryDependencies;

                [TagStructure(Size = 0x2)]
                public class CategoryDependency : TagStructure
                {
                    public ushort Category;
                }
            }
        }
    }
}