using TagTool.Cache;
using TagTool.Serialization;

namespace TagTool.Tags
{
    [TagStructure(Size = 0x14)]
    public class TagData
    {
        /// <summary>
        /// The size of the referenced data in bytes.
        /// </summary>
        public int Size;

        public int Unused4;
        public int Unused8;

        /// <summary>
        /// The address of the referenced data.
        /// </summary>
        public CacheAddress Address;

        public int Unused10;

        public TagData()
        {
        }

        public TagData(int size, CacheAddress address)
        {
            Size = size;
            Address = address;
        }
    }
}
