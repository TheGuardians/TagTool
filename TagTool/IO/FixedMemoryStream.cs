using System;
using System.IO;

namespace TagTool.IO
{
    // Prefer using this over MemoryStream. It acts just like a normal MemoryStream,
    // except it can take a slice of a byte array without the need for an intermediate copy.
    public class FixedMemoryStream : Stream
    {
        private Memory<byte> _buffer;
        long _position;

        public FixedMemoryStream(Memory<byte> buffer)
        {
            _buffer = buffer;
            _position = 0;
        }

        public FixedMemoryStream(byte[] buffer) : this(buffer.AsMemory()) { }

        public FixedMemoryStream(byte[] buffer, int offset, int length) : this(buffer.AsMemory(offset, length)) { }

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => true;

        public override long Length => _buffer.Length;

        public override long Position { get => _position; set => _position = value; }

        public override void Flush() { }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Begin)
                _position = offset;
            else if (origin == SeekOrigin.Current)
                _position += offset;
            else if (origin == SeekOrigin.End)
                _position = _buffer.Length - offset;

            return _position;
        }

        public override void SetLength(long value) => throw new NotSupportedException();

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_position >= _buffer.Length)
                return 0;

            int length = Math.Min((int)(_buffer.Length - _position), count);
            if (length >= 0)
            {
                _buffer.Slice((int)_position, length).CopyTo(buffer.AsMemory(offset));
                _position += length;
            }
            return length;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (_position + count > _buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(count), "Tried to write beyond the end of the stream");

            buffer.AsMemory(offset, count).CopyTo(_buffer.Slice((int)_position));
            _position += count;
        }
    }
}
