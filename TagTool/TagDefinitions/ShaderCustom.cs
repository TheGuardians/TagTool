using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "shader_custom", Tag = "rmcs", Size = 0x4)]
    public class ShaderCustom : RenderMethod
    {
        public StringId Material;
    }
}