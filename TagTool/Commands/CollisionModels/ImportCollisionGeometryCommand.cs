using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Geometry;
using TagTool.Tags;
using TagTool.Geometry.BspCollisionGeometry;
using Assimp;
using TagTool.Tags.Definitions;
using TagTool.Commands.CollisionModels;

namespace TagTool.Commands.CollisionModels
{
    class ImportCollisionGeometryCommand : Command
    {
        private GameCacheHaloOnlineBase Cache { get; }
        private CollisionGeometry Bsp { get; set; }
        private List<Assimp.Vector3D> Vertices { get; set; }
        private int[] Indices { get; set; }
        private bool debug = false;
        private int max_surface_edges = 8;

        public ImportCollisionGeometryCommand(GameCacheHaloOnlineBase cache)
            : base(false,

                  "ImportCollisionGeometry",
                  "Collision geometry import command",

                  "ImportCollisionGeometry <filepath> <tagname>",
                  
                  "Import an obj file as a collision model tag")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            //Arguments needed: <filepath> <tagname>
            if (args.Count < 2)
                return new TagToolError(CommandError.ArgCount);

            var filepath = args[0];
            var tagName = args[1];

            CachedTag tag;

            //check inputs
            if(Cache.TagCache.TryGetTag(tagName, out tag))
                return new TagToolError(CommandError.OperationFailed, "Selected TagName already exists in the cache!");
            if (!Path.GetExtension(filepath).Equals(".obj"))
                return new TagToolError(CommandError.FileNotFound);
            if (!Directory.Exists(filepath))
                return new TagToolError(CommandError.FileType);

            tag = Cache.TagCacheGenHO.AllocateTag(Cache.TagCache.TagDefinitions.GetTagDefinitionType("coll"), tagName);

            //import the mesh and get the vertices and indices
            using (var importer = new AssimpContext())
            {
                Scene model;

                using (var logStream = new LogStream((msg, userData) => Console.WriteLine(msg)))
                {
                    logStream.Attach();
                    model = importer.ImportFile(filepath,
                        PostProcessSteps.OptimizeMeshes |
                        PostProcessSteps.FindDegenerates |
                        PostProcessSteps.OptimizeGraph |
                        PostProcessSteps.JoinIdenticalVertices |
                        PostProcessSteps.PreTransformVertices |
                        PostProcessSteps.Triangulate);
                    logStream.Detach();
                }

                if (model.Meshes.Count > 1)
                    Console.WriteLine("###WARNING: More than one mesh found, only the first mesh will be used.");

                Indices = model.Meshes[0].GetIndices();
                Vertices = model.Meshes[0].Vertices;
            }

            //set up collision model tag
            var collisionModel = new CollisionModel();
            collisionModel.Regions = new List<CollisionModel.Region>();

            var permutation = new CollisionModel.Region.Permutation()
            {
                Name = Cache.StringTable.GetStringId("default"),
                Bsps = new List<CollisionModel.Region.Permutation.Bsp>()
            };

            collisionModel.Regions = new List<CollisionModel.Region>()
                {
                    new CollisionModel.Region()
                    {
                        Name = Cache.StringTable.GetStringId("default"),
                        Permutations = new List<CollisionModel.Region.Permutation>() { permutation }
                    }
                };
           
            //begin building the collision geometry
            Bsp = collisionModel.Regions[0].Permutations[0].Bsps[0].Geometry;
            if (!collision_geometry_add_surfaces() || !collision_geometry_check_for_open_edges() || !reduce_collision_geometry())
            {
                Console.WriteLine("### Failed to import collision geometry!");
                return false;
            }

            //build the collision bsp
            GenerateCollisionBSPCommand bsp_builder = new GenerateCollisionBSPCommand(ref collisionModel);
            if (!bsp_builder.generate_bsp(0, 0, 0))
                return false;
            
            //write out the tag
            using (var stream = Cache.OpenCacheReadWrite())
            {
                Cache.Serialize(stream, tag, collisionModel);
            }

            Console.Write($"Successfully imported coll to: {tag.Name}");
            TagPrinter.PrintTagShort(tag);

