using TagTool.Tags;

namespace TagTool.Cache
{
    /// <summary>
    /// 
    /// </summary>
    [TagStructure(Size = 0x30)]
    public sealed class CacheFileInterop : TagStructure
	{
        /// <summary>
        /// 
        /// </summary>
        public uint ResourceBaseAddress;

        /// <summary>
        /// 
        /// </summary>
        public int DebugSectionSize;

        /// <summary>
        /// 
        /// </summary>
        public uint RuntimeBaseAddress;

        /// <summary>
        /// 
        /// </summary>
        public uint UnknownBaseAddress;

        /// <summary>
        /// Sections in a map file, ordered and offset is determined by the sizes after the map header.
        /// </summary>
        [TagField(Length = (int)CacheFileSectionType.Count)]
        public CacheFileSection[] Sections = new CacheFileSection[(int)CacheFileSectionType.Count];
    }
}