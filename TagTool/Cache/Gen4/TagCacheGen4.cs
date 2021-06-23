using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags;
using TagTool.BlamFile;
using TagTool.Serialization;
using System.Diagnostics;

namespace TagTool.Cache.Gen4
{
    [TagStructure(Size = 0x28, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x50, Platform = CachePlatform.MCC)]
    public class TagCacheGen4Header
    {
        public int TagGroupCount;
        [TagField(Platform = CachePlatform.MCC)]
        public Tag TagGroupSignature = new Tag("343i");
        [TagField(Platform = CachePlatform.Original)]
        public uint TagGroupsAddress32;
        [TagField(Platform = CachePlatform.MCC)]
        public ulong TagGroupsAddress64;

        public int TagInstancesCount;
        [TagField(Platform = CachePlatform.MCC)]
        public Tag TagInstancesSignature = new Tag("343i");
        [TagField(Platform = CachePlatform.Original)]
        public uint TagInstancesAddress32;
        [TagField(Platform = CachePlatform.MCC)]
        public ulong TagInstancesAddress64;

        public int GlobalIndicesCount;
        [TagField(Platform = CachePlatform.MCC)]
        public Tag GlobalIndicesSignature = new Tag("343i");
        [TagField(Platform = CachePlatform.Original)]
        public uint GlobalIndicesAddress32;
        [TagField(Platform = CachePlatform.MCC)]
        public ulong GlobalIndicesAddress64;

        public int InteropsCount;
        [TagField(Platform = CachePlatform.MCC)]
        public Tag InteropsSignature = new Tag("343i");
        [TagField(Platform = CachePlatform.Original)]
        public uint InteropsAddress32;
        [TagField(Platform = CachePlatform.MCC)]
        public ulong InteropsAddress64;

        [TagField(Platform = CachePlatform.MCC)]
        public uint Unknown1;

        public int CRC;
        public Tag Signature = new Tag("tags");

        [TagField(Platform = CachePlatform.MCC)]
        public uint Unknown2;
    }

    public class TagCacheGen4 : TagCache
    {
        public TagCacheGen4Header Header;

        public List<TagGroupGen4> Groups;

        public List<CachedTagGen4> Instances;

        /// <summary>
        /// Globals tag instances in the cache file.
        /// </summary>
        public Dictionary<Tag, CachedTagGen4> GlobalInstances;

        public string TagsKey = "";

        public override IEnumerable<CachedTag> TagTable { get => Instances; }

        public override CachedTag GetTag(uint ID) => GetTag((int)(ID & 0xFFFF));

        public override CachedTag GetTag(int index)
        {
            if (index > 0 && index < Instances.Count)
                return Instances[index];
            else
                return null;
        }

