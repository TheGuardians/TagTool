using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "curve_scalar", Tag = "crvs", Size = 0x14)]
    public class CurveScalar : TagStructure
    {
        public ScalarFunctionNamedStructDefaultOne Function;
        
        [TagStructure(Size = 0x14)]
        public class ScalarFunctionNamedStructDefaultOne : TagStructure
        {
            public MappingFunctionDefaultOne Function;
            
            [TagStructure(Size = 0x14)]
            public class MappingFunctionDefaultOne : TagStructure
            {
                public byte[] Data;
            }
        }
    }
}
