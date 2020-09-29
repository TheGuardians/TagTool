using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "decorators", Tag = "DECP", Size = 0x40)]
    public class Decorators : TagStructure
    {
        public RealPoint3d GridOrigin;
        public int CellCountPerDimension;
        public List<DecoratorCacheBlock> CacheBlocks;
        public List<DecoratorGroup> Groups;
        public List<DecoratorCellCollection> Cells;
        public List<DecoratorProjectedDecal> Decals;
        
        [TagStructure(Size = 0x3C)]
        public class DecoratorCacheBlock : TagStructure
        {
            public GeometryBlockInfoStruct GeometryBlockInfo;
            public List<DecoratorCacheBlockData> CacheBlockData;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding2;
            
            [TagStructure(Size = 0x28)]
            public class GeometryBlockInfoStruct : TagStructure
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
            
            [TagStructure(Size = 0x9C)]
            public class DecoratorCacheBlockData : TagStructure
            {
                public List<DecoratorPlacement> Placements;
                public List<RasterizerVertexDecoratorDecal> DecalVertices;
                public List<Word> DecalIndices;
                public VertexBuffer DecalVertexBuffer;
                [TagField(Flags = Padding, Length = 16)]
                public byte[] Padding1;
                public List<RasterizerVertexDecoratorSprite> SpriteVertices;
                public List<Word> SpriteIndices;
                public VertexBuffer SpriteVertexBuffer;
                [TagField(Flags = Padding, Length = 16)]
                public byte[] Padding2;
                
                [TagStructure(Size = 0x18)]
                public class DecoratorPlacement : TagStructure
                {
                    public int InternalData1;
                    public int CompressedPosition;
                    public ArgbColor TintColor;
                    public ArgbColor LightmapColor;
                    public int CompressedLightDirection;
                    public int CompressedLight2Direction;
                }
                
                [TagStructure(Size = 0x20)]
                public class RasterizerVertexDecoratorDecal : TagStructure
                {
                    public RealPoint3d Position;
                    public RealPoint2d Texcoord0;
                    public RealPoint2d Texcoord1;
                    public ArgbColor Color;
                }
                
                [TagStructure(Size = 0x2)]
                public class Word : TagStructure
                {
                    public short Index;
                }
                
                [TagStructure(Size = 0x30)]
                public class RasterizerVertexDecoratorSprite : TagStructure
                {
                    public RealPoint3d Position;
                    public RealVector3d Offset;
                    public RealVector3d Axis;
                    public RealPoint2d Texcoord;
                    public ArgbColor Color;
                }
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class DecoratorGroup : TagStructure
        {
            public sbyte DecoratorSet;
            public DecoratorTypeValue DecoratorType;
            public sbyte ShaderIndex;
            public sbyte CompressedRadius;
            public short Cluster;
            public short CacheBlock;
            public short DecoratorStartIndex;
            public short DecoratorCount;
            public short VertexStartOffset;
            public short VertexCount;
            public short IndexStartOffset;
            public short IndexCount;
            public int CompressedBoundingCenter;
            
            public enum DecoratorTypeValue : sbyte
            {
                Model,
                FloatingDecal,
                ProjectedDecal,
                ScreenFacingQuad,
                AxisRotatingQuad,
                CrossQuad
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class DecoratorCellCollection : TagStructure
        {
            public short ChildIndex;
            [TagField(Length = 8)]
            public short ChildIndices;
            public short CacheBlockIndex;
            public short GroupCount;
            public int GroupStartIndex;
        }
        
        [TagStructure(Size = 0x40)]
        public class DecoratorProjectedDecal : TagStructure
        {
            public sbyte DecoratorSet;
            public sbyte DecoratorClass;
            public sbyte DecoratorPermutation;
            public sbyte SpriteIndex;
            public RealPoint3d Position;
            public RealVector3d Left;
            public RealVector3d Up;
            public RealVector3d Extents;
            public RealPoint3d PreviousPosition;
        }
    }
}

