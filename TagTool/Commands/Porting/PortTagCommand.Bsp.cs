using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Geometry.Utils;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        public ScenarioStructureBsp ConvertScenarioStructureBsp(ScenarioStructureBsp sbsp, CachedTag instance, Dictionary<ResourceLocation, Stream> resourceStreams)
        {
            var converter = new RenderGeometryConverter(CacheContext, BlamCache);   // should be made static

            var blamDecoratorResourceDefinition = BlamCache.ResourceCache.GetRenderGeometryApiResourceDefinition(sbsp.DecoratorGeometry.Resource);
            var blamGeometryResourceDefinition = BlamCache.ResourceCache.GetRenderGeometryApiResourceDefinition(sbsp.Geometry.Resource);

            RenderGeometryApiResourceDefinition decoratorGeometry = null;
            if(blamDecoratorResourceDefinition != null)
                decoratorGeometry = converter.Convert(sbsp.DecoratorGeometry, blamDecoratorResourceDefinition);

            var geometry = converter.Convert(sbsp.Geometry, blamGeometryResourceDefinition);

            foreach (var cluster in sbsp.Clusters)
            {
                if (blamDecoratorResourceDefinition != null)
                {
                    List<ScenarioStructureBsp.Cluster.DecoratorGrid> newDecoratorGrids = new List<ScenarioStructureBsp.Cluster.DecoratorGrid>();

                    foreach (var grid in cluster.DecoratorGrids)
                    {
                        VertexBufferDefinition buffer = null;
                        if(CacheVersionDetection.IsInGen(CacheGeneration.HaloOnline, BlamCache.Version))
                            buffer = blamDecoratorResourceDefinition.VertexBuffers[grid.HaloOnlineInfo.VertexBufferIndex].Definition;
                        else
                            buffer = blamDecoratorResourceDefinition.VertexBuffers[grid.Gen3Info.VertexBufferIndex].Definition;
                        
                        var offset = grid.VertexBufferOffset;
                        grid.Vertices = new List<TinyPositionVertex>();
                        using (var stream = new MemoryStream(buffer.Data.Data))
                        {
                            var vertexStream = VertexStreamFactory.Create(BlamCache.Version, BlamCache.Platform, stream);
                            stream.Position = offset;

                            for (int i = 0; i < grid.Amount; i++)
                                grid.Vertices.Add(vertexStream.ReadTinyPositionVertex());
                        }

                        if (grid.Amount == 0)
                            newDecoratorGrids.Add(grid);
                        else
                        {
                            // Get the new grids
                            var newGrids = ConvertDecoratorGrid(grid.Vertices, grid);

                            // Add all to list
                            foreach (var newGrid in newGrids)
                                newDecoratorGrids.Add(newGrid);
                        }
                    }
                    cluster.DecoratorGrids = newDecoratorGrids;
                }
                
                if(CacheContext.Version == CacheVersion.HaloOnlineED && BlamCache.Version < CacheVersion.HaloReach)
                {
                    foreach (var cubemap in cluster.ClusterCubemaps)
                    {
                        cubemap.ReferencePoints = new List<ScenarioStructureBsp.Cluster.StructureClusterCubemap.CubemapReferencePointsBlock>()
                        {
                            new ScenarioStructureBsp.Cluster.StructureClusterCubemap.CubemapReferencePointsBlock() { ReferencePoint = cubemap.Position }
                        };
                    }
                }
            }

            // convert all the decorator vertex buffers
            // conversion already completed for halo online generation caches
            if (blamDecoratorResourceDefinition != null)
            {
                if (!CacheVersionDetection.IsInGen(CacheGeneration.HaloOnline, BlamCache.Version))
                {
                    for (var bufferindex = 0; bufferindex < blamDecoratorResourceDefinition.VertexBuffers.Count; bufferindex++)
                    {
                        var d3dBuffer = blamDecoratorResourceDefinition.VertexBuffers[bufferindex];
                        VertexBufferConverter.ConvertVertexBuffer(BlamCache.Version, BlamCache.Platform, CacheContext.Version, CacheContext.Platform, d3dBuffer.Definition);
                        decoratorGeometry.VertexBuffers.Add(d3dBuffer);
                    }
                }

                sbsp.DecoratorGeometry.Resource = CacheContext.ResourceCache.CreateRenderGeometryApiResource(decoratorGeometry);
            }
            sbsp.Geometry.Resource = CacheContext.ResourceCache.CreateRenderGeometryApiResource(geometry);

            sbsp.CollisionBspResource = ConvertStructureBspTagResources(sbsp, out StructureBspTagResources sbspTagResources);
            sbsp.PathfindingResource = ConvertStructureBspCacheFileTagResources(sbsp, sbspTagResources, instance);
            sbsp.UseResourceItems = 1;

            //
            // Set compatibility flag for H3 mopps for the engine to perform some fixups just in time
            //

            if (BlamCache.Platform == CachePlatform.Original && (BlamCache.Version == CacheVersion.Halo3Retail || BlamCache.Version == CacheVersion.Halo3Beta))
                sbsp.CompatibilityFlags |= ScenarioStructureBsp.StructureBspCompatibilityValue.UseMoppIndexPatch;

            if (BlamCache.Version >= CacheVersion.HaloReach)
                sbsp.CompatibilityFlags |= ScenarioStructureBsp.StructureBspCompatibilityValue.Reach;

            //
            // Temporary Fixes:
            //

            // Without this 005_intro crash on cortana sbsp       
            sbsp.Geometry.MeshClusterVisibility = new List<RenderGeometry.PerMeshMoppBlock>();

            if (BlamCache.Version >= CacheVersion.HaloReach)
            {
                // Temporary fix for collision - prior to sbsp version 3, instance buckets were used for collision
                sbsp.ImportVersion = 2;

                ConvertInstanceBucketsReach(sbsp, sbspTagResources);
                ConvertReachEnvironmentMopp(sbsp);

                // Reach invalid shader is an inconspicuous gray material and was left on many maps
                if (CacheContext.TagCache.TryGetCachedTag("levels\\shared\\shaders\\simple\\grey.shader", out CachedTag defaultGray))
                {
                    foreach (var material in sbsp.CollisionMaterials)
                    {
                        if (material.RenderMethod?.ToString() == "shaders\\invalid.shader")
                            material.RenderMethod = defaultGray;
                    }
                    foreach (var material in sbsp.Materials)
                    {
                        if (material.RenderMethod?.ToString() == "shaders\\invalid.shader")
                            material.RenderMethod = defaultGray;
                    }
                }
            }

            return sbsp;
        }

        void ConvertInstanceBucketsReach(ScenarioStructureBsp sbsp, StructureBspTagResources resources)
        {
            for (int i = 0; i < sbsp.Clusters.Count; i++)
            {
                var cluster = sbsp.Clusters[i];
                var clusterMesh = sbsp.Geometry.Meshes[cluster.MeshIndex];

                clusterMesh.InstanceBuckets = new List<Mesh.InstancedBucketBlock>();

                var instanceGroupSphere = sbsp.ClusterToInstanceGroupSpheres[i];
                for (int j = 0; j < instanceGroupSphere.InstanceGroupIndices.Count; j++)
                {
                    var instanceSphere = sbsp.InstanceGroupToInstanceSpheres[instanceGroupSphere.InstanceGroupIndices[j].Index];
                    for (int k = 0; k < instanceSphere.InstanceIndices.Count; k++)
                    {
                        int instanceIndex = instanceSphere.InstanceIndices[k].Index;
                        if (instanceIndex < 0 || instanceIndex >= sbsp.InstancedGeometryInstances.Count)
                            continue;

                        var instance = sbsp.InstancedGeometryInstances[instanceIndex];
                        var defintion = resources.InstancedGeometry[instance.DefinitionIndex];

                        var instanceBucket = clusterMesh.InstanceBuckets.FirstOrDefault(x => x.MeshIndex == defintion.MeshIndex && x.DefinitionIndex == instance.DefinitionIndex);
                        if (instanceBucket == null)
                        {
                            instanceBucket = new Mesh.InstancedBucketBlock();
                            instanceBucket.MeshIndex = defintion.MeshIndex;
                            instanceBucket.DefinitionIndex = instance.DefinitionIndex;
                            instanceBucket.Instances = new List<Mesh.InstancedBucketBlock.InstanceIndexBlock>();
                            clusterMesh.InstanceBuckets.Add(instanceBucket);
                        }

                        if(!instanceBucket.Instances.Any(x => x.InstanceIndex == instanceIndex))
                            instanceBucket.Instances.Add(new Mesh.InstancedBucketBlock.InstanceIndexBlock() { InstanceIndex = (short)instanceIndex });
                    }
                }
            }
        }

        public InstancedGeometryPhysics ConvertInstancedGeometryPhysicsReach(InstancedGeometryPhysicsReach bspPhysicsReach)
        {
            var bspPhysics = new InstancedGeometryPhysics();
            bspPhysics.MoppBvTreeShape = new Havok.CMoppBvTreeShape()
            {
                ReferencedObject = new Havok.HkpReferencedObject(),
                Type = 27,
                Scale = bspPhysicsReach.MoppBvTreeShape.MoppScale,
            };

            if (bspPhysicsReach.GeometryShape.Count > 0)
            {
                bspPhysics.GeometryShape = bspPhysicsReach.GeometryShape[0];
            }
            else if (bspPhysicsReach.PoopShape.Count > 0)
            {
                var poop = bspPhysicsReach.PoopShape[0];
                bspPhysics.PoopShape = new TagTool.Tags.TagBlock<DecomposedPoopShape>() { poop };
            }

            return bspPhysics;
        }

        public CollisionBspPhysicsDefinition ConvertCollisionBspPhysicsReach(CollisionBspPhysicsDefinition bspPhysics)
        {
            bspPhysics.MoppBvTreeShape = new Havok.CMoppBvTreeShape()
            {
                ReferencedObject = new Havok.HkpReferencedObject(),
                Type = 27,
                Scale = bspPhysics.MoppBvTreeShapeReach.MoppScale,
            };
            return bspPhysics;
        }

        List<ScenarioStructureBsp.Cluster.DecoratorGrid> ConvertDecoratorGrid(List<TinyPositionVertex> vertices, ScenarioStructureBsp.Cluster.DecoratorGrid grid)
        {
            List<ScenarioStructureBsp.Cluster.DecoratorGrid> decoratorGrids = new List<ScenarioStructureBsp.Cluster.DecoratorGrid>();

            List<DecoratorData> decoratorData = ParseVertices(vertices);

            foreach(var data in decoratorData)
            {
                var newGrid = grid.DeepClone();

                newGrid.HaloOnlineInfo = new ScenarioStructureBsp.Cluster.DecoratorGrid.HaloOnlineDecoratorInfo();
                newGrid.Amount = data.Amount;
                newGrid.VertexBufferOffset = grid.VertexBufferOffset + data.GeometryOffset;

                newGrid.HaloOnlineInfo.Variant = data.Variant;
                if (CacheVersionDetection.IsInGen(CacheGeneration.HaloOnline, BlamCache.Version))
                {
                    newGrid.HaloOnlineInfo.PaletteIndex = grid.HaloOnlineInfo.PaletteIndex;
                    newGrid.HaloOnlineInfo.VertexBufferIndex = grid.HaloOnlineInfo.VertexBufferIndex;
                    newGrid.HaloOnlineInfo.Variant = grid.HaloOnlineInfo.Variant;
                }
                else
                {
                    newGrid.HaloOnlineInfo.PaletteIndex = grid.Gen3Info.PaletteIndex;
                    newGrid.HaloOnlineInfo.VertexBufferIndex = grid.Gen3Info.VertexBufferIndex; // this doesn't change as each vertex buffer corresponds to the palette index
                }            

                if(BlamCache.Version >= CacheVersion.HaloReach)
                    newGrid.GridSize /= ushort.MaxValue;

                decoratorGrids.Add(newGrid);
            }
            return decoratorGrids;
        }

        List<DecoratorData> ParseVertices(List<TinyPositionVertex> vertices)
        {
            List<DecoratorData> decoratorData = new List<DecoratorData>();

            int currentIndex = 0;
            while (currentIndex < vertices.Count)
            {
                var currentVariant = (vertices[currentIndex].Variant >> 8) & 0xff;

                DecoratorData data = new DecoratorData(0, (short)currentVariant, currentIndex * 16);
                decoratorData.Add(data);
                while (currentIndex < vertices.Count && currentVariant == ((vertices[currentIndex].Variant >> 8) & 0xff))
                {
                    data.Amount++;
                    currentIndex++;
                }
            }
            return decoratorData;
        }

        void ConvertReachEnvironmentMopp(ScenarioStructureBsp sbsp)
        {
            if (sbsp.InstancedGeometryInstances == null)
                return;

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
                return key;
            }
        }
    }

    class DecoratorData
    {
        public short Amount;
        public short Variant;
        public int GeometryOffset;

        //Add position data if needed

        public DecoratorData(short count, short variant, int offset)
        {
            Amount = count;
            Variant = variant;
            GeometryOffset = offset;
        }
    }
}