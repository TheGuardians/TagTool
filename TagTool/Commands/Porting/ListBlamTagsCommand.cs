using TagTool.Cache;
using System;
using System.Collections.Generic;
using TagTool.Common;

namespace TagTool.Commands.Porting
{
    public class ListBlamTagsCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CacheFile BlamCache;

        public ListBlamTagsCommand(HaloOnlineCacheContext cacheContext, CacheFile blamCache)
            : base(false,

                  "ListBlamTags",
                  "Lists tag instances that are of the specified tag group.",

                  "ListBlamTags <group tag>",

                  "Lists tag instances that are of the specified tag group.")
        {
            CacheContext = cacheContext;
            BlamCache = blamCache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1)
                return false;

            var groupTag = (args.Count == 0 || args[0].EndsWith(":")) ? Tag.Null : ArgumentParser.ParseGroupTag(CacheContext.StringIdCache, args[0]);

            if (!args[0].EndsWith(":"))
                args.RemoveAt(0);

            var startFilter = "";
            var endFilter = "";
            var filter = "";

            while (args.Count > 1)
            {
                switch (args[0].ToLower())
                {
                    case "starts:":
                    case "startswith:":
                    case "starts_with:":
                    case "starting:":
                    case "startingwith:":
                    case "starting_with:":
                    case "start_filter:":
                    case "starting_filter:":
                        startFilter = args[1];
                        args.RemoveAt(1);
                        break;

                    case "ends:":
                    case "ending:":
                    case "endingwith:":
                    case "ending_with:":
                    case "endswith:":
                    case "ends_with:":
                    case "end_filter:":
                    case "ending_filter:":
                        endFilter = args[1];
                        args.RemoveAt(1);
                        break;

                    case "named:":
                    case "filter:":
                    case "contains:":
                    case "containing:":
                        filter = args[1];
                        args.RemoveAt(1);
                        break;

                    default:
                        throw new FormatException(args[0]);
                }

                args.RemoveAt(0);
            }

            foreach (var tag in BlamCache.IndexItems)
            {
                if (tag == null || (groupTag != Tag.Null && !tag.IsInGroup(groupTag)))
                    continue;

                if (!tag.Name.StartsWith(startFilter) || !tag.Name.Contains(filter) || !tag.Name.EndsWith(endFilter))
                    continue;

                Console.WriteLine($"[Index: 0x{tag.Index:X4}, Offset: 0x{tag.Offset:X8}, Size: 0x{tag.Size:X4}] {tag.Name}.{tag.GroupName}");
            }

            return true;
        }
    }
}