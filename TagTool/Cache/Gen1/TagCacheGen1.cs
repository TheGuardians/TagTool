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

    public class TagCacheGen1 : TagCache
    {
        public TagCacheGen1Header Header;
        public uint BaseTagAddress;
        public List<CachedTagGen1> Tags = new List<CachedTagGen1>();

        public TagCacheGen1(EndianReader reader, MapFile mapFile)
        {
            TagDefinitions = new TagDefinitionsGen1();
            var tagDataSectionOffset = mapFile.Header.TagsHeaderAddress32;
            reader.SeekTo(tagDataSectionOffset);

            var dataContext = new DataSerializationContext(reader);
            var deserializer = new TagDeserializer(mapFile.Version);
            Header = deserializer.Deserialize<TagCacheGen1Header>(dataContext);

            if (mapFile.Version == CacheVersion.HaloXbox)
                BaseTagAddress = 0x803A6000;
            else
                BaseTagAddress = 0x40440000;
            
            //
            // Read all tags offsets are all broken, need some proper look
            //

            reader.SeekTo(Header.CachedTagArrayAddress - BaseTagAddress + tagDataSectionOffset);
            
            for (int i = 0; i < Header.TagCount; i++)
            {
                var group = new TagGroupGen1(new Tag(reader.ReadInt32()), new Tag(reader.ReadInt32()), new Tag(reader.ReadInt32()));

                if (!TagDefinitions.TagDefinitionExists(group))
                    Debug.WriteLine($"Warning: tag definition for {group} does not exists!");

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
                if (tag != null && groupTag == tag.Group.Tag && name == tag.Name)
                    return tag;
            }
            return null;
        }
    }
}
