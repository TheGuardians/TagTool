using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Commands;
using TagTool.TagDefinitions;

namespace TagTool.Commands.Files
{
    class ExtractFilesCommand : Command
    {
        private VFilesList Definition { get; }

        public ExtractFilesCommand(VFilesList definition)
            : base(CommandFlags.Inherit,

                  "ExtractFiles",
                  "Extracts all virtual files from the tag.",

                  "ExtractFiles [output path]",

                  "If not output path is specified, files will be extracted to the current directory.")
        {
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 1)
                return false;

            var baseDir = (args.Count == 1) ? args[0] : Directory.GetCurrentDirectory();

            foreach (var file in Definition.Files)
            {
                var outDir = Path.Combine(baseDir, file.Folder);
                Directory.CreateDirectory(outDir);

                var outPath = Path.Combine(outDir, file.Name);
                var data = Definition.Extract(file);
                File.WriteAllBytes(outPath, data);
            }

            Console.WriteLine("Extracted {0} files.", Definition.Files.Count);

            return true;
        }
    }
}
