using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;

namespace TagTool.Commands.Porting
{
    public class NameBlamTagCommand : Command
    {
        private GameCache PortingCache;

        public NameBlamTagCommand(GameCache portingCache)
            : base(true,

                "NameBlamTag",
                "Sets the name of the specified tag in the porting cache.",

                "NameBlamTag <Tag> <Name>",

                "Sets the name of the specified tag in the porting cache.")
        {
            PortingCache = portingCache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return new TagToolError(CommandError.ArgCount);

            if (!PortingCache.TagCache.TryGetCachedTag(args[0], out var tag))
                return new TagToolError(CommandError.TagInvalid);

            var newTagName = args[1];

            Console.WriteLine($"\"{tag}\" renamed to \"{newTagName}\"");

            tag.Name = newTagName;

            return true;
        }
    }
}
