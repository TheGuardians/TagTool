using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache.Gen2
{
    public class TagGroupGen2 : TagGroupNew
    {
        public TagGroupGen2() : base() { }
        public TagGroupGen2(Tag tag) : base(tag) { }
        public TagGroupGen2(Tag tag, Tag parentTag) : base(tag, parentTag) { }
        public TagGroupGen2(Tag tag, Tag parentTag, Tag grandparentTag) : base(tag, parentTag, grandparentTag) { }
    }
}
