using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Commands;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Files
{
    class ExtractFileCommand : Command
    {
        private VFilesList Definition { get; }

        public ExtractFileCommand(VFilesList definition)
            : base(CommandFlags.Inherit,

                  "ExtractFile",
                  "Extract a file from the tag.",

                  "ExtractFile <virtual path> [output path]",

                  "Extracts a file stored in the tag. If the output path is not specified, it will\n" +
                  "be the same as the file's virtual path.\n" +
                  "\n" +
                  "Use the \"ListFiles\" command to find files in the tag.")
        {
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1 && args.Count != 2)
                return false;

            var virtualPath = args[0];
            var outputPath = (args.Count == 2) ? args[1] : virtualPath;
            var file = Definition.Find(virtualPath);

            if (file == null)
            {
                Console.WriteLine("Unable to find file {0}.", virtualPath);
                return true;
            }

            var outDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outDir))
                Directory.CreateDirectory(outDir);

            var data = Definition.Extract(file);
            File.WriteAllBytes(outputPath, data);

            Console.WriteLine("Wrote 0x{0:X} bytes to {1}.", data.Length, outputPath);

            return true;
        }
    }
}
