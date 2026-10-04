using TagTool.Tags;

namespace TagTool.Cache.Resources
{
    [TagStructure(Size = 0x90, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x74, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline449175)]
    [TagStructure(Size = 0x90, MinVersion = CacheVersion.HaloOnline454665, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0xAC, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0xB8, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.MCC)]
    public class DebugZoneManifest : TagStructure
    {
        public ZoneManifest CacheZoneManifest;

        public int DiskSize;
        public int UnusedSize;
        public CachedTag OwnerTag;
    }
}
