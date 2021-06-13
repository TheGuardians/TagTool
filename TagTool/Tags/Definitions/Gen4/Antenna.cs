using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "antenna", Tag = "ant!", Size = 0x50)]
    public class Antenna : TagStructure
    {
        // the marker name where the antenna should be attached
        public StringId AttachmentMarkerName;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Bitmaps;
        [TagField(ValidTags = new [] { "pphy" })]
        public CachedTag Physics;
        // strength of the spring (larger values make the spring stronger)
        public float SpringStrengthCoefficient;
        public float TexelToWorldWidthScale;
        public float FalloffPixels;
        public float CutoffPixels;
        // [0,1]
        public float PointOfBend;
        // [0,1]
        public float StartingBend;
        // [0,1]
        public float EndingBend;
        public float RuntimeTotalLength;
        public List<AntennaVertexBlock> Vertices;
        
        [TagStructure(Size = 0x40)]
        public class AntennaVertexBlock : TagStructure
        {
            // direction toward next vertex!
            public RealEulerAngles2d Angles;
            // distance between this vertex and the next
            public float Length; // world units
            // bitmap group sequence index for this vertex's texture
            public short SequenceIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // color at this vertex
            public RealArgbColor Color;
            // color at this vertex for the low-LOD line primitives!
            public RealArgbColor LodColor;
            public float HermiteT;
            public RealVector3d VectorToNext;
        }
    }
}
