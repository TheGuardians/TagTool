using BlamCore.Cache;
using BlamCore.Commands;
using System.Collections.Generic;

namespace TagTool.Tags
{
    class SaveTagNamesCommand : Command
    {
        public GameCacheContext CacheContext { get; }

        public SaveTagNamesCommand(GameCacheContext cacheContext) :
            base(CommandFlags.Inherit,

                "SaveTagNames",
                "Saves the current tag names to the specified csv file.",

                "SaveTagNames [csv path]",

                "Saves the current tag names to the specified csv file.")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 1)
                return false;

            CacheContext.SaveTagNames(args.Count == 1 ? args[0] : null);

            return true;
        }
    }
}