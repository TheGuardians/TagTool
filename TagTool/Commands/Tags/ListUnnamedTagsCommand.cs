using System;
using System.Collections.Generic;
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
            var topmostTags = new List<CachedTagInstance>();

            foreach (var tag in CacheContext.TagCache.Index)
            {
                if (tag != null && (tag.Name == null || tag.Name == ""))
                {
                    Console.WriteLine($"0x{tag.Index:X4}.{tag.Group.Tag.ToString()}");
                    unnamedTags.Add(tag);
                }
            }

            Console.WriteLine($"Total unnamed tag count: {unnamedTags.Count}");

            foreach (var tag in unnamedTags)
            {
                var topmost = FindTopmost(tag);

                if (topmost != null && !topmostTags.Contains(topmost))
                    topmostTags.Add(topmost);
            }

            Console.WriteLine("Topmost tags:");

            foreach (var tag in topmostTags)
            {
                var tagName = (tag.Name == null || tag.Name == "") ?
                    $"0x{tag.Index:X4}" :
                    tag.Name;

                Console.WriteLine($"{tagName}.{tag.Group.Tag}");
            }

            return true;
        }

        private CachedTagInstance FindTopmost(CachedTagInstance instance)
        {
            if (instance == null)
                return null;

            foreach (var tag in CacheContext.TagCache.Index)
            {
                if (tag == null || tag == instance)
                    continue;

                foreach (var dep in tag.Dependencies)
                {
                    var depTag = CacheContext.GetTag(dep);

                    if (depTag == instance)
                        return FindTopmost(tag);
                }
            }

            return instance;
        }
    }
}