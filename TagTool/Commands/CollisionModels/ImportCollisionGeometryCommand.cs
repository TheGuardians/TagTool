using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Geometry;
using TagTool.Tags;
using TagTool.Geometry.BspCollisionGeometry;
using Assimp;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.CollisionModels
{
    class ImportCollisionGeometryCommand : Command
    {
        private GameCacheHaloOnlineBase Cache { get; }
        private CollisionGeometry Bsp { get; set; }
        private List<Assimp.Vector3D> Vertices { get; set; }
        private int[] Indices { get; set; }

        public ImportCollisionGeometryCommand(GameCacheHaloOnlineBase cache)
            : base(false,

                  "ImportCollisionGeometry",
                  "Collision geometry import command",

                  "ImportCollisionGeometry <filepath>|<dirpath> <index>|<new> [force]",
                  
                  "Import an obj file as a collision model tag")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            //Arguments needed: filepath, <new>|<tagIndex>
            if (args.Count < 2)
                return new TagToolError(CommandError.ArgCount);

            CachedTag tag;

            // optional argument: forces overwriting of tags that are not type: coll
            var b_force = (args.Count >= 3 && args[2].ToLower().Equals("force"));

            if (args[1].ToLower().Equals("new"))
            {
                tag = Cache.TagCacheGenHO.AllocateTag(Cache.TagCache.TagDefinitions.GetTagDefinitionType("coll"));
            }
            else
            {
                if (!Cache.TagCache.TryGetTag(args[1], out tag))
                    return new TagToolError(CommandError.TagInvalid);
            }

            if (!b_force && !tag.IsInGroup("coll"))
                return new TagToolError(CommandError.ArgInvalid, "Tag to override was not of class- 'coll'. Use third argument- 'force' to inject into this tag.");

            string filepath = args[0];
            string[] fpaths = null;
            CollisionModel coll = null;
            bool b_singleFile = Path.GetExtension(filepath).Equals(".model_collision_geometry")
                && !Directory.Exists(filepath);

            //import the mesh and get the vertices and indices
            using (var importer = new AssimpContext())
            {
                Scene model;

                using (var logStream = new LogStream((msg, userData) => Console.WriteLine(msg)))
                {
                    logStream.Attach();
                    model = importer.ImportFile(args[0],
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

            //begin building the collision geometry
            Bsp = new CollisionGeometry();
            collision_geometry_add_surfaces();

            using (var stream = Cache.OpenCacheReadWrite())
            {
                Cache.Serialize(stream, tag, coll);
            }

            Console.Write("Successfully imported coll to: ");
            TagPrinter.PrintTagShort(tag);

            return true;
        }

        public void collision_geometry_add_surfaces()
        {
            foreach(var vertex in Vertices)
            {
                Bsp.Vertices.Add(new Vertex { Point = new TagTool.Common.RealPoint3d { X = vertex.X, Y = vertex.Y, Z = vertex.Z } });
            }
            int triangle_index = 0;
            List<int> triangle = new List<int>();
            while (true)
            {
                triangle.Add(Indices[triangle_index]);
                triangle.Add(Indices[triangle_index+1]);
                triangle.Add(Indices[triangle_index+2]);
                triangle_index += 3;
                Bsp.Surfaces.Add(new Surface());
                int surface_index = Bsp.Surfaces.Count;
                Bsp.Surfaces[surface_index].Plane = ushort.MaxValue;


            }
        }

        public int collision_geometry_add_edge(int point0_index, int point1_index, int surface_index)
        {
            int matching_edge_index = -1;
            for(int edge_index = 0; edge_index < Bsp.Edges.Count; edge_index++)
            {
                if(Bsp.Edges[edge_index].StartVertex == point1_index &&
                    Bsp.Edges[edge_index].EndVertex == point0_index)
                {
                    Bsp.Edges[edge_index].RightSurface = (ushort)surface_index;
                    matching_edge_index = edge_index;
                }
            }

            if(matching_edge_index == -1)
            {
                matching_edge_index = Bsp.Edges.Count;
                Bsp.Edges.Add(new Edge
                {
                    StartVertex = (ushort)point0_index,
                    EndVertex = (ushort)point1_index,
                    LeftSurface = (ushort)surface_index,
                    RightSurface = ushort.MaxValue,
                    ForwardEdge = ushort.MaxValue,
                    ReverseEdge = ushort.MaxValue
                });
            }

            return matching_edge_index;
        }
    }
}
