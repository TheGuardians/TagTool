using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache.HaloOnline;
using TagTool.Cache.Resources;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Cache.ModPackages
{
    public class ResourceCachesModPackage : ResourceCachesHaloOnlineBase
    {
        private ModPackage Package;

        private Dictionary<string, ResourcePage> ExistingResources;

        private ResourceCacheHaloOnline ResourceCache;

        private GameCacheModPackage ModCache => (GameCacheModPackage)Cache;

        public ResourceCachesModPackage(GameCacheModPackage cache, ModPackage package)
        {
            Package = package;
            Cache = cache;
            ExistingResources = new Dictionary<string, ResourcePage>();
            ResourceCache = new ResourceCacheHaloOnline(package.PackageVersion, package.PackagePlatform, package.ResourcesStream);
            Serializer = new ResourceSerializer(Cache.Version, Cache.Platform);
            Deserializer = new ResourceDeserializer(Cache.Version, Cache.Platform);
        }

        public override ResourceCacheHaloOnline GetResourceCache(ResourceLocation location)
        {
            if (location == ResourceLocation.Mods)
                return ResourceCache;
            else
                return ModCache.BaseCacheReference.ResourceCaches.GetResourceCache(location);
        }

        public override Stream OpenCacheRead(ResourceLocation location)
        {
            if (location == ResourceLocation.Mods)
                return Package.ResourcesStream;
            else
                return ModCache.BaseCacheReference.ResourceCaches.OpenCacheRead(location);
        }

        public override Stream OpenCacheReadWrite(ResourceLocation location)
        {
            if (location == ResourceLocation.Mods)
                return Package.ResourcesStream;
            else
                return ModCache.BaseCacheReference.ResourceCaches.OpenCacheReadWrite(location);
        }

        public override Stream OpenCacheWrite(ResourceLocation location)
        {
            if (location == ResourceLocation.Mods)
                return Package.ResourcesStream;
            else
                return ModCache.BaseCacheReference.ResourceCaches.OpenCacheWrite(location);
        }

        public void RebuildResourceDictionary()
        {
            throw new Exception("Not implemented");
        }
        
        public override void AddResource(PageableResource resource, Stream dataStream)
        {
            // check hash of existing resources 
            if (resource == null)
                throw new ArgumentNullException("resource");
            if (!dataStream.CanRead)
                throw new ArgumentException("The input stream is not open for reading", "dataStream");

            // change resource location
            resource.ChangeLocation(ResourceLocation.Mods);

            var dataSize = (int)(dataStream.Length - dataStream.Position);
            var data = new byte[dataSize];
            dataStream.Read(data, 0, dataSize);

            string hash;
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                hash = Convert.ToBase64String(sha1.ComputeHash(data));
            }
            // check if a perfect resource match exists, if yes reuse it to save memory in multicache packages
            if (ExistingResources.ContainsKey(hash) && ExistingResources[hash].UncompressedBlockSize == dataSize)
            {
                var existingPage = ExistingResources[hash];
                resource.Page = existingPage;
                resource.DisableChecksum();
                Debug.WriteLine("Found perfect resource match, reusing resource!");
            }
            else
            {
                ExistingResources[hash] = resource.Page;
                var cache = GetResourceCache(ResourceLocation.Mods);
                var stream = OpenCacheReadWrite(ResourceLocation.Mods);

                resource.Page.Index = cache.Add(stream, data, out uint compressedSize);
                resource.Page.CompressedBlockSize = compressedSize;
                resource.Page.UncompressedBlockSize = (uint)dataSize;
                resource.DisableChecksum();
            }
        }
        
    }
}
