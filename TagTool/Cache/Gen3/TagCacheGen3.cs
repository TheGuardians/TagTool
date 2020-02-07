using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags;
using TagTool.BlamFile;
using TagTool.Serialization;

namespace TagTool.Cache.Gen3
{
    [TagStructure(Size = 0x28)]
    public class TagCacheGen3Header
    {
        public int TagGroupCount;
        public uint TagGroupsAddress;
        public int TagCount;
        public uint TagsAddress;
        public int PrimaryTagsCount;
        public uint PrimaryTagsInfoAddress;
        public int TagInfoHeaderCount2;
        public uint TagInfoHeaderAddress2;
        public int CRC;
        public Tag Tags;
        
    }

    public class TagCacheGen3 : TagCache
    {
        public List<CachedTagGen3> Tags;
        public TagCacheGen3Header TagTableHeader;
        public List<TagGroup> TagGroups;
        public string TagsKey = "";

        /// <summary>
        /// Hardcoded list of tags that are most likely for runtime use.
        /// </summary>
        public Dictionary<Tag, CachedTagGen3> HardcodedTags;

        public override IEnumerable<CachedTag> TagTable { get => Tags; }

        public override CachedTag GetTag(uint ID) => GetTag((int)(ID & 0xFFFF));

        public override CachedTag GetTag(int index)
        {
            if (index > 0 && index < Tags.Count)
                return Tags[index];
            else
                return null;
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

        public override CachedTag AllocateTag(TagGroup type, string name = null)
        {
            throw new NotImplementedException();
        }

        public override CachedTag CreateCachedTag(int index, TagGroup group, string name = null)
        {
            return new CachedTagGen3(index, group, name);
        }

        public override CachedTag CreateCachedTag()
        {
            return new CachedTagGen3(-1, TagGroup.None, null);
        }

        public TagCacheGen3(EndianReader reader, MapFile baseMapFile, StringTableGen3 stringTable)
        {
            Tags = new List<CachedTagGen3>();
            TagGroups = new List<TagGroup>();
            HardcodedTags = new Dictionary<Tag, CachedTagGen3>();
            Version = baseMapFile.Version;

            switch (Version)
            {
                case CacheVersion.Halo3Beta:
                case CacheVersion.Halo3Retail:
                case CacheVersion.Halo3ODST:
                    TagsKey = "";
                    break;
                case CacheVersion.HaloReach:
                    TagsKey = "LetsAllPlayNice!";
                    break;
            }

            var sectionTable = baseMapFile.Header.SectionTable;
            var sectionOffset = sectionTable.GetSectionOffset(CacheFileSectionType.TagSection);

            // means no tags
            if (sectionTable.Sections[(int)CacheFileSectionType.TagSection].Size == 0)
                return;

            var tagAddressToOffset = baseMapFile.Header.TagBaseAddress - sectionOffset;

            var tagTableHeaderOffset = baseMapFile.Header.TagIndexAddress - tagAddressToOffset;

            reader.SeekTo(tagTableHeaderOffset);

            var dataContext = new DataSerializationContext(reader);
            var deserializer = new TagDeserializer(baseMapFile.Version);
            TagTableHeader = deserializer.Deserialize<TagCacheGen3Header>(dataContext);


            if (TagTableHeader.TagInfoHeaderCount2 != 0)
                throw new Exception("wEll HelLo THerE");

            var tagGroupsOffset = TagTableHeader.TagGroupsAddress - tagAddressToOffset;
            var tagsOffset = TagTableHeader.TagsAddress - tagAddressToOffset;
            var primaryTagBufferOffset = TagTableHeader.PrimaryTagsInfoAddress - tagAddressToOffset;

            var tagNamesOffsetsTableOffset = sectionTable.GetOffset(CacheFileSectionType.StringSection, baseMapFile.Header.TagNamesOffsetsTableAddress);
            var tagNamesBufferOffset = sectionTable.GetOffset(CacheFileSectionType.StringSection, baseMapFile.Header.TagNamesBufferAddress);
            
            #region Read Class List
            reader.SeekTo(tagGroupsOffset);
            for (int i = 0; i < TagTableHeader.TagGroupCount; i++)
            {
                var group = new TagGroup()
                {
                    Tag = new Tag(reader.ReadChars(4)),
                    ParentTag = new Tag(reader.ReadChars(4)),
                    GrandparentTag = new Tag(reader.ReadChars(4)),
                    Name = new StringId(reader.ReadUInt32())
                };
                TagGroups.Add(group);
            }
            #endregion

            #region Read Tags Info
            reader.SeekTo(tagsOffset);
            for (int i = 0; i < TagTableHeader.TagCount; i++)
            {
                var groupIndex = reader.ReadInt16();
                var tagGroup = groupIndex == -1 ? new TagGroup() : TagGroups[groupIndex];
                string groupName = groupIndex == -1 ? "" : stringTable.GetString(tagGroup.Name);
                uint ID = (uint)((reader.ReadInt16() << 16) | i);
                var offset = reader.ReadUInt32() - tagAddressToOffset;
                CachedTagGen3 tag = new CachedTagGen3(groupIndex, ID, offset, i, tagGroup, groupName);
                Tags.Add(tag);
            }
            #endregion

            #region Read Indices

            reader.SeekTo(tagNamesOffsetsTableOffset);
            int[] stringOffsets = new int[TagTableHeader.TagCount];
            for (int i = 0; i < TagTableHeader.TagCount; i++)
                stringOffsets[i] = reader.ReadInt32();
            #endregion

            #region Read Names
            reader.SeekTo(tagNamesBufferOffset);

            EndianReader newReader = null;
            if (TagsKey == "" || TagsKey == null)
                newReader = new EndianReader(new MemoryStream(reader.ReadBytes(baseMapFile.Header.TagNamesBufferSize)), EndianFormat.BigEndian);
            else
                newReader = new EndianReader(reader.DecryptAesSegment(baseMapFile.Header.TagNamesBufferSize, TagsKey), EndianFormat.BigEndian);

            for (int i = 0; i < stringOffsets.Length; i++)
            {
                if (stringOffsets[i] == -1)
                {
                    Tags[i].Name = null;
                    continue;
                }

                newReader.SeekTo(stringOffsets[i]);
                Tags[i].Name = newReader.ReadNullTerminatedString();
            }

            newReader.Close();
            newReader.Dispose();
            #endregion

            #region Read primary tags
            reader.SeekTo(primaryTagBufferOffset);
            for (int i = 0; i < TagTableHeader.PrimaryTagsCount; i++)
            {
                var tag = new Tag(reader.ReadChars(4));
                var ID = reader.ReadUInt32();
                HardcodedTags[tag] = (CachedTagGen3)GetTag(ID);
            }
            #endregion

        }
    }

}
