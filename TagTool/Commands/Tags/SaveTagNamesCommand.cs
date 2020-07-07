using TagTool.Cache;
using TagTool.Commands.Common;
using System.Collections.Generic;

namespace TagTool.Commands.Tags
{
    class SaveTagNamesCommand : Command
    {
        public GameCacheHaloOnlineBase Cache { get; }

        public SaveTagNamesCommand(GameCacheHaloOnlineBase cache) :
            base(true,

                "SaveTagNames",
                "Saves the current tag names to the specified csv file.",

                "SaveTagNames [csv path]",

                "Saves the current tag names to the specified csv file.")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 1)
                return new TagToolError(CommandError.ArgCount);

            Cache.SaveTagNames(args.Count == 1 ? args[0] : null);

            return true;
        }
    }
}