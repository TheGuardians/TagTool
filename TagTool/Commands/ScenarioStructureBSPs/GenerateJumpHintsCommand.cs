using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using static TagTool.Tags.Definitions.ScenarioStructureBsp.PathfindingDatum.PathfindingHint.HintTypeValue;

namespace TagTool.Commands.ScenarioStructureBSPs
{
    class GenerateJumpHintsCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private ScenarioStructureBsp Definition { get; }

        public GenerateJumpHintsCommand(HaloOnlineCacheContext cacheContext, ScenarioStructureBsp definition) :
            base(true,

                "GenerateJumpHints",
                "Generates jump hint data for pre-ODST bsp pathfinding data.",

                "GenerateJumpHints",

                "Generates jump hint data for pre-ODST bsp pathfinding data.")
        {
            CacheContext = cacheContext;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (Definition.PathfindingResource == null)
            {
                Console.WriteLine("ERROR: Pathfinding geometry does not have a resource associated with it.");
                return true;
            }

            var resourceContext = new ResourceSerializationContext(CacheContext, Definition.PathfindingResource);
            var resourceDefinition = CacheContext.Deserializer.Deserialize<StructureBspCacheFileTagResources>(resourceContext);

            using (var resourceStream = new MemoryStream())
            using (var reader = new EndianReader(resourceStream))
            using (var writer = new EndianWriter(resourceStream))
            {
                CacheContext.ExtractResource(Definition.PathfindingResource, resourceStream);
                var dataContext = new DataSerializationContext(reader, writer);

                foreach (var pathfindingDatum in resourceDefinition.PathfindingData)
                {
                    resourceStream.Position = pathfindingDatum.Sectors.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.Sectors.Count; i++)
                        pathfindingDatum.Sectors.Add(
                            CacheContext.Deserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.Sector>(dataContext));

                    resourceStream.Position = pathfindingDatum.Links.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.Links.Count; i++)
                        pathfindingDatum.Links.Add(
                            CacheContext.Deserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.Link>(dataContext));

                    resourceStream.Position = pathfindingDatum.References.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.References.Count; i++)
                        pathfindingDatum.References.Add(
                            CacheContext.Deserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.Reference>(dataContext));

