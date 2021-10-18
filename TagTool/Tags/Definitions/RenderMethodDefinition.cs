using TagTool.Cache;
using TagTool.Common;
using TagTool.Shaders;
using System.Collections.Generic;
using System;

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
        public RenderMethodDefinitionFlags Flags;
        public uint Version;
        [TagField(Length = 256, MinVersion = CacheVersion.HaloReach)]
        public string Location;

        [TagStructure(Size = 0x18)]
        public class CategoryBlock : TagStructure
        {
            [TagField(Flags = TagFieldFlags.Label)]
            public StringId Name;
            public List<ShaderOption> ShaderOptions;
            public StringId VertexFunction;
            public StringId PixelFunction;

            [TagStructure(Size = 0x1C)]
            public class ShaderOption : TagStructure
            {
                [TagField(Flags = TagFieldFlags.Label)]
                public StringId Name;
                public CachedTag Option;
                public StringId VertexFunction;
                public StringId PixelFunction;
            }
        }

        [TagStructure(Size = 0x10)]
        public class EntryPointBlock : TagStructure
        {
            [TagField(MaxVersion = CacheVersion.HaloOnline235640)]
            public EntryPoint_32 EntryPoint;
            [TagField(MinVersion = CacheVersion.HaloOnline301003, MaxVersion = CacheVersion.HaloOnline700123)]
            public EntryPointMs30_32 EntryPointHO;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public EntryPointReach_32 EntryPointReach;

            public List<PassBlock> Passes;

            [TagStructure(Size = 0x10, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x1C, MinVersion = CacheVersion.HaloReach)]
            public class PassBlock : TagStructure
            {
                public PassFlags Flags;
                [TagField(Flags = TagFieldFlags.Padding, Length = 0x2)]
                public byte[] Padding;
                public List<CategoryDependency> CategoryDependencies;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public List<CategoryDependency> SharedVSCategoryDependencies;

                [Flags]
                public enum PassFlags : ushort
                {
                    None,
                    HasSharedPixelShader
                }

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
            [TagField(Flags = TagFieldFlags.Label)]
            public VertexTypeValue VertexType;
            [TagField(Flags = TagFieldFlags.Padding, Length = 0x2)]
            public byte[] Padding;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public List<EntryPointDependency> Dependencies;

            public enum VertexTypeValue : ushort
            {
                World,
                Rigid,
                Skinned,
                ParticleModel,
                FlatWorld,
                FlatRigid,
                FlatSkinned,
                Screen,
                Debug,
                Transparent,
                Particle,
                Contrail,
                LightVolume,
                SimpleChud,
                FancyChud,
                Decorator,
                TinyPosition,
                PatchyFog,
                Water,
                Ripple,
                Implicit,
                Beam,
                DualQuat
            }

            [TagStructure(Size = 0x10)]
            public class EntryPointDependency : TagStructure
            {
                [TagField(MaxVersion = CacheVersion.HaloOnline235640)]
                public EntryPoint_32 EntryPoint;
                [TagField(MinVersion = CacheVersion.HaloOnline301003, MaxVersion = CacheVersion.HaloOnline700123)]
                public EntryPointMs30_32 EntryPointHO;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public EntryPointReach_32 EntryPointReach;

                public List<CategoryDependency> CategoryDependencies;

                [TagStructure(Size = 0x2)]
                public class CategoryDependency : TagStructure
                {
                    public ushort Category;
                }
            }
        }

        [Flags]
        public enum RenderMethodDefinitionFlags : uint
        {
            None,
            UseAutomaticMacros
        }
    }
}