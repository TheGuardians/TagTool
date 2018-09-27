using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x58)]
    public class CharacterRetreatProperties : TagStructure
	{
        public CharacterRetreatFlags Flags;
        public float ShieldThreshold;
        public float ScaryTargetThreshold;
        public float DangerThreshold;
        public float ProximityThreshold;
        public Bounds<float> ForcedCowerTimeBounds;
        public Bounds<float> CowerTimeBounds;
        public float ProximityAmbushThreshold;
        public float AwarenessAmbushThreshold;
        public float LeaderDeadRetreatChance;
        public float PeerDeadRetreatChance;
        public float SecondPeerDeadRetreatChance;
        public float FleeTimeout;
        public Angle ZigZagAngle;
        public float ZigZagPeriod;
        public float RetreatGrenadeChance;
        public CachedTagInstance BackupWeapon;
    }
}
