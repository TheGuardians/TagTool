using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Tags.Definitions;
using TagTool.Tags;

namespace TagTool.Commands.CollisionModels
{
    class GenerateCollisionBSPCommand : Command
    {
        private GameCache Cache { get; }
        private CollisionModel Definition { get; }
        private CollisionGeometry Bsp { get; }

        public GenerateCollisionBSPCommand(GameCache cache, CollisionModel definition) :
            base(true,

                "GenerateCollisionBSP",
                "Generates a Collision BSP for the current collision tag, removing the previous collision BSP",

                "GenerateCollisionBSP",

                "")
        {
            Cache = cache;
            Definition = definition;
            Bsp = definition.Regions[0].Permutations[0].Bsps[0].Geometry.DeepClone();
        }

        public override object Execute(List<string> args)
        {
            //make sure there is nothing in the bsp blocks before starting
            Bsp.Leaves.Clear();
            Bsp.Bsp2dNodes.Clear();
            Bsp.Bsp2dReferences.Clear();
            Bsp.Bsp3dNodes.Clear();

            //allocate surface array before starting the bsp build
            surface_array_definition surface_array = new surface_array_definition { free_count = Bsp.Surfaces.Count, used_count = 0, surface_array = new List<int>()};
            for (int i = 0; i < Bsp.Surfaces.Count; i++)
            {
                if(Bsp.Surfaces[i].Flags.HasFlag(SurfaceFlags.Climbable) || Bsp.Surfaces[i].Flags.HasFlag(SurfaceFlags.Breakable))
                    surface_array.surface_array.Add(i);
                else
                    surface_array.surface_array.Add((int)(i | 0x80000000));
            }
                
            //NOTE: standard limit for number of bsp3dnodes (bsp planes) is 128 (maximum bsp depth)
            int bsp3dnode_index = -1;
            if(build_bsp_tree_main(surface_array, ref bsp3dnode_index) && set_bsp3dnode_leaf_children())
            {
                Console.WriteLine("### Collision bsp built successfully!");
                Definition.Regions[0].Permutations[0].Bsps[0].Geometry = Bsp;
            }
            else
            {
                Console.WriteLine("### Failed to build collision bsp!");
                return false;
            }

            return true;
        }

        public int bsp_tree_decision_switch(ref surface_array_definition surface_array)
        {
            int surface_array_total_count = surface_array.used_count + surface_array.free_count;
            if (surface_array_total_count <= 0)
                return 2;
            int free_surfaces_positive = 0;
            int used_surfaces_positive = 0;
            int used_surfaces_negative = 0;
            for (int surface_index_index = 0; surface_index_index < surface_array_total_count; surface_index_index++)
            {
                int surface_index = surface_array.surface_array[surface_index_index];
                if(surface_index_index >= surface_array.free_count) //surface is used
                {
                    if (surface_index >= 0)
                        used_surfaces_positive++;
                    else
                        used_surfaces_negative++;
                }
                else //surface is free
                {
                    if (surface_index < 0)
                        return 1; //if there is a free surface with a negative surface index, create bsp3dnodes
                    free_surfaces_positive++;
                }
            }
            bool no_free_surfaces_positive = free_surfaces_positive == 0;
            bool no_used_surfaces_positive = used_surfaces_positive == 0;
            if (free_surfaces_positive > 0)
            {
                if (no_used_surfaces_positive)
                    return 1;
            }
            if (no_free_surfaces_positive && no_used_surfaces_positive)
                return 2;
            if(used_surfaces_negative > 0 || used_surfaces_positive <= 0)
                return surface_array_recalculate_counts(ref surface_array);
            return 3;
        }

        public int surface_array_recalculate_counts(ref surface_array_definition surface_array)
        {
            return 0;
        }

        public bool set_bsp3dnode_leaf_children()
        {
            int leaf_index = 0;
            for(int bsp3dnode_index = 0; bsp3dnode_index < Bsp.Bsp3dNodes.Count; bsp3dnode_index++)
            {
                if ((Bsp.Bsp3dNodes[bsp3dnode_index].FrontChildMid & 0x80) != 0)
                {
                    //Bsp.Bsp3dNodes[bsp3dnode_index].FrontChildMid = leaf_index < 0 ? (byte)(leaf_index | 0x80) : (byte)leaf_index;
                    leaf_index++;
                }
                if ((Bsp.Bsp3dNodes[bsp3dnode_index].BackChildMid & 0x80) != 0)
                {
                    //Bsp.Bsp3dNodes[bsp3dnode_index].BackChildMid = leaf_index < 0 ? (byte)(leaf_index | 0x80) : (byte)leaf_index;
                    leaf_index++;
                }
            }
            if(leaf_index > Bsp.Leaves.Count || leaf_index != Bsp.Bsp3dNodes.Count + 1)
            {
                Console.WriteLine("### ERROR the number of leaves does not match the bsp!");
                return false;
            }
            return true;
        }

        public bool build_bsp_tree_main(surface_array_definition surface_array, ref int bsp3dnode_index)
        {
            switch (bsp_tree_decision_switch(ref surface_array))
            {
                case 1: //construct bsp3d nodes
                    plane_splitting_parameters splitting_parameters = find_surface_splitting_plane(surface_array);
                    surface_array_definition back_surface_array = new surface_array_definition {free_count = splitting_parameters.BackSurfaceCount, used_count = splitting_parameters.BackSurfaceUsedCount, surface_array = new List<int>() };
                    surface_array_definition front_surface_array = new surface_array_definition { free_count = splitting_parameters.FrontSurfaceCount, used_count = splitting_parameters.FrontSurfaceUsedCount, surface_array = new List<int>() };
                    if (!split_object_surfaces_with_plane(surface_array, splitting_parameters.plane_index, ref back_surface_array, ref front_surface_array))
                        return false;
                    Bsp.Bsp3dNodes.Add(new Bsp3dNode());
                    int current_bsp3dnode_index = Bsp.Bsp3dNodes.Count - 1;
                    int back_child_node_index = -1;
                    int front_child_node_index = -1;
                    //this function is recursive, and continues branching until no more 3d nodes can be made
                    if(build_bsp_tree_main(back_surface_array,ref back_child_node_index) && build_bsp_tree_main(front_surface_array, ref front_child_node_index))
                    {
                        Bsp.Bsp3dNodes[current_bsp3dnode_index].Plane = (short)splitting_parameters.plane_index;
                        Bsp.Bsp3dNodes[current_bsp3dnode_index].FrontChildMid = front_child_node_index < 0 ? (byte)(front_child_node_index | 0x80) : (byte)front_child_node_index;
                        Bsp.Bsp3dNodes[current_bsp3dnode_index].BackChildMid = back_child_node_index < 0 ? (byte)(back_child_node_index | 0x80) : (byte)back_child_node_index;
                        bsp3dnode_index = current_bsp3dnode_index;
                    }
                    else
                    {
                        Console.WriteLine("### ERROR couldn't build surface lists.");
                        return false;
                    }
                    return true;
                case 2: //build leaves
                    if(set_used_surface_flags(ref surface_array) == -1)
                    {
                        Console.WriteLine("### ERROR tried to build leaves while there are free surfaces.");
                        return false;
                    }
                    int leaf_index = -1;
                    if(build_leaves(ref surface_array, ref leaf_index))
                    {
                        bsp3dnode_index = (int)(leaf_index | 0x80000000);
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("### ERROR couldn't build leaf.");
                        return false;
                    }
                case 3: //all done!
                    bsp3dnode_index = -1;
                    return true;
                default:
                    Console.WriteLine("### ERROR couldn't decide what to build.\n");
                    return false;
            }
        }

