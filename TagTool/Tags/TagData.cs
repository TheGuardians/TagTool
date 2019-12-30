using TagTool.Cache;

namespace TagTool.Tags
{
    [TagStructure(Size = 0x14)] // TODO: refactor into not a tag structure, this has a special case in the deserializer now
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
        public CacheAddress Address;

        public int Unused10;

        [TagField(Flags = TagFieldFlags.Runtime)]
        public byte[] Data;

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
