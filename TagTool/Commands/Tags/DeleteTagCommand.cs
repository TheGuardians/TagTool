using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using System.Linq;

namespace TagTool.Commands.Tags
{
    class DeleteTagCommand : Command
    {
        public GameCache Cache { get; }

        public DeleteTagCommand(GameCache cache)
            : base(false,

                  "DeleteTag",
                  "Nulls and removes a tag from the cache.",

                  "DeleteTag <tag>",
                  "Nulls and removes a tag from the cache.")
        {
            Cache = cache;
        }
        
        public override object Execute(List<string> args)
        {
            if (args.Count != 1 || !Cache.TagCache.TryGetCachedTag(args[0], out var tag))
                return false;

            // store these before tag is nulled
            var tagName = tag.Name ?? tag.Index.ToString("X");
            var tagGroup = tag.Group;

            using (var stream = Cache.OpenCacheReadWrite()) // TODO: implement better way of nulling tags, support gen3
            {
                if (Cache is GameCacheHaloOnlineBase)
                {
                    var cacheHaloOnline = Cache as GameCacheHaloOnlineBase;

                    cacheHaloOnline.TagCacheGenHO.Tags[tag.Index] = null;

                    byte[] blankheader = Enumerable.Repeat((byte)0x00, 0x24).ToArray();
                    cacheHaloOnline.TagCacheGenHO.SetTagDataRaw(stream, (CachedTagHaloOnline)tag, blankheader);
                }

                else
                {
                    throw new NotImplementedException();
                }
            }

            Console.WriteLine($"Deleted {tagGroup} tag {tagName}");
            return true;
        }
    }
}