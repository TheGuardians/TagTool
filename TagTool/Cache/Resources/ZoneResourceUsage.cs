using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache.Resources
{
    [TagStructure(Size = 0x14, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x18, MinVersion = CacheVersion.HaloReach)]
    public class ZoneResourceUsage : TagStructure
    {
        [TagField(Flags = TagFieldFlags.Label, MinVersion = CacheVersion.HaloReach)]
        public StringId Name;

        public uint RequiredPageableSize;
        public uint DeferredRequiredSize;
        public uint OptionalMemorySize;
        public uint StreamedSize;
        public uint DvdMemorySize;
    }
}