using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions.Gen4;

namespace TagTool.Tags.Resources.Gen4
{
    [TagStructure(Size = 0x30)]
    public class RenderGeometryApiResourceDefinition : TagStructure
    {
        public List<VertexBuffersBlock> PcVertexBuffers;
        public List<IndexBuffersBlock> PcIndexBuffers;
        public List<RenderVertexBufferBlock> XenonVertexBuffers;
        public List<RenderIndexBufferBlock> XenonIndexBuffers;
        
        [TagStructure(Size = 0xC)]
        public class VertexBuffersBlock : TagStructure
        {
            public byte DeclarationType;
            public byte Stride;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public uint Count;
            public int D3dHardwareFormat;
        }
        
        [TagStructure(Size = 0xC)]
        public class IndexBuffersBlock : TagStructure
        {
            public byte DeclarationType;
            public byte Stride;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public uint Count;
            public int D3dHardwareFormat;
        }
        
        [TagStructure(Size = 0xC)]
        public class RenderVertexBufferBlock : TagStructure
        {
            public TagInterop VertexBufferInterop;
        }
        
        [TagStructure(Size = 0xC)]
        public class RenderIndexBufferBlock : TagStructure
        {
            public TagInterop IndexBufferInterop;
        }
    }
}
