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
            
            var namedWith = "";

            while (args.Count > 1)
            {
                switch (args[0].ToLower())
                {
                    case "named:":
                        namedWith = args[1];
                        args.RemoveAt(0);
                        break;

                    default:
                        throw new FormatException(args[0]);
                }

                args.RemoveAt(0);
            }

            var groupTag = args.Count == 0 ? Tag.Null : ArgumentParser.ParseGroupTag(CacheContext.StringIdCache, args[0]);

            foreach (var tag in BlamCache.IndexItems)
            {
                if (tag == null || (groupTag != Tag.Null && !tag.IsInGroup(groupTag)))
                    continue;

                if (namedWith != "" && !tag.Filename.Contains(namedWith))
                    continue;

                Console.WriteLine($"[Index: 0x{tag.metaIndex:X4}, Offset: 0x{tag.Offset:X8}, Size: 0x{tag.Size:X4}] {tag.Filename}.{tag.GroupName}");
            }

            return true;
        }
    }
}
