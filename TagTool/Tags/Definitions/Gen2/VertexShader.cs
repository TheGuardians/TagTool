using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "vertex_shader", Tag = "vrtx", Size = 0x14)]
    public class VertexShader : TagStructure
    {
        public PlatformValue Platform;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        public List<VertexShaderClassification> GeometryClassifications;
        public int OutputSwizzles;
        
        public enum PlatformValue : short
        {
            Pc,
            Xbox
        }
        
        [TagStructure(Size = 0x2C)]
        public class VertexShaderClassification : TagStructure
        {
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public byte[] CompiledShader;
            public byte[] Code;
        }
    }
}

