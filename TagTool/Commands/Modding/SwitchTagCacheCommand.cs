using TagTool.Cache;
using System.Collections.Generic;

namespace TagTool.Commands.Modding
{
    class SwitchTagCacheCommand : Command
    {
        public GameCacheModPackage Cache { get; }

        public SwitchTagCacheCommand(GameCacheModPackage cache) :
            base(true,

                "SwitchTagCache",
                "Change the the current tag cache to the specified index.",

                "SwitchTagCache <index>",

                "Change the the current tag cache to the specified index.")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            if (!int.TryParse(args[0],System.Globalization.NumberStyles.Integer, null, out int tagCacheIndex))
                return false;

            Cache.SetActiveTagCache(tagCacheIndex);

            return true;
        }
    }
}