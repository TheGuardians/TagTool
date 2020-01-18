using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Commands.Tags
{
    class CreateTagCommand : Command
    {
        public GameCacheHaloOnline Cache { get; }

        public CreateTagCommand(GameCacheHaloOnline cache)
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
                return false;

            begin:
            var groupTagString = args[0];

            if (groupTagString.Length > 4)
            {
                Console.WriteLine($"ERROR: Invalid group tag: {groupTagString}");
                return true;
            }

            if (!Cache.TryParseGroupTag(groupTagString, out var groupTag))
            {
                var chars = new char[] { ' ', ' ', ' ', ' ' };

                for (var i = 0; i < chars.Length; i++)
                    chars[i] = groupTagString[i];

                groupTag = new Tag(new string(chars));
            }

            if (!TagGroup.Instances.ContainsKey(groupTag))
            {
                Console.WriteLine($"ERROR: No tag group definition for group tag '{groupTag}'!");
                Console.Write($"(BE CAREFUL WITH THIS!!!) Define '{groupTag}' tag group? [y/n]: ");

                var answer = Console.ReadLine().ToLower();

                if (answer != "y" && answer != "yes")
                    return true;

                Console.WriteLine("Enter the tag group specification in the following format");
                Console.WriteLine("<group tag> [parent group tag] [grandparent group tag] <group name>:");
                Console.WriteLine();
                Console.Write($"{groupTag} specification> ");

                answer = Console.ReadLine();

                var groupArgs = ArgumentParser.ParseCommand(answer, out string redirect);

                switch (groupArgs.Count)
                {
                    case 2: new TagGroup(new Tag(groupArgs[0]), Tag.Null, Tag.Null, Cache.StringTable.GetStringId(groupArgs[1])); break;
                    case 3: new TagGroup(new Tag(groupArgs[0]), new Tag(groupArgs[1]), Tag.Null, Cache.StringTable.GetStringId(groupArgs[2])); break;
                    case 4: new TagGroup(new Tag(groupArgs[0]), new Tag(groupArgs[1]), new Tag(groupArgs[2]), Cache.StringTable.GetStringId(groupArgs[3])); break;
                    default: return false;
                }

                goto begin;
            }

            CachedTag instance = null;
            TagGroup.Instances.TryGetValue(groupTag, out var tagGroup);

            using (var stream = Cache.OpenCacheReadWrite())
            {
                if (args.Count == 2)
                {
                    var tagIndex = -1;

                    if (!Cache.TryGetCachedTag(args[1], out instance))
                    {
                        if (args[1].StartsWith("0x"))
                            tagIndex = Convert.ToInt32(args[1], 16);
                        else
                            return false;
                    }
                    else
                    {
                        tagIndex = instance.Index;
                    }

                    while (tagIndex >= Cache.TagCache.Count)
                        Cache.TagCache.AllocateTag(TagGroup.None);

                    if (tagIndex < Cache.TagCache.Count)
                    {
                        if (Cache.TagCacheGenHO.Tags[tagIndex] != null)
                        {
                            var oldInstance = Cache.TagCacheGenHO.Tags[tagIndex];
                            Cache.TagCacheGenHO.Tags[tagIndex] = null;
                            Cache.TagCacheGenHO.SetTagDataRaw(stream, oldInstance, new byte[] { });
                        }

                        instance = Cache.TagCache.CreateCachedTag(tagIndex, TagGroup.Instances[groupTag]);
                        Cache.TagCacheGenHO.Tags[tagIndex] = (CachedTagHaloOnline)instance;
                    }
                }

                if (instance == null)
                    instance = Cache.TagCache.AllocateTag(TagGroup.Instances[groupTag]);

                Cache.Serialize(stream, instance, Activator.CreateInstance(TagDefinition.Find(groupTag)));
            }

            var tagName = instance.Name ?? $"0x{instance.Index:X4}";

            Console.WriteLine($"[Index: 0x{instance.Index:X4}] {tagName}.{Cache.StringTable.GetString(instance.Group.Name)}");
            return true;
        }
    }
}
