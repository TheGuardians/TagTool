using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache
{

    public abstract class CachedTag
    {
        public string Name;
        public int Index;
        public uint ID;
        public TagGroupNew Group;

        public abstract uint DefinitionOffset { get; }

        public CachedTag()
        {
            Index = -1;
            Name = null;
            Group = TagGroupNew.None;
        }

        public CachedTag(int index, string name = null) : this(index, TagGroupNew.None, name) { }

        public CachedTag(int index, TagGroupNew group, string name = null)
        {
            Index = index;
            Group = group;
            if (name != null)
                Name = name;
        }

        public override string ToString()
        {
            if (Name == null)
                return $"0x{Index.ToString("X8")}.{Group.ToString()}";
            else
                return $"{Name}.{Group.ToString()}";
        }

        public bool IsInGroup(params Tag[] groupTags)
        {
            return Group.BelongsTo(groupTags);
        }
    }

}
