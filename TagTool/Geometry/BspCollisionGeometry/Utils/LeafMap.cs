using System;
using System.Collections.Generic;
using TagTool.Common;
using TagTool.Commands.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Geometry.BspCollisionGeometry.Utils
{
    class LeafMap
    {
        private LargeCollisionBspBlock Bsp { get; set; }
        private LargeCollisionBSPBuilder Bsp_Builder { get; set; }
        private int[] leaf_map_node_stack;
        private int leaf_map_node_stack_count;
        public class leafy_bsp
        {
            public int bsp_leaf_count;
            public List<leaf> leaves;
            public List<leaf_map_leaf> leaf_map_leaves = new List<leaf_map_leaf>();
            public List<leaf_map_portal> leaf_map_portals = new List<leaf_map_portal>();
        }
        public class leaf
        {
            public int immediate_parent_3dnode;
            public List<int> plane_designators;
            public float[,] polygon_areas_by_type = new float[2,3];
            public float[] polygon_area_sums = new float[2];
            public short[,] polygon_counts = new short[2,3];
            public List<polygon> polygons = new List<polygon>(); //cut with planes that do not include margins
            public List<polygon> polygons2 = new List<polygon>(); //cut with planes that include margins
        };
        public class polygon
        {
            public int surface_index;
            public int plane_index;
            public int polygon_type;
            public float polygon_area;
            public List<RealPoint3d> points = new List<RealPoint3d>();
        };
        public class leaf_map_portal
        {
            public int plane_index;
            public int back_leaf_index;
            public int front_leaf_index;
            public List<RealPoint3d> vertices = new List<RealPoint3d>();
        }
        public class leaf_map_leaf
        {
            public List<leaf_face> faces = new List<leaf_face>();
            public List<int> portal_indices = new List<int>();
        }
        public class leaf_face
        {
            public int bsp3dnode_index;
            public List<RealPoint2d> vertices = new List<RealPoint2d>();
        }

        public bool munge_collision_bsp(LargeCollisionBSPBuilder bsp_builder)
        {
            Bsp_Builder = bsp_builder;
            Bsp = bsp_builder.Bsp;
            leafy_bsp leafybsp = new leafy_bsp();
            if (!setup_floating_leaves(ref leafybsp) ||
                !setup_leafy_bsp(ref leafybsp) ||
                !setup_leaf_map(ref leafybsp))
                return false;

            if(leafybsp.leaf_map_leaves.Count > 0)
            {
                for(var leaf_index = 0; leaf_index < leafybsp.leaf_map_leaves.Count; leaf_index++)
                {
                    leaf bsp_leaf = leafybsp.leaves[leaf_index];
                    if (leaf_portal_area_mismatch_count(leafybsp, leaf_index) > 0)
                    {                       
                        if(bsp_leaf.polygon_counts[1,0] > 0) //polygons2 type 0
                        {
                            new TagToolWarning("Fixing phantom leaf!");
                            int new_leaf_index = -1;
                            if (reconstruct_bsp_leaf(bsp_leaf, ref new_leaf_index))
                            {
                                //assign new leaf index to bsp3d tree
                                if (bsp_leaf.immediate_parent_3dnode < 0)
                                    Bsp.Bsp3dNodes[bsp_leaf.immediate_parent_3dnode & 0x7FFFFFFF].FrontChild = new_leaf_index;
                                else
                                    Bsp.Bsp3dNodes[bsp_leaf.immediate_parent_3dnode & 0x7FFFFFFF].BackChild = new_leaf_index;
                                //erase existing leaf
                                if(leaf_index < leafybsp.bsp_leaf_count)
                                    Bsp.Leaves[leaf_index] = new Leaf
                                    {
                                        Flags = 0,
                                        Bsp2dReferenceCount = 0,
                                        FirstBsp2dReference = uint.MaxValue
                                    };
                            }
                        }
                        else if (leaf_index >= leafybsp.bsp_leaf_count)
                        {
                            new TagToolWarning("Solid leaf didn't have any floating surfaces!");
                        }
                        else
                        {
                            //wipe out leaf and leaf reference in bsp tree
                            Bsp.Leaves[leaf_index] = new Leaf
                            {
                                Flags = 0,
                                Bsp2dReferenceCount = 0,
                                FirstBsp2dReference = uint.MaxValue
                            };
                            if (bsp_leaf.immediate_parent_3dnode < 0)
                                Bsp.Bsp3dNodes[bsp_leaf.immediate_parent_3dnode & 0x7FFFFFFF].FrontChild = -1;
                            else
                                Bsp.Bsp3dNodes[bsp_leaf.immediate_parent_3dnode & 0x7FFFFFFF].BackChild = -1;
                        }
                    }
                }
            }
            return true;
        }

        public bool reconstruct_bsp_leaf(leaf bsp_leaf, ref int new_leaf_index)
        {
            int build_type = 0;

            if (bsp_leaf.polygon_counts[1, 0] <= 0)
            {
                build_type = 2;
                if (bsp_leaf.polygon_counts[1, 2] < bsp_leaf.polygon_counts[1, 1])
                    build_type = 3;
            }
            else
                build_type = 1;

            switch (build_type)
            {
                case 1:
                    //find unused polygon with largest area, use it to split
                    float max_area = float.MinValue;
                    int plane_index = -1;
                    foreach(var polygon in bsp_leaf.polygons2)
                    {
                        if(polygon.polygon_type == 0 && polygon.polygon_area > max_area)
                        {
                            max_area = polygon.polygon_area;
                            plane_index = polygon.plane_index & 0x7FFFFFFF;
                        }
                    }                  
                    LargeBsp3dNode new_node = new LargeBsp3dNode { Plane = plane_index };
                    Bsp.Bsp3dNodes.Add(new_node);
                    new_leaf_index = Bsp.Bsp3dNodes.Count - 1;
                    for (int i = 0; i < 2; i++)
                    {
                        int child_index = -1;
                        leaf new_leaf = new leaf();
                        if (!bsp3d_split_manual(bsp_leaf, i, plane_index, ref new_leaf) ||
                            !reconstruct_bsp_leaf(new_leaf, ref child_index))
                            return false;
                        if (i == 0)
                            Bsp.Bsp3dNodes[new_leaf_index].BackChild = child_index;
                        else
                            Bsp.Bsp3dNodes[new_leaf_index].FrontChild = child_index;
                    }                
                    break;
                case 2:
                    //build surface array from polygons, build leaves in the usual fashion
                    LargeCollisionBSPBuilder.surface_array_definition surface_array = new LargeCollisionBSPBuilder.surface_array_definition
                    {
                        surface_array = new List<int>()
                    };
                    foreach(var polygon in bsp_leaf.polygons2)
                    {
                        if(polygon.polygon_type == 2)
                        {
                            surface_array.surface_array.Add(polygon.surface_index);
                            surface_array.used_count++;
                        }
                    }
                    Bsp_Builder.build_leaves(ref surface_array, ref new_leaf_index);
                    break;
                case 3:
                    //nothing to do
                    new_leaf_index = -1;
                    break;
            }
            return true;
        }

        public bool bsp3d_split_manual(leaf bsp_leaf, int split_side, int plane_index, ref leaf new_leaf)
        {
            for(var use_plane_margins = 0; use_plane_margins < 2; use_plane_margins++)
            {
                RealPlane3d plane_block = Bsp.Planes[plane_index].Value;
                if (use_plane_margins == 1)
                    plane_block.D += split_side == 1 ? -0.00024414062f : 0.00024414062f;
                List <polygon> polygonlist = use_plane_margins == 0 ? bsp_leaf.polygons : bsp_leaf.polygons2;
                foreach(var polygon in polygonlist)
                {
                    List<RealPoint3d> polygon_points = new List<RealPoint3d>();
                    int polygon_type = 0;
                    if((polygon.plane_index & 0x7FFFFFFF) == plane_index)
                    {
                        polygon_points = polygon.points;
                        polygon_type = ((polygon.plane_index < 0) != (split_side == 1)) ? 2 : 1;
                    }
                    else
                    {
                        polygon_points = plane_cut_polygon(polygon.points, plane_block, split_side == 1);
                        polygon_type = polygon.polygon_type;
                    }
                    if (polygon_points.Count <= 0)
                        continue;

                    RealPlane3d polygon_plane = Bsp.Planes[polygon.plane_index & 0x7FFFFFFF].Value;
                    List<RealPoint2d> polygon_points2d = polygon_project_on_plane(polygon_points, polygon_plane, polygon.plane_index);
                    float polygon_area = polygon_get_area(polygon_points2d, polygon_plane);

                    polygon new_polygon = new polygon
                    {
                        polygon_type = polygon_type,
                        points = polygon_points,
                        polygon_area = polygon_area,
                        surface_index = polygon.surface_index,
                        plane_index = polygon.plane_index
                    };

                    if (use_plane_margins == 0)
                    {
                        new_leaf.polygons.Add(new_polygon);
                        new_leaf.polygon_counts[0, polygon_type]++;
                        new_leaf.polygon_areas_by_type[0, polygon_type] += polygon_area;
                        new_leaf.polygon_area_sums[0] += polygon_area;
                    }
                    else
                    {
                        new_leaf.polygons2.Add(new_polygon);
                        new_leaf.polygon_counts[1, polygon_type]++;
                        new_leaf.polygon_areas_by_type[1, polygon_type] += polygon_area;
                        new_leaf.polygon_area_sums[1] += polygon_area;
                    }                        
                }
            }
            return true;
        }

        public int leaf_portal_area_mismatch_count(leafy_bsp leafybsp, int leaf_index)
        {
            leaf_map_leaf map_leaf = leafybsp.leaf_map_leaves[leaf_index];
            leaf bsp_leaf = leafybsp.leaves[leaf_index];
            int mismatching_leaf_portal_count = 0;
            if(map_leaf.portal_indices.Count > 0)
            {
                foreach (int portal_index in map_leaf.portal_indices)
                {
                    //portal must not be flagged
                    if(portal_index >= 0)
                    {
                        leaf_map_portal leaf_portal = leafybsp.leaf_map_portals[portal_index & 0x7FFFFFFF];
                        int portal_other_leaf_index = leaf_portal.back_leaf_index == leaf_index ? leaf_portal.front_leaf_index : leaf_portal.back_leaf_index;
                        leaf other_leaf = leafybsp.leaves[portal_other_leaf_index];

                        if (other_leaf.polygon_area_sums[0] > bsp_leaf.polygon_area_sums[0] && //polygons1 area sum mismatch
                            (bsp_leaf.polygon_counts[1, 0] > 0 || //polygons2 type 0 count > 0
                            portal_other_leaf_index >= leafybsp.bsp_leaf_count && //other leaf was previously -1
                            leaf_portal_area_mismatch_count(leafybsp, portal_other_leaf_index) == 0)) //
                        {
                            mismatching_leaf_portal_count++;
                        }
                    }                  
                }
            }
            return mismatching_leaf_portal_count;
        }

        //a floating leaf is a leaf that was previously -1 and now has a leaf index > the leaf count
        public bool setup_floating_leaves(ref leafy_bsp leafybsp)
        {
            int leaf_count = Bsp.Leaves.Count;
            for (var i = 0; i < Bsp.Bsp3dNodes.Count; i++)
            {
                if (Bsp.Bsp3dNodes[i].FrontChild == -1)
                    Bsp.Bsp3dNodes[i].FrontChild = (int)(leaf_count++ | 0x80000000);
                if (Bsp.Bsp3dNodes[i].BackChild == -1)
                    Bsp.Bsp3dNodes[i].BackChild = (int)(leaf_count++ | 0x80000000);
            }
            return true;
        }
        public bool setup_leafy_bsp(ref leafy_bsp leafybsp)
        {
            leafybsp.bsp_leaf_count = Bsp.Leaves.Count;
            leafybsp.leaves = new List<leaf>(Bsp.Bsp3dNodes.Count + 1);
            for (var i = 0; i < Bsp.Bsp3dNodes.Count + 1; i++)
                leafybsp.leaves.Add(new leaf());

            if (!populate_plane_designators(ref leafybsp, new List<int>(), 0, -1))
                return false;
          
            for (int use_plane_margins = 0; use_plane_margins < 2; use_plane_margins++)
            {
                int surface_index = 0;
                while (true)
                {
                    LargeSurface surface_block = Bsp.Surfaces[surface_index];
                    if (!surface_block.Flags.HasFlag(SurfaceFlags.TwoSided))
                    {
                        List<RealPoint3d> pointlist = surface_collect_vertices(surface_index);
                        if (!surface_clip_to_leaves(ref leafybsp, 0, use_plane_margins, 0, pointlist, surface_index, surface_block.Plane))
                            return false;
                    }
                    surface_index++;
                    if (surface_index >= Bsp.Surfaces.Count)
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
                            current_leaf.polygon_areas_by_type[use_plane_margins, current_polygon.polygon_type] += current_polygon.polygon_area;
                            ++current_leaf.polygon_counts[use_plane_margins, current_polygon.polygon_type];
                        }
                    }
                    int leaf_is_solid = leaf_index < leafybsp.bsp_leaf_count ? 1 : 0;
                    float polygon_type_areas_sum = current_leaf.polygon_areas_by_type[use_plane_margins, 0] + current_leaf.polygon_areas_by_type[use_plane_margins, 1] + current_leaf.polygon_areas_by_type[use_plane_margins, 2];
                    if (polygon_type_areas_sum <= 0.0)
                        current_leaf.polygon_area_sums[use_plane_margins] = 0.0f;
                    else
                        current_leaf.polygon_area_sums[use_plane_margins] = current_leaf.polygon_areas_by_type[use_plane_margins, 1 + leaf_is_solid] / polygon_type_areas_sum;
                }
            }

            return true;
        }

        public bool setup_leaf_map(ref leafy_bsp leafybsp)
        {
            init_leaf_map(ref leafybsp);
            for (int leaf_portal_index = 0; leaf_portal_index < leafybsp.leaf_map_portals.Count; leaf_portal_index++)
            {
                leaf_map_portal leaf_portal = leafybsp.leaf_map_portals[leaf_portal_index];
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
                                List<RealPoint2d> portal_sliced_polygon = portal_slice_polygon(polygon_points, leaf_portal_points, 0.000099999997d);
                                float sliced_polygon_area = polygon_get_area(portal_sliced_polygon, Bsp.Planes[leaf_portal.plane_index].Value);
                                //greater than count and polygon type 2 or less than count and polygon type 1
                                if (portal_child < leafybsp.bsp_leaf_count == (polygon.polygon_type == 2))
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
                    leaf_portal_designator_set_flags(ref leafybsp, leaf_portal_index);
                else if (back_child_area > 0.1f * leaf_portal_area && front_child_area > 0.1f * leaf_portal_area)
                    leaf_portal_designator_set_flags(ref leafybsp, leaf_portal_index);
            }
            return true;
        }

        public void init_leaf_map(ref leafy_bsp leafybsp)
        {
            leafybsp.leaf_map_leaves = new List<leaf_map_leaf>(Bsp.Bsp3dNodes.Count + 1);
            for (var i = 0; i < Bsp.Bsp3dNodes.Count + 1; i++)
                leafybsp.leaf_map_leaves.Add(new leaf_map_leaf());
            if (Bsp.Bsp3dNodes.Count > 0)
            {
                //should not surpass 256 in length
                leaf_map_node_stack = new int[256];
                init_leaf_map_faces_node(ref leafybsp, 0);
                init_leaf_map_portals(ref leafybsp, 0);
            }
        }
        public void init_leaf_map_faces_node(ref leafy_bsp leafybsp, int bsp3dnode_index)
        {
            LargeBsp3dNode node = Bsp.Bsp3dNodes[bsp3dnode_index];
            for (var i = 0; i < 2; i++)
            {
                int node_stack_node_index = (i == 0) ? (int)(bsp3dnode_index | 0x80000000) : bsp3dnode_index;
                int child_index = (i == 0) ? node.BackChild : node.FrontChild;
                //node stack is 1-indexed instead of 0-indexed
                leaf_map_node_stack[leaf_map_node_stack_count + 1] = node_stack_node_index;
                leaf_map_node_stack_count++;
                if (child_index >= 0)
                    init_leaf_map_faces_node(ref leafybsp, child_index);
                else if (child_index != -1)
                    init_leaf_map_faces_leaf(ref leafybsp, child_index);
                leaf_map_node_stack_count--;
            }
        }
        public void init_leaf_map_faces_leaf(ref leafy_bsp leafybsp, int bsp3dnode_index)
        {
            for (int i = 0; i < leaf_map_node_stack_count; i++)
            {
                //move up the stack from the bottom
                int leaf_node = leaf_map_node_stack[leaf_map_node_stack_count - i];
                LargeBsp3dNode leaf_node_block = Bsp.Bsp3dNodes[leaf_node & 0x7FFFFFFF];
                RealPlane3d leaf_node_plane = Bsp.Planes[leaf_node_block.Plane].Value;
                //maximally sized face will be cut down by bsp planes
                List<RealPoint2d> result_vertices = new List<RealPoint2d>
                {
                    new RealPoint2d(-1536,-1536), 
                    new RealPoint2d(1536,-1536), 
                    new RealPoint2d(1536,1536), 
                    new RealPoint2d(-1536,1536)
                };
                int levels_up = 0;
                while(result_vertices.Count > 0)
                {
                    int current_node = leaf_map_node_stack[leaf_map_node_stack_count - levels_up];
                    if(current_node != leaf_node)
                    {
                        LargeBsp3dNode current_node_block = Bsp.Bsp3dNodes[current_node & 0x7FFFFFFF];
                        RealPlane3d current_node_plane = Bsp.Planes[current_node_block.Plane].Value;
                        if (current_node < 0)
                            current_node_plane = new RealPlane3d(-current_node_plane.I, -current_node_plane.J, -current_node_plane.K, -current_node_plane.D);
                        RealPlane2d intersection_plane = new RealPlane2d();
                        //get 2d intersection plane of current node plane with leaf node plane
                        int plane_intersection_result = planes_get_intersection_plane2d(leaf_node_plane, current_node_plane, ref intersection_plane);
                        if (plane_intersection_result == 1)
                        {
                            //use this intersection line to cut the polygon down to size
                            result_vertices = plane_cut_polygon_2d_new(intersection_plane, result_vertices, 0.00024414062d);
                        }
                        else if (plane_intersection_result == 0)
                        {
                            //if the planes dont intersect, abort
                            result_vertices = new List<RealPoint2d>();
                            break;
                        }
                    }
                    levels_up++;
                    if (levels_up >= leaf_map_node_stack_count)
                        break;
                }
                //write out remaining vertices to a new face on this leaf map leaf
                if(result_vertices.Count > 0)
                {
                    leaf_face new_face = new leaf_face
                    {
                        vertices = result_vertices.DeepClone(),
                        bsp3dnode_index = leaf_node & 0x7FFFFFFF
                    };
                    leafybsp.leaf_map_leaves[bsp3dnode_index & 0x7FFFFFFF].faces.Add(new_face);
                }
            }
        }

        public int planes_get_intersection_plane2d(RealPlane3d plane_A, RealPlane3d plane_B, ref RealPlane2d plane2d)
        {
            int projection_axis = Bsp_Builder.plane_get_projection_coefficient(plane_A);
            int projection_sign = Bsp_Builder.plane_get_projection_sign(plane_A, projection_axis) ? 1 : 0;
            float[] plane_A_parameters = new float[4] { plane_A.I, plane_A.J, plane_A.K, plane_A.D };
            float[] plane_B_parameters = new float[4] { plane_B.I, plane_B.J, plane_B.K, plane_B.D };

            RealPlane3d temp_plane = new RealPlane3d();
            if(plane_B_parameters[projection_axis] == 0.0)
            {
                temp_plane = plane_B.DeepClone();
            }
            else
            {
                double v11 = plane_B_parameters[projection_axis] / plane_A_parameters[projection_axis];
                temp_plane = new RealPlane3d
                {
                    I = (float)(plane_B.I - v11 * plane_A.I),
                    J = (float)(plane_B.J - v11 * plane_A.J),
                    K = (float)(plane_B.K - v11 * plane_A.K),
                    D = (float)(plane_B.D - v11 * plane_A.D),
                };
            }
            float v1 = normalize_realplane3d(ref temp_plane);
            if(v1 == 0.0)
            {
                if (temp_plane.D <= 0.0)
                    return 2;
                else
                    return 0;
            }
            else
            {
                //temporarily pretend the plane is a 3d point so we can pass it into existing 2d projection code
                RealPoint3d temp_point = new RealPoint3d(temp_plane.I, temp_plane.J, temp_plane.K);
                RealPoint2d projected_plane = Bsp_Builder.vertex_get_projection_relevant_coords(temp_point, projection_axis, projection_sign);
                plane2d = new RealPlane2d(projected_plane.X, projected_plane.Y, temp_plane.D / v1);
            }
            return 1;
        }
        public float normalize_realplane3d(ref RealPlane3d plane)
        {
            double v1 = Math.Sqrt(plane.I * plane.I + plane.J * plane.J + plane.K * plane.K);
            if (v1 == 0.0)
                return 0.0f;
            plane = new RealPlane3d
            {
                I = (float)(plane.I / v1),
                J = (float)(plane.J / v1),
                K = (float)(plane.K / v1),
                D = plane.D
            };
            return (float)v1;
        }

        public void init_leaf_map_portals(ref leafy_bsp leafybsp, int bsp3dnode_index)
        {
            LargeBsp3dNode node = Bsp.Bsp3dNodes[bsp3dnode_index];
            for (var i = 0; i < 2; i++)
            {
                int node_stack_node_index = (i == 0) ? (int)(bsp3dnode_index | 0x80000000) : bsp3dnode_index;
                int child_index = (i == 0) ? node.BackChild : node.FrontChild;
                //node stack is 1-indexed instead of 0-indexed
                leaf_map_node_stack[leaf_map_node_stack_count + 1] = node_stack_node_index;
                leaf_map_node_stack_count++;
                if (child_index >= 0)
                    init_leaf_map_portals(ref leafybsp, child_index);
                else if (child_index != -1)
                    init_leaf_map_portals_first_leaf(ref leafybsp, -1, child_index & 0x7FFFFFFF, 0, leaf_map_node_stack_count - 1);
                leaf_map_node_stack_count--;
            }
        }
        public void init_leaf_map_portals_first_leaf(ref leafy_bsp leafybsp, int ancestor_node_index, int leaf_index, int bsp3dnode_index, int levels_up)
        {
            LargeBsp3dNode bsp3dnode_block = Bsp.Bsp3dNodes[bsp3dnode_index];
            int first_traversal_node;
            if (ancestor_node_index == -1)
                first_traversal_node = leaf_map_node_stack[leaf_map_node_stack_count - levels_up];
            else
                first_traversal_node = -1;

            //subfunction which I am inlining here
            int node_is_in_nodestack = 0;
            int use_node_backside = 0;
            for (var levels_up_temp = 0; levels_up_temp < leaf_map_node_stack_count; levels_up_temp++)
            {
                int node_stack_element = leaf_map_node_stack[leaf_map_node_stack_count - levels_up_temp];
                int node_stack_plane = Bsp.Bsp3dNodes[node_stack_element & 0x7FFFFFFF].Plane;
                if (node_stack_plane == bsp3dnode_block.Plane)
                {
                    node_is_in_nodestack = 1;
                    use_node_backside = node_stack_element < 0 ? 1 : 0;
                    break;
                }                    
            }

            int v9 = 0;
            for (var i = 0; i < 2; i++)
            {
                int child_index = (i == 0) ? bsp3dnode_block.BackChild : bsp3dnode_block.FrontChild;
                if (ancestor_node_index == -1 && i == 1 && first_traversal_node < 0)
                    v9 = 1;
                else
                {
                    v9 = 0;
                    if (ancestor_node_index != -1)
                    {
                        if (node_is_in_nodestack == 1 && use_node_backside == i)
                            continue;
                        //LABEL 30
                        if (child_index >= 0)
                        {
                            int bsp_ancestor_index = v9 == 1 ? bsp3dnode_index : ancestor_node_index;
                            init_leaf_map_portals_first_leaf(ref leafybsp, bsp_ancestor_index, leaf_index, child_index, levels_up - 1);
                        }
                        else if (child_index != -1 && (child_index & 0x7FFFFFFF) != leaf_index)
                        {
                            int bsp_ancestor_index = v9 == 1 ? bsp3dnode_index : ancestor_node_index;
                            init_leaf_map_portals_second_leaf(ref leafybsp, bsp_ancestor_index, leaf_index, child_index);
                        }
                        continue;
                    }
                }
                if (i == 0 && first_traversal_node >= 0)
                    continue;
                if (v9 == 1)
                {
                    leaf_map_leaf leaf = leafybsp.leaf_map_leaves[leaf_index & 0x7FFFFFFF];
                    if (leaf.faces.Count <= 0)
                        continue;
                    //there must be one face with a matching bsp3dnode_index in this leaf
                    int face_index = 0;
                    for (face_index = 0; face_index < leaf.faces.Count; face_index++)
                    {
                        if (leaf.faces[face_index].bsp3dnode_index == bsp3dnode_index)
                            break;
                    }
                    if (face_index >= leaf.faces.Count)
                        continue;
                }
                
                //LABEL 30
                if (child_index >= 0)
                {
                    int bsp_ancestor_index = v9 == 1 ? bsp3dnode_index : ancestor_node_index;
                    init_leaf_map_portals_first_leaf(ref leafybsp, bsp_ancestor_index, leaf_index, child_index, levels_up - 1);
                }
                else if (child_index != -1 && (child_index & 0x7FFFFFFF) != leaf_index)
                {
                    int bsp_ancestor_index = v9 == 1 ? bsp3dnode_index : ancestor_node_index;
                    init_leaf_map_portals_second_leaf(ref leafybsp, bsp_ancestor_index, leaf_index, child_index);
                }
            }
        }
        public void init_leaf_map_portals_second_leaf(ref leafy_bsp leafybsp, int bsp3dnode_index, int leaf_index_0, int leaf_index_1)
        {
            leaf_map_leaf leaf0 = leafybsp.leaf_map_leaves[leaf_index_0 & 0x7FFFFFFF];
            leaf_map_leaf leaf1 = leafybsp.leaf_map_leaves[leaf_index_1 & 0x7FFFFFFF];
            //find faces on leaf with matching node index
            //inlined subfunctions
            int leaf_0_face_index = leaf0.faces.FindIndex(face => face.bsp3dnode_index == bsp3dnode_index);
            int leaf_1_face_index = leaf1.faces.FindIndex(face => face.bsp3dnode_index == bsp3dnode_index);

            if (leaf_0_face_index == -1 || leaf_1_face_index == -1)
                return;

            leaf_face leaf_0_face = leaf0.faces[leaf_0_face_index];
            leaf_face leaf_1_face = leaf1.faces[leaf_1_face_index];

            //cut leaf 1 face with leaf 0 face
            List<RealPoint2d> points = portal_slice_polygon(leaf_0_face.vertices, leaf_1_face.vertices, 0.00048828125d);

            if (points.Count <= 0)
                return;

            leaf_map_portal new_leaf_portal = new leaf_map_portal
            {
                plane_index = Bsp.Bsp3dNodes[bsp3dnode_index].Plane,
                back_leaf_index = leaf_index_0 & 0x7FFFFFFF,
                front_leaf_index = leaf_index_1 & 0x7FFFFFFF,
                vertices = new List<RealPoint3d>()
            };

            //convert projected 2d points on node plane back in to 3d points and add to leaf portal
            RealPlane3d node_plane = Bsp.Planes[Bsp.Bsp3dNodes[bsp3dnode_index].Plane].Value;
            foreach(var point in points)
            {
                new_leaf_portal.vertices.Add(point2d_and_plane_to_point3d(node_plane, point));
            }

            //add new leaf portal to array and add corresponding leaf portal indices to both leaves
            leafybsp.leaf_map_portals.Add(new_leaf_portal);
            leaf0.portal_indices.Add(leafybsp.leaf_map_portals.Count - 1);
            leaf1.portal_indices.Add(leafybsp.leaf_map_portals.Count - 1);

            //flag leaf portals that are very small
            float area = polygon_get_area_internal(points);
            double area_perimeter_ratio = Math.Sqrt(area) / polygon_get_perimeter(points);
            if (area < 0.0024999999d || area_perimeter_ratio < 0.0099999998d)
                leaf_portal_designator_set_flags(ref leafybsp, leafybsp.leaf_map_portals.Count - 1);
        }

        public RealPoint3d point2d_and_plane_to_point3d(RealPlane3d plane, RealPoint2d point)
        {
            int projection_axis = Bsp_Builder.plane_get_projection_coefficient(plane);
            int projection_sign = Bsp_Builder.plane_get_projection_sign(plane, projection_axis) ? 1 : 0;
            float[] planecoords = new float[3] { plane.I, plane.J, plane.K };
            float[] result_coords = new float[3];
            int v4 = 2 * (projection_sign + 2 * projection_axis);
            List<int> coordinate_list = new List<int> { 2, 1, 1, 2, 0, 2, 2, 0, 1, 0, 0, 1 };
            int vertex_X_coord_index = coordinate_list[v4];
            int vertex_Y_coord_index = coordinate_list[v4 + 1];

            //assign X and Y coords
            result_coords[vertex_X_coord_index] = point.X;
            result_coords[vertex_Y_coord_index] = point.Y;

            //calculate projection axis coordinate
            //otherwise result coord at projection axis defaults to 0.0
            if (Math.Abs(planecoords[projection_axis]) >= 0.00009999999747378752d)
            {
                result_coords[projection_axis] = 
                    (plane.D - planecoords[vertex_X_coord_index] * point.X - planecoords[vertex_Y_coord_index] * point.Y) /
                    planecoords[projection_axis];
            }
            return new RealPoint3d(result_coords[0], result_coords[1], result_coords[2]);
        }

        public float polygon_get_perimeter(List<RealPoint2d> projected_points)
        {
            //first get distance between last and first point
            double xdiff1 = projected_points[0].X - projected_points[projected_points.Count - 1].X;
            double ydiff1 = projected_points[0].Y - projected_points[projected_points.Count - 1].Y;
            double perimeter = Math.Sqrt(xdiff1 * xdiff1 + ydiff1 * ydiff1);
            //then loop through remaining points and add to the sum
            for (var i = 1; i < projected_points.Count; i++)
            {
                double xdiff = projected_points[i].X - projected_points[i - 1].X;
                double ydiff = projected_points[i].Y - projected_points[i - 1].Y;

                perimeter = perimeter + Math.Sqrt(xdiff * xdiff + ydiff * ydiff);
            }
            return (float)perimeter;
        }

        public void leaf_portal_designator_set_flags(ref leafy_bsp leafybsp, int leaf_portal_index)
        {
            leaf_map_portal portal = leafybsp.leaf_map_portals[leaf_portal_index];
            for (var i = 0; i < 2; i++)
            {
                int child_leaf_index = (i == 0) ? portal.back_leaf_index : portal.front_leaf_index;
                leaf_map_leaf child_leaf = leafybsp.leaf_map_leaves[child_leaf_index];
                for (var j = 0; j < child_leaf.portal_indices.Count; j++)
                {
                    if (leaf_portal_index == (child_leaf.portal_indices[j] & 0x7FFFFFFF))
                    {
                        child_leaf.portal_indices[j] = (int)(child_leaf.portal_indices[j] | 0x80000000);
                        break;
                    }
                }
            }
        }
        public List<RealPoint2d> portal_slice_polygon(List<RealPoint2d> polygon_points, List<RealPoint2d> leaf_portal_points, double margin)
        {
            //generate a 2d plane from every consecutive pair of points in the portal and use it to cut the polygon
            for(int portal_point_index = 0; portal_point_index < leaf_portal_points.Count; portal_point_index++)
            {
                int last_portal_point_index = portal_point_index - 1;
                //for the first set wrap around to the end of the poly for the previous point
                if (portal_point_index == 0)
                    last_portal_point_index = leaf_portal_points.Count - 1;
                RealPlane2d plane2d = Bsp_Builder.generate_bsp2d_plane_parameters(leaf_portal_points[portal_point_index], leaf_portal_points[last_portal_point_index]).Plane;
                polygon_points = plane_cut_polygon_2d_new(plane2d, polygon_points, margin);
                if (polygon_points.Count <= 0)
                    break;
            }
            return polygon_points;
        }

        public List<RealPoint2d> plane_cut_polygon_2d_new(RealPlane2d plane2d, List<RealPoint2d> polygon_points, double margin)
        {
            List<int> plane_d_list = new List<int>();
            bool point_less_than_negative_margin = false;
            bool point_greater_than_margin = false;
            for (var i = 0; i < polygon_points.Count; i++)
            {
                RealPoint2d current_point = polygon_points[i];
                double d = current_point.X * plane2d.I + current_point.Y * plane2d.J - plane2d.D;
                if (d <= margin)
                {
                    if (d >= -margin)
                    {
                        plane_d_list.Add(-1);
                    }
                    else
                    {
                        plane_d_list.Add(0);
                        point_less_than_negative_margin = true;
                    }
                }
                else
                {
                    plane_d_list.Add(1);
                    point_greater_than_margin = true;
                }
            }
            List<RealPoint2d> output_points = new List<RealPoint2d>();
            if (point_less_than_negative_margin)
            {
                if (point_greater_than_margin)
                {
                    int previous_point_index = polygon_points.Count - 1;
                    for (var i = 0; i < polygon_points.Count; i++)
                    {
                        if (plane_d_list[i] != 0)
                        {
                            RealPoint2d current_point = polygon_points[i];
                            RealPoint2d previous_point = polygon_points[previous_point_index];
                            if (plane_d_list[i] == 1 && plane_d_list[previous_point_index] == 0)
                            {
                                RealPoint2d vector = current_point - previous_point;
                                double dot_product = vector.X * plane2d.I + vector.Y * plane2d.J;
                                if (dot_product < -0.000099999997 || dot_product > 0.000099999997)
                                {
                                    double d1 = previous_point.X * plane2d.I + previous_point.Y * plane2d.J - plane2d.D;
                                    double dratio = d1 / dot_product;
                                    output_points.Add(new RealPoint2d
                                    {
                                        X = (float)(previous_point.X - vector.X * dratio),
                                        Y = (float)(previous_point.Y - vector.Y * dratio),
                                    });
                                }
                            }
                            output_points.Add(current_point);
                        }
                        else if(plane_d_list[previous_point_index] == 1)
                        {
                            RealPoint2d current_point = polygon_points[i];
                            RealPoint2d previous_point = polygon_points[previous_point_index];
                            RealPoint2d vector = current_point - previous_point;
                            double dot_product = vector.X * plane2d.I + vector.Y * plane2d.J;
                            if(dot_product < -0.000099999997 || dot_product > 0.000099999997)
                            {
                                double d1 = current_point.X * plane2d.I + current_point.Y * plane2d.J - plane2d.D;
                                double dratio = d1 / dot_product;
                                output_points.Add(new RealPoint2d
                                {
                                    X = (float)(current_point.X - vector.X * dratio),
                                    Y = (float)(current_point.Y - vector.Y * dratio),
                                });
                            }
                        }
                        previous_point_index = i;
                    }
                }
                else
                {
                    return new List<RealPoint2d>();
                }
            }
            else
            {
                return polygon_points;
            }

            if (output_points.Count < 3)
                return new List<RealPoint2d>();

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
                int node_child = i == 1 ? current_node.FrontChild : current_node.BackChild;
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
                if (surface_clip_to_leaves(ref leafybsp, current_node.BackChild, use_plane_margins, node_side + 1, points, surface_index, plane_index))
                {
                    return surface_clip_to_leaves(ref leafybsp, current_node.FrontChild, use_plane_margins, other_node_side + 1, points, surface_index, plane_index);
                }
                return false;
            }
            RealPlane3d node_plane = Bsp.Planes[current_node.Plane].Value;
            double margin = use_plane_margins == 0 ? 0.0d : 0.00024414062d;
            RealPlane3d back_plane = new RealPlane3d(node_plane.I, node_plane.J, node_plane.K, (float)(node_plane.D + margin));
            List<RealPoint3d> back_points = plane_cut_polygon(points, back_plane, false);
            if(back_points.Count > 0 && !surface_clip_to_leaves(ref leafybsp, current_node.BackChild, use_plane_margins, polygon_type, back_points, surface_index, plane_index))
            {
                return false;
            }
            RealPlane3d front_plane = new RealPlane3d(node_plane.I, node_plane.J, node_plane.K, (float)(node_plane.D - margin));
            List<RealPoint3d> front_points = plane_cut_polygon(points, front_plane, true);
            if (front_points.Count <= 0)
                return true;
            return surface_clip_to_leaves(ref leafybsp, current_node.FrontChild, use_plane_margins, polygon_type, front_points, surface_index, plane_index);
        }

        public List<RealPoint3d> plane_cut_polygon(List<RealPoint3d> points, RealPlane3d plane, bool plane_side)
        {
            List<RealPoint3d> output_points = new List<RealPoint3d>();
            RealPoint3d previous_point = points[points.Count - 1];
            double d0 = previous_point.X * plane.I + previous_point.Y * plane.J + previous_point.Z * plane.K - plane.D;
            for(var vertex_index = 0; vertex_index < points.Count; vertex_index++)
            {
                RealPoint3d current_point = points[vertex_index];
                double d1 = current_point.X * plane.I + current_point.Y * plane.J + current_point.Z * plane.K - plane.D;
                //are the current and final vertex on the same side of the plane?
                if(d1 >= 0 != d0 >= 0)
                {
                    output_points.Add(vertex_shift_to_plane(previous_point, current_point, d0, d1));
                }
                if(d1 >= 0 == plane_side)
                {
                    output_points.Add(current_point);
                }
                previous_point = current_point;
                d0 = d1;
            }
            return output_points;
        }

        public RealPoint3d vertex_shift_to_plane(RealPoint3d p0, RealPoint3d p1, double d0, double d1)
        {
            double dratio = d0 / (d0 - d1);
            RealPoint3d result = new RealPoint3d
            {
                X = (float)((p1.X - p0.X) * dratio + p0.X),
                Y = (float)((p1.Y - p0.Y) * dratio + p0.Y),
                Z = (float)((p1.Z - p0.Z) * dratio + p0.Z),
            };
            return result;
        }

        public float polygon_get_area(List<RealPoint2d> projected_points, RealPlane3d plane)
        {
            float area = polygon_get_area_internal(projected_points);
            if (area == 0.0f)
                return 0.0f;

            int plane_projection_axis = Bsp_Builder.plane_get_projection_coefficient(plane);
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

        public float polygon_get_area_internal(List<RealPoint2d> projected_points)
        {
            double area = 0.0f;
            for (var i = 0; i < projected_points.Count - 2; i++)
            {
                RealPoint2d v21 = projected_points[i + 1] - projected_points[0];
                RealPoint2d v31 = projected_points[i + 2] - projected_points[0];

                //cross product of two vectors times 0.5
                area = area + (v31.Y * v21.X - v31.X * v21.Y) * 0.5f;
            }
            return (float)Math.Abs(area);
        }

        public List<RealPoint2d> polygon_project_on_plane(List<RealPoint3d> points, RealPlane3d plane, int plane_index)
        {
            int plane_projection_axis = Bsp_Builder.plane_get_projection_coefficient(plane);
            bool plane_projection_positive = Bsp_Builder.plane_get_projection_sign(plane, plane_projection_axis);

            int plane_mirror_check = plane_projection_positive != (plane_index < 0) ? 1 : 0;
            List<RealPoint2d> projected_points = new List<RealPoint2d>();
            for (var i = 0; i < points.Count; i++)
            {
                RealPoint2d projected_coords = Bsp_Builder.vertex_get_projection_relevant_coords(points[i], plane_projection_axis, plane_mirror_check);
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
    
