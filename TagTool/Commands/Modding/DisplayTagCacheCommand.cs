using TagTool.Cache;
using System.Collections.Generic;
using System;

namespace TagTool.Commands.Modding
{
    class DisplayTagCache : Command
    {
        public GameCacheModPackage Cache { get; }

        public DisplayTagCache(GameCacheModPackage cache) :
            base(true,

                "DisplayTagCache",
                "Display name/index of current active tag cache",

                "DisplayTagCache ",

                "Display name/index of current active tag cache")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;

            Console.WriteLine($"Current tag cache: {Cache.DisplayName} index: {Cache.GetCurrentTagCacheIndex()}");

            return true;
        }
    }
}