        public int set_used_surface_flags(ref surface_array_definition surface_array)
        {
            if (surface_array.free_count > 0)
                return -1;
            for(int surface_index_index = 0; surface_index_index < surface_array.used_count; surface_index_index++)
            {
                surface_array.surface_array[surface_index_index] = (int)(surface_array.surface_array[surface_index_index] | 0x80000000);
            }
            return surface_array.used_count;
        }

        public surface_array_definition collect_plane_matching_surfaces(ref surface_array_definition surface_array, int plane_index)
        {
            surface_array_definition plane_matched_surface_array = new surface_array_definition {surface_array = new List<int>() };
            for (int surface_array_index = 0; surface_array_index < surface_array.used_count; surface_array_index++)
            {
                int surface_index = surface_array.surface_array[surface_array_index] & 0x7FFFFFFF;
                if(surface_index < 0 && Bsp.Surfaces[surface_index].Plane == plane_index)
                {
                    plane_matched_surface_array.surface_array.Add(surface_index);
                    //reset plane matching surfaces in the primary array to an unused state
                    surface_array.surface_array[surface_array_index] = surface_index;
                }
            }
            return plane_matched_surface_array;
        }

        public int plane_determine_axis_minimum_coefficient(Plane plane_block)
        {
            int minimum_coefficient;
            float plane_I = Math.Abs(plane_block.Value.I);
            float plane_J = Math.Abs(plane_block.Value.J);
            float plane_K = Math.Abs(plane_block.Value.K);
            if (plane_K < plane_J || plane_K < plane_I)
            {
                if (plane_J >= plane_I)
                    minimum_coefficient = 1;
                else
                    minimum_coefficient = 0;
            }
            else
                minimum_coefficient = 2;
            return minimum_coefficient;
        }

        public bool check_plane_projection_parameter_greater_than_0(Plane plane_block, int projection_axis)
        {
            switch (projection_axis)
            {
                case 0: //x axis
                    return plane_block.Value.I > 0;
                case 1: //y axis
                    return plane_block.Value.J > 0;
                case 2: //z axis
                    return plane_block.Value.K > 0;
            }
            return false;
        }

        public void sort_surfaces_by_plane_2D(int plane_projection_axis, int plane_mirror_check, Bsp2dNode bsp2dnode_block, surface_array_definition plane_matched_surface_array, surface_array_definition back_surface_array, surface_array_definition front_surface_array)
        {
            foreach(int surface_index in plane_matched_surface_array.surface_array)
            {
                Plane_Relationship surface_location = determine_surface_plane_relationship_2D(surface_index, bsp2dnode_block, plane_projection_axis, plane_mirror_check);
                if (surface_location.HasFlag(Plane_Relationship.BackofPlane))
                {
                    back_surface_array.surface_array.Add(surface_index);
                }
                if (surface_location.HasFlag(Plane_Relationship.FrontofPlane))
                {
                    front_surface_array.surface_array.Add(surface_index);
                }
            }
        }

        public int create_bsp2dnodes(int plane_projection_axis, int plane_mirror_check, surface_array_definition plane_matched_surface_array)
        {
            int bsp2dnode_index = -1;
            int back_bsp2dnode_index = -1;
            int front_bsp2dnode_index = -1;
            Bsp2dNode bsp2dnode_block = new Bsp2dNode(){LeftChild = -1, RightChild = -1};
            plane_splitting_parameters splitting_parameters = generate_best_splitting_plane_2D(plane_projection_axis, plane_mirror_check, ref bsp2dnode_block, plane_matched_surface_array);
            if(splitting_parameters.FrontSurfaceCount > 0 && splitting_parameters.BackSurfaceCount > 0)
            {
                surface_array_definition back_surface_array = new surface_array_definition();
                surface_array_definition front_surface_array = new surface_array_definition();

                Bsp.Bsp2dNodes.Add(bsp2dnode_block);
                bsp2dnode_index = Bsp.Bsp2dNodes.Count - 1;

                sort_surfaces_by_plane_2D(plane_projection_axis, plane_mirror_check, bsp2dnode_block, plane_matched_surface_array, back_surface_array, front_surface_array);

                //create a child node with the back surface array first
                back_bsp2dnode_index = create_bsp2dnodes(plane_projection_axis, plane_mirror_check, back_surface_array);
                if (back_bsp2dnode_index == -1)
                {
                    bsp2dnode_index = -1;
                }
                else
                {
                    front_bsp2dnode_index = create_bsp2dnodes(plane_projection_axis, plane_mirror_check, front_surface_array);
                    if (front_bsp2dnode_index == -1)
                    {
                        bsp2dnode_index = -1;
                    }
                    else
                    {
                        bsp2dnode_block.LeftChild = (short)back_bsp2dnode_index;
                        bsp2dnode_block.RightChild = (short)front_bsp2dnode_index;
                    }
                }
                return bsp2dnode_index;
            }
            Console.WriteLine("### ERROR couldn't build bsp because of overlapping surfaces.");
            return bsp2dnode_index;
        }

        public bool build_leaves(ref surface_array_definition surface_array, ref int leaf_index)
        {
            bool result = true;

            Bsp.Leaves.Add(new Leaf());
            leaf_index = Bsp.Leaves.Count - 1;

            //allocate initial leaf values
            Bsp.Leaves[leaf_index].Flags = 0;
            Bsp.Leaves[leaf_index].Bsp2dReferenceCount = 0;
            Bsp.Leaves[leaf_index].FirstBsp2dReference = -1;

            for(int surface_array_index = 0; surface_array_index < surface_array.free_count + surface_array.used_count; surface_array_index++)
            {
                int surface_index = surface_array.surface_array[surface_array_index];
                Surface surface_block = Bsp.Surfaces[(int)(surface_index & 0x7FFFFFFF)];

                //carry over surface flags
                if (surface_block.Flags.HasFlag(SurfaceFlags.TwoSided))
                    Bsp.Leaves[leaf_index].Flags |= LeafFlags.ContainsDoubleSidedSurfaces;

                if(surface_index < 0)
                {
                    int plane_index = surface_block.Plane;
                    surface_array_definition plane_matched_surface_array = collect_plane_matching_surfaces(ref surface_array, plane_index);
                    if(plane_matched_surface_array.surface_array.Count > 0)
                    {
                        Bsp.Bsp2dReferences.Add(new Bsp2dReference());
                        int bsp2drefindex = Bsp.Bsp2dReferences.Count - 1;
                        Plane plane_block = Bsp.Planes[plane_index & 0x7FFFFFFF];
                        int plane_projection_axis = plane_determine_axis_minimum_coefficient(plane_block);
                        bool plane_projection_parameter_greater_than_0 = check_plane_projection_parameter_greater_than_0(plane_block, plane_projection_axis);
                        Bsp.Bsp2dReferences[bsp2drefindex].PlaneIndex = (short)plane_index;
                        Bsp.Bsp2dReferences[bsp2drefindex].Bsp2dNodeIndex = -1;

                        //update leaf block parameters given new bsp2dreference
                        Bsp.Leaves[leaf_index].Bsp2dReferenceCount++;
                        if (Bsp.Leaves[leaf_index].FirstBsp2dReference == -1)
                            Bsp.Leaves[leaf_index].FirstBsp2dReference = bsp2drefindex;

                        //not 100% sure what this is checking
                        int plane_mirror_check = plane_projection_parameter_greater_than_0 != plane_index < 0 ? 1 : 0;

                        int bsp2dnodeindex = create_bsp2dnodes(plane_projection_axis, plane_mirror_check, plane_matched_surface_array);
                        Bsp.Bsp2dReferences[bsp2drefindex].Bsp2dNodeIndex = (short)bsp2dnodeindex;
                        if (bsp2dnodeindex == -1)
                        {
                            Console.WriteLine("### ERROR couldn't build bsp2d.");
                            result = false;
                        }
                    }
                }
            }
            return result;
        }

