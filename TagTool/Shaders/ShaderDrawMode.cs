using TagTool.Serialization;

namespace TagTool.Shaders
{
    [TagStructure(Size = 0x2)]
    public class ShaderDrawMode
    {
        public byte Index;
        public byte Count;
    }
}