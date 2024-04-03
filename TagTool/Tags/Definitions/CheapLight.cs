using TagTool.Common;
using System;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "cheap_light", Tag = "gldf", Size = 0x74)]
    public class CheapLight : TagStructure
    {
        public LightColorFunctionStruct Color;
        public LightScalarFunctionStruct Intensity;
        public LightScalarFunctionStruct Radius;
        public float Falloff; // how wide the falloff is relative to the radius ([0-1])
        public float CloseupFadingDistance; // world units

        [TagStructure(Size = 0x24)]
        public class LightColorFunctionStruct : TagStructure
        {
            public StringId InputVariable;
            public StringId RangeVariable;
            public OutputModEnum OutputModifier;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] BVCG;
            public StringId OutputModifierInput;
            public TagFunction Mapping;

            public enum OutputModEnum : short
            {
                None,
                Plus,
                Times
            }
        }

        [TagStructure(Size = 0x24)]
        public class LightScalarFunctionStruct : TagStructure
        {
            public StringId InputVariable;
            public StringId RangeVariable;
            public OutputModEnum OutputModifier;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] BVCG;
            public StringId OutputModifierInput;
            public TagFunction Mapping;

            public enum OutputModEnum : short
            {
                None,
                Plus,
                Times
            }
        }
    }
}
