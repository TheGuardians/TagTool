using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using TagTool.Commands.Common;
using TagTool.Tags;

namespace TagTool.Commands.Tags
{
    class ListUnnamedTagsCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }

        public ListUnnamedTagsCommand(HaloOnlineCacheContext cacheContext) :
            base(CommandFlags.Inherit,
                
                "ListUnnamedTags",
                "Lists any non-null tag indices that do not have names assigned to them.",
                
                "ListUnnamedTags",

                "Lists any non-null tag indices that do not have names assigned to them.")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;

            var groups = new Dictionary<Tag, List<CachedTagInstance>>();

            for (var i = 0; i < CacheContext.TagCache.Index.Count; i++)
            {
                var tag = CacheContext.GetTag(i);

                if (tag == null || CacheContext.TagNames.ContainsKey(tag.Index))
                    continue;

                if (!groups.ContainsKey(tag.Group.Tag))
                    groups[tag.Group.Tag] = new List<CachedTagInstance>();

                groups[tag.Group.Tag].Add(tag);
            }

            foreach (var group in groups)
            {
                if (group.Value.Count == 0)
                    continue;

                var groupString = $"{CacheContext.GetString(TagGroup.Instances[group.Key].Name)} ({group.Key}):";
                Console.WriteLine(groupString);

                for (var i = 0; i < groupString.Length; i++)
                    Console.Write('-');

                Console.WriteLine();

                foreach (var tag in group.Value)
                    TagPrinter.PrintTagShort(tag);

                Console.WriteLine();
            }

            return true;
        }
    }
}