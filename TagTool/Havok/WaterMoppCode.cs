using TagTool.Cache;
using TagTool.Tags;
using System.Collections.Generic;

namespace TagTool.Havok
{
    [TagStructure(Size = 0x10, Align = 0x10)]
    public class WaterMoppCode : MoppCode
    {
        public List<Datum> Data;

        public sbyte MoppBuildType;

        [TagField(Flags = TagFieldFlags.Padding, Length = 3)]
        public byte[] Unused = new byte[3];

        [TagStructure(Size = 0x1)]
		public class Datum : TagStructure
		{
            public byte Value;
        }
    }
}