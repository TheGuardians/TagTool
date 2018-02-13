using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_datasource_definition", Tag = "dsrc", Size = 0x20)]
    public class GuiDatasourceDefinition
    {
        public StringId Name;
        public uint Unknown;
        public uint Unknown2;
        public uint Unknown3;
        public List<Datum> Data;
        public uint Unknown4;

        [TagStructure(Size = 0x28)]
        public class Datum
        {
            public List<IntegerValue> IntegerValues;
            public List<StringValue> StringValues;
            public List<StringidValue> StringidValues;
            public StringId Unknown;

            [TagStructure(Size = 0x8)]
            public class IntegerValue
            {
                public StringId DataType;
                public int Value;
            }

            [TagStructure(Size = 0x24)]
            public class StringValue
            {
                public StringId DataType;
                [TagField(Length = 20)] public string Value;
            }

            [TagStructure(Size = 0x8)]
            public class StringidValue
            {
                public StringId DataType;
                public StringId Value;
            }
        }
    }
}
