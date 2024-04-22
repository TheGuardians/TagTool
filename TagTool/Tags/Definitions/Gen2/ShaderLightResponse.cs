using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "shader_light_response", Tag = "slit", Size = 0x14)]
    public class ShaderLightResponse : TagStructure
    {
        public List<ShaderTemplateCategoryBlock> Categories;
        public List<ShaderTemplateLevelOfDetailBlock> ShaderLods;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        
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
    }
}

