using System;
using TagTool.Cache;
using TagTool.IO;

namespace TagTool.Serialization
{
    class MapSerializationContext : ISerializationContext
    {
        public uint AddressToOffset(uint currentOffset, uint address)
        {
            throw new NotImplementedException();
        }

        public EndianReader BeginDeserialize(TagStructureInfo info)
        {
            throw new NotImplementedException();
        }

        public void BeginSerialize(TagStructureInfo info)
        {
            throw new NotImplementedException();
        }

        public IDataBlock CreateBlock()
        {
            throw new NotImplementedException();
        }

        public void EndDeserialize(TagStructureInfo info, object obj)
        {
            throw new NotImplementedException();
        }

        public void EndSerialize(TagStructureInfo info, byte[] data, uint mainStructOffset)
        {
            throw new NotImplementedException();
        }

        public CachedTagInstance GetTagByIndex(int index)
        {
            throw new NotImplementedException();
        }
    }
}





