using TagTool.Cache;
using TagTool.IO;
using System;
using System.IO;
using TagTool.Tags;
using System.Collections;

namespace TagTool.Serialization
{
    public class DataSerializationContext : ISerializationContext
    {
        public EndianReader Reader { get; }
        public EndianWriter Writer { get; }
        public CacheResourceAddressType AddressType { get; }

        public DataSerializationContext(EndianReader reader, EndianWriter writer, CacheResourceAddressType addressType = CacheResourceAddressType.Memory)
        {
            Reader = reader;
            Writer = writer;
            AddressType = addressType;
        }

        public DataSerializationContext(EndianReader reader, CacheResourceAddressType addressType = CacheResourceAddressType.Memory) :
            this(reader, null, addressType)
        {
        }

        public DataSerializationContext(EndianWriter writer, CacheResourceAddressType addressType = CacheResourceAddressType.Memory) :
            this(null, writer, addressType)
        {
        }

        public uint AddressToOffset(uint currentOffset, uint address)
        {
            var resourceAddress = new CacheResourceAddress(address);

            if (resourceAddress.Type != AddressType)
                throw new InvalidOperationException("Cannot dereference a resource address of type " + resourceAddress.Type);

            return (uint)resourceAddress.Offset;
        }

        public EndianReader BeginDeserialize(TagStructureInfo info)
        {
            return Reader;
        }

        public void BeginSerialize(TagStructureInfo info)
        {
        }

        public IDataBlock CreateBlock()
        {
            return new GenericDataBlock();
        }

        public void EndDeserialize(TagStructureInfo info, object obj)
        {
        }

        public void EndSerialize(TagStructureInfo info, byte[] data, uint mainStructOffset)
        {
            Writer.Write(data);
        }

        public CachedTagInstance GetTagByIndex(int index)
        {
            return null;
        }

        public CachedTagInstance GetTagByName(TagGroup group, string name)
        {
            throw new NotImplementedException();
        }

        public void AddResourceBlock(int count, CacheResourceAddress address, IList block)
        {
            throw new NotImplementedException();
        }

        private class GenericDataBlock : IDataBlock
        {
            public MemoryStream Stream { get; private set; }
            public BinaryWriter Writer { get; private set; }

            public GenericDataBlock()
            {
                Stream = new MemoryStream();
                Writer = new BinaryWriter(Stream);
            }

            public void WritePointer(uint targetOffset, Type type)
            {
                Writer.Write(targetOffset);
            }

            public object PreSerialize(TagFieldAttribute info, object obj)
            {
                return obj;
            }

            public void SuggestAlignment(uint align)
            {
            }

            public uint Finalize(Stream outStream)
            {
                var dataOffset = (uint)outStream.Position;
                outStream.Write(Stream.GetBuffer(), 0, (int)Stream.Length);
                
                Writer.Close();
                Stream = null;
                Writer = null;

                return dataOffset;
            }
        }
    }
}
