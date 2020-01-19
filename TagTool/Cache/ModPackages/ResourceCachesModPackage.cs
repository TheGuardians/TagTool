using System;
using System.Collections.Generic;
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

        public override LoadedResourceCache GetResourceCache(ResourceLocation location)
        {
            throw new NotImplementedException();
        }
    }
}
