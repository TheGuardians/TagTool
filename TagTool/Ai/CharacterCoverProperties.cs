using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x54)]
    public class CharacterCoverProperties : TagStructure
	{
        public CharacterCoverFlags Flags;
        public Bounds<float> HideBehindCoverTime;
        public float CoverVitalityThreshold;
        public float CoverShieldFraction;
        public float CoverCheckDelay;
        public float CoverDangerThreshold;
        public float DangerUpperThreshold;
        public Bounds<float> CoverChanceBounds;
        public uint Unknown;
        public uint Unknown2;
        public uint Unknown3;
        public float ProximitySelfPreserve;
        public float DisallowCoverDistance;
        public float ProximityMeleeDistance;
        public float UnreachableEnemyDangerThreashold;
        public float ScaryTargetThreshold;
        public float VitalityFractionShieldEquipment;
        public float RecentDamageShieldEquipment;
        public float MinimumEnemyDistance;
    }
}
