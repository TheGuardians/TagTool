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
            if (args.Count != 1)
                return false;

            var path = args[0];
            var file = new FileInfo(path);

            GameCache cache = GameCache.Open(file);
            
            var tag = cache.TagCache.GetTagByName("levels\\atlas\\sc100\\h100_shared", "sbsp");

            using (var cacheStream = cache.OpenCacheRead())
            {
                var def = cache.Deserialize(cacheStream, tag);
            }
            
            return true;

            /*
            MapFile map;

            using(var stream = new FileStream(path, FileMode.Open))
            using(var reader = new EndianReader(stream))
            {
                map = new MapFile(reader);
            }
            */
        }
    }
}

