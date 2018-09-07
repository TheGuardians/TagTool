using TagTool.Serialization;

namespace TagTool.Cache
{
    /// <summary>
    /// A virtual section of a cache file.
    /// </summary>
    [TagStructure(Size = 0x8)]
    public class CacheFileSection
    {
        /// <summary>
        /// The virtual address of the cache file interop section.
        /// </summary>
        public uint VirtualAddress;

        /// <summary>
        /// The size of the cache file interop section.
        /// </summary>
        public int Size;

        [TagField(Local = true)]
        public int CacheOffset = -1;

        [TagField(Local = true)]
        public uint AddressMask = uint.MaxValue;

        public void InitializeCacheOffset(int cacheOffset, bool interopIsNull)
        {
            if (interopIsNull)
            {
                CacheOffset = (int)VirtualAddress;
                return;
            }

            CacheOffset = cacheOffset;

            if (VirtualAddress != 0)
                AddressMask = VirtualAddress - (uint)cacheOffset;
        }
    }
}