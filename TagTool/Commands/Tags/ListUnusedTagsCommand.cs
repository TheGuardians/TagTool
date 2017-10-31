using System;
using System.Collections.Generic;
using System.Linq;
using BlamCore.Cache;

namespace TagTool.Commands.Tags
{
    class ListUnusedTagsCommand : Command
    {
        public GameCacheContext CacheContext { get; }

        public ListUnusedTagsCommand(GameCacheContext cacheContext)
            : base(CommandFlags.None,

                  "ListUnusedTags",
                  "Lists all unreferenced tags in the current tag cache",

                  "ListUnusedTags",

                  "Lists all unreferenced tags in the current tag cache")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;

            var nonNullTags = CacheContext.TagCache.Index.NonNull();

            foreach (var tag in CacheContext.TagCache.Index)
            {
                if (tag == null || tag.IsInGroup("cfgt") || tag.IsInGroup("scnr"))
                    continue;
                
                var dependsOn = nonNullTags.Where(t => t.Dependencies.Contains(tag.Index));

                if (dependsOn.Count() == 0)
                {
                    var tagName = CacheContext.TagNames.ContainsKey(tag.Index) ?
                        CacheContext.TagNames[tag.Index] :
                        "<unnamed>";

                    Console.Write($"{tagName} ");
                    TagPrinter.PrintTagShort(tag);
                }
            }

            return true;
        }
    }
}