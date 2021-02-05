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
                  
                  "DuplicateTag",
                  "Copies a tag's data into a new tag and optionally names it.",
                  
                  "DuplicateTag <tag> <desired name>",
                  "Copies a tag's data into a new tag and optionally names it. Tag names are saved by this command.")
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
            if (args.Count == 2)
                name = args[1];

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

            if (args.Count == 2)
                Console.WriteLine($"[Index: 0x{newTag.Index:X4}] {args[1]}.{newTag.Group}");
            else
                Console.Write($"Tag duplicated to 0x{newTag.Index.ToString("X")}\n");

            return true;
        }
    }
}
