using TagTool.Tags;

namespace TagTool.Cache.Gen3
{
    public class CachedTagGen3 : CachedTag
    {
        public uint Offset;
        public int GroupIndex;
        public int Size;

        public override uint DefinitionOffset => Offset;

        public CachedTagGen3() : base() { }

        public CachedTagGen3(int index, TagGroupGen3 group, string name = null) : base(index, group, name) { }

        public CachedTagGen3(int groupIndex, uint id, uint offset, int index, TagGroupGen3 tagGroup)
        {
            GroupIndex = groupIndex;
            ID = id;
            Offset = offset;
            Index = index;
            Group = tagGroup;
        }
    }
}
