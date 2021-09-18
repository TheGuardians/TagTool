using System.IO;
using TagTool.Cache;
using TagTool.Tags;
using TagTool.Cache.HaloOnline;

namespace TagTool.Serialization
{
    class ModPackageTagSerializationContext : HaloOnlineSerializationContext
    {
        private TagCacheHaloOnline ModPackageTagCache;

        public ModPackageTagSerializationContext(Stream stream, GameCacheHaloOnlineBase context, CachedTagHaloOnline tag) : base(stream, context, tag)
        {
            ModPackageTagCache = context.TagCacheGenHO;
        }

        public override CachedTag GetTagByIndex(int index)
        {
            if (index < 0)
                return null;

            return ModPackageTagCache.Tags[index];
        }

        public override CachedTag GetTagByName(TagGroup group, string name)
        {
            foreach(var tag in Context.TagCacheGenHO.Tags)
            {
                if (tag.Name == name && group == tag.Group)
                    return tag;
            }
            return null;
        }
    }
}
