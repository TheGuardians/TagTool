using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private ScenarioStructureBsp ConvertScenarioStructureBsp(ScenarioStructureBsp sbsp, CachedTagInstance instance, Dictionary<ResourceLocation, Stream> resourceStreams)
        {
            sbsp.CollisionBspResource = ConvertStructureBspTagResources(sbsp, resourceStreams);
            sbsp.PathfindingResource = ConvertStructureBspCacheFileTagResources(sbsp, resourceStreams);

            sbsp.Unknown86 = 1;

            //
            // Fix cluster tag ref and decorator grids
            //

            var resource = sbsp.Geometry.Resource;

            if (resource != null && resource.Page.Index >= 0 && resource.GetLocation(out var location))
            {
                var resourceContext = new ResourceSerializationContext(sbsp.Geometry.Resource);
                var definition = CacheContext.Deserializer.Deserialize<RenderGeometryApiResourceDefinition>(resourceContext);

                using (var edResourceStream = new MemoryStream())
                using (var edResourceReader = new EndianReader(edResourceStream, EndianFormat.LittleEndian))
                {
                    var pageable = sbsp.Geometry.Resource;

                    if (pageable == null)
                        throw new ArgumentNullException("sbsp.Geometry.Resource");

                    if (!edResourceStream.CanWrite)
                        throw new ArgumentException("The output stream is not open for writing", "outStream");
                    
                    pageable.GetLocation(out var resourceLocation);

                    var cache = CacheContext.GetResourceCache(resourceLocation);

                    if (!resourceStreams.ContainsKey(resourceLocation))
                    {
                        resourceStreams[resourceLocation] = new MemoryStream();

                        using (var resourceStream = CacheContext.OpenResourceCacheRead(resourceLocation))
                            resourceStream.CopyTo(resourceStreams[resourceLocation]);
                    }

                    cache.Decompress(resourceStreams[resourceLocation], pageable.Page.Index, pageable.Page.CompressedBlockSize, edResourceStream);

                    var inVertexStream = VertexStreamFactory.Create(CacheVersion.HaloOnline106708, edResourceStream);

                    foreach (var cluster in sbsp.Clusters)
                    {
                        List<ScenarioStructureBsp.Cluster.DecoratorGrid> newDecoratorGrids = new List<ScenarioStructureBsp.Cluster.DecoratorGrid>();

                        foreach (var grid in cluster.DecoratorGrids)
                        {
                            grid.DecoratorGeometryIndex_HO = grid.DecoratorGeometryIndex_H3;
                            grid.DecoratorIndex_HO = grid.DecoratorIndex_H3;

                            if (grid.Amount == 0)
                                newDecoratorGrids.Add(grid);
                            else
                            {
                                List<TinyPositionVertex> vertices = new List<TinyPositionVertex>();

                                // Get the buffer the right grid
                                var vertexBuffer = definition.VertexBuffers[grid.DecoratorGeometryIndex_HO].Definition;
                                // Get the offset from the grid
                                edResourceStream.Position = vertexBuffer.Data.Address.Offset + grid.DecoratorGeometryOffset;
                                // Read all vertices and add to the list
                                for (int i = 0; i < grid.Amount; i++)
                                    vertices.Add(inVertexStream.ReadTinyPositionVertex());

                                // Get the new grids
                                List<ScenarioStructureBsp.Cluster.DecoratorGrid> newGrids = ConvertDecoratorGrid(vertices, grid);

                                // Add all to list
                                foreach (var newGrid in newGrids)
                                    newDecoratorGrids.Add(newGrid);

                            }
                        }

                        cluster.DecoratorGrids = newDecoratorGrids;
                    }
                }
            }

            //
            // Temporary Fixes:
            //

            // Without this 005_intro crash on cortana sbsp

            for (int i = 0; i < sbsp.Clusters.Count; i++)
            {
                sbsp.Clusters[i].ObjectPlacements = new List<ScenarioStructureBsp.Cluster.ObjectPlacement>();
                sbsp.Clusters[i].Unknown25 = new List<ScenarioStructureBsp.Cluster.UnknownBlock2>();
            }


            sbsp.Geometry2.UnknownSections = new List<RenderGeometry.UnknownSection>();
            
            return sbsp;
        }

        private StructureDesign ConvertStructureDesign(StructureDesign sddt)
        {
            foreach(var mopp in sddt.WaterMoppCodes)
                mopp.Data.ForEach(i => i.ValueNew = (ushort)i.ValueOld);

            return sddt;
        }

        List<ScenarioStructureBsp.Cluster.DecoratorGrid> ConvertDecoratorGrid(List<TinyPositionVertex> vertices, ScenarioStructureBsp.Cluster.DecoratorGrid grid)
        {
            List<ScenarioStructureBsp.Cluster.DecoratorGrid> decoratorGrids = new List<ScenarioStructureBsp.Cluster.DecoratorGrid>();

            List<DecoratorData> decoratorData = ParseVertices(vertices);

            foreach(var data in decoratorData)
            {
                ScenarioStructureBsp.Cluster.DecoratorGrid newGrid = grid.Copy();

                newGrid.Amount = data.Amount;
                newGrid.DecoratorGeometryOffset = grid.DecoratorGeometryOffset+data.GeometryOffset;
                newGrid.DecoratorVariant = data.Variant;

                //Position fixups should go here if needed

                decoratorGrids.Add(newGrid);
            }
            return decoratorGrids;
        }

        List<DecoratorData> ParseVertices(List<TinyPositionVertex> vertices)
        {
            List<DecoratorData> decoratorData = new List<DecoratorData>();
            var currentIndex = 0;
            while(currentIndex < vertices.Count)
            {
                var currentVertex = vertices[currentIndex];
                var currentVariant = currentVertex.Variant;

                DecoratorData data = new DecoratorData(0,(short)currentVariant,currentIndex*16);

                while(currentIndex < vertices.Count && currentVariant == currentVertex.Variant)
                {
                    currentVertex = vertices[currentIndex];
                    data.Amount++;
                    currentIndex++;
                }

                decoratorData.Add(data);
            }

            return decoratorData;
        }
    }

    class DecoratorData
    {
        public short Amount;
        public short Variant;
        public int GeometryOffset;

        //Add position data if needed

        public DecoratorData(short count, short variant, int offset)
        {
            Amount = count;
            Variant = variant;
            GeometryOffset = offset;
        }
    }
    
}