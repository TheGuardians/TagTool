using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands;

namespace TagTool.Commands.Tags
{
    class NameTagCommand : Command
    {
        public HaloOnlineCacheContext CacheContext { get; }

        public NameTagCommand(HaloOnlineCacheContext cacheContext)
            : base(true,
                  
                  "NameTag",
                  "Sets the name of a tag file in the current cache.",
                  
                  "NameTag <tag> <name> [csv path]",
                  
                  "<tag>  - A valid tag index, tag name, or * for the last tag in the current cache. \n" +
                  "\n" +
                  "<name> - The name of the tag. Should be a concise name that resembles the format \n" +
                  "         of existing tag names.\n")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 3)
                return false;

            var tag = ArgumentParser.ParseTagSpecifier(CacheContext, args[0]);

            if (tag == null)
            {
                Console.WriteLine($"ERROR: Invalid tag specifier: {args[0]}");
                return false;
            }

            if (args.Count < 2)
            {
                CacheContext.TagNames.Remove(tag.Index);
                return true;
            }

            CacheContext.TagNames[tag.Index] = args[1];

            Console.WriteLine($"[Index: 0x{tag.Index:X4}, Offset: 0x{tag.HeaderOffset:X8}, Size: 0x{tag.TotalSize:X4}] {args[1]}.{CacheContext.GetString(tag.Group.Name)}");

            return true;
        }
    }
}