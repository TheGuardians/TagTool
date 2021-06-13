using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "spring_acceleration", Tag = "sadt", Size = 0x14)]
    public class SpringAcceleration : TagStructure
    {
        public List<SpringLinearAccelerationBlock> LinearAcceleartions;
        public short XAxis;
        public short YAxis;
        public short ZAxis;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        
        [TagStructure(Size = 0x38)]
        public class SpringLinearAccelerationBlock : TagStructure
        {
            public float AccelerationRange; // world units
            public float InverseAccelerationRange; // world units*!
            // 0 defaults to 1, scale the acceleration the object itself applies on this system.
            public float WorldAccelerationScale;
            public ScalarFunctionNamedStruct SpringDamping;
            public float VelocityDomain;
            public ScalarFunctionNamedStruct SpringAcceleration;
            
            [TagStructure(Size = 0x14)]
            public class ScalarFunctionNamedStruct : TagStructure
            {
                public MappingFunction Function;
                
                [TagStructure(Size = 0x14)]
                public class MappingFunction : TagStructure
                {
                    public byte[] Data;
                }
            }
        }
    }
}
