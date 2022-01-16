using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Tags;
using System.Reflection;

namespace TagTool.Geometry.BspCollisionGeometry.Utils
{
    public class ResizeCollisionBSP
    {
        public LargeCollisionBspBlock GrowCollisionBsp(CollisionGeometry bsp)
        {
            LargeCollisionBspBlock newbsp = new LargeCollisionBspBlock();
            newbsp.Bsp3dNodes = new TagBlock<LargeBsp3dNode>();
            foreach(var bsp3dnode in bsp.Bsp3dNodes)
            {
                newbsp.Bsp3dNodes.Add(new LargeBsp3dNode
                {
                    BackChild = (int)((bsp3dnode.BackChild & 0x800000) > 0 ? bsp3dnode.BackChild & 0x7FFFFF | 0x80000000 : bsp3dnode.BackChild),
                    FrontChild = (int)((bsp3dnode.FrontChild & 0x800000) > 0 ? bsp3dnode.FrontChild & 0x7FFFFF | 0x80000000 : bsp3dnode.FrontChild),
                    Plane = bsp3dnode.Plane
                });
            }

            //these are unchanged
            newbsp.Bsp3dSupernodes = bsp.Bsp3dSupernodes.DeepClone();
            newbsp.Planes = bsp.Planes.DeepClone();
            newbsp.Leaves = bsp.Leaves.DeepClone();

            newbsp.Bsp2dReferences = new TagBlock<LargeBsp2dReference>();
            foreach (var bsp2dreference in bsp.Bsp2dReferences)
            {
                newbsp.Bsp2dReferences.Add(new LargeBsp2dReference
                {
                    PlaneIndex = (int)((bsp2dreference.PlaneIndex & 0x8000) > 0 ? bsp2dreference.PlaneIndex & 0x7FFF | 0x80000000 : bsp2dreference.PlaneIndex),
                    Bsp2dNodeIndex = (int)((bsp2dreference.Bsp2dNodeIndex & 0x8000) > 0 ? bsp2dreference.Bsp2dNodeIndex & 0x7FFF | 0x80000000 : bsp2dreference.Bsp2dNodeIndex)
                });
            }

            newbsp.Bsp2dNodes = new TagBlock<LargeBsp2dNode>();
            foreach (var bsp2dnode in bsp.Bsp2dNodes)
            {
                newbsp.Bsp2dNodes.Add(new LargeBsp2dNode
                {
                    Plane = bsp2dnode.Plane.DeepClone(),
                    LeftChild = (int)((bsp2dnode.LeftChild & 0x8000) > 0 ? (int)bsp2dnode.LeftChild & 0x7FFF | 0x80000000 : bsp2dnode.LeftChild),
                    RightChild = (int)((bsp2dnode.RightChild & 0x8000) > 0 ? (int)bsp2dnode.RightChild & 0x7FFF | 0x80000000 : bsp2dnode.RightChild),
                });
            }

            newbsp.Surfaces = new TagBlock<LargeSurface>();
            foreach (var surface in bsp.Surfaces)
            {
                newbsp.Surfaces.Add(new LargeSurface
                {
                    Plane = (int)((surface.Plane & 0x8000) > 0 ? (int)surface.Plane & 0x7FFF | 0x80000000 : surface.Plane),
                    FirstEdge = surface.FirstEdge,
                    Material = surface.MaterialIndex,
                    BreakableSurfaceSet = surface.BreakableSurfaceSet,
                    BreakableSurface = surface.BreakableSurfaceIndex,
                    Flags = surface.Flags,
                    BestPlaneCalculationVertex = (sbyte)surface.BestPlaneCalculationVertex
                });
            }

            newbsp.Edges = new TagBlock<LargeEdge>();
            foreach (var edge in bsp.Edges)
            {
                newbsp.Edges.Add(new LargeEdge
                {
                    StartVertex = edge.StartVertex,
                    EndVertex = edge.EndVertex,
                    ForwardEdge = edge.ForwardEdge,
                    ReverseEdge = edge.ReverseEdge,
                    LeftSurface = edge.LeftSurface,
                    RightSurface = edge.RightSurface
                });
            }

            newbsp.Vertices = new TagBlock<LargeVertex>();
            foreach (var vertex in bsp.Vertices)
            {
                newbsp.Vertices.Add(new LargeVertex
                {
                    Point = vertex.Point,
                    FirstEdge = vertex.FirstEdge,
                    Sink = vertex.Sink
                });
            }
            return newbsp;
        }

