using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "cheap_light", Tag = "gldf", Size = 0x80)]
    public class CheapLight : TagStructure
    {
        public CheaplightFunctionInputEnum FunctionInput;
        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public LightColorFunctionStruct Color;
        public LightScalarFunctionStruct Intensity;
        public LightScalarFunctionStruct FalloffEnd;
        // Ratio of falloff start to falloff end
        public float FalloffBeginRatio; // [0-1]
        public float NearFadeDistance; // world units
        public float FarFadeBegin; // world units
        public float FarFadeCutoff; // world units
        
        public enum CheaplightFunctionInputEnum : sbyte
        {
            TimeAge,
            ScaleA,
            ScaleB
        }
        
        [TagStructure(Size = 0x24)]
        public class LightColorFunctionStruct : TagStructure
        {
            public StringId InputVariable;
            public StringId RangeVariable;
            public OutputModEnum OutputModifier;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public StringId OutputModifierInput;
            public MappingFunction Mapping;
            
            public enum OutputModEnum : short
            {
                Unknown,
                Plus,
                Times
            }
            
            [TagStructure(Size = 0x14)]
            public class MappingFunction : TagStructure
            {
                public byte[] Data;
            }
        }
        
        [TagStructure(Size = 0x24)]
        public class LightScalarFunctionStruct : TagStructure
        {
            public StringId InputVariable;
            public StringId RangeVariable;
            public OutputModEnum OutputModifier;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public StringId OutputModifierInput;
            public MappingFunction Mapping;
            
            public enum OutputModEnum : short
            {
                Unknown,
                Plus,
                Times
            }
            
            [TagStructure(Size = 0x14)]
            public class MappingFunction : TagStructure
            {
                public byte[] Data;
            }
        }
    }
}
