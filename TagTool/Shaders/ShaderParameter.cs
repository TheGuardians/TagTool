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
        public RType RegisterType;

        public enum RType : byte
        {
            Boolean = 0,
            Integer = 1,
            Vector = 2,
            Sampler = 3
        }
    }
}
