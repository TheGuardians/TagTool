using System.Collections.Generic;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache.Resources
{
    [TagStructure(Size = 0xC)]
    public class ResourceProperty : TagStructure
    {
        public List<ResourceNamedValue> NamedValues;

        [TagStructure(Size = 0x14)]
        public class ResourceNamedValue : TagStructure
        {
            public StringId Name;
            public NamedValueType Type;
            public StringId StringValue;
            public float RealValue;
            public int IntValue;

            public enum NamedValueType : int
            {
                Unknown,
                String,
                Real,
                Int,
            }
        }
    }
}
