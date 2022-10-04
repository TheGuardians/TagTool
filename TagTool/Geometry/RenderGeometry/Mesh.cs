using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using static TagTool.Cache.CacheVersion;
using static TagTool.Tags.TagFieldFlags;
using TagTool.Tags.Resources;

namespace TagTool.Geometry
{
    [TagStructure(Size = 0x70, MinVersion = Halo2Xbox, MaxVersion = Halo2Vista)]
    public class Gen2ResourceMesh : TagStructure
    {
        public List<Part> Parts;
        public List<SubPart> SubParts;

        public List<VisibilityBinding> VisibilityBounds;

        public List<RawVertex> RawVertices;

        public List<StripIndex> StripIndices;


        public byte[] VisibilityMoppCodeData;


        public List<StripIndex> MoppReorderTable;


        public List<VertexBuffer> VertexBuffers;

        [TagField(Flags = Padding, Length = 4, MaxVersion = Halo2Vista)]
        public byte[] Unused1;

        public List<RawPoint> RawPoints;


        public byte[] RuntimePointData;


        public List<RigidPointGroup> RigidPointGroups;


        public List<PointDataIndex> VertexPointIndices;


        public List<NodeMapping> NodeMap;

        [TagField(Length = 4, Flags = TagFieldFlags.Padding, MaxVersion = Halo2Vista)]
        public byte[] Unused2 = new byte[4];

        [TagStructure(Size = 0x14)]
        public class VisibilityBinding : TagStructure
        {
            public RealPoint3d Position;
            public float Radius;
            public byte NodeIndex;

            [TagField(Flags = Padding, Length = 3)]
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

            [TagField(Flags = TagFieldFlags.Padding, Length = 30)]
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


    }

    [TagStructure(Size = 0x44, MinVersion = Halo2Xbox, MaxVersion = Halo2Vista)]
    public class Gen2BSPResourceMesh : TagStructure
    {
        public List<Part> Parts;
        public List<SubPart> SubParts;

        public List<VisibilityBinding> VisibilityBounds;

        public List<RawVertex> RawVertices;

        public List<StripIndex> StripIndices;


        public byte[] VisibilityMoppCodeData;


        public List<StripIndex> MoppReorderTable;


        public List<VertexBuffer> VertexBuffers;

        [TagField(Flags = Padding, Length = 4, MaxVersion = Halo2Vista)]
        public byte[] Unused1;

        [TagStructure(Size = 0x14)]
        public class VisibilityBinding : TagStructure
        {
            public RealPoint3d Position;
            public float Radius;
            public byte NodeIndex;

            [TagField(Flags = Padding, Length = 3)]
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
            public RealVector2d PrimaryLightmapTexcoord;
            public RealVector3d PrimaryLightmapIncidentDirection;
            public RealRgbColor SecondaryLightmapColor;
            public RealVector2d SecondaryLightmapTexcoord;
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

            [TagField(Flags = TagFieldFlags.Padding, Length = 30)]
            public byte[] Unknown = new byte[30];
        }
    }

    /// <summary>
    /// A 3D mesh which can be rendered.
    /// </summary>
    [TagStructure(Size = 0x4C, MaxVersion = HaloOnline700123)]
    [TagStructure(Size = 0x5C, MinVersion = HaloReach)]
    public class Mesh : TagStructure
	{
        public List<Part> Parts;
        public List<SubPart> SubParts;

        [TagField(Length = 8, MinVersion = Halo3Beta)]
        public short[] VertexBufferIndices;

        [TagField(Length = 2, MinVersion = Halo3Beta)]
        public short[] IndexBufferIndices;

        /// <summary>
        /// These should match the values in VertexBufferIndices, taken from the resource. Each time modification to render geometry is made these should be set.
        /// </summary>
        [TagField(Flags = Runtime)]
        public VertexBufferDefinition[] ResourceVertexBuffers;

        /// <summary>
        /// These should match the values in IndexBufferIndices, taken from the resource. Each time modification to render geometry is made these should be set.
        /// </summary>
        [TagField(Flags = Runtime)]
        public IndexBufferDefinition[] ResourceIndexBuffers;

