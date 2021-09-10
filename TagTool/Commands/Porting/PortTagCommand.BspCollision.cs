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
                    resourceDefinition.CollisionBsps[i] = ConvertCollisionGeometry(resourceDefinition.CollisionBsps[i]);

                foreach (var instancedGeometry in resourceDefinition.InstancedGeometry)
                {
                    instancedGeometry.CollisionInfo = ConvertCollisionGeometry(instancedGeometry.CollisionInfo);
                    if (instancedGeometry.CollisionGeometries != null)
                    {
                        for (int i = 0; i < instancedGeometry.CollisionGeometries.Count; i++)
                            instancedGeometry.CollisionGeometries[i] = ConvertCollisionGeometry(instancedGeometry.CollisionGeometries[i]);
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

        private CollisionGeometry ConvertCollisionGeometry(CollisionGeometry geometry)
        {
            return geometry;

            if(geometry.Bsp3dSupernodes.Count > 0)
            {
                var coll = new CollisionModel();
                coll.Regions = new List<CollisionModel.Region>() {
                    new CollisionModel.Region() {
                        Permutations = new List<CollisionModel.Region.Permutation>() {
                            new CollisionModel.Region.Permutation() {
                                Bsps = new List<CollisionModel.Region.Permutation.Bsp>() {
                                    new CollisionModel.Region.Permutation.Bsp() { Geometry = geometry }
                                }
                            }
                        }
                    }
                };

                var generator = new GenerateCollisionBSPCommand(ref coll);
                generator.Execute(new List<string>());
            }

            return geometry;
        }
    }
}
