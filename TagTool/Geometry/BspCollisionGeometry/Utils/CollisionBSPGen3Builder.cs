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
        private float[,] Bounds = new float[3,2];
        private List<float> BoundingCoords = new List<float>();

        public class polygon
        {
            public List<RealPoint3d> vertices;
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
            public List<polygon_plane> polygons = new List<polygon_plane>();
            public List<polygon_plane> polygons_no_margin = new List<polygon_plane>();
        }
        public class node
        {
            public int parent_node = -1;
            public int back_child = -1;
            public int front_child = -1;
            public int plane_index = -1;
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

            return true;
        }

        public void get_geometry_bounds()
        {
            for(var i = 0; i < 2; i++)
            {
                Bounds[i, 0] = float.MaxValue;
                Bounds[i, 1] = float.MinValue;
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

            //create a list of 48 floats that consist of 2d points that compose each face of a bounding box surrounding the geometry
            int[,] coordinate_pairs = new int[,] 
            { { 2, 1 }, { 1, 2 }, { 0, 2 }, { 2, 0 }, { 1, 0 }, { 0, 1 }  };
            for(var i = 0; i < coordinate_pairs.Length; i++)
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

        public void build_bsp3d_node(int parent_node_index, ref int node_index, List<polygon_plane> polygon_planes, List<polygon_plane> polygon_planes_no_margin)
        {
            if(polygon_planes.Count <= 0)
            {
                initial_leaves.Add(new leaf 
                { 
                    parent_node = parent_node_index,
                    polygons = polygon_planes,
                    polygons_no_margin = polygon_planes_no_margin           
                });
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
                polygon_planes_split(polygon_planes_no_margin, splitting_plane_index, ref back_array_no_margin, ref front_array_no_margin, false);
                
                node new_node = new node
                {
                    parent_node = parent_node_index,
                    plane_index = splitting_plane_index
                };
                initial_nodes.Add(new_node);
                node_index = initial_nodes.Count - 1;

                build_bsp3d_node(node_index, ref initial_nodes[node_index].back_child, back_array, back_array_no_margin);
                build_bsp3d_node(node_index, ref initial_nodes[node_index].front_child, front_array, front_array_no_margin);

                List<polygon> node_polygons = new List<polygon>();
                int node_poly_plane_index = polygon_planes_no_margin.FindIndex(p => p.plane_index == splitting_plane_index);
                if (node_poly_plane_index != -1)
                    node_polygons = polygon_planes_no_margin[node_poly_plane_index].polygons;

                connection_polygon_add_new(initial_nodes[node_index], ref node_polygons);
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
                    //variable margin determined by alignment of polygon plane and splitting plane
                    //smaller margin if planes are closely aligned
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
                    if (back_points.Count > 0)
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

        public void connection_polygon_add_new(node current_node, ref List<polygon> polygon_array)
        {
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
                    X = BoundingCoords[4 * projection_axis + 2 * projection_sign + i],
                    Y = BoundingCoords[4 * projection_axis + 2 * projection_sign + i + 1]
                };
                connection_polygon.vertices.Add(point2d_and_plane_to_point3d(splitting_plane, point2d));
            }

            //TODO: cut by axis aligned planes w/ min and max bounds on projection axis

            //cut polygon by all parent node planes
            node bsp_node = current_node;
            while(bsp_node.parent_node != -1)
            {
                int child_index = initial_nodes.IndexOf(bsp_node);
                bsp_node = initial_nodes[bsp_node.parent_node];
                bool split_side = bsp_node.front_child == child_index;
                RealPlane3d bsp_node_plane = Bsp.Planes[bsp_node.plane_index & 0x7FFFFFFF].Value;
                connection_polygon.vertices = plane_cut_polygon(connection_polygon.vertices, bsp_node_plane, split_side);
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
                    poly.above = node_index;
                else
                    poly.below = node_index;
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
                RealPlane3d poly_plane = Bsp.Planes[poly.plane_index & 0x7FFFFFFF].Value;
                double polygon_area = polygon_get_area(poly);
                double polygon_area_signed = polygon_area;
                if (poly.plane_index < 0)
                    polygon_area_signed = -polygon_area_signed;
                leaf above_leaf = initial_leaves[poly.above];

                //find matching leaf connection if it exists
                int leaf_connection_index = -1;
                for(var i = 0; i < above_leaf.leaf_connections.Count; i++)
                {
                    if (above_leaf.leaf_connections[i].above == poly.above ||
                       above_leaf.leaf_connections[i].below == poly.above)
                    {
                        leaf_connection_index = i;
                        break;
                    }
                }
                //if none are found, create a new one
                if (leaf_connection_index == -1)
                {
                    above_leaf.leaf_connections.Add(new leaf_connection
                    {
                        plane_index = poly.plane_index & 0x7FFFFFFF,
                        below = poly.below,
                        above = poly.above,
                        polygons = new List<polygon> { poly }
                    });
                    leaf_connection_index = above_leaf.leaf_connections.Count - 1;
                }
                //otherwise just add polygon
                else
                {
                    above_leaf.leaf_connections[leaf_connection_index].polygons.Add(poly);
                }

                //add to area sum fields of leaf connection with polygon area
                if (!poly.is_double_sided)
                {
                    if (poly.is_connection)
                    {
                        above_leaf.leaf_connections[leaf_connection_index].connection_total = (float)polygon_area_signed;
                        above_leaf.leaf_connections[leaf_connection_index].connection_vs_nonconnection += (float)polygon_area_signed;
                    }
                    else
                    {
                        above_leaf.leaf_connections[leaf_connection_index].connection_vs_nonconnection -= (float)polygon_area_signed;
                        above_leaf.leaf_connections[leaf_connection_index].nonconnection_total += (float)polygon_area;
                    }
                }
            }
        }
        public double polygon_get_area(polygon poly)
        {
            RealPlane3d poly_plane = Bsp.Planes[poly.plane_index].Value;
            double area = 0.0f;
            for (var i = 0; i < poly.vertices.Count - 2; i++)
            {
                RealPoint3d v21 = poly.vertices[i + 1] - poly.vertices[0];
                RealPoint3d v31 = poly.vertices[i + 2] - poly.vertices[0];

                RealVector3d vector = cross_product_3d(point_to_vector(v31), point_to_vector(v21));

                //cross product of two vectors times 0.5
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
    }
}
