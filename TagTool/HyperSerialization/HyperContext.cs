using System;
using System.IO;
using TagTool.Cache;
using TagTool.IO;
using TagTool.Serialization;

namespace TagTool.HyperSerialization
{
	partial class HyperContext : ISerializationContext
	{
		private Stream RootStream;
		private GameCacheContext CacheContext;

		public HyperContext(Stream rootStream, GameCacheContext cacheContext)
		{
			this.RootStream = rootStream;
			this.CacheContext = cacheContext;
		}

		public void BeginSerialize(TagStructureInfo info)
		{
			throw new NotImplementedException();
		}

		public void EndSerialize(TagStructureInfo info, byte[] data, uint mainStructOffset)
		{
			throw new NotImplementedException();
		}

		public EndianReader BeginDeserialize(TagStructureInfo info)
		{
			throw new NotImplementedException();
		}

		public void EndDeserialize(TagStructureInfo info, object obj)
		{
			throw new NotImplementedException();
		}

		public uint AddressToOffset(uint currentOffset, uint address)
		{
			throw new NotImplementedException();
		}

		public CachedTagInstance GetTagByIndex(int index)
		{
			throw new NotImplementedException();
		}

		public IDataBlock CreateBlock()
		{
			throw new NotImplementedException();
		}
	}
}
