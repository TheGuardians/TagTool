using System.Collections.Generic;
using TagTool.Common;

namespace TagTool.Tags.Definitions.Common
{
    [TagStructure(Size = 0x24)]
    public class RuntimeGpuData : TagStructure
    {
        public List<RealVector4dBlock> Properties;
        public List<GpuFunctionBlock> Functions;
        public List<RealVector4dBlock> Colors;

        [TagStructure(Size = 0x40, Align = 0x10)]
        public class GpuFunctionBlock : TagStructure
        {
            [TagField(Length = 4)]
            public RealVector4dBlock[] Elements;
        }

        [TagStructure(Size = 0x10, Align = 0x10)]
        public class RealVector4dBlock : TagStructure
        {
            public RealRgbColor Color;
            public float Magnitude;
        }
    }
}