        public void divide_surface_into_two_surfaces(int original_surface_index, int plane_index, ref int new_surface_index_A, ref int new_surface_index_B)
        {
            RealPlane3d plane_block = Bsp.Planes[plane_index].Value;
            Surface original_surface = Bsp.Surfaces[original_surface_index];
            Surface new_surface_block_A = Bsp.Surfaces[new_surface_index_A];
            Surface new_surface_block_B = Bsp.Surfaces[new_surface_index_B];

            //copy over values that won't change
            new_surface_block_A.Flags = original_surface.Flags;
            new_surface_block_A.BreakableSurfaceIndex = original_surface.BreakableSurfaceIndex;
            new_surface_block_A.Plane = original_surface.Plane;
            new_surface_block_B.Flags = original_surface.Flags;
            new_surface_block_B.BreakableSurfaceIndex = original_surface.BreakableSurfaceIndex;
            new_surface_block_B.Plane = original_surface.Plane;
            
            int surface_edge_index = original_surface.FirstEdge;

            int dividing_edge_index = -1;
            int previous_new_edge_index = -1;
            int first_new_edge_index = -1;

            //find edges of the original surface that are split by the plane and divide them
            while (true)
            {
                //grab edge vertices
                Edge surface_edge_block = Bsp.Edges[surface_edge_index];
                int edge_vertex_A_index;
                int edge_vertex_B_index;

                if (surface_edge_block.RightSurface == original_surface_index)
                {
                    edge_vertex_A_index = surface_edge_block.EndVertex;
                    edge_vertex_B_index = surface_edge_block.StartVertex;
                }
                else
                {
                    edge_vertex_A_index = surface_edge_block.StartVertex;
                    edge_vertex_B_index = surface_edge_block.EndVertex;
                }

                RealPoint3d edge_vertex_A = Bsp.Vertices[edge_vertex_A_index].Point;
                RealPoint3d edge_vertex_B = Bsp.Vertices[edge_vertex_B_index].Point;

                //calculate vertex plane relationships
                Plane_Relationship vertex_plane_relationship_A = determine_vertex_plane_relationship(edge_vertex_A, plane_block);
                Plane_Relationship vertex_plane_relationship_B = determine_vertex_plane_relationship(edge_vertex_B, plane_block);

                //check if there is a vertex on both sides of the plane
                Plane_Relationship edge_plane_relationship = vertex_plane_relationship_A | vertex_plane_relationship_B;

                //if this edge spans the plane, then create a new dividing edge
                if((edge_plane_relationship & Plane_Relationship.FrontofPlane) > 0 && 
                    ((edge_plane_relationship & Plane_Relationship.BackofPlane) > 0))
                {
                    //allocate new edges and vertex
                    Bsp.Vertices.Add(new Vertex());
                    int new_vertex_index_A = Bsp.Vertices.Count - 1;
                    Vertex new_vertex_block_A = Bsp.Vertices[new_vertex_index_A];
                    Bsp.Edges.Add(new Edge());
                    Bsp.Edges.Add(new Edge());
                    int new_edge_index_A = Bsp.Edges.Count - 2;
                    int new_edge_index_B = Bsp.Edges.Count - 1;
                    Edge new_edge_block_A = Bsp.Edges[new_edge_index_A];
                    Edge new_edge_block_B = Bsp.Edges[new_edge_index_B];

                    if (dividing_edge_index == -1)
                    {
                        //allocate new dividing edge
                        Bsp.Edges.Add(new Edge());
                        dividing_edge_index = Bsp.Edges.Count - 1;
                        Edge dividing_edge_block = Bsp.Edges[dividing_edge_index];

                        //new edge C will be the edge that separates the two new surfaces
                        dividing_edge_block.StartVertex = (short)new_vertex_index_A;
                        dividing_edge_block.ReverseEdge = (ushort)new_edge_index_B;
                        if (vertex_plane_relationship_A.HasFlag(Plane_Relationship.FrontofPlane))
                            dividing_edge_block.LeftSurface = (short)new_surface_index_B;
                        else
                            dividing_edge_block.LeftSurface = (short)new_surface_index_A;
                        if (vertex_plane_relationship_B.HasFlag(Plane_Relationship.FrontofPlane))
                            dividing_edge_block.RightSurface = (short)new_surface_index_B;
                        else
                            dividing_edge_block.RightSurface = (short)new_surface_index_A;
                    }
                    else
                    {
                        Edge dividing_edge_block = Bsp.Edges[dividing_edge_index];
                        dividing_edge_block.EndVertex = (short)new_vertex_index_A;
                        dividing_edge_block.ForwardEdge = (ushort)new_edge_index_B;
                    }

                    float plane_vertex_input_A = edge_vertex_A.X * plane_block.I + edge_vertex_A.Y * plane_block.J + edge_vertex_A.Z * plane_block.K - plane_block.D;
                    float plane_vertex_input_B = edge_vertex_B.X * plane_block.I + edge_vertex_B.Y * plane_block.J + edge_vertex_B.Z * plane_block.K - plane_block.D;
                    float plane_vertex_input_AB_ratio = plane_vertex_input_A / (plane_vertex_input_A - plane_vertex_input_B);

                    new_vertex_block_A.Point.X = (edge_vertex_B.X - edge_vertex_A.X) * plane_vertex_input_AB_ratio + edge_vertex_A.X;
                    new_vertex_block_A.Point.Y = (edge_vertex_B.Y - edge_vertex_A.Y) * plane_vertex_input_AB_ratio + edge_vertex_A.Y;
                    new_vertex_block_A.Point.Z = (edge_vertex_B.Z - edge_vertex_A.Z) * plane_vertex_input_AB_ratio + edge_vertex_A.Z;
                    new_vertex_block_A.FirstEdge = (short)new_edge_index_A;

                    new_edge_block_A.ForwardEdge = (ushort)dividing_edge_index;
                    new_edge_block_A.StartVertex = (short)edge_vertex_A_index;
                    new_edge_block_A.EndVertex = (short)new_vertex_index_A;
                    if (vertex_plane_relationship_A.HasFlag(Plane_Relationship.FrontofPlane))
                        new_edge_block_A.LeftSurface = (short)new_surface_index_B;
                    else
                        new_edge_block_A.LeftSurface = (short)new_surface_index_A;
                    if (vertex_plane_relationship_B.HasFlag(Plane_Relationship.FrontofPlane))
                        new_edge_block_A.RightSurface = (short)new_surface_index_B;
                    else
                        new_edge_block_A.RightSurface = (short)new_surface_index_A;

                    //connect previous edge to generated edges
                    if(previous_new_edge_index != -1)
                        Bsp.Edges[previous_new_edge_index].ForwardEdge = (ushort)new_edge_index_A;
                    previous_new_edge_index = new_edge_index_B;

                }

                //not sure about what this is for, if vertex A isn't on the plane or vertex B IS on the plane...
                else if (!vertex_plane_relationship_A.HasFlag(Plane_Relationship.OnPlane) || vertex_plane_relationship_B.HasFlag(Plane_Relationship.OnPlane))
                {
                    //allocate new edge
                    Bsp.Edges.Add(new Edge());
                    int new_edge_index_D = Bsp.Edges.Count - 1;
                    Edge new_edge_block_D = Bsp.Edges[new_edge_index_D];

                    new_edge_block_D.StartVertex = (short)edge_vertex_A_index;
                    new_edge_block_D.EndVertex = (short)edge_vertex_B_index;
                    new_edge_block_D.ReverseEdge = ushort.MaxValue; //set to -1
                    if (vertex_plane_relationship_A.HasFlag(Plane_Relationship.FrontofPlane))
                        new_edge_block_D.LeftSurface = (short)new_surface_index_B;
                    else
                        new_edge_block_D.LeftSurface = (short)new_surface_index_A;
                    new_edge_block_D.RightSurface = (short)-1;
                    if (first_new_edge_index == -1)
                        first_new_edge_index = new_edge_index_D;

                    //connect previous edge to generated edges
                    if (previous_new_edge_index != -1)
                        Bsp.Edges[previous_new_edge_index].ForwardEdge = (ushort)new_edge_index_D;
                    previous_new_edge_index = new_edge_index_D;
                }
                else
                {
                    //allocate new edge
                    Bsp.Edges.Add(new Edge());
                    int new_edge_index_E = Bsp.Edges.Count - 1;
                    Edge new_edge_block_E = Bsp.Edges[new_edge_index_E];

                    if(dividing_edge_index == -1)
                    {
                        //allocate new dividing edge
                        Bsp.Edges.Add(new Edge());
                        dividing_edge_index = Bsp.Edges.Count - 1;
                        Edge dividing_edge_block = Bsp.Edges[dividing_edge_index];

                        dividing_edge_block.StartVertex = (short)edge_vertex_A_index;
                        dividing_edge_block.EndVertex = (short)-1;
                        dividing_edge_block.ForwardEdge = ushort.MaxValue; //set to -1
                        dividing_edge_block.ReverseEdge = (ushort)new_edge_index_E;

                        if (vertex_plane_relationship_B.HasFlag(Plane_Relationship.BackofPlane))
                            dividing_edge_block.LeftSurface = (short)new_surface_index_B;
                        else
                            dividing_edge_block.LeftSurface = (short)new_surface_index_A;
                        if (vertex_plane_relationship_B.HasFlag(Plane_Relationship.FrontofPlane))
                            dividing_edge_block.RightSurface = (short)new_surface_index_B;
                        else
                            dividing_edge_block.RightSurface = (short)new_surface_index_A;
                    }
                    else
                    {
                        Edge dividing_edge_block = Bsp.Edges[dividing_edge_index];
                        dividing_edge_block.EndVertex = (short)edge_vertex_A_index;
                        dividing_edge_block.ForwardEdge = (ushort)new_edge_index_E;
                    }

                    new_edge_block_E.EndVertex = (short)edge_vertex_B_index;
                    new_edge_block_E.StartVertex = (short)edge_vertex_A_index;
                    new_edge_block_E.ForwardEdge = ushort.MaxValue; //set to -1
                    new_edge_block_E.ReverseEdge = ushort.MaxValue; //set to -1
                    new_edge_block_E.RightSurface = -1;

                    if (vertex_plane_relationship_B.HasFlag(Plane_Relationship.FrontofPlane))
                        new_edge_block_E.LeftSurface = (short)new_surface_index_B;
                    else
                        new_edge_block_E.LeftSurface = (short)new_surface_index_A;

                    if (first_new_edge_index == -1)
                        first_new_edge_index = dividing_edge_index;

                    //connect previous edge to new generated edges
                    if (previous_new_edge_index != -1)
                        Bsp.Edges[previous_new_edge_index].ForwardEdge = (ushort)dividing_edge_index;
                    previous_new_edge_index = new_edge_index_E;
                }

                if (surface_edge_block.RightSurface == original_surface_index)
                    surface_edge_index = surface_edge_block.ReverseEdge;
                else
                    surface_edge_index = surface_edge_block.ForwardEdge;
                //break the loop if we have finished circulating the surface
                if (surface_edge_index == original_surface.FirstEdge)
                    break;
            }
            //connect loose ends and set first edge of new surfaces
            Bsp.Edges[previous_new_edge_index].ForwardEdge = (ushort)first_new_edge_index;
            new_surface_block_A.FirstEdge = (ushort)dividing_edge_index;
            new_surface_block_B.FirstEdge = (ushort)dividing_edge_index;
        }

