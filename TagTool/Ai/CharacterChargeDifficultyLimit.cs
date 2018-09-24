using TagTool.Serialization;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x6)]
    public class CharacterChargeDifficultyLimit : TagStructure
	{
        public short MaximumKamikazeCount;
        public short MaximumBerserkCount;
        public short MinimumBerserkCount;
    }
}
