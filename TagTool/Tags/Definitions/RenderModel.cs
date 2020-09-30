using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "render_model", Tag = "mode", Size = 0x1CC, MinVersion = CacheVersion.Halo3Retail)]
    public class RenderModel : TagStructure
	{
        public StringId Name;
        public FlagsValue Flags;
        public short Version;
        public int Checksum;

        public List<Region> Regions;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int Unknown18;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int InstanceStartingMeshIndex;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public List<InstancePlacement> InstancePlacements;

        public int NodeListChecksum;
        public List<Node> Nodes;

        public List<MarkerGroup> MarkerGroups;
        public List<RenderMaterial> Materials;

        [TagField(Flags = Padding, Length = 12)]
        public byte[] Unused; // "Errors" block

        public float DontDrawOverCameraCosineAngle;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public RenderGeometry Geometry = new RenderGeometry();

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public List<TagBlock17> UnknownE8;

        [TagField(Length = 16, MinVersion = CacheVersion.Halo3Retail)]
        public float[] SHRed = new float[SphericalHarmonics.Order3Count];
        [TagField(Length = 16, MinVersion = CacheVersion.Halo3Retail)]
        public float[] SHGreen = new float[SphericalHarmonics.Order3Count];
        [TagField(Length = 16, MinVersion = CacheVersion.Halo3Retail)]
        public float[] SHBlue = new float[SphericalHarmonics.Order3Count];

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public List<UnknownSHProbe> UnknownSHProbes;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public List<RuntimeNodeOrientation> RuntimeNodeOrientations;

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

        [TagStructure(Size = 0x5C, MaxVersion = CacheVersion.Halo2Vista)]
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
            public DatumHandle BlockOffset;
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

        [TagStructure(Size = 0xC, MaxVersion = CacheVersion.Halo2Vista)]
        public class InvalidSectionPairBit : TagStructure
		{
            public int Bits;
			public int Unknown_0x04;
			public int Unknown_0x08;
		}

		[TagStructure(Size = 0xC, MaxVersion = CacheVersion.Halo2Vista)]
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

            [TagStructure(Size = 0x10, MaxVersion = CacheVersion.Halo2Vista)]
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

        [TagStructure(Size = 0x3C, MinVersion = CacheVersion.Halo3Retail)]
        public class InstancePlacement : TagStructure
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

        [TagStructure(Size = 0xC, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3Retail)]
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

        [TagStructure(Size = 0x1C)]
        public class TagBlock17 : TagStructure
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
        public class UnknownSHProbe : TagStructure
		{
            public RealPoint3d Position;
            [TagField(Length = 81, MinVersion = CacheVersion.Halo3Retail)]
            public float[] Coefficients = new float[81];
        }

        [TagStructure(Align = 0x10, Size = 0x20)]
        public class RuntimeNodeOrientation : TagStructure
		{
            public RealQuaternion Rotation;
            public RealPoint3d Translation;
            public float Scale;
        }
    }
}