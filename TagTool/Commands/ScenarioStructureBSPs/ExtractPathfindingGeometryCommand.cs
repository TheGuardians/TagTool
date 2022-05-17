﻿using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;
using TagTool.Pathfinding;

namespace TagTool.Commands.ScenarioStructureBSPs
{
    class ExtractPathfindingGeometryCommand : Command
    {
        private GameCache Cache { get; }
        private ScenarioStructureBsp Definition { get; }

        public ExtractPathfindingGeometryCommand(GameCache cache, ScenarioStructureBsp definition) :
            base(true,

                "ExtractPathfindingGeometry",
                "",

                "ExtractPathfindingGeometry <OBJ File>",

                "")
        {
            Cache = cache;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return new TagToolError(CommandError.ArgCount);

            if (Cache.Version >= CacheVersion.Halo3ODST && Definition.PathfindingResource == null)
            {
                new TagToolError(CommandError.OperationFailed, "Pathfinding geometry does not have a resource associated with it.");
                return true;
            }

            var pathfinding = new ResourcePathfinding();
            if (Cache.Version >= CacheVersion.Halo3ODST)
            {
                var resourceDefinition = Cache.ResourceCache.GetStructureBspCacheFileTagResources(Definition.PathfindingResource);
                pathfinding = resourceDefinition.PathfindingData[0];
            }
            else
            {
                var temp = Definition.PathfindingData[0];
                pathfinding = new ResourcePathfinding
                {
                    Sectors = temp.Sectors,
                    PathfindingHints = temp.PathfindingHints,
                    Links = temp.Links,
                    Vertices = temp.Vertices,
                    //can fill out the rest if necessary
                };
            }

            using (var writer = File.CreateText(args[0]))
            {
                writer.WriteLine("mtllib Blue.mtl");

                foreach (Pathfinding.Vertex vertex in pathfinding.Vertices)
                {
                    writer.WriteLine($"v {vertex.Position.X} {vertex.Position.Z} {vertex.Position.Y}");
                }

                writer.WriteLine("usemtl Blue");
                writer.WriteLine("g JumpHints");

                for (var i = 0; i < pathfinding.PathfindingHints.Count; i++)
                {
                    var hint = pathfinding.PathfindingHints[i];
                    if (hint.HintType == Pathfinding.PathfindingHint.HintTypeValue.JumpLink)
                    {
                        int v2 = (hint.Data[0] >> 16) + 1;
                        int v1 = (hint.Data[0] & 0xFFFF) + 1;
                        int v4 = (hint.Data[1] >> 16) + 1;
                        int v3 = (hint.Data[1] & 0xFFFF) + 1;
                        int landing = hint.Data[3] & 0xFFFF;
                        writer.WriteLine($"o JumpHint {i}, Landing Sector {landing}");
                        writer.WriteLine($"f {v1} {v2} {v4} {v3}");
                    }
                }

                writer.WriteLine("usemtl White");
                writer.WriteLine("g Sectors");

                for (var i = 0; i < pathfinding.Sectors.Count; i++)
                {
                    var sector = pathfinding.Sectors[i];
                    var vertices = new HashSet<short>();
                    var link = pathfinding.Links[sector.FirstLink];

                    writer.WriteLine($"o Sector {i}");
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

            return true;
        }
    }
}