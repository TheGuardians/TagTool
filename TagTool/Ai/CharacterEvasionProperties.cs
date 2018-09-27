using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x14)]
    public class CharacterEvasionProperties : TagStructure
	{
        public float EvasionDangerThreshold;
        public float EvasionDelayTimer;
        public float EvasionChance;
        public float EvasionProximityThreshold;
        public float DiveRetreatChance;
    }
}
