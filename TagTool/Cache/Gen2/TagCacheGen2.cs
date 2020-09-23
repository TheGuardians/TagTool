using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public uint ScenarioID;
        public uint GlobalsID;
        public int CRC;
        public int TagCount;
        public Tag Tags;
    }


    public class TagCacheGen2 : TagCache
    {
        /// <summary>
        /// Address in memory (xbox) of the tag data. For Halo 2 Vista, this values turns out to be 0. Every address in the tag data is converted to an offset using this value.
        /// </summary>
        public uint BaseTagAddress;

        public TagCacheGen2Header Header;
        public List<CachedTagGen2> Tags = new List<CachedTagGen2>();
        public Dictionary<Tag, CachedTagGen2> HardcodedTags = new Dictionary<Tag, CachedTagGen2>();

        public TagCacheGen2(EndianReader reader, MapFile mapFile)
        {
            Version = mapFile.Version;
            TagDefinitions = new TagDefinitionsGen2();
            var tagDataSectionOffset = mapFile.Header.TagsHeaderAddress32;
            reader.SeekTo(tagDataSectionOffset);

            var dataContext = new DataSerializationContext(reader);
            var deserializer = new TagDeserializer(mapFile.Version);
            Header = deserializer.Deserialize<TagCacheGen2Header>(dataContext);

            BaseTagAddress = (Header.TagGroupsOffset - 0x20);

            //
            // Read tag groups
            //

            //seek to the tag groups offset, seems to be contiguous to the header
            reader.SeekTo(tagDataSectionOffset + Header.TagGroupsOffset - BaseTagAddress);   // TODO: check how halo 2 xbox uses this

            for(int i = 0; i < Header.TagGroupCount; i++)
            {
                var group = new TagGroupGen2(new Tag(reader.ReadInt32()), new Tag(reader.ReadInt32()), new Tag(reader.ReadInt32()));
                if (!TagDefinitions.TagDefinitionExists(group))
                    Debug.WriteLine($"Warning: tag definition for {group} does not exists!");
            }

            //
            // Read cached tags
            //

            reader.SeekTo(tagDataSectionOffset + Header.TagsOffset - BaseTagAddress);

            for (int i = 0; i < Header.TagCount; i++)
            {
                var tag = new Tag(reader.ReadInt32());

                uint ID = reader.ReadUInt32();
                uint address = reader.ReadUInt32();
                int size = reader.ReadInt32();
                if (tag.Value == -1 || tag.Value == 0 || size == -1 || address == 0xFFFFFFFF || ID == 0 || ID == 0xFFFFFFFF)
                    Tags.Add(null);
                else
                    Tags.Add(new CachedTagGen2((int)(ID & 0xFFFF), ID, (TagGroupGen2)TagDefinitions.GetTagGroupFromTag(tag), address, size, null, address == 0));
            }

            reader.SeekTo(mapFile.Header.TagNameIndicesOffset);
            var tagNamesOffset = new int[Header.TagCount];
            for (int i = 0; i < Header.TagCount; i++)
                tagNamesOffset[i] = reader.ReadInt32();

            //
            // Read tag names
            //

            reader.SeekTo(mapFile.Header.TagNamesBufferOffset);

            for (int i = 0; i < tagNamesOffset.Length; i++)
            {
                if (Tags[i] == null)
                    continue;

                if (tagNamesOffset[i] == -1)
                    continue;

                reader.SeekTo(tagNamesOffset[i] + mapFile.Header.TagNamesBufferOffset);

                Tags[i].Name = reader.ReadNullTerminatedString();
            }

            //
            // Set hardcoded tags from the header
            //

            var scnrTag = GetTag(Header.ScenarioID);
            HardcodedTags[scnrTag.Group.Tag] = (CachedTagGen2)scnrTag;
            var globalTag = GetTag(Header.GlobalsID);
            HardcodedTags[globalTag.Group.Tag] = (CachedTagGen2)globalTag;
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
                if (tag == null || tag.Group is null)
                    continue;

                if (groupTag == tag.Group.Tag && name == tag.Name)
                    return tag;
            }
            return null;
        }
    }
}
