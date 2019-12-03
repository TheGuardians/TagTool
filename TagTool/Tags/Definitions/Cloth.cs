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
        public StringId SecondMarkerAttachmentName;
        public CachedTagInstance Shader;
        public short GridXDimension;
        public short GridYDimension;
        public float GridSpacingX;
        public float GridSpacingY;
        public List<CollisionSphere> CollisionSpheres;
        public ClothProperties Properties;
        public List<Vertex> Vertices;
        public List<short> Indices;
        public List<short> StripIndices;
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
            public RealPoint3d Position;
            public RealVector2d Uv;
        }
        
        [TagStructure(Size = 0x8)]
        public class Link : TagStructure
		{
            public short Index1;
            public short Index2;
            public float DefaultDistance;
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
        public short NumberIterations;
        public float Weight;
        public float Drag;
        public float WindScale;
        public float WindFlappinessScale;
        public float LongestRod;

        [TagField(Flags = Padding, Length = 24)]
        public byte[] Unused;
    }
}