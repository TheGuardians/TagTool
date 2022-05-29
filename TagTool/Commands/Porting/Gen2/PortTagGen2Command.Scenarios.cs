﻿using System;
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
using TagTool.Serialization;
using TagTool.Geometry.Utils;

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
            newSbsp.ImportVersion = 7;

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

            int collisionEdgeCount = 0;
            foreach (var bsp in gen2Tag.CollisionBsp)
            {
                var newBsp = ConvertCollisionGeometry(bsp);
                newBsp.Bsp3dNodes.AddressType = CacheAddressType.Data;
                newBsp.Planes.AddressType = CacheAddressType.Data;
                newBsp.Leaves.AddressType = CacheAddressType.Data;
                newBsp.Bsp2dReferences.AddressType = CacheAddressType.Data;
                newBsp.Bsp2dNodes.AddressType = CacheAddressType.Data;
                newBsp.Surfaces.AddressType = CacheAddressType.Data;
                newBsp.Edges.AddressType = CacheAddressType.Data;
                newBsp.Vertices.AddressType = CacheAddressType.Data;
                collisionEdgeCount = newBsp.Edges.Count;
                CollisionResource.CollisionBsps.Add(newBsp);
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

            //transparent planes
            newSbsp.TransparentPlanes = new List<ScenarioStructureBsp.TransparentPlane>();
            foreach(var plane in gen2Tag.TransparentPlanes)
            {
                newSbsp.TransparentPlanes.Add(new ScenarioStructureBsp.TransparentPlane
                {
                    MeshIndex = plane.SectionIndex,
                    PartIndex = plane.PartIndex,
                    Plane = plane.Plane
                });
            }

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

            List<Gen2BSPResourceMesh> Gen2Meshes = new List<Gen2BSPResourceMesh>();

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
                Gen2Meshes.AddRange(clustermeshes);

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
                Gen2Meshes.AddRange(instancemeshes);

                //block values
                var newinstance = new InstancedGeometryBlock
                {
                    Checksum = instanced.Checksum,
                    BoundingSphereOffset = instanced.BoundingSphereCenter,
                    BoundingSphereRadius = instanced.BoundingSphereRadius,
                    //index of mesh just builts
                    MeshIndex = (short)(builder.Meshes.Count - 1),
                };

                var bsp = ConvertCollisionGeometry(instanced.CollisionInfo);
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
                    BoundingSphereRadiusBounds = new Bounds<float>(instanced.BoundingSphereRadius, instanced.BoundingSphereRadius),
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
            pathfindingresource.EdgeToSeams = new TagBlock<EdgeToSeamMapping>(CacheAddressType.Data);
            for (int i = 0; i < collisionEdgeCount; i++)
                pathfindingresource.EdgeToSeams.Add(new EdgeToSeamMapping() { SeamIndex = -1, SeamEdgeIndex = -1 });

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

            //fixup per mesh visibility mopp
            newSbsp.Geometry.MeshClusterVisibility = new List<RenderGeometry.MoppClusterVisiblity>();
            newSbsp.Geometry.PerMeshSubpartVisibility = new List<RenderGeometry.PerMeshSubpartVisibilityBlock>();
            for(var i = 0; i < Gen2Meshes.Count; i++)
            {
                Gen2BSPResourceMesh gen2mesh = Gen2Meshes[i];
                //mesh visibility mopp and mopp reorder table
                if(gen2mesh.VisibilityMoppCodeData.Length > 0 && gen2mesh.MoppReorderTable != null)
                {
                    newSbsp.Geometry.MeshClusterVisibility.Add(new RenderGeometry.MoppClusterVisiblity
                    {
                        MoppData = ConvertH2MoppData(gen2mesh.VisibilityMoppCodeData),
                        UnknownMeshPartIndicesCount = gen2mesh.MoppReorderTable.Select(m => m.Index).ToList()
                    });
                }
                //visibility bounds (approximate conversion)
                for(var j = 0; j < gen2mesh.VisibilityBounds.Count; j++)
                {
                    newSbsp.Geometry.PerMeshSubpartVisibility.Add(new RenderGeometry.PerMeshSubpartVisibilityBlock
                    {
                        BoundingSpheres = new List<RenderGeometry.BoundingSphere> { new RenderGeometry.BoundingSphere
                    {
                        Position = gen2mesh.VisibilityBounds[j].Position,
                        Radius = gen2mesh.VisibilityBounds[j].Radius,
                        NodeIndices = new sbyte[]{ (sbyte)gen2mesh.VisibilityBounds[j].NodeIndex, 0, 0, 0}
                    } }
                    });
                }

            }

            InstanceBucketGenerator.Generate(newSbsp, CollisionResource);
            ConvertGen2EnvironmentMopp(newSbsp);
            return newSbsp;
        }

        public List<TagHkpMoppCode> ConvertH2MOPP(byte[] moppdata)
        {
            var result = new List<TagHkpMoppCode>();

            using (var moppStream = new MemoryStream(moppdata))
            using (var moppReader = new EndianReader(moppStream, Gen2Cache.Endianness))
            {
                var context = new DataSerializationContext(moppReader);
                var deserializer = new TagDeserializer(Gen2Cache.Version, Gen2Cache.Platform);
                while(!moppReader.EOF)
                {
                    long startOffset = moppReader.Position;
                    Havok.Gen2.MoppCodeHeader moppHeader = deserializer.Deserialize<Havok.Gen2.MoppCodeHeader>(context);
                    byte[] moppCode = moppReader.ReadBytes((int)(moppHeader.Size - 0x30));
                    moppReader.SeekTo((startOffset + moppHeader.Size) + 0xF & ~0xF);

                    result.Add(new TagHkpMoppCode
                    {
                        Info = new CodeInfo
                        {
                            Offset = moppHeader.Offset
                        },
                        ArrayBase = new HkArrayBase
                        {
                            Size = (uint)moppCode.Length,
                            CapacityAndFlags = (uint)(moppCode.Length | 0x80000000)
                        },
                        Data = new TagBlock<byte>(CacheAddressType.Data)
                        {
                            Elements = moppCode.ToList()
                        }
                    });
                }
            }

            return result;
        }

        public byte[] ConvertH2MoppData(byte[] data)
        {
            if (data == null || data.Length == 0)
                return data;

            byte[] result;
            using (var inputReader = new EndianReader(new MemoryStream(data), CacheVersionDetection.IsLittleEndian(Gen2Cache.Version, Gen2Cache.Platform) ? EndianFormat.LittleEndian : EndianFormat.BigEndian))
            using (var outputStream = new MemoryStream())
            using (var outputWriter = new EndianWriter(outputStream, CacheVersionDetection.IsLittleEndian(Cache.Version, Cache.Platform) ? EndianFormat.LittleEndian : EndianFormat.BigEndian))
            {
                var dataContext = new DataSerializationContext(inputReader, outputWriter);
                var deserializer = new TagDeserializer(Gen2Cache.Version, Gen2Cache.Platform);
                var serializer = new TagSerializer(Cache.Version, Cache.Platform);
                while (!inputReader.EOF)
                {
                    var header = deserializer.Deserialize<Havok.Gen2.MoppCodeHeader>(dataContext);
                    var dataSize = header.Size - 0x30;
                    var nextOffset = (inputReader.Position + dataSize) + 0xf & ~0xf;
                    

                    List<byte> moppCodes = new List<byte>();
                    for (int j = 0; j < dataSize; j++)
                    {
                        moppCodes.Add(inputReader.ReadByte());
                    }
                    inputReader.SeekTo(nextOffset);

                    var newHeader = new HkpMoppCode
                    {
                        Info = new CodeInfo
                        {
                            Offset = header.Offset
                        },
                        ArrayBase = new HkArrayBase
                        {
                            Size = (uint)moppCodes.Count,
                            CapacityAndFlags = (uint)(moppCodes.Count | 0x80000000)
                        }
                    };

                    serializer.Serialize(dataContext, newHeader);
                    for (int j = 0; j < moppCodes.Count; j++)
                        outputWriter.Write(moppCodes[j]);

                    StreamUtil.Align(outputStream, 0x10);
                }
                result = outputStream.ToArray();
            }
            return result;
        }

        void ConvertGen2EnvironmentMopp(ScenarioStructureBsp sbsp)
        {
            if (sbsp.Physics.CollisionMoppCodes.Count == 0)
                return;

            var data = sbsp.Physics.CollisionMoppCodes[0].Data;
            for (int i = 0; i < data.Count; i++)
            {
                switch (data[i])
                {
                    case 0x00: // HK_MOPP_RETURN
                        break;
                    case 0x05: // HK_MOPP_JUMP8
                    case 0x09: // HK_MOPP_TERM_REOFFSET8
                    case 0x50: // HK_MOPP_TERM8
                    case 0x54: // HK_MOPP_NTERM_8
                    case 0x60: // HK_MOPP_PROPERTY8_0
                    case 0x61: // HK_MOPP_PROPERTY8_1
                    case 0x62: // HK_MOPP_PROPERTY8_2
                    case 0x63: // HK_MOPP_PROPERTY8_3
                        i += 1;
                        break;
                    case 0x06: // HK_MOPP_JUMP16
                    case 0x0A: // HK_MOPP_TERM_REOFFSET16
                    case 0x0C: // HK_MOPP_JUMP_CHUNK
                    case 0x20: // HK_MOPP_SINGLE_SPLIT_X
                    case 0x21: // HK_MOPP_SINGLE_SPLIT_Y
                    case 0x22: // HK_MOPP_SINGLE_SPLIT_Z
                    case 0x26: // HK_MOPP_DOUBLE_CUT_X
                    case 0x27: // HK_MOPP_DOUBLE_CUT_Y
                    case 0x28: // HK_MOPP_DOUBLE_CUT_Z
                    case 0x51: // HK_MOPP_TERM16
                    case 0x55: // HK_MOPP_NTERM_16
                    case 0x64: // HK_MOPP_PROPERTY16_0
                    case 0x65: // HK_MOPP_PROPERTY16_1
                    case 0x66: // HK_MOPP_PROPERTY16_2
                    case 0x67: // HK_MOPP_PROPERTY16_3
                        i += 2;
                        break;
                    case 0x01: // HK_MOPP_SCALE1
                    case 0x02: // HK_MOPP_SCALE2
                    case 0x03: // HK_MOPP_SCALE3
                    case 0x04: // HK_MOPP_SCALE4
                    case 0x07: // HK_MOPP_JUMP24
                    case 0x10: // HK_MOPP_SPLIT_X
                    case 0x11: // HK_MOPP_SPLIT_Y
                    case 0x12: // HK_MOPP_SPLIT_Z
                    case 0x13: // HK_MOPP_SPLIT_YZ
                    case 0x14: // HK_MOPP_SPLIT_YMZ
                    case 0x15: // HK_MOPP_SPLIT_XZ
                    case 0x16: // HK_MOPP_SPLIT_XMZ
                    case 0x17: // HK_MOPP_SPLIT_XY
                    case 0x18: // HK_MOPP_SPLIT_XMY
                    case 0x19: // HK_MOPP_SPLIT_XYZ
                    case 0x1A: // HK_MOPP_SPLIT_XYMZ
                    case 0x1B: // HK_MOPP_SPLIT_XMYZ
                    case 0x1C: // HK_MOPP_SPLIT_XMYMZ
                    case 0x52: // HK_MOPP_TERM24
                    case 0x56: // HK_MOPP_NTERM_24
                        i += 3;
                        break;
                    case 0x57: // HK_MOPP_NTERM_32
                    case 0x68: // HK_MOPP_PROPERTY32_0
                    case 0x69: // HK_MOPP_PROPERTY32_1
                    case 0x6A: // HK_MOPP_PROPERTY32_2
                    case 0x6B: // HK_MOPP_PROPERTY32_3
                        i += 4;
                        break;
                    case 0x0D: // HK_MOPP_DATA_OFFSET
                        i += 5;
                        break;
                    case 0x23: // HK_MOPP_SPLIT_JUMP_X
                    case 0x24: // HK_MOPP_SPLIT_JUMP_Y
                    case 0x25: // HK_MOPP_SPLIT_JUMP_Z
                    case 0x29: // HK_MOPP_DOUBLE_CUT24_X
                    case 0x2A: // HK_MOPP_DOUBLE_CUT24_Y
                    case 0x2B: // HK_MOPP_DOUBLE_CUT24_Z
                        i += 6;
                        break;
                    case byte op when op >= 0x30 && op <= 0x4F:
                        break;
                    case 0x0B: // HK_MOPP_TERM_REOFFSET32
                    case 0x53: // HK_MOPP_TERM32
                        int key = data[i + 4] + ((data[i + 3] + ((data[i + 2] + (data[i + 1] << 8)) << 8)) << 8);
                        Console.WriteLine(key);
                        key = ConvertShapeKey(key);
                        data[i + 1] = (byte)((key & 0x7F000000) >> 24);
                        data[i + 2] = (byte)((key & 0x00FF0000) >> 16);
                        data[i + 3] = (byte)((key & 0x0000FF00) >> 8);
                        data[i + 4] = (byte)(key & 0x000000FF);
                        i += 4;
                        break;
                    default:
                        throw new NotSupportedException($"Opcode 0x{data[i]:X2}");
                }
            }

            int ConvertShapeKey(int key)
            {
                int type = key >> 29;
                if (type == 1)
                    key |= (5 << 26); // needed to pass group filter
                return key;
            }
        }
    }
}
