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
    class LocalizeTagResourcesCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private ScenarioStructureBsp Definition { get; }
        private CachedTagInstance Tag { get; }

        public LocalizeTagResourcesCommand(HaloOnlineCacheContext cacheContext, ScenarioStructureBsp definition, CachedTagInstance tag) :
            base(true,
                
                "LocalizeTagResources",
                "",
                
                "LocalizeTagResources",
                
                "")
        {
            CacheContext = cacheContext;
            Definition = definition;
            Tag = tag;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;

            if (Definition.CollisionBspResource == null)
            {
                Console.WriteLine("ERROR: Collision geometry does not have a resource associated with it.");
                return true;
            }

            var resourceDefinition = CacheContext.Deserialize<StructureBspTagResources>(Definition.CollisionBspResource);

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
            }

            Definition.TagResources = new List<ScenarioStructureBsp.TagResourcesBlock>
            {
                new ScenarioStructureBsp.TagResourcesBlock
                {
                    CollisionBsps = resourceDefinition.CollisionBsps.Select(
                        x => new CollisionGeometry
                    {
                        Bsp3dNodes = x.Bsp3dNodes.Elements,
                        Planes = x.Planes.Elements,
                        Leaves = x.Leaves.Elements,
                        Bsp2dReferences = x.Bsp2dReferences.Elements,
                        Bsp2dNodes = x.Bsp2dNodes.Elements,
                        Surfaces = x.Surfaces.Elements,
                        Edges = x.Edges.Elements,
                        Vertices = x.Vertices.Elements
                    }).ToList(),

                    LargeCollisionBsps = resourceDefinition.LargeCollisionBsps.Select(
                        x => new ScenarioStructureBsp.TagResourcesBlock.LargeCollisionBspBlock
                    {
                        Bsp3dNodes = x.Bsp3dNodes.Elements,
                        Planes = x.Planes.Elements,
                        Leaves = x.Leaves.Elements,
                        Bsp2dReferences = x.Bsp2dReferences.Elements,
                        Bsp2dNodes = x.Bsp2dNodes.Elements,
                        Surfaces = x.Surfaces.Elements,
                        Edges = x.Edges.Elements,
                        Vertices = x.Vertices.Elements
                    }).ToList(),

                    InstancedGeometry = resourceDefinition.InstancedGeometry.Select(
                        x => new ScenarioStructureBsp.TagResourcesBlock.InstancedGeometryBlock
                    {
                        Checksum = x.Checksum,
                        BoundingSphereOffset = x.BoundingSphereOffset,
                        BoundingSphereRadius = x.BoundingSphereRadius,

                        CollisionInfo = new CollisionGeometry
                        {
                            Bsp3dNodes = x.CollisionInfo.Bsp3dNodes.Elements,
                            Planes = x.CollisionInfo.Planes.Elements,
                            Leaves = x.CollisionInfo.Leaves.Elements,
                            Bsp2dReferences = x.CollisionInfo.Bsp2dReferences.Elements,
                            Bsp2dNodes = x.CollisionInfo.Bsp2dNodes.Elements,
                            Surfaces = x.CollisionInfo.Surfaces.Elements,
                            Edges = x.CollisionInfo.Edges.Elements,
                            Vertices = x.CollisionInfo.Vertices.Elements
                        },

                        CollisionGeometries = x.CollisionGeometries.Select(
                            y => new CollisionGeometry
                        {
                            Bsp3dNodes = y.Bsp3dNodes.Elements,
                            Planes = y.Planes.Elements,
                            Leaves = y.Leaves.Elements,
                            Bsp2dReferences = y.Bsp2dReferences.Elements,
                            Bsp2dNodes = y.Bsp2dNodes.Elements,
                            Surfaces = y.Surfaces.Elements,
                            Edges = y.Edges.Elements,
                            Vertices = y.Vertices.Elements
                        }).ToList(),

                        BspPhysics = x.BspPhysics.Select(
                            y => new ScenarioStructureBsp.TagResourcesBlock.CollisionBspPhysicsBlock
                        {
                            Unused1 = y.Unused1,
                            Size = y.Size,
                            Count = y.Count,
                            Address = y.Address,
                            Unused2 = y.Unused2,
                            Offset = y.Offset,
                            Unused3 = y.Unused3,
                            DataSize = y.DataSize,
                            DataCapacityAndFlags = y.DataCapacityAndFlags,
                            DataBuildType = y.DataBuildType,
                            Unused4 = y.Unused4,
                            Unused5 = y.Unused5,
                            Data = y.Data.Elements,
                            MoppBuildType = y.MoppBuildType,
                            Unused6 = y.Unused6,
                            Unused7 = y.Unused7
                        }).ToList(),

                        Unknown1 = x.Unknown1.Elements,
                        Unknown2 = x.Unknown2.Elements,
                        Unknown3 = x.Unknown3.Elements,
                        MeshIndex = x.MeshIndex,
                        CompressionIndex = x.CompressionIndex,
                        Unknown4 = x.Unknown4,
                        Unknown5 = x.Unknown5.Elements,
                        Unknown6 = x.Unknown6
                    }).ToList(),

                    HavokData = resourceDefinition.HavokData
                }
            };

            return true;
        }
    }
}
