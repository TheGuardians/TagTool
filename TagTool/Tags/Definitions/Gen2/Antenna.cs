using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "antenna", Tag = "ant!", Size = 0xB4)]
    public class Antenna : TagStructure
    {
        public StringId AttachmentMarkerName; // the marker name where the antenna should be attached
        public CachedTag Bitmaps;
        public CachedTag Physics;
        [TagField(Flags = Padding, Length = 80)]
        public byte[] Padding1;
        public float SpringStrengthCoefficient; // strength of the spring (larger values make the spring stronger)
        public float FalloffPixels;
        public float CutoffPixels;
        [TagField(Flags = Padding, Length = 40)]
        public byte[] Padding2;
        public List<AntennaVertex> Vertices;
        
        [TagStructure(Size = 0x80)]
        public class AntennaVertex : TagStructure
        {
            public float SpringStrengthCoefficient; // strength of the spring (larger values make the spring stronger)
            [TagField(Flags = Padding, Length = 24)]
            public byte[] Padding1;
            public RealEulerAngles2d Angles; // direction toward next vertex
            public float Length; // world units
            public short SequenceIndex; // bitmap group sequence index for this vertex's texture
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            public RealArgbColor Color; // color at this vertex
            public RealArgbColor LodColor; // color at this vertex for the low-LOD line primitives
            [TagField(Flags = Padding, Length = 40)]
            public byte[] Padding3;
            [TagField(Flags = Padding, Length = 12)]
            public byte[] Padding4;
        }
    }
}

