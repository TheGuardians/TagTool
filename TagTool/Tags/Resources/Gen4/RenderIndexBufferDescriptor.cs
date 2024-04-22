using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions.Gen4;

namespace TagTool.Tags.Resources.Gen4
{
    [TagStructure(Size = 0x1C)]
    public class RenderIndexBufferDescriptor : TagStructure
    {
        public int PrimitiveType;
        public sbyte IsIndex32;
        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public TagData IndexData;
    }
}
