using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x18, Align = 0x10)]
    public class CharacterUnknownProperties : TagStructure
	{
        public uint Unknown;
        public uint Unknown2;
        public uint Unknown3;
        public uint Unknown4;
        public uint Unknown5;
        public uint Unknown6;
    }
}
