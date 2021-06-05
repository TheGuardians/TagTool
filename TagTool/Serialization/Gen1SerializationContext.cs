using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using System;
using TagTool.Tags;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using TagTool.Cache.Gen1;

namespace TagTool.Serialization
{
    public class Gen1SerializationContext : ISerializationContext
    {
        public GameCacheGen1 GameCache { get; private set; }
        public CachedTagGen1 Tag { get; private set; }
        public Stream Stream;

        public Gen1SerializationContext(Stream stream, GameCacheGen1 gameCache, CachedTagGen1 tag)
        {
            GameCache = gameCache;
            Tag = tag;
            Stream = stream;
        }

        public uint AddressToOffset(uint currentOffset, uint address)
        {
            if (Tag.AddressToOffsetOverride != null)
                return Tag.AddressToOffsetOverride(currentOffset, address);

            return (uint)(address - GameCache.TagCacheGen1.BaseTagAddress + GameCache.BaseMapFile.Header.GetTagTableHeaderOffset());
        }

        public EndianReader BeginDeserialize(TagStructureInfo info)
        {
            var reader = new EndianReader(Stream, GameCache.BaseMapFile.EndianFormat);
            reader.SeekTo(AddressToOffset(0, Tag.Offset));
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

            var group = (tag != null) ? tag.Group : new TagGroup();

            if (index == -1 || group.IsNull())
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

        public CacheVersion GetVersion() => GameCache.Version;

        public CachePlatform GetCachePlatform() => GameCache.Platform;

        public PlatformType GetPlatformType() => CacheVersionDetection.GetPlatformType(GameCache.Platform);
    }
}
