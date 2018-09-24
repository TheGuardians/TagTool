using TagTool.Serialization;

namespace TagTool.Shaders
{
    [TagStructure(Size = 0x2)]
    public class ShaderDrawMode : TagStructure
	{
        public byte Offset;
        public byte Count;
    }
}