using LZ4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.IO;

namespace TagTool.Cache
{
    // TODO: Come up with a common class for managing .dat files

    /// <summary>
    /// A .dat file containing resource data (e.g. resources.dat).
    /// </summary>
    public class ResourceCache
    {
        private const int ChunkHeaderSize = 0x8;
        private const int MaxDecompressedBlockSize = 0x7FFF8; // Decompressed chunks cannot exceed this size

        private readonly List<Resource> _resources = new List<Resource>();

        /// <summary>
        /// Loads a resource cache from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public ResourceCache(Stream stream)
        {
            if (stream.Length != 0)
                Load(stream);
            else
                _resources = new List<Resource>();
        }

        /// <summary>
        /// Gets the number of resources in the archive.
        /// </summary>
        public int Count
        {
            get { return _resources.Count; }
        }

        /// <summary>
        /// Adds a resource to the cache.
        /// </summary>
        /// <param name="inStream">The stream open on the resource cache.</param>
        /// <param name="data">The data to compress.</param>
        /// <param name="compressedSize">On return, the size of the compressed data.</param>
        /// <returns>The index of the resource that was added.</returns>
        public int Add(Stream inStream, byte[] data, out uint compressedSize)
        {
            var resourceIndex = NewResource();
            compressedSize = Compress(inStream, resourceIndex, data);
            return resourceIndex;
        }

        /// <summary>
        /// Adds a raw, pre-compressed resource to the cache.
        /// </summary>
        /// <param name="inStream">The stream open on the resource cache.</param>
        /// <param name="rawData">The raw data to add.</param>
        /// <returns>The index of the resource that was added.</returns>
        public int AddRaw(Stream inStream, byte[] rawData)
        {
            var resourceIndex = NewResource();
            ImportRaw(inStream, resourceIndex, rawData);
            return resourceIndex;
        }

        /// <summary>
        /// Extracts raw, compressed resource data.
        /// </summary>
        /// <param name="inStream">The stream open on the resource cache.</param>
        /// <param name="resourceIndex">The index of the resource to decompress.</param>
        /// <param name="compressedSize">Total size of the compressed data, including chunk headers.</param>
        /// <returns>The raw, compressed resource data.</returns>
        public byte[] ExtractRaw(Stream inStream, int resourceIndex, uint compressedSize)
        {
            if (resourceIndex < 0 || resourceIndex >= _resources.Count)
                throw new ArgumentOutOfRangeException("resourceIndex");

            var resource = _resources[resourceIndex];
            inStream.Position = resource.Offset;
            var result = new byte[compressedSize];
            inStream.Read(result, 0, result.Length);
            return result;
        }

        /// <summary>
        /// Overwrites a resource with raw, pre-compressed data.
        /// </summary>
        /// <param name="inStream">The stream open on the resource cache.</param>
        /// <param name="resourceIndex">The index of the resource to overwrite.</param>
        /// <param name="data">The raw, pre-compressed data to overwrite it with.</param>
        public void ImportRaw(Stream inStream, int resourceIndex, byte[] data)
        {
            if (resourceIndex < 0 || resourceIndex >= _resources.Count)
                throw new ArgumentOutOfRangeException("resourceIndex");

            var roundedSize = ResizeResource(inStream, resourceIndex, (uint)data.Length);
            var resource = _resources[resourceIndex];
            inStream.Position = resource.Offset;
            inStream.Write(data, 0, data.Length);
            StreamUtil.Fill(inStream, 0, (int)(roundedSize - data.Length)); // Padding
        }

        public void NullResource(Stream resourceStream, int resourceIndex)
        {
            if (resourceIndex < 0 || resourceIndex >= _resources.Count)
                throw new ArgumentOutOfRangeException("resourceIndex");

            var resource = _resources[resourceIndex];
            var writer = new BinaryWriter(resourceStream);

            if (IsResourceShared(resourceIndex))
                return;

            if (resource.Offset != uint.MaxValue  && resource.Size > 0)
            {
                StreamUtil.Copy(resourceStream, resource.Offset, resource.Offset, resourceStream.Length - resource.Offset);

                for (var i = 0; i < _resources.Count; i++)
                    if (_resources[i].Offset > resource.Offset)
                        _resources[i].Offset = (uint)(_resources[i].Offset - resource.Size);
            }
            
            resource.Offset = uint.MaxValue;
            resource.Size = 0;

            UpdateResourceTable(resourceStream);
        }

