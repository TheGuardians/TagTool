using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x6)]
    public class CharacterChargeDifficultyLimit : TagStructure
	{
        public short MaximumKamikazeCount; // How many guys in a single clump can be kamikazing at one time
        public short MaximumBerserkCount; // How many guys in a single clump can be berserking at one time
        public short MinimumBerserkCount; // We'd like at least this number of guys in a single clump can be berserking at one time (primarily combat forms)
    }
}
