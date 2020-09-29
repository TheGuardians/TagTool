using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "shader_light_response", Tag = "slit", Size = 0x1C)]
    public class ShaderLightResponse : TagStructure
    {
        public List<ShaderTemplateCategory> Categories;
        public List<ShaderTemplateLevelOfDetail> ShaderLods;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding2;
        
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
    }
}

