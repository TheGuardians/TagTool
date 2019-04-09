using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.CollisionModels
{
    class ExtractModelCommand : Command
    {
        private CollisionModel Definition { get; }

        public ExtractModelCommand(CollisionModel definition) :
            base(true,

                "ExtractModel",
                "",

                "ExtractModel <File>",

                "")
        {
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
                    foreach (var permutation in region.Permutations)
                    {
                        foreach (var collisionBsp in permutation.Bsps)
                        {
                            for (var i = 0; i < collisionBsp.Geometry.Vertices.Count; i++)
                            {
                                var vertex = collisionBsp.Geometry.Vertices[i];
                                writer.WriteLine($"v {vertex.Point.X} {vertex.Point.Z} {vertex.Point.Y}");
                            }

                            writer.WriteLine($"g bsp_surfaces_{permutation.Bsps.IndexOf(collisionBsp)}");

                            for (var i = 0; i < collisionBsp.Geometry.Surfaces.Count; i++)
                            {
                                var surface = collisionBsp.Geometry.Surfaces[i];
                                var vertices = new HashSet<short>();
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