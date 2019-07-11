using System.Collections.Generic;
using System.IO;
using TagTool.Tags.Definitions;
using TagTool.Cache;
using TagTool.IO;
using TagTool.Tags.Resources;
using TagTool.Serialization;
using TagTool.Geometry;

namespace TagTool.Commands.ScenarioStructureBSPs
{
    class CollisionTestCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private ScenarioStructureBsp BSP { get; }

        public CollisionTestCommand(HaloOnlineCacheContext cacheContext, CachedTagInstance tag, ScenarioStructureBsp bsp)
            : base(true,

                  "CollisionTest",
                  "A test resource-loading command for 'sbsp' tag collision.",
                  
                  "collision-test",

                  "A test resource-loading command for 'sbsp' tag collision.")
        {
            CacheContext = cacheContext;
            Tag = tag;
            BSP = bsp;
        }

        public override object Execute(List<string> args)
        {
            // Deserialize the definition data
            var resourceContext = new ResourceSerializationContext(CacheContext, BSP.CollisionBspResource);
            var resourceDefinition = CacheContext.Deserializer.Deserialize<StructureBspTagResources>(resourceContext);

            // Extract the resource data
            using (var resourceDataStream = new MemoryStream())
            using (var reader = new EndianReader(resourceDataStream))
            {
                CacheContext.ExtractResource(BSP.CollisionBspResource, resourceDataStream);

                #region collision bsps

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

                #endregion

                #region large collision bsps

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

                #endregion

                #region compressions

                foreach (var instance in resourceDefinition.InstancedGeometry)
                {
                    #region compression's resource data

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

                    #endregion

                    #region compression's other resource data

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

                    #endregion

                    #region Unknown Data

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

                    #endregion

                    #region compression's havok collision data

                    foreach (var collision in instance.BspPhysics)
                    {
                        for (var i = 0; i < collision.Data.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(byte));
                            collision.Data.Add(new StructureBspTagResources.CollisionBspPhysicsBlock.Datum { Value = (byte)element });
                        }
                    }

                    #endregion
                }

                #endregion
            }

            return true;
        }
    }
}