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
            Bsp = definition.Regions[0].Permutations[0].Bsps[0].Geometry;
        }

        public override object Execute(List<string> args)
        {
            //make sure there is nothing in the bsp blocks before starting
            Bsp.Leaves.Clear();
            Bsp.Bsp2dNodes.Clear();
            Bsp.Bsp2dReferences.Clear();
            Bsp.Bsp3dNodes.Clear();

            //allocate surface array before starting the bsp build
            surface_array_definition surface_array = new surface_array_definition { free_count = Bsp.Surfaces.Count, used_count = 0, surface_array = new List<short>()};
            //run build_bsp_tree_main here
            //NOTE: standard limit for number of bsp3dnodes (bsp planes) is 128 (maximum bsp depth)

            return true;
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

        public void split_object_surfaces_with_plane(surface_array_definition surface_array, short plane_index, ref surface_array_definition back_surfaces_array, ref surface_array_definition front_surfaces_array)
        {
            int surface_array_index = 0;
            while (true)
            {
                short surface_index = surface_array.surface_array[surface_array_index];
                bool surface_is_double_sided = Bsp.Surfaces[surface_index].Flags.HasFlag(SurfaceFlags.TwoSided);
                bool surface_is_mirrored = false;
                if (surface_index < 0)
                    surface_is_mirrored = true;
                surface_index &= 0x7FFF;
                bool surface_is_free = surface_array_index < surface_array.free_count;
                switch(determine_surface_plane_relationship(surface_index, plane_index, new RealPlane3d()))
                {
                    case Plane_Relationship.Unknown: //surface does not appear to be on either side of the plane and is not on the plane either
                        if(surface_index != -1)
                        {
                            if (surface_is_free)
                            {
                                //free surfaces need to come first in the list
                                back_surfaces_array.surface_array.Insert(0, surface_index);
                                back_surfaces_array.free_count++;
                                front_surfaces_array.surface_array.Insert(0, surface_index);
                                front_surfaces_array.free_count++;
                            }
                            else
                            {
                                back_surfaces_array.surface_array.Add(surface_index);
                                back_surfaces_array.used_count++;
                                front_surfaces_array.surface_array.Add(surface_index);
                                front_surfaces_array.used_count++;
                            }
                        }
                        break;
                    case Plane_Relationship.BackofPlane: //surface is in back of plane
                        if (surface_index != -1)
                        {
                            if (surface_is_free)
                            {
                                //free surfaces need to come first in the list
                                back_surfaces_array.surface_array.Insert(0, surface_index);
                                back_surfaces_array.free_count++;
                            }
                            else
                            {
                                back_surfaces_array.surface_array.Add(surface_index);
                                back_surfaces_array.used_count++;
                            }
                        }
                        break;
                    case Plane_Relationship.FrontofPlane: //surface is in front of plane
                        if (surface_index != -1)
                        {
                            if (surface_is_free)
                            {
                                //free surfaces need to come first in the list
                                front_surfaces_array.surface_array.Insert(0, surface_index);
                                front_surfaces_array.free_count++;
                            }
                            else
                            {
                                front_surfaces_array.surface_array.Add(surface_index);
                                front_surfaces_array.used_count++;
                            }
                        }
                        break;
                    case Plane_Relationship.BothSidesofPlane: //surface is both in front of and behind plane
                        Bsp.Surfaces.Add(new Surface());
                        Bsp.Surfaces.Add(new Surface());
                        int new_surface_A_index = Bsp.Surfaces.Count - 2;
                        int new_surface_B_index = Bsp.Surfaces.Count - 1;

                        //split surface into two new surfaces, one on each side of the plane
                        divide_surface_into_two_surfaces(surface_index, plane_index, ref new_surface_A_index, ref new_surface_B_index);

                        //propagate surface index flags to child surfaces
                        uint back_surface_index;
                        uint front_surface_index;
                        if (surface_is_mirrored)
                        {
                            back_surface_index = (uint)new_surface_A_index | 0x80000000;
                            front_surface_index = (uint)new_surface_B_index | 0x80000000;
                        }
                        else
                        {
                            back_surface_index = (uint)new_surface_A_index & 0x7FFFFFFF;
                            front_surface_index = (uint)new_surface_B_index & 0x7FFFFFFF;
                        }

                        //add child surfaces to appropriate arrays
                        if (surface_is_free)
                        {
                            //free surfaces need to come first in the list
                            back_surfaces_array.surface_array.Insert(0, (short)back_surface_index);
                            back_surfaces_array.free_count++;
                            front_surfaces_array.surface_array.Insert(0, (short)front_surface_index);
                            front_surfaces_array.free_count++;
                        }
                        else
                        {
                            back_surfaces_array.surface_array.Add((short)back_surface_index);
                            back_surfaces_array.used_count++;
                            front_surfaces_array.surface_array.Add((short)front_surface_index);
                            front_surfaces_array.used_count++;
                        }
                        break;
                    case Plane_Relationship.OnPlane: //surface is ON the plane
                        if (!surface_is_mirrored)
                        {

                        }
                        if (surface_is_double_sided)
                        {
                            if (!surface_is_mirrored)
                            {
                                back_surface_index = (uint)surface_index & 0x7FFFFFFF;
                            }
                            else
                            {
                                back_surface_index = (uint)surface_index | 0x80000000;
                            }

                        }
                        else
                        {
                            if (!surface_is_mirrored)
                            {
                                front_surface_index = (uint)surface_index & 0x7FFFFFFF;
                            }
                            else
                                back_surface_index = (uint)surface_index | 0x80000000;
                        }
                        if (surface_is_double_sided)
                        {

                        }
                        else
                        {

                        }
                        break;
                }

                surface_array_index++;
            }
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
            Plane_Relationship vertex_plane_relationship = new Plane_Relationship();

            float plane_equation_vertex_input = vertex.X * plane.I + vertex.Y * plane.J + vertex.Z * plane.K - plane.D;

            if (plane_equation_vertex_input >= -0.00024414062)
            {
                vertex_plane_relationship |= Plane_Relationship.FrontofPlane;
            }
            if (plane_equation_vertex_input <= 0.00024414062)
            {
                vertex_plane_relationship |= Plane_Relationship.BackofPlane;
            }
            //if it fits both of these parameters, we consider the point to be on the plane
            if (vertex_plane_relationship.HasFlag(Plane_Relationship.BothSidesofPlane))
                vertex_plane_relationship = Plane_Relationship.OnPlane;

            return vertex_plane_relationship;
        }

        public Plane_Relationship determine_surface_plane_relationship(int surface_index, int plane_index, RealPlane3d plane_block)
        {
            Plane_Relationship surface_plane_relationship = 0;
            Surface surface_block = Bsp.Surfaces[surface_index];

            //check if surface is on the plane
            if ((surface_block.Plane & (short)0x7FFF) == plane_index)
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
            public List<short> surface_array;
        }

        private plane_splitting_parameters determine_plane_splitting_effectiveness(surface_array_definition surface_array, int plane_index, RealPlane3d plane_block, plane_splitting_parameters splitting_Parameters)
        {
            if(surface_array.free_count + surface_array.used_count > 0)
            {
                int current_surface_array_index = 0;
                while (true)
                {
                    short surface_index = surface_array.surface_array[current_surface_array_index];
                    bool surface_is_free = current_surface_array_index < surface_array.free_count;
                    Plane_Relationship relationship = determine_surface_plane_relationship((surface_index & (short)0x7FFF), plane_index, plane_block);
                    if (relationship.HasFlag(Plane_Relationship.OnPlane))
                    {
                        if(surface_index < 0)
                        {
                            Surface surface_block = Bsp.Surfaces[surface_index];
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
                            if (relationship.HasFlag(Plane_Relationship.FrontofPlane))
                                splitting_Parameters.BackSurfaceCount++;
                            if (relationship.HasFlag(Plane_Relationship.BackofPlane))
                                splitting_Parameters.FrontSurfaceCount++;
                        }
                        else
                        {
                            if (relationship.HasFlag(Plane_Relationship.FrontofPlane))
                                splitting_Parameters.BackSurfaceUsedCount++;
                            if (relationship.HasFlag(Plane_Relationship.BackofPlane))
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

        public plane_splitting_parameters surface_plane_splitting_effectiveness_check_loop(surface_array_definition surface_array)
        {
            plane_splitting_parameters lowest_plane_splitting_parameters = new plane_splitting_parameters();
            lowest_plane_splitting_parameters.plane_splitting_effectiveness = double.MaxValue;

            //loop through free surfaces to see how effectively their associated planes split the remaining surfaces. Find the one that most effectively splits the remaining surfaces.
            for (short i = 0; i < surface_array.free_count; i++)
            {
                int current_plane_index = Bsp.Surfaces[(surface_array.surface_array[i] & 0x7FFF)].Plane & 0x7FFFFFFF;
                plane_splitting_parameters current_plane_splitting_parameters = determine_plane_splitting_effectiveness(surface_array, current_plane_index, new RealPlane3d(), new plane_splitting_parameters());
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
                lowest_plane_splitting_parameters = determine_plane_splitting_effectiveness(surface_array, ideal_plane_index, new RealPlane3d(), new plane_splitting_parameters());
            }
            //otherwise just add a new plane with ideal characteristics
            else
            {
                Bsp.Planes.Add(new Plane { Value = best_plane });
                ideal_plane_index = Bsp.Planes.Count - 1;
                lowest_plane_splitting_parameters = determine_plane_splitting_effectiveness(surface_array, ideal_plane_index, new RealPlane3d(), new plane_splitting_parameters());
            }
            return lowest_plane_splitting_parameters;
        }
    }
}