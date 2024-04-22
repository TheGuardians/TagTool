using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Cache.HaloOnline;
using TagTool.Cache.Monolithic;
using TagTool.Tags;
using TagTool.Common;

namespace TagTool.Commands.Tags
{
    class ExtractAllTagsCommand : Command
    {
        public GameCache Cache { get; }

        public ExtractAllTagsCommand(GameCache cache)
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
                return new TagToolError(CommandError.ArgCount);

            var directory = args[0];

            if (!Directory.Exists(directory))
            {
                Console.Write("Destination directory does not exist. Create it? [y/n] ");
                var answer = Console.ReadLine().ToLower();

                if (answer.Length == 0 || !(answer.StartsWith("y") || answer.StartsWith("n")))
                    return new TagToolError(CommandError.YesNoSyntax);

                if (answer.StartsWith("y"))
                    Directory.CreateDirectory(directory);
                else
                    return true;
            }

            using (var cacheStream = Cache.OpenCacheRead())
            {
                foreach (var instance in Cache.TagCache.TagTable)
                {
                    if (instance == null)
                        continue;

                    if (instance.Group.Tag == Tag.Null)
                        continue;

                    var tagName = instance.Name + "." + instance.Group;
                    var tagPath = Path.Combine(directory, tagName);
                    var tagDirectory = Path.GetDirectoryName(tagPath);

                    if (!Directory.Exists(tagDirectory))
                        Directory.CreateDirectory(tagDirectory);

                    var tagData = ExtractTag(cacheStream, instance);
                    if (tagData == null)
                        continue;

                    using (var tagStream = File.Create(tagPath))
                    using (var writer = new BinaryWriter(tagStream))
                    {
                        writer.Write(tagData);
                    }

                    Console.WriteLine($"Exported {tagName}");
                }
            }

            Console.WriteLine("Done!");
            return true;
        }

        private byte[] ExtractTag(Stream cacheStream, CachedTag instance)
        {
            switch(Cache)
            {
                case GameCacheHaloOnlineBase hoCache:
                    return hoCache.TagCacheGenHO.ExtractTagRaw(cacheStream, (CachedTagHaloOnline)instance);
                case GameCacheMonolithic monolithicCache:
                    return monolithicCache.Backend.ExtractTagRaw(instance.Index);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
