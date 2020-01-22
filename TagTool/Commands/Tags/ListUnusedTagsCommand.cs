using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;

namespace TagTool.Commands.Tags
{
    class ListUnusedTagsCommand : Command
    {
        public GameCacheHaloOnlineBase Cache { get; }

        public ListUnusedTagsCommand(GameCacheHaloOnlineBase cache)
            : base(false,

                  "ListUnusedTags",
                  "Lists all unreferenced tags in the current tag cache",

                  "ListUnusedTags",

                  "Lists all unreferenced tags in the current tag cache")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;

            var depCounts = new Dictionary<int, int>();

            foreach (var tag in Cache.TagCacheGenHO.Tags)
            {
                if (tag == null)
                    continue;

                foreach (var dep in tag.Dependencies)
                {
                    var depTag = Cache.TagCache.GetTag(dep);

                    if (depTag == null)
                        continue;

                    if (!depCounts.ContainsKey(dep))
                        depCounts[dep] = 0;

                    depCounts[dep]++;
                }
            }

            for (var i = 0; i < Cache.TagCache.Count; i++)
            {
                var tag = Cache.TagCache.GetTag(i);

                if (tag == null || tag.IsInGroup("cfgt") || tag.IsInGroup("scnr"))
                    continue;

                if (depCounts.ContainsKey(i))
                    continue;

                var tagName = tag.Name ?? "<unnamed>";

                Console.Write($"{tagName} ");
                TagPrinter.PrintTagShort(tag);
            }

            return true;
        }
    }
}