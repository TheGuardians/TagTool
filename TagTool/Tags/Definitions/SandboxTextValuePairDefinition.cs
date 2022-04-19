using TagTool.Common;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sandbox_text_value_pair_definition", Tag = "jmrq", Size = 0xC)]
    public class SandboxTextValuePairDefinition : TagStructure
	{
        public List<SandboxTextValuePair> SandboxTextValuePairs;

        [TagStructure(Size = 0x10)]
        public class SandboxTextValuePair : TagStructure
		{
            public StringId ParameterName;
            public List<TextValuePair> TextValuePairs;

            [TagStructure(Size = 0x14)]
            public class TextValuePair : TagStructure
			{
                public byte Flags;
                public ExpectedValueTypeValue ExpectedValueType;

                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Pad;

                public int EnumeratedValue;
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