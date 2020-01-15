using TagTool.Tags;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Havok
{
    [TagStructure(Size = 0x10, Align = 0x10)]
    public class CollisionMoppCode : MoppCode
    {
        public List<byte> Data;
        public sbyte MoppBuildType;

        [TagField(Flags = Padding, Length = 3)]
        public byte[] Unused = new byte[3];
    }
}