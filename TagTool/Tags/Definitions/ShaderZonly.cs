using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "shader_zonly", Tag = "rmzo", Size = 0x8)]
    public class ShaderZonly : RenderMethod
    {
        public StringId Material;
        public uint Unknown9;
    }
}