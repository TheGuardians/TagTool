using TagTool.Serialization;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "shader_zonly", Tag = "rmzo", Size = 0x8)]
    public class ShaderZonly : RenderMethod
    {
        public uint Unknown8;
        public uint Unknown9;
    }
}