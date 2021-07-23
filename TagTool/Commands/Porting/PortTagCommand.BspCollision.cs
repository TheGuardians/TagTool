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
                        mopps.Data.Elements = HavokConverter.ConvertMoppCodes(BlamCache.Version, BlamCache.Platform, CacheContext.Version, CacheContext.Platform, mopps.Data.Elements);
                    }
                }
            }

            bsp.CollisionBspResource = CacheContext.ResourceCache.CreateStructureBspResource(resourceDefinition);

            return bsp.CollisionBspResource;
        }
    }
}
