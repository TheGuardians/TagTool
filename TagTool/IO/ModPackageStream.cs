using System.IO;

namespace TagTool.IO
{
    /// <summary>
	/// Manages the lifetime of the base cache stream to ensure it gets disposed with the mod package
	/// </summary>
    public class ModPackageStream : Stream
    {
        public readonly Stream ModStream;
        public readonly Stream BaseStream;

        public ModPackageStream(Stream baseStream, Stream sharedStream)
        {
            ModStream = baseStream;
            BaseStream = sharedStream;
        }

        public override bool CanRead => ModStream.CanRead;

        public override bool CanSeek => ModStream.CanSeek;

        public override bool CanWrite => ModStream.CanWrite;

        public override long Length => ModStream.Length;

        public override long Position { get => ModStream.Position; set => ModStream.Position = value; }

        public override void Flush() => ModStream.Flush();

        public override int Read(byte[] buffer, int offset, int count) => ModStream.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin) => ModStream.Seek(offset, origin);

        public override void SetLength(long value) => ModStream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count) => ModStream.Write(buffer, offset, count);

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ModStream?.Dispose();
                BaseStream?.Dispose();
            }    
        }
    }
}
