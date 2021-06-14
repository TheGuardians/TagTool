using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions.Gen4;

namespace TagTool.Tags.Resources.Gen4
{
    [TagStructure(Size = 0x14)]
    public class VectorartvertexBufferDescriptor : TagStructure
    {
        public byte[] Vertices;
    }
}
