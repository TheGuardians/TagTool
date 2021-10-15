using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x2C)]
    public class CharacterPerceptionProperties : TagStructure
	{
        public CharacterPerceptionFlags Flags;
        public float MaxVisionDistance; // maximum range of sight (world units)
        public Angle CentralVisionAngle; // horizontal angle within which we see targets out to our maximum range
        public Angle MaxVisionAngle; // maximum horizontal angle within which we see targets at range
        public Angle PeripheralVisionAngle; // maximum horizontal angle within which we can see targets out of the corner of our eye
        public float PeripheralDistance; // maximum range at which we can see targets our of the corner of our eye (world units)
        public float HearingDistance; // maximum range at which sounds can be heard (world units)
        public float NoticeProjectileChance; // random chance of noticing a dangerous enemy projectile, e.g. grenade (0-1)
        public float NoticeVehicleChance; // random chance of noticing a dangerous vehicle (0-1)
        public float PerceptionTime; // time required to acknowledge a visible enemy (seconds)
        public float FirstAcknowledgeSurpriseDistance; // If a new prop is acknowledged within the given distance, surprise is registered
    }
}
