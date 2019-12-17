using TagTool.Common;
using TagTool.Cache;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_datasource_definition", Tag = "dsrc", Size = 0x1C)]
    public class GuiDatasourceDefinition : TagStructure
	{
        public StringId Name;
        public uint Unknown;
        public uint Unknown2;
        public uint Unknown3;
        public List<Datum> Data;

        [TagStructure(Size = 0x28)]
        public class Datum : TagStructure
		{
            public List<IntegerValue> IntegerValues;
            public List<StringValue> StringValues;
            public List<StringidValue> StringidValues;
            public StringId Unknown;

            [TagStructure(Size = 0x8)]
            public class IntegerValue : TagStructure
			{
                public StringId DataType;
                public int Value;
            }

            [TagStructure(Size = 0x24)]
            public class StringValue : TagStructure
			{
                public StringId DataType;

                [TagField(Length = 32)]
                public string Value;
            }

            [TagStructure(Size = 0x8)]
            public class StringidValue : TagStructure
			{
                public StringId DataType;
                public StringId Value;
            }
        }
    }
}
