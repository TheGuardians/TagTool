using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "camo", Tag = "cmoe", Size = 0x3C)]
    public class Camo : TagStructure
    {
        public CamoFlags Flags;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public CamoScalarFunctionStruct ActiveCamoAmount;
        public CamoScalarFunctionStruct ShadowAmount;
        
        [Flags]
        public enum CamoFlags : ushort
        {
            AlsoApplyToObjectChildren = 1 << 0
        }
        
        [TagStructure(Size = 0x1C)]
        public class CamoScalarFunctionStruct : TagStructure
        {
            public StringId InputVariable;
            public StringId RangeVariable;
            public MappingFunction Mapping;
            
            [TagStructure(Size = 0x14)]
            public class MappingFunction : TagStructure
            {
                public byte[] Data;
            }
        }
    }
}
