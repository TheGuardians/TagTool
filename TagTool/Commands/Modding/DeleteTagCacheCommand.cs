using TagTool.Cache;
using System.Collections.Generic;
using System;

namespace TagTool.Commands.Modding
{
    class DeleteTagCacheCommand : Command
    {
        private GameCacheModPackage Cache;

        public DeleteTagCacheCommand(GameCacheModPackage modCache) :
            base(true,

                "DeleteTagCache",
                "Deletes the tag cache at the specified index.\n",

                "DeleteTagCache <index>",

                "Deletes the tag cache at the specified index.\n")
        {
            Cache = modCache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 1 || args.Count == 0)
                return false;

            int tagCacheIndex = 0;
            if (args.Count > 0)
            {
                if (!int.TryParse(args[0], System.Globalization.NumberStyles.Integer, null, out tagCacheIndex))
                    return false;
            }

            if (tagCacheIndex == Cache.GetCurrentTagCacheIndex())
            {
                Console.WriteLine("Cant delete the tag cache while it's in use");
                return true;
            }
            if (tagCacheIndex < Cache.BaseModPackage.TagCachesStreams.Count && tagCacheIndex >= 0)
            {
                Console.WriteLine($"Delete tag cache {tagCacheIndex} ({Cache.BaseModPackage.CacheNames[tagCacheIndex]}) from the package? (y/n)");
                string response = Console.ReadLine();
                if (response.ToLower().StartsWith("y"))
                {
                    Console.WriteLine($"Deleted tag cache {tagCacheIndex} ({Cache.BaseModPackage.CacheNames[tagCacheIndex]}) from package...");

                    Cache.BaseModPackage.TagCachesStreams.RemoveAt(tagCacheIndex);
                    Cache.BaseModPackage.CacheNames.RemoveAt(tagCacheIndex);
                    Cache.BaseModPackage.TagCacheNames.RemoveAt(tagCacheIndex);
                }
                else
                {
                    Console.WriteLine("Canceling delete");
                }
            }
            else
            {
                Console.WriteLine("Invalid tag cache index");
                return false;
            }

            return true;

        }
    }
}