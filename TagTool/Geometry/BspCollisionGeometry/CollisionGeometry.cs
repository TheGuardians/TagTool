using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using System;
using System.Collections.Generic;
using TagTool.Havok;

namespace TagTool.Geometry.BspCollisionGeometry
{
    [TagStructure(Size = 0x60, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x6C, MinVersion = CacheVersion.HaloReach)]
    public class CollisionGeometry : TagStructure
	{
        public TagBlock<Bsp3dNode> Bsp3dNodes;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public TagBlock<Bsp3dSupernode> Bsp3dSupernodes;

        public TagBlock<Plane> Planes;
        public TagBlock<Leaf> Leaves;
        public TagBlock<Bsp2dReference> Bsp2dReferences;
        public TagBlock<Bsp2dNode> Bsp2dNodes;
        public TagBlock<Surface> Surfaces;
        public TagBlock<Edge> Edges;
        public TagBlock<Vertex> Vertices;

        public Dictionary<int, List<int>> GenerateIndices()
        {
            var result = new Dictionary<int, List<int>>();

            for (var surfaceIndex = 0; surfaceIndex < Surfaces.Count; surfaceIndex++)
            {
                var indices = new List<int>();

                for (var edgeIndex = 0; edgeIndex < Edges.Count; edgeIndex++)
                {
                    var edge = Edges[edgeIndex];

                    if (surfaceIndex == edge.LeftSurface || surfaceIndex == edge.RightSurface)
                    {
                        if (!indices.Contains(edge.StartVertex))
                            indices.Add(edge.StartVertex);
                        if (!indices.Contains(edge.EndVertex))
                            indices.Add(edge.EndVertex);
                    }
                }

                result[surfaceIndex] = indices;
            }

            return result;
        }
    }

    [TagStructure(Size = 0x8, Align = 0x8)]
    public class Bsp3dNode : TagStructure
    {
        public ulong Value;

        public int Plane
        {
            get => (int)(Value & 0xffff);
            set => Value |= ((ulong)value & 0xffff);
        }

        public int BackChild
        {
            get => (int)((Value >> 16) & 0xffffff);
            set => Value = (Value & 0xffffff000000ffffUL) | (((ulong)value & 0xffffff) << 16);
        }

        public int FrontChild
        {
            get => (int)((Value >> 40) & 0xffffff);
            set => Value = (Value & 0xffffffffffUL) | (((ulong)value & 0xffffff) << 40);
        }

        public int GetChild(Side side)
        {
            return side == Side.Front ? FrontChild : BackChild;
        }

        public static int GetChildIndex(int child)
        {
            int index = child & 0x7fffff;
            int signExtMask = 1 << 22;
            return (signExtMask ^ index) - signExtMask;
        }

        public static ChildType GetChildType(int child)
        {
            return (ChildType)((child >> 23) & 1);
        }

        public static int CreateChild(ChildType type, int index)
        {
            return ((int)type << 23) | (index & 0x7fffff);
        }

        public enum ChildType
        {
            Node,
            Leaf
        }

        public enum Side
        {
            Front,
            Back
        }
    }

    [TagStructure(Size = 0x80)]
    public class Bsp3dSupernode
    {
        [TagField(Length = 15)]
        public float[] PlaneValues = new float[15];

        public int PlaneDimensions;

        [TagField(Length = 16)]
        public int[] ChildIndices = new int[16];
    }

    [TagStructure(Size = 0x10)]
    public class Plane : TagStructure
    {
        public RealPlane3d Value;
    }

    [TagStructure(Size = 0x4, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0x8, MinVersion = CacheVersion.Halo3Retail)]
    public class Leaf : TagStructure
    {
        public LeafFlags Flags;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public LeafFlags Flags2;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public byte Bsp2dReferenceCount_H2;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public ushort Bsp2dReferenceCount;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public short FirstBsp2dReference_H2;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public uint FirstBsp2dReference;
    }

    [TagStructure(Size = 0x4)]
    public class Bsp2dReference : TagStructure
    {
        public short PlaneIndex;
        public short Bsp2dNodeIndex;
    }

    [TagStructure(Size = 0x10, Align = 0x10)]
    public class Bsp2dNode : TagStructure
    {
        public RealPlane2d Plane;
        public short LeftChild;
        public short RightChild;
    }

    [TagStructure(Size = 0x8, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0xC, MinVersion = CacheVersion.Halo3Retail)]
    public class Surface : TagStructure
    {
        public ushort Plane;

        public ushort FirstEdge;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public SurfaceFlags Flags_H2;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public short MaterialIndex;
        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public short BreakableSurfaceSet;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public byte BreakableSurfaceIndex_H2;
        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public short BreakableSurfaceIndex;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public short MaterialIndex_H2;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public SurfaceFlags Flags;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public byte BestPlaneCalculationVertex;
    }

    [TagStructure(Size = 0xC)]
    public class Edge : TagStructure
    {
        public ushort StartVertex;
        public ushort EndVertex;
        public ushort ForwardEdge;
        public ushort ReverseEdge;
        public ushort LeftSurface;
        public ushort RightSurface;
    }

    [TagStructure(Size = 0x10, Align = 0x10)]
    public class Vertex : TagStructure
    {
        public RealPoint3d Point;
        public ushort FirstEdge;
        public short Sink;
    }

    [Flags]
    public enum SurfaceFlags : byte
    {
        None = 0,
        TwoSided = 1 << 0,
        Invisible = 1 << 1,
        Climbable = 1 << 2,
        Breakable = 1 << 3,
        Invalid = 1 << 4,
        Conveyor = 1 << 5,
        Slip = 1 << 6,
        PlaneNegated = 1 << 7
    }

    [Flags]
    public enum LeafFlags : byte
    {
        None = 0,
        ContainsDoubleSidedSurfaces = 1 << 0
    }
}