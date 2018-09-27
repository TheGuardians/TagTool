using System;
using System.IO;
using TagTool.Cache;
using TagTool.IO;
using TagTool.Tags;

namespace TagTool.Serialization
{
    partial class HyperSerializer : ISerializationContext
	{
		private Stream RootStream;
		private GameCacheContext CacheContext;
		private bool DoesBranching;

		public HyperSerializer(Stream rootStream, GameCacheContext cacheContext, bool doesBranching = false)
		{
			RootStream = rootStream;
			CacheContext = cacheContext;
			DoesBranching = doesBranching;
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

        public CachedTagInstance GetTagByName(TagGroup group, string name)
        {
            throw new NotImplementedException();
        }

        public IDataBlock CreateBlock()
		{
			throw new NotImplementedException();
		}
    }
}
