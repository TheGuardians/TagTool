using TagTool.Common;

namespace TagTool.Cache.Monolithic
{
    public interface ISingleTagFilePersistContext
    {
        StringId AddStringId(string stringId);

        void AddTagResource(DatumHandle resourceHandle, TagResourceXSyncState xsyncState);

        void AddTagResourceData(byte[] data);

        CachedTag GetTag(Tag groupTag, string name);
    }
}
