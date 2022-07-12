using System.Collections.Generic;
using TagTool.Common;

namespace TagTool.Cache.Monolithic
{
    public class MonolithicTagPersistContext : ISingleTagFilePersistContext
    {
        public GameCacheMonolithic Cache;
        public Dictionary<DatumHandle, TagResourceXSyncState> TagResources;

        public MonolithicTagPersistContext(GameCacheMonolithic cache)
        {
            Cache = cache;
            TagResources = new Dictionary<DatumHandle, TagResourceXSyncState>();
        }

        public void AddTagResource(DatumHandle resourceHandle, TagResourceXSyncState state)
        {
            TagResources.Add(resourceHandle, state);
        }
        public void AddTagResourceData(byte[] data)
        {

        }

        public StringId AddStringId(string stringvalue)
        {
            var stringId = Cache.StringTableMono.GetStringId(stringvalue);
            if (stringId == StringId.Invalid)
                stringId = Cache.StringTableMono.AddString(stringvalue);
            return stringId;
        }

        public CachedTag GetTag(Tag groupTag, string name)
        {
            if (Cache.TagCache.TryGetCachedTag($"{name}.{groupTag}", out CachedTag tag))
                return tag;
            return null;
        }
    }
}
