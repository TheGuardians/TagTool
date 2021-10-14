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
                        surfacePlane.PlaneCountNew = surfacePlane.PlaneCountOld;
                        surfacePlane.PlaneIndexNew = surfacePlane.PlaneIndexOld;
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

                for (int i = 0; i < resourceDefinition.CollisionBsps.Count; i++)
                    resourceDefinition.CollisionBsps[i] = ConvertCollisionBsp(resourceDefinition.CollisionBsps[i]);

                for (int i = 0; i < resourceDefinition.LargeCollisionBsps.Count; i++)
                    resourceDefinition.LargeCollisionBsps[i] = ConvertLargeCollisionBsp(resourceDefinition.LargeCollisionBsps[i]);

                foreach (var instancedGeometry in resourceDefinition.InstancedGeometry)
                {
                    instancedGeometry.CollisionInfo = ConvertCollisionBsp(instancedGeometry.CollisionInfo);
                    if (instancedGeometry.CollisionGeometries != null || instancedGeometry.CollisionGeometries.Count == 0)
                    {
                        for (int i = 0; i < instancedGeometry.CollisionGeometries.Count; i++)
                            instancedGeometry.CollisionGeometries[i] = ConvertCollisionBsp(instancedGeometry.CollisionGeometries[i]);
                    }

                    if(instancedGeometry.CollisionMoppCodes == null || instancedGeometry.CollisionMoppCodes.Count == 0)
                    {
                        if (instancedGeometry.CollisionInfo.Surfaces.Count > 0 && instancedGeometry.Polyhedra.Count > 0)
                        {
                            var moppCode = HavokMoppGenerator.GenerateMoppCode(instancedGeometry.CollisionInfo);
                            moppCode.Data.AddressType = CacheAddressType.Data;
                            instancedGeometry.CollisionMoppCodes = new TagBlock<TagHkpMoppCode>(CacheAddressType.Definition);
                            instancedGeometry.CollisionMoppCodes.Add(moppCode);
                        }
                    }
                }

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

                largebsp = supernodeconverter.Convert(largebsp);

                //if(!largebuilder.generate_bsp(ref largebsp, false) || !resizer.collision_bsp_check_counts(largebsp))
                //    new TagToolError(CommandError.CustomError, "Failed to generate collision bsp!");

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
                bsp = supernodeconverter.Convert(bsp);

                //if (!new LargeCollisionBSPBuilder().generate_bsp(ref bsp, true))
                //    new TagToolError(CommandError.CustomError, "Failed to generate large collision bsp!");
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
