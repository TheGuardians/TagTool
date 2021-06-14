using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions.Gen4;

namespace TagTool.Tags.Resources.Gen4
{
    [TagStructure(Size = 0x1C)]
    public class RenderVertexBufferDescriptor : TagStructure
    {
        public int VertexCount;
        public short Declaration;
        public short Stride;
        public byte[] Vertices;
    }
}
