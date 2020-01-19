using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;

namespace TagTool.Cache.HaloOnline
{
    class ResourceCachesHaloOnline : ResourceCachesHaloOnlineBase
    {
        public DirectoryInfo Directory;

        public Dictionary<ResourceLocation, string> ResourceCacheNames { get; } = new Dictionary<ResourceLocation, string>()
        {
            { ResourceLocation.Resources, "resources.dat" },
            { ResourceLocation.Textures, "textures.dat" },
            { ResourceLocation.TexturesB, "textures_b.dat" },
            { ResourceLocation.Audio, "audio.dat" },
            { ResourceLocation.ResourcesB, "resources_b.dat" },
            { ResourceLocation.RenderModels, "render_models.dat" },
            { ResourceLocation.Lightmaps, "lightmaps.dat" },
            { ResourceLocation.Mods, "mods.dat" }
        };

        private Dictionary<ResourceLocation, LoadedResourceCache> LoadedResourceCaches { get; } = new Dictionary<ResourceLocation, LoadedResourceCache>();

        public ResourceCachesHaloOnline(GameCacheHaloOnlineBase cache)
        {
            Cache = cache;
            Directory = Cache.Directory;
        }

        public override LoadedResourceCache GetResourceCache(ResourceLocation location)
        {
            if (!LoadedResourceCaches.TryGetValue(location, out LoadedResourceCache cache))
            {
                ResourceCacheHaloOnline resourceCache;

                var file = new FileInfo(Path.Combine(Directory.FullName, ResourceCacheNames[location]));

                using (var stream = file.OpenRead())
                {
                    resourceCache = new ResourceCacheHaloOnline(Cache.Version, stream);
                }

                cache = new LoadedResourceCache
                {
                    File = file,
                    Cache = resourceCache
                };
                LoadedResourceCaches[location] = cache;
            }
            return cache;
        }
    }
}
