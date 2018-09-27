using TagTool.Tags;

namespace TagTool.Shaders
{
    [TagStructure(Size = 0x8)]
    public class ShaderCodeHeader : TagStructure
	{
        public uint ConstantDataSize;
        public uint CodeDataSize;
    }
}
