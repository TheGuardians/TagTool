using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "shader_cortana", Tag = "rmct", Size = 0x4)]
    public class ShaderCortana : RenderMethod
    {
        public StringId Material;
    }
}