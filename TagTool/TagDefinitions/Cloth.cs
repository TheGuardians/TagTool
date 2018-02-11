using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "cloth", Tag = "clwd", Size = 0x94)]
    public class Cloth
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
        public class CollisionSphere
        {
            [TagField(Label = true)]
            public StringId ObjectMarkerName;
            public float Radius;
        }

        [TagStructure(Size = 0x14)]
        public class Vertex
        {
            public RealPoint3d Position;
            public RealVector2d Uv;
        }
        
        [TagStructure(Size = 0x8)]
        public class Link
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
    public class ClothProperties
    {
        public ClothIntegrationType IntegrationType;
        public short NumberIterations;
        public float Weight;
        public float Drag;
        public float WindScale;
        public float WindFlappinessScale;
        public float LongestRod;

        [TagField(Padding = true, Length = 24)]
        public byte[] Unused;
    }
}