using TagTool.Cache;
using TagTool.Commands;
using System.Collections.Generic;

namespace TagTool.Commands.Tags
{
    class SaveTagNamesCommand : Command
    {
        public HaloOnlineCacheContext CacheContext { get; }

        public SaveTagNamesCommand(HaloOnlineCacheContext cacheContext) :
            base(true,

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