        [TagField(MinVersion = Halo3Beta, MaxVersion = HaloOnline700123)]
        public MeshFlags Flags;
        [TagField(MinVersion = HaloReach, Downgrade = nameof(Flags))]
        public MeshFlagsReach FlagsReach;

        [TagField(MinVersion = Halo3Beta, Format = nameof(Flags))]
        public sbyte RigidNodeIndex;

        [TagField(MinVersion = Halo3Beta, MaxVersion = CacheVersion.HaloOnline700123)]
        public VertexType Type;

        [TagField(MinVersion = HaloReach)]
        public VertexTypeReach ReachType;

        [TagField(MinVersion = Halo3Beta)]
        public PrtSHType PrtType;

        [TagField(MinVersion = HaloReach)]
        public byte LightingPolicy;

        [TagField(MinVersion = Halo3Beta)]
        public PrimitiveType IndexBufferType;

        [TagField(Flags = Padding, Length = 3, MinVersion = Halo3Beta, MaxVersion = HaloOnline700123)]
        public byte[] Unused3;
        [TagField(Flags = Padding, Length = 1, MinVersion = HaloReach)]
        public byte[] Unused4;

        [TagField(MinVersion = Halo3Beta)]
        public List<InstancedBucketBlock> InstanceBuckets;

        [TagField(MinVersion = Halo3Beta)]
        public List<WaterBlock> Water;

        [TagField(MinVersion = HaloReach)]
        public float RuntimeBoundingRadius;
        [TagField(MinVersion = HaloReach)]
        public RealPoint3d RuntimeBoundingOffset;

        [TagStructure(Size = 0x10)]
        public class InstancedBucketBlock : TagStructure
		{
            public short MeshIndex;
            public short DefinitionIndex;
            public List<InstanceIndexBlock> Instances;

            [TagStructure(Size = 0x2)]
			public class InstanceIndexBlock : TagStructure
			{
                public short InstanceIndex;
            }
        }

        [TagStructure(Size = 0x2)]
		public class WaterBlock : TagStructure
		{
            public short Value;
        }
    }
    /// <summary>
    /// Associates geometry with a specific material.
    /// </summary>
    [TagStructure(Size = 0x48, MaxVersion = Halo2Vista)]
    [TagStructure(Size = 0x10, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x14, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnlineED)]
    [TagStructure(Size = 0x10, MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x18, MinVersion = CacheVersion.HaloReach)]
    public class Part : TagStructure
    {
        [TagField(MaxVersion = Halo2Vista, Upgrade = nameof(TypeNew))]
        public PartTypeOld TypeOld;

        [TagField(MaxVersion = Halo2Vista, Upgrade = nameof(FlagsNew))]
        public PartFlagsOld FlagsOld;

        /// <summary>
        /// The block index of the material of the mesh part.
        /// </summary>
        public short MaterialIndex;

        /// <summary>
        /// The transparent sorting index of the mesh part.
        /// </summary>
        [TagField(MinVersion = Halo3Beta)]
        public short TransparentSortingIndex;

        /// <summary>
        /// The index of the first vertex in the index buffer.
        /// </summary>
        public IndexBufferIndex FirstIndex;

        /// <summary>
        /// The number of indices in the part.
        /// </summary>
        public IndexBufferIndex IndexCount;

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
        [TagField(MinVersion = Halo3Beta, Downgrade = nameof(TypeOld))]
        public PartTypeNew TypeNew;

        [TagField(MinVersion = HaloReach)]
        public SpecializedRenderType SpecializedRender; // specialized render?

        /// <summary>
        /// The flags of the mesh part.
        /// </summary>
        [TagField(MinVersion = Halo3Beta, MaxVersion = CacheVersion.HaloOnline700123, Downgrade = nameof(FlagsOld))]
        public PartFlagsNew FlagsNew;
        [TagField(MinVersion = HaloReach)]
        public PartFlagsReach FlagsReach;

