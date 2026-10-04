using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Common.Logging;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.Cache.HaloOnline
{
    public class ResourceCachesHaloOnline : ResourceCachesHaloOnlineBase
    {
        public DirectoryInfo Directory;

        public static Dictionary<ResourceLocation, string> ResourceCacheNames { get; } = new Dictionary<ResourceLocation, string>()
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

        /// <summary>
        /// The stock game's file name for <see cref="ResourceLocation.ResourcesB"/> (shared file type "video", page flag bit 5).
        /// ElDewrito renames it to resources_b.dat
        /// </summary>
        public const string VideoCacheName = "video.dat";

        private Dictionary<ResourceLocation, LoadedResourceCache> LoadedResourceCaches { get; } = new Dictionary<ResourceLocation, LoadedResourceCache>();


        public ResourceCachesHaloOnline(GameCacheHaloOnlineBase cache)
        {
            Cache = cache;
            Serializer = new ResourceSerializer(Cache.Version, Cache.Platform);
            Deserializer = new ResourceDeserializer(Cache.Version, Cache.Platform);
            Directory = Cache.Directory;
        }

        public override Stream OpenCacheRead(ResourceLocation location) => LoadResourceCache(location).File.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

        public override Stream OpenCacheReadWrite(ResourceLocation location) => LoadResourceCache(location).File.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

        public override Stream OpenCacheWrite(ResourceLocation location) => LoadResourceCache(location).File.OpenWrite();

        public override ResourceCacheHaloOnline GetResourceCache(ResourceLocation location)
        {
            return LoadResourceCache(location).Cache;
        }

        /// <summary>
        /// Gets the file backing a resource location. <see cref="ResourceLocation.ResourcesB"/> is resources_b.dat in ElDewrito
        /// and video.dat in stock Halo Online
        /// </summary>
        public FileInfo GetResourceCacheFile(ResourceLocation location)
        {
            var name = location == ResourceLocation.ResourcesB && !CacheVersionDetection.IsEldewrito(Cache.Version) ?
                VideoCacheName : ResourceCacheNames[location];
            return new FileInfo(Path.Combine(Directory.FullName, name));
        }

        /// <summary>
        /// Use ResourcesB in ElDewrito, otherwise use Resources
        /// </summary>
        public static ResourceLocation GetOverflowResourceLocation(CacheVersion version) =>
            CacheVersionDetection.IsEldewrito(version) ? ResourceLocation.ResourcesB : ResourceLocation.Resources;

        private LoadedResourceCache LoadResourceCache(ResourceLocation location)
        {
            if (!LoadedResourceCaches.TryGetValue(location, out LoadedResourceCache cache) && location != ResourceLocation.None)
            {
                ResourceCacheHaloOnline resourceCache;

                var file = GetResourceCacheFile(location);
                Stream stream;

                try
                {
                    stream = file.Open(FileMode.OpenOrCreate);
                }
                catch (Exception ex)
                {
                    if (ex is IOException && ex.Message.EndsWith("used by another process."))
                    {
                        Log.Warning($"Another process using {file.Name}: opening read-only stream");
                        stream = file.OpenRead();
                    }
                    else
                        throw;
                }

                using (stream)
                {
                    resourceCache = new ResourceCacheHaloOnline(Cache.Version, Cache.Platform, stream);
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
