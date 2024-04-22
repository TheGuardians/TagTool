using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "cloth", Tag = "clwd", Size = 0x94)]
    public class Cloth : TagStructure
	{
        public ClothFlags Flags;
        public StringId MarkerAttachmentName;
        public StringId SecondaryAxisAttachmentName;

        [TagField(ValidTags = new[] { "rmsh" })]
        public CachedTag Shader;

        public short GridXDimension;
        public short GridYDimension;
        public float GridSpacingX;
        public float GridSpacingY;
        public List<CollisionSphere> CollisionSpheres;
        public ClothProperties Properties;
        public List<Vertex> Vertices;
        public List<ClothIndex> Indices;
        public List<ClothIndex> StripIndices;
        public List<Link> Links;

        [TagStructure(Size = 0x8)]
        public class CollisionSphere : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId ObjectMarkerName;
            public float Radius;
        }

        [TagStructure(Size = 0x14)]
        public class Vertex : TagStructure
		{
            public RealPoint3d InitialPosition;
            public RealVector2d Uv;
        }
        
        [TagStructure(Size = 0x8)]
        public class Link : TagStructure
		{
            public short Index1;
            public short Index2;
            public float DefaultDistance;
        }

        [TagStructure(Size = 0x2)]
        public class ClothIndex : TagStructure
        {
            public short Index;
        }
    }

    [Flags]
    public enum ClothFlags : int
    {
        None = 0,
        DoesNotUseWind = 1 << 0,
        UsesGridAttachTop = 1 << 1
    }

    public enum ClothIntegrationType : short
    {
        Verlet
    }

    [TagStructure(Size = 0x30)]
    public class ClothProperties : TagStructure
	{
        public ClothIntegrationType IntegrationType;
        public short NumberIterations; // [1-8] sug 1
        public float Weight; // [-10.0 - 10.0] sug 1.0
        public float Drag; // [0.0 - 0.5] sug 0.07
        public float WindScale; // [0.0 - 3.0] sug 1.0
        public float WindFlappinessScale; // [0.0 - 1.0] sug 0.75
        public float LongestRod; // [1.0 - 10.0] sug 3.5

        [TagField(Flags = TagFieldFlags.Padding, Length = 24)]
        public byte[] Padding;
    }
}