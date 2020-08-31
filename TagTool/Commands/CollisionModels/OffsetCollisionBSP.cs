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

namespace TagTool.Commands.CollisionModels.OffsetCollisonBsp
{
    public class OffsetCollisionBSP
    {
        private CollisionGeometry Bsp { get; set; }
        private CollisionGeometry NewBsp { get; set; }
        private RealPoint3d GeometryOffset { get; set; }
        private bool debug = false;

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

            if (recalculate_bsp_planes() && recalculate_bsp2dreferences())
            {
                Console.WriteLine($"### Collision bsp offset successfully!");
                inputbsp = NewBsp;
                return true;
            }
            else
            {
                Console.WriteLine($"### Failed to offset collision bsp!");
                return false;
            }
        }

        public bool recalculate_bsp_planes()
        {
            for(int plane_index = 0; plane_index < Bsp.Planes.Count; plane_index++)
            {
                RealPlane3d plane = Bsp.Planes[plane_index].Value;

                if(plane_regeneration_hack(plane_index))
                {
                    return true;
                }
                else if (plane.I == 1.0f && plane.J == 0.0f && plane.K == 0.0f)
                {
                    NewBsp.Planes[plane_index].Value.D -= GeometryOffset.X;
                }
                else if (plane.J == 1.0f && plane.I == 0.0f && plane.K == 0.0f)
                {
                    NewBsp.Planes[plane_index].Value.D -= GeometryOffset.Y;
                }
                else if (plane.K == 1.0f && plane.J == 0.0f && plane.I == 0.0f)
                {
                    NewBsp.Planes[plane_index].Value.D -= GeometryOffset.Z;
                }
                else
                {
                    Console.WriteLine($"### ERROR: Plane {plane_index} could not be regenerated!");
                    return false;
                }
            }
            return true;
        }

        public bool plane_regeneration_hack(int plane_index)
        {
            RealPoint3d matching_point = new RealPoint3d();
            int matching_vertex_index = -1;
            double plane_fit = float.MaxValue;
            RealPlane3d plane = Bsp.Planes[plane_index].Value;
            for(int vertex_index = 0; vertex_index < Bsp.Vertices.Count; vertex_index++)
            {
                RealPoint3d vertex = Bsp.Vertices[vertex_index].Point;
                double plane_equation_vertex_input = vertex.X * plane.I + vertex.Y * plane.J + vertex.Z * plane.K - plane.D;

                if (plane_equation_vertex_input >= -0.00024414062 && plane_equation_vertex_input <= 0.00024414062 
                    && plane_equation_vertex_input < plane_fit)
                {
                    plane_fit = plane_equation_vertex_input;
                    matching_vertex_index = vertex_index;
                    matching_point = NewBsp.Vertices[vertex_index].Point;
                }
            }

            if (matching_vertex_index == -1)
                return false;

            float new_plane_D = plane.I * matching_point.X + plane.J * matching_point.Y + plane.K * matching_point.Z;
            NewBsp.Planes[plane_index].Value.D = new_plane_D;
            return true;
        }

