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

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private TagResourceReference ConvertStructureBspCacheFileTagResources(ScenarioStructureBsp bsp, CachedTag instance)
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
                    SurfacePlanes = new TagBlock<SurfacesPlanes>(CacheAddressType.Data, bsp.SurfacePlanes),
                    Planes = new TagBlock<PlaneReference>(CacheAddressType.Data, bsp.Planes),
                    EdgeToSeams = new TagBlock<EdgeToSeamMapping>(CacheAddressType.Data, bsp.EdgeToSeams),
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
                    surfacePlane.PlaneCountNew = surfacePlane.PlaneCountOld;
                    surfacePlane.PlaneIndexNew = surfacePlane.PlaneIndexOld;
                }
            }

            //
            // Convert reach instanced geometry instances
            //

            if(BlamCache.Version == CacheVersion.HaloReach)
            {
                bsp.InstancedGeometryInstances = new System.Collections.Generic.List<InstancedGeometryInstance>();
                bsp.InstancedGeometryInstances.AddRange(resourceDefinition.InstancedGeometryInstances);
                foreach(var instancedgeo in bsp.InstancedGeometryInstances)
                {
                    if(instancedgeo.SeamBitVector.Skip(1).Any(x => x != 0))
                        new TagToolWarning("Instanced seam bit vector truncated!");

                    instancedgeo.SeamBitVector = new uint[] { instancedgeo.SeamBitVector[0] };
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
                
                // Convert surface references
                foreach(var surfaceReference in resourceDefinition.Planes)
                    ConvertReachSurfaceReference(surfaceReference);
            }

            bsp.PathfindingResource = CacheContext.ResourceCache.CreateStructureBspCacheFileResource(resourceDefinition);

            if (BlamCache.Version < CacheVersion.Halo3ODST)
            {
                bsp.SurfacePlanes.Clear();
                bsp.Planes.Clear();
                bsp.EdgeToSeams.Clear();
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