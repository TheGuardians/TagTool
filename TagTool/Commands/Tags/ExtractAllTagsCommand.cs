using System;
using System.Collections.Generic;
using System.IO;
using BlamCore.Cache;

namespace TagTool.Commands.Tags
{
    class ExtractAllTagsCommand : Command
    {
        public GameCacheContext CacheContext { get; }

        public ExtractAllTagsCommand(GameCacheContext cacheContext)
            : base(CommandFlags.None,

                  "ExtractAllTags",
                  "Extracts all tags in the current tag cache to a specific directory.",

                  "ExtractAllTags <output directory>",

                  "Extracts all tags in the current tag cache to a specific directory.")
        {
            CacheContext = cacheContext;
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

            using (var cacheStream = CacheContext.OpenTagCacheRead())
            {
                foreach (var instance in CacheContext.TagCache.Index)
                {
                    if (instance == null)
                        continue;

                    var tagName = CacheContext.TagNames[instance.Index] + "." + CacheContext.GetString(instance.Group.Name);
                    var tagPath = Path.Combine(directory, tagName);
                    var tagDirectory = Path.GetDirectoryName(tagPath);

                    if (!Directory.Exists(tagDirectory))
                        Directory.CreateDirectory(tagDirectory);

                    using (var tagStream = File.Create(tagPath))
                    using (var writer = new BinaryWriter(tagStream))
                    {
                        writer.Write(CacheContext.TagCache.ExtractTagRaw(cacheStream, instance));
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
