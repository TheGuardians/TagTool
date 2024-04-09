using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Numerics;
using TagTool.Cache;
using TagTool.Commands.Porting;
using TagTool.Common;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using TagTool.Commands.CollisionModels.OffsetCollisonBsp;
using TagTool.Commands.CollisionModels;

namespace TagTool.Geometry.Utils
{
    public class GeometryToObjectConverter
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
        private StructureBspCacheFileTagResources StructureBspCacheFileResources;
        private Dictionary<short, short> CollisionMaterialMapping;
        private RealPoint3d GeometryOffset;
        private RenderGeometryCompression OriginalCompression;
        private bool HasValidCollisions = true;
        public PortTagCommand PortTag { get; private set; }

        public GeometryToObjectConverter(
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



            if (SourceCache.Version >= CacheVersion.Halo3ODST || (SourceCache.Version >= CacheVersion.Halo3Retail && SourceCache.Platform == CachePlatform.MCC))
            {
                foreach (var lbspTag in sLdT.PerPixelLightmapDataReferences.Select(r => r.LightmapBspData))
                {
                    if (lbspTag == null)
                        continue;
                    var test_lbsp = SourceCache.Deserialize<ScenarioLightmapBspData>(SourceStream, lbspTag);

                    if (StructureBspIndex == test_lbsp.BspIndex)
                    {
                        Lbsp = test_lbsp;
                        break;
                    }
                }
            }
            else
            {
                foreach(var lightmapdata in sLdT.Lightmaps)
                {
                    if (lightmapdata.BspIndex == StructureBspIndex)
                    {
                        Lbsp = lightmapdata;
                        break;
                    }
                }
            }
           
            var resourceDefinition = SourceCache.ResourceCache.GetRenderGeometryApiResourceDefinition(Lbsp.Geometry.Resource);
            Lbsp.Geometry.SetResourceBuffers(resourceDefinition, false);

            StructureBspResources = SourceCache.ResourceCache.GetStructureBspTagResources(StructureBsp.CollisionBspResource);
            StructureBspCacheFileResources = SourceCache.ResourceCache.GetStructureBspCacheFileTagResources(StructureBsp.PathfindingResource);
        }

