using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Resources
{
    [TagStructure(Name = "structure_bsp_tag_resources", Size = 0x18, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "structure_bsp_tag_resources", Size = 0x30, MinVersion = CacheVersion.Halo3ODST)]
    public class StructureBspTagResourcesTest : TagStructure
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
            public List<CollisionGeometry.Bsp3dNode> Bsp3dNodes;
            //
            // TODO: Add supernodes block for reach and beyond?
            //
            public List<CollisionGeometry.Plane> Planes;
            public List<CollisionGeometry.Leaf> Leaves;
            public List<CollisionGeometry.Bsp2dReference> Bsp2dReferences;
            public List<CollisionGeometry.Bsp2dNode> Bsp2dNodes;
            public List<CollisionGeometry.Surface> Surfaces;
            public List<CollisionGeometry.Edge> Edges;
            public List<CollisionGeometry.Vertex> Vertices;
        }

        [TagStructure(Size = 0x60)]
        public class LargeCollisionBspBlock : TagStructure
		{
            public List<Bsp3dNode> Bsp3dNodes;
            //
            // TODO: Add supernodes block for reach and beyond?
            //
            public List<CollisionGeometry.Plane> Planes;
            public List<CollisionGeometry.Leaf> Leaves;
            public List<Bsp2dReference> Bsp2dReferences;
            public List<Bsp2dNode> Bsp2dNodes;
            public List<Surface> Surfaces;
            public List<Edge> Edges;
            public List<Vertex> Vertices;

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
            public List<CollisionBspBlock> CollisionGeometries;
            public List<CollisionBspPhysicsBlock> BspPhysics;
            public List<Unknown1Block> Unknown1;
            public List<Unknown2Block> Unknown2;
            public List<Unknown3Block> Unknown3;
            public short MeshIndex;
            public short CompressionIndex;

            public float Unknown4;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public List<Unknown4Block> Unknown5;

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

            [TagStructure(Size = 0x4)]
            public class Unknown3Block : TagStructure
			{
                public short Unknown;
                public short Unknown1;
            }

            [TagStructure(Size = 0x4)]
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
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unused;
                public int CollisionType;
                public int ShapeCount;
                public byte[] Data;
            }
        }

        [TagStructure(Size = 0x40, Align = 0x10)]
        public class CollisionBspPhysicsBlock : TagStructure
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
            public List<Datum> Data;
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