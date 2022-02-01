using TagTool.Common;

namespace TagTool.Cache.Monolithic
{
    public interface ISingleTagFilePersistContext
    {
        StringId AddStringId(string stringId);

        void AddTagResource(DatumHandle resourceHandle, TagResourceXSyncState xsyncState);

        CachedTag GetTag(Tag groupTag, string name);
    }
}
