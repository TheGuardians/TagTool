using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Commands;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Files
{
    class ListFilesCommand : Command
    {
        private readonly VFilesList Definition;

        public ListFilesCommand(VFilesList definition)
            : base(true,

                  "ListFiles",
                  "List files stored in the tag.",

                  "ListFiles [filter]",

                  "If a filter is specified, only files which contain the filter in their path\n" +
                  "will be listed.")
        {
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 1)
                return false;

            string filter = null;

            if (args.Count == 1)
                filter = args[0];

            // Just sort the paths in alphabetical order
            var paths = Definition.Files
                .Select(f => Path.Combine(f.Folder, f.Name))
                .Where(p => filter == null || p.Contains(filter))
                .OrderBy(p => p)
                .ToList();

            foreach (var path in paths)
                Console.WriteLine(path);

            return true;
        }
    }
}
