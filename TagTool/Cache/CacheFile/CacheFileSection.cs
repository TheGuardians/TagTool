using TagTool.Tags;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Cache
{
    /// <summary>
    /// Section table in gen3 cache files.
    /// </summary>
    [TagStructure(Size = 0x30)]
    public class CacheFileSectionTable : TagStructure
    {
        /// <summary>
        /// Add this value to the section virtual address to get the file offset
        /// </summary>
        [TagField(Length = (int)CacheFileSectionType.Count)]
        public int[] SectionAddressToOffsets;

        /// <summary>
        /// Sections in a map file, ordered and offset is determined by the sizes after the map header.
        /// </summary>
        [TagField(Length = (int)CacheFileSectionType.Count)]
        public CacheFileSection[] Sections = new CacheFileSection[(int)CacheFileSectionType.Count];

        /// <summary>
        /// Get the section's file offset given the type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public uint GetSectionOffset(CacheFileSectionType type)
        {
            return (uint)(Sections[(int)type].VirtualAddress + SectionAddressToOffsets[(int)type]);
        }

        /// <summary>
        /// Get the offset of the specified address in the section.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public uint GetOffset(CacheFileSectionType type, uint address)
        {
            return (uint)(address + SectionAddressToOffsets[(int)type]);
        }
    }

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
        StringSection,
        ResourceSection,
        TagSection,
        LocalizationSection,

        Count
    }
}