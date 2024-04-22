using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;

namespace TagTool.Commands.Porting
{
    class IgnoreBlamTagCommand : Command
    {
        private GameCache PortingCache;

        public IgnoreBlamTagCommand(GameCache portingCache)
               : base(true,

                     "IgnoreBlamTag",
                     "Prevents the specified tags from being replaced when porting tags.",

                     "IgnoreBlamTag <tag name>",

                     "Prevents the specified tag from being ported.\n" +
                     "")
        {
            UserDefinedIgnoredBlamTagsIndicies = new List<int>();
            PortingCache = portingCache;
        }

        public static List<int> UserDefinedIgnoredBlamTagsIndicies = new List<int>();

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return new TagToolError(CommandError.ArgCount);
            
            if (PortingCache.TagCache.TryGetCachedTag(args[0], out var tagInstance))
            {
                UserDefinedIgnoredBlamTagsIndicies.Add(tagInstance.Index);
                return true;
            }else
                return new TagToolError(CommandError.TagInvalid);
        }
    }
}

