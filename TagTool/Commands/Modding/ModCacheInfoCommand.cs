using TagTool.Cache;
using TagTool.Commands.Common;
using System.Collections.Generic;
using System;

namespace TagTool.Commands.Modding
{
    class ModCacheInfoCommand : Command
    {
        public GameCacheModPackage Cache { get; }

        public ModCacheInfoCommand(GameCacheModPackage cache) :
            base(true,

                "ModCacheInfo",
                "Display information about the currently active mod cache",

                "ModCacheInfo ",

                "Display information about the currently active mod cache")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return new TagToolError(CommandError.ArgCount);

            Console.WriteLine($"Cache contains {Cache.BaseModPackage.GetTagCacheCount()} tag caches and {Cache.BaseModPackage.MapFileStreams.Count} map files.");

            if(Cache.BaseModPackage.CampaignFileStream != null && Cache.BaseModPackage.CampaignFileStream.Length > 0)
                Console.WriteLine($"Cache contains a campaign file");

            Console.WriteLine($"Current tag cache: {Cache.DisplayName} index: {Cache.GetCurrentTagCacheIndex()}");

            return true;
        }
    }
}