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
        public CacheFile BlamCache { get; private set; }
        public CacheFile.IndexItem BlamTag { get; private set; }

        public CacheSerializationContext(ref CacheFile blamCache, CacheFile.IndexItem blamTag)
        {
            if (blamCache.Version < CacheVersion.Halo3Retail)
            {
                var oldBlamCache = blamCache;
                var oldBlamTag = blamTag;

                if (blamTag.External)
                {
                    try
                    {
                        blamCache = CacheFileGen2.MainMenuCache;
                        blamTag = blamCache.IndexItems[blamTag.GroupTag, blamTag.Name];
                    }
                    catch (KeyNotFoundException)
                    {
                        blamCache = oldBlamCache;
                        blamTag = oldBlamTag;
                    }
                }
                if (blamTag.External)
                {
                    try
                    {
                        blamCache = CacheFileGen2.SharedCache;
                        blamTag = blamCache.IndexItems[blamTag.GroupTag, blamTag.Name];
                    }
                    catch (KeyNotFoundException)
                    {
                        blamCache = oldBlamCache;
                        blamTag = oldBlamTag;
                    }
                }
                if (blamTag.External)
                {
                    try
                    {
                        blamCache = CacheFileGen2.SinglePlayerSharedCache;
                        blamTag = blamCache.IndexItems[blamTag.GroupTag, blamTag.Name];
                    }
                    catch (KeyNotFoundException)
                    {
                        blamCache = oldBlamCache;
                        blamTag = oldBlamTag;
                    }
                }
                if (blamTag.External)
                    throw new KeyNotFoundException($"[{blamTag.GroupTag}] {blamTag.Name}");
            }

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
                    item.GroupTag,
                    item.ParentGroupTag,
                    item.GrandparentGroupTag,
                    BlamCache.CacheContext?.GetStringId(item.GroupName) ?? StringId.Invalid) :
                TagGroup.None;

            if (index == -1 || group.BelongsTo(Tag.Null))
                return null;

            return new CachedTagInstance(index, group);
        }
    }
}
