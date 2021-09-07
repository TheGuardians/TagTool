using System.IO;
using TagTool.Cache;
using TagTool.Tags;
using TagTool.Cache.HaloOnline;

namespace TagTool.Serialization
{
    class ModPackageTagSerializationContext : HaloOnlineSerializationContext
    {
        private GameCacheModPackage ModCache;

        public ModPackageTagSerializationContext(Stream stream, GameCacheModPackage modCache, GameCacheHaloOnlineBase context, CachedTagHaloOnline tag) : base(stream, context, tag)
        {
            ModCache = modCache;
        }

        public override CachedTag GetTagByIndex(int index)
        {
            if (index < 0)
                return null;

            var tag = ModCache.TagCacheGenHO.Tags[index];
            if (tag == null || tag.IsEmpty())
                return ModCache.BaseCacheReference.TagCacheGenHO.Tags[index];

            return tag;
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
