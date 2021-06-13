using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "weather_globals", Tag = "wxcg", Size = 0x38)]
    public class WeatherGlobals : TagStructure
    {
        public List<GlobalTexturesRefsBlock> GlobalTextures;
        [TagField(ValidTags = new [] { "rain" })]
        public CachedTag DefaultRain;
        public RainRippleSettingBlock RainRippleSetting;
        
        [TagStructure(Size = 0x10)]
        public class GlobalTexturesRefsBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Texture;
        }
        
        [TagStructure(Size = 0x1C)]
        public class RainRippleSettingBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag RainRippleTexture;
            public float GroupPeriodTime; // seconds
            public float TextureTileSize; // world units
            public float ReflectionIntensify;
        }
    }
}
