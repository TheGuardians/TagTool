using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Common;
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

            var resourceContext = new ResourceSerializationContext(CacheContext, Definition.CollisionBspResource.HaloOnlinePageableResource);
            var resourceDefinition = CacheContext.Deserializer.Deserialize<StructureBspTagResources>(resourceContext);

            using (var resourceStream = new MemoryStream())
            {
                CacheContext.ExtractResource(Definition.CollisionBspResource.HaloOnlinePageableResource, resourceStream);

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

                    foreach (var cbsp in resourceDefinition.LargeCollisionBsps)
                    {
                        reader.BaseStream.Position = cbsp.Bsp3dNodes.Address.Offset;
                        for (var i = 0; i < cbsp.Bsp3dNodes.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(StructureBspTagResources.LargeCollisionBspBlock.Bsp3dNode));
                            cbsp.Bsp3dNodes.Add((StructureBspTagResources.LargeCollisionBspBlock.Bsp3dNode)element);
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
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(StructureBspTagResources.LargeCollisionBspBlock.Bsp2dReference));
                            cbsp.Bsp2dReferences.Add((StructureBspTagResources.LargeCollisionBspBlock.Bsp2dReference)element);
                        }

                        reader.BaseStream.Position = cbsp.Bsp2dNodes.Address.Offset;
                        for (var i = 0; i < cbsp.Bsp2dNodes.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(StructureBspTagResources.LargeCollisionBspBlock.Bsp2dNode));
                            cbsp.Bsp2dNodes.Add((StructureBspTagResources.LargeCollisionBspBlock.Bsp2dNode)element);
                        }

                        reader.BaseStream.Position = cbsp.Surfaces.Address.Offset;
                        for (var i = 0; i < cbsp.Surfaces.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(StructureBspTagResources.LargeCollisionBspBlock.Surface));
                            cbsp.Surfaces.Add((StructureBspTagResources.LargeCollisionBspBlock.Surface)element);
                        }

                        reader.BaseStream.Position = cbsp.Edges.Address.Offset;
                        for (var i = 0; i < cbsp.Edges.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(StructureBspTagResources.LargeCollisionBspBlock.Edge));
                            cbsp.Edges.Add((StructureBspTagResources.LargeCollisionBspBlock.Edge)element);
                        }

                        reader.BaseStream.Position = cbsp.Vertices.Address.Offset;
                        for (var i = 0; i < cbsp.Vertices.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(StructureBspTagResources.LargeCollisionBspBlock.Vertex));
                            cbsp.Vertices.Add((StructureBspTagResources.LargeCollisionBspBlock.Vertex)element);
                        }
                    }

                    foreach (var instance in resourceDefinition.InstancedGeometry)
                    {
                        reader.BaseStream.Position = instance.CollisionInfo.Bsp3dNodes.Address.Offset;
                        for (var i = 0; i < instance.CollisionInfo.Bsp3dNodes.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Bsp3dNode));
                            instance.CollisionInfo.Bsp3dNodes.Add((CollisionGeometry.Bsp3dNode)element);
                        }

                        reader.BaseStream.Position = instance.CollisionInfo.Planes.Address.Offset;
                        for (var i = 0; i < instance.CollisionInfo.Planes.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Plane));
                            instance.CollisionInfo.Planes.Add((CollisionGeometry.Plane)element);
                        }

                        reader.BaseStream.Position = instance.CollisionInfo.Leaves.Address.Offset;
                        for (var i = 0; i < instance.CollisionInfo.Leaves.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Leaf));
                            instance.CollisionInfo.Leaves.Add((CollisionGeometry.Leaf)element);
                        }

                        reader.BaseStream.Position = instance.CollisionInfo.Bsp2dReferences.Address.Offset;
                        for (var i = 0; i < instance.CollisionInfo.Bsp2dReferences.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Bsp2dReference));
                            instance.CollisionInfo.Bsp2dReferences.Add((CollisionGeometry.Bsp2dReference)element);
                        }

                        reader.BaseStream.Position = instance.CollisionInfo.Bsp2dNodes.Address.Offset;
                        for (var i = 0; i < instance.CollisionInfo.Bsp2dNodes.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Bsp2dNode));
                            instance.CollisionInfo.Bsp2dNodes.Add((CollisionGeometry.Bsp2dNode)element);
                        }

                        reader.BaseStream.Position = instance.CollisionInfo.Surfaces.Address.Offset;
                        for (var i = 0; i < instance.CollisionInfo.Surfaces.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Surface));
                            instance.CollisionInfo.Surfaces.Add((CollisionGeometry.Surface)element);
                        }

                        reader.BaseStream.Position = instance.CollisionInfo.Edges.Address.Offset;
                        for (var i = 0; i < instance.CollisionInfo.Edges.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Edge));
                            instance.CollisionInfo.Edges.Add((CollisionGeometry.Edge)element);
                        }

                        reader.BaseStream.Position = instance.CollisionInfo.Vertices.Address.Offset;
                        for (var i = 0; i < instance.CollisionInfo.Vertices.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Vertex));
                            instance.CollisionInfo.Vertices.Add((CollisionGeometry.Vertex)element);
                        }

                        foreach (var cbsp in instance.CollisionGeometries)
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

                        for (var i = 0; i < instance.Unknown1.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(StructureBspTagResources.InstancedGeometryBlock.Unknown1Block));
                            instance.Unknown1.Add((StructureBspTagResources.InstancedGeometryBlock.Unknown1Block)element);
                        }

                        for (var i = 0; i < instance.Unknown2.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(StructureBspTagResources.InstancedGeometryBlock.Unknown2Block));
                            instance.Unknown2.Add((StructureBspTagResources.InstancedGeometryBlock.Unknown2Block)element);
                        }

                        for (var i = 0; i < instance.Unknown3.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(StructureBspTagResources.InstancedGeometryBlock.Unknown3Block));
                            instance.Unknown3.Add((StructureBspTagResources.InstancedGeometryBlock.Unknown3Block)element);
                        }

                        foreach (var collision in instance.BspPhysics)
                        {
                            for (var i = 0; i < collision.Data.Count; i++)
                            {
                                var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(byte));
                                collision.Data.Add(new StructureBspTagResources.CollisionBspPhysicsBlock.Datum { Value = (byte)element });
                            }
                        }
                    }
                }

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
                        if (instanceDef.InstanceDefinition == -1)
                            continue;

                        var instance = resourceDefinition.InstancedGeometry[instanceDef.InstanceDefinition];

                        var instanceName = instanceDef.Name != StringId.Invalid ?
                            CacheContext.GetString(instanceDef.Name) :
                            $"instance_{instanceDef.InstanceDefinition}";

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
