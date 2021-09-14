using System.Collections.Generic;

using TagTool.Cache;
using TagTool.Commands.Common;

namespace TagTool.Commands.Modding
{
    class SwitchTagCacheCommand : Command
    {
        public GameCacheModPackage Cache { get; }

        public SwitchTagCacheCommand(GameCacheModPackage cache) :
            base(true,

                "SwitchTagCache",
                "Change the the current tag cache to the specified index.",

                "SwitchTagCache [index]",

                "If no cache index is specified, a list will be presented to choose from.")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            int tagCacheIndex;

            if (args.Count > 1)
                return new TagToolError(CommandError.ArgCount);
            
            else if (args.Count == 0)
                tagCacheIndex = new TagToolChoicePrompt(Cache.BaseModPackage.CacheNames, indent: 3).Prompt();             

            else if(!int.TryParse(args[0], System.Globalization.NumberStyles.Integer, null, out tagCacheIndex))
                return new TagToolError(CommandError.ArgInvalid, $"\"{args[0]}\"");              


            System.Console.WriteLine();
            Cache.SetActiveTagCache(tagCacheIndex);

            return true;
        }
    }
}