using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "vertex_shader", Tag = "vrtx", Size = 0x10)]
    public class VertexShader : TagStructure
    {
        public PlatformValue Platform;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public List<VertexShaderClassificationBlock> GeometryClassifications;
        public int OutputSwizzles;
        
        public enum PlatformValue : short
        {
            Pc,
            Xbox
        }
        
        [TagStructure(Size = 0x14)]
        public class VertexShaderClassificationBlock : TagStructure
        {
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public byte[] CompiledShader;
            public byte[] Code;
        }
    }
}

