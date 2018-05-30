using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System;
using System.Collections.Generic;

namespace TagTool.Geometry
{
    [TagStructure(Size = 0x44, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0x5C, MinVersion = CacheVersion.Halo3Retail)]
    public class CollisionGeometry
    {
        public List<Bsp3dNode> Bsp3dNodes;
        public List<Plane> Planes;
        public List<Leaf> Leaves;
        public List<Bsp2dReference> Bsp2dReferences;
        public List<Bsp2dNode> Bsp2dNodes;
        public List<Surface> Surfaces;
        public List<Edge> Edges;
        public List<Vertex> Vertices;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float Unknown;

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

        [TagStructure(Size = 0x8, Align = 0x8)]
        public class Bsp3dNode
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

        [TagStructure(Size = 0x10, Align = 0x10)]
        public class Plane
        {
            public RealPlane3d Value;
        }

        [Flags]
        public enum LeafFlags : byte
        {
            None = 0,
            ContainsDoubleSidedSurfaces = 1 << 0
        }

        [TagStructure(Size = 0x4, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x8, MinVersion = CacheVersion.Halo3Retail)]
        public class Leaf
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
        public class Bsp2dReference
        {
            public short PlaneIndex;
            public short Bsp2dNodeIndex;
        }

        [TagStructure(Size = 0x10, Align = 0x10)]
        public class Bsp2dNode
        {
            public RealPlane2d Plane;
            public short LeftChild;
            public short RightChild;
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

        [TagStructure(Size = 0x8, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0xC, MinVersion = CacheVersion.Halo3Retail)]
        public class Surface
        {
            public short Plane;

            public short FirstEdge;

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
        public class Edge
        {
            public short StartVertex;
            public short EndVertex;
            public short ForwardEdge;
            public short ReverseEdge;
            public short LeftSurface;
            public short RightSurface;
        }

        [TagStructure(Size = 0x10, Align = 0x10)]
        public class Vertex
        {
            public RealPoint3d Point;
            public short FirstEdge;
            public short Sink;
        }
    }
}