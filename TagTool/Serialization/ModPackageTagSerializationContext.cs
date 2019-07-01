using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Serialization
{
    class ModPackageTagSerializationContext : TagSerializationContext
    {
        private ModPackageSimplified Package;

        public ModPackageTagSerializationContext(Stream stream, HaloOnlineCacheContext context, ModPackageSimplified package, CachedTagInstance tag) : base(stream, context, tag)
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

        public override CachedTagInstance GetTagByName(TagGroup group, string name)
        {
           foreach(var tag in Package.Tags.Index)
            {
                if (tag.Name == name && tag.Group == group)
                    return tag;
            }

            return null;
        }
    }
}
