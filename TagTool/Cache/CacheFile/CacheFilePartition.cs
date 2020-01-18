using TagTool.Tags;

namespace TagTool.Cache
{
    /// <summary>
    /// A partition within a cache file.
    /// </summary>
    [TagStructure(Size = 0x8)]
	public class CacheFilePartition : TagStructure
	{
        /// <summary>
        /// The base address of the cache file partition.
        /// </summary>
        public uint BaseAddress;

        /// <summary>
        /// The size of the cache file partition.
        /// </summary>
        public int Size;
    }
}