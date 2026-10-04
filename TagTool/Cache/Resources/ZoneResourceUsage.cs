using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache.Resources
{
    [TagStructure(Size = 0x14, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x18, MinVersion = CacheVersion.HaloReach)]
    [TagStructure(Size = 0x10, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline449175)]
    [TagStructure(Size = 0xC, MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
    public class ZoneResourceUsage : TagStructure
    {
        [TagField(Flags = TagFieldFlags.Label, MinVersion = CacheVersion.HaloReach)]
        public StringId Name;

        public uint RequiredPageableSize;
        public uint DeferredRequiredSize;
        public uint OptionalMemorySize;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline449175)]
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint StreamedSize;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint DvdMemorySize;
    }
}