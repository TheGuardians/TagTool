using System.Collections.Generic;
using TagTool.Common;

namespace TagTool.Tags.Definitions.Common
{
    [TagStructure(Size = 0x24)]
    public class RuntimeGpuData : TagStructure
    {
        public List<RealVector4dBlock> Properties;
        public List<RealVector4dBlock> Functions;
        public List<RealVector4dBlock> Colors;

        [TagStructure(Size = 0x10)]
        public class RealVector4dBlock : TagStructure
        {
            public RealVector3d Color;
            public float Magnitude;
        }
    }
}
