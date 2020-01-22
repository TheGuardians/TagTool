using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.BlamFile;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.Cache.Gen1
{
    [TagStructure(MaxVersion = CacheVersion.HaloXbox, Size = 0x24)]
    [TagStructure(MinVersion = CacheVersion.HaloPC, MaxVersion = CacheVersion.HaloPC, Size = 0x28)]
    public class TagCacheGen1Header : TagStructure
    {
        public uint CachedTagArrayAddress;
        public uint ScenarioTagID;
        public uint Unused;
        public uint TagCount;

        public uint ModelPartCount; // looks like vertex and index buffer to me
        [TagField(MinVersion = CacheVersion.HaloPC)]
        public uint ModelDataFileOffset;
        [TagField(MaxVersion = CacheVersion.HaloXbox)]
        public uint UnknownAddress;

        public uint ModelPartCount2;
        [TagField(MinVersion = CacheVersion.HaloPC)]
        public uint ModelVertexSize;
        [TagField(MaxVersion = CacheVersion.HaloXbox)]
        public uint UnknownAddress2;

        [TagField(MinVersion = CacheVersion.HaloPC)]
        public uint ModelDataSize;

        public Tag TagsTag;
    }

    public class TagCacheGen1 : TagCache
    {
        public TagCacheGen1Header Header;
        public uint BaseTagAddress;
        public List<CachedTagGen1> Tags = new List<CachedTagGen1>();

        public TagCacheGen1(EndianReader reader, MapFile mapFile)
        {
            var tagDataSectionOffset = mapFile.Header.TagDataOffset;
            reader.SeekTo(tagDataSectionOffset);

            var dataContext = new DataSerializationContext(reader);
            var deserializer = new TagDeserializer(mapFile.Version);
            Header = deserializer.Deserialize<TagCacheGen1Header>(dataContext);
            BaseTagAddress = Header.CachedTagArrayAddress & 0xFFFF0000;

            //
            // Read all tags offsets are all broken, need some proper look
            //

            reader.SeekTo(Header.CachedTagArrayAddress - BaseTagAddress + tagDataSectionOffset);
            

            for (int i = 0; i < Header.TagCount; i++)
            {
                var group = new TagGroup()
                {
                    Tag = new Tag(reader.ReadInt32()),
                    ParentTag = new Tag(reader.ReadInt32()),
                    GrandparentTag = new Tag(reader.ReadInt32())
                };
                var tagID = reader.ReadUInt32();
                var tagPathNameAddress = reader.ReadUInt32();
                var currentPos = reader.Position;
                string name = "";
                if (tagPathNameAddress != 0)
                {
                    reader.SeekTo(tagPathNameAddress - BaseTagAddress + tagDataSectionOffset);
                    name = reader.ReadNullTerminatedString();
                    reader.SeekTo(currentPos);
                }
                var tagDataAddress = reader.ReadUInt32();
                var weird2 = reader.ReadUInt32();
                var unused = reader.ReadUInt32();
                Tags.Add(new CachedTagGen1((int)(tagID & 0xFFFF), tagID, group, tagDataAddress, name));
            }
        }

        public override CachedTag AllocateTag(TagGroup type, string name = null)
        {
            throw new NotImplementedException();
        }

        public override CachedTag CreateCachedTag(int index, TagGroup group, string name = null)
        {
            throw new NotImplementedException();
        }

        public override CachedTag CreateCachedTag()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<CachedTag> TagTable { get => Tags; }

        public override CachedTag GetTag(uint ID) => GetTag((int)(ID & 0xFFFF));

        public override CachedTag GetTag(int index)
        {
            if (index < 0 || index >= Tags.Count)
                return null;
            else
                return Tags[index];
        }

        public override CachedTag GetTag(string name, Tag groupTag)
        {
            foreach (var tag in Tags)
            {
                if (groupTag == tag.Group.Tag && name == tag.Name)
                    return tag;
            }
            return null;
        }
    }
}
