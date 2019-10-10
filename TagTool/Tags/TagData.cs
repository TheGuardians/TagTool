using TagTool.Cache;

namespace TagTool.Tags
{
    [TagStructure(Size = 0x14)]
    public class TagData : TagStructure
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
        public CacheResourceAddress Address;

        public int Unused10;

        [TagField(Flags = TagFieldFlags.Runtime)]
        public byte[] Data;

        public TagData()
        {
        }

        public TagData(int size, CacheResourceAddress address)
        {
            Size = size;
            Address = address;
        }
    }
}
