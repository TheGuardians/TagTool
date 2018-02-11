using TagTool.Serialization;
using TagTool.Shaders;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "pixel_shader", Tag = "pixl", Size = 0x20)]
    public class PixelShader
    {
        public uint Unknown;
        public List<ShaderDrawMode> DrawModes;
        public uint Unknown3;
        public List<ShaderData> Shaders;
    }
}