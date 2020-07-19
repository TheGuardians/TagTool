using TagTool.Cache;
using TagTool.IO;
using TagTool.Tags;
using System;
using System.IO;
using TagTool.Serialization;
using System.Collections;

namespace TagTool.Serialization
{
    public class RuntimeSerializationContext : ISerializationContext
    {
        private GameCache Cache { get; }
        private ProcessMemoryStream ProcessStream { get; }
        private uint StartAddress;
        private uint OriginalStructOffset;
        private uint HeaderSize;
        private uint OriginalSize;

        public RuntimeSerializationContext(GameCache cache, ProcessMemoryStream processStream, uint tagAddress, uint originalOffset, uint headerSize, uint originalSize)
        {
            Cache = cache;
            ProcessStream = processStream;
            StartAddress = tagAddress;
            OriginalStructOffset = originalOffset;
            HeaderSize = headerSize;
            OriginalSize = originalSize;
        }

        public uint AddressToOffset(uint currentOffset, uint address)
        {
            return address;
        }

        public EndianReader BeginDeserialize(TagStructureInfo info)
        {
            return new EndianReader(ProcessStream);
        }

        public void BeginSerialize(TagStructureInfo info)
        {
        }

        public IDataBlock CreateBlock()
        {
            return new DataBlock(StartAddress, HeaderSize);
        }

        public void EndDeserialize(TagStructureInfo info, object obj)
        {
        }

        public void EndSerialize(TagStructureInfo info, byte[] data, uint mainStructOffset)
        {
            if (mainStructOffset + HeaderSize == OriginalStructOffset && data.Length <= OriginalSize - HeaderSize)
            {
                ProcessStream.Seek(StartAddress + HeaderSize, SeekOrigin.Begin);
                ProcessStream.Write(data, 0, data.Length);
                ProcessStream.Flush();
            }
            else if(data.Length > OriginalSize - HeaderSize)
            {
                throw new InvalidDataException($"Cannot poke a tag larger than the tag currently in memory. Size: 0x{OriginalSize.ToString("X8")} Poking: 0x{data.Length.ToString("X8")}");
            }
            else if (mainStructOffset + HeaderSize != OriginalStructOffset)
            {
                throw new InvalidDataException($"Error: tag size changed or the serializer failed!");
            }
        }

        public CachedTag GetTagByIndex(int index)
        {
            return (index >= 0 && index < Cache.TagCache.Count) ? Cache.TagCache.GetTag(index) : null;
        }

        public CachedTag GetTagByName(TagGroup group, string name)
        {
            throw new NotImplementedException();
        }

        public void AddResourceBlock(int count, CacheAddress address, IList block)
        {
            throw new NotImplementedException();
        }

        private class DataBlock : IDataBlock
        {
            public MemoryStream Stream { get; private set; }
            public EndianWriter Writer { get; private set; }
            private uint StartAddress;
            private uint HeaderSize;

            public DataBlock(uint startAddress, uint headerSize)
            {
                Stream = new MemoryStream();
                Writer = new EndianWriter(Stream);
                StartAddress = startAddress;
                HeaderSize = headerSize;
            }

            public void WritePointer(uint targetOffset, Type type)
            {
                Writer.Write(targetOffset + StartAddress + HeaderSize);
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