        public CachedTag ConvertGeometry(int geometryIndex, string desiredTagName = null, bool iscluster = false, bool centergeometry = true)
        {
            //return null tag and skip to next bsp if bsp resources is null
            if (StructureBspResources == null)
                return null;

            var scenarioFolder = Path.GetDirectoryName(Scenario.StructureBsps[StructureBspIndex].StructureBsp.Name);
            string bspindex = Scenario.StructureBsps.Count() > 1 ? $"{StructureBspIndex}" + "_" : "";
            StringId geoID = StringId.Invalid;
            if (SourceCache.Version >= CacheVersion.HaloReach)
                geoID = StructureBsp.InstancedGeometryInstanceNames[geometryIndex].Name;
            else
                geoID = StructureBsp.InstancedGeometryInstances[geometryIndex].Name;

            string geoname = SourceCache.StringTable.GetString(geoID);

            var tagName = $"objects\\{scenarioFolder}\\instanced\\{bspindex}{geometryIndex}_{geoname}";

            if (iscluster)
            {
                tagName = $"objects\\{scenarioFolder}\\clusters\\{bspindex}{geometryIndex}";
            }
                      
            if (desiredTagName != null && desiredTagName != string.Empty)
                tagName = desiredTagName;

            // reset state
            CollisionMaterialMapping = new Dictionary<short, short>();

            // check if we've already converted the tag, if so just return it
            CachedTag scenTag;
            if (DestCache.TagCache.TryGetTag<Scenery>(tagName, out scenTag))
                return scenTag;

            // allocate tags
            var modelTag = DestCache.TagCache.AllocateTag<Model>(tagName);
            var renderModelTag = DestCache.TagCache.AllocateTag<RenderModel>(tagName);
            var collisionModelTag = DestCache.TagCache.AllocateTag<CollisionModel>(tagName);
            scenTag = DestCache.TagCache.AllocateTag<Scenery>(tagName);

            // generate the definitions
            var renderModel = GenerateRenderModel(geometryIndex, iscluster);
            var collisionModel = GenerateCollisionModel(modelTag, geometryIndex, iscluster);
            var model = GenerateModel(renderModel, collisionModel);
            var gameObject = GenerateObject(geometryIndex, ComputeRenderModelEnclosingRadius(renderModel));

            //fix items being offset from origin
            //first determine what the offset is from the origin
            OriginalCompression = renderModel.Geometry.Compression[0].DeepClone();
            RealVector3d scale = new RealVector3d
            {
                I = renderModel.Geometry.Compression[0].X.Upper - renderModel.Geometry.Compression[0].X.Lower,
                J = renderModel.Geometry.Compression[0].Y.Upper - renderModel.Geometry.Compression[0].Y.Lower,
                K = renderModel.Geometry.Compression[0].Z.Upper - renderModel.Geometry.Compression[0].Z.Lower
            };

            GeometryOffset.X = OriginalCompression.X.Lower - (-scale.I / 2);
            GeometryOffset.Y = OriginalCompression.Y.Lower - (-scale.J / 2);
            GeometryOffset.Z = OriginalCompression.Z.Lower - (-scale.K / 2);

            if (collisionModel.Regions[0].Permutations[0].Bsps[0].Geometry.Vertices.Count < 1 || collisionModel.Regions[0].Permutations[0].BspPhysics.Count < 1)
                HasValidCollisions = false;

            //if the offset from the origin is >2 units in any dimension, center the object
            if(centergeometry && (Math.Abs(GeometryOffset.X) >= 2.0f || Math.Abs(GeometryOffset.Y) >= 2.0f || Math.Abs(GeometryOffset.Z) >= 2.0f))
            {
                if (HasValidCollisions)
                {
                    var newCollisionGeometry = collisionModel.Regions[0].Permutations[0].Bsps[0].Geometry;

                    //proceed only if bsp generation succeeds, otherwise just revert to noncentered
                    OffsetCollisionBSP offset_class = new OffsetCollisionBSP();
                    if (offset_class.offset_bsp(GeometryOffset, ref newCollisionGeometry))
                    {
                        //apply results of function
                        collisionModel.Regions[0].Permutations[0].Bsps[0].Geometry = newCollisionGeometry;

                        //offset MOPP extents to origin
                        foreach (var bspPhysics in collisionModel.Regions[0].Permutations[0].BspPhysics)
                        {
                            bspPhysics.GeometryShape.AABB_Center = new RealQuaternion(
                                bspPhysics.GeometryShape.AABB_Center.I - GeometryOffset.X,
                                bspPhysics.GeometryShape.AABB_Center.J - GeometryOffset.Y,
                                bspPhysics.GeometryShape.AABB_Center.K - GeometryOffset.Z,
                                bspPhysics.GeometryShape.AABB_Center.W);
                        }
                        //fix mopp code offsets to origin
                        foreach (var mopp in collisionModel.Regions[0].Permutations[0].BspMoppCodes)
                        {
                            mopp.Info.Offset = new RealQuaternion(
                                mopp.Info.Offset.I - GeometryOffset.X,
                                mopp.Info.Offset.J - GeometryOffset.Y,
                                mopp.Info.Offset.K - GeometryOffset.Z,
                                mopp.Info.Offset.W);
                        }
                        //set render model compression to center around origin
                        renderModel.Geometry.Compression[0].X = new Bounds<float>(-scale.I / 2, scale.I / 2);
                        renderModel.Geometry.Compression[0].Y = new Bounds<float>(-scale.J / 2, scale.J / 2);
                        renderModel.Geometry.Compression[0].Z = new Bounds<float>(-scale.K / 2, scale.K / 2);
                    }
                }
                else
                {
                    //set render model compression to center around origin
                    renderModel.Geometry.Compression[0].X = new Bounds<float>(-scale.I / 2, scale.I / 2);
                    renderModel.Geometry.Compression[0].Y = new Bounds<float>(-scale.J / 2, scale.J / 2);
                    renderModel.Geometry.Compression[0].Z = new Bounds<float>(-scale.K / 2, scale.K / 2);
                }
            }

            // fixup tag refs
            //if coll is empty but referenced here, the game crashes. Only reference coll if it has contents.
            if (HasValidCollisions)
            {
                model.CollisionModel = collisionModelTag;
            }
            model.RenderModel = renderModelTag;
            gameObject.Model = modelTag;

            // finally serialize all the tags
            DestCache.Serialize(DestStream, renderModelTag, renderModel);
            DestCache.Serialize(DestStream, collisionModelTag, collisionModel);
            DestCache.Serialize(DestStream, modelTag, model);
            DestCache.Serialize(DestStream, scenTag, gameObject);

            Console.WriteLine($"['{renderModelTag.Group}', 0x{renderModelTag.Index:X04}] {renderModelTag.Name}");
            Console.WriteLine($"['{collisionModelTag.Group}', 0x{collisionModelTag.Index:X04}] {collisionModelTag.Name}");
            Console.WriteLine($"['{modelTag.Group}', 0x{modelTag.Index:X04}] {modelTag.Name}");
            Console.WriteLine($"['{scenTag.Group}', 0x{scenTag.Index:X04}] {scenTag.Name}");

            return scenTag;
        }

