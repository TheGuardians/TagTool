using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "sandbox_text_value_pair_definition", Tag = "jmrq", Size = 0xC)]
    public class SandboxTextValuePairDefinition : TagStructure
    {
        public List<SandboxPropertyAllowedValuesReferenceBlock> PropertyValues;
        
        [TagStructure(Size = 0x14)]
        public class SandboxPropertyAllowedValuesReferenceBlock : TagStructure
        {
            public StringId PropertyName;
            public TextValuePairParameterType ParameterType;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<TextValuePairReferenceBlock> AllowedValues;
            
            public enum TextValuePairParameterType : sbyte
            {
                Integer,
                StringId,
                RealRange,
                Real
            }
            
            [TagStructure(Size = 0x18)]
            public class TextValuePairReferenceBlock : TagStructure
            {
                public TextValuePairFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public int EnumeratedValue;
                public float RealValue;
                public StringId StringIdValue;
                public StringId LabelStringId;
                public StringId DescriptionStringId;
                
                [Flags]
                public enum TextValuePairFlags : byte
                {
                    DefaultSetting = 1 << 0,
                    RealValueUnchanged = 1 << 1
                }
            }
        }
    }
}
