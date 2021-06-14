using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache.Gen4
{
    public class TagGroupGen4 : TagGroup
    {
        public string Name;
        public TagGroupGen4() : base() { Name = ""; }
        public TagGroupGen4(Tag tag, string name) : base(tag) { Name = name; }
        public TagGroupGen4(Tag tag, Tag parentTag, string name) : base(tag, parentTag) { Name = name; }
        public TagGroupGen4(Tag tag, Tag parentTag, Tag grandparentTag, string name) : base(tag, parentTag, grandparentTag) { Name = name; }
        public override string ToString() => Name == "" ? Tag.ToString() : Name;
    }
}
