using TagTool.Cache;
using TagTool.Commands.Common;
using System.Collections.Generic;
using System;

namespace TagTool.Commands.Modding
{
    public class DeleteTagCacheCommand : Command
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
            if (args.Count != 1)
                return new TagToolError(CommandError.ArgCount);

            int tagCacheIndex;
            if (!int.TryParse(args[0], System.Globalization.NumberStyles.Integer, null, out tagCacheIndex))
                return new TagToolError(CommandError.ArgInvalid, $"\"{args[0]}\"");

            if (tagCacheIndex == Cache.GetCurrentTagCacheIndex())
                return new TagToolError(CommandError.CustomMessage, "A tag cache cannot be deleted while it is in use");

            if (tagCacheIndex < Cache.BaseModPackage.TagCachesStreams.Count && tagCacheIndex >= 0)
            {
                Console.WriteLine($"Delete tag cache {tagCacheIndex} ({Cache.BaseModPackage.CacheNames[tagCacheIndex]}) from the package? (y/n)");

                string response = Console.ReadLine();

                if (response.ToLower().StartsWith("y"))
                {
                    string cacheName = Cache.BaseModPackage.CacheNames[tagCacheIndex];
                    Cache.BaseModPackage.TagCachesStreams.RemoveAt(tagCacheIndex);
                    Cache.BaseModPackage.CacheNames.RemoveAt(tagCacheIndex);
                    Cache.BaseModPackage.TagCacheNames.RemoveAt(tagCacheIndex);
                    Console.WriteLine($"Deleted tag cache {tagCacheIndex} ({cacheName}) from package...");
                    return true;
                }
                else
                {
                    Console.WriteLine("Canceling delete");
                }
            }
            else
            {
                return new TagToolError(CommandError.ArgInvalid, $"No tag cache exists at index {tagCacheIndex}");
            }

            return true;

        }
    }
}
