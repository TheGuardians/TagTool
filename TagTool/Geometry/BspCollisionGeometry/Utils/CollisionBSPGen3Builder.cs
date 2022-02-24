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
        public List<node> initial_nodes = new List<node>();
        public LargeCollisionBspBlock Bsp { get; set; }
        public float[,] Bounds = new float[3,2];
        public List<float> BoundingCoords = new List<float>();
        public bool debug = false;

        public class polygon
        {
            public List<RealPoint3d> vertices = new List<RealPoint3d>();
            public int surface_index = -1;
            public int plane_index = -1;
            public bool is_connection = false;
            public bool is_double_sided = false;
            public int above = -1;
            public int below = -1;
        }

        public class polygon_plane
        {
            public int plane_index = -1;
            public List<polygon> polygons = new List<polygon>();
        }

        public class leaf_connection
        {
            public int above = -1;
            public int below = -1;
            public float connection_total;
            public float connection_vs_nonconnection;
            public float nonconnection_total;
            public int plane_index = -1;
            public List<polygon> polygons = new List<polygon>();
        };

        public class leaf
        {
            public int parent_node = -1;
            public float float10;
            public List<leaf_connection> leaf_connections = new List<leaf_connection>();
            public List<polygon> polygons = new List<polygon>();
            public List<int> surface_indices = new List<int>();
        }
        public class node
        {
            public int parent_node = -1;
            public int back_child = -1;
            public int front_child = -1;
            public int plane_index = -1;
            public List<polygon_plane> node_polygons = new List<polygon_plane>();
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

            get_geometry_bounds();

            List<polygon_plane> polygon_planes = build_polygon_planes();
            int blank = 0;
            build_bsp3d_node(-1, ref blank, polygon_planes, polygon_planes);
            color_leaves();
            populate_nodes_leaves();
            return true;
        }

        public void get_geometry_bounds()
        {
            for(var i = 0; i < 2; i++)
            {
                Bounds[i, 0] = float.MaxValue;
                Bounds[i, 1] = -float.MaxValue;
            }

            //get object bounds
            foreach (var vertex in Bsp.Vertices)
            {
                RealPoint3d vert = vertex.Point;
                if (vert.X < Bounds[0, 0])
                    Bounds[0, 0] = vert.X;
                if (vert.X > Bounds[0, 1])
                    Bounds[0, 1] = vert.X;
                if (vert.Y < Bounds[1, 0])
                    Bounds[1, 0] = vert.Y;
                if (vert.Y > Bounds[1, 1])
                    Bounds[1, 1] = vert.Y;
                if (vert.Z < Bounds[2, 0])
                    Bounds[2, 0] = vert.Z;
                if (vert.Z > Bounds[2, 1])
                    Bounds[2, 1] = vert.Z;
            }

            //the used bounds are essentially twice as large as the object for some reason
            for(var i = 0; i < 3; i++)
            {
                float radius = (Bounds[i, 1] - Bounds[i, 0]) * 0.5f;
                Bounds[i, 0] -= radius;
                Bounds[i, 1] += radius;
            }

            //create a list of 48 floats that consist of 2d points that compose each face of a bounding box surrounding the geometry
            int[,] coordinate_pairs = new int[,] 
            { { 2, 1 }, { 1, 2 }, { 0, 2 }, { 2, 0 }, { 1, 0 }, { 0, 1 }  };
            for(var i = 0; i < 6; i++)
            {
                BoundingCoords.Add(Bounds[coordinate_pairs[i, 0], 0]);
                BoundingCoords.Add(Bounds[coordinate_pairs[i, 1], 0]);
                BoundingCoords.Add(Bounds[coordinate_pairs[i, 0], 1]);
                BoundingCoords.Add(Bounds[coordinate_pairs[i, 1], 0]);
                BoundingCoords.Add(Bounds[coordinate_pairs[i, 0], 1]);
                BoundingCoords.Add(Bounds[coordinate_pairs[i, 1], 1]);
                BoundingCoords.Add(Bounds[coordinate_pairs[i, 0], 0]);
                BoundingCoords.Add(Bounds[coordinate_pairs[i, 1], 1]);
            }
        }

        public void populate_nodes_leaves()
        {
            foreach (var node in initial_nodes)
            {
                LargeBsp3dNode newnode = new LargeBsp3dNode
                {
                    Plane = node.plane_index
                };

                if(node.back_child < 0)
                {
                    if (initial_leaves[node.back_child & 0x7FFFFFFF].float10 < 0)
                        newnode.BackChild = -1;
                    else
                    {
                        int new_leaf_index = build_leaves_external(initial_leaves[node.back_child & 0x7FFFFFFF]);
                        newnode.BackChild = (int)(new_leaf_index | 0x80000000);                           
                    }
                }
                else
                {
                    newnode.BackChild = node.back_child;
                }
                if(node.front_child < 0)
                {
                    if (initial_leaves[node.front_child & 0x7FFFFFFF].float10 < 0)
                        newnode.FrontChild = -1;
                    else
                    {
                        int new_leaf_index = build_leaves_external(initial_leaves[node.front_child & 0x7FFFFFFF]);
                        newnode.FrontChild = (int)(new_leaf_index | 0x80000000);
                    }
                }
                else
                {
                    newnode.FrontChild = node.front_child;
                }
                Bsp.Bsp3dNodes.Add(newnode);
            }
        }

        public int build_leaves_external(leaf initial_leaf)
        {
            surface_array_definition surface_array = new surface_array_definition
            {
                used_count = initial_leaf.surface_indices.Count,
                surface_array = new List<int>()
            };

            foreach(var surface in initial_leaf.surface_indices)
            {
                surface_array.surface_array.Add((int)(surface | 0x80000000));
            }

            int leaf_index = -1;
            build_leaves(ref surface_array, ref leaf_index);
            return leaf_index;
        }

        public void color_leaves()
        {
            for(var i = 0; i < 1024; i++)
            {
                double delta = 0.0;
                //Console.WriteLine($"Coloring Leaves (iteration {i}, delta {delta}");
                for(var j = 0; j < initial_leaves.Count; j++)
                {
                    leaf initial_leaf = initial_leaves[j];

                    float connection_total_sum = 0.0f;
                    float nonconnection_total_sum = 0.0f;

                    foreach(var connection in initial_leaf.leaf_connections)
                    {
                        bool leaf_is_below = connection.below == j;
                        int other_leaf_index = leaf_is_below ? connection.above : connection.below;
                        float other_leaf_ratio = initial_leaves[other_leaf_index].float10;

                        connection_total_sum += connection.connection_total;

                        float nonconnection_total = connection.nonconnection_total;
                        if (leaf_is_below)
                            nonconnection_total = -nonconnection_total;

                        nonconnection_total_sum = nonconnection_total_sum + other_leaf_ratio * connection.connection_vs_nonconnection + nonconnection_total;
                    }

                    float result = 0.0f;
                    if(connection_total_sum != 0.0)
                    {
                        result = nonconnection_total_sum / connection_total_sum;
                        //restrict to range of -1 to 1
                        result = Math.Max(result, -1.0f);
                        result = Math.Min(result, 1.0f);
                    }

                    //keep highest delta for cycle through leaves
                    float temp_delta = Math.Abs(result - initial_leaf.float10);
                    if (temp_delta > delta)
                        delta = temp_delta;

                    //write result to leaf
                    initial_leaf.float10 = result;
                }
                //ConsoleClearLine();

                //stop iterating if highest delta in iteration below threshold
                if (delta < 0.0078125)
                    break;
            }
        }

        public void ConsoleClearLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new String(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }

        public void build_bsp3d_node(int parent_node_index, ref int node_index, List<polygon_plane> polygon_planes, List<polygon_plane> polygon_planes_no_margin)
        {
            if(polygon_planes.Count <= 0)
            {
                initial_leaves.Add(new leaf 
                { 
                    parent_node = parent_node_index,
                });
                node_index = (int)((initial_leaves.Count - 1) | 0x80000000);
            }
            else
            {
                int splitting_plane_index = find_surface_splitting_plane(polygon_planes);

                if (parent_node_index != -1 && splitting_plane_index == initial_nodes[parent_node_index].plane_index)
                    Console.WriteLine("Error Failed Split!");

                node new_node = new node
                {
                    parent_node = parent_node_index,
                    plane_index = splitting_plane_index
                };
                initial_nodes.Add(new_node);
                node_index = initial_nodes.Count - 1;

                List<polygon_plane> back_array = new List<polygon_plane>();
                List<polygon_plane> front_array = new List<polygon_plane>();
                List<polygon_plane> back_array_no_margin = new List<polygon_plane>();
                List<polygon_plane> front_array_no_margin = new List<polygon_plane>();
                polygon_planes_split(initial_nodes[node_index], polygon_planes, splitting_plane_index, ref back_array, ref front_array, true);
                polygon_planes_split(initial_nodes[node_index], polygon_planes_no_margin, splitting_plane_index, ref back_array_no_margin, ref front_array_no_margin, false);

                build_bsp3d_node(node_index, ref initial_nodes[node_index].back_child, back_array, back_array_no_margin);
                build_bsp3d_node(node_index, ref initial_nodes[node_index].front_child, front_array, front_array_no_margin);

                List<polygon> node_polygons = new List<polygon>();
                int node_poly_plane_index = polygon_planes_no_margin.FindIndex(p => p.plane_index == splitting_plane_index);
                if (node_poly_plane_index != -1)
                    node_polygons = polygon_planes_no_margin[node_poly_plane_index].polygons;

                connection_polygon_add_new(node_index, ref node_polygons);
                polygons_clip_to_leaves(initial_nodes[node_index], ref node_polygons);
                build_leaf_connections(node_polygons);
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
                if (surface.Flags.HasFlag(SurfaceFlags.TwoSided))
                    new_polygon.is_double_sided = true;
                
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
                    plane_index = poly.plane_index & 0x7FFFFFFF,
                    polygons = new List<polygon> { poly }
                });
            }
            else
                polygon_planes[matching_index].polygons.Add(poly);
        }

        public void polygon_planes_split(node current_node, List<polygon_plane> polygon_planes, int plane_index, ref List<polygon_plane> back_array, ref List<polygon_plane> front_array, bool use_margins)
        {
            foreach(var poly_plane in polygon_planes)
            {
                if (poly_plane.plane_index == plane_index)
                    continue;
                if (plane_index < 0 || plane_index >= Bsp.Planes.Count)
                    Console.WriteLine("ERROR: Invalid splitting plane index!");
                RealPlane3d plane = Bsp.Planes[plane_index].Value;
                foreach (var poly in poly_plane.polygons)
                {
                    RealPlane3d polygon_plane = Bsp.Planes[poly.plane_index & 0x7FFFFFFF].Value;

                    RealPlane3d front_plane = plane.DeepClone();
                    RealPlane3d back_plane = plane.DeepClone();
                    //variable margin determined by alignment of polygon plane and splitting plane
                    //smaller margin if planes are closely aligned
                    double plane_margin = 0.0d;
                    if (use_margins)
                    {
                        plane_margin = determine_plane_margin(polygon_plane, plane);
                        front_plane.D -= (float)plane_margin;
                        back_plane.D += (float)plane_margin;
                    }

                    float min_distance = 0.0f;
                    float max_distance = 0.0f;
                    polygon_get_plane_distance_min_max(poly, plane, ref min_distance, ref max_distance);
                    bool back_of_plane = min_distance < -plane_margin;
                    bool front_of_plane = max_distance > plane_margin;
                    //polygon is split by plane
                    if (back_of_plane && front_of_plane)
                    {
                        List<RealPoint3d> back_points = plane_cut_polygon(poly.vertices, back_plane, false);
                        if (back_points.Count > 0)
                        {
                            polygon back_poly = poly.DeepClone();
                            back_poly.vertices = back_points;
                            polygon_planes_add_polygon(back_array, back_poly);
                        }
                        List<RealPoint3d> front_points = plane_cut_polygon(poly.vertices, front_plane, true);
                        if (front_points.Count > 0)
                        {
                            polygon front_poly = poly.DeepClone();
                            front_poly.vertices = front_points;
                            polygon_planes_add_polygon(front_array, front_poly);
                        }
                    }
                    //store polygons that are on current node plane
                    else if (min_distance > -0.00012207031 &&
                        max_distance < 0.00012207031 && use_margins)
                    {
                        polygon_planes_add_polygon(current_node.node_polygons, poly);
                    }
                    else if(back_of_plane)
                        polygon_planes_add_polygon(back_array, poly);
                    else if (front_of_plane)
                        polygon_planes_add_polygon(front_array, poly);
                }
            }
        }

        public void polygon_get_plane_distance_min_max(polygon poly, RealPlane3d plane, ref float min_distance, ref float max_distance)
        {
            min_distance = float.MaxValue;
            max_distance = -float.MaxValue;
            foreach(var vert in poly.vertices)
            {
                double distance = point_get_plane_distance(vert, plane);
                if (distance < min_distance)
                    min_distance = (float)distance;
                if (distance > max_distance)
                    max_distance = (float)distance;
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
            double result = Math.Abs(Math.Sqrt(vector.I * vector.I + vector.J * vector.J + vector.K * vector.K) * max_margin);
            if (result < min_margin)
                return min_margin;
            return result;            
        }

        public RealVector3d cross_product_3d(RealVector3d a, RealVector3d b)
        {
            RealVector3d vector = new RealVector3d()
            {
                I = a.J * b.K - a.K * b.J,
                J = a.K * b.I - a.I * b.K,
                K = a.I * b.J - a.J * b.I
            };
            return vector;
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
                    best_plane_index = polygon_plane.plane_index & 0x7FFFFFFF;
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
            return 3.0d * total_count - back_count - front_count + Math.Abs(back_count - front_count);
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
            if (points.Count == 0)
                return points;
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
            return point.X * plane.I + point.Y * plane.J + point.Z * plane.K - plane.D;
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

        public void connection_polygon_add_new(int current_node_index, ref List<polygon> polygon_array)
        {
            node current_node = initial_nodes[current_node_index];
            RealPlane3d splitting_plane = Bsp.Planes[current_node.plane_index & 0x7FFFFFFF].Value;
            int projection_axis = plane_get_projection_coefficient(splitting_plane);
            int projection_sign = plane_get_projection_sign(splitting_plane, projection_axis) ? 1 : 0;

            //polygon with four vertices built from geometry bounds projected onto splitting plane
            //maximally sized polygon will be cut down by bsp planes
            polygon connection_polygon = new polygon
            {
                is_connection = true,
                plane_index = current_node.plane_index & 0x7FFFFFFF
            };
            for (var i = 0; i < 8; i += 2)
            {
                RealPoint2d point2d = new RealPoint2d
                {
                    X = BoundingCoords[16 * projection_axis + 8 * projection_sign + i],
                    Y = BoundingCoords[16 * projection_axis + 8 * projection_sign + i + 1]
                };
                connection_polygon.vertices.Add(point2d_and_plane_to_point3d(splitting_plane, point2d));
            }

            //TODO: cut by axis aligned planes w/ min and max bounds on projection axis

            //cut polygon by all parent node planes
            node bsp_node = current_node;
            int child_index = current_node_index;
            while(bsp_node.parent_node != -1)
            {
                bsp_node = initial_nodes[bsp_node.parent_node];
                bool split_side = bsp_node.front_child == child_index;
                RealPlane3d bsp_node_plane = Bsp.Planes[bsp_node.plane_index & 0x7FFFFFFF].Value;
                List<RealPoint3d> temp_vertices = plane_cut_polygon(connection_polygon.vertices, bsp_node_plane, split_side);
                connection_polygon.vertices = temp_vertices;
                child_index = initial_nodes[child_index].parent_node;
            }

            polygon_array.Add(connection_polygon);
        }

        public void polygons_clip_to_leaves(node current_node, ref List<polygon> polygon_array)
        {
            List<polygon> clipped_array = new List<polygon>();
            foreach(var poly in polygon_array)
            {
                polygon_clip_to_leaves(current_node.back_child, poly, ref clipped_array);
            }
            //wipe polygon array of initial polygons, to be repopulated momentarily
            polygon_array = new List<polygon>();
            foreach (var poly in clipped_array)
            {
                polygon_clip_to_leaves(current_node.front_child, poly, ref polygon_array);
            }
        }

        public void polygon_clip_to_leaves(int node_index, polygon poly, ref List<polygon> polygon_array)
        {
            //clip polygon into its multiple components that end up in leaves
            //in addition, keep track of what two leaves each polygon eventually belongs to
            if(node_index < 0)
            {
                if (poly.below != -1)
                    poly.above = node_index & 0x7FFFFFFF;
                else
                    poly.below = node_index & 0x7FFFFFFF;
                polygon_array.Add(poly);
            }
            else
            {
                node current_node = initial_nodes[node_index];
                RealPlane3d node_plane = Bsp.Planes[current_node.plane_index].Value;

                List<RealPoint3d> back_polygon_points = plane_cut_polygon(poly.vertices, node_plane, false);
                if(back_polygon_points.Count > 0)
                {
                    polygon back_polygon = poly.DeepClone();
                    back_polygon.vertices = back_polygon_points;
                    polygon_clip_to_leaves(current_node.back_child, back_polygon, ref polygon_array);
                }

                List<RealPoint3d> front_polygon_points = plane_cut_polygon(poly.vertices, node_plane, true);
                if (front_polygon_points.Count > 0)
                {
                    polygon front_polygon = poly.DeepClone();
                    front_polygon.vertices = front_polygon_points;
                    polygon_clip_to_leaves(current_node.front_child, front_polygon, ref polygon_array);
                }
            }
        }

        public void build_leaf_connections(List<polygon> polygon_array)
        {
            foreach(var poly in polygon_array)
            {
                double polygon_area = polygon_get_area(poly);
                double polygon_area_signed = polygon_area;
                if (poly.plane_index < 0)
                    polygon_area_signed = -polygon_area_signed;

                for(var i = 0; i < 2; i++)
                {
                    leaf target_leaf = i == 0 ? initial_leaves[poly.below] : initial_leaves[poly.above];

                    //find matching leaf connection if it exists
                    int leaf_connection_index = -1;
                    for (var j = 0; j < target_leaf.leaf_connections.Count; j++)
                    {
                        int test_leaf_index = i == 0 ? poly.above : poly.below;
                        if (target_leaf.leaf_connections[j].above == test_leaf_index ||
                           target_leaf.leaf_connections[j].below == test_leaf_index)
                        {
                            leaf_connection_index = j;
                            break;
                        }
                    }
                    //if none are found, create a new one
                    if (leaf_connection_index == -1)
                    {
                        target_leaf.leaf_connections.Add(new leaf_connection
                        {
                            plane_index = poly.plane_index & 0x7FFFFFFF,
                            below = poly.below,
                            above = poly.above,
                            polygons = new List<polygon> { poly }
                        });
                        leaf_connection_index = target_leaf.leaf_connections.Count - 1;
                        target_leaf.leaf_connections[leaf_connection_index].polygons.Add(poly);
                        target_leaf.polygons.Add(poly);
                        if (poly.surface_index != -1 && !target_leaf.surface_indices.Contains(poly.surface_index))
                            target_leaf.surface_indices.Add(poly.surface_index);
                    }
                    //otherwise just add polygon
                    else
                    {
                        target_leaf.leaf_connections[leaf_connection_index].polygons.Add(poly);
                        target_leaf.polygons.Add(poly);
                        if (poly.surface_index != -1 && !target_leaf.surface_indices.Contains(poly.surface_index))
                            target_leaf.surface_indices.Add(poly.surface_index);
                    }

                    //add to area sum fields of leaf connection with polygon area
                    if (!poly.is_double_sided)
                    {
                        if (poly.is_connection)
                        {
                            target_leaf.leaf_connections[leaf_connection_index].connection_total = (float)polygon_area_signed;
                            target_leaf.leaf_connections[leaf_connection_index].connection_vs_nonconnection += (float)polygon_area_signed;
                        }
                        else
                        {
                            target_leaf.leaf_connections[leaf_connection_index].connection_vs_nonconnection -= (float)polygon_area_signed;
                            target_leaf.leaf_connections[leaf_connection_index].nonconnection_total += (float)polygon_area;
                        }
                    }
                }                
            }
        }
        public double polygon_get_area(polygon poly)
        {
            RealPlane3d poly_plane = Bsp.Planes[poly.plane_index & 0x7FFFFFFF].Value;
            double area = 0.0f;
            for (var i = 0; i < poly.vertices.Count - 2; i++)
            {
                RealPoint3d v21 = poly.vertices[i + 1] - poly.vertices[0];
                RealPoint3d v31 = poly.vertices[i + 2] - poly.vertices[0];

                RealVector3d vector = cross_product_3d(point_to_vector(v31), point_to_vector(v21));

                //dot product of two vectors times 0.5
                area = area + (vector.I * poly_plane.I + vector.J * poly_plane.J + vector.K * poly_plane.K) * 0.5d;
            }
            return Math.Abs(area);
        }

        public RealVector3d point_to_vector(RealPoint3d point)
        {
            return new RealVector3d(point.X, point.Y, point.Z);
        }
        public RealPoint3d point2d_and_plane_to_point3d(RealPlane3d plane, RealPoint2d point)
        {
            int projection_axis = plane_get_projection_coefficient(plane);
            int projection_sign = plane_get_projection_sign(plane, projection_axis) ? 1 : 0;
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

        public int plane_get_projection_coefficient(RealPlane3d plane)
        {
            int minimum_coefficient;
            float plane_I = Math.Abs(plane.I);
            float plane_J = Math.Abs(plane.J);
            float plane_K = Math.Abs(plane.K);
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

        public bool plane_get_projection_sign(RealPlane3d plane, int projection_axis)
        {
            switch (projection_axis)
            {
                case 0: //x axis
                    return plane.I > 0.0f;
                case 1: //y axis
                    return plane.J > 0.0f;
                case 2: //z axis
                    return plane.K > 0.0f;
            }
            return false;
        }

        //BSP2D classes identical to Gen1 with very minor alterations
        public class surface_array_definition
        {
            public int free_count;
            public int used_count;
            public List<int> surface_array;
        }

        public double determine_plane_splitting_effectiveness_2D(LargeBsp2dNode bsp2dnode_block, int plane_projection_axis, int a3, surface_array_definition plane_matched_surface_array)
        {
            double splitting_effectiveness = 0.0d;
            int BackSurfaceCount = 0;
            int FrontSurfaceCount = 0;
            int TotalCount = 0;
            foreach (int surface_index in plane_matched_surface_array.surface_array)
            {
                planerelationship surface_plane_orientation = determine_surface_plane_relationship_2D(surface_index, bsp2dnode_block, plane_projection_axis, a3);
                if (surface_plane_orientation == planerelationship.BackofPlane)
                    BackSurfaceCount++;
                if (surface_plane_orientation == planerelationship.FrontofPlane)
                    FrontSurfaceCount++;
                TotalCount++;
            }
            //if all of the surfaces are on one side or the other, this is not a good split
            if (BackSurfaceCount > 0 &&
                BackSurfaceCount < TotalCount &&
                FrontSurfaceCount > 0 &&
                FrontSurfaceCount < TotalCount)
            {
                splitting_effectiveness = 3 * TotalCount - FrontSurfaceCount - BackSurfaceCount + Math.Abs(BackSurfaceCount - FrontSurfaceCount);
            }
            else
                splitting_effectiveness = double.MaxValue;
            return splitting_effectiveness;
        }

        public LargeBsp2dNode generate_bsp2d_plane_parameters(RealPoint2d Coords1, RealPoint2d Coords2)
        {
            LargeBsp2dNode bsp2dnode_block = new LargeBsp2dNode();
            double plane_I = Coords2.Y - Coords1.Y;
            double plane_J = Coords1.X - Coords2.X;

            double dist = Math.Sqrt(plane_I * plane_I + plane_J * plane_J);

            if (Math.Abs(dist) < 0.00009999999747378752)
            {
                bsp2dnode_block.Plane.I = (float)plane_I;
                bsp2dnode_block.Plane.J = (float)plane_J;
                bsp2dnode_block.Plane.D = 0;
            }
            else
            {
                bsp2dnode_block.Plane.I = (float)(plane_I / dist);
                bsp2dnode_block.Plane.J = (float)(plane_J / dist);
                bsp2dnode_block.Plane.D = bsp2dnode_block.Plane.J * Coords1.Y + bsp2dnode_block.Plane.I * Coords1.X;
            }
            return bsp2dnode_block;
        }

        public double generate_best_splitting_plane_2D(int plane_projection_axis, int plane_mirror_check, ref LargeBsp2dNode bsp2dnode_block, surface_array_definition plane_matched_surface_array)
        {
            double lowest_plane_splitting_effectiveness = double.MaxValue;

            for (int surface_index_index = 0; surface_index_index < plane_matched_surface_array.surface_array.Count; surface_index_index++)
            {
                int surface_index = plane_matched_surface_array.surface_array[surface_index_index] & 0x7FFFFFFF;
                LargeSurface surface_block = Bsp.Surfaces[surface_index];
                int surface_edge_index = surface_block.FirstEdge;
                while (true)
                {
                    LargeEdge surface_edge_block = Bsp.Edges[surface_edge_index];
                    LargeVertex start_vertex = Bsp.Vertices[surface_edge_block.StartVertex];
                    LargeVertex end_vertex = Bsp.Vertices[surface_edge_block.EndVertex];
                    RealPoint2d Coords1;
                    RealPoint2d Coords2;
                    if (surface_edge_block.RightSurface == surface_index)
                    {
                        Coords1 = vertex_get_projection_relevant_coords(end_vertex.Point, plane_projection_axis, plane_mirror_check);
                        Coords2 = vertex_get_projection_relevant_coords(start_vertex.Point, plane_projection_axis, plane_mirror_check);
                    }
                    else
                    {
                        Coords2 = vertex_get_projection_relevant_coords(end_vertex.Point, plane_projection_axis, plane_mirror_check);
                        Coords1 = vertex_get_projection_relevant_coords(start_vertex.Point, plane_projection_axis, plane_mirror_check);
                    }

                    LargeBsp2dNode current_bsp2dnode_block = generate_bsp2d_plane_parameters(Coords1, Coords2);

                    double current_plane_splitting_effectiveness = determine_plane_splitting_effectiveness_2D(current_bsp2dnode_block, plane_projection_axis, plane_mirror_check, plane_matched_surface_array);

                    if (current_plane_splitting_effectiveness < lowest_plane_splitting_effectiveness)
                    {
                        lowest_plane_splitting_effectiveness = current_plane_splitting_effectiveness.DeepClone();
                        bsp2dnode_block = current_bsp2dnode_block.DeepClone();
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
            return lowest_plane_splitting_effectiveness;
        }

        public RealPoint2d vertex_get_projection_relevant_coords(RealPoint3d vertex_block, int plane_projection_axis, int plane_mirror_check)
        {
            //plane mirror check = "projection_sign"
            RealPoint2d relevant_coords = new RealPoint2d();
            int v4 = 2 * (plane_mirror_check + 2 * plane_projection_axis);
            List<int> coordinate_list = new List<int> { 2, 1, 1, 2, 0, 2, 2, 0, 1, 0, 0, 1 };
            int vertex_coord_X_index = coordinate_list[v4];
            int vertex_coord_Y_index = coordinate_list[v4 + 1];

            float[] vertex_values = new float[3] { vertex_block.X, vertex_block.Y, vertex_block.Z };
            relevant_coords.X = vertex_values[vertex_coord_X_index];
            relevant_coords.Y = vertex_values[vertex_coord_Y_index];
            return relevant_coords;
        }

        public planerelationship determine_surface_plane_relationship_2D(int surface_index, LargeBsp2dNode bsp2dnodeblock, int plane_projection_axis, int plane_mirror_check)
        {
            planerelationship surface_plane_relationship = 0;
            LargeSurface surface_block = Bsp.Surfaces[surface_index];
            List<RealPoint2d> pointlist = new List<RealPoint2d>();
            List<double> inputlist = new List<double>();

            int surface_edge_index = surface_block.FirstEdge;
            while (true)
            {
                LargeEdge surface_edge_block = Bsp.Edges[surface_edge_index];
                LargeVertex edge_vertex;
                if (surface_edge_block.RightSurface == surface_index)
                    edge_vertex = Bsp.Vertices[surface_edge_block.EndVertex];
                else
                    edge_vertex = Bsp.Vertices[surface_edge_block.StartVertex];

                RealPoint2d relevant_coords = vertex_get_projection_relevant_coords(edge_vertex.Point, plane_projection_axis, plane_mirror_check);
                pointlist.Add(relevant_coords);

                double plane_equation_vertex_input = bsp2dnodeblock.Plane.I * relevant_coords.X + bsp2dnodeblock.Plane.J * relevant_coords.Y - bsp2dnodeblock.Plane.D;
                inputlist.Add(plane_equation_vertex_input);

                if (plane_equation_vertex_input < -0.00009999999747378752)
                {
                    surface_plane_relationship |= planerelationship.BackofPlane;
                }
                if (plane_equation_vertex_input > 0.00009999999747378752)
                {
                    surface_plane_relationship |= planerelationship.FrontofPlane;
                }

                if (surface_plane_relationship.HasFlag(planerelationship.BothSidesofPlane))
                    break;

                if (surface_edge_block.RightSurface == surface_index)
                    surface_edge_index = surface_edge_block.ReverseEdge;
                else
                    surface_edge_index = surface_edge_block.ForwardEdge;
                //break the loop if we have finished circulating the surface
                if (surface_edge_index == surface_block.FirstEdge)
                    break;
            }

            return surface_plane_relationship;
        }

        public int create_bsp2dnodes(int plane_projection_axis, int plane_mirror_check, surface_array_definition plane_matched_surface_array)
        {
            int bsp2dnode_index = -1;
            int back_bsp2dnode_index = -1;
            int front_bsp2dnode_index = -1;
            if (plane_matched_surface_array.surface_array.Count <= 1)
                return (int)(plane_matched_surface_array.surface_array[0] | 0x80000000);
            LargeBsp2dNode bsp2dnode_block = new LargeBsp2dNode() { LeftChild = -1, RightChild = -1 };
            bool warning_posted = false;

            while (plane_matched_surface_array.surface_array.Count > 1)
            {
                double splitting_effectiveness = generate_best_splitting_plane_2D(plane_projection_axis, plane_mirror_check, ref bsp2dnode_block, plane_matched_surface_array);
                //check to see that a valid split was found
                if (splitting_effectiveness < double.MaxValue)
                {
                    break;
                }
                else
                {
                    if (!warning_posted && debug)
                    {
                        Console.WriteLine("###ERROR Overlapping surfaces found!");                        
                        warning_posted = true;
                    }

                    //this addendum to the function is a bit of black magic from H2Tool.exe. 
                    //the surface with the smallest surface area is just removed from the leaf, as this is most likely to be the issue
                    //This ultimately allows bsp compilation to complete even when there are 'overlapping' surfaces
                    int remove_surface_index = -1;
                    double smallest_surface_area = double.MaxValue;
                    for (var i = 0; i < plane_matched_surface_array.surface_array.Count; i++)
                    {
                        double current_surface_area = surface_calculate_area(plane_matched_surface_array.surface_array[i] & 0x7FFFFFFF);
                        if (current_surface_area < smallest_surface_area)
                        {
                            smallest_surface_area = current_surface_area;
                            remove_surface_index = i;
                        }
                    }
                    plane_matched_surface_array.surface_array.RemoveAt(remove_surface_index);
                }
            }

            if (plane_matched_surface_array.surface_array.Count <= 1)
                return (int)(plane_matched_surface_array.surface_array[0] | 0x80000000);

            surface_array_definition back_surface_array = new surface_array_definition { surface_array = new List<int>() };
            surface_array_definition front_surface_array = new surface_array_definition { surface_array = new List<int>() };

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
                    //move the flag so that it won't get chopped off in the int cast
                    Bsp.Bsp2dNodes[bsp2dnode_index].LeftChild = back_bsp2dnode_index < 0 ? (int)(back_bsp2dnode_index | 0x80000000) : back_bsp2dnode_index;
                    Bsp.Bsp2dNodes[bsp2dnode_index].RightChild = front_bsp2dnode_index < 0 ? (int)(front_bsp2dnode_index | 0x80000000) : front_bsp2dnode_index;
                }
            }
            return bsp2dnode_index;
        }

        public double surface_calculate_area(int surface_index)
        {
            double surface_area = 0;
            LargeSurface surface_block = Bsp.Surfaces[surface_index];
            RealPlane3d plane = Bsp.Planes[surface_block.Plane & 0x7FFFFFFF].Value;
            int first_Edge_index = surface_block.FirstEdge;

            int current_edge_index = surface_block.FirstEdge;
            LargeEdge edge_block = Bsp.Edges[current_edge_index];
            bool surface_is_right_of_edge = edge_block.RightSurface == surface_index;

            LargeVertex BaseVertex = Bsp.Vertices[surface_is_right_of_edge ? edge_block.EndVertex : edge_block.StartVertex];

            //after collecting the base vertex, move to the next edge so two more can be collected
            current_edge_index = surface_is_right_of_edge ? edge_block.ReverseEdge : edge_block.ForwardEdge;
            edge_block = Bsp.Edges[current_edge_index];
            surface_is_right_of_edge = edge_block.RightSurface == surface_index;

            if ((surface_is_right_of_edge ? edge_block.ReverseEdge : edge_block.ForwardEdge) != surface_block.FirstEdge)
            {
                do
                {
                    LargeVertex VertexA = Bsp.Vertices[surface_is_right_of_edge ? edge_block.EndVertex : edge_block.StartVertex];
                    LargeVertex VertexB = Bsp.Vertices[surface_is_right_of_edge ? edge_block.StartVertex : edge_block.EndVertex];

                    RealPoint3d d10 = VertexA.Point - BaseVertex.Point;
                    RealPoint3d d20 = VertexB.Point - BaseVertex.Point;

                    double v19 = d20.Z * d10.Y - d20.Y * d10.Z;
                    double v20 = d20.X * d10.Z - d20.Z * d10.X;
                    double v21 = d20.Y * d10.X - d20.X * d10.Y;
                    double current_surface_area = plane.I * v19 + plane.J * v20 + plane.K * v21;
                    surface_area += current_surface_area;

                    current_edge_index = surface_is_right_of_edge ? edge_block.ReverseEdge : edge_block.ForwardEdge;
                    edge_block = Bsp.Edges[current_edge_index];
                    surface_is_right_of_edge = edge_block.RightSurface == surface_index;
                }
                while (current_edge_index != first_Edge_index);
            }
            //account for surfaces on the plane with an inverted normal
            if ((surface_block.Plane & 0x80000000) != 0)
                surface_area = -surface_area;
            if (surface_area <= 0.0)
                return 0.0;

            return surface_area;
        }

        public void sort_surfaces_by_plane_2D(int plane_projection_axis, int plane_mirror_check, LargeBsp2dNode bsp2dnode_block, surface_array_definition plane_matched_surface_array, surface_array_definition back_surface_array, surface_array_definition front_surface_array)
        {
            int back_surface_count = 0;
            int front_surface_count = 0;
            foreach (int surface_index in plane_matched_surface_array.surface_array)
            {
                planerelationship surface_location = determine_surface_plane_relationship_2D(surface_index, bsp2dnode_block, plane_projection_axis, plane_mirror_check);
                if (surface_location.HasFlag(planerelationship.BackofPlane))
                {
                    back_surface_array.surface_array.Add(surface_index);
                    back_surface_count++;
                }
                if (surface_location.HasFlag(planerelationship.FrontofPlane))
                {
                    front_surface_array.surface_array.Add(surface_index);
                    front_surface_count++;
                }
            }
        }

        public bool build_leaves(ref surface_array_definition surface_array, ref int leaf_index)
        {
            bool result = true;

            Bsp.Leaves.Add(new Leaf());
            leaf_index = Bsp.Leaves.Count - 1;

            //allocate initial leaf values
            Bsp.Leaves[leaf_index].Flags = 0;
            Bsp.Leaves[leaf_index].Bsp2dReferenceCount = 0;
            Bsp.Leaves[leaf_index].FirstBsp2dReference = 0xFFFFFFFF;

            for (int surface_array_index = 0; surface_array_index < surface_array.free_count + surface_array.used_count; surface_array_index++)
            {
                int surface_index = surface_array.surface_array[surface_array_index];
                LargeSurface surface_block = Bsp.Surfaces[surface_index & 0x7FFFFFFF];

                //carry over surface flags
                if (surface_block.Flags.HasFlag(SurfaceFlags.TwoSided))
                    Bsp.Leaves[leaf_index].Flags |= LeafFlags.ContainsDoubleSidedSurfaces;

                if (surface_index < 0)
                {
                    int plane_index = surface_block.Plane;
                    surface_array_definition plane_matched_surface_array = collect_plane_matching_surfaces(ref surface_array, plane_index);
                    if (plane_matched_surface_array.surface_array.Count > 0)
                    {
                        Bsp.Bsp2dReferences.Add(new LargeBsp2dReference());
                        int bsp2drefindex = Bsp.Bsp2dReferences.Count - 1;
                        Plane plane_block = Bsp.Planes[plane_index & 0x7FFFFFFF];
                        Bsp.Bsp2dReferences[bsp2drefindex].PlaneIndex = plane_index;
                        Bsp.Bsp2dReferences[bsp2drefindex].Bsp2dNodeIndex = -1;

                        //update leaf block parameters given new bsp2dreference
                        Bsp.Leaves[leaf_index].Bsp2dReferenceCount++;
                        if (Bsp.Leaves[leaf_index].FirstBsp2dReference == 0xFFFFFFFF)
                            Bsp.Leaves[leaf_index].FirstBsp2dReference = (uint)bsp2drefindex;

                        int plane_projection_axis = plane_get_projection_coefficient(plane_block.Value);
                        bool plane_projection_positive = plane_get_projection_sign(plane_block.Value, plane_projection_axis);
                        int plane_mirror_check = plane_projection_positive != (plane_index < 0) ? 1 : 0;

                        int bsp2dnodeindex = create_bsp2dnodes(plane_projection_axis, plane_mirror_check, plane_matched_surface_array);
                        Bsp.Bsp2dReferences[bsp2drefindex].Bsp2dNodeIndex = bsp2dnodeindex < 0 ? (int)(bsp2dnodeindex | 0x80000000) : bsp2dnodeindex;
                        if (bsp2dnodeindex == -1)
                        {
                            Console.WriteLine("###ERROR couldn't build bsp2d.");
                            result = false;
                        }
                    }
                }
            }
            return result;
        }
        public surface_array_definition collect_plane_matching_surfaces(ref surface_array_definition surface_array, int plane_index)
        {
            surface_array_definition plane_matched_surface_array = new surface_array_definition { surface_array = new List<int>() };
            for (int surface_array_index = 0; surface_array_index < surface_array.used_count; surface_array_index++)
            {
                int surface_index = surface_array.surface_array[surface_array_index];
                int absolute_surface_index = surface_index & 0x7FFFFFFF;

                int test_plane = Bsp.Surfaces[absolute_surface_index].Plane;
                if (surface_index < 0 && test_plane == plane_index)
                {
                    plane_matched_surface_array.surface_array.Add(absolute_surface_index);
                    //reset plane matching surfaces in the primary array to an unflagged state
                    surface_array.surface_array[surface_array_index] = absolute_surface_index;
                }
            }
            return plane_matched_surface_array;
        }
    }
}
