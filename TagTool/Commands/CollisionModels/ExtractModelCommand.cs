using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.CollisionModels
{
    class ExtractModelCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CollisionModel Definition { get; }

        public ExtractModelCommand(HaloOnlineCacheContext cacheContext, CollisionModel definition) :
            base(true,

                "ExtractModel",
                "",

                "ExtractModel <File>",

                "")
        {
            CacheContext = cacheContext;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var file = new FileInfo(args[0]);

            if (!file.Directory.Exists)
                file.Directory.Create();

            using (var writer = new StreamWriter(file.Create()))
            {
                var baseVertex = 0;

                foreach (var region in Definition.Regions)
                {
                    var regionName = CacheContext.GetString(region.Name);

                    foreach (var permutation in region.Permutations)
                    {
                        var permutationName = CacheContext.GetString(permutation.Name);

                        foreach (var collisionBsp in permutation.Bsps)
                        {
                            var offset = new RealPoint3d();

                            for (var i = 0; i < collisionBsp.Geometry.Vertices.Count; i++)
                            {
                                var v = offset + collisionBsp.Geometry.Vertices[i].Point;
                                writer.WriteLine($"v {v.X} {v.Z} {v.Y}");
                            }

                            writer.WriteLine($"g {regionName}:{permutationName}");

                            for (var i = 0; i < collisionBsp.Geometry.Surfaces.Count; i++)
                            {
                                var surface = collisionBsp.Geometry.Surfaces[i];
                                var edge = collisionBsp.Geometry.Edges[surface.FirstEdge];

                                writer.Write("f");

                                while (true)
                                {
                                    if (edge.LeftSurface == i)
                                    {
                                        writer.Write($" {baseVertex + edge.StartVertex + 1}");

                                        if (edge.ForwardEdge == surface.FirstEdge)
                                            break;
                                        else
                                            edge = collisionBsp.Geometry.Edges[edge.ForwardEdge];
                                    }
                                    else if (edge.RightSurface == i)
                                    {
                                        writer.Write($" {baseVertex + edge.EndVertex + 1}");

                                        if (edge.ReverseEdge == surface.FirstEdge)
                                            break;
                                        else
                                            edge = collisionBsp.Geometry.Edges[edge.ReverseEdge];
                                    }
                                }

                                writer.WriteLine();
                            }

                            baseVertex += collisionBsp.Geometry.Vertices.Count;
                        }
                    }
                }
            }

            return true;
        }
    }
}