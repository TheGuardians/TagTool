using TagTool.Tags;

namespace TagTool.Cache.Monolithic
{
    [TagStructure(Size = 0x10)]
    public class TagFileBlock : TagStructure, IDatum
    {
        public ushort Identifier;
        public ushort Unknown1;
        public int TagHeapEntryIndex;
        public int CacheHeapEntryIndex;
        public uint Unknown4;

        ushort IDatum.Identifier { get => Identifier; set => Identifier = value; }
    }
}