        public bool split_object_surfaces_with_plane(surface_array_definition surface_array, int plane_index, ref surface_array_definition back_surfaces_array, ref surface_array_definition front_surfaces_array)
        {
            int surface_array_index = 0;

            int back_free_count = 0;
            int back_used_count = 0;
            int front_free_count = 0;
            int front_used_count = 0;

            int unknown_count = 0;
            int back_of_plane_count = 0;
            int front_of_plane_count = 0;
            int split_plane_count = 0;
            int on_plane_count = 0;

            if(surface_array.free_count + surface_array.used_count > 0)
            {
                while (true)
                {
                    int surface_index = surface_array.surface_array[surface_array_index];
                    bool surface_index_less_than_0 = false;
                    if (surface_index < 0)
                        surface_index_less_than_0 = true;
                    int absolute_surface_index = surface_index & 0x7FFFFFFF;
                    bool surface_is_free = surface_array_index < surface_array.free_count;
                    int current_surface_plane_index = Bsp.Surfaces[absolute_surface_index].Plane;
                    bool surface_is_double_sided = Bsp.Surfaces[absolute_surface_index].Flags.HasFlag(SurfaceFlags.TwoSided);
                    int back_surface_index = -1;
                    int front_surface_index = -1;
                    switch (determine_surface_plane_relationship(absolute_surface_index, plane_index, new RealPlane3d()))
                    {
                        case Plane_Relationship.Unknown: //surface does not appear to be on either side of the plane and is not on the plane either
                            unknown_count++;
                            if (surface_index != -1)
                            {
                                if (surface_is_free)
                                {
                                    //free surfaces need to come first in the list
                                    back_surfaces_array.surface_array.Insert(0, surface_index);
                                    back_free_count++;
                                    front_surfaces_array.surface_array.Insert(0, surface_index);
                                    front_free_count++;
                                }
                                else
                                {
                                    back_surfaces_array.surface_array.Add(surface_index);
                                    back_used_count++;
                                    front_surfaces_array.surface_array.Add(surface_index);
                                    front_used_count++;
                                }
                            }
                            break;

                        case Plane_Relationship.BackofPlane: //surface is in back of plane
                            back_of_plane_count++;
                            if (surface_index != -1)
                            {
                                if (surface_is_free)
                                {
                                    //free surfaces need to come first in the list
                                    back_surfaces_array.surface_array.Insert(0, surface_index);
                                    back_free_count++;
                                }
                                else
                                {
                                    back_surfaces_array.surface_array.Add(surface_index);
                                    back_used_count++;
                                }
                            }
                            break;

                        case Plane_Relationship.FrontofPlane: //surface is in front of plane
                            front_of_plane_count++;
                            if (surface_index != -1)
                            {
                                if (surface_is_free)
                                {
                                    //free surfaces need to come first in the list
                                    front_surfaces_array.surface_array.Insert(0, surface_index);
                                    front_free_count++;
                                }
                                else
                                {
                                    front_surfaces_array.surface_array.Add(surface_index);
                                    front_used_count++;
                                }
                            }
                            break;

                        case Plane_Relationship.BothSidesofPlane: //surface is both in front of and behind plane
                            split_plane_count++;
                            Bsp.Surfaces.Add(new Surface());
                            Bsp.Surfaces.Add(new Surface());
                            int new_surface_A_index = Bsp.Surfaces.Count - 2;
                            int new_surface_B_index = Bsp.Surfaces.Count - 1;

                            //split surface into two new surfaces, one on each side of the plane
                            divide_surface_into_two_surfaces(surface_index, plane_index, ref new_surface_A_index, ref new_surface_B_index);

                            //propagate surface index flags to child surfaces
                            if (surface_index_less_than_0)
                            {
                                back_surface_index = (int)(new_surface_A_index | 0x80000000);
                                front_surface_index = (int)(new_surface_B_index | 0x80000000);
                            }
                            else
                            {
                                back_surface_index = (int)new_surface_A_index & 0x7FFFFFFF;
                                front_surface_index = (int)new_surface_B_index & 0x7FFFFFFF;
                            }

                            //add child surfaces to appropriate arrays
                            if (surface_is_free)
                            {
                                //free surfaces need to come first in the list
                                if (back_surface_index != -1)
                                {
                                    back_surfaces_array.surface_array.Insert(0, back_surface_index);
                                    back_free_count++;
                                }
                                if (front_surface_index != -1)
                                {
                                    front_surfaces_array.surface_array.Insert(0, front_surface_index);
                                    front_free_count++;
                                }
                            }
                            else
                            {
                                if (back_surface_index != -1)
                                {
                                    back_surfaces_array.surface_array.Add(back_surface_index);
                                    back_used_count++;
                                }
                                if (front_surface_index != -1)
                                {
                                    front_surfaces_array.surface_array.Add(front_surface_index);
                                    front_used_count++;
                                }
                            }
                            break;

                        case Plane_Relationship.OnPlane: //surface is ON the plane
                            on_plane_count++;
                            if (!surface_index_less_than_0)
                            {
                                break;
                            }
                            if(!surface_is_free || (current_surface_plane_index & 0x7FFFFFFF) != plane_index)
                            {
                                Console.WriteLine("### ERROR Surface on plane does not have matching plane index!");
                            }
                            if (!surface_is_double_sided) //surface is NOT double sided
                            {
                                if (current_surface_plane_index >= 0)
                                {
                                    back_surface_index = (int)absolute_surface_index & 0x7FFFFFFF;
                                    front_surface_index = (int)(absolute_surface_index | 0x80000000);
                                }
                                else
                                {
                                    back_surface_index = (int)(absolute_surface_index | 0x80000000);
                                    front_surface_index = (int)absolute_surface_index & 0x7FFFFFFF;
                                }
                            }
                            else
                            {
                                if (current_surface_plane_index >= 0)
                                {
                                    front_surface_index = (int)(absolute_surface_index | 0x80000000);
                                    back_surface_index = -1;
                                }
                                else
                                {
                                    back_surface_index = (int)(absolute_surface_index | 0x80000000);
                                    front_surface_index = -1;
                                }
                            }
                            //add child surfaces to appropriate arrays
                            //none of these surfaces are considered free, as surface_is_free is set to false for all conditions
                            if (back_surface_index != -1)
                            {
                                back_surfaces_array.surface_array.Add(back_surface_index);
                                back_used_count++;
                            }
                            if (front_surface_index != -1)
                            {
                                front_surfaces_array.surface_array.Add(front_surface_index);
                                front_used_count++;
                            }
                            break;
                    }

                    if (++surface_array_index >= surface_array.free_count + surface_array.used_count)
                        break;
                }
            }
            
            if (back_free_count != back_surfaces_array.free_count)
            {
                Console.WriteLine("back_free_count != back_surfaces->free_count");
                return false;
            }
            if (back_used_count != back_surfaces_array.used_count)
            {
                Console.WriteLine("back_used_count != back_surfaces->used_count");
                return false;
            }
            if (front_free_count != front_surfaces_array.free_count)
            {
                Console.WriteLine("front_free_count != front_surfaces->free_count");
                return false;
            }
            if (front_used_count != front_surfaces_array.used_count)
            {
                Console.WriteLine("front_used_count != front_surfaces->used_count");
                return false;
            }
            return true;
        }