        [TagField(MaxVersion = Halo2Vista)]
        public byte MaxNodesPerVertex;

        [TagField(MaxVersion = Halo2Vista)]
        public byte ContributingCompoundNodeCount;

        /// <summary>
        /// The number of vertices that the mesh part uses.
        /// </summary>
        [TagField(MinVersion = Halo3Beta)]
        public ushort VertexCount;

        [TagField(MinVersion = HaloReach)]
        public ushort Unknown1; // Tessellation?

        [TagField(MaxVersion = Halo2Vista)]
        public RealPoint3d Position;

        [TagField(Length = 4, MaxVersion = Halo2Vista)]
        public byte[] NodeIndex = new byte[4];

        [TagField(Length = 3, MaxVersion = Halo2Vista)]
        public float[] NodeWeight = new float[3];

        [TagField(MaxVersion = Halo2Vista)]
        public float LodMipmapMagicNumber;

        [TagField(Length = 6, MaxVersion = Halo2Vista)]
        public float[] Unused2 = new float[6];

        public enum PartTypeOld : short
        {
            NotDrawn,
            OpaqueShadowOnly,
            OpaqueShadowCasting,
            OpaqueNonshadowing,
            Transparent,
            LightmapOnly
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
        public enum PartFlagsOld : ushort
        {
            None,
            Decalable = 1 << 0,
            NewPartTypes = 1 << 1,
            DislikesPhotons = 1 << 2,
            OverrideTriangleList = 1 << 3,
            IgnoredByLightmapper = 1 << 4
        }

        [Flags]
        public enum PartFlagsNew : byte
        {
            DislikesPhotons = 1 << 0,
            IgnoredByLightmapper = 1 << 1,
            HasTransparentSortingPlane = 1 << 2,
            IsWaterSurface = 1 << 3,
            DrawCullRenderingShields = 1 << 4,
            Unused = 1 << 5,
            PreventBackfaceCulling = 1 << 6, // unused?
            PerVertexLightmapPart = 1 << 7, // used at runtime to fix reach static cluster per vertex
        }

        [Flags]
        public enum PartFlagsReach : ushort
        {
            IsWaterSurface = 1 << 0,
            PerVertexLightmapPart = 1 << 1,
            DebugFlagInstancePart = 1 << 2,
            SubpartsHasUberlightsInfo = 1 << 3,
            DrawCullDistanceMedium = 1 << 4,
            DrawCullDistanceClose = 1 << 5,
            DrawCullRenderingShields = 1 << 6,
            CannotSinglePassRender = 1 << 7,
            IsTransparent = 1 << 8,
            CannotTwoPass = 1 << 9,
            // expensive
            TransparentShouldOutputDepthForDoF = 1 << 10,
            DoNotIncludeInStaticLightmap = 1 << 11,
            DoNotIncludeInPvsGeneration = 1 << 12,
            DrawCullRenderingActiveCamo = 1 << 13
        }

        public enum SpecializedRenderType : sbyte
        {
            None,
            Fail,
            Fur,
            FurStencil,
            Decal,
            Shield,
            Water,
            LightmapOnly,
            Hologram
        }
    }

    /// <summary>
    /// A subpart of a mesh which can be rendered selectively.
    /// </summary>
    [TagStructure(Size = 0x8, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0xC, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnlineED)]
    [TagStructure(Size = 0x8, MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x10, MinVersion = CacheVersion.HaloReach)]
    public class SubPart : TagStructure
    {
        /// <summary>
        /// The index of the first vertex in the subpart.
        /// </summary>
        public IndexBufferIndex FirstIndex;

        /// <summary>
        /// The number of indices in the subpart.
        /// </summary>
        public IndexBufferIndex IndexCount;

        /// <summary>
        /// The index of the subpart visibility bounds.
        /// </summary>
        [TagField(MaxVersion = Halo2Vista)]
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
        [TagField(MinVersion = Halo3Beta)]
        public ushort VertexCount;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint AnalyticalLightIndex;
    }

}