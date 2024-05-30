using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Tags;
using TagTool.Cache.HaloOnline;
using TagTool.Cache.Gen3;

namespace TagTool.Commands.Tags
{
    public class CreateTagCommand : Command
    {
        public GameCacheHaloOnlineBase Cache { get; }

        public CreateTagCommand(GameCacheHaloOnlineBase cache)
            : base(true,

                  "CreateTag",
                  "Creates a new tag of the specified tag group in the current tag cache.",

                  "CreateTag <group tag> [index] [name]",

                  "Creates a new tag of the specified tag group in the current tag cache.")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 3)
                return new TagToolError(CommandError.ArgCount);

            var groupTagString = args[0];

            var tagNameString = "";
            var tagIndexString = "";

            switch(args.Count)
            {
                case 1:
                    break;
                case 2:
                    if (args[1].StartsWith("0x"))
                        tagIndexString = args[1];
                    else
                        tagNameString = args[1];
                    break;
                case 3:
                    if (args[1].StartsWith("0x"))
                    {
                        tagIndexString = args[1];
                        tagNameString = args[2];
                    }
                    else
                    {
                        tagNameString = args[1];
                        tagIndexString = args[2];
                    }
                    break;
            }

            if (groupTagString.Length > 4)
                return new TagToolError(CommandError.CustomError, $"Invalid group tag: {groupTagString}");

            while (groupTagString.Length < 4)
                groupTagString += " ";

            if (!Cache.TagCache.TryParseGroupTag(groupTagString, out var groupTag))
            {
                var chars = new char[] { ' ', ' ', ' ', ' ' };

                for (var i = 0; i < chars.Length; i++)
                    chars[i] = groupTagString[i];

                groupTag = new Tag(new string(chars));

                if (!Cache.TagCache.TryParseGroupTag(groupTagString, out groupTag))
                    return new TagToolError(CommandError.CustomError, $"Invalid group tag: {groupTagString}");
            }

            var tagGroup = Cache.TagCache.TagDefinitions.GetTagGroupFromTag(groupTag);

            if (args.Count >= 2 && tagNameString != "")
            {
                tagNameString = tagNameString.Split('.')[0];

                var fullName = $"{tagNameString}.{tagGroup}";
                if (!Cache.TagCache.IsTagPathValid(fullName))
                    return new TagToolError(CommandError.CustomError, $"Malformed target tag path '{tagNameString}'");
                else if (Cache.TagCache.TryGetCachedTag(fullName, out var previoustag))
                    return new TagToolError(CommandError.CustomError, $"Tag name already in use: [Index: 0x{previoustag.Index:X4}] {previoustag}");
            }

            CachedTag instance = null;

            using (var stream = Cache.OpenCacheReadWrite())
            {
                if (args.Count >= 2 && tagIndexString != "")
                {
                    var tagIndex = -1;

                    if (!Cache.TagCache.TryGetCachedTag(tagIndexString, out instance))
                    {
                        tagIndex = Convert.ToInt32(tagIndexString, 16);
                    }
                    else
                    {
                        tagIndex = instance.Index;
                    }

                    while (tagIndex >= Cache.TagCache.Count)
                        Cache.TagCache.AllocateTag(new TagGroupGen3());

                    if (tagIndex < Cache.TagCache.Count)
                    {
                        if (Cache.TagCacheGenHO.Tags[tagIndex] != null)
                        {
                            var oldInstance = Cache.TagCacheGenHO.Tags[tagIndex];
                            Cache.TagCacheGenHO.Tags[tagIndex] = null;
                            Cache.TagCacheGenHO.SetTagDataRaw(stream, oldInstance, new byte[] { });
                        }

                        instance = Cache.TagCache.CreateCachedTag(tagIndex, tagGroup);
                        Cache.TagCacheGenHO.Tags[tagIndex] = (CachedTagHaloOnline)instance;
                    }
                }

                if (instance == null)
                    instance = Cache.TagCache.AllocateTag(tagGroup);


                if (!string.IsNullOrWhiteSpace(tagNameString))
                    instance.Name = tagNameString;
                else
                    instance.Name = $"0x{instance.Index:X4}";

                Cache.SaveTagNames();
                Cache.Serialize(stream, instance, Activator.CreateInstance(Cache.TagCache.TagDefinitions.GetTagDefinitionType(groupTag)));
            }

            Console.WriteLine($"[Index: 0x{instance.Index:X4}] {instance}");
            return true;
        }
    }
}
