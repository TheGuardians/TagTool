using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Legacy.Base;
using System;
using TagTool.Tags;

namespace TagTool.Serialization
{
    public class CacheSerializationContext : ISerializationContext
    {
        public GameCacheContext CacheContext { get; }
        public CacheFile BlamCache { get; }
        public CacheFile.IndexItem BlamTag { get; }

        public CacheSerializationContext(GameCacheContext cacheContext, CacheFile blamCache, CacheFile.IndexItem blamTag)
        {
            CacheContext = cacheContext;
            BlamCache = blamCache;
            BlamTag = blamTag;
        }

        public uint AddressToOffset(uint currentOffset, uint address)
        {
            return address - (uint)BlamCache.Magic;
        }

        public EndianReader BeginDeserialize(TagStructureInfo info)
        {
            BlamCache.Reader.BaseStream.Position = BlamTag.Offset;
            return BlamCache.Reader;
        }

        public void BeginSerialize(TagStructureInfo info)
        {
            throw new NotImplementedException();
        }

        public IDataBlock CreateBlock()
        {
            throw new NotImplementedException();
        }

        public void EndDeserialize(TagStructureInfo info, object obj)
        {
        }

        public void EndSerialize(TagStructureInfo info, byte[] data, uint mainStructOffset)
        {
            throw new NotImplementedException();
        }

        public CachedTagInstance GetTagByIndex(int index)
        {
            var item = BlamCache.IndexItems.Find(i => i.ID == index);

            var group = item != null ?
                new TagGroup(new Tag(item.ClassCode), new Tag(item.ParentClass), new Tag(item.ParentClass2), CacheContext == null ? StringId.Null : CacheContext.GetStringId(item.ClassName)) :
                TagGroup.Null;

            return new CachedTagInstance(index, group);
        }
    }
}
