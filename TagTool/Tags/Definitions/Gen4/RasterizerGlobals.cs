using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "rasterizer_globals", Tag = "rasg", Size = 0xC8)]
    public class RasterizerGlobals : TagStructure
    {
        public List<DefaultTexturesRefsBlock> DefaultBitmaps;
        public List<MaterialTexturesRefsBlock> MaterialTextures;
        public List<ExplicitShaderRefsBlock> ExplicitShaders;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag ActiveCamoDistortionTexture;
        [TagField(ValidTags = new [] { "perf" })]
        public CachedTag DefaultPerformanceThrottles;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag LogoTexture;
        [TagField(ValidTags = new [] { "cptl" })]
        public CachedTag CheapParticleTypeLibrary;
        [TagField(ValidTags = new [] { "mlib" })]
        public CachedTag EmblemLibrary;
        // max amount to blur, as a percentage of the screen
        public float MaxBlur; // [0 - 0.2]
        // scales blur for a given velocity
        public float BlurScale; // [0 - 0.5]
        // larger values make smaller areas of no blur
        public float CenterFalloffX; // [0 - 20]
        // larger values make smaller areas of no blur
        public float CenterFalloffY; // [0 - 20]
        // for all screen space light without shader reference
        public float CheapAlbedoBlend;
        // the floating point values are linear and what the shader will sample for albedo
        public RealRgbColor LightingLayerAlbedoColor;
        [TagField(ValidTags = new [] { "cfxs" })]
        public CachedTag HologramCameraFx;
        [TagField(ValidTags = new [] { "ldsc" })]
        public CachedTag GlobalLoadScreenModel;
        public float GlobalLoadScreenGradientCoordinate;
        [TagField(ValidTags = new [] { "mode" })]
        public CachedTag AirprobeRenderModel;
        
        [TagStructure(Size = 0x14)]
        public class DefaultTexturesRefsBlock : TagStructure
        {
            public GlobalBitmapFlags Options;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag DefaultBitmaps;
            
            [Flags]
            public enum GlobalBitmapFlags : uint
            {
                DonTLoadBitmapByDefault = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class MaterialTexturesRefsBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag MaterialTextures;
        }
        
        [TagStructure(Size = 0x24)]
        public class ExplicitShaderRefsBlock : TagStructure
        {
            public StringId Name;
            [TagField(ValidTags = new [] { "mat ","mats","vtsh" })]
            public CachedTag ExplicitVertexShader;
            [TagField(ValidTags = new [] { "mat ","mats","pixl" })]
            public CachedTag ExplicitPixelShader;
        }
    }
}
