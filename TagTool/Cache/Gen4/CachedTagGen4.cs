using TagTool.Tags;

namespace TagTool.Cache.Gen4
{
    public class CachedTagGen4 : CachedTag
    {
        public uint Offset;
        public int GroupIndex;
        public int Size;

        public override uint DefinitionOffset => Offset;

        public CachedTagGen4() : base() { }

        public CachedTagGen4(int index, TagGroupGen4 group, string name = null) : base(index, group, name) { }

        public CachedTagGen4(int groupIndex, uint id, uint offset, int index, TagGroupGen4 tagGroup)
        {
            GroupIndex = groupIndex;
            ID = id;
            Offset = offset;
            Index = index;
            Group = tagGroup;
        }
    }
}
