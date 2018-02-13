using TagTool.Serialization;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "shader_halogram", Tag = "rmhg", Size = 0x4)]
    public class ShaderHalogram : RenderMethod
    {
        public uint Unknown8;
    }
}