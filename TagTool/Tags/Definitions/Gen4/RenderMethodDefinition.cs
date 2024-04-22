using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "render_method_definition", Tag = "rmdf", Size = 0x15C)]
    public class RenderMethodDefinition : TagStructure
    {
        [TagField(ValidTags = new [] { "rmop" })]
        public CachedTag GlobalOptions;
        public List<RenderMethodCategoryBlock> Categories;
        public List<RenderMethodEntryPointsBlock> EntryPoints;
        public List<VertexTypesBlock> VertexTypes;
        [TagField(ValidTags = new [] { "glps" })]
        public CachedTag SharedPixelShaders;
        [TagField(ValidTags = new [] { "glvs" })]
        public CachedTag SharedVertexShaders;
        public RenderMethodDefinitionFlags Flags;
        public uint Version; // bump to force recompile
        [TagField(Length = 256)]
        public string Location;
        
        [Flags]
        public enum RenderMethodDefinitionFlags : uint
        {
            UseAutomaticMacros = 1 << 0,
            BuildConstantTableInShader = 1 << 1
        }
        
        [TagStructure(Size = 0x18)]
        public class RenderMethodCategoryBlock : TagStructure
        {
            public StringId CategoryName;
            public List<RenderMethodOptionsBlock> Options;
            public StringId VertexFunction;
            public StringId PixelFunction;
            
            [TagStructure(Size = 0x1C)]
            public class RenderMethodOptionsBlock : TagStructure
            {
                public StringId OptionName;
                [TagField(ValidTags = new [] { "rmop" })]
                public CachedTag Option;
                public StringId VertexFunction;
                public StringId PixelFunction;
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class RenderMethodEntryPointsBlock : TagStructure
        {
            public EntryPointEnum EntryPoint;
            public List<RenderMethodPassBlock> Passes;
            
            public enum EntryPointEnum : int
            {
                Default,
                Albedo,
                StaticPerPixel,
                StaticPerPixelHybridRefinement,
                StaticPerPixelAnalytic,
                StaticPerPixelAnalyticHybridRefinement,
                StaticPerPixelFloatingShadow,
                StaticPerVertex,
                StaticProbe,
                StaticPerPixelForge,
                StaticPerPixelObject,
                StaticPerVertexObject,
                DynamicLight,
                ShadowGenerate,
                ShadowApply,
                ActiveCamo,
                LightmapDebugMode,
                VertexColorLighting,
                WaterTessellation,
                WaterShading,
                Unused2,
                SinglePassPerPixel,
                SinglePassPerVertex,
                SinglePassSingleProbe,
                SinglePassAsDecal,
                MidnightSpotlight,
                MidnightSpotlightTransparents,
                MotionBlur,
                VolumeFogStencil,
                VolumeFogDepth,
                CurvedCui,
                SinglePassShadowedNoFogPerPixel,
                SinglePassShadowedNoFogPerVertex,
                SinglePassShadowedNoFogSingleProbe,
                StaticPerPixelFloatingShadowSimple,
                StaticPerPixelSimple,
                StaticPerPixelAo,
                StaticPerVertexAo,
                StaticLitCui,
                CurvedStaticLitCui
            }
            
            [TagStructure(Size = 0x1C)]
            public class RenderMethodPassBlock : TagStructure
            {
                public RenderMethodPassFlags Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<RenderMethodPassCategoryDependencies> CategoryDependencies;
                public List<RenderMethodPassCategoryDependencies> SharedVsCategoryDependencies;
                
                [Flags]
                public enum RenderMethodPassFlags : ushort
                {
                    SharedEntryPointCompilation = 1 << 0,
                    SharedVsOnlyCareNonDefaultOptionOfDependedCategory = 1 << 1,
                    OnlyBeCompiledToXenonPlatform = 1 << 2,
                    AllowFailedShaderCompile = 1 << 3
                }
                
                [TagStructure(Size = 0x2)]
                public class RenderMethodPassCategoryDependencies : TagStructure
                {
                    public short Category;
                }
            }
        }
        
        [TagStructure(Size = 0x4)]
        public class VertexTypesBlock : TagStructure
        {
            public VertexTypesNamesEnum VertexType;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum VertexTypesNamesEnum : short
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
                Rigid2uv,
                LightVolume,
                ChudSimple,
                ChudFancy,
                Decorator,
                TinyPosition,
                PatchyFog,
                Water,
                Ripple,
                ImplicitGeometry,
                Skinned2uv,
                WorldTessellated,
                RigidTessellated,
                SkinnedTessellated,
                ShaderCache,
                StructureInstanceImposter,
                ObjectImposter,
                RigidCompressed,
                SkinnedUncompressed,
                LightVolumePrecompiled,
                BlendshapeRigid,
                BlendshapeRigidBlendshaped,
                RigidBlendshaped,
                BlendshapeSkinned,
                BlendshapeSkinnedBlendshaped,
                SkinnedBlendshaped,
                VirtualGeometryHwtess,
                VirtualGeometryMemexport,
                PositionOnly,
                VirtualGeometryDebug,
                BlendshapeRigidCompressedPosition,
                SkinnedUncompressedPositionBlendshaped,
                BlendshapeSkinnedUncompressedPosition,
                Tracer,
                Polyart,
                Vectorart,
                RigidBoned,
                RigidBoned2uv,
                BlendshapeSkinned2uv,
                BlendshapeSkinned2uvBlendshaped,
                Skinned2uvBlendshaped,
                Polyartuv,
                BlendshapeSkinnedUncompressedPositionBlendshaped
            }
        }
    }
}
