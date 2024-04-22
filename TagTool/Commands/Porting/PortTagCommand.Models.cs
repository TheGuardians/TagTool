using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.Commands.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {       
        private object ConvertParticleModel(CachedTag edTag, CachedTag blamTag, ParticleModel particleModel)
        {
            var blamResourceDefinition = BlamCache.ResourceCache.GetRenderGeometryApiResourceDefinition(particleModel.Geometry.Resource);
            if (blamResourceDefinition == null)
            {
                // clear geometry, prevents crashes instead of nulling the tag itself or nulling the resource
                particleModel.Geometry = null;
                return particleModel;
            }
            var newParticleModelGeometry = GeometryConverter.Convert(particleModel.Geometry, blamResourceDefinition);
            particleModel.Geometry.Resource = CacheContext.ResourceCache.CreateRenderGeometryApiResource(newParticleModelGeometry);
            return particleModel;
        }

        private object ConvertGen3RenderModel(CachedTag edTag, CachedTag blamTag, RenderModel mode)
        {
            var blamResourceDefinition = BlamCache.ResourceCache.GetRenderGeometryApiResourceDefinition(mode.Geometry.Resource);
            if (blamResourceDefinition == null)
            {
                // clear geometry, prevents crashes instead of nulling the tag itself or nulling the resource
                mode.Geometry = null;
                return mode;
            }

            var newRenderModelGeometry = GeometryConverter.Convert(mode.Geometry, blamResourceDefinition);
            mode.Geometry.Resource = CacheContext.ResourceCache.CreateRenderGeometryApiResource(newRenderModelGeometry);

            switch (blamTag.Name)
            {
                case @"levels\multi\snowbound\sky\sky":
                    mode.Materials[11].RenderMethod = CacheContext.TagCache.GetTag<Shader>(@"levels\multi\snowbound\sky\shaders\dust_clouds");
                    break;
                case @"levels\multi\isolation\sky\sky":
                    mode.Geometry.Meshes[0].Flags = MeshFlags.UseRegionIndexForSorting;
                    break;
            }

            if (BlamCache.Version >= CacheVersion.HaloReach)
            {
                // Fixup foliage material
                foreach (var mesh in mode.Geometry.Meshes)
                {
                    foreach (var part in mesh.Parts)
                    {
                        if (part.MaterialIndex != -1 &&
                            mode.Materials[part.MaterialIndex].RenderMethod != null &&
                            mode.Materials[part.MaterialIndex].RenderMethod.Group.Tag == "rmfl")
                        {
                            part.FlagsNew |= Part.PartFlagsNew.PreventBackfaceCulling;
                        }
                    }
                }
            }

            return mode;
        }
    }
}