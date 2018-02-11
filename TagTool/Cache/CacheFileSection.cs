using TagTool.Serialization;

namespace TagTool.Cache
{
    /// <summary>
    /// A virtual section of a cache file.
    /// </summary>
    [TagStructure(Size = 0x8)]
    public struct CacheFileSection
    {
        /// <summary>
        /// The virtual address of the cache file interop section.
        /// </summary>
        public uint VirtualAddress;

        /// <summary>
        /// The size of the cache file interop section.
        /// </summary>
        public int Size;
    }
}