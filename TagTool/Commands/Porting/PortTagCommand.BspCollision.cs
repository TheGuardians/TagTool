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
                    if (instancedGeometry.CollisionGeometries != null)
                    {
                        for (int i = 0; i < instancedGeometry.CollisionGeometries.Count; i++)
                            instancedGeometry.CollisionGeometries[i] = ConvertCollisionBsp(instancedGeometry.CollisionGeometries[i]);
                    }

                    if(instancedGeometry.CollisionMoppCodes == null)
                    {
                        var moppCode = HavokMoppGenerator.GenerateMoppCode(instancedGeometry.CollisionInfo);
                        moppCode.Data.AddressType = CacheAddressType.Data;
                        instancedGeometry.CollisionMoppCodes = new TagBlock<TagHkpMoppCode>(CacheAddressType.Data);
                        instancedGeometry.CollisionMoppCodes.Add(moppCode);
                    }
                }

                // convert instance sbsp surface references
                foreach (var instance in resourceDefinition.InstancedGeometry)
                    foreach (var surfaceReference in instance.Planes)
                        ConvertReachSurfaceReference(surfaceReference);
            }

            bsp.CollisionBspResource = CacheContext.ResourceCache.CreateStructureBspResource(resourceDefinition);

            return bsp.CollisionBspResource;
        }

        private static void ConvertReachSurfaceReference(PlaneReference surfaceReference)
        {
            // reach uses 12 bits for the cluster index and 20 bits for the strip index
            short clusterIndex = (short)(surfaceReference.PackedReference & 0xFFF);
            uint stripIndex = surfaceReference.PackedReference >> 12;
            if (stripIndex > ushort.MaxValue)
            {
                new TagToolWarning("sbsp surface reference strip index truncated!!!");
                stripIndex = stripIndex & 0xFFFF;
            }

            surfaceReference.ClusterIndex = clusterIndex;
            surfaceReference.StripIndex = (ushort)stripIndex;
        }

        private CollisionGeometry ConvertCollisionBsp(CollisionGeometry bsp)
        {
            if (bsp.Bsp3dSupernodes.Count > 0)
            {
                if (!new CollisionBSPBuilder().generate_bsp(ref bsp, true))
                    new TagToolError(CommandError.CustomError, "Failed to generate collision bsp!");
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
            if (bsp.Bsp3dSupernodes.Count > 0)
            {
                if (!new LargeCollisionBSPBuilder().generate_bsp(ref bsp, true))
                    new TagToolError(CommandError.CustomError, "Failed to generate large collision bsp!");
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
