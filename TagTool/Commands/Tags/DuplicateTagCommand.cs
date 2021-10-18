using System;
using System.Collections.Generic;

using TagTool.Cache;
using TagTool.Commands.Common;

namespace TagTool.Commands.Tags
{
    class DuplicateTagCommand : Command
    {
        private GameCache Cache { get; }

        public DuplicateTagCommand(GameCache cache)
            : base(false,
                  
                  name:
                  "DuplicateTag",
                  
                  description:
                  "Copies a tag's data into a new tag and optionally names it.",
                  
                  usage:
                  "DuplicateTag <tag> <desired name>",
                  
                  examples:
                  "DuplicateTag shaders\\invalid.shader shaders\\custom\\my_shader",

                  helpMessage:
                  "- Tag target paths\\names should contain [a-z0-9\\_] only.\n"+
                  "- Tag names are saved by this command.")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            
            if (args.Count < 1 || args.Count > 2)
                return new TagToolError(CommandError.ArgCount);
            
            if (!Cache.TagCache.TryGetCachedTag(args[0], out var originalTag))
                return new TagToolError(CommandError.TagInvalid);

            var name = "";
            if (args.Count > 1)
            {
                var split = args[1].ToLower().Split('.');
                name = split[0];

                if (split.Length > 1 && (
                      !Cache.TagCache.TryParseGroupTag(split[1], out var requiredTagGroup) || requiredTagGroup != originalTag.Group.Tag
                    ))
                    return new TagToolError(CommandError.CustomError, $"Requested tag group does not match destination.");

                if (!Cache.TagCache.IsTagPathValid($"{name}.{originalTag.Group}"))
                    return new TagToolError(CommandError.CustomError, $"Malformed target tag path '{name}'");

                if (Cache.TagCache.TagExists($"{name}.{originalTag.Group}"))
                    return new TagToolError(CommandError.CustomError, "A tag with the requested name already exists!");
            }

            var newTag = Cache.TagCache.AllocateTag(originalTag.Group, name);

            using (var stream = Cache.OpenCacheReadWrite())
            {
                var originalDefinition = Cache.Deserialize(stream, originalTag);
                Cache.Serialize(stream, newTag, originalDefinition);

                if (Cache is GameCacheHaloOnlineBase)
                {
                    var hoCache = Cache as GameCacheHaloOnlineBase;
                    hoCache.SaveTagNames();
                }
            }

            if (args.Count > 1)
                Console.WriteLine($"\n   [Index: 0x{newTag.Index:X4}] {name}.{newTag.Group}");
            else
                Console.WriteLine($"\n   Tag duplicated to 0x{newTag.Index:X}");
            return true;
        }
    }
}