using TagTool.Shaders;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "pixel_shader", Tag = "pixl", Size = 0x20)]
    public class PixelShader : TagStructure
	{
        public EntryPointBitMask EntryPoints;
        public List<ShortOffsetCountBlock> EntryPointShaders;
        public int Version;
        public List<PixelShaderBlock> Shaders;
    }
}