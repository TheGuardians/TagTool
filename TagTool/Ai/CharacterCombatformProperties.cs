using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x8)]
    public class CharacterCombatformProperties : TagStructure
	{
        public float BerserkDistance; // Distance at which the combatform will be forced into berserking. (world units)
        public float BerserkChance; // Chance of which the combatform will be forced into berserking this second.
    }
}
