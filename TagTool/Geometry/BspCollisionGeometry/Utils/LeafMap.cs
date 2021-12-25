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
        public class leafy_bsp
        {
            public int bsp_leaf_count;
            public List<leaf> leaves;
            public List<leaf_map_leaf> leaf_map_leaves;
            public List<leaf_map_portal> leaf_map_portals;
        }
        public class leaf
        {
            public int immediate_parent_3dnode;
            public List<int> plane_designators;
            public float[,] polygon_areas = new float[2,3];
            public float[] polygon_area_sums = new float[2];
            public short[,] polygon_counts = new short[2,3];
            public List<polygon> polygons; //cut with planes that do not include margins
            public List<polygon> polygons2; //cut with planes that include margins
        };
        public class polygon
        {
            public int surface_index;
            public int plane_index;
            public int polygon_type;
            public float polygon_area;
            public List<RealPoint3d> points;
        };

        public class leaf_map_portal
        {
            public int plane_index;
            public int back_leaf_index;
            public int front_leaf_index;
            public List<RealPoint3d> vertices;
        }
        public class leaf_map_leaf
        {
            public List<leaf_face> faces;
            public List<int> portal_indices;
        }
        public class leaf_face
        {
            public int bsp3dnode_index;
            public List<int> vertices;
        }
        public bool setup_leafy_bsp(ref leafy_bsp leafybsp)
        {
            leafybsp.bsp_leaf_count = Bsp.Leaves.Count;
            leafybsp.leaves = new List<leaf>(Bsp.Bsp3dNodes.Count + 1);

            if (!populate_plane_designators(ref leafybsp, new List<int>(), 0, -1))
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
                        if (!surface_clip_to_leaves(ref leafybsp, 0, use_plane_margins, 0, pointlist, surface_index, surface_block.Plane))
                            return false;
                    }
                    if (++surface_index >= Bsp.Surfaces.Count)
                        break;
                }
            }

            for (int leaf_index = 0; leaf_index < leafybsp.leaves.Count; leaf_index++)
            {
                leaf current_leaf = leafybsp.leaves[leaf_index];
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
                            RealPlane3d plane_block = Bsp.Planes[plane_index & 0x7FFFFFFF].Value;
                            List<RealPoint2d> polygon_points = polygon_project_on_plane(current_polygon.points, plane_block, plane_index);
                            current_polygon.polygon_area = polygon_get_area(polygon_points, plane_block);
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

        public bool setup_leaf_map(ref leafy_bsp leafybsp)
        {
            if (true) //init_leaf_map
            {
                foreach (var leaf_portal in leafybsp.leaf_map_portals)
                {
                    List<RealPoint2d> leaf_portal_points = polygon_project_on_plane(leaf_portal.vertices, Bsp.Planes[leaf_portal.plane_index].Value, leaf_portal.plane_index);
                    float leaf_portal_area = polygon_get_area(leaf_portal_points, Bsp.Planes[leaf_portal.plane_index].Value);
                    float back_child_area = 0.0f;
                    float front_child_area = 0.0f;
                    for (int i = 0; i < 2; i++)
                    {
                        bool is_back_child = i == 0;
                        int portal_child = is_back_child ? leaf_portal.back_leaf_index : leaf_portal.front_leaf_index;
                        leaf child_leaf = leafybsp.leaves[portal_child];
                        if (child_leaf.polygons2.Count > 0)
                        {
                            foreach (var polygon in child_leaf.polygons2)
                            {
                                if (polygon.polygon_type > 0 && (polygon.plane_index & 0x7FFFFFFF) == leaf_portal.plane_index)
                                {
                                    List<RealPoint2d> polygon_points = polygon_project_on_plane(polygon.points, Bsp.Planes[leaf_portal.plane_index].Value, leaf_portal.plane_index);
                                    List<RealPoint2d> portal_sliced_polygon = portal_slice_polygon(polygon_points, leaf_portal_points);
                                    float sliced_polygon_area = polygon_get_area(portal_sliced_polygon, Bsp.Planes[leaf_portal.plane_index].Value);
                                    //greater than count and polygon type 2 or less than count and polygon type 1
                                    if (portal_child > leafybsp.bsp_leaf_count == (polygon.polygon_type == 2))
                                    {
                                        if (is_back_child)
                                            back_child_area += sliced_polygon_area;
                                        else
                                            front_child_area += sliced_polygon_area;
                                    }
                                    else
                                    {
                                        if (is_back_child)
                                            back_child_area -= sliced_polygon_area;
                                        else
                                            front_child_area -= sliced_polygon_area;
                                    }
                                }
                            }
                        }
                    }
                    if (leaf_portal.back_leaf_index < leafybsp.bsp_leaf_count == leaf_portal.front_leaf_index < leafybsp.bsp_leaf_count &&
                        back_child_area < 0.9f * leaf_portal_area && front_child_area < 0.9f * leaf_portal_area)
                        continue; //TODO: call leaf map portal function here 
                    else if (back_child_area > 0.1f * leaf_portal_area && front_child_area > 0.1f * leaf_portal_area)
                        continue; //TODO: call leaf map portal function here 
                }
            }
            else
                return false;
            return true;
        }

        public bool init_leaf_map(ref leafy_bsp leafybsp)
        {
            leafybsp.leaf_map_leaves = new List<leaf_map_leaf>(Bsp.Bsp3dNodes.Count + 1);
            if(Bsp.Bsp3dNodes.Count > 0)
            {

            }
            return true;
        }
        public List<RealPoint2d> portal_slice_polygon(List<RealPoint2d> polygon_points, List<RealPoint2d> leaf_portal_points)
        {
            LargeCollisionBSPBuilder BSPclass = new LargeCollisionBSPBuilder();
            //generate a 2d plane from every consecutive pair of points in the portal and use it to cut away vertices from the polygon
            for(int portal_point_index = 0; portal_point_index < leaf_portal_points.Count; portal_point_index++)
            {
                int last_portal_point_index = portal_point_index - 1;
                //for the first set wrap around to the end of the poly for the previous point
                if (portal_point_index == 0)
                    last_portal_point_index = leaf_portal_points.Count - 1;
                RealPlane2d plane2d = BSPclass.generate_bsp2d_plane_parameters(leaf_portal_points[portal_point_index], leaf_portal_points[last_portal_point_index]).Plane;
                polygon_points = plane_cut_polygon_2d(plane2d, polygon_points);
            }
            return polygon_points;
        }

        public List<RealPoint2d> plane_cut_polygon_2d(RealPlane2d plane2d, List<RealPoint2d> polygon_points)
        {
            List<RealPoint2d> output_points = new List<RealPoint2d>();
            RealPoint2d final_point = polygon_points[polygon_points.Count - 1];
            float d0 = final_point.X * plane2d.I + final_point.Y * plane2d.J - plane2d.D;
            for (var vertex_index = 0; vertex_index < polygon_points.Count; vertex_index++)
            {
                RealPoint2d current_point = polygon_points[vertex_index];
                float d1 = current_point.X * plane2d.I + current_point.Y * plane2d.J - plane2d.D;
                //are the current and final vertex on the same side of the plane?
                if (d1 < 0 != d0 < 0)
                {
                    
                }
                if (d1 >= 0)
                {
                    output_points.Add(current_point);
                }
            }
            return output_points;
        }

        public bool populate_plane_designators(ref leafy_bsp leafybsp, List<int> plane_designators, int bsp3dnode_index, int parent_bsp3dnode_index)
        {
            if(bsp3dnode_index < 0)
            {
                leaf current_leaf = leafybsp.leaves[bsp3dnode_index & 0x7FFFFFFF];
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
                if (!populate_plane_designators(ref leafybsp, plane_designators, node_child, masked_node_index))
                    break;
                plane_designators.RemoveAt(plane_designators.Count - 1);
            }
            return true;
        }

        public bool surface_clip_to_leaves(ref leafy_bsp leafybsp, int bsp3dnode_index, int use_plane_margins, int polygon_type, List<RealPoint3d> points, int surface_index, int plane_index)
        {
            if (bsp3dnode_index < 0)
            {
                leaf current_leaf = leafybsp.leaves[bsp3dnode_index & 0x7FFFFFFF];
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
                if (surface_clip_to_leaves(ref leafybsp, current_node.FrontChild, use_plane_margins, node_side + 1, points, surface_index, plane_index))
                {
                    return surface_clip_to_leaves(ref leafybsp, current_node.BackChild, use_plane_margins, other_node_side + 1, points, surface_index, plane_index);
                }
                return false;
            }
            RealPlane3d node_plane = Bsp.Planes[current_node.Plane].Value;
            float margin = use_plane_margins == 0 ? 0.0f : 0.00024414062f;
            RealPlane3d front_plane = new RealPlane3d(node_plane.I, node_plane.J, node_plane.K, node_plane.D + margin);
            List<RealPoint3d> front_points = plane_cut_polygon(points, front_plane);
            if(front_points.Count > 0 && !surface_clip_to_leaves(ref leafybsp, current_node.FrontChild, use_plane_margins, polygon_type, front_points, surface_index, plane_index))
            {
                return false;
            }
            RealPlane3d back_plane = new RealPlane3d(node_plane.I, node_plane.J, node_plane.K, node_plane.D - margin);
            List<RealPoint3d> back_points = plane_cut_polygon(points, back_plane);
            if (back_points.Count <= 0)
                return true;
            return surface_clip_to_leaves(ref leafybsp, current_node.BackChild, use_plane_margins, polygon_type, back_points, surface_index, plane_index);
        }

        public List<RealPoint3d> plane_cut_polygon(List<RealPoint3d> points, RealPlane3d plane)
        {
            List<RealPoint3d> output_points = new List<RealPoint3d>();
            RealPoint3d final_point = points[points.Count - 1];
            float d0 = final_point.X * plane.I + final_point.Y * plane.J + final_point.Z + plane.K - plane.D;
            for(var vertex_index = 0; vertex_index < points.Count; vertex_index++)
            {
                RealPoint3d current_point = points[vertex_index];
                float d1 = current_point.X * plane.I + current_point.Y * plane.J + current_point.Z + plane.K - plane.D;
                //are the current and final vertex on the same side of the plane?
                if(d1 < 0 != d0 < 0)
                {
                    output_points.Add(vertex_plane_transform(final_point, current_point, d0, d1));
                }
                if(d1 >= 0)
                {
                    output_points.Add(current_point);
                }
            }
            return output_points;
        }

        public RealPoint3d vertex_plane_transform(RealPoint3d p0, RealPoint3d p1, float d0, float d1)
        {
            float dratio = d0 / (d0 - d1);
            RealPoint3d result = new RealPoint3d
            {
                X = (p1.X - p0.X) * dratio + p0.X,
                Y = (p1.Y - p0.Y) * dratio + p0.Y,
                Z = (p1.Z - p0.Z) * dratio + p0.Z,
            };
            return result;
        }

        public float polygon_get_area(List<RealPoint2d> projected_points, RealPlane3d plane)
        {
            float area = 0.0f;
            for (var i = 0; i < projected_points.Count - 2; i++)
            {
                RealPoint2d v21 = projected_points[i + 1] - projected_points[0];
                RealPoint2d v31 = projected_points[i + 2] - projected_points[0];

                //cross product of two vectors times 0.5
                area = area + (v31.Y * v21.X - v31.X * v21.Y) * 0.5f;
            }

            LargeCollisionBSPBuilder BSPclass = new LargeCollisionBSPBuilder();
            int plane_projection_axis = BSPclass.plane_get_projection_coefficient(new Plane { Value = plane });
            float plane_coefficient = 0.0f;
            switch (plane_projection_axis)
            {
                case 0: //x axis
                    plane_coefficient = plane.I;
                    break;
                case 1: //y axis
                    plane_coefficient = plane.J;
                    break;
                case 2: //z axis
                    plane_coefficient = plane.K;
                    break;
            }

            //divide area by absolute value of plane projection coefficient
            return area / Math.Abs(plane_coefficient);
        }

        public List<RealPoint2d> polygon_project_on_plane(List<RealPoint3d> points, RealPlane3d plane, int plane_index)
        {
            //initialize BSP builder class so we can borrow some already implemented functions
            LargeCollisionBSPBuilder BSPclass = new LargeCollisionBSPBuilder();

            int plane_projection_axis = BSPclass.plane_get_projection_coefficient(new Plane { Value = plane });
            bool plane_projection_positive = BSPclass.plane_get_projection_sign(new Plane { Value = plane }, plane_projection_axis);

            int plane_mirror_check = plane_projection_positive != (plane_index < 0) ? 1 : 0;
            List<RealPoint2d> projected_points = new List<RealPoint2d>();
            for (var i = 0; i < points.Count; i++)
            {
                RealPoint2d projected_coords = BSPclass.vertex_get_projection_relevant_coords(points[i], plane_projection_axis, plane_mirror_check);
                projected_points.Add(projected_coords);
            }
            return projected_points;
        }

        public List<RealPoint3d> surface_collect_vertices(int surface_index)
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
    
