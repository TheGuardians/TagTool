using TagTool.Tags;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Cache
{
    /// <summary>
    /// A virtual section of a cache file.
    /// </summary>
    [TagStructure(Size = 0x8)]
    public class CacheFileSection : TagStructure
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

    /// <summary>
    /// Enum to be used as index in CacheFileInterop in Gen3 map files.
    /// </summary>
    public enum CacheFileSectionType : int
    {
        StringIdSection,
        ResourceSection,
        TagSection,
        LocalizationSection,

        Count
    }
}