        public override CachedTag GetTag(string name, Tag groupTag)
        {
            foreach (var tag in Instances)
            {
                if (tag != null && groupTag == tag.Group.Tag && name == tag.Name)
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
            return new CachedTagGen4(index, (TagGroupGen4)group, name);
        }

        public override CachedTag CreateCachedTag()
        {
            return new CachedTagGen4(-1, new TagGroupGen4(), null);
        }

        public TagCacheGen4(EndianReader reader, MapFile baseMapFile, StringTableGen4 stringTable)
        {
            CachePlatform = baseMapFile.CachePlatform;
            Version = baseMapFile.Version;
            TagDefinitions = new TagDefinitionsGen4();
            Groups = new List<TagGroupGen4>();
            Instances = new List<CachedTagGen4>();
            GlobalInstances = new Dictionary<Tag, CachedTagGen4>();

            var Gen4Header = (CacheFileHeaderGen4)baseMapFile.Header;
            var tagNamesHeader = Gen4Header.GetTagNameHeader();
            var tagMemoryHeader = Gen4Header.GetTagMemoryHeader();

            switch (Version)
            {
                case CacheVersion.Halo3Beta:
                case CacheVersion.Halo3Retail:
                case CacheVersion.Halo3ODST:
                    TagsKey = "";
                    break;
                case CacheVersion.HaloReach:
                case CacheVersion.Halo4:
                    TagsKey = "LetsAllPlayNice!";
                    break;
            }

            uint sectionOffset;

            uint tagNamesOffsetsTableOffset;
            uint tagNamesBufferOffset;
            ulong addressMask;

            if (Version > CacheVersion.Halo3Beta)
            {
                var sectionTable = Gen4Header.SectionTable;
                sectionOffset = sectionTable.GetSectionOffset(CacheFileSectionType.TagSection);

                // means no tags
                if (sectionTable.Sections[(int)CacheFileSectionType.TagSection].Size == 0)
                    return;

                tagNamesOffsetsTableOffset = sectionTable.GetOffset(CacheFileSectionType.StringSection, tagNamesHeader.TagNameIndicesOffset);
                tagNamesBufferOffset = sectionTable.GetOffset(CacheFileSectionType.StringSection, tagNamesHeader.TagNamesBufferOffset);

                addressMask = CachePlatform == CachePlatform.MCC ?
                (Gen4Header.VirtualBaseAddress64 - (ulong)sectionOffset) :
                (ulong)(Gen4Header.VirtualBaseAddress32 - sectionOffset);
            }
            else
            {
                tagNamesOffsetsTableOffset = tagNamesHeader.TagNameIndicesOffset;
                tagNamesBufferOffset = tagNamesHeader.TagNamesBufferOffset;
                addressMask = Gen4Header.VirtualBaseAddress32 - tagMemoryHeader.MemoryBufferOffset;
            }

            var tagTableHeaderOffset = CachePlatform == CachePlatform.MCC ?
                (Gen4Header.TagTableHeaderOffset64 - addressMask) :
                ((ulong)Gen4Header.TagTableHeaderOffset32 - addressMask);

            reader.SeekTo((long)tagTableHeaderOffset);

            var dataContext = new DataSerializationContext(reader);
            var deserializer = new TagDeserializer(baseMapFile.Version, CachePlatform);

            Header = deserializer.Deserialize<TagCacheGen4Header>(dataContext);

            var tagGroupsOffset = CachePlatform == CachePlatform.MCC ?
                Header.TagGroupsAddress64 - addressMask :
                (ulong)Header.TagGroupsAddress32 - addressMask;

            var tagInstancesOffset = CachePlatform == CachePlatform.MCC ?
                Header.TagInstancesAddress64 - addressMask :
                (ulong)Header.TagInstancesAddress32 - addressMask;

            var globalIndicesOffset = CachePlatform == CachePlatform.MCC ?
                Header.GlobalIndicesAddress64 - addressMask :
                (ulong)Header.GlobalIndicesAddress32 - addressMask;

            #region Read Tag Groups

            reader.SeekTo((long)tagGroupsOffset);

            for (int i = 0; i < Header.TagGroupCount; i++)
            {
                var group = new TagGroupGen4()
                {
                    Tag = reader.ReadTag(),
                    ParentTag = reader.ReadTag(),
                    GrandParentTag = reader.ReadTag(),
                    Name = stringTable.GetString(new StringId(reader.ReadUInt32()))
                };
                Groups.Add(group);
                if(!TagDefinitions.TagDefinitionExists(group))
                    Debug.WriteLine($"Warning: tag definition for {group.Tag} : {group.Name} does not exists!");
            }

            #endregion

            #region Read Tags Info

            reader.SeekTo((long)tagInstancesOffset);

            for (int i = 0; i < Header.TagInstancesCount; i++)
            {
                var groupIndex = reader.ReadInt16();
                var tagGroup = groupIndex == -1 ? new TagGroupGen4() : Groups[groupIndex];
                uint ID = (uint)((reader.ReadInt16() << 16) | i);

                var offset = CachePlatform == CachePlatform.MCC ?
                    (uint)((ulong)Gen4Header.SectionTable.SectionAddressToOffsets[2] + (ulong)Gen4Header.SectionTable.Sections[2].Offset + (((ulong)reader.ReadUInt32() * 4) - (Gen4Header.VirtualBaseAddress64 - 0x50000000))) :
                    (uint)(reader.ReadUInt32() - addressMask);

                CachedTagGen4 tag = new CachedTagGen4(groupIndex, ID, offset, i, tagGroup);
                Instances.Add(tag);
            }

            #endregion

            #region Read Indices

            reader.SeekTo(tagNamesOffsetsTableOffset);

            var stringOffsets = new int[Header.TagInstancesCount];

            for (int i = 0; i < Header.TagInstancesCount; i++)
                stringOffsets[i] = reader.ReadInt32();

            #endregion

            #region Read Names

            reader.SeekTo(tagNamesBufferOffset);

            using (var newReader = (TagsKey == "" || TagsKey == null) ?
                new EndianReader(new MemoryStream(reader.ReadBytes(tagNamesHeader.TagNamesBufferSize)), EndianFormat.BigEndian) :
                new EndianReader(reader.DecryptAesSegment(tagNamesHeader.TagNamesBufferSize, TagsKey), EndianFormat.BigEndian))
            {
                for (int i = 0; i < stringOffsets.Length; i++)
                {
                    if (stringOffsets[i] == -1)
                    {
                        Instances[i].Name = null;
                        continue;
                    }

                    newReader.SeekTo(stringOffsets[i]);
                    Instances[i].Name = newReader.ReadNullTerminatedString();
                }
            }

            #endregion

            #region Read Global Tags

            reader.SeekTo((long)globalIndicesOffset);

            for (int i = 0; i < Header.GlobalIndicesCount; i++)
            {
                var tag = reader.ReadTag();
                var ID = reader.ReadUInt32();
                GlobalInstances[tag] = (CachedTagGen4)GetTag(ID);
            }

            #endregion

        }
    }

}
