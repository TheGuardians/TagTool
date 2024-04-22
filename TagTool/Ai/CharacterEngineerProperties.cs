using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x38)]
    public class CharacterEngineerProperties : TagStructure
	{
        public float DeathHeight; // try and rise this amount before dying (wu)
        public float DeathRiseTime; // spend this time rising (seconds)
        public float DeathDetonationTime; // spend this time detonating (seconds)
        public float ShieldBoostRadiusMax; // Boost the shields of allies within this radius during combat
        public float ShieldBoostRadiusMin; // Allies within this radius get maximum shield boost
        public float ShieldBoostVitality; // Boost allies' shields by this amount during combat

        /* Detonation Thresholds */
        public float DetonationShieldThreshold;
        public float DetonationBodyVitality;
        public float ProximityRadius; // if target enters within this radius, either detonate or deploy equipment (wus)
        public float ProximityDetonationChance; // chance of detonating if target enters the drain radius radius

        [TagField(ValidTags = new[] { "eqip" })]
        public CachedTag ProximityEquipment; // if target enters radius and detonation is not chosen, deploy this equipment.
    }
}
