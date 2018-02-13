using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;
using System.Collections.Generic;
using System.Linq;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private ScenarioStructureBsp ConvertScenarioStructureBsp(ScenarioStructureBsp sbsp, CachedTagInstance instance)
        {
            sbsp.CollisionBspResource = ConvertStructureBspTagResources(sbsp);
            sbsp.PathfindingResource = ConvertStructureBspCacheFileTagResources(sbsp);

            sbsp.Unknown86 = 1;
            
            //
            // Fix cluster tag ref and decorator grids indices
            //

            foreach (var cluster in sbsp.Clusters)
            {
                cluster.Bsp = instance;
                foreach(var grid in cluster.DecoratorGrids)
                {
                    grid.DecoratorIndexScattering_HO = grid.DecoratorIndexScattering_H3;
                    grid.DecoratorIndex_HO = grid.DecoratorIndex_H3;
                }
            }

            //
            // Temporary Fixes:
            //

            // Disable decorator geometry for now entirely
            for (var i = 0; i < sbsp.Decorators.Count; i++)
                sbsp.Decorators[i] = new TagReferenceBlock { Instance = CacheContext.TagCache.Index[0x2ECD] };

            for (int i = 0; i < sbsp.Clusters.Count; i++)
            {
                sbsp.Clusters[i].DecoratorGrids = new List<ScenarioStructureBsp.Cluster.DecoratorGrid>();
                sbsp.Clusters[i].ObjectPlacements = new List<ScenarioStructureBsp.Cluster.ObjectPlacement>();
                sbsp.Clusters[i].Unknown25 = new List<ScenarioStructureBsp.Cluster.UnknownBlock2>();
            }
            
            //
            // Remove decals
            //

            sbsp.RuntimeDecals = new List<ScenarioStructureBsp.RuntimeDecal>();

            foreach (var Cluster in sbsp.Clusters)
            {
                Cluster.RuntimeDecalStartIndex = -1;
                Cluster.RuntimeDecalEntryCount = 0;
            }

            // These aren't removed properly, to verify
            sbsp.Geometry2.UnknownSections = new List<TagTool.Geometry.RenderGeometry.UnknownSection>();
            
            return sbsp;
        }

        private StructureDesign ConvertStructureDesign(StructureDesign sddt)
        {
            foreach(var mopp in sddt.WaterMoppCodes)
                mopp.Data.ForEach(i => i.ValueNew = (ushort)i.ValueOld);

            return sddt;
        }
    }
}