        public plane_splitting_parameters find_surface_splitting_plane(surface_array_definition surface_array)
        {
            plane_splitting_parameters splitting_parameters = new plane_splitting_parameters();

            //this code should only be used with <1024 surfaces as it loops and checks every surface. large meshes first need to be split with generate_object_splitting_plane
            if (surface_array.free_count < 1024)
            {
                splitting_parameters = surface_plane_splitting_effectiveness_check_loop(surface_array);
                return splitting_parameters;
            }

            splitting_parameters = generate_object_splitting_plane(surface_array);
            return splitting_parameters;
        }

        [Flags]
        public enum Plane_Relationship : int
        {
            Unknown = 0,
            BackofPlane = 1,
            FrontofPlane = 2,
            BothSidesofPlane = 3, //both 1 & 2 
            OnPlane = 4
        }

        public Plane_Relationship determine_vertex_plane_relationship(RealPoint3d vertex, RealPlane3d plane)
        {
            float plane_equation_vertex_input = vertex.X * plane.I + vertex.Y * plane.J + vertex.Z * plane.K - plane.D;

            if (plane_equation_vertex_input >= -0.00024414062)
            {
                //if the plane distance is within both of these bounds, it is considered ON the plane
                if (plane_equation_vertex_input <= 0.00024414062)
                    return Plane_Relationship.OnPlane;
                else
                    return Plane_Relationship.FrontofPlane;
            }
            else
            {
                return Plane_Relationship.BackofPlane;
            }
        }

