using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Commands.ScenarioStructureBSPs
{
    class ExtractCollisionGeometryCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private ScenarioStructureBsp Definition { get; }

        public ExtractCollisionGeometryCommand(HaloOnlineCacheContext cacheContext, ScenarioStructureBsp definition) :
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
                return false;

            if (Definition.CollisionBspResource == null)
            {
                Console.WriteLine("ERROR: Collision geometry does not have a resource associated with it.");
                return true;
            }

            var resourceContext = new ResourceSerializationContext(CacheContext, Definition.CollisionBspResource);
            var resourceDefinition = CacheContext.Deserializer.Deserialize<StructureBspTagResources>(resourceContext);

            using (var resourceStream = new MemoryStream())
            {
                CacheContext.ExtractResource(Definition.CollisionBspResource, resourceStream);

                using (var reader = new EndianReader(resourceStream))
                {
                    foreach (var cbsp in resourceDefinition.CollisionBsps)
                    {
                        reader.BaseStream.Position = cbsp.Bsp3dNodes.Address.Offset;
                        for (var i = 0; i < cbsp.Bsp3dNodes.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Bsp3dNode));
                            cbsp.Bsp3dNodes.Add((CollisionGeometry.Bsp3dNode)element);
                        }

                        reader.BaseStream.Position = cbsp.Planes.Address.Offset;
                        for (var i = 0; i < cbsp.Planes.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Plane));
                            cbsp.Planes.Add((CollisionGeometry.Plane)element);
                        }

                        reader.BaseStream.Position = cbsp.Leaves.Address.Offset;
                        for (var i = 0; i < cbsp.Leaves.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Leaf));
                            cbsp.Leaves.Add((CollisionGeometry.Leaf)element);
                        }

                        reader.BaseStream.Position = cbsp.Bsp2dReferences.Address.Offset;
                        for (var i = 0; i < cbsp.Bsp2dReferences.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Bsp2dReference));
                            cbsp.Bsp2dReferences.Add((CollisionGeometry.Bsp2dReference)element);
                        }

                        reader.BaseStream.Position = cbsp.Bsp2dNodes.Address.Offset;
                        for (var i = 0; i < cbsp.Bsp2dNodes.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Bsp2dNode));
                            cbsp.Bsp2dNodes.Add((CollisionGeometry.Bsp2dNode)element);
                        }

                        reader.BaseStream.Position = cbsp.Surfaces.Address.Offset;
                        for (var i = 0; i < cbsp.Surfaces.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Surface));
                            cbsp.Surfaces.Add((CollisionGeometry.Surface)element);
                        }

                        reader.BaseStream.Position = cbsp.Edges.Address.Offset;
                        for (var i = 0; i < cbsp.Edges.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Edge));
                            cbsp.Edges.Add((CollisionGeometry.Edge)element);
                        }

                        reader.BaseStream.Position = cbsp.Vertices.Address.Offset;
                        for (var i = 0; i < cbsp.Vertices.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Vertex));
                            cbsp.Vertices.Add((CollisionGeometry.Vertex)element);
                        }
                    }
                }

                var file = new FileInfo(args[0]);

                if (!file.Directory.Exists)
                    file.Directory.Create();

                using (var writer = new StreamWriter(file.Create()))
                {
                    foreach (var cbsp in resourceDefinition.CollisionBsps)
                    {
                        for (var i = 0; i < cbsp.Vertices.Count; i++)
                        {
                            var vertex = cbsp.Vertices[i];
                            writer.WriteLine($"v {vertex.Point.X} {vertex.Point.Z} {vertex.Point.Y}");
                        }

                        writer.WriteLine("g sectors");

                        for (var i = 0; i < cbsp.Surfaces.Count; i++)
                        {
                            var surface = cbsp.Surfaces[i];
                            var vertices = new HashSet<short>();
                            var edge = cbsp.Edges[surface.FirstEdge];

                            writer.Write("f");

                            while (true)
                            {
                                if (edge.LeftSurface == i)
                                {
                                    writer.Write($" {edge.StartVertex + 1}");

                                    if (edge.ForwardEdge == surface.FirstEdge)
                                        break;
                                    else
                                        edge = cbsp.Edges[edge.ForwardEdge];
                                }
                                else if (edge.RightSurface == i)
                                {
                                    writer.Write($" {edge.EndVertex + 1}");

                                    if (edge.ReverseEdge == surface.FirstEdge)
                                        break;
                                    else
                                        edge = cbsp.Edges[edge.ReverseEdge];
                                }
                            }

                            writer.WriteLine();
                        }
                    }
                }
            }

            return true;
        }
    }
}
