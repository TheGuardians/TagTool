using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Analysis;
using TagTool.Cache;
using TagTool.Layouts;
using TagTool.Commands.Common;
using TagTool.Cache.HaloOnline;

namespace TagTool.Commands.Definitions
{
    class GenerateTagStructuresCommand : Command
    {
        private GameCacheHaloOnlineBase Cache { get; }

        public GenerateTagStructuresCommand(GameCacheHaloOnlineBase cache) : base(
            true,

            "GenerateTagStructures",
            "Generates tag structures in either C# or C++.",

            "GenerateTagStructures <type> <output dir>",

            "Scans all tags in the file to guess tag layouts.\n" +
            "Layouts will be written to the output directory in the chosen format.\n" +
            "\n" +
            "Supported types: C#, C++")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return new TagToolError(CommandError.ArgCount);

            var type = args[0];
            var outDir = args[1];

            TagLayoutWriter writer;

            switch (type.ToLower())
            {
                case "c#":
                case "cs":
                case "csharp":
                    writer = new CSharpLayoutWriter();
                    break;

                case "c++":
                case "cpp":
                case "cplusplus":
                    writer = new CppLayoutWriter();
                    break;
                default:
                    return new TagToolError(CommandError.ArgInvalid, $"\"{type}\"");
            }

            Directory.CreateDirectory(outDir);

            var count = 0;
            using (var stream = Cache.OpenCacheRead())
            {
                foreach (var groupTag in Cache.TagCache.NonNull().Select(t => t.Group.Tag).Distinct())
                {
                    TagLayoutGuess layout = null;
                    CachedTag lastTag = null;

                    foreach (var tag in Cache.TagCache.FindAllInGroup(groupTag))
                    {
                        Console.Write("Analyzing ");
                        TagPrinter.PrintTagShort(tag);

                        lastTag = tag;

                        var analyzer = new TagAnalyzer(Cache.TagCacheGenHO);
                        var data = Cache.TagCacheGenHO.ExtractTag(stream, (CachedTagHaloOnline)tag);
                        var tagLayout = analyzer.Analyze(data);

                        if (layout != null)
                            layout.Merge(tagLayout);
                        else
                            layout = tagLayout;
                    }

                    if (layout != null && lastTag != null)
                    {
                        Console.WriteLine("Writing {0} layout", groupTag);

                        var name = lastTag.Group.ToString();
                        var tagLayout = LayoutGuessFinalizer.MakeLayout(layout, name, groupTag);
                        var path = Path.Combine(outDir, writer.GetSuggestedFileName(tagLayout));

                        writer.WriteLayout(tagLayout, path);

                        count++;
                    }
                }
            }

            Console.WriteLine("Successfully generated {0} layouts!", count);

            return true;
        }
    }
}
