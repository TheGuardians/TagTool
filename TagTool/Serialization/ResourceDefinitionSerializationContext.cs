using TagTool.Cache;
using TagTool.IO;
using System;
using System.IO;
using TagTool.Tags;
using System.Collections;
using System.Collections.Generic;
using static TagTool.Tags.TagResourceGen3;

namespace TagTool.Serialization
{
    public class ResourceDefinitionSerializationContext : ISerializationContext
    {
        public EndianReader DataReader { get; }
        public EndianWriter DataWriter { get; }
        public EndianReader SecondaryDataReader { get; }
        public EndianWriter SecondaryDataWriter { get; }
        public EndianReader DefinitionReader { get; }
        public EndianWriter DefinitionWriter { get;  }

        public CacheAddressType InitialAddressType { get; }

        public List<ResourceFixup> ResourceFixups = new List<ResourceFixup>();

        public ResourceDefinitionSerializationContext(EndianReader dataReader, EndianWriter dataWriter, EndianReader secondaryDataReader, EndianWriter secondaryDataWriter, EndianReader definitionReader, EndianWriter definitionWriter, CacheAddressType initialAddressType)
        {
            DataReader = dataReader;
            DataWriter = dataWriter;
            DefinitionReader = definitionReader;
            DefinitionWriter = definitionWriter;
            SecondaryDataReader = secondaryDataReader;
            SecondaryDataWriter = secondaryDataWriter;
            InitialAddressType = initialAddressType;
        }

        public ResourceDefinitionSerializationContext(EndianReader dataReader, EndianReader definitionReader, CacheAddressType initialAddressType) :
            this(dataReader, null, null, null, definitionReader, null, initialAddressType)
        {
        }

        public ResourceDefinitionSerializationContext(EndianWriter dataWriter, EndianWriter definitionWriter, CacheAddressType initialAddressType) :
            this(null, dataWriter, null, null, null, definitionWriter, initialAddressType)
        {
        }

        public ResourceDefinitionSerializationContext(EndianReader dataReader, EndianReader secondaryDataReader, EndianReader definitionReader, CacheAddressType initialAddressType) :
            this(dataReader, null, secondaryDataReader, null, definitionReader, null, initialAddressType)
        {
        }

        public ResourceDefinitionSerializationContext(EndianWriter dataWriter, EndianWriter secondaryDataWriter, EndianWriter definitionWriter, CacheAddressType initialAddressType) :
            this(null, dataWriter, null, secondaryDataWriter, null, definitionWriter, initialAddressType)
        {
        }

        public uint AddressToOffset(uint currentOffset, uint address)
        {
            var resourceAddress = new CacheAddress(address);
            return (uint)resourceAddress.Offset;
        }

        public EndianReader GetReader(CacheAddressType type)
        {
            switch (type)
            {
                case CacheAddressType.Data:
                    return DataReader;
                case CacheAddressType.Definition:
                    return DefinitionReader;
                case CacheAddressType.SecondaryData:
                    return SecondaryDataReader;
                default:
                    return null;
            }
        }

        public EndianWriter GetWriter(CacheAddressType type)
        {
            switch (type)
            {
                case CacheAddressType.Data:
                    return DataWriter;
                case CacheAddressType.Definition:
                    return DefinitionWriter;
                case CacheAddressType.SecondaryData:
                    return SecondaryDataWriter;
                default:
                    return null;
            }
        }

        public EndianReader BeginDeserialize(TagStructureInfo info)
        {
            return GetReader(InitialAddressType);
        }

        public void BeginSerialize(TagStructureInfo info)
        {
        }

        public IDataBlock CreateBlock()
        {
            return new GenericDataBlock();
        }

        public IDataBlock CreateBlockForResource(EndianWriter writer)
        {
            return new GenericDataBlock(writer);
        }

        public void EndDeserialize(TagStructureInfo info, object obj)
        {
        }

        public void EndSerialize(TagStructureInfo info, byte[] data, uint mainStructOffset)
        {
            GetWriter(InitialAddressType).Write(data);
        }

        public CachedTagInstance GetTagByIndex(int index)
        {
            return null;
        }

        public CachedTagInstance GetTagByName(TagGroup group, string name)
        {
            throw new NotImplementedException();
        }

        public void AddResourceBlock(int count, CacheAddress address, IList block)
        {
            throw new NotImplementedException();
        }

        private class GenericDataBlock : IDataBlock
        {
            public MemoryStream Stream { get; private set; }
            public EndianWriter Writer { get; private set; }

            public GenericDataBlock()
            {
                Stream = new MemoryStream();
                Writer = new EndianWriter(Stream);
            }

            public GenericDataBlock(EndianWriter writer)
            {
                Stream = (MemoryStream)writer.BaseStream;
                Writer = writer;
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

            public void AddTagReference(CachedTagInstance referencedTag, bool isShort)
            {
            }
        }
    }
}
