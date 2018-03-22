using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

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

            var resourceContext = new ResourceSerializationContext(sbsp.Geometry.Resource);
            var definition = CacheContext.Deserializer.Deserialize<RenderGeometryApiResourceDefinition>(resourceContext);

            using (var edResourceStream = new MemoryStream())
            using (var edResourceReader = new EndianReader(edResourceStream, EndianFormat.LittleEndian))
            {
                CacheContext.ExtractResource(sbsp.Geometry.Resource, edResourceStream);
                var inVertexStream = VertexStreamFactory.Create(CacheVersion.HaloOnline106708, edResourceStream);

                foreach (var cluster in sbsp.Clusters)
                {
                    cluster.Bsp = instance;
                    foreach (var grid in cluster.DecoratorGrids)
                    {
                        grid.DecoratorGeometryIndex = grid.DecoratorVariant_H3;
                        grid.DecoratorIndex_HO = grid.DecoratorIndex_H3;
                        

                        var vertexBuffer = definition.VertexBuffers[grid.DecoratorGeometryIndex].Definition;

                        edResourceStream.Position = vertexBuffer.Data.Address.Offset;

                        var tiny = inVertexStream.ReadTinyPositionVertex();

                        //undo conversion

                        var floatvalue = tiny.Position.W;

                        if ((floatvalue - 1.0f / 32767.0f )> 0 && floatvalue <=1)
                            floatvalue -= 1.0f / 32767.0f;

                        floatvalue = (floatvalue / 2.0f) + 0.5f;

                        ushort result = (ushort)(floatvalue * ushort.MaxValue);

                        var variant = ((result >> 8) & 0xFF);

                        grid.DecoratorVariant_HO = (short)variant;

                    }
                }
            }

            

            //
            // Temporary Fixes:
            //

            // Disable decorator geometry for now entirely

            /*for (var i = 0; i < sbsp.Decorators.Count; i++)
                sbsp.Decorators[i] = new TagReferenceBlock { Instance = CacheContext.TagCache.Index[0x2ECD] };*/
            
            for (int i = 0; i < sbsp.Clusters.Count; i++)
            {
                //sbsp.Clusters[i].DecoratorGrids = new List<ScenarioStructureBsp.Cluster.DecoratorGrid>();
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