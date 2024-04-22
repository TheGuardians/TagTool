using TagTool.Shaders;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "global_cache_file_pixel_shaders", Tag = "gpix", Size = 0x1C)]
    public class GlobalCacheFilePixelShaders : TagStructure
    {
        public uint Unknown0;
        public uint Count;
        public uint Unknown2;
        public uint Unknown3;
        public List<PixelShaderBlock> Shaders;
    }
}
