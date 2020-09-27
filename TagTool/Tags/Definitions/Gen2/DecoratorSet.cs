using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "decorator_set", Tag = "DECR", Size = 0x8C)]
    public class DecoratorSet : TagStructure
    {
        public List<DecoratorShaderReference> Shaders;
        public float LightingMinScale; // 0.0 defaults to 0.4
        public float LightingMaxScale; // 0.0 defaults to 2.0
        public List<DecoratorClassDefinition> Classes;
        public List<DecoratorModelDefinition> Models;
        public List<DecoratorModelVertex> RawVertices;
        public List<Word> Indices;
        public List<CachedDataBlock> CachedData;
        public GeometryBlockInfo GeometrySectionInfo;
        [TagField(Flags = Padding, Length = 16)]
        public byte[] Padding1;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding2;
        
        [TagStructure(Size = 0x10)]
        public class DecoratorShaderReference : TagStructure
        {
            public CachedTag Shader;
        }
        
        [TagStructure(Size = 0x18)]
        public class DecoratorClassDefinition : TagStructure
        {
            public StringId Name;
            public TypeValue Type;
            [TagField(Flags = Padding, Length = 3)]
            public byte[] Padding1;
            public float Scale;
            public List<DecoratorPermutationDefinition> Permutations;
            
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
            public class DecoratorPermutationDefinition : TagStructure
            {
                public StringId Name;
                public sbyte Shader;
                [TagField(Flags = Padding, Length = 3)]
                public byte[] Padding1;
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
        public class DecoratorModelDefinition : TagStructure
        {
            public StringId ModelName;
            public short IndexStart;
            public short IndexCount;
        }
        
        [TagStructure(Size = 0x38)]
        public class DecoratorModelVertex : TagStructure
        {
            public RealPoint3d Position;
            public RealVector3d Normal;
            public RealVector3d Tangent;
            public RealVector3d Binormal;
            public RealPoint2d Texcoord;
        }
        
        [TagStructure(Size = 0x2)]
        public class Word : TagStructure
        {
            public short Index;
        }
        
        [TagStructure(Size = 0x20)]
        public class CachedDataBlock : TagStructure
        {
            public VertexBuffer VertexBuffer;
        }
        
        [TagStructure(Size = 0x28)]
        public class GeometryBlockInfo : TagStructure
        {
            /// <summary>
            /// BLOCK INFO
            /// </summary>
            public int BlockOffset;
            public int BlockSize;
            public int SectionDataSize;
            public int ResourceDataSize;
            public List<GeometryBlockResource> Resources;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public short OwnerTagSectionOffset;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding3;
            
            [TagStructure(Size = 0x10)]
            public class GeometryBlockResource : TagStructure
            {
                public TypeValue Type;
                [TagField(Flags = Padding, Length = 3)]
                public byte[] Padding1;
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

