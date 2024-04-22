using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "gbxmodel", Tag = "mod2", Size = 0xE8)]
    public class Gbxmodel : TagStructure
    {
        public FlagsValue Flags;
        public int NodeListChecksum;
        public float SuperHighDetailCutoff; // pixels
        public float HighDetailCutoff; // pixels
        public float MediumDetailCutoff; // pixels
        public float LowDetailCutoff; // pixels
        public float SuperLowCutoff; // pixels
        public short SuperHighDetailNodeCount; // nodes
        public short HighDetailNodeCount; // nodes
        public short MediumDetailNodeCount; // nodes
        public short LowDetailNodeCount; // nodes
        public short SuperLowDetailNodeCount; // nodes
        [TagField(Length = 0x2)]
        public byte[] Padding;
        [TagField(Length = 0x8)]
        public byte[] Padding1;
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public float BaseMapUScale;
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public float BaseMapVScale;
        [TagField(Length = 0x74)]
        public byte[] Padding2;
        public List<ModelMarkersBlock> Markers;
        public List<ModelNodeBlock> Nodes;
        public List<GbxmodelRegionBlock> Regions;
        public List<GbxmodelGeometryBlock> Geometries;
        public List<ModelShaderReferenceBlock> Shaders;
        
        [Flags]
        public enum FlagsValue : uint
        {
            BlendSharedNormals = 1 << 0,
            PartsHaveLocalNodes = 1 << 1,
            IgnoreSkinning = 1 << 2
        }
        
        [TagStructure(Size = 0x40)]
        public class ModelMarkersBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public short MagicIdentifier;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            [TagField(Length = 0x10)]
            public byte[] Padding1;
            public List<ModelMarkerInstanceBlock> Instances;
            
            [TagStructure(Size = 0x20)]
            public class ModelMarkerInstanceBlock : TagStructure
            {
                public sbyte RegionIndex;
                public sbyte PermutationIndex;
                public sbyte NodeIndex;
                [TagField(Length = 0x1)]
                public byte[] Padding;
                public RealPoint3d Translation;
                public RealQuaternion Rotation;
            }
        }
        
        [TagStructure(Size = 0x9C)]
        public class ModelNodeBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public short NextSiblingNodeIndex;
            public short FirstChildNodeIndex;
            public short ParentNodeIndex;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            public RealPoint3d DefaultTranslation;
            public RealQuaternion DefaultRotation;
            public float NodeDistanceFromParent;
            [TagField(Length = 0x20)]
            public byte[] Padding1;
            [TagField(Length = 0x34)]
            public byte[] Padding2;
        }
        
        [TagStructure(Size = 0x4C)]
        public class GbxmodelRegionBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            [TagField(Length = 0x20)]
            public byte[] Padding;
            public List<GbxmodelRegionPermutationBlock> Permutations;
            
            [TagStructure(Size = 0x58)]
            public class GbxmodelRegionPermutationBlock : TagStructure
            {
                [TagField(Length = 32)]
                public string Name;
                public FlagsValue Flags;
                [TagField(Length = 0x1C)]
                public byte[] Padding;
                public short SuperLow;
                public short Low;
                public short Medium;
                public short High;
                public short SuperHigh;
                [TagField(Length = 0x2)]
                public byte[] Padding1;
                public List<ModelRegionPermutationMarkerBlock> Markers;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    CannotBeChosenRandomly = 1 << 0
                }
                
                [TagStructure(Size = 0x50)]
                public class ModelRegionPermutationMarkerBlock : TagStructure
                {
                    [TagField(Length = 32)]
                    public string Name;
                    public short NodeIndex;
                    [TagField(Length = 0x2)]
                    public byte[] Padding;
                    public RealQuaternion Rotation;
                    public RealPoint3d Translation;
                    [TagField(Length = 0x10)]
                    public byte[] Padding1;
                }
            }
        }
        
        [TagStructure(Size = 0x30)]
        public class GbxmodelGeometryBlock : TagStructure
        {
            public FlagsValue Flags;
            [TagField(Length = 0x20)]
            public byte[] Padding;
            public List<GbxmodelGeometryPartBlock> Parts;
            
            [Flags]
            public enum FlagsValue : uint
            {
            }
            
            [TagStructure(Size = 0x84)]
            public class GbxmodelGeometryPartBlock : TagStructure
            {
                public FlagsValue Flags;
                public short ShaderIndex;
                public sbyte PrevFilthyPartIndex;
                public sbyte NextFilthyPartIndex;
                public short CentroidPrimaryNode;
                public short CentroidSecondaryNode;
                public float CentroidPrimaryWeight;
                public float CentroidSecondaryWeight;
                public RealPoint3d Centroid;
                public List<ModelVertexUncompressedBlock> UncompressedVertices;
                public List<ModelVertexCompressedBlock> CompressedVertices;
                public List<ModelTriangleBlock> Triangles;
                [TagField(Length = 0x14)]
                public byte[] Padding;
                [TagField(Length = 0x10)]
                public byte[] Padding1;
                [TagField(Length = 0x1)]
                public byte[] Padding2;
                [TagField(Length = 0x1)]
                public byte[] Padding3;
                [TagField(Length = 0x1)]
                public byte[] Padding4;
                [TagField(Length = 0x1)]
                public byte[] Padding5;
                [TagField(Length = 0x18)]
                public byte[] Padding6;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    StrippedInternal = 1 << 0,
                    Zoner = 1 << 1 //  _model_part_local_nodes
                }
                
                [TagStructure(Size = 0x44)]
                public class ModelVertexUncompressedBlock : TagStructure
                {
                    public RealPoint3d Position;
                    public RealVector3d Normal;
                    public RealVector3d Binormal;
                    public RealVector3d Tangent;
                    public RealPoint2d TextureCoords;
                    public short Node0Index;
                    public short Node1Index;
                    public float Node0Weight;
                    public float Node1Weight;
                }
                
                [TagStructure(Size = 0x20)]
                public class ModelVertexCompressedBlock : TagStructure
                {
                    public RealPoint3d Position;
                    public int Normal111110Bit;
                    public int Binormal111110Bit;
                    public int Tangent111110Bit;
                    public short TextureCoordinateU16Bit;
                    public short TextureCoordinateV16Bit;
                    public sbyte Node0IndexX3;
                    public sbyte Node1IndexX3;
                    public short Node0Weight16Bit;
                }
                
                [TagStructure(Size = 0x6)]
                public class ModelTriangleBlock : TagStructure
                {
                    public short Vertex0Index;
                    public short Vertex1Index;
                    public short Vertex2Index;
                }
            }
        }
        
        [TagStructure(Size = 0x20)]
        public class ModelShaderReferenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "shdr" })]
            public CachedTag Shader;
            public short Permutation;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            [TagField(Length = 0xC)]
            public byte[] Padding1;
        }
    }
}

