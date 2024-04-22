using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x54)]
    public class CharacterCoverProperties : TagStructure
	{
        public CharacterCoverFlags Flags;
        public Bounds<float> HideBehindCoverTime; // how long we stay behind cover after seeking cover (seconds)
        public float CoverShieldFraction; // Only cover when shield falls below this level
        public float CoverVitalityThreshold; // Only cover when vitality falls below this level
        public float DisallowCoverDistance; // Disallow covering from visible target closer than this distance (world units)
        public float CoverDangerThreshold; // Danger must be this high to cover. At a danger level of 'danger threshold', the chance of seeking cover is the cover chance lower bound (below)
        public float DangerUpperThreshold; // At or above danger level of upper threshold, the chance of seeking cover is the cover chance upper bound (below)
        public Bounds<float> CoverChanceBounds; // Bounds on the chances of seeking cover, given the above conditions are valid.
        public Bounds<float> ShieldedCoverChance; // Bounds on the chances of seeking cover when shielded, given the above conditions are valid.
        public float CoverCheckDelay; // Amount of time I will wait before trying again after covering (0 value defaults to 2 seconds)
        public float EmergeShieldThreshold; // Emerge from cover when shield fraction reaches threshold
        public float ProximitySelfPreserve; // Triggers self-preservation when target within this distance (assuming proximity_self_preserve_impulse is enabled)
        public float ProximityMeleeDistance; // When self preserving from a target less than given distance, causes melee attack (assuming proximity_melee_impulse is enabled)
        public float UnreachableEnemyDangerThreashold; // When danger from an unreachable enemy surpasses threshold, actor cover (assuming unreachable_enemy_cover impulse is enabled)
        public float ScaryTargetThreshold; // When target is aware of me and surpasses the given scariness, self-preserve (assuming scary_target_cover_impulse is enabled)
        public float VitalityFractionShieldEquipment; // Probability of going straight into cover when shield is depleted (assuming shield_depleted_cover_impulse is enabled)
        public float ShieldEquipmentVitalityFraction; // Fraction of vitality below which an equipped shield equipment (instant cover/bubbleshield) will be activated (once damage has died down, and assuming shield_equipment_impulse is enabled)
        public float RecentDamageShieldEquipment; // Must have less than this amount of recent body damage before we can deploy our equipped shield equipment.
    }
}
