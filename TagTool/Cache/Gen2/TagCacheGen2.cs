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
using TagTool.Tags.Definitions.Gen2;

namespace TagTool.Cache.Gen2
{
    [TagStructure(Size = 0x14, MaxVersion = CacheVersion.Halo2Beta)]
    [TagStructure(Size = 0x20, MinVersion = CacheVersion.Halo2Xbox)]
    public class TagCacheGen2Header : TagStructure
    {
        [TagField(MinVersion = CacheVersion.Halo2Xbox)]
        public uint TagGroupsOffset;

        [TagField(MinVersion = CacheVersion.Halo2Xbox)]
        public int TagGroupCount;

        public uint TagsOffset;
        public uint ScenarioID;

        [TagField(MinVersion = CacheVersion.Halo2Xbox)]
        public uint GlobalsID;

        public int CRC;
        public int TagCount;
        public Tag Tags;
    }

    [TagStructure(Size = 0x20, MaxVersion = CacheVersion.Halo2Beta)]
    [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo2Xbox)]
    public class TagTableEntryGen2 : TagStructure
    {
        public Tag Tag;

        [TagField(MaxVersion = CacheVersion.Halo2Beta)]
        public Tag ParentTag;
        [TagField(MaxVersion = CacheVersion.Halo2Beta)]
        public Tag GrandParentTag;

        public uint ID;

        [TagField(MaxVersion = CacheVersion.Halo2Beta)]
        public uint TagNameAddress;

        public uint Address;

        public int Size;

        [TagField(MaxVersion = CacheVersion.Halo2Beta)]
        public uint Unknown2;
    }


    public class TagCacheGen2 : TagCache
    {
        /// <summary>
        /// Address in memory (xbox) of the tag data. For Halo 2 Vista, this values turns out to be 0. Every address in the tag data is converted to an offset using this value.
        /// </summary>
        public uint VirtualAddress;

        public TagCacheGen2Header Header;
        public List<CachedTagGen2> Tags = new List<CachedTagGen2>();
        public Dictionary<Tag, CachedTagGen2> HardcodedTags = new Dictionary<Tag, CachedTagGen2>();
        public readonly bool IsShared = false;

        public TagCacheGen2(EndianReader reader, MapFile mapFile)
        {
            Version = mapFile.Version;
            CachePlatform = mapFile.CachePlatform;

            TagDefinitions = new TagDefinitionsGen2();
            IsShared = mapFile.Header.GetCacheType() == CacheFileType.Shared || 
                        mapFile.Header.GetCacheType() == CacheFileType.SharedCampaign;

            var tagDataSectionOffset = (uint)mapFile.Header.GetTagTableHeaderOffset();
            reader.SeekTo(tagDataSectionOffset);

            var dataContext = new DataSerializationContext(reader);
            var deserializer = new TagDeserializer(Version, CachePlatform);
            Header = deserializer.Deserialize<TagCacheGen2Header>(dataContext);



            uint tagCacheVirtualAddress;
            var headerSize = TagStructure.GetStructureSize(typeof(TagCacheGen2Header), Version, CachePlatform);
            if (Version > CacheVersion.Halo2Beta)
                tagCacheVirtualAddress = (Header.TagGroupsOffset - headerSize);
            else
                tagCacheVirtualAddress = (Header.TagsOffset - headerSize);

            //
            // Read tag groups
            //

            if(Version > CacheVersion.Halo2Beta)
            {
                //seek to the tag groups offset, seems to be contiguous to the header
                reader.SeekTo(tagDataSectionOffset + Header.TagGroupsOffset - tagCacheVirtualAddress);   // TODO: check how halo 2 xbox uses this

                for (int i = 0; i < Header.TagGroupCount; i++)
                {
                    var group = new TagGroupGen2(new Tag(reader.ReadInt32()), new Tag(reader.ReadInt32()), new Tag(reader.ReadInt32()));
                    if (!TagDefinitions.TagDefinitionExists(group))
                        Debug.WriteLine($"Warning: tag definition for {group} does not exists!");
                }
            }

            //
            // Read cached tags
            //

            reader.SeekTo(tagDataSectionOffset + Header.TagsOffset - tagCacheVirtualAddress);

            for (int i = 0; i < Header.TagCount; i++)
            {
                var entry = deserializer.Deserialize<TagTableEntryGen2>(dataContext);
                string name = null;
                if (Version < CacheVersion.Halo2Xbox)
                {
                    var group = new TagGroupGen2(entry.Tag, entry.ParentTag, entry.GrandParentTag);
                    if (!TagDefinitions.TagDefinitionExists(group))
                        Debug.WriteLine($"Warning: tag definition for {group} does not exists!");


                    var streamPosition = reader.BaseStream.Position;
                    reader.SeekTo(tagDataSectionOffset + entry.TagNameAddress - tagCacheVirtualAddress);
                    name = reader.ReadNullTerminatedString();
                    reader.SeekTo(streamPosition);
                }

                if (entry.Tag.Value == -1 || entry.Tag.Value == 0 || entry.Size == -1 || entry.Address == 0xFFFFFFFF || entry.ID == 0 || entry.ID == 0xFFFFFFFF)
                    Tags.Add(null);
                else
                    Tags.Add(new CachedTagGen2((int)(entry.ID & 0xFFFF), entry.ID, (TagGroupGen2)TagDefinitions.GetTagGroupFromTag(entry.Tag), entry.Address, entry.Size, name, IsShared));
            }

            //
            // Read tag names
            //

            if(Version > CacheVersion.Halo2Beta)
            {
                var tagNamesHeader = mapFile.Header.GetTagNameHeader();

                reader.SeekTo(tagNamesHeader.TagNameIndicesOffset);
                var tagNamesOffset = new int[Header.TagCount];
                for (int i = 0; i < Header.TagCount; i++)
                    tagNamesOffset[i] = reader.ReadInt32();

                reader.SeekTo(tagNamesHeader.TagNamesBufferOffset);

                for (int i = 0; i < tagNamesOffset.Length; i++)
                {
                    if (Tags[i] == null)
                        continue;

                    if (tagNamesOffset[i] == -1)
                        continue;

                    reader.SeekTo(tagNamesOffset[i] + tagNamesHeader.TagNamesBufferOffset);

                    Tags[i].Name = reader.ReadNullTerminatedString();
                }
            }

            //
            // Set hardcoded tags from the header
            //

            var scnrTag = (CachedTagGen2)GetTag(Header.ScenarioID);
            HardcodedTags[scnrTag.Group.Tag] = scnrTag;

            if (Version > CacheVersion.Halo2Beta)
            {
                var globalTag = (CachedTagGen2)GetTag(Header.GlobalsID);
                HardcodedTags[globalTag.Group.Tag] = globalTag;
            }

            //
            // Update virtual address if on Xbox
            //

            if (Version <= CacheVersion.Halo2Xbox)
                VirtualAddress = Tags[0].Offset;
            else if (Version == CacheVersion.Halo2Vista)
                VirtualAddress = mapFile.Header.GetTagMemoryHeader().VirtualAddress;

        }

        public void FixupStructureBspTagsForXbox(EndianReader reader, MapFile mapFile)
        {
            var scnrTag = (CachedTagGen2)GetTag(Header.ScenarioID);

            uint magic = (uint)mapFile.Header.GetTagTableHeaderOffset() + mapFile.Header.GetTagMemoryHeader().MemoryBufferOffset - VirtualAddress;

            // seek to the sbsp reference block
            reader.BaseStream.Position = scnrTag.Offset + magic + 0x210;

            // read the sbsp reference block
            int sbspCount = reader.ReadInt32();
            uint sbspsRefsAddress = reader.ReadUInt32();

            for (uint i = 0u; i < sbspCount; i++)
            {
                // seek to the sbsp reference offset
                uint sbspRefSize = TagStructure.GetStructureSize(typeof(Scenario.ScenarioStructureBspReferenceBlock), Version, CachePlatform);
                reader.BaseStream.Position = (sbspsRefsAddress + i * sbspRefSize) + magic;

                // read the tag data addresses from the cache file globals sbsp header
                uint sbsHeaderOffset =  reader.ReadUInt32();
                uint bufferSize = reader.ReadUInt32();
                uint sbspHeaderAddress = reader.ReadUInt32();

                reader.BaseStream.Position = sbsHeaderOffset + 4;
                uint sbspTagAddress = reader.ReadUInt32();
                uint ltmpTagAddress = reader.ReadUInt32();

                // read the tag ids from the sbsp reference block
                reader.BaseStream.Position = (sbspsRefsAddress + magic) + 0x14;
                uint sbspTagId = reader.ReadUInt32();
                reader.BaseStream.Position = (sbspsRefsAddress + magic) + 0x1C;
                uint ltmpTagId = reader.ReadUInt32();

                CachedTag.AddressToOffsetOverrideDelegate sbspAddressToOffset = (currentOffset, address) => (address - sbspHeaderAddress) + sbsHeaderOffset;

                // fixup the tag data addresse
                var sbspTag = (CachedTagGen2)GetTag(sbspTagId);
                if (sbspTag != null)
                {
                    sbspTag.Offset = sbspTagAddress;
                    sbspTag.AddressToOffsetOverride = sbspAddressToOffset;
                } 
                var ltmpTag = (CachedTagGen2)GetTag(ltmpTagId);
                if (ltmpTag != null)
                {
                    ltmpTag.Offset = ltmpTagAddress;
                    ltmpTag.AddressToOffsetOverride = sbspAddressToOffset;
                }
            }
        }

        private TagCacheGen2() { }

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

        public static TagCacheGen2 Combine(TagCacheGen2 cache1, TagCacheGen2 cache2)
        {
            // copy from cache1 for now
            var result = new TagCacheGen2()
            {
                Header = cache1.Header,
                VirtualAddress = cache1.VirtualAddress,
                HardcodedTags = cache1.HardcodedTags,
                Version = cache1.Version,
                TagDefinitions = cache1.TagDefinitions,
            };

            result.Tags.AddRange(cache1.Tags);

            // h2 hardcodes a main cache tag limit of 10k. Everything after that is a shared tag
            for (int i = 0; i < cache2.Tags.Count; i++)
            {
                var tag = cache2.Tags[i];
                if (i >= 10000)
                {
                    if (i >= result.Tags.Count)
                        result.Tags.Add(tag);
                    else
                        result.Tags[i] = tag;
                }
            }

            return result;
        }
    }
}
