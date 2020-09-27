using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "shader_template", Tag = "stem", Size = 0x9C)]
    public class ShaderTemplate : TagStructure
    {
        public byte[] Documentation;
        public StringId DefaultMaterialName;
        /// <summary>
        /// FLAGS
        /// </summary>
        /// <remarks>
        /// * Force Active Camo: Should be used with cautuion, as this causes a backbuffer copy when this shader is rendered.
        /// * Water: ???.
        /// * Foliage: Used with lightmapped foliage (two-sided lighting) shaders. It affects importing but not rendering.
        /// </remarks>
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        public FlagsValue Flags;
        public List<ShaderTemplateProperty> Properties;
        public List<ShaderTemplateCategory> Categories;
        /// <summary>
        /// LIGHT RESPONSE
        /// </summary>
        /// <remarks>
        /// Not used anymore.
        /// </remarks>
        public CachedTag LightResponse;
        public List<ShaderTemplateLevelOfDetail> Lods;
        public List<ShaderTemplateRuntimeExternalLightResponseIndexBlock> Unknown1;
        public List<ShaderTemplateRuntimeExternalLightResponseIndexBlock> Unknown2;
        /// <summary>
        /// RECURSIVE RENDERING
        /// </summary>
        /// <remarks>
        /// Really cool stuff.
        /// </remarks>
        public CachedTag Aux1Shader;
        public Aux1LayerValue Aux1Layer;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding2;
        public CachedTag Aux2Shader;
        public Aux2LayerValue Aux2Layer;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding3;
        public List<ShaderTemplatePostprocessDefinitionNew> PostprocessDefinition;
        
        [Flags]
        public enum FlagsValue : ushort
        {
            ForceActiveCamo = 1 << 0,
            Water = 1 << 1,
            Foliage = 1 << 2,
            HideStandardParameters = 1 << 3
        }
        
        [TagStructure(Size = 0x8)]
        public class ShaderTemplateProperty : TagStructure
        {
            public PropertyValue Property;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
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
        
        [TagStructure(Size = 0x10)]
        public class ShaderTemplateCategory : TagStructure
        {
            public StringId Name;
            public List<ShaderTemplateParameter> Parameters;
            
            [TagStructure(Size = 0x48)]
            public class ShaderTemplateParameter : TagStructure
            {
                public StringId Name;
                public byte[] Explanation;
                public TypeValue Type;
                public FlagsValue Flags;
                public CachedTag DefaultBitmap;
                public float DefaultConstValue;
                public RealRgbColor DefaultConstColor;
                public BitmapTypeValue BitmapType;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public BitmapAnimationFlagsValue BitmapAnimationFlags;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding2;
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
        
        [TagStructure(Size = 0x10)]
        public class ShaderTemplateLevelOfDetail : TagStructure
        {
            public float ProjectedDiameter; // pixels
            public List<ShaderTemplatePassReference> Pass;
            
            [TagStructure(Size = 0x20)]
            public class ShaderTemplatePassReference : TagStructure
            {
                public LayerValue Layer;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public CachedTag Pass;
                [TagField(Flags = Padding, Length = 12)]
                public byte[] Padding2;
                
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
            public int Unknown1;
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
        
        [TagStructure(Size = 0x3C)]
        public class ShaderTemplatePostprocessDefinitionNew : TagStructure
        {
            public List<ShaderTemplatePostprocessLevelOfDetailNew> LevelsOfDetail;
            public List<TagBlockIndex> Layers;
            public List<ShaderTemplatePostprocessPassNew> Passes;
            public List<ShaderTemplatePostprocessImplementationNew> Implementations;
            public List<ShaderTemplatePostprocessRemappingNew> Remappings;
            
            [TagStructure(Size = 0xA)]
            public class ShaderTemplatePostprocessLevelOfDetailNew : TagStructure
            {
                public TagBlockIndex Layers;
                public int AvailableLayers;
                public float ProjectedHeightPercentage;
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndex : TagStructure
                {
                    public short BlockIndexData;
                }
            }
            
            [TagStructure(Size = 0x2)]
            public class TagBlockIndex : TagStructure
            {
                public TagBlockIndex Indices;
            }
            
            [TagStructure(Size = 0x12)]
            public class ShaderTemplatePostprocessPassNew : TagStructure
            {
                public CachedTag Pass;
                public TagBlockIndex Implementations;
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndex : TagStructure
                {
                    public short BlockIndexData;
                }
            }
            
            [TagStructure(Size = 0x6)]
            public class ShaderTemplatePostprocessImplementationNew : TagStructure
            {
                public TagBlockIndex Bitmaps;
                public TagBlockIndex PixelConstants;
                public TagBlockIndex VertexConstants;
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndex : TagStructure
                {
                    public short BlockIndexData;
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class ShaderTemplatePostprocessRemappingNew : TagStructure
            {
                [TagField(Flags = Padding, Length = 3)]
                public byte[] Unknown1;
                public sbyte SourceIndex;
            }
        }
    }
}