        public CollisionGeometry ShrinkCollisionBsp(LargeCollisionBspBlock bsp)
        {
            CollisionGeometry newbsp = new CollisionGeometry();
            newbsp.Bsp3dNodes = new TagBlock<Bsp3dNode>();
            foreach (var bsp3dnode in bsp.Bsp3dNodes)
            {
                newbsp.Bsp3dNodes.Add(new Bsp3dNode
                {
                    BackChild = (int)((bsp3dnode.BackChild & 0x80000000) > 0 ? bsp3dnode.BackChild | 0x800000 : bsp3dnode.BackChild),
                    FrontChild = (int)((bsp3dnode.FrontChild & 0x80000000) > 0 ? bsp3dnode.FrontChild | 0x800000 : bsp3dnode.FrontChild),
                    Plane = bsp3dnode.Plane
                });
            }

            //these are unchanged
            newbsp.Bsp3dSupernodes = bsp.Bsp3dSupernodes.DeepClone();
            newbsp.Planes = bsp.Planes.DeepClone();
            newbsp.Leaves = bsp.Leaves.DeepClone();

            newbsp.Bsp2dReferences = new TagBlock<Bsp2dReference>();
            foreach (var bsp2dreference in bsp.Bsp2dReferences)
            {
                newbsp.Bsp2dReferences.Add(new Bsp2dReference
                {
                    PlaneIndex = (short)((bsp2dreference.PlaneIndex & 0x80000000) > 0 ? bsp2dreference.PlaneIndex | 0x8000 : bsp2dreference.PlaneIndex),
                    Bsp2dNodeIndex = (short)((bsp2dreference.Bsp2dNodeIndex & 0x80000000) > 0 ? bsp2dreference.Bsp2dNodeIndex | 0x8000 : bsp2dreference.Bsp2dNodeIndex)
                });
            }

            newbsp.Bsp2dNodes = new TagBlock<Bsp2dNode>();
            foreach (var bsp2dnode in bsp.Bsp2dNodes)
            {
                newbsp.Bsp2dNodes.Add(new Bsp2dNode
                {
                    Plane = bsp2dnode.Plane.DeepClone(),
                    LeftChild = (short)((bsp2dnode.LeftChild & 0x80000000) > 0 ? (int)bsp2dnode.LeftChild | 0x8000 : bsp2dnode.LeftChild),
                    RightChild = (short)((bsp2dnode.RightChild & 0x80000000) > 0 ? (int)bsp2dnode.RightChild | 0x8000 : bsp2dnode.RightChild),
                });
            }

            newbsp.Surfaces = new TagBlock<Surface>();
            foreach (var surface in bsp.Surfaces)
            {
                newbsp.Surfaces.Add(new Surface
                {
                    Plane = (ushort)((surface.Plane & 0x80000000) > 0 ? (int)surface.Plane | 0x8000 : surface.Plane),
                    FirstEdge = (ushort)surface.FirstEdge,
                    MaterialIndex = surface.Material,
                    BreakableSurfaceSet = surface.BreakableSurfaceSet,
                    BreakableSurfaceIndex = surface.BreakableSurface,
                    Flags = surface.Flags,
                    BestPlaneCalculationVertex = (byte)surface.BestPlaneCalculationVertex
                });
            }

            newbsp.Edges = new TagBlock<Edge>();
            foreach (var edge in bsp.Edges)
            {
                newbsp.Edges.Add(new Edge
                {
                    StartVertex = (ushort)edge.StartVertex,
                    EndVertex = (ushort)edge.EndVertex,
                    ForwardEdge = (ushort)edge.ForwardEdge,
                    ReverseEdge = (ushort)edge.ReverseEdge,
                    LeftSurface = (ushort)edge.LeftSurface,
                    RightSurface = (ushort)edge.RightSurface
                });
            }

            newbsp.Vertices = new TagBlock<Vertex>();
            foreach (var vertex in bsp.Vertices)
            {
                newbsp.Vertices.Add(new Vertex
                {
                    Point = vertex.Point,
                    FirstEdge = (ushort)vertex.FirstEdge,
                    Sink = (short)vertex.Sink
                });
            }
            return newbsp;
        }

