using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags.Definitions;
using TagTool.Commands.Common;
using TagTool.Geometry;
using TagTool.Tags;
using TagTool.Tags.Resources;
using TagTool.Geometry.Utils;

namespace TagTool.Commands.Scenarios
{
    class ConvertInstancedGeometryCommand : Command
    {
        private GameCacheHaloOnlineBase HoCache { get; }
        private Scenario Scnr { get; }
        private bool centergeometry = true;

        public ConvertInstancedGeometryCommand(GameCache cacheContext, Scenario scnr) :
            base(false,

                "ConvertInstancedGeometry",
                "Convert Instanced Geometry in Halo Online maps to forge objects",

                "ConvertInstancedGeometry <BspIndex> [nocenter] [<Instance index or 'all'> [New Tagname]]",

                "Convert Instanced Geometry in Halo Online maps to forge objects")
        {
            HoCache = (GameCacheHaloOnlineBase)cacheContext;
            Scnr = scnr;
        }

        public override object Execute(List<string> args)
        {
            var argStack = new Stack<string>(args.AsEnumerable().Reverse());

            if (argStack.Count < 1)
                return new TagToolError(CommandError.ArgCount, "Expected bsp index!");

            if (!int.TryParse(argStack.Pop(), out int sbspIndex))
                return new TagToolError(CommandError.ArgInvalid, "Invalid bsp index");

            using (var hoCacheStream = HoCache.OpenCacheReadWrite())
            {
                var hoSbsp = HoCache.Deserialize<ScenarioStructureBsp>(hoCacheStream, Scnr.StructureBsps[sbspIndex].StructureBsp);

                var desiredInstances = new Dictionary<int, string>();
                var converted = new HashSet<short>();

                if (argStack.Count > 0 && argStack.Peek().ToLower() == "nocenter")
                {
                    argStack.Pop();
                    centergeometry = false;
                }
                if (argStack.Count > 0)
                {
                    if(argStack.Peek().ToLower() == "all")
                    {
                        for(var instanceindex = 0; instanceindex < hoSbsp.InstancedGeometryInstances.Count; instanceindex++)
                        {
                            short meshindex = hoSbsp.InstancedGeometryInstances[instanceindex].DefinitionIndex;
                            if (converted.Contains(meshindex))
                                continue;
                            converted.Add(meshindex);
                            desiredInstances.Add(instanceindex, null);
                        }
                    }
                    else
                    {
                        int.TryParse(argStack.Pop(), out int index);
                        string desiredName = null;
                        if (argStack.Count > 0)
                        {
                            desiredName = argStack.Pop();
                        }

                        desiredInstances.Add(index, desiredName);
                    }
                }
                else
                {
                    Console.WriteLine("------------------------------------------------------------------");
                    Console.WriteLine("Enter each instance with the format <Index> [New tagname]");
                    Console.WriteLine("Enter a blank line to finish.");
                    Console.WriteLine("------------------------------------------------------------------");
                    for (string line; !String.IsNullOrWhiteSpace(line = Console.ReadLine());)
                    {
                        var parts = line.Split(' ');
                        int.TryParse(parts[0], out var index);
                        var name = parts.Length > 1 ? string.Join(" ", parts.Skip(1)) : null;

                        if (index == -1)
                            return new TagToolError(CommandError.OperationFailed);

                        desiredInstances.Add(index, name);
                    }
                }

                if (desiredInstances.Count < 1)
                    return true;

                var converter = new GeometryToObjectConverter(HoCache, hoCacheStream, HoCache, hoCacheStream, Scnr, sbspIndex);

                foreach (var kv in desiredInstances)
                {
                    try
                    {
                        var instance = hoSbsp.InstancedGeometryInstances[kv.Key];
                        var tag = converter.ConvertGeometry(kv.Key, kv.Value, false, centergeometry);
                    }
                    finally
                    {
                        HoCache.SaveStrings();
                        HoCache.SaveTagNames();
                    }
                }
            }

            return true;
        }
    }
}
