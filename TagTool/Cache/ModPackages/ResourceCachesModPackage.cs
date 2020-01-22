using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache.HaloOnline;
using TagTool.Common;

namespace TagTool.Cache.ModPackages
{
    public class ResourceCachesModPackage : ResourceCachesHaloOnlineBase
    {
        private ModPackage Package;

        public ResourceCachesModPackage(ModPackage package)
        {
            Package = package;
        }

        public override ResourceCacheHaloOnline GetResourceCache(ResourceLocation location) => Package.Resources;

        public override Stream OpenCacheRead(ResourceLocation location) =>  Package.ResourcesStream;

        public override Stream OpenCacheReadWrite(ResourceLocation location) => Package.ResourcesStream;

        public override Stream OpenCacheWrite(ResourceLocation location) => Package.ResourcesStream;
    }
}
