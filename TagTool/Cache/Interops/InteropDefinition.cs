using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache.Interops
{
    [TagStructure(Size = 0x14)]
    public class InteropDefinition : TagStructure
    {
        [TagField(Length = 4)]
        public uint[] Guid;

        [TagField(Flags = TagFieldFlags.Label)]
        public StringId Name;
    }
}