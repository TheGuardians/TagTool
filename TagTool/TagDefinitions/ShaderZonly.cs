using TagTool.Serialization;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "shader_zonly", Tag = "rmzo", Size = 0x8)]
    public class ShaderZonly : RenderMethod
    {
        public uint Unknown8;
        public uint Unknown9;
    }
}