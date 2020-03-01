using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Commands.Porting;
using TagTool.Common;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Geometry.Utils
{
    public class InstancedGeometryToObjectConverter
    {
        private GameCache DestCache;
        private Stream DestStream;
        private GameCache SourceCache;
        private Stream SourceStream;
        private int StructureBspIndex;
        private Scenario Scenario;
        private ScenarioStructureBsp StructureBsp;
        private ScenarioLightmap sLdT;
        private ScenarioLightmapBspData Lbsp;
        private StructureBspTagResources StructureBspResources;
        private Dictionary<short, short> CollisionMaterialMapping;
        public PortTagCommand PortTag { get; private set; }

        public InstancedGeometryToObjectConverter(
            GameCacheHaloOnlineBase destCache, Stream destStream, GameCache sourceCache,
            Stream sourceStream, Scenario scenario, int structureBspIndex)
        {
            DestCache = destCache;
            DestStream = destStream;
            SourceCache = sourceCache;
            SourceStream = sourceStream;
            StructureBspIndex = structureBspIndex;
            PortTag = new PortTagCommand(destCache, sourceCache);
            PortTag.SetFlags(PortTagCommand.PortingFlags.Default);

            Scenario = scenario;
            StructureBspIndex = structureBspIndex;
            StructureBsp = SourceCache.Deserialize<ScenarioStructureBsp>(SourceStream, Scenario.StructureBsps[structureBspIndex].StructureBsp);
            sLdT = SourceCache.Deserialize<ScenarioLightmap>(SourceStream, Scenario.Lightmap);

            if(SourceCache.Version >= CacheVersion.Halo3ODST)
            {
                Lbsp = SourceCache.Deserialize<ScenarioLightmapBspData>(SourceStream, sLdT.LightmapDataReferences[StructureBspIndex]);
            }
            else
            {
                Lbsp = sLdT.Lightmaps[StructureBspIndex];
            }
           
            var resourceDefinition = SourceCache.ResourceCache.GetRenderGeometryApiResourceDefinition(Lbsp.Geometry.Resource);
            Lbsp.Geometry.SetResourceBuffers(resourceDefinition);

            StructureBspResources = SourceCache.ResourceCache.GetStructureBspTagResources(StructureBsp.CollisionBspResource);
        }

        public CachedTag Convert(string instanceName)
        {
            var stringId = SourceCache.StringTable.GetStringId(instanceName);
            return ConvertInstance(stringId);
        }

        public CachedTag ConvertInstance(StringId instanceName)
        {
            for (int i = 0; i < StructureBsp.InstancedGeometryInstances.Count; i++)
            {
                if (StructureBsp.InstancedGeometryInstances[i].Name == instanceName)
                    return ConvertInstance(i);
            }

            return null;
        }

        public CachedTag ConvertInstance(int instanceIndex)
        {
            var instancedGeometryInstance = StructureBsp.InstancedGeometryInstances[instanceIndex];

            string instanceName = "";
            if (instancedGeometryInstance.Name == StringId.Invalid)
            {
                instanceName = $"instance_{instanceIndex:000}";
            }
            else
            {
                instanceName = SourceCache.StringTable.GetString(instancedGeometryInstance.Name);
                instanceName = Regex.Replace(instanceName, @"[^a-zA-Z0-9_]", string.Empty);
            }

            var scenarioFolder = Path.GetDirectoryName(Scenario.StructureBsps[StructureBspIndex].StructureBsp.Name);
            var tagName = $"objects\\{scenarioFolder}\\instanced\\{StructureBspIndex:00}_{instanceName}";

            // reset state
            CollisionMaterialMapping = new Dictionary<short, short>();

            // check if we've already converted the tag, if so just return it
            CachedTag scenTag;
            if (DestCache.TryGetTag<Scenery>(tagName, out scenTag))
                return scenTag;

            // allocate tags
            scenTag = DestCache.TagCache.AllocateTag<Scenery>(tagName);
            var collisionModelTag = DestCache.TagCache.AllocateTag<CollisionModel>(tagName);
            var renderModelTag = DestCache.TagCache.AllocateTag<RenderModel>(tagName);
            var modelTag = DestCache.TagCache.AllocateTag<Model>(tagName);

            // generate the definitions
            var renderModel = GenerateRenderModel(instanceIndex);
            var collisionModel = GenerateCollisionModel(modelTag, instanceIndex);
            var model = GenerateModel(renderModel, collisionModel, instanceIndex);
            var gameObject = GenerateObject(instanceIndex, ComputeRenderModelEnclosingRadius(renderModel));

            // fixcup tag refs
            model.CollisionModel = collisionModelTag;
            model.RenderModel = renderModelTag;
            gameObject.Model = modelTag;

            // finally serialize all the tags
            DestCache.Serialize(DestStream, collisionModelTag, collisionModel);
            DestCache.Serialize(DestStream, modelTag, model);
            DestCache.Serialize(DestStream, renderModelTag, renderModel);
            DestCache.Serialize(DestStream, scenTag, gameObject);

            return scenTag;
        }

        private GameObject GenerateObject(int instancedGeometryInstanceIndex, float boundingSphere)
        {
            var scenery = new Scenery()
            {
                ObjectType = new GameObjectType() { Halo3ODST = GameObjectTypeHalo3ODST.Scenery }, // TODO: generic object type
                BoundingRadius = boundingSphere,
                AccelerationScale = 1.0f,
                SweetenerSize = GameObject.SweetenerSizeValue.Large,
                MultiplayerObject = new List<GameObject.MultiplayerObjectBlock>()
                {
                    new GameObject.MultiplayerObjectBlock() { SpawnTime = 30, AbandonTime = 60 }
                },
                // SceneryFlags = Scenery.SceneryFlagBits.PhysicallySimulates // static lighting updates (breaks player collision :/)
            };

            return scenery;
        }

        private Model GenerateModel(RenderModel renderModel, CollisionModel collisionModel, int instancedGeometryInstanceIndex)
        {
            var model = new Model();
            model.ReduceToL1SuperLow = 300.0f;
            model.ReduceToL2Low = 280.0f;

            // add the collision regions/permutations
            model.CollisionRegions = new List<Model.CollisionRegion>();
            if (collisionModel.Regions != null && collisionModel.Regions.Count > 0)
            {
                for (var regionIndex = 0; regionIndex < collisionModel.Regions.Count; regionIndex++)
                {
                    var collisionRegion = collisionModel.Regions[regionIndex];
                    var modelRegion = new Model.CollisionRegion()
                    {
                        Name = collisionRegion.Name,
                        Permutations = new List<Model.CollisionRegion.Permutation>()
                    };

                    for (var permIndex = 0; permIndex < collisionRegion.Permutations.Count; permIndex++)
                    {
                        var collisionPermutation = collisionRegion.Permutations[permIndex];
                        modelRegion.Permutations.Add(new Model.CollisionRegion.Permutation()
                        {
                            Name = collisionPermutation.Name,
                            CollisionPermutationIndex = (sbyte)permIndex,
                            PhysicsPermutationIndex = (sbyte)permIndex
                        });
                    }

                    model.CollisionRegions.Add(modelRegion);
                }
            }
            else
            {
                model.CollisionRegions = new List<Model.CollisionRegion>()
                {
                    new Model.CollisionRegion()
                    {
                        Name = DestCache.StringTable.GetStringId("default"),
                        CollisionRegionIndex = -1,
                        PhysicsRegionIndex = 0,
                        Permutations = new List<Model.CollisionRegion.Permutation>()
                        {
                            new Model.CollisionRegion.Permutation()
                            {
                                Name = DestCache.StringTable.GetStringId("default"),
                                CollisionPermutationIndex = -1,
                                PhysicsPermutationIndex = 0
                            }
                        }
                    }
                };
            }

            // add the collision materials to the model
            model.Materials = new List<Model.Material>();
            for (int i = 0; i < CollisionMaterialMapping.Count; i++)
                model.Materials.Add(new Model.Material());

            foreach (var mapping in CollisionMaterialMapping)
            {
                var sbspMaterial = StructureBsp.CollisionMaterials[mapping.Key];
                model.Materials[mapping.Value] = new Model.Material()
                {
                    Name = StringId.Invalid,
                    MaterialType = Model.Material.MaterialTypeValue.Dirt,
                    DamageSectionIndex = -1,
                    RuntimeDamagerMaterialIndex = -1,
                    RuntimeCollisionMaterialIndex = 0,
                    MaterialName = StringId.Invalid,
                    GlobalMaterialIndex = sbspMaterial.RuntimeGlobalMaterialIndex,
                };
            }

            // add the render model nodes to the model
            model.Nodes = new List<Model.Node>();
            foreach (var node in renderModel.Nodes)
            {
                model.Nodes.Add(new Model.Node()
                {
                    Name = node.Name,
                    ParentNode = node.ParentNode,
                    FirstChildNode = node.FirstChildNode,
                    NextSiblingNode = node.NextSiblingNode,
                    ImportNodeIndex = -1,
                    DefaultTranslation = node.DefaultTranslation,
                    DefaultRotation = node.DefaultRotation,
                    DefaultScale = node.DefaultScale,
                    Inverse = new RealMatrix4x3(
                        node.InverseForward.I, node.InverseForward.J, node.InverseForward.K,
                        node.InverseLeft.I, node.InverseLeft.J, node.InverseLeft.K,
                        node.InverseUp.I, node.InverseUp.J, node.InverseUp.K,
                        node.InversePosition.X, node.InversePosition.Y, node.InversePosition.Z)
                });
            }

            return model;
        }

        private CollisionModel GenerateCollisionModel(CachedTag modelTag, int instancedGeometryInstanceIndex)
        {
            var collisionModel = new CollisionModel();
            collisionModel.Regions = new List<CollisionModel.Region>();

            var instancedGeometryInstance = StructureBsp.InstancedGeometryInstances[instancedGeometryInstanceIndex];
            var instancedGeometryDef = StructureBspResources.InstancedGeometry[instancedGeometryInstance.MeshIndex];

            if (instancedGeometryInstance.BspPhysics.Count > 0)
            {
                var permutation = new CollisionModel.Region.Permutation()
                {
                    Name = DestCache.StringTable.GetStringId("default"),
                    BspPhysics = new List<CollisionBspPhysicsDefinition>(),
                    BspMoppCodes = new List<TagTool.Havok.TagHkpMoppCode>(),
                    Bsps = new List<CollisionModel.Region.Permutation.Bsp>()
                };

                collisionModel.Regions = new List<CollisionModel.Region>()
                {
                    new CollisionModel.Region()
                    {
                        Name = DestCache.StringTable.GetStringId("default"),
                        Permutations = new List<CollisionModel.Region.Permutation>() { permutation }
                    }
                };

                // copy over and fixup bsp physics blocks
                foreach (var bspPhysics in instancedGeometryInstance.BspPhysics)
                {
                    var convertedBspPhysics = ConvertData(bspPhysics);
                    convertedBspPhysics.GeometryShape.Model = modelTag;
                    convertedBspPhysics.GeometryShape.BspIndex = -1;
                    convertedBspPhysics.GeometryShape.CollisionGeometryShapeKey = 0xffff;
                    convertedBspPhysics.GeometryShape.CollisionGeometryShapeType = 0;
                    permutation.BspPhysics.Add(convertedBspPhysics);
                }

                foreach (var mopp in instancedGeometryDef.CollisionMoppCodes)
                    permutation.BspMoppCodes.Add(ConvertData(mopp));
                    
                // fixup surfaces materials block
                // build a mapping of surface material indices to collision materials
                var newCollisionGeometry = instancedGeometryDef.CollisionInfo.DeepClone();

                foreach (var surface in newCollisionGeometry.Surfaces)
                {
                    if (surface.MaterialIndex == -1)
                        continue;

                    short modelMaterialIndex;
                    if (!CollisionMaterialMapping.TryGetValue(surface.MaterialIndex, out modelMaterialIndex))
                        CollisionMaterialMapping.Add(surface.MaterialIndex, modelMaterialIndex);

                    surface.MaterialIndex = modelMaterialIndex;
                }

                // add the collision geometry
                permutation.Bsps.Add(new CollisionModel.Region.Permutation.Bsp()
                {
                    NodeIndex = 0,
                    Geometry = newCollisionGeometry
                });
            }

            return collisionModel;
        }

        private RenderModel GenerateRenderModel(int instanceIndex)
        {
            var renderModel = new RenderModel();
            renderModel.Geometry = ConvertInstanceRenderGeometry(instanceIndex);
            renderModel.InstanceStartingMeshIndex = -1;
            renderModel.Nodes = new List<RenderModel.Node>();
            renderModel.Nodes.Add(new RenderModel.Node()
            {
                Name = DestCache.StringTable.GetStringId("default"),
                ParentNode = -1,
                FirstChildNode = -1,
                NextSiblingNode = -1,
                DefaultTranslation = new RealPoint3d(0, 0, 0),
                DefaultRotation = new RealQuaternion(0, 0, 0, -1),
                DefaultScale = 1.0f,
                InverseForward = new RealVector3d(1, 0, 0),
                InverseLeft = new RealVector3d(0, 1, 0),
                InverseUp = new RealVector3d(0, 0, 1),
                InversePosition = new RealPoint3d(0, 0, 0)
            });
            renderModel.Regions = new List<RenderModel.Region>();
            renderModel.Regions.Add(new RenderModel.Region()
            {
                Name = DestCache.StringTable.GetStringId("default"),
                NodeMapOffset = 0,
                NodeMapSize = 1,
                Permutations = new List<RenderModel.Region.Permutation>()
                    {
                       new RenderModel.Region.Permutation()
                       {
                          Name = DestCache.StringTable.GetStringId("default"),
                          MeshIndex = 0,
                          MeshCount = 1,
                       }
                    }
            });
            renderModel.RuntimeNodeOrientations = new List<RenderModel.RuntimeNodeOrientation>();
            renderModel.RuntimeNodeOrientations.Add(new RenderModel.RuntimeNodeOrientation()
            {
                Rotation = new RealQuaternion(0, 0, 0, -1),
                Translation = new RealPoint3d(0, 0, 0),
                Scale = 1.0f
            });

            renderModel.Compression = renderModel.Geometry.Compression;

            //copy over materials block, and reindex mesh part materials

            var materialMapping = new Dictionary<short, short>();
            var newmaterials = new List<RenderMaterial>();

            foreach (var mesh in renderModel.Geometry.Meshes)
            {
                foreach (var part in mesh.Parts)
                {
                    short newMateiralIndex;
                    if (!materialMapping.TryGetValue(part.MaterialIndex, out newMateiralIndex))
                    {
                        newMateiralIndex = (short)newmaterials.Count;
                        newmaterials.Add(ConvertData(StructureBsp.Materials[part.MaterialIndex]));
                    }
                    part.MaterialIndex = newMateiralIndex;
                }
            }

            renderModel.Materials = newmaterials;

            //copy over "bounding spheres" block, and reindex
            var newBoundingSpheres = new List<RenderGeometry.BoundingSphere>();
            for (int i = 0; i < renderModel.Geometry.Meshes.Count; i++)
            {
                var mesh = renderModel.Geometry.Meshes[i];
                for (int j = 0; j < mesh.Parts.Count; j++)
                {
                    var part = mesh.Parts[j];
                    if (part.TransparentSortingIndex == -1)
                        continue;

                    var newIndex = (short)newBoundingSpheres.Count;
                    var datum = ConvertData(Lbsp.Geometry.BoundingSpheres[part.TransparentSortingIndex]);
                    newBoundingSpheres.Add(datum);
                    part.TransparentSortingIndex = newIndex;
                }
            }
            renderModel.Geometry.BoundingSpheres = newBoundingSpheres;

            return renderModel;
        }

        private RenderGeometry ConvertInstanceRenderGeometry(int instanceIndex)
        {
            var instance = StructureBsp.InstancedGeometryInstances[instanceIndex];
            var instanceDef = StructureBspResources.InstancedGeometry[instance.MeshIndex];
            var mesh = Lbsp.Geometry.Meshes[instanceDef.MeshIndex];

            var resourceDefinition = GetSingleMeshResourceDefinition(Lbsp.Geometry, instanceDef.MeshIndex);

            var renderGeometry = new RenderGeometry();

            renderGeometry.Unknown2 = new List<RenderGeometry.UnknownBlock>();
            renderGeometry.Meshes = new List<Mesh>()
            {
                mesh
            };
            renderGeometry.MeshClusterVisibility = new List<RenderGeometry.MoppClusterVisiblity>()
            {
                Lbsp.Geometry.MeshClusterVisibility[instance.MeshIndex]
            };
            renderGeometry.Compression = new List<RenderGeometryCompression>
            {
                Lbsp.Geometry.Compression[instanceDef.CompressionIndex]
            };
            renderGeometry.InstancedGeometryPerPixelLighting = new List<RenderGeometry.StaticPerPixelLighting>();

            if (instance.LodDataIndex != -1)
                renderGeometry.InstancedGeometryPerPixelLighting.Add(
                    Lbsp.Geometry.InstancedGeometryPerPixelLighting[instance.LodDataIndex]);

            if (SourceCache.Version != DestCache.Version)
            {
                var renderGeometryConverter = new RenderGeometryConverter(DestCache, SourceCache);
                resourceDefinition = renderGeometryConverter.Convert(renderGeometry, resourceDefinition);

                var staticPerVertexLighting = Lbsp.StaticPerVertexLightingBuffers[mesh.VertexBufferIndices[0]];
                if (staticPerVertexLighting.VertexBufferIndex != -1)
                {
                    var blamResourceDefinition = SourceCache.ResourceCache.GetRenderGeometryApiResourceDefinition(Lbsp.Geometry.Resource);
                    staticPerVertexLighting.VertexBuffer = blamResourceDefinition.VertexBuffers[staticPerVertexLighting.VertexBufferIndex].Definition;
                    VertexBufferConverter.ConvertVertexBuffer(SourceCache.Version, DestCache.Version, staticPerVertexLighting.VertexBuffer);
                    var d3dPointer = new D3DStructure<VertexBufferDefinition>();
                    d3dPointer.Definition = staticPerVertexLighting.VertexBuffer;
                    resourceDefinition.VertexBuffers.Add(d3dPointer);
                    // set the new buffer index
                    staticPerVertexLighting.VertexBufferIndex = (short)(resourceDefinition.VertexBuffers.Elements.Count - 1);
                }
            }

            renderGeometry.Resource = DestCache.ResourceCache.CreateRenderGeometryApiResource(resourceDefinition);

            return renderGeometry;
        }

        private T ConvertData<T>(T data)
        {
            if (SourceCache.Version == DestCache.Version)
                return data;

            var resourceStreams = new Dictionary<ResourceLocation, Stream>();
            data = (T)PortTag.ConvertData(DestStream, SourceStream, resourceStreams, data, null, "");
            foreach (var stream in resourceStreams)
                stream.Value.Close();

            return data;
        }

        private static RenderGeometryApiResourceDefinition GetSingleMeshResourceDefinition(RenderGeometry renderGeometry, int meshindex)
        {
            RenderGeometryApiResourceDefinition result = new RenderGeometryApiResourceDefinition
            {
                IndexBuffers = new TagBlock<D3DStructure<IndexBufferDefinition>>(),
                VertexBuffers = new TagBlock<D3DStructure<VertexBufferDefinition>>()
            };

            // valid for gen3, InteropLocations should also point to the definition.
            result.IndexBuffers.AddressType = CacheAddressType.Definition;
            result.VertexBuffers.AddressType = CacheAddressType.Definition;

            var mesh = renderGeometry.Meshes[meshindex];

            for (int i = 0; i < mesh.ResourceVertexBuffers.Length; i++)
            {
                var vertexBuffer = mesh.ResourceVertexBuffers[i];
                if (vertexBuffer != null)
                {
                    var d3dPointer = new D3DStructure<VertexBufferDefinition>();
                    d3dPointer.Definition = vertexBuffer;
                    result.VertexBuffers.Add(d3dPointer);
                    mesh.VertexBufferIndices[i] = (short)(result.VertexBuffers.Elements.Count - 1);
                }
                else
                    mesh.VertexBufferIndices[i] = -1;
            }

            for (int i = 0; i < mesh.ResourceIndexBuffers.Length; i++)
            {
                var indexBuffer = mesh.ResourceIndexBuffers[i];
                if (indexBuffer != null)
                {
                    var d3dPointer = new D3DStructure<IndexBufferDefinition>();
                    d3dPointer.Definition = indexBuffer;
                    result.IndexBuffers.Add(d3dPointer);
                    mesh.IndexBufferIndices[i] = (short)(result.IndexBuffers.Elements.Count - 1);
                }
                else
                    mesh.IndexBufferIndices[i] = -1;
            }

            // if the mesh is unindexed the index in the index buffer should be 0, but the buffer is empty. Copying what h3\ho does.
            if (mesh.Flags.HasFlag(MeshFlags.MeshIsUnindexed))
            {
                mesh.IndexBufferIndices[0] = 0;
                mesh.IndexBufferIndices[1] = 0;
            }

            return result;
        }

        private static float ComputeRenderModelEnclosingRadius(RenderModel model)
        {
            var compressionInfo = model.Geometry.Compression[0];
            return Math.Max(compressionInfo.X.Length, 
                compressionInfo.Y.Length > compressionInfo.Z.Length ?
                compressionInfo.Y.Length : compressionInfo.Z.Length) * 2.0f;
        }
    }
}
