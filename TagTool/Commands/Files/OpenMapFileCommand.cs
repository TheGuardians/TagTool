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
                path = @"C:\Users\Tiger\Desktop\halo online\maps\halo3\010_jungle.map";
            var file = new FileInfo(path);

            GameCache cache = GameCache.Open(file);
            using(var stream = cache.TagCache.OpenTagCacheRead())
            {
                foreach (var tag in cache.TagCache.TagTable)
                {
                    // TODO: add resource size calculation for sbsp
                    // TODO: find proper way to deserialize the resource definitions using dual streams for both definition data and resources

                    if (tag.Group.Tag == "bitm")
                    {
                        var def = cache.Deserialize<Bitmap>(stream, tag);
                        foreach (var res in def.Resources)
                        {
                            var resource = cache.ResourceCache.GetBitmapTextureInteropResource(res);
                        }  
                    }
                    
                    if (tag.Group.Tag == "jmad")
                    {
                        var def = cache.Deserialize<ModelAnimationGraph>(stream, tag);
                        foreach (var res in def.ResourceGroups)
                        {
                            var resource = cache.ResourceCache.GetModelAnimationTagResource(res.ResourceReference);
                        }
                    }

                    /*
                    if (tag.Group.Tag == "sbsp")
                    {
                        var def = cache.Deserialize<ScenarioStructureBsp>(stream, tag);

                        var resourceDef = cache.ResourceCache.GetStructureBspTagResources(def.CollisionBspResource);
                        var data = cache.ResourceCache.GetResourceData(def.CollisionBspResource);

                    }*/
                }
            }
            return true;
        }
    }
}

