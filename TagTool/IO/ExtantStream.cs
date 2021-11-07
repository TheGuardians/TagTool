using System.Diagnostics;
using System.IO;

namespace TagTool.IO
{
    /// <summary>
	///  A wrapper for a Stream which prevents it from being closed or disposed by code unless explicitely required by the user.
	/// </summary>
	public class ExtantStream : Stream
    {
        private readonly Stream BaseStream;
        private bool CanBeDisposed;

        public ExtantStream(Stream baseStream)
        {
            BaseStream = baseStream;
            CanBeDisposed = false;
        }

        public void SetDisposable(bool canBeDisposed)
        {
            CanBeDisposed = canBeDisposed;
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
            get { return BaseStream.Length; }
        }

        public override long Position
        {
            get { return BaseStream.Position; }
            set { BaseStream.Position = value; }
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
                return BaseStream.Seek(offset, SeekOrigin.Begin);
            return BaseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            BaseStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            BaseStream.Write(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            if (CanBeDisposed)
            {
                if (disposing)
                    BaseStream.Dispose();
            }
            else
                Debug.WriteLine("Dispose was attempted on extant stream not signaled to be disposed.");
        }

        public override void Close()
        {
            if (CanBeDisposed)
            {
                base.Close();
            }
            else
                Debug.WriteLine("Dispose was attempted on extant stream not signaled to be disposed.");
        }
    }
}
