using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x14)]
    public class CharacterEvasionProperties : TagStructure
	{
        public float EvasionDangerThreshold; // Consider evading when immediate danger surpasses threshold
        public float EvasionDelayTimer; // Wait at least this delay between evasions
        public float EvasionChance; // If danger is above threshold, the chance that we will evade. Expressed as chance of evading within a 1 second time period
        public float EvasionProximityThreshold; // If target is within given proximity, possibly evade
        public float DiveRetreatChance; // Chance of retreating (fleeing) after danger avoidance dive
    }
}
