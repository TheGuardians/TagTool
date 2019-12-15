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
        private ModPackageExtended Package;

        public ModPackageTagSerializationContext(Stream stream, HaloOnlineCacheContext context, ModPackageExtended package, CachedTagInstance tag) : base(stream, context, tag)
        {
            Package = package;
        }

        public override CachedTagInstance GetTagByIndex(int index)
        {
            if (index < 0 || index > Package.Tags.Index.Count || Package.Tags.Index[index] == null)
                return null;

            var tag = Package.Tags.Index[index];

            // check if the tag is empty, meaning it is a reference to tag in the base cache
            if(tag.DefinitionOffset == tag.TotalSize)
            {

                CachedTagInstance baseTag = null;
                if(index < Context.TagCache.Index.Count)
                    baseTag = Context.GetTag(index);

                // mod tag has a name, first check if baseTag name is null, else if the names don't match or group don't match
                if ( baseTag != null && baseTag.Name != null && baseTag.Name == tag.Name && baseTag.Group == tag.Group)
                    return baseTag;
                else
                {
                    // tag name/group doesn't match base tag, try to look for it
                    foreach (var cacheTag in Context.TagCache.Index)
                    {
                        if (cacheTag.Group == tag.Group && cacheTag.Name == tag.Name)
                            return cacheTag;
                    }
                    // Failed to find tag in base cache
                    Console.Error.WriteLine($"Failed to find {tag.Name}.{tag.Group.ToString()} in the base cache, returning null tag reference.");
                    return null;
                }
                    
            }
            else
                return tag;            
        }

        public override CachedTagInstance GetTagByName(TagGroup group, string name)
        {
            foreach(var tag in Context.TagCache.Index)
            {
                if (tag.Name == name && group == tag.Group)
                    return tag;
            }
            return null;
        }
    }
}
