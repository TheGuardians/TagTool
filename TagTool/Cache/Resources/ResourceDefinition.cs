using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache.Resources
{
    [TagStructure(Size = 0x1C)]
    public class ResourceDefinition : TagStructure
    {
        [TagField(Length = 16)]
        public byte[] Guid;

        public short Unknown;
        public short Unknown2;
        public short Unknown3;
        public short Unknown4;

        [TagField(Flags = TagFieldFlags.Label)]
        public StringId Name;
    }
}