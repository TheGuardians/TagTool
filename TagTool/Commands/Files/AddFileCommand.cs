using TagTool.Cache;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;

namespace TagTool.Commands.Files
{
    class AddFileCommand : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private VFilesList Definition { get; }

        public AddFileCommand(GameCache cache, CachedTag tag, VFilesList definition) :
            base(true,
                
                "AddFile",
                "Adds a new file to the virtual files list.",

                "AddFile <folder> <path>",

                "Adds a new file to the virtual files list.")
        {
            Cache = cache;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return false;

            var folder = args[0].Replace('/', '\\');
            var file = new FileInfo(args[1]);

            if (!folder.EndsWith("\\"))
                folder += "\\";

            if (!file.Exists)
            {
                Console.WriteLine($"ERROR: File not found: \"{file.FullName}\"");
                return false;
            }

            Definition.Insert(file.Name, folder, File.ReadAllBytes(file.FullName));

            using (var stream = Cache.TagCache.OpenTagCacheReadWrite())
                Cache.Serialize(stream, Tag, Definition);

            Console.WriteLine($"Add virtual file \"{folder}\".");

            return true;
        }
    }
}
