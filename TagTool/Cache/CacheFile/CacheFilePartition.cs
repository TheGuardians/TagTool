using TagTool.Tags;

namespace TagTool.Cache
{
    /// <summary>
    /// Partitions within the tag section of a map file.
    /// </summary>
    [TagStructure(Size = 0x8, Platform = CachePlatform.Only32Bit)]
    [TagStructure(Size = 0x10, Platform = CachePlatform.Only64Bit)]
    public class CacheFilePartition : TagStructure
    {
        /// <summary>
        /// The 32-bit address of the partition in memory. Use MapHeader.BaseAddress to get the relative address in the tag section.
        /// </summary>
        [TagField(Platform = CachePlatform.Only32Bit)]
        public uint VirtualAddress32;

        /// <summary>
        /// The 64-bit address of the partition in memory. Use MapHeader.BaseAddress to get the relative address in the tag section.
        /// </summary>
        [TagField(Platform = CachePlatform.Only64Bit)]
        public ulong VirtualAddress64;

        /// <summary>
        /// The 32-bit size of the partition. The sum of all partitions should be equal to the size of the tag section.
        /// </summary>
        [TagField(Platform = CachePlatform.Only32Bit)]
        public int Size32;

        /// <summary>
        /// The 64-bit size of the partition. The sum of all partitions should be equal to the size of the tag section.
        /// </summary>
        [TagField(Platform = CachePlatform.Only64Bit)]
        public long Size64;
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