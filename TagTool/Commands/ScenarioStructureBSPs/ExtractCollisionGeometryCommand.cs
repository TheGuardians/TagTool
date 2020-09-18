using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.ScenarioStructureBSPs
{
    class ExtractCollisionGeometryCommand : Command
    {
        private GameCache CacheContext { get; }
        private ScenarioStructureBsp Definition { get; }

        public ExtractCollisionGeometryCommand(GameCache cacheContext, ScenarioStructureBsp definition) :
            base(false,
                
                "ExtractCollisionGeometry",
                "",
                
                "ExtractCollisionGeometry <OBJ File>",
                
                "")
        {
            CacheContext = cacheContext;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return new TagToolError(CommandError.ArgCount);

            if (Definition.CollisionBspResource == null)
            {
                Console.WriteLine("Collision geometry does not have a resource associated with it.");
                return true;
            }

            var resourceDefinition = CacheContext.ResourceCache.GetStructureBspTagResources(Definition.CollisionBspResource);

            using (var resourceStream = new MemoryStream())
            {
                var file = new FileInfo(args[0]);

                if (!file.Directory.Exists)
                    file.Directory.Create();

                using (var writer = new StreamWriter(file.Create()))
                {
                    var baseVertex = 0;

                    foreach (var bsp in resourceDefinition.CollisionBsps)
                    {
                        for (var i = 0; i < bsp.Vertices.Count; i++)
                        {
                            var vertex = bsp.Vertices[i];
                            writer.WriteLine($"v {vertex.Point.X} {vertex.Point.Z} {vertex.Point.Y}");
                        }

                        writer.WriteLine($"g bsp_surfaces_{resourceDefinition.CollisionBsps.IndexOf(bsp)}");

                        for (var i = 0; i < bsp.Surfaces.Count; i++)
                        {
                            var surface = bsp.Surfaces[i];
                            var vertices = new HashSet<short>();
                            var edge = bsp.Edges[surface.FirstEdge];

                            writer.Write("f");

                            while (true)
                            {
                                if (edge.LeftSurface == i)
                                {
                                    writer.Write($" {baseVertex + edge.StartVertex + 1}");

                                    if (edge.ForwardEdge == surface.FirstEdge)
                                        break;
                                    else
                                        edge = bsp.Edges[edge.ForwardEdge];
                                }
                                else if (edge.RightSurface == i)
                                {
                                    writer.Write($" {baseVertex + edge.EndVertex + 1}");

                                    if (edge.ReverseEdge == surface.FirstEdge)
                                        break;
                                    else
                                        edge = bsp.Edges[edge.ReverseEdge];
                                }
                            }

                            writer.WriteLine();
                        }

                        baseVertex += bsp.Vertices.Count;
                    }

                    foreach (var largeBsp in resourceDefinition.LargeCollisionBsps)
                    {
                        for (var i = 0; i < largeBsp.Vertices.Count; i++)
                        {
                            var vertex = largeBsp.Vertices[i];
                            writer.WriteLine($"v {vertex.Point.X} {vertex.Point.Z} {vertex.Point.Y}");
                        }

                        writer.WriteLine($"g large_bsp_surfaces_{resourceDefinition.LargeCollisionBsps.IndexOf(largeBsp)}");

                        for (var i = 0; i < largeBsp.Surfaces.Count; i++)
                        {
                            var surface = largeBsp.Surfaces[i];
                            var vertices = new HashSet<short>();
                            var edge = largeBsp.Edges[surface.FirstEdge];

                            writer.Write("f");

                            while (true)
                            {
                                if (edge.LeftSurface == i)
                                {
                                    writer.Write($" {baseVertex + edge.StartVertex + 1}");

                                    if (edge.ForwardEdge == surface.FirstEdge)
                                        break;
                                    else
                                        edge = largeBsp.Edges[edge.ForwardEdge];
                                }
                                else if (edge.RightSurface == i)
                                {
                                    writer.Write($" {baseVertex + edge.EndVertex + 1}");

                                    if (edge.ReverseEdge == surface.FirstEdge)
                                        break;
                                    else
                                        edge = largeBsp.Edges[edge.ReverseEdge];
                                }
                            }

                            writer.WriteLine();
                        }

                        baseVertex += largeBsp.Vertices.Count;
                    }

                    foreach (var instanceDef in Definition.InstancedGeometryInstances)
                    {
                        if (instanceDef.DefinitionIndex == -1)
                            continue;

                        var instance = resourceDefinition.InstancedGeometry[instanceDef.DefinitionIndex];

                        var instanceName = instanceDef.Name != StringId.Invalid ?
                            CacheContext.StringTable.GetString(instanceDef.Name) :
                            $"instance_{instanceDef.DefinitionIndex}";

                        for (var i = 0; i < instance.CollisionInfo.Vertices.Count; i++)
                        {
                            var vertex = instance.CollisionInfo.Vertices[i];
                            var point = Vector3.Transform(
                                new Vector3(vertex.Point.X, vertex.Point.Y, vertex.Point.Z),
                                new Matrix4x4(
                                    instanceDef.Matrix.m11, instanceDef.Matrix.m12, instanceDef.Matrix.m13, 0.0f,
                                    instanceDef.Matrix.m21, instanceDef.Matrix.m22, instanceDef.Matrix.m23, 0.0f,
                                    instanceDef.Matrix.m31, instanceDef.Matrix.m32, instanceDef.Matrix.m33, 0.0f,
                                    instanceDef.Matrix.m41, instanceDef.Matrix.m42, instanceDef.Matrix.m43, 0.0f));

                            writer.WriteLine($"v {point.X} {point.Z} {point.Y}");
                        }

                        writer.WriteLine($"g {instanceName}_main_surfaces");

                        for (var i = 0; i < instance.CollisionInfo.Surfaces.Count; i++)
                        {
                            var surface = instance.CollisionInfo.Surfaces[i];
                            var vertices = new HashSet<short>();
                            var edge = instance.CollisionInfo.Edges[surface.FirstEdge];

                            writer.Write("f");

                            while (true)
                            {
                                if (edge.LeftSurface == i)
                                {
                                    writer.Write($" {baseVertex + edge.StartVertex + 1}");

                                    if (edge.ForwardEdge == surface.FirstEdge)
                                        break;
                                    else
                                        edge = instance.CollisionInfo.Edges[edge.ForwardEdge];
                                }
                                else if (edge.RightSurface == i)
                                {
                                    writer.Write($" {baseVertex + edge.EndVertex + 1}");

                                    if (edge.ReverseEdge == surface.FirstEdge)
                                        break;
                                    else
                                        edge = instance.CollisionInfo.Edges[edge.ReverseEdge];
                                }
                            }

                            writer.WriteLine();
                        }

                        baseVertex += instance.CollisionInfo.Vertices.Count;

                        foreach (var bsp in instance.CollisionGeometries)
                        {
                            for (var i = 0; i < bsp.Vertices.Count; i++)
                            {
                                var vertex = bsp.Vertices[i];
                                writer.WriteLine($"v {vertex.Point.X} {vertex.Point.Z} {vertex.Point.Y}");
                            }

                            writer.WriteLine($"g {instanceName}_bsp_surfaces_{resourceDefinition.CollisionBsps.IndexOf(bsp)}");

                            for (var i = 0; i < bsp.Surfaces.Count; i++)
                            {
                                var surface = bsp.Surfaces[i];
                                var vertices = new HashSet<short>();
                                var edge = bsp.Edges[surface.FirstEdge];

                                writer.Write("f");

                                while (true)
                                {
                                    if (edge.LeftSurface == i)
                                    {
                                        writer.Write($" {baseVertex + edge.StartVertex + 1}");

                                        if (edge.ForwardEdge == surface.FirstEdge)
                                            break;
                                        else
                                            edge = bsp.Edges[edge.ForwardEdge];
                                    }
                                    else if (edge.RightSurface == i)
                                    {
                                        writer.Write($" {baseVertex + edge.EndVertex + 1}");

                                        if (edge.ReverseEdge == surface.FirstEdge)
                                            break;
                                        else
                                            edge = bsp.Edges[edge.ReverseEdge];
                                    }
                                }

                                writer.WriteLine();
                            }

                            baseVertex += bsp.Vertices.Count;
                        }
                    }
                }
            }

            return true;
        }
    }
}
