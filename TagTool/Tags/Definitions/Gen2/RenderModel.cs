using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Tag = "mode", Size = 0x84, MinVersion = CacheVersion.Halo2Xbox)]
    public class RenderModel : TagStructure
	{
        public StringId Name;
        public FlagsValue Flags;
        public short Version;
        public int Checksum;

        [TagField(Flags = Padding, Length = 8)]
        public byte[] Unused1 = new byte[8];            // import info block

        public List<RenderGeometryCompression> Compression;
        
        public List<Region> Regions;

        public List<Section> Sections;

        public List<InvalidSectionPairBit> InvalidSectionPairBits;

        public List<SectionGroup> SectionGroups;

        [TagField(Length = 6)]
        public sbyte[] LodSectionIndices = new sbyte[6];

        public short Unknown4;
        public int NodeListChecksum;
        public List<Node> Nodes;
        public List<NodeIndex> NodeMaps;

        public List<MarkerGroup> MarkerGroups;
        public List<RenderMaterial> Materials;

        [TagField(Flags = Padding, Length = 8)]
        public byte[] Unused;       // error block

        public float DontDrawOverCameraCosineAngle;

        public List<PrtInfoBlock> PrtInfo;

        public List<SectionRenderLeaf> SectionRenderLeaves;

       
        [Flags]
        public enum FlagsValue : ushort
        {
            None = 0,
            ForceThirdPerson = 1 << 0,
            ForceCarmackReverse = 1 << 1,
            ForceNodeMaps = 1 << 2,
            GeometryPostprocessed = 1 << 3,
            Bit4 = 1 << 4,
            Bit5 = 1 << 5,
            Bit6 = 1 << 6,
            Bit7 = 1 << 7,
            Bit8 = 1 << 8,
            Bit9 = 1 << 9,
            Bit10 = 1 << 10,
            Bit11 = 1 << 11,
            Bit12 = 1 << 12,
            Bit13 = 1 << 13,
            Bit14 = 1 << 14,
            Bit15 = 1 << 15
        }

        [TagStructure(Size = 0x10)]
        public class Region : TagStructure
		{
            /// <summary>
            /// The name of the region.
            /// </summary>
            [TagField(Flags = Label)]
            public StringId Name;

            /// <summary>
            /// The node map offset of the region.
            /// </summary>
            public short NodeMapOffset;

            /// <summary>
            /// The node map size of the region.
            /// </summary>
            public short NodeMapSize;

            /// <summary>
            /// The permutations belonging to the region.
            /// </summary>
            public List<Permutation> Permutations;

            /// <summary>
            /// A permutation of a region, associating a specific mesh with it.
            /// </summary>
            [TagStructure(Size = 0x10)]
            public class Permutation : TagStructure
			{
                /// <summary>
                /// The name of the permutation as a string id.
                /// </summary>
                [TagField(Flags = Label)]
                public StringId Name;

                /// <summary>
                /// The level-of-detail section indices of the permutation.
                /// </summary>
                [TagField(Length = 6)]
                public short[] LodSectionIndices = new short[6];
            }
        }

        [Flags]
        public enum SectionLightingFlags : ushort
        {
            None,
            HasLightmapTexcoords = 1 << 0,
            HasLightmapIncRad = 1 << 1,
            HasLightmapColors = 1 << 2,
            HasLightmapPrt = 1 << 3
        }

        [Flags]
        public enum SectionFlags : ushort
        {
            None,
            GeometryPostprocessed = 1 << 0
        }

        [TagStructure(Size = 0x5C)]
        public class Section : TagStructure
		{
            public RenderGeometryClassification GlobalGeometryClassification;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused = new byte[2];

            public ushort TotalVertexCount;
            public ushort TotalTriangleCount;
            public ushort TotalPartCount;
            public ushort ShadowCastingTriangleCount;
            public ushort ShadowCastingPartCount;
            public ushort OpaquePointCount;
            public ushort OpaqueVertexCount;
            public ushort OpaquePartCount;
            public ushort OpaqueMaxNodesVertex;
            public ushort ShadowCastingRigidTriangleCount;
            public RenderGeometryClassification GeometryClassification;
            public RenderGeometryCompressionFlags GeometryCompressionFlags;
            public List<RenderGeometryCompression> Compression;
            public byte HardwareNodeCount;
            public byte NodeMapSize;
            public ushort SoftwarePlaneSount;
            public ushort TotalSubpartCount;
            public SectionLightingFlags LightingFlags;
            public short RigidNode;
            public SectionFlags Flags;
            public List<Mesh> Meshes;
            public TagResourceReference BlockOffset;
            public int BlockSize;
            public uint SectionDataSize;
            public uint ResourceDataSize;
            public List<TagResourceGen2> Resources;

            [TagField(Flags = Short)]
            public CachedTag Original;

            public short OwnerTagSectionOffset;
            public byte RuntimeLinked;
            public byte RuntimeLoaded;

            [TagField(Flags = Short)]
            public CachedTag Runtime;
        }

        [TagStructure(Size = 0xC)]
        public class InvalidSectionPairBit : TagStructure
		{
            public int Bits;
			public int Unknown_0x04;
			public int Unknown_0x08;
		}

		[TagStructure(Size = 0xC)]
        public class SectionGroup : TagStructure
		{
            public DetailLevelFlags DetailLevels;
            public short Unknown;
            public List<CompoundNode> CompoundNodes;

            [Flags]
            public enum DetailLevelFlags : ushort
            {
                None,
                Level1 = 1 << 0,
                Level2 = 1 << 1,
                Level3 = 1 << 2,
                Level4 = 1 << 3,
                Level5 = 1 << 4,
                Level6 = 1 << 5
            }

            [TagStructure(Size = 0x10)]
            public class CompoundNode : TagStructure
			{
                [TagField(Length = 4)]
                public sbyte[] NodeIndices = new sbyte[4];

                [TagField(Length = 3)]
                public float[] NodeWeights = new float[3];
            }
        }

        [Flags]
        public enum InstancePlacementFlags : ushort
        {
            None = 0,
            Bit0 = 1 << 0,
            Bit1 = 1 << 1,
            Bit2 = 1 << 2,
            Bit3 = 1 << 3,
            Bit4 = 1 << 4,
            Bit5 = 1 << 5,
            Bit6 = 1 << 6,
            Bit7 = 1 << 7,
            Bit8 = 1 << 8,
            Bit9 = 1 << 9,
            Bit10 = 1 << 10,
            Bit11 = 1 << 11,
            Bit12 = 1 << 12,
            Bit13 = 1 << 13,
            Bit14 = 1 << 14,
            Bit15 = 1 << 15
        }

        [Flags]
        public enum NodeFlags : ushort
        {
            None = 0,
            ForceDeterministic = 1 << 0,
            ForceRenderThread = 1 << 1
        }

        [TagStructure(Size = 0x60)]
        public class Node : TagStructure
		{
            public StringId Name;
            public short ParentNode;
            public short FirstChildNode;
            public short NextSiblingNode;
            public NodeFlags Flags;
            public RealPoint3d DefaultTranslation;
            public RealQuaternion DefaultRotation;
            public float DefaultScale;
            public RealVector3d InverseForward;
            public RealVector3d InverseLeft;
            public RealVector3d InverseUp;
            public RealPoint3d InversePosition;
            public float DistanceFromParent;
        }

        [TagStructure(Size = 0x1)]
		public class NodeIndex : TagStructure
		{
            public byte Node;
        }

        [TagStructure(Size = 0xC)]
        public class MarkerGroup : TagStructure
		{
            public StringId Name;
            public List<Marker> Markers;

            [TagStructure(Size = 0x24)]
            public class Marker : TagStructure
			{
                public sbyte RegionIndex;
                public sbyte PermutationIndex;
                public sbyte NodeIndex;
                public sbyte Unknown3;
                public RealPoint3d Translation;
                public RealQuaternion Rotation;
                public float Scale;
            }
        }

        [TagStructure(Size = 0x58)]
        public class PrtInfoBlock : TagStructure
		{
            public ushort ShOrder;
            public ushort NumberOfClusters;
            public ushort PcaVectorsPerCluster;
            public ushort NumberOfRays;
            public ushort NumberOfBounces;
            public ushort MaterialIndexForSbsfcScattering;
            public float LengthScale;
            public ushort NumberOfLodsInModel;
            public ushort Unknown;
            public List<LodInfoBlock> LodInfo;
            public List<ClusterBasisBlock> ClusterBasis;
            public List<RawPcaDatum> RawPcaData;
            public List<Mesh.VertexBuffer> VertexBuffers;
            public TagResourceReference BlockOffset;
            public int BlockSize;
            public uint SectionDataSize;
            public uint ResourceDataSize;
            public List<TagResourceGen2> Resources;

            [TagField(Flags = Short)]
            public CachedTag Original;

            public short OwnerTagSectionOffset;
            public byte RuntimeLinked;
            public byte RuntimeLoaded;

            [TagField(Flags = Short)]
            public CachedTag Runtime;

            [TagStructure(Size = 0xC)]
            public class LodInfoBlock : TagStructure
			{
                public uint ClusterOffset;
                public List<SectionInfoBlock> SectionInfo;

                [TagStructure(Size = 0x8)]
                public class SectionInfoBlock : TagStructure
				{
                    public int SectionIndex;
                    public uint PcaDataOffset;
                }
            }

            [TagStructure(Size = 0x4)]
            public class ClusterBasisBlock : TagStructure
			{
                public float BasisData;
            }

            [TagStructure(Size = 0x4)]
            public class RawPcaDatum : TagStructure
			{
                public float PcaData;
            }
        }

        [TagStructure(Size = 0x8)]
        public class SectionRenderLeaf : TagStructure
		{
            public List<NodeRenderLeaf> NodeRenderLeaves;

            [TagStructure(Size = 0x10)]
            public class NodeRenderLeaf : TagStructure
			{
                public List<CollisionLeaf> CollisionLeaves;
                public List<SurfaceReference> SurfaceReferences;

                [TagStructure(Size = 0x8)]
                public class CollisionLeaf : TagStructure
				{
                    public short Cluster;
                    public short SurfaceReferenceCount;
                    public int FirstSurfaceReferenceIndex;
                }

                [TagStructure(Size = 0x8)]
                public class SurfaceReference : TagStructure
				{
                    public short StripIndex;
                    public short LightmapTriangleIndex;
                    public int BspNodeIndex;
                }
            }
        }
    }
}