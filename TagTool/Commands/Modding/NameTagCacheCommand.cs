using TagTool.Cache;
using System.Collections.Generic;
using System;

namespace TagTool.Commands.Modding
{
    class NameTagCacheCommand : Command
    {
        private GameCacheModPackage Cache;

        public NameTagCacheCommand(GameCacheModPackage modCache) :
            base(true,

                "NameTagCache",
                "Set the name of the tag cache at the specified index.\n",

                "NameTagCache <index>",

                "Set the name of the tag cache at the specified index.\n")
        {
            Cache = modCache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            int tagCacheIndex = 0;
            if (!int.TryParse(args[0], System.Globalization.NumberStyles.Integer, null, out tagCacheIndex))
                return false;

            if (tagCacheIndex < Cache.BaseModPackage.TagCachesStreams.Count && tagCacheIndex >= 0)
            {
                var oldName = Cache.BaseModPackage.CacheNames[tagCacheIndex];

                Console.WriteLine($"Enter the name for tag cache {tagCacheIndex} (32 chars max):");
                string name = Console.ReadLine().Trim();
                name = name.Length <= 32 ? name : name.Substring(0, 32);

                Cache.BaseModPackage.CacheNames[tagCacheIndex] = name;

                Console.WriteLine($"Tag cache {tagCacheIndex} has been renamed from {oldName} to {name}");
                return true;
            }
            else
            {
                Console.WriteLine("Invalid tag cache index");
                return false;
            }
        }
    }
}