        public RealPoint2d vertex_get_projection_relevant_coords(Vertex vertex_block, int plane_projection_axis, int plane_mirror_check)
        {
            RealPoint2d relevant_coords = new RealPoint2d();
            int v4 = 2 * (plane_mirror_check + 2 * plane_projection_axis);
            List<int> coordinate_list = new List<int>{ 2, 1, 1, 2, 0, 2, 2, 0, 1, 0, 0, 1 };
            int vertex_coord_A = coordinate_list[v4+1];
            int vertex_coord_B = coordinate_list[v4];
            switch (vertex_coord_A)
            {
                case 0:
                    relevant_coords.X = vertex_block.Point.X;
                    break;
                case 1:
                    relevant_coords.X = vertex_block.Point.Y;
                    break;
                case 2:
                    relevant_coords.X = vertex_block.Point.Z;
                    break;
            }
            switch (vertex_coord_B)
            {
                case 0:
                    relevant_coords.Y = vertex_block.Point.X;
                    break;
                case 1:
                    relevant_coords.Y = vertex_block.Point.Y;
                    break;
                case 2:
                    relevant_coords.Y = vertex_block.Point.Z;
                    break;
            }
            return relevant_coords;
        }

        public Plane_Relationship determine_surface_plane_relationship_2D(int surface_index, Bsp2dNode bsp2dnodeblock, int plane_projection_axis, int plane_mirror_check)
        {
            Plane_Relationship surface_plane_relationship = 0;
            Surface surface_block = Bsp.Surfaces[surface_index];

            int surface_edge_index = surface_block.FirstEdge;
            while (true)
            {
                Edge surface_edge_block = Bsp.Edges[surface_edge_index];
                Vertex edge_vertex;
                if (surface_edge_block.RightSurface == surface_index)
                    edge_vertex = Bsp.Vertices[surface_edge_block.EndVertex];
                else
                    edge_vertex = Bsp.Vertices[surface_edge_block.StartVertex];

                RealPoint2d relevant_coords = vertex_get_projection_relevant_coords(edge_vertex, plane_projection_axis, plane_mirror_check);
                float plane_equation_vertex_input = bsp2dnodeblock.Plane.I * relevant_coords.X + bsp2dnodeblock.Plane.J * relevant_coords.Y - bsp2dnodeblock.Plane.D;

                if (plane_equation_vertex_input >= -0.00024414062)
                {
                    surface_plane_relationship |= Plane_Relationship.FrontofPlane;
                }
                if (plane_equation_vertex_input <= 0.00024414062)
                {
                    surface_plane_relationship |= Plane_Relationship.BackofPlane;
                }

                if (surface_plane_relationship.HasFlag(Plane_Relationship.BothSidesofPlane))
                    break;

                if (surface_edge_block.RightSurface == surface_index)
                    surface_edge_index = surface_edge_block.ReverseEdge;
                else
                    surface_edge_index = surface_edge_block.ForwardEdge;
                //break the loop if we have finished circulating the surface
                if (surface_edge_index == surface_block.FirstEdge)
                    break;
            }

            //if it fits both of these parameters, we consider the point to be on the plane
            if (surface_plane_relationship.HasFlag(Plane_Relationship.BothSidesofPlane))
                return surface_plane_relationship;

            Console.WriteLine("### WARNING found possible T-junction.");
            return surface_plane_relationship;
        }

        public Plane_Relationship determine_surface_plane_relationship(int surface_index, int plane_index, RealPlane3d plane_block)
        {
            Plane_Relationship surface_plane_relationship = 0;
            Surface surface_block = Bsp.Surfaces[surface_index];

            //check if surface is on the plane
            if (((int)surface_block.Plane & 0x7FFFFFFF) == plane_index)
                return Plane_Relationship.OnPlane;

            //if plane block is empty, use the plane index to get one instead
            if (plane_block == new RealPlane3d())
                plane_block = Bsp.Planes[plane_index].Value;

            int surface_edge_index = surface_block.FirstEdge;
            while (true)
            {
                Edge surface_edge_block = Bsp.Edges[surface_edge_index];
                RealPoint3d edge_vertex;
                if (surface_edge_block.RightSurface == surface_index)
                    edge_vertex = Bsp.Vertices[surface_edge_block.EndVertex].Point;
                else
                    edge_vertex = Bsp.Vertices[surface_edge_block.StartVertex].Point;

                Plane_Relationship vertex_plane_relationship = determine_vertex_plane_relationship(edge_vertex, plane_block);
                //if a vertex is on the plane, just ignore it and continue testing the remainder
                if (!vertex_plane_relationship.HasFlag(Plane_Relationship.OnPlane))
                    surface_plane_relationship |= vertex_plane_relationship;

                if (surface_edge_block.RightSurface == surface_index)
                    surface_edge_index = surface_edge_block.ReverseEdge;
                else
                    surface_edge_index = surface_edge_block.ForwardEdge;
                //break the loop if we have finished circulating the surface
                if (surface_edge_index == surface_block.FirstEdge)
                    break;
            }

            //there is code here to deal with unclear results, but it appears that it will lead to an infinite loop
            /*
            if (surface_vertex_plane_relationship == Plane_Relationship.Unknown || 
                surface_vertex_plane_relationship.HasFlag(Plane_Relationship.SurfaceFrontofPlane) && surface_vertex_plane_relationship.HasFlag(Plane_Relationship.SurfaceBackofPlane))
            {
                surface_vertex_plane_relationship = determine_Plane_Relationship(surface_index, -1, plane_block);
            }
            */

            return surface_plane_relationship;
        }

        public class plane_splitting_parameters
        {
            public double plane_splitting_effectiveness;
            public int plane_index;
            public int BackSurfaceCount;
            public int BackSurfaceUsedCount;
            public int FrontSurfaceCount;
            public int FrontSurfaceUsedCount;
        }

        public class surface_array_definition
        {
            public int free_count;
            public int used_count;
            public List<int> surface_array;
        }

        public plane_splitting_parameters determine_plane_splitting_effectiveness_2D(Bsp2dNode bsp2dnode_block, int plane_projection_axis, int a3, surface_array_definition plane_matched_surface_array)
        {
            plane_splitting_parameters splitting_Parameters = new plane_splitting_parameters();
            foreach(int surface_index in plane_matched_surface_array.surface_array)
            {
                Plane_Relationship surface_plane_orientation = determine_surface_plane_relationship_2D(surface_index, bsp2dnode_block, plane_projection_axis, a3);
                if (surface_plane_orientation.HasFlag(Plane_Relationship.BackofPlane))
                    splitting_Parameters.BackSurfaceCount++;
                if (surface_plane_orientation.HasFlag(Plane_Relationship.FrontofPlane))
                    splitting_Parameters.FrontSurfaceCount++;
            }
            splitting_Parameters.plane_splitting_effectiveness =
                Math.Abs((splitting_Parameters.BackSurfaceCount - splitting_Parameters.FrontSurfaceCount) + 2 * (splitting_Parameters.FrontSurfaceCount + splitting_Parameters.BackSurfaceCount));

            return splitting_Parameters;
        }