        public bool recalculate_bsp2dnodes(int node_index, Plane plane_block, int plane_projection_axis, int plane_mirror_check)
        {
            //nodes with the 0x8000 flag are actually surface indices
            if ((node_index & 0x8000) != 0)
                return true;

            RealPlane2d plane_2d = Bsp.Bsp2dNodes[node_index].Plane;

            int vertex_index = 0;
            double planefit_2d = float.MaxValue;
            int matching_vertex_index = -1;
            for (vertex_index = 0; vertex_index < Bsp.Vertices.Count; vertex_index++)
            {
                Vertex vertex_block = Bsp.Vertices[vertex_index];
                RealPoint3d point = Bsp.Vertices[vertex_index].Point;
                if (determine_vertex_plane_relationship(point, plane_block.Value) == Plane_Relationship.OnPlane)
                {
                    RealPoint2d coords = vertex_get_projection_relevant_coords(vertex_block, plane_projection_axis, plane_mirror_check);
                    double plane_2d_coord_input = plane_2d.I * coords.X + plane_2d.J * coords.Y - plane_2d.D;

                    if (plane_2d_coord_input >= -0.00012207031 && plane_2d_coord_input <= 0.00012207031 &&
                        plane_2d_coord_input < planefit_2d)
                    {
                        matching_vertex_index = vertex_index;
                        planefit_2d = plane_2d_coord_input;
                    }
                }
            }

            //no matching vertex has been found
            if (matching_vertex_index == -1)
            {
                Console.WriteLine($"###ERROR: Could not find an vertex to generate a 2d plane for node {node_index}");
                return false;
            }

            Vertex new_vertex_block = NewBsp.Vertices[matching_vertex_index];
            RealPoint2d newcoords = vertex_get_projection_relevant_coords(new_vertex_block, plane_projection_axis, plane_mirror_check);
            float new_plane_D = plane_2d.I * newcoords.X + plane_2d.J * newcoords.Y;
            NewBsp.Bsp2dNodes[node_index].Plane.D = new_plane_D;

            int right_child_node = Bsp.Bsp2dNodes[node_index].RightChild & 0xFFFF;
            int left_child_node = Bsp.Bsp2dNodes[node_index].LeftChild & 0xFFFF;

            if (recalculate_bsp2dnodes(left_child_node, plane_block, plane_projection_axis, plane_mirror_check) &&
                recalculate_bsp2dnodes(right_child_node, plane_block, plane_projection_axis, plane_mirror_check))
                    return true;

            return false;
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

                if (!recalculate_bsp2dnodes(node_index, plane_block, plane_projection_axis, plane_mirror_check))
                {
                    Console.WriteLine($"###ERROR: Could not regenerate bsp2dnode {node_index}");
                    return false;
                }


                /*int edge_index = 0;
                for (edge_index = 0; edge_index < Bsp.Edges.Count; edge_index++)
                {
                    Vertex startvertex = Bsp.Vertices[Bsp.Edges[edge_index].StartVertex];
                    Vertex endvertex = Bsp.Vertices[Bsp.Edges[edge_index].EndVertex];
                    if(determine_vertex_plane_relationship(startvertex.Point, plane_block.Value) == Plane_Relationship.OnPlane &&
                        determine_vertex_plane_relationship(endvertex.Point, plane_block.Value) == Plane_Relationship.OnPlane)
                    {
                        RealPoint2d coords1 = vertex_get_projection_relevant_coords(startvertex, plane_projection_axis, plane_mirror_check);
                        RealPoint2d coords2 = vertex_get_projection_relevant_coords(endvertex, plane_projection_axis, plane_mirror_check);
                        RealPlane2d edge_plane = generate_bsp2d_plane_parameters(coords1, coords2);

                        if(compare2dplanes(plane_2d, edge_plane))
                        {
                            Vertex new_startvertex = NewBsp.Vertices[Bsp.Edges[edge_index].StartVertex];
                            Vertex new_endvertex = NewBsp.Vertices[Bsp.Edges[edge_index].EndVertex];
                            RealPoint2d new_coords1 = vertex_get_projection_relevant_coords(new_startvertex, plane_projection_axis, plane_mirror_check);
                            RealPoint2d new_coords2 = vertex_get_projection_relevant_coords(new_endvertex, plane_projection_axis, plane_mirror_check);
                            RealPlane2d new_edge_plane = generate_bsp2d_plane_parameters(coords1, coords2);
                            NewBsp.Bsp2dNodes[node_index].Plane = new_edge_plane;
                            break;
                        }
                    }

                }
                
                //no matching edge has been found
                if(edge_index >= Bsp.Edges.Count)
                {
                    Console.WriteLine($"###ERROR: Could not find an edge to generate a 2d plane for node {node_index}");
                    return false;
                }
                */
            }

