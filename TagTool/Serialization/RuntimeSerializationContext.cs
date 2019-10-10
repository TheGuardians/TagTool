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
        private HaloOnlineCacheContext CacheContext { get; }
        private ProcessMemoryStream ProcessStream { get; }

        public RuntimeSerializationContext(HaloOnlineCacheContext cacheContext, ProcessMemoryStream processStream)
        {
            CacheContext = cacheContext;
            ProcessStream = processStream;
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
            return new DataBlock();
        }

        public void EndDeserialize(TagStructureInfo info, object obj)
        {
        }

        public void EndSerialize(TagStructureInfo info, byte[] data, uint mainStructOffset)
        {
            ProcessStream.Write(data, 0, data.Length);
        }

        public CachedTagInstance GetTagByIndex(int index)
        {
            return (index >= 0 && index < CacheContext.TagCache.Index.Count) ? CacheContext.TagCache.Index[index] : null;
        }

        public CachedTagInstance GetTagByName(TagGroup group, string name)
        {
            throw new NotImplementedException();
        }

        public void AddResourceBlock(int count, CacheResourceAddress address, IList block)
        {
            throw new NotImplementedException();
        }

        private class DataBlock : IDataBlock
        {
            public MemoryStream Stream { get; private set; }
            public BinaryWriter Writer { get; private set; }

            public DataBlock()
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
