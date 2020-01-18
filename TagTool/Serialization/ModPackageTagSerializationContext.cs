using System.IO;
using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Serialization
{
    class ModPackageTagSerializationContext : HaloOnlineSerializationContext
    {
        private ModPackage Package;

        public ModPackageTagSerializationContext(Stream stream, GameCacheContextHaloOnline context, ModPackage package, CachedTagHaloOnline tag) : base(stream, context, tag)
        {
            Package = package;
        }

        public override CachedTag GetTagByIndex(int index)
        {
            if (index < 0)
                return null;

            return Package.TagCaches[0].Tags[index];
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
