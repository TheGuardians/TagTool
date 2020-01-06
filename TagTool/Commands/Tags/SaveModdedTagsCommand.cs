using TagTool.Cache;
using System.Collections.Generic;
using System;

namespace TagTool.Commands.Tags
{
    class SaveModdedTagsCommand : Command
    {
        public GameCacheContextHaloOnline Cache { get; }

        public SaveModdedTagsCommand(GameCacheContextHaloOnline cache) :
            base(true,

                "SaveModdedTags",
                "Saves the name of the modified tags from the current session.",

                "SaveModdedTags [csv path]",

                "Saves the name of the modified tags from the current session.")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 1)
                return false;

            Cache.SaveModifiedTagNames(args.Count == 1 ? args[0] : null);
            Console.WriteLine("Done!");
            return true;
        }
    }
}