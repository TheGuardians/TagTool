using TagTool.Cache;

namespace TagTool.Tags
{
    [TagStructure(Size = 0x14)]
    public class TagData
	{
        public CacheAddressType AddressType = CacheAddressType.Data;
        public byte[] Data;

        public TagData()
        {
        }

        public TagData(byte[] data)
        {
            Data = data;
        }
    }
}
