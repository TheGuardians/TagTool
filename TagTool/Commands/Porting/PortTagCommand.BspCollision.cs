using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.Havok;
using TagTool.IO;
using TagTool.Tags;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Commands.CollisionModels;
using TagTool.Commands.Common;
using TagTool.Geometry.BspCollisionGeometry.Utils;
using System.Threading.Tasks;
using System.Threading;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private TagResourceReference ConvertStructureBspTagResources(ScenarioStructureBsp bsp)
        {
            StructureBspTagResources resourceDefinition = BlamCache.ResourceCache.GetStructureBspTagResources(bsp.CollisionBspResource);

            if (resourceDefinition == null)
                return null;

            if (BlamCache.Version < CacheVersion.Halo3ODST)
            {
                // convert surface planes
                foreach(var instance in resourceDefinition.InstancedGeometry)
                {
                    foreach(var surfacePlane in instance.SurfacePlanes)
                    {
                        surfacePlane.SurfaceToTriangleMappingCount = surfacePlane.SurfaceToTriangleMappingCountOld;
                        surfacePlane.FirstSurfaceToTriangleMappingIndex = surfacePlane.FirstSurfaceToTriangleMappingIndexOld;
                    }

                    foreach(var mopps in instance.CollisionMoppCodes)
                    {
                        mopps.Data.Elements = HavokConverter.ConvertMoppCodes(BlamCache.Version, BlamCache.Platform, CacheContext.Version, mopps.Data.Elements);
                    }
                }
            }

            if (BlamCache.Version >= CacheVersion.HaloReach)
            {
                //
                // Convert collision geometry
                //

                Console.WriteLine($"Converting cluster collision...");

                if (PortingOptions.Current.CollisionLeafMapping)
                {
                    if (resourceDefinition.LargeCollisionBsps == null)
                        resourceDefinition.LargeCollisionBsps = new TagBlock<LargeCollisionBspBlock>();
                    for (int i = 0; i < resourceDefinition.CollisionBsps.Count; i++)
                    {
                        ResizeCollisionBSP resizer = new ResizeCollisionBSP();
                        LargeCollisionBspBlock largeblock = resizer.GrowCollisionBsp(resourceDefinition.CollisionBsps[i]);
                        resourceDefinition.LargeCollisionBsps.Add(largeblock);
                        resourceDefinition.LargeCollisionBsps.AddressType = CacheAddressType.Definition;
                    }
                    resourceDefinition.CollisionBsps.Clear();
                }

                for (int i = 0; i < resourceDefinition.CollisionBsps.Count; i++)
                    resourceDefinition.CollisionBsps[i] = ConvertCollisionBsp(resourceDefinition.CollisionBsps[i]);

                for (int i = 0; i < resourceDefinition.LargeCollisionBsps.Count; i++)
                    resourceDefinition.LargeCollisionBsps[i] = ConvertLargeCollisionBsp(resourceDefinition.LargeCollisionBsps[i]);

                int atomicNumProcessed = 0;
                Parallel.For(0, resourceDefinition.InstancedGeometry.Count, i =>
                {
                    int numProcessed = Interlocked.Increment(ref atomicNumProcessed);
                    float percentComplete = numProcessed * 100.0f / resourceDefinition.InstancedGeometry.Count;
                    Console.WriteLine($"Converting instanced geometry collision [{i}] {numProcessed}/{resourceDefinition.InstancedGeometry.Count} ({percentComplete:0.0}%)...");

                    // be careful what you do here, it must be thread safe!

                    var instancedGeometry = resourceDefinition.InstancedGeometry[i];
                    var convertedCollisionBsp = ConvertCollisionBsp(instancedGeometry.CollisionInfo);

                    if (instancedGeometry.RenderBsp != null || instancedGeometry.RenderBsp.Count == 0)
                    {
                        for (int j = 0; j < instancedGeometry.RenderBsp.Count; j++)
                            instancedGeometry.RenderBsp[j] = ConvertCollisionBsp(instancedGeometry.RenderBsp[j]);
                    }

                    if (instancedGeometry.CollisionMoppCodes == null || instancedGeometry.CollisionMoppCodes.Count == 0)
                    {
                        if (instancedGeometry.CollisionInfo.Surfaces.Count > 0 && instancedGeometry.Polyhedra.Count > 0)
                        {
                            var moppCode = HavokMoppGenerator.GenerateMoppCode(instancedGeometry.CollisionInfo);
                            if (moppCode == null)
                                new TagToolError(CommandError.OperationFailed, "Failed to generate mopp code!");

                            moppCode.Data.AddressType = CacheAddressType.Data;
                            instancedGeometry.CollisionMoppCodes = new TagBlock<TagHkpMoppCode>(CacheAddressType.Definition);
                            instancedGeometry.CollisionMoppCodes.Add(moppCode);
                        }
                    }

                    instancedGeometry.CollisionInfo = convertedCollisionBsp;
                });
            }

            bsp.CollisionBspResource = CacheContext.ResourceCache.CreateStructureBspResource(resourceDefinition);

            return bsp.CollisionBspResource;
        }

        private CollisionGeometry ConvertCollisionBsp(CollisionGeometry bsp)
        {
            if (bsp.Bsp3dSupernodes != null && bsp.Bsp3dSupernodes.Count > 0)
            {
                var largebuilder = new LargeCollisionBSPBuilder();
                var supernodeconverter = new SupernodeToNodeConverter();
                var resizer = new ResizeCollisionBSP();

                var largebsp = resizer.GrowCollisionBsp(bsp);

                if (PortingOptions.Current.CollisionLeafMapping)
                    largebuilder.useleafmap = true;

                if (PortingOptions.Current.ReachSuperNodeConversion)
                {
                    largebsp = supernodeconverter.Convert(largebsp);
                }
                else
                {
                    if (!largebuilder.generate_bsp(ref largebsp, false) || !resizer.collision_bsp_check_counts(largebsp))
                        new TagToolError(CommandError.CustomError, "Failed to generate collision bsp!");
                }

                bsp = resizer.ShrinkCollisionBsp(largebsp);
            }

            bsp.Bsp3dNodes.AddressType = CacheAddressType.Data;
            bsp.Planes.AddressType = CacheAddressType.Data;
            bsp.Leaves.AddressType = CacheAddressType.Data;
            bsp.Bsp2dReferences.AddressType = CacheAddressType.Data;
            bsp.Bsp2dNodes.AddressType = CacheAddressType.Data;
            bsp.Surfaces.AddressType = CacheAddressType.Data;
            bsp.Edges.AddressType = CacheAddressType.Data;
            bsp.Vertices.AddressType = CacheAddressType.Data;
            return bsp;
        }

        private LargeCollisionBspBlock ConvertLargeCollisionBsp(LargeCollisionBspBlock bsp)
        {
            if (bsp.Bsp3dSupernodes != null && bsp.Bsp3dSupernodes.Count > 0)
            {
                var supernodeconverter = new SupernodeToNodeConverter();
                var largebuilder = new LargeCollisionBSPBuilder();

                if (PortingOptions.Current.CollisionLeafMapping)
                    largebuilder.useleafmap = true;

                if (PortingOptions.Current.ReachSuperNodeConversion)
                {
                    bsp = supernodeconverter.Convert(bsp);
                }
                else
                {
                    if (!largebuilder.generate_bsp(ref bsp, false))
                        new TagToolError(CommandError.CustomError, "Failed to generate large collision bsp!");
                }
            }

            bsp.Bsp3dNodes.AddressType = CacheAddressType.Data;
            bsp.Planes.AddressType = CacheAddressType.Data;
            bsp.Leaves.AddressType = CacheAddressType.Data;
            bsp.Bsp2dReferences.AddressType = CacheAddressType.Data;
            bsp.Bsp2dNodes.AddressType = CacheAddressType.Data;
            bsp.Surfaces.AddressType = CacheAddressType.Data;
            bsp.Edges.AddressType = CacheAddressType.Data;
            bsp.Vertices.AddressType = CacheAddressType.Data;
            return bsp;
        }
    }
}
