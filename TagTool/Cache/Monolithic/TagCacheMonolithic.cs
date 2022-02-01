using System;
using System.Collections.Generic;
using TagTool.Cache.Gen3;
using TagTool.Cache.Gen4;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache.Monolithic
{
    public class TagCacheMonolithic : TagCache
    {
        public MonolithicTagFileBackend Backend;

        public List<CachedTagMonolithic> Tags = new List<CachedTagMonolithic>();
        public override IEnumerable<CachedTag> TagTable { get => Tags; }


        public TagCacheMonolithic(MonolithicTagFileBackend backend, CacheVersion version, CachePlatform platform)
        {
            Backend = backend;
            Version = version;
            CachePlatform = platform;

            TagDefinitions = GetTagGroupDictionary(version);
            LoadTags();
        }

        private TagDefinitions GetTagGroupDictionary(CacheVersion version)
        {
            switch (CacheVersionDetection.GetGeneration(version))
            {
                case CacheGeneration.Third:
                    return new TagDefinitionsGen3();
                case CacheGeneration.Fourth:
                    return new TagDefinitionsGen4();
                default:
                    throw new NotImplementedException();
            }
        }

        private void LoadTags()
        {
            var tagFileIndex = Backend.TagFileIndex;
            for(int i = 0; i < tagFileIndex.Index.Count; i++)
            {
                var entry = tagFileIndex.Index[i];

                var tag = new CachedTagMonolithic();
                tag.Name = entry.Name;
                tag.Group = TagDefinitions.GetTagGroupFromTag(entry.GroupTag) ?? new TagGroupGen3();
                tag.Index = i;
                tag.ID = entry.Id;
                tag.WideBlockIndex = entry.WideBlockIndex;
                Tags.Add(tag);
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

        public override CachedTag GetTag(uint ID) => GetTag((int)ID);

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
                if (tag != null && groupTag == tag.Group.Tag && name == tag.Name)
                    return tag;
            }
            return null;
        }
    }
}