        private GameObject GenerateObject(int instancedGeometryInstanceIndex, float boundingSphere)
        {
            var scenery = new Scenery()
            {
                ObjectType = new GameObjectType16() { Halo3ODST = GameObjectTypeHalo3ODST.Scenery }, // TODO: generic object type
                BoundingRadius = boundingSphere,
                AccelerationScale = 1.0f,
                SweetenerSize = GameObject.SweetenerSizeValue.Default,
                MultiplayerObject = new List<GameObject.MultiplayerObjectBlock>()
                {
                    new GameObject.MultiplayerObjectBlock() { DefaultSpawnTime = 30, DefaultAbandonTime = 60 }
                },
                // SceneryFlags = Scenery.SceneryFlagBits.PhysicallySimulates // static lighting updates (breaks player collision :/)
            };

            return scenery;
        }

        private Model GenerateModel(RenderModel renderModel, CollisionModel collisionModel)
        {
            var model = new Model();
            model.DisappearDistance = 300.0f;
            model.BeginFadeDistance = 280.0f;

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
                    MaterialName = StringId.Invalid,
                    MaterialType = Model.Material.MaterialTypeValue.Dirt,
                    DamageSectionIndex = -1,
                    RuntimeDamagerMaterialIndex = -1,
                    RuntimeCollisionMaterialIndex = 0,
                    GlobalMaterialName = StringId.Invalid,
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
        private float PointToPlaneDistance(Vector3 pointPosition, Vector3 planePosition, Vector3 planeNormal)
        {
            float sb, sn, sd;

            sn = -Vector3.Dot(planeNormal, (pointPosition - planePosition));
            sd = Vector3.Dot(planeNormal, planeNormal);
            sb = sn / sd;

            Vector3 result = pointPosition + sb * planeNormal;
            return Vector3.Distance(pointPosition, result);
        }

        private CollisionModel GenerateCollisionModel(CachedTag modelTag, int geometryIndex, bool iscluster)
        {
            var collisionModel = new CollisionModel();
            collisionModel.Regions = new List<CollisionModel.Region>();

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
            var newCollisionGeometry = new BspCollisionGeometry.CollisionGeometry();

            //instanced geometry 
            if (!iscluster)
            {
                //bsp physics
                if(SourceCache.Version < CacheVersion.HaloReach)
                {
                    var instancedGeometryInstance = StructureBsp.InstancedGeometryInstances[geometryIndex];
                    var instancedGeometryDef = StructureBspResources.InstancedGeometry[instancedGeometryInstance.DefinitionIndex];

                    foreach (var bspPhysics in instancedGeometryInstance.BspPhysics)
                    {
                        var data = ConvertData(bspPhysics);
                        permutation.BspPhysics.Add(new CollisionBspPhysicsDefinition()
                        {
                            GeometryShape = data.GeometryShape,
                            MoppBvTreeShape = data.MoppBvTreeShape
                        });
                    }

                    //mopps
                    foreach (var mopp in instancedGeometryDef.CollisionMoppCodes)
                    {
                        permutation.BspMoppCodes.Add(ConvertData(mopp));
                    }

                    //collision geometry
                    newCollisionGeometry = instancedGeometryDef.CollisionInfo.DeepClone();
                }
                else
                {
                    var instancedGeometryInstance = StructureBspCacheFileResources.InstancedGeometryInstances[geometryIndex];
                    var instancedGeometryDef = StructureBspResources.InstancedGeometry[instancedGeometryInstance.DefinitionIndex];
                    //collision geometry
                    newCollisionGeometry = instancedGeometryDef.CollisionInfo.DeepClone();
                }
            }

            //cluster geometry
            else
            {
                var cluster = StructureBsp.Clusters[geometryIndex];

                // bsp physics & mopps
                foreach (var mopp in cluster.InstancedGeometryPhysics.MoppCodes)
                {
                    permutation.BspMoppCodes.Add(ConvertData(mopp));

                    var bspPhysics =  new CollisionBspPhysicsDefinition
                    {
                        GeometryShape = new CollisionGeometryShape()
                        {
                            // need to double check this
                            AABB_Center = new RealQuaternion(cluster.BoundsX.Lower, cluster.BoundsY.Lower, cluster.BoundsZ.Lower, 0),
                            AABB_Half_Extents = new RealQuaternion(cluster.BoundsX.Upper, cluster.BoundsY.Upper, cluster.BoundsZ.Upper, 0),
                        },
                        MoppBvTreeShape = new Havok.CMoppBvTreeShape()
                    };
                    permutation.BspPhysics.Add(bspPhysics);
                }

                // collision geometry
                if (StructureBspResources.CollisionBsps.Count > 0)
                    newCollisionGeometry = StructureBspResources.CollisionBsps[0].DeepClone();

                //TODO: cull unnecessary parts of bsp collision
            }

            // fixup bsp fixups for collision model
            foreach (var bspPhysics in permutation.BspPhysics)
            {
                bspPhysics.GeometryShape.Model = modelTag;
                bspPhysics.GeometryShape.BspIndex = -1;
                bspPhysics.GeometryShape.CollisionGeometryShapeKey = 0xffff;
                bspPhysics.GeometryShape.CollisionGeometryShapeType = 0;
            }

            // fixup surfaces materials block
            // build a mapping of surface material indices to collision materials

            foreach (var surface in newCollisionGeometry.Surfaces)
            {
                if (surface.MaterialIndex == -1)
                    continue;

                short modelMaterialIndex;
                if (!CollisionMaterialMapping.TryGetValue(surface.MaterialIndex, out modelMaterialIndex))
                    CollisionMaterialMapping.Add(surface.MaterialIndex, modelMaterialIndex);

                surface.MaterialIndex = modelMaterialIndex;

                //Remove breakable surface flags, as this object will no longer be linked to sbsp and will cause crashes
                surface.Flags &= ~SurfaceFlags.Breakable;
            }

            // add the collision geometry
            permutation.Bsps.Add(new CollisionModel.Region.Permutation.Bsp()
            {
                NodeIndex = 0,
                Geometry = newCollisionGeometry
            });

            //check if the collision model actually has any contents, the game doesn't like empty collision model tags
            if (newCollisionGeometry.Vertices.Count() == 0 && permutation.BspMoppCodes.Count() == 0)
                HasValidCollisions = false;
            else
                HasValidCollisions = true;

            if(SourceCache.Version >= CacheVersion.HaloReach && HasValidCollisions == true)
            {
                new GenerateCollisionBSPCommand(ref collisionModel).Execute(new List<string>());
                var moppBuilt = (bool)new GenerateBspPhysicsCommand(ref collisionModel).Execute(new List<string>());
                if(moppBuilt)
                    collisionModel.Regions[0].Permutations[0].BspPhysics[0].GeometryShape.Model = modelTag;
            }

            return collisionModel;
        }

        private RenderModel GenerateRenderModel(int geometryIndex, bool iscluster)
        {
            var renderModel = new RenderModel();

            renderModel.Geometry = ConvertInstanceRenderGeometry(geometryIndex, iscluster);
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

            //copy over materials block, and reindex mesh part materials

            var materialMapping = new Dictionary<short, short>();
            var newmaterials = new List<RenderMaterial>();

            foreach (var mesh in renderModel.Geometry.Meshes)
            {
                foreach (var part in mesh.Parts)
                {
                    if(part.MaterialIndex != -1 && part.MaterialIndex < StructureBsp.Materials.Count)
                    {
                        short newMaterialIndex;
                        if (!materialMapping.TryGetValue(part.MaterialIndex, out newMaterialIndex))
                        {
                            newMaterialIndex = (short)newmaterials.Count;
                            newmaterials.Add(ConvertData(StructureBsp.Materials[part.MaterialIndex]));
                        }
                        part.MaterialIndex = newMaterialIndex;
                    }
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
                    datum.Position -= GeometryOffset;
                    newBoundingSpheres.Add(datum);
                    part.TransparentSortingIndex = newIndex;
                }
            }
            renderModel.Geometry.BoundingSpheres = newBoundingSpheres;

            return renderModel;
        }

        private RenderGeometry ConvertRenderGeometry(int meshindex, int compressionindex)
        {
            var resourceDefinition = GetSingleMeshResourceDefinition(Lbsp.Geometry, meshindex, out Mesh mesh);

            var renderGeometry = new RenderGeometry();

            renderGeometry.UserData = new List<RenderGeometry.UserDataBlock>();
            renderGeometry.Meshes = new List<Mesh>()
            {
                mesh
            };

            renderGeometry.MeshClusterVisibility = new List<RenderGeometry.PerMeshMoppBlock>();
            if (meshindex != -1 && meshindex < Lbsp.Geometry.MeshClusterVisibility.Count)
                renderGeometry.MeshClusterVisibility.Add(Lbsp.Geometry.MeshClusterVisibility[meshindex].DeepClone());

            //instanced geo has a compression index
            if (compressionindex != -1)
            {
                renderGeometry.Compression = new List<RenderGeometryCompression>();
                if (compressionindex != -1 && compressionindex < Lbsp.Geometry.Compression.Count)
                    renderGeometry.Compression.Add(Lbsp.Geometry.Compression[compressionindex].DeepClone());
            }

            renderGeometry.InstancedGeometryPerPixelLighting = new List<RenderGeometry.StaticPerPixelLighting>();

            if (meshindex < Lbsp.Geometry.InstancedGeometryPerPixelLighting.Count)
                renderGeometry.InstancedGeometryPerPixelLighting.Add(
                    Lbsp.Geometry.InstancedGeometryPerPixelLighting[meshindex].DeepClone());

            if (SourceCache.Version != DestCache.Version)
            {
                var renderGeometryConverter = new RenderGeometryConverter(DestCache, SourceCache);
                resourceDefinition = renderGeometryConverter.Convert(renderGeometry, resourceDefinition);
            }

            // convert world to rigid (or else it can't be dragged)
            if (mesh.ResourceVertexBuffers.Count() > 0 && mesh.ResourceVertexBuffers[0] != null && mesh.ResourceVertexBuffers[0].Format == VertexBufferFormat.World)
            {
                renderGeometry.Meshes[0].ResourceVertexBuffers[0].Format = VertexBufferFormat.Rigid;
                renderGeometry.Meshes[0].Type = VertexType.Rigid;
                renderGeometry.Meshes[0].RigidNodeIndex = 0;
                var compressionInfo = CompressVertexBuffer(mesh.ResourceVertexBuffers[0]);
                renderGeometry.Compression = new List<RenderGeometryCompression>() { compressionInfo };
            }

            renderGeometry.Resource = DestCache.ResourceCache.CreateRenderGeometryApiResource(resourceDefinition);

            return renderGeometry;
        }

        private RenderGeometry ConvertInstanceRenderGeometry(int geometryIndex, bool iscluster)
        {
            int meshindex;
            int compressionindex;

            if (!iscluster)
            {
                if (SourceCache.Version >= CacheVersion.HaloReach)
                {
                    var instance = StructureBspCacheFileResources.InstancedGeometryInstances[geometryIndex];
                    var instanceDef = StructureBspResources.InstancedGeometry[instance.DefinitionIndex];
                    meshindex = instanceDef.MeshIndex;
                    compressionindex = instanceDef.CompressionIndex;
                }
                else
                {
                    var instance = StructureBsp.InstancedGeometryInstances[geometryIndex];
                    var instanceDef = StructureBspResources.InstancedGeometry[instance.DefinitionIndex];
                    meshindex = instanceDef.MeshIndex;
                    compressionindex = instanceDef.CompressionIndex;
                }
            }
            else
            {
                meshindex = StructureBsp.Clusters[geometryIndex].MeshIndex;
                compressionindex = -1;
            }

            return ConvertRenderGeometry(meshindex, compressionindex);
        }

        private T ConvertData<T>(T data)
        {
            if (SourceCache.Version == DestCache.Version)
                return data;

            data = data.DeepClone();

            var resourceStreams = new Dictionary<ResourceLocation, Stream>();
            data = (T)PortTag.ConvertData(DestStream, SourceStream, resourceStreams, data, null, "");
            foreach (var stream in resourceStreams)
                stream.Value.Close();

            return data;
        }

        private RenderGeometryCompression CompressVertexBuffer(VertexBufferDefinition vertexBuffer)
        {
            Debug.Assert(vertexBuffer.Format == VertexBufferFormat.Rigid);

            var compression = new RenderGeometryCompression();

            var rigidVertices = new List<RigidVertex>();
            using (var stream = new MemoryStream(vertexBuffer.Data.Data))
            {
                var vertexStream = VertexStreamFactory.Create(DestCache.Version, DestCache.Platform, stream);
                for (int i = 0; i < vertexBuffer.Count; i++)
                {
                    var vertex = vertexStream.ReadRigidVertex();
                    rigidVertices.Add(vertex);
                }
            }

            var positions = rigidVertices.Select(v => v.Position);
            var texCoords = rigidVertices.Select(v => v.Texcoord);

            if (positions != null && positions.Count() > 0)
            {
                compression.X.Lower = Math.Min(compression.X.Lower, positions.Min(v => v.I));
                compression.Y.Lower = Math.Min(compression.Y.Lower, positions.Min(v => v.J));
                compression.Z.Lower = Math.Min(compression.Z.Lower, positions.Min(v => v.K));
                compression.X.Upper = Math.Max(compression.X.Upper, positions.Max(v => v.I));
                compression.Y.Upper = Math.Max(compression.Y.Upper, positions.Max(v => v.J));
                compression.Z.Upper = Math.Max(compression.Z.Upper, positions.Max(v => v.K));
            }
            if (texCoords != null && texCoords.Count() > 0)
            {
                compression.U.Lower = Math.Min(compression.U.Lower, texCoords.Min(v => v.I));
                compression.V.Lower = Math.Min(compression.V.Lower, texCoords.Min(v => v.J));
                compression.U.Upper = Math.Max(compression.U.Upper, texCoords.Max(v => v.I));
                compression.V.Upper = Math.Max(compression.V.Upper, texCoords.Max(v => v.J));
            }

            var compressor = new VertexCompressor(compression);
            using (var outStream = new MemoryStream())
            {
                var outVertexStream = VertexStreamFactory.Create(DestCache.Version, DestCache.Platform, outStream);
                foreach (var vertex in rigidVertices)
                {
                    vertex.Position = compressor.CompressPosition(vertex.Position);
                    vertex.Texcoord = compressor.CompressUv(vertex.Texcoord);
                    outVertexStream.WriteRigidVertex(vertex);
                }

                vertexBuffer.Data.Data = outStream.ToArray();
            }

            return compression;
        }

        private static RenderGeometryApiResourceDefinition GetSingleMeshResourceDefinition(RenderGeometry renderGeometry, int meshindex, out Mesh mesh)
        {
            RenderGeometryApiResourceDefinition result = new RenderGeometryApiResourceDefinition
            {
                IndexBuffers = new TagBlock<D3DStructure<IndexBufferDefinition>>(),
                VertexBuffers = new TagBlock<D3DStructure<VertexBufferDefinition>>()
            };

            // valid for gen3, InteropLocations should also point to the definition.
            result.IndexBuffers.AddressType = CacheAddressType.Definition;
            result.VertexBuffers.AddressType = CacheAddressType.Definition;

            mesh = renderGeometry.Meshes[meshindex].DeepClone();

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
