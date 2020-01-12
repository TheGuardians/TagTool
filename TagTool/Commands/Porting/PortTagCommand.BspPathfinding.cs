using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using static TagTool.Tags.Definitions.ScenarioStructureBsp.TagPathfindingDatum.PathfindingHint.HintTypeValue;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private TagResourceReference ConvertStructureBspCacheFileTagResources(ScenarioStructureBsp bsp)
        {
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
                resourceDefinition = new StructureBspCacheFileTagResourcesTest()
                {
                    SurfacePlanes = new TagBlock<ScenarioStructureBsp.SurfacesPlanes>(CacheAddressType.Data, bsp.SurfacePlanes),
                    Planes = new TagBlock<ScenarioStructureBsp.Plane>(CacheAddressType.Data, bsp.Planes),
                    EdgeToSeams = new TagBlock<ScenarioStructureBsp.EdgeToSeamMapping>(CacheAddressType.Data, bsp.EdgeToSeams),
                    PathfindingData = new TagBlock<StructureBspCacheFileTagResourcesTest.PathfindingDatum>(CacheAddressType.Data)
                };
                foreach(var pathfinding in bsp.PathfindingData)
                {
                    resourceDefinition.PathfindingData.Add(pathfinding.CreateResourcePathfinding());
                }
                // convert hints
                foreach(var pathfindingDatum in resourceDefinition.PathfindingData)
                {
                    foreach(var hint in pathfindingDatum.PathfindingHints)
                    {
                        if (hint.HintType == JumpLink || hint.HintType == WallJumpLink)
                        {
                            hint.Data[3] = (hint.Data[3] & ~ushort.MaxValue) | ((hint.Data[2] >> 16) & ushort.MaxValue);
                            hint.Data[2] = (hint.Data[2] & ~(ushort.MaxValue << 16)); //remove old landing sector
                            hint.Data[2] = (hint.Data[2] | ((hint.Data[2] & (byte.MaxValue << 8)) << 8)); //move jump height flags
                            hint.Data[2] = (hint.Data[2] & ~(byte.MaxValue << 8)); //remove old flags
                        }
                    }
                }
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
    }
}