                    resourceStream.Position = pathfindingDatum.Bsp2dNodes.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.Bsp2dNodes.Count; i++)
                        pathfindingDatum.Bsp2dNodes.Add(
                            CacheContext.Deserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.Bsp2dNode>(dataContext));

                    resourceStream.Position = pathfindingDatum.PathfindingHints.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.PathfindingHints.Count; i++)
                        pathfindingDatum.PathfindingHints.Add(
                            CacheContext.Deserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.PathfindingHint>(dataContext));

                    resourceStream.Position = pathfindingDatum.Vertices.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.Vertices.Count; i++)
                        pathfindingDatum.Vertices.Add(
                            CacheContext.Deserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.Vertex>(dataContext));

                    for (var objRefIdx = 0; objRefIdx < pathfindingDatum.ObjectReferences.Count; objRefIdx++)
                    {
                        for (var bspRefIdx = 0; bspRefIdx < pathfindingDatum.ObjectReferences[objRefIdx].Bsps.Count; bspRefIdx++)
                        {
                            var bspRef = pathfindingDatum.ObjectReferences[objRefIdx].Bsps[bspRefIdx];

                            resourceStream.Position = bspRef.Bsp2dRefs.Address.Offset;

                            for (var bsp2dRefIdx = 0; bsp2dRefIdx < bspRef.Bsp2dRefs.Count; bsp2dRefIdx++)
                                bspRef.Bsp2dRefs.Add(
                                    CacheContext.Deserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.ObjectReference.BspReference.Bsp2dRef>(dataContext));
                        }
                    }

                    resourceStream.Position = pathfindingDatum.InstancedGeometryReferences.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.InstancedGeometryReferences.Count; i++)
                        pathfindingDatum.InstancedGeometryReferences.Add(
                            CacheContext.Deserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.InstancedGeometryReference>(dataContext));

                    resourceStream.Position = pathfindingDatum.Doors.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.Doors.Count; i++)
                        pathfindingDatum.Doors.Add(
                            CacheContext.Deserializer.Deserialize<ScenarioStructureBsp.PathfindingDatum.Door>(dataContext));

                    for (var i = 0; i < pathfindingDatum.PathfindingHints.Count; i++)
                    {
                        var hint = pathfindingDatum.PathfindingHints[i];

                        if (hint.HintType != JumpLink || hint.HintType != WallJumpLink)
                            continue;

                        var hintverts = new List<short>();
                        var success = false;

                        hintverts.Add((short)(hint.Data[1] & ushort.MaxValue));
                        hintverts.Add((short)((hint.Data[1] >> 16) & ushort.MaxValue));

                        if (hintverts[0] == -1 || hintverts[1] == -1)
                            continue;

                        float hint_x = (pathfindingDatum.Vertices[hintverts[0]].Position.X + pathfindingDatum.Vertices[hintverts[1]].Position.X) / 2.0f;
                        float hint_y = (pathfindingDatum.Vertices[hintverts[0]].Position.Y + pathfindingDatum.Vertices[hintverts[1]].Position.Y) / 2.0f;
                        float hint_z = (pathfindingDatum.Vertices[hintverts[0]].Position.Z + pathfindingDatum.Vertices[hintverts[1]].Position.Z) / 2.0f;

                        var sectorlist = new List<int>();
                        var zavelist = new List<float>();

                        for (var s = 0; s < pathfindingDatum.Sectors.Count; s++)
                        {
                            var sector = pathfindingDatum.Sectors[s];
                            var vertices = new HashSet<short>();
                            var link = pathfindingDatum.Links[sector.FirstLink];

                            while (true)
                            {
                                if (link.LeftSector == s)
                                {
                                    vertices.Add(link.Vertex1);
                                    vertices.Add(link.Vertex2);
                                    if (link.ForwardLink == sector.FirstLink)
                                        break;
                                    else
                                        link = pathfindingDatum.Links[link.ForwardLink];
                                }
                                else if (link.RightSector == s)
                                {
                                    vertices.Add(link.Vertex1);
                                    vertices.Add(link.Vertex2);
                                    if (link.ReverseLink == sector.FirstLink)
                                        break;
                                    else
                                        link = pathfindingDatum.Links[link.ReverseLink];
                                }
                            }

                            var xlist = new List<float>();
                            var ylist = new List<float>();
                            var zlist = new List<float>();

                            foreach (var vert in vertices)
                            {
                                xlist.Add(pathfindingDatum.Vertices[vert].Position.X);
                                ylist.Add(pathfindingDatum.Vertices[vert].Position.Y);
                                zlist.Add(pathfindingDatum.Vertices[vert].Position.Z);
                            }

                            float xmin = xlist.Min();
                            float xmax = xlist.Max();
                            float ymin = ylist.Min();
                            float ymax = ylist.Max();
                            float zmin = zlist.Min();
                            float zmax = zlist.Max();
                            float zave = zlist.Average();

                            int pnpoly(int nvert, List<float> vertx, List<float> verty, float testx, float testy)
                            {
                                int q, j, c = 0;
                                for (q = 0, j = nvert - 1; q < nvert; j = q++)
                                {
                                    if (((verty[q] > testy) != (verty[j] > testy)) &&
                                     (testx < (vertx[j] - vertx[q]) * (testy - verty[q]) / (verty[j] - verty[q]) + vertx[q]))
                                        if (c == 0)
                                            c = 1;
                                        else
                                            c = 0;
                                }
                                return c;
                            }

                            if (pnpoly(xlist.Count, xlist, ylist, hint_x, hint_y) == 1)
                            {
                                sectorlist.Add(s);
                                zavelist.Add(Math.Abs(hint_z - zave));
                            }
                        }

                        if (sectorlist.Count > 0)
                        {
                            var s = sectorlist[zavelist.IndexOf(zavelist.Min())];
                            var hiword = (short)(hint.Data[3] >> 16);
                            hint.Data[3] = hiword << 16 | s;
                            success = true;
                        }

                        if (!success)
                            Console.WriteLine("Pathfinding Jump Hint sector not found!");
                    }

                    resourceStream.Position = pathfindingDatum.PathfindingHints.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.PathfindingHints.Count; i++)
                        CacheContext.Serializer.Serialize(dataContext, pathfindingDatum.PathfindingHints[i]);

                    CacheContext.ReplaceResource(Definition.PathfindingResource, resourceStream);
                }
            }

            return true;
        }
    }
}