            return true;
        }

        public bool collision_geometry_add_surfaces()
        {
            foreach (var vertex in Vertices)
            {
                Bsp.Vertices.Add(new Vertex { Point = new TagTool.Common.RealPoint3d { X = vertex.X, Y = vertex.Y, Z = vertex.Z }, FirstEdge = ushort.MaxValue });
            }
            int index_buffer_index = 0;
            List<int> triangle = new List<int>();
            while (index_buffer_index < Indices.Length)
            {
                triangle.Add(Indices[index_buffer_index]);
                triangle.Add(Indices[index_buffer_index + 1]);
                triangle.Add(Indices[index_buffer_index + 2]);
                index_buffer_index += 3;
                Bsp.Surfaces.Add(new Surface {BreakableSurfaceIndex = -1});
                int surface_index = Bsp.Surfaces.Count;
                Bsp.Surfaces[surface_index].Plane = ushort.MaxValue;

                int edge_index0 = collision_geometry_add_edge(triangle[0], triangle[1], surface_index);
                int edge_index1 = collision_geometry_add_edge(triangle[1], triangle[2], surface_index);
                int edge_index2 = collision_geometry_add_edge(triangle[2], triangle[0], surface_index);

                if (edge_index0 == -1 || edge_index1 == -1 || edge_index2 == -1)
                    return false;

                //find the edge with the lowest edge index, this will be the surface first edge
                int first_edge_index;
                int v29 = edge_index2;
                if (edge_index1 <= edge_index2)
                    v29 = edge_index1;
                if (edge_index0 <= v29)
                    first_edge_index = edge_index0;               
                else if (edge_index1 <= edge_index2)
                    first_edge_index = edge_index1;
                else
                    first_edge_index = edge_index2;
                Bsp.Surfaces[surface_index].FirstEdge = (ushort)first_edge_index;

                //assign forward and reverse edges
                bool surface_right_of_edge_0 = Bsp.Edges[edge_index0].RightSurface == surface_index;
                bool surface_right_of_edge_1 = Bsp.Edges[edge_index1].RightSurface == surface_index;
                bool surface_right_of_edge_2 = Bsp.Edges[edge_index2].RightSurface == surface_index;

                if (surface_right_of_edge_0)
                    Bsp.Edges[edge_index0].ForwardEdge = (ushort)edge_index1;
                else
                    Bsp.Edges[edge_index0].ReverseEdge = (ushort)edge_index1;
                if (surface_right_of_edge_1)
                    Bsp.Edges[edge_index1].ForwardEdge = (ushort)edge_index2;
                else
                    Bsp.Edges[edge_index1].ReverseEdge = (ushort)edge_index2;
                if (surface_right_of_edge_2)
                    Bsp.Edges[edge_index2].ForwardEdge = (ushort)edge_index0;
                else
                    Bsp.Edges[edge_index2].ReverseEdge = (ushort)edge_index0;

                //set point first edges
                if (Bsp.Vertices[triangle[0]].FirstEdge == ushort.MaxValue)
                    Bsp.Vertices[triangle[0]].FirstEdge = (ushort)edge_index0;
                if (Bsp.Vertices[triangle[1]].FirstEdge == ushort.MaxValue)
                    Bsp.Vertices[triangle[1]].FirstEdge = (ushort)edge_index1;
                if (Bsp.Vertices[triangle[2]].FirstEdge == ushort.MaxValue)
                    Bsp.Vertices[triangle[2]].FirstEdge = (ushort)edge_index2;
            }
            return true;
        }

