using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "render_method_definition", Size = 0x5C, Tag = "rmdf", MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "render_method_definition", Size = 0x15C, Tag = "rmdf", MinVersion = CacheVersion.HaloReach)]
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
        [TagField(Length = 256, MinVersion = CacheVersion.HaloReach)]
        public string Location;

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

            [TagStructure(Size = 0x10, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x1C, MinVersion = CacheVersion.HaloReach)]
            public class PassBlock : TagStructure
            {
                public ushort Flags;
                [TagField(Flags = TagFieldFlags.Padding, Length = 0x2)]
                public byte[] Padding;
                public List<CategoryDependency> CategoryDependencies;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public List<CategoryDependency> SharedVSCategoryDependencies;

                [TagStructure(Size = 0x2)]
                public class CategoryDependency : TagStructure
				{
                    public ushort Category;
                }
            }
        }

        [TagStructure(Size = 0x10, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x4, MinVersion = CacheVersion.HaloReach)]
        public class VertexBlock : TagStructure
        {
            public short VertexType;
            [TagField(Flags = TagFieldFlags.Padding, Length = 0x2)]
            public byte[] Padding;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
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