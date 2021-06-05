using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using System;
using TagTool.Tags;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using TagTool.Cache.Gen3;

namespace TagTool.Serialization
{
    public class Gen3SerializationContext : ISerializationContext
    {
        public GameCacheGen3 GameCache { get; private set; }
        public CachedTagGen3 Tag { get; private set; }
        public Stream Stream;

        public Gen3SerializationContext(Stream stream, GameCacheGen3 gameCache, CachedTagGen3 tag)
        {
            GameCache = gameCache;
            Tag = tag;
            Stream = stream;
        }

        public uint AddressToOffset(uint currentOffset, uint address)
        {
            return GameCache.TagAddressToOffset(address);
        }

        public EndianReader BeginDeserialize(TagStructureInfo info)
        {
            var reader = new EndianReader(Stream, GameCache.BaseMapFile.EndianFormat);
            reader.SeekTo(Tag.Offset);
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
