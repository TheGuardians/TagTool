using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Commands.ScenarioStructureBSPs
{
    class ExtractPathfindingGeometryCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private ScenarioStructureBsp Definition { get; }

        public ExtractPathfindingGeometryCommand(HaloOnlineCacheContext cacheContext, ScenarioStructureBsp definition) :
            base(true,

                "ExtractPathfindingGeometry",
                "",

                "ExtractPathfindingGeometry <OBJ File>",

                "")
        {
            CacheContext = cacheContext;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            if (Definition.CacheFileTagResources == null)
            {
                Console.WriteLine("ERROR: Pathfinding geometry does not have a resource associated with it.");
                return true;
            }

            var definition = CacheContext.Deserialize<StructureBspCacheFileTagResources>(Definition.CacheFileTagResources);

            using (var resourceStream = new MemoryStream())
            using (var resourceReader = new EndianReader(resourceStream))
            {
                CacheContext.ExtractResource(Definition.CacheFileTagResources, resourceStream);

                var dataContext = new DataSerializationContext(resourceReader);

                resourceStream.Position = definition.SurfacePlanes.Address.Offset;

                for (var i = 0; i < definition.SurfacePlanes.Count; i++)
                    definition.SurfacePlanes.Add(
                        CacheContext.Deserialize<ScenarioStructureBsp.SurfacePlane>(dataContext));

                resourceStream.Position = definition.Planes.Address.Offset;

                for (var i = 0; i < definition.Planes.Count; i++)
                    definition.Planes.Add(
                        CacheContext.Deserialize<ScenarioStructureBsp.Plane>(dataContext));

                resourceStream.Position = definition.EdgeToSeams.Address.Offset;

                for (var i = 0; i < definition.EdgeToSeams.Count; i++)
                    definition.EdgeToSeams.Add(
                        CacheContext.Deserialize<ScenarioStructureBsp.EdgeToSeamMapping>(dataContext));

                foreach (var pathfindingDatum in definition.PathfindingData)
                {
                    resourceStream.Position = pathfindingDatum.Sectors.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.Sectors.Count; i++)
                        pathfindingDatum.Sectors.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Sector>(dataContext));

                    resourceStream.Position = pathfindingDatum.Links.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.Links.Count; i++)
                        pathfindingDatum.Links.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Link>(dataContext));

                    resourceStream.Position = pathfindingDatum.References.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.References.Count; i++)
                        pathfindingDatum.References.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Reference>(dataContext));

                    resourceStream.Position = pathfindingDatum.Bsp2dNodes.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.Bsp2dNodes.Count; i++)
                        pathfindingDatum.Bsp2dNodes.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Bsp2dNode>(dataContext));

                    resourceStream.Position = pathfindingDatum.Vertices.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.Vertices.Count; i++)
                        pathfindingDatum.Vertices.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Vertex>(dataContext));

                    for (var objRefIdx = 0; objRefIdx < pathfindingDatum.ObjectReferences.Count; objRefIdx++)
                    {
                        for (var bspRefIdx = 0; bspRefIdx < pathfindingDatum.ObjectReferences[objRefIdx].Bsps.Count; bspRefIdx++)
                        {
                            var bspRef = pathfindingDatum.ObjectReferences[objRefIdx].Bsps[bspRefIdx];

                            resourceStream.Position = bspRef.Bsp2dRefs.Address.Offset;

                            for (var bsp2dRefIdx = 0; bsp2dRefIdx < bspRef.Bsp2dRefs.Count; bsp2dRefIdx++)
                                bspRef.Bsp2dRefs.Add(
                                    CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.ObjectReference.BspReference.Bsp2dRef>(dataContext));
                        }
                    }

                    resourceStream.Position = pathfindingDatum.PathfindingHints.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.PathfindingHints.Count; i++)
                        pathfindingDatum.PathfindingHints.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.PathfindingHint>(dataContext));

                    resourceStream.Position = pathfindingDatum.InstancedGeometryReferences.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.InstancedGeometryReferences.Count; i++)
                        pathfindingDatum.InstancedGeometryReferences.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.InstancedGeometryReference>(dataContext));

                    resourceStream.Position = pathfindingDatum.GiantPathfinding.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.GiantPathfinding.Count; i++)
                        pathfindingDatum.GiantPathfinding.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.GiantPathfindingBlock>(dataContext));

                    foreach (var seam in pathfindingDatum.Seams)
                    {
                        resourceStream.Position = seam.LinkIndices.Address.Offset;

                        for (var i = 0; i < seam.LinkIndices.Count; i++)
                            seam.LinkIndices.Add(
                                CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Seam.LinkIndexBlock>(dataContext));
                    }

                    foreach (var jumpSeam in pathfindingDatum.JumpSeams)
                    {
                        resourceStream.Position = jumpSeam.JumpIndices.Address.Offset;

                        for (var i = 0; i < jumpSeam.JumpIndices.Count; i++)
                            jumpSeam.JumpIndices.Add(
                                CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.JumpSeam.JumpIndexBlock>(dataContext));
                    }

                    resourceStream.Position = pathfindingDatum.Doors.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.Doors.Count; i++)
                        pathfindingDatum.Doors.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Door>(dataContext));
                }
            }

            using (var writer = File.CreateText(args[0]))
            {
                foreach (var pathfinding in definition.PathfindingData)
                {
                    foreach (ScenarioStructureBsp.PathfindingDatum.Vertex vertex in pathfinding.Vertices)
                    {
                        writer.WriteLine($"v {vertex.Position.X} {vertex.Position.Z} {vertex.Position.Y}");
                    }

                    writer.WriteLine("g sectors");

                    for (var i = 0; i < pathfinding.Sectors.Count; i++)
                    {
                        var sector = pathfinding.Sectors[i];
                        var vertices = new HashSet<short>();
                        var link = pathfinding.Links[sector.FirstLink];

                        writer.Write("f");

                        while (true)
                        {
                            if (link.LeftSector == i)
                            {
                                writer.Write($" {link.Vertex1 + 1}");

                                if (link.ForwardLink == sector.FirstLink)
                                    break;
                                else
                                    link = pathfinding.Links[link.ForwardLink];
                            }
                            else if (link.RightSector == i)
                            {
                                writer.Write($" {link.Vertex2 + 1}");

                                if (link.ReverseLink == sector.FirstLink)
                                    break;
                                else
                                    link = pathfinding.Links[link.ReverseLink];
                            }
                        }

                        writer.WriteLine();
                    }
                }
            }

            return true;
        }
    }
}