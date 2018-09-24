using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x2C)]
    public class CharacterPerceptionProperties : TagStructure
	{
        public CharacterPerceptionMode Mode;
        public CharacterPerceptionFlags Flags;
        public float MaxVisionDistance;
        public Angle CentralVisionAngle;
        public Angle MaxVisionAngle;
        public Angle PeripheralVisionAngle;
        public float PeripheralDistance;
        public float HearingDistance;
        public float NoticeProjectileChance;
        public float NoticeVehicleChance;
        public float PerceptionTime;
        public float FirstAcknowledgeSurpriseDistance;
    }
}
