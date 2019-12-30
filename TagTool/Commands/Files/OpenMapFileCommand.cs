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
                path = @"C:\Users\Tiger\Desktop\halo online\maps\haloonline\guardian.map";
            var file = new FileInfo(path);

            GameCache cache = GameCache.Open(file);
            using(var stream = cache.TagCache.OpenTagCacheRead())
            {
                foreach (var tag in cache.TagCache.TagTable)
                {
                    
                    if (tag.Group.Tag == "bitm")
                    {
                        var def = cache.Deserialize<Bitmap>(stream, tag);
                        foreach (var res in def.Resources)
                        {
                            var resource = cache.ResourceCache.GetBitmapTextureInteropResource(res);
                        }
                        foreach (var res in def.InterleavedResources)
                        {
                            var interleavedResource = cache.ResourceCache.GetBitmapTextureInterleavedInteropResource(res);
                        }
                            
                    }
                    /*
                    if (tag.Group.Tag == "jmad")
                    {
                        var def = cache.Deserialize<ModelAnimationGraph>(stream, tag);
                        foreach (var res in def.ResourceGroups)
                        {
                            var resource = cache.ResourceCache.GetModelAnimationTagResource(res.ResourceReference);
                        }
                    }

                    if(tag.Group.Tag == "mode")
                    {
                        var def = cache.Deserialize<RenderModel>(stream, tag);
                        var resource = cache.ResourceCache.GetRenderGeometryApiResourceDefinition(def.Geometry.Resource);
                    }
                    
                    if(tag.Group.Tag == "snd!")
                    {
                        var def = cache.Deserialize<Sound>(stream, tag);
                        var resource = cache.ResourceCache.GetSoundResourceDefinition(def.Resource);
                    }
                    
                    */
                    if (tag.Group.Tag == "sbsp")
                    {
                        var def = cache.Deserialize<ScenarioStructureBsp>(stream, tag);

                        var geo1Resource = cache.ResourceCache.GetRenderGeometryApiResourceDefinition(def.Geometry.Resource);
                        var geo2Resource = cache.ResourceCache.GetRenderGeometryApiResourceDefinition(def.Geometry2.Resource);
                        var collisionResource = cache.ResourceCache.GetStructureBspTagResources(def.CollisionBspResource);
                        var pathfindingResource = cache.ResourceCache.GetStructureBspCacheFileTagResources(def.PathfindingResource);
                    }
                }
            }
            return true;
        }
    }
}

