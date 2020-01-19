using TagTool.Tags;

namespace TagTool.Cache
{
    /// <summary>
    /// Partitions within the tag section of a map file.
    /// </summary>
    [TagStructure(Size = 0x8)]
	public class CacheFilePartition : TagStructure
	{
        /// <summary>
        /// The address of the partition in memory. Use MapHeader.BaseAddress to get the relative address in the tag section.
        /// </summary>
        public uint VirtualAddress;

        /// <summary>
        /// The size of the partition. The sum of all partitions should be equal to the size of the tag section.
        /// </summary>
        public int Size;
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
}