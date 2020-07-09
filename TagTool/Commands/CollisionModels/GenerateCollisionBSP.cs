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
            Bsp.Leaves.Clear();
            Bsp.Bsp2dNodes.Clear();
            Bsp.Bsp2dReferences.Clear();
            Bsp.Bsp3dNodes.Clear();

            return true;
        }

        [Flags]
        public enum Surface_Plane_Relationship : int
        {
            Unknown = 0,
            SurfaceFrontofPlane = 1 << 0,
            SurfaceBackofPlane = 1 << 1,
            SurfaceOnPlane = 1 << 2
        }

        private Surface_Plane_Relationship determine_surface_plane_relationship(int surface_index, int plane_index, RealPlane3d plane_block)
        {
            Surface_Plane_Relationship surface_vertex_plane_relationship = 0;
            Surface surface_block = Bsp.Surfaces[surface_index];

            //check if surface is on the plane
            if ((surface_block.Plane & (short)0x7FFF) == plane_index)
                return Surface_Plane_Relationship.SurfaceOnPlane;

            //if plane block is empty, use the plane index to get one instead
            if (plane_block == new RealPlane3d())
                plane_block = Bsp.Planes[plane_index].Value;

            int surface_edge_index = surface_block.FirstEdge;
            while(true)
            {
                Edge surface_edge_block = Bsp.Edges[surface_edge_index];
                RealPoint3d edge_vertex;
                if (surface_edge_block.RightSurface == surface_index)
                    edge_vertex = Bsp.Vertices[surface_edge_block.EndVertex].Point;
                else
                    edge_vertex = Bsp.Vertices[surface_edge_block.StartVertex].Point;
                float plane_equation_vertex_input = edge_vertex.X * plane_block.I + edge_vertex.Y * plane_block.J + edge_vertex.Z * plane_block.K - plane_block.D;

                if (plane_equation_vertex_input >= -0.00024414062)
                {
                    //if the plane equation vertex input is within these tolerances, it is considered to be on the plane 
                    if (plane_equation_vertex_input <= 0.00024414062)
                    {
                        if (surface_edge_block.RightSurface == surface_index)
                            surface_edge_index = surface_edge_block.ReverseEdge;
                        else
                            surface_edge_index = surface_edge_block.ForwardEdge;
                        //break the loop if we have finished circulating the surface
                        if (surface_edge_index == surface_block.FirstEdge)
                            break;
                        continue;
                    }
                    else
                        surface_vertex_plane_relationship |= Surface_Plane_Relationship.SurfaceBackofPlane;
                }
                else
                    surface_vertex_plane_relationship |= Surface_Plane_Relationship.SurfaceFrontofPlane;

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
            if (surface_vertex_plane_relationship == Surface_Plane_Relationship.Unknown || 
                surface_vertex_plane_relationship.HasFlag(Surface_Plane_Relationship.SurfaceFrontofPlane) && surface_vertex_plane_relationship.HasFlag(Surface_Plane_Relationship.SurfaceBackofPlane))
            {
                surface_vertex_plane_relationship = determine_surface_plane_relationship(surface_index, -1, plane_block);
            }
            */

            return surface_vertex_plane_relationship;
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
                    Surface_Plane_Relationship relationship = determine_surface_plane_relationship((surface_index & (short)0x7FFF), plane_index, plane_block);
                    if (relationship.HasFlag(Surface_Plane_Relationship.SurfaceOnPlane))
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
                        if (!relationship.HasFlag(Surface_Plane_Relationship.SurfaceBackofPlane) && !relationship.HasFlag(Surface_Plane_Relationship.SurfaceFrontofPlane))
                        {
                            relationship |= Surface_Plane_Relationship.SurfaceFrontofPlane;
                            relationship |= Surface_Plane_Relationship.SurfaceBackofPlane;
                        }
                        if (surface_is_free)
                        {
                            if (relationship.HasFlag(Surface_Plane_Relationship.SurfaceFrontofPlane))
                                splitting_Parameters.BackSurfaceCount++;
                            if (relationship.HasFlag(Surface_Plane_Relationship.SurfaceBackofPlane))
                                splitting_Parameters.FrontSurfaceCount++;
                        }
                        else
                        {
                            if (relationship.HasFlag(Surface_Plane_Relationship.SurfaceFrontofPlane))
                                splitting_Parameters.BackSurfaceUsedCount++;
                            if (relationship.HasFlag(Surface_Plane_Relationship.SurfaceBackofPlane))
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