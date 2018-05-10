using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands;

namespace TagTool.Commands.Tags
{
    class ListTagsCommand : Command
    {
        private GameCacheContext CacheContext { get; }

        public ListTagsCommand(GameCacheContext cacheContext)
            : base(CommandFlags.Inherit,

                  "ListTags",
                  "Lists tag instances that are of the specified tag group.",

                  "ListTags [Named | Unnamed] [search_term] <group tag 1> ... <group tag n>",

                  "Lists tag instances that are of the specified tag group." +
                  "Multiple group tags to list tags from can be specified.\n" +
                  "Tags of a group which inherit from the given group tags will also\n" +
                  "be printed. If no group tag is specified, all tags in the current\n" +
                  "tag cache file will be listed.")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            var searchClasses = ArgumentParser.ParseGroupTags(CacheContext.StringIdCache, args);

            if (searchClasses == null)
                return false;

            var named = false;
            var unnamed = false;
			var search_term = "";

            while (args.Count > 0)
            {

                switch (args[0].ToLower())
                {
                    case "named":
                        named = true;
                        args.RemoveAt(0);
                        break;

                    case "unnamed":
                        unnamed = true;
                        args.RemoveAt(0);
						break;
					default:
						search_term = args[0];
						args.RemoveAt(0);
                        break;
                }
            }

            CachedTagInstance[] tags;
            if (args.Count > 0)
                tags = CacheContext.TagCache.Index.FindAllInGroups(searchClasses).ToArray();
            else
                tags = CacheContext.TagCache.Index.NonNull().ToArray();

            if (tags.Length == 0)
            {
                Console.Error.WriteLine("No tags found.");
                return true;
            }

            foreach (var tag in tags)
            {
                if (unnamed && !CacheContext.TagNames.ContainsKey(tag.Index))
                    Console.WriteLine($"[Index: 0x{tag.Index:X4}, Offset: 0x{tag.HeaderOffset:X8}, Size: 0x{tag.TotalSize:X4}] {CacheContext.GetString(tag.Group.Name)} ({tag.Group.Tag})");
                else if (named && CacheContext.TagNames.ContainsKey(tag.Index))
                    Console.WriteLine($"[Index: 0x{tag.Index:X4}, Offset: 0x{tag.HeaderOffset:X8}, Size: 0x{tag.TotalSize:X4}] {CacheContext.TagNames[tag.Index]}.{CacheContext.GetString(tag.Group.Name)}");
                else if (!unnamed && !named)
                {
                    var tagName = CacheContext.TagNames.ContainsKey(tag.Index) ?
                        CacheContext.TagNames[tag.Index] :
                        $"0x{tag.Index:X4}";

                    Console.WriteLine($"[Index: 0x{tag.Index:X4}, Offset: 0x{tag.HeaderOffset:X8}, Size: 0x{tag.TotalSize:X4}] {tagName}.{CacheContext.GetString(tag.Group.Name)}");
                }
            }

            return true;
        }
    }
}
