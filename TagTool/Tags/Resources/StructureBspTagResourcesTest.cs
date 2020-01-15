using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using static TagTool.Tags.TagFieldFlags;
using TagTool.Tags.Definitions;

namespace TagTool.Tags.Resources
{
    [TagStructure(Name = "structure_bsp_tag_resources", Size = 0x18, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "structure_bsp_tag_resources", Size = 0x30, MinVersion = CacheVersion.Halo3ODST)]
    public class StructureBspTagResourcesTest : TagStructure
	{
        public TagBlock<CollisionBspBlock> CollisionBsps;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public TagBlock<LargeCollisionBspBlock> LargeCollisionBsps;

        public TagBlock<InstancedGeometryBlock> InstancedGeometry;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public TagBlock<HavokDatum> HavokData;

        [TagStructure(Size = 0x60)]
        public class CollisionBspBlock : TagStructure
		{
            public TagBlock<CollisionGeometry.Bsp3dNode> Bsp3dNodes;
            //
            // TODO: Add supernodes block for reach and beyond?
            //
            public TagBlock<CollisionGeometry.Plane> Planes;
            public TagBlock<CollisionGeometry.Leaf> Leaves;
            public TagBlock<CollisionGeometry.Bsp2dReference> Bsp2dReferences;
            public TagBlock<CollisionGeometry.Bsp2dNode> Bsp2dNodes;
            public TagBlock<CollisionGeometry.Surface> Surfaces;
            public TagBlock<CollisionGeometry.Edge> Edges;
            public TagBlock<CollisionGeometry.Vertex> Vertices;
        }

        [TagStructure(Size = 0x60)]
        public class LargeCollisionBspBlock : TagStructure
		{
            public TagBlock<Bsp3dNode> Bsp3dNodes;
            //
            // TODO: Add supernodes block for reach and beyond?
            //
            public TagBlock<CollisionGeometry.Plane> Planes;
            public TagBlock<CollisionGeometry.Leaf> Leaves;
            public TagBlock<Bsp2dReference> Bsp2dReferences;
            public TagBlock<Bsp2dNode> Bsp2dNodes;
            public TagBlock<Surface> Surfaces;
            public TagBlock<Edge> Edges;
            public TagBlock<Vertex> Vertices;

            [TagStructure(Size = 0xC)]
            public class Bsp3dNode : TagStructure
			{
                public int Plane;
                public int BackChild;
                public int FrontChild;
            }

            [TagStructure(Size = 0x8)]
            public class Bsp2dReference : TagStructure
			{
                public int PlaneIndex;
                public int Bsp2dNodeIndex;
            }

            [TagStructure(Size = 0x14)]
            public class Bsp2dNode : TagStructure
			{
                public RealPlane2d Plane;
                public int LeftChild;
                public int RightChild;
            }

            [TagStructure(Size = 0x10, Align = 0x10)]
            public class Surface : TagStructure
			{
                public int Plane;
                public int FirstEdge;
                public short Material;
                public short Unknown;
                public short BreakableSurface;
                public CollisionGeometry.SurfaceFlags Flags;
                public sbyte BestPlaneCalculationVertex;
            }

            [TagStructure(Size = 0x18)]
            public class Edge : TagStructure
			{
                public int StartVertex;
                public int EndVertex;
                public int ForwardEdge;
                public int ReverseEdge;
                public int LeftSurface;
                public int RightSurface;
            }

            [TagStructure(Size = 0x14, Align = 0x8)]
            public class Vertex : TagStructure
			{
                public RealPoint3d Point;
                public int FirstEdge;
                public int Sink;
            }
        }

        [TagStructure(Size = 0xB8, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0xC8, MinVersion = CacheVersion.HaloOnline106708)]
        public class InstancedGeometryBlock : TagStructure
		{
            public int Checksum;
            public RealPoint3d BoundingSphereOffset;
            public float BoundingSphereRadius;
            public CollisionBspBlock CollisionInfo;
            public TagBlock<CollisionBspBlock> CollisionGeometries;
            public TagBlock<CollisionMoppCodesBlock> CollisionMoppCodes;
            public TagBlock<ScenarioStructureBsp.BreakableSurface> BreakableSurfaces;
            public TagBlock<ScenarioStructureBsp.SurfacesPlanes> SurfacePlanes;
            public TagBlock<ScenarioStructureBsp.Plane> Planes;
            public short MeshIndex;
            public short CompressionIndex;
            public float Unknown4;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public TagBlock<CollisionMoppCodesBlock> UnknownBspPhysics;
			[TagField(MinVersion = CacheVersion.HaloOnline106708)]
			public float Unknown6;
        }

        [TagStructure(Size = 0x34)]
        public class HavokDatum : TagStructure
		{
            public int PrefabIndex;
            public TagBlock<HavokGeometry> HavokGeometries;
            public TagBlock<HavokGeometry> HavokInvertedGeometries;
            public RealPoint3d ShapesBoundsMinimum;
            public RealPoint3d ShapesBoundsMaximum;

            [TagStructure(Size = 0x20)]
            public class HavokGeometry : TagStructure
			{
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unused;
                public int CollisionType;
                public int ShapeCount;
                public byte[] Data;
            }
        }

        [TagStructure(Size = 0x40)]
        public class CollisionMoppCodesBlock : TagStructure //todo remove that and define in Havok folder
		{
            public int Unused1;
            public short Size;
            public short Count;
            public int Address;
            public int Unused2;
            public RealQuaternion Offset;
            public int Unused3;
            public int DataSize;
            public int DataCapacityAndFlags;
            public sbyte DataBuildType;
            public sbyte Unused4;
            public short Unused5;
            public TagBlock<byte> Data;
            public sbyte MoppBuildType;
            public byte Unused6;
            public short Unused7;
        }
    }
}