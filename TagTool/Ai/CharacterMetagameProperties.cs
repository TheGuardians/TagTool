using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x8)]
    public class CharacterMetagameProperties : TagStructure
	{
        public CharacterMetagameFlags Flags;
        public CharacterMetagameUnit Unit;
        public CharacterMetagameClassification Classification;
        [TagField(Flags = TagFieldFlags.Padding, Length = 1)]
        public byte[] pad0 = new byte[1];
        public short Points;
        [TagField(Flags = TagFieldFlags.Padding, Length = 2)]
        public byte[] pad1 = new byte[2];
    }
}