using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "cloth", Tag = "clwd", Size = 0x94)]
    public class Cloth : TagStructure
    {
        public ClothFlags Flags;
        public StringId MarkerAttachmentName;
        public StringId SecondaryAxisAttachmentName;
        [TagField(ValidTags = new [] { "mat " })]
        public CachedTag Shader;
        public short GridXDimension;
        public short GridYDimension;
        public float GridSpacingX;
        public float GridSpacingY;
        public List<CollisionSphereBlock> CollisionSpheres;
        public ClothProperties Properties;
        public List<ClothVerticesBlock> Vertices;
        public List<ClothIndicesBlock> Indices;
        public List<ClothIndicesBlock> StripIndices;
        public List<ClothLinksBlock> Links;
        
        [Flags]
        public enum ClothFlags : uint
        {
            DoesnTUseWind = 1 << 0,
            UsesGridAttachTop = 1 << 1
        }
        
        [TagStructure(Size = 0x8)]
        public class CollisionSphereBlock : TagStructure
        {
            public StringId ObjectMarkerName;
            public float Radius;
        }
        
        [TagStructure(Size = 0x30)]
        public class ClothProperties : TagStructure
        {
            public ClothIntegrationEnum IntegrationType;
            // [1-8] sug 1
            public short NumberIterations;
            // [-10.0 - 10.0] sug 1.0
            public float Weight;
            // [0.0 - 0.5] sug 0.07
            public float Drag;
            // [0.0 - 3.0] sug 1.0
            public float WindScale;
            // [0.0 - 1.0] sug 0.75
            public float WindFlappinessScale;
            // [1.0 - 10.0] sug 3.5
            public float LongestRod;
            [TagField(Length = 0x18, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum ClothIntegrationEnum : short
            {
                Verlet
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class ClothVerticesBlock : TagStructure
        {
            public RealPoint3d InitialPosition;
            public RealVector2d Uv;
        }
        
        [TagStructure(Size = 0x2)]
        public class ClothIndicesBlock : TagStructure
        {
            public short Index;
        }
        
        [TagStructure(Size = 0x10)]
        public class ClothLinksBlock : TagStructure
        {
            public float DefaultDistance;
            public int Index1;
            public int Index2;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
    }
}
