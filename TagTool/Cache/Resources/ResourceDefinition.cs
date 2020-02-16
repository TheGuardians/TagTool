using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache.Resources
{
    [TagStructure(Size = 0x1C)]
    public class ResourceDefinition : TagStructure
    {
        [TagField(Length = 4)]
        public uint[] Guid;

        public short Unknown;
        public short Unknown2;
        public short Unknown3;
        public short Unknown4;

        [TagField(Flags = TagFieldFlags.Label)]
        public StringId Name;
    }
}