using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.RenderModels
{
    class ListMultiMeshModelsCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }

        public ListMultiMeshModelsCommand(HaloOnlineCacheContext cacheContext) :
            base(true,
                
                "ListMultiMeshModels",
                "",
                
                "ListMultiMeshModels",
                
                "")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;

            using (var cacheStream = CacheContext.OpenTagCacheRead())
            {
                foreach (var instance in CacheContext.TagCache.Index)
                {
                    if (instance == null || !instance.IsInGroup<RenderModel>())
                        continue;

                    var definition = CacheContext.Deserialize<RenderModel>(cacheStream, instance);
                    var isMulti = false;

                    foreach (var region in definition.Regions)
                    {
                        foreach (var permutation in region.Permutations)
                        {
                            if (permutation.MeshCount > 1)
                            {
                                isMulti = true;
                                break;
                            }
                        }

                        if (isMulti)
                            break;
                    }

                    if (isMulti)
                    {
                        var tagName = instance.Name == null ?
                            $"0x{instance.Index:X4}" :
                            instance.Name;

                        Console.WriteLine($"[Index: 0x{instance.Index:X4}, Offset: 0x{instance.HeaderOffset:X8}, Size: 0x{instance.TotalSize:X4}] {tagName}.render_model");
                    }
                }
            }

            return true;
        }
    }
}
