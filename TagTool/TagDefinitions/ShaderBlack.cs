using TagTool.Serialization;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "shader_black", Tag = "rmbk", Size = 0x4)]
    public class ShaderBlack : RenderMethod
    {
        public uint Unknown1;
    }
}