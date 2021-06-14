using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "shader_particle", Tag = "rmp", Size = 0x34)]
    public class ShaderParticle : RenderMethod
    {
        public RealRgbColor BrightTint;
        public RealRgbColor AmbientTint;
        public float Contrast;
        public float BlurWeight;
        public float IntensityScale;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Palette;
    }
}
