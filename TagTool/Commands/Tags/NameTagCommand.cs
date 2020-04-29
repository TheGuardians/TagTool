using System;
using System.Collections.Generic;
using TagTool.Cache;

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
                return false;

            if (!Cache.TagCache.TryGetCachedTag(args[0], out var tag))
            {
                Console.WriteLine($"ERROR: Invalid tag specifier: {args[0]}");
                return false;
            }

            if (args.Count < 2)
            {
                tag.Name = null;
                return true;
            }

            tag.Name = args[1];

            Console.WriteLine($"[Index: 0x{tag.Index:X4}] {args[1]}.{tag.Group}");

            return true;
        }
    }
}