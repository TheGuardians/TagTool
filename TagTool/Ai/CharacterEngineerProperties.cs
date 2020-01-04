using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x38)]
    public class CharacterEngineerProperties : TagStructure
	{
        /// <summary>
        /// World Units; The amount that the engineer attempts to rise before dying.
        /// </summary>
        public float DeathHeight;

        /// <summary>
        /// Seconds; The amount of time that the engineer spends rising before dying.
        /// </summary>
        public float DeathRiseTime;

        /// <summary>
        /// Seconds; The amount of time that the engineer spends detonating on death.
        /// </summary>
        public float DeathDetonationTime;

        /// <summary>
        /// The radius that the engineer boosts the shields of allies during combat.
        /// </summary>
        public float ShieldBoostRadius;

        /// <summary>
        /// Seconds; The time within the shield boost pings of the engineer.
        /// </summary>
        public float ShieldBoostPeriod;

        /// <summary>
        /// The strenght of the engineer shield boost.
        /// </summary>
        public float ShieldBoostStrenght;

        public float DetonationShieldThreshold;
        public float DetonationBodyVitality;

        /// <summary>
        /// World Units; If target enters within this radius, either detonate or deploy equipment.
        /// </summary>
        public float ProximityRadius;

        /// <summary>
        /// The chance that the engineer will detonate if target enters the drain radius.
        /// </summary>
        public float ProximityDetonationChance;

        /// <summary>
        /// The equipment that the engineer deploys if target enters radius and detonation is not chosen.
        /// </summary>
        public CachedTag ProximityEquipment;
    }
}