        public int collision_geometry_add_edge(int point0_index, int point1_index, int surface_index)
        {
            for(int edge_index = 0; edge_index < Bsp.Edges.Count; edge_index++)
            {
                if(Bsp.Edges[edge_index].StartVertex == point1_index &&
                    Bsp.Edges[edge_index].EndVertex == point0_index)
                {
                    if(Bsp.Edges[edge_index].RightSurface == ushort.MaxValue)
                    {
                        Bsp.Edges[edge_index].RightSurface = (ushort)surface_index;
                        return edge_index;
                    }
                    else
                    {
                        Console.WriteLine($"###ERROR: Edge Index {edge_index} is degenerate!!");
                        return -1;
                    }
                }
                if (Bsp.Edges[edge_index].StartVertex == point0_index &&
                    Bsp.Edges[edge_index].EndVertex == point1_index)
                {
                    Console.WriteLine($"###ERROR: Edge Index {edge_index} is degenerate!!");
                    return -1;
                }
            }

            int matching_edge_index = Bsp.Edges.Count;
            Bsp.Edges.Add(new Edge
            {
                StartVertex = (ushort)point0_index,
                EndVertex = (ushort)point1_index,
                LeftSurface = (ushort)surface_index,
                RightSurface = ushort.MaxValue,
                ForwardEdge = ushort.MaxValue,
                ReverseEdge = ushort.MaxValue
            });
            
            return matching_edge_index;
        }

        public bool collision_geometry_check_for_open_edges()
        {
            for(int edge_index = 0; edge_index < Bsp.Edges.Count; edge_index++)
            {
                Edge edge = Bsp.Edges[edge_index];
                if(edge.RightSurface == ushort.MaxValue)
                {
                    Console.WriteLine($"###ERROR: Edge {edge_index} is open!");
                    return false;
                }
            }
            return true;
        }

        public bool check_plane_projection_parameter_greater_than_0(TagTool.Geometry.BspCollisionGeometry.Plane plane_block, int projection_axis)
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

        public int plane_determine_axis_minimum_coefficient(TagTool.Geometry.BspCollisionGeometry.Plane plane_block)
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

        int surface_count_edges(int surface_index)
        {
            int edge_count = 0;
            Surface surface_block = Bsp.Surfaces[surface_index];
            int first_Edge_index = surface_block.FirstEdge;
            int current_edge_index = surface_block.FirstEdge;
            do
            {
                Edge edge_block = Bsp.Edges[current_edge_index];
                ++edge_count;
                if (edge_block.RightSurface == surface_index)
                    current_edge_index = edge_block.ReverseEdge;
                else
                    current_edge_index = edge_block.ForwardEdge;
            }
            while (current_edge_index != first_Edge_index);
            return edge_count;
        }

