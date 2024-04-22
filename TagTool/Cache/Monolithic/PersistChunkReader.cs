using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;

namespace TagTool.Cache.Monolithic
{
    public class PersistChunkReader : EndianReader
    {
        public DataSerializationContext SerializationContext;
        public TagDeserializer Deserializer;

        public PersistChunkReader(Stream stream, EndianFormat type = EndianFormat.LittleEndian) : base(stream, type)
        {
        }

        public PersistChunkReader(Stream Stream, bool leaveOpen, EndianFormat Type = EndianFormat.LittleEndian) : base(Stream, leaveOpen, Type)
        {
        }

        void InitDeserializer()
        {
            if (SerializationContext != null)
                return;

            SerializationContext = new DataSerializationContext(this, CacheAddressType.Memory, false);
            Deserializer = new TagDeserializer(CacheVersion.Unknown, CachePlatform.All);
        }

        public PersistChunkHeader ReadChunkHeader()
        {
            var header = new PersistChunkHeader();
            header.Signature = this.ReadTag();
            header.Version = ReadInt32();
            header.Size = ReadInt32();
            return header;
        }

        public IEnumerable<PersistChunk> ReadChunks()
        {
            while (!EOF)
            {
                var chunkHeader = ReadChunkHeader();
                var chunkStream = new MemoryStream(ReadBytes(chunkHeader.Size));
                yield return new PersistChunk(chunkHeader, chunkStream);
            }
        }

        public PersistChunk ReadNextChunk()
        {
            return ReadChunks().FirstOrDefault();
        }

        public PersistChunk ReadNextChunk(Tag expectedChunk)
        {
            var chunk = ReadNextChunk();
            if (chunk.Header.Signature != expectedChunk)
                throw new Exception($"Persist chunk tag incorrect");
            return chunk;
        }

        public T Deserialize<T>()
        {
            InitDeserializer();
            return Deserializer.Deserialize<T>(SerializationContext);
        }

        public IEnumerable<T> Deserialize<T>(int count)
        {
            InitDeserializer();
            for (int i = 0; i < count; i++)
                yield return Deserializer.Deserialize<T>(SerializationContext);
        }
    }
}
