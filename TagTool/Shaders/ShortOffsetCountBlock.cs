using System;
using TagTool.Tags;

namespace TagTool.Shaders
{
    [TagStructure(Size = 0x2)]
    public class ShortOffsetCountBlock : TagStructure
	{
        public byte Offset;
        public byte Count;
    }
}