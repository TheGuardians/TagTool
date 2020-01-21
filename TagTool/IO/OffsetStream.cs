using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.IO
{
    /// <summary>
	///  A wrapper for a Stream which shifts its seek offsets by a specified amount.
	/// </summary>
	public class OffsetStream : Stream
    {
        private readonly Stream BaseStream;
        public long Offset { get; }

        /// <summary>
        /// Constructs a new OffsetStream based off of another stream.
        /// </summary>
        /// <param name="baseStream">The underlying stream.</param>
        /// <param name="offset">The offset that should correspond to position 0 in the base stream.</param>
        public OffsetStream(Stream baseStream, long offset)
        {
            BaseStream = baseStream;
            Offset = offset;
        }

        public override bool CanRead
        {
            get { return BaseStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return BaseStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return BaseStream.CanWrite; }
        }

        public override long Length
        {
            get { return BaseStream.Length - Offset; }
        }

        public override long Position
        {
            get { return BaseStream.Position - Offset; }
            set { BaseStream.Position = value + Offset; }
        }

        public override bool CanTimeout
        {
            get { return BaseStream.CanTimeout; }
        }

        public override int ReadTimeout
        {
            get { return BaseStream.ReadTimeout; }
            set { BaseStream.ReadTimeout = value; }
        }

        public override int WriteTimeout
        {
            get { return BaseStream.WriteTimeout; }
            set { BaseStream.WriteTimeout = value; }
        }

        public override void Flush()
        {
            BaseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return BaseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Begin)
                return BaseStream.Seek(offset + Offset, SeekOrigin.Begin);
            return BaseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            BaseStream.SetLength(value + Offset);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            BaseStream.Write(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                BaseStream.Dispose();
        }
    }
}
