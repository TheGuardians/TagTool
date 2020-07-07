using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;

namespace TagTool.Commands.Tags
{
    class ListUnnamedTagsCommand : Command
    {
        public GameCache Cache { get; }

        public ListUnnamedTagsCommand(GameCache cache)
            : base(false,

                  "ListUnnamedTags",
                  "Lists all unnamed tags in the current tag cache",

                  "ListUnnamedTags",

                  "Lists all unnamed tags in the current tag cache")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return new TagToolError(CommandError.ArgCount);

            var unnamedTags = new List<CachedTag>();

            foreach (var tag in Cache.TagCache.TagTable)
            {
                if (tag != null && (tag.Name == null || tag.Name == ""))
                {
                    Console.WriteLine($"0x{tag.Index:X4}.{tag.Group.Tag.ToString()}");
                    unnamedTags.Add(tag);
                }
            }

            Console.WriteLine($"Total unnamed tag count: {unnamedTags.Count}");

            return true;
        }
    }
}