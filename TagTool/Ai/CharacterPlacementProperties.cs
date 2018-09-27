using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x34)]
    public class CharacterPlacementProperties : TagStructure
	{
        [TagField(Padding = true, Length = 4)]
        public byte[] Unused;

        public float FewUpgradeChanceEasy;
        public float FewUpgradeChanceNormal;
        public float FewUpgradeChanceHeroic;
        public float FewUpgradeChanceLegendary;
        public float NormalUpgradeChanceEasy;
        public float NormalUpgradeChanceNormal;
        public float NormalUpgradeChanceHeroic;
        public float NormalUpgradeChanceLegendary;
        public float ManyUpgradeChanceEasy;
        public float ManyUpgradeChanceNormal;
        public float ManyUpgradeChanceHeroic;
        public float ManyUpgradeChanceLegendary;
    }
}
