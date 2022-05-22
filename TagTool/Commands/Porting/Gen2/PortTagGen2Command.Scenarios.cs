using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Common;
using TagTool.Geometry;
using static TagTool.Commands.Porting.Gen2.Gen2BspGeometryConverter;
using TagTool.Commands.Common;
using TagTool.Havok;
using TagTool.Cache;
using System.IO;
using TagTool.IO;

namespace TagTool.Commands.Porting.Gen2
{
	partial class PortTagGen2Command : Command
	{
        public object ConvertScenario(object gen2Tag)
        {
            Scenario newScenario = new Scenario();
            


            return newScenario;
        }

        public TagStructure ConvertStructureBSP(TagTool.Tags.Definitions.Gen2.ScenarioStructureBsp gen2Tag, Stream cacheStream, string tagname)
        {
            ScenarioStructureBsp newSbsp = new ScenarioStructureBsp();
            newSbsp.UseResourceItems = 1; // use CollisionBspResource

            //materials
            newSbsp.Materials = new List<RenderMaterial>();
            foreach(var material in gen2Tag.Materials)
            {
                newSbsp.Materials.Add(new RenderMaterial
                {
                    RenderMethod = material.Shader == null ? Cache.TagCache.GetTag(@"shaders\invalid.shader") : material.Shader
                });
            }

            //collision materials
            newSbsp.CollisionMaterials = new List<ScenarioStructureBsp.CollisionMaterial>();
            foreach (var material in gen2Tag.CollisionMaterials)
            {
                newSbsp.CollisionMaterials.Add(new ScenarioStructureBsp.CollisionMaterial
                {
                    RenderMethod = material.NewShader == null ? Cache.TagCache.GetTag(@"shaders\invalid.shader") : material.NewShader
                });
            }

            //RENDER GEO RESOURCE
            //begin building render geo resource
            var builder = new RenderModelBuilder(Cache);
            builder.BeginRegion(StringId.Invalid);
            builder.BeginPermutation(StringId.Invalid);

            //COLLISION RESOURCE
            //create new collisionresource and populate values from tag
            StructureBspTagResources CollisionResource = new StructureBspTagResources();

            //main collision geometry
            CollisionResource.CollisionBsps = new TagBlock<CollisionGeometry>(CacheAddressType.Definition);

            foreach (var bsp in gen2Tag.CollisionBsp)
            {
                bsp.Bsp3dNodes.AddressType = CacheAddressType.Data;
                bsp.Planes.AddressType = CacheAddressType.Data;
                bsp.Leaves.AddressType = CacheAddressType.Data;
                bsp.Bsp2dReferences.AddressType = CacheAddressType.Data;
                bsp.Bsp2dNodes.AddressType = CacheAddressType.Data;
                bsp.Surfaces.AddressType = CacheAddressType.Data;
                bsp.Edges.AddressType = CacheAddressType.Data;
                bsp.Vertices.AddressType = CacheAddressType.Data;
                CollisionResource.CollisionBsps.Add(bsp);
            }

            //structure physics
            newSbsp.Physics = new ScenarioStructureBsp.StructurePhysics
            {
                MoppBoundsMin = gen2Tag.StructurePhysics.MoppBoundsMin,
                MoppBoundsMax = gen2Tag.StructurePhysics.MoppBoundsMax
            };

            byte[] moppdata = gen2Tag.StructurePhysics.MoppCode;
            newSbsp.Physics.CollisionMoppCodes = ConvertH2MOPP(moppdata);

            /*
            var moppCode = HavokMoppGenerator.GenerateMoppCode(gen2Tag.CollisionBsp[0]);
            if (moppCode == null)
                new TagToolError(CommandError.OperationFailed, "Failed to generate mopp code!");
            moppCode.Data.AddressType = CacheAddressType.Data;
            newSbsp.Physics.CollisionMoppCodes.Add(moppCode);
            */

            //world bounds
            newSbsp.WorldBoundsX = gen2Tag.WorldBoundsX;
            newSbsp.WorldBoundsY = gen2Tag.WorldBoundsY;
            newSbsp.WorldBoundsZ = gen2Tag.WorldBoundsZ;

            //leaves
            newSbsp.Leaves = new List<ScenarioStructureBsp.Leaf>();
            foreach(var leaf in gen2Tag.Leaves)
            {
                newSbsp.Leaves.Add(new ScenarioStructureBsp.Leaf
                {
                    ClusterNew = (byte)leaf.Cluster
                });
            };

            //acoustic sound clusters (needed to prevent crash)
            newSbsp.AcousticsSoundClusters = new List<ScenarioStructureBsp.StructureBspSoundClusterBlock>() {
                    new ScenarioStructureBsp.StructureBspSoundClusterBlock() {
                        PaletteIndex = -1,
                    }
                };

            //cluster portals
            newSbsp.ClusterPortals = new List<ScenarioStructureBsp.ClusterPortal>();
            foreach(var portal in gen2Tag.ClusterPortals)
            {
                var newportal = new ScenarioStructureBsp.ClusterPortal
                {
                    BackCluster = portal.BackCluster,
                    FrontCluster = portal.FrontCluster,
                    PlaneIndex = portal.PlaneIndex,
                    Centroid = portal.Centroid,
                    BoundingRadius = portal.BoundingRadius,
                    Flags = (ScenarioStructureBsp.ClusterPortal.FlagsValue)portal.Flags,
                    Vertices = new List<ScenarioStructureBsp.ClusterPortal.Vertex>()
                };
                foreach(var vertex in portal.Vertices)
                {
                    newportal.Vertices.Add(new ScenarioStructureBsp.ClusterPortal.Vertex
                    {
                        Position = vertex.Point
                    });
                }
                newSbsp.ClusterPortals.Add(newportal);
            }

            //cluster data
            newSbsp.Clusters = new List<ScenarioStructureBsp.Cluster>();
            foreach (var cluster in gen2Tag.Clusters)
            {
                //render geometry
                var compressor = new VertexCompressor(
                    cluster.SectionInfo.Compression.Count > 0 ?
                        cluster.SectionInfo.Compression[0] :
                        new RenderGeometryCompression
                        {
                            X = new Bounds<float>(0.0f, 1.0f),
                            Y = new Bounds<float>(0.0f, 1.0f),
                            Z = new Bounds<float>(0.0f, 1.0f),
                            U = new Bounds<float>(0.0f, 1.0f),
                            V = new Bounds<float>(0.0f, 1.0f),
                            U2 = new Bounds<float>(0.0f, 1.0f),
                            V2 = new Bounds<float>(0.0f, 1.0f),
                        });
                List<Gen2BSPResourceMesh> clustermeshes = ReadResourceMeshes(Gen2Cache, cluster.GeometryBlockInfo,
                    cluster.SectionInfo.TotalVertexCount, (RenderGeometryCompressionFlags)cluster.SectionInfo.GeometryCompressionFlags,
                    (TagTool.Tags.Definitions.Gen2.RenderModel.SectionLightingFlags)cluster.SectionInfo.SectionLightingFlags, compressor);

                if (clustermeshes.Count > 1)
                {
                    new TagToolWarning("cluster had >1 render mesh! Culling extras...");
                    clustermeshes = new List<Gen2BSPResourceMesh> { clustermeshes.First() };
                }

                BuildMeshes(builder, clustermeshes, (RenderGeometryClassification)cluster.SectionInfo.GeometryClassification,
                    cluster.SectionInfo.OpaqueMaxNodesVertex, 0);

                //fixup mesh part fields
                var newmesh = builder.Meshes.Last();
                for (var i = 0; i < newmesh.Mesh.Parts.Count; i++)
                {
                    newmesh.Mesh.Parts[i].FirstSubPartIndex = clustermeshes[0].Parts[i].FirstSubPartIndex;
                    newmesh.Mesh.Parts[i].SubPartCount = clustermeshes[0].Parts[i].SubPartCount;
                    newmesh.Mesh.Parts[i].TypeNew = (Part.PartTypeNew)clustermeshes[0].Parts[i].TypeOld;
                }

                //block values
                var newcluster = new ScenarioStructureBsp.Cluster
                {
                    //mesh that was just built
                    MeshIndex = (short)(builder.Meshes.Count - 1),
                    BoundsX = cluster.BoundsX,
                    BoundsY = cluster.BoundsY,
                    BoundsZ = cluster.BoundsZ,
                    AtmosphereIndex = -1,
                    CameraFxIndex = -1,
                    BackgroundSoundEnvironmentIndex = -1,
                    AcousticsSoundClusterIndex = 0,
                    Unknown3 = -1,
                    Unknown4 = -1,
                    Unknown5 = -1,
                    RuntimeDecalStartIndex = -1,
                    Portals = new List<ScenarioStructureBsp.Cluster.Portal>(),
                    InstancedGeometryPhysics = new ScenarioStructureBsp.Cluster.InstancedGeometryPhysicsData
                    {
                        ClusterIndex = gen2Tag.Clusters.IndexOf(cluster),
                        MoppCodes = cluster.CollisionMoppCode.Length > 0 ? ConvertH2MOPP(cluster.CollisionMoppCode) : null
                    }
                };

                //cluster portal indices
                foreach(var portal in cluster.Portals)
                {
                    newcluster.Portals.Add(new ScenarioStructureBsp.Cluster.Portal
                    {
                        PortalIndex = portal.PortalIndex
                    });
                };

                newSbsp.Clusters.Add(newcluster);
            }

            //instanced geometry definitions
            CollisionResource.InstancedGeometry = new TagBlock<InstancedGeometryBlock>(CacheAddressType.Definition);
            foreach(var instanced in gen2Tag.InstancedGeometriesDefinitions)
            {
                //render geometry
                var compressor = new VertexCompressor(
                    instanced.RenderInfo.SectionInfo.Compression.Count > 0 ?
                        instanced.RenderInfo.SectionInfo.Compression[0] :
                        new RenderGeometryCompression
                            {
                                X = new Bounds<float>(0.0f, 1.0f),
                                Y = new Bounds<float>(0.0f, 1.0f),
                                Z = new Bounds<float>(0.0f, 1.0f),
                                U = new Bounds<float>(0.0f, 1.0f),
                                V = new Bounds<float>(0.0f, 1.0f),
                                U2 = new Bounds<float>(0.0f, 1.0f),
                                V2 = new Bounds<float>(0.0f, 1.0f),
                            });
                List<Gen2BSPResourceMesh> instancemeshes = ReadResourceMeshes(Gen2Cache, instanced.RenderInfo.GeometryBlockInfo,
                    instanced.RenderInfo.SectionInfo.TotalVertexCount, (RenderGeometryCompressionFlags)instanced.RenderInfo.SectionInfo.GeometryCompressionFlags, 
                    (TagTool.Tags.Definitions.Gen2.RenderModel.SectionLightingFlags)instanced.RenderInfo.SectionInfo.SectionLightingFlags, compressor);
                
                if(instancemeshes.Count > 1)
                {
                    new TagToolWarning("instance had >1 render mesh! Culling extras...");
                    instancemeshes = new List<Gen2BSPResourceMesh> { instancemeshes.First() };
                }
                
                BuildMeshes(builder, instancemeshes, (RenderGeometryClassification)instanced.RenderInfo.SectionInfo.GeometryClassification, 
                    instanced.RenderInfo.SectionInfo.OpaqueMaxNodesVertex, 0);

                //fixup mesh part fields
                var newmesh = builder.Meshes.Last();
                for(var i = 0; i < newmesh.Mesh.Parts.Count; i++)
                {
                    newmesh.Mesh.Parts[i].FirstSubPartIndex = instancemeshes[0].Parts[i].FirstSubPartIndex;
                    newmesh.Mesh.Parts[i].SubPartCount = instancemeshes[0].Parts[i].SubPartCount;
                    newmesh.Mesh.Parts[i].TypeNew = (Part.PartTypeNew)instancemeshes[0].Parts[i].TypeOld;
                }

                //block values
                var newinstance = new InstancedGeometryBlock
                {
                    Checksum = instanced.Checksum,
                    BoundingSphereOffset = instanced.BoundingSphereCenter,
                    BoundingSphereRadius = instanced.BoundingSphereRadius,
                    //index of mesh just builts
                    MeshIndex = (short)(builder.Meshes.Count - 1),
                };

                var bsp = instanced.CollisionInfo;
                bsp.Bsp3dNodes.AddressType = CacheAddressType.Data;
                bsp.Planes.AddressType = CacheAddressType.Data;
                bsp.Leaves.AddressType = CacheAddressType.Data;
                bsp.Bsp2dReferences.AddressType = CacheAddressType.Data;
                bsp.Bsp2dNodes.AddressType = CacheAddressType.Data;
                bsp.Surfaces.AddressType = CacheAddressType.Data;
                bsp.Edges.AddressType = CacheAddressType.Data;
                bsp.Vertices.AddressType = CacheAddressType.Data;
                newinstance.CollisionInfo = bsp;

                //build mopp codes from collision info and add
                if (instanced.BspPhysics != null && instanced.BspPhysics.Count > 0)
                {
                    var mopps = ConvertH2MOPP(instanced.BspPhysics[0].MoppCodeData);
                    newinstance.CollisionMoppCodes = new TagBlock<TagHkpMoppCode>(CacheAddressType.Definition, mopps);

                    /*
                    var mopp = HavokMoppGenerator.GenerateMoppCode(newinstance.CollisionInfo);
                    if (mopp == null)
                        new TagToolError(CommandError.OperationFailed, "Failed to generate mopp code!");
                    mopp.Data.AddressType = CacheAddressType.Data;
                    newinstance.CollisionMoppCodes = new TagBlock<TagHkpMoppCode>(CacheAddressType.Definition);
                    newinstance.CollisionMoppCodes.Add(mopp);
                    */
                }

                CollisionResource.InstancedGeometry.Add(newinstance);
            }

            //instanced geometry instances
            newSbsp.InstancedGeometryInstances = new List<InstancedGeometryInstance>();
            foreach (var instanced in gen2Tag.InstancedGeometryInstances)
            {
                var newinstance = new InstancedGeometryInstance
                {
                    Scale = instanced.Scale,
                    Matrix = new RealMatrix4x3(instanced.Forward.I, instanced.Forward.J, instanced.Forward.K,
                    instanced.Left.I, instanced.Left.J, instanced.Left.K,
                    instanced.Up.I, instanced.Up.J, instanced.Up.K,
                    instanced.Position.X, instanced.Position.Y, instanced.Position.Z),
                    DefinitionIndex = instanced.InstanceDefinition,
                    LodDataIndex = -1,
                    CompressionIndex = -1,
                    Name = instanced.Name,
                    WorldBoundingSphereCenter = instanced.WorldBoundingSphereCenter,
                    BoundingSphereRadiusBounds = new Bounds<float>(0, instanced.BoundingSphereRadius),
                    PathfindingPolicy = (Scenery.PathfindingPolicyValue)instanced.PathfindingPolicy,
                    LightmappingPolicy = (InstancedGeometryInstance.InstancedGeometryLightmappingPolicy)instanced.LightmappingPolicy,
                };

                //make sure there is a bsp physics block in the instance def
                var instancedef = gen2Tag.InstancedGeometriesDefinitions[instanced.InstanceDefinition];
                if(instancedef.BspPhysics != null && instancedef.BspPhysics.Count > 0)
                {
                    newinstance.Flags |= InstancedGeometryInstance.FlagsValue.Collidable;
                    newinstance.BspPhysics = new List<CollisionBspPhysicsDefinition>
                    {
                        new CollisionBspPhysicsDefinition
                        {
                            MoppBvTreeShape = new CMoppBvTreeShape
                            {
                                Scale = 1.0f,
                                Type = 27
                            },
                            GeometryShape = new CollisionGeometryShape
                            {
                                AABB_Center = instancedef.BspPhysics[0].AABB_Center,
                                AABB_Half_Extents = instancedef.BspPhysics[0].AABB_Half_Extents,
                                Scale = 1.0f
                            }
                        }
                    };
                }

                newSbsp.InstancedGeometryInstances.Add(newinstance);
            }

            //close out render geo resource
            builder.EndPermutation();
            builder.EndRegion();
            RenderModel meshbuild = builder.Build(Cache.Serializer);
            
            //create empty pathfinding resource
            var pathfindingresource = new StructureBspCacheFileTagResources();
            pathfindingresource.Planes = new TagBlock<StructureSurfaceToTriangleMapping>(CacheAddressType.Data);
            pathfindingresource.SurfacePlanes = new TagBlock<StructureSurface>(CacheAddressType.Data);
            pathfindingresource.EdgeToSeams = new TagBlock<EdgeToSeamMapping>(CacheAddressType.Data) { new EdgeToSeamMapping() { SeamIndex = -1, SeamEdgeIndex = -1 } };
            pathfindingresource.PathfindingData = new TagBlock<Pathfinding.ResourcePathfinding>(CacheAddressType.Data);

            //write pathfinding resource
            newSbsp.PathfindingResource = Cache.ResourceCache.CreateStructureBspCacheFileResource(pathfindingresource);

            //write collision resource
            newSbsp.CollisionBspResource = Cache.ResourceCache.CreateStructureBspResource(CollisionResource);
            //write meshes and render model resource

            /*
            var lbsp = new ScenarioLightmapBspData();
            lbsp.Geometry = meshbuild.Geometry;
            var destinationTag = Cache.TagCache.AllocateTag(lbsp.GetType(), tagname);
            Cache.Serialize(cacheStream, destinationTag, lbsp);
            */

            newSbsp.Geometry = meshbuild.Geometry;

            return newSbsp;
        }

        public List<TagHkpMoppCode> ConvertH2MOPP(byte[] moppdata)
        {
            RealQuaternion moppoffset;
            using (var moppStream = new MemoryStream(moppdata))
            using (var moppReader = new EndianReader(moppStream, Gen2Cache.Endianness))
            {
                moppoffset = new RealQuaternion(moppReader.ReadSingle(), moppReader.ReadSingle(), 
                    moppReader.ReadSingle(), moppReader.ReadSingle());
            };

            byte[] newmoppdata = new byte[moppdata.Length - 48];
            Array.Copy(moppdata, 48, newmoppdata, 0, newmoppdata.Length);

            var result = new List<TagHkpMoppCode>
            {
                new TagHkpMoppCode
                {
                    Info = new CodeInfo
                    {
                        Offset = moppoffset
                    },
                    ArrayBase = new HkArrayBase
                    {
                        Size = (uint)newmoppdata.Length,
                        CapacityAndFlags = (uint)(newmoppdata.Length | 0x80000000)
                    },
                    Data = new TagBlock<byte>(CacheAddressType.Data)
                    {
                        Elements = newmoppdata.ToList()
                    }
                }
            };

            return result;
        }
    }
}
