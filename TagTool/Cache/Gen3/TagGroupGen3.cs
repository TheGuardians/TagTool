using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache.Gen3
{
    public class TagGroupGen3 : TagGroupNew
    {
        public string Name;
        public static readonly new TagGroupGen3 None = new TagGroupGen3(new Tag(-1), new Tag(-1), new Tag(-1), "");
        public TagGroupGen3() : base() { Name = ""; }
        public TagGroupGen3(Tag tag, string name) : base(tag) { Name = name; }
        public TagGroupGen3(Tag tag, Tag parentTag, string name) : base(tag, parentTag) { Name = name; }
        public TagGroupGen3(Tag tag, Tag parentTag, Tag grandparentTag, string name) : base(tag, parentTag, grandparentTag) { Name = name; }
        public override string ToString() => Name == "" ? Tag.ToString() : Name;
    }
}
