using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using System;
using TagTool.Tags;
using System.Collections.Generic;
using System.IO;

namespace TagTool.Serialization
{
    public class CacheSerializationContext : ISerializationContext
    {
        public CacheFile BlamCache { get; private set; }
        public CacheFile.IndexItem BlamTag { get; private set; }

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
            if (BlamCache.Version < CacheVersion.Halo3Retail)
            {
                var oldBlamCache = BlamCache;
                var oldBlamTag = BlamTag;

                if (BlamTag.External)
                {
                    try
                    {
                        BlamCache = CacheFileGen2.MainMenuCache;
                        BlamTag = BlamCache.IndexItems[BlamTag.GroupTag, BlamTag.Filename];
                    }
                    catch (KeyNotFoundException)
                    {
                        BlamCache = oldBlamCache;
                        BlamTag = oldBlamTag;
                    }
                }
                if (BlamTag.External)
                {
                    try
                    {
                        BlamCache = CacheFileGen2.SharedCache;
                        BlamTag = BlamCache.IndexItems[BlamTag.GroupTag, BlamTag.Filename];
                    }
                    catch (KeyNotFoundException)
                    {
                        BlamCache = oldBlamCache;
                        BlamTag = oldBlamTag;
                    }
                }
                if (BlamTag.External)
                {
                    try
                    {
                        BlamCache = CacheFileGen2.SinglePlayerSharedCache;
                        BlamTag = BlamCache.IndexItems[BlamTag.GroupTag, BlamTag.Filename];
                    }
                    catch (KeyNotFoundException)
                    {
                        BlamCache = oldBlamCache;
                        BlamTag = oldBlamTag;
                    }
                }
                if (BlamTag.External)
                    throw new KeyNotFoundException($"[{BlamTag.GroupTag}] {BlamTag.Filename}");
            }

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
                TagGroup.Null;

            return new CachedTagInstance(index, group);
        }
    }
}
