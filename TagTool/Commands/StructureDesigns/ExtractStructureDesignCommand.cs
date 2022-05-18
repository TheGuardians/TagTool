using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;
using TagTool.Pathfinding;

namespace TagTool.Commands.StructureDesigns
{
    class ExtractStructureDesignCommand : Command
    {
        private GameCache Cache { get; }
        private StructureDesign Definition { get; }

        public ExtractStructureDesignCommand(GameCache cache, StructureDesign definition) :
            base(true,

                "ExtractStructureDesign",
                "",

                "ExtractStructureDesign <OBJ File>",

                "")
        {
            Cache = cache;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return new TagToolError(CommandError.ArgCount);

            using (var writer = File.CreateText(args[0]))
            {
                foreach (var softceiling in Definition.SoftCeilings)
                {
                    foreach(var triangle in softceiling.SoftCeilingTriangles)
                    {
                        writer.WriteLine($"v {triangle.Point1.X} {triangle.Point1.Z} {triangle.Point1.Y}");
                        writer.WriteLine($"v {triangle.Point2.X} {triangle.Point2.Z} {triangle.Point2.Y}");
                        writer.WriteLine($"v {triangle.Point3.X} {triangle.Point3.Z} {triangle.Point3.Y}");
                    }
                }

                var i = 1;
                foreach (var softceiling in Definition.SoftCeilings)
                {
                    writer.WriteLine($"g {Cache.StringTable.GetString(softceiling.Name)}");
                    foreach (var triangle in softceiling.SoftCeilingTriangles)
                    {
                        writer.WriteLine($"f {i} {i+1} {i+2}");
                        i += 3;
                    }
                }
            }

            return true;
        }
    }
}