        public plane_splitting_parameters determine_plane_splitting_effectiveness(surface_array_definition surface_array, int plane_index, RealPlane3d plane_block)
        {
            plane_splitting_parameters splitting_Parameters = new plane_splitting_parameters();
            if (surface_array.free_count + surface_array.used_count > 0)
            {
                int current_surface_array_index = 0;
                while (true)
                {
                    int surface_index = surface_array.surface_array[current_surface_array_index];
                    bool surface_is_free = current_surface_array_index < surface_array.free_count;
                    Plane_Relationship relationship = determine_surface_plane_relationship((surface_index & 0x7FFFFFFF), plane_index, plane_block);
                    if (relationship.HasFlag(Plane_Relationship.OnPlane))
                    {
                        if(surface_index < 0)
                        {
                            Surface surface_block = Bsp.Surfaces[surface_index & 0x7FFFFFFF];
                            if (surface_block.Flags.HasFlag(SurfaceFlags.TwoSided))
                            {
                                if(surface_block.Plane < 0)
                                {
                                    splitting_Parameters.BackSurfaceUsedCount++;
                                    if (++current_surface_array_index >= surface_array.free_count + surface_array.used_count)
                                        break;
                                    continue;
                                }
                            }
                            else
                            {
                                splitting_Parameters.BackSurfaceUsedCount++;
                            }
                            splitting_Parameters.FrontSurfaceUsedCount++;
                            if (++current_surface_array_index >= surface_array.free_count + surface_array.used_count)
                                break;
                            continue;
                        }
                    }
                    else
                    {
                        //if surface seems to be on neither side of the plane, consider it to be on both
                        if (!relationship.HasFlag(Plane_Relationship.BackofPlane) && !relationship.HasFlag(Plane_Relationship.FrontofPlane))
                        {
                            relationship |= Plane_Relationship.FrontofPlane;
                            relationship |= Plane_Relationship.BackofPlane;
                        }
                        if (surface_is_free)
                        {
                            if (relationship.HasFlag(Plane_Relationship.BackofPlane))
                                splitting_Parameters.BackSurfaceCount++;
                            if (relationship.HasFlag(Plane_Relationship.FrontofPlane))
                                splitting_Parameters.FrontSurfaceCount++;
                        }
                        else
                        {
                            if (relationship.HasFlag(Plane_Relationship.BackofPlane))
                                splitting_Parameters.BackSurfaceUsedCount++;
                            if (relationship.HasFlag(Plane_Relationship.FrontofPlane))
                                splitting_Parameters.FrontSurfaceUsedCount++;
                        }
                    }

                    if (++current_surface_array_index >= surface_array.free_count + surface_array.used_count)
                        break;
                }
            }

            splitting_Parameters.plane_splitting_effectiveness =
                Math.Abs((splitting_Parameters.BackSurfaceCount - splitting_Parameters.FrontSurfaceCount) + 2 * (splitting_Parameters.FrontSurfaceCount + splitting_Parameters.BackSurfaceCount));
            return splitting_Parameters;
        }

        public Bsp2dNode generate_bsp2d_plane_parameters(RealPoint2d Coords1, RealPoint2d Coords2)
        {
            Bsp2dNode bsp2dnode_block = new Bsp2dNode();
            bsp2dnode_block.Plane.I = Coords2.Y - Coords1.Y;
            bsp2dnode_block.Plane.J = Coords1.X - Coords2.X;

            float vertex_plane_vector_dot = (float)Math.Sqrt(bsp2dnode_block.Plane.I * bsp2dnode_block.Plane.I + (Coords1.X - Coords2.X) * (Coords1.X - Coords2.X));

            if(Math.Abs(vertex_plane_vector_dot) >= 0.00009999999747378752)
            {
                bsp2dnode_block.Plane.I = ((1.0f / vertex_plane_vector_dot) * bsp2dnode_block.Plane.I);
                bsp2dnode_block.Plane.J = ((1.0f / vertex_plane_vector_dot) * bsp2dnode_block.Plane.J);
                bsp2dnode_block.Plane.D = ((1.0f / vertex_plane_vector_dot) * bsp2dnode_block.Plane.J) * Coords1.Y + ((1.0f / vertex_plane_vector_dot) * bsp2dnode_block.Plane.I) * Coords1.X;
            }
            else
            {
                bsp2dnode_block.Plane.D = 0;
            }
            return bsp2dnode_block;
        }

        public plane_splitting_parameters generate_best_splitting_plane_2D(int plane_projection_axis, int plane_mirror_check, ref Bsp2dNode bsp2dnode_block, surface_array_definition plane_matched_surface_array)
        {
            plane_splitting_parameters lowest_plane_splitting_parameters = new plane_splitting_parameters();
            lowest_plane_splitting_parameters.plane_splitting_effectiveness = double.MaxValue;

            foreach(int surface_index in plane_matched_surface_array.surface_array)
            {
                Surface surface_block = Bsp.Surfaces[surface_index & 0x7FFFFFFF];
                int surface_edge_index = surface_block.FirstEdge;
                while (true)
                {
                    Edge surface_edge_block = Bsp.Edges[surface_edge_index];
                    Vertex start_vertex = Bsp.Vertices[surface_edge_block.StartVertex];
                    Vertex end_vertex = Bsp.Vertices[surface_edge_block.EndVertex];
                    RealPoint2d Coords1;
                    RealPoint2d Coords2;
                    if (surface_edge_block.RightSurface == surface_index)
                    {
                        Coords1 = vertex_get_projection_relevant_coords(end_vertex, plane_projection_axis, plane_mirror_check);
                        Coords2 = vertex_get_projection_relevant_coords(start_vertex, plane_projection_axis, plane_mirror_check);
                    }
                    else
                    {
                        Coords2 = vertex_get_projection_relevant_coords(end_vertex, plane_projection_axis, plane_mirror_check);
                        Coords1 = vertex_get_projection_relevant_coords(start_vertex, plane_projection_axis, plane_mirror_check);
                    }

                    Bsp2dNode current_bsp2dnode_block = generate_bsp2d_plane_parameters(Coords1, Coords2);

                    plane_splitting_parameters current_plane_splitting_parameters = determine_plane_splitting_effectiveness_2D(current_bsp2dnode_block, plane_projection_axis, plane_mirror_check, plane_matched_surface_array);

                    if(current_plane_splitting_parameters.plane_splitting_effectiveness < lowest_plane_splitting_parameters.plane_splitting_effectiveness)
                    {
                        lowest_plane_splitting_parameters = current_plane_splitting_parameters;
                        bsp2dnode_block = current_bsp2dnode_block;
                    }

                    if (surface_edge_block.RightSurface == surface_index)
                        surface_edge_index = surface_edge_block.ReverseEdge;
                    else
                        surface_edge_index = surface_edge_block.ForwardEdge;
                    //break the loop if we have finished circulating the surface
                    if (surface_edge_index == surface_block.FirstEdge)
                        break;
                }
            }
            return lowest_plane_splitting_parameters;
        }

        public plane_splitting_parameters surface_plane_splitting_effectiveness_check_loop(surface_array_definition surface_array)
        {
            plane_splitting_parameters lowest_plane_splitting_parameters = new plane_splitting_parameters();
            lowest_plane_splitting_parameters.plane_splitting_effectiveness = double.MaxValue;

            //loop through free surfaces to see how effectively their associated planes split the remaining surfaces. Find the one that most effectively splits the remaining surfaces.
            for (int i = 0; i < surface_array.free_count; i++)
            {
                int current_plane_index = Bsp.Surfaces[(surface_array.surface_array[i] & 0x7FFFFFFF)].Plane & 0x7FFFFFFF;
                plane_splitting_parameters current_plane_splitting_parameters = determine_plane_splitting_effectiveness(surface_array, current_plane_index, new RealPlane3d());
                if(current_plane_splitting_parameters.plane_splitting_effectiveness < lowest_plane_splitting_parameters.plane_splitting_effectiveness)
                {
                    lowest_plane_splitting_parameters = current_plane_splitting_parameters;
                    lowest_plane_splitting_parameters.plane_index = current_plane_index;
                }
            }

            return lowest_plane_splitting_parameters;
        }

