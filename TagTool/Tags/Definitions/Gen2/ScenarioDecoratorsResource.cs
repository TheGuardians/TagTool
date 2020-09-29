using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenario_decorators_resource", Tag = "dc*s", Size = 0x10)]
    public class ScenarioDecoratorsResource : TagStructure
    {
        public List<DecoratorPlacementDefinitionBlock> Decorator;
        public List<ScenarioDecoratorSetPaletteEntryBlock> DecoratorPalette;
        
        [TagStructure(Size = 0x30)]
        public class DecoratorPlacementDefinitionBlock : TagStructure
        {
            public RealPoint3d GridOrigin;
            public int CellCountPerDimension;
            public List<DecoratorCacheBlockBlock> CacheBlocks;
            public List<DecoratorGroupBlock> Groups;
            public List<DecoratorCellCollectionBlock> Cells;
            public List<DecoratorProjectedDecalBlock> Decals;
            
            [TagStructure(Size = 0x34)]
            public class DecoratorCacheBlockBlock : TagStructure
            {
                public GlobalGeometryBlockInfoStructBlock GeometryBlockInfo;
                public List<DecoratorCacheBlockDataBlock> CacheBlockData;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                
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
                
                [TagStructure(Size = 0x88)]
                public class DecoratorCacheBlockDataBlock : TagStructure
                {
                    public List<DecoratorPlacementBlock> Placements;
                    public List<DecalVerticesBlock> DecalVertices;
                    public List<IndicesBlock> DecalIndices;
                    public VertexBuffer DecalVertexBuffer;
                    [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public List<SpriteVerticesBlock> SpriteVertices;
                    public List<IndicesBlock1> SpriteIndices;
                    public VertexBuffer SpriteVertexBuffer;
                    [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding1;
                    
                    [TagStructure(Size = 0x18)]
                    public class DecoratorPlacementBlock : TagStructure
                    {
                        public int InternalData1;
                        public int CompressedPosition;
                        public ArgbColor TintColor;
                        public ArgbColor LightmapColor;
                        public int CompressedLightDirection;
                        public int CompressedLight2Direction;
                    }
                    
                    [TagStructure(Size = 0x20)]
                    public class DecalVerticesBlock : TagStructure
                    {
                        public RealPoint3d Position;
                        public RealPoint2d Texcoord0;
                        public RealPoint2d Texcoord1;
                        public ArgbColor Color;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class IndicesBlock : TagStructure
                    {
                        public short Index;
                    }
                    
                    [TagStructure(Size = 0x30)]
                    public class SpriteVerticesBlock : TagStructure
                    {
                        public RealPoint3d Position;
                        public RealVector3d Offset;
                        public RealVector3d Axis;
                        public RealPoint2d Texcoord;
                        public ArgbColor Color;
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class IndicesBlock1 : TagStructure
                    {
                        public short Index;
                    }
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class DecoratorGroupBlock : TagStructure
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
            public class DecoratorCellCollectionBlock : TagStructure
            {
                [TagField(Length = 8)]
                public short[] ChildIndex;
                public short CacheBlockIndex;
                public short GroupCount;
                public int GroupStartIndex;
            }
            
            [TagStructure(Size = 0x40)]
            public class DecoratorProjectedDecalBlock : TagStructure
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
        
        [TagStructure(Size = 0x8)]
        public class ScenarioDecoratorSetPaletteEntryBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "DECR" })]
            public CachedTag DecoratorSet;
        }
    }
}

