using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Geometry;
using System.Linq;
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
        private List<Face> Faces { get; set; }
        private List<triangle> Triangles { get; set; }
        private bool debug = false;
        private bool forceimport = false;
        private int max_surface_edges = 8;
        private bool buildmopp = false;
        private bool isobj = false;

        public ImportCollisionGeometryCommand(GameCacheHaloOnlineBase cache)
            : base(false,

                  "ImportCollisionGeometry",
                  "Collision geometry import command",

                  "ImportCollisionGeometry [mopp] [force] [debug] <filepath> <tagname>",
                  
                  "Import an obj file as a collision model tag. Use the mopp argument for mopp generation" +
                  ", and use the force argument to force import the collision geometry even if it has open edges")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            string filepath = "";
            int maxindex = Cache.TagCacheGenHO.Tags.Count;
            string tagName = $"newcoll{maxindex}";

            var argStack = new Stack<string>(args.AsEnumerable().Reverse());

            //Arguments needed: <filepath> <tagname>
            if (args.Count < 2 || args.Count > 5)
                return new TagToolError(CommandError.ArgCount);

            while(argStack.Count > 0)
            {
                var arg = argStack.Peek();
                switch (arg.ToLower())
                {
                    case "mopp":
                        buildmopp = true;
                        max_surface_edges = 3;
                        argStack.Pop();
                        break;
                    case "force":
                        forceimport = true;
                        argStack.Pop();
                        break;
                    case "debug":
                        debug = true;
                        argStack.Pop();
                        break;
                    default:
                        if(argStack.Count > 2)
                            return new TagToolError(CommandError.ArgInvalid);
                        else if (argStack.Count < 2)
                            return new TagToolError(CommandError.ArgCount, "filepath and tagname arguments are required!");
                        else
                        {
                            filepath = argStack.Pop();
                            tagName = argStack.Pop();
                        }
                        break;
                }
            }

            CachedTag tag;

            //the obj format seems to use different axes by default, adjust debug printouts 
            if (filepath.ToLower().EndsWith(".obj"))
                isobj = true;

            //check inputs
            if(Cache.TagCache.TryGetTag(tagName + ".coll", out tag))
                return new TagToolError(CommandError.OperationFailed, "Selected TagName already exists in the cache!");
            //if (!Directory.Exists(filepath))
            //    return new TagToolError(CommandError.FileNotFound);

            //import the mesh and get the vertices and indices
            Scene model;
            using (var importer = new AssimpContext())
            {
                using (var logStream = new LogStream((msg, userData) => Console.Write(msg)))
                {
                    logStream.Attach();
                    model = importer.ImportFile(filepath,
                        PostProcessSteps.OptimizeMeshes |
                        PostProcessSteps.RemoveComponent |
                        PostProcessSteps.Debone |
                        PostProcessSteps.OptimizeGraph |
                        PostProcessSteps.Triangulate);
                    logStream.Detach();
                }
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

            collisionModel.Nodes = new List<CollisionModel.Node>()
            {
                new CollisionModel.Node()
                {
                    Name = Cache.StringTable.GetStringId("default"),
                    ParentNode = -1,
                    NextSiblingNode = -1,
                    FirstChildNode = -1
                }
            };

            collisionModel.Materials = new List<CollisionModel.Material>();
            foreach(Assimp.Material material in model.Materials)
            {
                //note: if the material name string does not reflect a stringid currently in the cache, it'll be set to a null stringid
                collisionModel.Materials.Add(new CollisionModel.Material
                {
                    Name = Cache.StringTable.GetStringId(material.Name)
                });
            }

            //begin building the collision geometry
            collisionModel.Regions[0].Permutations[0].Bsps.Add(new CollisionModel.Region.Permutation.Bsp());
            collisionModel.Regions[0].Permutations[0].Bsps[0].Geometry = new CollisionGeometry
            {
                Bsp3dNodes = new TagBlock<Bsp3dNode>(),
                Planes = new TagBlock<TagTool.Geometry.BspCollisionGeometry.Plane>(),
                Surfaces = new TagBlock<Surface>(),
                Vertices = new TagBlock<Vertex>(),
                Bsp2dNodes = new TagBlock<Bsp2dNode>(),
                Bsp2dReferences = new TagBlock<Bsp2dReference>(),
                Edges = new TagBlock<Edge>(),
                Leaves = new TagBlock<Leaf>()
            };
            Bsp = collisionModel.Regions[0].Permutations[0].Bsps[0].Geometry;

            //loop through each mesh and add the triangles to the global list
            Triangles = new List<triangle>();
            foreach (Assimp.Mesh currentmesh in model.Meshes)
            {
                Vertices = currentmesh.Vertices;
                Faces = currentmesh.Faces;

                if(debug)
                    Console.WriteLine($"Mesh {currentmesh.Name} has {Faces.Count} Faces!");

                add_triangles(currentmesh.MaterialIndex);
            }

            //this code calculates the ?perimeter of each triangle, and sorts them. 
            //This will effect the efficiency of bsp generation later --
            //as it is better to use smaller surface planes to split larger surfaces rather than vice versa
            triangle_list_qsort_compar sorter = new triangle_list_qsort_compar();
            Triangles.Sort(sorter);

            if (!collision_geometry_add_surfaces() || !collision_geometry_check_for_open_edges() || !reduce_collision_geometry())
            {
                Console.WriteLine("### Failed to import collision geometry!");
                return false;
            }

            if (!forceimport)
            {
                if (!verify_collision_geometry())
                {
                    Console.WriteLine($"###Failed to verify collision geometry!");
                    return false;
                }
            }

            //build the collision bsp
            GenerateCollisionBSPCommand bsp_builder = new GenerateCollisionBSPCommand(ref collisionModel);
            if (!bsp_builder.generate_bsp(0, 0, 0))
                return false;

            if (buildmopp)
            {
                GenerateBspPhysicsCommand moppgenerator = new GenerateBspPhysicsCommand(ref collisionModel);
                moppgenerator.Bsp = collisionModel.Regions[0].Permutations[0].Bsps[0].Geometry;
                CollisionModel.Region.Permutation temp_permutation = collisionModel.Regions[0].Permutations[0];
                if(!moppgenerator.generate_mopp_codes(ref temp_permutation))
                {
                    Console.WriteLine("ERROR: Failed to build mopps!");
                    return false;
                }
                else
                {
                    Console.WriteLine("Mopps built successfully!");
                    collisionModel.Regions[0].Permutations[0] = temp_permutation;
                }
            }

            tag = Cache.TagCacheGenHO.AllocateTag(Cache.TagCache.TagDefinitions.GetTagDefinitionType("coll"), tagName);

            //write out the tag
            using (var stream = Cache.OpenCacheReadWrite())
            {
                Cache.Serialize(stream, tag, collisionModel);
            }
            Cache.SaveStrings();
            Cache.SaveTagNames();

            Console.WriteLine($"Successfully imported collision model to: {tag.Name}.coll");

            return true;
        }

        public struct triangle
        {
            public Vector3D a;
            public Vector3D b;
            public Vector3D c;
            public int material_index;
            public float sorting_parameter;
        }

        public class triangle_list_qsort_compar : IComparer<triangle>
        {
            public int Compare(triangle element1, triangle element2)
            {
                int result = 0;
                float v2 = element2.sorting_parameter;
                if (element1.sorting_parameter <= v2)
                    result = v2 > element1.sorting_parameter ? 1 : 0;
                else
                    result = -1;
                return result;
            }
        }

        public bool add_triangles(int materialindex)
        {
            for (int i = 0; i < Faces.Count; i++)
            {
                List<int> indices = Faces[i].Indices;
                triangle newtriangle = new triangle{ a = Vertices[indices[0]], b = Vertices[indices[1]], c = Vertices[indices[2]], material_index = materialindex};
                Vector3D point0 = Vertices[indices[0]];
                Vector3D point1 = Vertices[indices[1]];
                Vector3D point2 = Vertices[indices[2]];
                float xdiff_1_0 = point1.X - point0.X;
                float ydiff_1_0 = point1.Y - point0.Y;
                float zdiff_1_0 = point1.Z - point0.Z;
                float xdiff_2_0 = point2.X - point0.X;
                float ydiff_2_0 = point2.Y - point0.Y;
                float zdiff_2_0 = point2.Z - point0.Z;

                float v11 = xdiff_1_0 * ydiff_2_0 - xdiff_2_0 * ydiff_1_0;
                float v12 = xdiff_1_0 * ydiff_2_0 - xdiff_2_0 * ydiff_1_0;
                float v13 = zdiff_2_0 * ydiff_1_0 - ydiff_2_0 * zdiff_1_0;
                float v14 = zdiff_1_0 * xdiff_2_0 - zdiff_2_0 * xdiff_1_0;

                newtriangle.sorting_parameter = (float)Math.Sqrt((v11 * v12 + v14 * v14 + v13 * v13) * 0.5);
                Triangles.Add(newtriangle);
            }

            return true;
        }

        public int add_vertex(Vector3D vertex)
        {
            //the Y and Z axes are swapped in Halo, and the Y axis is also flipped, don't ask me why
            //we also need to scale everything down by 1/100 to account for editing software conventions
            Vertex newvertex = new Vertex { Point = new TagTool.Common.RealPoint3d { X = vertex.X * 0.01f, Y = vertex.Y * 0.01f, Z = vertex.Z * 0.01f }, FirstEdge = ushort.MaxValue };
            for(int i = 0; i < Bsp.Vertices.Count; i++)
            {
                Vertex testvertex = Bsp.Vertices[i];
                if (newvertex.Point == testvertex.Point)
                    return i;
            }
            if(Bsp.Vertices.Count >= ushort.MaxValue)
            {
                Console.WriteLine("###ERROR: Can only support up to 65535 unique vertices!");
                return -1;
            }
            Bsp.Vertices.Add(newvertex);
            return Bsp.Vertices.Count - 1;
        }

        public bool collision_geometry_add_surfaces()
        {
            foreach(triangle newtriangle in Triangles)
            {
                if (Bsp.Surfaces.Count >= ushort.MaxValue)
                {
                    Console.WriteLine("###ERROR: Can only support up to 65535 unique surfaces!");
                    return false;
                }
                Bsp.Surfaces.Add(new Surface());
                int surface_index = Bsp.Surfaces.Count - 1;
                Bsp.Surfaces[surface_index].Plane = ushort.MaxValue;
                Bsp.Surfaces[surface_index].MaterialIndex = (short)newtriangle.material_index;

                int point0 = add_vertex(newtriangle.a);
                int point1 = add_vertex(newtriangle.b);
                int point2 = add_vertex(newtriangle.c);

                if (point0 == -1 || point1 == -1 || point2 == -1)
                    return false;

                int edge_index0 = collision_geometry_add_edge(point0, point1, surface_index);
                int edge_index1 = collision_geometry_add_edge(point1, point2, surface_index);
                int edge_index2 = collision_geometry_add_edge(point2, point0, surface_index);

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
                    Bsp.Edges[edge_index0].ReverseEdge = (ushort)edge_index1;
                else
                    Bsp.Edges[edge_index0].ForwardEdge = (ushort)edge_index1;
                if (surface_right_of_edge_1)
                    Bsp.Edges[edge_index1].ReverseEdge = (ushort)edge_index2;
                else
                    Bsp.Edges[edge_index1].ForwardEdge = (ushort)edge_index2;
                if (surface_right_of_edge_2)
                    Bsp.Edges[edge_index2].ReverseEdge = (ushort)edge_index0;
                else
                    Bsp.Edges[edge_index2].ForwardEdge = (ushort)edge_index0;

                //set point first edges
                if (Bsp.Vertices[point0].FirstEdge == ushort.MaxValue)
                    Bsp.Vertices[point0].FirstEdge = (ushort)edge_index0;
                if (Bsp.Vertices[point1].FirstEdge == ushort.MaxValue)
                    Bsp.Vertices[point1].FirstEdge = (ushort)edge_index1;
                if (Bsp.Vertices[point2].FirstEdge == ushort.MaxValue)
                    Bsp.Vertices[point2].FirstEdge = (ushort)edge_index2;

                //generate surface plane
                if (!generate_surface_plane(surface_index))
                {
                    return false;
                }
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
                        Console.WriteLine($"###ERROR: Edge between the following vertices is contacted by more than two surfaces!!");
                        List<RealPoint3d> debugvertices = new List<RealPoint3d> { Bsp.Vertices[point0_index].Point, Bsp.Vertices[point1_index].Point };
                        debug_print_vertices(debugvertices);
                        return -1;
                    }
                }
                if (Bsp.Edges[edge_index].StartVertex == point0_index &&
                    Bsp.Edges[edge_index].EndVertex == point1_index)
                {
                    Console.WriteLine($"###ERROR: Edge between the following vertices is contacted by more than two surfaces!!");
                    List<RealPoint3d> debugvertices = new List<RealPoint3d> { Bsp.Vertices[point0_index].Point, Bsp.Vertices[point1_index].Point };
                    debug_print_vertices(debugvertices);
                    return -1;
                }
            }

            if (Bsp.Edges.Count >= ushort.MaxValue)
            {
                Console.WriteLine("###ERROR: Can only support up to 65535 unique edges!");
                return -1;
            }

            Bsp.Edges.Add(new Edge
            {
                StartVertex = (ushort)point0_index,
                EndVertex = (ushort)point1_index,
                LeftSurface = (ushort)surface_index,
                RightSurface = ushort.MaxValue,
                ForwardEdge = ushort.MaxValue,
                ReverseEdge = ushort.MaxValue
            });
            int matching_edge_index = Bsp.Edges.Count - 1;

            return matching_edge_index;
        }

        public bool collision_geometry_check_for_open_edges()
        {
            bool result = true;
            for(int edge_index = 0; edge_index < Bsp.Edges.Count; edge_index++)
            {
                Edge edge = Bsp.Edges[edge_index];
                if(edge.RightSurface == ushort.MaxValue)
                {
                    if (!forceimport)
                    {
                        Console.WriteLine($"###ERROR: Edge with below vertices is open!");
                        List<RealPoint3d> debugvertices = new List<RealPoint3d> { Bsp.Vertices[edge.StartVertex].Point, Bsp.Vertices[edge.EndVertex].Point };
                        debug_print_vertices(debugvertices);
                        result = false;
                    }
                    else
                    {
                        Console.WriteLine($"###WARNING: This mesh contains open edges which may lead to collision errors!" +
                            $" You have enabled the 'force' argument so import will proceed regardless!");
                        return true;
                    }
                }
            }
            return result;
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
            if (max_surface_edges == 3)
                return true;

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

                                    int next_edge_index_A = loop_direction_is_reverse ? edge_block.ForwardEdge : edge_block.ReverseEdge;
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
            Console.WriteLine($"Successfully removed {surfaces_removed} surfaces and {edges_removed} edges!");

            return true;
        }

        ///////////////////////////////////////////
        //plane generation stuff stored down here
        ///////////////////////////////////////////

        public bool generate_surface_plane(int surface_index)
        {
            List<RealPoint3d> pointlist = new List<RealPoint3d>();
            Surface surface_block = Bsp.Surfaces[surface_index];

            int surface_edge_index = surface_block.FirstEdge;
            //collect vertices on the plane
            while (true)
            {
                Edge surface_edge_block = Bsp.Edges[surface_edge_index];
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
            if (pointlist.Count < 3)
            {
                Console.WriteLine("###ERROR: Surface has less than 3 points!");
                return false;
            }
            if (!plane_generation_points_valid(pointlist[0], pointlist[1], pointlist[2]))
            {
                Console.WriteLine("###ERROR: Surface has overlapping vertices! (distance < 0.0001)");
                Console.WriteLine("#Make sure that your model is scaled properly and not too detailed!");
                Console.WriteLine("#If your model is scaled properly, try merging vertices by distance");
                debug_print_vertices(pointlist);
                return false;
            }
            else
            {
                int plane_index = -1;
                for (int test_plane_index = 0; test_plane_index < Bsp.Planes.Count; test_plane_index++)
                {
                    RealPlane3d existing_plane = Bsp.Planes[test_plane_index].Value;
                    if (plane_test_points(existing_plane, pointlist))
                    {
                        plane_index = test_plane_index;
                        if (plane_is_mirrored(pointlist[0], pointlist[1], pointlist[2], existing_plane))
                            Bsp.Surfaces[surface_index].Plane = (ushort)(plane_index | 0x8000);
                        else
                            Bsp.Surfaces[surface_index].Plane = (ushort)plane_index;
                        break;
                    }
                }
                if (plane_index == -1)
                {
                    RealPlane3d newplane = generate_plane_from_3_points(pointlist[0], pointlist[1], pointlist[2]);
                    if (plane_test_points(newplane, new List<RealPoint3d> { pointlist[0], pointlist[1], pointlist[2] }))
                    {
                        Bsp.Planes.Add(new TagTool.Geometry.BspCollisionGeometry.Plane { Value = newplane });
                        plane_index = Bsp.Planes.Count - 1;
                        Bsp.Surfaces[surface_index].Plane = (ushort)plane_index;
                    }
                    else
                    {
                        Console.WriteLine("###ERROR: Did not produce valid plane from points!");
                        return false;
                    }                                        
                }
            }
            return true;
        }

        public void debug_print_vertices(List<RealPoint3d> vertexlist)
        {
            if (!isobj)
            {
                foreach (RealPoint3d vertex in vertexlist)
                {
                    Console.WriteLine($"{vertex * 100.0f}");
                }
            }
            else
            {
                Console.WriteLine($"#NOTE: The below coordinates are fixed for OBJ convention!");
                foreach (RealPoint3d vertex in vertexlist)
                {
                    RealPoint3d vertex_fix = new RealPoint3d { X = vertex.X, Y = -vertex.Z, Z = vertex.Y };
                    Console.WriteLine($"{vertex_fix * 100.0f}");
                }
            }
        }

        public bool generate_surface_planes()
        {
            for (var surface_index = 0; surface_index < Bsp.Surfaces.Count; surface_index++)
            {
                List<RealPoint3d> pointlist = new List<RealPoint3d>();
                Surface surface_block = Bsp.Surfaces[surface_index];

                int surface_edge_index = surface_block.FirstEdge;
                //collect vertices on the plane
                while (true)
                {
                    Edge surface_edge_block = Bsp.Edges[surface_edge_index];
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
                if (pointlist.Count < 3)
                {
                    Console.WriteLine("###ERROR: Not enough points to generate a plane!");
                    return false;
                }
                else
                {
                    int plane_index = -1;
                    for(int test_plane_index = 0; test_plane_index < Bsp.Planes.Count; test_plane_index++)
                    {
                        RealPlane3d existing_plane = Bsp.Planes[test_plane_index].Value;
                        if (plane_test_points(existing_plane, pointlist))
                        {
                            plane_index = Bsp.Planes.IndexOf(existing_plane);
                            if (plane_is_mirrored(pointlist[0], pointlist[1], pointlist[2], existing_plane))
                                Bsp.Surfaces[surface_index].Plane = (ushort)(plane_index | 0x8000);
                            else
                                Bsp.Surfaces[surface_index].Plane = (ushort)plane_index;
                            break;
                        }
                    }
                    if (plane_index == -1)
                    {
                        int count = 0;
                        while (true)
                        {
                            if (plane_generation_points_valid(pointlist[count], pointlist[count + 1], pointlist[count + 2]))
                            {
                                RealPlane3d newplane = generate_plane_from_3_points(pointlist[count], pointlist[count + 1], pointlist[count + 2]);
                                if (plane_test_points(newplane, new List<RealPoint3d> { pointlist[count], pointlist[count + 1], pointlist[count + 2] }))
                                {
                                    Bsp.Planes.Add(new TagTool.Geometry.BspCollisionGeometry.Plane { Value = newplane });
                                    plane_index = Bsp.Planes.Count - 1;
                                    Bsp.Surfaces[surface_index].Plane = (ushort)plane_index;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("###ERROR: Did not produce valid plane from points!");
                                    return false;
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
                }
            }
            return true;
        }

        public bool plane_test_points(RealPlane3d plane, List<RealPoint3d> pointlist)
        {
            foreach (var point in pointlist)
            {
                double plane_fit = point.X * plane.I + point.Y * plane.J + point.Z * plane.K - plane.D;
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

        //////////////////////////
        //VERIFICATION CHECKS
        ///////////////////////////
        public bool verify_collision_geometry()
        {
            if (Bsp.Edges.Count > 0 && Bsp.Surfaces.Count > 0)
            {
                for (int edge_index = 0; edge_index < Bsp.Edges.Count; edge_index++)
                {
                    Edge edge_block = Bsp.Edges[edge_index];
                    if (edge_block.StartVertex < 0 || edge_block.StartVertex >= Bsp.Vertices.Count)
                    {
                        Console.WriteLine($"###ERROR edge {edge_index} has a bad start vertex index.");
                        return false;
                    }
                    if (edge_block.EndVertex < 0 || edge_block.EndVertex >= Bsp.Vertices.Count)
                    {
                        Console.WriteLine($"###ERROR edge {edge_index} has a bad end vertex index.");
                        return false;
                    }
                    if (edge_block.StartVertex == edge_block.EndVertex)
                    {
                        Console.WriteLine($"###ERROR edge {edge_index} references only one vertex.");
                        return false;
                    }
                    if (edge_get_length(edge_index) < 0.001)
                    {
                        Console.WriteLine($"###ERROR edge {edge_index} is too short.");
                        return false;
                    }
                    if (edge_block.ForwardEdge < 0 || edge_block.ForwardEdge >= Bsp.Edges.Count)
                    {
                        Console.WriteLine($"###ERROR edge {edge_index} has a bad forward edge index.");
                        return false;
                    }
                    if (edge_block.ReverseEdge < 0 || edge_block.ReverseEdge >= Bsp.Edges.Count)
                    {
                        Console.WriteLine($"###ERROR edge {edge_index} has a bad reverse edge index.");
                        return false;
                    }
                    if (edge_block.ForwardEdge == edge_index || edge_block.ReverseEdge == edge_index)
                    {
                        Console.WriteLine($"###ERROR edge {edge_index} references itself.");
                        return false;
                    }
                    if (edge_block.ForwardEdge == edge_block.ReverseEdge)
                    {
                        Console.WriteLine($"###ERROR edge {edge_index} references only one edge.");
                        return false;
                    }
                    for (int direction = 0; direction < 2; direction++)
                    {
                        int next_edge_index = direction == 0 ? edge_block.ForwardEdge : edge_block.ReverseEdge;
                        Edge next_edge_block = Bsp.Edges[next_edge_index];
                        int test_vertex = direction == 0 ? edge_block.EndVertex : edge_block.StartVertex;
                        int test_surface = direction == 0 ? edge_block.LeftSurface : edge_block.RightSurface;
                        if ((next_edge_block.StartVertex != test_vertex || next_edge_block.LeftSurface != test_surface)
                         && (next_edge_block.EndVertex != test_vertex || next_edge_block.RightSurface != test_surface))
                        {
                            string directionstring = direction == 0 ? "forward" : "reverse";
                            Console.WriteLine($"###ERROR edge {edge_index} doesn't share a vertex or surface with its {directionstring} edge.");
                            return false;
                        }
                    }
                    if (edge_block.LeftSurface < 0 || edge_block.LeftSurface >= Bsp.Surfaces.Count)
                    {
                        Console.WriteLine($"###ERROR edge {edge_index} has a bad left surface index.");
                        return false;
                    }
                    if (edge_block.RightSurface < 0 || edge_block.RightSurface >= Bsp.Surfaces.Count)
                    {
                        Console.WriteLine($"###ERROR edge {edge_index} has a bad right surface index.");
                        return false;
                    }
                    if (edge_block.LeftSurface == edge_block.RightSurface)
                    {
                        Console.WriteLine($"###ERROR edge {edge_index} references only one surface.");
                        return false;
                    }
                }
                for (int surface_index = 0; surface_index < Bsp.Surfaces.Count; surface_index++)
                {
                    Surface surface_block = Bsp.Surfaces[surface_index];
                    if (surface_block.FirstEdge < 0 || surface_block.FirstEdge >= Bsp.Edges.Count)
                    {
                        Console.WriteLine($"###ERROR surface {surface_index} has a bad first edge index");
                        return false;
                    }
                    if (surface_count_edges(surface_index) > 8)
                    {
                        Console.WriteLine($"###ERROR surface {surface_index} has too many edges");
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}
