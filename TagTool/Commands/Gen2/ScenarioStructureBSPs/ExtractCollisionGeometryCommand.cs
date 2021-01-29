using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions.Gen2;

namespace TagTool.Commands.Gen2.ScenarioStructureBSPs
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

            var file = new FileInfo(args[0]);

            if (!file.Directory.Exists)
                file.Directory.Create();

            using (var writer = new StreamWriter(file.Create()))
            {
                var baseVertex = 0;

                foreach (var bsp in Definition.CollisionBsp)
                {
                    for (var i = 0; i < bsp.Vertices.Count; i++)
                    {
                        var vertex = bsp.Vertices[i];
                        writer.WriteLine($"v {vertex.Point.X * 100.0} {-vertex.Point.Z * 100.0} {vertex.Point.Y * 100.0}");
                    }

                    writer.WriteLine($"g bsp_surfaces_{Definition.CollisionBsp.IndexOf(bsp)}");

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

                foreach (var instanceDef in Definition.InstancedGeometryInstances)
                {
                    if (instanceDef.InstanceDefinition == -1)
                        continue;

                    var instance = Definition.InstancedGeometriesDefinitions[instanceDef.InstanceDefinition];

                    var instanceName = instanceDef.Name != StringId.Invalid ?
                        CacheContext.StringTable.GetString(instanceDef.Name) :
                        $"instance_{instanceDef.InstanceDefinition}";

                    for (var i = 0; i < instance.CollisionInfo.Vertices.Count; i++)
                    {
                        var vertex = instance.CollisionInfo.Vertices[i];
                        var point = Vector3.Transform(
                            new Vector3(vertex.Point.X, vertex.Point.Y, vertex.Point.Z),
                            new Matrix4x4(
                                instanceDef.Forward.I, instanceDef.Forward.J, instanceDef.Forward.K, 0.0f,
                                instanceDef.Left.I, instanceDef.Left.J, instanceDef.Left.K, 0.0f,
                                instanceDef.Up.I, instanceDef.Up.J, instanceDef.Up.K, 0.0f,
                                instanceDef.Position.X, instanceDef.Position.Y, instanceDef.Position.Z, 0.0f));

                        writer.WriteLine($"v {point.X * 100.0} {-point.Z * 100.0} {point.Y * 100.0}");
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
                }
            }
            return true;
        }
    }
}
