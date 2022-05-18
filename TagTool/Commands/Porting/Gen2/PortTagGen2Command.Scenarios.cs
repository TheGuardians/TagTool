using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Common;
using TagTool.Geometry;
using static TagTool.Commands.Porting.Gen2.Gen2RenderGeometryConverter;
using TagTool.Commands.Common;
using TagTool.Havok;
using TagTool.Cache;

namespace TagTool.Commands.Porting.Gen2
{
	partial class PortTagGen2Command : Command
	{
        public object ConvertScenario(object gen2Tag)
        {
            Scenario newScenario = new Scenario();
            


            return newScenario;
        }

        public object ConvertStructureBSP(TagTool.Tags.Definitions.Gen2.ScenarioStructureBsp gen2Tag)
        {
            ScenarioStructureBsp newSbsp = new ScenarioStructureBsp();

            //RENDER GEO RESOURCE
            //begin building render geo resource
            var builder = new RenderModelBuilder(Cache);
            builder.BeginRegion(StringId.Invalid);
            builder.BeginPermutation(StringId.Invalid);

            //COLLISION RESOURCE
            //create new collisionresource and populate values from tag
            StructureBspTagResources CollisionResource = new StructureBspTagResources();

            //main collision geometry
            CollisionResource.CollisionBsps = new TagBlock<CollisionGeometry>();
            foreach (var collisiongeo in gen2Tag.CollisionBsp)
                CollisionResource.CollisionBsps.Add(collisiongeo);

            //structure physics
            newSbsp.Physics = new ScenarioStructureBsp.StructurePhysics
            {
                MoppBoundsMin = gen2Tag.StructurePhysics.MoppBoundsMin,
                MoppBoundsMax = gen2Tag.StructurePhysics.MoppBoundsMax
            };
            var moppCode = HavokMoppGenerator.GenerateMoppCode(gen2Tag.CollisionBsp[0]);
            if (moppCode == null)
                new TagToolError(CommandError.OperationFailed, "Failed to generate mopp code!");
            moppCode.Data.AddressType = CacheAddressType.Data;
            newSbsp.Physics.CollisionMoppCodes = new List<TagHkpMoppCode>();
            newSbsp.Physics.CollisionMoppCodes.Add(moppCode);

            //cluster data
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
                List<Gen2ResourceMesh> clustermeshes = ReadResourceMeshes(Gen2Cache, cluster.GeometryBlockInfo,
                    cluster.SectionInfo.TotalVertexCount, (RenderGeometryCompressionFlags)cluster.SectionInfo.GeometryCompressionFlags,
                    (TagTool.Tags.Definitions.Gen2.RenderModel.SectionLightingFlags)cluster.SectionInfo.SectionLightingFlags, compressor);

                if (clustermeshes.Count > 1)
                {
                    new TagToolWarning("cluster had >1 render mesh! Culling extras...");
                    clustermeshes = new List<Gen2ResourceMesh> { clustermeshes.First() };
                }

                BuildMeshes(builder, clustermeshes, (RenderGeometryClassification)cluster.SectionInfo.GeometryClassification,
                    cluster.SectionInfo.OpaqueMaxNodesVertex, 0);

                //block values
                var newcluster = new ScenarioStructureBsp.Cluster
                {
                    //mesh that was just built
                    MeshIndex = (short)(builder.Meshes.Count - 1),
                    BoundsX = cluster.BoundsX,
                    BoundsY = cluster.BoundsY,
                    BoundsZ = cluster.BoundsZ
                };
            }

            //instanced geometry definitions
            CollisionResource.InstancedGeometry = new TagBlock<InstancedGeometryBlock>();
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
                List<Gen2ResourceMesh> instancemeshes = ReadResourceMeshes(Gen2Cache, instanced.RenderInfo.GeometryBlockInfo,
                    instanced.RenderInfo.SectionInfo.TotalVertexCount, (RenderGeometryCompressionFlags)instanced.RenderInfo.SectionInfo.GeometryCompressionFlags, 
                    (TagTool.Tags.Definitions.Gen2.RenderModel.SectionLightingFlags)instanced.RenderInfo.SectionInfo.SectionLightingFlags, compressor);
                
                if(instancemeshes.Count > 1)
                {
                    new TagToolWarning("instance had >1 render mesh! Culling extras...");
                    instancemeshes = new List<Gen2ResourceMesh> { instancemeshes.First() };
                }
                
                BuildMeshes(builder, instancemeshes, (RenderGeometryClassification)instanced.RenderInfo.SectionInfo.GeometryClassification, 
                    instanced.RenderInfo.SectionInfo.OpaqueMaxNodesVertex, 0);

                //block values
                var newinstance = new InstancedGeometryBlock
                {
                    Checksum = instanced.Checksum,
                    BoundingSphereOffset = instanced.BoundingSphereCenter,
                    BoundingSphereRadius = instanced.BoundingSphereRadius,
                    CollisionInfo = instanced.CollisionInfo,
                    //index of mesh just built
                    MeshIndex = (short)(builder.Meshes.Count - 1),
                };

                //build mopp codes from collision info and add
                if (instanced.BspPhysics != null && instanced.BspPhysics.Count > 0)
                {
                    var mopp = HavokMoppGenerator.GenerateMoppCode(newinstance.CollisionInfo);
                    if (mopp == null)
                        new TagToolError(CommandError.OperationFailed, "Failed to generate mopp code!");
                    mopp.Data.AddressType = CacheAddressType.Data;
                    newinstance.CollisionMoppCodes = new TagBlock<TagHkpMoppCode>(CacheAddressType.Definition);
                    newinstance.CollisionMoppCodes.Add(mopp);
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
                    Flags = (InstancedGeometryInstance.FlagsValue)instanced.Flags,
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
                    newinstance.BspPhysics = new List<CollisionBspPhysicsDefinition>
                    {
                        new CollisionBspPhysicsDefinition
                        {
                            MoppBvTreeShape = new CMoppBvTreeShape(),
                            GeometryShape = new CollisionGeometryShape
                            {
                                AABB_Center = instancedef.BspPhysics[0].AABB_Center,
                                AABB_Half_Extents = instancedef.BspPhysics[0].AABB_Half_Extents
                            }
                        }
                    };
                }

                newSbsp.InstancedGeometryInstances.Add(newinstance);
            }

            //close out render geo resource and prepare to load it in to the lightmap
            builder.EndPermutation();
            builder.EndRegion();
            RenderModel result = builder.Build(Cache.Serializer);

            return newSbsp;
        }
    }
}
