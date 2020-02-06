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

namespace TagTool.Cache.Gen2
{
    [TagStructure(Size = 0x20)]
    public class TagCacheGen2Header : TagStructure
    {
        public uint TagGroupsOffset;
        public int TagGroupCount;
        public uint TagsOffset;
        public DatumIndex ScenarioHandle;
        public DatumIndex GlobalsHandle;
        public int CRC;
        public int TagCount;
        public Tag Tags;
    }


    public class TagCacheGen2 : TagCache
    {

        public TagCacheGen2Header Header;
        public uint BaseTagAddress;
        public List<CachedTagGen2> Tags = new List<CachedTagGen2>();
        public Dictionary<Tag, TagGroup> TagGroups = new Dictionary<Tag, TagGroup>();
        

        public TagCacheGen2(EndianReader reader, MapFile mapFile)
        {
            var tagDataSectionOffset = mapFile.Header.TagDataOffset;
            reader.SeekTo(tagDataSectionOffset);

            var dataContext = new DataSerializationContext(reader);
            var deserializer = new TagDeserializer(mapFile.Version);
            Header = deserializer.Deserialize<TagCacheGen2Header>(dataContext);

            //seek to the tag groups offset, seems to be contiguous to the header
            reader.SeekTo(tagDataSectionOffset + Header.TagGroupsOffset);   // TODO: check how halo 2 xbox uses this

            for(int i = 0; i < Header.TagGroupCount; i++)
            {
                var tag = new Tag(reader.ReadInt32());
                var group = new TagGroup()
                {
                    Tag = tag,
                    ParentTag = new Tag(reader.ReadInt32()),
                    GrandparentTag = new Tag(reader.ReadInt32()),
                    Name = StringId.Invalid // has no stringids in tag groups
                };
                TagGroups[tag] = group;
            }

            reader.SeekTo(tagDataSectionOffset + Header.TagsOffset);

            for (int i = 0; i < Header.TagCount; i++)
            {
                var tag = new Tag(reader.ReadInt32());

                uint ID = reader.ReadUInt32();
                uint address = reader.ReadUInt32();
                int size = reader.ReadInt32();
                if (tag.Value == -1 || tag.Value == 0 || size == -1 || address == 0xFFFFFFFF || ID == 0 || ID == 0xFFFFFFFF)
                    Tags.Add(null);
                else
                    Tags.Add(new CachedTagGen2((int)(ID & 0xFFFF), ID, TagGroups[tag], address, size, null));
            }

            reader.SeekTo(mapFile.Header.TagNamesOffsetsTableAddress);
            var tagNamesOffset = new int[Header.TagCount];
            for (int i = 0; i < Header.TagCount; i++)
                tagNamesOffset[i] = reader.ReadInt32();


            reader.SeekTo(mapFile.Header.TagNamesBufferAddress);

            for (int i = 0; i < tagNamesOffset.Length; i++)
            {
                if (Tags[i] == null)
                    continue;

                if (tagNamesOffset[i] == -1)
                    continue;

                reader.SeekTo(tagNamesOffset[i] + mapFile.Header.TagNamesBufferAddress);

                Tags[i].Name = reader.ReadNullTerminatedString();
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
