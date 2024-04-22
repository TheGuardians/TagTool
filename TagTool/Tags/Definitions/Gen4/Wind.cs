using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "wind", Tag = "wind", Size = 0x78)]
    public class Wind : TagStructure
    {
        public WindScalarFunctionStruct Direction;
        public WindScalarFunctionStruct Speed;
        public WindScalarFunctionStruct Bend;
        public WindScalarFunctionStruct Oscillation;
        public WindScalarFunctionStruct Frequency;
        public float GustSize; // world units
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag GustNoiseBitmap;
        
        [TagStructure(Size = 0x14)]
        public class WindScalarFunctionStruct : TagStructure
        {
            public MappingFunction Mapping;
            
            [TagStructure(Size = 0x14)]
            public class MappingFunction : TagStructure
            {
                public byte[] Data;
            }
        }
    }
}
