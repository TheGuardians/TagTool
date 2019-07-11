using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Commands.ScenarioStructureBSPs
{
    class PathfindingTestCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CachedTagInstance Instance;
        private ScenarioStructureBsp Definition { get; }

        public PathfindingTestCommand(HaloOnlineCacheContext cacheContext, CachedTagInstance instance, ScenarioStructureBsp definition) :
            base(true,
                
                "PathfindingTest",
                "A debug-only pathfinding test command.",
                
                "PathfindingTest",

                "A debug-only pathfinding test command.")
        {
            CacheContext = cacheContext;
            Instance = instance;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;

            var resourceDefinition = CacheContext.Deserialize<StructureBspCacheFileTagResources>(Definition.PathfindingResource);

            using (var resourceStream = new MemoryStream())
            using (var resourceReader = new EndianReader(resourceStream, EndianFormat.LittleEndian))
            {
                CacheContext.ExtractResource(Definition.PathfindingResource, resourceStream);

                var resourceDataContext = new DataSerializationContext(resourceReader);

                resourceStream.Position = resourceDefinition.SurfacePlanes.Address.Offset;

                for (var i = 0; i < resourceDefinition.SurfacePlanes.Count; i++)
                    resourceDefinition.SurfacePlanes.Add(
                        CacheContext.Deserialize<ScenarioStructureBsp.SurfacePlane>(
                            resourceDataContext));

                resourceStream.Position = resourceDefinition.Planes.Address.Offset;

                for (var i = 0; i < resourceDefinition.Planes.Count; i++)
                    resourceDefinition.Planes.Add(
                        CacheContext.Deserialize<ScenarioStructureBsp.Plane>(
                            resourceDataContext));

                resourceStream.Position = resourceDefinition.EdgeToSeamMappings.Address.Offset;

                for (var i = 0; i < resourceDefinition.EdgeToSeamMappings.Count; i++)
                    resourceDefinition.EdgeToSeamMappings.Add(
                        CacheContext.Deserialize<ScenarioStructureBsp.EdgeToSeamMapping>(
                            resourceDataContext));

                foreach (var pathfindingData in resourceDefinition.PathfindingData)
                {
                    resourceStream.Position = pathfindingData.Sectors.Address.Offset;

                    for (var i = 0; i < pathfindingData.Sectors.Count; i++)
                        pathfindingData.Sectors.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Sector>(
                                resourceDataContext));

                    resourceStream.Position = pathfindingData.Links.Address.Offset;

                    for (var i = 0; i < pathfindingData.Links.Count; i++)
                        pathfindingData.Links.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Link>(
                                resourceDataContext));

                    resourceStream.Position = pathfindingData.References.Address.Offset;

                    for (var i = 0; i < pathfindingData.References.Count; i++)
                        pathfindingData.References.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Reference>(
                                resourceDataContext));

                    resourceStream.Position = pathfindingData.Bsp2dNodes.Address.Offset;

                    for (var i = 0; i < pathfindingData.Bsp2dNodes.Count; i++)
                        pathfindingData.Bsp2dNodes.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Bsp2dNode>(
                                resourceDataContext));

                    resourceStream.Position = pathfindingData.Vertices.Address.Offset;

                    for (var i = 0; i < pathfindingData.Vertices.Count; i++)
                        pathfindingData.Vertices.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Vertex>(
                                resourceDataContext));

                    foreach (var objectReference in pathfindingData.ObjectReferences)
                    {
                        foreach (var bspReference in objectReference.Bsps)
                        {
                            resourceStream.Position = bspReference.Bsp2dRefs.Address.Offset;

                            for (var i = 0; i < bspReference.Bsp2dRefs.Count; i++)
                                bspReference.Bsp2dRefs.Add(
                                    CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.ObjectReference.BspReference.Bsp2dRef>(
                                        resourceDataContext));
                        }
                    }

                    resourceStream.Position = pathfindingData.PathfindingHints.Address.Offset;

                    for (var i = 0; i < pathfindingData.PathfindingHints.Count; i++)
                        pathfindingData.PathfindingHints.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.PathfindingHint>(
                                resourceDataContext));

                    resourceStream.Position = pathfindingData.InstancedGeometryReferences.Address.Offset;

                    for (var i = 0; i < pathfindingData.InstancedGeometryReferences.Count; i++)
                        pathfindingData.InstancedGeometryReferences.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.InstancedGeometryReference>(
                                resourceDataContext));

                    resourceStream.Position = pathfindingData.GiantPathfinding.Address.Offset;

                    for (var i = 0; i < pathfindingData.GiantPathfinding.Count; i++)
                        pathfindingData.GiantPathfinding.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.GiantPathfindingBlock>(
                                resourceDataContext));

                    foreach (var seam in pathfindingData.Seams)
                    {
                        resourceStream.Position = seam.LinkIndices.Address.Offset;

                        for (var i = 0; i < seam.LinkIndices.Count; i++)
                            seam.LinkIndices.Add(
                                CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Seam.LinkIndexBlock>(
                                    resourceDataContext));
                    }

                    foreach (var jumpSeam in pathfindingData.JumpSeams)
                    {
                        resourceStream.Position = jumpSeam.JumpIndices.Address.Offset;

                        for (var i = 0; i < jumpSeam.JumpIndices.Count; i++)
                            jumpSeam.JumpIndices.Add(
                                CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.JumpSeam.JumpIndexBlock>(
                                    resourceDataContext));
                    }

                    resourceStream.Position = pathfindingData.Doors.Address.Offset;

                    for (var i = 0; i < pathfindingData.Doors.Count; i++)
                        pathfindingData.Doors.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Door>(
                                resourceDataContext));
                }
            }

            return true;
        }
    }
}
