using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.Serialization;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "render_model", Tag = "mode", Size = 0x84, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Name = "render_model", Tag = "mode", Size = 0x1CC, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "render_model", Tag = "mode", Size = 0x1D0, MinVersion = CacheVersion.HaloOnline106708)]
    public class RenderModel
    {
        public StringId Name;
        public FlagsValue Flags;
        public short Version;
        public int Checksum;

        [TagField(Padding = true, Length = 8, MaxVersion = CacheVersion.Halo2Vista)]
        public byte[] Unused1 = new byte[8];

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public List<RenderGeometryCompression> Compression;
        
        public List<Region> Regions;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public List<Section> Sections;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public List<InvalidSectionPairBit> InvalidSectionPairBits;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public List<SectionGroup> SectionGroups;

        [TagField(Length = 6, MaxVersion = CacheVersion.Halo2Vista)]
        public sbyte[] LodSectionIndices = new sbyte[6];

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public short Unknown4;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int Unknown18;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int InstanceStartingMeshIndex;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public List<InstancePlacement> InstancePlacements;

        public int NodeListChecksum;
        public List<Node> Nodes;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public List<NodeIndex> NodeMaps;

        public List<MarkerGroup> MarkerGroups;
        public List<RenderMaterial> Materials;

        [TagField(Padding = true, Length = 12)]
        public byte[] Unused; // "Errors" block

        public float DontDrawOverCameraCosineAngle;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public List<PrtInfoBlock> PrtInfo;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public List<SectionRenderLeaf> SectionRenderLeaves;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public RenderGeometry Geometry = new RenderGeometry();

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public List<TagBlock17> UnknownE8;

        [TagField(Length = 48, MinVersion = CacheVersion.Halo3Retail)]
        public float[] Unknowns = new float[48];

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public List<TagBlock18> Unknown1B4;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public List<RuntimeNodeOrientation> RuntimeNodeOrientations;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public int Unknown1CC;

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

        /// <summary>
        /// A region of a model.
        /// </summary>
        [TagStructure(Size = 0x10)]
        public class Region
        {
            /// <summary>
            /// The name of the region.
            /// </summary>
            [TagField(Label = true)]
            public StringId Name;

            /// <summary>
            /// The node map offset of the region.
            /// </summary>
            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public short NodeMapOffset;

            /// <summary>
            /// The node map size of the region.
            /// </summary>
            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public short NodeMapSize;

            /// <summary>
            /// The permutations belonging to the region.
            /// </summary>
            public List<Permutation> Permutations;

            /// <summary>
            /// A permutation of a region, associating a specific mesh with it.
            /// </summary>
            [TagStructure(Size = 0x10, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x18, MinVersion = CacheVersion.Halo3ODST)]
            public class Permutation
            {
                /// <summary>
                /// The name of the permutation as a string id.
                /// </summary>
                [TagField(Label = true)]
                public StringId Name;

                /// <summary>
                /// The level-of-detail section indices of the permutation.
                /// </summary>
                [TagField(Length = 6, MaxVersion = CacheVersion.Halo2Vista)]
                public short[] LodSectionIndices = new short[6];

                /// <summary>
                /// The index of the first mesh belonging to the permutation.
                /// </summary>
                [TagField(MinVersion = CacheVersion.Halo3Retail)]
                public short MeshIndex;

                /// <summary>
                /// The number of meshes belonging to the permutation.
                /// </summary>
                [TagField(MinVersion = CacheVersion.Halo3Retail)]
                public ushort MeshCount;

                [TagField(MinVersion = CacheVersion.Halo3Retail)]
                public int Unknown8;

                [TagField(MinVersion = CacheVersion.Halo3Retail)]
                public int UnknownC;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public int Unknown10;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public int Unknown14;
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
        public class Section
        {
            public RenderGeometryClassification GlobalGeometryClassification;

            [TagField(Padding = true, Length = 2)]
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

            [TagField(Padding = true, Length = 8)]
            public byte[] Unused2 = new byte[8]; // "Section Data" block

            public int BlockOffset;
            public int BlockSize;
            public uint SectionDataSize;
            public uint ResourceDataSize;
            public List<ResourceGen2> Resources;

            [TagField(Short = true)]
            public CachedTagInstance Original;

            public short OwnerTagSectionOffset;
            public byte RuntimeLinked;
            public byte RuntimeLoaded;

            [TagField(Short = true)]
            public CachedTagInstance Runtime;
        }

        [TagStructure(Size = 0xC)]
        public class InvalidSectionPairBit
        {
            public int Bits;
        }

        [TagStructure(Size = 0xC)]
        public class SectionGroup
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
            public class CompoundNode
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

        [TagStructure(Size = 0x3C)]
        public class InstancePlacement
        {
            public StringId Name;
            public int NodeIndex;
            public float Scale;
            public RealPoint3d Forward;
            public RealPoint3d Left;
            public RealPoint3d Up;
            public RealPoint3d Position;
        }

        [Flags]
        public enum NodeFlags : ushort
        {
            None = 0,
            ForceDeterministic = 1 << 0,
            ForceRenderThread = 1 << 1
        }

        [TagStructure(Size = 0x60)]
        public class Node
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
        public struct NodeIndex
        {
            public byte Node;
        }

        [TagStructure(Size = 0xC, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3Retail)]
        public class MarkerGroup
        {
            public StringId Name;
            public List<Marker> Markers;

            [TagStructure(Size = 0x24)]
            public class Marker
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
        public class PrtInfoBlock
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
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public uint BlockOffset;
            public uint BlockSize;
            public uint SectionDataSize;
            public uint ResourceDataSize;
            public List<ResourceGen2> Resources;

            [TagField(Short = true)]
            public CachedTagInstance Model;

            public short OwnerTagSectionOffset;
            public short Unknown6;
            public uint Unknown7;

            [TagStructure(Size = 0xC)]
            public class LodInfoBlock
            {
                public uint ClusterOffset;
                public List<SectionInfoBlock> SectionInfo;

                [TagStructure(Size = 0x8)]
                public class SectionInfoBlock
                {
                    public int SectionIndex;
                    public uint PcaDataOffset;
                }
            }

            [TagStructure(Size = 0x4)]
            public class ClusterBasisBlock
            {
                public float BasisData;
            }
        }

        [TagStructure(Size = 0x8)]
        public class SectionRenderLeaf
        {
            public List<NodeRenderLeaf> NodeRenderLeaves;

            [TagStructure(Size = 0x10)]
            public class NodeRenderLeaf
            {
                public List<CollisionLeaf> CollisionLeaves;
                public List<SurfaceReference> SurfaceReferences;

                [TagStructure(Size = 0x8)]
                public class CollisionLeaf
                {
                    public short Cluster;
                    public short SurfaceReferenceCount;
                    public int FirstSurfaceReferenceIndex;
                }

                [TagStructure(Size = 0x8)]
                public class SurfaceReference
                {
                    public short StripIndex;
                    public short LightmapTriangleIndex;
                    public int BspNodeIndex;
                }
            }
        }

        [TagStructure(Size = 0x1C)]
        public class TagBlock17
        {
            public float Unknown0;
            public float Unknown4;
            public float Unknown8;
            public float UnknownC;
            public float Unknown10;
            public float Unknown14;
            public float Unknown18;
        }

        [TagStructure(Size = 0x150)]
        public class TagBlock18
        {
            public float Unknown0;
            public float Unknown4;
            public float Unknown8;
            public float UnknownC;
            public float Unknown10;
            public float Unknown14;
            public float Unknown18;
            public float Unknown1C;
            public float Unknown20;
            public float Unknown24;
            public float Unknown28;
            public float Unknown2C;
            public float Unknown30;
            public float Unknown34;
            public float Unknown38;
            public float Unknown3C;
            public float Unknown40;
            public float Unknown44;
            public float Unknown48;
            public float Unknown4C;
            public float Unknown50;
            public float Unknown54;
            public float Unknown58;
            public float Unknown5C;
            public float Unknown60;
            public float Unknown64;
            public float Unknown68;
            public float Unknown6C;
            public float Unknown70;
            public float Unknown74;
            public float Unknown78;
            public float Unknown7C;
            public float Unknown80;
            public float Unknown84;
            public float Unknown88;
            public float Unknown8C;
            public float Unknown90;
            public float Unknown94;
            public float Unknown98;
            public float Unknown9C;
            public float UnknownA0;
            public float UnknownA4;
            public float UnknownA8;
            public float UnknownAC;
            public float UnknownB0;
            public float UnknownB4;
            public float UnknownB8;
            public float UnknownBC;
            public float UnknownC0;
            public float UnknownC4;
            public float UnknownC8;
            public float UnknownCC;
            public float UnknownD0;
            public float UnknownD4;
            public float UnknownD8;
            public float UnknownDC;
            public float UnknownE0;
            public float UnknownE4;
            public float UnknownE8;
            public float UnknownEC;
            public float UnknownF0;
            public float UnknownF4;
            public float UnknownF8;
            public float UnknownFC;
            public float Unknown100;
            public float Unknown104;
            public float Unknown108;
            public float Unknown10C;
            public float Unknown110;
            public float Unknown114;
            public float Unknown118;
            public float Unknown11C;
            public float Unknown120;
            public float Unknown124;
            public float Unknown128;
            public float Unknown12C;
            public float Unknown130;
            public float Unknown134;
            public float Unknown138;
            public float Unknown13C;
            public float Unknown140;
            public float Unknown144;
            public float Unknown148;
            public float Unknown14C;
        }

        [TagStructure(Align = 0x10, Size = 0x20)]
        public class RuntimeNodeOrientation
        {
            public RealQuaternion Rotation;
            public RealPoint3d Translation;
            public float Scale;
        }
    }
}