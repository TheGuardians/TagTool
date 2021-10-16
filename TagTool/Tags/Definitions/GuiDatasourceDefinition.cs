using TagTool.Common;
using TagTool.Cache;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "gui_datasource_definition", Tag = "dsrc", Size = 0x1C)]
    public class GuiDatasourceDefinition : TagStructure
	{
        public StringId Name;
        [TagField(Length = 12, Flags = TagFieldFlags.Padding)]
        public byte[] DatasourcePadding;
        public List<DatasourceElementBlock> Elements;

        [TagStructure(Size = 0x28)]
        public class DatasourceElementBlock : TagStructure
		{
            public List<IntegerValue> IntegerValues;
            public List<StringValue> StringValues;
            public List<StringidValue> StringidValues;
            public StringId SubmenuControlName;

            [TagStructure(Size = 0x8)]
            public class IntegerValue : TagStructure
			{
                public StringId Name;
                public int Value;
            }

            [TagStructure(Size = 0x24)]
            public class StringValue : TagStructure
			{
                public StringId Name;
                [TagField(Length = 32)]
                public string Value;
            }

            [TagStructure(Size = 0x8)]
            public class StringidValue : TagStructure
			{
                public StringId Name;
                public StringId Value;
            }
        }
    }
}
