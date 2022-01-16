using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "shader_custom", Tag = "rmcs", Size = 0x4)]
    public class ShaderCustom : RenderMethod
    {
        [TagField(Flags = TagFieldFlags.GlobalMaterial)]
        public StringId Material;
    }
}