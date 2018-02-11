using System;
using System.IO;

namespace TagTool.IO
{
    public static class StreamUtil
    {
        /// <summary>
        /// Copies data between two different streams.
        /// </summary>
        /// <param name="input">The stream to read from.</param>
        /// <param name="output">The stream to copy the read data to.</param>
        /// <param name="size">The size of the data to copy.</param>
        public static void Copy(Stream input, Stream output, int size)
        {
            const int bufferSize = 0x1000;
            var buffer = new byte[bufferSize];
            while (size > 0)
            {
                var read = input.Read(buffer, 0, Math.Min(bufferSize, size));
                output.Write(buffer, 0, read);
                size -= bufferSize;
            }
        }

        /// <summary>
        /// Copies data between two locations in the same stream.
        /// The source and destination areas may overlap.
        /// </summary>
        /// <param name="stream">The stream to copy data in.</param>
        /// <param name="originalPos">The position of the block of data to copy.</param>
        /// <param name="targetPos">The position to copy the block to.</param>
        /// <param name="size">The number of bytes to copy.</param>
        public static void Copy(Stream stream, long originalPos, long targetPos, long size)
        {
            if (size == 0)
                return;
            if (size < 0)
                throw new ArgumentException("The size of the data to copy must be >= 0");

            const int bufferSize = 0x1000;
            var buffer = new byte[bufferSize];
            var remaining = size;
            while (remaining > 0)
            {
                var read = (int)Math.Min(bufferSize, remaining);

                if (targetPos > originalPos)
                    stream.Position = originalPos + remaining - read; // Seek backward
                else
                    stream.Position = originalPos + size - remaining; // Seek forward

                stream.Read(buffer, 0, read);

                if (targetPos > originalPos)
                    stream.Position = targetPos + remaining - read; // Seek backward
                else
                    stream.Position = targetPos + size - remaining; // Seek forward

                stream.Write(buffer, 0, read);
                remaining -= read;
            }
        }

        public static void Copy(EndianReader input, EndianWriter output)
        {
            const int BufferSize = 0x1000;

            var buffer = new byte[BufferSize];
            int read;

            while ((read = input.ReadBlock(buffer, 0, BufferSize)) > 0)
                output.WriteBlock(buffer, 0, read);
        }

        public static void Copy(EndianReader input, EndianWriter output, int size)
        {
            const int BufferSize = 0x1000;

            var buffer = new byte[BufferSize];

            while (size > 0)
            {
                int read = input.ReadBlock(buffer, 0, Math.Min(BufferSize, size));
                output.WriteBlock(buffer, 0, read);
                size -= BufferSize;
            }
        }

        /// <summary>
        /// Inserts space into a stream by copying everything back by a certain number of bytes.
        /// </summary>
        /// <param name="stream">The stream to insert space into.</param>
        /// <param name="size">The size of the space to insert.</param>
        /// <param name="fill">The byte to fill the inserted space with. See <see cref="Fill" />.</param>
        public static void Insert(Stream stream, int size, byte fill)
        {
            if (size == 0)
                return;
            if (size < 0)
                throw new ArgumentException("The size of the data to insert must be >= 0");

            var startPos = stream.Position;
            if (startPos < stream.Length)
            {
                Copy(stream, startPos, startPos + size, stream.Length - startPos);
                stream.Position = startPos;
            }
            Fill(stream, fill, size);
        }

        /// <summary>
        /// Removes bytes from a stream, moving everything after the bytes to the current position and decreasing the stream length.
        /// </summary>
        /// <param name="stream">The stream to remove bytes from.</param>
        /// <param name="size">The number of bytes to remove.</param>
        /// <exception cref="System.ArgumentException">The size of the data to remove must be >= 0</exception>
        public static void Remove(Stream stream, int size)
        {
            if (size == 0)
                return;
            if (size < 0)
                throw new ArgumentException("The size of the data to remove must be >= 0");

            var startPos = stream.Position;
            if (startPos + size >= stream.Length)
            {
                stream.SetLength(startPos);
                return;
            }
            Copy(stream, startPos + size, startPos, stream.Length - startPos - size);
            stream.SetLength(stream.Length - size);
        }

        /// <summary>
        /// Fills a section of a stream with a repeating byte.
        /// </summary>
        /// <param name="stream">The stream to fill a section of.</param>
        /// <param name="b">The byte to fill the section with.</param>
        /// <param name="size">The size of the section to fill.</param>
        public static void Fill(Stream stream, byte b, int size)
        {
            if (size == 0)
                return;
            if (size < 0)
                throw new ArgumentException("The size of the data to insert must be >= 0");

            const int bufferSize = 0x1000;
            var buffer = new byte[bufferSize];
            var pos = stream.Position;
            var endPos = pos + size;

            // Fill the buffer
            if (b != 0)
            {
                for (var i = 0; i < buffer.Length; i++)
                    buffer[i] = b;
            }

            // Write it
            while (pos < endPos)
            {
                stream.Write(buffer, 0, (int)Math.Min(endPos - pos, bufferSize));
                pos += bufferSize;
            }
        }

        /// <summary>
        /// Aligns the position of a stream to a power of two, padding the stream with zeroes.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="align">The power of two to align to.</param>
        public static void Align(Stream stream, int align)
        {
            var currentPos = stream.Position;
            var alignedPos = (currentPos + align - 1) & ~(align - 1);
            if (alignedPos > currentPos)
                Insert(stream, (int)(alignedPos - currentPos), 0);
        }
    }
}
