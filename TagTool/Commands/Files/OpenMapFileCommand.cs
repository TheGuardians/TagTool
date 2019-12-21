using TagTool.Cache;
using TagTool.IO;
using System.Collections.Generic;
using System.IO;
using System;
using TagTool.Common;

namespace TagTool.Commands.Porting
{
    public class OpenMapFileCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }

        public OpenMapFileCommand(HaloOnlineCacheContext cacheContext)
            : base(false,

                  "OpenMapFile",
                  "Opens a map file.",

                  "OpenMapFile <Map File>",

                  "Opens a map file.")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 1)
                return false;
            string path = "";
            if (args.Count == 1)
                path = args[0];
            else
                path = @"C:\Users\Tiger\Desktop\halo online\maps\haloonline\tags.dat";
            var file = new FileInfo(path);

            GameCache cache = GameCache.Open(file);
            
            var tag = cache.TagCache.GetTagByName("levels\\multi\\guardian\\guardian", "sbsp");

            using (var cacheStream = cache.TagCache.OpenTagCacheRead())
            {
                var def = cache.Deserialize(cacheStream, tag);
            }
            
            return true;
        }
    }
}