        public bool collision_bsp_check_counts(LargeCollisionBspBlock Bsp)
        {
            int max_surfaces = 32767;
            int max_edges = 65535;
            int max_vertices = 65535;
            int max_2drefs = 65535;
            int max_2dnodes = 32767;
            int max_3dnodes = 32767;
            int max_planes = 65535;
            int max_leaves = 32767;

            if (Bsp.Surfaces.Count > max_surfaces)
            {
                Console.WriteLine($"###ERROR: Number of surfaces ({Bsp.Surfaces.Count}) exceeded the maximum allowable ({max_surfaces})");
                return false;
            }
            if (Bsp.Vertices.Count > max_vertices)
            {
                Console.WriteLine($"###ERROR: Number of vertices ({Bsp.Vertices.Count}) exceeded the maximum allowable ({max_vertices})");
                return false;
            }
            if (Bsp.Edges.Count > max_edges)
            {
                Console.WriteLine($"###ERROR: Number of edges ({Bsp.Edges.Count}) exceeded the maximum allowable ({max_edges})");
                return false;
            }
            if (Bsp.Bsp2dReferences.Count > max_2drefs)
            {
                Console.WriteLine($"###ERROR: Number of bsp2dreferences ({Bsp.Bsp2dReferences.Count}) exceeded the maximum allowable ({max_2drefs})");
                return false;
            }
            if (Bsp.Bsp2dNodes.Count > max_2dnodes)
            {
                Console.WriteLine($"###ERROR: Number of bsp2dnodes ({Bsp.Bsp2dNodes.Count}) exceeded the maximum allowable ({max_2dnodes})");
                return false;
            }
            if (Bsp.Bsp3dNodes.Count > max_3dnodes)
            {
                Console.WriteLine($"###ERROR: Number of bsp3dnodes ({Bsp.Bsp3dNodes.Count}) exceeded the maximum allowable ({max_3dnodes})");
                return false;
            }
            if (Bsp.Planes.Count > max_planes)
            {
                Console.WriteLine($"###ERROR: Number of planes ({Bsp.Planes.Count}) exceeded the maximum allowable ({max_planes})");
                return false;
            }
            if (Bsp.Leaves.Count > max_leaves)
            {
                Console.WriteLine($"###ERROR: Number of leaves ({Bsp.Leaves.Count}) exceeded the maximum allowable ({max_leaves})");
                return false;
            }
            return true;
        }
        public bool collision_bsp_check_counts(CollisionGeometry Bsp)
        {
            int max_surfaces = 32767;
            int max_edges = 65535;
            int max_vertices = 65535;
            int max_2drefs = 65535;
            int max_2dnodes = 32767;
            int max_3dnodes = 32767;
            int max_planes = 65535;
            int max_leaves = 32767;

            if (Bsp.Surfaces.Count > max_surfaces)
            {
                Console.WriteLine($"###ERROR: Number of surfaces ({Bsp.Surfaces.Count}) exceeded the maximum allowable ({max_surfaces})");
                return false;
            }
            if (Bsp.Vertices.Count > max_vertices)
            {
                Console.WriteLine($"###ERROR: Number of vertices ({Bsp.Vertices.Count}) exceeded the maximum allowable ({max_vertices})");
                return false;
            }
            if (Bsp.Edges.Count > max_edges)
            {
                Console.WriteLine($"###ERROR: Number of edges ({Bsp.Edges.Count}) exceeded the maximum allowable ({max_edges})");
                return false;
            }
            if (Bsp.Bsp2dReferences.Count > max_2drefs)
            {
                Console.WriteLine($"###ERROR: Number of bsp2dreferences ({Bsp.Bsp2dReferences.Count}) exceeded the maximum allowable ({max_2drefs})");
                return false;
            }
            if (Bsp.Bsp2dNodes.Count > max_2dnodes)
            {
                Console.WriteLine($"###ERROR: Number of bsp2dnodes ({Bsp.Bsp2dNodes.Count}) exceeded the maximum allowable ({max_2dnodes})");
                return false;
            }
            if (Bsp.Bsp3dNodes.Count > max_3dnodes)
            {
                Console.WriteLine($"###ERROR: Number of bsp3dnodes ({Bsp.Bsp3dNodes.Count}) exceeded the maximum allowable ({max_3dnodes})");
                return false;
            }
            if (Bsp.Planes.Count > max_planes)
            {
                Console.WriteLine($"###ERROR: Number of planes ({Bsp.Planes.Count}) exceeded the maximum allowable ({max_planes})");
                return false;
            }
            if (Bsp.Leaves.Count > max_leaves)
            {
                Console.WriteLine($"###ERROR: Number of leaves ({Bsp.Leaves.Count}) exceeded the maximum allowable ({max_leaves})");
                return false;
            }
            return true;
        }

