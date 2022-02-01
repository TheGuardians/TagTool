using System;
using System.Collections;
using System.IO;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.Cache.Monolithic
{
    public class TagSerializationContextMonolithic : ISerializationContext
    {
        public GameCacheMonolithic Cache { get; private set; }
        public CachedTagMonolithic Tag { get; private set; }
        public Stream Stream;

        private MonolithicTagPersistContext FixupContext;

        public TagSerializationContextMonolithic(Stream stream, GameCacheMonolithic gameCache, CachedTagMonolithic tag)
        {
            Cache = gameCache;
            Tag = tag;
            Stream = stream;
        }

        public uint AddressToOffset(uint currentOffset, uint address)
        {
            var resourceAddress = new CacheAddress(address);
            return (uint)resourceAddress.Offset;
        }

        public EndianReader BeginDeserialize(TagStructureInfo info)
        {
            var tagData = Cache.Backend.ExtractTagRaw(Tag.Index);
            if (tagData == null)
                return new EndianReader(new MemoryStream());

            var singleFileTagReader = new SingleTagFileReader(new PersistChunkReader(new MemoryStream(tagData), Cache.Endianness));

            // read the lauyout
            if (!Cache.TagLayoutCache.TryGetValue(info.GroupTag, out TagLayout layout))
                Cache.TagLayoutCache.Add(info.GroupTag, layout = singleFileTagReader.ReadLayout());
            else
                singleFileTagReader.Reader.ReadNextChunk(); // skip the next chunk

            // read and fixup the data
            FixupContext = new MonolithicTagPersistContext(Cache);
            var newTagData = singleFileTagReader.ReadAndFixupData(Tag.ID, layout, FixupContext, out uint mainStructOffset);

            var newTagDataReader = new EndianReader(new MemoryStream(newTagData), Cache.Endianness);
            newTagDataReader.SeekTo(mainStructOffset);
            return newTagDataReader;
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
            FixupPageableResources(obj, info.Version, info.CachePlatform);
            FixupContext = null;
        }

        void FixupPageableResources(object data, CacheVersion version, CachePlatform platform)
        {
            switch (data)
            {
                case TagResourceReference resourceRef:
                    if (FixupContext.TagResources.TryGetValue(resourceRef.Gen3ResourceID, out var xsyncState))
                        resourceRef.XSyncState = xsyncState; // TODO: consider making storing in the existing PageableResource field
                    break;
                case TagStructure tagStruct:
                    foreach (var field in tagStruct.GetTagFieldEnumerable(version, platform))
                        FixupPageableResources(field.GetValue(data), version, platform);
                    break;
                case byte[] byteArray:
                    break;
                case IList list:
                    foreach (var element in list)
                        FixupPageableResources(element, version, platform);
                    break;
            }
        }

        public void EndSerialize(TagStructureInfo info, byte[] data, uint mainStructOffset)
        {
            throw new NotImplementedException();
        }

        public CachedTag GetTagByIndex(int index)
        {
            var tag = Cache.TagCache.GetTag((uint)index);

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
    }
}
