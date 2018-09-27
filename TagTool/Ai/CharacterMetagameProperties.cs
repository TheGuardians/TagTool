using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x8)]
    public class CharacterMetagameProperties : TagStructure
	{
        public CharacterMetagameFlags Flags;
        public CharacterMetagameUnit Unit;
        public CharacterMetagameClassification Classification;
        public sbyte Unknown;
        public short Points;
        public short Unknown2;
    }
}