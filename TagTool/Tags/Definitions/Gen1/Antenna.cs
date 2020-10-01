using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "antenna", Tag = "ant!", Size = 0xD0)]
    public class Antenna : TagStructure
    {
        /// <summary>
        /// the marker name where the antenna should be attached
        /// </summary>
        [TagField(Length = 32)]
        public string AttachmentMarkerName;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Bitmaps;
        [TagField(ValidTags = new [] { "pphy" })]
        public CachedTag Physics;
        [TagField(Length = 0x50)]
        public byte[] Padding;
        /// <summary>
        /// strength of the spring (larger values make the spring stronger)
        /// </summary>
        public float SpringStrengthCoefficient;
        public float FalloffPixels;
        public float CutoffPixels;
        [TagField(Length = 0x28)]
        public byte[] Padding1;
        public List<AntennaVertexBlock> Vertices;
        
        [TagStructure(Size = 0x80)]
        public class AntennaVertexBlock : TagStructure
        {
            /// <summary>
            /// strength of the spring (larger values make the spring stronger)
            /// </summary>
            public float SpringStrengthCoefficient;
            [TagField(Length = 0x18)]
            public byte[] Padding;
            /// <summary>
            /// direction toward next vertex
            /// </summary>
            public RealEulerAngles2d Angles;
            /// <summary>
            /// distance between this vertex and the next
            /// </summary>
            public float Length; // world units
            /// <summary>
            /// bitmap group sequence index for this vertex's texture
            /// </summary>
            public short SequenceIndex;
            [TagField(Length = 0x2)]
            public byte[] Padding1;
            /// <summary>
            /// color at this vertex
            /// </summary>
            public RealArgbColor Color;
            /// <summary>
            /// color at this vertex for the low-LOD line primitives
            /// </summary>
            public RealArgbColor LodColor;
            [TagField(Length = 0x28)]
            public byte[] Padding2;
            [TagField(Length = 0xC)]
            public byte[] Padding3;
        }
    }
}

