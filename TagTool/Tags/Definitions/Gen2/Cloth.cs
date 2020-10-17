using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "cloth", Tag = "clwd", Size = 0x6C)]
    public class Cloth : TagStructure
    {
        public FlagsValue Flags;
        public StringId MarkerAttachmentName;
        [TagField(ValidTags = new [] { "shad" })]
        public CachedTag Shader;
        /// <summary>
        /// if not importing from a render model, type a grid size
        /// </summary>
        public short GridXDimension;
        public short GridYDimension;
        public float GridSpacingX;
        public float GridSpacingY;
        public ClothPropertiesBlock Properties;
        public List<ClothVerticesBlock> Vertices;
        public List<ClothIndicesBlock> Indices;
        public List<ClothIndicesBlock1> StripIndices;
        public List<ClothLinksBlock> Links;
        
        [Flags]
        public enum FlagsValue : uint
        {
            DoesnTUseWind = 1 << 0,
            UsesGridAttachTop = 1 << 1
        }
        
        [TagStructure(Size = 0x30)]
        public class ClothPropertiesBlock : TagStructure
        {
            public IntegrationTypeValue IntegrationType;
            /// <summary>
            /// [1-8] sug 1
            /// </summary>
            public short NumberIterations;
            /// <summary>
            /// [-10.0 - 10.0] sug 1.0
            /// </summary>
            public float Weight;
            /// <summary>
            /// [0.0 - 0.5] sug 0.07
            /// </summary>
            public float Drag;
            /// <summary>
            /// [0.0 - 3.0] sug 1.0
            /// </summary>
            public float WindScale;
            /// <summary>
            /// [0.0 - 1.0] sug 0.75
            /// </summary>
            public float WindFlappinessScale;
            /// <summary>
            /// [1.0 - 10.0] sug 3.5
            /// </summary>
            public float LongestRod;
            [TagField(Length = 0x18, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum IntegrationTypeValue : short
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
        
        [TagStructure(Size = 0x2)]
        public class ClothIndicesBlock1 : TagStructure
        {
            public short Index;
        }
        
        [TagStructure(Size = 0x10)]
        public class ClothLinksBlock : TagStructure
        {
            public int AttachmentBits;
            public short Index1;
            public short Index2;
            public float DefaultDistance;
            public float DampingMultiplier;
        }
    }
}

