using System;
using System.Collections.Generic;
using TagTool.Cache;
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

                  "ListTags <Tag Group> {Options}",

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
            var groupTag = (args.Count == 0 || args[0].EndsWith(":")) ? Tag.Null : CacheContext.ParseGroupTag(args[0]);

            if (args.Count > 0 && !args[0].EndsWith(":"))
                args.RemoveAt(0);

            var named = false;
            var unnamed = false;
            
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
                        goto case "named";

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
                        goto case "named";

                    case "named:":
                    case "filter:":
                    case "contains:":
                    case "containing:":
                        filter = args[1];
                        args.RemoveAt(1);
                        goto case "named";

                    case "named":
                        named = true;
                        break;

                    case "unnamed":
                        unnamed = true;
						break;

                    default:
                        throw new FormatException(args[0]);
                }

                args.RemoveAt(0);
            }

            foreach (var tag in CacheContext.TagCache.Index)
            {
                if (tag == null || (groupTag != Tag.Null && !tag.IsInGroup(groupTag)))
                    continue;

                var tagName = CacheContext.TagNames.ContainsKey(tag.Index) ?
                    CacheContext.TagNames[tag.Index] :
                    $"0x{tag.Index:X4}";

                var groupName = CacheContext.GetString(tag.Group.Name);
                
                if (named)
                {
                    if (!CacheContext.TagNames.ContainsKey(tag.Index))
                        continue;

                    if (!tagName.StartsWith(startFilter) || !tagName.Contains(filter) || !tagName.EndsWith(endFilter))
                        continue;
                }
                
                if (unnamed && !CacheContext.TagNames.ContainsKey(tag.Index))
                    Console.WriteLine($"[Index: 0x{tag.Index:X4}, Offset: 0x{tag.HeaderOffset:X8}, Size: 0x{tag.TotalSize:X4}] {groupName} ({tag.Group.Tag})");
                else if (named && CacheContext.TagNames.ContainsKey(tag.Index))
                    Console.WriteLine($"[Index: 0x{tag.Index:X4}, Offset: 0x{tag.HeaderOffset:X8}, Size: 0x{tag.TotalSize:X4}] {tagName}.{groupName}");
                else if (!named && !unnamed)
                    Console.WriteLine($"[Index: 0x{tag.Index:X4}, Offset: 0x{tag.HeaderOffset:X8}, Size: 0x{tag.TotalSize:X4}] {tagName}.{groupName}");
            }

            return true;
        }
    }
}
