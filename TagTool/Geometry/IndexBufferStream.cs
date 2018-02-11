using TagTool.IO;
using System;
using System.IO;

namespace TagTool.Geometry
{
    /// <summary>
    /// Reads and writes index buffer data.
    /// </summary>
    public class IndexBufferStream
    {
        private readonly Stream _stream;
        private readonly EndianReader _reader;
        private readonly EndianWriter _writer;
        private readonly long _baseOffset;

        /// <summary>
        /// Creates an index buffer stream.
        /// </summary>
        /// <param name="stream">The base stream to use. It must point to the beginning of the index buffer.</param>
        /// <param name="endianFormat">The endian format of the index buffer.</param>
        public IndexBufferStream(Stream stream, EndianFormat endianFormat = EndianFormat.LittleEndian)
        {
            _stream = stream;
            _reader = new EndianReader(_stream, endianFormat);
            _writer = new EndianWriter(_stream, endianFormat);
            _baseOffset = _stream.Position;
        }

        /// <summary>
        /// The position of the stream in units of indices.
        /// </summary>
        public uint Position
        {
            get { return (uint)(_stream.Position - _baseOffset) / sizeof(ushort); }
            set { _stream.Position = _baseOffset + value * sizeof(ushort); }
        }

        /// <summary>
        /// Reads an index and advances the stream.
        /// </summary>
        /// <returns>The index that was read.</returns>
        public ushort ReadIndex() => _reader.ReadUInt16();

        /// <summary>
        /// Writes an index and advances the stream.
        /// </summary>
        /// <param name="index">The index to write.</param>
        public void WriteIndex(ushort index) => _writer.Write(index);

        /// <summary>
        /// Reads indices into an array and advances the stream.
        /// </summary>
        /// <param name="buffer">The buffer to read into.</param>
        /// <param name="offset">The offset into the buffer to start storing at.</param>
        /// <param name="count">The number of indices to read.</param>
        public void ReadIndices(ushort[] buffer, uint offset, uint count)
        {
            for (uint i = 0; i < count; i++)
                buffer[i + offset] = ReadIndex();
        }

        /// <summary>
        /// Reads an array of indices.
        /// </summary>
        /// <param name="count">The number of indices to read.</param>
        /// <returns>The indices that were read.</returns>
        public ushort[] ReadIndices(uint count)
        {
            var result = new ushort[count];
            ReadIndices(result, 0, count);
            return result;
        }

        /// <summary>
        /// Writes indices from an array and advances the stream.
        /// </summary>
        /// <param name="buffer">The buffer of indices to write.</param>
        /// <param name="offset">The offset into the buffer to start writing at.</param>
        /// <param name="count">The number of indices to write.</param>
        public void WriteIndices(ushort[] buffer, uint offset, uint count)
        {
            for (uint i = 0; i < count; i++)
                WriteIndex(buffer[i + offset]);
        }

        /// <summary>
        /// Writes indices from an array and advances the stream.
        /// </summary>
        /// <param name="buffer">The indices to write.</param>
        public void WriteIndices(ushort[] buffer) =>
            WriteIndices(buffer, 0, (uint)buffer.LongLength);

        /// <summary>
        /// Reads a triangle strip and converts it into a triangle list.
        /// Degenerate triangles will be included and must be discarded manually.
        /// </summary>
        /// <param name="indexCount">The number of indices in the strip. Cannot be 1 or 2.</param>
        /// <returns>The triangle strip converted into a triangle list.</returns>
        public ushort[] ReadTriangleStrip(uint indexCount)
        {
            if (indexCount == 0)
                return new ushort[0];
            if (indexCount < 3)
                throw new InvalidOperationException("Invalid triangle strip index buffer");

            var triangleCount = indexCount - 2;
            var result = new ushort[triangleCount * 3];
            var previous = ReadIndices(2);
            for (var i = 0; i < triangleCount; i++)
            {
                var index = ReadIndex();

                // Swap the order of the first two indices every other triangle
                // in order to preserve the winding order
                if (i % 2 == 0)
                {
                    result[i * 3] = previous[0];
                    result[i * 3 + 1] = previous[1];
                }
                else
                {
                    result[i * 3] = previous[1];
                    result[i * 3 + 1] = previous[0];
                }

                result[i * 3 + 2] = index;
                previous[0] = previous[1];
                previous[1] = index;
            }
            return result;
        }
    }
}
