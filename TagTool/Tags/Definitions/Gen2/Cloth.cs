using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "cloth", Tag = "clwd", Size = 0x84)]
    public class Cloth : TagStructure
    {
        public FlagsValue Flags;
        public StringId MarkerAttachmentName;
        public CachedTag Shader;
        /// <summary>
        /// Default cloth vertices
        /// </summary>
        /// <remarks>
        /// if not importing from a render model, type a grid size
        /// </remarks>
        public short GridXDimension;
        public short GridYDimension;
        public float GridSpacingX;
        public float GridSpacingY;
        /// <summary>
        /// Cloth Properties
        /// </summary>
        public ClothProperties Properties;
        /// <summary>
        /// Import or grid data
        /// </summary>
        public List<ClothVertexDefinition> Vertices;
        public List<ClothIndexDefinition> Indices;
        public List<ClothIndexDefinition> StripIndices;
        public List<ClothLinkDefinition> Links;
        
        [Flags]
        public enum FlagsValue : uint
        {
            DoesnTUseWind = 1 << 0,
            UsesGridAttachTop = 1 << 1
        }
        
        [TagStructure(Size = 0x30)]
        public class ClothProperties : TagStructure
        {
            public IntegrationTypeValue IntegrationType;
            public short NumberIterations; // [1-8] sug 1
            public float Weight; // [-10.0 - 10.0] sug 1.0
            public float Drag; // [0.0 - 0.5] sug 0.07
            public float WindScale; // [0.0 - 3.0] sug 1.0
            public float WindFlappinessScale; // [0.0 - 1.0] sug 0.75
            public float LongestRod; // [1.0 - 10.0] sug 3.5
            [TagField(Flags = Padding, Length = 24)]
            public byte[] Padding1;
            
            public enum IntegrationTypeValue : short
            {
                Verlet
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class ClothVertexDefinition : TagStructure
        {
            public RealPoint3d InitialPosition;
            public RealVector2d Uv;
        }
        
        [TagStructure(Size = 0x2)]
        public class ClothIndexDefinition : TagStructure
        {
            public short Index;
        }
        
        [TagStructure(Size = 0x10)]
        public class ClothLinkDefinition : TagStructure
        {
            public int AttachmentBits;
            public short Index1;
            public short Index2;
            public float DefaultDistance;
            public float DampingMultiplier;
        }
    }
}

