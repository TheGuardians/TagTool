using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using System;
using System.Collections.Generic;
using TagTool.Havok;

namespace TagTool.Geometry.BspCollisionGeometry
{
    [TagStructure(Size = 0x40, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0x60, MinVersion = CacheVersion.Halo3Retail)]
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

        public short Plane
        {
            get => (short)((Value >> 48) & 0xFFFF);
            set
            {
                var allBytes = BitConverter.GetBytes(Value);
                var newBytes = BitConverter.GetBytes(value);
                for (var i = 0; i < newBytes.Length; i++)
                    allBytes[((allBytes.Length - 1) - 0) - i] = newBytes[(newBytes.Length - 1) - i];
                Value = BitConverter.ToUInt64(allBytes, 0);
            }
        }

        public byte FrontChildLower
        {
            get => (byte)((Value >> 40) & 0xFF);
            set
            {
                var allBytes = BitConverter.GetBytes(Value);
                allBytes[(allBytes.Length - 1) - 2] = value;
                Value = BitConverter.ToUInt64(allBytes, 0);
            }
        }

        public byte FrontChildMid
        {
            get => (byte)((Value >> 32) & 0xFF);
            set
            {
                var allBytes = BitConverter.GetBytes(Value);
                allBytes[(allBytes.Length - 1) - 3] = value;
                Value = BitConverter.ToUInt64(allBytes, 0);
            }
        }

        public byte FrontChildUpper
        {
            get => (byte)((Value >> 24) & 0xFF);
            set
            {
                var allBytes = BitConverter.GetBytes(Value);
                allBytes[(allBytes.Length - 1) - 4] = value;
                Value = BitConverter.ToUInt64(allBytes, 0);
            }
        }

        public byte BackChildLower
        {
            get => (byte)((Value >> 16) & 0xFF);
            set
            {
                var allBytes = BitConverter.GetBytes(Value);
                allBytes[(allBytes.Length - 1) - 5] = value;
                Value = BitConverter.ToUInt64(allBytes, 0);
            }
        }

        public byte BackChildMid
        {
            get => (byte)((Value >> 8) & 0xFF);
            set
            {
                var allBytes = BitConverter.GetBytes(Value);
                allBytes[(allBytes.Length - 1) - 6] = value;
                Value = BitConverter.ToUInt64(allBytes, 0);
            }
        }

        public byte BackChildUpper
        {
            get => (byte)(Value & 0xFF);
            set
            {
                var allBytes = BitConverter.GetBytes(Value);
                allBytes[(allBytes.Length - 1) - 7] = value;
                Value = BitConverter.ToUInt64(allBytes, 0);
            }
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
        public short Bsp2dReferenceCount;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public short FirstBsp2dReference_H2;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int FirstBsp2dReference;
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
        public short Plane;

        public ushort FirstEdge;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public SurfaceFlags Flags_H2;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public short MaterialIndex;
        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public short Unknown;

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
        public short StartVertex;
        public short EndVertex;
        public ushort ForwardEdge;
        public ushort ReverseEdge;
        public short LeftSurface;
        public short RightSurface;
    }

    [TagStructure(Size = 0x10, Align = 0x10)]
    public class Vertex : TagStructure
    {
        public RealPoint3d Point;
        public short FirstEdge;
        public short Sink;
    }

    [Flags]
    public enum SurfaceFlags : byte
    {
        None = 0,
        TwoSided = 1 << 0,
        Invisible = 1 << 1,
        Climbable = 1 << 2,
        Invalid = 1 << 3,
        Conveyor = 1 << 4,
        Slip = 1 << 5,
        PlaneNegated = 1 << 6
    }

    [Flags]
    public enum LeafFlags : byte
    {
        None = 0,
        ContainsDoubleSidedSurfaces = 1 << 0
    }
}