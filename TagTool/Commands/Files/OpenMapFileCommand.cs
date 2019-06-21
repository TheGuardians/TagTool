using TagTool.Cache;
using TagTool.IO;
using System.Collections.Generic;
using System.IO;

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

            /*
            MapFile map;

            using(var stream = new FileStream(path, FileMode.Open))
            using(var reader = new EndianReader(stream))
            {
                map = new MapFile(reader);
            }
            */
            
            IGameCacheContext map;
            map = new GameCacheContextNew(file);
            

            return true;
        }
    }
}

