using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "shader_glass", Tag = "rmgl", Size = 0x4)]
    public class ShaderGlass : RenderMethod
    {
        public StringId Material;
    }
}