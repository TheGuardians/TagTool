using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands.Common;

namespace TagTool.Commands.Tags
{
    class ListUnnamedTagsCommand : Command
    {
        public HaloOnlineCacheContext CacheContext { get; }

        public ListUnnamedTagsCommand(HaloOnlineCacheContext cacheContext)
            : base(false,

                  "ListUnnamedTags",
                  "Lists all unnamed tags in the current tag cache",

                  "ListUnnamedTags",

                  "Lists all unnamed tags in the current tag cache")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;

            var unnamedTags = new List<CachedTagInstance>();

            foreach (var tag in CacheContext.TagCache.Index)
            {
                if (tag != null && (tag.Name == null || tag.Name == ""))
                {
                    //Console.WriteLine($"0x{tag.Index:X4}.{tag.Group.Tag.ToString()}");
                    unnamedTags.Add(tag);
                }
            }

            Console.WriteLine($"Total unnamed tag count: {unnamedTags.Count}");

            var visitedIndices = new HashSet<int>();

            var indexQueue = unnamedTags.Select(x => x.Index).ToList();
            var topmost = new List<int>();

            while (indexQueue.Count != 0)
            {
                var nextQueue = new List<int>();

                foreach (var index in indexQueue)
                {
                    foreach (var dependent in CacheContext.TagCache.Index.NonNull().Where(t => t.Dependencies.Contains(index)))
                    {
                        if (visitedIndices.Contains(dependent.Index))
                            continue;

                        if (dependent.Name == null)
                            nextQueue.Add(dependent.Index);
                        else if (!topmost.Contains(dependent.Index))
                            topmost.Add(dependent.Index);

                        visitedIndices.Add(dependent.Index);
                    }
                }

                indexQueue = nextQueue;
            }

            Console.WriteLine("Topmost tags:");

            foreach (var tagIndex in topmost)
            {
                var tag = CacheContext.GetTag(tagIndex);

                var tagName = (tag.Name == null || tag.Name == "") ?
                    $"0x{tag.Index:X4}" :
                    tag.Name;

                Console.WriteLine($"[0x{tag.Index:X4}] {tagName}.{tag.Group.Tag}");
            }

            Console.WriteLine($"Total topmost tag count: {indexQueue.Count}");

            return true;
        }
    }
}