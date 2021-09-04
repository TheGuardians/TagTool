using System;
using System.Linq;
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
                  
                  name:
                  "NameTag",

                  description:
                  "Sets the name of a tag file in the current cache.",
                  
                  usage:
                  "NameTag <tag> <name> [noSave] [csv path]",

                  examples:
                  @"nametag shaders\invalid.shader my\path\tag_name d:\csv\path.csv",

                  helpMessage:
                  "- Accepts valid tag index/name, or * for the last tag in the current cache.\n" +
                  "- If <name> is specified as !, the tag name will be emptied and become unnamed.\n" +
                  "- Names are saved by default, but can be overridden using the noSave keyword.\n" +
                  "- Names are saved to the default csv, but can be overridden with a full path.\n" +
                  "- Try to keep names descriptive and follow the format of existing tag names.\n")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 2 || args.Count > 4)
                return new TagToolError(CommandError.ArgCount);

            if (!Cache.TagCache.TryGetCachedTag(args[0], out var tag))
                return new TagToolError(CommandError.TagInvalid);


            // Name the tag
            tag.Name = (args[1] == "!") ? "" : args[1].Split('.')[0];


            var saveTagNames = !args.Contains("nosave", StringComparer.CurrentCultureIgnoreCase);
            var passArgs = new List<string>();

            if (args.Count > 2 && args[2].ToLower() != "nosave")
                passArgs = new List<string>() { args[2] };
            if (args.Count > 3 && args[3].ToLower() != "nosave")
                passArgs = new List<string>() { args[3] };

            if (saveTagNames && Cache is GameCacheHaloOnlineBase)
                new SaveTagNamesCommand(Cache as GameCacheHaloOnlineBase).Execute(passArgs);
            

            Console.WriteLine($"[Index: 0x{tag.Index:X4}] {tag.Name}.{tag.Group}");

            return true;
        }
    }
}