using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags;
using TagTool.BlamFile;
using TagTool.Serialization;
using System.Diagnostics;

namespace TagTool.Cache.Gen3
{
    [TagStructure(Size = 0x28, Platform = CachePlatform.Only32Bit)]
    [TagStructure(Size = 0x50, Platform = CachePlatform.Only64Bit)]
    public class TagCacheGen3Header
    {
        public int TagGroupCount;
        [TagField(Platform = CachePlatform.Only64Bit)]
        public Tag TagGroupSignature = new Tag("343i");
        [TagField(Platform = CachePlatform.Only32Bit)]
        public uint TagGroupsAddress32;
        [TagField(Platform = CachePlatform.Only64Bit)]
        public ulong TagGroupsAddress64;

        public int TagInstancesCount;
        [TagField(Platform = CachePlatform.Only64Bit)]
        public Tag TagInstancesSignature = new Tag("343i");
        [TagField(Platform = CachePlatform.Only32Bit)]
        public uint TagInstancesAddress32;
        [TagField(Platform = CachePlatform.Only64Bit)]
        public ulong TagInstancesAddress64;

        public int GlobalIndicesCount;
        [TagField(Platform = CachePlatform.Only64Bit)]
        public Tag GlobalIndicesSignature = new Tag("343i");
        [TagField(Platform = CachePlatform.Only32Bit)]
        public uint GlobalIndicesAddress32;
        [TagField(Platform = CachePlatform.Only64Bit)]
        public ulong GlobalIndicesAddress64;

        public int InteropsCount;
        [TagField(Platform = CachePlatform.Only64Bit)]
        public Tag InteropsSignature = new Tag("343i");
        [TagField(Platform = CachePlatform.Only32Bit)]
        public uint InteropsAddress32;
        [TagField(Platform = CachePlatform.Only64Bit)]
        public ulong InteropsAddress64;

        [TagField(Platform = CachePlatform.Only64Bit)]
        public uint Unknown1;

        public int CRC;
        public Tag Signature = new Tag("tags");

        [TagField(Platform = CachePlatform.Only64Bit)]
        public uint Unknown2;
    }

    public class TagCacheGen3 : TagCache
    {
        public TagCacheGen3Header Header;

        public List<TagGroupGen3> Groups;

        public List<CachedTagGen3> Instances;

        /// <summary>
        /// Globals tag instances in the cache file.
        /// </summary>
        public Dictionary<Tag, CachedTagGen3> GlobalInstances;

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
            return new CachedTagGen3(index, (TagGroupGen3)group, name);
        }

        public override CachedTag CreateCachedTag()
        {
            return new CachedTagGen3(-1, new TagGroupGen3(), null);
        }

        public TagCacheGen3(EndianReader reader, MapFile baseMapFile, StringTableGen3 stringTable)
        {
            Version = baseMapFile.Version;
            TagDefinitions = new TagDefinitionsGen3();
            Groups = new List<TagGroupGen3>();
            Instances = new List<CachedTagGen3>();
            GlobalInstances = new Dictionary<Tag, CachedTagGen3>();

            var gen3Header = (CacheFileHeaderGen3)baseMapFile.Header;
            var tagNamesHeader = gen3Header.GetTagNameHeader();
            var tagMemoryHeader = gen3Header.GetTagMemoryHeader();

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

            uint sectionOffset;

            uint tagNamesOffsetsTableOffset;
            uint tagNamesBufferOffset;
            ulong addressMask;

            if (Version > CacheVersion.Halo3Beta)
            {
                var sectionTable = gen3Header.SectionTable;
                sectionOffset = sectionTable.GetSectionOffset(CacheFileSectionType.TagSection);

                // means no tags
                if (sectionTable.Sections[(int)CacheFileSectionType.TagSection].Size == 0)
                    return;

                tagNamesOffsetsTableOffset = sectionTable.GetOffset(CacheFileSectionType.StringSection, tagNamesHeader.TagNameIndicesOffset);
                tagNamesBufferOffset = sectionTable.GetOffset(CacheFileSectionType.StringSection, tagNamesHeader.TagNamesBufferOffset);

                addressMask = CacheVersionDetection.IsInPlatform(CachePlatform.Only64Bit, Version) ?
                (gen3Header.VirtualBaseAddress64 - (ulong)sectionOffset) :
                (ulong)(gen3Header.VirtualBaseAddress32 - sectionOffset);
            }
            else
            {
                tagNamesOffsetsTableOffset = tagNamesHeader.TagNameIndicesOffset;
                tagNamesBufferOffset = tagNamesHeader.TagNamesBufferOffset;
                addressMask = gen3Header.VirtualBaseAddress32 - tagMemoryHeader.MemoryBufferOffset;
            }

            var tagTableHeaderOffset = CacheVersionDetection.IsInPlatform(CachePlatform.Only64Bit, Version) ?
                (gen3Header.TagTableHeaderOffset64 - addressMask) :
                ((ulong)gen3Header.TagTableHeaderOffset32 - addressMask);

            reader.SeekTo((long)tagTableHeaderOffset);

            var dataContext = new DataSerializationContext(reader);
            var deserializer = new TagDeserializer(baseMapFile.Version);

            Header = deserializer.Deserialize<TagCacheGen3Header>(dataContext);

            var tagGroupsOffset = CacheVersionDetection.IsInPlatform(CachePlatform.Only64Bit, Version) ?
                Header.TagGroupsAddress64 - addressMask :
                (ulong)Header.TagGroupsAddress32 - addressMask;

            var tagInstancesOffset = CacheVersionDetection.IsInPlatform(CachePlatform.Only64Bit, Version) ?
                Header.TagInstancesAddress64 - addressMask :
                (ulong)Header.TagInstancesAddress32 - addressMask;

            var globalIndicesOffset = CacheVersionDetection.IsInPlatform(CachePlatform.Only64Bit, Version) ?
                Header.GlobalIndicesAddress64 - addressMask :
                (ulong)Header.GlobalIndicesAddress32 - addressMask;

            #region Read Tag Groups

            reader.SeekTo((long)tagGroupsOffset);

            for (int i = 0; i < Header.TagGroupCount; i++)
            {
                var group = new TagGroupGen3()
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
                var tagGroup = groupIndex == -1 ? new TagGroupGen3() : Groups[groupIndex];
                uint ID = (uint)((reader.ReadInt16() << 16) | i);

                var offset = CacheVersionDetection.IsInPlatform(CachePlatform.Only64Bit, Version) ?
                    (uint)((ulong)gen3Header.SectionTable.SectionAddressToOffsets[2] + (ulong)gen3Header.SectionTable.Sections[2].Offset + (((ulong)reader.ReadUInt32() * 4) - (gen3Header.VirtualBaseAddress64 - 0x50000000))) :
                    (uint)(reader.ReadUInt32() - addressMask);

                CachedTagGen3 tag = new CachedTagGen3(groupIndex, ID, offset, i, tagGroup);
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
                GlobalInstances[tag] = (CachedTagGen3)GetTag(ID);
            }

            #endregion

        }
    }

}
