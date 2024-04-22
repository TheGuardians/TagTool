using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "rasterizer_cache_file_globals", Tag = "draw", Size = 0xC)]
    public class RasterizerCacheFileGlobals : TagStructure
    {
        public int TextureHeaderCount;
        public int MaximumVertexShaderGprs;
        public int MaximumPixelShaderGprs;
    }
}
