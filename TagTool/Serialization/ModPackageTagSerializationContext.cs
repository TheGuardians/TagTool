using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;

namespace TagTool.Serialization
{
    class ModPackageTagSerializationContext : TagSerializationContext
    {
        private ModPackage Package;

        public ModPackageTagSerializationContext(Stream stream, HaloOnlineCacheContext context, ModPackage package, CachedTagInstance tag) : base(stream, context, tag)
        {
            Package = package;
        }

        public override CachedTagInstance GetTagByIndex(int index)
        {
            if (index < 0)
                return null;
            else if(Package.Tags.Index[index] == null)
                return (index < Context.TagCache.Index.Count) ? Context.TagCache.Index[index] : null;
            else
                return Package.Tags.Index[index];
        }
    }
}
