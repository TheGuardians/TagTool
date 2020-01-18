using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;

namespace TagTool.Commands.Tags
{
    class ExtractAllTagsCommand : Command
    {
        public GameCacheHaloOnline Cache { get; }

        public ExtractAllTagsCommand(GameCacheHaloOnline cache)
            : base(false,

                  "ExtractAllTags",
                  "Extracts all tags in the current tag cache to a specific directory.",

                  "ExtractAllTags <output directory>",

                  "Extracts all tags in the current tag cache to a specific directory.")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var directory = args[0];

            if (!Directory.Exists(directory))
            {
                Console.Write("Destination directory does not exist. Create it? [y/n] ");
                var answer = Console.ReadLine().ToLower();

                if (answer.Length == 0 || !(answer.StartsWith("y") || answer.StartsWith("n")))
                    return false;

                if (answer.StartsWith("y"))
                    Directory.CreateDirectory(directory);
                else
                    return false;
            }

            using (var cacheStream = Cache.TagCache.OpenTagCacheRead())
            {
                foreach (var instance in Cache.TagCache.TagTable)
                {
                    if (instance == null)
                        continue;

                    var tagName = instance.Name + "." + Cache.StringTable.GetString(instance.Group.Name);
                    var tagPath = Path.Combine(directory, tagName);
                    var tagDirectory = Path.GetDirectoryName(tagPath);

                    if (!Directory.Exists(tagDirectory))
                        Directory.CreateDirectory(tagDirectory);

                    using (var tagStream = File.Create(tagPath))
                    using (var writer = new BinaryWriter(tagStream))
                    {
                        writer.Write(Cache.TagCacheGenHO.ExtractTagRaw(cacheStream, (CachedTagHaloOnline)instance));
                    }

                    Console.WriteLine($"Exported {tagName}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Done!");

            return true;
        }
    }
}
