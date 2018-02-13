using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sandbox_text_value_pair_definition", Tag = "jmrq", Size = 0xC)]
    public class SandboxTextValuePairDefinition
    {
        public List<SandboxTextValuePair> SandboxTextValuePairs;

        [TagStructure(Size = 0x10)]
        public class SandboxTextValuePair
        {
            public StringId ParameterName;
            public List<TextValuePair> TextValuePairs;

            [TagStructure(Size = 0x14)]
            public class TextValuePair
            {
                public byte Flags;
                public ExpectedValueTypeValue ExpectedValueType;
                public short Unknown;
                public int IntValue;
                public StringId RefName;
                public StringId Name;
                public StringId Description;

                public enum ExpectedValueTypeValue : sbyte
                {
                    IntegerIndex,
                    StringidReference,
                    Incremental
                }
            }
        }
    }
}