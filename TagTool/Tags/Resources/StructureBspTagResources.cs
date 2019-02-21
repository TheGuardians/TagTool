using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;

namespace TagTool.Tags.Resources
{
    [TagStructure(Name = "structure_bsp_tag_resources", Size = 0x18, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "structure_bsp_tag_resources", Size = 0x30, MinVersion = CacheVersion.Halo3ODST)]
    public class StructureBspTagResources : TagStructure
	{
        public List<CollisionBspBlock> CollisionBsps;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<LargeCollisionBspBlock> LargeCollisionBsps;

        public List<InstancedGeometryBlock> InstancedGeometry;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<HavokDatum> HavokData;

        [TagStructure(Size = 0x60)]
        public class CollisionBspBlock : TagStructure
		{
            public TagBlock<CollisionGeometry.Bsp3dNode> Bsp3dNodes;
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
            public RealQuaternion Position;
            public float Radius;
            public CollisionBspBlock CollisionBsp;
            public List<CollisionBspBlock> CollisionGeometries;
            public List<CollisionMoppCodeResource> CollisionMoppCodes;
            public TagBlock<Unknown1Block> Unknown1;
            public TagBlock<Unknown2Block> Unknown2;
            public TagBlock<Unknown3Block> Unknown3;
            public short MeshIndex;
            public short CompressionIndex;

            public float Unknown4;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public TagBlock<Unknown4Block> Unknown5;

			[TagField(MinVersion = CacheVersion.HaloOnline106708)]
			public float Unknown6;

            [TagStructure(Size = 0x4)]
            public class Unknown1Block : TagStructure
			{
                public uint Unknown;
            }

            [TagStructure(Size = 0x4, MaxVersion = CacheVersion.Halo3ODST)]
            [TagStructure(Size = 0x8, MinVersion = CacheVersion.HaloOnline106708)]
            public class Unknown2Block : TagStructure
			{
                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public short Unknown1_H3;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public int Unknown1;

                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public short Unknown2_H3;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public int Unknown2;
            }

            [TagStructure]
            public class Unknown3Block : TagStructure
			{
                // public uint Unknown;
                public short Unknown;
                public short Unknown1;
            }

            [TagStructure]
            public class Unknown4Block : TagStructure
			{
                public uint Unknown;
            }
        }

        [TagStructure(Size = 0x34)]
        public class HavokDatum : TagStructure
		{
            public int PrefabIndex;
            public List<HavokGeometry> HavokGeometries;
            public List<HavokGeometry> HavokInvertedGeometries;
            public RealPoint3d ShapesBoundsMinimum;
            public RealPoint3d ShapesBoundsMaximum;

            [TagStructure(Size = 0x20)]
            public class HavokGeometry : TagStructure
			{
                [TagField(Padding = true, Length = 4)]
                public byte[] Unused;
                public int CollisionType;
                public int ShapeCount;
                public byte[] Data;
            }
        }

        [TagStructure(Size = 0x40, Align = 0x10)]
        public class CollisionMoppCodeResource : TagStructure
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
            public TagBlock<Datum> Data;
            public sbyte MoppBuildType;
            public byte Unused6;
            public short Unused7;

			[TagStructure(Size = 0x1)]
			public class Datum : TagStructure
			{
				public byte Value;
			}
        }
    }
}