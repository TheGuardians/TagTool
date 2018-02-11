using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Shaders
{
    [TagStructure(Size = 0x8)]
    public class ShaderParameter
    {
        public StringId ParameterName;
        public ushort RegisterIndex;
        public byte RegisterCount;
        public ShaderParameterRegisterType RegisterType;
    }
}