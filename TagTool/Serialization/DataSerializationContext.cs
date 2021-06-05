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
        public CacheAddressType AddressType { get; }
        public uint MainStructOffset;
        public int PointerOffset = 0x0;

        public DataSerializationContext(EndianReader reader, EndianWriter writer, CacheAddressType addressType = CacheAddressType.Memory)
        {
            Reader = reader;
            Writer = writer;
            AddressType = addressType;
        }

        public DataSerializationContext(EndianReader reader, CacheAddressType addressType = CacheAddressType.Memory) :
            this(reader, null, addressType)
        {
        }

        public DataSerializationContext(EndianWriter writer, CacheAddressType addressType = CacheAddressType.Memory) :
            this(null, writer, addressType)
        {
        }

        public uint AddressToOffset(uint currentOffset, uint address)
        {
            var resourceAddress = new CacheAddress(address);
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
            return new GenericDataBlock(PointerOffset);
        }

        public void EndDeserialize(TagStructureInfo info, object obj)
        {
        }

        public void EndSerialize(TagStructureInfo info, byte[] data, uint mainStructOffset)
        {
            Writer.Write(data);
            MainStructOffset = mainStructOffset;
        }

        public CachedTag GetTagByIndex(int index)
        {
            return null;
        }

        public CachedTag GetTagByName(TagGroup group, string name)
        {
            throw new NotImplementedException();
        }

        public void AddResourceBlock(int count, CacheAddress address, IList block)
        {
            throw new NotImplementedException();
        }

        public CacheVersion GetVersion() => CacheVersion.Unknown;

        public CachePlatform GetCachePlatform() => CachePlatform.All;

        public PlatformType GetPlatformType() => PlatformType._32Bit;

        private class GenericDataBlock : IDataBlock
        {
            public MemoryStream Stream { get; private set; }
            public EndianWriter Writer { get; private set; }
            public int PointerOffset = 0x0;

            public GenericDataBlock(int offset)
            {
                Stream = new MemoryStream();
                Writer = new EndianWriter(Stream);
                PointerOffset = offset;
            }

            public void WritePointer(uint targetOffset, Type type)
            {
                Writer.Write((int)(targetOffset + PointerOffset));
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

            public void AddTagReference(CachedTag referencedTag, bool isShort)
            {
            }
        }
    }
}
