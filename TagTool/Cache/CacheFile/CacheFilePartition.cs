using TagTool.Tags;

namespace TagTool.Cache
{
    /// <summary>
    /// Partitions within the tag section of a map file.
    /// </summary>
    [TagStructure(Size = 0x8, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x10, Platform = CachePlatform.MCC)]
    public class CacheFilePartition : TagStructure
    {
        /// <summary>
        /// The address of the partition in memory. Use MapHeader.BaseAddress to get the relative address in the tag section.
        /// </summary>
        public PlatformUnsignedValue VirtualAddress;

        /// <summary>
        /// The size of the partition. The sum of all partitions should be equal to the size of the tag section.
        /// </summary>
        public PlatformSignedValue Size;
    }

    public enum CacheFilePartitionType : int
    {
        ResourcesTags,
        SoundResourcesTags,
        GlobalTags,
        SharedTags,
        BaseTags,
        MapTags,

        Count
    }

    public enum CacheFilePartitionTypeBeta : int
    {
        ResourcesTags,
        SoundResourcesTags,
        GlobalTags,

        Count
    }
}