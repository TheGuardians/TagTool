using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Tags;
using TagTool.Cache.HaloOnline;

namespace TagTool.Commands.Tags
{
    class CreateTagCommand : Command
    {
        public GameCacheHaloOnlineBase Cache { get; }

        public CreateTagCommand(GameCacheHaloOnlineBase cache)
            : base(true,

                  "CreateTag",
                  "Creates a new tag of the specified tag group in the current tag cache.",

                  "CreateTag <group tag> [index = *]",

                  "Creates a new tag of the specified tag group in the current tag cache.")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 2)
                return new TagToolError(CommandError.ArgCount);

            var groupTagString = args[0];

            if (groupTagString.Length > 4)
                return new TagToolError(CommandError.ArgInvalid, $"Invalid group tag: {groupTagString}");

            while (groupTagString.Length < 4)
                groupTagString += " ";

            if (!Cache.TagCache.TryParseGroupTag(groupTagString, out var groupTag))
            {
                var chars = new char[] { ' ', ' ', ' ', ' ' };

                for (var i = 0; i < chars.Length; i++)
                    chars[i] = groupTagString[i];

                groupTag = new Tag(new string(chars));
            }

            CachedTag instance = null;
            var tagGroup = Cache.TagCache.TagDefinitions.GetTagGroupFromTag(groupTag);

            using (var stream = Cache.OpenCacheReadWrite())
            {
                if (args.Count == 2)
                {
                    var tagIndex = -1;

                    if (!Cache.TagCache.TryGetCachedTag(args[1], out instance))
                    {
                        if (args[1].StartsWith("0x"))
                            tagIndex = Convert.ToInt32(args[1], 16);
                        else
                            return new TagToolError(CommandError.CustomError, "The specified tag index is invalid");
                    }
                    else
                    {
                        tagIndex = instance.Index;
                    }

                    while (tagIndex >= Cache.TagCache.Count)
                        Cache.TagCache.AllocateTag(new TagGroup());

                    if (tagIndex < Cache.TagCache.Count)
                    {
                        if (Cache.TagCacheGenHO.Tags[tagIndex] != null)
                        {
                            var oldInstance = Cache.TagCacheGenHO.Tags[tagIndex];
                            Cache.TagCacheGenHO.Tags[tagIndex] = null;
                            Cache.TagCacheGenHO.SetTagDataRaw(stream, oldInstance, new byte[] { });
                        }

                        instance = Cache.TagCache.CreateCachedTag(tagIndex, Cache.TagCache.TagDefinitions.GetTagGroupFromTag(groupTag));
                        Cache.TagCacheGenHO.Tags[tagIndex] = (CachedTagHaloOnline)instance;
                    }
                }

                if (instance == null)
                    instance = Cache.TagCache.AllocateTag(Cache.TagCache.TagDefinitions.GetTagGroupFromTag(groupTag));

                Cache.Serialize(stream, instance, Activator.CreateInstance(Cache.TagCache.TagDefinitions.GetTagDefinitionType(groupTag)));
            }

            var tagName = instance.Name ?? $"0x{instance.Index:X4}";

            Console.WriteLine($"[Index: 0x{instance.Index:X4}] {tagName}.{instance.Group}");
            return true;
        }
    }
}
