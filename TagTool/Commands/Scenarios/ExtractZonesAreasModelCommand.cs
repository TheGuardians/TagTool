using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Scenarios
{
    class ExtractZonesAreasModelCommand : Command
    {
        private GameCache Cache { get; }
        private Scenario Definition { get; }

        public ExtractZonesAreasModelCommand(GameCache cache, Scenario definition) :
            base(false,

                "ExtractZonesAreasModel",
                "",
                "ExtractZonesAreasModel <Path>",
                "")
        {
            Cache = cache;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var file = new FileInfo(args[0]);

            if (!file.Directory.Exists)
                file.Directory.Create();

            using (var writer = file.CreateText())
            {
                writer.WriteLine($"o {file.Name}");

                var baseIndex = 0;

                foreach (var zone in Definition.Zones)
                {
                    foreach (var area in zone.Areas)
                    {
                        if (area.Points.Count == 0)
                            continue;

                        foreach (var point in area.Points)
                            writer.WriteLine($"v {point.Position.X} {point.Position.Z} {point.Position.Y}");

                        writer.WriteLine($"g {zone.Name}_{area.Name}");
                        writer.Write("f");

                        foreach (var index in Enumerable.Range(0, area.Points.Count))
                            writer.Write($" {index + baseIndex + 1}");

                        writer.WriteLine();

                        baseIndex += area.Points.Count;
                    }
                }
            }

            return true;
        }
    }
}