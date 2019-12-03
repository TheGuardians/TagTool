using TagTool.Cache;
using System.Collections.Generic;
using System;

namespace TagTool.Commands.Tags
{
    class SaveModdedTagsCommand : Command
    {
        public HaloOnlineCacheContext CacheContext { get; }

        public SaveModdedTagsCommand(HaloOnlineCacheContext cacheContext) :
            base(true,

                "SaveModdedTags",
                "Saves the name of the modified tags from the current session.",

                "SaveModdedTags [csv path]",

                "Saves the name of the modified tags from the current session.")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 1)
                return false;

            CacheContext.SaveModifiedTagNames(args.Count == 1 ? args[0] : null);
            Console.WriteLine("Done!");
            return true;
        }
    }
}