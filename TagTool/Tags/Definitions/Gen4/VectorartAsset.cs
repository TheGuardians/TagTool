using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "vectorart_asset", Tag = "vaas", Size = 0x60)]
    public class VectorartAsset : TagStructure
    {
        public GPolyartassetStateFlags RuntimeFlags;
        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public float AntialiasingExtentSize;
        public float ImportFudgeFactor;
        public RealPoint2d Bounds;
        public float CurveSmoothness;
        public List<PolyartvertexBlock> Vertices;
        public List<PolyartindexBlock> Indices;
        public List<VertexbuffersBlockStruct> PcVertexBuffers;
        public List<IndexbuffersBlockStruct> PcIndexBuffers;
        public TagInterop VertexBufferInterop;
        public TagInterop IndexBufferInterop;
        
        public enum GPolyartassetStateFlags : sbyte
        {
            Processed,
            Available
        }
        
        [TagStructure(Size = 0xC)]
        public class PolyartvertexBlock : TagStructure
        {
            public short HalfX;
            public short HalfY;
            public short HalfZ;
            public short HalfAlpha;
            public short HalfU;
            public short HalfV;
        }
        
        [TagStructure(Size = 0x2)]
        public class PolyartindexBlock : TagStructure
        {
            public short Index;
        }
        
        [TagStructure(Size = 0xC)]
        public class VertexbuffersBlockStruct : TagStructure
        {
            public byte DeclarationType;
            public byte Stride;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public uint Count;
            public int D3dHardwareFormat;
        }
        
        [TagStructure(Size = 0xC)]
        public class IndexbuffersBlockStruct : TagStructure
        {
            public byte DeclarationType;
            public byte Stride;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public uint Count;
            public int D3dHardwareFormat;
        }
    }
}
