using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Geometry.BspCollisionGeometry.Utils
{
    public class CollisionBSPGen3Builder
    {
        public List<leaf> initial_leaves = new List<leaf>();
        public LargeCollisionBspBlock Bsp { get; set; }

        public class polygon
        {
            public List<RealPoint3d> vertices;
            public int surface_index = -1;
            public int plane_index = -1;
            public bool is_connection = false;
            public int leaf_above = -1;
            public int leaf_below = -1;
        }

        public class polygon_plane
        {
            public int plane_index = -1;
            public List<polygon> polygons = new List<polygon>();
        }

        public class leaf_connection
        {
            public int leaf_above = -1;
            public int leaf_below = -1;
            public float connection_total;
            public float connection_vs_nonconnection;
            public float nonconnection_total;
            public int plane_index = -1;
            public List<polygon> polygons = new List<polygon>();
        };

        public class leaf
        {
            public float float10;
            public List<leaf_connection> leaf_connections = new List<leaf_connection>();
        }
        public enum planerelationship : int
        {
            Unknown = 0,
            BackofPlane = 1,
            FrontofPlane = 2,
            BothSidesofPlane = 3, //both 1 & 2 
            OnPlane = 4
        }
        public bool build_bsp()
        {
            //make sure there is nothing in the bsp blocks before starting
            Bsp.Leaves.Clear();
            Bsp.Bsp2dNodes.Clear();
            Bsp.Bsp2dReferences.Clear();
            Bsp.Bsp3dNodes.Clear();

            List<polygon_plane> polygon_planes = build_polygon_planes();

            return true;
        }

        public void build_bsp3d_node(List<polygon_plane> polygon_planes, ref int node_index)
        {
            if(polygon_planes.Count <= 0)
            {
                initial_leaves.Add(new leaf());
                node_index = (int)((initial_leaves.Count - 1) | 0x80000000);
            }
            else
            {
                int splitting_plane_index = find_surface_splitting_plane(polygon_planes);
                List<polygon_plane> back_array = new List<polygon_plane>();
                List<polygon_plane> front_array = new List<polygon_plane>();
                List<polygon_plane> back_array_no_margin = new List<polygon_plane>();
                List<polygon_plane> front_array_no_margin = new List<polygon_plane>();
                polygon_planes_split(polygon_planes, splitting_plane_index, ref back_array, ref front_array, true);
                polygon_planes_split(polygon_planes, splitting_plane_index, ref back_array_no_margin, ref front_array_no_margin, false);
                LargeBsp3dNode new_node = new LargeBsp3dNode
                {
                    Plane = splitting_plane_index
                };
                Bsp.Bsp3dNodes.Add(new_node);
                node_index = Bsp.Bsp3dNodes.Count - 1;

            }
        }

        public List<polygon_plane> build_polygon_planes()
        {
            //build polygon planes
            List<polygon_plane> polygon_planes = new List<polygon_plane>();
            for (var i = 0; i < Bsp.Surfaces.Count; i++)
            {
                LargeSurface surface = Bsp.Surfaces[i];
                if (surface.Flags.HasFlag(SurfaceFlags.Invalid))
                    continue;

                polygon new_polygon = new polygon
                {
                    surface_index = i,
                    plane_index = surface.Plane,
                    vertices = surface_collect_vertices(i)
                };

                if (surface.Flags.HasFlag(SurfaceFlags.PlaneNegated))
                    new_polygon.plane_index = (int)(new_polygon.plane_index | 0x80000000);

                int matching_index = polygon_planes.FindIndex(p => p.plane_index == (surface.Plane & 0x7FFFFFFF));
                if (matching_index == -1)
                {
                    polygon_planes.Add(new polygon_plane
                    {
                        plane_index = surface.Plane & 0x7FFFFFFF,
                        polygons = new List<polygon> { new_polygon }
                    });
                }
                else
                    polygon_planes[matching_index].polygons.Add(new_polygon);
            }
            //order polygon planes by plane index
            polygon_planes.OrderBy(p => p.plane_index);
            return polygon_planes;
        }

        public void polygon_planes_add_polygon(List<polygon_plane> polygon_planes, polygon poly)
        {
            int matching_index = polygon_planes.FindIndex(p => p.plane_index == (poly.plane_index & 0x7FFFFFFF));
            if (matching_index == -1)
            {
                polygon_planes.Add(new polygon_plane
                {
                    plane_index = poly.plane_index,
                    polygons = new List<polygon> { poly }
                });
            }
            else
                polygon_planes[matching_index].polygons.Add(poly);
        }

        public void polygon_planes_split(List<polygon_plane> polygon_planes, int plane_index, ref List<polygon_plane> back_array, ref List<polygon_plane> front_array, bool use_margins)
        {
            foreach(var poly_plane in polygon_planes)
            {
                if ((poly_plane.plane_index & 0x7FFFFFFF) == plane_index)
                    continue;
                RealPlane3d plane = Bsp.Planes[plane_index].Value;
                foreach (var poly in poly_plane.polygons)
                {
                    RealPlane3d polygon_plane = Bsp.Planes[poly.plane_index & 0x7FFFFFFF].Value;

                    RealPlane3d front_plane = plane.DeepClone();
                    RealPlane3d back_plane = plane.DeepClone();
                    if (use_margins)
                    {
                        double plane_margin = determine_plane_margin(polygon_plane, plane);
                        front_plane.D -= (float)plane_margin;
                        back_plane.D += (float)plane_margin;
                    }

                    List<RealPoint3d> front_points = plane_cut_polygon(poly.vertices, front_plane, true);
                    if(front_points.Count > 0)
                    {
                        polygon front_poly = poly.DeepClone();
                        front_poly.vertices = front_points;
                        polygon_planes_add_polygon(front_array, front_poly);
                    }
                    List<RealPoint3d> back_points = plane_cut_polygon(poly.vertices, back_plane, false);
                    if (front_points.Count > 0)
                    {
                        polygon back_poly = poly.DeepClone();
                        back_poly.vertices = back_points;
                        polygon_planes_add_polygon(back_array, back_poly);
                    }
                }
            }
        }

        public double determine_plane_margin(RealPlane3d a, RealPlane3d b)
        {
            double max_margin = 0.00024414062d;
            double min_margin = 0.0000024414062d;

            RealVector3d vector = new RealVector3d()
            {
                I = a.J * b.K - a.K * b.J,
                J = a.K * b.I - a.I * b.K,
                K = a.I * b.J - a.J * b.I
            };
            double result = Math.Abs(Math.Sqrt(vector.I + vector.J + vector.K) * max_margin);
            if (result < min_margin)
                return min_margin;
            return result;            
        }

        public int find_surface_splitting_plane(List<polygon_plane> polygon_planes)
        {
            //this code should only be used with <1024 polygon planes as it loops and checks every plane. large meshes first need to be split with generate_object_splitting_plane
            if (polygon_planes.Count < 1024)
            {
                return surface_plane_splitting_effectiveness_check_loop(polygon_planes);
            }
            return generate_object_splitting_plane(polygon_planes);
        }

        public int surface_plane_splitting_effectiveness_check_loop(List<polygon_plane> polygon_planes)
        {
            int best_plane_index = -1;
            double lowest_plane_splitting_effectiveness = double.MaxValue;

            //loop through polygon planes to see how effectively their associated planes split the remaining polygons. Find the one that most effectively splits the remaining polygons.
            foreach(var polygon_plane in polygon_planes)
            {
                double current_plane_splitting_effectiveness = plane_get_splitting_effectiveness(polygon_plane.plane_index, polygon_planes);
                if (current_plane_splitting_effectiveness < lowest_plane_splitting_effectiveness)
                {
                    lowest_plane_splitting_effectiveness = current_plane_splitting_effectiveness;
                    best_plane_index = polygon_plane.plane_index;
                }
            }
            return best_plane_index;
        }

        public class extent_entry
        {
            public float coordinate;
            public bool is_max_coord;
        };

        public class polygon_array_qsort_compar : IComparer<extent_entry>
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

        public int generate_object_splitting_plane(List<polygon_plane> polygon_planes)
        {
            double lowest_plane_splitting_effectiveness = double.MaxValue;

            int best_axis_index = -1;
            double desired_plane_distance = 0;

            //check X, Y, and Z axes
            for (int current_test_axis = 0; current_test_axis < 3; current_test_axis++)
            {
                //generate a list of maximum and minimum coordinates on the specified plane for each surface
                List<extent_entry> extents_table = new List<extent_entry>();
                foreach (var poly_plane in polygon_planes)
                {
                    foreach (var poly in poly_plane.polygons)
                    {
                        extent_entry min_coordinate = new extent_entry { coordinate = float.MaxValue, is_max_coord = false };
                        extent_entry max_coordinate = new extent_entry { coordinate = float.MinValue, is_max_coord = true };
                        foreach (var vert in poly.vertices)
                        {
                            switch (current_test_axis)
                            {
                                case 0: //X axis
                                    if (min_coordinate.coordinate > vert.X)
                                        min_coordinate.coordinate = vert.X;
                                    if (max_coordinate.coordinate < vert.X)
                                        max_coordinate.coordinate = vert.X;
                                    break;
                                case 1: //Y axis
                                    if (min_coordinate.coordinate > vert.Y)
                                        min_coordinate.coordinate = vert.Y;
                                    if (max_coordinate.coordinate < vert.Y)
                                        max_coordinate.coordinate = vert.Y;
                                    break;
                                case 2: //Z axis
                                    if (min_coordinate.coordinate > vert.Z)
                                        min_coordinate.coordinate = vert.Z;
                                    if (max_coordinate.coordinate < vert.Z)
                                        max_coordinate.coordinate = vert.Z;
                                    break;
                            }
                        }
                        extents_table.Add(min_coordinate);
                        extents_table.Add(max_coordinate);
                    }
                }        
                //use the above defined comparer to sort the extent entries first by coordinate and then by whether they are a surface max coordinate or not
                polygon_array_qsort_compar sorter = new polygon_array_qsort_compar();
                extents_table.Sort(sorter);

                int front_count = extents_table.Count / 2;
                int back_count = 0;
                for (int extent_index = 1; extent_index < extents_table.Count; extent_index++)
                {
                    if (extents_table[extent_index - 1].is_max_coord)
                    {
                        if (--front_count < 0)
                        {
                            Console.WriteLine("###ERROR negative plane front count!");
                        }
                    }
                    else
                    {
                        if (++back_count > extents_table.Count / 2)
                        {
                            Console.WriteLine("###ERROR back count greater than polygon count!");
                        }
                    }
                    double current_splitting_effectiveness = Math.Abs(back_count - front_count) + 2 * (front_count + back_count);
                    if (current_splitting_effectiveness < lowest_plane_splitting_effectiveness)
                    {
                        lowest_plane_splitting_effectiveness = current_splitting_effectiveness;
                        best_axis_index = current_test_axis;
                        desired_plane_distance = (extents_table[extent_index].coordinate + extents_table[extent_index - 1].coordinate) * 0.5;
                    }
                }
            }
            if (best_axis_index == -1)
            {
                Console.WriteLine("###ERROR: Failed to find best axis index!");
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
            for (ideal_plane_index = 0; ideal_plane_index < Bsp.Planes.Count; ideal_plane_index++)
            {
                var plane = Bsp.Planes[ideal_plane_index].Value;
                if (Math.Abs(plane.I - best_plane.I) < plane_tolerance &&
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
            if (ideal_plane_index < Bsp.Planes.Count)
            {
                return ideal_plane_index;
            }
            //otherwise just add a new plane with ideal characteristics
            else
            {
                Bsp.Planes.Add(new Plane { Value = best_plane });
                ideal_plane_index = Bsp.Planes.Count - 1;
                return ideal_plane_index;
            }
        }

        public double plane_get_splitting_effectiveness(int plane_index, List<polygon_plane> polygon_planes)
        {
            int front_count = 0;
            int back_count = 0;
            int total_count = 0;
            foreach(var poly_plane in polygon_planes)
            {
                if (poly_plane.plane_index == plane_index)
                    continue;
                foreach(var poly in poly_plane.polygons)
                {
                    planerelationship polyrelationship = determine_polygon_plane_relationship(poly, plane_index);
                    //only count polygons completely on one side or the other
                    if (polyrelationship == planerelationship.FrontofPlane)
                        front_count++;
                    else if (polyrelationship == planerelationship.BackofPlane)
                        back_count++;
                    total_count++;
                }
            }
            return 2.0d * total_count + Math.Abs(back_count - front_count);
        }

        public planerelationship determine_polygon_plane_relationship(polygon poly, int plane_index)
        {
            planerelationship polygon_plane_relationship = 0;

            //check if surface is on the plane
            if ((poly.plane_index & 0x7FFFFFFF) == plane_index)
                return planerelationship.OnPlane;

            RealPlane3d plane = Bsp.Planes[plane_index & 0x7FFFFFFF].Value;

            foreach (var vert in poly.vertices)
            {
                planerelationship vertex_plane_relationship = determine_vertex_plane_relationship(vert, plane);
                if (!vertex_plane_relationship.HasFlag(planerelationship.OnPlane))
                    polygon_plane_relationship |= vertex_plane_relationship;

                if (polygon_plane_relationship.HasFlag(planerelationship.BothSidesofPlane))
                    break;
            }
            return polygon_plane_relationship;
        }
        public planerelationship determine_vertex_plane_relationship(RealPoint3d vertex, RealPlane3d plane)
        {
            double plane_equation_vertex_input = vertex.X * plane.I + vertex.Y * plane.J + vertex.Z * plane.K - plane.D;

            if (plane_equation_vertex_input >= -0.00024414062)
            {
                //if the plane distance is within both of these bounds, it is considered ON the plane
                if (plane_equation_vertex_input <= 0.00024414062)
                    return planerelationship.OnPlane;
                else
                    return planerelationship.FrontofPlane;
            }
            else
            {
                return planerelationship.BackofPlane;
            }
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

        public List<RealPoint3d> plane_cut_polygon(List<RealPoint3d> points, RealPlane3d plane, bool plane_side)
        {
            List<RealPoint3d> output_points = new List<RealPoint3d>();
            RealPoint3d previous_point = points[points.Count - 1];
            double d0 = point_get_plane_distance(previous_point, plane);
            for (var vertex_index = 0; vertex_index < points.Count; vertex_index++)
            {
                RealPoint3d current_point = points[vertex_index];
                double d1 = point_get_plane_distance(current_point, plane);
                //are the current and final vertex on the same side of the plane?
                if (d1 >= 0 != d0 >= 0)
                {
                    output_points.Add(vertex_shift_to_plane(previous_point, current_point, d0, d1));
                }
                if (d1 >= 0 == plane_side)
                {
                    output_points.Add(current_point);
                }
                previous_point = current_point;
                d0 = d1;
            }
            return output_points;
        }

        public double point_get_plane_distance(RealPoint3d point, RealPlane3d plane)
        {
            //precision requires this function to use identical order of operations as the compiled version
            //point.X * plane.I + point.Y * plane.J + point.Z * plane.K - plane.D
            double Z = point.Z;
            double ZK = Z * plane.K;
            double Y = point.Y;
            double YJ = Y * plane.J;
            double YJZK = YJ + ZK;
            double I = plane.I;
            double XI = I * point.X;
            double XIYJZK = XI + YJZK;
            return XIYJZK - plane.D;
        }

        public RealPoint3d vertex_shift_to_plane(RealPoint3d p0, RealPoint3d p1, double d0, double d1)
        {
            //precision requires this function to use identical order of operations as the compiled version
            /*
                double dratio = d0 / (d0 - d1);
                RealPoint3d result = new RealPoint3d
                {
                    X = (float)((p1.X - p0.X) * dratio + p0.X),
                    Y = (float)((p1.Y - p0.Y) * dratio + p0.Y),
                    Z = (float)((p1.Z - p0.Z) * dratio + p0.Z),
                };
            */
            RealPoint3d result = new RealPoint3d();
            double X1 = p1.X;
            double Xdiff = X1 - p0.X;
            double Y1 = p1.Y;
            double Ydiff = Y1 - p0.Y;
            double Z1 = p1.Z;
            float Zdiff = (float)(Z1 - p0.Z); //value is stored as float temporarily

            double Ddiff = d0 - d1;
            float Ddiv = (float)(d0 / Ddiff);

            double Xmul = Xdiff * Ddiv;
            result.X = (float)(Xmul + p0.X);

            double Ddiv2 = Ddiv;
            double Ymul = Ydiff * Ddiv2;
            result.Y = (float)(Ymul + p0.Y);

            double Zdiff2 = Zdiff;
            double Zmul = Zdiff2 * Ddiv2;
            result.Z = (float)(Zmul + p0.Z);
            return result;
        }
    }
}