            return true;
        }

        public bool compare2dplanes(RealPlane2d plane1, RealPlane2d plane2)
        {
            if (Math.Abs(plane1.I - plane2.I) < 0.0001 &&
                Math.Abs(plane1.J - plane2.J) < 0.0001 &&
                Math.Abs(plane1.D - plane2.D) < 0.0001)
                return true;
            return false;
        }

        public RealPlane2d generate_bsp2d_plane_parameters(RealPoint2d Coords1, RealPoint2d Coords2)
        {
            RealPlane2d bsp2dnode_block = new RealPlane2d();
            double plane_I = Coords2.Y - Coords1.Y;
            double plane_J = Coords1.X - Coords2.X;

            double dist = (float)Math.Sqrt(plane_I * plane_I + plane_J * plane_J);

            if (Math.Abs(dist) < 0.0001)
            {
                bsp2dnode_block.I = (float)plane_I;
                bsp2dnode_block.J = (float)plane_J;
                bsp2dnode_block.D = 0;
            }
            else
            {
                bsp2dnode_block.I = (float)(plane_I / dist);
                bsp2dnode_block.J = (float)(plane_J / dist);
                bsp2dnode_block.D = bsp2dnode_block.J * Coords1.Y + bsp2dnode_block.I * Coords1.X;
            }
            return bsp2dnode_block;
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

        public int find_plane_parent_surface(int plane_index)
        {
            for(int surface_index = 0; surface_index < Bsp.Surfaces.Count; surface_index++)
            {
                if (Bsp.Surfaces[surface_index].Plane == plane_index ||
                    ((Bsp.Surfaces[surface_index].Plane & 0x7FFF) == plane_index))
                    return surface_index;
            }
            return -1;
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

        public bool recalculate_surface_plane(int surface_index, int plane_index)
        {
            List<RealPoint3d> pointlist = new List<RealPoint3d>();
            Surface surface_block = Bsp.Surfaces[surface_index];

            int surface_edge_index = surface_block.FirstEdge;
            //collect vertices on the plane
            while (true)
            {
                Edge surface_edge_block = Bsp.Edges[surface_edge_index];
                if (surface_edge_block.RightSurface == surface_index)
                    pointlist.Add(NewBsp.Vertices[surface_edge_block.EndVertex].Point);
                else
                    pointlist.Add(NewBsp.Vertices[surface_edge_block.StartVertex].Point);


                if (surface_edge_block.RightSurface == surface_index)
                    surface_edge_index = surface_edge_block.ReverseEdge;
                else
                    surface_edge_index = surface_edge_block.ForwardEdge;
                //break the loop if we have finished circulating the surface
                if (surface_edge_index == surface_block.FirstEdge)
                    break;
            }
            if (pointlist.Count < 3)
            {
                Console.WriteLine("###ERROR: Not enough points to generate a plane!");
                return false;
            }
            else
            {
                int count = 0;
                while (true)
                {
                    if(plane_generation_points_valid(pointlist[count], pointlist[count + 1], pointlist[count + 2]))
                    {
                        RealPlane3d newplane = generate_plane_from_3_points(pointlist[count], pointlist[count + 1], pointlist[count + 2]);
                        if (plane_test_points(newplane, new List<RealPoint3d> { pointlist[count], pointlist[count + 1], pointlist[count + 2] }))
                        {
                            NewBsp.Planes[plane_index].Value = newplane;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("###ERROR: Did not produce valid plane from points!");
                        }
                    }
                    if (count++ + 2 >= pointlist.Count - 1)
                    {
                        count = -1;
                        break;
                    }
                }
                if (count == -1)
                {
                    Console.WriteLine("###ERROR: No valid planes could be produced from pointset!");
                    return false;
                }
            }
            return true;
        }

        public bool plane_test_points(RealPlane3d plane, List<RealPoint3d> pointlist)
        {
            foreach(var point in pointlist)
            {
                float plane_fit = point.X * plane.I + point.Y * plane.J + point.Z * plane.K - plane.D;
                if (plane_fit < -0.00024414062 || plane_fit > 0.00024414062)
                    return false;
            }
            return true;
        }

        public bool plane_is_mirrored(RealPoint3d point0, RealPoint3d point1, RealPoint3d point2, RealPlane3d plane)
        {
            float v10 = point1.X - point0.X;
            float v11 = point1.Y - point0.Y;
            float v12 = point1.Z - point0.Z;
            float v13 = point2.X - point0.X;
            float v14 = point2.Y - point0.Y;
            float v15 = point2.Z - point0.Z;
            float v16 = v15 * v11 - v14 * v12;
            float v17 = v12 * v13 - v15 * v10;
            float v18 = v10 * v14 - v11 * v13;
            if (v18 * plane.K
               + v17 * plane.J
               + v16 * plane.I > 0.0)
                return false;
            else
                return true;
        }

        public bool plane_generation_points_valid(RealPoint3d point0, RealPoint3d point1, RealPoint3d point2)
        {
            if (Math.Abs(point0.X - point1.X) < 0.00009999999747378752
              && Math.Abs(point0.Y - point1.Y) < 0.00009999999747378752
              && Math.Abs(point0.Z - point1.Z) < 0.00009999999747378752)
            {
                return false;
            }
            if (Math.Abs(point1.X - point2.X) < 0.00009999999747378752
              && Math.Abs(point1.Y - point2.Y) < 0.00009999999747378752
              && Math.Abs(point1.Z - point2.Z) < 0.00009999999747378752)
            {
                return false;
            }
            if (Math.Abs(point2.X - point0.X) < 0.00009999999747378752
              && Math.Abs(point2.Y - point0.Y) < 0.00009999999747378752
              && Math.Abs(point2.Z - point0.Z) < 0.00009999999747378752)
            {
                return false;
            }
            return true;
        }

        public RealPlane3d generate_plane_from_3_points(RealPoint3d point0, RealPoint3d point1, RealPoint3d point2)
        {
            RealPlane3d plane = new RealPlane3d();

            double xdiff10 = point1.X - point0.X;
            double ydiff10 = point1.Y - point0.Y;
            double zdiff10 = point1.Z - point0.Z;
            double xdiff20 = point2.X - point0.X;
            double ydiff20 = point2.Y - point0.Y;
            double zdiff20 = point2.Z - point0.Z;

            double v11 = zdiff20 * ydiff10 - ydiff20 * zdiff10;
            double v12 = xdiff20 * zdiff10 - zdiff20 * xdiff10;
            double v13 = ydiff20 * xdiff10 - xdiff20 * ydiff10;
            double v14 = 1.0 / Math.Sqrt(v13 * v13 + v12 * v12 + v11 * v11);
            double v15 = v14 * v11;
            double v16 = v14 * v12;
            double v17 = v14 * v13;
            double v18 = point0.Y * v16 + point0.X * v15 + point0.Z * v17;
            plane.I = (float)v15;
            plane.J = (float)v16;
            plane.K = (float)v17;
            plane.D = (float)v18;

            return plane;
        }
        
    }
}