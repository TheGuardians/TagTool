using TagTool.Cache;
using TagTool.IO;
using System;
using System.IO;
using TagTool.Tags;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TagTool.Serialization
{
    public class DataSerializationContext : ISerializationContext
    {
        public EndianReader Reader { get; }
        public EndianWriter Writer { get; }
        public CacheAddressType AddressType { get; }
        public uint MainStructOffset;
        public int PointerOffset = 0x0;
        private const int DefaultBlockAlign = 4;
        public static bool UseAlignment = true;
        public List<uint> PointerOffsets { get; private set; }

        public DataSerializationContext(EndianReader reader, EndianWriter writer, CacheAddressType addressType = CacheAddressType.Memory, bool useAlignment = true)
        {
            Reader = reader;
            Writer = writer;
            AddressType = addressType;
            UseAlignment = useAlignment;
            PointerOffsets = new List<uint>();
        }

        public DataSerializationContext(EndianReader reader, CacheAddressType addressType = CacheAddressType.Memory, bool useAlignment = true) :
            this(reader, null, addressType, useAlignment)
        {
        }

        public DataSerializationContext(EndianWriter writer, CacheAddressType addressType = CacheAddressType.Memory, bool useAlignment = true) :
            this(null, writer, addressType, useAlignment)
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
            return new GenericDataBlock(this, PointerOffset);
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

        private class GenericDataBlock : IDataBlock
        {
            private DataSerializationContext Context;
            public MemoryStream Stream { get; private set; }
            public EndianWriter Writer { get; private set; }
            public int PointerOffset = 0x0;
            private readonly List<uint> _pointerOffsets;
            private uint _align = DefaultBlockAlign;

            public GenericDataBlock(DataSerializationContext context, int offset)
            {
                Context = context;
                Stream = new MemoryStream();
                Writer = new EndianWriter(Stream);
                PointerOffset = offset;
                _pointerOffsets = new List<uint>();
            }

            public void WritePointer(uint targetOffset, Type type)
            {
                _pointerOffsets.Add((uint)Stream.Position);
                Writer.Write((int)(targetOffset + PointerOffset));
            }

            public object PreSerialize(TagFieldAttribute info, object obj)
            {
                return obj;
            }

            public void SuggestAlignment(uint align)
            {
                _align = Math.Max(_align, align);
            }

            public uint Finalize(Stream outStream)
            {
                // Write the data out, aligning the offset and size
                if(UseAlignment)
                    StreamUtil.Align(outStream, (int)_align);
                var dataOffset = (uint)outStream.Position;
                outStream.Write(Stream.GetBuffer(), 0, (int)Stream.Length);
                if (UseAlignment)
                    StreamUtil.Align(outStream, DefaultBlockAlign);

                Context.PointerOffsets.AddRange(_pointerOffsets.Select(o => o + dataOffset));

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
