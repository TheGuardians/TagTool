using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TagTool.BlamFile;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Definitions.Gen1;

namespace TagTool.Cache.Gen1
{
    [TagStructure(MaxVersion = CacheVersion.HaloXbox, Size = 0x24)]
    [TagStructure(MinVersion = CacheVersion.HaloPC, MaxVersion = CacheVersion.HaloCustomEdition, Size = 0x28)]
    public class TagCacheGen1Header : TagStructure
    {
        public uint CachedTagArrayAddress;
        public uint ScenarioTagID;
        public uint MapID;
        public uint TagCount;

        public uint VertexPartsCount;
        public uint ModelDataOffset;
        public uint IndexPartsCount;
        [TagField(MaxVersion = CacheVersion.HaloXbox)]
        public uint IndexPartsOffset;

        [TagField(MinVersion = CacheVersion.HaloPC)]
        public uint VertexDataSize;
        [TagField(MinVersion = CacheVersion.HaloPC)]
        public uint ModelDataSize; // (size of vertices + indices)

        public Tag Tags;
    }

    [TagStructure(Size = 0x20)]
    public class TagTableEntryGen1 : TagStructure
    {
        public Tag Tag;
        public Tag ParentTag;
        public Tag GrandParentTag;

        public uint ID;
        public uint TagNameAddress;

        public uint Address;

        public uint Unknown1;
        public uint Unknown2;
    }

    public class TagCacheGen1 : TagCache
    {
        public TagCacheGen1Header Header;
        public uint BaseTagAddress;
        public List<CachedTagGen1> Tags = new List<CachedTagGen1>();

        public TagCacheGen1(EndianReader reader, MapFile mapFile)
        {
            TagDefinitions = new TagDefinitionsGen1();
            Version = mapFile.Version;
            CachePlatform = mapFile.CachePlatform;

            var tagDataSectionOffset = (uint)mapFile.Header.GetTagTableHeaderOffset();
            reader.SeekTo(tagDataSectionOffset);

            var dataContext = new DataSerializationContext(reader);
            var deserializer = new TagDeserializer(mapFile.Version, mapFile.CachePlatform);
            Header = deserializer.Deserialize<TagCacheGen1Header>(dataContext);

            if (mapFile.Version == CacheVersion.HaloXbox)
                BaseTagAddress = 0x803A6000;
            else
                BaseTagAddress = 0x40440000;

            reader.SeekTo(Header.CachedTagArrayAddress - BaseTagAddress + tagDataSectionOffset);

            for (int i = 0; i < Header.TagCount; i++)
            {
                var entry = deserializer.Deserialize<TagTableEntryGen1>(dataContext);

                var group = new TagGroupGen1(entry.Tag, entry.ParentTag, entry.GrandParentTag);

                if (!TagDefinitions.TagDefinitionExists(group))
                    Debug.WriteLine($"Warning: tag definition for {group} does not exists!");

                var currentPos = reader.Position;
                string name = "";
                if (entry.TagNameAddress != 0)
                {
                    reader.SeekTo(entry.TagNameAddress - BaseTagAddress + tagDataSectionOffset);
                    name = reader.ReadNullTerminatedString();
                    reader.SeekTo(currentPos);
                }

                Tags.Add(new CachedTagGen1((int)(entry.ID & 0xFFFF), entry.ID, group, entry.Address, name));
            }

            FixupStructureBsps(reader, mapFile);
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
                if (tag != null && groupTag == tag.Group.Tag && name == tag.Name)
                    return tag;
            }
            return null;
        }

        private void FixupStructureBsps(EndianReader reader, MapFile mapFile)
        {
            uint AddressToOffset(uint address) => (address - BaseTagAddress) + (uint)mapFile.Header.GetTagTableHeaderOffset();

            var scnrTag = (CachedTagGen1)GetTag(Header.ScenarioTagID);

            reader.BaseStream.Position = AddressToOffset(scnrTag.Offset) + 0x5A4;

            uint sbspRefsCount = reader.ReadUInt32();
            uint sbspRefsAddress = reader.ReadUInt32();

            for (var i = 0u; i < sbspRefsCount; i++)
            {
                uint sbspRefOffset = AddressToOffset(sbspRefsAddress + i * TagStructure.GetStructureSize(typeof(Scenario.ScenarioStructureBspsBlock), Version, CachePlatform));

                reader.BaseStream.Position = sbspRefOffset;
                uint sbspHeaderOffset = reader.ReadUInt32();
                uint sbspDataSize = reader.ReadUInt32();
                uint sbspHeaderAddress = reader.ReadUInt32();

                reader.BaseStream.Position = sbspHeaderOffset;
                uint sbspDataAddress = reader.ReadUInt32();

                reader.BaseStream.Position = sbspRefOffset + 0x1C;
                var sbspTag = (CachedTagGen1)GetTag(reader.ReadUInt32());

                if (sbspTag != null)
                {
                    sbspTag.Offset = sbspDataAddress;
                    sbspTag.AddressToOffsetOverride = (currentOffset, address) => address - sbspHeaderAddress + sbspHeaderOffset;
                }
            }
        }
    }
}
