using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "decorator_set", Tag = "DECR", Size = 0x70)]
    public class DecoratorSet : TagStructure
    {
        public List<DecoratorShaderReferenceBlock> Shaders;
        /// <summary>
        /// 0.0 defaults to 0.4
        /// </summary>
        public float LightingMinScale;
        /// <summary>
        /// 0.0 defaults to 2.0
        /// </summary>
        public float LightingMaxScale;
        public List<DecoratorClassesBlock> Classes;
        public List<DecoratorModelsBlock> Models;
        public List<DecoratorModelVerticesBlock> RawVertices;
        public List<DecoratorModelIndicesBlock> Indices;
        public List<CachedDataBlock> CachedData;
        public GlobalGeometryBlockInfoStructBlock GeometrySectionInfo;
        [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        
        [TagStructure(Size = 0x8)]
        public class DecoratorShaderReferenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "shad" })]
            public CachedTag Shader;
        }
        
        [TagStructure(Size = 0x14)]
        public class DecoratorClassesBlock : TagStructure
        {
            public StringId Name;
            public TypeValue Type;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float Scale;
            public List<DecoratorPermutationsBlock> Permutations;
            
            public enum TypeValue : sbyte
            {
                Model,
                FloatingDecal,
                ProjectedDecal,
                ScreenFacingQuad,
                AxisRotatingQuad,
                CrossQuad
            }
            
            [TagStructure(Size = 0x28)]
            public class DecoratorPermutationsBlock : TagStructure
            {
                public StringId Name;
                public sbyte Shader;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public FlagsValue Flags;
                public FadeDistanceValue FadeDistance;
                public sbyte Index;
                public sbyte DistributionWeight;
                public Bounds<float> Scale;
                public ArgbColor Tint1;
                public ArgbColor Tint2;
                public float BaseMapTintPercentage;
                public float LightmapTintPercentage;
                public float WindScale;
                
                [Flags]
                public enum FlagsValue : byte
                {
                    AlignToNormal = 1 << 0,
                    OnlyOnGround = 1 << 1,
                    Upright = 1 << 2
                }
                
                public enum FadeDistanceValue : sbyte
                {
                    Close,
                    Medium,
                    Far
                }
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class DecoratorModelsBlock : TagStructure
        {
            public StringId ModelName;
            public short IndexStart;
            public short IndexCount;
        }
        
        [TagStructure(Size = 0x38)]
        public class DecoratorModelVerticesBlock : TagStructure
        {
            public RealPoint3d Position;
            public RealVector3d Normal;
            public RealVector3d Tangent;
            public RealVector3d Binormal;
            public RealPoint2d Texcoord;
        }
        
        [TagStructure(Size = 0x2)]
        public class DecoratorModelIndicesBlock : TagStructure
        {
            public short Index;
        }
        
        [TagStructure(Size = 0x20)]
        public class CachedDataBlock : TagStructure
        {
            public VertexBuffer VertexBuffer;
        }
        
        [TagStructure(Size = 0x24)]
        public class GlobalGeometryBlockInfoStructBlock : TagStructure
        {
            public int BlockOffset;
            public int BlockSize;
            public int SectionDataSize;
            public int ResourceDataSize;
            public List<GlobalGeometryBlockResourceBlock> Resources;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short OwnerTagSectionOffset;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            
            [TagStructure(Size = 0x10)]
            public class GlobalGeometryBlockResourceBlock : TagStructure
            {
                public TypeValue Type;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short PrimaryLocator;
                public short SecondaryLocator;
                public int ResourceDataSize;
                public int ResourceDataOffset;
                
                public enum TypeValue : sbyte
                {
                    TagBlock,
                    TagData,
                    VertexBuffer
                }
            }
        }
    }
}

