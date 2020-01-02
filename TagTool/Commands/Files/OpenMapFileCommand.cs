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
                    // TODO: gut tagtool and replace context system and fix all commands (end me)

                    /*
                    if (tag.Group.Tag == "bitm")
                    {
                        var def = cache.Deserialize<Bitmap>(stream, tag);
                        foreach (var res in def.Resources)
                        {
                            var resource = cache.ResourceCache.GetBitmapTextureInteropResource(res);
                            var newRes = cache.ResourceCache.CreateBitmapResource(resource);
                            var testResource = cache.ResourceCache.GetBitmapTextureInteropResource(newRes);
                        }
                    }
                    

                    
                    if (tag.Group.Tag == "jmad")
                    {
                        var def = cache.Deserialize<ModelAnimationGraph>(stream, tag);
                        foreach (var res in def.ResourceGroups)
                        {
                            var resource = cache.ResourceCache.GetModelAnimationTagResource(res.ResourceReference);
                            var newRes = cache.ResourceCache.CreateModelAnimationGraphResource(resource);
                            var testResource = cache.ResourceCache.GetModelAnimationTagResource(newRes);
                        }
                    }

                    

                    
                    if(tag.Group.Tag == "mode")
                    {
                        var def = cache.Deserialize<RenderModel>(stream, tag);
                        var res = def.Geometry.Resource;
                        var resource = cache.ResourceCache.GetRenderGeometryApiResourceDefinition(res);
                        var newRes = cache.ResourceCache.CreateRenderGeometryApiResource(resource);
                        var testResource = cache.ResourceCache.GetRenderGeometryApiResourceDefinition(newRes);
                    }
                    
                    
                    
                    
                    if(tag.Group.Tag == "snd!")
                    {
                        var def = cache.Deserialize<Sound>(stream, tag);
                        var resource = cache.ResourceCache.GetSoundResourceDefinition(def.Resource);
                        var newRes = cache.ResourceCache.CreateSoundResource(resource);
                        var testResource = cache.ResourceCache.GetSoundResourceDefinition(newRes);
                    }

                    
                    */
                    if (tag.Group.Tag == "sbsp")
                    {
                        var def = cache.Deserialize<ScenarioStructureBsp>(stream, tag);

                        var res = def.CollisionBspResource;
                        var collisionResoruce = cache.ResourceCache.GetStructureBspTagResources(res);
                        var newRes = cache.ResourceCache.CreateStructureBspResource(collisionResoruce);

                        var originalDef = new FileInfo(@"C:\Users\Tiger\Desktop\hodef.txt");
                        var newDef = new FileInfo(@"C:\Users\Tiger\Desktop\mydef.txt");

                        using (var fileStream1 = originalDef.Create())
                        using (var fileStream2 = newDef.Create())
                        {
                            var hodefdata = res.HaloOnlinePageableResource.Resource.DefinitionData;
                            var mydefdata = newRes.HaloOnlinePageableResource.Resource.DefinitionData;
                            fileStream1.Write(hodefdata, 0, hodefdata.Length);
                            fileStream2.Write(mydefdata, 0, mydefdata.Length);
                        }

                        for (int i = 0; i < newRes.HaloOnlinePageableResource.Resource.ResourceFixups.Count; i++)
                        {
                            var newResFixup = newRes.HaloOnlinePageableResource.Resource.ResourceFixups[i];
                            var oldresFixup = res.HaloOnlinePageableResource.Resource.ResourceFixups[i];
                            if( newResFixup.BlockOffset != oldresFixup.BlockOffset)
                            {
                                var test = -1;
                            }
                        }

                        var testResource = cache.ResourceCache.GetStructureBspTagResources(newRes);

                        //var pathfindingResource = cache.ResourceCache.GetStructureBspCacheFileTagResources(def.PathfindingResource);
                        
                    }
                    
                    
                }
            }

            return true;
        }
    }
}

