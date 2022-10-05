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
        private TagResourceReference ConvertStructureBspTagResources(ScenarioStructureBsp bsp, out StructureBspTagResources outResourceDefinition)
        {
            StructureBspTagResources resourceDefinition = BlamCache.ResourceCache.GetStructureBspTagResources(bsp.CollisionBspResource);
            outResourceDefinition = resourceDefinition;

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

                    if (instancedGeometry.RenderBsp != null && instancedGeometry.RenderBsp.Count > 0)
                    {
                        for (int j = 0; j < instancedGeometry.RenderBsp.Count; j++)
                            instancedGeometry.RenderBsp[j] = ConvertCollisionBsp(instancedGeometry.RenderBsp[j]);
                    }

                    if (instancedGeometry.CollisionInfo.Surfaces.Count > 0)
                    {
                        var moppCode = HavokMoppGenerator.GenerateMoppCode(instancedGeometry.CollisionInfo);
                        if (moppCode == null)
                            new TagToolError(CommandError.OperationFailed, "Failed to generate mopp code!");

                        moppCode.Data.AddressType = CacheAddressType.Data;
                        instancedGeometry.CollisionMoppCodes = new TagBlock<TagHkpMoppCode>(CacheAddressType.Definition);
                        instancedGeometry.CollisionMoppCodes.Add(moppCode);
                    }
                    else if (instancedGeometry.Polyhedra.Count > 0)
                    {
                        new TagToolWarning($"Instanced geometry #{i} has physics but no collision bsp!");
                        var mopp = new List<byte> { 0 };
                        var moppCode = new TagHkpMoppCode()
                        {
                            ReferencedObject = new HkpReferencedObject { ReferenceCount = 128 },
                            Info = new CodeInfo(),
                            ArrayBase = new HkArrayBase { Size = (uint)mopp.Count, CapacityAndFlags = (uint)(mopp.Count | 0x80000000) },
                            Data = new TagBlock<byte>(CacheAddressType.Data, mopp)
                        };
                        instancedGeometry.CollisionMoppCodes = new TagBlock<TagHkpMoppCode>(CacheAddressType.Definition);
                        instancedGeometry.CollisionMoppCodes.Add(moppCode);
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
                var resizer = new ResizeCollisionBSP();
                var largebsp = resizer.GrowCollisionBsp(bsp);
                // var largebuilder = new LargeCollisionBSPBuilder();
                // var gen3builder = new CollisionBSPGen3Builder();

                //if (PortingOptions.Current.Gen1Collision)
                //{
                //    if (!largebuilder.generate_bsp(ref largebsp, false) || !resizer.collision_bsp_check_counts(largebsp))
                //        new TagToolError(CommandError.CustomError, "Failed to generate collision bsp!");
                //}
                //else
                //{
                //    gen3builder.Bsp = largebsp;
                //    if (!gen3builder.build_bsp() || !resizer.collision_bsp_check_counts(largebsp))
                //        new TagToolError(CommandError.CustomError, "Failed to generate collision bsp!");
                //}
                var superNodeConverter = new SupernodeToNodeConverter();
                largebsp = superNodeConverter.Convert(largebsp);
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

        private LargeCollisionBspBlock ConvertLargeCollisionBsp(LargeCollisionBspBlock largebsp)
        {
            if (largebsp.Bsp3dSupernodes != null && largebsp.Bsp3dSupernodes.Count > 0)
            {
                var superNodeConverter = new SupernodeToNodeConverter();
                largebsp = superNodeConverter.Convert(largebsp);
                //var largebuilder = new LargeCollisionBSPBuilder();
                //var gen3builder = new CollisionBSPGen3Builder();

                //if (PortingOptions.Current.Gen1Collision)
                //{
                //    if (!largebuilder.generate_bsp(ref largebsp, false))
                //        new TagToolError(CommandError.CustomError, "Failed to generate collision bsp!");
                //}
                //else
                //{
                //    gen3builder.Bsp = largebsp;
                //    if (!gen3builder.build_bsp())
                //        new TagToolError(CommandError.CustomError, "Failed to generate collision bsp!");
                //}
            }

            largebsp.Bsp3dNodes.AddressType = CacheAddressType.Data;
            largebsp.Planes.AddressType = CacheAddressType.Data;
            largebsp.Leaves.AddressType = CacheAddressType.Data;
            largebsp.Bsp2dReferences.AddressType = CacheAddressType.Data;
            largebsp.Bsp2dNodes.AddressType = CacheAddressType.Data;
            largebsp.Surfaces.AddressType = CacheAddressType.Data;
            largebsp.Edges.AddressType = CacheAddressType.Data;
            largebsp.Vertices.AddressType = CacheAddressType.Data;
            return largebsp;
        }
    }
}
