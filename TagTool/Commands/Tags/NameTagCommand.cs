using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;

namespace TagTool.Commands.Tags
{
    class NameTagCommand : Command
    {
        public GameCache Cache { get; }

        public NameTagCommand(GameCache cache)
            : base(true,
                  
                  "NameTag",
                  "Sets the name of a tag file in the current cache.",
                  
                  "NameTag <tag> <name> [csv path]",
                  
                  "<tag>  - A valid tag index, tag name, or * for the last tag in the current cache. \n" +
                  "\n" +
                  "<name> - The name of the tag. Should be a concise name that resembles the format \n" +
                  "         of existing tag names.\n")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 3)
                return new TagToolError(CommandError.ArgCount);
            if (!Cache.TagCache.TryGetCachedTag(args[0], out var tag))
                return new TagToolError(CommandError.TagInvalid);

            if (args.Count < 2)
            {
                tag.Name = "";
                Console.WriteLine("Removed tag name");
                return true;
            }

            tag.Name = args[1].Split('.')[0];

            Console.WriteLine($"[Index: 0x{tag.Index:X4}] {args[1].Split('.')[0]}.{tag.Group}");

            return true;
        }
    }
}