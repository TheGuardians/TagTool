using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands;

namespace TagTool.Commands.Tags
{
    class ListNullTagsCommand : Command
    {
        public GameCacheContext CacheContext { get; }

        public ListNullTagsCommand(GameCacheContext cacheContext)
            : base(CommandFlags.None,
                  
                  "ListNullTags",
                  "Lists all null tag indices in the current tag cache",
                  
                  "ListNullTags",
                  
                  "Lists all null tag indices in the current tag cache")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;

            for (var i = 0; i < CacheContext.TagCache.Index.Count; i++)
            {
                if (CacheContext.TagCache.Index[i] == null)
                    Console.WriteLine($"0x{i:X4}");
            }

            return true;
        }
    }
}
