using System;
using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Commands.Tags
{
    class ListNullTagsCommand : Command
    {
        public GameCache Cache { get; }

        public ListNullTagsCommand(GameCache cache)
            : base(false,
                  
                  "ListNullTags",
                  "Lists all null tag indices in the current tag cache",
                  
                  "ListNullTags",
                  "Lists all null tag indices in the current tag cache")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;

            int currentIndex = 0x0;
            foreach (var tag in Cache.TagCache.TagTable)
            {
                if (tag == null)
                    Console.WriteLine($"0x{currentIndex:X4}");
                currentIndex++;
            }

            return true;
        }
    }
}
