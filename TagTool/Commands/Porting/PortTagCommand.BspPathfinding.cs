using TagTool.Cache;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Pathfinding;
using TagTool.Pathfinding.Utils;
using TagTool.Common;
using TagTool.Commands.Common;
using System.Linq;
using System.Collections.Generic;
using System;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private TagResourceReference ConvertStructureBspCacheFileTagResources(ScenarioStructureBsp bsp, StructureBspTagResources bspResources, CachedTag instance)
        {
            if(BlamCache.Platform == CachePlatform.MCC)
                return ConvertStructureBspCacheFileTagResourcesMCC(bsp);

            //
            // Set up ElDorado resource reference
            //

            if (BlamCache.Version < CacheVersion.Halo3ODST)
                bsp.PathfindingResource = new TagResourceReference();

            //
            // Load Blam resource data
            //

            var resourceDefinition = BlamCache.Version > CacheVersion.Halo3Retail ? BlamCache.ResourceCache.GetStructureBspCacheFileTagResources(bsp.PathfindingResource) : null;

            if (resourceDefinition == null && BlamCache.Version >= CacheVersion.Halo3ODST)
                return bsp.PathfindingResource;

            //
            // Port Blam resource definition
            //

            if (BlamCache.Version < CacheVersion.Halo3ODST)
            {
                resourceDefinition = new StructureBspCacheFileTagResources()
                {
                    SurfacePlanes = new TagBlock<StructureSurface>(CacheAddressType.Data, bsp.StructureSurfaces),
                    Planes = new TagBlock<StructureSurfaceToTriangleMapping>(CacheAddressType.Data, bsp.StructureSurfaceToTriangleMapping),
                    EdgeToSeams = new TagBlock<EdgeToSeamMapping>(CacheAddressType.Data, bsp.EdgeToSeamEdge),
                    PathfindingData = new TagBlock<ResourcePathfinding>(CacheAddressType.Definition)
                };
                foreach(var pathfinding in bsp.PathfindingData)
                {
                    resourceDefinition.PathfindingData.Add(PathfindingConverter.CreateResourcePathfindingFromTag(pathfinding));
                }
                // convert hints
                foreach(var pathfindingDatum in resourceDefinition.PathfindingData)
                {
                    foreach(var hint in pathfindingDatum.PathfindingHints)
                    {
                        if (hint.HintType == PathfindingHint.HintTypeValue.JumpLink || hint.HintType == PathfindingHint.HintTypeValue.WallJumpLink)
                        {
                            hint.Data[3] = (hint.Data[3] & ~ushort.MaxValue) | ((hint.Data[2] >> 16) & ushort.MaxValue);
                            hint.Data[2] = (hint.Data[2] & ~(ushort.MaxValue << 16)); //remove old landing sector
                            hint.Data[2] = (hint.Data[2] | ((hint.Data[2] & (byte.MaxValue << 8)) << 8)); //move jump height flags
                            hint.Data[2] = (hint.Data[2] & ~(byte.MaxValue << 8)); //remove old flags
                        }
                    }
                }
                // fix surface planes
                foreach(var surfacePlane in resourceDefinition.SurfacePlanes)
                {
                    surfacePlane.SurfaceToTriangleMappingCount = surfacePlane.SurfaceToTriangleMappingCountOld;
                    surfacePlane.FirstSurfaceToTriangleMappingIndex = surfacePlane.FirstSurfaceToTriangleMappingIndexOld;
                }
            }

            //
            // Convert reach instanced geometry instances
            //

            if(BlamCache.Version >= CacheVersion.HaloReach)
            {
                bsp.InstancedGeometryInstances = new List<InstancedGeometryInstance>();
                bsp.InstancedGeometryInstances.AddRange(resourceDefinition.InstancedGeometryInstances);

                // convert instances
                foreach(var instancedgeo in bsp.InstancedGeometryInstances)
                {
                    var definition = bspResources.InstancedGeometry[instancedgeo.DefinitionIndex];

                    instancedgeo.Flags = instancedgeo.FlagsReach.ConvertLexical<InstancedGeometryInstance.InstancedGeometryFlags>();

                    if (definition.Flags.HasFlag(InstancedGeometryBlock.InstancedGeometryDefinitionFlags.MiscoloredBsp))
                        instancedgeo.Flags |= InstancedGeometryInstance.InstancedGeometryFlags.MiscoloredBsp;
                    if (definition.Flags.HasFlag(InstancedGeometryBlock.InstancedGeometryDefinitionFlags.NoPhysics))
                        instancedgeo.Flags |= InstancedGeometryInstance.InstancedGeometryFlags.NoPhysics;

                    if (instancedgeo.SeamBitVector.Skip(1).Any(x => x != 0))
                        new TagToolWarning("Instanced seam bit vector truncated!");

                    instancedgeo.SeamBitVector = new uint[] { instancedgeo.SeamBitVector[0] };
              
                    instancedgeo.PathfindingPolicy = instancedgeo.LightmappingPolicyReach.ConvertLexical<Scenery.PathfindingPolicyValue>();

                    instancedgeo.LightmappingPolicy = instancedgeo.LightmappingPolicyReach.ConvertLexical<InstancedGeometryInstance.InstancedGeometryLightmappingPolicy>();
                    if (instancedgeo.LightmappingPolicy == InstancedGeometryInstance.InstancedGeometryLightmappingPolicy.SingleProbe)
                        instancedgeo.LightmappingPolicy = InstancedGeometryInstance.InstancedGeometryLightmappingPolicy.PerPixelShared;

                    if (instancedgeo.BspPhysicsReach.Count > 0)
                    {
                        instancedgeo.BspPhysics = new List<CollisionBspPhysicsDefinition>()
                        {
                            ConvertCollisionBspPhysicsReach(instancedgeo.BspPhysicsReach[0])
                        };
                    }
                }

                //convert cluster instanced geometry physics
                for(var i = 0; i < bsp.Clusters.Count; i++)
                {
                    if(bsp.Clusters[i].InstanceImposterClusterMoppIndex != -1)
                    {
                        bsp.Clusters[i].InstancedGeometryPhysics = new ScenarioStructureBsp.Cluster.InstancedGeometryPhysicsData
                        {
                            Type = 2,
                            StructureBsp = instance,
                            Shape = new Havok.HkpMoppBvTreeShape(),
                            ClusterIndex = i,
                            MoppCodes = new System.Collections.Generic.List<Havok.TagHkpMoppCode>
                            {
                                resourceDefinition.ClusterMoppCode[bsp.Clusters[i].InstanceImposterClusterMoppIndex]
                            }
                        };
                    }
                }
            }

            bsp.PathfindingResource = CacheContext.ResourceCache.CreateStructureBspCacheFileResource(resourceDefinition);

            if (BlamCache.Version < CacheVersion.Halo3ODST)
            {
                bsp.StructureSurfaces.Clear();
                bsp.StructureSurfaceToTriangleMapping.Clear();
                bsp.EdgeToSeamEdge.Clear();
                bsp.PathfindingData.Clear();
            }

            return bsp.PathfindingResource;
        }

        private TagResourceReference ConvertStructureBspCacheFileTagResourcesMCC(ScenarioStructureBsp bsp)
        {
            var resourceDefinition = BlamCache.ResourceCache.GetStructureBspCacheFileTagResources(bsp.PathfindingResource);
            bsp.PathfindingResource = CacheContext.ResourceCache.CreateStructureBspCacheFileResource(resourceDefinition);
            return bsp.PathfindingResource;
        }
    }
}