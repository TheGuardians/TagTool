using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using System;
using TagTool.Tags;
using System.Collections.Generic;

namespace TagTool.Serialization
{
    public class CacheSerializationContext : ISerializationContext
    {
        public CacheFile BlamCache { get; }
        public CacheFile.IndexItem BlamTag { get; }

        public CacheSerializationContext(CacheFile blamCache, CacheFile.IndexItem blamTag)
        {
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

            var group = (item != null) ?
                new TagGroup(
                    item.ClassCode == null ? Tag.Null : new Tag(item.ClassCode),
                    item.ParentClass == null ? Tag.Null : new Tag(item.ParentClass),
                    item.ParentClass2 == null ? Tag.Null : new Tag(item.ParentClass2),
                    BlamCache.CacheContext?.GetStringId(item.ClassName) ?? StringId.Invalid) :
                TagGroup.Null;

            return new CachedTagInstance(index, group);
        }
    }
}