        /// <summary>
        /// Decompresses a resource.
        /// </summary>
        /// <param name="inStream">The stream open on the resource cache.</param>
        /// <param name="resourceIndex">The index of the resource to decompress.</param>
        /// <param name="compressedSize">Total size of the compressed data, including chunk headers.</param>
        /// <param name="outStream">The stream to write the decompressed resource data to.</param>
        public void Decompress(Stream inStream, int resourceIndex, uint compressedSize, Stream outStream)
        {
            if (resourceIndex < 0 || resourceIndex >= _resources.Count)
                throw new ArgumentOutOfRangeException("resourceIndex");

            var reader = new BinaryReader(inStream);
            var resource = _resources[resourceIndex];
            reader.BaseStream.Position = resource.Offset;

            // Compressed resources are split into chunks, so decompress each chunk until the complete data is decompressed
            var totalProcessed = 0U;
            compressedSize = Math.Min(compressedSize, resource.Size);
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
        /// <param name="inStream">The stream open on the resource data. It must have read/write access.</param>
        /// <param name="resourceIndex">The index of the resource to edit.</param>
        /// <param name="data">The data to compress.</param>
        /// <returns>The total size of the compressed resource in bytes.</returns>
        public uint Compress(Stream inStream, int resourceIndex, byte[] data)
        {
            if (resourceIndex < 0 || resourceIndex >= _resources.Count)
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
            var writer = new BinaryWriter(inStream);
            var roundedSize = ResizeResource(writer.BaseStream, resourceIndex, newSize);
            var resource = _resources[resourceIndex];
            inStream.Position = resource.Offset;
            var sizeRemaining = data.Length;
            foreach (var chunk in chunks)
            {
                var decompressedSize = Math.Min(sizeRemaining, MaxDecompressedBlockSize);
                writer.Write(decompressedSize);
                writer.Write(chunk.Length);
                writer.Write(chunk);
                sizeRemaining -= decompressedSize;
            }
            StreamUtil.Fill(inStream, 0, (int)(roundedSize - newSize)); // Padding
            return newSize;
        }

        private int NewResource()
        {
            var lastResource = (_resources.Count > 0) ? _resources[_resources.Count - 1] : null;
            var resourceIndex = _resources.Count;
            _resources.Add(new Resource
            {
                Offset = (_resources.Count == 0) ? 0x20 : (lastResource != null) ? lastResource.Offset + lastResource.Size : uint.MaxValue,
                Size = 0,
            });
            return resourceIndex;
        }

        private uint ResizeResource(Stream resourceStream, int resourceIndex, uint minSize)
        {
            var resource = _resources[resourceIndex];
            var roundedSize = ((minSize + 0xF) & ~0xFU); // Round up to a multiple of 0x10
            var sizeDelta = (int)(roundedSize - resource.Size);
            var endOffset = resource.Offset + resource.Size;
            StreamUtil.Copy(resourceStream, endOffset, endOffset + sizeDelta, resourceStream.Length - endOffset);
            resource.Size = roundedSize;

            // Update resource offsets
            for (var i = resourceIndex + 1; i < _resources.Count; i++)
                _resources[i].Offset = (uint)(_resources[i].Offset + sizeDelta);
            UpdateResourceTable(resourceStream);
            return roundedSize;
        }

        public bool IsResourceShared(int index) => _resources.Where(i => i.Offset == _resources[index].Offset).Count() > 1;

        private void Load(Stream stream)
        {
            // Read the file header
            var reader = new BinaryReader(stream);
            reader.BaseStream.Position = 0x4;
            var tableAddress = reader.ReadUInt32();
            var resourceCount = reader.ReadInt32();

            if (resourceCount == 0)
                return;

            var addresses = new List<uint>();
            var sizes = new List<uint>();

            reader.BaseStream.Position = tableAddress;

            for (var i = 0; i < resourceCount; i++)
            {
                var address = reader.ReadUInt32();

                if (!addresses.Contains(address) && (address != uint.MaxValue))
                    addresses.Add(address);

                _resources.Add(new Resource { Offset = address });
            }

            addresses.Sort((a, b) => a.CompareTo(b));

            for (var i = 0; i < addresses.Count - 1; i++)
                sizes.Add(addresses[i + 1] - addresses[i]);

            sizes.Add(tableAddress - addresses.Last());

            foreach (var resource in _resources)
            {
                if (resource.Offset == uint.MaxValue)
                    continue;

                resource.Size = sizes[addresses.IndexOf(resource.Offset)];
            }
        }

        public void UpdateResourceTable(Stream resourceStream)
        {
            // Assume the table is past the last resource
            uint tableOffset = 0xC;

            var writer = new BinaryWriter(resourceStream);

            if (_resources.Count != 0)
            {
                var lastResource = _resources[_resources.Count - 1];
                tableOffset = lastResource.Offset + lastResource.Size;
            }

            writer.BaseStream.Position = tableOffset;

            // Write each resource offset
            foreach (var resource in _resources)
                writer.Write(resource.Offset);

            var tableEndOffset = writer.BaseStream.Position;

            // Update the file header
            writer.BaseStream.Position = 0x4;
            writer.Write(tableOffset);
            writer.Write(_resources.Count);

            writer.BaseStream.SetLength(tableEndOffset);
        }

        private class Resource
        {
            public uint Offset { get; set; }

            public uint Size { get; set; }
        }
    }
}
