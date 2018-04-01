using TagTool.Cache;
using TagTool.Commands;
using System;
using System.Collections.Generic;

namespace TagTool.Commands.Porting
{
    public class ListBlamTagsCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private CacheFile BlamCache { get; }

        public ListBlamTagsCommand(GameCacheContext cacheContext, CacheFile blamCache)
            : base(CommandFlags.None,

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
            if (args.Count > 1)
                return false;

            List<string> tagsList = new List<string>();
            if (args.Count == 1)
            {
                foreach (var tag in BlamCache.IndexItems)
                {
                    if (tag.ClassCode == args[0])
                    {
                        tagsList.Add("[" + tag.ClassCode.ToString() + "] " + tag.Filename.ToString()); // BlamCache.Header.scenarioName
                    }
                }
            }
            else
            {
                foreach (var tag in BlamCache.IndexItems)
                {
                    tagsList.Add("[" + tag.ClassCode.ToString() + "] " + tag.Filename.ToString());
                }
            }

            tagsList.Sort();
            foreach (var tagName in tagsList)
            {
                Console.WriteLine(tagName);
            }

            return true;
        }
    }
}
