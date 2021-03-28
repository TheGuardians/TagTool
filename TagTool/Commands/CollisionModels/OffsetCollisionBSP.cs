using System;
using System.Collections.Generic;
using System.IO;
using TagTool.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Tags.Definitions;
using TagTool.Tags;
using System.Diagnostics;

namespace TagTool.Commands.CollisionModels.OffsetCollisonBsp
{
    public class OffsetCollisionBSP
    {
        private CollisionGeometry Bsp { get; set; }
        private CollisionGeometry NewBsp { get; set; }
        private RealPoint3d GeometryOffset { get; set; }
        private bool debug = false;
        private int planewarnings = 0;

        public bool offset_bsp(RealPoint3d geometryoffset, ref CollisionGeometry inputbsp)
        {
            Bsp = inputbsp.DeepClone();
            NewBsp = inputbsp.DeepClone();
            GeometryOffset = geometryoffset;

            //center the offsets for the collision model
            for (var i = 0; i < NewBsp.Vertices.Count; i++)
            {
                NewBsp.Vertices[i].Point.X -= GeometryOffset.X;
                NewBsp.Vertices[i].Point.Y -= GeometryOffset.Y;
                NewBsp.Vertices[i].Point.Z -= GeometryOffset.Z;
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (recalculate_bsp_planes() && recalculate_bsp2dreferences())
            {
                verify_planes();
                verify_bsp2dreferences();
                sw.Stop();
                Console.WriteLine($"### Collision bsp offset successfully! ({planewarnings} warnings, {sw.Elapsed.TotalSeconds} seconds)");
                inputbsp = NewBsp;
                return true;
            }
            else
            {
                new TagToolError(CommandError.CustomError, "Failed to offset collision bsp!");
                return false;
            }
        }

        public bool recalculate_bsp_planes()
        {
            for(int plane_index = 0; plane_index < Bsp.Planes.Count; plane_index++)
            {
                RealPlane3d plane = Bsp.Planes[plane_index].Value;
                
                if (plane.I == 1.0f && plane.J == 0.0f && plane.K == 0.0f)
                {
                    NewBsp.Planes[plane_index].Value.D = Bsp.Planes[plane_index].Value.D - GeometryOffset.X;
                }
                else if (plane.J == 1.0f && plane.I == 0.0f && plane.K == 0.0f)
                {
                    NewBsp.Planes[plane_index].Value.D = Bsp.Planes[plane_index].Value.D - GeometryOffset.Y;
                }
                else if (plane.K == 1.0f && plane.J == 0.0f && plane.I == 0.0f)
                {
                    NewBsp.Planes[plane_index].Value.D = Bsp.Planes[plane_index].Value.D - GeometryOffset.Z;
                }
                else if (plane_regeneration_by_split_comparison(plane_index))
                {
                    continue;
                }
                else if (plane_regeneration_by_best_match(plane_index))
                {
                    continue;
                }
                else
                {
                    new TagToolError(CommandError.CustomError, $"Plane {plane_index} could not be regenerated!");
                    return false;
                }
            }
            return true;
        }

        public bool plane_regeneration_by_split_comparison(int plane_index)
        {
            int matching_vertex_index = -1;
            RealPlane3d plane = Bsp.Planes[plane_index].Value;
            for (int vertex_index = 0; vertex_index < Bsp.Vertices.Count; vertex_index++)
            {
                RealPoint3d vertex = Bsp.Vertices[vertex_index].Point;
                double plane_equation_vertex_input = vertex.X * plane.I + vertex.Y * plane.J + vertex.Z * plane.K - plane.D;

                if (plane_equation_vertex_input >= -0.00024414062 && plane_equation_vertex_input <= 0.00024414062)
                {
                    RealPoint3d newvertex = NewBsp.Vertices[vertex_index].Point;
                    float new_plane_D = plane.I * newvertex.X + plane.J * newvertex.Y + plane.K * newvertex.Z;
                    NewBsp.Planes[plane_index].Value.D = new_plane_D;
                    plane_vertex_splitting_parameters parameters = check_plane_split(plane_index);
                    if (verify_split_parameters(parameters))
                    {
                        matching_vertex_index = vertex_index;
                        break;
                    }
                }
            }

            if (matching_vertex_index == -1)
                return false;

            return true;
        }

        public bool plane_regeneration_by_best_match(int plane_index)
        {
            RealPoint3d matching_point = new RealPoint3d();
            int matching_vertex_index = -1;
            double plane_fit = double.MaxValue;
            RealPlane3d plane = Bsp.Planes[plane_index].Value;
            for (int vertex_index = 0; vertex_index < Bsp.Vertices.Count; vertex_index++)
            {
                RealPoint3d vertex = Bsp.Vertices[vertex_index].Point;
                double plane_equation_vertex_input = vertex.X * plane.I + vertex.Y * plane.J + vertex.Z * plane.K - plane.D;

                if (Math.Abs(plane_equation_vertex_input) < plane_fit)
                {
                    plane_fit = Math.Abs(plane_equation_vertex_input);
                    matching_vertex_index = vertex_index;
                    matching_point = NewBsp.Vertices[vertex_index].Point;
                }
            }

            float new_plane_D = plane.I * matching_point.X + plane.J * matching_point.Y + plane.K * matching_point.Z;
            NewBsp.Planes[plane_index].Value.D = new_plane_D;

            plane_vertex_splitting_parameters parameters = check_plane_split(plane_index);
            int loopcounter = 10000;
            byte direction_history = 0;
            float adjustment_factor = 0.1f;
            while (!verify_split_parameters(parameters))
            {
                if (parameters.new_front_count > parameters.front_count ||
                    parameters.new_back_count < parameters.back_count)
                {
                    NewBsp.Planes[plane_index].Value.D += adjustment_factor;
                    direction_history = (byte)((direction_history << 1) | (byte)0x1);
                }
                else if (parameters.new_front_count < parameters.front_count ||
                    parameters.new_back_count > parameters.back_count)
                {
                    NewBsp.Planes[plane_index].Value.D -= adjustment_factor;
                    direction_history = (byte)((direction_history << 1) & (byte)0xFE);
                }
                //direction history is a binary history of which direction the plane has been moving
                //0xA is 1010 and 0x5 is 0101, meaning the plane has been shifting back and forth, unable to properly split vertices
                //therefore, we will move to a finer adjustment for the plane
                if ((direction_history & 0xF) == 0xA ||
                    (direction_history & 0xF) == 0x5)
                    adjustment_factor /= 10.0f;
                parameters = check_plane_split(plane_index);
                if (loopcounter-- <= 0)
                    break;
            }

            return true;
        }

        public struct plane_vertex_splitting_parameters
        {
            public int front_count;
            public int back_count;
            public int onplane_count;
            public int new_front_count;
            public int new_back_count;
            public int new_onplane_count;
        }

        public bool verify_split_parameters(plane_vertex_splitting_parameters parameters)
        {
            if (parameters.back_count != parameters.new_back_count ||
                parameters.front_count != parameters.new_front_count ||
                parameters.onplane_count != parameters.new_onplane_count)
            {
                return false;
            }
            return true;
        }

        public plane_vertex_splitting_parameters check_plane_split(int plane_index)
        {
            RealPlane3d plane = Bsp.Planes[plane_index].Value;
            RealPlane3d newplane = NewBsp.Planes[plane_index].Value;
            plane_vertex_splitting_parameters parameters = new plane_vertex_splitting_parameters();
            for (int vertex_index = 0; vertex_index < Bsp.Vertices.Count; vertex_index++)
            {
                RealPoint3d vertex = Bsp.Vertices[vertex_index].Point;
                switch (determine_vertex_plane_relationship(vertex, plane))
                {
                    case Plane_Relationship.BackofPlane:
                        parameters.back_count++;
                        break;
                    case Plane_Relationship.FrontofPlane:
                        parameters.front_count++;
                        break;
                    case Plane_Relationship.OnPlane:
                        parameters.onplane_count++;
                        break;
                }

                RealPoint3d newvertex = NewBsp.Vertices[vertex_index].Point;
                switch (determine_vertex_plane_relationship(newvertex, newplane))
                {
                    case Plane_Relationship.BackofPlane:
                        parameters.new_back_count++;
                        break;
                    case Plane_Relationship.FrontofPlane:
                        parameters.new_front_count++;
                        break;
                    case Plane_Relationship.OnPlane:
                        parameters.new_onplane_count++;
                        break;
                }
            }

            return parameters;
        }

        public void verify_planes()
        {
            for (int plane_index = 0; plane_index < Bsp.Planes.Count; plane_index++)
            {
                plane_vertex_splitting_parameters parameters = check_plane_split(plane_index);
                if (!verify_split_parameters(parameters))
                {
                    if(debug)
                        Console.WriteLine($"### WARNING: Plane {plane_index} new offset does not match original!");
                    planewarnings++;
                }
            }
        }

        public plane_vertex_splitting_parameters check_bsp2dnode_split(int node_index, Plane plane_block, int plane_projection_axis, int plane_mirror_check)
        {
            RealPlane2d node = Bsp.Bsp2dNodes[node_index].Plane;
            RealPlane2d newnode = NewBsp.Bsp2dNodes[node_index].Plane;
            plane_vertex_splitting_parameters parameters = new plane_vertex_splitting_parameters();
            for (int vertex_index = 0; vertex_index < Bsp.Vertices.Count; vertex_index++)
            {
                Vertex vertex_block = Bsp.Vertices[vertex_index];
                RealPoint2d coords = vertex_get_projection_relevant_coords(vertex_block, plane_projection_axis, plane_mirror_check);
                switch (determine_vertex_plane_relationship_2D(coords, node))
                {
                    case Plane_Relationship.BackofPlane:
                        parameters.back_count++;
                        break;
                    case Plane_Relationship.FrontofPlane:
                        parameters.front_count++;
                        break;
                    case Plane_Relationship.OnPlane:
                        parameters.onplane_count++;
                        break;
                }

                Vertex new_vertex_block = NewBsp.Vertices[vertex_index];
                RealPoint2d newcoords = vertex_get_projection_relevant_coords(new_vertex_block, plane_projection_axis, plane_mirror_check);
                switch (determine_vertex_plane_relationship_2D(newcoords, newnode))
                {
                    case Plane_Relationship.BackofPlane:
                        parameters.new_back_count++;
                        break;
                    case Plane_Relationship.FrontofPlane:
                        parameters.new_front_count++;
                        break;
                    case Plane_Relationship.OnPlane:
                        parameters.new_onplane_count++;
                        break;
                }
            }
            return parameters;
        }

        public plane_vertex_splitting_parameters check_bsp2dnode_split_from_vertex_list(int node_index, Plane plane_block, int plane_projection_axis, int plane_mirror_check, List<int> plane_vertex_list)
        {
            RealPlane2d node = Bsp.Bsp2dNodes[node_index].Plane;
            RealPlane2d newnode = NewBsp.Bsp2dNodes[node_index].Plane;
            plane_vertex_splitting_parameters parameters = new plane_vertex_splitting_parameters();
            for (int vertex_index = 0; vertex_index < plane_vertex_list.Count; vertex_index++)
            {
                Vertex vertex_block = Bsp.Vertices[plane_vertex_list[vertex_index]];
                RealPoint2d coords = vertex_get_projection_relevant_coords(vertex_block, plane_projection_axis, plane_mirror_check);
                switch (determine_vertex_plane_relationship_2D(coords, node))
                {
                    case Plane_Relationship.BackofPlane:
                        parameters.back_count++;
                        break;
                    case Plane_Relationship.FrontofPlane:
                        parameters.front_count++;
                        break;
                    case Plane_Relationship.OnPlane:
                        parameters.onplane_count++;
                        break;
                }

                Vertex new_vertex_block = NewBsp.Vertices[plane_vertex_list[vertex_index]];
                RealPoint2d newcoords = vertex_get_projection_relevant_coords(new_vertex_block, plane_projection_axis, plane_mirror_check);
                switch (determine_vertex_plane_relationship_2D(newcoords, newnode))
                {
                    case Plane_Relationship.BackofPlane:
                        parameters.new_back_count++;
                        break;
                    case Plane_Relationship.FrontofPlane:
                        parameters.new_front_count++;
                        break;
                    case Plane_Relationship.OnPlane:
                        parameters.new_onplane_count++;
                        break;
                }
            }
            return parameters;
        }

        public bool recalculate_bsp2dnodes(int node_index, Plane plane_block, int plane_projection_axis, int plane_mirror_check, List<int> plane_matching_vertices)
        {
            //nodes with the 0x8000 flag are actually surface indices
            if ((node_index & 0x8000) != 0)
                return true;

            if (recalculate_bsp2dnodes_by_split_comparison(node_index, plane_block, plane_projection_axis, plane_mirror_check, plane_matching_vertices) ||
                recalculate_bsp2dnodes_by_best_match(node_index, plane_block, plane_projection_axis, plane_mirror_check, plane_matching_vertices))
            {
                int right_child_node = Bsp.Bsp2dNodes[node_index].RightChild & 0xFFFF;
                int left_child_node = Bsp.Bsp2dNodes[node_index].LeftChild & 0xFFFF;

                if (recalculate_bsp2dnodes(left_child_node, plane_block, plane_projection_axis, plane_mirror_check, plane_matching_vertices) &&
                    recalculate_bsp2dnodes(right_child_node, plane_block, plane_projection_axis, plane_mirror_check, plane_matching_vertices))
                    return true;
                return false;
            }
            else
            {
                return false;
            }
        }

        public bool recalculate_bsp2dnodes_by_split_comparison(int node_index, Plane plane_block, int plane_projection_axis, int plane_mirror_check, List<int> plane_matching_vertices)
        {
            //nodes with the 0x8000 flag are actually surface indices
            if ((node_index & 0x8000) != 0)
                return true;

            RealPlane2d plane_2d = Bsp.Bsp2dNodes[node_index].Plane;

            int vertex_index = 0;
            int matching_vertex_index = -1;
            for (vertex_index = 0; vertex_index < plane_matching_vertices.Count; vertex_index++)
            {
                Vertex vertex_block = Bsp.Vertices[plane_matching_vertices[vertex_index]];
                RealPoint3d point = vertex_block.Point;
                if (determine_vertex_plane_relationship(point, plane_block.Value) == Plane_Relationship.OnPlane)
                {
                    RealPoint2d coords = vertex_get_projection_relevant_coords(vertex_block, plane_projection_axis, plane_mirror_check);
                    double plane_2d_coord_input = plane_2d.I * coords.X + plane_2d.J * coords.Y - plane_2d.D;

                    if (plane_2d_coord_input >= -0.00012207031 && plane_2d_coord_input <= 0.00012207031)
                    {
                        Vertex new_vertex_block = NewBsp.Vertices[plane_matching_vertices[vertex_index]];
                        RealPoint2d newcoords = vertex_get_projection_relevant_coords(new_vertex_block, plane_projection_axis, plane_mirror_check);
                        float new_plane_D = plane_2d.I * newcoords.X + plane_2d.J * newcoords.Y;
                        NewBsp.Bsp2dNodes[node_index].Plane.D = new_plane_D;
                        plane_vertex_splitting_parameters parameters = check_bsp2dnode_split_from_vertex_list(node_index, plane_block, plane_projection_axis, plane_mirror_check, plane_matching_vertices);
                        if (verify_split_parameters(parameters))
                        {
                            matching_vertex_index = plane_matching_vertices[vertex_index];
                            break;
                        }
                    }
                }
            }

            //no matching vertex has been found
            if (matching_vertex_index == -1)
            {
                return false;
            }

            return true;
        }

        public bool recalculate_bsp2dnodes_by_best_match(int node_index, Plane plane_block, int plane_projection_axis, int plane_mirror_check, List<int> plane_matching_vertices)
        {
            //nodes with the 0x8000 flag are actually surface indices
            if ((node_index & 0x8000) != 0)
                return true;

            RealPlane2d plane_2d = Bsp.Bsp2dNodes[node_index].Plane;

            int vertex_index = 0;
            double planefit_2d = double.MaxValue;
            int matching_vertex_index = -1;
            for (vertex_index = 0; vertex_index < plane_matching_vertices.Count; vertex_index++)
            {
                Vertex vertex_block = Bsp.Vertices[plane_matching_vertices[vertex_index]];
                RealPoint3d point = vertex_block.Point;
                if (determine_vertex_plane_relationship(point, plane_block.Value) == Plane_Relationship.OnPlane)
                {
                    RealPoint2d coords = vertex_get_projection_relevant_coords(vertex_block, plane_projection_axis, plane_mirror_check);
                    double plane_2d_coord_input = plane_2d.I * coords.X + plane_2d.J * coords.Y - plane_2d.D;

                    if (plane_2d_coord_input >= -0.00012207031 && plane_2d_coord_input <= 0.00012207031 &&
                        Math.Abs(plane_2d_coord_input) < planefit_2d)
                    {
                        matching_vertex_index = plane_matching_vertices[vertex_index];
                        planefit_2d = Math.Abs(plane_2d_coord_input);
                    }
                }
            }

            //no matching vertex has been found
            if (matching_vertex_index == -1)
            {
                new TagToolError(CommandError.CustomError, $"Could not find a vertex to generate a 2d plane for node {node_index}");
                return false;
            }

            Vertex new_vertex_block = NewBsp.Vertices[matching_vertex_index];
            RealPoint2d newcoords = vertex_get_projection_relevant_coords(new_vertex_block, plane_projection_axis, plane_mirror_check);
            float new_plane_D = plane_2d.I * newcoords.X + plane_2d.J * newcoords.Y;
            NewBsp.Bsp2dNodes[node_index].Plane.D = new_plane_D;

            plane_vertex_splitting_parameters parameters = check_bsp2dnode_split_from_vertex_list(node_index, plane_block, plane_projection_axis, plane_mirror_check, plane_matching_vertices);
            int loopcounter = 10000;
            byte direction_history = 0;
            float adjustment_factor = 0.001f;
            while (!verify_split_parameters(parameters))
            {
                if (parameters.new_front_count > parameters.front_count ||
                    parameters.new_back_count < parameters.back_count)
                {
                    NewBsp.Bsp2dNodes[node_index].Plane.D += adjustment_factor;
                    direction_history = (byte)((direction_history << 1) | (byte)0x1);
                }
                else if (parameters.new_front_count < parameters.front_count ||
                    parameters.new_back_count > parameters.back_count)
                {
                    NewBsp.Bsp2dNodes[node_index].Plane.D -= adjustment_factor;
                    direction_history = (byte)((direction_history << 1) & (byte)0xFE);
                }
                //direction history is a binary history of which direction the plane has been moving
                //0xA is 1010 and 0x5 is 0101, meaning the plane has been shifting back and forth, unable to properly split vertices
                //therefore, we will move to a finer adjustment for the plane
                if ((direction_history & 0xF) == 0xA ||
                    (direction_history & 0xF) == 0x5)
                    adjustment_factor /= 10.0f;
                parameters = check_bsp2dnode_split(node_index, plane_block, plane_projection_axis, plane_mirror_check);
                if (loopcounter-- <= 0)
                    break;
            }

            return true;
        }

        public void verify_bsp2dreferences()
        {
            for (int bsp2dref_index = 0; bsp2dref_index < Bsp.Bsp2dReferences.Count; bsp2dref_index++)
            {
                int parent_plane = Bsp.Bsp2dReferences[bsp2dref_index].PlaneIndex;
                Plane plane_block = Bsp.Planes[parent_plane & 0x7FFF];
                int plane_projection_axis = plane_determine_axis_minimum_coefficient(plane_block);
                bool plane_projection_parameter_greater_than_0 = check_plane_projection_parameter_greater_than_0(plane_block, plane_projection_axis);

                bool plane_index_negative = parent_plane < 0;
                int plane_mirror_check = plane_projection_parameter_greater_than_0 != plane_index_negative ? 1 : 0;

                int node_index = Bsp.Bsp2dReferences[bsp2dref_index].Bsp2dNodeIndex & 0xFFFF;

                List<int> plane_matching_vertices = new List<int>();
                for (int vertex_index = 0; vertex_index < Bsp.Vertices.Count; vertex_index++)
                {
                    Vertex vertex_block = Bsp.Vertices[vertex_index];
                    RealPoint3d point = Bsp.Vertices[vertex_index].Point;
                    if (determine_vertex_plane_relationship(point, plane_block.Value) == Plane_Relationship.OnPlane)
                    {
                        plane_matching_vertices.Add(vertex_index);
                    }
                }

                verify_bsp2dnodes(node_index, plane_block, plane_projection_axis, plane_mirror_check, plane_matching_vertices);
            }
        }

        public void verify_bsp2dnodes(int node_index, Plane plane_block, int plane_projection_axis, int plane_mirror_check, List<int> plane_matching_vertices)
        {
            //nodes with the 0x8000 flag are actually surface indices
            if ((node_index & 0x8000) != 0)
                return;

            plane_vertex_splitting_parameters parameters = check_bsp2dnode_split_from_vertex_list(node_index, plane_block, plane_projection_axis, plane_mirror_check, plane_matching_vertices);
            if (!verify_split_parameters(parameters))
            {
                if (debug)
                    Console.WriteLine($"### WARNING: Bsp2dNode {node_index} new offset does not match original!");
                planewarnings++;
            }

            int right_child_node = Bsp.Bsp2dNodes[node_index].RightChild & 0xFFFF;
            int left_child_node = Bsp.Bsp2dNodes[node_index].LeftChild & 0xFFFF;

            verify_bsp2dnodes(left_child_node, plane_block, plane_projection_axis, plane_mirror_check, plane_matching_vertices);
            verify_bsp2dnodes(right_child_node, plane_block, plane_projection_axis, plane_mirror_check, plane_matching_vertices);

            return;
        }

        public bool recalculate_bsp2dreferences()
        {
            for (int bsp2dref_index = 0; bsp2dref_index < Bsp.Bsp2dReferences.Count; bsp2dref_index++)
            {
                int parent_plane = Bsp.Bsp2dReferences[bsp2dref_index].PlaneIndex;
                Plane plane_block = Bsp.Planes[parent_plane & 0x7FFF];
                int plane_projection_axis = plane_determine_axis_minimum_coefficient(plane_block);
                bool plane_projection_parameter_greater_than_0 = check_plane_projection_parameter_greater_than_0(plane_block, plane_projection_axis);

                bool plane_index_negative = parent_plane < 0;
                int plane_mirror_check = plane_projection_parameter_greater_than_0 != plane_index_negative ? 1 : 0;

                int node_index = Bsp.Bsp2dReferences[bsp2dref_index].Bsp2dNodeIndex & 0xFFFF;

                List<int> plane_matching_vertices = new List<int>();
                for (int vertex_index = 0; vertex_index < Bsp.Vertices.Count; vertex_index++)
                {
                    Vertex vertex_block = Bsp.Vertices[vertex_index];
                    RealPoint3d point = Bsp.Vertices[vertex_index].Point;
                    if (determine_vertex_plane_relationship(point, plane_block.Value) == Plane_Relationship.OnPlane)
                    {
                        plane_matching_vertices.Add(vertex_index);
                    }
                }

                if (!recalculate_bsp2dnodes(node_index, plane_block, plane_projection_axis, plane_mirror_check, plane_matching_vertices))
                {
                    new TagToolError(CommandError.CustomError, $"Could not regenerate bsp2dnode {node_index}");
                    return false;
                }
                
            }

            return true;
        }
        
        public RealPoint2d vertex_get_projection_relevant_coords(Vertex vertex_block, int plane_projection_axis, int plane_mirror_check)
        {
            //plane mirror check = "projection_sign"
            RealPoint2d relevant_coords = new RealPoint2d();
            int v4 = 2 * (plane_mirror_check + 2 * plane_projection_axis);
            List<int> coordinate_list = new List<int> { 2, 1, 1, 2, 0, 2, 2, 0, 1, 0, 0, 1 };
            int vertex_coord_A = coordinate_list[v4];
            int vertex_coord_B = coordinate_list[v4 + 1];
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
                    return plane_block.Value.I > 0.0f;
                case 1: //y axis
                    return plane_block.Value.J > 0.0f;
                case 2: //z axis
                    return plane_block.Value.K > 0.0f;
            }
            return false;
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

        public Plane_Relationship determine_vertex_plane_relationship_2D(RealPoint2d vertex, RealPlane2d plane)
        {
            float plane_equation_vertex_input = vertex.X * plane.I + vertex.Y * plane.J - plane.D;

            if (plane_equation_vertex_input >= -0.00012207031)
            {
                //if the plane distance is within both of these bounds, it is considered ON the plane
                if (plane_equation_vertex_input <= 0.00012207031)
                    return Plane_Relationship.OnPlane;
                else
                    return Plane_Relationship.FrontofPlane;
            }
            else
            {
                return Plane_Relationship.BackofPlane;
            }
        }

    }
}