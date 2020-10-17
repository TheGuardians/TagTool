using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "shader_template", Tag = "stem", Size = 0x60)]
    public class ShaderTemplate : TagStructure
    {
        public byte[] Documentation;
        public StringId DefaultMaterialName;
        /// <summary>
        /// * Force Active Camo: Should be used with cautuion, as this causes a backbuffer copy when this shader is rendered.
        /// *
        /// Water: ???.
        /// * Foliage: Used with lightmapped foliage (two-sided lighting) shaders. It affects importing but not
        /// rendering.
        /// </summary>
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public FlagsValue Flags;
        public List<ShaderTemplatePropertyBlock> Properties;
        public List<ShaderTemplateCategoryBlock> Categories;
        /// <summary>
        /// Not used anymore.
        /// </summary>
        [TagField(ValidTags = new [] { "slit" })]
        public CachedTag LightResponse;
        public List<ShaderTemplateLevelOfDetailBlock> Lods;
        public List<ShaderTemplateRuntimeExternalLightResponseIndexBlock> Unknown;
        public List<ShaderTemplateRuntimeExternalLightResponseIndexBlock1> Unknown1;
        /// <summary>
        /// Really cool stuff.
        /// </summary>
        [TagField(ValidTags = new [] { "shad" })]
        public CachedTag Aux1Shader;
        public Aux1LayerValue Aux1Layer;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        [TagField(ValidTags = new [] { "shad" })]
        public CachedTag Aux2Shader;
        public Aux2LayerValue Aux2Layer;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        public List<ShaderTemplatePostprocessDefinitionNewBlock> PostprocessDefinition;
        
        [Flags]
        public enum FlagsValue : ushort
        {
            ForceActiveCamo = 1 << 0,
            Water = 1 << 1,
            Foliage = 1 << 2,
            HideStandardParameters = 1 << 3
        }
        
        [TagStructure(Size = 0x8)]
        public class ShaderTemplatePropertyBlock : TagStructure
        {
            public PropertyValue Property;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public StringId ParameterName;
            
            public enum PropertyValue : short
            {
                Unused,
                DiffuseMap,
                LightmapEmissiveMap,
                LightmapEmissiveColor,
                LightmapEmissivePower,
                LightmapResolutionScale,
                LightmapHalfLife,
                LightmapDiffuseScale,
                LightmapAlphaTestMap,
                LightmapTranslucentMap,
                LightmapTranslucentColor,
                LightmapTranslucentAlpha,
                ActiveCamoMap,
                LightmapFoliageScale
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class ShaderTemplateCategoryBlock : TagStructure
        {
            public StringId Name;
            public List<ShaderTemplateParameterBlock> Parameters;
            
            [TagStructure(Size = 0x34)]
            public class ShaderTemplateParameterBlock : TagStructure
            {
                public StringId Name;
                public byte[] Explanation;
                public TypeValue Type;
                public FlagsValue Flags;
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag DefaultBitmap;
                public float DefaultConstValue;
                public RealRgbColor DefaultConstColor;
                public BitmapTypeValue BitmapType;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public BitmapAnimationFlagsValue BitmapAnimationFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public float BitmapScale;
                
                public enum TypeValue : short
                {
                    Bitmap,
                    Value,
                    Color,
                    Switch
                }
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    Animated = 1 << 0,
                    HideBitmapReference = 1 << 1
                }
                
                public enum BitmapTypeValue : short
                {
                    _2d,
                    _3d,
                    CubeMap
                }
                
                [Flags]
                public enum BitmapAnimationFlagsValue : ushort
                {
                    ScaleUniform = 1 << 0,
                    Scale = 1 << 1,
                    Translation = 1 << 2,
                    Rotation = 1 << 3,
                    Index = 1 << 4
                }
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class ShaderTemplateLevelOfDetailBlock : TagStructure
        {
            public float ProjectedDiameter; // pixels
            public List<ShaderTemplatePassReferenceBlock> Pass;
            
            [TagStructure(Size = 0x18)]
            public class ShaderTemplatePassReferenceBlock : TagStructure
            {
                public LayerValue Layer;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(ValidTags = new [] { "spas" })]
                public CachedTag Pass;
                [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                
                public enum LayerValue : short
                {
                    Texaccum,
                    EnvironmentMap,
                    SelfIllumination,
                    Overlay,
                    Transparent,
                    LightmapIndirect,
                    Diffuse,
                    Specular,
                    ShadowGenerate,
                    ShadowApply,
                    Boom,
                    Fog,
                    ShPrt,
                    ActiveCamo,
                    WaterEdgeBlend,
                    Decal,
                    ActiveCamoStencilModulate,
                    Hologram,
                    LightAlbedo
                }
            }
        }
        
        [TagStructure(Size = 0x4)]
        public class ShaderTemplateRuntimeExternalLightResponseIndexBlock : TagStructure
        {
            public int Unknown;
        }
        
        [TagStructure(Size = 0x4)]
        public class ShaderTemplateRuntimeExternalLightResponseIndexBlock1 : TagStructure
        {
            public int Unknown;
        }
        
        public enum Aux1LayerValue : short
        {
            Texaccum,
            EnvironmentMap,
            SelfIllumination,
            Overlay,
            Transparent,
            LightmapIndirect,
            Diffuse,
            Specular,
            ShadowGenerate,
            ShadowApply,
            Boom,
            Fog,
            ShPrt,
            ActiveCamo,
            WaterEdgeBlend,
            Decal,
            ActiveCamoStencilModulate,
            Hologram,
            LightAlbedo
        }
        
        public enum Aux2LayerValue : short
        {
            Texaccum,
            EnvironmentMap,
            SelfIllumination,
            Overlay,
            Transparent,
            LightmapIndirect,
            Diffuse,
            Specular,
            ShadowGenerate,
            ShadowApply,
            Boom,
            Fog,
            ShPrt,
            ActiveCamo,
            WaterEdgeBlend,
            Decal,
            ActiveCamoStencilModulate,
            Hologram,
            LightAlbedo
        }
        
        [TagStructure(Size = 0x28)]
        public class ShaderTemplatePostprocessDefinitionNewBlock : TagStructure
        {
            public List<ShaderTemplatePostprocessLevelOfDetailNewBlock> LevelsOfDetail;
            public List<TagBlockIndexBlock> Layers;
            public List<ShaderTemplatePostprocessPassNewBlock> Passes;
            public List<ShaderTemplatePostprocessImplementationNewBlock> Implementations;
            public List<ShaderTemplatePostprocessRemappingNewBlock> Remappings;
            
            [TagStructure(Size = 0xA)]
            public class ShaderTemplatePostprocessLevelOfDetailNewBlock : TagStructure
            {
                public TagBlockIndexStructBlock Layers;
                public int AvailableLayers;
                public float ProjectedHeightPercentage;
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndexStructBlock : TagStructure
                {
                    public short BlockIndexData;
                }
            }
            
            [TagStructure(Size = 0x2)]
            public class TagBlockIndexBlock : TagStructure
            {
                public TagBlockIndexStructBlock Indices;
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndexStructBlock : TagStructure
                {
                    public short BlockIndexData;
                }
            }
            
            [TagStructure(Size = 0xA)]
            public class ShaderTemplatePostprocessPassNewBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "spas" })]
                public CachedTag Pass;
                public TagBlockIndexStructBlock Implementations;
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndexStructBlock : TagStructure
                {
                    public short BlockIndexData;
                }
            }
            
            [TagStructure(Size = 0x6)]
            public class ShaderTemplatePostprocessImplementationNewBlock : TagStructure
            {
                public TagBlockIndexStructBlock Bitmaps;
                public TagBlockIndexStructBlock1 PixelConstants;
                public TagBlockIndexStructBlock2 VertexConstants;
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndexStructBlock : TagStructure
                {
                    public short BlockIndexData;
                }
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndexStructBlock1 : TagStructure
                {
                    public short BlockIndexData;
                }
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndexStructBlock2 : TagStructure
                {
                    public short BlockIndexData;
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class ShaderTemplatePostprocessRemappingNewBlock : TagStructure
            {
                [TagField(Length = 0x3)]
                public byte[] Unknown;
                public sbyte SourceIndex;
            }
        }
    }
}

