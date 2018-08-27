using System;
using System.IO;
using System.Threading;

namespace TagTool.HyperSerialization
{
	class StreamMultiplxer : Stream
	{
		static Mutex GlobalMuticesMutex = new Mutex();
		//TODO: Weak Dictionary
		static WeakDictionary<Stream, Mutex> GlobalMutices = new WeakDictionary<Stream, Mutex>();
		Mutex mutex;

		public Stream ParentStream;
		public StreamMultiplxer(Stream parent_stream)
		{
			ParentStream = parent_stream;
			switch (parent_stream)
			{
				case StreamMultiplxer multiplexd_stream:
					ParentStream = multiplexd_stream.ParentStream;
					break;
			}

			GlobalMuticesMutex.WaitOne();
			mutex = GlobalMutices.ContainsKey(ParentStream) ? GlobalMutices[ParentStream] : GlobalMutices[ParentStream] = new Mutex();
			GlobalMuticesMutex.ReleaseMutex();
		}

		public override long Position { get; set; }

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (Position + offset + count < 0xFFFFFFFF)
			{
				// if you can guarantee parent_stream.position is always 0
				if (ParentStream.Position != 0)
				{
					throw new Exception("The fuk");
				}

				// not sure if read is thread safe
				var result = ParentStream.Read(buffer, (int)Position + offset, count);
				return result;
			}
			else
			{
				mutex.WaitOne();
				var old_position = ParentStream.Position;
				ParentStream.Position = Position;
				var result = ParentStream.Read(buffer, offset, count);
				ParentStream.Position = old_position; // restore pos
				mutex.ReleaseMutex();
				return result;
			}
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			switch (origin)
			{
				case SeekOrigin.Begin:
					this.Position = offset;
					break;
				case SeekOrigin.End:
					throw new NotImplementedException();
					break;
				case SeekOrigin.Current:
					this.Position += offset;
					break;
			}
			return this.Position;
		}

		public override bool CanRead => true;

		public override bool CanSeek => true;

		public override bool CanWrite => false;

		public override long Length => ParentStream.Length;

		public override void Flush()
		{
			ParentStream.Flush();
		}

		public override void SetLength(long value)
		{
			ParentStream.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}
	}
}