        public class surface_array_qsort_compar: IComparer<extent_entry>
        {
            public int Compare(extent_entry element1, extent_entry element2)
            {
                if (element1.coordinate < element2.coordinate)
                    return -1;
                if (element1.coordinate > element2.coordinate)
                    return 1;
                if (element2.is_max_coord && !element1.is_max_coord)
                    return -1;
                if (element1.is_max_coord && !element2.is_max_coord)
                    return 1;
                return 0;
            }
        }

        public class extent_entry
        {
            public float coordinate;
            public bool is_max_coord;
        };

        public plane_splitting_parameters generate_object_splitting_plane(surface_array_definition surface_array)
        {
            plane_splitting_parameters lowest_plane_splitting_parameters = new plane_splitting_parameters();
            lowest_plane_splitting_parameters.plane_splitting_effectiveness = double.MaxValue;

            int best_axis_index = 0;
            double desired_plane_distance = 0;

            //check X, Y, and Z axes
            for (int current_test_axis = 0; current_test_axis < 3; current_test_axis++)
            {
                //generate a list of maximum and minimum coordinates on the specified plane for each surface
                List<extent_entry> extents_table = new List<extent_entry>();
                for (short i = 0; i < surface_array.free_count; i++)
                {
                    extent_entry min_coordinate = new extent_entry();
                    extent_entry max_coordinate = new extent_entry();
                    max_coordinate.is_max_coord = true;
                    int surface_index = surface_array.surface_array[i] & 0x7FFF;
                    Surface surface_block = Bsp.Surfaces[surface_index];
                    int surface_edge_index = surface_block.FirstEdge;
                    while (true)
                    {
                        Edge surface_edge_block = Bsp.Edges[surface_edge_index];
                        RealPoint3d edge_vertex;
                        if (surface_edge_block.RightSurface == surface_index)
                            edge_vertex = Bsp.Vertices[surface_edge_block.EndVertex].Point;
                        else
                            edge_vertex = Bsp.Vertices[surface_edge_block.StartVertex].Point;
                        switch (current_test_axis)
                        {
                            case 0: //X axis
                                if (min_coordinate.coordinate > edge_vertex.X)
                                    min_coordinate.coordinate = edge_vertex.X;
                                if (max_coordinate.coordinate < edge_vertex.X)
                                    max_coordinate.coordinate = edge_vertex.X;
                                break;
                            case 1: //Y axis
                                if (min_coordinate.coordinate > edge_vertex.Y)
                                    min_coordinate.coordinate = edge_vertex.Y;
                                if (max_coordinate.coordinate < edge_vertex.Y)
                                    max_coordinate.coordinate = edge_vertex.Y;
                                break;
                            case 2: //Z axis
                                if (min_coordinate.coordinate > edge_vertex.Z)
                                    min_coordinate.coordinate = edge_vertex.Z;
                                if (max_coordinate.coordinate < edge_vertex.Z)
                                    max_coordinate.coordinate = edge_vertex.Z;
                                break;
                        }

                        if (surface_edge_block.RightSurface == surface_index)
                            surface_edge_index = surface_edge_block.ReverseEdge;
                        else
                            surface_edge_index = surface_edge_block.ForwardEdge;
                        //break the loop if we have finished circulating the surface
                        if (surface_edge_index == surface_block.FirstEdge)
                            break;
                    }
                    extents_table.Add(min_coordinate);
                    extents_table.Add(max_coordinate);
                }
                //use the above defined comparer to sort the extent entries first by coordinate and then by whether they are a surface max coordinate or not
                surface_array_qsort_compar sorter = new surface_array_qsort_compar();
                extents_table.Sort(sorter);

                int front_count = surface_array.free_count;
                int back_count = 0;
                for (int extent_index = 0; extent_index < extents_table.Count; extent_index++)
                {
                    if (extents_table[extent_index].is_max_coord)
                    {
                        front_count--;
                        back_count++;
                    }
                    double current_splitting_effectiveness = Math.Abs((back_count - front_count) + 2 * (front_count + back_count));
                    if (current_splitting_effectiveness < lowest_plane_splitting_parameters.plane_splitting_effectiveness)
                    {
                        lowest_plane_splitting_parameters.plane_splitting_effectiveness = current_splitting_effectiveness;
                        best_axis_index = current_test_axis;
                        desired_plane_distance = (extents_table[extent_index].coordinate + extents_table[extent_index - 1].coordinate) * 0.5;
                    }
                }
            }
            //generate a plane based on the ideal plane characteristics calculated above
            RealPlane3d best_plane = new RealPlane3d();
            best_plane.D = (float)desired_plane_distance;
            switch (best_axis_index)
            {
                case 0:
                    best_plane.I = 1.0f;
                    break;
                case 1:
                    best_plane.J = 1.0f;
                    break;
                case 2:
                    best_plane.K = 1.0f;
                    break;
            }
            //an identical but opposite facing plane can also be used
            RealPlane3d inverse_best_plane = new RealPlane3d
            {
                I = best_plane.I * -1,
                J = best_plane.J * -1,
                K = best_plane.K * -1,
                D = best_plane.D * -1,
            };
            //we can use an existing plane if it is within this tolerance threshold 
            double plane_tolerance = 0.001000000047497451;
            int ideal_plane_index;
            for(ideal_plane_index = 0; ideal_plane_index < Bsp.Planes.Count; ideal_plane_index++)
            {
                var plane = Bsp.Planes[ideal_plane_index].Value;
                if(Math.Abs(plane.I - best_plane.I) < plane_tolerance &&
                    Math.Abs(plane.J - best_plane.J) < plane_tolerance &&
                    Math.Abs(plane.K - best_plane.K) < plane_tolerance &&
                    Math.Abs(plane.D - best_plane.D) < plane_tolerance)
                {
                    break;
                }
                else if (Math.Abs(plane.I - inverse_best_plane.I) < plane_tolerance &&
                    Math.Abs(plane.J - inverse_best_plane.J) < plane_tolerance &&
                    Math.Abs(plane.K - inverse_best_plane.K) < plane_tolerance &&
                    Math.Abs(plane.D - inverse_best_plane.D) < plane_tolerance)
                {
                    break;
                }
            }
            //if we have found a usable existing plane, use it
            if(ideal_plane_index < Bsp.Planes.Count)
            {
                lowest_plane_splitting_parameters = determine_plane_splitting_effectiveness(surface_array, ideal_plane_index, new RealPlane3d());
            }
            //otherwise just add a new plane with ideal characteristics
            else
            {
                Bsp.Planes.Add(new Plane { Value = best_plane });
                ideal_plane_index = Bsp.Planes.Count - 1;
                lowest_plane_splitting_parameters = determine_plane_splitting_effectiveness(surface_array, ideal_plane_index, new RealPlane3d());
            }
            return lowest_plane_splitting_parameters;
        }
    }
}