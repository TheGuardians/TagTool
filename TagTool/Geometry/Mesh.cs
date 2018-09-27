using TagTool.Tags;
using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Geometry
{
    /// <summary>
    /// A 3D mesh which can be rendered.
    /// </summary>
    [TagStructure(Size = 0xB4, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0x4C, MinVersion = CacheVersion.Halo3Retail)]
    public class Mesh : TagStructure
	{
        public List<Part> Parts;
        public List<SubPart> SubParts;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public List<VisibilityBinding> VisibilityBounds;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public List<RawVertex> RawVertices;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public List<StripIndex> StripIndices;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public byte[] VisibilityMoppCodeData;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public List<StripIndex> MoppReorderTable;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public List<VertexBuffer> VertexBuffers;

        [TagField(Padding = true, Length = 4, MaxVersion = CacheVersion.Halo2Vista)]
        public byte[] Unused1;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public List<RawPoint> RawPoints;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public byte[] RuntimePointData;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public List<RigidPointGroup> RigidPointGroups;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public List<PointDataIndex> VertexPointIndices;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public List<NodeMapping> NodeMap;

        [TagField(Padding = true, Length = 4, MaxVersion = CacheVersion.Halo2Vista)]
        public byte[] Unused2 = new byte[4];

        [TagField(Length = 8, MinVersion = CacheVersion.Halo3Retail)]
        public ushort[] VertexBufferIndices;

        [TagField(Length = 2, MinVersion = CacheVersion.Halo3Retail)]
        public ushort[] IndexBufferIndices;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public MeshFlags Flags;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public sbyte RigidNodeIndex;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public VertexType Type;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public PrtType PrtType;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public PrimitiveType IndexBufferType;

        [TagField(Padding = true, Length = 3, MinVersion = CacheVersion.Halo3Retail)]
        public byte[] Unused3;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public List<InstancedGeometryBlock> InstancedGeometry;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public List<WaterBlock> Water;

        /// <summary>
        /// Associates geometry with a specific material.
        /// </summary>
        [TagStructure(Size = 0x48, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3Retail)]
        public class Part : TagStructure
		{
            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public PartTypeOld TypeOld;

            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public PartFlagsOld FlagsOld;

            /// <summary>
            /// The block index of the material of the mesh part.
            /// </summary>
            public short MaterialIndex;

            /// <summary>
            /// The transparent sorting index of the mesh part.
            /// </summary>
            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public short TransparentSortingIndex;

            /// <summary>
            /// The index of the first vertex in the index buffer.
            /// </summary>
            public ushort FirstIndex;

            /// <summary>
            /// The number of indices in the part.
            /// </summary>
            public ushort IndexCount;

            /// <summary>
            /// The index of the first subpart that makes up this part.
            /// </summary>
            public short FirstSubPartIndex;

            /// <summary>
            /// The number of subparts that make up this part.
            /// </summary>
            public short SubPartCount;
            
            /// <summary>
            /// The type of the mesh part.
            /// </summary>
            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public PartTypeNew TypeNew;

            /// <summary>
            /// The flags of the mesh part.
            /// </summary>
            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public PartFlagsNew FlagsNew;

            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public byte MaxNodesPerVertex;

            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public byte ContributingCompoundNodeCount;

            /// <summary>
            /// The number of vertices that the mesh part uses.
            /// </summary>
            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public ushort VertexCount;

            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public RealPoint3d Position;

            [TagField(Length = 4, MaxVersion = CacheVersion.Halo2Vista)]
            public byte[] NodeIndex = new byte[4];

            [TagField(Length = 3, MaxVersion = CacheVersion.Halo2Vista)]
            public float[] NodeWeight = new float[3];

            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public float LodMipmapMagicNumber;

            [TagField(Padding = true, Length = 24, MaxVersion = CacheVersion.Halo2Vista)]
            public byte[] Unused = new byte[24];

            public enum PartTypeOld : short
            {
                NotDrawn,
                OpaqueShadowOnly,
                OpaqueShadowCasting,
                OpaqueNonshadowing,
                Transparent,
                LightmapOnly
            }

            [Flags]
            public enum PartFlagsOld : ushort
            {
                None,
                Decalable = 1 << 0,
                NewPartTypes = 1 << 1,
                DislikesPhotons = 1 << 2,
                OverrideTriangleList = 1 << 3,
                IgnoredByLightmapper = 1 << 4
            }

            public enum PartTypeNew : sbyte
            {
                NotDrawn,
                OpaqueShadowOnly,
                OpaqueShadowCasting,
                OpaqueNonshadowing,
                Transparent,
                LightmapOnly
            }

            [Flags]
            public enum PartFlagsNew : byte
            {
                None,
                IsWaterSurface = 1 << 0,
                PerVertexLightmapPart = 1 << 1,
                RenderInZPrepass = 1 << 2,
                CanBeRenderedInDrawBundles = 1 << 3,
                DrawCullDistanceMedium = 1 << 4,
                DrawCullDistanceClose = 1 << 5,
                DrawCullRenderingShields = 1 << 6,
                DrawCullRenderingActiveCamo = 1 << 7
            }
        }

        /// <summary>
        /// A subpart of a mesh which can be rendered selectively.
        /// </summary>
        [TagStructure(Size = 0x8)]
        public class SubPart : TagStructure
		{
            /// <summary>
            /// The index of the first vertex in the subpart.
            /// </summary>
            public ushort FirstIndex;

            /// <summary>
            /// The number of indices in the subpart.
            /// </summary>
            public ushort IndexCount;

            /// <summary>
            /// The index of the subpart visibility bounds.
            /// </summary>
            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public short VisibilityBoundsIndex;

            /// <summary>
            /// The index of the part which this subpart belongs to.
            /// </summary>
            public short PartIndex;

            /// <summary>
            /// The number of vertices that the part uses.
            /// </summary>
            /// <remarks>
            /// Note that this actually seems to be unused. The value is pulled from
            /// the vertex buffer definition instead.
            /// </remarks>
            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public ushort VertexCount;
        }

        [TagStructure(Size = 0x14)]
        public class VisibilityBinding : TagStructure
		{
            public RealPoint3d Position;
            public float Radius;
            public byte NodeIndex;

            [TagField(Padding = true, Length = 3)]
            public byte[] Unused = new byte[3];
        }

        [TagStructure(Size = 0x44)]
        public class RawPoint : TagStructure
		{
            public RealPoint3d Position;

            [TagField(Length = 4)]
            public int[] NodeIndicesOld = new int[4];

            [TagField(Length = 4)]
            public float[] NodeWeights = new float[4];

            [TagField(Length = 4)]
            public int[] NodeIndices = new int[4];

            public int UseNewNodeIndices;
            public int AdjustedCompoundNodeIndex;
        }

        [TagStructure(Size = 0xC4)]
        public class RawVertex : TagStructure
		{
            public RawPoint Point = new RawPoint();
            public RealPoint2d Texcoord;
            public RealVector3d Normal;
            public RealVector3d Binormal;
            public RealVector3d Tangent;
            public RealVector3d AnisotropicBinormal;
            public RealPoint2d SecondaryTexcoord;
            public RealRgbColor PrimaryLightmapColor;
            public RealPoint2d PrimaryLightmapTexcoord;
            public RealVector3d PrimaryLightmapIncidentDirection;
            public RealRgbColor SecondaryLightmapColor;
            public RealPoint2d SecondaryLightmapTexcoord;
            public RealVector3d SecondaryLightmapIncidentDirection;
        }

        [TagStructure(Size = 0x2)]
        public class StripIndex : TagStructure
		{
            public short Index;
        }

        [TagStructure(Size = 0x20)]
        public class VertexBuffer : TagStructure
		{
            public byte TypeIndex;
            public byte StrideIndex;

            [TagField(Length = 30)]
            public byte[] Unknown = new byte[30];
        }

        [TagStructure(Size = 0x4)]
        public class RigidPointGroup : TagStructure
		{
            public byte RigidNodeIndex;
            public byte PointIndex;
            public short PointCount;
        }

        [TagStructure(Size = 0x2)]
        public class PointDataIndex : TagStructure
		{
            public short Index;
        }

        [TagStructure(Size = 0x1)]
        public class NodeMapping : TagStructure
		{
            public byte NodeIndex;
        }

        [TagStructure(Size = 0x10)]
        public class InstancedGeometryBlock : TagStructure
		{
            public short Section1;
            public short Section2;
            public List<ContentsBlock> Contents;

            [TagStructure(Size = 0x2)]
			public /*was_struct*/ class ContentsBlock : TagStructure
			{
                public short Value;
            }
        }

        [TagStructure(Size = 0x2)]
		public /*was_struct*/ class WaterBlock : TagStructure
		{
            public short Value;
        }
    }
}