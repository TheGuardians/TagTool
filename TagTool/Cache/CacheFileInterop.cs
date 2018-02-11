using TagTool.Serialization;

namespace TagTool.Cache
{
    /// <summary>
    /// 
    /// </summary>
    [TagStructure(Size = 0x30)]
    public sealed class CacheFileInterop
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
        /// 
        /// </summary>
        [TagField(Length = (int)CacheFileSectionType.Count)]
        public CacheFileSection[] Sections = new CacheFileSection[(int)CacheFileSectionType.Count];
    }
}