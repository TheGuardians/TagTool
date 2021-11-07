using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x58)]
    public class CharacterRetreatProperties : TagStructure
	{
        public CharacterRetreatFlags Flags;
        public float ShieldThreshold; // When shield vitality drops below given amount, retreat is triggered by low_shield_retreat_impulse
        public float ScaryTargetThreshold; // When confronting an enemy of over the given scariness, retreat is triggered by scary_target_retreat_impulse
        public float DangerThreshold; // When perceived danger rises above the given threshold, retreat is triggered by danger_retreat_impulse
        public float ProximityThreshold; // When enemy closer than given threshold, retreat is triggered by proximity_retreat_impulse
        public Bounds<float> ForcedCowerTimeBounds; // actor cowers for at least the given amount of time
        public Bounds<float> CowerTimeBounds; // actor times out of cower after the given amount of time
        public float ProximityAmbushThreshold; // If target reaches is within the given proximity, an ambush is triggered by the proximity ambush impulse
        public float AwarenessAmbushThreshold; // If target is less than threshold (0-1) aware of me, an ambush is triggered by the vulnerable enemy ambush impulse
        public float LeaderDeadRetreatChance; // If leader-dead-retreat-impulse is active, gives the chance that we will flee when our leader dies within 4 world units of us
        public float PeerDeadRetreatChance; // If peer-dead-retreat-impulse is active, gives the chance that we will flee when one of our peers (friend of the same race) dies within 4 world units of us
        public float SecondPeerDeadRetreatChance; // If peer-dead-retreat-impulse is active, gives the chance that we will flee when a second peer (friend of the same race) dies within 4 world units of us
        public float FleeTimeout; // Flee for no longer than this time (if there is no cover, then we will keep fleeing indefinitely). Value of 0 means 'no timeout' (seconds)
        public Angle ZigZagAngle; // The angle from the intended destination direction that a zig-zag will cause
        public float ZigZagPeriod; // How long it takes to zig left and then zag right.
        public float RetreatGrenadeChance; // The likelihood of throwing down a grenade to cover our retreat
        public CachedTag BackupWeapon; // If I want to flee and I don't have flee animations with my current weapon, throw it away and try a ...
    }
}
