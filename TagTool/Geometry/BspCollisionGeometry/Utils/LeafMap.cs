using System;
using System.Collections.Generic;
using TagTool.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Geometry.BspCollisionGeometry.Utils
{
    class LeafMap
    {
        private LargeCollisionBspBlock Bsp { get; set; }
        class leaf
        {
            public int immediate_parent_3dnode;
            public List<int> plane_designators;
            public float[,] polygon_areas = new float[2,3];
            public float[] polygon_area_sums = new float[2];
            public short[,] polygon_counts = new short[2,3];
            public List<polygon> polygons; //cut with planes that do not include margins
            public List<polygon> polygons2; //cut with planes that include margins
        };
        class polygon
        {
            public int surface_index;
            public int plane_index;
            public int polygon_type;
            public float polygon_area;
            public List<RealPoint3d> points;
        };
        bool setup_leafy_bsp()
        {
            List<leaf> leaves = new List<leaf>(Bsp.Bsp3dNodes.Count + 1);

            if (!populate_plane_designators(ref leaves, new List<int>(), 0, -1))
                return false;

            int surface_index = 0;
            for (int use_plane_margins = 0; use_plane_margins < 2; use_plane_margins++)
            {
                while (true)
                {
                    LargeSurface surface_block = Bsp.Surfaces[surface_index];
                    if (!surface_block.Flags.HasFlag(SurfaceFlags.TwoSided))
                    {
                        List<RealPoint3d> pointlist = surface_collect_vertices(surface_index);
                        if (!surface_clip_to_leaves(leaves, 0, use_plane_margins, 0, pointlist, surface_index, surface_block.Plane))
                            return false;
                    }
                    if (++surface_index >= Bsp.Surfaces.Count)
                        break;
                }
            }

            for (int leaf_index = 0; leaf_index < leaves.Count; leaf_index++)
            {
                leaf current_leaf = leaves[leaf_index];
                for (int use_plane_margins = 0; use_plane_margins < 2; use_plane_margins++)
                {
                    int polygon_count = (use_plane_margins == 0) ? current_leaf.polygons.Count : current_leaf.polygons2.Count;
                    if (polygon_count > 0)
                    {
                        List<polygon> polygon_block = (use_plane_margins == 0) ? current_leaf.polygons : current_leaf.polygons2;
                        for (int polygon_index = 0; polygon_index < polygon_block.Count; polygon_index++)
                        {
                            polygon current_polygon = polygon_block[polygon_index];
                            int plane_index = current_polygon.plane_index;
                            bool plane_mirror_check = plane_index < 0;
                            RealPlane3d plane_block = Bsp.Planes[plane_index & 0x7FFFFFFF].Value;
                            current_polygon.polygon_area = 0.0f; //polygon_get_area();
                            current_leaf.polygon_areas[use_plane_margins, current_polygon.polygon_type] += current_polygon.polygon_area;
                            ++current_leaf.polygon_counts[use_plane_margins, current_polygon.polygon_type];
                        }
                    }
                    float polygon_type_areas_sum = current_leaf.polygon_areas[use_plane_margins, 0] + current_leaf.polygon_areas[use_plane_margins, 1] + current_leaf.polygon_areas[use_plane_margins, 2];
                    if (polygon_type_areas_sum <= 0.0)
                        current_leaf.polygon_area_sums[use_plane_margins] = 0.0f;
                    else
                        current_leaf.polygon_area_sums[use_plane_margins] = current_leaf.polygon_areas[use_plane_margins, 1] / polygon_type_areas_sum; //add (leaf_index < leafy_bsp->bsp_leaf_count)
                }
            }

            return true;
        }

        bool populate_plane_designators(ref List<leaf> leaves, List<int> plane_designators, int bsp3dnode_index, int parent_bsp3dnode_index)
        {
            if(bsp3dnode_index < 0)
            {
                leaf current_leaf = leaves[bsp3dnode_index & 0x7FFFFFFF];
                current_leaf.immediate_parent_3dnode = parent_bsp3dnode_index;
                current_leaf.plane_designators = plane_designators.DeepClone();
                return true;
            }
            LargeBsp3dNode current_node = Bsp.Bsp3dNodes[bsp3dnode_index];
            for(var i = 0; i < 2; i++)
            {
                int plane_index = current_node.Plane;
                int masked_plane_index = (int)(i == 1 ? plane_index & 0x7FFFFFFF : plane_index | 0x80000000);
                int masked_node_index = (int)(i == 1 ? bsp3dnode_index | 0x80000000 : bsp3dnode_index & 0x7FFFFFFF);
                int node_child = i == 1 ? current_node.BackChild : current_node.FrontChild;
                plane_designators.Add(masked_plane_index);
                if (!populate_plane_designators(ref leaves, plane_designators, node_child, masked_node_index))
                    break;
                plane_designators.RemoveAt(plane_designators.Count - 1);
            }
            return true;
        }

        bool surface_clip_to_leaves(List<leaf> leaves, int bsp3dnode_index, int use_plane_margins, int polygon_type, List<RealPoint3d> points, int surface_index, int plane_index)
        {
            if (bsp3dnode_index < 0)
            {
                leaf current_leaf = leaves[bsp3dnode_index & 0x7FFFFFFF];
                polygon new_polygon = new polygon 
                { 
                    surface_index = surface_index,
                    plane_index = plane_index,
                    polygon_type = polygon_type,
                    points = points
                };
                if(use_plane_margins == 0)
                    current_leaf.polygons.Add(new_polygon);
                else if (use_plane_margins == 1)
                    current_leaf.polygons2.Add(new_polygon);
                return true;
            }
            LargeBsp3dNode current_node = Bsp.Bsp3dNodes[bsp3dnode_index];
            if(current_node.Plane == (plane_index & 0x7FFFFFFF))
            {
                int node_side = (plane_index < 0) ? 1 : 0;
                int other_node_side = (plane_index >= 0) ? 1 : 0;
                //set polygon type depending on whether the surface is on the plane or the flipped plane
                if (surface_clip_to_leaves(leaves, current_node.FrontChild, use_plane_margins, node_side + 1, points, surface_index, plane_index))
                {
                    return surface_clip_to_leaves(leaves, current_node.BackChild, use_plane_margins, other_node_side + 1, points, surface_index, plane_index);
                }
                return false;
            }
            RealPlane3d node_plane = Bsp.Planes[current_node.Plane].Value;
            float margin = use_plane_margins == 0 ? 0.0f : 0.00024414062f;
            RealPlane3d front_plane = new RealPlane3d(node_plane.I, node_plane.J, node_plane.K, node_plane.D + margin);
            List<RealPoint3d> front_points = plane_cut_polygon(points, front_plane);
            if(front_points.Count > 0 && !surface_clip_to_leaves(leaves, current_node.FrontChild, use_plane_margins, polygon_type, front_points, surface_index, plane_index))
            {
                return false;
            }
            RealPlane3d back_plane = new RealPlane3d(node_plane.I, node_plane.J, node_plane.K, node_plane.D - margin);
            List<RealPoint3d> back_points = plane_cut_polygon(points, back_plane);
            if (back_points.Count <= 0)
                return true;
            return surface_clip_to_leaves(leaves, current_node.BackChild, use_plane_margins, polygon_type, back_points, surface_index, plane_index);
        }

        public List<RealPoint3d> plane_cut_polygon(List<RealPoint3d> points, RealPlane3d plane)
        {
            return points;
        }

        List<RealPoint3d> surface_collect_vertices(int surface_index)
        {
            List<RealPoint3d> pointlist = new List<RealPoint3d>();
            LargeSurface surface_block = Bsp.Surfaces[surface_index];

            int surface_edge_index = surface_block.FirstEdge;
            //collect vertices on the plane
            while (true)
            {
                LargeEdge surface_edge_block = Bsp.Edges[surface_edge_index];
                if (surface_edge_block.RightSurface == surface_index)
                {
                    pointlist.Add(Bsp.Vertices[surface_edge_block.EndVertex].Point);
                    surface_edge_index = surface_edge_block.ReverseEdge;
                }
                else
                {
                    pointlist.Add(Bsp.Vertices[surface_edge_block.StartVertex].Point);
                    surface_edge_index = surface_edge_block.ForwardEdge;
                }
                //break the loop if we have finished circulating the surface
                if (surface_edge_index == surface_block.FirstEdge)
                    break;
            }
            return pointlist;
        }
    }
}
    