        public bool reduce_collision_geometry()
        {
            if (Bsp.Edges.Count > 0 && Bsp.Surfaces.Count > 0)
            {
                List<edge_array_element> edge_array = new List<edge_array_element>();
                List<int> edge_deleted_table = new List<int>(new int[Bsp.Edges.Count]);
                List<int> surface_deleted_table = new List<int>(new int[Bsp.Surfaces.Count]);

                //allocate a list of edges with the length of each edge, and sort them
                for (var edge_index = 0; edge_index < Bsp.Edges.Count; edge_index++)
                {
                    edge_array.Add(new edge_array_element { edge_index = edge_index, edge_length = edge_get_length(edge_index) });
                }
                edge_array_qsort_compar sorter = new edge_array_qsort_compar();
                edge_array.Sort(sorter);

                //loop through edge array
                for (var edge_array_element_index = 0; edge_array_element_index < edge_array.Count; edge_array_element_index++)
                {
                    int edge_index = edge_array[edge_array_element_index].edge_index;
                    Edge edge_block = Bsp.Edges[edge_index];
                    int leftsurface_index = edge_block.LeftSurface;
                    int rightsurface_index = edge_block.RightSurface;

                    int surface_A = rightsurface_index;
                    int surface_B = leftsurface_index;

                    if (leftsurface_index <= rightsurface_index)
                    {
                        surface_A = leftsurface_index;
                        surface_B = rightsurface_index;
                    }

                    //check to make sure the surface indices are not null
                    if ((ushort)surface_A != ushort.MaxValue && (ushort)surface_B != ushort.MaxValue)
                    {
                        Surface surface_A_block = Bsp.Surfaces[surface_A];
                        Surface surface_B_block = Bsp.Surfaces[surface_B];

                        if (surface_A_block.Plane == surface_B_block.Plane && surface_A_block.BreakableSurfaceIndex == surface_B_block.BreakableSurfaceIndex)
                        {
                            //we only want to make surfaces with a maximum of 8 edges, so make sure that merging these two surfaces will result in a surface with no more than 8 edges
                            if (surface_count_edges(surface_A) + surface_count_edges(surface_B) - 2 <= max_surface_edges)
                            {
                                TagTool.Geometry.BspCollisionGeometry.Plane surface_plane = Bsp.Planes[surface_A_block.Plane & 0x7FFF];
                                int projection_axis = plane_determine_axis_minimum_coefficient(surface_plane);
                                int plane_mirror_check = check_plane_projection_parameter_greater_than_0(surface_plane, projection_axis) != (surface_A_block.Plane < 0) ? 1 : 0;

                                int loop_direction = 0;
                                while (true)
                                {
                                    bool loop_direction_is_reverse = loop_direction == 0;

                                    int next_edge_index_A = loop_direction_is_reverse ? edge_block.ReverseEdge : edge_block.ForwardEdge;
                                    Edge next_edge_A = Bsp.Edges[next_edge_index_A];
                                    bool next_right_surface_check = next_edge_A.RightSurface == (loop_direction_is_reverse ? edge_block.LeftSurface : edge_block.RightSurface);

                                    int next_edge_index_B = loop_direction_is_reverse ? edge_block.ReverseEdge : edge_block.ForwardEdge;
                                    Edge next_edge_B = Bsp.Edges[next_edge_index_B];

                                    bool surface_is_right_of_edge = false;
                                    while (next_edge_index_B != edge_index)
                                    {
                                        next_edge_B = Bsp.Edges[next_edge_index_B];

                                        surface_is_right_of_edge = loop_direction_is_reverse ? next_edge_B.RightSurface == edge_block.RightSurface : next_edge_B.RightSurface == edge_block.LeftSurface;

                                        next_edge_index_B = surface_is_right_of_edge ? next_edge_B.ReverseEdge : next_edge_B.ForwardEdge;
                                    }

                                    int next_vertex_index = surface_is_right_of_edge ? next_edge_B.EndVertex : next_edge_B.StartVertex;
                                    Vertex next_vertex = Bsp.Vertices[next_vertex_index];
                                    RealPoint2d coords1 = vertex_get_projection_relevant_coords(next_vertex, projection_axis, plane_mirror_check);

                                    int vertex_A_index = next_right_surface_check ? next_edge_A.EndVertex : next_edge_A.StartVertex;
                                    Vertex vertex_block_A = Bsp.Vertices[vertex_A_index];
                                    RealPoint2d coordsA = vertex_get_projection_relevant_coords(vertex_block_A, projection_axis, plane_mirror_check);

                                    int vertex_B_index = next_right_surface_check ? next_edge_A.StartVertex : next_edge_A.EndVertex;
                                    Vertex vertex_block_B = Bsp.Vertices[vertex_B_index];
                                    RealPoint2d coordsB = vertex_get_projection_relevant_coords(vertex_block_B, projection_axis, plane_mirror_check);

                                    if ((coordsB.Y - coordsA.Y) * (coordsA.X - coords1.X) - (coordsA.Y - coords1.Y) * (coordsB.X - coordsA.X) <= 0.000099999997)
                                        break;

                                    if (++loop_direction >= 2)
                                        break;
                                }

                                //if the edge fails the above check, then this code is responsible for changing the references of the related edges and vertices 
                                //it also flags the surface and edge so they can later be deleted
                                if (loop_direction >= 2)
                                {
                                    int loop_direction_A = 0;
                                    int next_edge = edge_block.ForwardEdge;
                                    while (loop_direction_A < 2)
                                    {
                                        bool loop_direction_is_reverse = loop_direction_A == 0;
                                        int next_edge_index = loop_direction_is_reverse ? edge_block.ReverseEdge : edge_block.ForwardEdge;
                                        Edge current_edge = Bsp.Edges[next_edge_index];
                                        bool next_right_surface_matches_current_surface = false;
                                        while (next_edge_index != edge_index)
                                        {
                                            current_edge = Bsp.Edges[next_edge_index];
                                            next_right_surface_matches_current_surface = current_edge.RightSurface == (loop_direction_is_reverse ? edge_block.RightSurface : edge_block.LeftSurface);
                                            if (next_right_surface_matches_current_surface)
                                                current_edge.RightSurface = (ushort)surface_A;
                                            else
                                                current_edge.LeftSurface = (ushort)surface_A;
                                            next_edge_index = next_right_surface_matches_current_surface ? current_edge.ReverseEdge : current_edge.ForwardEdge;
                                        }
                                        if (next_right_surface_matches_current_surface)
                                            current_edge.ReverseEdge = (ushort)next_edge;
                                        else
                                            current_edge.ForwardEdge = (ushort)next_edge;

                                        next_edge = edge_block.ReverseEdge;
                                        loop_direction_A++;
                                    }

                                    if (Bsp.Vertices[edge_block.StartVertex].FirstEdge == edge_index)
                                        Bsp.Vertices[edge_block.StartVertex].FirstEdge = edge_block.ReverseEdge;
                                    if (Bsp.Vertices[edge_block.EndVertex].FirstEdge == edge_index)
                                        Bsp.Vertices[edge_block.EndVertex].FirstEdge = edge_block.ForwardEdge;
                                    if (surface_A_block.FirstEdge == edge_index)
                                    {
                                        int v37 = edge_block.ForwardEdge;
                                        if (v37 > edge_block.ReverseEdge)
                                            v37 = edge_block.ReverseEdge;
                                        surface_A_block.FirstEdge = (ushort)v37;
                                    }
                                    surface_B_block.Plane = ushort.MaxValue;
                                    surface_B_block.FirstEdge = ushort.MaxValue;
                                    surface_B_block.Flags = 0;
                                    surface_B_block.BreakableSurfaceIndex = short.MaxValue;
                                    surface_deleted_table[surface_B] = -1;
                                    edge_block.StartVertex = ushort.MaxValue;
                                    edge_block.EndVertex = ushort.MaxValue;
                                    edge_block.ForwardEdge = ushort.MaxValue;
                                    edge_block.ReverseEdge = ushort.MaxValue;
                                    edge_block.LeftSurface = ushort.MaxValue;
                                    edge_block.RightSurface = ushort.MaxValue;
                                    edge_deleted_table[edge_index] = -1;
                                }
                            }
                        }
                    }
                }
                if (!recompile_collision_geometry(surface_deleted_table, edge_deleted_table))
                {
                    Console.WriteLine("###ERROR: Failed to recompile collision geometry!");
                    return false;
                }
                return true;
            }
            Console.WriteLine("###ERROR: Failed to reduce collision geometry");
            return false;
        }

