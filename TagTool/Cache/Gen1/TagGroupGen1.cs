using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache.Gen1
{
    public class TagGroupGen1 : TagGroup
    {
        public TagGroupGen1() : base() { }
        public TagGroupGen1(Tag tag) : base(tag) { }
        public TagGroupGen1(Tag tag, Tag parentTag) : base(tag, parentTag) { }
        public TagGroupGen1(Tag tag, Tag parentTag, Tag grandparentTag) : base(tag, parentTag, grandparentTag) { }
    }
}
