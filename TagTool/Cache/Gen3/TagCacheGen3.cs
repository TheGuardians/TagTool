using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags;
using TagTool.BlamFile;

namespace TagTool.Cache.Gen3
{

    public class TagTableHeaderGen3
    {
        public int TagGroupsOffset;
        public int TagGroupCount;
        public int TagsOffset;
        public DatumIndex ScenarioHandle;
        public DatumIndex GlobalsHandle;
        public int CRC;
        public int TagCount;
        public int TagInfoHeaderCount;
        public int TagInfoHeaderOffset;
        public int TagInfoHeaderCount2;
        public int TagInfoHeaderOffset2;
    }

    public class TagCacheGen3 : TagCache
    {
        public List<CachedTagGen3> Tags;
        public TagTableHeaderGen3 TagTableHeader;
        public List<TagGroup> TagGroups;
        public string TagsKey = "";
        private readonly GameCacheGen3 GameCache;

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

        public override Stream OpenTagCacheRead() => GameCache.OpenCacheRead();

        public override Stream OpenTagCacheReadWrite() => GameCache.OpenCacheReadWrite();

        public override Stream OpenTagCacheWrite() => GameCache.OpenCacheWrite();

        public TagCacheGen3(GameCacheGen3 cache, EndianReader reader, MapFile baseMapFile, StringTableGen3 stringTable, int Magic)
        {
            Tags = new List<CachedTagGen3>();
            TagGroups = new List<TagGroup>();
            TagTableHeader = baseMapFile.GetTagTableHeader(reader, Magic);
            Version = baseMapFile.Version;
            GameCache = cache;

            switch (Version)
            {
                case CacheVersion.Halo3Retail:
                case CacheVersion.Halo3ODST:
                    TagsKey = "";
                    break;
                case CacheVersion.HaloReach:
                    TagsKey = "LetsAllPlayNice!";
                    break;
            }

            #region Read Class List
            reader.SeekTo(TagTableHeader.TagGroupsOffset);
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
            reader.SeekTo(TagTableHeader.TagsOffset);
            for (int i = 0; i < TagTableHeader.TagCount; i++)
            {
                var groupIndex = reader.ReadInt16();
                var tagGroup = groupIndex == -1 ? new TagGroup() : TagGroups[groupIndex];
                string groupName = groupIndex == -1 ? "" : stringTable.GetString(tagGroup.Name);
                CachedTagGen3 tag = new CachedTagGen3(groupIndex, (uint)((reader.ReadInt16() << 16) | i), (uint)(reader.ReadUInt32() - Magic), i, tagGroup, groupName);
                Tags.Add(tag);
            }
            #endregion

            #region Read Indices
            reader.SeekTo(baseMapFile.Header.GetTagNamesIndicesOffset());
            int[] indices = new int[TagTableHeader.TagCount];
            for (int i = 0; i < TagTableHeader.TagCount; i++)
                indices[i] = reader.ReadInt32();
            #endregion

            #region Read Names
            reader.SeekTo(baseMapFile.Header.GetTagNamesBufferOffset());

            EndianReader newReader = null;

            if (TagsKey == "" || TagsKey == null)
            {
                newReader = new EndianReader(new MemoryStream(reader.ReadBytes(baseMapFile.Header.GetTagNamesBufferSize())), EndianFormat.BigEndian);
            }
            else
            {
                reader.BaseStream.Position = baseMapFile.Header.GetTagNamesBufferOffset();
                newReader = new EndianReader(reader.DecryptAesSegment(baseMapFile.Header.GetTagNamesBufferSize(), TagsKey), EndianFormat.BigEndian);
            }

            for (int i = 0; i < indices.Length; i++)
            {
                if (indices[i] == -1)
                {
                    Tags[i].Name = null;
                    continue;
                }

                newReader.SeekTo(indices[i]);

                int length;
                if (i == indices.Length - 1)
                    length = baseMapFile.Header.GetTagNamesBufferSize() - indices[i];
                else
                {
                    if (indices[i + 1] == -1)
                    {
                        int index = -1;

                        for (int j = i + 1; j < indices.Length; j++)
                        {
                            if (indices[j] != -1)
                            {
                                index = j;
                                break;
                            }
                        }

                        length = (index == -1) ? baseMapFile.Header.GetTagNamesBufferSize() - indices[i] : indices[index] - indices[i];
                    }
                    else
                        length = indices[i + 1] - indices[i];
                }

                if (length == 1)
                {
                    Tags[i].Name = "<blank>";
                    continue;
                }

                Tags[i].Name = newReader.ReadString(length);
            }

            newReader.Close();
            newReader.Dispose();
            #endregion

        }
    }

}