        public class TagStructComparer : IComparer<object>
        {
            public int Compare(object x, object y)
            {
                TypeInfo t = x.GetType().GetTypeInfo();
                IEnumerable<FieldInfo> fList = t.GetFields();
                foreach (FieldInfo f in fList)
                {
                    if (!f.GetValue(x).Equals(f.GetValue(y)))
                        return 0;
                }
                return 1;
            }
        }
        public bool CompareCollisionBsp(CollisionGeometry bsp_a, CollisionGeometry bsp_b)
        {
            TagStructComparer comparer = new TagStructComparer();
            bool result = true;
            for (var i = 0; i < bsp_a.Bsp3dNodes.Count; i++)
                if (comparer.Compare(bsp_a.Bsp3dNodes[i], bsp_b.Bsp3dNodes[i]) == 0)
                {
                    result = false;
                    break;
                }
            for (var i = 0; i < bsp_a.Bsp2dNodes.Count; i++)
                if (comparer.Compare(bsp_a.Bsp2dNodes[i], bsp_b.Bsp2dNodes[i]) == 0)
                {
                    result = false;
                    break;
                }
            for (var i = 0; i < bsp_a.Planes.Count; i++)
                if (comparer.Compare(bsp_a.Planes[i], bsp_b.Planes[i]) == 0)
                {
                    result = false;
                    break;
                }
            for (var i = 0; i < bsp_a.Surfaces.Count; i++)
                if (comparer.Compare(bsp_a.Surfaces[i], bsp_b.Surfaces[i]) == 0)
                {
                    result = false;
                    break;
                }
            for (var i = 0; i < bsp_a.Vertices.Count; i++)
                if (comparer.Compare(bsp_a.Vertices[i], bsp_b.Vertices[i]) == 0)
                {
                    result = false;
                    break;
                }
            for (var i = 0; i < bsp_a.Leaves.Count; i++)
                if (comparer.Compare(bsp_a.Leaves[i], bsp_b.Leaves[i]) == 0)
                {
                    result = false;
                    break;
                }
            for (var i = 0; i < bsp_a.Bsp2dReferences.Count; i++)
                if (comparer.Compare(bsp_a.Bsp2dReferences[i], bsp_b.Bsp2dReferences[i]) == 0)
                {
                    result = false;
                    break;
                }
            for (var i = 0; i < bsp_a.Edges.Count; i++)
                if (comparer.Compare(bsp_a.Edges[i], bsp_b.Edges[i]) == 0)
                {
                    result = false;
                    break;
                }

            return result;
        }
    }
}
