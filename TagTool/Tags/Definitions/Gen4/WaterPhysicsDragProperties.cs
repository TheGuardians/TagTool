using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "water_physics_drag_properties", Tag = "wpdp", Size = 0x38)]
    public class WaterPhysicsDragProperties : TagStructure
    {
        public PhysicsForceFunctionStruct Pressure;
        public PhysicsForceFunctionStruct Suction;
        public float LinearDamping;
        public float AngularDamping;
        
        [TagStructure(Size = 0x18)]
        public class PhysicsForceFunctionStruct : TagStructure
        {
            public MappingFunction VelocityToPressure;
            public float MaxVelocity; // wu/s
            
            [TagStructure(Size = 0x14)]
            public class MappingFunction : TagStructure
            {
                public byte[] Data;
            }
        }
    }
}