        public struct edge_array_element
        {
            public int edge_index;
            public float edge_length;
        }

        public float edge_get_length(int edge_index)
        {
            Edge target_edge = Bsp.Edges[edge_index];
            RealPoint3d start_vertex = Bsp.Vertices[target_edge.StartVertex].Point;
            RealPoint3d end_vertex = Bsp.Vertices[target_edge.EndVertex].Point;
            double xdiff = end_vertex.X - start_vertex.X;
            double ydiff = end_vertex.Y - start_vertex.Y;
            double zdiff = end_vertex.Z - start_vertex.Z;
            return (float)Math.Sqrt(xdiff * xdiff + zdiff * zdiff + ydiff * ydiff);
        }

        public class edge_array_qsort_compar : IComparer<edge_array_element>
        {
            public int Compare(edge_array_element element1, edge_array_element element2)
            {
                if (element1.edge_length > element2.edge_length)
                    return -1;
                if (element1.edge_length >= element2.edge_length)
                    return element1.edge_index - element2.edge_index;
                return 1;
            }
        }

        public bool recompile_collision_geometry(List<int> surface_deleted_table, List<int> edge_deleted_table)
        {
            int edge_starting_count = Bsp.Edges.Count.DeepClone();
            int surface_starting_count = Bsp.Surfaces.Count.DeepClone();

            //reindex and cull edges
            int edge_element_count = 0;
            for (int edge_index = 0; edge_index < Bsp.Edges.Count; ++edge_index)
            {
                if (edge_deleted_table[edge_index] != -1)
                {
                    edge_deleted_table[edge_index] = edge_element_count++;
                    if (edge_deleted_table[edge_index] > edge_index)
                    {
                        Console.WriteLine("edge_deleted_table[edge_index]>=edge_index");
                        return false;
                    }
                    Bsp.Edges[edge_deleted_table[edge_index]] = Bsp.Edges[edge_index];
                }
            }
            while (Bsp.Edges.Count > edge_element_count)
            {
                Bsp.Edges.RemoveAt(Bsp.Edges.Count - 1);
            }

            //reindex and cull surfaces
            int surface_element_count = 0;
            for (int surface_index = 0; surface_index < Bsp.Surfaces.Count; ++surface_index)
            {
                if (surface_deleted_table[surface_index] != -1)
                {
                    surface_deleted_table[surface_index] = surface_element_count++;
                    if (surface_deleted_table[surface_index] > surface_index)
                    {
                        Console.WriteLine("surface_deleted_table[surface_index]>=surface_index");
                        return false;
                    }
                    Bsp.Surfaces[surface_deleted_table[surface_index]] = Bsp.Surfaces[surface_index];
                }
            }
            while (Bsp.Surfaces.Count > surface_element_count)
            {
                Bsp.Surfaces.RemoveAt(Bsp.Surfaces.Count - 1);
            }

            //fix vertex first edges
            for (int vertex_index = 0; vertex_index < Bsp.Vertices.Count; vertex_index++)
            {
                if (Bsp.Vertices[vertex_index].FirstEdge != ushort.MaxValue)
                    Bsp.Vertices[vertex_index].FirstEdge = (ushort)edge_deleted_table[Bsp.Vertices[vertex_index].FirstEdge];
            }

            //fix edge references
            for (int edge_index = 0; edge_index < Bsp.Edges.Count; edge_index++)
            {
                if (Bsp.Edges[edge_index].ForwardEdge != ushort.MaxValue)
                    Bsp.Edges[edge_index].ForwardEdge = (ushort)edge_deleted_table[Bsp.Edges[edge_index].ForwardEdge];
                if (Bsp.Edges[edge_index].ReverseEdge != ushort.MaxValue)
                    Bsp.Edges[edge_index].ReverseEdge = (ushort)edge_deleted_table[Bsp.Edges[edge_index].ReverseEdge];
                if (Bsp.Edges[edge_index].LeftSurface != ushort.MaxValue)
                    Bsp.Edges[edge_index].LeftSurface = (ushort)surface_deleted_table[Bsp.Edges[edge_index].LeftSurface];
                if (Bsp.Edges[edge_index].RightSurface != ushort.MaxValue)
                    Bsp.Edges[edge_index].RightSurface = (ushort)surface_deleted_table[Bsp.Edges[edge_index].RightSurface];
            }

            //fix surface first edges
            for (int surface_index = 0; surface_index < Bsp.Surfaces.Count; surface_index++)
            {
                if (Bsp.Surfaces[surface_index].FirstEdge != ushort.MaxValue)
                {
                    int first_edge = edge_deleted_table[Bsp.Surfaces[surface_index].FirstEdge];
                    if (first_edge < 0 || first_edge > Bsp.Edges.Count)
                    {
                        Console.WriteLine("###ERROR: first_edge_index<0 && first_edge_index>bsp->edges.count");
                        return false;
                    }
                    Bsp.Surfaces[surface_index].FirstEdge = (ushort)first_edge;
                }
            }

            int surfaces_removed = surface_starting_count - Bsp.Surfaces.Count;
            int edges_removed = edge_starting_count - Bsp.Edges.Count;
            if (debug)
                Console.WriteLine($"Successfully removed {surfaces_removed} surfaces and {edges_removed} edges!");

            return true;
        }
    }
}
