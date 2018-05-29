using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands;
using TagTool.Common;

namespace TagTool.Commands.Tags
{
    class ListTagsCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }

        public ListTagsCommand(HaloOnlineCacheContext cacheContext)
            : base(true,

                  "ListTags",
                  "Lists tag instances that are of the specified tag group.",

                  "ListTags [Named | Unnamed] <group tag 1> ... <group tag n>",

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
            if (args.Count < 1)
                return false;

            var named = false;
            var namedWith = "";
            var unnamed = false;

            while (args.Count > 1)
            {
                switch (args[0].ToLower())
                {
                    case "named":
                        named = true;
                        break;

                    case "named:":
                        named = true;
                        namedWith = args[1];
                        args.RemoveAt(0);
                        break;

                    case "unnamed":
                        unnamed = true;
						break;
                }

                args.RemoveAt(0);
            }

            var groupTag = args.Count == 0 ? Tag.Null : ArgumentParser.ParseGroupTag(CacheContext.StringIdCache, args[0]);

            foreach (var tag in CacheContext.TagCache.Index)
            {
                if (tag == null || (groupTag != Tag.Null && !tag.IsInGroup(groupTag)))
                    continue;

                if (named && namedWith != "" && (!CacheContext.TagNames.ContainsKey(tag.Index) || !CacheContext.TagNames[tag.Index].Contains(namedWith)))
                    continue;

                if (unnamed && !CacheContext.TagNames.ContainsKey(tag.Index))
                    Console.WriteLine($"[Index: 0x{tag.Index:X4}, Offset: 0x{tag.HeaderOffset:X8}, Size: 0x{tag.TotalSize:X4}] {CacheContext.GetString(tag.Group.Name)} ({tag.Group.Tag})");
                else if (named && CacheContext.TagNames.ContainsKey(tag.Index))
                    Console.WriteLine($"[Index: 0x{tag.Index:X4}, Offset: 0x{tag.HeaderOffset:X8}, Size: 0x{tag.TotalSize:X4}] {CacheContext.TagNames[tag.Index]}.{CacheContext.GetString(tag.Group.Name)}");
                else if (!named && !unnamed)
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
