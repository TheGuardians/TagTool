using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using System;
using TagTool.Tags;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using TagTool.Cache.Gen2;

namespace TagTool.Serialization
{
    public class Gen2SerializationContext : ISerializationContext
    {
        public GameCacheGen2 GameCache { get; private set; }
        public CachedTagGen2 Tag { get; private set; }
        public Stream Stream;

        public Gen2SerializationContext(Stream stream, GameCacheGen2 gameCache, CachedTagGen2 tag)
        {
            GameCache = gameCache;
            Tag = tag;
            Stream = stream;
        }

        public uint AddressToOffset(uint currentOffset, uint address)
        {
            return (uint)(address - GameCache.TagCacheGen2.BaseTagAddress + GameCache.BaseMapFile.Header.TagsHeaderAddress32);
        }

        public EndianReader BeginDeserialize(TagStructureInfo info)
        {
            var reader = new EndianReader(Stream, GameCache.BaseMapFile.EndianFormat);
            reader.SeekTo(Tag.Offset - GameCache.TagCacheGen2.BaseTagAddress + GameCache.BaseMapFile.Header.TagsHeaderAddress32);
            return reader;
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

        public CachedTag GetTagByIndex(int index)
        {
            var tag = GameCache.TagCache.GetTag((uint)index);

            var group = (tag != null) ? tag.Group : TagGroup.None;

            if (index == -1 || group == TagGroup.None)
                return null;

            return tag;
        }

        public CachedTag GetTagByName(TagGroup group, string name)
        {
            throw new NotImplementedException();
        }

        public void AddResourceBlock(int count, CacheAddress address, IList block)
        {
            throw new NotImplementedException();
        }

        public void AddTagReference(CachedTag referencedTag)
        {
            throw new NotImplementedException();
        }
    }
}
