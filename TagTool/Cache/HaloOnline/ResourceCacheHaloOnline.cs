using LZ4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.Cache.HaloOnline
{
    [TagStructure(Size = 0x20)]
    public class ResourceCacheHaloOnlineHeader
    {
        public int UnusedTag;
        public uint ResourceTableOffset;
        public int ResourceCount;
        public int Unused;
        public long CreationTime;
        public int Unused2;
        public int Unused3;
    }

    // Class for .dat files containing resources
    public class ResourceCacheHaloOnline
    {
        public CacheVersion Version;
        public ResourceCacheHaloOnlineHeader Header;

        private List<Resource> Resources;

        private const int ChunkHeaderSize = 0x8;
        private const int MaxDecompressedBlockSize = 0x7FFF8; // Decompressed chunks cannot exceed this size

        public int Count
        {
            get { return Resources.Count; }
        }

        public ResourceCacheHaloOnline(CacheVersion version, Stream stream)
        {
            Version = version;
            Resources = new List<Resource>();
            if (stream.Length == 0)
                CreateEmptyResourceCache(stream);
            else
                Read(stream);
        }

        public ResourceCacheHaloOnline(CacheVersion version)
        {
            Version = version;
            Resources = new List<Resource>();
            Header = new ResourceCacheHaloOnlineHeader
            {
                ResourceTableOffset = 0x20,
                CreationTime = 0x01D0631BCC92931B
            };
        }

        private void Read(Stream stream)
        {
            // don't use using{}, we want to maintain the stream open. reader/writers will automatically close the stream when done in an using.
            var reader = new EndianReader(stream, EndianFormat.LittleEndian);
            stream.Position = 0;
            var addresses = new List<uint>();
            var sizes = new List<uint>();
            var dataContext = new DataSerializationContext(reader);
            var deserializer = new TagDeserializer(Version);
            Header = deserializer.Deserialize<ResourceCacheHaloOnlineHeader>(dataContext);

            reader.SeekTo(Header.ResourceTableOffset);

            // read all resource offsets

            if (Header.ResourceCount == 0)
                return;

            for (var i = 0; i < Header.ResourceCount; i++)
            {
                var address = reader.ReadUInt32();

                if (!addresses.Contains(address) && (address != uint.MaxValue))
                    addresses.Add(address);

                Resources.Add(new Resource { Offset = address });
            }

            // compute chunk sizes

            addresses.Sort((a, b) => a.CompareTo(b));

            for (var i = 0; i < addresses.Count - 1; i++)
                sizes.Add(addresses[i + 1] - addresses[i]);

            sizes.Add(Header.ResourceTableOffset - addresses.Last());

            foreach (var resource in Resources)
            {
                if (resource.Offset == uint.MaxValue)
                    continue;

                resource.ChunkSize = sizes[addresses.IndexOf(resource.Offset)];
            }
        }

        private void WriteHeader(Stream stream)
        {
            stream.Position = 0;
            using (var writer = new EndianWriter(stream, EndianFormat.LittleEndian))
            {
                var dataContext = new DataSerializationContext(writer);
                var serializer = new TagSerializer(Version);
                serializer.Serialize(dataContext, Header);
            }
        }

        private void CreateEmptyResourceCache(Stream stream)
        {
            Header = new ResourceCacheHaloOnlineHeader
            {
                ResourceTableOffset = 0x20,
                CreationTime = 0x01D0631BCC92931B
            };
            stream.Position = 0;
            var writer = new EndianWriter(stream, EndianFormat.LittleEndian);
            var dataContext = new DataSerializationContext(writer);
            var serializer = new TagSerializer(CacheVersion.HaloOnline106708);
            serializer.Serialize(dataContext, Header);
            stream.Position = 0;
        }

        /// <summary>
        /// Adds a resource to the cache.
        /// </summary>
        /// <param name="resourceCacheStream">The stream open on the resource cache.</param>
        /// <param name="tagResourceData">The data to compress.</param>
        /// <param name="compressedSize">On return, the size of the compressed data.</param>
        /// <returns>The index of the resource that was added.</returns>
        public int Add(Stream resourceCacheStream, byte[] tagResourceData, out uint compressedSize)
        {
            var resourceIndex = NewResource();
            compressedSize = Compress(resourceCacheStream, resourceIndex, tagResourceData);
            return resourceIndex;
        }

        /// <summary>
        /// Adds a raw, pre-compressed resource to the cache.
        /// </summary>
        /// <param name="resourceCacheStream">The stream open on the resource cache.</param>
        /// <param name="rawData">The raw data to add.</param>
        /// <returns>The index of the resource that was added.</returns>
        public int AddRaw(Stream resourceCacheStream, byte[] rawData)
        {
            var resourceIndex = NewResource();
            ImportRaw(resourceCacheStream, resourceIndex, rawData);
            return resourceIndex;
        }

        /// <summary>
        /// Extracts raw, compressed resource data.
        /// </summary>
        /// <param name="resourceCacheStream">The stream open on the resource cache.</param>
        /// <param name="resourceIndex">The index of the resource to decompress.</param>
        /// <param name="compressedSize">Total size of the compressed data, including chunk headers.</param>
        /// <returns>The raw, compressed resource data.</returns>
        public byte[] ExtractRaw(Stream resourceCacheStream, int resourceIndex, uint compressedSize)
        {
            if (resourceIndex < 0 || resourceIndex >= Resources.Count)
                throw new ArgumentOutOfRangeException("resourceIndex");

            var resource = Resources[resourceIndex];
            resourceCacheStream.Position = resource.Offset;
            var result = new byte[compressedSize];
            resourceCacheStream.Read(result, 0, result.Length);
            return result;
        }

        /// <summary>
        /// Overwrites a resource with raw, pre-compressed data.
        /// </summary>
        /// <param name="resourceCacheStream">The stream open on the resource cache.</param>
        /// <param name="resourceIndex">The index of the resource to overwrite.</param>
        /// <param name="data">The raw, pre-compressed data to overwrite it with.</param>
        public void ImportRaw(Stream resourceCacheStream, int resourceIndex, byte[] data)
        {
            if (resourceIndex < 0 || resourceIndex >= Resources.Count)
                throw new ArgumentOutOfRangeException("resourceIndex");

            var roundedSize = ResizeResource(resourceCacheStream, resourceIndex, (uint)data.Length);
            var resource = Resources[resourceIndex];
            resourceCacheStream.Position = resource.Offset;
            resourceCacheStream.Write(data, 0, data.Length);
            StreamUtil.Fill(resourceCacheStream, 0, (int)(roundedSize - data.Length)); // Padding
        }

        public void NullResource(Stream resourceStream, int resourceIndex)
        {
            if (resourceIndex < 0 || resourceIndex >= Resources.Count)
                throw new ArgumentOutOfRangeException("resourceIndex");

            var resource = Resources[resourceIndex];
            var writer = new BinaryWriter(resourceStream);

            if (IsResourceShared(resourceIndex))
                return;

            if (resource.Offset != uint.MaxValue && resource.ChunkSize > 0)
            {
                StreamUtil.Copy(resourceStream, resource.Offset + resource.ChunkSize, resource.Offset, resourceStream.Length - resource.Offset);

                for (var i = 0; i < Resources.Count; i++)
                    if (Resources[i].Offset > resource.Offset)
                        Resources[i].Offset = (Resources[i].Offset - resource.ChunkSize);
            }

            resource.Offset = uint.MaxValue;
            resource.ChunkSize = 0;

            UpdateResourceTable(resourceStream);
        }

        /// <summary>
        /// Decompresses a resource.
        /// </summary>
        /// <param name="resourceStream">The stream open on the resource cache.</param>
        /// <param name="resourceIndex">The index of the resource to decompress.</param>
        /// <param name="compressedSize">Total size of the compressed data, including chunk headers.</param>
        /// <param name="outStream">The stream to write the decompressed resource data to.</param>
        public void Decompress(Stream resourceStream, int resourceIndex, uint compressedSize, Stream outStream)
        {
            if (resourceIndex < 0 || resourceIndex >= Resources.Count)
                throw new ArgumentOutOfRangeException("resourceIndex");

            var reader = new BinaryReader(resourceStream);
            var resource = Resources[resourceIndex];
            reader.BaseStream.Position = resource.Offset;

            // Compressed resources are split into chunks, so decompress each chunk until the complete data is decompressed
            var totalProcessed = 0U;
            compressedSize = Math.Min(compressedSize, resource.ChunkSize);
            while (totalProcessed < compressedSize)
            {
                // Each chunk begins with a 32-bit decompressed size followed by a 32-bit compressed size
                var chunkDecompressedSize = reader.ReadInt32();
                var chunkCompressedSize = reader.ReadInt32();
                totalProcessed += 8;
                if (totalProcessed >= compressedSize)
                    break;

                // Decompress the chunk and write it to the output stream
                var compressedData = new byte[chunkCompressedSize];
                reader.Read(compressedData, 0, chunkCompressedSize);
                var decompressedData = LZ4Codec.Decode(compressedData, 0, chunkCompressedSize, chunkDecompressedSize);
                outStream.Write(decompressedData, 0, chunkDecompressedSize);
                totalProcessed += (uint)chunkCompressedSize;
            }
        }

        /// <summary>
        /// Compresses and saves data for a resource.
        /// </summary>
        /// <param name="resourceStream">The stream open on the resource data. It must have read/write access.</param>
        /// <param name="resourceIndex">The index of the resource to edit.</param>
        /// <param name="data">The data to compress.</param>
        /// <returns>The total size of the compressed resource in bytes.</returns>
        public uint Compress(Stream resourceStream, int resourceIndex, byte[] data)
        {
            if (resourceIndex < 0 || resourceIndex >= Resources.Count)
                throw new ArgumentOutOfRangeException("resourceIndex");

            // Divide the data into chunks with decompressed sizes no larger than the maximum allowed size
            var chunks = new List<byte[]>();
            var startOffset = 0;
            uint newSize = 0;
            while (startOffset < data.Length)
            {
                var chunkSize = Math.Min(data.Length - startOffset, MaxDecompressedBlockSize);
                var chunk = LZ4Codec.Encode(data, startOffset, chunkSize);
                chunks.Add(chunk);
                startOffset += chunkSize;
                newSize += (uint)(ChunkHeaderSize + chunk.Length);
            }

            // Write the chunks in
            var writer = new BinaryWriter(resourceStream);
            var roundedSize = ResizeResource(writer.BaseStream, resourceIndex, newSize);
            var resource = Resources[resourceIndex];
            resourceStream.Position = resource.Offset;
            var sizeRemaining = data.Length;
            foreach (var chunk in chunks)
            {
                var decompressedSize = Math.Min(sizeRemaining, MaxDecompressedBlockSize);
                writer.Write(decompressedSize);
                writer.Write(chunk.Length);
                writer.Write(chunk);
                sizeRemaining -= decompressedSize;
            }
            StreamUtil.Fill(resourceStream, 0, (int)(roundedSize - newSize)); // Padding
            return newSize;
        }

        private int NewResource()
        {
            var lastResource = (Resources.Count > 0) ? Resources[Resources.Count - 1] : null;
            var resourceIndex = Resources.Count;
            Resources.Add(new Resource
            {
                Offset = (Resources.Count == 0) ? 0x20 : (lastResource != null) ? lastResource.Offset + lastResource.ChunkSize : uint.MaxValue,
                ChunkSize = 0,
            });
            return resourceIndex;
        }

        private uint ResizeResource(Stream resourceStream, int resourceIndex, uint minSize)
        {
            var resource = Resources[resourceIndex];
            var roundedSize = ((minSize + 0xF) & ~0xFU); // Round up to a multiple of 0x10
            var sizeDelta = (int)(roundedSize - resource.ChunkSize);
            var endOffset = resource.Offset + resource.ChunkSize;
            StreamUtil.Copy(resourceStream, endOffset, endOffset + sizeDelta, resourceStream.Length - endOffset);
            resource.ChunkSize = roundedSize;

            // Update resource offsets
            for (var i = resourceIndex + 1; i < Resources.Count; i++)
                Resources[i].Offset = (uint)(Resources[i].Offset + sizeDelta);
            UpdateResourceTable(resourceStream);
            return roundedSize;
        }

        public bool IsResourceShared(int index) => Resources.Where(i => i.Offset == Resources[index].Offset).Count() > 1;

        public void UpdateResourceTable(Stream resourceStream)
        {
            // Assume the table is past the last resource
            uint tableOffset = 0xC;

            var writer = new BinaryWriter(resourceStream);

            if (Resources.Count != 0)
            {
                var lastResource = Resources[Resources.Count - 1];
                tableOffset = lastResource.Offset + lastResource.ChunkSize;
            }

            writer.BaseStream.Position = tableOffset;

            // Write each resource offset
            foreach (var resource in Resources)
                writer.Write(resource.Offset);

            var tableEndOffset = writer.BaseStream.Position;

            // Update the file header
            writer.BaseStream.Position = 0x4;
            writer.Write(tableOffset);
            writer.Write(Resources.Count);

            writer.BaseStream.SetLength(tableEndOffset);
        }

        //
        // Utilities
        //

        private class Resource
        {
            // Offset in the resource file
            public uint Offset;
            // Distance between Offset and next resource Offset, may not be equal to the actual resource size
            public uint ChunkSize;
        }
    }
}
