using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "antenna", Tag = "ant!", Size = 0x4C)]
    public class Antenna : TagStructure
	{
        public StringId AttachmentMarkerName; // the marker name where the antenna should be attached
        public CachedTag Bitmaps;
        public CachedTag Physics;
        public float SpringStrengthCoefficient; // strength of the spring (larger values make the spring stronger)
        public float FalloffPixels;
        public float CutoffPixels;
        public float PointOfBend; // [0,1]
        public float StartingBend; // [0,1]
        public float EndingBend; // [0,1]
        public float RuntimeTotalLength;
        public List<AntennaVertex> Vertices;

        [TagStructure(Size = 0x40)]
        public class AntennaVertex : TagStructure
		{
            public RealEulerAngles2d Angles; // direction toward next vertex!
            public float Length; // distance between this vertex and the next (in world units)
            public short SequenceIndex; // bitmap group sequence index for this vertex's texture

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused;

            public RealArgbColor Color; // color at this vertex
            public RealArgbColor LodColor; // color at this vertex for the low-LOD line primitives!
            public float HermiteT;
            public RealVector3d VectorToNext;
        }
    }
}