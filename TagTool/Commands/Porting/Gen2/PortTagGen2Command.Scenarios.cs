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

            //cluster data
            foreach(var cluster in gen2Tag.ClusterData)
            {

            }

            //instanced geometry definitions
            CollisionResource.InstancedGeometry = new TagBlock<InstancedGeometryBlock>();
            foreach(var instanced in gen2Tag.InstancedGeometriesDefinitions)
            {
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
                    new TagToolWarning("instance had >1 mesh! Culling extras...");
                    instancemeshes = new List<Gen2ResourceMesh> { instancemeshes.First() };
                }
                
                BuildMeshes(builder, instancemeshes, (RenderGeometryClassification)instanced.RenderInfo.SectionInfo.GeometryClassification, 
                    instanced.RenderInfo.SectionInfo.OpaqueMaxNodesVertex, 0);

                CollisionResource.InstancedGeometry.Add(new InstancedGeometryBlock
                {
                    Checksum = instanced.Checksum,
                    BoundingSphereOffset = instanced.BoundingSphereCenter,
                    BoundingSphereRadius = instanced.BoundingSphereRadius,
                    CollisionInfo = instanced.CollisionInfo,
                    //index of mesh just built
                    MeshIndex = (short)(builder.Meshes.Count - 1),
                });
            }

            //instanced geometry instances
            newSbsp.InstancedGeometryInstances = new List<InstancedGeometryInstance>();
            foreach (var instanced in gen2Tag.InstancedGeometryInstances)
            {
                //TODO: BSP Physics, Matrix
                newSbsp.InstancedGeometryInstances.Add(new InstancedGeometryInstance
                {
                    Scale = instanced.Scale,
                    Matrix = new RealMatrix4x3 { },
                    DefinitionIndex = instanced.InstanceDefinition,
                    Flags = (InstancedGeometryInstance.FlagsValue)instanced.Flags,
                    Name = instanced.Name,
                    WorldBoundingSphereCenter = instanced.WorldBoundingSphereCenter,
                    BoundingSphereRadiusBounds = new Bounds<float>(0, instanced.BoundingSphereRadius),
                    PathfindingPolicy = (Scenery.PathfindingPolicyValue)instanced.PathfindingPolicy,
                    LightmappingPolicy = (InstancedGeometryInstance.InstancedGeometryLightmappingPolicy)instanced.LightmappingPolicy

                });
            }

            //close out render geo resource and prepare to load it in to the lightmap
            builder.EndPermutation();
            builder.EndRegion();
            RenderModel result = builder.Build(Cache.Serializer);

            return newSbsp;
        }
    }
}
