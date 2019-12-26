using TagTool.Cache;
using TagTool.IO;
using System.Collections.Generic;
using System.IO;
using System;
using TagTool.Common;
using TagTool.Tags.Definitions;
using TagTool.Tags;
using TagTool.Serialization;
using TagTool.Bitmaps;
using TagTool.Tags.Resources;

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
                path = @"C:\Users\Tiger\Desktop\halo online\maps\odst\sc100.map";
            var file = new FileInfo(path);

            GameCache cache = GameCache.Open(file);

            var tag = cache.TagCache.GetTagByName("there they are all standing in a row", "zone");

            using (var cacheStream = cache.TagCache.OpenTagCacheRead())
            {
                var zone = cache.Deserialize<CacheFileResourceGestalt>(cacheStream, tag);
                var play = cache.Deserialize<CacheFileResourceLayoutTable>(cacheStream, cache.TagCache.GetTagByID(0x0001));

                for(int i = 0; i < zone.TagResources.Count; i++)
                {
                    var tagResource = zone.TagResources[i];
                    if (tagResource.ResourceTypeIndex == -1)
                        continue;

                    var tempRef = new TagResourceReference();
                    tempRef.Gen3ResourceID = new DatumIndex(0, (ushort)i);
                    byte[] data = cache.ResourceCache.GetResourceData(tempRef);
                    // TODO: test ODST 
                }
                return true;
            }
        }
    }
}

