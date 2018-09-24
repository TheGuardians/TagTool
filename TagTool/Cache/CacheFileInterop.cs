using TagTool.Serialization;

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
        /// 
        /// </summary>
        [TagField(Length = (int)CacheFileSectionType.Count)]
        public CacheFileSection[] Sections = new CacheFileSection[(int)CacheFileSectionType.Count];

        public bool IsNull =>
            ResourceBaseAddress == 0 &&
            DebugSectionSize == 0 &&
            RuntimeBaseAddress == 0 &&
            UnknownBaseAddress == 0;

        public void PostprocessForCacheRead(int cache_header_sizeof)
        {
            var cache_offset = cache_header_sizeof;

            var type = CacheFileSectionType.Debug;

            var section = Sections[(int)type];
            section.InitializeCacheOffset(cache_offset, this.IsNull);

            var release_offset = DebugSectionSize;

            for (++type; type < CacheFileSectionType.Count; type++, release_offset += section.Size)
            {
                section = Sections[(int)type];

                if (section.Size == 0)
                    continue;

                section.InitializeCacheOffset(release_offset, this.IsNull);
            }
        }
    }
}